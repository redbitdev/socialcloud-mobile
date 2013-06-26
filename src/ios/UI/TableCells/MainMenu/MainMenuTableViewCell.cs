using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Collections.Generic;
using RedBit;

namespace BackChannel
{
	/// <summary>
	/// Custom table cell for menu items
	/// </summary>
	public partial class MainMenuTableViewCell : UITableViewCell
	{
		private MainMenuTableCellView _view;

		public MainMenuTableViewCell () : base ()
		{

		}

		public MainMenuTableViewCell(IntPtr handle) : base(handle){
			_view = new MainMenuTableCellView (this);
			ContentView.InsertSubview(_view,0);
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			_view.Frame = ContentView.Bounds;
			_view.SetNeedsDisplay ();
		}

		public void Bind (MenuItem menuItem)
		{
			// set the colors for the labels
//			lblTitle.TextColor = Themes.CellTheme.TextForegroundColor;
//			lblTitle.HighlightedTextColor = Themes.CellTheme.TextHightlightColor;
//			lblSubTitle.TextColor = Themes.CellTheme.TextSubForegroundColor;
//			lblSubTitle.HighlightedTextColor = Themes.CellTheme.TextSubHighlightColor;
//
//			// bind the data
//			if (!menuItem.IsHeroItem) {
//				lblTitle.Text = menuItem.Title;
//				lblSubTitle.Text = menuItem.SubTitle;
//				imgIcon.Image = UIImage.FromFile (menuItem.IconFileName);
//
//			} else {
//				lblSubTitle.Text = "";
//				lblTitle.Text = "";
//				lblTitle.Bounds = new RectangleF(0,0,this.Bounds.Width, this.Bounds.Height);
//			}

			// update the view
			_view.Update(menuItem);
		}
	}


	/// <summary>
	/// This view is used to do any custom drawing for the table cell. Basically owner draw the thing
	/// </summary>
	public class MainMenuTableCellView : UIView {
		/// <summary>
		/// The menu item object
		/// </summary>
		MenuItem _menuItem;

		/// <summary>
		/// the table cell object
		/// </summary>
		MainMenuTableViewCell _parentCell;
		private static UIColor _foregroundColor = UIColor.FromRGB(83,83,83);
		private static UIColor _backgroundColor = UIColor.FromRGB(242,242,242);
		private static UIColor _headerForegroundColor = UIColor.FromRGB(242,242,242);
		private static UIColor _HeaderBackgroundColor = UIColor.FromRGB(83,83,83);

		public MainMenuTableCellView (MainMenuTableViewCell parentCell)
		{
			_parentCell = parentCell;
		}

		// Public method, that allows the code to externally update
		// what we are rendering.   
		public void Update (MenuItem menuItem)
		{
			this._menuItem = menuItem;
			SetNeedsDisplay ();
		}

		internal const int CELL_HEIGHT_HEADER = 30;
		internal const int CELL_HEIGHT_ITEM = 50;

		public override void Draw (RectangleF rect)
		{
			if (_menuItem == null)
				return;

			//get the context 
			var context = UIGraphics.GetCurrentContext ();

			// get the bounds of view
			var tBounds = new RectangleF (0, 0, Bounds.Width, _menuItem.IsHeader ? CELL_HEIGHT_HEADER : CELL_HEIGHT_ITEM);
				 
			if(_menuItem.IsHeader){
				// draw the background
				context.SetFillColor (_HeaderBackgroundColor.CGColor);
				context.FillRect (tBounds);

				// draw the text
				tBounds.X += 5;
				this.DrawText(_menuItem.Text, context, tBounds, false);
				tBounds.X -= 5;

				// draw the color bar
				context.SetFillColor (_menuItem.Color.CGColor);
				context.FillRect (new RectangleF(Bounds.Width - 10,0, 10, CELL_HEIGHT_ITEM));
			}
			else{

				// draw a line at the bottom
				context.SetFillColor (UIColor.FromRGB(214,214,214).CGColor);
				tBounds.X = (_menuItem.IsHeader ? CELL_HEIGHT_HEADER : CELL_HEIGHT_ITEM) - 1;
				context.FillRect (tBounds);
				tBounds.X = 0;

				// draw the background
				context.SetFillColor (_parentCell.Highlighted ? _foregroundColor.CGColor : _backgroundColor.CGColor);
				context.FillRect (tBounds);

				// draw the image
				var t = _menuItem.Image.CGImage.GetScaledSizeForControl ();
				var imgBounds = new RectangleF(5, Bounds.Height / 2 - t.Height / 2, t.Width, t.Height);
				DrawImage (_menuItem.Image, context, imgBounds);

				// draw the text
				tBounds = new RectangleF (imgBounds.Right + 5, tBounds.Y, tBounds.Width - imgBounds.Right - 5, tBounds.Height);
				this.DrawText (_menuItem.Text, context, tBounds, false);
			}

			// call base
			base.Draw (rect);
		}

		private void DrawText (string text, CGContext context, RectangleF bounds, bool withShadow)
		{
			// save state and transform
			context.SaveState ();
			context.TranslateCTM (0, bounds.Height);
			context.ScaleCTM (1.0f, -1.0f);

			// calculate location
			context.SelectFont("Helvetica", UIFont.LabelFontSize,CGTextEncoding.MacRoman);
			NSString t = new NSString(text);
			var font = UIFont.FromName("Helvetica", UIFont.LabelFontSize);
			var size = t.StringSize(font);
			var x = bounds.Left;
			var y = bounds.Height/2 - size.Height/2;

			// draw the shadow at an offset
			if (withShadow) {
				UIColor.FromRGB(193,193,193).SetFill();
				context.ShowTextAtPoint(x+1, y+4, text);
			}

			// draw the text
			if (_parentCell.Highlighted && !_menuItem.IsHeader) 
				_headerForegroundColor.SetFill ();
			else if (_menuItem.IsHeader)
				_headerForegroundColor.SetFill ();
			else
				_foregroundColor.SetFill();
			context.ShowTextAtPoint(x, y+5, text);

			// restore context
			context.RestoreState();
		}

		private void DrawImage (UIImage image, CGContext context, RectangleF bounds)
		{
			// required hack to get the images to draw rightside up
			context.SaveState();
			context.TranslateCTM(0, bounds.Height);
			context.ScaleCTM(1.0f, -1.0f);
			bounds.Y = -bounds.Y;
			context.DrawImage(bounds, image.CGImage);
			context.RestoreState();
		}
	}

	/// <summary>
	/// view source for the table so we know what is selected
	/// </summary>
	public class MainMenuViewSource : UITableViewSource
	{
		/// <summary>
		/// Event that is raised when a row is clicked
		/// </summary>
		public event EventHandler<RowClickedEventArgs<MenuItem>> RowClicked;

		// todo create events so we can know when a row is selected
		private List<MenuItem> _items;
		private string cellIdentifier = "MainMenuTableViewCell";

		public MainMenuViewSource (List<MenuItem> items)
		{
			_items = items;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return _items.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = TableCellFactory<MainMenuTableViewCell>.GetCell(tableView,cellIdentifier,cellIdentifier);
			cell.Bind(_items[indexPath.Row]);
			return cell;
		}

		//		private int lastRowClicked = -1;
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			//			if (lastRowClicked != indexPath.Row) {
			if(!_items[indexPath.Row].IsHeader) OnRowClicked (_items [indexPath.Row]);
			//				lastRowClicked = indexPath.Row;
			//			}
		}

		private void OnRowClicked (MenuItem item)
		{
			if(RowClicked!=null)
				RowClicked(this, new RowClickedEventArgs<MenuItem>(){ Item = item});
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (_items [indexPath.Row].IsHeader)
				return MainMenuTableCellView.CELL_HEIGHT_HEADER;
			else
				return MainMenuTableCellView.CELL_HEIGHT_ITEM;
		}
	}


}


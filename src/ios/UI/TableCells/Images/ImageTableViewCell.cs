using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using RedBit.CCAdmin;
using RedBit;
using System.Collections.Generic;
using SDWebImage;

namespace BackChannel
{
	/// <summary>
	/// Custom table cell for menu items
	/// </summary>
	public partial class ImageTableViewCell : UITableViewCell
	{
		public ImageTableViewCell () : base ()
		{

		}


		public ImageTableViewCell(IntPtr handle) : base(handle){
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
		}

		public UIImageView ContentImage {get{return this.image;}}
		public UIView ContentView {get{return this.contentView;}}

		private Image _contentItem;
		private UIImageTableView _parent;

		public void Bind (Image contentItem, UIImageTableView parent)
		{
			_parent = parent;
			lblDate.Text = contentItem.DateTime.ToLocalTime().ToFriendlyDate ();
			lblAuthor.Text = contentItem.AuthorName;
			btnDelete.SetImage (UIImage.FromFile ("images/icons/delete.png"), UIControlState.Normal);
			btnDelete.TouchUpInside += (object sender, EventArgs e) => {
				_parent.RemoveRow(_contentItem);
			};
			btnPromote.SetImage (UIImage.FromFile ("images/icons/promote.png"), UIControlState.Normal);
			btnPromote.TouchUpInside += (object sender, EventArgs e) => {
				_parent.PromoteRow(_contentItem);
			};
			_contentItem = contentItem;
		}
	}



	/// <summary>
	/// view source for the table so we know what is selected
	/// </summary>
	public class ImageViewSource : UITableViewSource
	{

		internal const int CELL_HEIGHT_ITEM = 93;

		/// <summary>
		/// Event that is raised when a row is clicked
		/// </summary>
		public event EventHandler<RowClickedEventArgs<Image>> RowClicked;

		// todo create events so we can know when a row is selected
		private List<Image> _items;
		private string cellIdentifier = "ImageTableViewCell";

		public ImageViewSource (List<Image> items)
		{
			_items = items;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return _items.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = TableCellFactory<ImageTableViewCell>.GetCell(tableView,"cellIdentifier",cellIdentifier);
			cell.Bind(_items[indexPath.Row], tableView as UIImageTableView);
			cell.ContentImage.SetImage (url:new NSUrl(_items[indexPath.Row].Content),
			                            placeholder: UIImage.FromFile("images/placeholder.png"));

			return cell;
		}

		public int RemoveItem(Image image){
			var index = _items.FindIndex((t)=>{return t.Id == image.Id;});
			_items.RemoveAt(index);
			return index;
		}

		//		private int lastRowClicked = -1;
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			OnRowClicked (_items [indexPath.Row]);
		}

		private void OnRowClicked (Image item)
		{
			if(RowClicked!=null)
				RowClicked(this, new RowClickedEventArgs<Image>(){ Item = item});
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return CELL_HEIGHT_ITEM;
		}
	}
}


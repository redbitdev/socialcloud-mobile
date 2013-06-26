using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using RedBit.CCAdmin;
using RedBit;
using System.Collections.Generic;

namespace BackChannel
{
	/// <summary>
	/// Custom table cell for menu items
	/// </summary>
	public partial class BlackListTableViewCell : UITableViewCell
	{
		public BlackListTableViewCell () : base ()
		{

		}


		public BlackListTableViewCell(IntPtr handle) : base(handle){
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
		}

		public UIView ContentView2 {get{return contentView;}}

		private BlacklistItem _contentItem;
		private UIBlackListTableView _parent;

		public void Bind (BlacklistItem contentItem, UIBlackListTableView parent)

		{
			_parent = parent;
			lblBlacklist.Text = contentItem.Value;

			btnDelete.SetImage (UIImage.FromFile ("images/icons/delete.png"), UIControlState.Normal);
			btnDelete.TouchUpInside += (object sender, EventArgs e) => {
				_parent.DeleteItem(_contentItem);
			};

			_contentItem = contentItem;
		}
	}



	/// <summary>
	/// view source for the table so we know what is selected
	/// </summary>
	public class BlackListViewSource : UITableViewSource
	{

		internal const int CELL_HEIGHT_ITEM = 61;

		/// <summary>
		/// Event that is raised when a row is clicked
		/// </summary>
		public event EventHandler<RowClickedEventArgs<BlacklistItem>> RowClicked;

		// todo create events so we can know when a row is selected
		private List<BlacklistItem> _items;
		private string cellIdentifier = "BlackListTableViewCell";

		public BlackListViewSource (List<BlacklistItem> items)
		{
			_items = items;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return _items.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = TableCellFactory<BlackListTableViewCell>.GetCell(tableView,"cellIdentifier",cellIdentifier);
			cell.Bind(_items[indexPath.Row], tableView as UIBlackListTableView);

			return cell;
		}

		public int RemoveItem(BlacklistItem tweet){
			var index = _items.FindIndex((t)=>{return t.Value == tweet.Value;});
			_items.RemoveAt(index);
			return index;
		}

		//		private int lastRowClicked = -1;
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			OnRowClicked (_items [indexPath.Row]);
		}

		private void OnRowClicked (BlacklistItem item)
		{
			if(RowClicked!=null)
				RowClicked(this, new RowClickedEventArgs<BlacklistItem>(){ Item = item});
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return CELL_HEIGHT_ITEM;
		}
	}
}


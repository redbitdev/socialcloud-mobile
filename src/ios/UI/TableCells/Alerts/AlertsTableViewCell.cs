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
	public partial class AlertsTableViewCell : UITableViewCell
	{
		public AlertsTableViewCell () : base ()
		{

		}


		public AlertsTableViewCell(IntPtr handle) : base(handle){
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
		}

		public UIView ContentView2 {get{return contentView;}}

		private Alert _contentItem;
		private UIAlertsTableView _parent;

		public void Bind (Alert contentItem, UIAlertsTableView parent)

		{
			_parent = parent;
			lblAlertName.Text = contentItem.RuleName;
			lblCount.Text = "Total Count: " + contentItem.Count;
			lblDateTime.Text = contentItem.Dt.ToFriendlyDate ();

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
	public class AlertsViewSource : UITableViewSource
	{

		internal const int CELL_HEIGHT_ITEM = 85;

		/// <summary>
		/// Event that is raised when a row is clicked
		/// </summary>
		public event EventHandler<RowClickedEventArgs<Alert>> RowClicked;

		// todo create events so we can know when a row is selected
		private List<Alert> _items;
		private string cellIdentifier = "AlertsTableViewCell";

		public AlertsViewSource (List<Alert> items)
		{
			_items = items;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return _items.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = TableCellFactory<AlertsTableViewCell>.GetCell(tableView,"cellIdentifier",cellIdentifier);
			cell.Bind(_items[indexPath.Row], tableView as UIAlertsTableView);

			return cell;
		}

		public int RemoveItem(Alert tweet){
			var index = _items.FindIndex((t)=>{return t.Id == tweet.Id;});
			_items.RemoveAt(index);
			return index;
		}

		//		private int lastRowClicked = -1;
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			OnRowClicked (_items [indexPath.Row]);
		}

		private void OnRowClicked (Alert item)
		{
			if(RowClicked!=null)
				RowClicked(this, new RowClickedEventArgs<Alert>(){ Item = item});
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return CELL_HEIGHT_ITEM;
		}
	}
}


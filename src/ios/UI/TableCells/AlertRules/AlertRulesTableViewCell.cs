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
	public partial class AlertRulesTableViewCell : UITableViewCell
	{
		public AlertRulesTableViewCell () : base ()
		{

		}


		public AlertRulesTableViewCell(IntPtr handle) : base(handle){
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
		}

		public UIView ContentView2 {get{return contentView;}}

		private Rule _contentItem;
		private UIAlertRulesTableView _parent;

		public void Bind (Rule contentItem, UIAlertRulesTableView parent)

		{
			_parent = parent;
			lblRuleName.Text = contentItem.RuleName;
			lblMobile.Text = "Send Mobile: " + contentItem.SendMobile;
			lblSearchTerm.Text = contentItem.SearchTerm;
			lblSendDash.Text = "Send Dash:" + contentItem.SendDashboard;
			lblSendEmail.Text = "Send Email: " + contentItem.SendEmail;
			lblSendText.Text = "Send Text: " + contentItem.SendText;

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
	public class AlertRulesViewSource : UITableViewSource
	{

		internal const int CELL_HEIGHT_ITEM = 95;

		/// <summary>
		/// Event that is raised when a row is clicked
		/// </summary>
		public event EventHandler<RowClickedEventArgs<Rule>> RowClicked;

		// todo create events so we can know when a row is selected
		private List<Rule> _items;
		private string cellIdentifier = "AlertRulesTableViewCell";

		public AlertRulesViewSource (List<Rule> items)
		{
			_items = items;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return _items.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = TableCellFactory<AlertRulesTableViewCell>.GetCell(tableView,"cellIdentifier",cellIdentifier);
			cell.Bind(_items[indexPath.Row], tableView as UIAlertRulesTableView);

			return cell;
		}

		public int RemoveItem(Rule tweet){
			var index = _items.FindIndex((t)=>{return t.Id == tweet.Id;});
			_items.RemoveAt(index);
			return index;
		}

		//		private int lastRowClicked = -1;
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			OnRowClicked (_items [indexPath.Row]);
		}

		private void OnRowClicked (Rule item)
		{
			if(RowClicked!=null)
				RowClicked(this, new RowClickedEventArgs<Rule>(){ Item = item});
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return CELL_HEIGHT_ITEM;
		}
	}
}


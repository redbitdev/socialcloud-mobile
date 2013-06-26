using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using RedBit;
using RedBit.CCAdmin;

namespace BackChannel
{
	public partial class SettingsTableViewCell : UITableViewCell
	{
		public SettingsTableViewCell () : base ()
		{

		}


		public SettingsTableViewCell(IntPtr handle) : base(handle){
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
		}

		Setting _settingItem;
		public void Bind (Setting settingItem)
		{
			_settingItem = settingItem;
			this.lblKey.Text = _settingItem.Key;
			this.lblValue.Text = _settingItem.Value;
		}
	}

	/// <summary>
	/// view source for the table so we know what is selected
	/// </summary>
	public class SettingsViewSource : UITableViewSource
	{

		internal const int CELL_HEIGHT_ITEM = 44;

		/// <summary>
		/// Event that is raised when a row is clicked
		/// </summary>
		public event EventHandler<RowClickedEventArgs<Setting>> RowClicked;

		// todo create events so we can know when a row is selected
		private List<Setting> _items;
		private string cellIdentifier = "SettingsTableViewCell";

		public SettingsViewSource (List<Setting> items)
		{
			_items = items;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return _items.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = TableCellFactory<SettingsTableViewCell>.GetCell(tableView,"cellIdentifier",cellIdentifier);
			cell.Bind(_items[indexPath.Row]);

			return cell;
		}

		//		private int lastRowClicked = -1;
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			OnRowClicked (_items [indexPath.Row]);
		}

		private void OnRowClicked (Setting item)
		{
			if(RowClicked!=null)
				RowClicked(this, new RowClickedEventArgs<Setting>(){ Item = item});
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return CELL_HEIGHT_ITEM;
		}
	}
}


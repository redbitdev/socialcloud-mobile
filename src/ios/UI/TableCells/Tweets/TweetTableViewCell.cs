
using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Collections.Generic;
using RedBit;
using RedBit.CCAdmin;
using SDWebImage;
using MonoTouch.ObjCRuntime;

namespace BackChannel
{
	/// <summary>
	/// Custom table cell for menu items
	/// </summary>
	public partial class TweetTableViewCell : UITableViewCell
	{
		public TweetTableViewCell () : base ()
		{

		}


		public TweetTableViewCell(IntPtr handle) : base(handle){
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
		}

		public UIView TweetView {get{return tweetView;}}
		public UIImageView ProfileImage {get{return this.image;}}

		private Tweet _contentItem;
		private UITweetTableView _parent;

		public void Bind (Tweet contentItem, UITweetTableView parent)

		{
			_parent = parent;
			lblTweet.Text = contentItem.Content;
			lblDate.Text = contentItem.DateTime.ToLocalTime().ToFriendlyDate ();
			lblUser.Text = contentItem.AuthorName;
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
	public class TweetViewSource : UITableViewSource
	{

		internal const int CELL_HEIGHT_ITEM = 90;

		/// <summary>
		/// Event that is raised when a row is clicked
		/// </summary>
		public event EventHandler<RowClickedEventArgs<Tweet>> RowClicked;

		// todo create events so we can know when a row is selected
		private List<Tweet> _items;
		private string cellIdentifier = "TweetTableViewCell";

		public TweetViewSource (List<Tweet> items)
		{
			_items = items;
		}

		public override int RowsInSection (UITableView tableview, int section)
		{
			return _items.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = TableCellFactory<TweetTableViewCell>.GetCell(tableView,"cellIdentifier",cellIdentifier);
			cell.Bind(_items[indexPath.Row], tableView as UITweetTableView);
			cell.ProfileImage.SetImage (url:new NSUrl(_items[indexPath.Row].AuthorProfileUrl),
			                         placeholder: UIImage.FromFile("images/placeholder.png"));
		
			return cell;
		}

		public int RemoveItem(Tweet tweet){
			var index = _items.FindIndex((t)=>{return t.Id == tweet.Id;});
			_items.RemoveAt(index);
			return index;
		}

		//		private int lastRowClicked = -1;
		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			OnRowClicked (_items [indexPath.Row]);
		}

		private void OnRowClicked (Tweet item)
		{
			if(RowClicked!=null)
				RowClicked(this, new RowClickedEventArgs<Tweet>(){ Item = item});
		}

		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return CELL_HEIGHT_ITEM;
		}
	}


}


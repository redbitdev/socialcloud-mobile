using MonoTouch.UIKit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using RedBit.CCAdmin;
using AlertView;
using RedBit;

namespace BackChannel
{
    public class UITweetsView : UIView
    {
        public UITweetsView() { }

        public override RectangleF Frame
        {
            get
            {
                return base.Frame;
            }
            set
            {
                base.Frame = value;
                // layout the controls
                Layout();
            }
        }

		public Action<Tweet> TweetClicked {get;set;}
		public Tweet CurrentTweet { get; set; }
        private UITweetTableView _tableView;

        void Layout()
        {
			if (_tableView == null) {
				_tableView = new UITweetTableView ();
				var t = this.Frame;
				_tableView.Frame = new RectangleF (0, 0, UIScreen.MainScreen.ApplicationFrame.Width, UIScreen.MainScreen.ApplicationFrame.Height - 44); // 44 is height of nav bar
				//				_tableView.ScrollEnabled = false;
				_tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
				_tableView.BackgroundColor = UIColor.FromRGB (242, 242, 242); //UIColor.FromPatternImage (UIImage.FromFile("images/login/loginBg.png"));

				// add a swipe handler for options
				var s = new UISwipeGestureRecognizer (SwipeHandler);
				s.Direction = UISwipeGestureRecognizerDirection.Left;
				_tableView.AddGestureRecognizer (s);
				s = new UISwipeGestureRecognizer (SwipeHandler);
				s.Direction = UISwipeGestureRecognizerDirection.Right;
				_tableView.AddGestureRecognizer (s);

				// wure up the remove row
				_tableView.RemoveRow = (tweet) => {

					var alert = MBAlertView.AlertWithBody ("Are you sure you want to delete this tweet? You cannot undo this.", "No", null);
					alert.AddButtonWithText ("Yes", MBAlertViewItemType.Destructive, () => {
						// call server to delete the tweet
						Helper.Default.ShowHud("deleting ...");
						Api.Default.DeleteTweetItem (tweet, (result) => {
							this.InvokeOnMainThread(()=>{
								if (result.Result == "ok") {
									RemoveTweetFromView (tweet);
									Helper.Default.HideHud();
								}
								else{
									Helper.Default.HideHud("Could not delete, try again.");
								}
							});
						});
					});
					alert.AddToDisplayQueue ();
				};

				_tableView.PromoteRow = (tweet) => {

					var alert = MBAlertView.AlertWithBody ("Are you sure you want to promote this tweet? You cannot undo this.", "No", null);
					alert.AddButtonWithText ("Yes", MBAlertViewItemType.Positive, () => {
						// call server to delete the tweet
						Helper.Default.ShowHud("promoting ...");
						Api.Default.PromoteTweetItem (tweet, (result) => {
							this.InvokeOnMainThread(()=>{
								if (result.Result == "ok") {
									Helper.Default.HideHud("Done!");
									_lastcell.Animate2 (0.2, ()=>AnimateCellRight(_lastcell), ()=>{
										_lastcell = null;
									});
								}
								else{
									Helper.Default.HideHud("Could not promote, try again.");
								}
							});
						});
					});
					alert.AddToDisplayQueue ();
				};


				// add to view
				this.AddSubview (_tableView);
			} 
        }
		private TweetViewSource TweetViewSource { get{return this._tableView.Source as TweetViewSource;}}

		private UIView _lastcell = null;
		private void SwipeHandler(UISwipeGestureRecognizer args){
			if(args.State == UIGestureRecognizerState.Ended){
				// find the cell
				var swipeLocation = args.LocationInView(_tableView);
				var index = _tableView.IndexPathForRowAtPoint(swipeLocation);
				var cell = _tableView.CellAt(index);

				var view = ((TweetTableViewCell)cell).TweetView;
				if (args.Direction == UISwipeGestureRecognizerDirection.Left) {
					if(_lastcell != null){
						_lastcell.Animate2 (0.2, ()=>AnimateCellRight(_lastcell), ()=>AnimateCellLeft(view));
					}
					_lastcell = view;
					view.Animate2 (0.2, ()=>AnimateCellLeft(view));
				}
				else if(args.Direction == UISwipeGestureRecognizerDirection.Right){
					_lastcell = null;
					view.Animate2 (0.2, ()=>AnimateCellRight(view));
				}
			}
		}

		public void RemoveTweetFromView (Tweet tweet)
		{
			var index = this.TweetViewSource.RemoveItem (tweet);
			this._tableView.DeleteRows (new MonoTouch.Foundation.NSIndexPath[] {
				MonoTouch.Foundation.NSIndexPath.Create (0, index)
			}, UITableViewRowAnimation.Fade);
		}

		private void AnimateCellRight(UIView view){
			view.Frame = new RectangleF(
				0,
				view.Frame.Y,
				view.Frame.Width,
				view.Frame.Height);
		}

		private void AnimateCellLeft(UIView view){
			view.Frame = new RectangleF(
				-(view.Frame.Width / 2),
				view.Frame.Y,
				view.Frame.Width,
				view.Frame.Height);
		}

		public void LoadTweets(){
			Helper.Default.ShowHud ("loading tweets");
			Api.Default.GetTweetsAsync ((results)=>{
				InvokeOnMainThread(()=> {
					if(results.Error == null){
						_tableView.Source = new TweetViewSource(results.Results.ToList());
						(_tableView.Source as TweetViewSource).RowClicked += RowClickHandler;
						_tableView.ReloadData();
						Helper.Default.HideHud();
					}
					else{
						Helper.Default.HideHud(null, MBAlertViewHUDType.ExclamationMark, 0);
						var alert = MBAlertView.AlertWithBody ("Unable to get tweets :( Error: " + results.Error.Message + ".", "Try Again", ()=>{
							this.BeginInvokeOnMainThread(LoadTweets);
						});
						alert.AddToDisplayQueue ();
					}
				});
			});
		}

		private void RowClickHandler(object sender, RedBit.RowClickedEventArgs<Tweet> e){
			CurrentTweet = e.Item;
			if (TweetClicked != null) {
				TweetClicked (e.Item);
			}
		}

    }

	public class UITweetTableView : UITableView
	{
		public UITweetTableView(){
		}

		public Action<Tweet> RemoveRow { get; set; }

		public Action<Tweet> PromoteRow {get;set; }
	}
}

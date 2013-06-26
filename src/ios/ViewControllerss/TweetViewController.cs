using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using RedBit.CCAdmin;
using RedBit;
using AlertView;
using System.Threading;
using System.Collections.Generic;
using SDWebImage;

namespace BackChannel
{
	public partial class TweetViewController : UIViewController
	{
		public TweetViewController (IntPtr handle) : base (handle)
		{
		}

		public Action Done { get; set;}

		public Tweet TweetItem { get; set; }

		public bool WasDeleted { get; set; }

		void OnDone ()
		{
			if (Done != null)
				Done ();
			this.DismissViewControllerAsync (true);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.btnDone.Clicked += (object sender, EventArgs e) => {
				OnDone ();
			};

			this.btnDelete.Clicked += (object sender, EventArgs e) => {
				var alert = MBAlertView.AlertWithBody ("Are you sure you want to delete this tweet? You cannot undo this.", "No", null);
				alert.AddButtonWithText ("Yes", MBAlertViewItemType.Destructive, () => {
					// call server to delete the tweet
					Helper.Default.ShowHud("deleting ...");
					Api.Default.DeleteTweetItem (TweetItem, (result) => {
						this.InvokeOnMainThread(()=>{
							if (result.Result == "ok") {
								Helper.Default.HideHud();
								WasDeleted = true;
								OnDone();
							}
							else{
								Helper.Default.HideHud("Could not delete, try again.");
							}
						});
					});
				});
				alert.AddToDisplayQueue ();
			};

			this.btnPromote.Clicked += (object sender, EventArgs e) => {
				var alert = MBAlertView.AlertWithBody ("Are you sure you want to promote this tweet? You cannot undo this.", "No", null);
				alert.AddButtonWithText ("Yes", MBAlertViewItemType.Positive, () => {
					// call server to delete the tweet
					Helper.Default.ShowHud("promoting ...");
					Api.Default.PromoteTweetItem (TweetItem, (result) => {
						this.InvokeOnMainThread(()=>{
							if (result.Result == "ok") {
								Helper.Default.HideHud("Done!");
								this.btnPromote.Enabled = false;
							}
							else{
								Helper.Default.HideHud("Could not promote, try again.");
							}
						});
					});
				});
				alert.AddToDisplayQueue ();
			};

			this.lblDate.Text = TweetItem.DateTime.ToFriendlyDate ();
			this.lblTweet.Text = TweetItem.Content;
			this.lblUser.Text = TweetItem.AuthorName;
			this.image.SetImage (url:new NSUrl(TweetItem.AuthorProfileUrl),
			                     placeholder: UIImage.FromFile("images/placeholder.png"));
		}

		private void SetLabel(string content){
//			var s = new NSString (content);
//			System.Diagnostics.Debug.WriteLine (content);
//			var size = new SizeF (lblTweet.Frame.Width, lblTweet.Frame.Height);
//			var sSize = s.StringSize (lblTweet.Font, size, UILineBreakMode.WordWrap);
//			lblTweet.Frame = new RectangleF (lblTweet.Frame.X, lblTweet.Frame.Y, sSize.Width, sSize.Height);
			var t = lblTweet.Frame;
			lblTweet.BackgroundColor = UIColor.Purple;
			lblTweet.Text = content;
			lblTweet.SizeToFit ();
//			s.Dispose ();
//			s = null;
		}

	}
}


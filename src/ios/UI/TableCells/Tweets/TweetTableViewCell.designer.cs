// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace BackChannel
{
	[Register ("TweetTableViewCell")]
	partial class TweetTableViewCell
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnPromote { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnDelete { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblTweet { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblDate { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblUser { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView image { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView tweetView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnPromote != null) {
				btnPromote.Dispose ();
				btnPromote = null;
			}

			if (btnDelete != null) {
				btnDelete.Dispose ();
				btnDelete = null;
			}

			if (lblTweet != null) {
				lblTweet.Dispose ();
				lblTweet = null;
			}

			if (lblDate != null) {
				lblDate.Dispose ();
				lblDate = null;
			}

			if (lblUser != null) {
				lblUser.Dispose ();
				lblUser = null;
			}

			if (image != null) {
				image.Dispose ();
				image = null;
			}

			if (tweetView != null) {
				tweetView.Dispose ();
				tweetView = null;
			}
		}
	}
}

// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace BackChannel
{
	[Register ("TweetViewController")]
	partial class TweetViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem btnDelete { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem btnDone { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIBarButtonItem btnPromote { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView image { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblDate { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblTweet { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblUser { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnDone != null) {
				btnDone.Dispose ();
				btnDone = null;
			}

			if (image != null) {
				image.Dispose ();
				image = null;
			}

			if (lblUser != null) {
				lblUser.Dispose ();
				lblUser = null;
			}

			if (lblTweet != null) {
				lblTweet.Dispose ();
				lblTweet = null;
			}

			if (lblDate != null) {
				lblDate.Dispose ();
				lblDate = null;
			}

			if (btnPromote != null) {
				btnPromote.Dispose ();
				btnPromote = null;
			}

			if (btnDelete != null) {
				btnDelete.Dispose ();
				btnDelete = null;
			}
		}
	}
}

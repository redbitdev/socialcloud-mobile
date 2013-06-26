// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace BackChannel
{
	[Register ("ImageTableViewCell")]
	partial class ImageTableViewCell
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView image { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblDate { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblAuthor { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnPromote { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnDelete { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView contentView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (image != null) {
				image.Dispose ();
				image = null;
			}

			if (lblDate != null) {
				lblDate.Dispose ();
				lblDate = null;
			}

			if (lblAuthor != null) {
				lblAuthor.Dispose ();
				lblAuthor = null;
			}

			if (btnPromote != null) {
				btnPromote.Dispose ();
				btnPromote = null;
			}

			if (btnDelete != null) {
				btnDelete.Dispose ();
				btnDelete = null;
			}

			if (contentView != null) {
				contentView.Dispose ();
				contentView = null;
			}
		}
	}
}

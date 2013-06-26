// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace BackChannel
{
	[Register ("BlackListTableViewCell")]
	partial class BlackListTableViewCell
	{
		[Outlet]
		MonoTouch.UIKit.UILabel lblBlacklist { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView contentView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnEdit { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnDelete { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lblBlacklist != null) {
				lblBlacklist.Dispose ();
				lblBlacklist = null;
			}

			if (contentView != null) {
				contentView.Dispose ();
				contentView = null;
			}

			if (btnEdit != null) {
				btnEdit.Dispose ();
				btnEdit = null;
			}

			if (btnDelete != null) {
				btnDelete.Dispose ();
				btnDelete = null;
			}
		}
	}
}

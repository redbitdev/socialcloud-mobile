// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace BackChannel
{
	[Register ("AlertsTableViewCell")]
	partial class AlertsTableViewCell
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnDelete { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView contentView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblAlertName { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblCount { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblDateTime { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lblAlertName != null) {
				lblAlertName.Dispose ();
				lblAlertName = null;
			}

			if (lblCount != null) {
				lblCount.Dispose ();
				lblCount = null;
			}

			if (lblDateTime != null) {
				lblDateTime.Dispose ();
				lblDateTime = null;
			}

			if (contentView != null) {
				contentView.Dispose ();
				contentView = null;
			}

			if (btnDelete != null) {
				btnDelete.Dispose ();
				btnDelete = null;
			}
		}
	}
}

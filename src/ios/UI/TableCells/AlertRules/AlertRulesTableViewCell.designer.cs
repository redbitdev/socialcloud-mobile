// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace BackChannel
{
	[Register ("AlertRulesTableViewCell")]
	partial class AlertRulesTableViewCell
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnDelete { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView contentView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblMobile { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblRuleName { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblSearchTerm { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblSendDash { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblSendEmail { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblSendText { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lblRuleName != null) {
				lblRuleName.Dispose ();
				lblRuleName = null;
			}

			if (lblMobile != null) {
				lblMobile.Dispose ();
				lblMobile = null;
			}

			if (lblSearchTerm != null) {
				lblSearchTerm.Dispose ();
				lblSearchTerm = null;
			}

			if (lblSendDash != null) {
				lblSendDash.Dispose ();
				lblSendDash = null;
			}

			if (lblSendEmail != null) {
				lblSendEmail.Dispose ();
				lblSendEmail = null;
			}

			if (lblSendText != null) {
				lblSendText.Dispose ();
				lblSendText = null;
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

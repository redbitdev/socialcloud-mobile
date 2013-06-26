// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace BackChannel
{
	[Register ("AddAlertRuleViewController")]
	partial class AddAlertRuleViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel lblCount { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIScrollView scrollView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISlider sliderCount { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISwitch switchAppAlert { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISwitch switchDashboard { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISwitch switchEmail { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISwitch switchSMS { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField txtCellNumber { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField txtEmail { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField txtRuleName { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField txtSearchTerm { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lblCount != null) {
				lblCount.Dispose ();
				lblCount = null;
			}

			if (sliderCount != null) {
				sliderCount.Dispose ();
				sliderCount = null;
			}

			if (switchAppAlert != null) {
				switchAppAlert.Dispose ();
				switchAppAlert = null;
			}

			if (switchDashboard != null) {
				switchDashboard.Dispose ();
				switchDashboard = null;
			}

			if (switchEmail != null) {
				switchEmail.Dispose ();
				switchEmail = null;
			}

			if (switchSMS != null) {
				switchSMS.Dispose ();
				switchSMS = null;
			}

			if (txtCellNumber != null) {
				txtCellNumber.Dispose ();
				txtCellNumber = null;
			}

			if (txtEmail != null) {
				txtEmail.Dispose ();
				txtEmail = null;
			}

			if (txtRuleName != null) {
				txtRuleName.Dispose ();
				txtRuleName = null;
			}

			if (txtSearchTerm != null) {
				txtSearchTerm.Dispose ();
				txtSearchTerm = null;
			}

			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}
		}
	}
}

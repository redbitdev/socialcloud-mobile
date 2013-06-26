// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace BackChannel
{
	[Register ("SettingsTableViewCell")]
	partial class SettingsTableViewCell
	{
		[Outlet]
		MonoTouch.UIKit.UILabel lblKey { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel lblValue { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (lblKey != null) {
				lblKey.Dispose ();
				lblKey = null;
			}

			if (lblValue != null) {
				lblValue.Dispose ();
				lblValue = null;
			}
		}
	}
}

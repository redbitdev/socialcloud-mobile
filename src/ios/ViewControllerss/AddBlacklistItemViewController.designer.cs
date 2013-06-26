// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace BackChannel
{
	[Register ("AddBlacklistItemViewController")]
	partial class AddBlacklistItemViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITextField txtBlacklistItemText { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txtBlacklistItemText != null) {
				txtBlacklistItemText.Dispose ();
				txtBlacklistItemText = null;
			}
		}
	}
}

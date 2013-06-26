using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using RedBit.CCAdmin;
using RedBit;

namespace BackChannel
{
	public partial class AddBlacklistItemViewController : UIViewController
	{
		public AddBlacklistItemViewController (IntPtr handle) : base (handle)
		{
		}

		public Action Done { get; set;}

		public Action Cancel { get; set; }


		public string BlacklistItemValue {get {return txtBlacklistItemText.Text;}}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.NavigationItem.RightBarButtonItem.Clicked+= (object sender, EventArgs e) => {

				// save and upload
				Helper.Default.ShowHud("updating ...");
				Api.Default.AddBlackListItem(BlacklistItemValue, (result)=>{
					this.InvokeOnMainThread(()=>{
						if (result.Result == "ok") {
							if(Done!= null)
								Done();
							this.DismissViewControllerAsync(true);
							Helper.Default.HideHud();
						}
						else{
							Helper.Default.HideHud("Could not add, try again.", AlertView.MBAlertViewHUDType.ExclamationMark);
						}
					});
				});
			};

			this.NavigationItem.LeftBarButtonItem.Clicked += (object sender, EventArgs e) => {
				if(Cancel!=null)
					Cancel();
				
				this.DismissViewControllerAsync(true);
			};


		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

	}
}


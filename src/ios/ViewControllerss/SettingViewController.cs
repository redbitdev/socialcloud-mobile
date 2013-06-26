using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using RedBit.CCAdmin;
using RedBit;
using AlertView;
using System.Threading;
using System.Collections.Generic;
using SDWebImage;

namespace BackChannel
{
	public partial class SettingViewController : UIViewController
	{
		public SettingViewController (IntPtr handle) : base (handle)
		{
		}

		public Action Save { get; set;}
		public Action Cancel {get;set;}

		public Setting SettingItem { get; set; }

		void OnSave ()
		{
			if (Save != null)
				Save ();
			this.DismissViewControllerAsync (true);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.btnSave.Clicked+= (object sender, EventArgs e) => {

				// save and upload
				Helper.Default.ShowHud("saving ...");
				Api.Default.SaveSettingAsync(SettingItem, (result)=>{
					this.InvokeOnMainThread(()=>{
						if (result.Result == "ok") {
							if(Save!= null)
								Save();
							this.DismissViewControllerAsync(true);
							Helper.Default.HideHud();
						}
						else{
							Helper.Default.HideHud("Could not save, try again.", AlertView.MBAlertViewHUDType.ExclamationMark);
						}
					});
				});
			};

			this.btnCancel.Clicked += (object sender, EventArgs e) => {
				if(Cancel!=null)
					Cancel();

				this.DismissViewControllerAsync(true);
			};

			lblKey.Text = SettingItem.Key;
			lblValue.Text = SettingItem.Value;
		}


	}
}


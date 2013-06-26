using MonoTouch.UIKit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using RedBit.CCAdmin;
using AlertView;
using RedBit;

namespace BackChannel
{
	public class UISettingView : UIView
    {
		public UISettingView() { }

        public override RectangleF Frame
        {
            get
            {
                return base.Frame;
            }
            set
            {
                base.Frame = value;
                // layout the controls
                Layout();
            }
        }

		public Action<Setting> SettingClicked {get;set;}
		public Setting CurrentSettingItem { get; set; }
		private UITableView _tableView;

        void Layout()
        {
			if (_tableView == null) {
				_tableView = new UITableView ();
				var t = this.Frame;
				_tableView.Frame = new RectangleF (0, 0, UIScreen.MainScreen.ApplicationFrame.Width, UIScreen.MainScreen.ApplicationFrame.Height - 44); // 44 is height of nav bar
				//				_tableView.ScrollEnabled = false;
				_tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
				_tableView.BackgroundColor = UIColor.FromRGB (242, 242, 242); //UIColor.FromPatternImage (UIImage.FromFile("images/login/loginBg.png"));

			
				// add to view
				this.AddSubview (_tableView);
			} 
        }
		private SettingsViewSource ViewSource { get{return this._tableView.Source as SettingsViewSource;}}

		public void LoadContent(){
			Helper.Default.ShowHud ("loading settings");
			Api.Default.GetSettingsAsync ((results)=>{
				InvokeOnMainThread(()=> {
					if(results.Error == null){
						_tableView.Source = new SettingsViewSource(results.Results.ToList());
						(_tableView.Source as SettingsViewSource).RowClicked += RowClickHandler;
						_tableView.ReloadData();
						Helper.Default.HideHud();
					}
					else{
						Helper.Default.HideHud(null, MBAlertViewHUDType.ExclamationMark,0);
						var alert = MBAlertView.AlertWithBody ("Unable to get settings :( Error: " + results.Error.Message + ".", "Try Again", ()=>{
							this.BeginInvokeOnMainThread(LoadContent);
						});
						alert.AddToDisplayQueue ();
					}
				});
			});
		}

		private void RowClickHandler(object sender, RedBit.RowClickedEventArgs<Setting> e){
			CurrentSettingItem = e.Item;
			if (SettingClicked != null)
				SettingClicked (e.Item);
		}


    }
}

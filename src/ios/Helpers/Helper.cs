using System;
using AlertView;

namespace RedBit
{
	public class Helper
	{
		private static Helper _default;
		public static Helper Default{
			get{
				if (_default == null)
					_default = new Helper ();
				return _default;
			}
		}
		private Helper ()
		{
		}

		public void ShowHud(string text){
			MBHUDView.HudWithBody (
				body: text, 
				aType: MBAlertViewHUDType.ActivityIndicator, 
				delay: 0.0f, 
				showNow: true
				);
		}

		public void HideHud(string text = null, MBAlertViewHUDType  type = MBAlertViewHUDType.Checkmark, int delay = 2){
			MBHUDView.DismissCurrentHUD ();
			if (text != null) {
				MBHUDView.HudWithBody (
					body: text, 
					aType: type, 
					delay: 0.0f,
					showNow: true
					);

				MBHUDView.DismissCurrentHUD (delay);
			}
		}
	}
}


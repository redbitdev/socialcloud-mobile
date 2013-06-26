using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace BackChannel
{
	public partial class LoginView : UIView
	{
		public LoginView ()
		{

		}

        public Action LoginClicked { get; internal set; }

		public override RectangleF Frame {
			get {
				return base.Frame;
			}
			set {
				base.Frame = value;
				// layout the controls
				Layout ();
			}
		}

		private UIImageView _whiteBg;
		private UIView _title;
		private UIButton _btnLogin;

		private void Layout(){
			// add the white bg
			if(_whiteBg == null){
				var img = UIImage.FromFile ("images/login/loginBgWhite.png");
				var size = img.CGImage.GetScaledSizeForControl ();
				_whiteBg = new UIImageView (img);
				_whiteBg.Frame = new RectangleF (this.Frame.Width/2 - size.Width/2,
				                                this.Frame.Height / 2 - size.Width / 2,
				                                size.Width, size.Height);

				// create the login button
				img = UIImage.FromFile ("images/login/btnLogin.png");
				_btnLogin = new UIButton (UIButtonType.Custom);
				size = img.CGImage.GetScaledSizeForControl ();
				_btnLogin.Frame = new RectangleF (
					_whiteBg.Frame.Width / 2 - size.Width /2,
					_whiteBg.Frame.Height - size.Height - 10, 
					size.Width, size.Height); 
				_btnLogin.SetBackgroundImage (img, UIControlState.Normal);
				_btnLogin.SetBackgroundImage (UIImage.FromFile ("images/login/btnLoginSelected.png"), UIControlState.Highlighted);
				_btnLogin.SetBackgroundImage (UIImage.FromFile ("images/login/btnLoginSelected.png"), UIControlState.Selected);
				_btnLogin.TouchUpInside += (object sender, EventArgs e) => {
					if(LoginClicked!=null)
						LoginClicked();
				};

				// add the title + icon
				var lblHeight = 20;
				_title = new UIView ();
				_title.Frame = new RectangleF (_whiteBg.Frame.X, _whiteBg.Frame.Top - _whiteBg.Frame.Height / 2 - 20,
				                              _whiteBg.Frame.Width, _whiteBg.Frame.Height / 2);
				img = UIImage.FromFile ("images/icon.png");
				size = img.CGImage.GetScaledSizeForControl ();
				var uiimg = new UIImageView (img);
				uiimg.Frame = new RectangleF (5, 5, size.Width, size.Height);
				var lbl = new UILabel ();
				lbl.Text = "Social Cloud Admin";
				lbl.BackgroundColor = UIColor.Clear;
				lbl.TextColor = UIColor.White;
				lbl.Frame = new RectangleF (size.Width + 20, _title.Frame.Height / 2 - lblHeight / 2,
				                           _title.Frame.Width - size.Width, lblHeight);
				_title.AddSubviews(uiimg,lbl);

				// add to the main view
				this.AddSubviews (_whiteBg, _btnLogin, _title);
			}
			else{
				_whiteBg.Frame = new RectangleF (this.Frame.Width/2 - _whiteBg.Frame.Width/2,
				                                 this.Frame.Height / 2 - _whiteBg.Frame.Width / 2,
				                                 _whiteBg.Frame.Width, _whiteBg.Frame.Height);

				_btnLogin.Frame = new RectangleF (
					(_whiteBg.Frame.Width / 2 - _btnLogin.Frame.Width /2) + _whiteBg.Frame.Left,
					(_whiteBg.Frame.Height/2 - _btnLogin.Frame.Height/2) + _whiteBg.Frame.Top +5, 
					_btnLogin.Frame.Width, _btnLogin.Frame.Height); 

				_title.Frame = new RectangleF (_whiteBg.Frame.X, _whiteBg.Frame.Top - _whiteBg.Frame.Height / 2 - 20,
				                               _whiteBg.Frame.Width, _whiteBg.Frame.Height / 2);
			}


		}
	}
}


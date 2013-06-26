using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using RedBit.CCAdmin;
using RedBit;
using AlertView;
using System.Threading;
using System.Collections.Generic;

namespace BackChannel
{
	public partial class AddAlertRuleViewController : UIViewController
	{
		public AddAlertRuleViewController (IntPtr handle) : base (handle)
		{
		}

		public Action Done { get; set;}

		public Action Cancel { get; set; }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			scrollView.BackgroundColor = UIColor.White;

			this.NavigationItem.RightBarButtonItem.Clicked += (object sender, EventArgs e) => {

				if (!ValidateForm ()) {
					var rule = new Rule () {
						Email = txtEmail.Text,
						RuleName = txtRuleName.Text,
						SearchTerm = txtSearchTerm.Text,
						SendDashboard = switchDashboard.On,
						SendEmail = switchEmail.On,
						SendMobile = switchAppAlert.On,
						SendText = switchSMS.On,
						Sms = txtCellNumber.Text,
						Threashold = (int)sliderCount.Value
					};
					// save and upload
					Helper.Default.ShowHud ("updating ...");
					Api.Default.AddAlertRule (rule, (result) => {
						this.InvokeOnMainThread (() => {
							if (result.Result == "ok") {
								if (Done != null)
									Done ();
								this.DismissViewControllerAsync (true);
								Helper.Default.HideHud();
							} else {
								Helper.Default.HideHud ("Could not add, try again.", AlertView.MBAlertViewHUDType.ExclamationMark);
							}
						});
					});
				}
			};

			this.NavigationItem.LeftBarButtonItem.Clicked += (object sender, EventArgs e) => {
				if(Cancel!=null)
					Cancel();
				
				this.DismissViewControllerAsync(true);
			};

			sliderCount.ValueChanged+= (object sender, EventArgs e) => {
				lblCount.Text = ((int)sliderCount.Value).ToString();
			};

			switchEmail.ValueChanged += (object sender, EventArgs e) =>  {
				txtEmail.Enabled = switchEmail.On;
			};

			switchSMS.ValueChanged += (object sender, EventArgs e) =>  {
				txtCellNumber.Enabled = switchSMS.On;
			};

			var tap = new UITapGestureRecognizer (() => {
				this.View.EndEditing(true);
			});
			scrollView.AddGestureRecognizer (tap);

			RegisterForKeyboardNotifications();
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);

			UnregisterKeyboardNotifications ();
		}

		NSObject _keyboardObserverWillShow;
		NSObject _keyboardObserverWillHide;

		protected virtual void RegisterForKeyboardNotifications ()
		{
			_keyboardObserverWillShow = NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillShowNotification, KeyboardWillShowNotification);
			_keyboardObserverWillHide = NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillHideNotification, KeyboardWillHideNotification);
		}

		protected virtual void UnregisterKeyboardNotifications()
		{
			NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardObserverWillShow);
			NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardObserverWillHide);
		}

		/// &lt;summary&gt;
		/// Gets the UIView that represents the &quot;active&quot; user input control (e.g. textfield, or button under a text field)
		/// &lt;/summary&gt;
		/// &lt;returns&gt;
		/// A &lt;see cref=&quot;UIView&quot;/&gt;
		/// &lt;/returns&gt;
		protected virtual UIView KeyboardGetActiveView()
		{
			return this.View.FindFirstResponder();
		}

		protected virtual void KeyboardWillShowNotification (NSNotification notification)
		{
			UIView activeView = KeyboardGetActiveView();
			if (activeView == null)
				return;

			UIScrollView scrollView = activeView.FindSuperviewOfType(this.View, typeof(UIScrollView)) as UIScrollView;
			if (scrollView == null)
				return;

			RectangleF keyboardBounds = UIKeyboard.FrameBeginFromNotification(notification);

			UIEdgeInsets contentInsets = new UIEdgeInsets(0.0f, 0.0f, keyboardBounds.Size.Height, 0.0f);
			scrollView.ContentInset = contentInsets;
			scrollView.ScrollIndicatorInsets = contentInsets;

			// If activeField is hidden by keyboard, scroll it so it's visible
			RectangleF viewRectAboveKeyboard = new RectangleF(this.View.Frame.Location, new SizeF(this.View.Frame.Width, this.View.Frame.Size.Height - keyboardBounds.Size.Height));

			RectangleF activeFieldAbsoluteFrame = activeView.Superview.ConvertRectToView(activeView.Frame, this.View);
			// activeFieldAbsoluteFrame is relative to this.View so does not include any scrollView.ContentOffset

			// Check if the activeField will be partially or entirely covered by the keyboard
			if (!viewRectAboveKeyboard.Contains(activeFieldAbsoluteFrame))
			{
				// Scroll to the activeField Y position + activeField.Height + current scrollView.ContentOffset.Y - the keyboard Height
				PointF scrollPoint = new PointF(0.0f, activeFieldAbsoluteFrame.Location.Y + activeFieldAbsoluteFrame.Height + scrollView.ContentOffset.Y - viewRectAboveKeyboard.Height);
				scrollView.SetContentOffset(scrollPoint, true);
			}
		}

		protected virtual void KeyboardWillHideNotification (NSNotification notification)
		{
			UIView activeView = KeyboardGetActiveView();
			if (activeView == null)
				return;

			UIScrollView scrollView = activeView.FindSuperviewOfType (this.View, typeof(UIScrollView)) as UIScrollView;
			if (scrollView == null)
				return;

			// Reset the content inset of the scrollView and animate using the current keyboard animation duration
			double animationDuration = UIKeyboard.AnimationDurationFromNotification(notification);
			UIEdgeInsets contentInsets = new UIEdgeInsets(0.0f, 0.0f, 0.0f, 0.0f);
			UIView.Animate(animationDuration, delegate{
				scrollView.ContentInset = contentInsets;
				scrollView.ScrollIndicatorInsets = contentInsets;
			});
		}

		private bool ValidateForm(){
			// check to make sure fields are filled in
			var msg = "The following must be filled in before submitting:\r\n";
			var ret = false;

			if (string.IsNullOrEmpty (txtRuleName.Text)) {
				msg += "\tRule Name\r\n";
				ret |= true;
			}
			if (string.IsNullOrEmpty (txtSearchTerm.Text)) {
				msg += "\tSearch term\r\n";
				ret |= true;
			}
			if (switchSMS.On && string.IsNullOrEmpty (txtCellNumber.Text)) {
				msg += "\tCell number for text messages\r\n";
				ret |= true;
			}
			if (switchEmail.On && string.IsNullOrEmpty (txtEmail.Text)) {
				msg += "\tE-mail address\r\n";
				ret |= true;
			}

			if (ret) {
				var alert = MBAlertView.AlertWithBody (msg, "Ok", null);
				alert.AddToDisplayQueue ();
			}
			return ret;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

	}
}


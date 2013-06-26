using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using RedBit.CCAdmin;
using RedBit;

namespace BackChannel
{
	public partial class BackChannelViewController : UIViewController
	{
		public BackChannelViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

      
        private void SetupLogin() {

			// Create the login view
			if (_loginView == null) {
				_loginView = new LoginView ();
				_loginView.Frame = new RectangleF (0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
				_currentView = _loginView;
				this.View.AddSubview (_loginView);
			} else {
				_currentView.ExitView (() => {
					this.NavigationController.NavigationBarHidden = true;
					_backgroundImage.Frame = new RectangleF (0, 0, UIScreen.MainScreen.ApplicationFrame.Width, UIScreen.MainScreen.ApplicationFrame.Height);
					_currentView = _loginView;
					_loginView.EnterView ();
				});
			}

			if (_loginView.LoginClicked == null) {
				_loginView.LoginClicked = () => {
					LoginManager.Default.LoginAsync (this, () => {
						System.Diagnostics.Debug.WriteLine ("Login complete");
						ShowNextAfterLogin();
					}, 
					                                  () => {
						System.Diagnostics.Debug.WriteLine ("Login Cancled by user");
					});
				};

				if (LoginManager.Default.CredentialCacheAvailable)
					ShowNextAfterLogin ();
			}

		}

		private void ShowNextAfterLogin(){
			// show the twitter menu
			this.InvokeOnMainThread (() => {
				this.NavigationController.NavigationBarHidden = false;
				this._backgroundImage.Frame = new RectangleF (0, -35, UIScreen.MainScreen.ApplicationFrame.Width, UIScreen.MainScreen.ApplicationFrame.Height);
				ShowView (UIMenuBarView.Views.Tweets);
			});
		}

		UIMenuBarView _menu;
		LoginView _loginView;
		UITweetsView _tweetView;
		UIImageContentView _imagesView;
		UIBlackListView _blacklistView;
		UIView _currentView;
		UIButton _btnAdd;
		UIAlertsView _alertsView;
		UIAlertsRuleView _alertsRulesView;
		UISettingView _settingView;

		private void SetupMenu(){
			_menu = new UIMenuBarView ();
			_menu.Frame = new RectangleF (-UIScreen.MainScreen.Bounds.Width, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
			_menu.MenuItemClicked = (id) => {
				_menu.AnimateMenuOut(()=>{
					if(id == UIMenuBarView.Views.Logout){
						LoginManager.Default.Logout();
						SetupLogin();
					}
					else{
						ShowView(id);
					}
				});
			};

			this.View.AddSubview (_menu);
		}

		private void ShowView(UIMenuBarView.Views view){
			this.NavigationItem.SetRightBarButtonItem (null, true);
			// show the right view
			switch(view){
			case UIMenuBarView.Views.AlertRules:
				ShowAlertRulesList ();
				break;
			case UIMenuBarView.Views.YourAlerts:
				ShowAlertList ();
				break;
			case UIMenuBarView.Views.Blacklist:
				ShowBlackList ();
				break;
			case UIMenuBarView.Views.General:
				ShowSettings ();
				break;
			case UIMenuBarView.Views.Images:
				ShowImages ();
				break;
			case UIMenuBarView.Views.Tweets:
				ShowTweets ();
				break;
			}
		}

		private void ShowBlackList(){
			if(_blacklistView == null){
				SetupBlacklistView ();
			}

			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem(_btnAdd);

			this.NavigationItem.Title = "Blacklist";

			_currentView.ExitView (()=>{
				_blacklistView.EnterView(()=>{
					_blacklistView.LoadContent ();
					_currentView = _blacklistView;
				});
			});
		}

		private void ShowAlertList(){
			if(_alertsView == null){
				SetupAlertsView ();
			}

			this.NavigationItem.Title = "Alerts";

			_currentView.ExitView (()=>{
				_alertsView.EnterView(()=>{
					_alertsView.LoadContent ();
					_currentView = _alertsView;
				});
			});
		}

		private void ShowAlertRulesList(){
			if(_alertsRulesView == null){
				SetupAlertRulesView ();
			}

			this.NavigationItem.RightBarButtonItem = new UIBarButtonItem(_btnAdd);

			this.NavigationItem.Title = "Alert Rules";

			_currentView.ExitView (()=>{
				_alertsRulesView.EnterView(()=>{
					_alertsRulesView.LoadContent ();
					_currentView = _alertsRulesView;
				});
			});
		}

		private void ShowImages(){
			if(_imagesView == null){
				SetupImagesView ();
			}

			this.NavigationItem.Title = "Images";

			_currentView.ExitView (()=>{
				_imagesView.EnterView(()=>{
					_imagesView.LoadContent ();
					_currentView = _imagesView;
				});
			});
   		}

		private void ShowTweets(){
			if (_tweetView == null) {
				SetupTweetsView ();
			}

			this.NavigationItem.Title = "Tweets";

			_currentView.ExitView (() => {
				_tweetView.EnterView (() => {
					_tweetView.LoadTweets ();
					_currentView = _tweetView;
				});
			});
		}

		private void ShowSettings(){
			if(_settingView == null){
				SetupSettingsView ();
			}

			this.NavigationItem.Title = "Settings";

			_currentView.ExitView (()=>{
				_settingView.EnterView(()=>{
					_settingView.LoadContent ();
					_currentView = _settingView;
				});
			});
		}

		private void SetupBlacklistView(){

			_blacklistView = new UIBlackListView();
			_blacklistView.Frame = new RectangleF(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
			this.View.InsertSubview(_blacklistView, this.View.Subviews.Length-1);
		}

		private void SetupAlertsView(){
			_alertsView = new UIAlertsView();
			_alertsView.Frame = new RectangleF(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
			this.View.InsertSubview(_alertsView, this.View.Subviews.Length-1);
		}

		private void SetupAlertRulesView(){
			_alertsRulesView = new UIAlertsRuleView();
			_alertsRulesView.Frame = new RectangleF(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
			this.View.InsertSubview(_alertsRulesView, this.View.Subviews.Length-1);
		}

		private void SetupTweetsView(){
			_tweetView = new UITweetsView();
			_tweetView.Frame = new RectangleF(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
			_tweetView.TweetClicked = (tweet) =>{
				this.PerformSegue("segueViewTweet", this);
			};
			this.View.InsertSubview(_tweetView, this.View.Subviews.Length-1);
		}

		private void SetupImagesView(){
			_imagesView = new UIImageContentView ();
			_imagesView.Frame = new RectangleF(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
			this.View.InsertSubview(_imagesView, this.View.Subviews.Length-1);
		}

		private void SetupSettingsView(){
			_settingView = new UISettingView ();
			_settingView.Frame = new RectangleF(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
			_settingView.SettingClicked = (setting) => {
				this.PerformSegue("segueEditSetting", this);
			};
			this.View.InsertSubview(_settingView, this.View.Subviews.Length-1);
		}

		private void CustomizeNavigationBar(){


			UINavigationBar.Appearance.SetBackgroundImage(UIImage.FromFile("images/titleBar-Background.png"), UIBarMetrics.Default);
			UINavigationBar.Appearance.TintColor = UIColor.Clear;
			this.NavigationController.NavigationBarHidden = true;

			// create the menu button
			var btn = new UIButton (UIButtonType.Custom);
			btn.TouchUpInside += delegate(object sender, EventArgs e){
				// hide menu
				if (_menu.IsMenuVisible)
					_menu.AnimateMenuOut ();
				else
					_menu.AnimateMenuIn ();
			};
			var s = UIImage.FromFile ("images/titlebar-menu-button.png").CGImage.GetScaledSizeForControl ();
			btn.SetBackgroundImage (UIImage.FromFile("images/titlebar-menu-button.png"), UIControlState.Normal);
			btn.SetBackgroundImage (UIImage.FromFile("images/titlebar-menu-button-press.png"), UIControlState.Highlighted);
			btn.Frame = new RectangleF (0, 0, s.Width, s.Height);

			// create the add button
			_btnAdd = new UIButton (UIButtonType.Custom);
			_btnAdd.TouchUpInside += AddButtonClicked;
			s = UIImage.FromFile ("images/icons/add.png").CGImage.GetScaledSizeForControl ();
			_btnAdd.SetImage(UIImage.FromFile("images/icons/add.png"),UIControlState.Normal);
			_btnAdd.Frame = new RectangleF (0, 0, s.Width, s.Height);

			// add to the nav bar
			this.NavigationItem.SetHidesBackButton (true, false);
			this.NavigationItem.LeftBarButtonItem = new UIBarButtonItem (btn);

		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			var view = ((UINavigationController)segue.DestinationViewController).TopViewController;
			if((view as AddBlacklistItemViewController)!= null){
				var v = view as AddBlacklistItemViewController;
				v.Done = () => {
					_blacklistView.LoadContent (); 
					v.Done = null;
				};
			}
			else if((view as AddAlertRuleViewController)!= null){
				var v = view as AddAlertRuleViewController;
				v.Done = () => {
					_alertsRulesView.LoadContent (); 
					v.Done = null;
				};
			}
			else if((view as TweetViewController) != null){
				var v = view as TweetViewController;
				v.TweetItem = _tweetView.CurrentTweet;
				v.Done = () => {
					if(v.WasDeleted){
						_tweetView.RemoveTweetFromView(v.TweetItem);
					}
					v.TweetItem = null;
					v.Done = null;
				};
			}
			else if((view as SettingViewController) != null){
				var v = view as SettingViewController;
				v.SettingItem = _settingView.CurrentSettingItem;
				v.Save = () => {
					_settingView.LoadContent();
					v.SettingItem = null;
					v.Cancel = v.Save = null;
				};
				v.Cancel = () => {
					v.SettingItem = null;
					v.Cancel = v.Save = null;
				};
			}
			base.PrepareForSegue (segue, sender);
		}

		private void AddButtonClicked(object s, EventArgs e){
			if(_currentView == _blacklistView)
			{
				this.PerformSegue("segueEditBlackList",this);
			}
			else if(_currentView == _alertsRulesView){
				this.PerformSegue ("segueRule", this);
			}
		}

		#region View lifecycle
		private UIImageView _backgroundImage;
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			_backgroundImage = new UIImageView(new RectangleF(0,0,UIScreen.MainScreen.ApplicationFrame.Width, UIScreen.MainScreen.ApplicationFrame.Height));
			_backgroundImage.Image = UIImage.FromFile ("images/login/loginBg.png");

			// setup the view
			CustomizeNavigationBar ();
            SetupLogin();
			SetupMenu ();

			this.View.InsertSubview (_backgroundImage, 0);

			//this.View.BackgroundColor = UIColor.FromPatternImage (UIImage.FromFile("images/login/loginBg.png"));
			// Perform any additional setup after loading the view, typically from a nib.
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);


		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}
		#endregion


		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			// Return true for supported orientations
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}
	}
}


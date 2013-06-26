using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Collections.Generic;
using RedBit.CCAdmin;

namespace BackChannel
{
	public class UIMenuBarView : UIView
	{
		public UIMenuBarView ()
		{
		}

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

		private void SetupMenuSwipe(){
			var s = new UISwipeGestureRecognizer (SwipeHandler);
			s.Direction = UISwipeGestureRecognizerDirection.Left;
			this.AddGestureRecognizer (s);
			s = new UISwipeGestureRecognizer (SwipeHandler);
			s.Direction = UISwipeGestureRecognizerDirection.Right;
			this.AddGestureRecognizer (s);
		}

		private void SwipeHandler(UISwipeGestureRecognizer args){
			if(LoginManager.Default.IsAuthenticated){
				if(args.Direction == UISwipeGestureRecognizerDirection.Left){
					// hide menu
					if (this.IsMenuVisible)
						this.AnimateMenuOut ();
				}
				else{
					// show menu
					if (!this.IsMenuVisible)
						this.AnimateMenuIn ();
				}
			}
		}

		public void AnimateMenuIn(Action complete = null){
			this.Animate2 (.45, () => {
				// move the control to the right
				base.Frame = new RectangleF (0, 0, this.Frame.Width, this.Frame.Height);
			},
			complete);
		}
		public void AnimateMenuOut(Action complete = null){
			this.Animate2 (.45, () => {
				// move the control to the right
				base.Frame = new RectangleF (-this.Frame.Width, 0, this.Frame.Width, this.Frame.Height);
			},complete);
		}

		public Action<Views> MenuItemClicked{ get; set; }

		public bool IsMenuVisible {
			get{ return base.Frame.X == 0;}
		}

		private UITableView _tableView;
		private static int TABLEVIEW_WIDTH = (int)(UIScreen.MainScreen.Bounds.Width /2 +25);

		void Layout ()
		{
			this.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 125);

			if(_tableView == null){
				// setup the swipe handling
				SetupMenuSwipe ();

				// create the table view
				_tableView = new UITableView ();
				_tableView.Frame = new RectangleF (0, 0, TABLEVIEW_WIDTH, UIScreen.MainScreen.Bounds.Height-54);
//				_tableView.ScrollEnabled = false;
				_tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

				_tableView.BackgroundColor = UIColor.FromRGB (242, 242, 242);
				_tableView.Source = new MainMenuViewSource (new List<MenuItem>(){
					new MenuItem() {Id=Views.None, Color = UIColor.FromRGB(255,176,64), IsHeader = true, Text = "Settings"},
					new MenuItem() {Id=Views.General, IconFile = "images/menu/icons/gears.png", IsHeader = false, Text = "General"},
					new MenuItem() {Id=Views.Blacklist, IconFile = "images/menu/icons/blacklist.png", IsHeader = false, Text = "Blacklist"},
					new MenuItem() {Id=Views.None, Color = UIColor.FromRGB(92,198,242), IsHeader = true, Text = "Social Messages"},
					new MenuItem() {Id=Views.Tweets, IconFile = "images/menu/icons/tweets.png", IsHeader = false, Text = "Tweets"},
					new MenuItem() {Id=Views.Images, IconFile = "images/menu/icons/images.png", IsHeader = false, Text = "Images"},
					new MenuItem() {Id=Views.None, Color = UIColor.FromRGB(174,9,83), IsHeader = true, Text = "Alerts"},
					new MenuItem() {Id=Views.YourAlerts, IconFile = "images/menu/icons/alerts.png", Text = "Triggered Alerts"},
					new MenuItem() {Id=Views.AlertRules, IconFile = "images/menu/icons/alertRules.png", Text = "Alert Rules"},
					new MenuItem() {Id=Views.None, Color = UIColor.FromRGB(255,176,64), IsHeader = true, Text = "Account"},
					new MenuItem() {Id=Views.Logout, IconFile = "images/menu/icons/logout.png", Text = "Log Off"},
				});
				(_tableView.Source as MainMenuViewSource).RowClicked += (object sender, RedBit.RowClickedEventArgs<MenuItem> e) => {
					if(MenuItemClicked!=null)
						MenuItemClicked(e.Item.Id);
				};
				this.AddSubview (_tableView);
			}
		}

		// views available
		public enum Views{
			None,
			General,
			Blacklist ,
			Tweets,
			Images,
			YourAlerts,
			AlertRules,
			Logout
		}
	}
}


using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using AlertView;

namespace BackChannel
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		
		public override UIWindow Window {
			get;
			set;
		}
		// This method is invoked when the application is about to move from active to inactive state.
		// OpenGL applications should use this method to pause.
		public override void OnResignActivation (UIApplication application)
		{
		}
		// This method should be used to release shared resources and it should store the application state.
		// If your application supports background exection this method is called instead of WillTerminate
		// when the user quits.
		public override void DidEnterBackground (UIApplication application)
		{
		}
		/// This method is called as part of the transiton from background to active state.
		public override void WillEnterForeground (UIApplication application)
		{
		}
		/// This method is called when the application is about to terminate. Save data, if needed. 
		public override void WillTerminate (UIApplication application)
		{
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary options)
		{
			//This tells our app to go ahead and ask the user for permission to use Push Notifications
			// You have to specify which types you want to ask permission for
			// Most apps just ask for them all and if they don't use one type, who cares
			UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(UIRemoteNotificationType.Alert
			                                                                   | UIRemoteNotificationType.Badge
			                                                                   | UIRemoteNotificationType.Sound);

			// check for a notification
			if(options != null) {

				// check for a local notification
//				if(options.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey)) {
//
//					UILocalNotification localNotification = options[UIApplication.LaunchOptionsLocalNotificationKey] as UILocalNotification;
//					if(localNotification != null) {
//
//						new UIAlertView(localNotification.AlertAction, localNotification.AlertBody, null, "OK", null).Show();
//						// reset our badge
//						UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
//					}
//				}

				// check for a remote notification
				if(options.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey)) {

					NSDictionary remoteNotification = options[UIApplication.LaunchOptionsRemoteNotificationKey] as NSDictionary;
					if(remoteNotification != null) {
//						new UIAlertView(remoteNotification["AlertAction"].va, remoteNotification["AlertBody"], null, "OK", null).Show();
					}
				}
			}

			Window.MakeKeyAndVisible ();

			return true;

		}

		public override void ReceivedRemoteNotification (UIApplication application, NSDictionary userInfo)
		{
			// show an alert
			var body = userInfo.ValueForKey (new NSString("inAppMessage")).ToString ();
			var alert = MBAlertView.AlertWithBody (body, "Ok", null);
			alert.AddToDisplayQueue ();
//			new UIAlertView("Back Channel Word Alert", userInfo.ValueForKey("inAppMessage").ToString(), null, "OK", null).Show();

			// reset our badge
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}

		/// <summary>
		/// The iOS will call the APNS in the background and issue a device token to the device. when that's 
		/// accomplished, this method will be called.
		/// 
		/// Note: the device token can change, so this needs to register with your server application everytime 
		/// this method is invoked, or at a minimum, cache the last token and check for a change.
		/// </summary>
		public override void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
		{
			//The deviceToken is of interest here, this is what your push notification server needs to send out a notification
			// to the device.  So, most times you'd want to send the device Token to your servers when it has changed

			//First, get the last device token we know of
			string lastDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("deviceToken");

			//There's probably a better way to do this
			NSString strFormat = new NSString("%@");
			NSString newDeviceToken = new NSString(MonoTouch.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr(new MonoTouch.ObjCRuntime.Class("NSString").Handle, new MonoTouch.ObjCRuntime.Selector("stringWithFormat:").Handle, strFormat.Handle, deviceToken.Handle));

			//We only want to send the device token to the server if it hasn't changed since last time
			// no need to incur extra bandwidth by sending the device token every time
			if (!newDeviceToken.ToString().Equals(lastDeviceToken))
			{
				RedBit.CCAdmin.LoginManager.Default.AcquirePushChannel (newDeviceToken);

				//Save the new device token for next application launch
				NSUserDefaults.StandardUserDefaults.SetString(newDeviceToken, "deviceToken");
			}


		}

		/// <summary>
		/// Registering for push notifications can fail, for instance, if the device doesn't have network access.
		/// 
		/// In this case, this method will be called.
		/// </summary>
		public override void FailedToRegisterForRemoteNotifications (UIApplication application, NSError error)
		{
			new UIAlertView ("Error registering push notifications", error.LocalizedDescription, null, "OK", null).Show();
		}

	}
}


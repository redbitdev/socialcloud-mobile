using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using RedBit.CCAdmin;
using PushSharp.Client;

namespace CCadmin
{
    [Activity(Label = "CC Admin", MainLauncher = true)]
    public class LoginActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.LoginLayout);

			/* Check to ensure everything's set up right */
			PushClient.CheckDevice (this);
			PushClient.CheckManifest (this);

			// setup the button click for login button
            Button loginButton = FindViewById<Button>(Resource.Id.loginLoginButton);
            loginButton.Click += delegate
            {
                LoginManager.Default.LoginAsync(this, () => {
					if (LoginManager.Default.IsAuthenticated)
						ShowMainScreen ();
                    },
                    () =>
                    {
                        System.Diagnostics.Debug.WriteLine("use canceled");
                    });            
			};

			// if there is a login cache just show main screen
			if (LoginManager.Default.CredentialCacheAvailable)
				ShowMainScreen ();
        }

		private void ShowMainScreen()
		{
			// show main screen
			StartActivity(new Intent(this, typeof(MainActivity)));

			// setup push notifications
			this.SetupPushNotifications ();
		}

		private const string PUSH_CHANNEL_ID_TOKEN = "pcid";
		private string PushChannelId {
			get {

				var aquired = LoginManager.Default.Preferences.GetString (PUSH_CHANNEL_ID_TOKEN, string.Empty);
				return aquired;
			}
			set {
				var edit = LoginManager.Default.Preferences.Edit ();
				edit.PutString (PUSH_CHANNEL_ID_TOKEN, value);
				edit.Commit ();
			}
		}
	
		private void SetupPushNotifications()
		{
			PushClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);

			string registrationId = PushClient.GetRegistrationId(this);
			string prevRegId = this.PushChannelId;

			// if its not equal to the previous then register with backend.
			if (!registrationId.Equals(prevRegId))
			{
				this.PushChannelId = registrationId;
				RedBit.CCAdmin.LoginManager.Default.AcquirePushChannel (registrationId);
			}
		}
    }
}

using System.Threading;
using System.Threading.Tasks;



#if ANDROID
using Android.Content;
using Android.Preferences;
using Android.App;
using Android.Provider;
#elif IOS
using MonoTouch.UIKit;
using MonoTouch.Foundation;
#elif WINDOWS_PHONE
using Microsoft.Phone.Notification;
using System.Threading;
using System.IO.IsolatedStorage;
#endif

using Microsoft.WindowsAzure.MobileServices;

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace RedBit.CCAdmin
{
    /// <summary>
    /// Common code to login to backend services
    /// </summary>
    public class LoginManager
    {
        private static LoginManager _default;
        public static LoginManager Default
        {
            get
            {
				if (_default == null) {
					_default = new LoginManager ();
					_default.LoadStoredCredentials ();
				}
                return _default;
            }
        }

        private const string MOBILE_SERVICES_URL = "https://YOUR-MOBILE-SERVICES-SITE.azure-mobile.net/";
        private const string MOBILE_SERVICES_KEY = "YOUR-MOBILE-SERVICES-KEY";
        private MobileServiceClient _mobileService;


        private LoginManager()
        {
            this._mobileService = new MobileServiceClient(MOBILE_SERVICES_URL, MOBILE_SERVICES_KEY);
        }

        /// <summary>
        /// Performs a login using mobile services
        /// </summary>
        /// <param name="callback"></param>
#if ANDROID
        public async void LoginAsync(Context view, Action complete, Action canceled)
#elif IOS
        public async void LoginAsync(UIViewController view, Action complete, Action canceled)
#elif WINDOWS_PHONE
        public async void LoginAsync(Action complete, Action canceled)
#endif
        {

			try{
#if WINDOWS_PHONE
	            var t = await this._mobileService.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
#else
                var t = await this._mobileService.LoginAsync(view, MobileServiceAuthenticationProvider.MicrosoftAccount);
#endif
                this.AuthenticationToken = t.MobileServiceAuthenticationToken;
	            this.UserId = t.UserId;
				this.StoreCredentials();
#if WINDOWS_PHONE // need to do this because IOS does things differently for push
				AcquirePushChannel();
#endif
                GetUserData();
                
				complete();
			}
			catch(OperationCanceledException){
				canceled();
			}
			catch(Exception){
				// TODO what to do with exceptions?
			}

		}

		private const string TOKEN_USER_ID = "userId";
		private const string TOKEN_USER_TOKEN = "userToken";

		public void Logout ()
		{
#if IOS
			NSUserDefaults.StandardUserDefaults.RemoveObject(TOKEN_USER_ID);
			NSUserDefaults.StandardUserDefaults.RemoveObject(TOKEN_USER_TOKEN);
			NSUserDefaults.StandardUserDefaults.Synchronize();
#elif WINDOWS_PHONE
			IsolatedStorageSettings.ApplicationSettings.Remove(TOKEN_USER_ID);
			IsolatedStorageSettings.ApplicationSettings.Remove(TOKEN_USER_TOKEN);
            IsolatedStorageSettings.ApplicationSettings.Save();

#elif ANDROID
			var edit = Preferences.Edit();
			edit.Remove(TOKEN_USER_ID);
			edit.Remove(TOKEN_USER_TOKEN);
			edit.Commit();
#endif
			RemoteIdentity = null;
			AuthenticationToken = null;
			UserId = null;
		}

		private void StoreCredentials(){
			// save to datastore
#if IOS
			NSUserDefaults.StandardUserDefaults.SetString (this.UserId, TOKEN_USER_ID);
			NSUserDefaults.StandardUserDefaults.SetString (this.AuthenticationToken, TOKEN_USER_TOKEN);
			NSUserDefaults.StandardUserDefaults.Synchronize();
#elif WINDOWS_PHONE
			IsolatedStorageSettings.ApplicationSettings[TOKEN_USER_ID] = this.UserId;
			IsolatedStorageSettings.ApplicationSettings[TOKEN_USER_TOKEN] = this.AuthenticationToken;
            IsolatedStorageSettings.ApplicationSettings.Save();
#elif ANDROID
			var edit = Preferences.Edit();
			edit.PutString(TOKEN_USER_ID, this.UserId);
			edit.PutString(TOKEN_USER_TOKEN, this.AuthenticationToken);
			edit.Commit();
#endif

		}

		private void LoadStoredCredentials(){

#if IOS
			var userId = NSUserDefaults.StandardUserDefaults.ValueForKey (new NSString (TOKEN_USER_ID));
			var userToken = NSUserDefaults.StandardUserDefaults.ValueForKey (new NSString (TOKEN_USER_TOKEN));
#elif WINDOWS_PHONE
            var set = IsolatedStorageSettings.ApplicationSettings;
			string userId = set.Contains(TOKEN_USER_ID) ? set[TOKEN_USER_ID] as string : null;
			string userToken = set.Contains(TOKEN_USER_TOKEN) ? set[TOKEN_USER_TOKEN] as string : null;
#elif ANDROID
			var userId = Preferences.GetString(TOKEN_USER_ID, null);
			var userToken = Preferences.GetString(TOKEN_USER_TOKEN, null);
#endif


			if(userId != null){
				AuthenticationToken = userToken.ToString ();
				UserId = userId.ToString ();
				this._mobileService.CurrentUser = new MobileServiceUser (UserId) { MobileServiceAuthenticationToken = AuthenticationToken };

				// go in a background thread to try and get the user details
				ThreadPool.QueueUserWorkItem ((o) => {
					try {
						GetUserData ().Wait ();
					} catch {
						// if there is an exception the creds have expired
						// this is a HACK to rethrow because Xamarin was causing some invalid IL exceptions
						// when handling this on the UI side using async and tasks. Attributing it to because 
						// the build being used was beta build
						this.Logout ();
						throw; // just crash the app when creds expire, not the best way to handle it but when it restarts it will ask to login in again
					}
				});
			}
		}

#if ANDROID
		private ISharedPreferences _preferences = null;
		public ISharedPreferences Preferences{
			get{
				if(_preferences == null)
					_preferences = Application.Context.GetSharedPreferences("SoCloud", FileCreationMode.Private); 
				return _preferences;
			}
		}
#endif

        public Task GetUserData()
		{
			return Task.Run(async ()=>{
	           	var data = await _mobileService.GetTable<Identities>().ReadAsync();
				this.RemoteIdentity = data.FirstOrDefault ();
			});
		}


        public string AuthenticationToken { get; internal set; }
        public string UserId { get; internal set; }

        public Identities RemoteIdentity { get; internal set; }

		public bool IsAuthenticated { get { return UserId != null; } }

		public bool CredentialCacheAvailable 
		{
			get{
 #if IOS
				var userId = NSUserDefaults.StandardUserDefaults.ValueForKey (new NSString (TOKEN_USER_ID));
#elif WINDOWS_PHONE
                var set = IsolatedStorageSettings.ApplicationSettings;
				string userId = set.Contains(TOKEN_USER_ID) ? set[TOKEN_USER_ID] as string : null;
#elif ANDROID
				var userId = Preferences.GetString(TOKEN_USER_ID, null);
#endif
                return userId != null;
			}
		}

#if WINDOWS_PHONE
		private void AcquirePushChannel()
#else
		public void AcquirePushChannel(string token)
#endif
        {
            var uri = "";
			
#if WINDOWS_PHONE
            var CurrentChannel = HttpNotificationChannel.Find("MyPushChannel");


            if (CurrentChannel == null)
            {
                CurrentChannel = new HttpNotificationChannel("MyPushChannel");
                CurrentChannel.Open();
                CurrentChannel.BindToShellToast();
            }

            while (CurrentChannel.ChannelUri == null)
            {
                Thread.Sleep(100);
            }

            uri = CurrentChannel.ChannelUri.ToString();
#else
            uri = token;
#endif


            IMobileServiceTable<PushChannel> channelTable = _mobileService.GetTable<PushChannel>();
            var channel = new PushChannel
            {
                Uri = uri,

#if ANDROID
				Platform = "Android",
                DeviceId = Settings.Secure.GetString(Application.Context.ContentResolver, Settings.Secure.AndroidId)

#elif IOS
				Platform = "iOS",
				DeviceId = UIDevice.CurrentDevice.IdentifierForVendor.AsString()
#elif WINDOWS_PHONE
				Platform = "WP8",
				DeviceId = Windows.Phone.System.Analytics.HostInformation.PublisherHostId
#endif
            };
            channelTable.InsertAsync(channel);
        }

    }
}
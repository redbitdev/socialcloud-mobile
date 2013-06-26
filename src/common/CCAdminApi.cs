using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace RedBit.CCAdmin
{
    /// <summary>
    /// The API to the CC Admin services
    /// </summary>
    public class Api
    {
        private static Api _default;
        public static Api Default
        {
            get
			{
                if (_default == null)
                    _default = new Api();
                return _default;
            }
        }

        private const string DESTINATION_URL = "http://YOUR-AZURE-WEBSITE.azurewebsites.net/api/";
        private const string TWEETS_URL = "tweets?pageSize={0}&page={1}";
        private const string IMAGES_URL = "images?pageSize={0}&page={1}";
        private const string SETTING_URL = "settings";
        private const string BLACKLIST_URL = "blacklist";
        private const string ALERT_URL = "alerts";
        private const string RULES_URL = "rules";
        private const int PAGE_SIZE = 15;
        private const int PAGE = 1;

        private Api() { }



        public void GetAlertsAsync(Action<EntityResultsArgs<Alert>> callback)
        {
            DoGetRequestAsync((response) =>
            {
                if (response.Error != null)
                {
                    // something went wrong so bubble it back up
                    Console.WriteLine(response.Error.Message);
                    if (callback != null)
                        callback(new EntityResultsArgs<Alert>(response.Error));
                }
                else
                {
                    // we are good so parse
                    var j = JArray.Parse(response.Result);
                    List<Alert> items = new List<Alert>();
                    foreach (var t in j)
                    {
                        items.Add(new Alert(t));
                    }

                    callback(new EntityResultsArgs<Alert>(items));
                }
            },
           ALERT_URL);
        }

        public void GetRulesAsync(Action<EntityResultsArgs<Rule>> callback)
        {
            DoGetRequestAsync((response) =>
            {
                if (response.Error != null)
                {
                    // something went wrong so bubble it back up
                    Console.WriteLine(response.Error.Message);
                    if (callback != null)
                        callback(new EntityResultsArgs<Rule>(response.Error));
                }
                else
                {
                    // we are good so parse
                    var j = JArray.Parse(response.Result);
                    List<Rule> items = new List<Rule>();
                    foreach (var t in j)
                    {
                        items.Add(new Rule(t));
                    }

                    callback(new EntityResultsArgs<Rule>(items));
                }
            },
           RULES_URL);
        }

        /// <summary>
        /// Gets a list of tweets in the backend system
        /// </summary>
        /// <param name="callback"></param>
        public void GetTweetsAsync(Action<EntityResultsArgs<Tweet>> callback, int pageSize = PAGE_SIZE, int page = PAGE)
        {
            DoGetRequestAsync((response) =>
            {
                if (response.Error != null)
                {
                    // something went wrong so bubble it back up
                    Console.WriteLine(response.Error.Message);
                    if (callback != null)
                        callback(new EntityResultsArgs<Tweet>(response.Error));
                }
                else
                {
                    // we are good so parse
                    var j = JArray.Parse(response.Result);
                    List<Tweet> items = new List<Tweet>();
                    foreach (var t in j) 
                    {
                        items.Add(new Tweet(t));
                    }

                    callback(new EntityResultsArgs<Tweet>(items));
                }
            }, 
            string.Format(TWEETS_URL, pageSize, page));
        }

        /// <summary>
        /// Gets a list of images in the backend system
        /// </summary>
        /// <param name="callback"></param>
        public void GetImagesAsync(Action<EntityResultsArgs<Image>> callback, int pageSize = PAGE_SIZE, int page = PAGE)
        {
            DoGetRequestAsync((response) =>
            {
                if (response.Error != null)
                {
                    // something went wrong so bubble it back up
                    Console.WriteLine(response.Error.Message);
                    if (callback != null)
                        callback(new EntityResultsArgs<Image>(response.Error));
                }
                else
                {
                    // we are good so parse
                    var j = JArray.Parse(response.Result);
                    List<Image> items = new List<Image>();
                    foreach (var t in j)
                    {
                        items.Add(new Image(t));
                    }

                    callback(new EntityResultsArgs<Image>(items));
                }
            },
            string.Format(IMAGES_URL, pageSize, page));
        }

        /// <summary>
        /// Gets settings from the system
        /// </summary>
        /// <param name="callback"></param>
        public void GetSettingsAsync(Action<EntityResultsArgs<Setting>> callback)
        {
            DoGetRequestAsync((response) =>
            {
                if (response.Error != null)
                {
                    // something went wrong so bubble it back up
                    Console.WriteLine(response.Error.Message);
                    if (callback != null)
                        callback(new EntityResultsArgs<Setting>(response.Error));
                }
                else
                {
                    // we are good so parse
                    var j = JArray.Parse(response.Result);
                    List<Setting> items = new List<Setting>();
                    foreach (var t in j)
                    {
                        items.Add(new Setting(t));
                    }

                    callback(new EntityResultsArgs<Setting>(items));
                }
            },
            SETTING_URL);
        }

		public void SaveSettingAsync(Setting item, Action<EntityResultArgs<string>> callback){
			var data = string.Format ("_id={0}&key={1}&value={2}", item.Id, item.Key, item.Value);
			DoPutRequestAsync <string>((response) => {
				if (response.Error != null)
				{
					// something went wrong so bubble it back up
					Console.WriteLine(response.Error.Message);
					if (callback != null)
						callback(new EntityResultArgs<string>(response.Error));
				}
				else
				{
					// we are good so parse
					callback(new EntityResultArgs<string>("ok"));
				}
			}, SETTING_URL,data);
		}

        /// <summary>
        /// Gets blacklists from the system
        /// </summary>
        /// <param name="callback"></param>
        public void GetBlacklistAsync(Action<EntityResultsArgs<BlacklistItem>> callback)
        {
            DoGetRequestAsync((response) =>
            {
                if (response.Error != null)
                {
                    // something went wrong so bubble it back up
                    Console.WriteLine(response.Error.Message);
                    if (callback != null)
                        callback(new EntityResultsArgs<BlacklistItem>(response.Error));
                }
                else
                {
                    // we are good so parse
                    var j = JArray.Parse(response.Result);
					List<BlacklistItem> items = new List<BlacklistItem>();
                    foreach (var t in j)
                    {
						items.Add(new BlacklistItem(t));
                    }

                    callback(new EntityResultsArgs<BlacklistItem>(items));
                }
            },
            BLACKLIST_URL);
        }

		public void DeleteTweetItem(BaseContent item, Action<EntityResultArgs<string>> callback){
			DeleteContentItem(item,callback, TWEETS_URL);
		}

		public void PromoteTweetItem(BaseContent item, Action<EntityResultArgs<string>> callback){
			PromoteContentItem(item,callback, TWEETS_URL);
		}

		public void DeleteImageItem(BaseContent item, Action<EntityResultArgs<string>> callback){
			DeleteContentItem(item,callback, IMAGES_URL);
		}

		public void PromoteImageItem(BaseContent item, Action<EntityResultArgs<string>> callback){
			PromoteContentItem(item,callback, IMAGES_URL);
		}

		/// <summary>
		/// Deletes an item from the server
		/// </summary>
		/// <param name="callback">Callback.</param>
		internal void DeleteContentItem(BaseContent item, Action<EntityResultArgs<string>> callback,string url){
			DoDeleteRequestAsync <string>((response) => {
				if (response.Error != null)
				{
					// something went wrong so bubble it back up
					Console.WriteLine(response.Error.Message);
					if (callback != null)
						callback(new EntityResultArgs<string>(response.Error));
				}
				else
				{
					// we are good so parse
					callback(new EntityResultArgs<string>("ok"));
				}
			}, url,"id=" + item.Id);
		}

		internal void PromoteContentItem(BaseContent item, Action<EntityResultArgs<string>> callback, string url){
			DoPutRequestAsync <string>((response) => {
				if (response.Error != null)
				{
					// something went wrong so bubble it back up
					Console.WriteLine(response.Error.Message);
					if (callback != null)
						callback(new EntityResultArgs<string>(response.Error));
				}
				else
				{
					// we are good so parse
					callback(new EntityResultArgs<string>("ok"));
				}
			}, url,"word=" + item);
		}

		/// <summary>
		/// Deletes an item from the server
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void DeleteBlackListItem(string item, Action<EntityResultArgs<string>> callback){
			DoDeleteRequestAsync <string>((response) => {
				if (response.Error != null)
				{
					// something went wrong so bubble it back up
					Console.WriteLine(response.Error.Message);
					if (callback != null)
						callback(new EntityResultArgs<string>(response.Error));
				}
				else
				{
					// we are good so parse
					callback(new EntityResultArgs<string>("ok"));
				}
			}, BLACKLIST_URL,"word=" + item);
		}

		/// <summary>
		/// updates an item from the server
		/// </summary>
		/// <param name="callback">Callback.</param>
		public void AddBlackListItem(string item, Action<EntityResultArgs<string>> callback){
			DoPostRequestAsync <string>((response) => {
				if (response.Error != null)
				{
					// something went wrong so bubble it back up
					Console.WriteLine(response.Error.Message);
					if (callback != null)
						callback(new EntityResultArgs<string>(response.Error));
				}
				else
				{
					// we are good so parse
					callback(new EntityResultArgs<string>("ok"));
				}
			}, BLACKLIST_URL,"word=" + item);
		}

		public void DeleteAlertNotification (Alert item, Action<EntityResultArgs<string>> callback)
		{
			DoDeleteRequestAsync <string>((response) => {
				if (response.Error != null)
				{
					// something went wrong so bubble it back up
					Console.WriteLine(response.Error.Message);
					if (callback != null)
						callback(new EntityResultArgs<string>(response.Error));
				}
				else
				{
					// we are good so parse
					callback(new EntityResultArgs<string>("ok"));
				}
			}, ALERT_URL,"id=" + item.Id);
		}

		public void AddAlertRule (Rule rule, Action<EntityResultArgs<string>> callback)
		{
			var data = string.Format ("id={0}&ruleName={1}&term={2}&threashold={3}&smsToggle={4}&sms={5}&emailToggle={6}&email={7}&dashToggle={8}&mobileToggle={9}",
			                         rule.Id, rule.RuleName, rule.SearchTerm, rule.Threashold, rule.SendMobile, rule.Sms, rule.SendEmail,
			                         rule.Email, rule.SendDashboard, rule.SendMobile);

						
			DoPostRequestAsync <string>((response) => {
				if (response.Error != null)
				{
					// something went wrong so bubble it back up
					Console.WriteLine(response.Error.Message);
					if (callback != null)
						callback(new EntityResultArgs<string>(response.Error));
				}
				else
				{
					// we are good so parse
					callback(new EntityResultArgs<string>("ok"));
				}
			}, RULES_URL,data);
		}

		public void DeleteAlertRule (Rule item, Action<EntityResultArgs<string>> callback)
		{
			DoDeleteRequestAsync <string>((response) => {
				if (response.Error != null)
				{
					// something went wrong so bubble it back up
					Console.WriteLine(response.Error.Message);
					if (callback != null)
						callback(new EntityResultArgs<string>(response.Error));
				}
				else
				{
					// we are good so parse
					callback(new EntityResultArgs<string>("ok"));
				}
			}, RULES_URL,"id=" + item.Id);
		}

        private void DoGetRequestAsync(Action<EntityResultArgs<string>> callback, string api)
        {
            // make the webrequest
            var request = HttpWebRequest.Create(DESTINATION_URL + api) as HttpWebRequest;

            //request.ContentType = "application/json";

            // get the response
            request.BeginGetResponse((result) =>
            {
                HttpWebResponse resp = null;
                try
                {
                    resp = request.EndGetResponse(result) as HttpWebResponse;

                    var text = string.Empty;
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                        text = reader.ReadToEnd();

                    if (resp.StatusCode != HttpStatusCode.OK)
                    {
                        //There was an issue so respond with that
                        callback(new EntityResultArgs<string>(new ServerNotFoundException(resp.StatusCode)));
                    }
                    else
                    {
                        // send back the data 
                        callback(new EntityResultArgs<string>(text));
                    }
                }
                catch (WebException ex)
                {
                    //some web exception
                    callback(new EntityResultArgs<string>(new ServerNotFoundException(ex.Status)));
                }
                catch (Exception ex)
                {
                    //var str = ex.Message;
                    callback(new EntityResultArgs<string>(ex));
                }
                finally
                {
                    if (resp != null)
                        resp.Close();
                }
            }, null);
        }

        private async void DoPostRequestAsync<T>(Action<EntityResultArgs<string>> callback, string api, T postData, string  method = "POST")
        {
            // make the webrequest
            var request = HttpWebRequest.Create(DESTINATION_URL + api) as HttpWebRequest;

			request.ContentType = "application/x-www-form-urlencoded";
            
            // set the method
            request.Method = method;

            // serialize the data
            try
            {
                // post the data
				byte[] bytes = new byte[0];
				if(postData.GetType() == typeof(string)){
					bytes = System.Text.UTF8Encoding.UTF8.GetBytes(postData.ToString());
				} 
				else{
					var data = Newtonsoft.Json.JsonConvert.SerializeObject(postData);
					bytes = System.Text.UTF8Encoding.UTF8.GetBytes(data);
				}
                using (var postStream = await request.GetRequestStreamAsync())
                {
                    postStream.Write(bytes, 0, bytes.Length);
                    postStream.Close();
                }
            }
            catch (Exception ex)
            {
                //var str = ex.Message;
                callback(new EntityResultArgs<string>(ex));
                return;
            }


            // get the response
            request.BeginGetResponse((result) =>
            {
                HttpWebResponse resp = null;
                try
                {
                    resp = request.EndGetResponse(result) as HttpWebResponse;

                    var text = string.Empty;
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                        text = reader.ReadToEnd();

                    if (resp.StatusCode != HttpStatusCode.OK)
                    {
                        //There was an issue so respond with that
                        callback(new EntityResultArgs<string>(new ServerNotFoundException(resp.StatusCode)));
                    }
                    else
                    {
                        callback(new EntityResultArgs<string>(text));
                    }
                }
                catch (WebException ex)
                {
                    //some web exception
                    callback(new EntityResultArgs<string>(new ServerNotFoundException(ex.Status)));
                }
                catch (Exception ex)
                {
                    //var str = ex.Message;
                    callback(new EntityResultArgs<string>(ex));
                }
                finally
                {
                    if (resp != null)
                        resp.Close();
                }
            }, null);
        }

		private void DoDeleteRequestAsync<T>(Action<EntityResultArgs<string>> callback, string api, T postData)
		{
			DoPostRequestAsync<T> (callback, api, postData, "DELETE");
   		}

		private void DoPutRequestAsync<T>(Action<EntityResultArgs<string>> callback, string api, T postData)
		{
			DoPostRequestAsync<T> (callback, api, postData, "PUT");
		}
    }
}
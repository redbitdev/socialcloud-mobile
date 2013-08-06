using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;
using Microsoft.WindowsAzure.MobileServices;
using UrlImageViewHelper;

namespace CCadmin
{
	[Activity (Label = "MainActivity")]
	public class MainActivity : Activity
	{
		FlyOutContainer _dashboardFlyOutContainer;
		TextView _headerTitleTextView;
		TableLayout _contentTableLayout;
		//LinearLayout _loadingLinearLayout;

		LinearLayout _dashboardGeneralSettingsLinearLayout, _dashboardBlacklistLinearLayout, _dashboardTwitterLinearLayout,
			_dashboardImagesLinearLayout, _dashboardAlertsLinearLayout, _dashboardAlertRulesLinearLayout;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.MainLayout);

			_dashboardFlyOutContainer = FindViewById<FlyOutContainer> (Resource.Id.FlyOutContainer);
			_dashboardFlyOutContainer.SetOpened(false);

			_contentTableLayout = FindViewById<TableLayout> (Resource.Id.contentTableLayout);

			//_loadingLinearLayout = FindViewById<LinearLayout> (Resource.Id.loadingLinearLayout);

			_headerTitleTextView = FindViewById<TextView> (Resource.Id.headerTitleTextView);

			FindViewById<ImageButton> (Resource.Id.mainDashboardImageButton).Click += delegate {
				_dashboardFlyOutContainer.SetOpened(!_dashboardFlyOutContainer.Opened);
			};

			_dashboardGeneralSettingsLinearLayout = FindViewById<LinearLayout> (Resource.Id.dashboardGeneralSettingsLinearLayout);
			_dashboardBlacklistLinearLayout = FindViewById<LinearLayout> (Resource.Id.dashboardBlacklistLinearLayout);
			_dashboardTwitterLinearLayout = FindViewById<LinearLayout> (Resource.Id.dashboardTwitterLinearLayout);
			_dashboardImagesLinearLayout = FindViewById<LinearLayout> (Resource.Id.dashboardImagesLinearLayout);
			_dashboardAlertsLinearLayout = FindViewById<LinearLayout> (Resource.Id.dashboardAlertsLinearLayout);
			_dashboardAlertRulesLinearLayout = FindViewById<LinearLayout> (Resource.Id.dashboardAlertRulesLinearLayout);

			_dashboardGeneralSettingsLinearLayout.Click += delegate {
				_headerTitleTextView.Text = "General Settings";
				ChangeLayout(_dashboardGeneralSettingsLinearLayout);
				PopulateSettingsTable();
			};

			_dashboardBlacklistLinearLayout.Click += delegate {
				_headerTitleTextView.Text = "Blacklist";
				ChangeLayout(_dashboardBlacklistLinearLayout);
				PopulateBlacklistTable();
			};

			_dashboardTwitterLinearLayout.Click += delegate {
				_headerTitleTextView.Text = "Tweets";
				ChangeLayout(_dashboardTwitterLinearLayout);
				PopulateTwitterTable();
			};

			_dashboardImagesLinearLayout.Click += delegate {
				_headerTitleTextView.Text = "Images";
				ChangeLayout(_dashboardImagesLinearLayout);
				PopulateImagesTable();
			};

			_dashboardAlertsLinearLayout.Click += delegate {
				_headerTitleTextView.Text = "Alerts";
				ChangeLayout(_dashboardAlertsLinearLayout);
				PopulateAlertsTable();
			};

			_dashboardAlertRulesLinearLayout.Click += delegate {
				_headerTitleTextView.Text = "Alert Rules";
				ChangeLayout(_dashboardAlertRulesLinearLayout);
				PopulateRulesTable();
			};

			PopulateTwitterTable();
		}

		void ChangeLayout(LinearLayout newLayout)
		{
			this.RunOnUiThread(() =>
			{
				_dashboardGeneralSettingsLinearLayout.SetBackgroundColor (Color.Rgb (240, 240, 240));
				_dashboardBlacklistLinearLayout.SetBackgroundColor (Color.Rgb (240, 240, 240));
				_dashboardTwitterLinearLayout.SetBackgroundColor (Color.Rgb (240, 240, 240));
				_dashboardImagesLinearLayout.SetBackgroundColor (Color.Rgb (240, 240, 240));
				_dashboardAlertsLinearLayout.SetBackgroundColor (Color.Rgb (240, 240, 240));
				_dashboardAlertRulesLinearLayout.SetBackgroundColor (Color.Rgb (240, 240, 240));

				newLayout.SetBackgroundColor (Color.Rgb (208, 208, 208));

				_contentTableLayout.RemoveAllViewsInLayout();
				_dashboardFlyOutContainer.SetOpened(false);
			});
		}

		void PopulateImagesTable()
		{
			/* Pull Images and Populate the Content Table Layout */
			RedBit.CCAdmin.Api.Default.GetImagesAsync((results) =>
			{
				if (results.Error != null)
					Console.WriteLine(results.Error.Message);
				else
				{
					foreach (var t in results.Results)
					{
						this.RunOnUiThread(() =>
						{
							TableRow row = (TableRow)LayoutInflater.From(this).Inflate(Resource.Layout.ImagesTableRow, null, true);

							row.FindViewById<ImageView> (Resource.Id.rowAuthorImageView).SetUrlDrawable(t.AuthorProfileUrl);
							row.FindViewById<ImageView> (Resource.Id.rowImageImageView).SetUrlDrawable(t.Content);
							row.FindViewById<TextView> (Resource.Id.rowAuthorTextView).Text = "@" + t.AuthorName;
							row.FindViewById<TextView> (Resource.Id.rowDateTimeTextView).Text = string.Format("{0:MM/dd/yyyy h:mmt}M", t.DateTime);

							_contentTableLayout.AddView (row);
						});
					}
				}
			});
		}

		void PopulateRulesTable()
		{
			/* Pull Alert Rules and Populate the Content Table Layout */
			RedBit.CCAdmin.Api.Default.GetRulesAsync((results) =>
			                                          {
				if (results.Error != null)
					Console.WriteLine(results.Error.Message);
				else
				{
					foreach (var t in results.Results)
					{
						this.RunOnUiThread(() =>
						{
							TableRow row = (TableRow)LayoutInflater.From(this).Inflate(Resource.Layout.RulesTableRow, null, true);

							row.FindViewById<TextView> (Resource.Id.rowRuleTextView).Text = t.RuleName;
							row.FindViewById<TextView> (Resource.Id.rowTermTextView).Text = t.SearchTerm;
							row.FindViewById<TextView> (Resource.Id.rowMobileTextView).Text = "Send Mobile: " + t.SendMobile.ToString();
							row.FindViewById<TextView> (Resource.Id.rowDashTextView).Text = "Send Dash: " + t.SendDashboard.ToString();
							row.FindViewById<TextView> (Resource.Id.rowEmailTextView).Text = "Send Email: " + t.SendEmail.ToString();
							row.FindViewById<TextView> (Resource.Id.rowTextTextView).Text = "Send Text: " + t.SendText.ToString();

							_contentTableLayout.AddView (row);
						});
					}
				}
			});
		}

		void PopulateAlertsTable()
		{
			/* Pull Alerts and Populate the Content Table Layout */
			RedBit.CCAdmin.Api.Default.GetAlertsAsync((results) =>
			{
				if (results.Error != null)
					Console.WriteLine(results.Error.Message);
				else
				{
					foreach (var t in results.Results)
					{
						this.RunOnUiThread(() =>
						{
							TableRow row = (TableRow)LayoutInflater.From(this).Inflate(Resource.Layout.AlertsTableRow, null, true);

							row.FindViewById<TextView> (Resource.Id.rowAlertTextView).Text = t.RuleName;
							row.FindViewById<TextView> (Resource.Id.rowCountTextView).Text = "Total Count: " + t.Count.ToString();
							row.FindViewById<TextView> (Resource.Id.rowDateTimeTextView).Text = string.Format("{0:MM/dd/yyyy h:mmt}M", t.Dt);

							_contentTableLayout.AddView (row);
						});
					}
				}
			});
		}

		void PopulateSettingsTable()
		{
			/* Pull General Settings and Populate the Content Table Layout */
			RedBit.CCAdmin.Api.Default.GetSettingsAsync((results) =>
			{
				if (results.Error != null)
					Console.WriteLine(results.Error.Message);
				else
				{
					foreach (var t in results.Results)
					{
						this.RunOnUiThread(() =>
						{
							TableRow row = (TableRow)LayoutInflater.From(this).Inflate(Resource.Layout.GeneralTableRow, null, true);

							row.FindViewById<TextView> (Resource.Id.rowSettingTextView).Text = t.Key;
							row.FindViewById<TextView> (Resource.Id.rowValueTextView).Text = t.Value;

							_contentTableLayout.AddView (row);
						});
					}
				}
			});
		}

		void PopulateBlacklistTable()
		{
			/* Pull Blacklist and Populate the Content Table Layout */
			RedBit.CCAdmin.Api.Default.GetBlacklistAsync((results) =>
			{
				if (results.Error != null)
					Console.WriteLine(results.Error.Message);
				else
				{
					foreach (var t in results.Results)
					{
						this.RunOnUiThread(() =>
						{
							TableRow row = (TableRow)LayoutInflater.From(this).Inflate(Resource.Layout.BlacklistTableRow, null, true);

							row.FindViewById<TextView> (Resource.Id.rowWordTextView).Text = t.Value;

							_contentTableLayout.AddView (row);
						});
					}
				}
			});
		}

		void PopulateTwitterTable()
		{
			/* Pull Tweets and Populate the Content Table Layout */
			RedBit.CCAdmin.Api.Default.GetTweetsAsync((results) =>
			{
				if (results.Error != null)
					Console.WriteLine(results.Error.Message);
				else
				{
					foreach (var t in results.Results)
					{
						this.RunOnUiThread(() =>
						{
							TableRow row = (TableRow)LayoutInflater.From(this).Inflate(Resource.Layout.TweetsTableRow, null, true);

							row.FindViewById<ImageView> (Resource.Id.rowAuthorImageView).SetUrlDrawable(t.AuthorProfileUrl);
							row.FindViewById<TextView> (Resource.Id.rowTweetTextView).Text = t.Content;
							row.FindViewById<TextView> (Resource.Id.rowAuthorTextView).Text = "@" + t.AuthorName;
							row.FindViewById<TextView> (Resource.Id.rowDateTimeTextView).Text = string.Format("{0:MM/dd/yyyy h:mmt}M", t.DateTime);
							_contentTableLayout.AddView (row);
						});
					}
				}
			});
		}
	}
}
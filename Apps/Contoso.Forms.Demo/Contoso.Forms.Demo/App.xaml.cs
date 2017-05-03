﻿using Xamarin.Forms;

using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using Microsoft.Azure.Mobile.Distribute;
using System.Threading.Tasks;

namespace Contoso.Forms.Demo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainDemoPage());
            MobileCenter.LogLevel = LogLevel.Verbose;
            Distribute.ReleaseAvailable = OnReleaseAvailable;
            MobileCenter.Start("uwp=5bce20c8-f00b-49ca-8580-7a49d5705d4c;android=987b5941-4fac-4968-933e-98a7ff29237c;ios=fe2bf05d-f4f9-48a6-83d9-ea8033fbb644",
                               typeof(Analytics), typeof(Crashes), typeof(Distribute));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        bool OnReleaseAvailable(ReleaseDetails releaseDetails)
        {
            MobileCenterLog.Info("MobileCenterDemo", "OnReleaseAvailable id=" + releaseDetails.Id
                                            + " version=" + releaseDetails.Version
                                            + " releaseNotesUrl=" + releaseDetails.ReleaseNotesUrl);
            var custom = releaseDetails.ReleaseNotes?.ToLowerInvariant().Contains("custom") ?? false;
            if (custom)
            {
                var title = "Version " + releaseDetails.ShortVersion + " available!";
                Task answer;
                if (releaseDetails.MandatoryUpdate)
                {
                    answer = Current.MainPage.DisplayAlert(title, releaseDetails.ReleaseNotes, "Update now!");
                }
                else
                {
                    answer = Current.MainPage.DisplayAlert(title, releaseDetails.ReleaseNotes, "Update now!", "Maybe tomorrow...");
                }
                answer.ContinueWith((task) =>
                {
                    if (releaseDetails.MandatoryUpdate || (task as Task<bool>).Result)
                    {
                        Distribute.NotifyUpdateAction(UpdateAction.Update);
                    }
                    else
                    {
                        Distribute.NotifyUpdateAction(UpdateAction.Postpone);
                    }
                });
            }
            return custom;
        }
    }
}

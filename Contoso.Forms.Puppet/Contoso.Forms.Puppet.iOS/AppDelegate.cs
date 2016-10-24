﻿using Foundation;
using Microsoft.Sonoma.Core;
using UIKit;

namespace Contoso.Forms.Puppet.iOS
{
	[Register("AppDelegate")]
	public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
		{
			Xamarin.Forms.Forms.Init();
			Sonoma.Initialize("44cd8722-bfe0-4748-ac14-7692e031a8a5");
			LoadApplication(new App());

			return base.FinishedLaunching(uiApplication, launchOptions);
		}
	}
}

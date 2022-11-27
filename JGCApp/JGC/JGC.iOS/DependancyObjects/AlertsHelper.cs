using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs.Infrastructure;
using Foundation;
using JGC.Common.Interfaces;
using UIKit;

namespace JGC.iOS.DependancyObjects
{
    public class AlertsHelper : IAlertsHelper
    {
        public async Task CloseAll()
        {
            while (UIApplication.SharedApplication.GetTopViewController() is UIAlertController alertVC)
            {
                await alertVC.DismissViewControllerAsync(false);
                alertVC.Dispose();
                alertVC = null;
            }
        }
    }
}
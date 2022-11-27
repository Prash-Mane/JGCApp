using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using JGC.Common.Interfaces;
using JGC.Droid.DependancyObjects;
using Xamarin.Forms;
using static Android.Provider.Settings;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceID))]
namespace JGC.Droid.DependancyObjects
{
    public class DeviceID : Java.Lang.Object, IDeviceID
    {


      
        public async Task<string> GetDeviceID()
        {
            string id = string.Empty;

            if (!string.IsNullOrWhiteSpace(id))
                return id;

            id = Android.OS.Build.Serial;
            if (string.IsNullOrWhiteSpace(id) || id == Build.Unknown || id == "0")
            {
                try
                {
                    var context = Android.App.Application.Context;
                    id = Secure.GetString(context.ContentResolver, Secure.AndroidId);
                }
                catch (Exception ex)
                {
                    Android.Util.Log.Warn("DeviceInfo", "Unable to get id: " + ex.ToString());
                }
            }

            return id;
           
        }

       
    }
}
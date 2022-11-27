using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using JGC.Common.Interfaces;
using JGC.Droid.DependancyObjects;

[assembly: Xamarin.Forms.Dependency(typeof(ToastMessage))]
namespace JGC.Droid.DependancyObjects
{
    public class ToastMessage : IToastMessage
    {
        public void Show(string message)
        {
            Toast toast = Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short);
            View view = toast.View;
            view.SetBackgroundColor(Color.Black);
            TextView text = toast.View.FindViewById<TextView>(Android.Resource.Id.Message);
            text.SetTextColor(Color.White);

            toast.Show();
            //Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }
    }
}
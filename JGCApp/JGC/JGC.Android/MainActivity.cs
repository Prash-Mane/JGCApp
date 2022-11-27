using System;
using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Permissions;
using JGC.Common.Helpers;
using Xamarin.Forms;
using JGC.Common.Interfaces;
using JGC.ViewModels.E_Test_Package;
using ZXing.Mobile;
using JGC.Views;

namespace JGC.Droid
{
    //[Activity(Label = "JGC", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [Activity(Label = "JGC Connect", Icon = "@mipmap/ic_launcher", RoundIcon = "@mipmap/ic_launcher_round", Theme = "@style/Theme.Splash",  MainLauncher = true,
        LaunchMode = LaunchMode.SingleInstance, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.SetTheme(Resource.Style.MainTheme);
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;
                base.OnCreate(bundle);
                Rg.Plugins.Popup.Popup.Init(this, bundle);
                global::Xamarin.Forms.Forms.Init(this, bundle);
                UserDialogs.Init(this);
                Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
                MobileBarcodeScanner.Initialize(Application);
                LoadApplication(new App());

                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    int RequestLocationId = 0;
                        string[] PermissionsLocation = {
                        Manifest.Permission.AccessNetworkState,
                        Manifest.Permission.Camera,
                        Manifest.Permission.Internet,
                        Manifest.Permission.ReadExternalStorage,
                        Manifest.Permission.WriteExternalStorage,
                    };
                    RequestPermissions(PermissionsLocation, RequestLocationId);
                }

                //DependencyService.Register<IGetPunchData, PunchViewModel>();

                //allowing the device to change the screen orientation based on the rotation
                MessagingCenter.Subscribe<ModulesPage>(this, "allowLandScapePortrait", sender =>
                {
                    RequestedOrientation = ScreenOrientation.Unspecified;
                });
                //during page close setting back to portrait
                MessagingCenter.Subscribe<ModulesPage>(this, "preventPortrait", sender =>
                {
                    RequestedOrientation = ScreenOrientation.SensorLandscape;
                });
            }
            catch (Exception ex)
            {

            }
        }

        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            try
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                if (grantResults[0] == 0)
                {
                    // code
                }
            }
            catch (Exception ex)
            {

            }

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnRestart()
        {
            base.OnRestart();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnStop()
        {
            Settings.IsStop = 1;
            base.OnStop();
        }
    }
}
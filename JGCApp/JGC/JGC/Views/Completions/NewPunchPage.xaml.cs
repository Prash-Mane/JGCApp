using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.Completions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPunchPage : ContentPage
    {
        private byte[] imageAsByte;
        public NewPunchPage()
        {
            InitializeComponent();
        }
        private async Task<bool> CheckPermissions()
        {
            try
            {
                var cameraStatus = await Plugin.Permissions.CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);
                var storageStatus = await Plugin.Permissions.CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage);
                if (cameraStatus != Plugin.Permissions.Abstractions.PermissionStatus.Granted || storageStatus != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        var results = await Plugin.Permissions.CrossPermissions.Current.RequestPermissionsAsync(new[] { Plugin.Permissions.Abstractions.Permission.Camera, Plugin.Permissions.Abstractions.Permission.Storage });
                        storageStatus = results[Plugin.Permissions.Abstractions.Permission.Storage];
                        cameraStatus = results[Plugin.Permissions.Abstractions.Permission.Camera];
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var results = await Plugin.Permissions.CrossPermissions.Current.RequestPermissionsAsync(new[] { Plugin.Permissions.Abstractions.Permission.Camera, Plugin.Permissions.Abstractions.Permission.Storage });
                            storageStatus = results[Plugin.Permissions.Abstractions.Permission.Storage];
                            cameraStatus = results[Plugin.Permissions.Abstractions.Permission.Camera];
                        });
                    }
                }
                return cameraStatus != Plugin.Permissions.Abstractions.PermissionStatus.Granted && storageStatus != Plugin.Permissions.Abstractions.PermissionStatus.Granted;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<MediaFile> TakekPhotoAsync()
        {
            try
            {
                var denied = await CheckPermissions();
                if (denied)
                {
                    await Application.Current.MainPage.DisplayAlert("Unable to pick a file.", "Permissions Denied. Please modify app permisions in settings.", "OK");
                    return null;
                }

                if (!Plugin.Media.CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("No Camera", "Please Connect Camera.", "OK");
                    return null;
                }
                else
                {
                    try
                    {
                        var file = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });
                        return file == null ? null : file;
                    }
                    catch (Exception)
                    {
                        await Application.Current.MainPage.DisplayAlert("No Camera", "Please Connect Camera.", "OK");
                        return null;
                    }
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        private async void AddFileFromGallry(object sender, EventArgs e)
        {
            try
            {
                var viewModel = (ViewModels.Completions.NewPunchViewModel)BindingContext;
                //if (viewModel.SelectedPunchList == null)
                //{
                //    await Application.Current.MainPage.DisplayAlert("", "Can't add image, no punch number is generated for the punch. Please sync with server and try again", "OK");
                //    return;
                //}
                //if (string.IsNullOrEmpty(viewModel.SelectedPunchList.uniqueno))
                //{
                //    await Application.Current.MainPage.DisplayAlert("", "Can't add image, no punch number is generated for the punch. Please sync with server and try again", "OK");
                //    return;
                //}
                var mediaFile = await TakekPhotoAsync();
                if (mediaFile == null)
                {
                    return;
                }
                var memoryStream = new System.IO.MemoryStream();
                await mediaFile.GetStream().CopyToAsync(memoryStream);
                imageAsByte = memoryStream.ToArray();
                System.IO.Stream stream = new System.IO.MemoryStream(imageAsByte);

                viewModel.CapturedImage = ImageSource.FromStream(() => stream);
                viewModel.CaptureImageSave(imageAsByte);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
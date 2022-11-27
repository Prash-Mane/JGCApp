using Plugin.Media.Abstractions;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.Completions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateNewPunchPage : ContentPage
    {
        private byte[] imageAsByte;
        public CreateNewPunchPage()
        {
            InitializeComponent();
        }
        private async Task<bool> CheckPermissions()
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
        public async Task<MediaFile> TakekPhotoAsync()
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
        private async void AddFileFromGallry(object sender, EventArgs e)
        {
            var mediaFile = await TakekPhotoAsync();
            if (mediaFile == null)
            {
                return;
            }
            var memoryStream = new System.IO.MemoryStream();
            await mediaFile.GetStream().CopyToAsync(memoryStream);
            imageAsByte = memoryStream.ToArray();
            System.IO.Stream stream = new System.IO.MemoryStream(imageAsByte);
            var viewModel = (ViewModels.Completions.CreateNewPunchViewModel)BindingContext;
            viewModel.CapturedImage = ImageSource.FromStream(() => stream);
            viewModel.CaptureImageSave(imageAsByte);
        }
    }
}
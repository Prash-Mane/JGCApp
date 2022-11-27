using JGC.Common.Enumerators;
using JGC.Common.Extentions;
using JGC.DataBase.DataTables;
using JGC.Models.E_Test_Package;
using JGC.UserControls.CustomControls;
using JGC.ViewModels.E_Test_Package;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;
using JGC.UserControls.Touch;
using JGC.Common.Helpers;
using static JGC.Common.Enumerators.Enumerators;
using JGC.UserControls.Zoom;

namespace JGC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestRecordPage : ContentPage
    {
        Dictionary<long, SKPath> _inProgressPaths = new Dictionary<long, SKPath>();
        List<SKPath> _completedPaths = new List<SKPath>();
        SKCanvas _saveBitmapCanvas;
        private string _filePath;
        public IMedia _media;
        private string DrawColor;
        List<long> touchIds = new List<long>();
        public SpoolDrawingModel SelectedDrawing;
        T_TestLimitDrawing PDFDrawing;
        private ImageSource PDFImage;
        private byte[] imageAsByte;

        public TestRecordPage()
        {
            CurrentPageHelper.ColorPicker = "#000000";
            InitializeComponent();
            EnableDisableDrawing.Text = "Disable Drawing";
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var Control = sender.GetType().Name;
            var Vm = (TestRecordViewModel)BindingContext;

            if (Control == "Button")
            {
                RecordConfirmation SelectedRecord = (RecordConfirmation)((Button)sender).CommandParameter;
                Vm.gvTestRecordConfirmation_CellContentClick("btnNA", SelectedRecord);
                Vm.SelectedConfirmationSource = SelectedRecord;
            }
            else if (Control == "Image")
            {
                RecordConfirmation SelectedRecord = (RecordConfirmation)((TappedEventArgs)e).Parameter;
                Vm.gvTestRecordConfirmation_CellContentClick("Image", SelectedRecord);
                Vm.SelectedConfirmationSource = SelectedRecord;
            }
        }
        SKPoint ConvertToPixel(SKPoint pt)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                pt = new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                               (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
            }
            return ResizeCameraPoints(pt);
        }
        SKPoint ResizeCameraPoints(SKPoint pt)
        {
            var Vm = (TestRecordViewModel)BindingContext;
            return new SKPoint((float)((pt.X + (Vm.bitmap.Matrix.TransX * -1)) / Vm.bitmap.Matrix.ScaleX),
                               (float)((pt.Y + (Vm.bitmap.Matrix.TransY * -1))) / Vm.bitmap.Matrix.ScaleY);
        }

        private void TapOnFingerDraw(object sender, EventArgs e)
        {
            var vm = (TestRecordViewModel)BindingContext;
            vm._drawFinger = true;
            UpdateBitmap();
        }

        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Violet,
            StrokeWidth = 5,
        };

        async void UpdateBitmap()
        {
            var Vm = (TestRecordViewModel)BindingContext;
            if (Vm != null)
            {
                paint.Color = SKColor.Parse(CurrentPageHelper.ColorPicker != null ? CurrentPageHelper.ColorPicker : "#000000");

                using (_saveBitmapCanvas = new SKCanvas(Vm.Bitmap))
                {
                    if (Vm._drawFinger)
                    {
                        foreach (SKPath path in _inProgressPaths.Values)
                        {
                            _saveBitmapCanvas.DrawPath(path, paint);
                        }
                    }
                }
                canvasView.InvalidateSurface();
            }
           
        }

        private async void Cameraclick(object sender, EventArgs e)
        {
            if (((JGC.UserControls.CustomControls.CustomPicker)sender).SelectedItem == null) return;
            var Vm = (TestRecordViewModel)BindingContext;
            UserDialogs.Instance.ShowLoading("Loading..!");
            if (_saveBitmapCanvas != null)
            {
                _completedPaths.Clear();
                _inProgressPaths.Clear();
                UpdateBitmap();
                canvasView.InvalidateSurface();
            }
            var mediaFile = await TakePhotoAsync("CapturedImage");
            if (mediaFile == null)
            {
                UserDialogs.Instance.HideLoading();
                return;
            }

            ImageFiles.SelectedItem = null;
            _filePath = mediaFile.Path;
            using (Stream stream = mediaFile.GetStream())
            {
                Vm.Bitmap = SKBitmap.Decode(stream);
                Vm.bitmap = new TouchManipulationBitmap(Vm.Bitmap);
                Vm.bitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                UpdateBitmap();
                EnableDisableDrawing.Text = "Disable Drawing";
                Vm.btnSaveDelete = "Save";
                Vm.ColorPickerbtnVisible = Vm.Showbuttons = Vm.IsVisibleSaveDelete = true;
                Vm.RenameImage = false;
            }
            IsVisible = true;
            UserDialogs.Instance.HideLoading();
        }

        private async void AddFileFromGallry(object sender, EventArgs e)
        {
            var Vm = (TestRecordViewModel)BindingContext;
            UserDialogs.Instance.ShowLoading("Loading..!");
            if (_saveBitmapCanvas != null)
            {
                _completedPaths.Clear();
                _inProgressPaths.Clear();
                UpdateBitmap();
                canvasView.InvalidateSurface();
            }
            var mediaFile = await PickPhoto();
            if (mediaFile == null)
            {
                UserDialogs.Instance.HideLoading();
                return;
            }
            _filePath = mediaFile.Path;
            using (Stream stream = mediaFile.GetStream())
            {
                Vm.Bitmap = SKBitmap.Decode(stream);
                Vm.ColorPickerbtnVisible = false;
                Vm.CameraSaveBT_Click(Path.GetFileName(_filePath));
                Vm.btnSaveDelete = "Delete";
                Vm.IsVisibleSaveDelete = Vm.RenameImage = true;

                
                //Vm.SelectedImageFiles = null;
                //Vm.SelectedImageFiles = Vm.ImageFiles.LastOrDefault();
                //  ImageFiles.SelectedItem = ImageFiles.ItemsSource[ImageFiles.ItemsSource.Count-1];
            }
            IsVisible = true;
            Vm.IsSaveVisible = false;
            UserDialogs.Instance.HideLoading();
        }

        public async Task<MediaFile> PickPhoto()
        {
            var Vm = (TestRecordViewModel)BindingContext;
            var denied = await CheckPermissions();
            if (denied)
            {
                await Application.Current.MainPage.DisplayAlert("Unable to take photos.", "Permissions Denied. Please modify app permisions in settings.", "OK");
                return null;
            }
            MediaFile file = null;
            try
            {
                file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });

            }
            catch (Exception ex)
            {

            }
            Vm._drawFinger = false;
            Vm.IsSaveVisible = false;
            return file == null ? null : file;
        }

        public async Task<MediaFile> TakePhotoAsync(string pageName)
        {
            var denied = await CheckPermissions();
            if (denied)
            {
                await Application.Current.MainPage.DisplayAlert("Unable to take photos.", "Permissions Denied. Please modify app permisions in settings.", "OK");

                return null;
            }

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", "No camera available!", "OK");
                return null;
            }

            MediaFile file = null;
            try
            {
                if (pageName == "CapturedImage")
                {
                    file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Name = pageName,
                        Directory = "JGC",

                    });
                }
                else
                {
                    file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Name = pageName,
                    });
                }
            }
            catch (Exception ex)
            {

            }

            return file == null ? null : file;
        }
        private ImageSource _capturedImage;

        public ImageSource CapturedImage;
        private string _categoryBackgroundColor;

        private async Task<bool> CheckPermissions()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                    storageStatus = results[Permission.Storage];
                    cameraStatus = results[Permission.Camera];
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                        storageStatus = results[Permission.Storage];
                        cameraStatus = results[Permission.Camera];
                    });
                }
            }
            return cameraStatus != PermissionStatus.Granted && storageStatus != PermissionStatus.Granted;
        }

        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            var Vm = (TestRecordViewModel)BindingContext;
            //// Convert Xamarin.Forms point to pixels
            Point pt = args.Location;
            SKPoint Camerapoint = new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                                              (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));

            if (Vm.bitmap == null)
                return;

            if (Vm._drawFinger)
            {
                switch (args.Type)
                {
                    case TouchActionType.Pressed:
                        if (!_inProgressPaths.ContainsKey(args.Id))
                        {
                            SKPath path = new SKPath();
                            path.MoveTo(ConvertToPixel(Camerapoint));
                            _inProgressPaths.Add(args.Id, path);
                        }
                        break;

                    case TouchActionType.Moved:
                        if (Vm._drawFinger)
                        {
                            if (_inProgressPaths.ContainsKey(args.Id))
                            {
                                SKPath path = _inProgressPaths[args.Id];
                                path.LineTo(ConvertToPixel(Camerapoint));
                                UpdateBitmap();
                            }
                        }
                        break;

                    case TouchActionType.Released:
                        if (_inProgressPaths.ContainsKey(args.Id))
                        {
                            _completedPaths.Add(_inProgressPaths[args.Id]);
                            _inProgressPaths.Remove(args.Id);
                            UpdateBitmap();
                        }
                        break;
                    case TouchActionType.Cancelled:
                        if (Vm._drawFinger)
                        {
                            if (_inProgressPaths.ContainsKey(args.Id))
                            {
                                _inProgressPaths.Remove(args.Id);
                                UpdateBitmap();
                            }
                        }
                        break;
                }
            }
            else
            {
                switch (args.Type)
                {
                    case TouchActionType.Pressed:
                        if (Vm.bitmap.HitTest(Camerapoint))
                        {
                            touchIds.Add(args.Id);
                            Vm.bitmap.ProcessTouchEvent(args.Id, args.Type, Camerapoint);
                            break;
                        }
                        break;

                    case TouchActionType.Moved:
                        if (touchIds.Contains(args.Id))
                        {
                            Vm.bitmap.ProcessTouchEvent(args.Id, args.Type, Camerapoint);
                            canvasView.InvalidateSurface();
                        }
                        break;

                    case TouchActionType.Released:
                    case TouchActionType.Cancelled:
                        if (touchIds.Contains(args.Id))
                        {
                            Vm.bitmap.ProcessTouchEvent(args.Id, args.Type, Camerapoint);
                            touchIds.Remove(args.Id);
                            canvasView.InvalidateSurface();
                        }
                        break;
                }
            }
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var Vm = (TestRecordViewModel)BindingContext;
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            if (Vm.bitmap != null)
            {
                Vm.bitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                // Display the bitmap
                Vm.bitmap.Paint(canvas);
            }
        }

        private void Image_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Vm = (TestRecordViewModel)BindingContext;
            if (Vm != null)
            {
                Vm._drawFinger = false;
                var selectedItem = sender as Picker;
                var obj = (TestPackageImage)selectedItem.SelectedItem;
                if (obj != null)
                {
                    byte[] imageBytes = Convert.FromBase64String(obj.FileBytes);

                    UserDialogs.Instance.ShowLoading("Loading..!");
                    if (_saveBitmapCanvas != null)
                    {
                        _completedPaths.Clear();
                        _inProgressPaths.Clear();
                        UpdateBitmap();
                        canvasView.InvalidateSurface();
                    }

                    Vm.Bitmap = SKBitmap.Decode(imageBytes);
                    Vm.bitmap = new TouchManipulationBitmap(Vm.Bitmap);
                    Vm.bitmap.TouchManager.Mode = TouchManipulationMode.PanOnly;
                    UpdateBitmap();

                    //bind data with Viewmodel    
                    Vm.btnSaveDelete = "Delete";
                    Vm.IsSaveVisible = false;
                    Vm.IsVisibleSaveDelete = true;
                   // Vm.SelectedImageFiles = obj;
                    Vm.RenameImage = IsVisible = Vm.Showbuttons = true;
                    Vm.ColorPickerbtnVisible = false;
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    Vm.Bitmap = new SKBitmap();
                    Vm.bitmap = new TouchManipulationBitmap(Vm.Bitmap);
                    Vm.bitmap.TouchManager.Mode = TouchManipulationMode.PanOnly;
                    UpdateBitmap();
                    Vm.RenameImage = Vm.ColorPickerbtnVisible = false;
                }
            }
        }

        //new methods added by prashant
        private void ClickedOn_ClearImage(object sender, EventArgs e)
        {
            clearBitmap();
        }

        private void OnCameraIcon_Clicked(object sender, EventArgs e)
        {
            var Vm = (TestRecordViewModel)BindingContext;
            clearBitmap();
            Vm.GetTestRecordImages(false);
            Vm.MainGrid = false;
            Vm.CameraGrid = true;
        }

        private void clearBitmap()
        {
            var Vm = (TestRecordViewModel)BindingContext;
            ImageFiles.SelectedItem = null;
            Vm.Bitmap = new SKBitmap();
            _saveBitmapCanvas = new SKCanvas(Vm.Bitmap);
            Vm.bitmap = new TouchManipulationBitmap(Vm.Bitmap);
            Vm.bitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
            UpdateBitmap();
        }

        private void Clicked_EnableDisableDrawCamera(object sender, EventArgs e)
        {
            var vm = (TestRecordViewModel)BindingContext;
            vm._drawFinger = !vm._drawFinger;
            if (vm._drawFinger)
                EnableDisableDrawing.Text = "Disable Drawing";
            else
                EnableDisableDrawing.Text = "Enable Drawing";
        }

        private void Button_ClickedAcceptedBY(object sender, EventArgs e)
        {
            var Vm = (TestRecordViewModel)BindingContext;
            RecordAcceptedBy SelectedRecord = (RecordAcceptedBy)((TappedEventArgs)e).Parameter;
            Vm.gvTestRecordAcceptedBy_CellContentClick("Image", SelectedRecord);
            Vm.SelectedrecordAcceptedBySource = SelectedRecord;
        }
        private void TapGestureRecognizer_TappedDescription(object sender, EventArgs e)
        {
            var obj = (Xamarin.Forms.TappedEventArgs)e;
            var parameter = (string)obj.Parameter;
            var viewModel = (TestRecordViewModel)BindingContext;
            viewModel.ShowDescriptionPopup(parameter);
        }
    }
}
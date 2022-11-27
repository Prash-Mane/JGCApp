using Acr.UserDialogs;
using JGC.Common.Helpers;
using JGC.DataBase.DataTables.WorkPack;
using JGC.Models.Work_Pack;
using JGC.UserControls.Touch;
using JGC.ViewModels.Work_Pack;
using Plugin.Media.Abstractions;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static JGC.Common.Enumerators.Enumerators;

namespace JGC.Views.Work_Pack
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CWPTagStatusPage : ContentPage
    {
        //Camera Canvas
        List<long> touchIds = new List<long>();
        CWPTagStatusViewModel CwpVm;
        private string _filePath;
        SKBitmap CameraBitmap = new SKBitmap();
        TouchManipulationBitmap Camerabitmap;
        Dictionary<long, SKPath> _inProgressCameraPaths = new Dictionary<long, SKPath>();
        List<SKPath> _completedCameraPaths = new List<SKPath>();
        SKCanvas _saveCameraBitmapCanvas;
        T_TagMilestoneImages SelectedImage;

        public CWPTagStatusPage()
        {
            InitializeComponent();
        }
        private void TappedOnRadiobtn(object sender, EventArgs e)
        {
            var viewModel = (CWPTagStatusViewModel)BindingContext;
            var args = (TappedEventArgs)e;
            viewModel.SelectedCWPTag = (TagMilestoneStatusModel)args.Parameter;
            viewModel.OnClickButton("ClickedRadioBtn");
        }
        private void TappedOnCameraBtn(object sender, EventArgs e)
        {
            var args = (TappedEventArgs)e;
            var viewModel = (CWPTagStatusViewModel)BindingContext;
            viewModel.SelectedCWPTag = (TagMilestoneStatusModel)args.Parameter;
            viewModel.CameraItems = new ObservableCollection<string> { "Camera" };
            viewModel.BindImageFiles("");
            viewModel.CapturedImage = null;
            viewModel.btnSaveRename = "Save Image";
            viewModel.CameraGrid = true;
            CameraUpdateBitmap();
        }


        //Camera events
        #region Camera 
        private async void SelectedCameraItemChanged(object sender, EventArgs e)
        {
            CwpVm = (CWPTagStatusViewModel)BindingContext;
            var x = (UserControls.CustomControls.CustomPicker)sender;
            if (x.SelectedItem == null) return;
            try
            {
                var mediaFile = await CwpVm.TakePhotoAsync();
                UserDialogs.Instance.ShowLoading("Loading..!");
                if (_saveCameraBitmapCanvas != null)
                {
                    _completedCameraPaths.Clear();
                    _inProgressCameraPaths.Clear();
                    CameraUpdateBitmap();
                    CameracanvasView.InvalidateSurface();
                }
                if (mediaFile == null)
                {
                    UserDialogs.Instance.HideLoading();
                    return;
                }
                UserDialogs.Instance.HideLoading();

                CwpVm.imageAsByte = ConvertMediaFileToByteArray(mediaFile);
                CwpVm.imageAsByte = await CwpVm.ResizeImage(CwpVm.imageAsByte);
                Stream stream = new MemoryStream(CwpVm.imageAsByte);
                CameraBitmap = SKBitmap.Decode(stream);
                this.Camerabitmap = new TouchManipulationBitmap(CameraBitmap);
                this.Camerabitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                CameraUpdateBitmap();
                CwpVm.CapturedImage = ImageSource.FromStream(() => stream);
                CwpVm.btnSaveRename = "Save Image";
                CwpVm.SelectedImageFiles = null;
            }
            catch (Exception ex)
            {
            }
        }

        private async void AddFromFileClicked(object sender, EventArgs e)
        {
            CwpVm = (CWPTagStatusViewModel)BindingContext;
            try
            {
                var mediaFile = await CwpVm.PickFileAsync();
                UserDialogs.Instance.ShowLoading("Loading..!");
                if (_saveCameraBitmapCanvas != null)
                {
                    _completedCameraPaths.Clear();
                    _inProgressCameraPaths.Clear();
                    CameraUpdateBitmap();
                    CameracanvasView.InvalidateSurface();
                }
                // var mediaFile = await PickFileAsync();
                if (mediaFile == null)
                {
                    UserDialogs.Instance.HideLoading();
                    return;
                }
                CwpVm.imageAsByte = ConvertMediaFileToByteArray(mediaFile);
                CwpVm.imageAsByte = await CwpVm.ResizeImage(CwpVm.imageAsByte);
                using (Stream stream = mediaFile.GetStream())
                {
                    CameraBitmap = SKBitmap.Decode(stream);
                    this.Camerabitmap = new TouchManipulationBitmap(CameraBitmap);
                    this.Camerabitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                    CameraUpdateBitmap();
                }
                CwpVm.SavePickedImageFromGallery(mediaFile);

                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(null, "Error saving image to database.", "OK");
            }
        }

        private void PImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            var x = (UserControls.CustomControls.CustomPicker)sender;
            SelectedImage = (T_TagMilestoneImages)x.SelectedItem;

            if (SelectedImage != null)
            {
                CameraBitmap = SKBitmap.Decode(Convert.FromBase64String(SelectedImage.FileBytes));
                this.Camerabitmap = new TouchManipulationBitmap(CameraBitmap);
                this.Camerabitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                CameraUpdateBitmap();
                var viewModel = (CWPTagStatusViewModel)BindingContext;

            }
            else
            {
                CameraBitmap = new SKBitmap();
                _saveCameraBitmapCanvas = new SKCanvas(CameraBitmap);
                this.Camerabitmap = new TouchManipulationBitmap(CameraBitmap);
                this.Camerabitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                CameraUpdateBitmap();
            }
        }
        private byte[] ConvertMediaFileToByteArray(MediaFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.GetStream().CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        private void OnCameraCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();
            if (Camerabitmap != null)
            {
                Camerabitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                // Display the bitmap
                Camerabitmap.Paint(canvas);
            }
        }

        private void OnCameraTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            // Convert Xamarin.Forms point to pixels
            Point pt = args.Location;
            SKPoint Camerapoint = new SKPoint((float)(CameracanvasView.CanvasSize.Width * pt.X / CameracanvasView.Width),
                                              (float)(CameracanvasView.CanvasSize.Height * pt.Y / CameracanvasView.Height));

            if (Camerabitmap == null)
                return;

            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (Camerabitmap.HitTest(Camerapoint))
                    {
                        touchIds.Add(args.Id);
                        Camerabitmap.ProcessTouchEvent(args.Id, args.Type, Camerapoint);
                        break;
                    }
                    break;

                case TouchActionType.Moved:
                    if (touchIds.Contains(args.Id))
                    {
                        Camerabitmap.ProcessTouchEvent(args.Id, args.Type, Camerapoint);
                        CameracanvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (touchIds.Contains(args.Id))
                    {
                        Camerabitmap.ProcessTouchEvent(args.Id, args.Type, Camerapoint);
                        touchIds.Remove(args.Id);
                        CameracanvasView.InvalidateSurface();
                    }
                    break;
            }
        }

        private async void CameraUpdateBitmap()
        {
            CameracanvasView.InvalidateSurface();
        }
        #endregion

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            height = height - (height / 4);

            if (width != CameracanvasView.WidthRequest || height != CameracanvasView.HeightRequest)
            {
                CameracanvasView.WidthRequest = width;
                CameracanvasView.HeightRequest = height;
            }
        }
    }
}
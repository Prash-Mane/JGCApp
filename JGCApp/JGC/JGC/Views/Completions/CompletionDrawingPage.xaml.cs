using JGC.Common.Extentions;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.UserControls.Touch;
using JGC.ViewModels.Completions;
using Rg.Plugins.Popup.Services;
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
using static JGC.Common.Enumerators.Enumerators;

namespace JGC.Views.Completions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompletionDrawingPage : ContentPage
    {
        // Canvas declairation 
        SKBitmap SKBitmap;
        SKCanvas _saveBitmapCanvas;
        Dictionary<long, SKPath> _inProgressPaths = new Dictionary<long, SKPath>();
        List<SKPath> _completedPaths = new List<SKPath>();
        TouchManipulationBitmap FullViewbitmap; //Camerabitmap
        List<long> touchIds = new List<long>();
        string base64image;
        bool _drawNewPunch, _zoom, _draw;
        bool IsDrawVisible;
        string DrawingColor;

        public CompletionDrawingPage()
        {
            InitializeComponent();
            _zoom = true;
            IsDrawVisible = true;
        }
        private void Clicked_AddGreenLinePopup(object sender, EventArgs e)
        {
            var ThisBtn = (Button)sender;
            if (ThisBtn.Text == "Add Green Line")
            {
                BtnRedLine.IsVisible = false;
                if (_completedPaths.Count > 0)
                {
                    _completedPaths.Clear();

                    // FullviewReDrawPdf(base64image);
                    canvasFullView.InvalidateSurface();
                    FullViewUpdateBitmap();
                }
                _draw = true;
                _zoom = !_draw;
                ThisBtn.Text = "Cancel";
            }
            else
            {
                BtnRedLine.IsVisible = true;
                if (_completedPaths.Count > 0)
                {
                    _completedPaths.Clear();

                    // FullviewReDrawPdf(base64image);
                    canvasFullView.InvalidateSurface();
                    FullViewUpdateBitmap();
                }
                _draw = false;
                _zoom = !_draw;
                ThisBtn.Text = "Add Green Line";
            }

            //bool result = await Application.Current.MainPage.DisplayActionSheet("Click ok, then tap image at spot you want to add note.", null, null, "Ok", "Cancel") == "Ok" ? true : false;
        }
        private void Clicked_AddRedLinePopup(object sender, EventArgs e)
        {
            var ThisBtn = (Button)sender;
            if (ThisBtn.Text == "Add Red Line")
            {
                BtnGreenLine.IsVisible = false;
                if (_completedPaths.Count > 0)
                {
                    _completedPaths.Clear();

                    //  FullviewReDrawPdf(base64image);
                    canvasFullView.InvalidateSurface();
                    FullViewUpdateBitmap();
                }
                DrawingColor = "red";
                _draw = true;
                _zoom = !_draw;
                ThisBtn.Text = "Cancel";
            }
            else
            {
                BtnGreenLine.IsVisible = true;
                if (_completedPaths.Count > 0)
                {
                    _completedPaths.Clear();

                    //   FullviewReDrawPdf(base64image);
                    canvasFullView.InvalidateSurface();
                    FullViewUpdateBitmap();
                }
                DrawingColor = "green";
                _draw = false;
                _zoom = !_draw;
                ThisBtn.Text = "Add Red Line";
            }
            //bool result = await Application.Current.MainPage.DisplayActionSheet("Click ok, then tap image at spot you want to add note.", null, null, "Ok", "Cancel") == "Ok" ? true : false;
        }
        private async void Button_ClickedFullScreen(object sender, EventArgs e)
        {
            var viewModel = (CompletionDrawingViewModel)BindingContext;
            if (viewModel.SelectedDrawingList != null)
            {
                viewModel.FullPdfUrl = viewModel.PdfUrl;
                viewModel.Mainpage = false;
                viewModel.FullScreenView = true;

                var selectedPDF = viewModel.SelectedDrawingList;
                string DirectoryPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath("Drawings" + "\\" + Settings.ProjectName);
                base64image = await DependencyService.Get<ISaveFiles>().ConvertPDFtoByte(DirectoryPath + "\\" + selectedPDF + ".pdf");
                // byte[] bytes = System.Convert.FromBase64String(base64image);
                FullviewReDrawPdf(base64image);
                canvasFullView.InvalidateSurface();
            }
            else
            {
                string action = await DisplayActionSheet("No drawing selected.", null, "Ok");
            }
        }
        private async Task<byte[]> getfile(string path)
        {
            byte[] FileBytes;
            if (Device.RuntimePlatform == Device.UWP)
                FileBytes = await DependencyService.Get<ISaveFiles>().ReadBytes(path);
            else
                FileBytes = File.ReadAllBytes(path);
            return FileBytes;
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            height = height - (height / 4);
            if (width != canvasFullView.WidthRequest || height != canvasFullView.HeightRequest)
            {
                canvasFullView.WidthRequest = width;
                canvasFullView.HeightRequest = height;
                Pdf.HeightRequest = height;
                Pdf.WidthRequest = width;
            }
        }
        SKPaint redpaint = new SKPaint
        {
            TextSize = 35,
            Color = SKColor.Parse("#FF0000"),
            StrokeWidth = 1,
        };
        SKPaint greenPaint = new SKPaint
        {
            TextSize = 35,
            Color = SKColor.Parse("#00ff00"),
            StrokeWidth = 1,
        };
        //FullView 
        SKPoint ConvertToPixel(SKPoint pt)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                float ScalingFactor = (float)Xamarin.Forms.Device.Info.ScalingFactor;
                pt = new SKPoint((float)(SKBitmap.Width * pt.X / canvasFullView.Width) / ScalingFactor,
                             (float)(SKBitmap.Height * pt.Y / canvasFullView.Height) / ScalingFactor);
            }
            return pt;
        }
        //SKPoint ResizePoints(SKPoint pt)
        //{
        //    return new SKPoint((float)((pt.X + (SKBitmap.TransX * -1)) / SKBitmap.Matrix.ScaleX),
        //                       (float)((pt.Y + (SKBitmap.Matrix.TransY * -1))) / SKBitmap.Matrix.ScaleY);
        //}
        //SKPoint ConvertToPixel(Point pt)
        //{
        //    return new SKPoint((float)(SKBitmap.Width * pt.X / canvasFullView.Width),
        //                       (float)(SKBitmap.Height * pt.Y / canvasFullView.Height));
        //}

        //float x = (info.Width - scale * SKBitmap.Width) / 2;
        //float y = (info.Height - scale * SKBitmap.Height) / 2;
        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            Point pt = args.Location;
            SKPoint point;
            SKPoint ScalingFactorPoints;
            if (SKBitmap != null)
            {
                point = new SKPoint((float)(SKBitmap.Width * pt.X / canvasFullView.Width),
                                   (float)(SKBitmap.Height * pt.Y / canvasFullView.Height));

                float ScalingFactor = (float)Xamarin.Forms.Device.Info.ScalingFactor;
                ScalingFactorPoints = new SKPoint((float)(SKBitmap.Width * pt.X / canvasFullView.Width) / ScalingFactor,
                             (float)(SKBitmap.Height * pt.Y / canvasFullView.Height) / ScalingFactor);
            }
            else
            {
                point =
              new SKPoint((float)(canvasFullView.CanvasSize.Width * pt.X / canvasFullView.Width),
                          (float)(canvasFullView.CanvasSize.Height * pt.Y / canvasFullView.Height));

                float ScalingFactor = (float)Xamarin.Forms.Device.Info.ScalingFactor;
                ScalingFactorPoints = new SKPoint((float)(canvasFullView.CanvasSize.Width * pt.X / canvasFullView.Width) / ScalingFactor,
                             (float)(canvasFullView.CanvasSize.Width * pt.Y / canvasFullView.Height) / ScalingFactor);
            }
            if (FullViewbitmap == null)
                return;
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (_draw)
                    {
                        if (IsDrawVisible)
                        {
                            SKPath path = new SKPath();
                            path.MoveTo(point);
                            _inProgressPaths.Clear();
                            _inProgressPaths.Add(args.Id, path);
                        }
                        break;
                    }
                    else
                    {
                        if (FullViewbitmap.HitTest(ScalingFactorPoints))
                        {
                            touchIds.Add(args.Id);
                            FullViewbitmap.ProcessTouchEvent(args.Id, args.Type, ScalingFactorPoints);
                            break;
                        }
                        break;
                    }
                case TouchActionType.Moved:
                    if (_zoom)
                    {
                        if (touchIds.Contains(args.Id))
                        {
                            FullViewbitmap.ProcessTouchEvent(args.Id, args.Type, ScalingFactorPoints);
                            canvasFullView.InvalidateSurface();
                        }
                    }
                    break;
                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (_draw && IsDrawVisible)
                    {
                        if (_inProgressPaths.Count > 0)
                        {
                            try
                            {
                                _completedPaths.Add(_inProgressPaths[args.Id]);
                            }
                            catch { }

                            _inProgressPaths.Remove(args.Id);
                            FullViewUpdateBitmap();
                        }
                        canvasFullView.InvalidateSurface();
                        break;
                    }
                    else
                    {
                        if (touchIds.Contains(args.Id))
                        {
                            FullViewbitmap.ProcessTouchEvent(args.Id, args.Type, ScalingFactorPoints);
                            touchIds.Remove(args.Id);
                            canvasFullView.InvalidateSurface();
                        }
                        break;
                    }
            }
        }
        private void OnCanvasFullViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            SKRect destRect;
            if (FullViewbitmap != null)
            {
                canvas.Clear();
                if (SKBitmap != null)
                {
                    float scale = Math.Min((float)info.Width / SKBitmap.Height,
                      (float)info.Height / SKBitmap.Height);
                    float x = (info.Width - scale * SKBitmap.Width) / 2;
                    float y = (info.Height - scale * SKBitmap.Height) / 2;
                    destRect = new SKRect(x, y, x + scale * SKBitmap.Width,
                                                      y + scale * SKBitmap.Height);

                }
                else
                {
                    destRect = new SKRect(0, 0, info.Width, info.Height);
                }


                // SKRect dest = new SKRect(0, 0, info.Width, info.Height);
                BitmapStretch stretch = (BitmapStretch.AspectFit);
                BitmapAlignment horizontal = (BitmapAlignment.Start);
                BitmapAlignment vertical = (BitmapAlignment.Start);
                // Display the bitmap
                //  FullViewbitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                FullViewbitmap.Paint(canvas, destRect, stretch, horizontal, vertical);
            }
            //canvas.Clear();

            //if (FullViewbitmap != null)
            //{
            //    FullViewbitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;

            //    // Display the bitmap
            //    FullViewbitmap.Paint(canvas);
            //}
        }
        public static Task<string> ReadPdfCommentPopup()
        {
            var vm = new RejectPopupViewModel();
            //vm.FilterList = Source;
            var tcs = new TaskCompletionSource<string>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var page = new PdfCommentPopup(vm);
                await PopupNavigation.PushAsync(page);
                var value = await vm.GetValue();
                await PopupNavigation.PopAsync(true);
                tcs.SetResult(value);
            });
            return tcs.Task;
        }
        private async void FullViewUpdateBitmap()
        {
            using (_saveBitmapCanvas = new SKCanvas(SKBitmap))
            {
                // image size 
                // canvas size
                // bit touch size
                if (IsDrawVisible && _completedPaths.Count > 0)
                {
                    SKPointMode pointMode = (SKPointMode.Lines);
                    //  _saveBitmapCanvas.DrawCircle(_completedPaths.Last().Points[0].X, _completedPaths.Last().Points[0].Y, 5, paint);

                    string text = await ReadPdfCommentPopup();

                    if (string.IsNullOrWhiteSpace(text)) text = "";
                    SKPaint paint = new SKPaint();
                    if (DrawingColor == "red")
                    {
                        paint = redpaint;
                        _draw = false;
                        _zoom = !_draw;
                        BtnRedLine.Text = "Add red Line";
                        BtnGreenLine.IsVisible = true;
                    }
                    else
                    {
                        paint = greenPaint;
                        _draw = false;
                        _zoom = !_draw;
                        BtnGreenLine.Text = "Add Green Line";
                        BtnRedLine.IsVisible = true;
                    }
                    _saveBitmapCanvas.DrawText(text, _completedPaths.Last().Points[0].X, _completedPaths.Last().Points[0].Y, paint);
                    canvasFullView.InvalidateSurface();



                }
            }
            //    if (_drawNewPunch)
            //    {
            //        //if (_completedPaths.Count > 1)
            //        //{
            //        for (int i = 0; i < _completedPaths.Count - 1; i++)
            //        {
            //            SKPoint[] point = new SKPoint[2];
            //            point[0] = _completedPaths[i].LastPoint;
            //            point[1] = _completedPaths[i + 1].LastPoint;

            //            SKPointMode pointMode = (SKPointMode.Lines);
            //            _saveBitmapCanvas.DrawPoints(pointMode, point, redpaint);
            //        }
            //        _drawNewPunch = !_drawNewPunch;
            //        _completedPaths.Clear();
            //        //}
            //        //else
            //        //{
            //        //    await Application.Current.MainPage.DisplayAlert("", "Required atleast two points for line", "OK");
            //        //}
            //    }
            //}
            // canvasView.InvalidateSurface();

            //if (_completedPaths.Count % 2 == 0 && _completedPaths.Count > 0)
            //{

            //    var selectedAction = await Application.Current.MainPage.DisplayActionSheet("Do you want to confim punch points? Or add additional points ?",
            //                          "Cancel", null, "Confirm", "Add Additional Points");

            //    if (selectedAction == "Cancel")
            //    {
            //        _completedPaths.Clear();
            //        FullviewReDrawPdf(base64image);
            //        canvasFullView.InvalidateSurface();
            //        FullViewUpdateBitmap();
            //    }
            //}


        }


        public async void FullviewReDrawPdf(string base64String)
        {
            var viewModel = (CompletionDrawingViewModel)BindingContext;
            if (viewModel.SelectedDrawingList != null)
            {
                byte[] Base64Stream = Convert.FromBase64String(base64String);
                using (Stream stream = new MemoryStream(Base64Stream))
                {
                    SKBitmap = SKBitmap.Decode(stream);
                    this.FullViewbitmap = new TouchManipulationBitmap(SKBitmap);
                    this.FullViewbitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                }
            }
        }
    }
}
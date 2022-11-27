using JGC.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Acr.UserDialogs;
using JGC.Common.Extentions;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase.DataTables;
using JGC.UserControls.Touch;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static JGC.Common.Enumerators.Enumerators;
using JGC.Common.Constants;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JGC.DataBase.DataTables.WorkPack;
using System.IO;
using JGC.Models.Work_Pack;

namespace JGC.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PunchControlPage : ContentPage
	{
        private double width;
        private double height;

        //Drawing Canvas
        TouchManipulationBitmap bitmap;
        List<long> touchIds = new List<long>();
        Dictionary<long, SKPath> _inProgressPaths = new Dictionary<long, SKPath>();
        List<SKPath> _completedPaths = new List<SKPath>();
        MatrixDisplay matrixDisplay = new MatrixDisplay();
        SKBitmap SKBitmap;
        SKCanvas _saveBitmapCanvas;

        //Camera Canvas
        SKBitmap CameraBitmap = new SKBitmap();
        TouchManipulationBitmap Camerabitmap;
        List<long> CameratouchIds = new List<long>();
        Dictionary<long, SKPath> _inProgressCameraPaths = new Dictionary<long, SKPath>();
        List<SKPath> _completedCameraPaths = new List<SKPath>();
        SKCanvas _saveCameraBitmapCanvas;
        private byte[] imageAsByte = null;
        private string CameraPunchID;
        T_IWPPunchImage SelectedImage;

        bool _drawNewPunch, _zoom, _draw, _drawFinger;
        private string _filePath;
        public T_IWPDrawings SelectedDrawing;
        
        private ImageSource PDFImage;
        public SQLiteConnection conn;

        public ObservableCollection<PunchLayersData> PunchLayers { get; set; }
        public bool PunchLayerGrid { get; private set; }
        public bool PDFGrid { get; private set; }

      
        public PunchControlPage ()
		{
            InitializeComponent ();
            conn = DependencyService.Get<ISQLite>().GetConnection();
            PunchLayers = new ObservableCollection<PunchLayersData>();
            GridTags.IsVisible = false;

        }
        //Canvas elements
        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.StrokeAndFill,
            Color = SKColors.Red,
            StrokeWidth = 5,
        };
        SKPaint Camerapaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Red,
            StrokeWidth = 5,
        };
        SKPoint ConvertToPixel(SKPoint pt)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                float ScalingFactor = (float)Xamarin.Forms.Device.Info.ScalingFactor;
                pt = new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width) / ScalingFactor,
                             (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height) / ScalingFactor);

            }
            return ResizePoints(pt);
        }
        SKPoint ResizePoints(SKPoint pt)
        {
            return new SKPoint((float)((pt.X + (bitmap.Matrix.TransX * -1)) / bitmap.Matrix.ScaleX),
                               (float)((pt.Y + (bitmap.Matrix.TransY * -1))) / bitmap.Matrix.ScaleY);
        }

        //camera 
        SKPoint ConvertToPixelforCamera(SKPoint pt)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                pt = new SKPoint((float)(CameracanvasView.CanvasSize.Width * pt.X / CameracanvasView.Width),
                             (float)(CameracanvasView.CanvasSize.Height * pt.Y / CameracanvasView.Height));
            }
            return ResizeCameraPoints(pt);
        }
        SKPoint ResizeCameraPoints(SKPoint pt)
        {
            return new SKPoint((float)((pt.X + (Camerabitmap.Matrix.TransX * -1)) / Camerabitmap.Matrix.ScaleX),
                               (float)((pt.Y + (Camerabitmap.Matrix.TransY * -1))) / Camerabitmap.Matrix.ScaleY);
        }
        public class PunchLayersData
        {
            public string PunchLayersList { get; set; }
        }

        //Drawing Canvas
        #region drawing canvas
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //var args = (TappedEventArgs)e;
            //string PunchLayer = args.Parameter.ToString();
           

            Drawpoints.IsVisible = _draw = true;
            _zoom = !_draw;
            Drawpoints.Text = "Disable Drawing";

            var viewModel = (PunchControlViewModel)BindingContext;
            if (viewModel.SelectedPunch.VMHub_DocumentsID == 0) { Application.Current.MainPage.DisplayAlert("", "Record Not Found", "OK"); return; }

            var Drawing = conn.Query<T_IWPDrawings>("SELECT * FROM [T_IWPDrawings] WHERE [VMHub_DocumentsID] ='" + viewModel.SelectedPunch.VMHub_DocumentsID + "'");

             viewModel.PDFDrawing = Drawing.FirstOrDefault();
            SelectedDrawing = viewModel.PDFDrawing;
            viewModel.LoadPunchLayerImageAsync(viewModel.PDFDrawing.Name);
           
               

            //  var Drawing = conn.Query<T_IWPDrawings>("SELECT * FROM [T_IWPDrawings] WHERE [Name] ='" + SelectedDrawing.Name + "'");

            

            //viewModel.PunchLayerGrid = false;
            //viewModel.PDFGrid = true;
            viewModel.SelectedPDF = viewModel.PDFDrawing.Name;

            ReDrawAllPunch();
            IWPHelper.IsDrawVisible = false;
            UpdateBitmap();
            canvasView.InvalidateSurface();

        }
        private void TapGestureRecognizer_PopUp(object sender, EventArgs e)
        {
            var obj = (Xamarin.Forms.TappedEventArgs)e;
            var parameter = (IWPPunchListModel)obj.Parameter;
            var viewModel = (PunchControlViewModel)BindingContext;
            viewModel.PopUp(parameter.PunchID);
        }
        private void TapGestureRecognizer_CameraGrid(object sender, EventArgs e)
        {
           
            if (this.Camerabitmap != null)
            ClickedOn_ClearImage(sender, e);
            var args = (TappedEventArgs)e;
            var viewModel = (PunchControlViewModel)BindingContext;         
            var parameter = (IWPPunchListModel)args.Parameter;
            viewModel.SelectedCameraItem = null;
            viewModel.CameraPunch = parameter;
            viewModel.SelectedPunch = parameter;
            GetPunchImages();
           // EnableDisableDrawing.Text = "Disable Drawing";
            
            viewModel.UpdateGrid("CaptureImage", parameter);
        }
        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            Point pt = args.Location;
            SKPoint point =
                new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                            (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));

            if (bitmap == null)
                return;


            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (_draw)
                    {
                        if (IWPHelper.IsDrawVisible)
                        {
                            SKPath path = new SKPath();
                            path.MoveTo(ConvertToPixel(point));
                            _inProgressPaths.Clear();
                            _inProgressPaths.Add(args.Id, path);
                        }
                        break;
                    }
                    else
                    {
                        if (bitmap.HitTest(point))
                        {
                            touchIds.Add(args.Id);
                            bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                            break;
                        }
                        break;
                    }
                case TouchActionType.Moved:
                    if (_zoom)
                    {
                        if (touchIds.Contains(args.Id))
                        {
                            bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                            canvasView.InvalidateSurface();
                        }
                    }
                    break;
                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (_draw && IWPHelper.IsDrawVisible)
                    {
                        if (_inProgressPaths.Count > 0)
                        {
                            try
                            {
                                _completedPaths.Add(_inProgressPaths[args.Id]);
                            }
                            catch { }
                            _inProgressPaths.Remove(args.Id);
                            UpdateBitmap();
                        }
                        canvasView.InvalidateSurface();
                        break;
                    }
                    else
                    {
                        if (touchIds.Contains(args.Id))
                        {
                            bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                            touchIds.Remove(args.Id);
                            canvasView.InvalidateSurface();
                        }
                        break;
                    }
            }

        }
        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            if (bitmap != null)
            {

                bitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;

                // Display the bitmap
                bitmap.Paint(canvas);

            }
        }
        public void ReDrawAllPunch()
        {
            var viewModel = (PunchControlViewModel)BindingContext;
            if (viewModel.PDFDrawing != null)
            {
                byte[] Base64Stream = Convert.FromBase64String(viewModel.PDFDrawing.BinaryCode); //   base64s  PDFDrawing.FileBytes

                PDFImage = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
                using (Stream stream = new MemoryStream(Base64Stream))
                {
                    SKBitmap = SKBitmap.Decode(stream);
                    this.bitmap = new TouchManipulationBitmap(SKBitmap);
                    this.bitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                }

                RedrawingPunchLayer();

            }
        }
        private async void UpdateBitmap()
        {
            using (_saveBitmapCanvas = new SKCanvas(SKBitmap))
            {
                // image size 
                // canvas size
                // bit touch size

                if (IWPHelper.IsDrawVisible && _completedPaths.Count > 0)
                {
                    SKPointMode pointMode = (SKPointMode.Lines);
                    _saveBitmapCanvas.DrawCircle(_completedPaths.Last().Points[0].X, _completedPaths.Last().Points[0].Y, 5, paint);

                }
                if (_drawNewPunch)
                {
                    if (_completedPaths.Count > 1)
                    {
                        for (int i = 0; i < _completedPaths.Count - 1; i++)
                        {
                            SKPoint[] point = new SKPoint[2];
                            point[0] = _completedPaths[i].LastPoint;
                            point[1] = _completedPaths[i + 1].LastPoint;

                            SKPointMode pointMode = (SKPointMode.Lines);
                            _saveBitmapCanvas.DrawPoints(pointMode, point, paint);
                        }
                        _drawNewPunch = !_drawNewPunch;
                        _completedPaths.Clear();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("", "Required atleast two points for line", "OK");
                    }
                }
            }
            // canvasView.InvalidateSurface();

            if (_completedPaths.Count % 2 == 0 && _completedPaths.Count > 0)
            {

                var selectedAction = await Application.Current.MainPage.DisplayActionSheet("Do you want to confim punch points? Or add additional points ?",
                                      "Cancel", null, "Confirm", "Add Additional Points");

                if (selectedAction == "Cancel")
                {
                    _completedPaths.Clear();
                    ReDrawAllPunch();
                    canvasView.InvalidateSurface();
                    UpdateBitmap();
                }
                else if (selectedAction == "Confirm")
                {
                    IWPHelper.PathPoints = _completedPaths;
                    var viewModel = (PunchControlViewModel)BindingContext;
                    viewModel.LoadAddEditPunchTabsAsync("NewPunch");
                }

            }

        }
        private void ClickedOn_ClearImage(object sender, EventArgs e)
        {
            var viewModel = (PunchControlViewModel)BindingContext;
            viewModel.SelectedCameraItem = null;
            ImageFiles.SelectedItem = null;
            CameraBitmap = new SKBitmap();
            _saveCameraBitmapCanvas = new SKCanvas(CameraBitmap);
            this.Camerabitmap = new TouchManipulationBitmap(CameraBitmap);
            this.Camerabitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;

            CameraUpdateBitmap();
        }
        private void Enable_Functionality(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var text = button.Text;
            _draw = _zoom = false;
            if (text == "Disable Drawing")
            {
                _zoom = true;
                Drawpoints.Text = "Enable Drawing";
            }
            else
            {
                _draw = true;
                Drawpoints.Text = "Disable Drawing";
            }


        }
        public async void RedrawingPunchLayer()
        {
            //graphic.SmoothingMode = SmoothingMode.AntiAlias;
            var viewModel = (PunchControlViewModel)BindingContext;

            if (viewModel.SelectedPunchLayer == null)
                return;

            string sqlquery = " SELECT [PunchID],[WorkConfirmed],[XPOS1],[YPOS1],[XPOS2],[YPOS2],[Category] FROM [T_IWPPunchControlItem] "
                           + " WHERE [IWPID] = '" + viewModel.PDFDrawing.IWPID + "' AND [VMHub_DocumentsID] = '" + viewModel.PDFDrawing.VMHub_DocumentsID
                           + "' AND [PunchAdminID] = '" + viewModel.SelectedPunchLayer.ID + "' AND [PunchID] ='"+ viewModel.SelectedPunch.PunchID+"'";
                        //   + " AND [CWPID] = '" + viewModel.PDFDrawing.CWPID + "'"; 
                          // + "' AND ([Cancelled] = 0 OR [Cancelled] IS NULL) AND [OnDocument] = 1 ORDER BY [Category] ASC";

            var PunchControlItem = conn.Query<T_IWPPunchControlItem>(sqlquery);

            List<T_IWPPunchControlItem> punches = new List<T_IWPPunchControlItem>();

            string previousCategory = "", hexColour = "#ffa500";

            foreach (T_IWPPunchControlItem PLitem in PunchControlItem)
            {
                int xpos1 = PLitem.XPOS1;
                int ypos1 = PLitem.YPOS1;
                int xpos2 = PLitem.XPOS2;
                int ypos2 = PLitem.YPOS2;

                string punchID = PLitem.PunchID;
                Boolean workConfirmed = PLitem.WorkConfirmed;
                string category = PLitem.Category;

                if (category != previousCategory)
                {
                    if (viewModel.PunchCategories != null && viewModel.PunchCategories.Count > 0)
                    {
                        foreach (T_IWPPunchCategories value in viewModel.PunchCategories)
                        {
                            if (value.Category == category)
                            {
                                if (!string.IsNullOrEmpty(value.ColourCode))
                                {
                                    hexColour = value.ColourCode;
                                    break;
                                }
                            }
                        }
                    }
                }

                previousCategory = category;

                 DrawPunch(punchID, workConfirmed, hexColour, xpos1, ypos1, xpos2, ypos2);
            }
        }

        public void DrawPunch(string text, Boolean WorkConfirmed, string hexColour, int xpos1, int ypos1, int xpos2, int ypos2)
        {
            SKPaint Linepaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColor.Parse(hexColour),
                StrokeWidth = Device.RuntimePlatform == Device.UWP ? 2 : 5,
                IsAntialias = true
                //PathEffect = SKPathEffect.Create1DPath(SKPath.ParseSvgPathData("M -25 -10 L 25 -10, 25 10, -25 10 Z"),
                //                          55, 0, SKPath1DPathEffectStyle.Morph)
            };

            SKPaint Textpaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColor.Parse(hexColour),
                TextAlign = SKTextAlign.Center,
                Typeface = SKTypeface.FromFamilyName("Arial", SKTypefaceStyle.Bold),
                TextSize = Device.RuntimePlatform == Device.UWP ? 20.0f : 30.0f,
                IsAntialias = true,
            };
            SKPaint BlankRectpaint = new SKPaint()
            {
                Style = SKPaintStyle.StrokeAndFill,
                Color = WorkConfirmed? SKColors.Gray: SKColors.White,
                TextAlign = SKTextAlign.Center,
                IsAntialias = true,
                StrokeWidth = Device.RuntimePlatform == Device.UWP ? 2 : 5
            };
            SKPaint Rectpaint = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColor.Parse(hexColour),
                TextAlign = SKTextAlign.Center,
                IsAntialias = true,
                StrokeWidth = Device.RuntimePlatform == Device.UWP ? 2 : 5
            };
            //arrow
            SKPaint PathPaint = new SKPaint
            {
                Color = SKColor.Parse(hexColour),
                StrokeWidth = Device.RuntimePlatform == Device.UWP ? 20 : 50,
                Style = SKPaintStyle.Fill,
            };

            using (_saveBitmapCanvas = new SKCanvas(SKBitmap))
            {
                SKRect textBounds = new SKRect();
                Textpaint.MeasureText(text, ref textBounds);
                float margin = Device.RuntimePlatform == Device.UWP ? 10 : 15;
                double angleDeg = Math.Round(Math.Atan2(ypos1 - ypos2, xpos1 - xpos2) * 180 / Math.PI);



                // Define the first arrow contour
                SKPath firstArrowPath = new SKPath();

                // center point
                float centerP = (float)Math.Sqrt(Math.Pow((xpos1 - xpos2), 2) + Math.Pow((ypos1 - ypos2), 2));
                float centerX = xpos1 + (margin * 2) * (xpos1 - xpos2) / centerP;
                float centerY = ypos1 + (margin * 2) * (ypos1 - ypos2) / centerP;

                firstArrowPath.MoveTo(centerX, centerY);

                SKPoint vector = new SKPoint(xpos1, ypos1) - new SKPoint(centerX, centerY);
                float length = (float)Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
                vector.X /= length;
                vector.Y /= length;
                SKPoint rotate90 = new SKPoint(-vector.Y, vector.X);
                rotate90.X *= margin;
                rotate90.Y *= margin;

                //first point 
                firstArrowPath.LineTo(new SKPoint(xpos1, ypos1) + rotate90);
                //second point
                firstArrowPath.LineTo(new SKPoint(xpos1, ypos1) - rotate90);


                firstArrowPath.Close();

                int XRectToArrow = 0, YRectToArrow = 0;

                SKRect borderRect = SKRect.Create(new SKPoint(xpos2 - (textBounds.Size.Width + margin) / 2, ypos2 - textBounds.Size.Height - margin / 2),
                                                  new SKSize(textBounds.Size.Width + margin, textBounds.Size.Height + margin));
                if (ypos1 > ypos2)
                    YRectToArrow = (int)(borderRect.Height - margin) / 2;
                if (ypos1 < ypos2)
                    YRectToArrow = -(int)(borderRect.Height - margin / 2);

                if (angleDeg < 0)
                    angleDeg = 360 + (angleDeg);
                if (angleDeg < 45 || angleDeg > 315 || (angleDeg > 135 && angleDeg < 225))
                {
                    if (xpos1 > xpos2)
                        XRectToArrow = (int)(borderRect.Width / 2);
                    if (xpos1 < xpos2)
                        XRectToArrow = -(int)(borderRect.Width / 2);
                    YRectToArrow = -10;
                }

                _saveBitmapCanvas.DrawLine(new SKPoint(xpos1, ypos1), new SKPoint(xpos2 + XRectToArrow, ypos2 + YRectToArrow), Linepaint);
                _saveBitmapCanvas.DrawRect(borderRect, BlankRectpaint);
                _saveBitmapCanvas.DrawRect(borderRect, Rectpaint);
                _saveBitmapCanvas.DrawText(text, xpos2, ypos2, Textpaint);
                _saveBitmapCanvas.DrawPath(firstArrowPath, PathPaint);
            }


        }
        #endregion


        //Camera events
        #region Camera 
        private void TapGestureRecognizer_Camera(object sender, EventArgs e)
        {
            var obj = (Xamarin.Forms.TappedEventArgs)e;
            _drawFinger = true;
            var parameter = (IWPPunchListModel)obj.Parameter;
            CameraPunchID = parameter.PunchID;
            GetPunchImages();
            var viewModel = (PunchControlViewModel)BindingContext;
            viewModel.IsSaveVisible = false;
            viewModel.UpdateGrid("CaptureImage", parameter);
        }
        private void TapGestureRecognizer_Delete(object sender, EventArgs e)
        {
            var obj = (TappedEventArgs)e;
            var parameter = (IWPPunchListModel)obj.Parameter;
            var viewModel = (PunchControlViewModel)BindingContext;
            viewModel.UpdateGrid("DeleteRow", parameter);
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

            if (_drawFinger)
            {
                switch (args.Type)
                {
                    case TouchActionType.Pressed:
                        if (!_inProgressCameraPaths.ContainsKey(args.Id))
                        {
                            SKPath path = new SKPath();
                            path.MoveTo(ConvertToPixelforCamera(Camerapoint));
                            _inProgressCameraPaths.Add(args.Id, path);
                        }
                        break;

                    case TouchActionType.Moved:
                        if (_drawFinger)
                        {
                            if (_inProgressCameraPaths.ContainsKey(args.Id))
                            {
                                SKPath path = _inProgressCameraPaths[args.Id];
                                path.LineTo(ConvertToPixelforCamera(Camerapoint));
                                CameraUpdateBitmap();
                            }
                        }
                        break;

                    case TouchActionType.Released:
                        if (_inProgressCameraPaths.ContainsKey(args.Id))
                        {
                            _completedCameraPaths.Add(_inProgressCameraPaths[args.Id]);
                            _inProgressCameraPaths.Remove(args.Id);
                            CameraUpdateBitmap();
                        }
                        break;
                    case TouchActionType.Cancelled:
                        if (_drawFinger)
                        {
                            if (_inProgressCameraPaths.ContainsKey(args.Id))
                            {
                                _inProgressCameraPaths.Remove(args.Id);
                                CameraUpdateBitmap();
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
        }

        private void Clicked_EnableDisableDrawCamera(object sender, EventArgs e)
        {
            _drawFinger = !_drawFinger;
            if (_drawFinger)
                EnableDisableDrawing.Text = "Disable Drawing";
            else
                EnableDisableDrawing.Text = "Enable Drawing";
        }
        private async void CameraUpdateBitmap()
        {
            Camerapaint.Color = SKColor.Parse(IWPHelper.ColorPicker != null ? IWPHelper.ColorPicker : "#000000");

            using (_saveCameraBitmapCanvas = new SKCanvas(CameraBitmap))
            {
                if (_drawFinger)
                {

                    foreach (SKPath path in _inProgressCameraPaths.Values)
                    {
                        _saveCameraBitmapCanvas.DrawPath(path, Camerapaint);
                    }
                }
            }
            CameracanvasView.InvalidateSurface();
        }
        private async void Cameraclick(object sender, EventArgs e)
        {
            var viewModel = (PunchControlViewModel)BindingContext;
            if (viewModel != null)
            {
                if (viewModel.SelectedCameraItem == null)
                    return;

                var mediaFile = await TakePhotoAsync("CapturedImage");
                UserDialogs.Instance.ShowLoading("Loading..!");
                if (_saveCameraBitmapCanvas != null)
                {
                    _completedCameraPaths.Clear();
                    _inProgressCameraPaths.Clear();
                    CameraUpdateBitmap();
                    CameracanvasView.InvalidateSurface();
                }
                //commented by gunjan on 07/11/19
                //var mediaFile = await TakePhotoAsync("CapturedImage");
                if (mediaFile == null)
                {
                    UserDialogs.Instance.HideLoading();
                    return;
                }
                _filePath = mediaFile.Path;
                using (Stream stream = mediaFile.GetStream())
                {
                    CameraBitmap = SKBitmap.Decode(stream);
                    this.Camerabitmap = new TouchManipulationBitmap(CameraBitmap);
                    this.Camerabitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                    CameraUpdateBitmap();
                }
                IsVisible = viewModel.IsSaveVisible = true;
                _drawFinger = false;
                EnableDisableDrawing.Text = "Enable Drawing";
                UserDialogs.Instance.HideLoading();
            }
        }
        private async void Clicked_PickImage(object sender, EventArgs e)
        {
            var viewModel = (PunchControlViewModel)BindingContext;
            try
            {                
                UserDialogs.Instance.ShowLoading("Loading..!");
                if (_saveCameraBitmapCanvas != null)
                {
                    _completedCameraPaths.Clear();
                    _inProgressCameraPaths.Clear();
                    CameraUpdateBitmap();
                    CameracanvasView.InvalidateSurface();
                }
                var mediaFile = await PickFileAsync();
                if (mediaFile == null)
                {
                    UserDialogs.Instance.HideLoading();
                    return;
                }

                imageAsByte = ConvertMediaFileToByteArray(mediaFile);
                imageAsByte = await viewModel.ResizeImage(imageAsByte);
                T_IWPPunchImage img = new T_IWPPunchImage()
                {
                    DisplayName = Path.GetFileNameWithoutExtension(mediaFile.Path),
                    FileName = Path.GetFileName(mediaFile.Path),
                    Extension = Path.GetExtension(mediaFile.Path),
                    FileBytes = Convert.ToBase64String(imageAsByte),
                    LinkedID = viewModel.SelectedPunch.ID.ToString(),
                    FileSize = imageAsByte.Count().ToString(),
                    ProjectID = Settings.ProjectID,
                    IWPID = IWPHelper.IWP_ID,
                    Module = "WorkPack_Punch",
                    Comment ="",
                };

                imageAsByte = ConvertMediaFileToByteArray(mediaFile);


                using (Stream stream = mediaFile.GetStream())
                {
                    CameraBitmap = SKBitmap.Decode(stream);
                    this.Camerabitmap = new TouchManipulationBitmap(CameraBitmap);
                    this.Camerabitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                    CameraUpdateBitmap();
                }

                if (img.FileBytes != null)
                {
                    bool insertImage = false;

                    conn.InsertOrReplace(img);   //Query<T_IWPPunchImage>(sqlinsertImage);
                    UpdatedWorkPack(IWPHelper.IWP_ID);
                    GetPunchImages();
                    ImageFiles.SelectedItem = ImageFiles.ItemsSource[ImageFiles.ItemsSource.Count - 1];
                    insertImage = true;

                    if (insertImage)
                    {
                        using (Stream stream = mediaFile.GetStream())
                        {
                            CameraBitmap = SKBitmap.Decode(stream);
                            this.Camerabitmap = new TouchManipulationBitmap(CameraBitmap);
                            this.Camerabitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                            CameraUpdateBitmap();
                        }
                    }
                }
            }
            catch (Exception EX)
            {
                await Application.Current.MainPage.DisplayAlert(null, "Error saving image to database.", "OK");
            }
            IsVisible = true;
            viewModel.IsSaveVisible = _drawFinger = false;
            UserDialogs.Instance.HideLoading();
        }
        private async void GetPunchImages()
        {
            var viewModel = (PunchControlViewModel)BindingContext;
            string Sql = " SELECT * FROM [T_IWPPunchImage] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [IWPID]= '" + IWPHelper.IWP_ID + "' AND [LinkedID]= '" + viewModel.SelectedPunch.ID + "'";
                     //  + " AND [PunchID] = '" + viewModel.CameraPunch.PunchID + "'";
            ImageFiles.ItemsSource = conn.Query<T_IWPPunchImage>(Sql);
        }
        private void UpdatedWorkPack(int IWP)
        {
            string SQL = "UPDATE [T_IWP] SET [Updated] = 1 WHERE [ID] = '" + IWP + "'";
            var result =  conn.Query<T_IWP>(SQL);
        }
        public async Task<MediaFile> TakePhotoAsync(string pageName)
        {
            MediaFile file = null;
            var denied = await CheckPermissions();
            if (denied)
            {
                await Application.Current.MainPage.DisplayAlert("Unable to take photos.", "Permissions Denied. Please modify app permisions in settings.", "OK");

                return null;
            }
            //commented by gunjan on 07/11/19
            //MediaFile file = null;
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", "No camera available!", "OK");
                return null;
            }

            
            try
            {
                if (pageName == "CapturedImage")
                {
                    file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        //SaveToAlbum = true,
                        Name = pageName,
                        Directory = "JGC",
                        // SaveMetaData = true
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
           // return file;
            return file == null ? null : file;
        }
        public async Task<MediaFile> PickFileAsync()
        {
            var denied = await CheckPermissions();
            if (denied)
            {
                await Application.Current.MainPage.DisplayAlert("Unable to pick a file.", "Permissions Denied. Please modify app permisions in settings.", "OK");

                return null;
            }

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Gallery", "Picking a photo is not supported.", "OK");
                return null;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();
            return file == null ? null : file;
        }
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
        private byte[] ConvertMediaFileToByteArray(MediaFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.GetStream().CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
        private async void Clicked_SaveCaptureImage(object sender, EventArgs e)
        {
            string newDisplayName = GetNewImageDisplayName();
            var viewModel = (PunchControlViewModel)BindingContext;
            try
            {
                using (SKImage image = SKImage.FromBitmap(CameraBitmap))
                {
                    if (image == null)
                        return;
                    SKData data = image.Encode();
                    data.Size.CompareTo(30);
                    imageAsByte = await viewModel.ResizeImage(data.ToArray());
                }

                T_IWPPunchImage img = new T_IWPPunchImage()
                {
                    DisplayName = newDisplayName,
                    FileName = newDisplayName + ".jpeg",
                    Extension = ".jpeg",
                    FileBytes = Convert.ToBase64String(imageAsByte),
                    LinkedID = viewModel.SelectedPunch.ID.ToString(),
                    FileSize = imageAsByte.Count().ToString(),
                    ProjectID = Settings.ProjectID,
                    IWPID = IWPHelper.IWP_ID,
                    Module = "WorkPack_Punch",
                    Comment = "",
                };

                if (img.FileBytes != null)
                {
                    bool insertImage = false;

                    conn.InsertOrReplace(img);   //Query<T_IWPPunchImage>(sqlinsertImage);
                    UpdatedWorkPack(IWPHelper.IWP_ID);
                    GetPunchImages();
                    ImageFiles.SelectedItem = ImageFiles.ItemsSource[ImageFiles.ItemsSource.Count - 1];
                    insertImage = true;
                    if (insertImage)
                    {
                        GetPunchImages();
                        ImageFiles.SelectedItem = ImageFiles.ItemsSource[ImageFiles.ItemsSource.Count - 1];
                        await Application.Current.MainPage.DisplayAlert("Image Save", "Saved Successfully", "OK");
                    }
                    else
                        await Application.Current.MainPage.DisplayAlert(null, "Error saving image to database.", "OK");
                }
                else
                    await Application.Current.MainPage.DisplayAlert(null, "Please select camera and take a picture to save", "OK");
                viewModel.IsSaveVisible = false;

            }
            catch (Exception ex)
            {

            }
        }
        private string GetNewImageDisplayName()
        {
            try
            {
                var UserProject = conn.Query<T_UserProject>("Select * from T_UserProject where Project_ID = '" + Settings.ProjectID + "'");
                T_UserProject CurrentUserProject = UserProject.FirstOrDefault();
                DateTime CurrentUTC = DateTime.UtcNow;
                TimeZoneInfo ProjectTimeZone = TimeZoneInfo.FindSystemTimeZoneById(CurrentUserProject.TimeZone);
                CurrentUTC = TimeZoneInfo.ConvertTimeFromUtc(CurrentUTC, ProjectTimeZone);
                return CurrentUTC.ToString(AppConstant.CameraDateFormat);
            }
            catch (Exception e)
            {
                DateTime CurrentUTC = DateTime.UtcNow;
                TimeZoneInfo ProjectTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                CurrentUTC = TimeZoneInfo.ConvertTimeFromUtc(CurrentUTC, ProjectTimeZone);
                return CurrentUTC.ToString(AppConstant.CameraDateFormat);
            }
        }
        private void PImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            var x = (UserControls.CustomControls.CustomPicker)sender;
            SelectedImage = (T_IWPPunchImage)x.SelectedItem;

            if (SelectedImage != null)
            {
                CameraBitmap = SKBitmap.Decode(Convert.FromBase64String(SelectedImage.FileBytes));
                this.Camerabitmap = new TouchManipulationBitmap(CameraBitmap);
                this.Camerabitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                CameraUpdateBitmap();
                var viewModel = (PunchControlViewModel)BindingContext;
                viewModel.IsSaveVisible = _drawFinger = false;
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
        private async void Clicked_SaveImageName(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(RenameEntry.Text))
            {
                if (RenameEntry.Text != SelectedImage.DisplayName)
                {
                    bool insertImage = false;
                    try
                    {
                        string renamesql = " UPDATE [T_IWPPunchImage] SET [DisplayName] = '" + RenameEntry.Text.Trim() + "',[FileName] = '" + RenameEntry.Text.Trim() + SelectedImage.Extension + "' "
                                         + " WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [IWPID] ='" + SelectedImage.IWPID + "'"
                                         + " AND [LinkedID] = '" + SelectedImage.LinkedID + "' AND DisplayName = '" + SelectedImage.DisplayName + "'";
                        conn.Query<T_IWPPunchImage>(renamesql);
                        GetPunchImages();
                        insertImage = true;
                    }
                    catch (Exception EX)
                    {
                        insertImage = false;
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Rename Image", "Unable to rename this image as it already exists on VMLive", "OK");

                var viewModel = (PunchControlViewModel)BindingContext;
                viewModel.Showbuttons = true;

            }
        }
        private async void Clicked_Rename(object sender, EventArgs e)
        {
            if (SelectedImage != null)
            {
                var viewModel = (PunchControlViewModel)BindingContext;
                viewModel.Showbuttons = false;
                RenameEntry.Text = SelectedImage.DisplayName;
            }
            else
                await Application.Current.MainPage.DisplayAlert(null, "Please select image for rename", "OK");
        }

        private void ClickedOn_CancleNewPunch(object sender, EventArgs e)
        {
            var viewModel = (PunchControlViewModel)BindingContext;
            _completedPaths.Clear();
            ReDrawAllPunch();
            canvasView.InvalidateSurface();
            UpdateBitmap();
            viewModel.CameraGrid = viewModel.PunchControlGrid = viewModel.PunchCategoryGrid = viewModel.PunchLayerGrid = false;
            viewModel.PDFGrid = true;
        }
        private async void ClickedOn_SaveNewPunch(object sender, EventArgs e)
        {
            var viewModel = (PunchControlViewModel)BindingContext;
            if (viewModel.SelectedCategory != null && viewModel.SelectedFunctionCode != null && viewModel.SelectedCompanyCategoryCodes != null)
            {
               bool result =  viewModel.SavePunch_Click();
                if (result) 
                {
                    await Task.Delay(2000);

                    ReDrawAllPunch();
                    canvasView.InvalidateSurface();
                    _completedPaths.Clear();

                    viewModel.CameraGrid = viewModel.PunchControlGrid = viewModel.PunchCategoryGrid = viewModel.PunchLayerGrid = false;
                    viewModel.PDFGrid = true;
                    IWPHelper.IsDrawVisible = false;
                }
               
            }
            else
                await Application.Current.MainPage.DisplayAlert( "Required Fields", "Please select Category, Function Code and Company Category Code", "OK");
           
        }

        private void ClikedOnShowDrawings(object sender, EventArgs e)
        {
            GridDrawings.IsVisible = true;
            GridTags.IsVisible = false;
        }

        private void ClikedOnShowTags(object sender, EventArgs e)
        {
            GridDrawings.IsVisible = false;
            GridTags.IsVisible = true;
        }

        private void clicked_CancleRename(object sender, EventArgs e)
        {
            var viewModel = (PunchControlViewModel)BindingContext;
            viewModel.Showbuttons = true;
        }
        private async void Clicked_DeleteImage(object sender, EventArgs e)
        {
            if (SelectedImage != null)
            {
                if(SelectedImage.IsUploaded)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", "Images downloaded from VMlive cannot be deleted on hand held", "Ok");
                    return;
                }
                bool result = await Application.Current.MainPage.DisplayActionSheet("Are you sure you want to delete this image?", null, null, "Yes", "No") == "Yes" ? true : false;
                if (result)
                {
                    try
                    {
                        Boolean deleted = false;

                        string deletesql = " DELETE FROM [T_IWPPunchImage] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [IWPID] = '" + SelectedImage.IWPID + "'"
                                         + " AND [LinkedID] = '" + SelectedImage.LinkedID + "' AND DisplayName = '" + SelectedImage.DisplayName + "'";
                        conn.Query<T_IWPPunchImage>(deletesql);
                        GetPunchImages();
                        await Application.Current.MainPage.DisplayAlert("Delete Image", "File has been deleted successfully", "Ok");
                    }
                    catch (Exception ex)
                    {
                        await Application.Current.MainPage.DisplayAlert("Delete Image", "Error occurred deleting image", "Ok");
                    }
                }
            }
            else
                await Application.Current.MainPage.DisplayAlert(null, "Please select image for delete", "OK");
        }
        #endregion
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            height = height - (height / 4);
            if (width != canvasView.WidthRequest || height != canvasView.HeightRequest)
            {
                canvasView.WidthRequest = width;
                canvasView.HeightRequest = height;
            }
            if (width != CameracanvasView.WidthRequest || height != CameracanvasView.HeightRequest)
            {
                CameracanvasView.WidthRequest = width;
                CameracanvasView.HeightRequest = height;
            }
        }

    }
}
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase.DataTables;
using JGC.UserControls.Touch;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static JGC.Common.Enumerators.Enumerators;

namespace JGC.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PandidPage : ContentPage
	{
        public SQLiteConnection conn;
        TouchManipulationBitmap bitmap;
        SKBitmap SKBitmap;        
        MatrixDisplay matrixDisplay = new MatrixDisplay();
        byte[] Base64Stream;
        List<long> touchIds = new List<long>();
        public PandidPage ()
		{
			InitializeComponent ();
            conn = DependencyService.Get<ISQLite>().GetConnection();
            canvasView.HeightRequest = Device.RuntimePlatform == Device.UWP ? 800: App.ScreenHeight - (0.2 * App.ScreenHeight‬);
           // canvasView.WidthRequest = App.ScreenWidth;
            GetAttachmentData();
        }
        private async void GetAttachmentData()
        {
            string sql = "SELECT * FROM [T_TestLimitDrawing] WHERE [ProjectID] = '" + CurrentPageHelper.ETestPackage.ProjectID + "' AND[ETestPackageID] = '" + CurrentPageHelper.ETestPackage.ID + "' AND[IsPID] = 1 ORDER BY[OrderNo] ASC";         // var gata = await _drawingsRepository.GetAsync();
            var PunchList = conn.Query<T_TestLimitDrawing>(sql);
            List<T_TestLimitDrawing> punches = new List<T_TestLimitDrawing>();
            //PunchList.Select(i => i).ToList();
            AttachmentList.ItemsSource = PunchList.Select(i => i).ToList();
        }
        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            // Convert Xamarin.Forms point to pixels
            Point pt = args.Location;
            SKPoint point =
                new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                            (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));

            if (bitmap == null)
                return;
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (bitmap.HitTest(point))
                    {
                        touchIds.Add(args.Id);
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        break;
                    }
                    break;

                case TouchActionType.Moved:
                    if (touchIds.Contains(args.Id))
                    {
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (touchIds.Contains(args.Id))
                    {
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        touchIds.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;
            }
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
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

                // Display the matrix in the lower-right corner
                SKSize matrixSize = matrixDisplay.Measure(bitmap.Matrix);

                matrixDisplay.Paint(canvas, bitmap.Matrix,
                    new SKPoint(info.Width - matrixSize.Width,
                                info.Height - matrixSize.Height));
            }
        }

        private void SelectedAttacehed_Click(object sender, EventArgs e)
        {

            var args = ((Xamarin.Forms.Picker)sender).SelectedItem;
            var parameter = (T_TestLimitDrawing)args;
            if (parameter == null)
            {
                return;
            }
            Base64Stream = Convert.FromBase64String(parameter.FileBytes);
            using (Stream stream = new MemoryStream(Base64Stream))
            {
                SKBitmap = SKBitmap.Decode(stream);
                this.bitmap = new TouchManipulationBitmap(SKBitmap);
                this.bitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
            }
            canvasView.InvalidateSurface();
        }

        private void Clicked_Reload(object sender, EventArgs e)
        {
            if (Base64Stream != null)
            {
                using (Stream stream = new MemoryStream(Base64Stream))
                {
                    SKBitmap = SKBitmap.Decode(stream);
                    this.bitmap = new TouchManipulationBitmap(SKBitmap);
                    this.bitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                }
                canvasView.InvalidateSurface();
            }
        }
    }
}
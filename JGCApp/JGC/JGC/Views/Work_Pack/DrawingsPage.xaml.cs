using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase.DataTables;
using JGC.DataBase.DataTables.WorkPack;
using JGC.UserControls.Touch;
using JGC.ViewModels.Work_Pack;
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

namespace JGC.Views.Work_Pack
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DrawingsPage : ContentPage
	{
        SKBitmap Bitmap = new SKBitmap();
        TouchManipulationBitmap bitmap;
        List<long> touchIds = new List<long>();
        Dictionary<long, SKPath> _inProgressPaths = new Dictionary<long, SKPath>();
        List<SKPath> _completedPaths = new List<SKPath>();
        SKCanvas _saveBitmapCanvas;
        private byte[] imageAsByte = null;
        SKBitmap SKBitmap;
        public DrawingsPage ()
		{
			InitializeComponent ();
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
        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            // Convert Xamarin.Forms point to pixels
            Point pt = args.Location;
            SKPoint point = new SKPoint((float)(CanvasView.CanvasSize.Width * pt.X / CanvasView.Width),
                                              (float)(CanvasView.CanvasSize.Height * pt.Y / CanvasView.Height));

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
                            CanvasView.InvalidateSurface();
                        }
                        break;

                    case TouchActionType.Released:
                    case TouchActionType.Cancelled:
                        if (touchIds.Contains(args.Id))
                        {
                            bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                            touchIds.Remove(args.Id);
                            CanvasView.InvalidateSurface();
                        }
                        break;
                }
            }

        private void ClickedOn_SelectedItem(object sender, EventArgs e)
        {
            var args = ((Xamarin.Forms.Picker)sender).SelectedItem;
            var viewModel = (DrawingsViewModel)BindingContext;
            var parameter = (T_IWPAttachments)args;
            if (parameter == null)
            {
                SKBitmap = new SKBitmap();
                this.bitmap = new TouchManipulationBitmap(SKBitmap);
                this.bitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                CanvasView.InvalidateSurface();
               if(viewModel.CurrentDrawings != null)
                    viewModel.SelectedDrawings = viewModel.DrawingsList.Where(i => i.DisplayName == viewModel.CurrentDrawings.DisplayName).Select(i => i).FirstOrDefault();
                return;
            }
            if (Path.GetExtension(parameter.FileName).ToLower() != ".pdf")
            {
                byte[] Base64Stream = Convert.FromBase64String(parameter.FileBytes);
                using (Stream stream = new MemoryStream(Base64Stream))
                {
                    SKBitmap = SKBitmap.Decode(stream);
                    this.bitmap = new TouchManipulationBitmap(SKBitmap);
                    this.bitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                }
                CanvasView.InvalidateSurface();
            }
            else
            {
                SKBitmap = new SKBitmap();
                this.bitmap = new TouchManipulationBitmap(SKBitmap);
                this.bitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                CanvasView.InvalidateSurface();
                viewModel.LoadDrawings(parameter);
            }
        }
    }
}
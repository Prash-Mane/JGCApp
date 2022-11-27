using JGC.Common.ColorChanged;
using JGC.Common.Helpers;
using JGC.UserControls.Touch;
using JGC.ViewModels.E_Test_Package;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.UserControls.PopupControls.ColorSelection_CustomColor
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ColorPickerPopup : PopupPage
    {
        private ColorPickerViewModel viewModel;
        private const int _shrinkage = 50;
        private SKColor[] _colors = new SKColor[8];

        private static bool _ColorChanged;

        private readonly SKPaint _touchCircleOutline = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.White.ToSKColor(),
            StrokeWidth = 5,
            IsAntialias = true
        };

        private readonly SKPaint _touchCircleFill = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        private readonly SKPaint _circlePalette = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        private readonly SKPaint _rectanglePalette = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        private SKPoint _touchLocation;
        private float _touchCircleRadius = 15;
        private SKColor _selectedColor = Color.Transparent.ToSKColor();

        private SKPoint _center;
        private float _radius;
                
        public event EventHandler<ColorChangedEventArgs> ColorChanged;
        public ColorPickerPopup()
        {
            InitializeComponent();
            BindingContext = viewModel = new ColorPickerViewModel();

            _ColorChanged = false;
            for (int i = 0; i < _colors.Length; i++)
            {
                _colors[i] = SKColor.FromHsl(i * 360f / 7, 100, 50);
            }
        }

        private async void ShowColorsPopup(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new ColorSelectionPopup());
        }

        async void OnClose(object sender, EventArgs e)
        {
            if (this.Parent.FindByName("WheelPickerButton") is Button btn)
            {
                btn.IsEnabled = true;
            }

            await Navigation.PopPopupAsync();
        }

        async void OnSelected(object sender, EventArgs e)
        {
            if (_ColorChanged)
            {
                ColorChanged?.Invoke(this, new ColorChangedEventArgs(_selectedColor.ToFormsColor()));
                var selectedcolor = _selectedColor.ToString();

                CurrentPageHelper.ColorPicker = IWPHelper.ColorPicker =  selectedcolor;
                _ColorChanged = false;

            }

            if (this.Parent.FindByName("WheelPickerButton") is Button btn)
            {
                btn.IsEnabled = true;
            }

            await Navigation.PopPopupAsync(true);
        }

        void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {

            var info = e.Info;
            var surface = e.Surface;
            var canvas = surface.Canvas;
            canvas.Clear();

            _center = new SKPoint(info.Rect.MidX, info.Rect.MidY);
            _radius = (Math.Min(info.Width, info.Height) - _shrinkage) / 2;
            _circlePalette.Shader = SKShader.CreateSweepGradient(_center, _colors, null);

            canvas.DrawCircle(_center, _radius, _circlePalette);

            var rectLeft = info.Rect.MidX - _radius;
            var rectTop = 0;
            var rectRight = rectLeft + _radius * 2;
            var rectBottom = rectTop + _shrinkage;
            var rect = new SKRect(rectLeft, rectTop, rectRight, rectBottom);

            var rectPaletteLeft = new SKPoint(rectLeft, _shrinkage / 2);
            var rectPaletteRight = new SKPoint(rectLeft + _radius * 2, _shrinkage / 2);

            _rectanglePalette.Shader = SKShader.CreateLinearGradient(rectPaletteLeft, rectPaletteRight, _colors, null, SKShaderTileMode.Clamp);

            canvas.DrawRect(rect, _rectanglePalette);

            //insure touch circle in the center of color wheel
            if (_touchLocation == SKPoint.Empty)
            {
                _touchLocation = _center;
               // _ColorChanged = true;
            }

            if (_ColorChanged)
            {
                using (var bmp = new SKBitmap(info))
                {
                    IntPtr dstpixels = bmp.GetPixels();

                    var succeed = surface.ReadPixels(info, dstpixels, info.RowBytes, (int)_touchLocation.X, (int)_touchLocation.Y);
                    if (succeed)
                    {
                        _selectedColor = bmp.GetPixel(0, 0);
                        _touchCircleFill.Color = _selectedColor;
                    }
                }
            }

            canvas.DrawCircle(_touchLocation, _touchCircleRadius, _touchCircleOutline);
            canvas.DrawCircle(_touchLocation, _touchCircleRadius, _touchCircleFill);
        }

        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            var skPoint = args.Location.ToPixelSKPoint(canvasView);
            if (skPoint.IsInsideCircle(_center, _radius))
            {
                _touchLocation = skPoint;

                if (args.Type == TouchActionType.Entered ||
                    args.Type == TouchActionType.Pressed ||
                    //args.Type == TouchActionType.Moved ||
                    args.Type == TouchActionType.Released)
                {
                    _ColorChanged = true;
                    canvasView.InvalidateSurface();
                }
            }
            //else
            //{
            //    _colorChanged = false;
            //}
        }
    }
}
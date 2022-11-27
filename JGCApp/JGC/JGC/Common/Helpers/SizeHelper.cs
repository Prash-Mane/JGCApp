using System;
using System.Threading.Tasks;
using JGC.Common.Constants;
using JGC.Common.Extentions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace JGC
{
    public static class SizeHelper
    {
        static float heightCoef, widthCoef, fontCoef;
        static Size pageSize;

        public static float StatusBarHeight { get; set; } = 20;

        public static Thickness HeaderMargin
        {
            get
            {
                var coef = GetResizeCoeficientsAsync().Result;
                var yOffset = StatusBarHeight + (40 * coef.y); //40 is HeaderUC HeightRequest
                return new Thickness(0, yOffset, 0, 0);
            }
        }

        public static Thickness FooterMargin
        {
            get
            {
                //var coef = GetResizeCoeficientsAsync().Result;
                //var yOffset = 51 * coef.y; //51 is FooterUC HeightRequest
                return new Thickness(0, 0, 0, 51);
            }
        }

        public static Thickness SafeArea { get; set; }

        public static Thickness HeaderAndFooterMargin => new Thickness(0, HeaderMargin.Top, 0, FooterMargin.Bottom);


        public async static Task<Size> GetPageSize()
        {
            if (pageSize != default(Size))
                return pageSize;


            while (App.Current.MainPage?.Width < 1 && App.Current.MainPage?.Height < 1)
            {
                await Task.Delay(100);
            }

            if (pageSize == default(Size))
                pageSize = new Size(App.Current.MainPage.Width, App.Current.MainPage.Height);
            return pageSize;
        }

        public static async Task<(float x, float y)> GetResizeCoeficientsAsync()
        {
            if (heightCoef > 0 && widthCoef > 0)
                return (widthCoef, heightCoef);

            var size = await GetPageSize();
            if (Device.RuntimePlatform == Device.Android)
            {
                heightCoef = (float)(size.Height / AppConstant.defaultDroidPageHeight);
                widthCoef = (float)(size.Width / AppConstant.defaultDroidPageWidth);
            }
            else
            {
                heightCoef = (float)(size.Height / AppConstant.defaultIOSPageHeight);
                widthCoef = (float)(size.Width / AppConstant.defaultIOSPageWidth);
            }
            return (widthCoef, heightCoef);
        }

        public static async Task AdjustFont(IFontElement fontElement)
        {
            if (fontCoef == default(float))
            {
                var coefs = await GetResizeCoeficientsAsync();
                fontCoef = Math.Min(coefs.x, coefs.y);
            }

            var propFontSize = fontElement.GetType().GetProperty("FontSize");
            propFontSize.SetValue(fontElement, (double)propFontSize.GetValue(fontElement) * fontCoef);
        }

        public static async Task AdjustSize(Element element)
        {
            if (element.ClassId == "doNotResize")
                return;

            var coefs = await GetResizeCoeficientsAsync();

            if (element is Grid grid)
            {
                foreach (var rowDef in grid.RowDefinitions)
                {
                    if (rowDef.Height.IsAbsolute && rowDef.Height.Value > 0)
                    {
                        rowDef.Height = rowDef.Height.Value * heightCoef;
                    }
                }

                foreach (var colDef in grid.ColumnDefinitions)
                {
                    if (colDef.Width.IsAbsolute && colDef.Width.Value > 0)
                    {
                        colDef.Width = colDef.Width.Value * widthCoef;
                    }
                }
            }
            else if (element is ListView listView)
            {
                if (!listView.HasUnevenRows && listView.RowHeight > 0)
                    listView.RowHeight = (int)(listView.RowHeight * heightCoef);
            }
            else if (element is StackLayout stackLayout)
            {
                if (stackLayout.Spacing > 0)
                    stackLayout.Spacing *= stackLayout.Orientation == StackOrientation.Horizontal ? widthCoef : heightCoef;
            }

            var view = element as View;

            //1x1 aspect is resized keeping ratio
            if (view.HeightRequest == view.WidthRequest && view.HeightRequest > 0)
            {
                //find out if we need to increase or decrease view. If aspects change in different directions, do not resize
                double mediumCoef = 1;

                if (element.ClassId == "resizeByHeight")
                    mediumCoef = heightCoef;
                else if (element.ClassId == "resizeByWidth")
                    mediumCoef = widthCoef;
                else
                {
                    if (widthCoef >= 1 && heightCoef >= 1)
                        mediumCoef = Math.Min(widthCoef, heightCoef);
                    else if (widthCoef < 1 && heightCoef < 1)
                        mediumCoef = Math.Max(widthCoef, heightCoef);
                }


                var binding = view.GetBinding(View.HeightRequestProperty);
                if (binding == null)
                {
                    view.HeightRequest *= mediumCoef;
                    view.WidthRequest *= mediumCoef;
                }
            }
            else if (element.ClassId == "resizeByHeight")
            {
                view.HeightRequest *= heightCoef;
                view.WidthRequest *= heightCoef;
            }
            else if (element.ClassId == "resizeByWidth")
            {
                view.HeightRequest *= widthCoef;
                view.WidthRequest *= widthCoef;
            }
            else
            {
                if (view.HeightRequest > 0)
                {
                    var binding = view.GetBinding(View.HeightRequestProperty);
                    if (binding == null) //if there' binding, we suppose it's calculated properly in VM. Otherwise - binding is lost. So things like accordion view expand won't work
                        view.HeightRequest *= heightCoef;
                }
                if (view.WidthRequest > 0)
                {
                    var binding = view.GetBinding(View.WidthRequestProperty);
                    if (binding == null)
                        view.WidthRequest *= widthCoef;
                }
            }

            if (view.Margin != default(Thickness))
            {
                var binding = view.GetBinding(View.MarginProperty);

                if (binding == null)
                {
                    var margin = view.Margin;
                    margin.Left *= widthCoef;
                    margin.Right *= widthCoef;
                    margin.Top *= heightCoef;
                    margin.Bottom *= heightCoef;
                    view.Margin = margin;
                }
            }

            var propPadding = view.GetType().GetProperty("Padding");
            if (propPadding != null)
            {
                var padding = (Thickness)propPadding.GetValue(view);
                var binding = view.GetBinding(Layout.PaddingProperty);//add other padding props if needed. For now, the only binding we have in app is for StackLayout
                if (padding != default(Thickness) && binding == null)
                {
                    padding.Left *= widthCoef;
                    padding.Right *= widthCoef;
                    padding.Top *= heightCoef;
                    padding.Bottom *= heightCoef;
                    propPadding.SetValue(view, padding);
                }
            }
        }
    }
}


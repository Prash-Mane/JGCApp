using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JGC.UserControls.CustomControls
{
    public class RectangleDatePicker : DatePicker
	{
		public static readonly BindableProperty ImageProperty =
		   BindableProperty.Create(nameof(Image), typeof(string), typeof(RectangleDatePicker), string.Empty);

		public string Image
		{
			get { return (string)GetValue(ImageProperty); }
			set { SetValue(ImageProperty, value); }
		}
		public static readonly BindableProperty IsBorderErrorVisibleProperty =
			BindableProperty.Create(nameof(IsBorderErrorVisible), typeof(bool), typeof(RectangleDatePicker), false, BindingMode.TwoWay);

		public bool IsBorderErrorVisible
		{
			get { return (bool)GetValue(IsBorderErrorVisibleProperty); }
			set
			{
				SetValue(IsBorderErrorVisibleProperty, value);
			}
		}

		public static readonly BindableProperty BorderErrorColorProperty =
			BindableProperty.Create(nameof(BorderErrorColor), typeof(Xamarin.Forms.Color), typeof(RectangleDatePicker), Xamarin.Forms.Color.Transparent, BindingMode.TwoWay);

		public Xamarin.Forms.Color BorderErrorColor
		{
			get { return (Xamarin.Forms.Color)GetValue(BorderErrorColorProperty); }
			set
			{
				SetValue(BorderErrorColorProperty, value);
			}
		}
	}
}
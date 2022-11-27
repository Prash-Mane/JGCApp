using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JGC.UserControls.CustomControls
{
    public class CustomCheckBox : StackLayout
    {
        BoxView boxBackground = new BoxView { HeightRequest = 25, WidthRequest = 25, BackgroundColor = Color.LightGray, VerticalOptions = LayoutOptions.Center };
        BoxView boxSelected = new BoxView { IsVisible = false, HeightRequest = 24, WidthRequest = 24, BackgroundColor = Color.Accent, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center };
        Label lblSelected = new Label { Text = "✓", FontSize = 19, FontAttributes = FontAttributes.Bold, IsVisible = false, TextColor = Color.Accent, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
       // Label lblOption = new Label { VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 14 };
        private CheckType _type = CheckType.Box;

        public CustomCheckBox()
        {
            this.Orientation = StackOrientation.Horizontal;
            this.Margin = new Thickness(20, 0);
            this.Padding = new Thickness(0);
           // this.Spacing = 10;
            this.Children.Add(new Grid { Children = { boxBackground, boxSelected } });
           // this.Children.Add(lblOption);
            this.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => { IsChecked = !IsChecked; }),
            });
        }
        public CustomCheckBox(string optionName, int key) : this()
        {
            Key = key;
          //  lblOption.Text = optionName;
        }
        public int Key { get; set; }
       // public string Text { get => lblOption.Text; set => lblOption.Text = value; }
        public bool IsChecked
        {
            get => ((this.Children[0] as Grid).Children[1]).IsVisible; set
            {
                (this.Children[0] as Grid).Children[1].IsVisible = value;
                SetValue(IsCheckedProperty, value);
            }
        }
        public Color Color { get => boxSelected.BackgroundColor; set { boxSelected.BackgroundColor = value; lblSelected.TextColor = value; } }
      //  public Color TextColor { get => lblOption.TextColor; set => lblOption.TextColor = value; }
        public CheckType Type { get => _type; set { _type = value; UpdateType(value); } }

        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(CustomCheckBox), Color.Accent, propertyChanged: (bo, ov, nv) => (bo as CustomCheckBox).Color = (Color)nv);
      //  public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CustomCheckBox), Color.Gray, propertyChanged: (bo, ov, nv) => (bo as CustomCheckBox).TextColor = (Color)nv);
        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CustomCheckBox), false, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as CustomCheckBox).IsChecked = (bool)nv);
        public static readonly BindableProperty KeyProperty = BindableProperty.Create(nameof(Key), typeof(int), typeof(CustomCheckBox), 0, propertyChanged: (bo, ov, nv) => (bo as CustomCheckBox).Key = (int)nv);
      //  public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomCheckBox), "", propertyChanged: (bo, ov, nv) => (bo as CustomCheckBox).Text = (string)nv);



        void UpdateType(CheckType _Type)
        {
            switch (_Type)
            {
                case CheckType.Box:
                    (this.Children[0] as Grid).Children.Remove(lblSelected);
                    (this.Children[0] as Grid).Children.Add(boxSelected);
                    break;
                case CheckType.Check:
                    lblSelected.Text = "✓";
                    (this.Children[0] as Grid).Children.Remove(boxSelected);
                    (this.Children[0] as Grid).Children.Add(lblSelected);
                    break;
                case CheckType.Cross:
                    lblSelected.Text = "✕";
                    (this.Children[0] as Grid).Children.Remove(boxSelected);
                    (this.Children[0] as Grid).Children.Add(lblSelected);
                    break;
                case CheckType.Star:
                    lblSelected.Text = "★";
                    (this.Children[0] as Grid).Children.Remove(boxSelected);
                    (this.Children[0] as Grid).Children.Add(lblSelected);
                    break;
            }
        }
        public enum CheckType
        {
            Box,
            Check,
            Cross,
            Star
        }
    }
}

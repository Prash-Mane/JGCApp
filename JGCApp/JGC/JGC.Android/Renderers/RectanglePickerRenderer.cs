﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using JGC.Droid.Renderers;
using JGC.UserControls.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RectanglePicker), typeof(RectanglePickerRenderer))]
namespace JGC.Droid.Renderers
{
    public class RectanglePickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            SetControlStyle();

            UpdateBorders();
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            SetControlStyle();

            if (e.PropertyName == RectanglePicker.IsBorderErrorVisibleProperty.PropertyName)
                UpdateBorders();
        }
        private void SetControlStyle()
        {
            if (Control != null)
            {
                Drawable imgDropDownArrow = Resources.GetDrawable(Resource.Drawable.ddarrow);
                imgDropDownArrow.SetBounds(5, 5, 5, 5);
                Control.SetCompoundDrawablesRelativeWithIntrinsicBounds(null, null, imgDropDownArrow, null);
            }
        }

        public LayerDrawable AddPickerStyles(string imagePath)
        {
            ShapeDrawable border = new ShapeDrawable();
            border.Paint.Color = Android.Graphics.Color.Gray;
            border.SetPadding(10, 10, 10, 10);
            border.Paint.SetStyle(Paint.Style.Stroke);

            Drawable[] layers = { border, GetDrawable(imagePath) };
            LayerDrawable layerDrawable = new LayerDrawable(layers);
            layerDrawable.SetLayerInset(0, 0, 0, 0, 0);

            return layerDrawable;
        }
        void UpdateBorders()
        {
            GradientDrawable shape = new GradientDrawable();
            shape.SetShape(ShapeType.Rectangle);
            shape.SetCornerRadius(10);

            if (((RectanglePicker)this.Element).IsBorderErrorVisible)
            {
                shape.SetStroke(3, ((RectanglePicker)this.Element).BorderErrorColor.ToAndroid());
            }
            else
            {
                shape.SetStroke(3, Android.Graphics.Color.LightGray);
                this.Control.SetBackground(shape);
            }

            this.Control.SetBackground(shape);
        }

        private BitmapDrawable GetDrawable(string imagePath)
        {
            int resID = Resources.GetIdentifier(imagePath, "drawable", this.Context.PackageName);
            var drawable = Android.Support.V4.Content.ContextCompat.GetDrawable(this.Context, resID);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 70, 70, true));
            result.Gravity = Android.Views.GravityFlags.Right;

            return result;
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}
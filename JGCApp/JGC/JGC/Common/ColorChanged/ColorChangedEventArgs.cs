using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;


namespace JGC.Common.ColorChanged
{
    public class ColorChangedEventArgs : EventArgs
    {

        public ColorChangedEventArgs(Color color)
        {
            this.Color = color;
        }

        public Color Color { get; private set; }
    }
}
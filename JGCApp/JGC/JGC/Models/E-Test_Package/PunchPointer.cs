using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JGC.Models.E_Test_Package
{
    public class PunchPointer
    {
        public Image SpoolDrawingImage { get; set; }
        public Boolean SelectingPunchPoints { get; set; }
        public Point FirstPoint { get; set; }
        public Point SecondPoint { get; set; }
        public string Error { get; set; }
    }
}

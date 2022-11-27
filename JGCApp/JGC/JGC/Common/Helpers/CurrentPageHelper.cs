using JGC.DataBase.DataTables;
using JGC.Models.E_Test_Package;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Common.Helpers
{
  public static class CurrentPageHelper
    {
        public static T_ETestPackages  ETestPackage  { get; set;}
        public static T_TestLimitDrawing CurrentDrawing { get; set; }
        public static T_AdminPunchLayer CurrentLayer { get; set; }
        public static PunchOverview CurrentPunchOverview { get; set; }
        public static string PunchID { get; set; }
        public static SpoolDrawingModel SelectedDrawing { get; set; }
        public static T_TestLimitDrawing PDFDrawing { get; set; }
        public static List<T_AdminPunchCategories> PunchCategories { get; set; }
        public static List<SKPath> PathPoints = new List<SKPath>();
        public static string ColorPicker { get; set; }
        public static bool IsDrawVisible { get; set; }
        public static bool IsOnlyOverview { get; set; }
        public static List<T_ControlLogSignature> CurrentSessionSignature { get; set; }

    }
}

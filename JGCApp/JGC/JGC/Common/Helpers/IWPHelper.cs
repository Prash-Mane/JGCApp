using JGC.DataBase.DataTables.WorkPack;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Common.Helpers
{
    public static class IWPHelper
    {
        public static bool includedAttachments { get; set; }
        public static bool includedDrawings { get; set; }
        public static bool includedPredecessors { get; set; }
        public static bool includedSuccessors { get; set; }
        public static bool includedCWPTags { get; set; }
        //public static T_IWP SelectedJobSetting { get; set; }
        public static int IWP_ID { get; set; }
        public static int PreviousIWP_ID { get; set; }
        public static bool IsDrawVisible { get; set; }
        public static string ColorPicker { get; set; }
        public static List<SKPath> PathPoints = new List<SKPath>();
    }
}

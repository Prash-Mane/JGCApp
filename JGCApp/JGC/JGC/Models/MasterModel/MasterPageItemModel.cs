using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JGC.Models.MasterModel
{
    public class MasterPageItemModel
    {
        public string Title { get; set; }
        public string IconSource { get; set; }
        public int IconRotation { get; set; }
        public Type TargetType { get; set; }
        public string Contents { get; set; }
        public int ItemId { get; set; }
        public Color ActiveTextColor { get; set; }
        public Color SubMenuBGColor { get; set; }
        public bool IsContentVisible { get; set; }
        public double ContentHeight { get; set; }
    }
}

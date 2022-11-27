using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JGC.Common.Constants
{
    public class Cache
    {       
        public static int CurrentScreenHeight;      
        public static string accessToken { get; set; }
        public static bool isModel { get; set; }
        public static string profileImage { get; set; }
        public static MasterDetailPage MasterPage { get; set; }
    }
}

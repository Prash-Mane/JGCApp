using JGC.Common.Interfaces;
using JGC.UWP.DependancyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[assembly: Xamarin.Forms.Dependency(typeof(CloseApplication))]

namespace JGC.UWP.DependancyObjects
{
    public class CloseApplication : ICloseApplication
    {
        public void closeApplication()
        {
            Windows.UI.Xaml.Application.Current.Exit();
        }
    }
}

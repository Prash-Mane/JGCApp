using JGC.Common.Helpers;
using JGC.ViewModels;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.UserControls
{

	public partial class FooterUC 
	{
		public FooterUC ()
		{
			InitializeComponent ();
            if (Settings.Report != null && Settings.Report != "")
                Footer.IsVisible = true;
            else
                Footer.IsVisible = false;
            
        }

    }
}
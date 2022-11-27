using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectListPage : ContentPage
    {
        public ProjectListPage()
        {
            InitializeComponent();
        }
        void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
            //Debug.WriteLine("Tapped: " + e.Item);
            ((ListView)sender).SelectedItem = null;//e.Item; // de-select the row
        }
    }
}
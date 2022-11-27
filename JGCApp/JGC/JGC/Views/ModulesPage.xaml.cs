using JGC.Common.Helpers;
using JGC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Windows.UI.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModulesPage : ContentPage
    {
        public ModulesPage()
        {
            InitializeComponent();
           // SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        protected override void OnAppearing()
        {
            MessagingCenter.Send(this, "allowLandScapePortrait");
            base.OnAppearing();
        }
        //during page close setting back to portrait
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (Settings.ModuleName == "Completions")
                MessagingCenter.Send(this, "preventPortrait");
            else
                MessagingCenter.Send(this, "allowLandScapePortrait");
        }
    }
}
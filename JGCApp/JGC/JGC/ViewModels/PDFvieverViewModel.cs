using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.Common.Extentions;
using JGC.Models;
using Plugin.Connectivity;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.UserControls.PopupControls;
using Newtonsoft.Json;
using JGC.ViewModels.Work_Pack; 

namespace JGC.ViewModels
{
    

    public class PDFvieverViewModel : BaseViewModel
    {

        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
    


        #region fields      
       
        // private T_UserProject CurrentProject;
        PageHelper _pageHelper = CheckValidLogin._pageHelper;
        #endregion

        private string pdfUrl;
        public string PdfUrl
        {
            get { return pdfUrl; }
            set
            {
                SetProperty(ref pdfUrl, value);
            }
        }


        #region Delegate Commands   

        public ICommand CloseCommand
        {
            get
            {
                return new Command(OnClickCloseButton);
            }
        }

        private async void OnClickCloseButton()
        {
            if (Settings.ModuleName == "JobSetting")
                await navigationService.GoBackToRootAsync();
            else
                await navigationService.GoBackAsync();
           
        }
        #endregion

        public PDFvieverViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin
          
          ) : base(_navigationService, _httpHelper, _checkValidLogin)
        {

            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
        }

   
      
        #region Public
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
           


            base.OnNavigatedTo(parameters);
            if (parameters.Count == 0)
            {
                return;
            }

            if (parameters.Count > 1 )
            {
                PdfUriModel  PdfUri  = (PdfUriModel)parameters["uri"];
                PdfUrl = PdfUri.PdfUriPath;
            }
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {

        }
        #endregion

        #region INotifyPropertyChanged implementation       
        private void OnPropertyChanged(string property)
        {


            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;


        #endregion





    }
}

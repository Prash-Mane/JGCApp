using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.Models;
using JGC.Common.Extentions;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System;

namespace JGC.ViewModels.E_Test_Package
{


    public class PackageDetailViewModel : BaseViewModel
    {

        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;


        #region Properties
        private T_ETestPackages ETPSelected;
        public T_ETestPackages SelectedETP
        {
            get { return ETPSelected; }
            set { ETPSelected = value; RaisePropertyChanged(); }
        }
        private string selectedETPTitle;
        public string SelectedETPTitle
        {
            get { return selectedETPTitle; }
            set { selectedETPTitle = value; RaisePropertyChanged(); }
        }
        #endregion
        #region Delegate Commands   

        #region Delegate Commands  
        public ICommand BtnCommand
        {
            get
            {
                return new Command<string>(OnClickButtonAsync);
            }
        }
        #endregion

        private async void OnClickButtonAsync(string param)
        {
            if (param == "Punchoverview")
            {
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add(NavigationParametersConstants.SelectedETP, SelectedETP);
                navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
                await navigationService.NavigateAsync<PunchOverviewViewModel>(navigationParameters);
            }
            else if (param == "PunchView")
            {
                await navigationService.NavigateAsync<PunchViewModel>();
            }
            else if (param == "TestRecord")
            {
                await navigationService.NavigateAsync<TestRecordViewModel>();
            }
            else if (param == "ControlLog")
            {
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add(NavigationParametersConstants.SelectedETP, SelectedETP);
                navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
                await navigationService.NavigateAsync<ControlLogViewModel>(navigationParameters);
            }
            else if (param == "Pandid")
            {
                await navigationService.NavigateAsync<PandidViewModel>();
            }
            else if (param == "DrainRecord")
            {
                await navigationService.NavigateAsync<DrainRecordViewModel>();
            }
        }
  

        #endregion


        public PackageDetailViewModel(
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
            _userDialogs.HideLoading();
          
            PageHeaderText = "E-Test package Details";
        }
        #region Private
      
        private async void OnBackPressed()
        {
            CheckValidLogin._pageHelper = new PageHelper();
        }
        #endregion
        #region Public
       
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            // CheckValidLogin._pageHelper = new PageHelper();
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.Count == 0)
            {
                return;
            }

            if (parameters.Count > 1 && parameters.ContainsKey(NavigationParametersConstants.SelectedETP))
            {
                ETPSelected = (T_ETestPackages)parameters[NavigationParametersConstants.SelectedETP];
                SelectedETPTitle = ETPSelected.TestPackage;
            }
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        #endregion
    }
}

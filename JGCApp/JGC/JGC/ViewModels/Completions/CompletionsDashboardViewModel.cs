using Acr.UserDialogs;
using JGC.Common.Interfaces;
using JGC.Views.Completions;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.Common.Extentions;
using System.Diagnostics;

namespace JGC.ViewModels.Completions
{
    public class CompletionsDashboardViewModel : BaseViewModel
    {

        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;

        #region Delegate Commands   
        public ICommand TapedCommand
        {
            get
            {
                return new Command<string>(OnTapedAsync);
            }
        }
        #endregion
        public CompletionsDashboardViewModel(INavigationService _navigationService,
           IUserDialogs _userDialogs,
           IHttpHelper _httpHelper,
           ICheckValidLogin _checkValidLogin) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
        }

        [System.Obsolete]
        private async void OnTapedAsync(string param)
        {
           if (!App.IsBusy)
            {
                App.IsBusy = true;
                if (param == "TagRegister")
                    await navigationService.NavigateAsync<TagRegisterViewModel>();
                else if (param == "Sync")
                    await PopupNavigation.PushAsync(new SyncPage());
                else if (param == "PunchList")
                    await navigationService.NavigateAsync<CompletionPunchListViewModel>();
                else if (param == "Drawings")
                    await navigationService.NavigateAsync<CompletionDrawingViewModel>();
                else if (param == "handover")
                    await navigationService.NavigateAsync<HandoverViewModel>();
                else if (param == "TestPacks")
                    await navigationService.NavigateAsync<CompletionTestPackViewModel>();
                else if (param == "ITR")
                await navigationService.NavigateAsync<ITRViewModel>();
                else if (param == "Setting")                    
                    await navigationService.NavigateAsync<CompletionSettingViewModel>();
            }
            //await _userDialogs.AlertAsync("This functionality under Development", "Alert", "OK");

        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            App.IsBusy = false;
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }

    }
}


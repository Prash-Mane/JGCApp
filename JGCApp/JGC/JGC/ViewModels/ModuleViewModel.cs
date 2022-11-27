using Acr.UserDialogs;
using JGC.Common.Interfaces;
using JGC.Models;
using Prism.Navigation;
using JGC.Common.Extentions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows.Input;
using JGC.Views;
using JGC.ViewModels.E_Test_Package;
using JGC.Common.Helpers;
using JGC.ViewModels.E_Reporter;
using JGC.Common.Constants;
using JGC.DataBase.DataTables.ModsCore;
using Newtonsoft.Json;
using JGC.DataBase;
using System.Linq;
using JGC.ViewModels.Completions;
namespace JGC.ViewModels
{
    public class ModuleViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;

        private readonly IRepository<T_UserControl> _UserControlRepository;
        private readonly IRepository<T_UserProjects> _UserProjectsRepository;
        private T_UserControl CurrentUser = new T_UserControl();
        private List<T_UserProjects> UserProjectList = new List<T_UserProjects>();


        #region Properties
        private LoginModel _loginModel;
        public LoginModel LoginModel
        {
            get { return _loginModel; }
            set
            {
                _loginModel = value; RaisePropertyChanged();
                //   GetProjectListData();
            }
        }
        private bool isVisibleCompletion;
        public bool IsVisibleCompletion
        {
            get { return isVisibleCompletion; }
            set
            {
                isVisibleCompletion = value; RaisePropertyChanged();
                //   GetProjectListData();
            }
        }
        #endregion

        #region Delegate Commands  
        public ICommand BtnCommand
        {
            get
            {
                return new Command<string>(OnClickButton);
            }
        }
        #endregion

        public ModuleViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_UserControl> _UserControlRepository,
            IRepository<T_UserProjects> _UserProjectsRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            Settings.IsStop = 0;
            _userDialogs.HideLoading();

            if (Settings.Server_Url == "https://jgctest.vmlive.net/WebServiceCompletions/API/" || Settings.Server_Url == "https://yocdemo.vmlive.net/WebServiceCompletions/API/" || 
               Settings.Server_Url == "https://harmony.vmlive.net/WebServiceCompletions/API/" || Settings.Server_Url == "http://harmonycomp.vmlive.net/WebServiceCompletions/API")
                IsVisibleCompletion = true;
            else
                IsVisibleCompletion = false;
        }
        #region Private       
       
        private async void OnClickButton(string param)
        {
            if (param == null || IsRunningTasks)
            {
                return;
            }

            IsRunningTasks = true;
            if (param == "EReporter" || param == "TestPackage" || param == "JobSetting") 
            {
                if (Settings.IsMODSApp)
                {
                    Settings.ModuleName = param;
                    if (param == "TestPackage")
                        Settings.DownloadParam = "";
                    await navigationService.NavigateAsync<ProjectViewModel>();
                    Settings.Server_Url = Settings.Server_UrlForConstructionModule;
                }
                else
                    await _userDialogs.AlertAsync("User doesn’t have access to this project ", "", "OK");

            }
            else if (param == "Completions")
            {
                if (Settings.IsCompletionApp)
                {
                    Settings.ModuleName = param;
                    //  MessagingCenter.Send(this, "preventPortrait");
                    await navigationService.NavigateAsync<CompletionProjectViewModel>();
                }
                else
                    await _userDialogs.AlertAsync("User doesn’t have access to this project ", "", "OK");
            }
            else
            {
                await _userDialogs.AlertAsync("Development under progress", "", "OK");
            }
            
            IsRunningTasks = false;
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
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        #endregion
    }
}

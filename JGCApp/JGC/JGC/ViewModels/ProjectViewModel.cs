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
using JGC.Views;
using JGC.ViewModels.E_Test_Package;
using JGC.ViewModels.Work_Pack;
using Newtonsoft.Json;
using Plugin.Connectivity;

namespace JGC.ViewModels
{
    public class ProjectViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_AdminPunchCategories> _adminPunchCategoriesRepository;
        private readonly IRepository<T_UserDetails> _UserDetailsRepository;



        #region Properties
        private T_UserProject selectedProject;
        public T_UserProject SelectedProject
        {
            get { return selectedProject; }
            set
            {
                if (SetProperty(ref selectedProject, value))
                {
                    NavigateToEReportSelectionPage(selectedProject);
                }
            }
        }
        private List<T_UserProject> userProjects;
        public List<T_UserProject> UserProjects
        {
            get => userProjects;
            set
            {
                SetProperty(ref userProjects, value);
                RaisePropertyChanged("UserProjectList");
            }
        }
        #endregion
        public ProjectViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_UserProject> _userProjectRepository,
             IRepository<T_UserDetails> _UserDetailsRepository,
        IRepository<T_AdminPunchCategories> _adminPunchCategoriesRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._userProjectRepository = _userProjectRepository;
            this._checkValidLogin = _checkValidLogin;
            this._adminPunchCategoriesRepository = _adminPunchCategoriesRepository;
            this._userDialogs = _userDialogs;
            this._UserDetailsRepository = _UserDetailsRepository;
            GetProjectListData();
            PageHeaderText = "Project List";
        }
        #region Private
        private async Task GetProjectListData()
        {
            var UserProjectList = await _userProjectRepository.GetAsync();
            if (UserProjectList.Count > 0)
                UserProjects = UserProjectList.Where(p => p.User_ID == Settings.UserID).ToList();

            _userDialogs.HideLoading();
        }
        private async void OnBackPressed()
        {
            CheckValidLogin._pageHelper = new PageHelper();
        }
        #endregion
        #region Public
        public async void NavigateToEReportSelectionPage(T_UserProject Project)
        {
            if (Project == null || IsRunningTasks)
            {
                return;
            }

            IsRunningTasks = true;
            Settings.ProjectID = Project.Project_ID;
            Settings.ModelName = Project.ModelName;
            Settings.JobCode = Project.JobCode;
            Settings.Report = Settings.ModuleName;

            if (CrossConnectivity.Current.IsConnected) 
            {
                _userDialogs.ShowLoading("Syncing User Accounts");
                //offline functionality 
                await Task.Run(() =>  _UserDetailsRepository.DeleteAll());
                await Task.Run(() => _userProjectRepository.DeleteAll());
                string JsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetAllUserAccounts(), Settings.AccessToken);
                var CurrentUser = JsonConvert.DeserializeObject<List<T_UserDetails>>(JsonString);
                if (CurrentUser != null)
                    CurrentUser.ForEach( async x => 
                    { if (x.Error == null) 
                        {
                            await Task.Run(() => _UserDetailsRepository.InsertOrReplaceAsync(x));
                            await Task.Run(() => _userProjectRepository.InsertOrReplaceAsync(x.UserProjects));
                        }
                      
                    });
              
                await Task.Run(() => _userDialogs.AlertAsync("User accounts have successfully synced"));
            }
            else
            {
                _userDialogs.ShowLoading("Loading...");
            }


            var navigationParameters = new NavigationParameters();
            navigationParameters.Add(NavigationParametersConstants.SelectedProjectParameter, Project);
            //navigationParameters.Add(NavigationParametersConstants.SelectedProjectParameter, Project.User_ID);
            navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);


            if (Settings.ModuleName == "EReporter") //await navigationService.NavigateFromMenuAsync(typeof(EReportSelectionViewModel), navigationParameters);    
                _ = navigationService.NavigateAsync<EReportSelectionViewModel>(navigationParameters);
            else if (Settings.ModuleName == "TestPackage")
                _ = navigationService.NavigateAsync<ETestPackageVewModel>(navigationParameters);
            else if (Settings.ModuleName == "JobSetting")
                _ = navigationService.NavigateAsync<IWPSelectionViewModel>(navigationParameters);
                
            IsRunningTasks = false;
        }
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

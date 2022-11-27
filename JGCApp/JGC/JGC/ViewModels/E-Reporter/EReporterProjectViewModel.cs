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
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Plugin.Connectivity;
using Newtonsoft.Json;

namespace JGC.ViewModels.E_Reporter
{
    public class EReporterProjectViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_AdminPunchCategories> _adminPunchCategoriesRepository;
        public static DWRHelper _DWRHelper = new DWRHelper();
        private readonly IRepository<T_UserDetails> _UserDetailsRepository;


        #region Properties
        private UserProject selectedProject;
        public UserProject SelectedProject
        {
            get { return selectedProject; }
            set
            {
                if (SetProperty(ref selectedProject, value))
                {
                    ChangedColors();
                }
            }
        }
        private ObservableCollection<UserProject> userProjects;
        public ObservableCollection<UserProject> UserProjects
        {
            get => userProjects;
            set
            {
                SetProperty(ref userProjects, value);
                RaisePropertyChanged("UserProjectList");
            }
        }
        #endregion

        #region Delegate Commands 
        public ICommand ClickOnNext
        {
            get
            {
                return new Command(NextButton);
            }
        }
        private async void NextButton()
        {
            if (SelectedProject != null)
            {
                Settings.ProjectID = SelectedProject.Project_ID;
                Settings.ModelName = SelectedProject.ModelName;
                Settings.JobCode = SelectedProject.JobCode;
                Settings.Report = Settings.ModuleName;
                T_UserProject project = new T_UserProject
                {
                    Project_ID = SelectedProject.Project_ID,
                    User_ID = SelectedProject.User_ID,
                    ProjectName = SelectedProject.ProjectName,
                    ModelName = SelectedProject.ModelName,
                    JobCode = SelectedProject.JobCode,
                    TimeZone = SelectedProject.TimeZone,
                    WebService = SelectedProject.WebService,
                    LocalWebService = SelectedProject.LocalWebService,
                };

                if (CrossConnectivity.Current.IsConnected)
                {
                    _userDialogs.ShowLoading("Syncing User Accounts");
                    //offline functionality 
                    await Task.Run(() => _UserDetailsRepository.DeleteAll());
                    await Task.Run(() => _userProjectRepository.DeleteAll());
                    string JsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetAllUserAccounts(), Settings.AccessToken);
                    var CurrentUser = JsonConvert.DeserializeObject<List<T_UserDetails>>(JsonString);
                    if (CurrentUser != null)
                        CurrentUser.ForEach(async x =>
                        {
                            if (x.Error == null)
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
                navigationParameters.Add(NavigationParametersConstants.SelectedProjectParameter, project);
                //navigationParameters.Add(NavigationParametersConstants.SelectedProjectParameter, Project.User_ID);
                navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
                await navigationService.NavigateFromMenuAsync(typeof(EReportSelectionViewModel), navigationParameters);
            }
        }
        #endregion
        public EReporterProjectViewModel(
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
            Settings.DownloadParam = "";
            DWRHelper.SelectedDWR = null;
            DWRHelper.DWRTargetType = typeof(EReporterProjectViewModel);
            GetProjectListData();
            PageHeaderText = "Project List";

        }
        #region Private
        private async Task GetProjectListData()
        {
            var UserProjectList = await _userProjectRepository.GetAsync(p => p.User_ID == Settings.UserID);
            //if (UserProjectList.Count > 0)
            //  var UserProjectsdata = UserProjectList.Where(p => p.User_ID == Settings.UserID).ToList();
            List<UserProject> UserProl = new List<UserProject>();
            foreach (T_UserProject UP in UserProjectList)
            {
                UserProject list = new UserProject()
                {
                    Project_ID = UP.Project_ID,
                    User_ID = UP.User_ID,
                    ProjectName = UP.ProjectName,
                    ModelName = UP.ModelName,
                    JobCode = UP.JobCode,
                    TimeZone = UP.TimeZone,
                    WebService = UP.WebService,
                    LocalWebService = UP.LocalWebService,
                    SelectionBG = "White",
                    SelectionText = "Black"
                };
                UserProl.Add(list);
                UserProjects = new ObservableCollection<UserProject>(UserProl);
            }
            _userDialogs.HideLoading();
        }
        private async void OnBackPressed()
        {
            CheckValidLogin._pageHelper = new PageHelper();
        }
        private async void ChangedColors()
        {
            var list = UserProjects.ToList();
            list.ForEach(i => { i.SelectionBG = "White";i.SelectionText = "Black"; });
            foreach(UserProject item in list.Where(i => i.Project_ID == SelectedProject.Project_ID))
            {
                item.SelectionBG = "#FB1610";
                item.SelectionText = "White";
            }
            UserProjects = new ObservableCollection<UserProject>(list);
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

    public class UserProject
    {
        public int Project_ID { get; set; }
        public int User_ID { get; set; }
        public string ProjectName { get; set; }
        public string ModelName { get; set; }
        public string JobCode { get; set; }
        public string TimeZone { get; set; }
        public string WebService { get; set; }
        public string LocalWebService { get; set; }
        public string SelectionBG { get; set; }
        public string SelectionText { get; set; }
    }
}

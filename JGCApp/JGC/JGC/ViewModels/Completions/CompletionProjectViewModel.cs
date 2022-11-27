using Acr.UserDialogs;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables.ModsCore;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.Common.Extentions;
using Plugin.Connectivity;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JGC.DataBase.DataTables;
using JGC.Common.Constants;
using JGC.DataBase.DataTables.Completions;

namespace JGC.ViewModels.Completions
{
    public class CompletionProjectViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_UserProjects> _UserProjectsRepository;
        private readonly IRepository<T_CompletionsUsers> _CompletionsUserRepository;
        #region Properties   
        private T_UserProjects selectedProject;
        public T_UserProjects SelectedProject
        {
            get { return selectedProject; }
            set
            {
                if (SetProperty(ref selectedProject, value))
                {
                    // NavigateToDashBoard();
                }
            }
        }

        //private async void NavigateToDashBoard()
        //{
        //    await navigationService.NavigateAsync<CompletionProjectViewModel>();
        //}

        private List<T_UserProjects> userProjects;
        public List<T_UserProjects> UserProjects
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
        public ICommand BtnCommand
        {
            get
            {
                return new Command<string>(OnClickButtonAsync);
            }
        }
        
        #endregion
        public CompletionProjectViewModel(INavigationService _navigationService,
         IUserDialogs _userDialogs,
         IHttpHelper _httpHelper,
         IRepository<T_UserProjects> _UserProjectsRepository,
         IRepository<T_CompletionsUsers> _CompletionsUserRepository,
        ICheckValidLogin _checkValidLogin) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._UserProjectsRepository = _UserProjectsRepository;
            this._CompletionsUserRepository = _CompletionsUserRepository;
            LoadProjectListAsync();
        }
        private async void LoadProjectListAsync()
        {

            var Result = await _UserProjectsRepository.GetAsync();
            if (Result.Any())
                UserProjects = Result.Where(x => System.Convert.ToInt32(x.User_ID) == Settings.CompletionUserID && !x.ProjectName.StartsWith("yard: ")).ToList();

        }
        private async void OnClickButtonAsync(string obj)
        {
            if (SelectedProject != null)
            {
                Settings.ModelName = SelectedProject.ModelName;
                Settings.ProjectName = selectedProject.ProjectName;
                //Settings.ProjectID = Convert.ToInt32(selectedProject.ID);
                Settings.ModelID = Convert.ToInt32(selectedProject.Project_ID);
                Settings.ProjectID = Convert.ToInt32(selectedProject.Project_ID);
                Settings.JobCode = SelectedProject.JobCode;//"0-7723-2";
                SyncUserforIssuerOwner();
                await navigationService.NavigateAsync<CompletionsDashboardViewModel>();
            }
            else
                _userDialogs.AlertAsync("Please select project", " ", "OK");
        }
        private void SyncUserforIssuerOwner()
        {
            try
            {
                _userDialogs.ShowLoading("Syncing User Accounts");
                if (CrossConnectivity.Current.IsConnected)
                {                    
                    //offline functionality 
                     Task.Run(() => _CompletionsUserRepository.DeleteAll());
                    string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.Url_GetCompletionsUsers(Settings.CurrentDB), Settings.CompletionAccessToken);
                    var CurrentUser = JsonConvert.DeserializeObject<List<T_CompletionsUsers>>(JsonString);
                    if (CurrentUser != null)
                        CurrentUser.ForEach(async x =>
                        {
                            await Task.Run(() => _CompletionsUserRepository.InsertOrReplaceAsync(x));
                        });
                    DependencyService.Get<IToastMessage>().LongAlert("User accounts have successfully synced");
                    //await Task.Run(() => _userDialogs.AlertAsync("User accounts have successfully synced"));
                }
                else
                {
                    _userDialogs.ShowLoading("Loading...");
                }
            }
            catch(Exception e)
            {

            }
            _userDialogs.HideLoading();
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
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


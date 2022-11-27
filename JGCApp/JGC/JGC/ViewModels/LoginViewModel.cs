using Acr.UserDialogs;
using JGC.ViewModels;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.Common.Extentions;
using Plugin.Connectivity;
using System.Threading.Tasks;
using JGC.Models;
using JGC.Common.Constants;
using JGC.Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using JGC.Common.Helpers;
using Rg.Plugins.Popup.Services;
using JGC.UserControls.PopupControls;
using System.Linq;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using System.Collections;
using System.Collections.Generic;
using JGC.DataBase.DataTables.ModsCore;
using JGC.DataBase.DataTables.Completions;
using JGC.Models.Completions;
using System.Net;
using System.Threading;
using System.IO;

namespace JGC.ViewModel
{
  public  class LoginViewModel: BaseViewModel 
    {  
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_UnitID> _unitIDRepository;
        private readonly IRepository<T_UserControl> _UserControlRepository;
        private readonly IRepository<T_UserProjects> _UserProjectsRepository;
        private readonly IRepository<T_PunchComponent> _PunchComponentRepository;
        private readonly IRepository<T_PunchSystem> _PunchSystemRepository;
        private readonly IRepository<T_PunchPCWBS> _PunchPCWBSRepository;
        private readonly IRepository<T_PunchFWBS> _PunchFWBSRepository;
        private readonly IRepository<T_SectionCode> _SectionCodeRepository;
        private readonly IRepository<T_CompanyCategoryCode> _CompanyCategoryCodeRepository;
        private readonly IRepository<T_CompletionSystems> _CompletionSystemsRepository;

        private T_UserDetails CurrentUser = new T_UserDetails();
        PageHelper _pageHelper = CheckValidLogin._pageHelper;
        CompletionPageHelper _CompletionpageHelper = CheckValidLogin._CompletionpageHelper;
        private List<T_UserProject> UserProjects = new List<T_UserProject>();
        private T_UserControl CurrentUserControl = new T_UserControl();
        private List<T_UserProjects> UserProjectList = new List<T_UserProjects>();
       // bool ModsApp, CompletionApp;
        private string _testonline ;
        private static string VMToken1 = "jv5980fj90ewFdr234rfTfsdsF53gssstfsdn4387ns9FGfd3f";
        private static string VMToken2 = "jvr890eJ78jjthjghvxcv3ffgSsfd233";
        private static string VMToken3 = "K675dVsgetSgeawJdHk96G54xgdr";

        #region Properties   
        private string version;
        public string Version
        {
            get { return version; }
            set { SetProperty(ref version, value); }
        }
      
        public string TestOnline
        {
            get { return _testonline; }
            set { SetProperty(ref _testonline, value); }
        }
        private List<string> serverList;
        public List<string> ServerList
        {
            get { return serverList; }
            set
            {
                SetProperty(ref serverList, value);
            }
        }
        //private string selectedServer;
        //public string SelectedServer
        //{
        //    get { return selectedServer; }
        //    set
        //    {
        //        if (SetProperty(ref selectedServer, value))
        //        {
        //            SelectServer(selectedServer);
        //            OnPropertyChanged();
        //        }
        //    }
        //}
        private List<string> CompletionserverList;
        public List<string> CompletionServerList
        {
            get { return CompletionserverList; }
            set
            {
                SetProperty(ref CompletionserverList, value);
            }
        }
        private string completionselectedServer;
        public string CompletionSelectedServer
        {
            get { return completionselectedServer; }
            set
            {
                if (SetProperty(ref completionselectedServer, value))
                {
                    CompletionSelectServer(completionselectedServer);
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Delegate Commands   
        public ICommand LoginCommand
        {
            get
            {
                return new Command<string>(OnClickLoginButton);
            }
        }
        #endregion

        public LoginViewModel(
           INavigationService _navigationService,
           IUserDialogs _userDialogs,
           IHttpHelper _httpHelper,
           ICheckValidLogin _checkValidLogin,
           IRepository<T_UserDetails> _userDetailsRepository,
           IRepository<T_UnitID> unitIDRepository,
           IRepository<T_UserProject> _userProjectRepository,
           IRepository<T_UserControl> _UserControlRepository,
           IRepository<T_UserProjects> _UserProjects,
           IRepository<T_UserProjects> _UserProjectsRepository,
           IRepository<T_PunchComponent> _PunchComponentRepository,
           IRepository<T_PunchSystem> _PunchSystemRepository,
           IRepository<T_PunchPCWBS> _PunchPCWBSRepository,
           IRepository<T_PunchFWBS> _PunchFWBSRepository,
           IRepository<T_SectionCode> _SectionCodeRepository,
           IRepository<T_CompanyCategoryCode> _CompanyCategoryCodeRepository,
           IRepository<T_CompletionSystems> _CompletionSystemsRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._userDetailsRepository = _userDetailsRepository;
            this._userProjectRepository = _userProjectRepository;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._unitIDRepository = unitIDRepository;
            this._UserControlRepository = _UserControlRepository;
            this._UserProjectsRepository = _UserProjectsRepository;
            this._PunchComponentRepository = _PunchComponentRepository;
            this._PunchSystemRepository = _PunchSystemRepository;
            this._PunchPCWBSRepository = _PunchPCWBSRepository;
            this._PunchFWBSRepository = _PunchFWBSRepository;
            this._SectionCodeRepository = _SectionCodeRepository;
            this._CompanyCategoryCodeRepository = _CompanyCategoryCodeRepository;
            this._CompletionSystemsRepository = _CompletionSystemsRepository;

            //LoginModel = new LoginModel();
            CompletionLoginModel = new LoginModel();
            //ServerList = new List<string> { "https://jgctest.vmlive.net", "https://mru.vmlive.net", "https://hrm3pj.vmlive.net", "https://yocdemo.vmlive.net","https://harmony.vmlive.net", "https://lngc.vmlive.net" };
         
            CompletionServerList = new List<string> { "https://jgctest.vmlive.net", "https://yocdemo.vmlive.net", "https://rovuma.vmlive.net", "https://harmony.vmlive.net", "https://harmonycomp.vmlive.net" , "https://lngc.vmlive.net", "https://hmds.vmlive.net", "https://ghawar.vmlive.net" };
            Settings.IsCompletionApp = Settings.IsMODSApp = false;
            Settings.Report = null;
            Version = Settings.AppCurrentVersion;
            _pageHelper.TokenExpiry = DateTime.Today.AddDays(-1);
            _CompletionpageHelper.CompletionTokenExpiry = DateTime.Today.AddDays(-1);

            if (Settings.IsStop == 1 && Settings.UserID > 0)
                OnClickLoginButton("withoutclick");
        }

        #region Private  

        private async Task NavigateToErrorPage()
        {           
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (PopupNavigation.PopupStack.Any())
                    await PopupNavigation.PopAllAsync(false);

                if (Device.RuntimePlatform == Device.iOS) //ios is not able to display page if there's alert visible
                {
                    var helper = DependencyService.Get<IAlertsHelper>();
                    await helper.CloseAll();
                }
                await PopupNavigation.PushAsync(new NetworkErrorPopup(), true);
            });
        }

        private async void OnClickLoginButton(string click)
        {
            try
            {
                if (click == "withclick")
                {
                    Settings.IsCompletionApp = Settings.IsMODSApp = false;
                    if (CompletionSelectedServer == "https://rovuma.vmlive.net")
                        Settings.CurrentDB = "ROVUMA_TEST";
                    else if (CompletionSelectedServer == "https://harmony.vmlive.net")
                        Settings.CurrentDB = "JGC_HARMONY";
                    else if(CompletionSelectedServer == "https://harmonycomp.vmlive.net")
                        Settings.CurrentDB = "JGC_HARMONYCOMP";
                    else if ( CompletionSelectedServer == "https://yocdemo.vmlive.net")
                        Settings.CurrentDB = "YOC_DEMO";
                    else
                        Settings.CurrentDB = "JGC_DEMO";

                    if (CompletionSelectedServer == null && string.IsNullOrEmpty(CompletionSelectedServer))
                    {
                        await _userDialogs.AlertAsync("Please select server...!", null, "OK");
                        return;
                    }
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        _userDialogs.ShowLoading("Authenticating...");

                        var sqlresult = await _unitIDRepository.GetAsync();
                        if (!sqlresult.Any())
                        {
                            var deviceid = await DependencyService.Get<IDeviceID>().GetDeviceID();
                            T_UnitID unitId = new T_UnitID { ID = 1, DeviceID = deviceid };
                            Settings.UnitID = deviceid;
                            await _unitIDRepository.InsertOrReplaceAsync(unitId);
                        }
                        else
                        {
                            Settings.UnitID = sqlresult.FirstOrDefault().DeviceID;
                        }

                        Settings.IsMODSApp = await _checkValidLogin.GetValidToken(CompletionLoginModel);
                        Settings.IsCompletionApp = await _checkValidLogin.GetValidCompletionToken(CompletionLoginModel);

                        if (Settings.IsMODSApp)
                        {
                           // Settings.Server_UrlForConstructionModule = "";//
                            //  string JsonString = ModsTools.WebServiceGet("User?UserName=" + AppSetting.Default_UserId + "&Password=" + AppSetting.Default_UserPassword, Settings.AccessToken);
                            string JsonString = ModsTools.WebServiceGet(ApiUrls.Url_login(CompletionLoginModel.UserName.TrimEnd(), CompletionLoginModel.Password.TrimEnd()), Settings.AccessToken);

                            CurrentUser = JsonConvert.DeserializeObject<T_UserDetails>(JsonString);

                            if (CurrentUser.Error != null && CurrentUser.Error != string.Empty)
                            {
                                _userDialogs.HideLoading();
                                Settings.IsMODSApp = false;
                                //await _userDialogs.AlertAsync(AppConstant.LOGIN_FAILURE, "Login", "OK");
                            }
                            else
                            {  
                                if (CurrentUser != null)
                                {
                                    await _userDetailsRepository.InsertOrReplaceAsync(CurrentUser);
                                    Settings.UserID = CurrentUser.ID;
                                    //Settings.ProjectID = CurrentUser.pr
                                    if (CurrentUser.UserProjects != null)
                                    {
                                        UserProjects = CurrentUser.UserProjects;
                                        await _userProjectRepository.DeleteAll();
                                        var ExistUserProjects = await _userProjectRepository.GetAsync();
                                        foreach (T_UserProject item in UserProjects)
                                        {
                                            //Settings.ModelName = item.ModelName;
                                            // if (!(ExistUserProjects.Select(i => i.Project_ID).Contains(item.Project_ID) && ExistUserProjects.Select(i => i.User_ID).Contains(item.User_ID)))
                                            var IsExist = ExistUserProjects.Where(i => i.Project_ID == item.Project_ID && i.User_ID == item.User_ID && i.ModelName == item.ModelName).ToList();
                                            if (!ExistUserProjects.Where(i => i.Project_ID == item.Project_ID && i.User_ID == item.User_ID && i.ModelName == item.ModelName).ToList().Any())
                                                await _userProjectRepository.InsertOrReplaceAsync(item);
                                        }
                                    }
                                }
                                //await navigationService.NavigateAsync<ModuleViewModel>();
                            }
                        }

                        if (Settings.IsCompletionApp)
                        {
                            //Completion App uers
                            string UserJsonString = ModsTools.CompletionWebServiceGet(ApiUrls.GetUser(CompletionLoginModel.UserName.TrimEnd(), CompletionLoginModel.Password.TrimEnd(), Settings.CurrentDB), Settings.CompletionAccessToken);

                            CurrentUserControl = JsonConvert.DeserializeObject<T_UserControl>(UserJsonString);

                            if (CurrentUserControl == null)
                            {
                                _userDialogs.HideLoading();
                                Settings.IsCompletionApp = false;
                               // await _userDialogs.AlertAsync(AppConstant.LOGIN_FAILURE, "Login", "OK");
                            }
                            else
                            {
                                if (CurrentUserControl != null)
                                {
                                    await _UserControlRepository.InsertOrReplaceAsync(CurrentUserControl);
                                    Settings.CompletionUserID = Convert.ToInt32(CurrentUserControl.ID);
                                    Settings.CompletionUserName = CurrentUserControl.FullName;

                                    //getUserProjects
                                    await _UserProjectsRepository.QueryAsync<T_UserProjects>("DELETE FROM [T_UserProjects]");
                                    string JsonStringForProjects = ModsTools.CompletionWebServiceGet(ApiUrls.GetUserProjects(CurrentUserControl.ID, "true", Settings.CurrentDB), Settings.CompletionAccessToken);
                                    UserProjectList = JsonConvert.DeserializeObject<List<T_UserProjects>>(JsonStringForProjects);

                                    //UserProjects = CurrentUser.UserProjects;
                                    var ExistCompletionUserProjects = await _UserProjectsRepository.GetAsync();
                                    foreach (T_UserProjects item in UserProjectList)
                                    {
                                        if (ExistCompletionUserProjects == null)
                                            await _UserProjectsRepository.InsertOrReplaceAsync(item);
                                        else if (!ExistCompletionUserProjects.Where(i => i.ID == item.ID && i.User_ID == item.User_ID).ToList().Any())
                                            await _UserProjectsRepository.InsertOrReplaceAsync(item);
                                    }

                                   BG_DownloadPunchData(UserProjectList);
                                }
                                _userDialogs.HideLoading();
                                // await navigationService.NavigateAsync<ModuleViewModel>();
                            }
                        }

                       if (Settings.IsMODSApp || Settings.IsCompletionApp)
                           await navigationService.NavigateAsync<ModuleViewModel>();
                        else
                        {
                            Settings.AccessToken = string.Empty;
                            Settings.RenewalToken = string.Empty;
                            Settings.DisplayName = string.Empty;
                            Cache.accessToken = string.Empty;
                            Settings.IsMODSApp = Settings.IsCompletionApp = false;

                            Settings.CompletionAccessToken = string.Empty;
                            Cache.accessToken = string.Empty;

                            _pageHelper.TokenExpiry = DateTime.Today.AddDays(-1);
                            _CompletionpageHelper.CompletionTokenExpiry = DateTime.Today.AddDays(-1);
                            _userDialogs.HideLoading();
                            await _userDialogs.AlertAsync(AppConstant.LOGIN_FAILURE, "Login", "OK");
                        }
                    }
                    else
                    {
                        // NavigateToErrorPage();
                        //var result = await _checkValidLogin.GetValidToken(LoginModel);
                        var userDetails = await _userDetailsRepository.GetAsync();
                        var IsExist = userDetails.Where(u => u.UserName == LoginModel.UserName && u.Password == LoginModel.Password).ToList();
                        if (IsExist.Any())
                        {
                            Settings.UserID = IsExist.Select(i => i.ID).FirstOrDefault();
                            await navigationService.NavigateAsync<ModuleViewModel>();
                        }

                        else
                            await _userDialogs.AlertAsync("Account not found, double check your details or attempt to login online", "Login Failed", "OK");
                    }
                }
                else
                {
                    Settings.IsMODSApp = await _checkValidLogin.GetValidToken(CompletionLoginModel);
                    Settings.IsCompletionApp = await _checkValidLogin.GetValidCompletionToken(CompletionLoginModel);

                    if (Settings.UserID > 0 && Settings.IsMODSApp || Settings.IsCompletionApp)
                        await navigationService.NavigateAsync<ModuleViewModel>();

                    //LoginModel.UserName = Settings.UserName; LoginModel.Password = Settings.PassWord;
                    //var result = await _checkValidLogin.GetValidToken(LoginModel);                    
                    //if (Settings.UserID > 0 && result)
                    // await navigationService.NavigateAsync<ModuleViewModel>();

                }
            }
            catch(Exception ex)
            {
                Settings.AccessToken = string.Empty;
                Settings.RenewalToken = string.Empty;
                Settings.DisplayName = string.Empty;
                Cache.accessToken = string.Empty;
                Settings.IsMODSApp = Settings.IsCompletionApp = false;
                Settings.CompletionAccessToken = string.Empty;
                Cache.accessToken = string.Empty;

                _pageHelper.TokenExpiry = DateTime.Today.AddDays(-1);
                _CompletionpageHelper.CompletionTokenExpiry = DateTime.Today.AddDays(-1);
                _userDialogs.HideLoading();
                await _userDialogs.AlertAsync(AppConstant.LOGIN_FAILURE, "Login", "OK");
            }
        }             
        
        //void SelectServer(string SelectServer)
        //{
        //    if (SelectServer == null && string.IsNullOrEmpty(SelectServer))
        //    return;
           
        //    Settings.Server_Url = SelectServer+ "/WebService/api/";
        //}
        void CompletionSelectServer(string CompletionSelectedServer)
        {
            if (CompletionSelectedServer == null && string.IsNullOrEmpty(CompletionSelectedServer))
                return;
            //if (CompletionSelectedServer == "https://rovuma.vmlive.net" || CompletionSelectedServer == "https://harmony.vmlive.net" ||
            //    CompletionSelectedServer == "https://jgctest.vmlive.net" || CompletionSelectedServer == "https://yocdemo.vmlive.net" ||
            //    CompletionSelectedServer == "https://harmonycomp.vmlive.net")
            //{
            //    Settings.Server_UrlForConstructionModule = "https://jgctest.vmlive.net/WebService/api/";
            //    Settings.Server_Url = Settings.CompletionServer_Url = "https://jgctest.vmlive.net/WebServiceCompletions/API/";

            //}
            //else
            //{
               Settings.Server_Url = Settings.CompletionServer_Url = CompletionSelectedServer + "/WebServiceCompletions/API/";
            //    Settings.Server_UrlForConstructionModule = "https://jgctest.vmlive.net/WebService/api/";
            //}
            Settings.Server_UrlForConstructionModule = CompletionSelectedServer + "/WebService/api/";
        }

        public async void BG_DownloadPunchData(List<T_UserProjects> UserProjects)
        {
            Task.Run(async () =>
            {
                try
                {
                    foreach (T_UserProjects item in UserProjects)
                    {
                        if (String.IsNullOrEmpty(item.JobCode) || String.IsNullOrEmpty(item.ModelName))
                            continue;
                        string PunchDDLJsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getPunchDropdownData(item.ModelName, item.JobCode, Settings.CurrentDB), Settings.CompletionAccessToken);
                        PunchDropDownData PunchData = JsonConvert.DeserializeObject<PunchDropDownData>(PunchDDLJsonString);

                        string SectionCodesJsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getSectionCodes(item.ModelName, Settings.CurrentDB), Settings.CompletionAccessToken);
                        //string SectionCodesJsonString = CMSWeberviceCall("https://jgctest.vmlive.net/MODSTestWebservice/API/Admin/GetSectionCodes");
                        Dictionary<string, string> SectionCodes = JsonConvert.DeserializeObject<Dictionary<string, string>>(SectionCodesJsonString);
                        List<T_SectionCode> SectionCodeList = new List<T_SectionCode>();
                        foreach (KeyValuePair<string, string> section in SectionCodes)
                        {
                            SectionCodeList.Add(new T_SectionCode { SectionCode = section.Key, Description = section.Value, ModelName = item.ModelName });
                        }
                        await _SectionCodeRepository.QueryAsync("DELETE FROM [T_SectionCode] WHERE [ModelName]= '" + item.ModelName + "'");
                        await _SectionCodeRepository.InsertOrReplaceAsync(SectionCodeList);

                        string CompanyCategoryCodesJsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getCompanyCategoryCodes(item.ModelName, Settings.CurrentDB), Settings.CompletionAccessToken);
                        //string CompanyCategoryCodesJsonString = CMSWeberviceCall("https://jgctest.vmlive.net/MODSTestWebservice/API/Admin/GetCompanyCategoryCodes");
                        Dictionary<string, string> CompanyCategoryCodes = JsonConvert.DeserializeObject<Dictionary<string, string>>(CompanyCategoryCodesJsonString);
                        List<T_CompanyCategoryCode> CompanyCategoryCodeList = new List<T_CompanyCategoryCode>();
                        foreach (KeyValuePair<string, string> Category in CompanyCategoryCodes)
                        {
                            CompanyCategoryCodeList.Add(new T_CompanyCategoryCode { CompanyCategoryCode = Category.Key, Description = Category.Value, ModelName = item.ModelName });
                        }
                        await _CompanyCategoryCodeRepository.QueryAsync("DELETE FROM [T_CompanyCategoryCode] WHERE [ModelName]= '" + item.ModelName + "'");
                        await _CompanyCategoryCodeRepository.InsertOrReplaceAsync(CompanyCategoryCodeList);

                        if (PunchData != null)
                        {
                            if (PunchData.punchComponent != null)
                            {
                                PunchData.punchComponent.ForEach(i => { i.Jobcode = item.JobCode; i.ModelName = item.ModelName; });
                                await _PunchComponentRepository.QueryAsync<T_PunchComponent>("DELETE FROM [T_PunchComponent] WHERE [Jobcode] = '" + item.JobCode + "' AND [ModelName]= '" + item.ModelName + "'");
                                await _PunchComponentRepository.InsertOrReplaceAsync(PunchData.punchComponent);
                            }
                            if (PunchData.system != null)
                            {

                                List<T_PunchSystem> PunchSystemData = PunchData.system.Select(i => new T_PunchSystem { SystemKey = i.Key, SystemValue = i.Value, Jobcode = item.JobCode, ModelName = item.ModelName }).ToList();
                                await _PunchSystemRepository.QueryAsync<T_PunchSystem>(@"DELETE FROM [T_PunchSystem] WHERE [Jobcode] = '" + item.JobCode + "' AND [ModelName]= '" + item.ModelName + "'");
                                await _PunchSystemRepository.InsertOrReplaceAsync(PunchSystemData);
                            }
                            if (PunchData.pcwbs != null)
                            {
                                List<T_PunchPCWBS> PunchPCWBSData = PunchData.pcwbs.Select(i => new T_PunchPCWBS { pcwbs = i, Jobcode = item.JobCode, ModelName = item.ModelName }).ToList();
                                await _PunchPCWBSRepository.QueryAsync<T_PunchPCWBS>(@"DELETE FROM [T_PunchPCWBS] WHERE [Jobcode] ='" + item.JobCode + "' AND [ModelName]= '" + item.ModelName + "'");
                                await _PunchPCWBSRepository.InsertOrReplaceAsync(PunchPCWBSData);
                            }
                            if (PunchData.fwbs != null)
                            {
                                List<T_PunchFWBS> PunchFWBSData = PunchData.fwbs.Select(i => new T_PunchFWBS { fwbs = i, Jobcode = item.JobCode, ModelName = item.ModelName }).ToList();
                                await _PunchFWBSRepository.QueryAsync<T_PunchFWBS>(@"DELETE FROM [T_PunchFWBS] WHERE [Jobcode] = '" + item.JobCode + "' AND [ModelName]= '" + item.ModelName + "'");
                                await _PunchFWBSRepository.InsertOrReplaceAsync(PunchFWBSData);
                            }
                        }

                        await _CompletionSystemsRepository.QueryAsync("DELETE FROM [T_CompletionSystems] WHERE [modelName]= '" + item.ModelName + "'");
                        var dd = await _CompletionSystemsRepository.GetAsync();
                        string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getSystems(item.ModelName, Settings.CurrentDB), Settings.CompletionAccessToken);
                        var SystemData = JsonConvert.DeserializeObject<List<T_CompletionSystems>>(JsonString);
                        await _CompletionSystemsRepository.InsertOrReplaceAsync(SystemData);
                    }
                }
                catch(Exception ex)
                {
                    
                }

            }).ConfigureAwait(false);
        }
        #endregion

        #region Public
        private LoginModel _loginModel;
                public LoginModel LoginModel
                {
                    get { return _loginModel; }
                    set { _loginModel = value; RaisePropertyChanged();
                     //   GetProjectListData();
                    }
                }
                private LoginModel _completionloginModel;
                public LoginModel CompletionLoginModel
                {
                    get { return _completionloginModel; }
                    set
                    {
                        _completionloginModel = value; RaisePropertyChanged();
                    }
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
                #endregion
    }
}

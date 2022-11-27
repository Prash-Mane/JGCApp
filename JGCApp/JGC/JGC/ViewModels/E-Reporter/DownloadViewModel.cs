
using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.Models;
using JGC.UserControls.PopupControls;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.Common.Extentions;
using JGC.ViewModels.E_Test_Package;
using JGC.ViewModels.Work_Pack;
using JGC.DataBase.DataTables.WorkPack;

namespace JGC.ViewModels
{
   
    public class DownloadViewModel : BaseViewModel
    {

        protected readonly INavigationService _navigationService;
        private readonly IRepository<T_EReports> _eReport;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IDownloadService _downloadService;
        private readonly IRepository<T_ManPowerResource> _manPowerResourceRepository;

        #region fields 
        private bool isRefreshing;
        private T_UserProject userProject;
        PageHelper _pageHelper = CheckValidLogin._pageHelper;
        private bool IsBack = false;
        List<T_ETestPackages> ETestPackagesData = new List<T_ETestPackages>();
        List<T_IWP> JBS;
        #endregion

        #region Properties

        private ObservableCollection<T_EReports> eReports;
        public ObservableCollection<T_EReports> EReports
        {
            get { return eReports; }
            set { eReports = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_ETestPackages> eTestPackages;
        public ObservableCollection<T_ETestPackages> ETestPackages
        {
            get { return eTestPackages; }
            set { eTestPackages = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<string> downloadingProgressList;

        public ObservableCollection<string> DownloadingProgressList
        {
            get
            {
                return downloadingProgressList ?? (downloadingProgressList = new ObservableCollection<string>());
            }
        }
        private bool close_IsVisible { get; set; }
        public bool Close_IsVisible
        {
            get { return close_IsVisible; }
            set { close_IsVisible = value; RaisePropertyChanged(); }
        }
        private bool progressBarIsVisible { get; set; }
        public bool ProgressBarIsVisible
        {
            get { return progressBarIsVisible; }
            set { progressBarIsVisible = value; RaisePropertyChanged(); }
        }
        

        private float _progressValue;
        public float ProgressValue
        {
            get { return _progressValue; }
            set { SetProperty(ref _progressValue, value); }
        }

        private bool etp_progressBarIsVisible { get; set; }
        public bool ETP_ProgressBarIsVisible
        {
            get { return etp_progressBarIsVisible; }
            set { etp_progressBarIsVisible = value; RaisePropertyChanged(); }
        }


        private float etp_progressValue;
        public float ETP_ProgressValue
        {
            get { return etp_progressValue; }
            set { SetProperty(ref etp_progressValue, value); }
        }

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); }
        }
        //private bool _eReporterGrid { get; set; }
        //public bool EReporterGrid
        //{
        //    get { return _eReporterGrid; }
        //    set { _eReporterGrid = value; RaisePropertyChanged(); }
        //}
        private bool _eTestPackageGrid { get; set; }
        public bool ETestPackageGrid
        {
            get { return _eTestPackageGrid; }
            set { _eTestPackageGrid = value; RaisePropertyChanged(); }
        }
        
        private string downloadingFor;
        public string DownloadingFor
        {
            get { return downloadingFor; }
            set
            {
                if (SetProperty(ref downloadingFor, value))
                {
                    //GetETestPackagesListData(false);
                    OnPropertyChanged();
                }
            }
        }
        public ICommand RefreshCommand { get; set; }
        private string pcwbs;
        public string PCWBS
        {
            get { return pcwbs; }
            set
            {
                if (SetProperty(ref pcwbs, value))
                {
                    //GetETestPackagesListData(false);
                    OnPropertyChanged();
                }
            }
        }

        private string testPackage;
        public string TestPackage
        {
            get { return testPackage; }
            set
            {
                if (SetProperty(ref testPackage, value))
                {
                    //GetETestPackagesListData(false);
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> actionByList;
        public ObservableCollection<string> ActionByList
        {
            get { return actionByList; }
            set { actionByList = value; RaisePropertyChanged(); }
        }
        private string selectedActionBy;
        public string SelectedActionBy
        {
            get { return selectedActionBy; }
            set
            {
                if (SetProperty(ref selectedActionBy, value))
                {
                    //GetETestPackagesListData(false);
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> pendingActionList;
        public ObservableCollection<string> PendingActionList
        {
            get { return pendingActionList; }
            set { pendingActionList = value; RaisePropertyChanged(); }
        }
        private string selectedPendingAction;
        public string SelectedPendingAction
        {
            get { return selectedPendingAction; }
            set
            {
                if (SetProperty(ref selectedPendingAction, value))
                {
                    //GetETestPackagesListData(false);
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> priorityList;
        public ObservableCollection<string> PriorityList
        {
            get { return priorityList; }
            set { priorityList = value; RaisePropertyChanged(); }
        }
        private string selectedPriority;
        public string SelectedPriority
        {
            get { return selectedPriority; }
            set
            {
                if (SetProperty(ref selectedPriority, value))
                {
                    //GetETestPackagesListData(false);
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> subContractorList;
        public ObservableCollection<string> SubContractorList
        {
            get { return subContractorList; }
            set { subContractorList = value; RaisePropertyChanged(); }
        }
        private string selectedSubContractor;
        public string SelectedSubContractor
        {
            get { return selectedSubContractor; }
            set
            {
                if (SetProperty(ref selectedSubContractor, value))
                {
                    //GetETestPackagesListData(false);
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<string> testMethodList;
        public ObservableCollection<string> TestMethodList
        {
            get { return testMethodList; }
            set { testMethodList = value; RaisePropertyChanged(); }
        }
        private string selectedTestMethod;
        public string SelectedTestMethod
        {
            get { return selectedTestMethod; }
            set
            {
                if (SetProperty(ref selectedTestMethod, value))
                {
                    //GetETestPackagesListData(false);
                    OnPropertyChanged();
                }
            }
        }
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }
        private bool _noDataIsVisible;
        public bool NoDataIsVisible
        {
            get { return _noDataIsVisible; }
            set { SetProperty(ref _noDataIsVisible, value); }
        }
        //new code

        //Properties
        private bool eReporterGrid { get; set; }
        private bool eReporterVisibleList { get; set; }
        private bool etpVisibleList { get; set; }
        private bool isVisibleDownlaodProgress { get; set; }

        public bool EReporterGrid
        {
            get { return eReporterGrid; }
            set { eReporterGrid = value; RaisePropertyChanged(); }
        }

        public bool EReporterVisibleList
        {
            get { return eReporterVisibleList; }
            set { eReporterVisibleList = value; RaisePropertyChanged(); }
        }
        public bool ETPVisibleList
        {
            get { return etpVisibleList; }
            set { etpVisibleList = value; RaisePropertyChanged(); }
        }

        public bool IsVisibleDownlaodProgress
        {
            get { return isVisibleDownlaodProgress; }
            set { isVisibleDownlaodProgress = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_EReports> downloadList;

        public ObservableCollection<T_EReports> DownloadList
        {
            get { return downloadList; }
            set { downloadList = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_ETestPackages> etpdownloadList;

        public ObservableCollection<T_ETestPackages> ETPDownloadList
        {
            get { return etpdownloadList; }
            set { etpdownloadList = value; RaisePropertyChanged(); }
        }

        List<T_EReports> dList = new List<T_EReports>();
        List<T_ETestPackages> ETPList = new List<T_ETestPackages>();

        private T_EReports selectedReport;
        public T_EReports SelectedReport
        {
            get { return selectedReport; }
            set
            {
                if (SetProperty(ref selectedReport, value))
                {
                    AddToDownloadList(selectedReport);
                }
            }
        }
        private T_ETestPackages selectedETP;
        public T_ETestPackages SelectedETP
        {
            get { return selectedETP; }
            set
            {
                SetProperty(ref selectedETP, value);
            }
        }

        //Job Setting 

        private bool jobSettingGrid;
        public bool JobSettingGrid
        {
            get { return jobSettingGrid; }
            set { jobSettingGrid = value; RaisePropertyChanged(); }
        }
        
        private ObservableCollection<T_IWP> jobSetting;
        public ObservableCollection<T_IWP> JobSetting
        {
            get { return jobSetting; }
            set { jobSetting = value; RaisePropertyChanged(); }
        }
        private bool jobsettingVisibleList;
        public bool JobsettingVisibleList
        {
            get { return jobsettingVisibleList; }
            set { jobsettingVisibleList = value; RaisePropertyChanged(); }
        }
        
        private ObservableCollection<T_IWP> jobSDownloadList;
        public ObservableCollection<T_IWP> JobSDownloadList
        {
            get { return jobSDownloadList; }
            set { jobSDownloadList = value; RaisePropertyChanged(); }
        }
        
        private T_IWP selectedJobSetting;
        public T_IWP SelectedJobSetting
        {
            get { return selectedJobSetting; }
            set { selectedJobSetting = value; RaisePropertyChanged(); }
        }

        private string bgAttachments;
        public string BGAttachments
        {
            get { return bgAttachments; }
            set { bgAttachments = value; RaisePropertyChanged(); }
        }
        private string bgDrawingst;
        public string BGDrawings
        {
            get { return bgDrawingst; }
            set { bgDrawingst = value; RaisePropertyChanged(); }
        }
        private string bgPredecessors;
        public string BGPredecessors
        {
            get { return bgPredecessors; }
            set { bgPredecessors = value; RaisePropertyChanged(); }
        }
        private string bgSuccessors;
        public string BGSuccessors
        {
            get { return bgSuccessors; }
            set { bgSuccessors = value; RaisePropertyChanged(); }
        }
        
        private string bgCWPTags;
        public string BGCWPTags
        {
            get { return bgCWPTags; }
            set { bgCWPTags = value; RaisePropertyChanged(); }
        }


        private bool attachments;
        public bool Attachments
        {
            get { return attachments; }
            set { attachments = value; RaisePropertyChanged(); }
        }
        private bool drawing;
        public bool Drawings
        {
            get { return drawing; }
            set { drawing = value; RaisePropertyChanged(); }
        }
        private bool predecessors;
        public bool Predecessors
        {
            get { return predecessors; }
            set { predecessors = value; RaisePropertyChanged(); }
        }
        private bool successors;
        public bool Successors
        {
            get { return successors; }
            set { successors = value; RaisePropertyChanged(); }
        }
        private bool _CWPTags;
        public bool CWPTags
        {
            get { return _CWPTags; }
            set { _CWPTags = value; RaisePropertyChanged(); }
        }




        private string _JBSPCWBS;
        public string JBSPCWBS
        {
            get { return _JBSPCWBS; }
            set { _JBSPCWBS = value; RaisePropertyChanged(); }
        }
        private string _JBSFWBS;
        public string JBSFWBS
        {
            get { return _JBSFWBS; }
            set { _JBSFWBS = value; RaisePropertyChanged(); }
        }
        private string _JBSIWP;
        public string JBSIWP
        {
            get { return _JBSIWP; }
            set { _JBSIWP = value; RaisePropertyChanged(); }
        }
        private string _JBSDiscipline;
        public string JBSDiscipline
        {
            get { return _JBSDiscipline; }
            set { _JBSDiscipline = value; RaisePropertyChanged(); }
        }
        private string closeBtnColor;
        public string CloseBtnColor
        {
            get { return closeBtnColor; }
            set { closeBtnColor = value; RaisePropertyChanged(); }
        }
        #endregion

        #region Delegate Commands   
        public ICommand AllDownloadCommand
        {
            get
            {
                return new Command(OnClickAllDownloadButton);
            }
        }
        public ICommand CloseCommand
        {
            get
            {
                return new Command(OnClickCloseButton);
            }
        }
        public ICommand BtnCommand
        {
            get
            {
                return new Command<string>(OnClickButton);
            }
        }
        public ICommand SearchBtnCommand
        {
            get
            {
                return new Command(OnClickSearchButton);
            }
        }

        //commands
        public ICommand AddToListCommand
        {
            get
            {
                return new Command<string>(OnClickAddToListdButton);
            }
        }

        public ICommand DownloadCommand
        {
            get
            {
                return new Command<string>(OnClickDownloadButton);
            }
        }

        public ICommand ETPAddToListCommand
        {
            get
            {
                return new Command<string>(OnClickETPAddToListdButton);
            }
        }
        public ICommand ETPDownloadCommand
        {
            get
            {
                return new Command<string>(OnClickETPDownloadButton);
            }
        }
        public ICommand ETPAllDownloadCommand
        {
            get
            {
                return new Command<string>(OnClickAllETPDownloadButton);
            }
        }

        //Job Setting 
        public ICommand JobSettingOnClickCommand
        {
            get
            {
                return new Command<string>(OnClickCommand);
            }
        }
        public ICommand BGColorChangeCommand
        {
            get
            {
                return new Command<string>(OnBGColorChangeClickCommand);
            }
        }
        public ICommand JBSSearchBtnCommand
        {
            get
            {
                return new Command(OnSearchClickCommand);
            }
        }

        #endregion

        public DownloadViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_UserProject> _userProjectRepository,
            IDownloadService _downloadService,
            IRepository<T_ManPowerResource> _manPowerResourceRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._userProjectRepository = _userProjectRepository;
            this._downloadService = _downloadService;
            this._manPowerResourceRepository = _manPowerResourceRepository;
            SearchText = "";
            BGColorChange();
            try
            {
                if (Settings.ModuleName == "EReporter")
                {
                    CloseBtnColor = "#FB1610";
                    EReporterGrid = true;
                    ETestPackageGrid = JobSettingGrid = false;
                    GetDownloadEReporterListData(false);
                }                   
                else if(Settings.ModuleName == "TestPackage")
                {
                    CloseBtnColor = "#C4BB46";
                    ETestPackageGrid = true;
                    EReporterGrid = JobSettingGrid = false;
                    GetDownloadETestPackageListData("Load");
                }
                else if (Settings.ModuleName == "JobSetting")
                {
                    CloseBtnColor = "#3B87C7";
                    IWPHelper.includedAttachments = IWPHelper.includedDrawings = IWPHelper.includedPredecessors = IWPHelper.includedSuccessors = false;
                    ETestPackageGrid = EReporterGrid = false;
                    JobSettingGrid = true;
                    JBS = new List<T_IWP>();
                    GetDownloadJobSettingListData(false);
                }

                PageHeaderText = "Download";
            }
            catch (Exception ex)
            {

            }
           
            // RefreshCommand = new Command(CmdRefresh);
        }
        #region Private
        private async Task GetDownloadEReporterListData(bool search)
        {
            _userDialogs.ShowLoading("Loading");
            NoDataIsVisible = false; 
            var UserProjectList = await _userProjectRepository.GetAsync(); // Task.Run(async () => await _userProjectRepository.GetAsync()).Result;
            if (UserProjectList.Count > 0)
                userProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID== Settings.ProjectID).FirstOrDefault();

            string JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getEReportHeaders(userProject.ModelName, Settings.Report, userProject.User_ID, Settings.UnitID), Settings.AccessToken);
           var EReportsList = new ObservableCollection<T_EReports>(JsonConvert.DeserializeObject<List<T_EReports>>(JsonString));

            if (search)
            {
                List<T_EReports> SearchEReports = new List<T_EReports>();
                foreach (T_EReports row in EReportsList)
                {
                    Boolean CanAdd = true;
                    if (SearchText != string.Empty)
                    {
                        string RowValue = row.ReportType + " " + row.AFINo + " " + row.ReportNo + " " + row.ReportDate;

                        if (!RowValue.ToUpper().Contains(SearchText.ToUpper()))
                            CanAdd = false;
                    }
                    if (CanAdd)
                    {
                        SearchEReports.Add(row);
                    }
                }
                EReports = new ObservableCollection<T_EReports>(SearchEReports);
            }
            else 
            EReports = new ObservableCollection<T_EReports>(EReportsList);

            if (EReports.Count <= 0)
                NoDataIsVisible = true;
            _userDialogs.HideLoading();
        }

        private async Task GetDownloadETestPackageListData(string Option)
        {
            try
            {
                if (!IsBack)
                {
                    _userDialogs.ShowLoading("Loading");
                    string JsonString = string.Empty;                   
                    var UserProjectList = await _userProjectRepository.GetAsync();
                    if (UserProjectList.Count > 0)
                        userProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();
                    if (Option == "ShowAll")
                    {
                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getETestPackageHeaders(userProject.ModelName), Settings.AccessToken);
                        ETestPackagesData = JsonConvert.DeserializeObject<List<T_ETestPackages>>(JsonString);
                        //ETestPackages = new ObservableCollection<T_ETestPackages>(JsonConvert.DeserializeObject<List<T_ETestPackages>>(JsonString));
                    }
                    else if (Option == "Load")
                    {
                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getETestPackageHeaders_UserID(userProject.ModelName, userProject.User_ID), Settings.AccessToken);
                        ETestPackagesData = JsonConvert.DeserializeObject<List<T_ETestPackages>>(JsonString);
                        // ETestPackages = new ObservableCollection<T_ETestPackages>(JsonConvert.DeserializeObject<List<T_ETestPackages>>(JsonString));                        
                    }
                    else if (ETestPackagesData.Count >0 && Option == "Search")
                    {
                        List<T_ETestPackages> Searchlist = new List<T_ETestPackages>();
                        string[] Searchstring = { pcwbs, testPackage,
                                                  selectedActionBy !=null? selectedActionBy : null,
                                                  selectedPendingAction != null ? selectedPendingAction : null,
                                                  selectedPriority,
                                                  selectedSubContractor!=null? selectedSubContractor :null,
                                                  selectedTestMethod != null ?selectedTestMethod :null};

                        foreach (T_ETestPackages row in ETestPackagesData)
                        {
                            string[] rowstring ={ row.PCWBS, row.TestPackage,
                                                  selectedActionBy !=null? row.ActionBy :null,
                                                  selectedPendingAction != null ? row.Status :null,
                                                  selectedPriority,
                                                  selectedSubContractor!=null? row.SubContractor :null,
                                                  selectedTestMethod != null ?  row.TestMedia :null};
                            Boolean CanAdd = true;
                            for (int i = 0; i < Searchstring.Count(); i++)
                            {
                                if (Searchstring[i] == null || Searchstring[i] == "ALL")
                                    continue;
                                if (!rowstring[i].ToUpper().Contains(Searchstring[i].ToUpper()))
                                    CanAdd = false;
                            }
                            if (CanAdd)
                            {
                                Searchlist.Add(row);
                            }
                        }
                        ETestPackages = new ObservableCollection<T_ETestPackages>(Searchlist);
                    }
                    if (ETestPackagesData.Count > 0 && (Option == "Load"|| Option == "ShowAll"))
                    {
                        //ActionByList = new ObservableCollection<string>(ETestPackagesData.Where(l => l.ActionBy != null).Select(l => l.ActionBy).Distinct().ToList());
                        //PendingActionList = new ObservableCollection<string>(ETestPackagesData.Where(c => c.Status != null).Select(c => c.Status).Distinct().ToList());
                        //PriorityList = new ObservableCollection<int>(ETestPackagesData.Select(c => Convert.ToInt32(c.Priority)).Distinct().ToArray());
                        //SubContractorList = new ObservableCollection<string>(ETestPackagesData.Where(s => s.SubContractor != null).Select(s => s.SubContractor).Distinct().ToList());
                        //TestMethodList = new ObservableCollection<string>(ETestPackagesData.Where(c => c.TestMedia != null).Select(c => c.TestMedia).Distinct().ToList());

                        var Action = ETestPackagesData.Where(l => l.ActionBy != null).Select(l => l.ActionBy).Distinct().ToList();
                        Action.Insert(0, "ALL");
                        ActionByList = new ObservableCollection<string>(Action);

                        var PendingAction = ETestPackagesData.Where(c => c.Status != null).Select(c => c.Status).Distinct().ToList();
                        PendingAction.Insert(0, "ALL");
                        PendingActionList = new ObservableCollection<string>(PendingAction);

                        List<string> priority = ETestPackagesData.Select(c => (Convert.ToInt32(c.Priority)).ToString()).Distinct().ToList();
                        priority.Insert(0, "ALL");
                        PriorityList = new ObservableCollection<string>(priority);


                        var SubContractor = ETestPackagesData.Where(s => s.SubContractor != null).Select(s => s.SubContractor).Distinct().ToList();
                        SubContractor.Insert(0, "ALL");
                        SubContractorList = new ObservableCollection<string>(SubContractor);


                        var TestMethod = ETestPackagesData.Where(c => c.TestMedia != null).Select(c => c.TestMedia).Distinct().ToList();
                        TestMethod.Insert(0, "ALL");
                        TestMethodList = new ObservableCollection<string>(TestMethod);
                    }
                    if(Option!="Search")
                        ETestPackages = new ObservableCollection<T_ETestPackages>(ETestPackagesData);

                    _userDialogs.HideLoading();
                }

            }
            catch (Exception ex)
            {

            }
        }

        private async Task GetDownloadJobSettingListData(bool search)
        {
            _userDialogs.ShowLoading("Loading");
            NoDataIsVisible = false;
            var UserProjectList = await _userProjectRepository.GetAsync();
            if (UserProjectList.Count > 0)
                userProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();

            string JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getIWP(userProject.ModelName), Settings.AccessToken);
            var JobSettingList = new ObservableCollection<T_IWP>(JsonConvert.DeserializeObject<List<T_IWP>>(JsonString));

            if (search)
            {
                List<T_IWP> SearchJobSettings = new List<T_IWP>();
                string Searchstring = string.Empty;
                if (!String.IsNullOrWhiteSpace(JBSPCWBS))
                    Searchstring = Searchstring + JBSPCWBS.ToUpper();
                if (!String.IsNullOrWhiteSpace(JBSFWBS))
                    Searchstring = Searchstring + JBSFWBS.ToUpper();
                if (!String.IsNullOrWhiteSpace(JBSIWP))
                    Searchstring = Searchstring + JBSIWP.ToUpper();
                if (!String.IsNullOrWhiteSpace(JBSDiscipline))
                    Searchstring = Searchstring + JBSDiscipline.ToUpper();

                foreach (T_IWP row in JobSettingList)
                {
                    Boolean CanAdd = true;
                    if (Searchstring != string.Empty)
                    {
                        string RowValue = row.PCWBS + row.FWBS +  row.Title + row.Discipline;

                        if (!RowValue.ToUpper().Contains(Searchstring))
                            CanAdd = false;
                    }
                    if (CanAdd)
                    {
                        SearchJobSettings.Add(row);
                    }
                }
                JobSetting = new ObservableCollection<T_IWP>(SearchJobSettings);
            }
            else
            JobSetting = new ObservableCollection<T_IWP>(JobSettingList);

            if (JobSetting.Count <= 0)
                NoDataIsVisible = true;
            _userDialogs.HideLoading();
        }
        private async void OnClickAllDownloadButton()
        {
            if (EReports == null)
                return;
            if (EReports.Count <= 0)
                return;
            DownloadList = new ObservableCollection<T_EReports>(EReports);
            
                EReporterGrid = false;
                EReporterVisibleList = true;
                IsVisibleDownlaodProgress = false;
            
            //try
            //{
            //    _userDialogs.ShowLoading();
            //    //var EReportDownloadList = EReports.Select(i => i.ID).ToList();
            //    //foreach (int CurrentID in EReportDownloadList)
            //    //{
            //    //    string JsonResonceString = ModsTools.WebServiceGet(ApiUrls.Url_getEReport(CurrentID, _pageHelper.UnitID), Settings.AccessToken);
            //    //   // EReports = JsonConvert.DeserializeObject<List<T_EReports>>(JsonResonceString);
            //    //    await _eReportsRepository.InsertOrReplaceAsync(EReports);
            //    //    var Ereport = await _eReportsRepository.GetAsync();
            //    //}
            //    var isDownloaded = await _downloadService.DownloadAsync();
            //    _userDialogs.HideLoading();
            //}
            //catch (Exception ex)
            //{
            //    _userDialogs.HideLoading();
            //    _userDialogs.AlertAsync("Download Failed..!", "Download", "OK");
            //}
        }

        private async void OnClickButton(string param)
        {
            GetDownloadETestPackageListData(param);
        }
        private async void OnClickSearchButton()
        {
             if(!String.IsNullOrEmpty(SearchText.Trim()))
             {
                GetDownloadEReporterListData(true);               
            }
        }
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

            //await Task.Delay(3000);
        }
        private async void OnClickCloseButton()
        {

            DownloadingProgressList.Clear();
            //IsVisibleDataGrid = true;
            //IsVisibleList = false;
            //IsVisibleDownlaodProgress = false;
            if (Settings.ModuleName == "EReporter")
                await navigationService.NavigateAsync<DashboardViewModel>();
            else if (Settings.ModuleName == "TestPackage")
                await navigationService.NavigateAsync<ETestPackageVewModel>();
            else if (Settings.ModuleName == "JobSetting")
                await navigationService.NavigateAsync<IWPSelectionViewModel>();
            IsRunningTasks = false;
        }       
        #endregion
        #region Public
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            IsBack = true;
            base.OnNavigatedFrom(parameters);           
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            //if (parameters.Count == 0)
            //{
            //    return;
            //}
            //GetDownloadListData();

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

        
        private void AddToDownloadList(T_EReports selectedReport)
        {
           // DownloadList.Add(selectedReport);
           // throw new NotImplementedException();
        }      

        //methods
        private async void OnClickAddToListdButton(string param)
        {
            if (SelectedReport != null)
            {
                if (dList == null || !dList.Where(x => x.ID == SelectedReport.ID).Any())
                {
                    dList.Add(SelectedReport);
                    DownloadList = new ObservableCollection<T_EReports>(dList);
                }
                if (param == "AddtoList")
                {
                    EReporterGrid = false;
                    EReporterVisibleList = true;
                    IsVisibleDownlaodProgress = false;
                }
                else
                {
                    EReporterGrid = true;
                    EReporterVisibleList = false;
                    IsVisibleDownlaodProgress = false;
                }
            }
        }

        private async void OnClickDownloadButton(string param)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    EReporterGrid = false;
                    EReporterVisibleList = false;
                    IsVisibleDownlaodProgress = true;
                    DownloadingFor = "E-Report";

                    //code for download all

                    //_userDialogs.ShowLoading();
                    var selectedEReportIds = DownloadList.Select(s => s.ID).ToList();                 

                    var EReportDownloadList = selectedEReportIds;
                    int DownloadCount =EReportDownloadList.Count;
                   
                    if (EReportDownloadList.Count > 0)
                    {
                        float per = (float)1 / (float)DownloadCount;

                        DownloadingProgressList.Add("Downloading...!");
                      
                        //ProgressValue = per;
                        ProgressBarIsVisible = true;
                        foreach (int CurrentID in EReportDownloadList)
                        {
                            ProgressValue += per / 2;
                            var name = DownloadList.Where(i => i.ID == CurrentID).Select(i => i.ReportNo).FirstOrDefault();
                            DownloadingProgressList.Add($"Downloading {name}");
                            var isDownloaded = await _downloadService.DownloadAsync(CurrentID);
                            if (isDownloaded)
                            {
                                DownloadingProgressList.Add($"{name} Downloaded");
                                ProgressValue += per/2;
                            }                               
                            else
                                DownloadingProgressList.Add($"Download failed {name}");
                        }
                        DownloadingProgressList.Add("Download Completed");
                        Close_IsVisible = true;
                        ProgressBarIsVisible = false;
                    }                   
                                
                }
                else
                {
                    NavigateToErrorPage();
                }
            }
            catch (Exception ex)
            {
                //_userDialogs.HideLoading();
                  DownloadList.Clear();
                _userDialogs.AlertAsync("Download Failed..!", "Download", "OK");
                Close_IsVisible = true;
            }

        }

        private async void OnClickETPAddToListdButton(string param)
        {
            if (SelectedETP != null)
            {
                if (ETPList == null || !ETPList.Where(x => x.ID == SelectedETP.ID).Distinct().Any())
                {
                    ETPList.Add(SelectedETP);
                    ETPDownloadList = new ObservableCollection<T_ETestPackages>(ETPList);
                }
                if (param == "AddtoList")
                {
                    EReporterGrid = ETestPackageGrid= false;
                    ETPVisibleList = true;
                    IsVisibleDownlaodProgress = false;
                }
                else
                {
                    ETestPackageGrid= true;
                    ETPVisibleList = false;
                    IsVisibleDownlaodProgress = false;
                }
            }
        }

        private async void OnClickAllETPDownloadButton(string param)
        {
            if (ETestPackages.Count > 0)
            {
                ETPDownloadList = new ObservableCollection<T_ETestPackages>(ETestPackages);
                foreach(T_ETestPackages item in ETPDownloadList)
                ETPList.Add(item);
                EReporterGrid = ETestPackageGrid = false;
                ETPVisibleList = true;
                IsVisibleDownlaodProgress = false;
            }
        }

        private async void OnClickETPDownloadButton(string param)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    EReporterGrid = false;
                    ETPVisibleList = false;
                    IsVisibleDownlaodProgress = true;
                    DownloadingFor = "Test Package";

                    //code for download all

                    //_userDialogs.ShowLoading();
                    var selectedETPIds = ETPDownloadList.Select(s => s.ID).Distinct().ToList();

                    var etpDownloadList = selectedETPIds;
                    int DownloadCount = etpDownloadList.Count;

                    if (etpDownloadList.Count > 0)
                    {
                        float per = (float)1 / (float)DownloadCount;

                        DownloadingProgressList.Add("Downloading...!");

                        //ProgressValue = per;
                        ProgressBarIsVisible = true;
                        foreach (int CurrentID in etpDownloadList)
                        {
                            var name = ETPDownloadList.Where(i => i.ID == CurrentID).Select(i => i).FirstOrDefault();
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                ProgressValue += per / 2;
                                
                                DownloadingProgressList.Add($"Downloading {name}");
                                var isDownloaded = await _downloadService.DownloadAsync(CurrentID);
                                if (isDownloaded)
                                {
                                    DownloadingProgressList.Add($"{name} Downloaded");
                                    ProgressValue += per / 2;
                                }
                                else
                                    DownloadingProgressList.Add($"Download failed {name}");
                            }
                            else
                                DownloadingProgressList.Add($"Download failed {name}");
                        }
                        DownloadingProgressList.Add("Download Completed");
                        Close_IsVisible = true;
                        ProgressBarIsVisible = false;
                    }

                }
                else
                {
                    NavigateToErrorPage();
                }
            }
            catch (Exception ex)
            {
                //_userDialogs.HideLoading();
                DownloadList.Clear();
                _userDialogs.AlertAsync("Download Failed..!", "Download", "OK");
                Close_IsVisible = true;
            }

        }



        //Job Setting 
        private async void OnClickCommand(string param)
        {
            if(param == "AddAlltoList")
            {
                if (JobSetting.Count > 0)
                {
                    JobsettingVisibleList = true;
                    JobSettingGrid = false;
                    JobSDownloadList = JobSetting;
                }

            }
            else if(param == "AddtoList")
            {
                if (SelectedJobSetting != null)
                {
                    JobsettingVisibleList = true;
                    JobSettingGrid = false;
                    if (JBS == null || !JBS.Where(x => x.ID == SelectedJobSetting.ID).Distinct().Any())
                        JBS.Add(SelectedJobSetting);
                    JobSDownloadList = new ObservableCollection<T_IWP>(JBS);
                }
            }           
            else if (param == "AddMore")
            {
                JobsettingVisibleList = false;
                JobSettingGrid = true;
            }
            else if (param == "Download")
            {
                try
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {                        
                        DownloadingFor = "IWP(s)";

                        //code for download all

                        //_userDialogs.ShowLoading();
                        var selectedJBSIds = JobSDownloadList.Select(s => s).Distinct().ToList();

                        //get all PDF ID's
                        List<DownloadJOBSettingList> SelectedIDs = new List<DownloadJOBSettingList>();

                        foreach (T_IWP iwpID in selectedJBSIds)
                        {
                            if (!SelectedIDs.Select(i=>i.ID).Contains(iwpID.ID))
                                SelectedIDs.Add(new DownloadJOBSettingList { ID = iwpID.ID,Name = iwpID.Title });

                            if (IWPHelper.includedPredecessors)
                            {
                                foreach (T_Predecessor PredecessorSubIWP in iwpID.PredecessorList)
                                {
                                    if (!SelectedIDs.Select(i => i.ID).Contains(PredecessorSubIWP.SubIWP_ID))
                                        SelectedIDs.Add(new DownloadJOBSettingList { ID = PredecessorSubIWP.SubIWP_ID, Name = PredecessorSubIWP.Title });
                                }
                            }
                            if (IWPHelper.includedSuccessors)
                            {
                                foreach (T_Successor SuccessorSubIWP in iwpID.SuccessorList)
                                {
                                    if (!SelectedIDs.Select(i => i.ID).Contains(SuccessorSubIWP.SubIWP_ID))
                                        SelectedIDs.Add(new DownloadJOBSettingList { ID = SuccessorSubIWP.SubIWP_ID, Name = SuccessorSubIWP.Title });
                                }
                            }
                        }

                        var jbsDownloadList = SelectedIDs;
                        int DownloadCount = jbsDownloadList.Count;
                        //Hide Jobsettinglist and Display Downlaod list
                        //JobsettingVisibleList = JobSettingGrid = false;
                        //IsVisibleDownlaodProgress = true;

                        if (jbsDownloadList.Count > 0)
                        {
                            float per = 1 / (float)DownloadCount;

                            DownloadingProgressList.Add("Downloading...!");
                            JobsettingVisibleList = JobSettingGrid = false;
                            IsVisibleDownlaodProgress = true;

                            //ProgressValue = per;
                            ProgressBarIsVisible = true;
                            foreach (DownloadJOBSettingList CurrentID in SelectedIDs)
                            {
                                var name = CurrentID.Name;
                                if (CrossConnectivity.Current.IsConnected)
                                {
                                    ProgressValue += per / 2;

                                    DownloadingProgressList.Add($"Downloading {name}");
                                    var isDownloaded = await _downloadService.DownloadAsync(CurrentID.ID);
                                    if (isDownloaded)
                                    {
                                        DownloadingProgressList.Add($"{name} Downloaded");
                                        ProgressValue += per / 2;
                                    }
                                    else
                                        DownloadingProgressList.Add($"Download failed {name}");
                                }
                                else
                                    DownloadingProgressList.Add($"Download failed {name}");
                            }                           
                            DownloadingProgressList.Add("Download Completed");
                            Close_IsVisible = true;
                            ProgressBarIsVisible = false;
                        }

                    }
                    else
                    {
                        NavigateToErrorPage();
                    }
                }
                catch (Exception ex)
                {
                    //_userDialogs.HideLoading();
                    DownloadList.Clear();
                    _userDialogs.AlertAsync("Download Failed..!", "Download", "OK");
                    Close_IsVisible = true;
                }
            }

        }
        private async void OnBGColorChangeClickCommand(string param)
        {
            if (param == "Attachments")
            {
                IWPHelper.includedAttachments = Attachments = !Attachments;
            }
            else if (param == "Drawings")
            {
                IWPHelper.includedDrawings = Drawings = !Drawings;
            }
            else if (param == "Predecessors")
            {
                IWPHelper.includedPredecessors = Predecessors = !Predecessors;
            }
            else if (param == "Successors")
            {
                IWPHelper.includedSuccessors = Successors = !Successors;
            }
            else if (param == "CWPTags")
            {
                IWPHelper.includedCWPTags = CWPTags = !CWPTags;
            }
            BGColorChange();
        }
        private void BGColorChange()
        {
            if (Attachments)
                BGAttachments = "#3B87C7";
            else
                BGAttachments = "#B0B0B0";
            if (Drawings)
                BGDrawings = "#3B87C7";
            else
                BGDrawings = "#B0B0B0";
            if (Predecessors)
                BGPredecessors = "#3B87C7";
            else
                BGPredecessors = "#B0B0B0";
            if (Successors)
                BGSuccessors = "#3B87C7";
            else
                BGSuccessors = "#B0B0B0";
            if (CWPTags)
                BGCWPTags = "#3B87C7";
            else
                BGCWPTags = "#B0B0B0";
        }
        private void OnSearchClickCommand()
        {
            GetDownloadJobSettingListData(true);
        }
        public class DownloadJOBSettingList
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
    }
}

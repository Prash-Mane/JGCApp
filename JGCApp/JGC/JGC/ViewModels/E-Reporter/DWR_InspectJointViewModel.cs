using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Extentions;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.Models;
using Newtonsoft.Json;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JGC.ViewModels.E_Reporter
{
    public class DWR_InspectJointViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_RT_Defects> _RT_DefectsRepository;
        private readonly IRepository<T_Welders> _weldersRepository;
        private readonly IRepository<T_DWR_HeatNos> _DWR_HeatNosRepository;
        private readonly IRepository<T_WPS_MSTR> _WPS_MSTRRepository;
        private readonly IRepository<T_BaseMetal> _BaseMetalRepository;
        private readonly IRepository<T_WeldProcesses> _WeldProcessesRepository;
        private readonly IRepository<T_DWR> _DWRRepository;
        private readonly IRepository<T_Drawings> _drawingsRepository;
        private readonly IRepository<T_EReports_Signatures> _signaturesRepository;
        private readonly IRepository<T_EReports_UsersAssigned> _usersAssignedRepository;
        private readonly IRepository<T_EReports> _eReportsRepository;
        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private T_UserProject userProject;
        T_EReports ThisEReport;
        private T_UserDetails UserDetail;


        #region Properties
        private ObservableCollection<string> downloadingProgressList;
        public ObservableCollection<string> DownloadingProgressList
        {
            get
            {
                return downloadingProgressList ?? (downloadingProgressList = new ObservableCollection<string>());
            }
        }

        private ObservableCollection<DWR> _InspectDWRDownLoadList;
        public ObservableCollection<DWR> InspectDWRDownLoadList
        {
            get { return _InspectDWRDownLoadList; }
            set { _InspectDWRDownLoadList = value; OnPropertyChanged(); }
        }
        private ObservableCollection<DWR> _SelectedDWRList;
        public ObservableCollection<DWR> SelectedDWRList
        {
            get { return _SelectedDWRList; }
            set { _SelectedDWRList = value; OnPropertyChanged(); }
        }

        private DWR _SelectedInspectDWRItem;
        public DWR SelectedInspectDWRItem
        {
            get { return _SelectedInspectDWRItem; }
            set { _SelectedInspectDWRItem = value; OnPropertyChanged(); }
        }
        private ObservableCollection<T_DWR> _DWRDownLoadedList;
        public ObservableCollection<T_DWR> DWRDownLoadedList
        {
            get { return _DWRDownLoadedList; }
            set { _DWRDownLoadedList = value; OnPropertyChanged(); }
        }

        private T_DWR selectedDWRReport;
        public T_DWR SelectedDWRReport
        {
            get { return selectedDWRReport; }
            set { selectedDWRReport = value; RaisePropertyChanged(); }
        }
      
        private bool isVisbleInspectList { get; set; }
        public bool IsVisbleInspectList
        {
            get { return isVisbleInspectList; }
            set { isVisbleInspectList = value; RaisePropertyChanged(); }
        }
        private bool isVisbleSelectedDownloadDWR { get; set; }
        public bool IsVisbleSelectedDownloadDWR
        {
            get { return isVisbleSelectedDownloadDWR; }
            set { isVisbleSelectedDownloadDWR = value; RaisePropertyChanged(); }
        }
        private bool isVisibleDownloadingProgressList { get; set; }
        public bool IsVisibleDownloadingProgressList
        {
            get { return isVisibleDownloadingProgressList; }
            set { isVisibleDownloadingProgressList = value; RaisePropertyChanged(); }
        }
        private bool isVisibleCloseBtn { get; set; }
        public bool IsVisibleCloseBtn
        {
            get { return isVisibleCloseBtn; }
            set { isVisibleCloseBtn = value; RaisePropertyChanged(); }
        }
        private bool isVisibleProgressBar { get; set; }
        public bool IsVisibleProgressBar
        {
            get { return isVisibleProgressBar; }
            set { isVisibleProgressBar = value; RaisePropertyChanged(); }
        }
        private float _progressValue;
        public float ProgressValue
        {
            get { return _progressValue; }
            set { SetProperty(ref _progressValue, value); }
        }
        private string downloadingFor;
        public string DownloadingFor
        {
            get { return downloadingFor; }
            set
            {
                SetProperty(ref downloadingFor, value); OnPropertyChanged();
            }
        }
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }
        private string testPack;
        public string TestPack
        {
            get { return testPack; }
            set { SetProperty(ref testPack, value); }
        }
        private string spoolDWGNo;
        public string SpoolDWGNo
        {
            get { return spoolDWGNo; }
            set { SetProperty(ref spoolDWGNo, value); }
        }
        private string jointNo;
        public string JointNo
        {
            get { return jointNo; }
            set { SetProperty(ref jointNo, value); }
        }
        
        #endregion
        #region Delegate Commands  
        public ICommand OnBtnClickCommand
        {
            get
            {
                return new Command<string>(OnBtnClick);
            }
        }
        public ICommand DownloadCommand
        {
            get
            {
                return new Command(OnClickDownloadButton);
            }
        }
        public ICommand CloseCommand
        {
            get
            {
                return new Command(OnClickCloseButton);
            }
        }
        #endregion


        public DWR_InspectJointViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_UserProject> _userProjectRepository,
            IRepository<T_RT_Defects> _RT_DefectsRepository,
            IRepository<T_Welders> _weldersRepository,
            IRepository<T_DWR_HeatNos> _DWR_HeatNosRepository,
            IRepository<T_WPS_MSTR> _WPS_MSTRRepository,
            IRepository<T_BaseMetal> _BaseMetalRepository,
            IRepository<T_WeldProcesses> _WeldProcessesRepository,
            IRepository<T_DWR> _DWRRepository,
            IRepository<T_Drawings> _drawingsRepository,
            IRepository<T_EReports_Signatures> _signaturesRepository,
            IRepository<T_EReports_UsersAssigned> _usersAssignedRepository,
            IRepository<T_EReports> _eReportsRepository,
            IRepository<T_UserDetails> _userDetailsRepository
           ) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._userProjectRepository = _userProjectRepository;
            this._RT_DefectsRepository = _RT_DefectsRepository;
            this._weldersRepository = _weldersRepository;
            this._DWR_HeatNosRepository = _DWR_HeatNosRepository;
            this._WPS_MSTRRepository = _WPS_MSTRRepository;
            this._BaseMetalRepository = _BaseMetalRepository;
            this._WeldProcessesRepository = _WeldProcessesRepository;
            this._DWRRepository = _DWRRepository;
            this._drawingsRepository = _drawingsRepository;
            this._signaturesRepository = _signaturesRepository;
            this._usersAssignedRepository = _usersAssignedRepository;
            this._eReportsRepository = _eReportsRepository;
            this._userDetailsRepository = _userDetailsRepository;
            _userDialogs.HideLoading();
            JobSettingHeaderVisible = true;
            PageHeaderText = "Download page";
            DWRHelper.DWRTargetType = typeof(DWR_InspectJointViewModel);
            GetDownloadDWRListData(false);
            GetUserDetails();
            IsVisbleInspectList = true;
            IsVisibleDownloadingProgressList = false;

        }

        private async void OnClickCloseButton()
        {
            await navigationService.NavigateFromMenuAsync(typeof(JointDetailsViewModel));
        }
        private void OnBtnClick(string param)
        {
            if (param == "AddAll")
            {
                List<DWR> selectedList = InspectDWRDownLoadList.ToList();
                if (selectedList.Count > 0)
                {
                    selectedList.ForEach(i => i.Selected = true);
                    SelectedDWRList = new ObservableCollection<DWR>(selectedList);
                    IsVisbleInspectList = false;
                    IsVisbleSelectedDownloadDWR = true;
                }
                else
                    _userDialogs.AlertAsync("", "No DWR to Add", "Ok");
            }
            else if (param == "AddToDownload")
            {

                List<DWR> selectedList = InspectDWRDownLoadList.Where(x => x.Selected == true).ToList();
                if (selectedList.Count > 0)
                {
                    SelectedDWRList = new ObservableCollection<DWR>(selectedList);
                    IsVisbleInspectList = false;
                    IsVisbleSelectedDownloadDWR = true;
                }
                else
                    _userDialogs.AlertAsync("", "Please select DWR", "Ok");

            }
            else if (param == "AddMore")
            {
                if (InspectDWRDownLoadList != null)
                {
                    SelectedInspectDWRItem = null;
                    IsVisbleSelectedDownloadDWR = false;
                    IsVisbleInspectList = true;
                }
            }
            else if (param == "searchdownloadList")
            {
                GetDownloadDWRListData(true);
            }
        }
        private async void OnClickDownloadButton()
        {
            try
            {
                if (SelectedDWRList != null)
                {
                    float per = (float)1 / (float)SelectedDWRList.Count;
                    DownloadingFor = "DWR";
                    DownloadingProgressList.Add("Downloading...!");
                    IsVisbleInspectList = IsVisbleSelectedDownloadDWR = false;
                    IsVisibleDownloadingProgressList = IsVisibleProgressBar = true;
                    //List<string> subContractors = new List<string>();

                    foreach (DWR item in SelectedDWRList)
                    {
                        ProgressValue += per / 2;

                        List<T_Drawings> CurrentDrawingList = new List<T_Drawings>();

                        T_Drawings CurrentDrawing = new T_Drawings()
                        {
                            EReportID = item.ID,
                            JobCode = Settings.JobCode,
                            Name = item.SpoolDrawingNo,
                            Sheet_No = item.SheetNo,
                            Revision = item.RevNo
                        };
                        CurrentDrawingList.Add(CurrentDrawing);

                        //Get DWR
                        //string DWRJsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetDWR(Settings.ProjectID, Settings.JobCode, item.SpoolDrawingNo, item.JointNo), Settings.AccessToken);
                        //Get EReporter
                        string DWRJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getEReport(item.ID, Settings.UnitID), Settings.AccessToken);
                        ThisEReport = JsonConvert.DeserializeObject<T_EReports>(DWRJsonString);
                      //  ThisEReport.ID = item.ID;
                        T_DWR DWRdata = JsonConvert.DeserializeObject<T_DWR>(ThisEReport.JSONString);
                       
                        var IDList = await _DWRRepository.GetAsync();
                        T_DWR rowIDs = new T_DWR();
                        if (IDList.Count > 0)
                            rowIDs = IDList.OrderByDescending(u => u.RowID).FirstOrDefault();

                        if (DWRdata != null)
                        {
                            DWRdata.RowID = rowIDs.RowID + 1;
                            DWRdata.ID = item.ID;
                            DWRdata.ProjectID = Settings.ProjectID;
                            DWRdata.DownloadedDate = DateTime.Now;
                            await _DWRRepository.QueryAsync("DELETE FROM [T_DWR] Where ProjectID ='" + Settings.ProjectID + "' AND ID='" + item.ID + "' AND SpoolDrawingNo ='" + DWRdata.SpoolDrawingNo
                                                           + "' AND TestPackage='" + item.TestPackage + "' AND JointNo='" + item.JointNo + "'");

                            await _DWRRepository.InsertOrReplaceAsync(DWRdata);

                            if (ThisEReport.ID > 0)
                                await _eReportsRepository.QueryAsync<T_EReports>(@"DELETE FROM [T_EReports] WHERE [ID] = '" + ThisEReport.ID + "'");

                            ThisEReport.RowID = DWRdata.RowID;
                            await _eReportsRepository.InsertOrReplaceAsync(ThisEReport);
                        }

                        //get VI and DI images
                        var ImageList = ModsTools.WebServiceGet(ApiUrls.Url_getImages("EREPORTER", item.ID), Settings.AccessToken);
                        List<VMHub> AllImagesList = JsonConvert.DeserializeObject<List<VMHub>>(ImageList);
                        if (AllImagesList.Count > 0)
                        {
                            foreach (VMHub CurrentImage in AllImagesList.ToArray())
                            {
                                string Folder = string.Empty;
                                if (CurrentImage.DisplayName.StartsWith("VI"))
                                    Folder = ("Photo Store\\" + item.JobCode + "\\" + Settings.UserID + "\\DWR\\" + DWRdata.RowID.ToString() + "\\" + "VI");
                                else if (CurrentImage.DisplayName.StartsWith("DI"))
                                    Folder = ("Photo Store\\" + item.JobCode + "\\" + Settings.UserID + "\\DWR\\" + DWRdata.RowID.ToString() + "\\" + "DI");

                                var InspectionPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath(Folder);

                                await DependencyService.Get<ISaveFiles>().SavePictureToDisk(Device.RuntimePlatform == Device.UWP ? Folder : InspectionPath,
                                    Device.RuntimePlatform == Device.UWP ? CurrentImage.FileName : Path.GetFileNameWithoutExtension(CurrentImage.FileName), Convert.FromBase64String(CurrentImage.FileBytes).ToArray());
                            }
                        }
                        if (CurrentDrawingList.Count > 0)
                        {
                            string DrawingListJson = ModsTools.ToJson(CurrentDrawingList);

                            string DrawingList = ModsTools.WebServicePost(ApiUrls.Url_postDrawingList(true), DrawingListJson, Settings.AccessToken);
                            CurrentDrawingList = JsonConvert.DeserializeObject<List<T_Drawings>>(DrawingList);

                            foreach (T_Drawings DWRCurrentDrawing in CurrentDrawingList.ToArray())
                            {
                                string DWRFolder = ("PDF Store" + "\\" + Settings.JobCode + "\\" + item.ID.ToString());
                                byte[] DWRPDFBytes = Convert.FromBase64String(DWRCurrentDrawing.BinaryCode);
                                DWRCurrentDrawing.FileLocation = await DependencyService.Get<ISaveFiles>().SavePDFToDisk(DWRFolder, DWRCurrentDrawing.FileName, DWRPDFBytes);
                                DWRCurrentDrawing.EReportID = ThisEReport.ID;
                                DWRCurrentDrawing.RowID = ThisEReport.RowID;
                                //T_Drawings Drawing = new T_Drawings
                                //{
                                //    EReportID = item.ID,
                                //    RowID = rowIDs.RowID + 1,
                                //    JobCode = Settings.JobCode,
                                //    Name = DWRCurrentDrawing.Name,
                                //    Sheet_No = "",
                                //    Total_Sheets = "",
                                //    FileName = DWRCurrentDrawing.FileName,
                                //    FileLocation = DWRCurrentDrawing.FileLocation,
                                //    Revision = "",
                                //};

                                await _drawingsRepository.InsertOrReplaceAsync(DWRCurrentDrawing);
                            }
                        }


                        string WeldProcessesJsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetWeldProcesses(Settings.ProjectID), Settings.AccessToken);
                        var WeldProcesses = JsonConvert.DeserializeObject<List<T_WeldProcesses>>(WeldProcessesJsonString);
                        WeldProcesses.ForEach(i => i.ProjectID = Settings.ProjectID);
                        await _WeldProcessesRepository.QueryAsync("DELETE FROM [T_WeldProcesses] Where ProjectID =" + Settings.ProjectID);
                        await _WeldProcessesRepository.InsertOrReplaceAsync(WeldProcesses);

                        //string CheckEReportCompleteJsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetCheckEReportComplete(item.ID), Settings.AccessToken);

                        //string GetSpoolsJsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetSpools(Settings.ProjectID, Settings.JobCode, "1"), Settings.AccessToken);
                        //var GetSpools = JsonConvert.DeserializeObject<List<string>>(GetSpoolsJsonString);

                        //string GetSpoolsWithSubContractorJsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetSpoolsWithSubContractor(Settings.ProjectID, Settings.JobCode, item.SubContractor, "1"), Settings.AccessToken);
                        //var GetSpoolsWithSubContractor = JsonConvert.DeserializeObject<List<string>>(GetSpoolsWithSubContractorJsonString);

                        //string GetJointsJsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetJoints(Settings.ProjectID, Settings.JobCode, item.SpoolDrawingNo), Settings.AccessToken);

                        //string GetJointsWithSubContractorJsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetJointsWithSubContractor(Settings.ProjectID, Settings.JobCode, item.SubContractor, item.SpoolDrawingNo), Settings.AccessToken);

                        //Get RTDefects
                        string RTDefectsJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getRTDefects(Settings.ProjectID), Settings.AccessToken);
                        var RTDefects = JsonConvert.DeserializeObject<List<string>>(RTDefectsJsonString);
                        if (RTDefects != null && RTDefects.Count > 0)
                        {
                            await _RT_DefectsRepository.QueryAsync<T_RT_Defects>("DELETE FROM [T_RT_Defects]");
                            foreach (string value in RTDefects)
                            {

                                T_RT_Defects RT_Defects = new T_RT_Defects();

                                RT_Defects.RT_Defect_Code = value.Split(new string[] { " - " }, StringSplitOptions.None)[0].Trim();
                                RT_Defects.Description = value.Split(new string[] { " - " }, StringSplitOptions.None)[1].Trim();

                                await _RT_DefectsRepository.InsertOrReplaceAsync(RT_Defects);
                            }
                        }

                        //get Welders

                        string WeldersJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getWelders(Settings.ProjectID, Settings.JobCode, item.SubContractor), Settings.AccessToken);
                        var Welders = JsonConvert.DeserializeObject<List<string>>(WeldersJsonString);
                        await _weldersRepository.QueryAsync<T_Welders>("DELETE FROM [T_Welders] WHERE [Project_ID] = " + Settings.ProjectID + " AND [SubContractor] = '" + item.SubContractor + "'");

                        foreach (string value in Welders)
                        {
                            T_Welders Welder = new T_Welders();
                            Welder.Welder_Code = value.Split(new string[] { " - " }, StringSplitOptions.None)[0].Trim().Replace("'", "''");
                            Welder.Welder_Name = value.Split(new string[] { " - " }, StringSplitOptions.None)[1].Trim().Replace("'", "''");

                            Welder.Project_ID = Settings.ProjectID;
                            Welder.SubContractor = item.SubContractor;
                            await _weldersRepository.InsertOrReplaceAsync(Welder);
                        }


                        //Get HeatNumbers
                        string HeatNumbersJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getHeatNumbers(Settings.ProjectID, Settings.JobCode), Settings.AccessToken);
                        var HeatNumbers = JsonConvert.DeserializeObject<List<string>>(HeatNumbersJsonString);
                        if (HeatNumbers != null && HeatNumbers.Count > 0)
                        {
                            await _DWR_HeatNosRepository.QueryAsync<T_DWR_HeatNos>("DELETE FROM [T_DWR_HeatNos] WHERE [Project_ID] = '" + Settings.ProjectID + "' AND Updated = 0 ");
                            foreach (string value in HeatNumbers)
                            {
                                T_DWR_HeatNos DWR_HeatNos = new T_DWR_HeatNos();

                                DWR_HeatNos.Project_ID = Settings.ProjectID;
                                DWR_HeatNos.Ident_Code = value.Split(new string[] { " - " }, 2, StringSplitOptions.None)[0].Trim();
                                DWR_HeatNos.Heat_No = value.Split(new string[] { " - " }, 2, StringSplitOptions.None)[1].Trim();

                                await _DWR_HeatNosRepository.InsertOrReplaceAsync(DWR_HeatNos);
                            }
                        }


                        //Get WPS
                        string WPSNosJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getWPSNos(Settings.ProjectID, Settings.JobCode), Settings.AccessToken);
                        var WPSNos = JsonConvert.DeserializeObject<List<string>>(WPSNosJsonString);
                        if (WPSNos != null && WPSNos.Count > 0)
                        {
                            await _WPS_MSTRRepository.QueryAsync<T_WPS_MSTR>("DELETE FROM [T_WPS_MSTR] WHERE [Project_ID] = " + Settings.ProjectID);
                            foreach (string value in WPSNos)
                            {
                                T_WPS_MSTR WPS_MSTR = new T_WPS_MSTR();
                                WPS_MSTR.Project_ID = Settings.ProjectID;
                                WPS_MSTR.Wps_No = value.Split(new string[] { " - " }, StringSplitOptions.None)[0].Trim().Replace("'", "''");
                                WPS_MSTR.Description = value.Split(new string[] { " - " }, StringSplitOptions.None)[1].Trim().Replace("'", "''");

                                await _WPS_MSTRRepository.InsertOrReplaceAsync(WPS_MSTR);
                            }
                        }

                        //Get BaseMetals
                        string BaseMetalsJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getBaseMetals(Settings.ProjectID), Settings.AccessToken);
                        var BaseMetalsList = JsonConvert.DeserializeObject<List<string>>(BaseMetalsJsonString);

                        if (BaseMetalsList != null && BaseMetalsList.Count > 0)
                        {
                            await _BaseMetalRepository.QueryAsync<T_BaseMetal>("DELETE FROM [T_BaseMetal]");
                            foreach (string value in BaseMetalsList)
                            {
                                T_BaseMetal BaseMetal = new T_BaseMetal
                                {
                                    Base_Material = value,
                                };
                                await _BaseMetalRepository.InsertOrReplaceAsync(BaseMetal);
                            }
                        }

                        //Get Signatures
                        //string JsonResonceString = ModsTools.WebServiceGet(ApiUrls.Url_getEReport(item.ID, Settings.UnitID), Settings.AccessToken);
                        //ThisEReport = JsonConvert.DeserializeObject<T_EReports>(JsonResonceString);
                        //if (ThisEReport != null && ThisEReport.Signatures.Count > 0)
                        if (ThisEReport != null && ThisEReport.Signatures != null)
                        {
                            if (ThisEReport.Signatures.Count > 0)
                            {
                                ThisEReport.Signatures.ForEach(x => x.RowID = ThisEReport.RowID);
                                await _signaturesRepository.QueryAsync<T_EReports_Signatures>("DELETE FROM [T_EReports_Signatures] WHERE [EReportID] = " + ThisEReport.ID);
                                await _signaturesRepository.InsertOrReplaceAsync(ThisEReport.Signatures);
                                var data = await _signaturesRepository.GetAsync();
                            }
                        }
                        //Get UseAssinged 
                        //if (ThisEReport != null && ThisEReport.UsersAssigned.Count > 0)
                        if (ThisEReport != null && ThisEReport.UsersAssigned != null)
                        {
                            if (ThisEReport.UsersAssigned.Count > 0)
                            {
                                ThisEReport.UsersAssigned.ForEach(x => x.RowID = ThisEReport.RowID);
                                await _usersAssignedRepository.QueryAsync<T_EReports_UsersAssigned>("DELETE FROM [T_EReports_UsersAssigned] WHERE [EReportID] = " + ThisEReport.ID);
                                await _usersAssignedRepository.InsertOrReplaceAsync(ThisEReport.UsersAssigned);
                            }
                        }
                        ProgressValue += per / 2;
                        DownloadingProgressList.Add(item.SpoolDrawingNo);
                    }
                    ProgressValue = 1;
                    DownloadingProgressList.Add("Downloading Completed");

                    IsVisibleCloseBtn = true;
                    IsVisibleProgressBar = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private async Task GetUserDetails()
        {
            var UserDetailsList = await _userDetailsRepository.GetAsync(i => i.ID == Settings.UserID);
            if (UserDetailsList.Count > 0)
                UserDetail = UserDetailsList.FirstOrDefault();
        }
        private async Task GetDownloadDWRListData(bool search)
        {
            try
            {
                _userDialogs.ShowLoading("Loading");
                var UserProjectList = await _userProjectRepository.GetAsync(); // Task.Run(async () => await _userProjectRepository.GetAsync()).Result;
                if (UserProjectList.Count > 0)
                    userProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();

                string EReportHeaderJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getEReportHeaders(userProject.ModelName, "Daily Weld Report", userProject.User_ID, Settings.UnitID), Settings.AccessToken);
                var EReportHeadersList = JsonConvert.DeserializeObject<List<T_EReports>>(EReportHeaderJsonString);
                T_EReports ThisEReport;
                List<DWR> DWRList = new List<DWR>();
                foreach (T_EReports EReports in EReportHeadersList)
                {
                   // string JsonResonceString = ModsTools.WebServiceGet(ApiUrls.Url_getEReport(EReports.ID, Settings.UnitID), Settings.AccessToken);
                    //ThisEReport = JsonConvert.DeserializeObject<T_EReports>(JsonResonceString);
                    DWR DWR = JsonConvert.DeserializeObject<DWR>(EReports.JSONString);
                    DWR.ID = EReports.ID;
                    DWRList.Add(DWR);
                }

                //string JsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetDWR(userProject.Project_ID, userProject.JobCode, "A110-AI-63-40017", "010"), Settings.AccessToken);
                //List<NewDWR> DWRList = new List<NewDWR>();
                //DWRList.Add(JsonConvert.DeserializeObject<NewDWR>(JsonString));
                var EReportsList = new ObservableCollection<DWR>(DWRList);

                if (search)
                {
                    List<DWR> SearchEReports = new List<DWR>();
                    SearchText = string.Empty;
                    if (!String.IsNullOrWhiteSpace(TestPack))
                        SearchText += TestPack;
                    if (!String.IsNullOrWhiteSpace(SpoolDWGNo))
                        SearchText += SpoolDWGNo;
                    if (!String.IsNullOrWhiteSpace(JointNo))
                        SearchText += JointNo;

                    foreach (DWR row in EReportsList)
                    {
                        Boolean CanAdd = true;
                        if (SearchText != string.Empty)
                        {
                            string RowValue = row.TestPackage + row.SpoolDrawingNo + row.JointNo;

                            if (!RowValue.ToUpper().Contains(SearchText.ToUpper()))
                                CanAdd = false;
                        }
                        if (CanAdd)
                        {
                            SearchEReports.Add(row);
                        }
                    }
                    InspectDWRDownLoadList = new ObservableCollection<DWR>(SearchEReports);
                }
                else
                    InspectDWRDownLoadList = new ObservableCollection<DWR>(EReportsList);

                _userDialogs.HideLoading();
            }
            catch (Exception ex)
            {

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
    }
}

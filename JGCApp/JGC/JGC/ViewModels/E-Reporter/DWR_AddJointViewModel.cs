using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Extentions;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.Models;
using JGC.Models.E_Reporter;
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
    public class DWR_AddJointViewModel : BaseViewModel
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
        List<DWR> DWRList = new List<DWR>();
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

        private ObservableCollection<DWR> _SelectedDWRList;
        public ObservableCollection<DWR> SelectedDWRList
        {
            get { return _SelectedDWRList; }
            set { _SelectedDWRList = value; OnPropertyChanged(); }
        }

        private T_DWR selectedDWRReport;
        public T_DWR SelectedDWRReport
        {
            get { return selectedDWRReport; }
            set { selectedDWRReport = value; RaisePropertyChanged(); }
        }
        private bool isVisbleDownloadList { get; set; }
        public bool IsVisbleDownloadList
        {
            get { return isVisbleDownloadList; }
            set { isVisbleDownloadList = value; RaisePropertyChanged(); }
        }
        private bool isVisbleInspectDownloadDWList { get; set; }
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

        private bool _IsEnabledAddJoints;
        public bool IsEnabledAddJoints
        {
            get { return _IsEnabledAddJoints; }
            set { SetProperty(ref _IsEnabledAddJoints, value); }
        }

        private ObservableCollection<string> _AddTestPackList;
        public ObservableCollection<string> AddTestPackList
        {
            get { return _AddTestPackList; }
            set { SetProperty(ref _AddTestPackList, value); }
        }
        private string _SelectedTestPack;
        public string SelectedTestPack
        {
            get { return _SelectedTestPack; }
            //set { SetProperty(ref _SelectedTestPack, value); }
            set
            {
                if (SetProperty(ref _SelectedTestPack, value))
                {
                    FilterList();
                    OnPropertyChanged();
                }
            }
        }
        private string addSpoolDWGNo;
        public string AddSpoolDWGNo
        {
            get { return addSpoolDWGNo; }
            set { SetProperty(ref addSpoolDWGNo, value); }
        }
        private ObservableCollection<string> _AddJointNoList;
        public ObservableCollection<string> AddJointNoList
        {
            get { return _AddJointNoList; }
            set { SetProperty(ref _AddJointNoList, value); }
        }
        private string _SelectedJointNo;
        public string SelectedJointNo
        {
            get { return _SelectedJointNo; }
            //set { SetProperty(ref _SelectedJointNo, value); }
            set
            {
                if (SetProperty(ref _SelectedJointNo, value))
                {
                    FilterList();
                    OnPropertyChanged();
                }
            }
        }

        private bool isVisbleSpoolDWRList { get; set; }
        public bool IsVisbleSpoolDWRList
        {
            get { return isVisbleSpoolDWRList; }
            set { isVisbleSpoolDWRList = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<SpoolDWRModel> _SpoolDWRDownLoadList;
        public ObservableCollection<SpoolDWRModel> SpoolDWRDownLoadList
        {
            get { return _SpoolDWRDownLoadList; }
            set { _SpoolDWRDownLoadList = value; OnPropertyChanged(); }
        }

        private SpoolDWRModel _selectedSpoolDWRItem;
        public SpoolDWRModel SelectedSpoolDWRItem
        {
            get { return _selectedSpoolDWRItem; }
            set { _selectedSpoolDWRItem = value; RaisePropertyChanged(); }
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

        private ObservableCollection<DWR> _AddDWRDownLoadList;
        public ObservableCollection<DWR> AddDWRDownLoadList
        {
            get { return _AddDWRDownLoadList; }
            set { _AddDWRDownLoadList = value; OnPropertyChanged(); }
        }
        private DWR _SelectedAddDWRItem;
        public DWR SelectedAddDWRItem
        {
            get { return _SelectedAddDWRItem; }
            set { _SelectedAddDWRItem = value; RaisePropertyChanged(); }
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
        public ICommand SpoolNextBtnCommand
        {
            get
            {
                return new Command(OnClickSpoolNextButton);
            }
        }
        #endregion


        public DWR_AddJointViewModel(
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
            DWRHelper.DWRTargetType = typeof(DWR_AddJointViewModel);
            PageHeaderText = "Download page";
            GetUserDetails();
            JobSettingHeaderVisible = true;
            IsVisbleDownloadList = true;
            IsVisbleSelectedDownloadDWR = IsVisibleDownloadingProgressList = IsEnabledAddJoints = IsVisbleSpoolDWRList = false;   
        }
        private async Task GetUserDetails()
        {
            var UserDetailsList = await _userDetailsRepository.GetAsync(i => i.ID == Settings.UserID);
            if (UserDetailsList.Count > 0)
                UserDetail = UserDetailsList.FirstOrDefault();
        }
        private void OnBtnClick(string param)
        {
            if (param == "AddJointsAll")
            {
                List<DWR> selectedList = AddDWRDownLoadList.ToList();
                if (selectedList.Count > 0)
                {
                    selectedList.ForEach(i => i.Selected = true);
                    SelectedDWRList = new ObservableCollection<DWR>(selectedList);
                    IsVisbleSpoolDWRList = IsVisbleDownloadList = false;
                    IsVisbleSelectedDownloadDWR = true;
                }
                else
                    _userDialogs.AlertAsync("", "No DWR to Add", "Ok");
            }
            else if (param == "AddToDownloadJoints")
            {
                List<DWR> selectedList = AddDWRDownLoadList.Where(x => x.Selected == true).ToList();
                if (selectedList.Count > 0)
                {
                    SelectedDWRList = new ObservableCollection<DWR>(selectedList);
                    IsVisbleSpoolDWRList = IsVisbleDownloadList = false;
                    IsVisbleSelectedDownloadDWR = true;
                }
                else
                    _userDialogs.AlertAsync("", "Please select DWR", "Ok");

            }
            else if (param == "AddMore")
            {
                SelectedAddDWRItem = null;
                IsVisbleDownloadList = true;
                IsVisbleSelectedDownloadDWR = IsVisbleSpoolDWRList = false;
            }
            else if (param == "AddsearchdownloadList")
            {
                if (AddSpoolDWGNo == "" || String.IsNullOrWhiteSpace(AddSpoolDWGNo) || IsEnabledAddJoints)
                {
                    _userDialogs.AlertAsync("Please enter value in Spool ", "", "Ok");
                }
                else if (AddSpoolDWGNo != "" || !String.IsNullOrWhiteSpace(AddSpoolDWGNo))
                {
                    //Get Soool
                    string JsonSpoolString = ModsTools.WebServiceGet(ApiUrls.Url_GetSpools(Settings.ProjectID, Settings.JobCode, AddSpoolDWGNo), Settings.AccessToken);
                    var Soooldata = JsonConvert.DeserializeObject<List<string>>(JsonSpoolString);
                    List<SpoolDWRModel> SpoolData = Soooldata.Select(e => new SpoolDWRModel { Selected = false, SpoolDWR = e }).ToList();
                    if (SpoolData.Count > 0)
                    {
                        SpoolDWRDownLoadList = new ObservableCollection<SpoolDWRModel>(SpoolData);
                        IsVisbleDownloadList = false;
                        IsVisbleSpoolDWRList = true;
                    }
                    else
                        _userDialogs.AlertAsync("No Result found ", "", "Ok");
                }
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
                    IsVisbleSelectedDownloadDWR = IsVisbleSpoolDWRList = false;
                    IsVisibleDownloadingProgressList = IsVisibleProgressBar = true;
                    //List<string> subContractors = new List<string>();

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

                   var GroupedSubContractor =   SelectedDWRList.GroupBy(x => x.SubContractor, (key, group) => group.First());
                    foreach (DWR item in GroupedSubContractor)
                    {

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
                    }

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
                        string DWRJsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetDWR(Settings.ProjectID, Settings.JobCode, item.SpoolDrawingNo, item.JointNo), Settings.AccessToken);
                        ThisEReport = JsonConvert.DeserializeObject<T_EReports>(DWRJsonString);
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

                            if(ThisEReport.ID > 0)
                            await _eReportsRepository.QueryAsync<T_EReports>(@"DELETE FROM [T_EReports] WHERE [ID] = '" + ThisEReport.ID + "'");

                            ThisEReport.RowID = DWRdata.RowID;
                            ThisEReport.ModelName = Settings.ModelName;
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


                        ////string WeldProcessesJsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetWeldProcesses(Settings.ProjectID), Settings.AccessToken);
                        ////var WeldProcesses = JsonConvert.DeserializeObject<List<T_WeldProcesses>>(WeldProcessesJsonString);
                        ////WeldProcesses.ForEach(i => i.ProjectID = Settings.ProjectID);
                        ////await _WeldProcessesRepository.QueryAsync("DELETE FROM [T_WeldProcesses] Where ProjectID =" + Settings.ProjectID);
                        ////await _WeldProcessesRepository.InsertOrReplaceAsync(WeldProcesses);

                        //////string CheckEReportCompleteJsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetCheckEReportComplete(item.ID), Settings.AccessToken);

                        //////string GetSpoolsJsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetSpools(Settings.ProjectID, Settings.JobCode, "1"), Settings.AccessToken);
                        //////var GetSpools = JsonConvert.DeserializeObject<List<string>>(GetSpoolsJsonString);

                        //////string GetSpoolsWithSubContractorJsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetSpoolsWithSubContractor(Settings.ProjectID, Settings.JobCode, item.SubContractor, "1"), Settings.AccessToken);
                        //////var GetSpoolsWithSubContractor = JsonConvert.DeserializeObject<List<string>>(GetSpoolsWithSubContractorJsonString);

                        //////string GetJointsJsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetJoints(Settings.ProjectID, Settings.JobCode, item.SpoolDrawingNo), Settings.AccessToken);

                        //////string GetJointsWithSubContractorJsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetJointsWithSubContractor(Settings.ProjectID, Settings.JobCode, item.SubContractor, item.SpoolDrawingNo), Settings.AccessToken);

                        //////Get RTDefects
                        ////string RTDefectsJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getRTDefects(Settings.ProjectID), Settings.AccessToken);
                        ////var RTDefects = JsonConvert.DeserializeObject<List<string>>(RTDefectsJsonString);
                        ////if (RTDefects != null && RTDefects.Count > 0)
                        ////{
                        ////    await _RT_DefectsRepository.QueryAsync<T_RT_Defects>("DELETE FROM [T_RT_Defects]");
                        ////    foreach (string value in RTDefects)
                        ////    {

                        ////        T_RT_Defects RT_Defects = new T_RT_Defects();

                        ////        RT_Defects.RT_Defect_Code = value.Split(new string[] { " - " }, StringSplitOptions.None)[0].Trim();
                        ////        RT_Defects.Description = value.Split(new string[] { " - " }, StringSplitOptions.None)[1].Trim();

                        ////        await _RT_DefectsRepository.InsertOrReplaceAsync(RT_Defects);
                        ////    }
                        ////}

                        //////get Welders

                        ////string WeldersJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getWelders(Settings.ProjectID, Settings.JobCode, item.SubContractor), Settings.AccessToken);
                        ////var Welders = JsonConvert.DeserializeObject<List<string>>(WeldersJsonString);
                        ////await _weldersRepository.QueryAsync<T_Welders>("DELETE FROM [T_Welders] WHERE [Project_ID] = " + Settings.ProjectID + " AND [SubContractor] = '" + item.SubContractor + "'");

                        ////foreach (string value in Welders)
                        ////{
                        ////    T_Welders Welder = new T_Welders();
                        ////    Welder.Welder_Code = value.Split(new string[] { " - " }, StringSplitOptions.None)[0].Trim().Replace("'", "''");
                        ////    Welder.Welder_Name = value.Split(new string[] { " - " }, StringSplitOptions.None)[1].Trim().Replace("'", "''");

                        ////    Welder.Project_ID = Settings.ProjectID;
                        ////    Welder.SubContractor = item.SubContractor;
                        ////    await _weldersRepository.InsertOrReplaceAsync(Welder);
                        ////}


                        //////Get HeatNumbers
                        ////string HeatNumbersJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getHeatNumbers(Settings.ProjectID, Settings.JobCode), Settings.AccessToken);
                        ////var HeatNumbers = JsonConvert.DeserializeObject<List<string>>(HeatNumbersJsonString);
                        ////if (HeatNumbers != null && HeatNumbers.Count > 0)
                        ////{
                        ////    await _DWR_HeatNosRepository.QueryAsync<T_DWR_HeatNos>("DELETE FROM [T_DWR_HeatNos] WHERE [Project_ID] = '" + Settings.ProjectID + "' AND Updated = 0 ");
                        ////    foreach (string value in HeatNumbers)
                        ////    {
                        ////        T_DWR_HeatNos DWR_HeatNos = new T_DWR_HeatNos();

                        ////        DWR_HeatNos.Project_ID = Settings.ProjectID;
                        ////        DWR_HeatNos.Ident_Code = value.Split(new string[] { " - " }, 2, StringSplitOptions.None)[0].Trim();
                        ////        DWR_HeatNos.Heat_No = value.Split(new string[] { " - " }, 2, StringSplitOptions.None)[1].Trim();

                        ////        await _DWR_HeatNosRepository.InsertOrReplaceAsync(DWR_HeatNos);
                        ////    }
                        ////}


                        //////Get WPS
                        ////string WPSNosJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getWPSNos(Settings.ProjectID, Settings.JobCode), Settings.AccessToken);
                        ////var WPSNos = JsonConvert.DeserializeObject<List<string>>(WPSNosJsonString);
                        ////if (WPSNos != null && WPSNos.Count > 0)
                        ////{
                        ////    await _WPS_MSTRRepository.QueryAsync<T_WPS_MSTR>("DELETE FROM [T_WPS_MSTR] WHERE [Project_ID] = " + Settings.ProjectID);
                        ////    foreach (string value in WPSNos)
                        ////    {
                        ////        T_WPS_MSTR WPS_MSTR = new T_WPS_MSTR();
                        ////        WPS_MSTR.Project_ID = Settings.ProjectID;
                        ////        WPS_MSTR.Wps_No = value.Split(new string[] { " - " }, StringSplitOptions.None)[0].Trim().Replace("'", "''");
                        ////        WPS_MSTR.Description = value.Split(new string[] { " - " }, StringSplitOptions.None)[1].Trim().Replace("'", "''");

                        ////        await _WPS_MSTRRepository.InsertOrReplaceAsync(WPS_MSTR);
                        ////    }
                        ////}

                        //////Get BaseMetals
                        ////string BaseMetalsJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getBaseMetals(Settings.ProjectID), Settings.AccessToken);
                        ////var BaseMetalsList = JsonConvert.DeserializeObject<List<string>>(BaseMetalsJsonString);

                        ////if (BaseMetalsList != null && BaseMetalsList.Count > 0)
                        ////{
                        ////    await _BaseMetalRepository.QueryAsync<T_BaseMetal>("DELETE FROM [T_BaseMetal]");
                        ////    foreach (string value in BaseMetalsList)
                        ////    {
                        ////        T_BaseMetal BaseMetal = new T_BaseMetal
                        ////        {
                        ////            Base_Material = value,
                        ////        };
                        ////        await _BaseMetalRepository.InsertOrReplaceAsync(BaseMetal);
                        ////    }
                        ////}

                        //Get Signatures
                        //string JsonResonceString = ModsTools.WebServiceGet(ApiUrls.Url_getEReport(item.ID, Settings.UnitID), Settings.AccessToken);
                        //ThisEReport = JsonConvert.DeserializeObject<T_EReports>(JsonResonceString);
                        //if (ThisEReport != null && ThisEReport.Signatures.Count > 0)
                        if (ThisEReport != null && ThisEReport.Signatures != null)
                        {
                            if (ThisEReport.Signatures.Count > 0)
                            {
                                ThisEReport.Signatures.ForEach(x => x.RowID = ThisEReport.RowID);
                                if(ThisEReport.ID > 0)
                                   await _signaturesRepository.QueryAsync<T_EReports_Signatures>("DELETE FROM [T_EReports_Signatures] WHERE [EReportID] = " + ThisEReport.ID);
                                  
                                await _signaturesRepository.InsertOrReplaceAsync(ThisEReport.Signatures);
                             var data =   await _signaturesRepository.GetAsync();
                            }
                        }
                        //Get UseAssinged 
                        //if (ThisEReport != null && ThisEReport.UsersAssigned.Count > 0)
                        if (ThisEReport != null && ThisEReport.UsersAssigned != null)
                        {
                            if (ThisEReport.UsersAssigned.Count > 0)
                            {
                                ThisEReport.UsersAssigned.ForEach(x => x.RowID = ThisEReport.RowID);
                                if (ThisEReport.ID > 0)
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
        private void OnClickSpoolNextButton()
        {
            Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.ShowLoading("Loading ...", MaskType.Black));
            Task.Run(async () => {SpoolNextButton();})
                .ContinueWith(result => Device.BeginInvokeOnMainThread(() => { UserDialogs.Instance.HideLoading();}));
        }
        private async void SpoolNextButton()
        {
            try
            {
                List<SpoolDWRModel> SDlist = SpoolDWRDownLoadList.Where(x => x.Selected == true).ToList();
                if (SDlist.Count > 0)
                {
                    DWRList = new List<DWR>();
                    foreach (SpoolDWRModel CurrentSpoolDWR in SDlist)
                    {
                        string JsonJointString = string.Empty;
                        if (UserDetail.Company_Category_Code.ToUpper() == "S")
                            JsonJointString = ModsTools.WebServiceGet(ApiUrls.Url_GetJointsWithSubContractor(Settings.ProjectID, Settings.JobCode, UserDetail.Company_Code, CurrentSpoolDWR.SpoolDWR), Settings.AccessToken);
                        else
                            JsonJointString = ModsTools.WebServiceGet(ApiUrls.Url_GetJoints(Settings.ProjectID, Settings.JobCode, CurrentSpoolDWR.SpoolDWR), Settings.AccessToken);
                        var Jointdata = JsonConvert.DeserializeObject<List<string>>(JsonJointString);

                        foreach (string Joint in Jointdata)
                        {
                            string DWRJsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetDWR(Settings.ProjectID, Settings.JobCode, CurrentSpoolDWR.SpoolDWR, Joint), Settings.AccessToken);
                            var EReportsdata = JsonConvert.DeserializeObject<T_EReports>(DWRJsonString);
                            DWRList.Add(JsonConvert.DeserializeObject<DWR>(EReportsdata.JSONString));
                        }
                    }
                    BindAddJointDDL();
                    AddDWRDownLoadList = new ObservableCollection<DWR>(DWRList);
                    IsVisbleDownloadList = true;
                    IsVisbleSelectedDownloadDWR = IsVisibleDownloadingProgressList = IsEnabledAddJoints = IsVisbleSpoolDWRList = false;
                   
                }
                else
                  await _userDialogs.AlertAsync("", "Please select Spool Drawing", "Ok");

            }
            catch (Exception ex)
            {
            }
        }
        private async void BindAddJointDDL()
        {
            try
            {
                if (DWRList.Count > 0)
                {
                    AddTestPackList = new ObservableCollection<string>(DWRList.Where(x => x.TestPackage != null).Select(x => x.TestPackage).Distinct().OrderBy(x => x).ToList());
                    AddTestPackList.Insert(0, "- Select -");
                    SelectedTestPack = AddTestPackList.FirstOrDefault();
                    AddJointNoList = new ObservableCollection<string>(DWRList.Where(x => x.JointNo != null).Select(x => x.JointNo).Distinct().OrderBy(x => x).ToList());
                    AddJointNoList.Insert(0, "- Select -");
                    SelectedJointNo = AddJointNoList.FirstOrDefault();
                    IsEnabledAddJoints = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private async void FilterList()
        {
            List<DWR> AddJointdata = DWRList;
            if (SelectedTestPack != "- Select -" && !string.IsNullOrEmpty(SelectedTestPack))
            {
                AddJointdata = AddJointdata.Where(x => x.TestPackage == SelectedTestPack).ToList();
            }
            if (SelectedJointNo != "- Select -" && !string.IsNullOrEmpty(SelectedJointNo))
            {
                AddJointdata = AddJointdata.Where(x => x.JointNo == SelectedJointNo).ToList();
            }
            AddDWRDownLoadList = new ObservableCollection<DWR>(AddJointdata);
        }
        private async void OnClickCloseButton()
        {
            await navigationService.NavigateFromMenuAsync(typeof(JointDetailsViewModel));
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

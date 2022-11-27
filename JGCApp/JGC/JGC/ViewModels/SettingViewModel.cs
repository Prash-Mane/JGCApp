using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Extentions;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.DataBase.DataTables.WorkPack;
using JGC.Models;
using JGC.ViewModel;
using Newtonsoft.Json;
using Plugin.Media;
using Prism.Navigation;
using Xamarin.Forms;

namespace JGC.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {       
        private readonly IRepository<T_Setting> _settingtable;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_EReports> _eReportsRepository;
        private readonly IRepository<T_EReports_Signatures> _signaturesRepository;
        private readonly IRepository<T_EReports_UsersAssigned> _usersAssignedRepository;
        private readonly IRepository<T_Drawings> _drawingsRepository;
        private readonly IRepository<T_CMR_HeatNos> _CMR_HeatNosRepository;
        private readonly IRepository<T_Welders> _weldersRepository;
        private readonly IRepository<T_DWR_HeatNos> _DWR_HeatNosRepository;
        private readonly IRepository<T_WPS_MSTR> _WPS_MSTRRepository;
        private readonly IRepository<T_RT_Defects> _RT_DefectsRepository;
        private readonly IRepository<T_StorageAreas> _StorageAreasRepository;
        private readonly IRepository<T_CMR_StorageAreas> _CMS_AllStorageAreasRepository;
        private readonly IRepository<T_BaseMetal> _BaseMetalRepository;
        private readonly IRepository<T_UserDetails> _UserDetails;
        private readonly IRepository<T_BaseMetal> _BaseMetal; 
        private readonly IRepository<T_PartialRequest> _PartialRequest;
        private readonly IRepository<T_UnitID> _unitIDRepository;
        private readonly IRepository<T_WeldProcesses> _WeldProcessesRepository;
        private readonly IRepository<T_DWR> _DWRRepository;


        private readonly IRepository<T_ETestPackages> _eTestPackagesRepository;
        private readonly IRepository<T_ControlLogSignature> _controlLogSignatureRepository;
        private readonly IRepository<T_AttachedDocument> _attachedDocumentRepository;
        private readonly IRepository<T_PunchList> _punchListRepository;
        private readonly IRepository<T_PunchImage> _punchImageRepository;
        private readonly IRepository<T_AdminFolders> _adminFoldersRepository;
        private readonly IRepository<T_AdminControlLog> _adminControlLogRepository;
        private readonly IRepository<T_AdminPresetPunches> _adminPresetPunchesRepository;
        private readonly IRepository<T_AdminPunchCategories> _adminPunchCategoriesRepository;
        private readonly IRepository<T_AdminFunctionCodes> _adminFunctionCodesRepository;
        private readonly IRepository<T_AdminPunchLayer> _adminPunchLayerRepository;
        private readonly IRepository<T_AdminControlLogFolder> _adminControlLogFolderRepository;
        private readonly IRepository<T_AdminDrainRecordContent> _adminDrainRecordContentRepository;
        private readonly IRepository<T_AdminDrainRecordAcceptedBy> _adminDrainRecordAcceptedBy;
        private readonly IRepository<T_AdminTestRecordDetails> _adminTestRecordDetailsRepository;
        private readonly IRepository<T_AdminTestRecordConfirmation> _adminTestRecordConfirmationRepository;
        private readonly IRepository<T_AdminTestRecordAcceptedBy> _adminTestRecordAcceptedByRepository;
        private readonly IRepository<T_DrainRecordContent> _drainRecordContentRepository;
        private readonly IRepository<T_DrainRecordAcceptedBy> _drainRecordAcceptedByRepository;
        private readonly IRepository<T_TestRecordDetails> _testRecordDetailsRepository;
        private readonly IRepository<T_TestRecordConfirmation> _testRecordConfirmationRepository;
        private readonly IRepository<T_TestRecordAcceptedBy> _testRecordAcceptedByRepository;
        private readonly IRepository<T_TestRecordImage> _testRecordImageRepository;
        private readonly IRepository<T_TestLimitDrawing> _testLimitDrawingRepository;
        private readonly IRepository<T_AdminControlLogPunchLayer> _adminControlLogPunchLayerRepository;
        private readonly IRepository<T_AdminControlLogPunchCategory> _adminControlLogPunchCategoryRepository;
        private readonly IRepository<T_AdminControlLogActionParty> _adminControlLogActionPartyRepository;
        private readonly IRepository<T_AdminControlLogNaAutoSignatures> _adminControlLogNaAutoSignaturesRepository;


        private readonly IRepository<T_IWP> _iwpRepository;
        private readonly IRepository<T_IWPStatus> _iwpStatusRepository;
        private readonly IRepository<T_Predecessor> _predecessorRepository;
        private readonly IRepository<T_Successor> _successorRepository;
        private readonly IRepository<T_IWPDrawings> _iwpDrawingsRepository;
        private readonly IRepository<T_IWPAttachments> _iwpAttachmentsRepository;
        private readonly IRepository<T_ManPowerResource> _manPowerResourceRepository;
        private readonly IRepository<T_ManPowerLog> _manPowerLogRepository;
        private readonly IRepository<T_TagMilestoneStatus> _tagMilestoneStatusRepository;
        private readonly IRepository<T_TagMilestoneImages> _tagMilestoneImagesRepository;
        private readonly IRepository<T_IWPAdminControlLog> _iwpAdminControlLogRepository;
        private readonly IRepository<T_IWPControlLogSignatures> _iwpControlLogSignaturesRepository;
        private readonly IRepository<T_IWPPunchCategory> _iwpPunchCategoryRepository;
        private readonly IRepository<T_IWPPunchLayer> _iwpPunchLayerRepository;
        private readonly IRepository<T_CWPDrawings> _cwpDrawingsRepository;
        private readonly IRepository<T_WorkerScanned> _workerScannedRepository;
        private readonly IRepository<T_IWPPunchControlItem> _iwpPunchControlItemRepository;
        private readonly IRepository<T_IWPPunchImage> _iwpPunchImagesRepository;
        private readonly IRepository<T_IWPPunchCategories> _iWPPunchCategoriesRepository;
        private readonly IRepository<T_IWPFunctionCodes> _iWPFunctionCodesRepository;
        private readonly IRepository<T_IWPAdminPunchLayer> _iWPAdminPunchLayerRepository;
        private readonly IRepository<T_IWPCompanyCategoryCodes> _iWPCompanyCategoryCodesRepository;


        private readonly IUserDialogs _userDialogs;
        private T_UserProject userProject;
        PageHelper _pageHelper = CheckValidLogin._pageHelper;
        CompletionPageHelper _CompletionpageHelper = CheckValidLogin._CompletionpageHelper;
        private ObservableCollection<T_EReports> eReports;
        public ObservableCollection<T_EReports> EReports
        {
            get { return eReports; }
            set { eReports = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_Setting> _SettingDetails;
        public ObservableCollection<T_Setting> SettingDetails
        {
            get { return _SettingDetails; }
            set { SetProperty(ref _SettingDetails, value); }
        }

        private ObservableCollection<string> _cameraName;
        public ObservableCollection<string> CameraNames
        {
            get { return _cameraName; }
            set { SetProperty(ref _cameraName, value);}
        }
        private string _selectedCamera;
        public string SelectedCamera
        {
            get { return _selectedCamera; }
            set
            {
                if (SetProperty(ref _selectedCamera, value))
                {
                    LoadCameraDetails(_selectedCamera);
                    OnPropertyChanged();
                }
            }
        }
        private string height;
        public string Height
        {
            get { return height; }
            set
            {
                SetProperty(ref height, value);
            }
        }
        private string width;
        public string Width
        {
            get { return width; }
            set
            {
                SetProperty(ref width, value);
            }
        }

        private string unitID;
        public string UnitID
        {
            get { return unitID; }
            set
            {
                SetProperty(ref unitID, value);
            }
        }

        private bool isVisibleFactoryReset;
        public bool IsVisibleFactoryReset
        {
            get { return isVisibleFactoryReset; }
            set
            {
                SetProperty(ref isVisibleFactoryReset, value);
                RaisePropertyChanged();
            }
        }
        private string BGColor;
        public string Button_BGColor
        {
            get { return BGColor; }
            set
            {
                SetProperty(ref BGColor, value);
            }
        }

        #region Delegate Commands  
        public ICommand BtnCommand
        {
            get
            {
                return new Command<string>(OnClickButton);
            }
        }
        #endregion

        public SettingViewModel(INavigationService navigationService,
                                IHttpHelper httpHelper,  
                                ICheckValidLogin _checkValidLogin,
                                IUserDialogs _userDialogs,
                                IRepository<T_Setting> _settingtable,
                                IRepository<T_UserProject> _userProjectRepository,
                                IRepository<T_EReports> _eReportsRepository,
                                IRepository<T_EReports_Signatures> _signaturesRepository,
                                IRepository<T_EReports_UsersAssigned> _usersAssignedRepository,
                                IRepository<T_Drawings> _drawingsRepository,
                                IRepository<T_CMR_HeatNos> _CMR_HeatNosRepository,
                                IRepository<T_Welders> _weldersRepository,
                                IRepository<T_DWR_HeatNos> _DWR_HeatNosRepository,
                                IRepository<T_WPS_MSTR> _WPS_MSTRRepository,
                                IRepository<T_RT_Defects> _RT_DefectsRepository,
                                IRepository<T_StorageAreas> _StorageAreasRepository,
                                IRepository<T_CMR_StorageAreas> _CMS_AllStorageAreasRepository,
                                IRepository<T_BaseMetal> _BaseMetalRepository,
                                IRepository<T_UserDetails> _UserDetails,
                                IRepository<T_BaseMetal> _BaseMetal,
                                IRepository<T_UnitID> unitIDRepository,
                                IRepository<T_PartialRequest> _PartialRequest,
                                IRepository<T_WeldProcesses> _WeldProcessesRepository,
                                IRepository<T_DWR> _DWRRepository,

                                IRepository<T_ETestPackages> _eTestPackagesRepository,
                                IRepository<T_ControlLogSignature> _controlLogSignatureRepository,
                                IRepository<T_AttachedDocument> _attachedDocumentRepository,
                                IRepository<T_PunchList> _punchListRepository,
                                IRepository<T_PunchImage> _punchImageRepository,
                                IRepository<T_AdminFolders> _adminFoldersRepository,
                                IRepository<T_AdminControlLog> _adminControlLogRepository,
                                IRepository<T_AdminPresetPunches> _adminPresetPunchesRepository,
                                IRepository<T_AdminPunchCategories> _adminPunchCategoriesRepository,
                                IRepository<T_AdminFunctionCodes> _adminFunctionCodesRepository,
                                IRepository<T_AdminPunchLayer> _adminPunchLayerRepository,
                                IRepository<T_AdminControlLogFolder> _adminControlLogFolderRepository,
                                IRepository<T_AdminDrainRecordContent> _adminDrainRecordContentRepository,
                                IRepository<T_AdminDrainRecordAcceptedBy> _adminDrainRecordAcceptedBy,
                                IRepository<T_AdminTestRecordDetails> _adminTestRecordDetailsRepository,
                                IRepository<T_AdminTestRecordConfirmation> _adminTestRecordConfirmationRepository,
                                IRepository<T_AdminTestRecordAcceptedBy> _adminTestRecordAcceptedByRepository,
                                IRepository<T_DrainRecordContent> _drainRecordContentRepository,
                                IRepository<T_DrainRecordAcceptedBy> _drainRecordAcceptedByRepository,
                                IRepository<T_TestRecordDetails> _testRecordDetailsRepository,
                                IRepository<T_TestRecordConfirmation> _testRecordConfirmationRepository,
                                IRepository<T_TestRecordAcceptedBy> _testRecordAcceptedByRepository,
                                IRepository<T_TestRecordImage> _testRecordImageRepository,
                                IRepository<T_TestLimitDrawing> _testLimitDrawingRepository,
                                IRepository<T_AdminControlLogPunchLayer> _adminControlLogPunchLayerRepository,
                                IRepository<T_AdminControlLogPunchCategory> _adminControlLogPunchCategoryRepository,
                                IRepository<T_AdminControlLogActionParty> _adminControlLogActionPartyRepository,
                                IRepository<T_AdminControlLogNaAutoSignatures> _adminControlLogNaAutoSignaturesRepository,

                                IRepository<T_IWP> _iwpRepository,
                                IRepository<T_IWPStatus> _iwpStatusRepository,
                                IRepository<T_Predecessor> _predecessorRepository,
                                IRepository<T_Successor> _successorRepository,
                                IRepository<T_IWPDrawings> _iwpDrawingsRepository,
                                IRepository<T_IWPAttachments> _iwpAttachmentsRepository,
                                IRepository<T_ManPowerLog> _manPowerLogRepository,
                                IRepository<T_TagMilestoneStatus> _tagMilestoneStatusRepository,
                                IRepository<T_TagMilestoneImages> _tagMilestoneImagesRepository,
                                IRepository<T_ManPowerResource> _manPowerResourceRepository,
                                IRepository<T_IWPAdminControlLog> _iwpAdminControlLogRepository,
                                IRepository<T_IWPControlLogSignatures> _iwpControlLogSignaturesRepository,
                                IRepository<T_IWPPunchCategory> _iwpPunchCategoryRepository,
                                IRepository<T_IWPPunchLayer> _iwpPunchLayerRepository,
                                IRepository<T_WorkerScanned> _workerScannedRepository,
                                IRepository<T_CWPDrawings> _cwpDrawingsRepository,
                                IRepository<T_IWPPunchControlItem> _iwpPunchControlItemRepository,
                                IRepository<T_IWPPunchImage> _iwpPunchImagesRepository,
                                IRepository<T_IWPPunchCategories> _iWPPunchCategoriesRepository,
                                IRepository<T_IWPFunctionCodes> _iWPFunctionCodesRepository,
                                IRepository<T_IWPAdminPunchLayer> _iWPAdminPunchLayerRepository,
                                IRepository<T_IWPCompanyCategoryCodes> _iWPCompanyCategoryCodesRepository) 
        : base(navigationService, httpHelper, _checkValidLogin)
        {
            this._settingtable = _settingtable;
            this._userDialogs = _userDialogs;
            this._userProjectRepository = _userProjectRepository;
            this._eReportsRepository = _eReportsRepository;
            this._signaturesRepository = _signaturesRepository;
            this._usersAssignedRepository = _usersAssignedRepository;
            this._drawingsRepository = _drawingsRepository;
            this._CMR_HeatNosRepository = _CMR_HeatNosRepository;
            this._weldersRepository = _weldersRepository;
            this._DWR_HeatNosRepository = _DWR_HeatNosRepository;
            this._WPS_MSTRRepository = _WPS_MSTRRepository;
            this._RT_DefectsRepository = _RT_DefectsRepository;
            this._StorageAreasRepository = _StorageAreasRepository;
            this._CMS_AllStorageAreasRepository = _CMS_AllStorageAreasRepository;
            this._BaseMetalRepository = _BaseMetalRepository;
            this._UserDetails = _UserDetails;
            this._BaseMetal = _BaseMetal;
            this._PartialRequest = _PartialRequest;
            this._unitIDRepository = unitIDRepository;
            this._WeldProcessesRepository = _WeldProcessesRepository;
            this._DWRRepository = _DWRRepository;


            this._eTestPackagesRepository = _eTestPackagesRepository;
            this._controlLogSignatureRepository = _controlLogSignatureRepository;
            this._attachedDocumentRepository = _attachedDocumentRepository;
            this._punchListRepository = _punchListRepository;
            this._punchImageRepository = _punchImageRepository;
            this._adminFoldersRepository = _adminFoldersRepository;
            this._adminControlLogRepository = _adminControlLogRepository;
            this._adminPresetPunchesRepository = _adminPresetPunchesRepository;
            this._adminPunchCategoriesRepository = _adminPunchCategoriesRepository;
            this._adminFunctionCodesRepository = _adminFunctionCodesRepository;
            this._adminPunchLayerRepository = _adminPunchLayerRepository;
            this._adminControlLogFolderRepository = _adminControlLogFolderRepository;
            this._adminDrainRecordContentRepository = _adminDrainRecordContentRepository;
            this._adminDrainRecordAcceptedBy = _adminDrainRecordAcceptedBy;
            this._adminTestRecordDetailsRepository = _adminTestRecordDetailsRepository;
            this._adminTestRecordConfirmationRepository = _adminTestRecordConfirmationRepository;
            this._adminTestRecordAcceptedByRepository = _adminTestRecordAcceptedByRepository;
            this._drainRecordContentRepository = _drainRecordContentRepository;
            this._drainRecordAcceptedByRepository = _drainRecordAcceptedByRepository;
            this._testRecordDetailsRepository = _testRecordDetailsRepository;
            this._testRecordConfirmationRepository = _testRecordConfirmationRepository;
            this._testRecordAcceptedByRepository = _testRecordAcceptedByRepository;
            this._testRecordImageRepository = _testRecordImageRepository;
            this._testLimitDrawingRepository = _testLimitDrawingRepository;
            this._adminControlLogPunchLayerRepository = _adminControlLogPunchLayerRepository;
            this._adminControlLogPunchCategoryRepository = _adminControlLogPunchCategoryRepository;
            this._adminControlLogActionPartyRepository = _adminControlLogActionPartyRepository;
            this._adminControlLogNaAutoSignaturesRepository = _adminControlLogNaAutoSignaturesRepository;

            this._iwpRepository = _iwpRepository;
            this._iwpStatusRepository = _iwpStatusRepository;
            this._predecessorRepository = _predecessorRepository;
            this._successorRepository = _successorRepository;
            this._iwpDrawingsRepository = _iwpDrawingsRepository;
            this._iwpAttachmentsRepository = _iwpAttachmentsRepository;
            this._manPowerResourceRepository = _manPowerResourceRepository;
            this._manPowerLogRepository = _manPowerLogRepository;
            this._tagMilestoneStatusRepository = _tagMilestoneStatusRepository;
            this._tagMilestoneImagesRepository = _tagMilestoneImagesRepository;
            this._iwpAdminControlLogRepository = _iwpAdminControlLogRepository;
            this._iwpControlLogSignaturesRepository = _iwpControlLogSignaturesRepository;
            this._iwpPunchCategoryRepository = _iwpPunchCategoryRepository;
            this._iwpPunchLayerRepository = _iwpPunchLayerRepository;
            this._cwpDrawingsRepository = _cwpDrawingsRepository;
            this._workerScannedRepository = _workerScannedRepository;
            this._iwpPunchControlItemRepository = _iwpPunchControlItemRepository;
            this._iwpPunchImagesRepository = _iwpPunchImagesRepository;
            this._iWPPunchCategoriesRepository = _iWPPunchCategoriesRepository;
            this._iWPFunctionCodesRepository = _iWPFunctionCodesRepository;
            this._iWPAdminPunchLayerRepository = _iWPAdminPunchLayerRepository;
            this._iWPCompanyCategoryCodesRepository = _iWPCompanyCategoryCodesRepository;

            Button_BGColor = Settings.ModuleName == "EReporter" ? "#FB1610" : Settings.ModuleName == "TestPackage" ? "#C4BB46" : Settings.ModuleName == "JobSetting" ? "#3B87C7" : "Gray";
            PageHeaderText = "Setting";
            getcamera();
            CheckUser();
           

        }

        private async void CheckUser()
        {
            var check = await _UserDetails.GetAsync(x => x.ID == Settings.UserID);
            if (check.FirstOrDefault().ModsUser)
            {
                IsVisibleFactoryReset = true;
            }
            else
            {
                IsVisibleFactoryReset = false;
            }
        }
        public async void getcamera()
        {
            var Settinginfo = await _settingtable.GetAsync();
            if (Settinginfo.Count <= 0)
            {
                T_Setting data = new T_Setting {
                                                ID = 1,
                                                Name = "Camera",
                                                Height = 480,
                                                Width = 640,
                                                Error=""
                                               };                
                await _settingtable.InsertOrReplaceAsync(data);             

            }
            var settingdata = await _settingtable.GetAsync();
            SettingDetails = new ObservableCollection<T_Setting>(settingdata);

            CameraNames = new ObservableCollection<string>(SettingDetails.Select(i => i.Name).ToList());
            LoadCameraDetails(CameraNames.FirstOrDefault());

        }
        private async void LoadCameraDetails(string Cameraselected)
        {
            if (Cameraselected == null)
                return;
            SelectedCamera = Cameraselected;
            var set = SettingDetails.Where(i => i.Name == Cameraselected).ToList();
           foreach(var item in set)
            {
                Height = item.Height.ToString();
                Width = item.Width.ToString();
            }
            UnitID = Settings.UnitID;
        }
        private void OnClickButton(string param)
        {
            if (param == "SaveCameraSetting")
            {
                SaveCameraSetting();
            }
            else if (param == "SaveUnitID")
            {
                SaveUnitID();
            }
            else if (param == "FactoryReset")
            {
                FactoryReset();
            }
            else if(param == "ClearSaveData")
            {
                ClearSaveData();
            }
            else if(param == "PrivacyPolicy")
            {
                Device.OpenUri(new System.Uri("https://www.modsvm.com/privacy-policy"));
            }
        }

        public async void SaveCameraSetting()
        {
            if (StringToIntCheck(Height + Width))
            {
                List<T_Setting> update = SettingDetails.Where(i => i.Name == SelectedCamera).ToList();
                foreach (var item in update)
                {
                    await _settingtable.QueryAsync<T_Setting>(@"UPDATE T_Setting SET Height = " + Convert.ToUInt32(Height) + ", Width = " + Convert.ToUInt32(Width) + " WHERE ID = " + item.ID);
                    _userDialogs.AlertAsync("Saved successfully", "Saved Data", "Ok");
                }
            }
            else
                _userDialogs.AlertAsync("Unable to save setting", "Invalid input", "Ok");
        }
        public async void SaveUnitID()
        {
            if (!string.IsNullOrEmpty(UnitID))
            {
                T_UnitID unitid = new T_UnitID { ID = 1, DeviceID = UnitID };
                await _unitIDRepository.UpdateAsync(unitid);
                Settings.UnitID = UnitID;
                _userDialogs.AlertAsync("Saved successfully", "Saved", "Ok");
            }
          


        }
        public async void FactoryReset()
        {
            if (await _userDialogs.ConfirmAsync("Are you sure you want to restore to factory default?", "Factory Reset","Yes","No"))
            {

                try
                {
                    var UserProjectList = await _userProjectRepository.GetAsync();
                    if (UserProjectList.Count > 0)
                        userProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();
                    if (Settings.ModuleName == "EReporter")
                    {
                        var EReportIDs = await _eReportsRepository.QueryAsync<T_EReports>(@"SELECT [ID] FROM [T_EReports]");

                        var AllIDs = EReportIDs.Select(i => i.ID).ToList();
                        string JoinIDs = string.Empty;
                        if (AllIDs.Count() > 0)
                        {
                            JoinIDs = "(" + string.Join(",", AllIDs) + ")";
                        }
                        Boolean CancelRemove = false;
                        if (AllIDs != null && JoinIDs.Length > 0)
                        {
                            foreach (long id in AllIDs)
                            {
                                var ereport = await _eReportsRepository.QueryAsync<T_EReports>(@"SELECT [ReportNo] FROM [T_EReports] WHERE [ID] = " + id);
                                string reportNo = ereport.Select(i => i.ReportNo).ToString();

                                Boolean Result = ModsTools.WebServicePostBoolean(ApiUrls.Url_UnlockEReport(id), Settings.AccessToken);
                                if (Result)
                                {
                                    //AddToProgressPanel(reportNo + " unlocked");
                                }
                                else
                                {
                                    //AddToProgressPanel("Error unlocking " + reportNo);
                                    CancelRemove = true;
                                    break;
                                }
                            }
                            if (!CancelRemove)
                            {

                               
                                await _signaturesRepository.DeleteAll();
                                await _usersAssignedRepository.DeleteAll();
                                await _PartialRequest.DeleteAll();
                                await _RT_DefectsRepository.DeleteAll();
                                await _StorageAreasRepository.DeleteAll();
                                await _CMR_HeatNosRepository.DeleteAll();
                                await _CMS_AllStorageAreasRepository.DeleteAll();
                                await _weldersRepository.DeleteAll();
                                await _DWR_HeatNosRepository.DeleteAll();
                                await _WPS_MSTRRepository.DeleteAll();
                                await _BaseMetalRepository.DeleteAll();
                                await _UserDetails.DeleteAll();
                                await _BaseMetal.DeleteAll();
                                await _UserDetails.DeleteAll();
                                await _userProjectRepository.DeleteAll();
                                await _WeldProcessesRepository.DeleteAll();

                                var ER_IDS = await _eReportsRepository.GetAsync();
                                foreach (T_EReports EReporter in ER_IDS.Where(x=>x.ID > 0))
                                {
                                    ModsTools.WebServicePostBoolean(ApiUrls.Url_UnlockEReport(EReporter.ID), Settings.AccessToken);
                                }
                                await _eReportsRepository.DeleteAll();
                                await _DWRRepository.DeleteAll();
                                




                                string LocationPDFPath = "PDF Store/" + userProject.JobCode;
                                await DependencyService.Get<ISaveFiles>().RemoveAllFilefromFolder(LocationPDFPath);

                                string LocationPhotoPath = "Photo Store/" + userProject.JobCode;
                                bool result = await DependencyService.Get<ISaveFiles>().RemoveAllFilefromFolder(LocationPhotoPath);

                                await _drawingsRepository.DeleteAll();

                                ////Finish
                                //_userDialogs.AlertAsync("Software Reset successfully", "Reset Data", "Ok");

                                //Settings.AccessToken = string.Empty;
                                //Settings.RenewalToken = string.Empty;
                                //Settings.DisplayName = string.Empty;
                                //Cache.accessToken = string.Empty;

                                //_pageHelper.TokenExpiry = DateTime.Today.AddDays(-1);

                                //await navigationService.NavigateAsync<LoginViewModel>();
                            }
                            else
                                _userDialogs.AlertAsync("Removing e-reports cancelled as error occurded when unlocking e-reports", null, "Ok");
                        }
                        else
                            _userDialogs.AlertAsync("No E-Reports to remove", null, "Ok");
                    }
                    else if (Settings.ModuleName == "TestPackage")
                    {
                        ResetAndClear(false);
                    }
                    else if (Settings.ModuleName == "JobSetting")
                    {
                        JobSettingResetAndClear(false);
                    }
                    //Finish
                    _userDialogs.AlertAsync("Software Reset successfully", "Reset Data", "Ok");
                    Settings.AccessToken = string.Empty;
                    Settings.RenewalToken = string.Empty;
                    Settings.DisplayName = string.Empty;
                    Cache.accessToken = string.Empty;
                    Settings.IsMODSApp = Settings.IsCompletionApp = false;

                    Settings.CompletionAccessToken = string.Empty;
                    _pageHelper.TokenExpiry = DateTime.Today.AddDays(-1);
                    _CompletionpageHelper.CompletionTokenExpiry = DateTime.Today.AddDays(-1);

                    await navigationService.NavigateAsync<LoginViewModel>();
                }
                catch (Exception e)
                {
                    _userDialogs.AlertAsync("Error occured E-Report(s) not removed", null, "Ok");
                }
               
            }
            //else
            //    _userDialogs.AlertAsync("Unable to clear data, you must have an internet connection to clear data");
        }
        public async void ClearSaveData()
        {
            if (await _userDialogs.ConfirmAsync($"Are you sure you want to clear all data?", $"Clear data", "Yes", "No"))
            {
                try
                {
                    var UserProjectList = await _userProjectRepository.GetAsync();
                    if (UserProjectList.Count > 0)
                        userProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();
                    if (Settings.ModuleName == "EReporter")
                    {
                        //string JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getEReportHeaders(userProject.ModelName, Settings.Report, userProject.User_ID, _pageHelper.UnitID), Settings.AccessToken);
                        //EReports = new ObservableCollection<T_EReports>(JsonConvert.DeserializeObject<List<T_EReports>>(JsonString));

                        var EReportIDs = await _eReportsRepository.QueryAsync<T_EReports>(@"SELECT [ID] FROM [T_EReports] WHERE upper(ModelName) = '" + userProject.ModelName.ToUpper() + "'");

                        var AllIDs = EReportIDs.Select(i => i.ID).ToList();
                        string JoinIDs = string.Empty;
                        if (AllIDs.Count() > 0)
                        {
                            JoinIDs = "(" + string.Join(",", AllIDs) + ")";
                        }
                        Boolean CancelRemove = false;
                        if (AllIDs != null && JoinIDs.Length > 0)
                        {
                            foreach (long id in AllIDs)
                            {
                                var ereport = await _eReportsRepository.QueryAsync<T_EReports>(@"SELECT [ReportNo] FROM [T_EReports] WHERE [ID] = " + id);
                                string reportNo = ereport.Select(i => i.ReportNo).ToString();

                                Boolean Result = ModsTools.WebServicePostBoolean(ApiUrls.Url_UnlockEReport(id), Settings.AccessToken);
                                if (Result)
                                {
                                    //AddToProgressPanel(reportNo + " unlocked");
                                }
                                else
                                {
                                    //AddToProgressPanel("Error unlocking " + reportNo);
                                    CancelRemove = true;
                                    break;
                                }
                            }
                            if (!CancelRemove)
                            {
                                await _eReportsRepository.QueryAsync<T_EReports>(@"DELETE FROM [T_EReports] WHERE upper(ModelName) = '" + userProject.ModelName.ToUpper() + "'");

                                if (JoinIDs != string.Empty)
                                    await _usersAssignedRepository.QueryAsync<T_EReports_UsersAssigned>("DELETE FROM [T_EReports_UsersAssigned] WHERE [EReportID] IN " + JoinIDs);

                                if (JoinIDs != string.Empty)
                                    await _signaturesRepository.QueryAsync<T_EReports_Signatures>("DELETE FROM [T_EReports_Signatures] WHERE [EReportID] IN " + JoinIDs);

                                if (JoinIDs != string.Empty)
                                    await _PartialRequest.QueryAsync<T_PartialRequest>("DELETE FROM [T_PartialRequest] WHERE [EReportID] IN " + JoinIDs);

                                await _StorageAreasRepository.QueryAsync<T_StorageAreas>("DELETE FROM [T_StorageAreas] WHERE [PROJECT_ID] = " + userProject.Project_ID);

                                await _CMS_AllStorageAreasRepository.QueryAsync<T_CMR_StorageAreas>("DELETE FROM [T_CMR_StorageAreas] WHERE [Project_ID] = " + userProject.Project_ID);

                                await _CMR_HeatNosRepository.QueryAsync<T_CMR_HeatNos>("DELETE FROM [T_CMR_HeatNos] WHERE [Project_ID] = " + userProject.Project_ID);

                                await _WPS_MSTRRepository.QueryAsync<T_WPS_MSTR>("DELETE FROM [T_WPS_MSTR] WHERE [Project_ID] = " + userProject.Project_ID);

                                await _weldersRepository.QueryAsync<T_Welders>("DELETE FROM [T_Welders] WHERE [Project_ID] = " + userProject.Project_ID);

                                await _DWR_HeatNosRepository.QueryAsync<T_DWR_HeatNos>("DELETE FROM [T_DWR_HeatNos] WHERE [Project_ID] = " + userProject.Project_ID);

                                await _WeldProcessesRepository.QueryAsync<T_WeldProcesses>("DELETE FROM [T_WeldProcesses] WHERE [ProjectID] = " + userProject.Project_ID);
                                await _DWRRepository.QueryAsync<T_DWR>("DELETE FROM [T_DWR] WHERE [ProjectID] = " + userProject.Project_ID);

                                string LocationPDFPath = "PDF Store/" + userProject.JobCode;
                                await DependencyService.Get<ISaveFiles>().RemoveAllFilefromFolder(LocationPDFPath);

                                string LocationPhotoPath = "Photo Store/" + userProject.JobCode;
                                bool result = await DependencyService.Get<ISaveFiles>().RemoveAllFilefromFolder(LocationPhotoPath);

                                await _drawingsRepository.QueryAsync<T_Drawings>("DELETE FROM [T_Drawings] WHERE [EReportID] IN " + JoinIDs);


                                //Finish
                                _userDialogs.AlertAsync("Data cleared successfully", "Cleared Data", "Ok");

                            }
                            else
                                _userDialogs.AlertAsync("Removing e-reports cancelled as error occurded when unlocking e-reports", null, "Ok");
                        }
                        else
                            _userDialogs.AlertAsync("No E-Reports to remove", null, "Ok");
                        // string EReportsInStatment = LocalSQLFunctions.SQLInStatmentValues(EReportIDs);

                        //string RTDefectsJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getRTDefects(), Settings.AccessToken);

                        //await _userProjectRepository.DeleteAll();
                        //await _eReportsRepository.DeleteAll();
                        //await _signaturesRepository.DeleteAll();
                        //await _usersAssignedRepository.DeleteAll();
                        //await _drawingsRepository.DeleteAll();
                        //await _CMR_HeatNosRepository.DeleteAll();
                        //await _weldersRepository.DeleteAll();
                        //await _DWR_HeatNosRepository.DeleteAll();
                        //await _WPS_MSTRRepository.DeleteAll();
                        //await _RT_DefectsRepository.DeleteAll();
                        //await _StorageAreasRepository.DeleteAll();
                        //await _CMS_AllStorageAreasRepository.DeleteAll();
                        //await _BaseMetalRepository.DeleteAll();
                        //await _UserDetails.DeleteAll();
                        //await _BaseMetal.DeleteAll();
                        //await _PartialRequest.DeleteAll();
                        //_userDialogs.HideLoading();
                        //_userDialogs.AlertAsync("Data cleared successfully", "Cleared Data", "Ok");
                    }
                    else if (Settings.ModuleName == "TestPackage")
                    {
                        ResetAndClear(true);
                    }
                    else if (Settings.ModuleName == "JobSetting")
                    {
                         JobSettingResetAndClear(true);
                    }
                }
                catch (Exception e)
                {
                    _userDialogs.AlertAsync("Error occured " + Settings.ModuleName + " not removed", null, "Ok");
                }
            }
        }
        public async void ResetAndClear(bool projectOnly)
        {
            string filter = projectOnly ? " WHERE [ProjectID] = '" + userProject.Project_ID + "'" : "";

            //string f = "DELETE FROM [T_AdminControlLog] " + filter;

            await _adminControlLogRepository.QueryAsync<T_AdminControlLog>("DELETE FROM [T_AdminControlLog] " + filter);

            await _adminFoldersRepository.QueryAsync("DELETE FROM [T_AdminFolders] " + filter);

            await _adminControlLogFolderRepository.QueryAsync("DELETE FROM [T_AdminControlLogFolder] " + filter);

            await _adminControlLogPunchLayerRepository.QueryAsync("DELETE FROM [T_AdminControlLogPunchLayer] " + filter);

            await _adminControlLogPunchCategoryRepository.QueryAsync("DELETE FROM [T_AdminControlLogPunchCategory]" + filter);

            await _adminControlLogActionPartyRepository.QueryAsync("DELETE FROM [T_AdminControlLogActionParty]" + filter);

            await _adminControlLogNaAutoSignaturesRepository.QueryAsync("DELETE FROM [T_AdminControlLogNaAutoSignatures]" + filter);

            await _adminFunctionCodesRepository.QueryAsync("DELETE FROM [T_AdminFunctionCodes]" + filter);

            await _adminPresetPunchesRepository.QueryAsync("DELETE FROM [T_AdminPresetPunches]" + filter);

            await _adminPunchCategoriesRepository.QueryAsync("DELETE FROM [T_AdminPunchCategories] " + filter);

            await _adminPunchLayerRepository.QueryAsync("DELETE FROM [T_AdminPunchLayer]" + filter);

            await _adminDrainRecordAcceptedBy.QueryAsync("DELETE FROM [T_AdminDrainRecordAcceptedBy]" + filter);

            await _adminDrainRecordContentRepository.QueryAsync("DELETE FROM [T_AdminDrainRecordContent]" + filter);

            await _adminTestRecordDetailsRepository.QueryAsync("DELETE FROM [T_AdminTestRecordDetails]" + filter);

            await _adminTestRecordConfirmationRepository.QueryAsync("DELETE FROM [T_AdminTestRecordConfirmation]" + filter);

            await _adminTestRecordAcceptedByRepository.QueryAsync("DELETE FROM [T_AdminTestRecordAcceptedBy]" + filter);

            await _attachedDocumentRepository.QueryAsync("DELETE FROM [T_AttachedDocument]" + filter);

            await _controlLogSignatureRepository.QueryAsync("DELETE FROM [T_ControlLogSignature]" + filter);

            await _eTestPackagesRepository.QueryAsync("DELETE FROM [T_ETestPackages]" + filter);

            await _punchListRepository.QueryAsync("DELETE FROM [T_PunchList] " + filter);

            await _punchImageRepository.QueryAsync("DELETE FROM [T_PunchImage] " + filter);

            await _testLimitDrawingRepository.QueryAsync("DELETE FROM [T_TestLimitDrawing]" + filter);

            await _drainRecordContentRepository.QueryAsync("DELETE FROM [T_DrainRecordContent] " + filter);

            await _drainRecordAcceptedByRepository.QueryAsync("DELETE FROM [T_DrainRecordAcceptedBy]  " + filter);

            await _testRecordDetailsRepository.QueryAsync("DELETE FROM [T_TestRecordDetails]  " + filter);

            await _testRecordConfirmationRepository.QueryAsync("DELETE FROM [T_TestRecordConfirmation] " + filter);

            await _testRecordAcceptedByRepository.QueryAsync("DELETE FROM [T_TestRecordAcceptedBy] " + filter);

            await _testRecordImageRepository.QueryAsync("DELETE FROM [T_TestRecordImage] " + filter);




            await _testRecordDetailsRepository.QueryAsync("DELETE FROM [T_PreTestRecordAcceptedBy]  " + filter);

            await _testRecordConfirmationRepository.QueryAsync("DELETE FROM [T_PreTestRecordContent] " + filter);

            await _testRecordAcceptedByRepository.QueryAsync("DELETE FROM [T_PostTestRecordAcceptedBy] " + filter);

            await _testRecordImageRepository.QueryAsync("DELETE FROM [T_PostTestRecordContent] " + filter);

            await _testRecordDetailsRepository.QueryAsync("DELETE FROM [T_AdminPreTestRecordContent]  " + filter);

            await _testRecordConfirmationRepository.QueryAsync("DELETE FROM [T_AdminPreTestRecordAcceptedBy] " + filter);

            await _testRecordAcceptedByRepository.QueryAsync("DELETE FROM [T_AdminPostTestRecordAcceptedBy] " + filter);

            await _testRecordImageRepository.QueryAsync("DELETE FROM [T_AdminPostTestRecordContent] " + filter);

            //if (!projectOnly)
            //{
            //    await _userProjectRepository.QueryAsync("DELETE FROM [T_UserProject] " + filter);
            //    await _UserDetails.QueryAsync("DELETE FROM [T_UserDetails] " + filter);
            //}

        }

        public async void JobSettingResetAndClear(bool projectOnly)
        {
                if (projectOnly)
                {
                    var Ids = await _iwpRepository.QueryAsync<T_IWP>("SELECT * FROM [T_IWP] WHERE [ModelName] = '" + Settings.ModelName + "'");
                    List<int> IWPIDs = Ids.Select(i => i.ID).ToList();
                    string IWPInStatment = string.Empty;
                    if (IWPIDs.Count > 0)
                    {
                        IWPInStatment = "( " + String.Join(", ", IWPIDs) + " )";

                        await _iwpRepository.QueryAsync<T_IWP>("DELETE FROM [T_IWP] WHERE [ModelName] = '" + Settings.ModelName + "'");
                        await _successorRepository.QueryAsync<T_Successor>("DELETE FROM [T_Successor] WHERE [IWP_ID] IN " + IWPInStatment);
                        await _predecessorRepository.QueryAsync<T_Predecessor>("DELETE FROM [T_Predecessor] WHERE [IWP_ID] IN " + IWPInStatment);
                        await _iwpStatusRepository.QueryAsync<T_IWPStatus>("DELETE FROM [T_IWPStatus] WHERE [IWP_ID] IN " + IWPInStatment);
                        await _iwpDrawingsRepository.QueryAsync<T_IWPDrawings>("DELETE FROM [T_IWPDrawings] WHERE [IWPID] IN " + IWPInStatment);
                        await _tagMilestoneImagesRepository.QueryAsync<T_TagMilestoneImages>("DELETE FROM [T_TagMilestoneImages] WHERE [Project_ID] =" + Settings.ProjectID);
                        await _tagMilestoneStatusRepository.QueryAsync<T_TagMilestoneStatus>("DELETE FROM [T_TagMilestoneStatus] WHERE [Project_ID] = " + Settings.ProjectID);

                        await _iwpControlLogSignaturesRepository.QueryAsync<T_IWPControlLogSignatures>("DELETE FROM [T_IWPControlLogSignatures] WHERE [IWP_ID] IN " + IWPInStatment);
                        await _cwpDrawingsRepository.QueryAsync<T_CWPDrawings>("DELETE FROM [T_CWPDrawings] WHERE [IWPID] IN " + IWPInStatment);
                        await _iwpPunchCategoryRepository.QueryAsync<T_IWPPunchCategory>("DELETE FROM [T_IWPPunchCategory] WHERE [ProjectID] =" + Settings.ProjectID);
                        await _iwpPunchLayerRepository.QueryAsync<T_IWPPunchLayer>("DELETE FROM [T_IWPPunchLayer] WHERE [ProjectID] = " + Settings.ProjectID);
                        await _iwpAdminControlLogRepository.QueryAsync<T_IWPAdminControlLog>("DELETE FROM [T_IWPAdminControlLog] WHERE [ProjectID] = " + Settings.ProjectID);

                        await _manPowerLogRepository.QueryAsync<T_ManPowerLog>("DELETE FROM [T_ManPowerLog] WHERE [IWPID] IN " + IWPInStatment);  
                        await _workerScannedRepository.QueryAsync<T_WorkerScanned>("DELETE FROM [T_WorkerScanned] WHERE [ProjectID] = " + Settings.ProjectID);

                        await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>("DELETE FROM [T_IWPPunchControlItem] WHERE [ProjectID] = " + Settings.ProjectID);
                        await _iwpPunchImagesRepository.QueryAsync<T_IWPPunchImage>("DELETE FROM [T_IWPPunchImage] WHERE [ProjectID] = " + Settings.ProjectID);
                        await _iWPPunchCategoriesRepository.QueryAsync<T_IWPPunchCategories>("DELETE FROM [T_IWPPunchCategories] WHERE [ProjectID] = " + Settings.ProjectID);
                        await _iWPFunctionCodesRepository.QueryAsync<T_IWPFunctionCodes>("DELETE FROM [T_IWPFunctionCodes] WHERE [ProjectID] = " + Settings.ProjectID);
                        await _iWPAdminPunchLayerRepository.QueryAsync<T_IWPAdminPunchLayer>("DELETE FROM [T_IWPAdminPunchLayer] WHERE [ProjectID] = " + Settings.ProjectID);
                        await _iWPCompanyCategoryCodesRepository.QueryAsync<T_IWPCompanyCategoryCodes>("DELETE FROM [T_IWPCompanyCategoryCodes] WHERE [ProjectID] = " + Settings.ProjectID);

                        string LocationPDFPath = "PDF Store/" + userProject.JobCode;
                        await DependencyService.Get<ISaveFiles>().RemovePDFFilefromFolder(LocationPDFPath);
                        //string LocationPhotoPath = "Photo Store/" + userProject.JobCode;
                        //bool result = await DependencyService.Get<ISaveFiles>().RemoveAllFilefromFolder(LocationPhotoPath);
                        //await _userDialogs.AlertAsync("Stored IWP photos removed", null, "Ok");
                        await _iwpAttachmentsRepository.QueryAsync<T_IWPAttachments>("DELETE FROM [T_IWPAttachments] WHERE [LinkedID] IN " + IWPInStatment);

                        _userDialogs.AlertAsync("Data cleared successfully", "Cleared Data", "Ok");

                }
                else
                    await _userDialogs.AlertAsync("No IWP(s) to remove", null, "Ok");

                }
                else
                {
                    await _iwpRepository.DeleteAll();                    
                    await _successorRepository.DeleteAll();
                    await _predecessorRepository.DeleteAll();
                    await _iwpStatusRepository.DeleteAll();
                    await _iwpAttachmentsRepository.DeleteAll();
                    await _iwpDrawingsRepository.DeleteAll(); 
                    await _manPowerResourceRepository.DeleteAll();
                    await _workerScannedRepository.DeleteAll();
                    await _manPowerLogRepository.DeleteAll();
                    await _tagMilestoneStatusRepository.DeleteAll();
                    await _tagMilestoneImagesRepository.DeleteAll();
                    await _iwpAdminControlLogRepository.DeleteAll();
                    await _iwpControlLogSignaturesRepository.DeleteAll();
                    await _iwpPunchCategoryRepository.DeleteAll();
                    await _iwpPunchLayerRepository.DeleteAll();
                    await _cwpDrawingsRepository.DeleteAll();
                    await _iwpPunchControlItemRepository.DeleteAll();
                    await _iwpPunchImagesRepository.DeleteAll();
                    await _iWPPunchCategoriesRepository.DeleteAll();
                    await _iWPFunctionCodesRepository.DeleteAll();
                    await _iWPAdminPunchLayerRepository.DeleteAll();
                    await _iWPCompanyCategoryCodesRepository.DeleteAll();

                    string LocationPDFPath = "PDF Store/" + userProject.JobCode;
                    await DependencyService.Get<ISaveFiles>().RemovePDFFilefromFolder(LocationPDFPath);

                    //string LocationPhotoPath = "Photo Store/" + userProject.JobCode;
                    //bool result = await DependencyService.Get<ISaveFiles>().RemoveAllFilefromFolder(LocationPhotoPath);   

                    //await _userDialogs.AlertAsync("Stored IWP photos removed", null, "Ok");

                }           
        }
        public Boolean StringToIntCheck(string value)
        {
            Boolean result = false;

            try
            {
                Convert.ToUInt32(value);
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }
        public override void Destroy()
        {
            base.Destroy();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        protected override void OnNavigation(string param, NavigationParameters parameters = null)
        {
            base.OnNavigation(param, parameters);
        }

        protected override void OnNavigationBack()
        {
            base.OnNavigationBack();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
        }

        protected override void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            base.OnPropertyChanged(propertyExpression);
        }

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            return base.SetProperty(ref storage, value, propertyName);
        }

        protected override bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
        {
            return base.SetProperty(ref storage, value, onChanged, propertyName);
        }
    }
}

using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.Common.Extentions;
using JGC.Models;
using Plugin.Connectivity;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.UserControls.PopupControls;
using Newtonsoft.Json;
using JGC.Models.E_Test_Package;
using JGC.DataBase.DataTables.WorkPack;
using JGC.Models.Work_Pack;

namespace JGC.ViewModels
{


    public class UploadViewModel : BaseViewModel
    {

        protected readonly INavigationService _navigationService;
        private readonly IRepository<T_EReports> _eReport;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_EReports_Signatures> _signaturesRepository;
        private readonly T_UserProject _selectedUserProject;
        private readonly IDownloadService _downloadService;
        private readonly IRepository<T_PartialRequest> partialRequestRepository;
        //upload For EtestPackage
        private readonly IRepository<T_ETestPackages> _ETestPackagestRepository;
        private readonly IRepository<T_PunchList> _PunchListRepository;
        private readonly IRepository<T_PunchImage> _PunchImageRepository;
        private readonly IRepository<T_ControlLogSignature> _ControlLogSignatureRepository;
        private readonly IRepository<T_TestRecordDetails> _TestRecordDetailsRepository;
        private readonly IRepository<T_DrainRecordContent> _DrainRecordContentRepository;
        private readonly IRepository<T_TestRecordImage> _TestRecordImageRepository;
        private readonly IRepository<T_TestLimitDrawing> _TestLimitDrawingRepository;
        private readonly IRepository<T_AttachedDocument> _AttachedDocumentRepository;
        private readonly IRepository<T_TestRecordConfirmation> _TestRecordConfirmationRepository;
        private readonly IRepository<T_TestRecordAcceptedBy> _TestRecordAcceptedByRepository;
        private readonly IRepository<T_DrainRecordAcceptedBy> _DrainRecordAcceptedByRepository;

        private readonly IRepository<T_IWP> _iwpRepository;        
        private readonly IRepository<T_IWPAdminControlLog> _iwpAdminControlLogRepository;
        private readonly IRepository<T_IWPControlLogSignatures> _iwpControlLogSignaturesRepository;
        private readonly IRepository<T_IWPDrawings> _iwpDrawingsRepository;
        private readonly IRepository<T_IWPPunchControlItem> _iwpPunchControlItemRepository;
        private readonly IRepository<T_IWPPunchImage> _iwpPunchImagesRepository;
        private readonly IRepository<T_ManPowerLog> _manPowerLogRepository;
        private readonly IRepository<T_TagMilestoneImages> _tagMilestoneImagesRepository;
        private readonly IRepository<T_TagMilestoneStatus> _tagMilestoneStatusRepository;
        private readonly IRepository<T_IWPAdminPunchLayer> _iWPAdminPunchLayerRepository;
        private readonly IRepository<T_IWPPunchCategory> _iwpPunchCategoryRepository;
        private readonly IRepository<T_IWPAttachments> _iwpAttachmentsRepository;
        private readonly IRepository<T_Successor> _successorRepository;
        private readonly IRepository<T_Predecessor> _predecessorRepository;
        private readonly IRepository<T_IWPStatus> _iwpStatusRepository;
        private readonly IRepository<T_CWPDrawings> _cwpDrawingsRepository;
        private readonly IRepository<T_IWPPunchLayer> _iwpPunchLayerRepository;

        private readonly IRepository<T_PreTestRecordContent> _PreTestRecordContent;
        private readonly IRepository<T_PreTestRecordAcceptedBy> _PreTestRecordAcceptedBy;
        private readonly IRepository<T_PostTestRecordContent> _PostTestRecordContent;
        private readonly IRepository<T_PostTestRecordAcceptedBy> _PostTestRecordAcceptedBy;
        

        private readonly IRepository<T_DWR> _DWRRepository;
        private readonly IRepository<T_DWR_HeatNos> _DWR_HeatNosRepository;



        #region fields 
        private bool isRefreshing;
        // private T_UserProject CurrentProject;
        PageHelper _pageHelper = CheckValidLogin._pageHelper;
        #endregion

        #region Properties

        private ObservableCollection<T_EReports> eReports;
        public ObservableCollection<T_EReports> EReports
        {
            get { return eReports; }
            set { eReports = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<string> uploadingProgressList;

        public ObservableCollection<string> UploadingProgressList
        {
            get
            {
                return uploadingProgressList ?? (uploadingProgressList = new ObservableCollection<string>());
            }

        }
        private string uploadingLable { get; set; }
        public string UploadingLable
        {
            get { return uploadingLable; }
            set { uploadingLable = value; RaisePropertyChanged(); }
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

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); }
        }

        private string closeBtnColor;
        public string CloseBtnColor
        {
            get { return closeBtnColor; }
            set { closeBtnColor = value; RaisePropertyChanged(); }
        }

        public ICommand RefreshCommand { get; set; }

        #endregion

        #region Delegate Commands   

        public ICommand CloseCommand
        {
            get
            {
                return new Command(OnClickCloseButton);
            }
        }

        private async void OnClickCloseButton()
        {
            if (!String.IsNullOrEmpty(Settings.DownloadParam))
                await navigationService.GoBackAsync();
            else
                await navigationService.NavigateAsync<ProjectViewModel>();

        }
        #endregion

        public UploadViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            T_UserProject SelectedUserProject,
            IRepository<T_UserProject> userProjectRepository,
            IRepository<T_EReports_Signatures> signaturesRepository,
            IRepository<T_EReports> eReports,
            IRepository<T_PartialRequest> _partialRequestRepository,
            IRepository<T_ETestPackages> _ETestPackagestRepository,
            IRepository<T_PunchList> _PunchListRepository,
            IRepository<T_PunchImage> _PunchImageRepository,
            IRepository<T_ControlLogSignature> _ControlLogSignatureRepository,
            IRepository<T_TestRecordDetails> _TestRecordDetailsRepository,
            IRepository<T_DrainRecordContent> _DrainRecordContentRepository,
            IRepository<T_TestRecordImage> _TestRecordImageRepository,
            IRepository<T_TestLimitDrawing> _TestLimitDrawingRepository,
            IRepository<T_AttachedDocument> _AttachedDocumentRepository,
            IRepository<T_TestRecordConfirmation> _TestRecordConfirmationRepository,
            IRepository<T_TestRecordAcceptedBy> _TestRecordAcceptedByRepository,
            IRepository<T_DrainRecordAcceptedBy> _DrainRecordAcceptedByRepository,

             IRepository<T_PreTestRecordContent> _PreTestRecordContent,
             IRepository<T_PreTestRecordAcceptedBy> _PreTestRecordAcceptedBy,
             IRepository<T_PostTestRecordContent> _PostTestRecordContent,
             IRepository<T_PostTestRecordAcceptedBy> _PostTestRecordAcceptedBy,

        IRepository<T_IWP> _iwpRepository,
            IRepository<T_ManPowerLog> _manPowerLogRepository,
            IRepository<T_TagMilestoneStatus> _tagMilestoneStatusRepository,
            IRepository<T_TagMilestoneImages> _tagMilestoneImagesRepository,
            IRepository<T_IWPControlLogSignatures> _iwpControlLogSignaturesRepository,
            IRepository<T_IWPPunchImage> _iwpPunchImagesRepository,
            IRepository<T_IWPPunchControlItem> _iwpPunchControlItemRepository,
            IRepository<T_IWPAdminControlLog> _iwpAdminControlLogRepository,
            IRepository<T_IWPAdminPunchLayer> _iWPAdminPunchLayerRepository,
            IRepository<T_IWPDrawings> _iwpDrawingsRepository,
            IRepository<T_Successor> _successorRepository,
            IRepository<T_Predecessor> _predecessorRepository,
            IRepository<T_IWPStatus> _iwpStatusRepository,
            IRepository<T_CWPDrawings> _cwpDrawingsRepository,

            IRepository<T_DWR> _DWRRepository,
            IRepository<T_DWR_HeatNos> _DWR_HeatNosRepository
          ) : base(_navigationService, _httpHelper, _checkValidLogin)
        {

            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._userProjectRepository = userProjectRepository;
            this._ETestPackagestRepository = _ETestPackagestRepository;
            this._selectedUserProject = SelectedUserProject;
            this._eReport = eReports;
            this.partialRequestRepository = _partialRequestRepository;

            this._PreTestRecordContent = _PreTestRecordContent;
            this._PreTestRecordAcceptedBy = _PreTestRecordAcceptedBy;
            this._PostTestRecordContent = _PostTestRecordContent;
            this._PostTestRecordAcceptedBy = _PostTestRecordAcceptedBy;

            //EtestProject
            this._signaturesRepository = signaturesRepository;
            this._PunchListRepository = _PunchListRepository;
            this._PunchImageRepository = _PunchImageRepository;
            this._ControlLogSignatureRepository = _ControlLogSignatureRepository;
            this._TestRecordDetailsRepository = _TestRecordDetailsRepository;
            this._DrainRecordContentRepository = _DrainRecordContentRepository;
            this._TestRecordImageRepository = _TestRecordImageRepository;
            this._TestLimitDrawingRepository = _TestLimitDrawingRepository;
            this._AttachedDocumentRepository = _AttachedDocumentRepository;
            this._TestRecordConfirmationRepository = _TestRecordConfirmationRepository;
            this._TestRecordAcceptedByRepository = _TestRecordAcceptedByRepository;
            this._DrainRecordAcceptedByRepository = _DrainRecordAcceptedByRepository;


            this._iwpRepository = _iwpRepository;
            this._manPowerLogRepository = _manPowerLogRepository;
            this._tagMilestoneStatusRepository = _tagMilestoneStatusRepository;
            this._tagMilestoneImagesRepository = _tagMilestoneImagesRepository;
            this._iwpControlLogSignaturesRepository = _iwpControlLogSignaturesRepository;
            this._iwpPunchImagesRepository = _iwpPunchImagesRepository;
            this._iwpPunchControlItemRepository = _iwpPunchControlItemRepository;
            this._iwpAdminControlLogRepository = _iwpAdminControlLogRepository;
            this._iwpDrawingsRepository = _iwpDrawingsRepository;
            this._successorRepository = _successorRepository;
            this._predecessorRepository = _predecessorRepository;
            this._iwpStatusRepository = _iwpStatusRepository;
            this._cwpDrawingsRepository = _cwpDrawingsRepository;

            this._DWRRepository = _DWRRepository;
            this._DWR_HeatNosRepository = _DWR_HeatNosRepository;


            PageHeaderText = "Upload";

            if (Settings.ProjectID > 0)
            {
                // UploadData();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Upload E-Report(s)", "Please select Project", "Yes");
                navigationService.GoBackAsync();
            }

            try
            {
                if (Settings.ModuleName == "EReporter")
                {
                    if (!String.IsNullOrEmpty(Settings.DownloadParam))
                        UploadDWRData();
                    else
                        UploadData();
                    UploadingLable = "Uploading E-Report";
                    CloseBtnColor = "#FB1610";
                    // EReporterGrid = true;
                    // ETestPackageGrid = false;

                }
                else if (Settings.ModuleName == "TestPackage")
                {
                    // ETestPackageGrid = true;
                    // EReporterGrid = false;
                    CloseBtnColor = "#C4BB46";
                    UploadDataTestPackage();
                    UploadingLable = "Uploading E-TestPackage";
                }
                else if (Settings.ModuleName == "JobSetting")
                {
                    CloseBtnColor = "#3B87C7";
                    UploadWorkPack();
                    UploadingLable = "Uploading Work Pack";
                }
            }
            catch(Exception ex)
            {

            }

        }

        private async void UploadData()
        {

            //var UserProjectList = await _userProjectRepository.GetAsync();
            //if (UserProjectList.Count > 0)
            //    CurrentProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();

            // get Selected UserProject
            //  var UserProjectList =   await _userProjectRepository.QueryAsync<T_UserProject >("SELECT * FROM [T_UserProject] WHERE [User_ID] = '" + Settings.UserID + "' AND [Project_ID] ='" + Settings.ProjectID + "'");
            //if (UserProjectList.Count > 0)
            //    CurrentProject = UserProjectList.FirstOrDefault(); // UserProjectList.FirstOrDefault();  

            //get upload Ereport
            var EReports = await _eReport.QueryAsync<T_EReports>("SELECT * FROM [T_EReports] WHERE upper(ModelName) = '" + Settings.ModelName.ToUpper() + "' AND Updated = 1");  // await _eReport.GetAsync(x => x.ModelName == Settings.ModelName && x.Updated == true);   
            var EReportIDS = EReports.ToList();
            int DownloadCount = EReportIDS.Count;


            if (EReportIDS.Count > 0)
            {
                ProgressBarIsVisible = true;
                float per = (float)1 / (float)DownloadCount;

                var answer1 = await Application.Current.MainPage.DisplayAlert("Upload E-Report(s)", "Are you sure you want to upload all altered E-Report(s) and images?", "Yes", "No");
                UploadingProgressList.Clear();
                UploadingProgressList.Add("Uploading E-Report(s)...");
                ProgressValue = per / 2;
                await Task.Delay(100);
                var answer = answer1;
                if (answer)
                {

                    //check wifi
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        UploadingProgressList.Add("Online");
                        //   ProgressValue = 1;
                        ////get Selected UserProject
                        //var UserProjectList = await _userProjectRepository.GetAsync();
                        //if (UserProjectList.Count > 0)
                        //    CurrentProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();


                        //Upload Images (needed first)
                        string PhotoStore = "Photo Store\\" + Settings.JobCode;
                        var DirectoryExists = await DependencyService.Get<ISaveFiles>().DirectoryExists(PhotoStore);
                       // await Application.Current.MainPage.DisplayAlert("Directory", "Picture Directory Status "+  DirectoryExists.ToString(), "Ok");
                        if (DirectoryExists)
                        {

                            string[] directories = await DependencyService.Get<ISaveFiles>().GetDirectories(PhotoStore);
                          //  await Application.Current.MainPage.DisplayAlert("directories found", directories.Count().ToString(), "Ok");
                            Boolean Found = false;
                            foreach (string EReport_Folder in directories)
                            {
                              //  await Application.Current.MainPage.DisplayAlert("directories", EReport_Folder, "Ok");
                                var Images = await DependencyService.Get<ISaveFiles>().GetImageFiles(EReport_Folder);


                                if (Images != null && Images.Count() != 0)
                                {
                                 //   await Application.Current.MainPage.DisplayAlert("Images", Images.Count().ToString(), "Ok");
                                    // per = (float)1 / (float)DownloadCount + Images.Count();
                                    //  ProgressValue += per / 2;
                                    string[] pathSplit = Device.RuntimePlatform == Device.UWP? Regex.Split(EReport_Folder, "\\\\") : Regex.Split(EReport_Folder, "/");// .Split(cah);

                                    var EReportID = pathSplit.Last();

                                    // var Reports = await _eReport.QueryAsync<T_EReports>("SELECT * FROM [T_EReports] WHERE [ID] = '" + EReportID + "'");

                                    // await _eReport.QueryAsync<T_EReports>("SELECT * FROM [T_EReports] WHERE [ModelName] = '" + Settings.ModelName + "' AND Updated = 1");
                                    var Report = EReportIDS.Where(X => X.ID.ToString() == EReportID).FirstOrDefault();//  Reports.FirstOrDefault();
                                    //Found Images
                                    if (Report != null)
                                    {
                                        UploadingProgressList.Add("Uploading " + Images.Count() + " images for " + Report.ReportNo);

                                        foreach (string ImageFile in Images)
                                        {
                                            //per += (float)1 / (float) Images.Count();
                                            //ProgressValue += per / 2;

                                            var CurrentFileInfo = new FileInfo(ImageFile);

                                            string Field = CurrentFileInfo.Directory.Name;
                                            byte[] FileBytes;
                                            if (Device.RuntimePlatform == Device.UWP)
                                                FileBytes = await DependencyService.Get<ISaveFiles>().ReadBytes(ImageFile);
                                            else
                                                 FileBytes = File.ReadAllBytes(ImageFile);

                                            VMHub CurrentVMHub = new VMHub();
                                            CurrentVMHub.Module = "EREPORTER";
                                            CurrentVMHub.LinkedID = EReportID;
                                            CurrentVMHub.Comment = "";
                                            CurrentVMHub.DisplayName = Field + " - " + Path.GetFileNameWithoutExtension(ImageFile);
                                            CurrentVMHub.FileName = CurrentFileInfo.Name;
                                            CurrentVMHub.Extension = CurrentFileInfo.Extension;
                                            CurrentVMHub.FileBytes = Convert.ToBase64String(FileBytes);
                                            CurrentVMHub.FileSize = FileBytes.Length.ToString();


                                            UploadingProgressList.Add("Uploading " + CurrentVMHub.DisplayName);

                                            Boolean Result = ModsTools.WebServiceVMHub(CurrentVMHub, Settings.AccessToken);

                                            if (Result)
                                            {
                                                UploadingProgressList.Add(CurrentVMHub.DisplayName + " uploaded");
                                                if (Device.RuntimePlatform == Device.UWP)
                                                {
                                                    await DependencyService.Get<ISaveFiles>().DeleteImage(ImageFile);
                                                }
                                                else
                                                {
                                                    if (File.Exists(ImageFile))
                                                        File.Delete(ImageFile);
                                                }
                                            }
                                            else
                                                UploadingProgressList.Add(CurrentVMHub.DisplayName + " failed to upload");

                                            Found = true;
                                        }
                                    }
                                }
                            }

                            if (Found)
                                UploadingProgressList.Add("Images uploaded");
                            else
                                UploadingProgressList.Add("No images to upload");
                        }
                        else
                            UploadingProgressList.Add("No images to upload");


                        bool uploadFail = false;

                        ////upload Ereport
                        //var EReports = await _eReport.GetAsync();// await _eReport.QueryAsync<List<T_EReports>>("SELECT [ID] FROM [T_EReports] WHERE [ModelName] = '" + CurrentProject.ModelName + "' AND [Updated] = TRUE").Result;

                        //var EReportIDS = EReports.Where(x => x.ModelName == CurrentProject.ModelName && x.Updated == true).ToList();
                        //   string[] EReportIDS = EReports.ToArray();
                        if (EReportIDS.Count() > 0)
                        {
                            UploadingProgressList.Add("Uploading " + EReportIDS.Count() + " E-Report(s)");


                            foreach (T_EReports ThisEReport in EReportIDS)
                            {
                                ProgressValue = 1;
                                // EReport ThisEReport = new EReport();
                                // ThisEReport.GetLocalSlim(Convert.ToInt32(CurrentID));
                                var signaturesData = await _signaturesRepository.QueryAsync<T_EReports_Signatures>(@"SELECT * FROM T_EReports_Signatures WHERE [EReportID] = '" + ThisEReport.ID + "' ORDER BY [SignatureNo] ASC");
                                ThisEReport.Signatures = signaturesData.ToList();

                                UploadingProgressList.Add("Uploading " + ThisEReport.ReportNo);

                                string Result = ModsTools.WebServicePost("EReporter/PostEReport?UnitID=" + Settings.UnitID, ModsTools.ToJson(ThisEReport), Settings.AccessToken);
                                var _Result = Result.ToUpper().Replace("\"", "");

                                if (_Result == "SUCCESS")
                                {
                                    ProgressValue = 1;
                                    //  partial receiving
                                    await UploadPartialReceivingAsync(ThisEReport);

                                    // ThisEReport.Delete(CurrentProject.JobCode);
                                    var dl1 = await _signaturesRepository.QueryAsync<T_EReports_Signatures>("DELETE FROM [T_EReports_Signatures] WHERE [EReportID] = '" + ThisEReport.ID + "'");
                                    var dl2 = await _eReport.QueryAsync<T_EReports>("DELETE FROM [T_EReports] WHERE [ID] = '" + ThisEReport.ID + "'");
                                    //var dl2 = await _eReport.DeleteAsync(ThisEReport);
                                    UploadingProgressList.Add("Updated " + ThisEReport.ReportNo + ", report removed from handheld's database");
                                }
                                else
                                {
                                    ProgressValue += 1;
                                    UploadingProgressList.Add("Error occurred updating " + ThisEReport.ReportNo);
                                    UploadingProgressList.Add(Result);
                                    uploadFail = true;
                                }
                            }

                            UploadingProgressList.Add("Updating E-Report Selection page");

                            //   string SearchValue = SearchTB.Text;

                            // LoadSearchGV(CurrentPageHelper.ReportType, SearchValue);

                            UploadingProgressList.Add("E-Report Selection page updated");
                            UploadingProgressList.Add("E-Report upload complete");
                            var navigationParameters = new NavigationParameters();
                            navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);

                        }
                        else
                            UploadingProgressList.Add("No E-Report(s) to be uploaded");
                        UploadingProgressList.Add("Upload complete");
                        ProgressBarIsVisible = false;
                        Close_IsVisible = true;
                    }
                    else
                    {
                        UploadingProgressList.Add("Offline");
                        await Application.Current.MainPage.DisplayAlert("Connection", "Check connection and try again", "Ok");
                    }

                }
                else
                {
                    await navigationService.GoBackAsync();
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Upload", "No E-Report(s) to be uploaded", "Ok");
                await navigationService.GoBackAsync();
            }
            ProgressBarIsVisible = false;
        }

        private async void UploadDWRData() 
        {
            //get upload DWR
            var DWRList = await _DWRRepository.GetAsync(x=>x.ProjectID == Settings.ProjectID && x.Updated == true);
            int DownloadCount = DWRList.Count;
            if (DWRList.Count > 0)
            {
                    ProgressBarIsVisible = true;
                    float per = (float)1 / (float)DownloadCount;

                    var answer1 = await Application.Current.MainPage.DisplayAlert("Upload E-Report(s)", "Are you sure you want to upload all altered E-Report(s) and images?", "Yes", "No");
                    UploadingProgressList.Clear();
                    UploadingProgressList.Add("Uploading DWR(s)...");
                    ProgressValue = per / 2;
                    await Task.Delay(100);
                    var answer = answer1;
                    if (answer)
                    {
                        //check wifi
                        if (CrossConnectivity.Current.IsConnected)
                        {
                            UploadingProgressList.Add("Online");
                            bool uploadFail = false;

                        ////upload Ereport
                        if (DWRList.Count() > 0)
                           {
                               UploadingProgressList.Add("Uploading " + DWRList.Count() + " E-Report(s)");

                               foreach (T_DWR ThisDWR in DWRList)
                               {
                                var EReports = await _eReport.GetAsync(x=>x.RowID == ThisDWR.RowID);
                                T_EReports ThisEReport = EReports.ToList().FirstOrDefault();
                                ThisEReport.JSONString = JsonConvert.SerializeObject(ThisDWR);
                                ProgressValue = 1; 
                                
                                var signaturesData = await _signaturesRepository.QueryAsync<T_EReports_Signatures>(@"SELECT * FROM T_EReports_Signatures WHERE [EReportID] = '" + ThisEReport.ID + "' AND [RowID] = '" + ThisEReport.RowID + "' ORDER BY [SignatureNo] ASC");
                                ThisEReport.Signatures = signaturesData.ToList();

                                UploadingProgressList.Add("Uploading " + ThisEReport.ReportNo);

                                string Result = ModsTools.WebServicePost("EReporter/PostEReport?UnitID=" + Settings.UnitID, ModsTools.ToJson(ThisEReport), Settings.AccessToken);
                                var _Result = Result.ToUpper().Replace("\"", "");

                                   if (_Result == "SUCCESS")
                                   {
                                    //Get New DWRID
                                    string DWRJsonString = ModsTools.WebServiceGet(ApiUrls.Url_GetDWRID(Settings.ProjectID, ThisDWR.TestPackage, ThisDWR.SpoolDrawingNo, ThisDWR.JointNo), Settings.AccessToken);
                                    int GeneratedID = Convert.ToInt32(DWRJsonString);

                                    //Upload Images
                                    string PhotoStore = "Photo Store\\" + Settings.JobCode + "\\" + Settings.UserID + "\\DWR";
                                    var DirectoryExists = await DependencyService.Get<ISaveFiles>().DirectoryExists(PhotoStore);
                                    if (DirectoryExists)
                                    {
                                        string[] directories = await DependencyService.Get<ISaveFiles>().GetDirectories(PhotoStore);
                                        Boolean Found = false;
                                        foreach (string EReport_Folder in directories)
                                        {
                                            var Images = await DependencyService.Get<ISaveFiles>().GetImageFiles(EReport_Folder);
                                            if (Images != null && Images.Count() != 0)
                                            {
                                                string[] pathSplit = Device.RuntimePlatform == Device.UWP ? Regex.Split(EReport_Folder, "\\\\") : Regex.Split(EReport_Folder, "/");// .Split(cah);

                                                var EReportID = pathSplit.Last();
                                                var Report = DWRList.Where(X => X.RowID.ToString() == EReportID).FirstOrDefault();

                                                if (Report != null)
                                                {
                                                    UploadingProgressList.Add("Uploading " + Images.Count() + " images for " + Report.ReportNo);

                                                    foreach (string ImageFile in Images)
                                                    {
                                                        var CurrentFileInfo = new FileInfo(ImageFile);

                                                        string Field = CurrentFileInfo.Directory.Name;
                                                        byte[] FileBytes;
                                                        if (Device.RuntimePlatform == Device.UWP)
                                                            FileBytes = await DependencyService.Get<ISaveFiles>().ReadBytes(ImageFile);
                                                        else
                                                            FileBytes = File.ReadAllBytes(ImageFile);

                                                        VMHub CurrentVMHub = new VMHub();
                                                        CurrentVMHub.Module = "EREPORTER";
                                                        CurrentVMHub.LinkedID = GeneratedID.ToString();
                                                        CurrentVMHub.Comment = "";
                                                        CurrentVMHub.DisplayName = Field + " - " + Path.GetFileNameWithoutExtension(ImageFile);
                                                        CurrentVMHub.FileName = CurrentFileInfo.Name;
                                                        CurrentVMHub.Extension = CurrentFileInfo.Extension;
                                                        CurrentVMHub.FileBytes = Convert.ToBase64String(FileBytes);
                                                        CurrentVMHub.FileSize = FileBytes.Length.ToString();


                                                        UploadingProgressList.Add("Uploading " + CurrentVMHub.DisplayName);

                                                        Boolean VMHubResult = ModsTools.WebServiceVMHub(CurrentVMHub, Settings.AccessToken);

                                                        if (VMHubResult)
                                                        {
                                                            UploadingProgressList.Add(CurrentVMHub.DisplayName + " uploaded");
                                                            if (Device.RuntimePlatform == Device.UWP)
                                                            {
                                                                await DependencyService.Get<ISaveFiles>().DeleteImage(ImageFile);
                                                            }
                                                            else
                                                            {
                                                                if (File.Exists(ImageFile))
                                                                    File.Delete(ImageFile);
                                                            }
                                                        }
                                                        else
                                                            UploadingProgressList.Add(CurrentVMHub.DisplayName + " failed to upload");

                                                        Found = true;
                                                    }
                                                }
                                            }
                                        }
                                        if (Found)
                                            UploadingProgressList.Add("Images uploaded");
                                        else
                                            UploadingProgressList.Add("No images to upload");
                                    }
                                    else
                                        UploadingProgressList.Add("No images to upload");
                                    uploadFail = false;

                                    ProgressValue = 1;
                                       var dl1 = await _signaturesRepository.QueryAsync<T_EReports_Signatures>("DELETE FROM [T_EReports_Signatures] WHERE [EReportID] = '" + ThisEReport.ID + "'AND [RowID] = '" + ThisEReport.RowID + "'");
                                       var dl2 = await _eReport.QueryAsync<T_EReports>("DELETE FROM [T_EReports] WHERE[ID] = '" + ThisEReport.ID + "'AND [RowID] = '" + ThisEReport.RowID + "'");
                                       var dl3 = await _DWRRepository.QueryAsync<T_DWR>("DELETE FROM [T_DWR] WHERE[ID] = '" + ThisDWR.ID + "'AND [RowID] = '" + ThisDWR.RowID + "'AND [ProjectID] = '"+ ThisDWR.ProjectID +"'");
                                   
                                    ModsTools.WebServicePostBoolean(ApiUrls.Url_UnlockEReport(ThisEReport.ID), Settings.AccessToken);
                                    UploadingProgressList.Add("Updated " + ThisEReport.ReportNo + ", report removed from handheld's database");
                                   }
                                   else
                                   {
                                       ProgressValue += 1;
                                       UploadingProgressList.Add("Error occurred updating " + ThisEReport.ReportNo);
                                       UploadingProgressList.Add(Result);
                                       uploadFail = true;
                                   }
                               }
                            // var HeatNos = await _DWR_HeatNosRepository.GetAsync(x => x.Updated == true);
                            //if (HeatNos.Count > 0)
                            // {
                            //     UploadingProgressList.Add("Uploading Heat Nos");
                            //     List<T_DWR_HeatNos> AllHeatNos = HeatNos.ToList();
                            //     var ResultHeatNos = ModsTools.WebServicePost(ApiUrls.Url_postAllHeatNos(Settings.ProjectID, Settings.JobCode), ModsTools.ToJson(AllHeatNos), Settings.AccessToken);

                            //     if(ResultHeatNos.Count > 0)
                            //     {
                            //     }
                            // }
                            

                            UploadingProgressList.Add("Updating E-Report Selection page");
                               UploadingProgressList.Add("E-Report Selection page updated");
                               UploadingProgressList.Add("E-Report upload complete");
                               var navigationParameters = new NavigationParameters();
                               navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);

                           }
                           else
                               UploadingProgressList.Add("No E-Report(s) to be uploaded");
                          
                        UploadingProgressList.Add("Upload complete");
                            ProgressBarIsVisible = false;
                            Close_IsVisible = true;
                        }
                        else
                        {
                            UploadingProgressList.Add("Offline");
                            await Application.Current.MainPage.DisplayAlert("Connection", "Check connection and try again", "Ok");
                        }

                    }
                    else
                    {
                        await navigationService.GoBackAsync();
                    }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Upload", "No E-Report(s) to be uploaded", "Ok");
                await navigationService.GoBackAsync();
            }
            ProgressBarIsVisible = false;

        }

        // progressBarIsVisible = false;

        private async void UploadDataTestPackage()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Upload E-Test Package(s)", "Are you sure you want to upload all altered E-Test Package(s) and images?", "OK", "No");
            if (result)
            {
                // PBUploadData.Image = Properties.Resources.up_white;
                UploadingProgressList.Clear();
                UploadingProgressList.Add("Uploading E-Test Package(s)");
                bool uploadEtpFail = false;
                bool uploadFail = false;

                if (CrossConnectivity.Current.IsConnected)
                {
                    UploadingProgressList.Add("Online");

                    List<T_ETestPackages> etestpackageList = await GetETestPackageHeaderUploadList(Settings.ProjectID);


                    if (etestpackageList != null && etestpackageList.Count() > 0)
                    {
                        ProgressBarIsVisible = true;
                        float per = (float)1 / (float)etestpackageList.Count();
                        ProgressValue = per / 2;
                        UploadingProgressList.Add("Uploading " + etestpackageList.Count() + " Test Package(s)");

                        foreach (T_ETestPackages etestpackage in etestpackageList)
                        {
                            UploadingProgressList.Add("");
                            UploadingProgressList.Add("Uploading " + etestpackage.TestPackage);

                            //Upload New  
                            List<T_PunchList> punchItems = await GetPunchesList(Settings.ProjectID, etestpackage.ID, false);

                            if (punchItems != null && punchItems.Count > 0)
                            {
                                UploadingProgressList.Add("Uploading " + punchItems.Count + " New Punches");

                                foreach (T_PunchList punchItem in punchItems)
                                {
                                    UploadingProgressList.Add(punchItem.PunchID);

                                    UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostNewPunch?ID=" + etestpackage.ID, ModsTools.ToJson(punchItem), Settings.AccessToken);

                                    if (uploadResult.Success)
                                    {
                                        UploadingProgressList.Add(uploadResult.Message);
                                        int punchVMID = uploadResult.NewID;

                                        Boolean punchIDHasChanged = (uploadResult.NewName.ToUpper() != punchItem.PunchID.ToUpper());
                                        string newPunchID = punchIDHasChanged ? uploadResult.NewName : punchItem.PunchID.ToUpper();

                                        List<TestPackageImage> punchImages = await GetPunchImages(Settings.ProjectID, etestpackage.ID, punchItem.PunchID);

                                        //Updated

                                        string SQL = "UPDATE [T_PunchList] SET [Updated] = 0 WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ETestPackageID] = '" + punchItem.ETestPackageID + "' AND [PunchID] = '" + punchItem.PunchID + "'";

                                        var resultData = await _PunchListRepository.QueryAsync<T_PunchList>(SQL);

                                        if (punchImages != null && punchImages.Count > 0)
                                        {

                                            foreach (PunchImage image in punchImages)
                                            {
                                                if (!image.Live)
                                                {
                                                    //Not live so upload
                                                    if (punchIDHasChanged)
                                                    {
                                                        image.PunchID = newPunchID;
                                                        image.DisplayName = image.DisplayName.Replace("_" + punchItem.PunchID + "_", "_" + newPunchID + "_");
                                                        image.FileName = image.FileName.Replace("_" + punchItem.PunchID + "_", "_" + newPunchID + "_");
                                                    }

                                                    uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostPunchImage?ID=" + punchVMID, ModsTools.ToJson(image), Settings.AccessToken);

                                                    UploadingProgressList.Add(uploadResult.Message);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        UploadingProgressList.Add(uploadResult.Message);
                                        uploadEtpFail = true;
                                    }

                                }
                            }
                            else
                                UploadingProgressList.Add("No New Punches To Upload");


                            //Upload edited punches
                            punchItems = await GetPunchesList(Settings.ProjectID, etestpackage.ID, true);

                            if (punchItems != null && punchItems.Count > 0)
                            {
                                UploadingProgressList.Add("Uploading " + punchItems.Count + " Edited Punches");

                                foreach (T_PunchList punchItem in punchItems)
                                {
                                    UploadingProgressList.Add(punchItem.PunchID);

                                    ModsTools.ToJson(punchItem);

                                    UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostPunchEdit?ProjectID= " + Settings.ProjectID + "&ID=" + etestpackage.ID, ModsTools.ToJson(punchItem), Settings.AccessToken);

                                    if (uploadResult.Success || uploadResult.Message.Contains("already up to date"))
                                    {
                                        UploadingProgressList.Add(uploadResult.Message);
                                        int punchVMID = uploadResult.NewID;

                                        Boolean punchIDHasChanged = (uploadResult.NewName.ToUpper() != punchItem.PunchID.ToUpper());
                                        string newPunchID = punchIDHasChanged ? uploadResult.NewName : punchItem.PunchID.ToUpper();

                                        List<TestPackageImage> punchImages = await GetPunchImages(Settings.ProjectID, etestpackage.ID, punchItem.PunchID);

                                        //Updated

                                        string SQL = "UPDATE [T_PunchList] SET [Updated] = 0 WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ETestPackageID] = '" + punchItem.ETestPackageID + "' AND [PunchID] = '" + punchItem.PunchID + "'";
                                        var resultData = await _PunchListRepository.QueryAsync<T_PunchList>(SQL);

                                        if (punchImages != null && punchImages.Count > 0)
                                        {

                                            foreach (PunchImage image in punchImages)
                                            {
                                                if (!image.Live)
                                                {
                                                    uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostPunchImage?ID=" + punchVMID, ModsTools.ToJson(image), Settings.AccessToken);
                                                    UploadingProgressList.Add(uploadResult.Message);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        UploadingProgressList.Add(uploadResult.Message);
                                        uploadEtpFail = true;
                                    }

                                }
                            }
                            else
                                UploadingProgressList.Add("No Edited Punches To Upload");


                            //Control Log
                            List<ControlLogSignature> controlLogSignatures = await GetControlLogSignatureUploadList(Settings.ProjectID, etestpackage.ID);

                            if (controlLogSignatures != null && controlLogSignatures.Count > 0)
                            {
                                UploadingProgressList.Add("Uploading " + controlLogSignatures.Count + " Control Log Signatures");

                                foreach (ControlLogSignature signature in controlLogSignatures)
                                {
                                    UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostNewControlLogSignature?ProjectID=" + Settings.ProjectID + "&ID=" + etestpackage.ID, ModsTools.ToJson(signature), Settings.AccessToken);
                                    UploadingProgressList.Add(uploadResult.Message);

                                    if (!uploadResult.Success)
                                        uploadEtpFail = true;
                                }

                            }
                            else
                                UploadingProgressList.Add("No New Control Log Signatures To Upload");


                            //Test Record Details Upload
                            List<CertificationDetails> testRecordDetails = await GetTestRecordDetailsUploadList(Settings.ProjectID, etestpackage.ID);

                            if (testRecordDetails != null && testRecordDetails.Count > 0)
                            {
                                UploadingProgressList.Add("Uploading " + testRecordDetails.Count + " Test Record Details");

                                UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostTestRecordDetails?ID=" + etestpackage.ID, ModsTools.ToJson(testRecordDetails), Settings.AccessToken);
                                UploadingProgressList.Add(uploadResult.Message);
                            }
                            else
                                UploadingProgressList.Add("No New Test Record Details To Upload");

                            //Test Record Confirmation Upload
                            List<CertificationSignature> testRecordConfirmationSignatures = await GetCertificationSignatureUploadList(Settings.ProjectID, etestpackage.ID, "T_TestRecordConfirmation");

                            if (testRecordConfirmationSignatures != null && testRecordConfirmationSignatures.Count > 0)
                            {
                                UploadingProgressList.Add("Uploading " + testRecordConfirmationSignatures.Count + " Test Record Confirmation Signatures");

                                UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostTestRecordConfirmation?ID=" + etestpackage.ID, ModsTools.ToJson(testRecordConfirmationSignatures), Settings.AccessToken);
                                UploadingProgressList.Add(uploadResult.Message);
                            }
                            else
                                UploadingProgressList.Add("No New Test Record Confirmation Signatures To Upload");


                            //Test Record Accepted By Upload
                            List<CertificationSignature> testRecordAcceptedBySignatures = await GetCertificationSignatureUploadList(Settings.ProjectID, etestpackage.ID, "T_TestRecordAcceptedBy");

                            if (testRecordAcceptedBySignatures != null && testRecordAcceptedBySignatures.Count > 0)
                            {
                                UploadingProgressList.Add("Uploading " + testRecordAcceptedBySignatures.Count + " Test Record Accepted By Signatures");

                                UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostTestRecordAcceptedBy?ProjectID=" + Settings.ProjectID + "&ID=" + etestpackage.ID, ModsTools.ToJson(testRecordAcceptedBySignatures), Settings.AccessToken);

                                UploadingProgressList.Add(uploadResult.Message);
                            }
                            else
                                UploadingProgressList.Add("No New Test Record Accepted By Signatures To Upload");


                            //Test Record Images Upload
                            List<TestPackageImage> testrecordImages = await GetTestRecordImages(Settings.ProjectID, etestpackage.ID);


                            if (testrecordImages != null && testrecordImages.Count > 0)
                            {
                                UploadingProgressList.Add("Uploading " + testrecordImages.Count + " Test Record Images");
                                foreach (TestRecordImage image in testrecordImages)
                                {
                                    UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostTestRecordImage?ID=" + etestpackage.ID, ModsTools.ToJson(image), Settings.AccessToken);
                                    UploadingProgressList.Add(uploadResult.Message);
                                }
                            }
                            else
                                UploadingProgressList.Add("No New Test Record Images To Upload");



                            //Drain Record Content Upload
                            List<CertificationSignature> drainRecordContentSignatures = await GetCertificationSignatureUploadList(Settings.ProjectID, etestpackage.ID, "T_DrainRecordContent");

                            if (drainRecordContentSignatures != null && drainRecordContentSignatures.Count > 0)
                            {
                                UploadingProgressList.Add("Uploading " + drainRecordContentSignatures.Count + " Drain Record Content Signatures");

                                UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostDrainRecordContent?ID=" + etestpackage.ID, ModsTools.ToJson(drainRecordContentSignatures), Settings.AccessToken);
                                UploadingProgressList.Add(uploadResult.Message);
                            }
                            else
                                UploadingProgressList.Add("No New Drain Record Content Signatures To Upload");


                            //Drain Record Accepted By Upload
                            List<CertificationSignature> drainRecordAcceptedBySignatures = await GetCertificationSignatureUploadList(Settings.ProjectID, etestpackage.ID, "T_DrainRecordAcceptedBy");

                            if (drainRecordAcceptedBySignatures != null && drainRecordAcceptedBySignatures.Count > 0)
                            {
                                UploadingProgressList.Add("Uploading " + drainRecordAcceptedBySignatures.Count + " Drain Record Accepted By Signatures");

                                UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostDrainRecordAcceptedBy?ProjectID=" + Settings.ProjectID + "&ID=" + etestpackage.ID, ModsTools.ToJson(drainRecordAcceptedBySignatures), Settings.AccessToken);

                                UploadingProgressList.Add(uploadResult.Message);
                            }
                            else
                                UploadingProgressList.Add("No New Drain Record Accepted By Signatures To Upload");


                            //pretest Record Content Upload
                            List<CertificationSignature> PrestestRecordContentSignatures = await GetCertificationSignatureUploadList(Settings.ProjectID, etestpackage.ID, "T_PreTestRecordContent");

                            if (PrestestRecordContentSignatures != null && PrestestRecordContentSignatures.Count > 0)
                            {
                                UploadingProgressList.Add("Uploading " + PrestestRecordContentSignatures.Count + " Pre Test Record Content Signatures");

                                UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostPreTestRecordConfirmation?ID=" + etestpackage.ID, ModsTools.ToJson(PrestestRecordContentSignatures), Settings.AccessToken);
                                UploadingProgressList.Add(uploadResult.Message);
                            }
                            else
                                UploadingProgressList.Add("No New Pre Test Record Content Signatures To Upload");


                            //pretest Record Accepted By Upload
                            List<CertificationSignature> PrestestRecordAcceptedBySignatures = await GetCertificationSignatureUploadList(Settings.ProjectID, etestpackage.ID, "T_PreTestRecordAcceptedBy");

                            if (PrestestRecordAcceptedBySignatures != null && PrestestRecordAcceptedBySignatures.Count > 0)
                            {
                                UploadingProgressList.Add("Uploading " + PrestestRecordAcceptedBySignatures.Count + " Pre Test Record Accepted By Signatures");

                                UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostPreTestRecordAcceptedBy?ProjectID=" + Settings.ProjectID + "&ID=" + etestpackage.ID, ModsTools.ToJson(PrestestRecordAcceptedBySignatures), Settings.AccessToken);

                                UploadingProgressList.Add(uploadResult.Message);
                            }
                            else
                                UploadingProgressList.Add("No New Pre Test Record Accepted By Signatures To Upload");

                            //Posttest Record Content Upload
                            List<CertificationSignature> PoststestRecordContentSignatures = await GetCertificationSignatureUploadList(Settings.ProjectID, etestpackage.ID, "T_PostTestRecordContent");

                            if (PoststestRecordContentSignatures != null && PoststestRecordContentSignatures.Count > 0)
                            {
                                UploadingProgressList.Add("Uploading " + PoststestRecordContentSignatures.Count + " Post Test Record Content Signatures");

                                UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostTestRecordPostConfirmation?ID=" + etestpackage.ID, ModsTools.ToJson(PoststestRecordContentSignatures), Settings.AccessToken);
                                UploadingProgressList.Add(uploadResult.Message);
                            }
                            else
                                UploadingProgressList.Add("No New Post Test Record Content Signatures To Upload");


                            //Posttest Record Accepted By Upload
                            List<CertificationSignature> PoststestRecordAcceptedBySignatures = await GetCertificationSignatureUploadList(Settings.ProjectID, etestpackage.ID, "T_PreTestRecordAcceptedBy");

                            if (PoststestRecordAcceptedBySignatures != null && PoststestRecordAcceptedBySignatures.Count > 0)
                            {
                                UploadingProgressList.Add("Uploading " + PoststestRecordAcceptedBySignatures.Count + " Pre Test Record Accepted By Signatures");

                                UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostTestRecordPostAcceptedBy?ProjectID=" + Settings.ProjectID + "&ID=" + etestpackage.ID, ModsTools.ToJson(PoststestRecordAcceptedBySignatures), Settings.AccessToken);

                                UploadingProgressList.Add(uploadResult.Message);
                            }
                            else
                                UploadingProgressList.Add("No New Pre Test Record Accepted By Signatures To Upload");

                            string afiNo = "", drainRecordRemarks = "", drainingDate = "", testRecordRemarks = "";
                            Boolean afiNoUpdated = false, drainRecordRemarksUpdated = false, drainingDateUpdated = false, testRecordRemarksUpdated = false;

                            //String SQL = "SELECT * FROM [ETestPackageHH] WHERE [ID] = '"+ etestpackage.ID + "'";
                            var data = await _ETestPackagestRepository.GetAsync(x => x.ID == etestpackage.ID);
                            if (data != null && data.Any())
                            {
                                var testPackageData = data.FirstOrDefault();

                                afiNo = testPackageData.AFINo;
                                afiNoUpdated = testPackageData.AFINoUpdated;

                                drainRecordRemarks = testPackageData.DrainRecordRemarks;
                                drainRecordRemarksUpdated = testPackageData.DrainRecordRemarksUpdated;

                                drainingDate = testPackageData.DrainingDate.ToString("yyyy-MM-dd");
                                drainingDateUpdated = testPackageData.DrainingDateUpdated;

                                testRecordRemarks = testPackageData.TestRecordRemarks;
                                testRecordRemarksUpdated = testPackageData.TestRecordRemarksUpdated;

                            }

                            if (drainRecordRemarksUpdated)
                            {
                                UploadingProgressList.Add("Uploading Drain Record Remarks");
                                UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostDrainRecordRemarks?ID=" + etestpackage.ID, ModsTools.ToJson(drainRecordRemarks), Settings.AccessToken);
                                UploadingProgressList.Add(uploadResult.Message);
                            }

                            if (drainingDateUpdated)
                            {
                                UploadingProgressList.Add("Uploading Draining Date");
                                UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostDrainingDate?ID=" + etestpackage.ID, ModsTools.ToJson(drainingDate), Settings.AccessToken);
                                UploadingProgressList.Add(uploadResult.Message);
                            }

                            if (afiNoUpdated)
                            {
                                UploadingProgressList.Add("Uploading AFI No");
                                UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostAFINo?ID=" + etestpackage.ID, ModsTools.ToJson(afiNo), Settings.AccessToken);
                                UploadingProgressList.Add(uploadResult.Message);
                            }

                            if (testRecordRemarksUpdated)
                            {
                                UploadingProgressList.Add("Uploading Test Record Remarks");
                                UploadResult uploadResult = ModsTools.WebServicePostWithResult("ETestPackage/PostTestRecordRemarks?ID=" + etestpackage.ID, ModsTools.ToJson(testRecordRemarks), Settings.AccessToken);
                                UploadingProgressList.Add(uploadResult.Message);
                            }

                            if (!uploadEtpFail)
                            {
                                DeleteTestPackage(Settings.ProjectID, etestpackage.ID);
                                UploadingProgressList.Add(etestpackage.TestPackage + " Removed From Device");
                            }
                            else
                            {
                                UploadingProgressList.Add(etestpackage.TestPackage + " has failed to upload all parts to VMLIVE");
                                uploadFail = true;
                            }
                        }
                        ProgressValue = 1;
                        UploadingProgressList.Add("Upload complete");
                        UploadingProgressList.Add("Updating Selection page");

                        // LoadSearchTab();

                        UploadingProgressList.Add("Selection page updated");
                    }
                    else
                        UploadingProgressList.Add("No Test Package(s) to be uploaded");

                    ProgressValue = 1;
                    UploadingProgressList.Add("Upload complete");
                    ProgressBarIsVisible = false;
                    Close_IsVisible = true;
                }
                else
                {
                    UploadingProgressList.Add("Offline");
                    UploadingProgressList.Add("Check connection and try again");
                    Close_IsVisible = true;
                }


                //   PBUploadData.Image = Properties.Resources.up_grey;

                // CloseButtonProgressPanel();

                if (uploadFail)
                {
                    _userDialogs.Alert(@"Data has failed to update MODS VMLive Server. The failed reports will stay on the device so data will not be lost. Please try to upload again. If unsuccessful please contact a member of the MODS team.", @"Error uploading to VMLIVE", "OK");
                }
            }
        }

        // Uploading Work pack data on Vmlive 
        private async void UploadWorkPack()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Upload Work Pack(s)", "Are you sure you want to upload all altered Work Pack(s) and images?", "OK", "No");
            if (result)
            {
                UploadingProgressList.Clear();

            //    UploadingProgressList.Add("Uploading Work Pack(s) under development");

             
                 UploadingProgressList.Add("Uploading Work Pack(s)");
                 bool uploadWorkPackFail = false;
                 bool uploadFail = false;
                ProgressBarIsVisible = true;
                Close_IsVisible = false;
                if (CrossConnectivity.Current.IsConnected)
                {

                    //Listout WorkPack data is available for Upload
                    List<T_IWP> WorkPackList = await GetWorkPackUploadList();
                    if (WorkPackList != null && WorkPackList.Count() > 0)
                    {
                     
                        float per = (float)1 / (float)WorkPackList.Count();
                        ProgressValue = per / 2;
                        UploadingProgressList.Add("Uploading " + WorkPackList.Count() + " Work Pack(s)");

                        //Uploading IWP one by one 
                        foreach (T_IWP IWP in WorkPackList)
                        {
                            UploadingProgressList.Add("");
                            UploadingProgressList.Add("Uploading " + IWP.Title);

                            //Get updated ManPowerLog for upload
                            string SQL = "SELECT * FROM [T_ManPowerLog] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [IWPID]= '" + IWP.ID + "' AND [Updated] = 1";
                            var Result = await _manPowerLogRepository.QueryAsync<T_ManPowerLog>(SQL);
                            List<T_ManPowerLog> ManPowerLogs = Result.ToList();

                            if (ManPowerLogs.Count > 0)
                            {
                                UploadingProgressList.Add("Uploading " + ManPowerLogs.Count + " Man Power Log");

                                string uploadResult = ModsTools.WebServicePost(ApiUrls.Url_postManPowerLogs(Settings.ProjectID, Settings.UserID), ModsTools.ToJson(ManPowerLogs), Settings.AccessToken);
                                if (JsonConvert.DeserializeObject(uploadResult).ToString().ToLower() == "true")
                                    UploadingProgressList.Add("Uploaded Man Power Logs");
                                else
                                    UploadingProgressList.Add("Failed to Upload Man Power Log");
                            }


                            //Get updated MilestoneStatus for upload
                            string SQLMilestoneStatus = "SELECT * FROM [T_TagMilestoneStatus] WHERE [Project_ID] = '" + Settings.ProjectID + "' AND [Updated] = 1";
                            var MilestoneStatusResult = await _tagMilestoneStatusRepository.QueryAsync<T_TagMilestoneStatus>(SQLMilestoneStatus);
                            List<T_TagMilestoneStatus> MilestoneStatuses = MilestoneStatusResult.ToList();

                            if (MilestoneStatuses.Count > 0)
                            {
                                UploadingProgressList.Add("Uploading " + MilestoneStatuses.Count + " CWP Tag Statuses");
                                foreach (T_TagMilestoneStatus MilestoneStatus in MilestoneStatuses)
                                {
                                    UploadingProgressList.Add(MilestoneStatus.TagNo + " " + MilestoneStatus.Milestone + " " + MilestoneStatus.MilestoneAttribute);
                                }
                                string uploadResult = ModsTools.WebServicePost(ApiUrls.Url_postTagMilestoneStatus(Settings.ProjectID), ModsTools.ToJson(MilestoneStatuses), Settings.AccessToken);
                                if(JsonConvert.DeserializeObject(uploadResult).ToString() !="True")
                                UploadingProgressList.Add(JsonConvert.DeserializeObject(uploadResult).ToString());
                            }

                            //Get updated ControlLog Milestone Images for upload
                            string SQLMilestoneImages = "SELECT * FROM [T_TagMilestoneImages] WHERE [Project_ID] = '" + Settings.ProjectID + "' AND [Updated] = 1";
                            var MilestoneImagesResult = await _tagMilestoneImagesRepository.QueryAsync<T_TagMilestoneImages>(SQLMilestoneImages);
                            List<T_TagMilestoneImages> MilestoneImages = MilestoneImagesResult.ToList();

                            if (MilestoneImages.Count > 0)
                            {
                                 UploadingProgressList.Add("Uploading " + MilestoneImages.Count + " CWP Tag Image(s)");
                                foreach (T_TagMilestoneImages MilestoneImage in MilestoneImages)
                                {
                                    UploadingProgressList.Add("Uploading " + MilestoneImage.DisplayName +" CWP Tag Image");
                                    string uploadResult = ModsTools.WebServicePost(ApiUrls.Url_postCWPMilestoneImage, ModsTools.ToJson(MilestoneImage), Settings.AccessToken);
                                    UploadTagsResult CWPMilestoneImageResult = JsonConvert.DeserializeObject<UploadTagsResult>(uploadResult);
                                    UploadingProgressList.Add(CWPMilestoneImageResult.Message);
                                    if (CWPMilestoneImageResult.Success == true)
                                    {
                                        //string deleteSQL = "DELETE FROM T_TagMilestoneImages Where DisplayName = '" + MilestoneImage.DisplayName + "' AND CWPID = '" + MilestoneImage.CWPID
                                        //                 + "' AND Project_ID = '" + MilestoneImage.Project_ID + "' AND Milestone = '" + MilestoneImage.Milestone
                                        //                 + "' AND MilestoneAttribute = '" + MilestoneImage.MilestoneAttribute + "'";
                                       // await _tagMilestoneImagesRepository.QueryAsync<T_TagMilestoneImages>(deleteSQL);
                                    }
                                    else
                                        UploadingProgressList.Add("Failed to uploading CWP Tag Image");
                                }
                            }


                            // Get Control logs for Upload
                            string SQLControlLogSignatures = "SELECT * FROM [T_IWPControlLogSignatures] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [Updated] = 1";
                            var ControlLogSignatures = await _iwpControlLogSignaturesRepository.QueryAsync<T_IWPControlLogSignatures>(SQLControlLogSignatures);
                            List<T_IWPControlLogSignatures> CLSignatures = ControlLogSignatures.ToList();

                            if (CLSignatures.Count > 0)
                            {
                                UploadingProgressList.Add("Uploading " + CLSignatures.Count + " Control Log Signatures");
                                foreach (T_IWPControlLogSignatures Signatures in CLSignatures)
                                {
                                    string uploadResult = ModsTools.WebServicePost(ApiUrls.Url_postControlLogSignature(Settings.ProjectID, IWP.ID), ModsTools.ToJson(Signatures), Settings.AccessToken);
                                    UploadTagsResult TagResult = JsonConvert.DeserializeObject<UploadTagsResult>(uploadResult);
                                    UploadingProgressList.Add(TagResult.Message);
                                    if (TagResult.Success == true)
                                    {
                                       
                                    }
                                    else
                                        UploadingProgressList.Add("Failed Uploading Control Log Signatures");
                                }

                                UploadingProgressList.Add("Uploaded Control Log Signatures");
                            }


                            //Get Updated Punch Control
                            string SQLControlItems = "SELECT * FROM [T_IWPPunchControlItem] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [IWPID]= '" + IWP.ID + "' AND [Updated] = 1"; // AND [PunchID] = '" + punchID + "'";
                            var PunchControlItems = await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(SQLControlItems);

                            PunchControlItems.Where(i=>i.IsCreated==true).ToList().ForEach(x => x.ID =0);
                            if (PunchControlItems != null && PunchControlItems.Any())
                            {
                                UploadingProgressList.Add("Uploading " + PunchControlItems.Count + " New Punch Control");
                                foreach (T_IWPPunchControlItem PunchItem in PunchControlItems)
                                {
                                    UploadingProgressList.Add("Uploading " + PunchItem.PunchID);
                                    string uploadResult = ModsTools.WebServicePost(ApiUrls.Url_postPunchItem, ModsTools.ToJson(PunchItem), Settings.AccessToken);
                                    UploadTagsResult PunchItemResult = JsonConvert.DeserializeObject<UploadTagsResult>(uploadResult);
                                    if (PunchItemResult.Success == true)
                                    {
                                        UploadingProgressList.Add( PunchItem.PunchID +" "+ PunchItemResult.Message);
                                    }
                                    else
                                        UploadingProgressList.Add("Failed Uploading Punch item");
                                }
                            }


                            //Get Updated Punch Control Images
                            string SQLPunchImage = "SELECT * FROM [T_IWPPunchImage] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [IWPID]= '" + IWP.ID + "' AND [IsUploaded] ='0'"; // AND [IsUploaded] ='0'

                            var PunchImages = await _iwpPunchImagesRepository.QueryAsync<T_IWPPunchImage>(SQLPunchImage);

                            if (PunchImages != null && PunchImages.Any())
                            {
                                //List<T_IWPPunchImage> PunchImageList = new List<T_IWPPunchImage>();
                                UploadingProgressList.Add("Uploading " + PunchImages.Count + " New Punch Image Successfully");
                                foreach (T_IWPPunchImage _punchImage in PunchImages)
                                {
                                    //T_IWPPunchImage punchImage = new T_IWPPunchImage
                                    //{
                                    //    //  PunchID = punchID,
                                    //    Module = _punchImage.Module,
                                    //    LinkedID = _punchImage.LinkedID,
                                    //    IWPID = _punchImage.IWPID,
                                    //    ProjectID = _punchImage.ProjectID,
                                    //    DisplayName = _punchImage.DisplayName,
                                    //    FileName = _punchImage.FileName,
                                    //    Comment = _punchImage.Comment,
                                    //    Extension = _punchImage.Extension,
                                    //    FileSize = _punchImage.FileSize,
                                    //    FileBytes = _punchImage.FileBytes,
                                    //    // Live = _punchImage.Live
                                    //};
                                    //PunchImageList.Add(punchImage);

                                    //string uploadResult = ModsTools.WebServicePost(ApiUrls.Url_postPunchImage, ModsTools.ToJson(punchImage), Settings.AccessToken);
                                    string uploadResult = ModsTools.WebServicePost(ApiUrls.Url_postPunchImage, ModsTools.ToJson(_punchImage), Settings.AccessToken);
                                    if (uploadResult.ToLower() == "true")
                                    {
                                      //  UploadingProgressList.Add("Uploaded " + _punchImage.DisplayName + " Punch Images");
                                    }
                                    else
                                        UploadingProgressList.Add("Failed Uploading Punch Images");
                                }
                            }

                            // After uploading live remove data from local DB
                            DeleteWorkPack(IWP.ID);
                        }

                       
                        ProgressValue = 1;
                    }
                    else
                        UploadingProgressList.Add("No Work Pack(s) to be uploaded");

                    ProgressValue = 1;
                    UploadingProgressList.Add("Upload Completed");
                    ProgressBarIsVisible = false;
                    Close_IsVisible = true;
                }
                else
                {
                    UploadingProgressList.Add("Offline");
                    UploadingProgressList.Add("Check connection and try again");
                    ProgressBarIsVisible = false;
                    Close_IsVisible = true;
                }

                if (uploadFail)
                {
                    _userDialogs.Alert(@"Data has failed to update MODS VMLive Server. The failed reports will stay on the device so data will not be lost. Please try to upload again. If unsuccessful please contact a member of the MODS team.", @"Error uploading to VMLIVE", "OK");
                } 

            }
            else
            {
                ProgressValue = 1;
                UploadingProgressList.Add("Upload Cancled");
                ProgressBarIsVisible = false;
                Close_IsVisible = true;
            }
        }


        private async Task<List<T_ETestPackages>> GetETestPackageHeaderUploadList(int projectID)
        {
            string SQL = "SELECT * FROM [T_ETestPackages] WHERE [ProjectID] = '" + projectID + "' AND [Updated] = 1 ORDER BY [TestPackage] ASC";

            List<T_ETestPackages> list = new List<T_ETestPackages>();
            var data = await _ETestPackagestRepository.QueryAsync<T_ETestPackages>(SQL);

            list = data.ToList();
            return list;

        }
        private async Task<List<T_IWP>> GetWorkPackUploadList()
        {
            string SQL = "SELECT * FROM [T_IWP] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [Updated] = 1 ";

            List<T_IWP> list = new List<T_IWP>();
            var data = await _iwpRepository.QueryAsync<T_IWP>(SQL);
            list = data.ToList();
            return list;

        }
        public async Task<List<T_PunchList>> GetPunchesList(int projectID, int etestpackageID, Boolean live)
        {
            List<T_PunchList> list = new List<T_PunchList>();

            string SQL = "SELECT * FROM [T_PunchList] WHERE [ProjectID] = '" + projectID + "' AND [ETestPackageID]= '" + etestpackageID + "' AND " + (live == false ? "([Live] = 0 OR [Live] IS NULL)" : "[Live] = 1") + " AND [Updated] = 1 ORDER BY [PunchNo] ASC";

            var Result = await _PunchListRepository.QueryAsync<T_PunchList>(SQL);
            list = Result.ToList();
            return list;

        }
        private async Task<List<TestPackageImage>> GetPunchImages(int projectID, int etestpackageID, string punchID)
        {

            List<TestPackageImage> list = new List<TestPackageImage>();

            string SQL = "SELECT * FROM [T_PunchImage] WHERE [ProjectID] = '" + projectID + "' AND [ETestPackageID]= '" + etestpackageID + "' AND [PunchID] = '" + punchID + "'";

            var result = await _PunchImageRepository.QueryAsync<T_PunchImage>(SQL);

            if (result != null && result.Any())
            {
                foreach (T_PunchImage _punchImage in result)
                {
                    PunchImage punchImage = new PunchImage
                    {
                        PunchID = punchID,
                        DisplayName = _punchImage.DisplayName,
                        FileName = _punchImage.FileName,
                        Extension = _punchImage.Extension,
                        FileSize = _punchImage.FileSize,
                        FileBytes = _punchImage.FileBytes,
                        Live = _punchImage.Live
                    };
                    list.Add(punchImage);
                }

            }

            return list;
        }

        public async Task<List<ControlLogSignature>> GetControlLogSignatureUploadList(int projectID, int etestPackageID)
        {
            string SQL = "SELECT CL.* FROM [T_ControlLogSignature] AS CL INNER JOIN [T_AdminControlLog] AS ADMIN ON ADMIN.[ProjectID] = CL.[ProjectID] AND ADMIN.[ID] = CL.[ControlLogAdminID] WHERE CL.[ProjectID] = '" + projectID + "' AND CL.[ETestPackageID] = '" + etestPackageID + "' AND CL.[Signed] = 1 AND CL.[Updated] = 1 ORDER BY ADMIN.[SignatureNo] ASC";
            List<ControlLogSignature> list = new List<ControlLogSignature>();
            var data = await _ControlLogSignatureRepository.QueryAsync<T_ControlLogSignature>(SQL);
            if (data != null && data.Any())
            {
                foreach (T_ControlLogSignature cl in data)
                {
                    ControlLogSignature signature = new ControlLogSignature
                    {
                        ControlLogAdminID = cl.ControlLogAdminID,
                        Signed = cl.Signed,
                        SignedByUserID = cl.SignedByUserID,
                        SignedOn = cl.SignedOn,
                    };
                    list.Add(signature);
                }
            }
            return list;
        }

        public async Task<List<CertificationDetails>> GetTestRecordDetailsUploadList(int projectID, int etestPackageID)
        {
            string SQL = "SELECT * FROM [T_TestRecordDetails] WHERE [ProjectID] = '" + projectID + "' AND [ETestPackageID] = '" + etestPackageID + "' AND [Updated] = 1";
            List<CertificationDetails> list = new List<CertificationDetails>();
            var data = await _TestRecordDetailsRepository.QueryAsync<T_TestRecordDetails>(SQL);

            if (data != null && data.Any())
            {
                foreach (T_TestRecordDetails Tr in data)
                {
                    CertificationDetails signature = new CertificationDetails
                    {
                        DetailsAdminID = Tr.DetailsAdminID,
                        InputValue = Tr.InputValue,
                    };

                    list.Add(signature);
                }
            }


            return list;
        }

        public async Task<List<CertificationSignature>> GetCertificationSignatureUploadList(int projectID, int etestPackageID, string databaseTable)
        {
            string SQL = "SELECT * FROM [" + databaseTable + "] WHERE [ProjectID] = '" + projectID + "' AND [ETestPackageID] = '" + etestPackageID + "' AND [Updated] = 1";

            List<CertificationSignature> list = new List<CertificationSignature>();

            //if (databaseTable == "T_TestRecordConfirmation")
            //{
            //    var data = await _DrainRecordContentRepository.QueryAsync<T_DrainRecordContent>(SQL);

            //    if (data != null && data.Any())
            //    {
            //        foreach (T_DrainRecordContent Tr in data)
            //        {
            //            CertificationSignature signature = new CertificationSignature
            //            {
            //                AdminID = Tr.AdminID,
            //                Signed = Tr.Signed,
            //                SignedByUserID = Tr.SignedByUserID,
            //                SignedBy = Tr.SignedBy,
            //                SignedOn = Tr.SignedOn,
            //            };

            //            list.Add(signature);
            //        }
            //    }
            //}

            if (databaseTable == "T_TestRecordConfirmation")
            {
                var data = await _TestRecordConfirmationRepository.QueryAsync<CertificationSignature>(SQL);
                list.AddRange(data);
            }
            else if (databaseTable == "T_TestRecordAcceptedBy")
            {
                var data = await _TestRecordAcceptedByRepository.QueryAsync<CertificationSignature>(SQL);
                list.AddRange(data);
            }
            else if (databaseTable == "T_DrainRecordContent")
            {
                var data = await _DrainRecordContentRepository.QueryAsync<CertificationSignature>(SQL);
                list.AddRange(data);
            }

            else if (databaseTable == "T_DrainRecordAcceptedBy")
            {
                var data = await _DrainRecordAcceptedByRepository.QueryAsync<CertificationSignature>(SQL);
                list.AddRange(data);
            }
            else if (databaseTable == "T_PreTestRecordContent")
            {
                var data = await _PreTestRecordContent.QueryAsync<CertificationSignature>(SQL);
                list.AddRange(data);
            }
            else if (databaseTable == "T_PreTestRecordAcceptedBy")
            {
                var data = await _PreTestRecordAcceptedBy.QueryAsync<CertificationSignature>(SQL);
                list.AddRange(data);
            }


            else if (databaseTable == "T_PostTestRecordContent")
            {
                var data = await _PostTestRecordContent.QueryAsync<CertificationSignature>(SQL);
                list.AddRange(data);
            }
            else if (databaseTable == "T_PreTestRecordAcceptedBy")
            {
                var data = await _PreTestRecordAcceptedBy.QueryAsync<CertificationSignature>(SQL);
                list.AddRange(data);
            }


            return list;
        }

        public async Task<List<TestPackageImage>> GetTestRecordImages(int projectID, int etestpackageID)
        {
            List<TestPackageImage> list = new List<TestPackageImage>();
            string SQL = "SELECT * FROM [T_TestRecordImage] WHERE [ProjectID] = '" + projectID + "' AND [ETestPackageID]= '" + etestpackageID + "'";
            var data = await _TestRecordImageRepository.QueryAsync<T_TestRecordImage>(SQL);
            foreach (T_TestRecordImage TR in data)
            {
                TestRecordImage img = new TestRecordImage
                {
                    DisplayName = TR.DisplayName,
                    FileName = TR.FileName,
                    Extension = TR.Extension,
                    FileSize = TR.FileSize,
                    FileBytes = TR.FileBytes
                };
                list.Add(img);
            }
            return list;
        }


        private async void DeleteTestPackage(int projectID, int etestpackageID)
        {
            var d1 = await _ETestPackagestRepository.QueryAsync("DELETE FROM [T_ETestPackages] WHERE [ProjectID] = " + projectID + " AND [ID] = " + etestpackageID);
            var d2 = await _ControlLogSignatureRepository.QueryAsync("DELETE FROM [T_ControlLogSignature] WHERE [ProjectID] = " + projectID + " AND [ETestPackageID] = " + etestpackageID);
            var d3 = await _TestLimitDrawingRepository.QueryAsync("DELETE FROM [T_TestLimitDrawing] WHERE [ProjectID] = " + projectID + " AND [ETestPackageID] = " + etestpackageID);
            var d4 = await _AttachedDocumentRepository.QueryAsync("DELETE FROM [T_AttachedDocument] WHERE [ProjectID] = " + projectID + " AND [ETestPackageID] = " + etestpackageID);
            var d5 = await _PunchListRepository.QueryAsync("DELETE FROM [T_PunchList] WHERE [ProjectID] = " + projectID + " AND [ETestPackageID] = " + etestpackageID);
            var d6 = await _PunchImageRepository.QueryAsync("DELETE FROM [T_PunchImage] WHERE [ProjectID] = " + projectID + " AND [ETestPackageID] = " + etestpackageID);
            var d7 = await _TestRecordDetailsRepository.QueryAsync("DELETE FROM [T_TestRecordDetails] WHERE [ProjectID] = " + projectID + " AND [ETestPackageID] = " + etestpackageID);
            var d8 = await _TestRecordConfirmationRepository.QueryAsync("DELETE FROM [T_TestRecordConfirmation] WHERE [ProjectID] = " + projectID + " AND [ETestPackageID] = " + etestpackageID);
            var d9 = await _TestRecordAcceptedByRepository.QueryAsync("DELETE FROM [T_TestRecordAcceptedBy] WHERE [ProjectID] = " + projectID + " AND [ETestPackageID] = " + etestpackageID);
            var d10 = await _DrainRecordContentRepository.QueryAsync("DELETE FROM [T_DrainRecordContent] WHERE [ProjectID] = " + projectID + " AND [ETestPackageID] = " + etestpackageID);
            var d11 = await _DrainRecordAcceptedByRepository.QueryAsync("DELETE FROM [T_DrainRecordAcceptedBy] WHERE [ProjectID] = " + projectID + " AND [ETestPackageID] = " + etestpackageID);
            var d12 = await _TestRecordImageRepository.QueryAsync("DELETE FROM [T_TestRecordImage] WHERE [ProjectID] = " + projectID + " AND [ETestPackageID] = " + etestpackageID);
            await _PreTestRecordContent.QueryAsync<T_PreTestRecordContent>("DELETE FROM [T_PreTestRecordContent] WHERE [ProjectID] = " + projectID + " AND [ETestPackageID] = '" + etestpackageID + "'");
            await _PreTestRecordAcceptedBy.QueryAsync<T_PreTestRecordAcceptedBy>("DELETE FROM [T_PreTestRecordAcceptedBy] WHERE [ProjectID] = " + projectID + " AND [ETestPackageID] = '" + etestpackageID + "'");
            await _PostTestRecordContent.QueryAsync<T_PostTestRecordContent>("DELETE FROM [T_PostTestRecordContent] WHERE [ProjectID] = " + projectID + " AND [ETestPackageID] = '" + etestpackageID + "'");
            await _PostTestRecordAcceptedBy.QueryAsync<T_PostTestRecordAcceptedBy>("DELETE FROM [T_PostTestRecordAcceptedBy] WHERE [ProjectID] = " + projectID + " AND [ETestPackageID] = '" + etestpackageID + "'");

        }
        private async void DeleteWorkPack(int IWPID)
        {

            await _iwpRepository.QueryAsync<T_IWP>("DELETE FROM [T_IWP] WHERE [Updated] = 1 AND ID =" + IWPID );
            //await _successorRepository.QueryAsync<T_Successor>("DELETE FROM [T_Successor] WHERE [IWP_ID] =" + IWPID);
            //await _predecessorRepository.QueryAsync<T_Predecessor>("DELETE FROM [T_Predecessor] WHERE [IWP_ID] =" + IWPID);
            //await _iwpStatusRepository.QueryAsync<T_IWPStatus>("DELETE FROM [T_IWPStatus] WHERE [IWP_ID] =" + IWPID);
            await _tagMilestoneImagesRepository.QueryAsync<T_TagMilestoneImages>("DELETE FROM [T_TagMilestoneImages] WHERE [Updated] = 1 AND Project_ID = '"+ Settings.ProjectID +"'");
            await _tagMilestoneStatusRepository.QueryAsync<T_TagMilestoneStatus>("DELETE FROM [T_TagMilestoneStatus] WHERE [Updated] = 1 AND IWPID = " + IWPID +" AND Project_ID = '" + Settings.ProjectID + "'");

            await _iwpControlLogSignaturesRepository.QueryAsync<T_IWPControlLogSignatures>("DELETE FROM [T_IWPControlLogSignatures] WHERE Updated= 1 AND [IWP_ID] =" + IWPID + " AND ProjectID = '" + Settings.ProjectID + "'");
            //await _iwpAdminControlLogRepository.QueryAsync<T_IWPAdminControlLog>("DELETE FROM [T_IWPAdminControlLog] WHERE [ProjectID] = " + Settings.ProjectID);

            await _manPowerLogRepository.QueryAsync<T_ManPowerLog>("DELETE FROM [T_ManPowerLog] WHERE Updated = 1 AND [IWPID] =" + IWPID + " AND ProjectID = '" + Settings.ProjectID + "'");

            await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>("DELETE FROM [T_IWPPunchControlItem] WHERE Updated = 1 AND [IWPID] = " + IWPID + " AND ProjectID = '" + Settings.ProjectID + "'");
            await _iwpPunchImagesRepository.QueryAsync<T_IWPPunchImage>("DELETE FROM [T_IWPPunchImage] WHERE [IWPID] = " + IWPID + " AND ProjectID = '" + Settings.ProjectID + "'");

        }
        #region Private
        private async Task UploadPartialReceivingAsync(T_EReports ThisEReport)
        {
            //If CMR or MRR check for partial request
            if (ThisEReport.ReportType.ToUpper() == "MATERIAL RECEIVING REPORT" || ThisEReport.ReportType.ToUpper() == "CONSTRUCTION MATERIAL REQUISITION")
            {
                var Userresult = await partialRequestRepository.QueryAsync<T_PartialRequest>("SELECT * FROM [T_PartialRequest] WHERE [EReportID] = " + ThisEReport.ID);
                if (Userresult.Any())
                {
                    var userID = Userresult.FirstOrDefault().RequestedByUserID;

                    if (userID > 0)
                    {
                        bool result = ModsTools.WebServiceGetBoolean("EReporter/RequestPartial?ID=" + ThisEReport.ID + "&UserID=" + userID, Settings.AccessToken);
                        UploadingProgressList.Add(ThisEReport.ReportNo + ", report removed from handheld's database");
                    }
                }
            }
        }
        #endregion
        #region Public
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            // UploadData();

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





    }
}

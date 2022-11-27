using JGC.Common.Constants;
using JGC.Common.Enumerators;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.DataBase.DataTables.WorkPack;
using JGC.Models;
using JGC.Models.Work_Pack;
using JGC.UserControls.PopupControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JGC.Common.Services
{
    public class DownloadService : IDownloadService
    {
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
        private readonly IRepository<T_PartialRequest> _PartialRequest;

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

        //new tables 
        private readonly IRepository<T_PreTestRecordContent> _PreTestRecordContentRepository;
        private readonly IRepository<T_PreTestRecordAcceptedBy> _PreTestRecordAcceptedByRepository;
        private readonly IRepository<T_PostTestRecordContent> _PostTestRecordContentRepository;
        private readonly IRepository<T_PostTestRecordAcceptedBy> _PostTestRecordAcceptedByrRepository;
        private readonly IRepository<T_AdminPreTestRecordContent> _AdminPreTestRecordContentRepository;
        private readonly IRepository<T_AdminPreTestRecordAcceptedBy> _AdminPreTestRecordAcceptedByRepository;
        private readonly IRepository<T_AdminPostTestRecordContent> _AdminPostTestRecordContentRepository;
        private readonly IRepository<T_AdminPostTestRecordAcceptedBy> _AdminPostTestRecordAcceptedByRepository;
        private readonly IRepository<T_IwpPresetPunch> _IwpPresetPunchRepository;
        private readonly IRepository<T_CwpTag> _CwpTAg;



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
        private readonly IRepository<T_IWPPunchControlItem> _iwpPunchControlItemRepository;
        private readonly IRepository<T_IWPPunchImage> _iwpPunchImagesRepository;
        private readonly IRepository<T_IWPAdminPunchLayer> _iWPAdminPunchLayerRepository;
        private readonly IRepository<T_IWPPunchCategories> _iWPPunchCategoriesRepository;
        private readonly IRepository<T_IWPFunctionCodes> _iWPFunctionCodesRepository;
        private readonly IRepository<T_IWPCompanyCategoryCodes> _iWPCompanyCategoryCodesRepository;


        private T_UserProject userProject;
        //private PageHelper _pageHelper = CheckValidLogin._pageHelper;
        private IList<T_EReports> Ereport;
        private ObservableCollection<T_ETestPackages> ETestPackages;
        public string FileLocation { get; set; }
        public decimal currentCounter;
        public int totalRequests;
        public bool isdisplayToast;
        public DownloadService(
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
         IRepository<T_PartialRequest> _PartialRequest,

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

         IRepository<T_PreTestRecordContent> _PreTestRecordContentRepository,
         IRepository<T_PreTestRecordAcceptedBy> _PreTestRecordAcceptedByRepository,
         IRepository<T_PostTestRecordContent> _PostTestRecordContentRepository,
         IRepository<T_PostTestRecordAcceptedBy> _PostTestRecordAcceptedByrRepository,
         IRepository<T_AdminPreTestRecordContent> _AdminPreTestRecordContentRepository,
         IRepository<T_AdminPreTestRecordAcceptedBy> _AdminPreTestRecordAcceptedByRepository,
         IRepository<T_AdminPostTestRecordContent> _AdminPostTestRecordContentRepository,
         IRepository<T_AdminPostTestRecordAcceptedBy> _AdminPostTestRecordAcceptedByRepository,



        IRepository<T_IWP> _iwpRepository,
         IRepository<T_IWPStatus> _iwpStatusRepository,
         IRepository<T_Predecessor> _predecessorRepository,
         IRepository<T_Successor> _successorRepository,
         IRepository<T_IWPDrawings> _iwpDrawingsRepository,
         IRepository<T_IWPAttachments> _iwpAttachmentsRepository,
         IRepository<T_ManPowerResource> _manPowerResourceRepository,
         IRepository<T_ManPowerLog> _manPowerLogRepository,
         IRepository<T_TagMilestoneStatus> _tagMilestoneStatusRepository,
         IRepository<T_TagMilestoneImages> _tagMilestoneImagesRepository,
         IRepository<T_IWPAdminControlLog> _iwpAdminControlLogRepository,
         IRepository<T_IWPControlLogSignatures> _iwpControlLogSignaturesRepository,
         IRepository<T_IWPPunchCategory> _iwpPunchCategoryRepository,
         IRepository<T_IWPPunchLayer> _iwpPunchLayerRepository,
         IRepository<T_CWPDrawings> _cwpDrawingsRepository,
         IRepository<T_IWPPunchControlItem> _iwpPunchControlItemRepository,
         IRepository<T_IWPPunchImage> _iwpPunchImagesRepository,
         IRepository<T_IWPAdminPunchLayer> _iWPAdminPunchLayerRepository,
         IRepository<T_IWPPunchCategories> _iWPPunchCategoriesRepository,
         IRepository<T_IWPFunctionCodes> _iWPFunctionCodesRepository,
         IRepository<T_IWPCompanyCategoryCodes> _iWPCompanyCategoryCodesRepository,
         IRepository<T_IwpPresetPunch> _IwpPresetPunchRepository,
         IRepository<T_CwpTag> _CwpTAg)
        {
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
            this._BaseMetalRepository = _BaseMetalRepository;
            this._CMS_AllStorageAreasRepository = _CMS_AllStorageAreasRepository;
            this._PartialRequest = _PartialRequest;

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

            this._PreTestRecordContentRepository = _PreTestRecordContentRepository;
            this._PreTestRecordAcceptedByRepository = _PreTestRecordAcceptedByRepository;
            this._PostTestRecordContentRepository = _PostTestRecordContentRepository;
            this._PostTestRecordAcceptedByrRepository = _PostTestRecordAcceptedByrRepository;
            this._AdminPreTestRecordContentRepository = _AdminPreTestRecordContentRepository;
            this._AdminPreTestRecordAcceptedByRepository = _AdminPreTestRecordAcceptedByRepository;
            this._AdminPostTestRecordContentRepository = _AdminPostTestRecordContentRepository;
            this._AdminPostTestRecordAcceptedByRepository = _AdminPostTestRecordAcceptedByRepository;


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
            this._iwpPunchControlItemRepository = _iwpPunchControlItemRepository;
            this._iwpPunchImagesRepository = _iwpPunchImagesRepository;
            this._iWPAdminPunchLayerRepository = _iWPAdminPunchLayerRepository;
            this._iWPPunchCategoriesRepository = _iWPPunchCategoriesRepository;
            this._iWPFunctionCodesRepository = _iWPFunctionCodesRepository;
            this._iWPCompanyCategoryCodesRepository = _iWPCompanyCategoryCodesRepository;
            this._IwpPresetPunchRepository = _IwpPresetPunchRepository;
            this._CwpTAg = _CwpTAg;

        }

        public async Task<bool> DownloadAsync(int CurrentID)
        {
            try
            {
                if (Settings.ModuleName == "EReporter")
                {
                    bool GetDefects = false, getWPSNos = false, getBaseMetals = false, getStorageAreas = false, getCMRStorageAreas = false,
                        getCMRHeatNos = false, getWelders = false, getDWRHeatNumbers = false;
                    List<string> storeLocationList = new List<string>();
                    List<string> subContractors = new List<string>();
                    List<CMRHHStorageArea> storageAreas = new List<CMRHHStorageArea>();
                    List<CMRHHHeatNos> heatNos = new List<CMRHHHeatNos>();
                    T_EReports ThisEReport;

                    var UserProjectList = await _userProjectRepository.GetAsync();
                    if (UserProjectList.Count > 0)
                        userProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();


                    string JsonResonceString = ModsTools.WebServiceGet(ApiUrls.Url_getEReport(CurrentID, Settings.UnitID), Settings.AccessToken);
                    ThisEReport = JsonConvert.DeserializeObject<T_EReports>(JsonResonceString);

                    var updatedEreport = await _eReportsRepository.QueryAsync<T_EReports>("SELECT [ID] FROM [T_EReports] WHERE upper(ModelName) = '" + ThisEReport.ModelName.ToUpper() + "' AND [ID] = " + ThisEReport.ID + " AND [Updated] = 1");
                    if (updatedEreport.Count() <= 0)
                    {
                        bool result = false;

                        if (ThisEReport != null)
                        {
                            var exist = await _eReportsRepository.QueryAsync<T_EReports>("SELECT [ID] FROM [T_EReports] WHERE  [ID] = " + ThisEReport.ID);
                            //Delete Old Entries
                            if (exist.Count() > 0)
                            {
                                await _eReportsRepository.QueryAsync<T_EReports>(@"DELETE FROM [T_EReports] WHERE [ID] = '" + ThisEReport.ID + "'");
                                await _usersAssignedRepository.QueryAsync<T_EReports_UsersAssigned>("DELETE FROM [T_EReports_UsersAssigned] WHERE [EReportID] = " + ThisEReport.ID);
                                await _signaturesRepository.QueryAsync<T_EReports_Signatures>("DELETE FROM [T_EReports_Signatures] WHERE [EReportID] = " + ThisEReport.ID);
                                await _PartialRequest.QueryAsync<T_PartialRequest>("DELETE FROM [T_PartialRequest] WHERE [EReportID] = " + ThisEReport.ID);

                                string LocationPDFPath = "PDF Store/" + userProject.JobCode;
                                await DependencyService.Get<ISaveFiles>().RemoveAllFilefromFolder(LocationPDFPath);

                                string LocationPhotoPath = "Photo Store/" + userProject.JobCode;
                                await DependencyService.Get<ISaveFiles>().RemoveAllFilefromFolder(LocationPhotoPath);

                                await _drawingsRepository.QueryAsync<T_Drawings>("DELETE FROM [T_Drawings] WHERE [EReportID] = " + ThisEReport.ID);
                            }


                            await _eReportsRepository.InsertOrReplaceAsync(ThisEReport);
                            Ereport = await _eReportsRepository.GetAsync();

                            await _signaturesRepository.InsertOrReplaceAsync(ThisEReport.Signatures);
                            //   var Signatures1 = await _signaturesRepository.GetAsync();

                            await _usersAssignedRepository.InsertOrReplaceAsync(ThisEReport.UsersAssigned);
                            //var UsersAssigned = await _usersAssignedRepository.GetAsync();
                            result = true;
                        }
                        if (result)
                        {
                            if (ThisEReport.JSONString != string.Empty)
                            {
                                switch (ThisEReport.ReportType.ToUpper())
                                {
                                    case "DAILY WELD REPORT":
                                        {
                                            GetDefects = true;
                                            getWPSNos = true;
                                            getBaseMetals = true;
                                            getWelders = true;
                                            getDWRHeatNumbers = true;

                                            if (!subContractors.Contains(ThisEReport.SubContractor))
                                                subContractors.Add(ThisEReport.SubContractor);

                                            //Get DWR Class
                                            try
                                            {
                                                DWR CurrentDWR = JsonConvert.DeserializeObject<DWR>(ThisEReport.JSONString);

                                                List<T_Drawings> CurrentDrawingList = new List<T_Drawings>();

                                                if (CurrentDWR != null)
                                                {
                                                    // foreach (DWR Row in CurrentDWR)
                                                    // {
                                                    T_Drawings CurrentDrawing = new T_Drawings()
                                                    {
                                                        EReportID = ThisEReport.ID,
                                                        JobCode = userProject.JobCode,
                                                        Name = CurrentDWR.SpoolDrawingNo,
                                                        Sheet_No = CurrentDWR.SheetNo,
                                                        Revision = CurrentDWR.RevNo
                                                    };
                                                    CurrentDrawingList.Add(CurrentDrawing);

                                                    //get VI and DI images
                                                    var ImageList = ModsTools.WebServiceGet(ApiUrls.Url_getImages("EREPORTER", CurrentID), Settings.AccessToken);
                                                    List<VMHub> AllImagesList = JsonConvert.DeserializeObject<List<VMHub>>(ImageList);
                                                    if (AllImagesList.Count > 0)
                                                    {
                                                        foreach (VMHub CurrentImage in AllImagesList.ToArray())
                                                        {
                                                            //string LineNumber = string.Empty; ;
                                                            //List<string> LineNumberList = CurrentImage.DisplayName.Split('~').ToList();
                                                            //if (LineNumberList.Count > 1)
                                                            //{
                                                            //    LineNumber = LineNumberList.LastOrDefault().Split('-').FirstOrDefault();
                                                            //}
                                                            //else
                                                            //{
                                                            //    LineNumber = CurrentDWR.Number;
                                                            //}

                                                            //string Folder = string.Empty;
                                                            //if (CurrentImage.DisplayName.StartsWith("VI"))
                                                            //    Folder = ("Photo Store" + "\\" + CurrentDWR.JobCode + "\\" + CurrentID.ToString() + "\\" + LineNumber + "\\" + "VI");
                                                            //else if (CurrentImage.DisplayName.StartsWith("DI"))
                                                            //    Folder = ("Photo Store" + "\\" + CurrentDWR.JobCode + "\\" + CurrentID.ToString() + "\\" + LineNumber + "\\" + "DI");

                                                            //var InspectionPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath(Folder);

                                                            //await DependencyService.Get<ISaveFiles>().SavePictureToDisk(Device.RuntimePlatform == Device.UWP ? Folder : InspectionPath,
                                                            //    Device.RuntimePlatform == Device.UWP ? CurrentImage.FileName : Path.GetFileNameWithoutExtension(CurrentImage.FileName), Convert.FromBase64String(CurrentImage.FileBytes).ToArray());
                                                        }
                                                    }
                                                    //}
                                                }

                                                if (CurrentDrawingList.Count > 0)
                                                {
                                                    string DrawingListJson = ModsTools.ToJson(CurrentDrawingList);

                                                    string DrawingList = ModsTools.WebServicePost(ApiUrls.Url_postDrawingList(true), DrawingListJson, Settings.AccessToken);
                                                    CurrentDrawingList = JsonConvert.DeserializeObject<List<T_Drawings>>(DrawingList);

                                                    foreach (T_Drawings DWRCurrentDrawing in CurrentDrawingList.ToArray())
                                                    {
                                                        string DWRFolder = ("PDF Store" + "\\" + userProject.JobCode + "\\" + ThisEReport.ID.ToString());
                                                        byte[] DWRPDFBytes = Convert.FromBase64String(DWRCurrentDrawing.BinaryCode);
                                                        DWRCurrentDrawing.FileLocation = await DependencyService.Get<ISaveFiles>().SavePDFToDisk(DWRFolder, DWRCurrentDrawing.FileName, DWRPDFBytes);
                                                        DWRCurrentDrawing.EReportID = ThisEReport.ID;
                                                        T_Drawings CurrentDrawing = new T_Drawings
                                                        {
                                                            EReportID = ThisEReport.ID,
                                                            JobCode = userProject.JobCode,
                                                            Name = DWRCurrentDrawing.Name,
                                                            Sheet_No = "",
                                                            Total_Sheets = "",
                                                            FileName = DWRCurrentDrawing.FileName,
                                                            FileLocation = DWRCurrentDrawing.FileLocation,
                                                            Revision = "",
                                                        };

                                                        await _drawingsRepository.InsertOrReplaceAsync(CurrentDrawing);
                                                    }

                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                return false;
                                            }
                                        }
                                        break;
                                    case "RECEIVING INSPECTION REQUEST":
                                        {
                                            //Get RIR Class
                                            try
                                            {
                                                //Get Attachments from VMLIVE
                                                string VMHubJSON = ModsTools.WebServiceGet(ApiUrls.Url_getDocuments("EREPORTER", ThisEReport.ID), Settings.AccessToken);
                                                List<VMHub> CurrentVMHubList = JsonConvert.DeserializeObject<List<VMHub>>(VMHubJSON);

                                                if (CurrentVMHubList.Count > 0)
                                                {
                                                    foreach (VMHub CurrentVMHub in CurrentVMHubList.ToArray())
                                                    {
                                                        string Folder = ("PDF Store" + "\\" + userProject.JobCode + "\\" + ThisEReport.ID.ToString());
                                                        byte[] PDFBytes = Convert.FromBase64String(CurrentVMHub.FileBytes);
                                                        string getFileLocation = await DependencyService.Get<ISaveFiles>().SavePDFToDisk(Folder, CurrentVMHub.FileName, PDFBytes);

                                                        T_Drawings CurrentDrawing = new T_Drawings
                                                        {
                                                            EReportID = ThisEReport.ID,
                                                            JobCode = userProject.JobCode,
                                                            Name = CurrentVMHub.DisplayName,
                                                            Sheet_No = "",
                                                            Total_Sheets = "",
                                                            FileName = CurrentVMHub.FileName,
                                                            FileLocation = getFileLocation,
                                                            Revision = "",
                                                        };
                                                        await _drawingsRepository.InsertOrReplaceAsync(CurrentDrawing);
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                return false;
                                            }
                                        }
                                        break;
                                    case "MATERIAL RECEIVING REPORT":
                                        {
                                            //Get MRR Class
                                            try
                                            {
                                                MRR thisMRR = JsonConvert.DeserializeObject<MRR>(ThisEReport.JSONString);

                                                if (thisMRR.Store_Location != string.Empty)
                                                {
                                                    if (!storeLocationList.Contains(thisMRR.Store_Location))
                                                    {
                                                        storeLocationList.Add(thisMRR.Store_Location);
                                                        getStorageAreas = true;
                                                    }
                                                }


                                                //Get Attachments from VMLIVE

                                                string MRRVMHubJSON = ModsTools.WebServiceGet(ApiUrls.Url_getDocuments("EREPORTER", ThisEReport.ID), Settings.AccessToken);
                                                List<VMHub> MRRCurrentVMHubList = JsonConvert.DeserializeObject<List<VMHub>>(MRRVMHubJSON);

                                                //string VMHubJSON = ModsTools.WebServiceGet("VMHub/GetDocuments?Module=EREPORTER&LinkedID=" + ThisEReport.ID, _pageHelper.Token);

                                                //List<VMHub> CurrentVMHubList = JsonConvert.DeserializeObject<List<VMHub>>(VMHubJSON);

                                                if (MRRCurrentVMHubList.Count > 0)
                                                {
                                                    foreach (VMHub MRRCurrentVMHub in MRRCurrentVMHubList.ToArray())
                                                    {
                                                        string MRRFolder = ("PDF Store" + "\\" + userProject.JobCode + "\\" + ThisEReport.ID.ToString());
                                                        byte[] MRRPDFBytes = Convert.FromBase64String(MRRCurrentVMHub.FileBytes);
                                                        string MRRgetFileLocation = await DependencyService.Get<ISaveFiles>().SavePDFToDisk(MRRFolder, MRRCurrentVMHub.FileName, MRRPDFBytes);

                                                        T_Drawings MRRCurrentDrawing = new T_Drawings
                                                        {
                                                            EReportID = ThisEReport.ID,
                                                            JobCode = userProject.JobCode,
                                                            Name = MRRCurrentVMHub.DisplayName,
                                                            Sheet_No = "",
                                                            Total_Sheets = "",
                                                            FileName = MRRCurrentVMHub.FileName,
                                                            FileLocation = MRRgetFileLocation,
                                                            Revision = "",
                                                        };
                                                        await _drawingsRepository.InsertOrReplaceAsync(MRRCurrentDrawing);
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                return false;
                                            }
                                        }
                                        break;
                                    case "CONSTRUCTION MATERIAL REQUISITION":
                                        {
                                            //Get CMR Class
                                            try
                                            {
                                                getCMRStorageAreas = true;
                                                getCMRHeatNos = true;
                                                CMR thisCMR = JsonConvert.DeserializeObject<CMR>(ThisEReport.JSONString);

                                                foreach (CMRSummaryRows Row in thisCMR.CMRSummaryRows)
                                                {
                                                    CMRHHStorageArea sa = new CMRHHStorageArea()
                                                    {
                                                        JobCode = thisCMR.JobCode,
                                                        Store_Location = thisCMR.Store_Location,
                                                        PJ_Commodity = Row.PJ_Commodity,
                                                        Sub_Commodity = Row.Sub_Commodity,
                                                        Size_Descr = Row.Size_Descr
                                                    };
                                                    storageAreas.Add(sa);

                                                    CMRHHHeatNos hn = new CMRHHHeatNos()
                                                    {
                                                        JobCode = thisCMR.JobCode,
                                                        Store_Location = thisCMR.Store_Location,
                                                        PJ_Commodity = Row.PJ_Commodity,
                                                        Sub_Commodity = Row.Sub_Commodity,
                                                        Size_Descr = Row.Size_Descr
                                                    };
                                                    heatNos.Add(hn);
                                                }

                                                //Get Attachments from VMLIVE
                                                string CMRVMHubJSON = ModsTools.WebServiceGet(ApiUrls.Url_getDocuments("EREPORTER", ThisEReport.ID), Settings.AccessToken);
                                                List<VMHub> CMRCurrentVMHubList = JsonConvert.DeserializeObject<List<VMHub>>(CMRVMHubJSON);

                                                //string VMHubJSON = ModsTools.WebServiceGet("VMHub/GetDocuments?Module=EREPORTER&LinkedID=" + ThisEReport.ID, CurrentPageHelper.Token);
                                                //List<VMHub> CurrentVMHubList = JsonConvert.DeserializeObject<List<VMHub>>(VMHubJSON);

                                                if (CMRCurrentVMHubList.Count > 0)
                                                {
                                                    foreach (VMHub CMRCurrentVMHub in CMRCurrentVMHubList.ToArray())
                                                    {
                                                        string CMRFolder = ("PDF Store" + "\\" + userProject.JobCode + "\\" + ThisEReport.ID.ToString());
                                                        byte[] CMRPDFBytes = Convert.FromBase64String(CMRCurrentVMHub.FileBytes);
                                                        string CMRgetFileLocation = await DependencyService.Get<ISaveFiles>().SavePDFToDisk(CMRFolder, CMRCurrentVMHub.FileName, CMRPDFBytes);

                                                        T_Drawings CMRCurrentDrawing = new T_Drawings
                                                        {
                                                            EReportID = ThisEReport.ID,
                                                            JobCode = userProject.JobCode,
                                                            Name = CMRCurrentVMHub.DisplayName,
                                                            Sheet_No = "",
                                                            Total_Sheets = "",
                                                            FileName = CMRCurrentVMHub.FileName,
                                                            FileLocation = CMRgetFileLocation,
                                                            Revision = "",
                                                        };

                                                        await _drawingsRepository.InsertOrReplaceAsync(CMRCurrentDrawing);
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                                return false;
                                            }

                                        }
                                        break;
                                }
                            }

                            if (GetDefects)
                            {
                                string RTDefectsJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getRTDefects(userProject.Project_ID), Settings.AccessToken);
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
                            }

                            if (getWelders)
                            {
                                if (subContractors.Count() > 0)
                                {
                                    foreach (string _subContractor in subContractors)
                                    {
                                        string WeldersJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getWelders(userProject.Project_ID, userProject.JobCode, _subContractor), Settings.AccessToken);
                                        var Welders = JsonConvert.DeserializeObject<List<string>>(WeldersJsonString);
                                        await _weldersRepository.QueryAsync<T_Welders>("DELETE FROM [T_Welders] WHERE [Project_ID] = " + userProject.Project_ID + " AND [SubContractor] = '" + _subContractor + "'");

                                        foreach (string value in Welders)
                                        {
                                            T_Welders Welder = new T_Welders();
                                            Welder.Welder_Code = value.Split(new string[] { " - " }, StringSplitOptions.None)[0].Trim().Replace("'", "''");
                                            Welder.Welder_Name = value.Split(new string[] { " - " }, StringSplitOptions.None)[1].Trim().Replace("'", "''");

                                            Welder.Project_ID = userProject.Project_ID;
                                            Welder.SubContractor = _subContractor;
                                            await _weldersRepository.InsertOrReplaceAsync(Welder);
                                        }
                                    }
                                }
                            }

                            //  add this code by prashant 
                            if (getStorageAreas)
                            {
                                if (storeLocationList.Count > 0)
                                {
                                    foreach (string storelocation in storeLocationList)
                                    {
                                        string storageareasjsonstring = ModsTools.WebServiceGet(ApiUrls.Url_getStorageAreas(userProject.Project_ID, userProject.JobCode, storelocation), Settings.AccessToken);
                                        List<string> storageAreaList = JsonConvert.DeserializeObject<List<string>>(storageareasjsonstring);
                                        if (storageAreaList != null && storageAreaList.Count > 0)
                                        {
                                            await _StorageAreasRepository.QueryAsync<T_StorageAreas>("DELETE FROM [T_StorageAreas] WHERE [PROJECT_ID] = " + userProject.Project_ID + " AND [STORE_LOCATION] = '" + storelocation.Replace("'", "''") + "'");
                                            foreach (string value in storageAreaList)
                                            {
                                                T_StorageAreas StorageAreas = new T_StorageAreas
                                                {
                                                    Project_ID = userProject.Project_ID,
                                                    Store_Location = storelocation.Replace("'", "''"),
                                                    Storage_Area_Code = value.Replace("'", "''"),
                                                    Sheet_No = "",
                                                };
                                                await _StorageAreasRepository.InsertOrReplaceAsync(StorageAreas);
                                            }
                                        }
                                    }
                                }
                            }

                            if (getDWRHeatNumbers)
                            {
                                string HeatNumbersJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getHeatNumbers(userProject.Project_ID, userProject.JobCode), Settings.AccessToken);
                                var HeatNumbers = JsonConvert.DeserializeObject<List<string>>(HeatNumbersJsonString);
                                if (HeatNumbers != null && HeatNumbers.Count > 0)
                                {
                                    await _DWR_HeatNosRepository.QueryAsync<T_DWR_HeatNos>("DELETE FROM [T_DWR_HeatNos] WHERE [Project_ID] = " + userProject.Project_ID);
                                    foreach (string value in HeatNumbers)
                                    {
                                        T_DWR_HeatNos DWR_HeatNos = new T_DWR_HeatNos();

                                        DWR_HeatNos.Project_ID = userProject.Project_ID;
                                        DWR_HeatNos.Ident_Code = value.Split(new string[] { " - " }, 2, StringSplitOptions.None)[0].Trim();
                                        DWR_HeatNos.Heat_No = value.Split(new string[] { " - " }, 2, StringSplitOptions.None)[1].Trim();

                                        await _DWR_HeatNosRepository.InsertOrReplaceAsync(DWR_HeatNos);
                                    }
                                }
                            }

                            //  add this code by prashant 
                            if (getCMRStorageAreas)
                            {
                                string postJSON = JsonConvert.SerializeObject(storageAreas);
                                string AllStorageAreasJsonString = ModsTools.WebServicePost(ApiUrls.Url_postAllStorageAreas(userProject.Project_ID, userProject.JobCode), postJSON, Settings.AccessToken);
                                List<CMRHHStorageArea> AllStorageAreas = JsonConvert.DeserializeObject<List<CMRHHStorageArea>>(AllStorageAreasJsonString);
                                if (AllStorageAreas != null && AllStorageAreas.Count > 0)
                                {
                                    await _CMS_AllStorageAreasRepository.QueryAsync<T_CMR_StorageAreas>("DELETE FROM [T_CMR_StorageAreas] WHERE [Project_ID] = " + userProject.Project_ID);
                                    foreach (CMRHHStorageArea value in AllStorageAreas)
                                    {
                                        T_CMR_StorageAreas CMRStorageArea = new T_CMR_StorageAreas
                                        {
                                            Project_ID = userProject.Project_ID,
                                            Job_Code_Key = value.JobCode.Replace("'", "''"),
                                            Store_Location = value.Store_Location.Replace("'", "''"),
                                            Storage_Area = value.Storage_Area.Replace("'", "''"),
                                            PJ_Commodity = value.PJ_Commodity.Replace("'", "''"),
                                            Sub_Commodity = value.Sub_Commodity.Replace("'", "''"),
                                            Size_Descr = value.Size_Descr.Replace("'", "''"),
                                            Avail_Stock_Qty = value.Avail_Stock_Qty
                                        };
                                        await _CMS_AllStorageAreasRepository.InsertOrReplaceAsync(CMRStorageArea);
                                    }
                                }

                            }

                            // add this code by prashant 
                            if (getCMRHeatNos)
                            {
                                string postJSON = JsonConvert.SerializeObject(heatNos);
                                string AllHeatNosJsonString = ModsTools.WebServicePost(ApiUrls.Url_postAllHeatNos(userProject.Project_ID, userProject.JobCode), postJSON, Settings.AccessToken);
                                List<CMRHHHeatNos> AllHeatNos = JsonConvert.DeserializeObject<List<CMRHHHeatNos>>(AllHeatNosJsonString);
                                if (AllHeatNos != null && AllHeatNos.Count > 0)
                                {
                                    await _CMR_HeatNosRepository.QueryAsync<T_CMR_HeatNos>("DELETE FROM [T_CMR_HeatNos] WHERE [Project_ID] = " + userProject.Project_ID);
                                    foreach (CMRHHHeatNos value in AllHeatNos)
                                    {
                                        T_CMR_HeatNos HeatNo = new T_CMR_HeatNos
                                        {
                                            Project_ID = userProject.Project_ID,
                                            Job_Code_Key = value.JobCode.Replace("'", "''"),
                                            Store_Location = value.Store_Location.Replace("'", "''"),
                                            Heat_No = value.Heat_No.Replace("'", "''"),
                                            PJ_Commodity = value.PJ_Commodity.Replace("'", "''"),
                                            Sub_Commodity = value.Sub_Commodity.Replace("'", "''"),
                                            Size_Descr = value.Size_Descr.Replace("'", "''"),
                                        };
                                        await _CMR_HeatNosRepository.InsertOrReplaceAsync(HeatNo);
                                    }
                                }


                            }

                            if (getWPSNos)
                            {
                                string WPSNosJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getWPSNos(userProject.Project_ID, userProject.JobCode), Settings.AccessToken);
                                var WPSNos = JsonConvert.DeserializeObject<List<string>>(WPSNosJsonString);
                                if (WPSNos != null && WPSNos.Count > 0)
                                {
                                    await _WPS_MSTRRepository.QueryAsync<T_WPS_MSTR>("DELETE FROM [T_WPS_MSTR] WHERE [Project_ID] = " + userProject.Project_ID);
                                    foreach (string value in WPSNos)
                                    {
                                        T_WPS_MSTR WPS_MSTR = new T_WPS_MSTR();
                                        WPS_MSTR.Project_ID = userProject.Project_ID;
                                        WPS_MSTR.Wps_No = value.Split(new string[] { " - " }, StringSplitOptions.None)[0].Trim().Replace("'", "''");
                                        WPS_MSTR.Description = value.Split(new string[] { " - " }, StringSplitOptions.None)[1].Trim().Replace("'", "''");

                                        await _WPS_MSTRRepository.InsertOrReplaceAsync(WPS_MSTR);
                                    }
                                }
                            }

                            if (getBaseMetals)
                            {
                                string BaseMetalsJsonString = ModsTools.WebServiceGet(ApiUrls.Url_getBaseMetals(userProject.Project_ID), Settings.AccessToken);
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

                            }
                            // } 
                        }
                    }
                }
                else if (Settings.ModuleName == "TestPackage")
                {
                    T_ETestPackages ThisETestPackage = null;
                    string JsonString = string.Empty;
                    var UserProjectList = await _userProjectRepository.GetAsync();
                    if (UserProjectList.Count > 0)
                        userProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();


                    var updatedETP = await _eTestPackagesRepository.QueryAsync<T_ETestPackages>("SELECT [ID] FROM [T_ETestPackages] WHERE [ProjectID] = " + userProject.Project_ID + " AND [ID] = " + CurrentID + " AND [Updated] = 1");
                    if (updatedETP.Count() <= 0)
                    {
                        var exist = await _eTestPackagesRepository.QueryAsync<T_ETestPackages>("SELECT [ID] FROM [T_ETestPackages] WHERE [ID] = '" + CurrentID + "'");
                        //Delete Old Entries
                        if (exist.Count() > 0)
                        {
                            await _eTestPackagesRepository.QueryAsync<T_ETestPackages>(@"DELETE FROM [T_ETestPackages] WHERE [ProjectID] = '" + userProject.Project_ID + "' AND [ID] = '" + CurrentID + "'");
                            await _controlLogSignatureRepository.QueryAsync<T_ControlLogSignature>("DELETE FROM [T_ControlLogSignature] WHERE [ProjectID] = " + userProject.Project_ID + " AND [ETestPackageID] = '" + CurrentID + "'");
                            await _testLimitDrawingRepository.QueryAsync<T_TestLimitDrawing>("DELETE FROM [T_TestLimitDrawing] WHERE [ProjectID] = " + userProject.Project_ID + " AND [ETestPackageID] = '" + CurrentID + "'");
                            await _attachedDocumentRepository.QueryAsync<T_AttachedDocument>("DELETE FROM [T_AttachedDocument] WHERE [ProjectID] = " + userProject.Project_ID + " AND [ETestPackageID] = '" + CurrentID + "'");
                            await _punchListRepository.QueryAsync<T_PunchList>("DELETE FROM [T_PunchList] WHERE [ProjectID] = " + userProject.Project_ID + " AND [ETestPackageID] = '" + CurrentID + "'");
                            await _punchImageRepository.QueryAsync<T_PunchImage>("DELETE FROM [T_PunchImage] WHERE [ProjectID] = " + userProject.Project_ID + " AND [ETestPackageID] = '" + CurrentID + "'");

                            await _testRecordDetailsRepository.QueryAsync<T_TestRecordDetails>("DELETE FROM [T_TestRecordDetails] WHERE [ProjectID] = " + userProject.Project_ID + " AND [ETestPackageID] = '" + CurrentID + "'");
                            await _testRecordConfirmationRepository.QueryAsync<T_TestRecordConfirmation>("DELETE FROM [T_TestRecordConfirmation] WHERE [ProjectID] = " + userProject.Project_ID + " AND [ETestPackageID] = '" + CurrentID + "'");
                            await _testRecordAcceptedByRepository.QueryAsync<T_TestRecordAcceptedBy>("DELETE FROM [T_TestRecordAcceptedBy] WHERE [ProjectID] = " + userProject.Project_ID + " AND [ETestPackageID] = '" + CurrentID + "'");

                            await _drainRecordContentRepository.QueryAsync<T_DrainRecordContent>("DELETE FROM [T_DrainRecordContent] WHERE [ProjectID] = " + userProject.Project_ID + " AND [ETestPackageID] = '" + CurrentID + "'");
                            await _drainRecordAcceptedByRepository.QueryAsync<T_DrainRecordAcceptedBy>("DELETE FROM [T_DrainRecordAcceptedBy] WHERE [ProjectID] = " + userProject.Project_ID + " AND [ETestPackageID] = '" + CurrentID + "'");

                            await _testRecordImageRepository.QueryAsync<T_TestRecordImage>("DELETE FROM [T_TestRecordImage] WHERE [ProjectID] = '" + userProject.Project_ID + "' AND [ETestPackageID] = '" + CurrentID + "'");

                            await _PreTestRecordContentRepository.QueryAsync<T_PreTestRecordContent>("DELETE FROM [T_PreTestRecordContent] WHERE [ProjectID] = " + userProject.Project_ID + " AND [ETestPackageID] = '" + CurrentID + "'");
                            await _PreTestRecordAcceptedByRepository.QueryAsync<T_PreTestRecordAcceptedBy>("DELETE FROM [T_PreTestRecordAcceptedBy] WHERE [ProjectID] = " + userProject.Project_ID + " AND [ETestPackageID] = '" + CurrentID + "'");

                            await _PostTestRecordContentRepository.QueryAsync<T_PostTestRecordContent>("DELETE FROM [T_PostTestRecordContent] WHERE [ProjectID] = " + userProject.Project_ID + " AND [ETestPackageID] = '" + CurrentID + "'");
                            await _PostTestRecordAcceptedByrRepository.QueryAsync<T_PostTestRecordAcceptedBy>("DELETE FROM [T_PostTestRecordAcceptedBy] WHERE [ProjectID] = " + userProject.Project_ID + " AND [ETestPackageID] = '" + CurrentID + "'");

                        }

                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getETestPackageHeaders(userProject.ModelName), Settings.AccessToken);
                        ETestPackages = new ObservableCollection<T_ETestPackages>(JsonConvert.DeserializeObject<List<T_ETestPackages>>(JsonString));
                        var Current = ETestPackages.Where(s => s.ID == CurrentID).Distinct().ToList();
                        foreach (T_ETestPackages item in Current)
                            ThisETestPackage = item;
                        if (ThisETestPackage != null)
                        {
                            ThisETestPackage.ProjectID = userProject.Project_ID;
                            await _eTestPackagesRepository.InsertOrReplaceAsync(ThisETestPackage);
                        }

                        //Get Control Log Signatures
                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getControlLogSignatures(CurrentID), Settings.AccessToken);
                        List<T_ControlLogSignature> controlLogSignatureList = JsonConvert.DeserializeObject<List<T_ControlLogSignature>>(JsonString);

                        if (controlLogSignatureList != null && controlLogSignatureList.Count > 0)
                        {
                            controlLogSignatureList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ETestPackageID = CurrentID; });
                            await _controlLogSignatureRepository.InsertOrReplaceAsync(controlLogSignatureList);
                        }


                        //Get Drain Record Content Signatures
                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getDrainRecordContent(CurrentID), Settings.AccessToken);
                        List<T_DrainRecordContent> drainRecordContentSignatureList = JsonConvert.DeserializeObject<List<T_DrainRecordContent>>(JsonString);

                        if (drainRecordContentSignatureList != null && drainRecordContentSignatureList.Count > 0)
                        {
                            drainRecordContentSignatureList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ETestPackageID = CurrentID; });
                            await _drainRecordContentRepository.InsertOrReplaceAsync(drainRecordContentSignatureList);
                        }


                        //Get Drain Record Accepted By Signatures
                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getDrainRecordAcceptedBy(CurrentID), Settings.AccessToken);
                        List<T_DrainRecordAcceptedBy> drainRecordAcceptedBySignatureList = JsonConvert.DeserializeObject<List<T_DrainRecordAcceptedBy>>(JsonString);

                        if (drainRecordAcceptedBySignatureList != null && drainRecordAcceptedBySignatureList.Count > 0)
                        {
                            drainRecordAcceptedBySignatureList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ETestPackageID = CurrentID; });
                            await _drainRecordAcceptedByRepository.InsertOrReplaceAsync(drainRecordAcceptedBySignatureList);
                        }



                        //Get Test Record Details
                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getTestRecordDetails(CurrentID), Settings.AccessToken);
                        List<T_TestRecordDetails> testRecordDetailsList = JsonConvert.DeserializeObject<List<T_TestRecordDetails>>(JsonString);

                        if (testRecordDetailsList != null && testRecordDetailsList.Count > 0)
                        {
                            testRecordDetailsList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ProjectName = userProject.ProjectName; x.ETestPackageID = CurrentID; });
                            await _testRecordDetailsRepository.InsertOrReplaceAsync(testRecordDetailsList);
                        }


                        //Get Test Record Content Signatures
                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getTestRecordConfirmation(CurrentID), Settings.AccessToken);
                        List<T_TestRecordConfirmation> testrecordConfirmationList = JsonConvert.DeserializeObject<List<T_TestRecordConfirmation>>(JsonString);

                        if (testrecordConfirmationList != null && testrecordConfirmationList.Count > 0)
                        {
                            testrecordConfirmationList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ETestPackageID = CurrentID; });
                            await _testRecordConfirmationRepository.InsertOrReplaceAsync(testrecordConfirmationList);
                        }

                        //Get Test Record Accepted By Signatures
                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getTestRecordAcceptedBy(CurrentID), Settings.AccessToken);
                        List<T_TestRecordAcceptedBy> testRecordAcceptedBySignatureList = JsonConvert.DeserializeObject<List<T_TestRecordAcceptedBy>>(JsonString);

                        if (testRecordAcceptedBySignatureList != null && testRecordAcceptedBySignatureList.Count > 0)
                        {
                            testRecordAcceptedBySignatureList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ProjectName = userProject.ProjectName; x.ETestPackageID = CurrentID; });
                            await _testRecordAcceptedByRepository.InsertOrReplaceAsync(testRecordAcceptedBySignatureList);
                        }

                        //Get PreTest Record Content Signatures
                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getPreTestRecordContent(CurrentID), Settings.AccessToken);
                        List<T_PreTestRecordContent> PretestrecordConfirmationList = JsonConvert.DeserializeObject<List<T_PreTestRecordContent>>(JsonString);

                        if (PretestrecordConfirmationList != null && PretestrecordConfirmationList.Count > 0)
                        {
                            PretestrecordConfirmationList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ETestPackageID = CurrentID; });
                            await _PreTestRecordContentRepository.InsertOrReplaceAsync(PretestrecordConfirmationList);
                        }

                        //Get PretTest Record Accepted By Signatures
                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getPretTestRecordAcceptedBy(CurrentID), Settings.AccessToken);
                        List<T_PreTestRecordAcceptedBy> PretestRecordAcceptedBySignatureList = JsonConvert.DeserializeObject<List<T_PreTestRecordAcceptedBy>>(JsonString);

                        if (PretestRecordAcceptedBySignatureList != null && PretestRecordAcceptedBySignatureList.Count > 0)
                        {
                            PretestRecordAcceptedBySignatureList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ETestPackageID = CurrentID; });
                            await _PreTestRecordAcceptedByRepository.InsertOrReplaceAsync(PretestRecordAcceptedBySignatureList);
                        }

                        //Get PostTest Record Content Signatures
                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getPostTestRecordContent(CurrentID), Settings.AccessToken);
                        List<T_PostTestRecordContent> PosttestrecordConfirmationList = JsonConvert.DeserializeObject<List<T_PostTestRecordContent>>(JsonString);

                        if (PosttestrecordConfirmationList != null && PosttestrecordConfirmationList.Count > 0)
                        {
                            PosttestrecordConfirmationList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ETestPackageID = CurrentID; });
                            await _PostTestRecordContentRepository.InsertOrReplaceAsync(PosttestrecordConfirmationList);
                        }

                        //Get PosttTest Record Accepted By Signatures
                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getPostTestRecordAcceptedBy(CurrentID), Settings.AccessToken);
                        List<T_PostTestRecordAcceptedBy> PosttestRecordAcceptedBySignatureList = JsonConvert.DeserializeObject<List<T_PostTestRecordAcceptedBy>>(JsonString);

                        if (PosttestRecordAcceptedBySignatureList != null && PosttestRecordAcceptedBySignatureList.Count > 0)
                        {
                            PosttestRecordAcceptedBySignatureList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ETestPackageID = CurrentID; });
                            await _PostTestRecordAcceptedByrRepository.InsertOrReplaceAsync(PosttestRecordAcceptedBySignatureList);
                        }


                        //Get Attached Documents
                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getAttachedDocuments(CurrentID), Settings.AccessToken);
                        List<T_AttachedDocument> attachedDocumentList = JsonConvert.DeserializeObject<List<T_AttachedDocument>>(JsonString);

                        if (attachedDocumentList != null && attachedDocumentList.Count > 0)
                        {
                            attachedDocumentList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ETestPackageID = CurrentID; });
                            await _attachedDocumentRepository.InsertOrReplaceAsync(attachedDocumentList);
                        }


                        //Get Test limit Drawings
                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getTestLimitDrawings(CurrentID), Settings.AccessToken);
                        List<T_TestLimitDrawing> testLimitDrawingList = JsonConvert.DeserializeObject<List<T_TestLimitDrawing>>(JsonString);

                        if (testLimitDrawingList != null && testLimitDrawingList.Count > 0)
                        {
                            testLimitDrawingList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ETestPackageID = CurrentID; });
                            await _testLimitDrawingRepository.InsertOrReplaceAsync(testLimitDrawingList);
                        }



                        //Get Punch Items
                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getPunchItems(CurrentID), Settings.AccessToken);
                        List<T_PunchList> punchItemList = JsonConvert.DeserializeObject<List<T_PunchList>>(JsonString);
                        var count = await _punchListRepository.QueryAsync<T_PunchList>("SELECT * FROM [T_PunchList]");
                        int total = count.Count();
                        if (punchItemList != null && punchItemList.Count > 0)
                        {
                            punchItemList.ForEach(x => { x.ID = ++total; x.ProjectID = userProject.Project_ID; x.ETestPackageID = CurrentID; });
                            await _punchListRepository.InsertOrReplaceAsync(punchItemList);
                        }

                        //Get Punch Images
                        JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getPunchImages(CurrentID), Settings.AccessToken);
                        List<T_PunchImage> punchImageList = JsonConvert.DeserializeObject<List<T_PunchImage>>(JsonString);

                        if (punchImageList != null && punchImageList.Count > 0)
                        {
                            punchImageList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ETestPackageID = CurrentID; });
                            await _punchImageRepository.InsertOrReplaceAsync(punchImageList);
                        }
                        bool RemoveDownloadFlag = ModsTools.WebServicePostBoolean(ApiUrls.Url_postRemoveDownloadFlag(CurrentID, userProject.User_ID), Settings.AccessToken);

                    }

                    //Folders Download
                    JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getFolders(userProject.Project_ID), Settings.AccessToken);
                    List<string> folderList = JsonConvert.DeserializeObject<List<string>>(JsonString);

                    if (folderList != null && folderList.Count > 0)
                    {
                        List<T_AdminFolders> adminfolder = new List<T_AdminFolders>();
                        var count = await _adminFoldersRepository.QueryAsync<T_AdminFolders>("SELECT * FROM [T_AdminFolders]");
                        int total = count.Count();
                        foreach (string item in folderList)
                        {
                            T_AdminFolders ad = new T_AdminFolders()
                            {
                                ID = ++total,
                                ProjectID = userProject.Project_ID,
                                FolderName = item
                            };
                            adminfolder.Add(ad);
                        }

                        await _adminFoldersRepository.InsertOrReplaceAsync(adminfolder);
                    }


                    //Control Log Download
                    JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getAdminControlLog(userProject.Project_ID), Settings.AccessToken);
                    List<T_AdminControlLog> adminControlLogList = JsonConvert.DeserializeObject<List<T_AdminControlLog>>(JsonString);

                    if (adminControlLogList != null && adminControlLogList.Count > 0)
                    {
                        adminControlLogList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ProjectName = userProject.ProjectName; });                      

                        await _adminControlLogRepository.InsertOrReplaceAsync(adminControlLogList);

                        await _adminControlLogPunchLayerRepository.QueryAsync<T_AdminControlLogPunchLayer>("DELETE FROM [T_AdminControlLogPunchLayer] WHERE [ProjectID] = '" + userProject.Project_ID + "'");
                        await _adminControlLogPunchCategoryRepository.QueryAsync<T_AdminControlLogPunchCategory>("DELETE FROM [T_AdminControlLogPunchCategory] WHERE [ProjectID] = '" + userProject.Project_ID + "'");
                        await _adminControlLogActionPartyRepository.QueryAsync<T_AdminControlLogActionParty>("DELETE FROM [T_AdminControlLogActionParty] WHERE [ProjectID] = '" + userProject.Project_ID + "'");
                        await _adminControlLogFolderRepository.QueryAsync<T_AdminControlLogFolder>("DELETE FROM [T_AdminControlLogFolder] WHERE [ProjectID] = '" + userProject.Project_ID + "'");
                        await _adminControlLogNaAutoSignaturesRepository.QueryAsync<T_AdminControlLogNaAutoSignatures>("DELETE FROM [T_AdminControlLogNaAutoSignatures] WHERE [ProjectID] = '" + userProject.Project_ID + "'");

                        foreach (T_AdminControlLog List in adminControlLogList)
                        {
                            foreach (int item in List.PunchLayer)
                            {

                                await _adminControlLogPunchLayerRepository.InsertOrReplaceAsync(new T_AdminControlLogPunchLayer { ProjectID = userProject.Project_ID, ControlLogAdminID = List.ID, PunchAdminId = item });
                            }
                            foreach (string item in List.PunchCategory)
                            {
                                //T_AdminControlLogPunchCategory
                                await _adminControlLogPunchCategoryRepository.InsertOrReplaceAsync(new T_AdminControlLogPunchCategory { ProjectID = userProject.Project_ID, ControlLogAdminID = List.ID, Category = item });
                            }
                            foreach (string item in List.PunchActionParty)
                            {
                                //T_AdminControlLogActionParty
                                await _adminControlLogActionPartyRepository.InsertOrReplaceAsync(new T_AdminControlLogActionParty { ProjectID = userProject.Project_ID, ControlLogAdminID = List.ID, FunctionCode = item });
                            }
                            foreach (int item in List.Folder)
                            {
                                //T_AdminControlLogFolder
                                await _adminControlLogFolderRepository.InsertOrReplaceAsync(new T_AdminControlLogFolder { ProjectID = userProject.Project_ID, ControlLogAdminID = List.ID, FolderAdminID = item });
                            }

                            foreach (int item in List.NAAutoSignOffControlLogID ?? new List<int>())
                            {
                                //T_AdminControlLogNaAutoSignatures
                                await _adminControlLogNaAutoSignaturesRepository.InsertOrReplaceAsync(new T_AdminControlLogNaAutoSignatures { ProjectID = userProject.Project_ID, ControlLogAdminID = List.ID, AutoSignOffControlLogAdminId = item });
                            }

                        }

                    }

                    //Download Preset Punches
                    JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getPresetPunches(userProject.Project_ID, userProject.JobCode), Settings.AccessToken);
                    List<T_AdminPresetPunches> presetPunchList = JsonConvert.DeserializeObject<List<T_AdminPresetPunches>>(JsonString);

                    if (presetPunchList != null && presetPunchList.Count > 0)
                    {
                        await _adminPresetPunchesRepository.QueryAsync("DELETE FROM [T_AdminPresetPunches] WHERE [ProjectID] = '" + userProject.Project_ID + "'");
                        presetPunchList.ForEach(x => x.ProjectID = userProject.Project_ID);
                        await _adminPresetPunchesRepository.InsertOrReplaceAsync(presetPunchList);
                    }

                    //Download Punch Categories
                    JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getPunchCategories(userProject.Project_ID, userProject.JobCode), Settings.AccessToken);
                    List<T_AdminPunchCategories> punchCategoryList = JsonConvert.DeserializeObject<List<T_AdminPunchCategories>>(JsonString);

                    if (punchCategoryList != null && punchCategoryList.Count > 0)
                    {
                        await _adminPunchCategoriesRepository.QueryAsync("DELETE FROM [T_AdminPunchCategories] WHERE [ProjectID] = '" + userProject.Project_ID + "'");
                        punchCategoryList.ForEach(x => x.ProjectID = userProject.Project_ID);
                        await _adminPunchCategoriesRepository.InsertOrReplaceAsync(punchCategoryList);
                    }

                    //Download Function Codes
                    JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getFunctionCodes(userProject.Project_ID), Settings.AccessToken);
                    List<string> functionCodeList = JsonConvert.DeserializeObject<List<string>>(JsonString);

                    if (functionCodeList != null && functionCodeList.Count > 0)
                    {
                        List<T_AdminFunctionCodes> FunctionCodes = new List<T_AdminFunctionCodes>();

                        await _adminFunctionCodesRepository.QueryAsync("DELETE FROM [T_AdminFunctionCodes] WHERE [ProjectID] = '" + userProject.Project_ID + "'");

                        foreach (string item in functionCodeList)
                        {
                            T_AdminFunctionCodes afc = new T_AdminFunctionCodes()
                            {
                                ProjectID = userProject.Project_ID,
                                FunctionCode = item.Split(new string[] { " - " }, 2, StringSplitOptions.None)[0].Trim(),
                                Description = item.Split(new string[] { " - " }, 2, StringSplitOptions.None)[1].Trim()
                            };
                            FunctionCodes.Add(afc);
                        }
                        await _adminFunctionCodesRepository.InsertOrReplaceAsync(FunctionCodes);
                    }

                    //Download Punch Layers
                    JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getAdminPunchLayer(userProject.Project_ID), Settings.AccessToken);
                    List<T_AdminPunchLayer> adminPunchLayerList = JsonConvert.DeserializeObject<List<T_AdminPunchLayer>>(JsonString);

                    if (adminPunchLayerList != null && adminPunchLayerList.Count > 0)
                    {
                        adminPunchLayerList.ForEach(x => x.ProjectID = userProject.Project_ID);
                        await _adminPunchLayerRepository.InsertOrReplaceAsync(adminPunchLayerList);
                    }



                    //Download Drain Record Template
                    JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getDrainRecordAdminCertificationContent(userProject.Project_ID), Settings.AccessToken);
                    List<T_AdminDrainRecordContent> adminCertificationContentList = JsonConvert.DeserializeObject<List<T_AdminDrainRecordContent>>(JsonString);

                    if (adminCertificationContentList != null && adminCertificationContentList.Count > 0)
                    {
                        adminCertificationContentList.ForEach(x => x.ProjectID = userProject.Project_ID);
                        await _adminDrainRecordContentRepository.InsertOrReplaceAsync(adminCertificationContentList);
                    }


                    JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getDrainRecordAdminCertificationAcceptedBy(userProject.Project_ID), Settings.AccessToken);
                    List<T_AdminDrainRecordAcceptedBy> adminCertificationAcceptedByList = JsonConvert.DeserializeObject<List<T_AdminDrainRecordAcceptedBy>>(JsonString);

                    if (adminCertificationAcceptedByList != null && adminCertificationAcceptedByList.Count > 0)
                    {
                        adminCertificationAcceptedByList.ForEach(x => x.ProjectID = userProject.Project_ID);
                        await _adminDrainRecordAcceptedBy.InsertOrReplaceAsync(adminCertificationAcceptedByList);
                    }


                    //Download Test Record Template
                    JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getTestRecordAdminCertificationDetails(userProject.Project_ID), Settings.AccessToken);
                    List<T_AdminTestRecordDetails> adminCertificationDetailsList = JsonConvert.DeserializeObject<List<T_AdminTestRecordDetails>>(JsonString);

                    if (adminCertificationDetailsList != null && adminCertificationDetailsList.Count > 0)
                    {
                        adminCertificationDetailsList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ProjectName = userProject.ProjectName; });
                        await _adminTestRecordDetailsRepository.InsertOrReplaceAsync(adminCertificationDetailsList);
                    }


                    JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getTestRecordAdminCertificationContent(userProject.Project_ID), Settings.AccessToken);
                    List<T_AdminTestRecordConfirmation> adminTestRecordConfirmationContentList = JsonConvert.DeserializeObject<List<T_AdminTestRecordConfirmation>>(JsonString);

                    if (adminTestRecordConfirmationContentList != null && adminTestRecordConfirmationContentList.Count > 0)
                    {
                        adminTestRecordConfirmationContentList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ProjectName = userProject.ProjectName; });
                        await _adminTestRecordConfirmationRepository.InsertOrReplaceAsync(adminTestRecordConfirmationContentList);
                    }


                    JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getTestRecordAdminCertificationAcceptedBy(userProject.Project_ID), Settings.AccessToken);
                    List<T_AdminTestRecordAcceptedBy> adminTestRecordCertificationAcceptedByList = JsonConvert.DeserializeObject<List<T_AdminTestRecordAcceptedBy>>(JsonString);

                    if (adminTestRecordCertificationAcceptedByList != null && adminTestRecordCertificationAcceptedByList.Count > 0)
                    {
                        adminTestRecordCertificationAcceptedByList.ForEach(x => { x.ProjectID = userProject.Project_ID; x.ProjectName = userProject.ProjectName; });
                        await _adminTestRecordAcceptedByRepository.InsertOrReplaceAsync(adminTestRecordCertificationAcceptedByList);
                    }

                    //pre test tempate
                    JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getPreTestAdminCertificationContent(userProject.Project_ID), Settings.AccessToken);
                    List<T_AdminPreTestRecordContent> adminPreTestRecordConfirmationContentList = JsonConvert.DeserializeObject<List<T_AdminPreTestRecordContent>>(JsonString);

                    if (adminPreTestRecordConfirmationContentList != null && adminPreTestRecordConfirmationContentList.Count > 0)
                    {
                        adminPreTestRecordConfirmationContentList.ForEach(x => x.ProjectID = userProject.Project_ID);
                        await _AdminPreTestRecordContentRepository.InsertOrReplaceAsync(adminPreTestRecordConfirmationContentList);
                    }


                    JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getPreTestAdminCertificationAcceptedBy(userProject.Project_ID), Settings.AccessToken);
                    List<T_AdminPreTestRecordAcceptedBy> adminPreTestRecordCertificationAcceptedByList = JsonConvert.DeserializeObject<List<T_AdminPreTestRecordAcceptedBy>>(JsonString);

                    if (adminPreTestRecordCertificationAcceptedByList != null && adminPreTestRecordCertificationAcceptedByList.Count > 0)
                    {
                        adminPreTestRecordCertificationAcceptedByList.ForEach(x => x.ProjectID = userProject.Project_ID);
                        await _AdminPreTestRecordAcceptedByRepository.InsertOrReplaceAsync(adminPreTestRecordCertificationAcceptedByList);
                    }

                    //post test tempate
                    JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getPostTestAdminCertificationContent(userProject.Project_ID), Settings.AccessToken);
                    List<T_AdminPostTestRecordContent> adminPostTestRecordConfirmationContentList = JsonConvert.DeserializeObject<List<T_AdminPostTestRecordContent>>(JsonString);

                    if (adminPostTestRecordConfirmationContentList != null && adminPostTestRecordConfirmationContentList.Count > 0)
                    {
                        adminPostTestRecordConfirmationContentList.ForEach(x => x.ProjectID = userProject.Project_ID);
                        await _AdminPostTestRecordContentRepository.InsertOrReplaceAsync(adminPostTestRecordConfirmationContentList);
                    }


                    JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getPostTestAdminCertificationAcceptedBy(userProject.Project_ID), Settings.AccessToken);
                    List<T_AdminPostTestRecordAcceptedBy> adminPOstTestRecordCertificationAcceptedByList = JsonConvert.DeserializeObject<List<T_AdminPostTestRecordAcceptedBy>>(JsonString);

                    if (adminPOstTestRecordCertificationAcceptedByList != null && adminPOstTestRecordCertificationAcceptedByList.Count > 0)
                    {
                        adminPOstTestRecordCertificationAcceptedByList.ForEach(x => x.ProjectID = userProject.Project_ID);
                        await _AdminPostTestRecordAcceptedByRepository.InsertOrReplaceAsync(adminPOstTestRecordCertificationAcceptedByList);
                    }

                    ////Download End

                }
                else if (Settings.ModuleName == "JobSetting")
                {
                    bool isPDF = false;
                    var UserProjectList = await _userProjectRepository.GetAsync();
                    if (UserProjectList.Count > 0)
                        userProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();

                    //string JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getIWP(userProject.ModelName), Settings.AccessToken);
                    //var JobSettingList = new ObservableCollection<T_IWP>(JsonConvert.DeserializeObject<List<T_IWP>>(JsonString));

                    var exist = await _iwpRepository.QueryAsync<T_IWP>("SELECT [ID] FROM [T_IWP] WHERE  [ID] = '" + CurrentID + "'");

                    //Delete Old Entries if exist
                    await _iwpRepository.QueryAsync<T_IWP>("DELETE FROM [T_IWP] WHERE [ID] = '" + CurrentID + "' AND ProjectID ='" + userProject.Project_ID + "'");
                    await _iwpStatusRepository.QueryAsync<T_IWPStatus>("DELETE FROM [T_IWPStatus] WHERE [IWP_ID] = " + CurrentID);
                    await _predecessorRepository.QueryAsync<T_Predecessor>("DELETE FROM [T_Predecessor] WHERE [IWP_ID] = " + CurrentID);
                    await _successorRepository.QueryAsync<T_Successor>("DELETE FROM [T_Successor] WHERE [IWP_ID] = " + CurrentID);
                    await _iwpAttachmentsRepository.QueryAsync<T_IWPAttachments>("DELETE FROM [T_IWPAttachments] WHERE [LinkedID] = " + CurrentID);
                    await _tagMilestoneStatusRepository.QueryAsync<T_TagMilestoneStatus>(@"DELETE FROM [T_TagMilestoneStatus] WHERE [Project_ID] = '" + userProject.Project_ID
                                                                                                                               + "' AND IWPID ='" + CurrentID + "'");

                    //get persetPunch(new added)
                    string PersetPunchs = ModsTools.WebServiceGet(ApiUrls.Url_getPresetPunches(userProject.Project_ID), Settings.AccessToken);
                    List<T_IwpPresetPunch> PersetPunchsList = JsonConvert.DeserializeObject<List<T_IwpPresetPunch>>(PersetPunchs);
                    if (PersetPunchsList != null && PersetPunchsList.Count > 0)
                    {
                        string DeleteSQL = "DELETE FROM [T_IwpPresetPunch] WHERE ProjectID ='" + userProject.Project_ID + "'";
                        await _IwpPresetPunchRepository.QueryAsync<T_IwpPresetPunch>(DeleteSQL);
                        PersetPunchsList.ForEach(x => x.ProjectID = userProject.Project_ID);
                        await _IwpPresetPunchRepository.InsertOrReplaceAsync(PersetPunchsList);

                    }


                    //get IWP
                    string JsonIWP_IDString = ModsTools.WebServiceGet(ApiUrls.Url_getIWP_ID(userProject.ModelName, CurrentID), Settings.AccessToken);
                    T_IWP IWP_ID = JsonConvert.DeserializeObject<T_IWP>(JsonIWP_IDString);
                    if (IWP_ID != null)
                    {
                        IWP_ID.ProjectID = Settings.ProjectID;
                        isPDF = true;
                        IWP_ID.DownloadDate = Convert.ToDateTime(DateTime.Now.ToString(AppConstant.DateSaveFormat));
                        await _iwpRepository.QueryAsync<T_IWP>("DELETE FROM [T_IWP] WHERE [ID] = '" + IWP_ID.ID + "'");
                        await _iwpRepository.InsertOrReplaceAsync(IWP_ID);
                    }

                    if (IWP_ID.IWPStatusList.Count > 0)
                        await _iwpStatusRepository.InsertOrReplaceAsync(IWP_ID.IWPStatusList);


                    if (IWPHelper.includedPredecessors && IWP_ID.PredecessorList.Count > 0)
                        await _predecessorRepository.InsertOrReplaceAsync(IWP_ID.PredecessorList);



                    if (IWPHelper.includedSuccessors && IWP_ID.SuccessorList.Count > 0)
                        await _successorRepository.InsertOrReplaceAsync(IWP_ID.SuccessorList);


                    //create PDF files
                    if (isPDF)
                    {
                        string IWPFolder = ("PDF Store" + "\\" + userProject.JobCode);
                        byte[] IWPPDFBytes = Convert.FromBase64String(IWP_ID.BinaryCode);
                        await DependencyService.Get<ISaveFiles>().SavePDFToDisk(IWPFolder, IWP_ID.Title + ".pdf", IWPPDFBytes);
                    }

                    //get Drawings
                    if (IWPHelper.includedDrawings)
                    {
                        string JsonResponceString = ModsTools.WebServiceGet(ApiUrls.Url_getIWPDrawings(userProject.Project_ID, CurrentID), Settings.AccessToken);
                        List<VMHub> VMHubList = JsonConvert.DeserializeObject<List<VMHub>>(JsonResponceString);

                        if (VMHubList.Count > 0)
                        {
                            foreach (VMHub CurrentVM in VMHubList)
                            {
                                string Folder = ("PDF Store" + "\\" + userProject.JobCode + "\\" + CurrentID.ToString());
                                byte[] PDFBytes = Convert.FromBase64String(CurrentVM.FileBytes);
                                string getFileLocation = await DependencyService.Get<ISaveFiles>().SavePDFToDisk(Folder, CurrentVM.FileName, PDFBytes);

                                T_IWPDrawings _drawing = new T_IWPDrawings
                                {
                                    ProjectID = userProject.Project_ID,
                                    IWPID = CurrentID,
                                    JobCode = userProject.JobCode,
                                    Name = CurrentVM.DisplayName,
                                    Sheet_No = "",
                                    Total_Sheets = "",
                                    FileName = CurrentVM.FileName,
                                    FileLocation = getFileLocation,
                                    Revision = "",
                                    BinaryCode = CurrentVM.FileBytes,
                                    VMHub_DocumentsID = CurrentVM.ID
                                };
                                await _iwpDrawingsRepository.QueryAsync<T_IWPDrawings>("DELETE FROM [T_IWPDrawings] WHERE [IWPID] = '" + CurrentID + "' AND FileName ='" + _drawing.FileName + "'");
                                await _iwpDrawingsRepository.InsertOrReplaceAsync(_drawing);
                            }
                        }

                        //string JsonResponceString = ModsTools.WebServiceGet(ApiUrls.Url_getIWPDrawingList(userProject.ModelName, CurrentID), Settings.AccessToken);
                        //List<string> IWPNames = new List<string>();
                        //IWPNames = JsonConvert.DeserializeObject<List<string>>(JsonResponceString);                       

                        //foreach (string item in IWPNames)
                        //{
                        //    string JsonIWPDrawingString = ModsTools.WebServiceGet(ApiUrls.Url_getIWPDrawing(item), Settings.AccessToken);
                        //    T_IWPDrawings file = JsonConvert.DeserializeObject<T_IWPDrawings>(JsonIWPDrawingString);
                        //    if (file != null)
                        //    {
                        //        file.ProjectID = userProject.Project_ID;
                        //        file.IWPID = CurrentID;
                        //        file.SpoolDrawingNo = Path.GetFileNameWithoutExtension(file.FileName);
                        //    }
                        //    string IWPFolder = "PDF Store\\" + userProject.JobCode + "\\Drawings\\" + IWP_ID.ID;
                        //    byte[] IWPPDFBytes = Convert.FromBase64String(file.BinaryCode);
                        //    await DependencyService.Get<ISaveFiles>().SavePDFToDisk(IWPFolder, file.FileName, IWPPDFBytes);
                        //    await _iwpDrawingsRepository.QueryAsync<T_IWPDrawings>("DELETE FROM [T_IWPDrawings] WHERE [IWPID] = '" + IWP_ID.ID + "' AND FileName ='"+ file.FileName + "'");
                        //    await _iwpDrawingsRepository.InsertOrReplaceAsync(file);
                        //}
                    }

                    //get Attachaments
                    if (IWPHelper.includedAttachments)
                    {
                        string JsonAttachmentString = ModsTools.WebServiceGet(ApiUrls.Url_getAllIWPAttachments(userProject.Project_ID, CurrentID), Settings.AccessToken);
                        List<T_IWPAttachments> listAttachments = JsonConvert.DeserializeObject<List<T_IWPAttachments>>(JsonAttachmentString);
                        if (listAttachments != null && listAttachments.Count > 0)
                        {
                            listAttachments.ForEach(x => x.ProjectID = userProject.Project_ID);
                            await _iwpAttachmentsRepository.QueryAsync<T_IWPAttachments>("DELETE FROM [T_IWPAttachments] WHERE [LinkedID] = " + IWP_ID.ID);
                            await _iwpAttachmentsRepository.InsertOrReplaceAsync(listAttachments);
                        }
                        foreach (T_IWPAttachments item in listAttachments)
                        {
                            string IWPFolder = "PDF Store\\" + userProject.JobCode + "\\Drawings\\" + CurrentID;
                            byte[] IWPPDFBytes = Convert.FromBase64String(item.FileBytes);
                            await DependencyService.Get<ISaveFiles>().SavePDFToDisk(IWPFolder, item.FileName, IWPPDFBytes);
                        }
                    }

                    //Get ManPowerResources
                    if (userProject != null)
                    {
                        try
                        {
                            await _manPowerResourceRepository.QueryAsync<T_ManPowerResource>("DELETE FROM [T_ManPowerResource]"); //WHERE [ProjectID] = '" + userProject.Project_ID + "'
                            string JsonManPowerResource = ModsTools.WebServiceGet(ApiUrls.Url_getManPowerResource(userProject.Project_ID), Settings.AccessToken);
                            List<T_ManPowerResource> MPRresponse = JsonConvert.DeserializeObject<List<T_ManPowerResource>>(JsonManPowerResource);

                            if (MPRresponse.Count > 0)
                            {
                                await _manPowerResourceRepository.InsertAllAsync(MPRresponse);
                            }
                        }
                        catch
                        {

                        }
                    }

                    //Get ManPowerLogs
                    try
                    {
                        string MPLResponse = ModsTools.WebServiceGet(ApiUrls.Url_getManPowerLogs(userProject.Project_ID, CurrentID), Settings.AccessToken);
                        List<T_ManPowerLog> ManPowerLogs = JsonConvert.DeserializeObject<List<T_ManPowerLog>>(MPLResponse);
                        if (ManPowerLogs.Count > 0)
                        {
                            string deleteSQL = "DELETE FROM [T_ManPowerLog] WHERE [ProjectID] = '" + userProject.Project_ID + "' AND IWPID ='" + CurrentID + "'";
                            await _manPowerLogRepository.QueryAsync<T_ManPowerLog>(deleteSQL);

                            ManPowerLogs.ForEach(async X =>
                            {

                                if (X.EndTime == null)
                                    await _manPowerLogRepository.InsertOrReplaceAsync(X);
                            });

                        }
                    }
                    catch { }
                    // get CWPTags
                    string CWPTagResponse = ModsTools.WebServiceGet(ApiUrls.Url_getCWPTags(IWP_ID.ID), Settings.AccessToken);
                    Dictionary<string, string> TMSValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(CWPTagResponse);
                    if (TMSValues.Any())
                    {
                        string deleteSQLforTAgs = "DELETE FROM [T_CwpTag] WHERE [ProjectID] = '" + userProject.Project_ID + "' AND IWPID ='" + CurrentID + "'";
                        await _CwpTAg.QueryAsync<T_CwpTag>(deleteSQLforTAgs);

                        foreach (KeyValuePair<string, string> tagvalue in TMSValues)
                        {
                            await _CwpTAg.InsertOrReplaceAsync(new T_CwpTag { ID = tagvalue.Key, TagNo = tagvalue.Value, IWPID = CurrentID, ProjectID = userProject.Project_ID });
                        }


                    }




                    if (TMSValues.Any())
                    {
                        var tagsArray = TMSValues.Values.ToArray();
                        var jsonTagsString = JsonConvert.SerializeObject(tagsArray);
                        if (IWPHelper.includedCWPTags)
                        {
                            //Get TagMilestoneStatus
                            string TMSResponse = ModsTools.WebServicePost(ApiUrls.Url_getTagMilestoneStatusesWithTagMember(userProject.Project_ID), jsonTagsString, Settings.AccessToken);
                            List<TagMilestoneStatusModel> TagMilestoneStatus = JsonConvert.DeserializeObject<List<TagMilestoneStatusModel>>(TMSResponse);
                            if (TagMilestoneStatus.Count > 0)
                            {
                                List<T_TagMilestoneStatus> TagStatuses = new List<T_TagMilestoneStatus>();
                                TagStatuses = TagMilestoneStatus.Select(s => new T_TagMilestoneStatus
                                {
                                    Project_ID = userProject.Project_ID,
                                    IWPID = IWP_ID.ID,
                                    CWPTagID = Convert.ToInt32(TMSValues.Where(x => x.Value.ToLower() == s.TagNo.ToLower()).FirstOrDefault().Key),
                                    TagNo = s.TagNo,
                                    Milestone = s.Milestone,
                                    MilestoneAttribute = s.MilestoneAttribute,
                                    // SignedByUserID = s.UpdatedByUserID,
                                    UpdatedByUserID = s.UpdatedByUserID,
                                    // SignedBy = s.SignedBy,
                                    SignedInCMS = s.SignedInCMS,
                                    SignedInVM = s.SignedInVM,
                                    StatusValue = s.StatusValue,
                                    TagMember = s.TagMember

                                }).ToList();

                                //string DeleteSQL = "DELETE FROM [T_TagMilestoneStatus] WHERE [Project_ID] = '" + userProject.Project_ID + "' AND CWPTagID ='" + kvp.Key + "'";
                                _ = _tagMilestoneStatusRepository.DeleteAll(TagStatuses);
                                //await _tagMilestoneStatusRepository.QueryAsync<T_TagMilestoneStatus>(DeleteSQL);
                                _ = _tagMilestoneStatusRepository.InsertOrReplaceAsync(TagStatuses);
                            }
                        }
                    }

                    foreach (KeyValuePair<string, string> kvp in TMSValues)
                    {
                        //if (IWPHelper.includedCWPTags)
                        //{
                        //    //Get TagMilestoneStatus
                        //    string TMSResponse = ModsTools.WebServiceGet(ApiUrls.Url_getTagMilestoneStatusesWithTagMember(userProject.Project_ID, kvp.), Settings.AccessToken);
                        //    List<TagMilestoneStatusModel> TagMilestoneStatus = JsonConvert.DeserializeObject<List<TagMilestoneStatusModel>>(TMSResponse);
                        //    if (TagMilestoneStatus.Count > 0)
                        //    {
                        //        List<T_TagMilestoneStatus> TagStatuses = new List<T_TagMilestoneStatus>();
                        //        TagStatuses = TagMilestoneStatus.Select(s => new T_TagMilestoneStatus
                        //        {
                        //            Project_ID = userProject.Project_ID,
                        //            IWPID = IWP_ID.ID,
                        //            CWPTagID = s.CWPTagID,
                        //            TagNo = s.TagNo,
                        //            Milestone = s.Milestone,
                        //            MilestoneAttribute = s.MilestoneAttribute,
                        //            SignedByUserID = s.SignedByUserID,
                        //            SignedBy = s.SignedBy,
                        //            StatusValue = s.StatusValue,
                        //            TagMember =s.TagMember

                        //        }).ToList();

                        //        string DeleteSQL = "DELETE FROM [T_TagMilestoneStatus] WHERE [Project_ID] = '" + userProject.Project_ID + "' AND CWPTagID ='" + kvp.Key + "'";
                        //        await _tagMilestoneStatusRepository.QueryAsync<T_TagMilestoneStatus>(DeleteSQL);
                        //        await _tagMilestoneStatusRepository.InsertOrReplaceAsync(TagStatuses);
                        //    }

                        ////Get TagMilestoneImages
                        //string CWPImages = ModsTools.WebServiceGet(ApiUrls.Url_getCWPMilestoneImages(Convert.ToInt32(kvp.Key)), Settings.AccessToken);
                        //List<T_TagMilestoneImages> CWPMilestoneImages = JsonConvert.DeserializeObject<List<T_TagMilestoneImages>>(CWPImages);
                        //if (CWPMilestoneImages.Count > 0)
                        //{
                        //    CWPMilestoneImages.ForEach(x => x.Project_ID = userProject.Project_ID);
                        //   string DeleteSQLforimages = "DELETE FROM [T_TagMilestoneImages] WHERE [Project_ID] = '" + userProject.Project_ID + "' AND CWPID ='" + kvp.Key + "'";
                        //   await _tagMilestoneImagesRepository.QueryAsync<T_TagMilestoneImages>(DeleteSQLforimages);
                        //    await _tagMilestoneImagesRepository.InsertOrReplaceAsync(CWPMilestoneImages);
                        //}
                        //}

                        //CWP punch control Drawings
                        string CWPDrawingsResponse = ModsTools.WebServiceGet(ApiUrls.Url_getCWPDrawings(Convert.ToInt32(kvp.Key)), Settings.AccessToken);
                        List<T_CWPDrawings> CWPDrawings = JsonConvert.DeserializeObject<List<T_CWPDrawings>>(CWPDrawingsResponse);
                        if (CWPDrawings.Count > 0)
                        {
                            CWPDrawings.ForEach(x => { x.Project_ID = userProject.Project_ID; x.IWPID = IWP_ID.ID; });
                            string DeleteSQL = "DELETE FROM [T_CWPDrawings] WHERE [Project_ID] = '" + userProject.Project_ID + "' AND CWPID ='" + kvp.Key + "' AND IWPID = '" + IWP_ID.ID + "'";
                            await _cwpDrawingsRepository.QueryAsync<T_CWPDrawings>(DeleteSQL);
                            await _cwpDrawingsRepository.InsertOrReplaceAsync(CWPDrawings);
                        }
                    }
                    //Get GetAdminControlLog
                    string IWPACLResponse = ModsTools.WebServiceGet(ApiUrls.Url_getIWPAdminControlLog(userProject.Project_ID), Settings.AccessToken);
                    List<T_IWPAdminControlLog> IWPACL = JsonConvert.DeserializeObject<List<T_IWPAdminControlLog>>(IWPACLResponse);
                    if (IWPACL.Count > 0)
                    {
                        string DeleteSQL = "DELETE FROM [T_IWPAdminControlLog] WHERE [ProjectID] = '" + userProject.Project_ID + "'";
                        await _iwpAdminControlLogRepository.QueryAsync<T_IWPAdminControlLog>(DeleteSQL);
                        IWPACL.ForEach(x => x.ProjectID = userProject.Project_ID);
                        await _iwpAdminControlLogRepository.InsertOrReplaceAsync(IWPACL);

                        foreach (T_IWPAdminControlLog List in IWPACL)
                        {
                            // Remove previously downloaded data before insert new downloaded
                            string DeletePC = "DELETE FROM [T_IWPPunchCategory] WHERE [ProjectID] = '" + userProject.Project_ID + "' AND [AdminControlLog_ID] = '" + List.ID + "'";
                            await _iwpPunchCategoryRepository.QueryAsync<T_IWPPunchCategory>(DeletePC);
                            string DeletePL = "DELETE FROM [T_IWPPunchLayer] WHERE [ProjectID] = '" + userProject.Project_ID + "' AND [AdminControlLog_ID] = '" + List.ID + "'";
                            await _iwpPunchLayerRepository.QueryAsync<T_IWPPunchLayer>(DeletePL);

                            foreach (string PCitem in List.PunchCategory)
                            {
                                // PunchCategory 
                                await _iwpPunchCategoryRepository.InsertOrReplaceAsync(new T_IWPPunchCategory { ProjectID = userProject.Project_ID, AdminControlLog_ID = List.ID, PunchCategory = PCitem });
                            }
                            foreach (string PLitem in List.PunchLayer)
                            {
                                // PunchLayer 
                                await _iwpPunchLayerRepository.InsertOrReplaceAsync(new T_IWPPunchLayer { ProjectID = userProject.Project_ID, AdminControlLog_ID = List.ID, PunchLayer = PLitem });
                            }
                        }
                    }

                    //Get ControlLogSignatures
                    string IWPCLSResponse = ModsTools.WebServiceGet(ApiUrls.Url_getIWPControlLogSignatures(IWP_ID.ID), Settings.AccessToken);
                    List<T_IWPControlLogSignatures> IWPCLS = JsonConvert.DeserializeObject<List<T_IWPControlLogSignatures>>(IWPCLSResponse);
                    if (IWPCLS.Count > 0)
                    {
                        string DeleteSQL = "DELETE FROM [T_IWPControlLogSignatures] WHERE [IWP_ID] = '" + IWP_ID.ID + "'";
                        await _iwpControlLogSignaturesRepository.QueryAsync<T_IWPControlLogSignatures>(DeleteSQL);
                        IWPCLS.ForEach(x => { x.IWP_ID = IWP_ID.ID; x.ProjectID = userProject.Project_ID; });
                        await _iwpControlLogSignaturesRepository.InsertOrReplaceAsync(IWPCLS);
                    }

                    //Get PunchItems
                    string IWPPunchItemResponse = ModsTools.WebServiceGet(ApiUrls.Url_getIWPPunchItems(IWP_ID.ID), Settings.AccessToken);

                    string PunchControlItemDeleteSQL = "DELETE FROM [T_IWPPunchControlItem] WHERE [IWPID] = '" + IWP_ID.ID + "'";
                    await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(PunchControlItemDeleteSQL);

                    if (IWPPunchItemResponse != "null")
                    {
                        List<T_IWPPunchControlItem> IWPPCI = JsonConvert.DeserializeObject<List<T_IWPPunchControlItem>>(IWPPunchItemResponse);
                        if (IWPPCI.Count > 0)
                        {
                            IWPPCI.ForEach(x => x.ProjectID = userProject.Project_ID);
                            await _iwpPunchControlItemRepository.InsertOrReplaceAsync(IWPPCI);
                        }

                        //foreach(int PunchID in IWPPCI.Select(i=>i.ID))
                        //{
                        //    // Get Punch Images 
                        //    string IWPPunchImagesResponse = ModsTools.WebServiceGet(ApiUrls.Url_getIWPPunchImages(PunchID), Settings.AccessToken);
                        //    if (IWPPunchImagesResponse != "null")
                        //    {
                        //        List<T_IWPPunchImage> PunchImage = JsonConvert.DeserializeObject<List<T_IWPPunchImage>>(IWPPunchImagesResponse);
                        //        if (PunchImage.Count > 0)
                        //        {
                        //            PunchImage.ForEach(x => { x.IWPID = IWP_ID.ID; x.ProjectID = userProject.Project_ID;x.IsUploaded = true; });
                        //            string DeleteSQL = "DELETE FROM [T_IWPPunchImage] WHERE [IWPID] = '" + IWP_ID.ID + "' AND LinkedID ='" + PunchID + "' AND ProjectID ='" + userProject.Project_ID + "'";
                        //            await _iwpPunchImagesRepository.QueryAsync<T_IWPPunchImage>(DeleteSQL);
                        //            await _iwpPunchImagesRepository.InsertOrReplaceAsync(PunchImage);
                        //        }
                        //    }
                        //}
                    }

                    //Get AdminPunchLayer
                    string IWPAdminPunchLayer = ModsTools.WebServiceGet(ApiUrls.Url_getIWPAdminPunchLayer(userProject.Project_ID), Settings.AccessToken);
                    List<T_IWPAdminPunchLayer> PunchLayers = JsonConvert.DeserializeObject<List<T_IWPAdminPunchLayer>>(IWPAdminPunchLayer);
                    if (PunchLayers != null && PunchLayers.Count > 0)
                    {
                        string DeleteSQL = "DELETE FROM [T_IWPAdminPunchLayer] WHERE [ProjectID] = '" + userProject.Project_ID + "'";
                        await _iWPAdminPunchLayerRepository.QueryAsync<T_IWPAdminPunchLayer>(DeleteSQL);
                        PunchLayers.ForEach(x => x.ProjectID = userProject.Project_ID);
                        await _iWPAdminPunchLayerRepository.InsertOrReplaceAsync(PunchLayers);
                    }

                    ////get persetPunch(new added)
                    //string PersetPunchs = ModsTools.WebServiceGet(ApiUrls.Url_getPresetPunches(userProject.Project_ID), Settings.AccessToken);
                    //List<T_IwpPresetPunch> PersetPunchsList = JsonConvert.DeserializeObject<List<T_IwpPresetPunch>>(PersetPunchs);
                    //if (PersetPunchsList != null && PersetPunchsList.Count > 0)
                    //{
                    //    string DeleteSQL = "DELETE FROM [T_IwpPresetPunch]";
                    //    await _IwpPresetPunchRepository.QueryAsync<T_IwpPresetPunch>(DeleteSQL);

                    //    await _IwpPresetPunchRepository.InsertOrReplaceAsync(PersetPunchsList);

                    //}
                    //Get PunchCategories
                    string IWPPunchCategoriesResponse = ModsTools.WebServiceGet(ApiUrls.Url_getIWPPunchCategories(userProject.Project_ID), Settings.AccessToken);
                    List<T_IWPPunchCategories> PunchCategories = JsonConvert.DeserializeObject<List<T_IWPPunchCategories>>(IWPPunchCategoriesResponse);
                    if (PunchCategories.Count > 0)
                    {
                        string DeleteSQL = "DELETE FROM [T_IWPPunchCategories] WHERE [ProjectID] = '" + userProject.Project_ID + "'";
                        await _iWPPunchCategoriesRepository.QueryAsync<T_IWPPunchCategories>(DeleteSQL);
                        PunchCategories.ForEach(x => x.ProjectID = userProject.Project_ID);
                        await _iWPPunchCategoriesRepository.InsertOrReplaceAsync(PunchCategories);
                    }

                    // Get FunctionCodes
                    string IWPFunctionCodesResponse = ModsTools.WebServiceGet(ApiUrls.Url_getIWPFunctionCodes(userProject.Project_ID), Settings.AccessToken);
                    List<string> FunctionCodesList = JsonConvert.DeserializeObject<List<string>>(IWPFunctionCodesResponse);
                    if (FunctionCodesList.Count > 0 && FunctionCodesList != null)
                    {
                        List<T_IWPFunctionCodes> FunctionCodes = new List<T_IWPFunctionCodes>();
                        string DeleteSQL = "DELETE FROM [T_IWPFunctionCodes] WHERE [ProjectID] = '" + userProject.Project_ID + "'";
                        await _iWPFunctionCodesRepository.QueryAsync<T_IWPFunctionCodes>(DeleteSQL);

                        foreach (string item in FunctionCodesList)
                        {
                            T_IWPFunctionCodes afc = new T_IWPFunctionCodes()
                            {
                                ProjectID = userProject.Project_ID,
                                FunctionCode = item.Split(new string[] { " - " }, 2, StringSplitOptions.None)[0].Trim(),
                                Description = item.Split(new string[] { " - " }, 2, StringSplitOptions.None)[1].Trim()
                            };
                            FunctionCodes.Add(afc);
                        }
                        await _iWPFunctionCodesRepository.InsertOrReplaceAsync(FunctionCodes);
                    }

                    // Get CompanyCategoryCodes
                    string CompanyCategoryCodes = ModsTools.WebServiceGet(ApiUrls.Url_getIWPCompanyCategoryCodes(userProject.Project_ID), Settings.AccessToken);
                    Dictionary<string, string> CompanyCategoryCodesValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(CompanyCategoryCodes);
                    string DeleteCCCSQL = "DELETE FROM [T_IWPCompanyCategoryCodes] WHERE [ProjectID] = '" + userProject.Project_ID + "'";
                    await _iWPCompanyCategoryCodesRepository.QueryAsync<T_IWPCompanyCategoryCodes>(DeleteCCCSQL);

                    foreach (KeyValuePair<string, string> kvp in CompanyCategoryCodesValues)
                    {
                        T_IWPCompanyCategoryCodes CCC = new T_IWPCompanyCategoryCodes
                        {
                            ProjectID = userProject.Project_ID,
                            CompanyCategoryCode = kvp.Key,
                            Description = kvp.Value,
                        };
                        await _iWPCompanyCategoryCodesRepository.InsertOrReplaceAsync(CCC);
                    }
                }
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}

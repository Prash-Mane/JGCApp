using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Common.Constants
{
    public static class ApiUrls
    {
        //Common
        public static string Url_token(string TimeStamp) => $"Token?TimeStamp={TimeStamp}";
        public static string Url_login(string UserName, string Password) => $"User?UserName={UserName}&Password={Password}";
        public static string Url_GetAllUserAccounts() => $"User/GetAllUserAccounts";
        public const string Url_connectionTest = "ConnectionTest";

        #region E-Reporter API's
        //Get
        public static string Url_getEReportHeaders(string ModelName, string ReportType, long UserID, string UnitID) 
              => $"EReporter/GetEReportHeaders?ModelName={ModelName}&ReportType={ReportType}&UserID={UserID}&UnitID={UnitID}";
        public static string Url_getEReport(long ID, string UnitID) => $"EReporter/GetEReport?ID={ID}&UnitID={UnitID}";
        public static string Url_getRTDefects(long ProjectID) => $"EReports/DWR/GetRTDefects?ProjectID={ProjectID}";
        public static string Url_getWelders(long ProjectID, string JobCode, string SubContractor) 
              => $"EReports/DWR/GetWelders?ProjectID={ProjectID}&JobCode={JobCode}&SubContractor={SubContractor}";
        public static string Url_getHeatNumbers(long ProjectID, string JobCode) => $"EReports/DWR/GetHeatNumbers?ProjectID={ProjectID}&JobCode={JobCode}";
       
        public static string Url_getWPSNos(long ProjectID, string JobCode) => $"EReports/DWR/GetWPSNos?ProjectID={ProjectID}&JobCode={JobCode}";
        public static string Url_getBaseMetals(long ProjectID) => $"EReports/DWR/GetBaseMetals?ProjectID={ProjectID}";
        public static string Url_getStorageAreas(long ProjectID, string JobCode, string StoreLocation) 
             => $"EReports/MRR/GetStorageAreas?ProjectID={ProjectID}&JobCode={JobCode}&StoreLocation={StoreLocation}";
      //  public static string Url_getDocuments(string Module, long LinkedID) => $"VMHub/GetDocuments?Module={Module}&LinkedID={LinkedID}"; 
        public static string Url_getDocuments(string Module, long LinkedID) => $"VMHub/GetAllAttachments?Module={Module}&LinkedID={LinkedID}";
        public static string Url_getRequestPartial(long ID, long UserID) => $"EReporter/RequestPartial?ID={ID}&UserID={UserID}";

        //https://jgctest.vmlive.net/WebService/api/VMHub/GetImages?Module=EREPORTER&LinkedID=2364

        public static string Url_getImages(string Module, long LinkedID) => $"VMHub/GetImages?Module={Module}&LinkedID={LinkedID}";

        //Post
        public static string Url_postDrawingList(bool IncludePDF) => $"EReporter/PostDrawingList?IncludePDF={IncludePDF}";
        public static string Url_postAllHeatNos(long ProjectID, string JobCode) => $"EReports/CMR/GetAllHeatNos?ProjectID={ProjectID}&JobCode={JobCode}";
        public static string Url_postEReport(string UnitID) => $"EReporter/PostEReport?UnitID={UnitID}";
        public static string Url_postAllStorageAreas(long ProjectID, string JobCode) => $"EReports/CMR/GetAllStorageAreas?ProjectID={ProjectID}&JobCode={JobCode}";
        public static string Url_postVMHub = $"VMHub";
        public static string Url_UnlockEReport(long ID) => $"EReporter/UnlockEReport?ID={ID}";


        // new DWR APIs
        public static string Url_GetDWR(long ProjectID, string JobCode, string Spool, string Joint) => $"EReports/DWR/GetDWR?ProjectID={ProjectID}&JobCode={JobCode}&Spool={Spool}&Joint={Joint}";
        public static string Url_GetWeldProcesses(long ProjectID) => $"EReports/DWR/GetWeldProcesses?ProjectID={ProjectID}";
        public static string Url_GetCheckEReportComplete(long ID) => $"EReporter/CheckEReportComplete?ID={ID}";
        public static string Url_GetSpools(long ProjectID, string JobCode, string Search) => $"EReports/DWR/GetSpools?ProjectID={ProjectID}&JobCode={JobCode}&Search={Search}";
        public static string Url_GetSpoolsWithSubContractor(long ProjectID, string JobCode, string SubContractor, string Search) => $"EReports/DWR/GetSpools?ProjectID={ProjectID}&JobCode={JobCode}&SubContractor={SubContractor}&Search={Search}";
        public static string Url_GetJoints(long ProjectID, string JobCode, string Spool) => $"EReports/DWR/GetJoints?ProjectID={ProjectID}&JobCode={JobCode}&Spool={Spool}";
        public static string Url_GetJointsWithSubContractor(long ProjectID, string JobCode, string SubContractor, string Spool) => $"EReports/DWR/GetJoints?ProjectID={ProjectID}&JobCode={JobCode}&SubContractor={SubContractor}&Spool={Spool}";
        public static string Url_GetDWRID(long ProjectID, string TestPackage, string SpoolDrawingNo, string JointNo) => $"EReporter/GetDWRID?ProjectID={ProjectID}&TestPackage={TestPackage}&SpoolDrawingNo={SpoolDrawingNo}&JointNo={JointNo}";
        #endregion

        #region E-Test Package API's
        //Get  
        public static string Url_getETestPackageHeaders_UserID(string ModelName, long UserID)=> $"ETestPackage/GetETestPackageHeaders?ModelName={ModelName}&UserID={UserID}";
        public static string Url_getETestPackageHeaders(string ModelName) => $"ETestPackage/GetETestPackageHeaders?ModelName={ModelName}";
        public static string Url_getFolders(long ProjectID) => $"ETestPackage/GetFolders?ProjectID={ProjectID}";
        public static string Url_getAdminControlLog(long ProjectID) => $"ETestPackage/GetAdminControlLog?ProjectID={ProjectID}";
        public static string Url_getAdminControlLogFolders(long ProjectID) => $"ETestPackage/GetAdminControlLogFolders?ProjectID={ProjectID}";
        public static string Url_getPresetPunches(long ProjectID, string JobCode)=> $"ETestPackage/GetPresetPunches?ProjectID={ProjectID}&JobCode={JobCode}";
        public static string Url_getPunchCategories(long ProjectID, string JobCode)  => $"ETestPackage/GetPunchCategories?ProjectID={ProjectID}&JobCode={JobCode}";
        public static string Url_getFunctionCodes(long ProjectID) => $"ETestPackage/GetFunctionCodes?ProjectID={ProjectID}";
        public static string Url_getAdminPunchLayer(long ProjectID) => $"ETestPackage/GetAdminPunchLayer?ProjectID={ProjectID}";
        public static string Url_getDrainRecordAdminCertificationContent(long ProjectID) => $"ETestPackage/GetDrainRecordAdminCertificationContent?ProjectID={ProjectID}";
        public static string Url_getDrainRecordAdminCertificationAcceptedBy(long ProjectID) => $"ETestPackage/GetDrainRecordAdminCertificationAcceptedBy?ProjectID={ProjectID}";
        public static string Url_getTestRecordAdminCertificationDetails(long ProjectID) => $"ETestPackage/GetTestRecordAdminCertificationDetails?ProjectID={ProjectID}";
        public static string Url_getTestRecordAdminCertificationContent(long ProjectID) => $"ETestPackage/GetTestRecordAdminCertificationContent?ProjectID={ProjectID}";
        public static string Url_getTestRecordAdminCertificationAcceptedBy(long ProjectID) => $"ETestPackage/GetTestRecordAdminCertificationAcceptedBy?ProjectID={ProjectID}";
        public static string Url_getPunchImages(long ID)=> $"ETestPackage/GetPunchImages?ID={ID}";
        public static string Url_getPunchItems(long ID) => $"ETestPackage/GetPunchItems?ID={ID}";
        public static string Url_getControlLogSignatures(long ID) => $"ETestPackage/GetControlLogSignatures?ID={ID}";
        public static string Url_getDrainRecordContent(long ID)=> $"ETestPackage/GetDrainRecordContent?ID={ID}";
        public static string Url_getDrainRecordAcceptedBy(long ID) => $"ETestPackage/GetDrainRecordAcceptedBy?ID={ID}";
        public static string Url_getTestRecordDetails(long ID) => $"ETestPackage/GetTestRecordDetails?ID={ID}";
        public static string Url_getTestRecordConfirmation(long ID) => $"ETestPackage/GetTestRecordConfirmation?ID={ID}";
        public static string Url_getTestRecordAcceptedBy(long ID) => $"ETestPackage/GetTestRecordAcceptedBy?ID={ID}";
        public static string Url_getAttachedDocuments(long ID)=> $"ETestPackage/GetAttachedDocuments?ID={ID}";
        public static string Url_getTestLimitDrawings(long ID) => $"ETestPackage/GetTestLimitDrawings?ID={ID}";
        //new calls for pre and post test 
        public static string Url_getPreTestRecordContent(long ID) => $"ETestPackage/GetPreTestRecordConfirmation?ID={ID}";
        public static string Url_getPretTestRecordAcceptedBy(long ID) => $"ETestPackage/GetPreTestRecordAcceptedBy?ID={ID}";
        public static string Url_getPostTestRecordContent(long ID) => $"ETestPackage/GetPostTestRecordConfirmation?ID={ID}";
        public static string Url_getPostTestRecordAcceptedBy(long ID) => $"ETestPackage/GetPostTestRecordAcceptedBy?ID={ID}";
        public static string Url_getPreTestAdminCertificationContent(long ProjectID) => $"ETestPackage/GetPreTestRecordAdminCertificationContent?ProjectID={ProjectID}";
        public static string Url_getPreTestAdminCertificationAcceptedBy(long ProjectID) => $"ETestPackage/GetPreTestRecordAdminCertificationAcceptedBy?ProjectID={ProjectID}";
        public static string Url_getPostTestAdminCertificationContent(long ProjectID) => $"ETestPackage/GetPostTestRecordAdminCertificationContent?ProjectID={ProjectID}";
        public static string Url_getPostTestAdminCertificationAcceptedBy(long ProjectID) => $"ETestPackage/GetPostTestRecordAdminCertificationAcceptedBy?ProjectID={ProjectID}";



        //Post
        public static string Url_postNewPunch(long ID) => $"ETestPackage/PostNewPunch?ID={ID}";
        public static string Url_postTestRecordImage(long ID) => $"ETestPackage/PostTestRecordImage?ID={ID}";
        public static string Url_postTestRecordRemarks(long ID) => $"ETestPackage/PostTestRecordRemarks?ID={ID}";
        public static string Url_postRemoveDownloadFlag(long ID,long UserID) => $"ETestPackage/RemoveDownloadFlag?ID={ID}&UserID={UserID}";

        #endregion

        #region Job Setting API's

        //Get

        public static string Url_getIWP(string ModelName) => $"IWP?ModelName={ModelName}";
        public static string Url_getIWP_ID(string ModelName, long ID) => $"IWP?ModelName={ModelName}&ID={ID}";
        public static string Url_getIWPDrawingList(string ModelName, long ID) => $"IWP/GetIWPDrawingList?ModelName={ModelName}&ID={ID}";
        public static string Url_getIWPDrawing(string DrawingName) => $"IWP/GetIWPDrawing?DrawingName={DrawingName}";
        //New
        //https://jgctest.vmlive.net/WebService/api/WorkPack/GetIWPDrawings?ProjectID=3&ID=3372
        public static string Url_getIWPDrawings(long ProjectID, long ID) => $"WorkPack/GetIWPDrawings?ProjectID={ProjectID}&ID={ID}";
        public static string Url_getCWPDrawings(long ID) => $"WorkPack/GetCWPDrawings?ID={ID}";
        //public static string Url_getAllAttachments(long LinkedID) => $"VMHub/GetAllAttachments?Module=JOB&LinkedID={LinkedID}";
        //new  WorkPack/GetAllIWPAttachments?ProjectID=3&ID=5121
        public static string Url_getAllIWPAttachments(long ProjectID, long ID) => $"WorkPack/GetAllIWPAttachments?ProjectID={ProjectID}&ID={ID}";
        public static string Url_getManPowerResource(long ProjectID) => $"WorkPack/GetManPowerResource?ProjectID={ProjectID}";
        public static string Url_getManPowerLogs(long ProjectID,long IWPID) => $"WorkPack/GetManPowerLogs?ProjectID={ProjectID}&IWPID={IWPID}";
        public static string Url_getCWPTags(long IWPID) => $"WorkPack/GetCWPTags?IWPID={IWPID}";
        public static string Url_getTagMilestoneStatuses(long ProjectID, long CWPTagID) => $"WorkPack/GetTagMilestoneStatuses?ProjectID={ProjectID}&CWPTagID={CWPTagID}";
        public static string Url_getCWPMilestoneImages(long ID) => $"WorkPack/GetCWPMilestoneImages?ID={ID}";
        public static string Url_getIWPAdminControlLog(long ProjectID) => $"WorkPack/GetAdminControlLog?ProjectID={ProjectID}";
        public static string Url_getIWPControlLogSignatures(long ID) => $"WorkPack/GetControlLogSignatures?ID={ID}";
        public static string Url_getIWPPunchItems(long IWPID) => $"WorkPack/GetPunchItems?IWPID={IWPID}";
        public static string Url_getIWPPunchImages(long PunchID) => $"WorkPack/GetPunchImages?PunchID={PunchID}";
        public static string Url_getIWPPunchCategories(long ProjectID) => $"WorkPack/GetPunchCategories?ProjectID={ProjectID}";
        public static string Url_getIWPFunctionCodes(long ProjectID) => $"WorkPack/GetFunctionCodes?ProjectID={ProjectID}";
        public static string Url_getIWPCompanyCategoryCodes(long ProjectID) => $"WorkPack/GetCompanyCategoryCodes?ProjectID={ProjectID}";
        public static string Url_getIWPAdminPunchLayer(long ProjectID) => $"WorkPack/GetAdminPunchLayer?ProjectID={ProjectID}";

        //new added for preset punch
        public static string Url_getPresetPunches(long ProjectID) => $"WorkPack/GetPresetPunches?ProjectID={ProjectID}";
       // WorkPack/GetPresetPunches
        //new added by amol
        //post
        public static string Url_getTagMilestoneStatusesWithTagMember(long ProjectID) => $"WorkPack/GetTagMilestoneStatus?ProjectID={ProjectID}";

        


        //Post
        //https://jgctest.vmlive.net/webservice/api/WorkPack/PostManPowerLogs?ProjectID=3&UserID=5
        public static string Url_postManPowerLogs(long ProjectID,long UserID) => $"WorkPack/PostManPowerLogs?ProjectID={ProjectID}&UserID={UserID}";

        //https://jgctest.vmlive.net/webservice/api/WorkPack/PostTagMilestoneStatus?ProjectID=1&UserID=1349
        public static string Url_postTagMilestoneStatus(long ProjectID) => $"WorkPack/PostTagStatus?ProjectID={ProjectID}";

        //https://jgctest.vmlive.net/WebService/API/WorkPack/PostControlLogSignature?ProjectID=3&IWPID=3371
        public static string Url_postControlLogSignature(long ProjectID, long IWPID) => $"WorkPack/PostControlLogSignature?ProjectID={ProjectID}&IWPID={IWPID}";

        //https://jgctest.vmlive.net/WebService/api/WorkPack/PostPunchImage
        public static string Url_postPunchImage => $"WorkPack/PostPunchImage";

        // https://jgctest.vmlive.net/WebService/api/WorkPack/PostPunchItem
        public static string Url_postPunchItem => $"WorkPack/PostPunchItem";

        //https://jgctest.vmlive.net/WebService/API/WorkPack/PostCWPMilestoneImage
        public static string Url_postCWPMilestoneImage => $"WorkPack/PostCWPMilestoneImage";

        #endregion

        #region Completions API's
        //login  Rest Api's 

        public static string GetToken(string password, string db) => $"token?timestamp={password}&db={db}";
        public static string GetUser(string username, string password, string db) => $"user?username={username}&password={password}&db={db}";
        public static string GetAllUsersForProject(string project, string db) => $"user?project={project}&db={db}";
        public static string GetUserProjects(string userID, string includeYards, string db) => $"userproject?id={userID}&includeYards={includeYards}&db={db}";

        public static string getSystems(string projectName, string db) => $"system?model={projectName}&db={db}";
        public static string getPunchListMetaData(string projectName, string db) => $"punchlistfields?projectname={projectName}&db={db}";
        public static string getDrawingsForProject(string projectName, string db) => $"drawings?projectname={projectName}&db={db}";
        public static string getPunchLists(string projectName, string db) => $"punchlist?projectName={projectName}&db={db}";
        public static string getPunchLists(string projectName, string db, string workpack) => $"punchlist?projectName={projectName}&db={db}&workpack={workpack}";

        // new sync
        public static string getTags(string model, string project, string columnName, string value, string additional, string db) => $"systemtags?model={model}&project={project}&columnName={columnName}&value={value}&additional={additional}&db={db}";
        public static string getTagHeaders(string model, string project, string db, string tagName, string additional) => $"systemtags?model={model}&project={project}&db={db}&tagName={tagName}&additional={additional}";
        public static string getSheetQuestions(string project, string db, string sheetName) => $"systemtags?project={project}&db={db}&sheetName={sheetName}";
        public static string getTagAnswers(string model, string project, string system, int page, int pageSize, string db) => $"taganswers?model={model}&project={project}&system={system}&page={page}&pageSize={pageSize}&db={db}";
        public static string getTagAnswersByWorkpack(string model, string project, string columnName, string value, string additional, string db) => $"taganswers?model={model}&project={project}&columnName={columnName}&value={value}&additional={additional}&db={db}";
        public static string getCheckSheetAnswers(string model, string project, string tagName, string sheetName, string db) => $"taganswers?model={model}&project={project}&tagName={tagName}&sheetName={sheetName}&db={db}";
        public static string getHandoverSystems(string model, string project, string db) => $"handover?model={model}&project={project}&db={db}";
        public static string getHandovers(string model, string project, string db, string system) => $"handover?model={model}&project={project}&db={db}&system={system}";
        public static string getTestPacks(string model, string project, string db) => $"testpack?model={model}&project={project}&db={db}";
        public static string getTestPack(string model, string project, string testPackName, string db) => $"testpack?model={model}&project={project}&testPackName={testPackName}&db={db}";


        //post
        public static string postPunchListItem(string projectname, string db) => $"punchlist?projectname={projectname}&db={db}";
        public static string postVMHub(string projectname, string db) => $"vmhub?projectname={projectname}&db={db}";
        public static string postAnswers(string projectname, string db) => $"vmhub?cctrAnswer={projectname}&modelname={projectname}&db={db}";
        public static string Post_GetTagsByID(string model, string projectname, string db) => $"GetTagsByID?model={model}&project={projectname}&db={db}";
        public static string Post_GetTagsBySystem(string model, string projectname, string db) => $"GetTagsBySystem?model={model}&project={projectname}&db={db}";
        public static string Post_GetTagsByColumn(string model, string projectname, string db, string column) => $"GetTagsByColumn?model={model}&project={projectname}&db={db}&column={column}";

        //jobs setting api's
        public static string getWorkPackList(string model, string system, bool isJIRequest, string db) => $"workpack?model={model}&system={system}&jirequest={isJIRequest}&db={db}";
        public static string getInitialDataList(string model, string system, bool isJIRequest, string db) => $"GetInitialData?model={model}&system={system}&jirequest={isJIRequest}&db={db}";
        public static string getJobPackList(string model, string db) => $"workpack?model={model}&db={db}";

        //New Completion API's
        public static string GetTagAndIDByColumn(string model, string project, string columnName, string value, string additional, string db) => $"getTagAndIDByColumn?model={model}&project={project}&columnName={columnName}&value={value}&additional={additional}&db={db}";
        public static string Post_GetTagNosByColumn(string model, string projectname, string db, string column) => $"GetTagNosByColumn?model={model}&project={projectname}&db={db}&column={column}";
        public static string Post_GetTagsAndItrsByID(string model, string projectname, string db) => $"GetTagsAndItrsByID?model={model}&project={projectname}&db={db}";
        public static string getPunchDropdownData(string model, string JobCode, string db) => $"PunchDropdownData?Model={model}&JobCode={JobCode}&db={db}";
        public static string getSectionCodes(string model, string db) => $"GetSectionCodes?Model={model}&db={db}";
        public static string getCompanyCategoryCodes(string model, string db) => $"GetCompanyCategoryCodes?Model={model}&db={db}";
        public static string Get7000ITRSeries(string tag, string checkSheet, string db) => $"Get_7000_SeriesITR?Tag={tag}&CheckSheet={checkSheet}&db={db}";
        public static string Post_7000_030A_031A(string db) => $"Post_7000_030A_031A?db={db}";
        public static string Post_7000_040A_041A_042A(string db) => $"Post_7000_040A_041A_042A?db={db}";
        public static string Post_7000_080A_090A_091A(string db) => $"Post_7000_080A_090A_091A?db={db}";
        public static string Post_8000_003A_(string db) => $"Post_8000_003A?db={db}";
        public static string Post_8100_001A(string db) => $"Post_8100_001A?db={db}";
        public static string Post_8100_002A(string db) => $"Post_8100_002A?db={db}";
        public static string Post_8140_001A(string db) => $"Post_8140_001A?db={db}";
        public static string Post_8121_002A(string db) => $"Post_8121_002A?db={db}";
        public static string Post_8260_002A(string db) => $"Post_8260_002A?db={db}";
        public static string Post_8161_001A(string db) => $"Post_8161_001A?db={db}";
        public static string Post_8121_004A(string db) => $"Post_8121_004A?db={db}";
        public static string Post_8161_002A(string db) => $"Post_8161_002A?db={db}";
        public static string Post_8000_101A(string db) => $"Post_8000_101A?db={db}";
        public static string Get_TestEquipmentData(int ProjectID,string db) => $"Get_TestEquipmentData?ProjectID={ProjectID}&db={db}";
        public static string Post_8211_002A(string db) => $"Post_8211_002A?db={db}";
        public static string Post_7000_101AHarmony(string db) => $"Post_7000_101AHarmony?db={db}";
        public static string Post_8140_002A(string db) => $"Post_8140_002A?db={db}";
        public static string Post_8140_004A(string db) => $"Post_8140_004A?db={db}";
        public static string PostVMHubPunchImages(string db) => $"VMHub/PostVMHubPunchImages?db={db}";


        //post sync status to vmhub
        public static string Url_TagITRStatusPostToDatahub(long ProjectID) => $"SyncLog/TagITRStatusPostToDatahub?ProjectID={ProjectID}";
        public static string Url_GetFormHeaderDatafromDatahub(long ProjectID, string TagNo, string db) => $"GetFormHeaderData?ProjectID={ProjectID}&TagNo={TagNo}&db={db}";
        public static string Url_GetDrowRevfromDatahub(long ProjectID, string DrawingNo, string db) => $"GetDrowRevfromDatahub?ProjectID={ProjectID}&DrawingNo={DrawingNo}&db={db}";
        //get document from db
        public static string Url_GetItrImages(string Module, int LinkedID) => $"VMHub/GetImages?Module={Module}&LinkedID={LinkedID}";
        //get signature rules for completions 
        public static string Url_GetSignaturesForTag(long ProjectID, string ProjectName,  string ItrText, string db) => $"SyncLog/GetSignaturesForTag?ProjectID={ProjectID}&ProjectName={ProjectName}&ItrText={ItrText}&db={db}";
        public static string Post_8170_002A(string db) => $"Post_8170_002A?db={db}";
        public static string Post_8300_003A(string db) => $"Post_8300_003A?db={db}";
        public static string Post_8170_007A(string db) => $"Post_8170_007A?db={db}";

        public static string Url_GetCompletionsUsers(string db) => $"User/GetUsers?db={db}";
        #endregion
    }
}

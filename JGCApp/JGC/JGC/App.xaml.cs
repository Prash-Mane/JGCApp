using Acr.UserDialogs;
using Autofac;
using JGC.Common.Helpers;
using JGC.Common.Extentions;
using JGC.Common.Interfaces;
using JGC.Common.Services;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.Models;
using JGC.UserControls.PopupControls;
using JGC.ViewModel;
using JGC.ViewModels;
using JGC.ViewModels.E_Test_Package;
using JGC.Views;
using JGC.Views.E_Test_Package;
using JGC.Viwes;
using Plugin.Connectivity;
using Plugin.Media;
using Prism.Autofac;
using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JGC.Views.Work_Pack;
using JGC.ViewModels.Work_Pack;
using System.Net.Http;
using System.Text.RegularExpressions;
using JGC.DataBase.DataTables.WorkPack;
using System.Threading;
using JGC.Views.MasterPage;
using JGC.ViewModels.MasterViewModel;
using Plugin.LatestVersion;
using JGC.DataBase.DataTables.ITR;
using JGC.Views.E_Reporter;
using JGC.ViewModels.E_Reporter;
using JGC.DataBase.DataTables.ModsCore;
using JGC.DataBase.DataTables.Completions;
using JGC.Views.Completions;
using JGC.ViewModels.Completions;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace JGC
{
    public partial class App : PrismApplication
    {
                
        #region Properties
        public static INavigationService Navigation { get; set; }
        public static bool IsNavigating { get; set; }
        public static bool IsBusy { get; set; }
        public static IContainerProvider ContainerProvider { get; set; }
        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }
        public static Action<bool> ChangeMenuPresenter { get; set; }

        #endregion

        #region Protected
        protected override void OnStart()
        {
            // Handle when your app starts
        }
        protected override void OnSleep()
        {
            if(!(Settings.IsStop == 1))
            Settings.IsStop = 0;
            // Handle when your app sleeps
        }
        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //E-Reporter Tables
            containerRegistry.Register<IRepository<T_UserDetails>, Repository<T_UserDetails>>();
            containerRegistry.Register<IRepository<T_UserProject>, Repository<T_UserProject>>();
            containerRegistry.Register<IRepository<T_EReports>, Repository<T_EReports>>();
            containerRegistry.Register<IRepository<T_EReports_Signatures>, Repository<T_EReports_Signatures>>();
            containerRegistry.Register<IRepository<T_EReports_UsersAssigned>, Repository<T_EReports_UsersAssigned>>();
            containerRegistry.Register<IRepository<T_Drawings>, Repository<T_Drawings>>();
            containerRegistry.Register<IRepository<T_CMR_HeatNos>, Repository<T_CMR_HeatNos>>();            
            containerRegistry.Register<IRepository<T_Welders>, Repository<T_Welders>>();
            containerRegistry.Register<IRepository<T_DWR_HeatNos>, Repository<T_DWR_HeatNos>>(); 
            containerRegistry.Register<IRepository<T_WPS_MSTR>, Repository<T_WPS_MSTR>>(); 
            containerRegistry.Register<IRepository<T_RT_Defects>, Repository<T_RT_Defects>>(); 
            containerRegistry.Register<IRepository<T_StorageAreas>, Repository<T_StorageAreas>>(); 
            containerRegistry.Register<IRepository<T_CMR_StorageAreas>, Repository<T_CMR_StorageAreas>>();
            containerRegistry.Register<IRepository<T_BaseMetal>, Repository<T_BaseMetal>>(); 
            containerRegistry.Register<IRepository<T_PartialRequest>, Repository<T_PartialRequest>>();
            containerRegistry.Register<IRepository<T_Setting>, Repository<T_Setting>>();
            containerRegistry.Register<IRepository<T_DWR>, Repository<T_DWR>>();
            containerRegistry.Register<IRepository<T_WeldProcesses>, Repository<T_WeldProcesses>>();

            //E-Test Package Tables
            containerRegistry.Register<IRepository<T_ETestPackages>, Repository<T_ETestPackages>>();
            containerRegistry.Register<IRepository<T_ControlLogSignature>, Repository<T_ControlLogSignature>>();
            containerRegistry.Register<IRepository<T_AttachedDocument>, Repository<T_AttachedDocument>>();
            containerRegistry.Register<IRepository<T_PunchList>, Repository<T_PunchList>>();
            containerRegistry.Register<IRepository<T_PunchImage>, Repository<T_PunchImage>>();
            containerRegistry.Register<IRepository<T_AdminFolders>, Repository<T_AdminFolders>>();
            containerRegistry.Register<IRepository<T_AdminControlLog>, Repository<T_AdminControlLog>>();
            containerRegistry.Register<IRepository<T_AdminPresetPunches>, Repository<T_AdminPresetPunches>>();
            containerRegistry.Register<IRepository<T_AdminPunchCategories>, Repository<T_AdminPunchCategories>>();
            containerRegistry.Register<IRepository<T_AdminFunctionCodes>, Repository<T_AdminFunctionCodes>>();
            containerRegistry.Register<IRepository<T_AdminPunchLayer>, Repository<T_AdminPunchLayer>>();
            containerRegistry.Register<IRepository<T_AdminControlLogFolder>, Repository<T_AdminControlLogFolder>>();
            containerRegistry.Register<IRepository<T_AdminDrainRecordContent>, Repository<T_AdminDrainRecordContent>>();
            containerRegistry.Register<IRepository<T_AdminDrainRecordAcceptedBy>, Repository<T_AdminDrainRecordAcceptedBy>>();
            containerRegistry.Register<IRepository<T_AdminControlLogPunchLayer>, Repository<T_AdminControlLogPunchLayer>>();
            containerRegistry.Register<IRepository<T_AdminControlLogPunchCategory>, Repository<T_AdminControlLogPunchCategory>>();
            containerRegistry.Register<IRepository<T_AdminControlLogActionParty>, Repository<T_AdminControlLogActionParty>>();
            containerRegistry.Register<IRepository<T_AdminControlLogNaAutoSignatures>, Repository<T_AdminControlLogNaAutoSignatures>>();
            containerRegistry.Register<IRepository<T_AdminTestRecordDetails>, Repository<T_AdminTestRecordDetails>>();
            containerRegistry.Register<IRepository<T_AdminTestRecordConfirmation>, Repository<T_AdminTestRecordConfirmation>>();
            containerRegistry.Register<IRepository<T_AdminTestRecordAcceptedBy>, Repository<T_AdminTestRecordAcceptedBy>>();
            containerRegistry.Register<IRepository<T_DrainRecordContent>, Repository<T_DrainRecordContent>>();
            containerRegistry.Register<IRepository<T_DrainRecordAcceptedBy>, Repository<T_DrainRecordAcceptedBy>>();
            containerRegistry.Register<IRepository<T_TestRecordDetails>, Repository<T_TestRecordDetails>>();
            containerRegistry.Register<IRepository<T_TestRecordConfirmation>, Repository<T_TestRecordConfirmation>>();
            containerRegistry.Register<IRepository<T_TestRecordAcceptedBy>, Repository<T_TestRecordAcceptedBy>>();
            containerRegistry.Register<IRepository<T_TestRecordImage>, Repository<T_TestRecordImage>>();
            containerRegistry.Register<IRepository<T_TestLimitDrawing>, Repository<T_TestLimitDrawing>>(); 
            containerRegistry.Register<IRepository<T_UnitID>, Repository<T_UnitID>>();
            //new added for pre test and post test records
            containerRegistry.Register<IRepository<T_PreTestRecordAcceptedBy>, Repository<T_PreTestRecordAcceptedBy>>();
            containerRegistry.Register<IRepository<T_PreTestRecordContent>, Repository<T_PreTestRecordContent>>();
            containerRegistry.Register<IRepository<T_PostTestRecordAcceptedBy>, Repository<T_PostTestRecordAcceptedBy>>();
            containerRegistry.Register<IRepository<T_PostTestRecordContent>, Repository<T_PostTestRecordContent>>();
            containerRegistry.Register<IRepository<T_AdminPreTestRecordContent>, Repository<T_AdminPreTestRecordContent>>();
            containerRegistry.Register<IRepository<T_AdminPreTestRecordAcceptedBy>, Repository<T_AdminPreTestRecordAcceptedBy>>();
            containerRegistry.Register<IRepository<T_AdminPostTestRecordAcceptedBy>, Repository<T_AdminPostTestRecordAcceptedBy>>();
            containerRegistry.Register<IRepository<T_AdminPostTestRecordContent>, Repository<T_AdminPostTestRecordContent>>();


            //Job Setting
            containerRegistry.Register<IRepository<T_IWP>, Repository<T_IWP>>(); 
            containerRegistry.Register<IRepository<T_IWPStatus>, Repository<T_IWPStatus>>();
            containerRegistry.Register<IRepository<T_Predecessor>, Repository<T_Predecessor>>();
            containerRegistry.Register<IRepository<T_Successor>, Repository<T_Successor>>();
            containerRegistry.Register<IRepository<T_IWPDrawings>, Repository<T_IWPDrawings>>();
            containerRegistry.Register<IRepository<T_IWPAttachments>, Repository<T_IWPAttachments>>();
            containerRegistry.Register<IRepository<T_ManPowerResource>, Repository<T_ManPowerResource>>();
            containerRegistry.Register<IRepository<T_ManPowerLog>, Repository<T_ManPowerLog>>();
            containerRegistry.Register<IRepository<T_WorkerScanned>, Repository<T_WorkerScanned>>();
            containerRegistry.Register<IRepository<T_TagMilestoneStatus>, Repository<T_TagMilestoneStatus>>(); 
            containerRegistry.Register<IRepository<T_TagMilestoneImages>, Repository<T_TagMilestoneImages>>();
            containerRegistry.Register<IRepository<T_IWPAdminControlLog>, Repository<T_IWPAdminControlLog>>();
            containerRegistry.Register<IRepository<T_IWPControlLogSignatures>, Repository<T_IWPControlLogSignatures>>();
            containerRegistry.Register<IRepository<T_IWPPunchCategory>, Repository<T_IWPPunchCategory>>();
            containerRegistry.Register<IRepository<T_IWPPunchLayer>, Repository<T_IWPPunchLayer>>();
            containerRegistry.Register<IRepository<T_CWPDrawings>, Repository<T_CWPDrawings>>();
            containerRegistry.Register<IRepository<T_IWPPunchControlItem>, Repository<T_IWPPunchControlItem>>(); 
            containerRegistry.Register<IRepository<T_IWPPunchImage>, Repository<T_IWPPunchImage>>();
            containerRegistry.Register<IRepository<T_IWPAdminPunchLayer>, Repository<T_IWPAdminPunchLayer>>();
            containerRegistry.Register<IRepository<T_IWPPunchCategories>, Repository<T_IWPPunchCategories>>();
            containerRegistry.Register<IRepository<T_IWPFunctionCodes>, Repository<T_IWPFunctionCodes>>();             
            containerRegistry.Register<IRepository<T_IWPCompanyCategoryCodes>, Repository<T_IWPCompanyCategoryCodes>>();
            containerRegistry.Register<IRepository<T_IwpPresetPunch>, Repository<T_IwpPresetPunch>>();
            containerRegistry.Register<IRepository<T_CwpTag>, Repository<T_CwpTag>>();
            


            //ITR
            containerRegistry.Register<IRepository<T_ITRCommonHeaderFooter>, Repository<T_ITRCommonHeaderFooter>>();
            containerRegistry.Register<IRepository<T_ITRRecords_30A_31A>, Repository<T_ITRRecords_30A_31A>>();
            containerRegistry.Register<IRepository<T_ITRTubeColors>, Repository<T_ITRTubeColors>>();
            containerRegistry.Register<IRepository<T_ITRRecords_040A_041A_042A>, Repository<T_ITRRecords_040A_041A_042A>>();
            containerRegistry.Register<IRepository<T_ITRInsulationDetails>, Repository<T_ITRInsulationDetails>>();
            containerRegistry.Register<IRepository<T_CommonHeaderFooterSignOff>, Repository<T_CommonHeaderFooterSignOff>>();
            containerRegistry.Register<IRepository<T_ITRRecords_080A_090A_091A>, Repository<T_ITRRecords_080A_090A_091A>>();
            containerRegistry.Register<IRepository<T_ITR8100_001A_CTdata>, Repository<T_ITR8100_001A_CTdata>>();
            containerRegistry.Register<IRepository<T_ITR8100_001A_InsulationResistanceTest>, Repository<T_ITR8100_001A_InsulationResistanceTest>>();
            containerRegistry.Register<IRepository<T_ITR8100_001A_RatioTest>, Repository<T_ITR8100_001A_RatioTest>>();
            containerRegistry.Register<IRepository<T_ITR8100_001A_TestInstrumentData>, Repository<T_ITR8100_001A_TestInstrumentData>>();
            containerRegistry.Register<IRepository<T_ITR8000_003ARecords>, Repository<T_ITR8000_003ARecords>>();
            containerRegistry.Register<IRepository<T_ITR8000_003A_AcceptanceCriteria>, Repository<T_ITR8000_003A_AcceptanceCriteria>>();
            containerRegistry.Register<IRepository<T_ITRRecords_8100_002A>, Repository<T_ITRRecords_8100_002A>>();
            containerRegistry.Register<IRepository<T_ITRRecords_8100_002A_InsRes_Test>, Repository<T_ITRRecords_8100_002A_InsRes_Test>>();
            containerRegistry.Register<IRepository<T_ITRRecords_8100_002A_Radio_Test>, Repository<T_ITRRecords_8100_002A_Radio_Test>>();
            containerRegistry.Register<IRepository<T_ITR8140_001A_ContactResisTest>, Repository<T_ITR8140_001A_ContactResisTest>>();
            containerRegistry.Register<IRepository<T_ITR8140_001ATestInstrucitonData>, Repository<T_ITR8140_001ATestInstrucitonData>>();
            containerRegistry.Register<IRepository<T_ITR8140_001AInsulationResistanceTest>, Repository<T_ITR8140_001AInsulationResistanceTest>>();
            containerRegistry.Register<IRepository<T_ITR8140_001ADialectricTest>, Repository<T_ITR8140_001ADialectricTest>>();
            containerRegistry.Register<IRepository<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents>, Repository<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents>>();
            containerRegistry.Register<IRepository<T_ITR8121_002A_Records>, Repository<T_ITR8121_002A_Records>>();
            containerRegistry.Register<IRepository<T_ITR8121_002A_TransformerRadioTest>, Repository<T_ITR8121_002A_TransformerRadioTest>>();
            containerRegistry.Register<IRepository<T_ITR_8260_002A_Body>, Repository<T_ITR_8260_002A_Body>>();
            containerRegistry.Register<IRepository<T_ITR_8260_002A_DielectricTest>, Repository<T_ITR_8260_002A_DielectricTest>>();
            containerRegistry.Register<IRepository<T_ITRRecords_8161_001A_Body>, Repository<T_ITRRecords_8161_001A_Body>>();
            containerRegistry.Register<IRepository<T_ITRRecords_8161_001A_InsRes>, Repository<T_ITRRecords_8161_001A_InsRes>>();
            containerRegistry.Register<IRepository<T_ITRRecords_8161_001A_ConRes>, Repository<T_ITRRecords_8161_001A_ConRes>>();
            containerRegistry.Register<IRepository<T_ITR8121_004AInCAndAuxiliary>, Repository<T_ITR8121_004AInCAndAuxiliary>>();
            containerRegistry.Register<IRepository<T_ITR8121_004ATransformerRatioTest>, Repository<T_ITR8121_004ATransformerRatioTest>>();
            containerRegistry.Register<IRepository<T_ITR8121_004ATestInstrumentData>, Repository<T_ITR8121_004ATestInstrumentData>>();
            containerRegistry.Register<IRepository<T_ITR8161_002A_Body>, Repository<T_ITR8161_002A_Body>>();
            containerRegistry.Register<IRepository<T_ITR8161_002A_DielectricTest>, Repository<T_ITR8161_002A_DielectricTest>>(); 
            containerRegistry.Register<IRepository<T_ITR8000_101A_Generalnformation>, Repository<T_ITR8000_101A_Generalnformation>>();
            containerRegistry.Register<IRepository<T_ITR8000_101A_RecordISBarrierDetails>, Repository<T_ITR8000_101A_RecordISBarrierDetails>>();
            containerRegistry.Register<IRepository<T_ITRRecords_8211_002A_Body>, Repository<T_ITRRecords_8211_002A_Body>>();
            containerRegistry.Register<IRepository<T_ITRRecords_8211_002A_RunTest>, Repository<T_ITRRecords_8211_002A_RunTest>>();
            containerRegistry.Register<IRepository<T_ITR_7000_101AHarmony_Genlnfo>, Repository<T_ITR_7000_101AHarmony_Genlnfo>>();
            containerRegistry.Register<IRepository<T_ITR_7000_101AHarmony_ActivityDetails>, Repository<T_ITR_7000_101AHarmony_ActivityDetails>>();
            containerRegistry.Register<IRepository<T_ITR_8140_002A_RecordsMechnicalOperation>, Repository<T_ITR_8140_002A_RecordsMechnicalOperation>>();
            containerRegistry.Register<IRepository<T_ITR_8140_002A_RecordsAnalogSignal>, Repository<T_ITR_8140_002A_RecordsAnalogSignal>>();
            containerRegistry.Register<IRepository<T_ITR_8140_002A_Records>, Repository<T_ITR_8140_002A_Records>>();
            containerRegistry.Register<IRepository<T_ITR_8140_004A_Records>, Repository<T_ITR_8140_004A_Records>>();
            containerRegistry.Register<IRepository<T_CompletionsUsers>, Repository<T_CompletionsUsers>>();

            containerRegistry.Register<IRepository<T_ITRRecords_8170_002A_Voltage_Reading>, Repository<T_ITRRecords_8170_002A_Voltage_Reading>>();
            containerRegistry.Register<IRepository<T_ITR_8170_002A_IndifictionLamp>, Repository<T_ITR_8170_002A_IndifictionLamp>>();
            containerRegistry.Register<IRepository<T_ITR_8170_002A_InsRes>, Repository<T_ITR_8170_002A_InsRes>>();
            containerRegistry.Register<IRepository<T_ITR_8300_003A_Body>, Repository<T_ITR_8300_003A_Body>>();
            containerRegistry.Register<IRepository<T_ITR_8300_003A_Illumin>, Repository<T_ITR_8300_003A_Illumin>>();
            containerRegistry.Register<IRepository<T_ITR_8170_007A_OP_Function_Test>, Repository<T_ITR_8170_007A_OP_Function_Test>>();

        containerRegistry.RegisterInstance(CrossConnectivity.Current);
            containerRegistry.RegisterInstance(UserDialogs.Instance);
            containerRegistry.RegisterInstance(CrossMedia.Current);
            containerRegistry.RegisterInstance<IContainerRegistry>(containerRegistry);
            containerRegistry.Register<IHttpHelper, HttpHelper>();
            containerRegistry.Register<ICheckValidLogin, CheckValidLogin>();
            containerRegistry.Register<IDownloadService, DownloadService>();
            containerRegistry.Register<IResizeImageService, ResizeImageService>();

            //Master Page 
            containerRegistry.RegisterTypeForViewModelNavigation<MainMasterDetailPage, MainMasterDetailViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<MasterNavigationPage, MasterNavigationViewModel>();

            //Registering ViewModelsForNavigation
            containerRegistry.RegisterTypeForViewModelNavigation<LoginPage, LoginViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<ProjectListPage, ProjectViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<EReporterProjectListPage, EReporterProjectViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<EReportSelectionPage, EReportSelectionViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<DashboardPage, DashboardViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<DownloadPage, DownloadViewModel>();
            //containerRegistry.RegisterTypeForViewModelNavigation<DWR_EReporterPage, DWR_EReporterViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<RIR_EReporterPage, RIR_EReporterViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<MRR_EReporterPage, MRR_EReporterViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<CMR_EReporterPage, CMR_EReporterViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<UploadPage, UploadViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<SettingPage, SettingViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<ModulesPage, ModuleViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<PDFviever, PDFvieverViewModel>();

            //E-Test Package pages
            containerRegistry.RegisterTypeForViewModelNavigation<ETestPackageList, ETestPackageVewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<PackageDetailPage, PackageDetailViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<PunchOverviewPage, PunchOverviewViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<PunchViewPage, PunchViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<TestRecordPage, TestRecordViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<ControlLogPage, ControlLogViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<PandidPage, PandidViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<DrainRecord, DrainRecordViewModel>();
            //New Added For Etest
            containerRegistry.RegisterTypeForViewModelNavigation<PreTestRecordPage, PreTestRecordViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<PostTestRecordPage, PostTestRecordViewModel>();


            //Job Setting pages
            containerRegistry.RegisterTypeForViewModelNavigation<IWPSelectionPage, IWPSelectionViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<IWPPdfPage, IWPPdfViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<DrawingsPage, DrawingsViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<IWPControlLogPage, IWPControlLogViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<CWPTagStatusPage, CWPTagStatusViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<ManPowerResourcePage, ManPowerResourceViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<PunchControlPage, PunchControlViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<AddPunchControlPage, AddPunchControlViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<EditPunchControlPage, EditPunchControlViewModel>(); 
            containerRegistry.RegisterTypeForViewModelNavigation<PredecessorPage, PredecessorViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<SuccessorPage, SuccessorViewModel>();


            //
            ////modsScoreTables   
            containerRegistry.Register<IRepository<T_BACKEND>, Repository<T_BACKEND>>();
            containerRegistry.Register<IRepository<T_MatListHH>, Repository<T_MatListHH>>();
            containerRegistry.Register<IRepository<T_TOKEN>, Repository<T_TOKEN>>();
            containerRegistry.Register<IRepository<T_UserProjects>, Repository<T_UserProjects>>();
            containerRegistry.Register<IRepository<T_UserControl>, Repository<T_UserControl>>();
          

            //Completion Tables 
            containerRegistry.Register<IRepository<T_DataRefs>, Repository<T_DataRefs>>();
            containerRegistry.Register<IRepository<T_CompletionsDrawings>, Repository<T_CompletionsDrawings>>();
            containerRegistry.Register<IRepository<T_Handover>, Repository<T_Handover>>();
            containerRegistry.Register<IRepository<T_HandoverWorkpacks>, Repository<T_HandoverWorkpacks>>();
            containerRegistry.Register<IRepository<T_HeaderItems>, Repository<T_HeaderItems>>();
            containerRegistry.Register<IRepository<T_JOINT>, Repository<T_JOINT>>();
            containerRegistry.Register<IRepository<T_JOINT_DRAWINGS>, Repository<T_JOINT_DRAWINGS>>();
            containerRegistry.Register<IRepository<T_JOINT_FLANGEMANAGEMENTCHECKLIST>, Repository<T_JOINT_FLANGEMANAGEMENTCHECKLIST>>();
            containerRegistry.Register<IRepository<T_JOINT_HEADER>, Repository<T_JOINT_HEADER>>();
            containerRegistry.Register<IRepository<T_JOINT_QUESTIONS>, Repository<T_JOINT_QUESTIONS>>();
            containerRegistry.Register<IRepository<T_JS_JOINT_LEAK_RECORDS>, Repository<T_JS_JOINT_LEAK_RECORDS>>();
            containerRegistry.Register<IRepository<T_JS_PUNCH_LIST>, Repository<T_JS_PUNCH_LIST>>();
            containerRegistry.Register<IRepository<T_JS_SIGN_OFF>, Repository<T_JS_SIGN_OFF>>();
            containerRegistry.Register<IRepository<T_PUNCH_SELECTIONS>, Repository<T_PUNCH_SELECTIONS>>();
            containerRegistry.Register<IRepository<T_CompletionsPunchList>, Repository<T_CompletionsPunchList>>();
            containerRegistry.Register<IRepository<T_PunchListDropDowns>, Repository<T_PunchListDropDowns>>();
            containerRegistry.Register<IRepository<T_SignOff>, Repository<T_SignOff>>();
            containerRegistry.Register<IRepository<T_SyncedTags>, Repository<T_SyncedTags>>();
            containerRegistry.Register<IRepository<T_Tag_headers>, Repository<T_Tag_headers>>();

            containerRegistry.Register<IRepository<T_CHECKSHEET>, Repository<T_CHECKSHEET>>();
            containerRegistry.Register<IRepository<T_CHECKSHEET_PER_TAG>, Repository<T_CHECKSHEET_PER_TAG>>();
            containerRegistry.Register<IRepository<T_CHECKSHEET_QUESTION>, Repository<T_CHECKSHEET_QUESTION>>();
            containerRegistry.Register<IRepository<T_SYSTEM>, Repository<T_SYSTEM>>();
            containerRegistry.Register<IRepository<T_TAG>, Repository<T_TAG>>();
            //  containerRegistry.Register<IRepository<T_TAG_HEADER>, Repository<T_TAG_HEADER>>();
            containerRegistry.Register<IRepository<T_TAG_SHEET_ANSWER>, Repository<T_TAG_SHEET_ANSWER>>();
            containerRegistry.Register<IRepository<T_TAG_SHEET_HEADER>, Repository<T_TAG_SHEET_HEADER>>();
            containerRegistry.Register<IRepository<T_SignOffHeader>, Repository<T_SignOffHeader>>();
            containerRegistry.Register<IRepository<T_WorkPack>, Repository<T_WorkPack>>();
            containerRegistry.Register<IRepository<T_PunchComponent>, Repository<T_PunchComponent>>();
            containerRegistry.Register<IRepository<T_PunchSystem>, Repository<T_PunchSystem>>();
            containerRegistry.Register<IRepository<T_PunchPCWBS>, Repository<T_PunchPCWBS>>();
            containerRegistry.Register<IRepository<T_PunchFWBS>, Repository<T_PunchFWBS>>();
            containerRegistry.Register<IRepository<T_SectionCode>, Repository<T_SectionCode>>();
            containerRegistry.Register<IRepository<T_CompanyCategoryCode>, Repository<T_CompanyCategoryCode>>(); 
            containerRegistry.Register<IRepository<T_CompletionSystems>, Repository<T_CompletionSystems>>();
            containerRegistry.Register<IRepository<T_TestEquipmentData>, Repository<T_TestEquipmentData>>(); 
            containerRegistry.Register<IRepository<T_ITRInstrumentData>, Repository<T_ITRInstrumentData>>();
            containerRegistry.Register<IRepository<T_Ccms_signature>, Repository<T_Ccms_signature>>();

            //Completions Pages
            containerRegistry.RegisterTypeForViewModelNavigation<CompletionProjectList, CompletionProjectViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<CompletionsDashboard, CompletionsDashboardViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<TagRegisterPage, TagRegisterViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<SyncPage, SyncViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<CompletionPunchList, CompletionPunchListViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<CreateNewPunchPage, CreateNewPunchViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<CompletionDrawingPage, CompletionDrawingViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<HandoverPage, HandoverViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<CompletionTestPack, CompletionTestPackViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<CompletionSetting, CompletionSettingViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<NewPunchPage, NewPunchViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<CopyITRData, CopyITRDataViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<ITRPage, ITRViewModel>();

            //Completions services
            containerRegistry.Register<IDrawingsSyncer, DrawingsSyncer>();
            containerRegistry.Register<IHandoverSyncer, HandoverSyncer>();
            containerRegistry.Register<ISystemPunchListSyncer, SystemPunchListSyncer>();
            containerRegistry.Register<ISystemsFullSystemSyncercs, SystemsFullSystemSyncer>();
            containerRegistry.Register<ITestPackSyncer, TestPackSyncer>();
            containerRegistry.Register<IWorkpackFullSystemSyncer, WorkpackFullSystemSyncer>();
            containerRegistry.Register<IWorkpackPunchListSyncer, WorkpackPunchListSyncer>();
            containerRegistry.Register<IFtpService, FtpService>();
            containerRegistry.Register<IITRService, ITRService>();

            containerRegistry.Register<IRepository<T_DWR>, Repository<T_DWR>>();
            containerRegistry.Register<IRepository<T_WeldProcesses>, Repository<T_WeldProcesses>>();


            //new added for pre test and post test records
            containerRegistry.Register<IRepository<T_PreTestRecordAcceptedBy>, Repository<T_PreTestRecordAcceptedBy>>();
            containerRegistry.Register<IRepository<T_PreTestRecordContent>, Repository<T_PreTestRecordContent>>();
            containerRegistry.Register<IRepository<T_PostTestRecordAcceptedBy>, Repository<T_PostTestRecordAcceptedBy>>();
            containerRegistry.Register<IRepository<T_PostTestRecordContent>, Repository<T_PostTestRecordContent>>();
            containerRegistry.Register<IRepository<T_AdminPreTestRecordContent>, Repository<T_AdminPreTestRecordContent>>();
            containerRegistry.Register<IRepository<T_AdminPreTestRecordAcceptedBy>, Repository<T_AdminPreTestRecordAcceptedBy>>();
            containerRegistry.Register<IRepository<T_AdminPostTestRecordAcceptedBy>, Repository<T_AdminPostTestRecordAcceptedBy>>();
            containerRegistry.Register<IRepository<T_AdminPostTestRecordContent>, Repository<T_AdminPostTestRecordContent>>();



            //new DWR pages
            containerRegistry.RegisterTypeForViewModelNavigation<DWRControlLogPag, DWRControlLogViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<AttachmentPage, AttachmentViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<DWRDrawingPage, DWRDrawingViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<JointDetailsPage, JointDetailsViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<InspectJointPage, InspectJointViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<DWR_AddJointPage, DWR_AddJointViewModel>();
            containerRegistry.RegisterTypeForViewModelNavigation<DWR_InspectJointPage, DWR_InspectJointViewModel>();
            //containerRegistry.RegisterTypeForViewModelNavigation<SpoolDrawingListPage, SpoolDrawingListViewModel>();

        }
        protected override void OnInitialized()
        {
            Navigation = NavigationService;
            Settings.AppCurrentVersion = VersionTracking.CurrentVersion;
            ScreenHeight = (int)DeviceDisplay.MainDisplayInfo.Height;
            ScreenWidth = (int)DeviceDisplay.MainDisplayInfo.Width;
        }

        #endregion

        #region Private
        private async Task CheckAppUpgraded(string NewVersion, string url)
        {
            var split = NewVersion.Split('.');
            int _NewVersionBase = int.Parse(split[0]);
            string _NewVersion = split[1] + "." + split[2];

            var currentVersion = Settings.AppCurrentVersion.Split('.');
            int _currentBase = int.Parse(currentVersion[0]);
            string _currentVersion = currentVersion[1] + "." + currentVersion[2];



            if (Device.RuntimePlatform == Device.UWP)
            {
                Container.GetContainer().Resolve<IRepository<T_UserDetails>>();
                Container.GetContainer().Resolve<IRepository<T_UserProject>>();
                //await NavigationService.NavigateFromMenuAsync<LoginViewModel>();
                MainPage = new LoginPage();
            }
            else if (_currentBase < _NewVersionBase )  // !string.Equals(Settings.AppCurrentVersion, version)
            {
                if (await UserDialogs.Instance.ConfirmAsync(" Please upgrade your app ", "Latest version is " + NewVersion, "Go to Store", "Cancel"))
                    Device.OpenUri(new Uri(Device.OnPlatform(url, url, url)));

                var closer = DependencyService.Get<ICloseApplication>();
                closer?.closeApplication();

            }
            else if (float.Parse(_currentVersion) < float.Parse(_NewVersion))
            {
                if (await UserDialogs.Instance.ConfirmAsync(" Please upgrade your app ", "Latest version is " + NewVersion, "Go to Store", "Cancel"))
                    Device.OpenUri(new Uri(Device.OnPlatform(url, url, url)));

                var closer = DependencyService.Get<ICloseApplication>();
                closer?.closeApplication();
            }
            else
            {
                Container.GetContainer().Resolve<IRepository<T_UserDetails>>();
                Container.GetContainer().Resolve<IRepository<T_UserProject>>();
                //await NavigationService.NavigateFromMenuAsync<LoginViewModel>();
                 MainPage = new LoginPage();
            }
        }

        private async Task IsConnected()
        {
            await UserDialogs.Instance.AlertAsync("Requires an Internet Connection.", "Network Error", "Ok");
                var closer = DependencyService.Get<ICloseApplication>();
                closer?.closeApplication();
        }

        private async void CheckAppUpgradeForUWP() 
        {
            try
            {
                //string latestVersionNumber = await CrossLatestVersion.Current.Ge();
                var isLatest = await CrossLatestVersion.Current.IsUsingLatestVersion();
                if (!isLatest)
                {
                    var update = await UserDialogs.Instance.ConfirmAsync("There is a new version of this app available. Would you like to update now?", "New Version", "Yes", "No");

                    if (update)
                    {
                        await CrossLatestVersion.Current.OpenAppInStore();
                    }
                    else
                    {
                        var closer = DependencyService.Get<ICloseApplication>();
                        closer?.closeApplication();
                    }
                }
                else 
                {
                    Container.GetContainer().Resolve<IRepository<T_UserDetails>>();
                    Container.GetContainer().Resolve<IRepository<T_UserProject>>();
                    //await NavigationService.NavigateFromMenuAsync<LoginViewModel>();
                    MainPage = new LoginPage();
                }
            }
            catch (Exception ex) 
            {
                Container.GetContainer().Resolve<IRepository<T_UserDetails>>();
                Container.GetContainer().Resolve<IRepository<T_UserProject>>();
                //await NavigationService.NavigateFromMenuAsync<LoginViewModel>();
                MainPage = new LoginPage();
            }
        }
        #endregion

        #region Public
        public  App()
        {
            InitializeComponent();

            string appName = "com.MODS.JGC";

            if (string.IsNullOrWhiteSpace(appName))
            {
                throw new ArgumentNullException(nameof(appName));
            }

            if (CrossConnectivity.Current.IsConnected)
            {
                var version = string.Empty;

                string url;
                if (Device.RuntimePlatform == Device.Android)
                {
                    url = $"https://play.google.com/store/apps/details?id={appName}";
                    using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                    {
                        using (var handler = new HttpClientHandler())
                        {
                            using (var client = new HttpClient(handler))
                            {
                                using (var responseMsg = Task.Run(async () => await client.SendAsync(request, HttpCompletionOption.ResponseContentRead)).Result)
                                {
                                    if (!responseMsg.IsSuccessStatusCode)
                                    {
                                        // throw new LatestVersionException($"Error connecting to the Play Store. Url={url}.");
                                    }
                                    try
                                    {
                                        var content = responseMsg.Content == null ? null : Task.Run(async () => await responseMsg.Content.ReadAsStringAsync()).Result;

                                        var versionMatch = Regex.Match(content, "<div[^>]*>Current Version</div><span[^>]*><div[^>]*><span[^>]*>(.*?)<").Groups[1];
                                        if (versionMatch.Success)
                                        {
                                            version = versionMatch.Value.Trim();

                                            _ = CheckAppUpgraded(version, url);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        //throw new LatestVersionException($"Error parsing content from the Play Store. Url={url}.", e);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    CheckAppUpgradeForUWP();
                    //Container.GetContainer().Resolve<IRepository<T_UserDetails>>();
                    //Container.GetContainer().Resolve<IRepository<T_UserProject>>();
                    ////await NavigationService.NavigateFromMenuAsync<LoginViewModel>();
                    //MainPage = new LoginPage();
                }

                //Container.GetContainer().Resolve<IRepository<T_UserDetails>>();
                //Container.GetContainer().Resolve<IRepository<T_UserProject>>();
                //// await NavigationService.NavigateFromMenuAsync<LoginViewModel>();
                //MainPage = new LoginPage();
            }
            else
            {
              
                Container.GetContainer().Resolve<IRepository<T_UserDetails>>();
                Container.GetContainer().Resolve<IRepository<T_UserProject>>();
            
                MainPage = new LoginPage();
            }

        }
        #endregion
    }
}

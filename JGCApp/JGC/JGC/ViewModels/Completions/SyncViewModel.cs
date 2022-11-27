using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.Common.Services;
using JGC.DataBase;
using JGC.DataBase.DataTables.Completions;
using JGC.DataBase.DataTables.ITR;
using JGC.DataBase.DataTables.ModsCore;
using JGC.Models.Completions;
using JGC.Models.ITR;
using Newtonsoft.Json;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JGC.ViewModels.Completions
{
    public class SyncViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly ITRService _ITRService;
        //syncerServices
        private readonly IDrawingsSyncer _IDrawingsSyncer;
        private readonly IHandoverSyncer _IHandoverSyncer;
        private readonly ISystemPunchListSyncer _ISystemPunchListSyncer;
        private readonly ISystemsFullSystemSyncercs _ISystemsFullSystemSyncercs;
        private readonly ITestPackSyncer _ITestPackSyncer;
        private readonly ITRSyncer _ITRSyncer;
        private readonly IWorkpackFullSystemSyncer _IWorkpackFullSystemSyncer;
        private readonly IWorkpackPunchListSyncer _IWorkpackPunchListSyncer;
        private readonly IFtpService _IFtpService;
        private readonly IRepository<T_Handover> _CompletionsHandoverRepository;
        private readonly IRepository<T_HandoverWorkpacks> _HandoverWorkpackRepository;
        private readonly IRepository<T_DataRefs> _DataRefsRepository;
        private readonly IRepository<T_WorkPack> _WorkPackRepository;
        private readonly IRepository<T_TAG> _TAGRepository;
        private readonly IRepository<T_Tag_headers> _Tag_headersRepository;
        private readonly IRepository<T_CHECKSHEET> _CHECKSHEETRepository;
        private readonly IRepository<T_CHECKSHEET_PER_TAG> _CHECKSHEET_PER_TAGRepository;
        private readonly IRepository<T_CHECKSHEET_QUESTION> _CHECKSHEET_QUESTIONRepository;
        private readonly IRepository<T_CompletionsPunchList> _CompletionsPunchListRepository;
        private readonly IRepository<T_TAG_SHEET_ANSWER> _TAG_SHEET_ANSWERRepository;
        private readonly IRepository<T_TAG_SHEET_HEADER> _TAG_SHEET_HEADERRepository;
        private readonly IRepository<T_SignOffHeader> _SignOffHeaderRepository;
        private readonly IRepository<T_UserControl> _UserControlRepository;
        private readonly IRepository<T_ITRCommonHeaderFooter> _CommonHeaderFooterRepository;
        private readonly IRepository<T_ITRRecords_30A_31A> _RecordsRepository;
        private readonly IRepository<T_ITRTubeColors> _TubeColorsRepository;
        private readonly IRepository<T_ITRRecords_040A_041A_042A> _Records_04XARepository;
        private readonly IRepository<T_ITRInsulationDetails> _InsulationDetailsRepository;
        private readonly IRepository<T_CommonHeaderFooterSignOff> _CommonHeaderFooterSignOffRepository;
        private readonly IRepository<T_ITRRecords_080A_090A_091A> _Records_080A_09XARepository;
        private readonly IRepository<T_ITR8000_003ARecords> _Records_8000003ARepository;
        private readonly IRepository<T_ITR8000_003A_AcceptanceCriteria> _Records_8000003A_AcceptanceCriteriaRepository;
        private readonly IRepository<T_ITR8100_001A_CTdata> _ITR8100_001A_CTdataRepository;
        private readonly IRepository<T_ITR8100_001A_InsulationResistanceTest> _ITR8100_001A_IRTestRepository;
        private readonly IRepository<T_ITR8100_001A_RatioTest> _ITR8100_001A_RatioTestRepository;
        private readonly IRepository<T_ITR8100_001A_TestInstrumentData> _ITR8100_001A_TIDataRepository;

        private readonly IRepository<T_ITRRecords_8100_002A> _ITRRecords_8100_002ARepository;
        private readonly IRepository<T_ITRRecords_8100_002A_InsRes_Test> _ITRRecords_8100_002A_InsRes_TestRepository;
        private readonly IRepository<T_ITRRecords_8100_002A_Radio_Test> _ITRRecords_8100_002A_Radio_TestRepository;
        private readonly IRepository<T_ITRRecords_8161_001A_Body> _ITRRecords_8161_001A_BodyRepository;
        private readonly IRepository<T_ITRRecords_8161_001A_InsRes> _ITRRecords_8161_001A_InsResRepository;
        private readonly IRepository<T_ITRRecords_8161_001A_ConRes> _ITRRecords_8161_001A_ConResRepository;
        private readonly IRepository<T_ITR8121_004AInCAndAuxiliary> _ITR8121_004AInCAndAuxiliaryRepository;
        private readonly IRepository<T_ITR8121_004ATransformerRatioTest> _ITR8121_004ATransformerRatioTestRepository;
        private readonly IRepository<T_ITR8121_004ATestInstrumentData> _ITR8121_004ATestInstrumentDataRepository;

        private readonly IRepository<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents> _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents;
        private readonly IRepository<T_ITR8121_002A_Records> _ITR8121_002A_Records;
        private readonly IRepository<T_ITR8121_002A_TransformerRadioTest> _ITR8121_002A_TransformerRadioTest;
        private readonly IRepository<T_ITR8140_001A_ContactResisTest> _T_ITR8140_001A_ContactResisTestRepository;

        private readonly IRepository<T_ITR_8260_002A_Body> _ITR_8260_002A_BodyRepository;
        private readonly IRepository<T_ITR_8260_002A_DielectricTest> _ITR_8260_002A_DielectricTestRepository;
        private readonly IRepository<T_ITR8140_001AInsulationResistanceTest> _T_ITR8140_001AInsulationResistanceTestRepository;
        private readonly IRepository<T_ITR8140_001ADialectricTest> _T_ITR8140_001ADialectricTestRepository;
        private readonly IRepository<T_ITR8140_001ATestInstrucitonData> _T_ITR8140_001ATestInstrumentDataRepository;
        private readonly IRepository<T_TestEquipmentData> _TestEquipmentDataRepository;
        private readonly IRepository<T_ITRInstrumentData> _ITRInstrumentDataRepository;
        private readonly IRepository<T_ITR8161_002A_Body> _ITR8161_002A_BodyRepository;
        private readonly IRepository<T_ITR8161_002A_DielectricTest> _ITR8161_002A_DielectricTestRepository;
        private readonly IRepository<T_ITR8000_101A_Generalnformation> _ITR8000_101A_GeneralnformationRepository;
        private readonly IRepository<T_ITR8000_101A_RecordISBarrierDetails> _ITR8000_101A_RecordISBarrierDetailsRepository;
        private readonly IRepository<T_ITRRecords_8211_002A_Body> _ITRRecords_8211_002A_BodyRepository;
        private readonly IRepository<T_ITRRecords_8211_002A_RunTest> _ITRRecords_8211_002A_RunTestRepository;
        private readonly IRepository<T_ITR_7000_101AHarmony_Genlnfo> _ITR7000_101AHarmony_GenlnfoRepository;
        private readonly IRepository<T_ITR_7000_101AHarmony_ActivityDetails> _ITR7000_101AHarmony_ActivityDetailsRepository;
        private readonly IRepository<T_ITR_8140_002A_Records> _ITR_8140_002A_RecordsRepository;
        private readonly IRepository<T_ITR_8140_002A_RecordsMechnicalOperation> _ITR_8140_002A_RecordsMechnicalOperation_RecordsRepository;
        private readonly IRepository<T_ITR_8140_002A_RecordsAnalogSignal> _ITR_8140_002A_RecordsAnalogSignalRepository;
        private readonly IRepository<T_ITR_8140_004A_Records> _ITR_8140_004A_RecordsRepository;
        private readonly IRepository<T_ITRRecords_8170_002A_Voltage_Reading> _ITRRecords_8170_002A_Voltage_ReadingRepository;
        private readonly IRepository<T_ITR_8170_002A_IndifictionLamp> _ITR_8170_002A_IndifictionLampRepository;
        private readonly IRepository<T_ITR_8170_002A_InsRes> _ITR_8170_002A_InsResRepository;
        private readonly IRepository<T_ITR_8300_003A_Body> _ITR_8300_003A_BodyRepository;
        private readonly IRepository<T_ITR_8300_003A_Illumin> _ITR_8300_003A_IlluminRepository;
        private readonly IRepository<T_ITR_8170_007A_OP_Function_Test> _ITR_8170_007A_OP_Function_TestRepository;


        // for sync Itr's only 
        private Dictionary<string, List<string>> tagToSheetList = new Dictionary<string, List<string>>();

        private InitialDataModel SyncInitialList;
        // SyncProgressLogs splogs;
        private static string SyncData;

        private List<T_WorkPack> CompletionsSyncTAgs;
        //table repositories

        #region Properties
        private bool isVisibleSyncPopup { get; set; }
        public bool IsVisibleSyncPopup
        {
            get { return isVisibleSyncPopup; }
            set { isVisibleSyncPopup = value; RaisePropertyChanged(); }
        }
        private string placeHolderText { get; set; }
        public string PlaceHolderText
        {
            get { return placeHolderText; }
            set { placeHolderText = value; RaisePropertyChanged(); }
        }
        
        private string syncBy { get; set; }
        public string SyncBy
        {
            get { return syncBy; }
            set { syncBy = value; RaisePropertyChanged(); }
        }

        private bool isVisibleTestPacksSyncProgressPopup { get; set; }
        public bool IsVisibleTestPacksSyncProgressPopup
        {
            get { return isVisibleTestPacksSyncProgressPopup; }
            set { isVisibleTestPacksSyncProgressPopup = value; RaisePropertyChanged(); }
        }
        private bool isVisibleITRSyncProgressPopup { get; set; }
        public bool IsVisibleITRSyncProgressPopup
        {
            get { return isVisibleITRSyncProgressPopup; }
            set { isVisibleITRSyncProgressPopup = value; RaisePropertyChanged(); }
        }

        private bool isVisibleTagPopup { get; set; }
        public bool IsVisibleTagPopup
        {
            get { return isVisibleTagPopup; }
            set { isVisibleTagPopup = value; RaisePropertyChanged(); }
        }
        private bool isVisibleSyncButton { get; set; }
        public bool IsVisibleSyncButton
        {
            get { return isVisibleSyncButton; }
            set { isVisibleSyncButton = value; RaisePropertyChanged(); }
        }        
        private bool isVisibleWBSBtnSyncType { get; set; }
        public bool IsVisibleWBSBtnSyncType
        {
            get { return isVisibleWBSBtnSyncType; }
            set { isVisibleWBSBtnSyncType = value; RaisePropertyChanged(); }
        }
        private bool isEnableCancelButton { get; set; }
        public bool IsEnableCancelButton
        {
            get { return isEnableCancelButton; }
            set { isEnableCancelButton = value; RaisePropertyChanged(); }
        }
        private string syncCancelButtonText { get; set; }
        public string SyncCancelButtonText
        {
            get { return syncCancelButtonText; }
            set { syncCancelButtonText = value; RaisePropertyChanged("SyncCancelButtonText"); }
        }
        private bool isVisibleSyncProgressPopup { get; set; }
        public bool IsVisibleSyncProgressPopup
        {
            get { return isVisibleSyncProgressPopup; }
            set { isVisibleSyncProgressPopup = value; RaisePropertyChanged(); }
        }
        private bool isVisibleHandoverSyncProgressPopup { get; set; }
        public bool IsVisibleHandoverSyncProgressPopup
        {
            get { return isVisibleHandoverSyncProgressPopup; }
            set { isVisibleHandoverSyncProgressPopup = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<dynamic> runnableSyncList;
        public ObservableCollection<dynamic> RunnableSyncList
        {
            get { return runnableSyncList; }
            set { runnableSyncList = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<dynamic> itemSourceRunnableSyncList;
        public ObservableCollection<dynamic> ItemSourceRunnableSyncList
        {
            get { return itemSourceRunnableSyncList; }
            set { itemSourceRunnableSyncList = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<dynamic> itemSourceAddedSyncList;
        public ObservableCollection<dynamic> ItemSourceAddedSyncList
        {
            get { return itemSourceAddedSyncList; }
            set { itemSourceAddedSyncList = value; RaisePropertyChanged(); }
        }
        private bool checkUnderReviews { get; set; }
        public bool CheckUnderReviews
        {
            get { return checkUnderReviews; }
            set
            {
                if (checkUnderReviews != value)
                {
                    checkUnderReviews = value;
                    OnPropertyChanged("CheckUnderReviews");
                    // RaisePropertyChanged();
                    WorkpackFiletr();
                }
            }
        }
        private bool checkSyncAllUsers { get; set; }
        public bool CheckSyncAllUsers
        {
            get { return checkSyncAllUsers; }
            set
            {
                if (checkSyncAllUsers != value)
                {
                    checkSyncAllUsers = value;
                    OnPropertyChanged("CheckSyncAllUsers");
                    // RaisePropertyChanged();
                    WorkpackFiletr();
                }
            }
        }
        private string txtFilter { get; set; }
        public string TxtFilter
        {
            get { return txtFilter; }
            set
            {
                if (txtFilter != value)
                {
                    txtFilter = value;
                    OnPropertyChanged("TxtFilter");

                    if (RunnableSyncList.Any() && !String.IsNullOrEmpty(TxtFilter))
                        ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(RunnableSyncList.Where(s => s.name.ToLower().Contains(TxtFilter.ToLower())));
                    else if (RunnableSyncList.Any() && String.IsNullOrEmpty(TxtFilter))
                        ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(RunnableSyncList);

                    // RaisePropertyChanged();
                    //switch (PlaceHolderText)
                    //{
                    //    case "Filter System List": OnClickAsync("SyncbySystem");break;
                    //    case "Filter WorkPack List": OnClickAsync("SyncbyWorkPack"); break;
                    //    case "Filter PCWBS List": OnClickAsync("SyncbyPCWBS"); break;
                    //    case "Filter FWBS List": OnClickAsync("SyncbyFWBS"); break;
                    //    case "Filter Tag List": OnClickAsync("FilterByTag"); break;
                    //}
                    // WorkpackFiletr();
                }
            }
        }
        //properties for sync by sytstems 
        private string btnSyncTypeTxt { get; set; }
        public string BtnSyncTypeTxt
        {
            get { return btnSyncTypeTxt; }
            set { btnSyncTypeTxt = value; RaisePropertyChanged(); }

        }
        private bool isVisibleCheckReview { get; set; }
        public bool IsVisibleCheckReview
        {
            get { return isVisibleCheckReview; }
            set { isVisibleCheckReview = value; RaisePropertyChanged(); }

        }
        private bool isVisibleBtnSyncType { get; set; }
        public bool IsVisibleBtnSyncType
        {
            get { return isVisibleBtnSyncType; }
            set { isVisibleBtnSyncType = value; RaisePropertyChanged(); }

        }
        private bool isCheckedSyncITRs { get; set; }
        public bool IsCheckedSyncITRs
        {
            get { return isCheckedSyncITRs; }
            set { isCheckedSyncITRs = value; RaisePropertyChanged(); }

        }
        private bool isCheckedSyncPunchList { get; set; }
        public bool IsCheckedSyncPunchList
        {
            get { return isCheckedSyncPunchList; }
            set { isCheckedSyncPunchList = value; RaisePropertyChanged(); }

        }
        private bool isCheckedSyncDrawings { get; set; }
        public bool IsCheckedSyncDrawings
        {
            get { return isCheckedSyncDrawings; }
            set { isCheckedSyncDrawings = value; RaisePropertyChanged(); }

        }
        private bool isCheckedSyncJoints { get; set; }
        public bool IsCheckedSyncJoints
        {
            get { return isCheckedSyncJoints; }
            set { isCheckedSyncJoints = value; RaisePropertyChanged(); }

        }
        private bool isCheckedSyncJointPunchList { get; set; }
        public bool IsCheckedSyncJointPunchList
        {
            get { return isCheckedSyncJointPunchList; }
            set { isCheckedSyncJointPunchList = value; RaisePropertyChanged(); }

        }
        private bool isCheckedSyncAllUSers { get; set; }
        public bool IsCheckedSyncAllUSers
        {
            get { return isCheckedSyncAllUSers; }
            set { isCheckedSyncAllUSers = value; RaisePropertyChanged(); }

        }


        private float _progressValue;
        public float ProgressITRs
        {
            get { return _progressValue; }
            set { SetProperty(ref _progressValue, value); }
        }
        private float _ProgressPunchList;
        public float ProgressPunchList
        {
            get { return _ProgressPunchList; }
            set { SetProperty(ref _ProgressPunchList, value); }
        }
        private float _ProgressDrawing;
        public float ProgressDrawing
        {
            get { return _ProgressDrawing; }
            set { SetProperty(ref _ProgressDrawing, value); }
        }
        private float _ProgressJoints;
        public float ProgressJoints
        {
            get { return _ProgressJoints; }
            set { SetProperty(ref _ProgressJoints, value); }
        }
        private float _ProgressJointPunchList;
        public float ProgressJointPunchList
        {
            get { return _ProgressJointPunchList; }
            set { SetProperty(ref _ProgressJointPunchList, value); }
        }

        private float _ProgressUserAccount;
        public float ProgressUserAccount
        {
            get { return _ProgressUserAccount; }
            set { SetProperty(ref _ProgressUserAccount, value); }
        }
        private bool isSyncCheckBoxEnable { get; set; }
        public bool IsSyncCheckBoxEnable
        {
            get { return isSyncCheckBoxEnable; }
            set { isSyncCheckBoxEnable = value; RaisePropertyChanged(); }

        }
        private bool _IsLoadingData { get; set; }
        public bool IsLoadingData
        {
            get { return _IsLoadingData; }
            set { _IsLoadingData = value; RaisePropertyChanged(); }

        }

        #endregion

        #region Delegate Commands   
        public ICommand ClickCommand
        {
            get
            {
                return new Command<string>(OnClickAsync);
            }
        }

        public void OnClickWorkPackAdd(dynamic obj)
        {
            if (!ItemSourceAddedSyncList.Contains(obj))
                ItemSourceAddedSyncList.Add(obj);
            //  throw new NotImplementedException();
        }
        #endregion
        public SyncViewModel(INavigationService _navigationService,
           IUserDialogs _userDialogs,
           IHttpHelper _httpHelper,
           IDrawingsSyncer _IDrawingsSyncer,
           IHandoverSyncer _IHandoverSyncer,
           ITRService _ITRService,
           ISystemPunchListSyncer _ISystemPunchListSyncer,
           ISystemsFullSystemSyncercs _ISystemsFullSystemSyncercs,
           ITestPackSyncer _ITestPackSyncer,
           IWorkpackFullSystemSyncer _IWorkpackFullSystemSyncer,
           IWorkpackPunchListSyncer _IWorkpackPunchListSyncer,

           ITRSyncer _ITRSyncer,

           IFtpService _IFtpService,
           IRepository<T_Handover> _CompletionsHandoverRepository,
           IRepository<T_HandoverWorkpacks> _HandoverWorkpackRepository,

            IRepository<T_DataRefs> _DataRefsRepository,
            IRepository<T_WorkPack> _WorkPackRepository,
            IRepository<T_TAG> _TAGRepository,
            IRepository<T_Tag_headers> _Tag_headersRepository,
            IRepository<T_CHECKSHEET> _CHECKSHEETRepository,
            IRepository<T_CHECKSHEET_PER_TAG> _CHECKSHEET_PER_TAGRepository,
            IRepository<T_CHECKSHEET_QUESTION> _CHECKSHEET_QUESTIONRepository,
            IRepository<T_CompletionsPunchList> _CompletionsPunchListRepository,
            IRepository<T_TAG_SHEET_ANSWER> _TAG_SHEET_ANSWERRepository,
            IRepository<T_SignOffHeader> _SignOffHeaderRepository,
            IRepository<T_UserControl> _UserControlRepository,
            IRepository<T_TAG_SHEET_HEADER> _TAG_SHEET_HEADERRepository,
            IRepository<T_ITRCommonHeaderFooter> _CommonHeaderFooterRepository,
            IRepository<T_ITRRecords_30A_31A> _RecordsRepository,
            IRepository<T_ITRTubeColors> _TubeColorsRepository,
            IRepository<T_CommonHeaderFooterSignOff> _CommonHeaderFooterSignOffRepository, 
            IRepository<T_ITRRecords_040A_041A_042A> _Records_04XARepository,
            IRepository<T_ITRInsulationDetails> _InsulationDetailsRepository,
            IRepository<T_ITRRecords_8100_002A> _ITRRecords_8100_002ARepository,
            IRepository<T_ITRRecords_8100_002A_InsRes_Test> _ITRRecords_8100_002A_InsRes_TestRepository,
            IRepository<T_ITRRecords_8100_002A_Radio_Test> _ITRRecords_8100_002A_Radio_TestRepository,
            IRepository<T_ITRRecords_8161_001A_Body> _ITRRecords_8161_001A_BodyRepository,
            IRepository<T_ITRRecords_8161_001A_InsRes> _ITRRecords_8161_001A_InsResRepository,
            IRepository<T_ITRRecords_8161_001A_ConRes> _ITRRecords_8161_001A_ConResRepository,
            IRepository<T_ITR8121_004AInCAndAuxiliary> _ITR8121_004AInCAndAuxiliaryRepository,
            IRepository<T_ITR8140_001A_ContactResisTest> _T_ITR8140_001A_ContactResisTestRepository,
            IRepository<T_ITR8140_001AInsulationResistanceTest> _T_ITR8140_001AInsulationResistanceTestRepository,
            IRepository<T_ITR8140_001ADialectricTest> _T_ITR8140_001ADialectricTestRepository,
            IRepository<T_ITR8140_001ATestInstrucitonData> _T_ITR8140_001ATestInstrumentDataRepository,
            IRepository<T_ITR8121_004ATransformerRatioTest> _ITR8121_004ATransformerRatioTestRepository,
            IRepository<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents> _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents,
            IRepository<T_ITR8121_002A_Records> _ITR8121_002A_Records,
            IRepository<T_ITR8121_002A_TransformerRadioTest> _ITR8121_002A_TransformerRadioTest,
            IRepository<T_ITR8121_004ATestInstrumentData> _ITR8121_004ATestInstrumentDataRepository,
            IRepository<T_ITR_8260_002A_Body> _ITR_8260_002A_BodyRepository,
            IRepository<T_ITR8000_003ARecords> _Records_8000003ARepository,
            IRepository<T_ITRRecords_080A_090A_091A> _Records_080A_09XARepository,
            IRepository<T_ITR8100_001A_CTdata> _ITR8100_001A_CTdataRepository,
            IRepository<T_ITR8100_001A_InsulationResistanceTest> _ITR8100_001A_IRTestRepository,
            IRepository<T_ITR8000_003A_AcceptanceCriteria> _Records_8000003A_AcceptanceCriteriaRepository,
            IRepository<T_ITR8100_001A_RatioTest> _ITR8100_001A_RatioTestRepository,
            IRepository<T_ITR8100_001A_TestInstrumentData> _ITR8100_001A_TIDataRepository,
            IRepository<T_ITR_8260_002A_DielectricTest> _ITR_8260_002A_DielectricTestRepository,
            IRepository<T_TestEquipmentData> _TestEquipmentDataRepository,
            IRepository<T_ITRInstrumentData> _ITRInstrumentDataRepository,
            IRepository<T_ITR8161_002A_Body> _ITR8161_002A_BodyRepository,
            IRepository<T_ITR8161_002A_DielectricTest> _ITR8161_002A_DielectricTestRepository,
            IRepository<T_ITR8000_101A_Generalnformation> _ITR8000_101A_GeneralnformationRepository,
            IRepository<T_ITR8000_101A_RecordISBarrierDetails> _ITR8000_101A_RecordISBarrierDetailsRepository,
            IRepository<T_ITRRecords_8211_002A_Body> _ITRRecords_8211_002A_BodyRepository,
            IRepository<T_ITRRecords_8211_002A_RunTest> _ITRRecords_8211_002A_RunTestRepository,
            IRepository<T_ITR_7000_101AHarmony_Genlnfo> _ITR7000_101AHarmony_GenlnfoRepository,
            IRepository<T_ITR_7000_101AHarmony_ActivityDetails> _ITR7000_101AHarmony_ActivityDetailsRepository,
            IRepository<T_ITR_8140_002A_Records> _ITR_8140_002A_RecordsRepository,
            IRepository<T_ITR_8140_002A_RecordsMechnicalOperation> _ITR_8140_002A_RecordsMechnicalOperation_RecordsRepository,
            IRepository<T_ITR_8140_002A_RecordsAnalogSignal> _ITR_8140_002A_RecordsAnalogSignalRepository,
            IRepository<T_ITR_8140_004A_Records> _ITR_8140_004A_RecordsRepository,
            IRepository<T_ITRRecords_8170_002A_Voltage_Reading> _ITRRecords_8170_002A_Voltage_ReadingRepository,
            IRepository<T_ITR_8170_002A_IndifictionLamp> _ITR_8170_002A_IndifictionLampRepository,
            IRepository<T_ITR_8170_002A_InsRes> _ITR_8170_002A_InsResRepository,
            IRepository<T_ITR_8300_003A_Body> _ITR_8300_003A_BodyRepository,
            IRepository<T_ITR_8300_003A_Illumin> _ITR_8300_003A_IlluminRepository,
            IRepository<T_ITR_8170_007A_OP_Function_Test> _ITR_8170_007A_OP_Function_TestRepository,
           ICheckValidLogin _checkValidLogin) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._CompletionsHandoverRepository = _CompletionsHandoverRepository;
            this._HandoverWorkpackRepository = _HandoverWorkpackRepository;
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._IDrawingsSyncer = _IDrawingsSyncer;
            this._IHandoverSyncer = _IHandoverSyncer;
            this._ISystemPunchListSyncer = _ISystemPunchListSyncer;
            this._ISystemsFullSystemSyncercs = _ISystemsFullSystemSyncercs;
            this._ITestPackSyncer = _ITestPackSyncer;
            this._IWorkpackFullSystemSyncer = _IWorkpackFullSystemSyncer;
            this._IWorkpackPunchListSyncer = _IWorkpackPunchListSyncer;
            this._IFtpService = _IFtpService;
            this._ITRSyncer = _ITRSyncer;
            this._ITRService = _ITRService;
            this._DataRefsRepository = _DataRefsRepository;
            this._WorkPackRepository = _WorkPackRepository;
            this._TAGRepository = _TAGRepository;
            this._Tag_headersRepository = _Tag_headersRepository;
            this._CHECKSHEETRepository = _CHECKSHEETRepository;
            this._CHECKSHEET_PER_TAGRepository = _CHECKSHEET_PER_TAGRepository;
            this._CHECKSHEET_QUESTIONRepository = _CHECKSHEET_QUESTIONRepository;
            this._CompletionsPunchListRepository = _CompletionsPunchListRepository;
            this._TAG_SHEET_ANSWERRepository = _TAG_SHEET_ANSWERRepository;
            this._SignOffHeaderRepository = _SignOffHeaderRepository;
            this._TAG_SHEET_HEADERRepository = _TAG_SHEET_HEADERRepository;
            this._UserControlRepository = _UserControlRepository;
            this._CommonHeaderFooterRepository = _CommonHeaderFooterRepository;
            this._RecordsRepository = _RecordsRepository;
            this._TubeColorsRepository = _TubeColorsRepository;
            this._Records_04XARepository = _Records_04XARepository;
            this._InsulationDetailsRepository = _InsulationDetailsRepository;
            this._CommonHeaderFooterSignOffRepository = _CommonHeaderFooterSignOffRepository;
            this._ITRRecords_8100_002ARepository = _ITRRecords_8100_002ARepository;
            this._ITRRecords_8100_002A_InsRes_TestRepository = _ITRRecords_8100_002A_InsRes_TestRepository;
            this._ITRRecords_8100_002A_Radio_TestRepository = _ITRRecords_8100_002A_Radio_TestRepository;
            this._ITRRecords_8161_001A_BodyRepository = _ITRRecords_8161_001A_BodyRepository;
            this._ITRRecords_8161_001A_InsResRepository = _ITRRecords_8161_001A_InsResRepository;
            this._ITRRecords_8161_001A_ConResRepository = _ITRRecords_8161_001A_ConResRepository;

            this._ITR_8260_002A_BodyRepository = _ITR_8260_002A_BodyRepository;
            this._ITR_8260_002A_DielectricTestRepository = _ITR_8260_002A_DielectricTestRepository;
            this._ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents = _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents;
            this._ITR8121_002A_Records = _ITR8121_002A_Records;
            this._ITR8121_002A_TransformerRadioTest = _ITR8121_002A_TransformerRadioTest;
            this._T_ITR8140_001A_ContactResisTestRepository = _T_ITR8140_001A_ContactResisTestRepository;
            this._T_ITR8140_001ADialectricTestRepository = _T_ITR8140_001ADialectricTestRepository;
            this._T_ITR8140_001ATestInstrumentDataRepository = _T_ITR8140_001ATestInstrumentDataRepository;
            this._T_ITR8140_001AInsulationResistanceTestRepository = _T_ITR8140_001AInsulationResistanceTestRepository;
            this._ITR8121_004AInCAndAuxiliaryRepository = _ITR8121_004AInCAndAuxiliaryRepository;
            this._ITR8121_004ATransformerRatioTestRepository = _ITR8121_004ATransformerRatioTestRepository;
            this._ITR8121_004ATestInstrumentDataRepository = _ITR8121_004ATestInstrumentDataRepository;

            this._Records_8000003ARepository = _Records_8000003ARepository;
            this._Records_8000003A_AcceptanceCriteriaRepository = _Records_8000003A_AcceptanceCriteriaRepository;
            this._Records_080A_09XARepository = _Records_080A_09XARepository;
            this._ITR8100_001A_CTdataRepository = _ITR8100_001A_CTdataRepository;
            this._ITR8100_001A_IRTestRepository = _ITR8100_001A_IRTestRepository;
            this._ITR8100_001A_RatioTestRepository = _ITR8100_001A_RatioTestRepository;
            this._ITR8100_001A_TIDataRepository = _ITR8100_001A_TIDataRepository;
            this._TestEquipmentDataRepository = _TestEquipmentDataRepository;
            this._ITRInstrumentDataRepository = _ITRInstrumentDataRepository;
            this._ITR8161_002A_BodyRepository = _ITR8161_002A_BodyRepository;
            this._ITR8161_002A_DielectricTestRepository = _ITR8161_002A_DielectricTestRepository;
            this._ITR8000_101A_GeneralnformationRepository = _ITR8000_101A_GeneralnformationRepository;
            this._ITR8000_101A_RecordISBarrierDetailsRepository = _ITR8000_101A_RecordISBarrierDetailsRepository;
            this._ITRRecords_8211_002A_BodyRepository = _ITRRecords_8211_002A_BodyRepository;
            this._ITRRecords_8211_002A_RunTestRepository = _ITRRecords_8211_002A_RunTestRepository;
            this._ITR7000_101AHarmony_GenlnfoRepository = _ITR7000_101AHarmony_GenlnfoRepository;
            this._ITR7000_101AHarmony_ActivityDetailsRepository = _ITR7000_101AHarmony_ActivityDetailsRepository;
            this._ITR_8140_002A_RecordsRepository = _ITR_8140_002A_RecordsRepository;
            this._ITR_8140_002A_RecordsMechnicalOperation_RecordsRepository = _ITR_8140_002A_RecordsMechnicalOperation_RecordsRepository;
            this._ITR_8140_002A_RecordsAnalogSignalRepository = _ITR_8140_002A_RecordsAnalogSignalRepository;
            this._ITR_8140_004A_RecordsRepository = _ITR_8140_004A_RecordsRepository;
            this._ITRRecords_8170_002A_Voltage_ReadingRepository = _ITRRecords_8170_002A_Voltage_ReadingRepository;
            this._ITR_8170_002A_IndifictionLampRepository = _ITR_8170_002A_IndifictionLampRepository;
            this._ITR_8170_002A_InsResRepository = _ITR_8170_002A_InsResRepository;
            this._ITR_8300_003A_BodyRepository = _ITR_8300_003A_BodyRepository;
            this._ITR_8300_003A_IlluminRepository = _ITR_8300_003A_IlluminRepository;
            this._ITR_8170_007A_OP_Function_TestRepository = _ITR_8170_007A_OP_Function_TestRepository;

            IsVisibleSyncPopup = true;
            ItemSourceAddedSyncList = new ObservableCollection<dynamic>();
            BtnSyncTypeTxt = "Press to sync by System";
            PlaceHolderText = "Filter System List";
            SyncBy = "SyncbySystem";
            IsVisibleCheckReview = IsVisibleBtnSyncType = IsVisibleCheckReview = true;
            IsCheckedSyncPunchList = IsCheckedSyncDrawings = IsCheckedSyncITRs =
            IsCheckedSyncJoints = IsCheckedSyncJointPunchList = IsCheckedSyncAllUSers = false;
            IsVisibleSyncButton = true;
            SyncCancelButtonText = "Close";
            IsEnableCancelButton = true;
            CheckSyncAllUsers = true;
            IsVisibleTestPacksSyncProgressPopup = false;

            if (Settings.CurrentDB == "JGC" || Settings.CurrentDB == "JGC_HARMONY" || Settings.CurrentDB == "JGC_ITR" || Settings.CurrentDB == "JGC_DEMO"|| 
                Settings.CurrentDB == "ROVUMA_TEST" || Settings.CurrentDB == "YOC_DEMO" || Settings.CurrentDB == "JGC_HARMONYCOMP")
                IsVisibleWBSBtnSyncType = true;
            //TxtFilter = " ";
            SyncProgressLogs.UpdateLogs.PunchListLogs = "Sync PunchList";
            SyncProgressLogs.UpdateLogs.ItrLogs = "Sync ITR's";

            _userDialogs.HideLoading();
        }

        private async void OnClickAsync(string param)
        {
            App.IsBusy = false;
            IsSyncCheckBoxEnable = true;
            if (param == "close")
                PopupNavigation.PopAsync(true);

            else if (param == "PunchList")
            {
                GetInitialData();
                if (Settings.CurrentDB == "JGC" || Settings.CurrentDB == "JGC_HARMONY" || Settings.CurrentDB == "JGC_ITR" || Settings.CurrentDB == "JGC_DEMO"|| 
                    Settings.CurrentDB == "ROVUMA_TEST" || Settings.CurrentDB == "YOC_DEMO" || Settings.CurrentDB == "JGC_HARMONYCOMP")
                    OnClickAsync("SyncbyPCWBS");
                else
                    OnClickAsync("SyncbyWorkPack");
                //{
                //    PlaceHolderText = "Filter WorkPack List";
                //    RunnableSyncList = ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(SyncInitialList.workpacks);

                //    if (ItemSourceRunnableSyncList.Count <= 0)
                //    {
                //        DependencyService.Get<IToastMessage>().ShortAlert("No WorkPack found. Please try again");
                //        OnClickAsync("SyncbySystem");
                //    }
                //}

            }
            else if (param == "SelectAll")
            {
                if (ItemSourceRunnableSyncList != null)
                {
                    ItemSourceAddedSyncList = ItemSourceRunnableSyncList;
                }
            }
            else if (param == "RemoveAll")
            {
                ItemSourceAddedSyncList = new ObservableCollection<dynamic>();
            }
            else if (param == "SyncbySystem")
            {
                SyncData = "Tags";
                ItemSourceAddedSyncList.Clear();
                PlaceHolderText = "Filter System List";
                BtnSyncTypeTxt = "Press to sync by Work Pack";
                SyncBy = "SyncbyWorkPack";
                IsVisibleCheckReview = false;
                //GetSystems();
                RunnableSyncList = ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(SyncInitialList.systems.Where(x=>!x.name.EndsWith(".") && !x.name.ToLower().StartsWith("sub system")).GroupBy(x => x.name).Select(grp => grp.FirstOrDefault()));

                if (ItemSourceRunnableSyncList.Any() && !String.IsNullOrEmpty(TxtFilter))
                    ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(ItemSourceRunnableSyncList.Where(s => s.name.ToLower().Contains(TxtFilter.ToLower())));

            }
            else if (param == "SyncbyWorkPack")
            {
                SyncData = "Tags";
                ItemSourceAddedSyncList.Clear();
                PlaceHolderText = "Filter WorkPack List";
                BtnSyncTypeTxt = "Press to sync by System";
                SyncBy = "SyncbySystem";
                IsVisibleCheckReview = true;
                //GetWorkPaks();
                RunnableSyncList = ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(SyncInitialList.workpacks);
                //if (ItemSourceRunnableSyncList.Count <= 0)
                //{
                //    DependencyService.Get<IToastMessage>().ShortAlert("No WorkPack found. Please try again");
                //    OnClickAsync("SyncbySystem");
                //}

                if (ItemSourceRunnableSyncList.Any() && !String.IsNullOrEmpty(TxtFilter))
                    ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(ItemSourceRunnableSyncList.Where(x => x.name.ToLower().Contains(TxtFilter.ToLower())));
            }
            else if (param == "SyncbyPCWBS")
            {
                SyncData = "Tags";
                List<PCWBSModel> PCWBS = new List<PCWBSModel>();
                ItemSourceAddedSyncList.Clear();
                foreach (string pcwbs in SyncInitialList.pcwbs)
                    PCWBS.Add(new PCWBSModel { pcwbs = pcwbs });
                PlaceHolderText = "Filter PCWBS List";
                RunnableSyncList = ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(PCWBS);

                if (ItemSourceRunnableSyncList.Any() && !String.IsNullOrEmpty(TxtFilter))
                    ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(ItemSourceRunnableSyncList.Where(x => x.name.ToLower().Contains(TxtFilter.ToLower())));
            }
            else if (param == "SyncbyFWBS")
            {
                SyncData = "Tags";
                List<FWBSModel> FWBS = new List<FWBSModel>();
                ItemSourceAddedSyncList.Clear();
                foreach (string fwbs in SyncInitialList.fwbs)
                    FWBS.Add(new FWBSModel { fwbs = fwbs });
                PlaceHolderText = "Filter FWBS List";
                RunnableSyncList = ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(FWBS);

                if (ItemSourceRunnableSyncList.Any() && !String.IsNullOrEmpty(TxtFilter))
                    ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(ItemSourceRunnableSyncList.Where(x => x.name.ToLower().Contains(TxtFilter.ToLower())));
            }
            else if (param == "Handovers")
            {
                SyncData = "Handovers";
                IsVisibleCheckReview = false;
                IsVisibleBtnSyncType = IsVisibleWBSBtnSyncType = false;
                GetHandovers();
            }
            else if (param == "ShowSyncPopup")
            {
                if (ItemSourceAddedSyncList.Count > 0)
                {
                    IsVisibleTagPopup = false;
                    TxtFilter = "";
                    if (SyncData == "Tags") //IsVisibleSyncProgressPopup = true;
                    {
                        GetTags();
                    }
                    else if (SyncData == "Handovers") IsVisibleHandoverSyncProgressPopup = true;
                    else if (SyncData == "TestPacks") IsVisibleTestPacksSyncProgressPopup = true;
                    else if (SyncData == "ITR") IsVisibleITRSyncProgressPopup = true;
                    else if (SyncData == "SyncTags") IsVisibleSyncProgressPopup = true;
                }
                else
                    _userDialogs.AlertAsync("Please select an " + SyncData + "(s) to sync", "Alert", "OK");


            }
            else if (param == "SyncClicked")
            {
                IsSyncCheckBoxEnable = false;
                if (SyncData == "TestPacks") StartTestPackSyncerAsync();
                else if (SyncData == "ITR") StartITRSyncerAsync();
                else if (SyncData == "SyncTags") Start_SaveSyncerAsync();
                else StartSyncerAsync();
            }
            else if (param == "HandoverSyncClicked")
            {
                StartHandoverSyncerAsync();
            }
            else if (param == "TestPacks")
            {
                SyncData = "TestPacks";
                GetTestPacks();
            }
            else if (param == "ITR")
            {
                Thread tid1 = new Thread(new ThreadStart(UpdateUI));
                Thread tid2 = new Thread(new ThreadStart(GetITR));

                tid1.Start();
                tid2.Start();

                //Task task1 = Task.Factory.StartNew(() => UpdateUI("Task1"));
                //Task task2 = Task.Factory.StartNew(() => GetITR());

                //Task.WaitAll(task1, task2);
                //_userDialogs.Loading();
                SyncData = "ITR";
                IsVisibleCheckReview = false;
                IsVisibleBtnSyncType = false;

                // GetITR();
            }
            else if (param == "FilterByTag")
            {
                if (CompletionsSyncTAgs == null)
                    return;

                RunnableSyncList = ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(CompletionsSyncTAgs.SelectMany(list => list.tags).Distinct().ToList());
                if (!String.IsNullOrEmpty(TxtFilter))
                    RunnableSyncList = ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(ItemSourceRunnableSyncList.Where(x => x.name.ToLower().Contains(TxtFilter.ToLower())));

            }
        }

        private void PCWBS(PCWBSModel pcwbs)
        {
            throw new NotImplementedException();
        }

        public void UpdateUI()
        {
            SyncData = "ITR";
            IsVisibleCheckReview = false;
            IsVisibleBtnSyncType = false;
           // _userDialogs.ShowLoading("Getting ITR's.");

        }

        private async void StartSyncerAsync()
        {
            SyncCancelButtonText = "Syncing";
            IsVisibleSyncButton = false;
            IsEnableCancelButton = false;
            float perCount = (float)1 / (float)ItemSourceAddedSyncList.Count;
            //  ProgressITRs = 0.1f;
            //workpack sync
            if (IsCheckedSyncITRs)
            {
                var GetType = RunnableSyncList.FirstOrDefault().GetType();
                int i = 0;
                 if (GetType == typeof(WorkpackModel))
                {
                    try
                    {
                        SyncProgressLogs.UpdateLogs.ItrLogs = "Sync Started";
                         SyncProgressLogs.UpdateLogs.ItrLogs = "Uploading Changes ";
                            _ = await _IWorkpackFullSystemSyncer.uploadChanges();
                            // ProgressITRs += perCount;
                            ProgressITRs = 0.1f;
                       
                        await _DataRefsRepository.DeleteAll();
                        //await _WorkPackRepository.DeleteAll();
                        await _TAGRepository.DeleteAll();
                        await _Tag_headersRepository.DeleteAll();
                        await _CHECKSHEETRepository.DeleteAll();
                        await _CHECKSHEET_PER_TAGRepository.DeleteAll();
                        await _CHECKSHEET_QUESTIONRepository.DeleteAll();
                        await _TAG_SHEET_ANSWERRepository.DeleteAll();
                        await _SignOffHeaderRepository.DeleteAll();
                        await _TAG_SHEET_HEADERRepository.DeleteAll();
                       
                        foreach (WorkpackModel workpack in ItemSourceAddedSyncList)
                        {
                            SyncProgressLogs.UpdateLogs.ItrLogs = "Downloading Workpack Info " + i + " of " + ItemSourceAddedSyncList.Count + " Complete.";
                            _ = await _IWorkpackFullSystemSyncer.downloadChanges(workpack.name);
                            ProgressITRs += perCount;
                            i++;
                        }
                        SyncProgressLogs.UpdateLogs.ItrLogs = "ITRs: Sync Complete";
                        // ProgressITRs = 1;
                        // Get PunchList
                    }
                    catch (Exception Ex)
                    {
                    }
                }
                else if (GetType == typeof(SystemModel) || GetType == typeof(PCWBSModel) || GetType == typeof(FWBSModel))
                {
                    try
                    {
                        await _DataRefsRepository.DeleteAll();
                        //await _WorkPackRepository.DeleteAll();
                        await _TAGRepository.DeleteAll();
                        await _Tag_headersRepository.DeleteAll();
                        await _CHECKSHEETRepository.DeleteAll();
                        await _CHECKSHEET_PER_TAGRepository.DeleteAll();
                        await _CHECKSHEET_QUESTIONRepository.DeleteAll();
                        await _TAG_SHEET_ANSWERRepository.DeleteAll();
                        await _SignOffHeaderRepository.DeleteAll();
                        await _TAG_SHEET_HEADERRepository.DeleteAll();

                        //foreach (SystemModel systems in ItemSourceAddedSyncList)
                        //{
                        //    SyncProgressLogs.UpdateLogs.ItrLogs = "Downloading System Info " + i + " of " + ItemSourceAddedSyncList.Count + " Complete.";
                        //    ProgressITRs = perCount / 2;
                        //    _ = await _ISystemsFullSystemSyncercs.downloadChanges(systems.ref1);
                        //    ProgressITRs += perCount;
                        //    i++;
                        //}
                        var TagsID = ItemSourceAddedSyncList.ToList().Select(x => x.name).ToArray();
                        string postJSON = JsonConvert.SerializeObject(TagsID);
                        string SyncType = string.Empty;

                        if (GetType == typeof(SystemModel))       SyncType = "SYSTEM";                      
                        else if (GetType == typeof(PCWBSModel))   SyncType = "PCWBS";
                        else if (GetType == typeof(FWBSModel))    SyncType = "FWBS";

                        _ = await _ISystemsFullSystemSyncercs.downloadChanges(postJSON, SyncType); 
                        
                        SyncProgressLogs.UpdateLogs.ItrLogs = "ITRs: Sync Complete";
                        ProgressITRs = 1;
                    }
                    catch (Exception Ex)
                    {
                    }
                }
            }

            if (IsCheckedSyncPunchList)
            {
                int i = 0;
                var GetType = RunnableSyncList.FirstOrDefault().GetType();
                if (GetType == typeof(WorkpackModel))
                {
                    await _CompletionsPunchListRepository.DeleteAll();
                    SyncProgressLogs.UpdateLogs.PunchListLogs = "Downloading";
                    foreach (WorkpackModel workpack in ItemSourceAddedSyncList)
                    {

                        SyncProgressLogs.UpdateLogs.PunchListLogs = "Downloading punch list " + i + " of " + ItemSourceAddedSyncList.Count + " Complete";
                        var result = await _IWorkpackPunchListSyncer.downloadChanges(workpack.name);
                        ProgressPunchList += perCount;
                        i++;
                    }
                    SyncProgressLogs.UpdateLogs.PunchListLogs = "Punch Lists: Sync Complete";
                }
                else if (GetType == typeof(SystemModel) || GetType == typeof(PCWBSModel) || GetType == typeof(FWBSModel))
                {
                    string SyncType = string.Empty;

                    if (GetType == typeof(SystemModel)) SyncType = "SYSTEM";
                    else if (GetType == typeof(PCWBSModel)) SyncType = "PCWBS";
                    else if (GetType == typeof(FWBSModel)) SyncType = "FWBS";
                    SyncProgressLogs.UpdateLogs.PunchListLogs = "Uploading Changes";
                    var Uploadresult = await _ISystemPunchListSyncer.uploadChanges(SyncType);
                    await _CompletionsPunchListRepository.DeleteAll();
                    SyncProgressLogs.UpdateLogs.PunchListLogs = "DownLoading Punch list";
                    var Downloadresult = await _ISystemPunchListSyncer.downloadChanges(SyncType);
                    ProgressPunchList += perCount;
                }
            }

            if (IsCheckedSyncDrawings)
            {
                try
                {
                    var result = await _IDrawingsSyncer.downloadChanges("Drawings");
                    ProgressDrawing += perCount;
                    ProgressJointPunchList = 1;
                    ProgressJoints = 1;
                }
                catch (Exception Ex)
                { }
                ProgressDrawing = 1;
            }
            if (IsCheckedSyncAllUSers)
            {
                try 
                { 
                string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.GetAllUsersForProject(Settings.ProjectName, Settings.CurrentDB), Settings.CompletionAccessToken);
                var allusers = JsonConvert.DeserializeObject<List<T_UserControl>>(JsonString);

                if (allusers != null)
                    await _UserControlRepository.InsertOrReplaceAsync(allusers);
                ProgressUserAccount += perCount;
                }
                catch (Exception Ex)
            { }
            ProgressUserAccount = 1;
            }

            IsEnableCancelButton = true;
            SyncCancelButtonText = "Close";
        }
        private async void Start_SaveSyncerAsync()
        {
            SyncCancelButtonText = "Syncing";
            IsVisibleSyncButton = false;
            IsEnableCancelButton = false;
            //float perCount = (float)1 / (float)ItemSourceAddedSyncList.Count;
            float perCount = (float)1 / (float)RunnableSyncList.Count;
            //  ProgressITRs = 0.1f;
            //workpack sync
            if (IsCheckedSyncITRs)
            {
                var GetType = RunnableSyncList.FirstOrDefault().GetType();
                int i = 0;
                if (GetType == typeof(WorkpackModel))
                {
                    try
                    {
                        SyncProgressLogs.UpdateLogs.ItrLogs = "Sync Started";
                        SyncProgressLogs.UpdateLogs.ItrLogs = "Uploading Changes ";
                            _ = await _IWorkpackFullSystemSyncer.uploadChanges();
                            // ProgressITRs += perCount;
                            ProgressITRs = 0.1f;
                       
                        await _DataRefsRepository.DeleteAll();
                        //await _WorkPackRepository.DeleteAll();
                        await _TAGRepository.DeleteAll();
                        await _Tag_headersRepository.DeleteAll();
                        await _CHECKSHEETRepository.DeleteAll();
                        await _CHECKSHEET_PER_TAGRepository.DeleteAll();
                        await _CHECKSHEET_QUESTIONRepository.DeleteAll();
                        await _TAG_SHEET_ANSWERRepository.DeleteAll();
                        await _SignOffHeaderRepository.DeleteAll();
                        await _TAG_SHEET_HEADERRepository.DeleteAll();


                        foreach (WorkpackModel workpack in ItemSourceAddedSyncList)
                        {
                           SyncProgressLogs.UpdateLogs.ItrLogs = "Downloading Workpack Info " + i + " of " + ItemSourceAddedSyncList.Count + " Complete.";
                            _ = await _IWorkpackFullSystemSyncer.downloadChanges(workpack.name);
                            ProgressITRs += perCount;
                            i++;
                        }
                        SyncProgressLogs.UpdateLogs.ItrLogs = "ITRs: Sync Complete";
                        ProgressITRs = 1;
                        // ProgressITRs = 1;
                        // Get PunchList
                    }
                    catch (Exception Ex)
                    {
                    }
                }
                else if (GetType == typeof(T_TAG))
                {
                    try
                    {
                        SyncProgressLogs.UpdateLogs.ItrLogs = "Sync Started";

                       
                         SyncProgressLogs.UpdateLogs.ItrLogs = "Uploading Changes ";
                       _ = await _ISystemsFullSystemSyncercs.uploadChanges();
                            // ProgressITRs += perCount;
                          ProgressITRs = 0.1f;
                       

                        await _DataRefsRepository.DeleteAll();
                        await _TAGRepository.DeleteAll();
                        await _Tag_headersRepository.DeleteAll();
                        await _CHECKSHEETRepository.DeleteAll();
                        await _CHECKSHEET_PER_TAGRepository.DeleteAll();
                        await _CHECKSHEET_QUESTIONRepository.DeleteAll();
                        await _TAG_SHEET_ANSWERRepository.DeleteAll();
                        await _SignOffHeaderRepository.DeleteAll();
                        await _TAG_SHEET_HEADERRepository.DeleteAll();
                        await _CommonHeaderFooterRepository.DeleteAll();
                        await _RecordsRepository.DeleteAll();
                        await _TubeColorsRepository.DeleteAll();
                        await _Records_04XARepository.DeleteAll();
                        await _InsulationDetailsRepository.DeleteAll();
                        await _CommonHeaderFooterSignOffRepository.DeleteAll();
                        await _ITRRecords_8100_002ARepository.DeleteAll();
                        await _ITRRecords_8100_002A_InsRes_TestRepository.DeleteAll();
                        await _ITRRecords_8100_002A_Radio_TestRepository.DeleteAll();
                        await _ITRRecords_8161_001A_BodyRepository.DeleteAll();
                        await _ITRRecords_8161_001A_InsResRepository.DeleteAll();
                        await _ITRRecords_8161_001A_ConResRepository.DeleteAll();
                        await _ITR8121_004AInCAndAuxiliaryRepository.DeleteAll();
                        await _ITR8121_004ATransformerRatioTestRepository.DeleteAll();
                        await _ITR8121_004ATestInstrumentDataRepository.DeleteAll();

                        await _T_ITR8140_001A_ContactResisTestRepository.DeleteAll();
                        await _T_ITR8140_001ADialectricTestRepository.DeleteAll();
                        await _T_ITR8140_001ATestInstrumentDataRepository.DeleteAll();
                        await _T_ITR8140_001AInsulationResistanceTestRepository.DeleteAll();

                        await _Records_8000003ARepository.DeleteAll();
                        await _Records_8000003A_AcceptanceCriteriaRepository.DeleteAll();
                        await _Records_080A_09XARepository.DeleteAll();
                        await _ITR8100_001A_CTdataRepository.DeleteAll();
                        await _ITR8100_001A_IRTestRepository.DeleteAll();
                        await _ITR8100_001A_RatioTestRepository.DeleteAll();
                        await _ITR8100_001A_TIDataRepository.DeleteAll();
                        await _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents.DeleteAll();
                        await _ITR8121_002A_Records.DeleteAll();
                        await _ITR8121_002A_TransformerRadioTest.DeleteAll();
                        await _ITR_8260_002A_BodyRepository.DeleteAll();
                        await _ITR_8260_002A_DielectricTestRepository.DeleteAll();
                        await _ITRRecords_8161_001A_BodyRepository.DeleteAll();
                        await _ITRRecords_8161_001A_InsResRepository.DeleteAll();
                        await _ITRRecords_8161_001A_ConResRepository.DeleteAll();
                        await _ITR8161_002A_BodyRepository.DeleteAll();
                        await _ITR8161_002A_DielectricTestRepository.DeleteAll();
                        await _ITR8000_101A_GeneralnformationRepository.DeleteAll();
                        await _ITR8000_101A_RecordISBarrierDetailsRepository.DeleteAll();
                       


                        await _ITRInstrumentDataRepository.DeleteAll();

                        await _ITRRecords_8211_002A_BodyRepository.DeleteAll();
                        await _ITRRecords_8211_002A_RunTestRepository.DeleteAll();
                        await _ITR7000_101AHarmony_GenlnfoRepository.DeleteAll();
                        await _ITR7000_101AHarmony_ActivityDetailsRepository.DeleteAll();
                        await _ITR_8140_002A_RecordsRepository.DeleteAll();
                        await _ITR_8140_002A_RecordsMechnicalOperation_RecordsRepository.DeleteAll();
                        await _ITR_8140_002A_RecordsAnalogSignalRepository.DeleteAll();
                        await _ITR_8140_004A_RecordsRepository.DeleteAll();
                        await _ITRRecords_8170_002A_Voltage_ReadingRepository.DeleteAll();
                        await _ITR_8170_002A_IndifictionLampRepository.DeleteAll();
                        await _ITR_8170_002A_InsResRepository.DeleteAll();
                        await  _ITR_8300_003A_BodyRepository.DeleteAll();
                        await _ITR_8300_003A_IlluminRepository.DeleteAll();
                        await _ITR_8170_007A_OP_Function_TestRepository.DeleteAll();

                        List<T_TAG> SelectedTagList = new List<T_TAG>();
                        Dictionary<string, List<string>> TagNameToSheetNameMap = new Dictionary<string, List<string>>();
                        List<Dictionary<string, List<string>>> DicList = new List<Dictionary<string, List<string>>>();
                        foreach (T_TAG dd in ItemSourceAddedSyncList)
                            SelectedTagList.Add(dd);

                        //Selected tag list for sync
                        SyncProgressLogs.UpdateLogs.ItrLogs = "Downloading";
                        ProgressITRs = 0.1f;
                        var ID_List = SelectedTagList.Select(s=>s.id);

                         GetTestEquipment();
                        // DownloadTagsData(ID_List.Take(20).ToArray());
                        _ = await _ISystemsFullSystemSyncercs.downloadChangesByID(JsonConvert.SerializeObject(ID_List.Take(20).ToArray()));
                        BG_Download_RemaingsTags(ID_List.Skip(20).ToArray());

                        SyncProgressLogs.UpdateLogs.ItrLogs = "ITRs: Sync Complete";
                        ProgressITRs = 1;

                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            if (IsCheckedSyncPunchList)
            {
                int i = 0;
                var GetType = RunnableSyncList.FirstOrDefault().GetType();
                if (GetType == typeof(WorkpackModel))
                {
                    await _CompletionsPunchListRepository.DeleteAll();
                    SyncProgressLogs.UpdateLogs.PunchListLogs = "Downloading";
                    foreach (WorkpackModel workpack in ItemSourceAddedSyncList)
                    {
                        SyncProgressLogs.UpdateLogs.PunchListLogs = "Downloading punch list " + i + " of " + ItemSourceAddedSyncList.Count + " Complete";
                        var result = await _IWorkpackPunchListSyncer.downloadChanges(workpack.name);
                        ProgressPunchList += perCount;
                        i++;
                    }
                    SyncProgressLogs.UpdateLogs.PunchListLogs = "Punch Lists: Sync Complete";
                }
                else if (GetType == typeof(SystemModel) || GetType == typeof(PCWBSModel) || GetType == typeof(FWBSModel))
                {
                    string SyncType = string.Empty;

                    if (GetType == typeof(SystemModel)) SyncType = "SYSTEM";
                    else if (GetType == typeof(PCWBSModel)) SyncType = "PCWBS";
                    else if (GetType == typeof(FWBSModel)) SyncType = "FWBS";
                    SyncProgressLogs.UpdateLogs.PunchListLogs = "Uploading Changes";
                    var Uploadresult = await _ISystemPunchListSyncer.uploadChanges(SyncType);
                    await _CompletionsPunchListRepository.DeleteAll();
                    SyncProgressLogs.UpdateLogs.PunchListLogs = "DownLoading Punch list";
                    var Downloadresult = await _ISystemPunchListSyncer.downloadChanges(SyncType);
                    ProgressPunchList += perCount;
                }
                else if (GetType == typeof(T_TAG))
                {
                    string SyncType = string.Empty;

                    SyncProgressLogs.UpdateLogs.PunchListLogs = "Uploading Changes";
                    var Uploadresult = await _ISystemPunchListSyncer.uploadChanges(SyncType);
                    await _CompletionsPunchListRepository.DeleteAll();
                    SyncProgressLogs.UpdateLogs.PunchListLogs = "DownLoading Punch list";
                    var Downloadresult = await _ISystemPunchListSyncer.downloadChanges(SyncType);
                    ProgressPunchList = 1;
                }
            }

            if (IsCheckedSyncDrawings)
            {
                try
                {
                    var result = await _IDrawingsSyncer.downloadChanges("Drawings");
                    ProgressDrawing += perCount;
                    ProgressJointPunchList = 1;
                    ProgressJoints = 1;
                }
                catch (Exception Ex)
                { }
                ProgressDrawing = 1;
            }
            if (IsCheckedSyncAllUSers)
            {

                try
                {
                    string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.GetAllUsersForProject(Settings.ProjectName, Settings.CurrentDB), Settings.CompletionAccessToken);
                var allusers = JsonConvert.DeserializeObject<List<T_UserControl>>(JsonString);



                if (allusers != null)
                    await _UserControlRepository.InsertOrReplaceAsync(allusers);
                    ProgressUserAccount += perCount;
                }
                catch (Exception Ex)
                { }

                ProgressUserAccount = 1;
            }

            IsEnableCancelButton = true;
            SyncCancelButtonText = "Close";
        }
        private async Task StartHandoverSyncerAsync()
        {
            var GetType = ItemSourceAddedSyncList.FirstOrDefault().GetType();
            SyncCancelButtonText = "Syncing";
            IsVisibleSyncButton =  false;
            IsEnableCancelButton = false;
            float perCount = (float)1 / (float)ItemSourceAddedSyncList.Count;
            //Handover sync
            if (IsCheckedSyncITRs)
            {
                if (GetType == typeof(HandoversRunnable))
                {
                    //delete all recoreds from T_HandoverWorkpacks and T_Handover
                    var BeforeData = _CompletionsHandoverRepository.GetAsync();
                    //await _CompletionsHandoverRepository.QueryAsync<T_Handover>("DELETE FROM [T_Handover]");
                    //await _HandoverWorkpackRepository.QueryAsync<T_HandoverWorkpacks>("DELETE FROM [T_HandoverWorkpacks]");
                    var AfterData = _CompletionsHandoverRepository.GetAsync();
                    foreach (HandoversRunnable Handovers in ItemSourceAddedSyncList)
                    {
                        var result = await _IHandoverSyncer.downloadChangesAsync(Handovers.name);
                        ProgressITRs += perCount;
                    }
                }
                else if (GetType == typeof(SystemModel))
                {
                    foreach (SystemModel systems in ItemSourceAddedSyncList)
                    {

                        var result = await _IWorkpackFullSystemSyncer.downloadChanges(systems.ref1);
                        ProgressITRs += perCount;

                    }
                }
                ProgressITRs = 1;
            }
            if (IsCheckedSyncPunchList)
            {
                foreach (HandoversRunnable Handovers in ItemSourceAddedSyncList)
                {
                    var result = await _ISystemPunchListSyncer.downloadChanges(Handovers.name);
                    ProgressPunchList += perCount;

                }
                ProgressITRs = 1;
            }
            IsEnableCancelButton = true;
            SyncCancelButtonText = "Close";
        }
        private async Task StartTestPackSyncerAsync()
        {
            SyncCancelButtonText = "Syncing";
            IsVisibleSyncButton = false;
            IsEnableCancelButton = false;
            float perCount = (float)1 / (float)ItemSourceAddedSyncList.Count;
            //Handover sync
            if (IsCheckedSyncITRs)
            {
                var GetType = ItemSourceAddedSyncList.FirstOrDefault().GetType();
                if (GetType == typeof(HandoversRunnable))
                {
                    foreach (HandoversRunnable TestPack in ItemSourceAddedSyncList)
                    {
                        var result = await _ITestPackSyncer.downloadChanges(TestPack.name);
                        ProgressITRs += perCount;
                    }
                }
                else if (GetType == typeof(SystemModel))
                {
                    foreach (SystemModel systems in ItemSourceAddedSyncList)
                    {
                        _ = await _ISystemsFullSystemSyncercs.downloadChanges(systems.ref1);
                        ProgressITRs += perCount;
                    }
                }
                ProgressITRs = 1;
            }
            //if (IsCheckedSyncPunchList)
            //{
            //    //await _CompletionsPunchListRepository.QueryAsync<T_CompletionsPunchList>("DELETE FROM [T_CompletionsPunchList]");
            //    foreach (WorkpacksRunnable workpack in ItemSourceAddedSyncList)
            //    {
            //        var result = await _IWorkpackPunchListSyncer.downloadChanges(workpack.name);
            //        ProgressPunchList += perCount;
            //    }
            //    ProgressITRs = 1;
            //}
            IsEnableCancelButton = true;
            SyncCancelButtonText = "Close";
        }
        private async Task StartITRSyncerAsync()
        {
            SyncCancelButtonText = "Syncing";
            IsVisibleSyncButton = false;
            IsEnableCancelButton = false;
            float perCount = (float)1 / (float)ItemSourceAddedSyncList.Count;
            //Handover sync
            if (IsCheckedSyncITRs)
            {
                var GetType = ItemSourceAddedSyncList.FirstOrDefault().GetType();
                if (GetType == typeof(ItrRunnable))
                {
                    foreach (ItrRunnable ITR in ItemSourceAddedSyncList)
                    {
                        // var Tagkeyvalueypair = tagToSheetList.Where(x => x.Value.Where(y => y.Contains("fgh"))).ToDictionary(x => x.Key, x => x.Value);
                        _ = await _ITRSyncer.downloadChanges(ITR.ItrNumber, ITR.WorkpackName);
                        ProgressITRs += perCount;
                    }
                }
                //else if (GetType == typeof(DataRef))
                //{
                //    foreach (DataRef systems in ItemSourceAddedSyncList)
                //    {
                //        var result = await _ISystemsFullSystemSyncercs.downloadChanges(systems.ref1);
                //        ProgressITRs += perCount;
                //    }
                //}

                IsEnableCancelButton = true;
                SyncCancelButtonText = "Close";
                ProgressITRs = 1;
            }


        }
        private void GetInitialData()
        {
            // ItemSourceWorkPacksList = new ObservableCollection<dynamic>();
            ItemSourceAddedSyncList = new ObservableCollection<dynamic>();
           // _userDialogs.ShowLoading("Getting WorkPacks...");
            string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getInitialDataList(Settings.ModelName, "dontmatter", false, Settings.CurrentDB), Settings.CompletionAccessToken);
            SyncInitialList = JsonConvert.DeserializeObject<InitialDataModel>(JsonString);           
            IsVisibleSyncPopup = false;
            IsVisibleTagPopup = true;
           // _userDialogs.HideLoading();
        }
        private async void GetTags()
        {
            try
            {
                // DependencyService.Get<IToastMessage>().LongAlert("Tag List gathering in progress");
                // DependencyService.Get<IToastMessage>().ShortAlert("Tag List gathering in progress");
                PlaceHolderText = "Filter Tag List";
                var TagList = ItemSourceAddedSyncList.ToList();
                string SyncType = string.Empty;

                if (TagList.Count <= 0)
                    return;
               
                var GetType = TagList.FirstOrDefault().GetType();
                if (GetType == typeof(SystemModel) || GetType == typeof(PCWBSModel) || GetType == typeof(FWBSModel))
                {
                    ItemSourceAddedSyncList = new ObservableCollection<dynamic>();
                    ItemSourceAddedSyncList.Clear();
                    List<TagsModel> TagsList = new List<TagsModel>();

                    if (GetType == typeof(SystemModel))
                    {
                        SyncType = "SYSTEM";
                        TagList = TagList.GroupBy(x => x.ref1).Select(i => i.FirstOrDefault()).ToList();
                    }
                    else if (GetType == typeof(PCWBSModel))
                        SyncType = "PCWBS";

                    else if (GetType == typeof(FWBSModel))
                        SyncType = "FWBS";

                    var TagsID = TagList.ToList().Select(x => x.name).ToArray();
                    string postJSON = JsonConvert.SerializeObject(TagsID);

                    //string JsonString = ModsTools.CompletionWebServicePost(ApiUrls.Post_GetTagsByColumn(Settings.ModelName, Settings.ProjectName, Settings.CurrentDB, SyncType), postJSON, Settings.CompletionAccessToken);
                    //var SystemsList = JsonConvert.DeserializeObject<List<T_WorkPack>>(JsonString);               

                    //CompletionsSyncTAgs = SystemsList;
                    //var taglist = SystemsList.SelectMany(list => list.tags).Distinct().ToList();

                    // RunnableSyncList = ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(SystemsList.SelectMany(list => list.tags).Select(i=>i.name).Distinct().ToList());

                    //RunnableSyncList = ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(SystemsList.SelectMany(list => list.tags).Distinct().ToList());



                    // GetTagNosByColumn
                    string JsonString = ModsTools.CompletionWebServicePost(ApiUrls.Post_GetTagNosByColumn(Settings.ModelName, Settings.ProjectName, Settings.CurrentDB, SyncType), postJSON, Settings.CompletionAccessToken);
                    var SystemsList = JsonConvert.DeserializeObject<List<T_TAG>>(JsonString);
                    RunnableSyncList = ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(SystemsList);


                    if (ItemSourceRunnableSyncList.Count > 0)
                        SyncData = "SyncTags";
                    else
                    {
                        DependencyService.Get<IToastMessage>().ShortAlert("No Record found. Please try again");
                        if (SyncType == "SYSTEM")
                            OnClickAsync("SyncbySystem");
                        else if (SyncType == "PCWBS")
                            OnClickAsync("SyncbyPCWBS");
                        else if (SyncType == "FWBS")
                            OnClickAsync("SyncbyFWBS");
                    }
                    IsVisibleSyncPopup = false;
                    IsVisibleTagPopup = true;
                }
                else
                    IsVisibleSyncProgressPopup = true;
            }
            catch(Exception e)
            {

            }
        }
        //private void GetWorkPaks()
        //{
        //    // ItemSourceWorkPacksList = new ObservableCollection<dynamic>();
        //    ItemSourceAddedSyncList = new ObservableCollection<dynamic>();
        //    _userDialogs.ShowLoading("Getting WorkPacks...");
        //    string JsonString = ModsTools.WebServiceGet(ApiUrls.getWorkPackList(Settings.ModelName, "dontmatter", false, Settings.CurrentDB), Settings.AccessToken);
        //    RunnableSyncList = new ObservableCollection<dynamic>(JsonConvert.DeserializeObject<ObservableCollection<WorkpacksRunnable>>(JsonString));
        //    ItemSourceRunnableSyncList = RunnableSyncList;
        //    IsVisibleSyncPopup = false;
        //    IsVisibleTagPopup = true;
        //    _userDialogs.HideLoading();
        //}
        //private void GetSystems()
        //{
        //    // ItemSourceWorkPacksList = new ObservableCollection<dynamic>();
        //    ItemSourceAddedSyncList = new ObservableCollection<dynamic>();
        //    _userDialogs.ShowLoading("Getting systems...");
        //    string JsonString = ModsTools.WebServiceGet(ApiUrls.getSystems(Settings.ModelName, Settings.CurrentDB), Settings.AccessToken);
        //    RunnableSyncList = new ObservableCollection<dynamic>(JsonConvert.DeserializeObject<ObservableCollection<DataRef>>(JsonString).GroupBy(x => x.ref1).Select(grp => grp.FirstOrDefault()));
        //    ItemSourceRunnableSyncList = RunnableSyncList;
        //    IsVisibleSyncPopup = false;
        //    IsVisibleTagPopup = true;
        //    _userDialogs.HideLoading();
        //}
        private void GetHandovers()
        {
            // ItemSourceWorkPacksList = new ObservableCollection<dynamic>();
            ItemSourceAddedSyncList = new ObservableCollection<dynamic>();
            //_userDialogs.ShowLoading("Getting handovers...");
            string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getHandoverSystems(Settings.ModelName, Settings.ProjectName, Settings.CurrentDB), Settings.CompletionAccessToken);
            if (String.IsNullOrEmpty(JsonString) || JsonString == "null")
            {
                DependencyService.Get<IToastMessage>().ShortAlert("No Record found. Please try again");
                return;
            }
            var Handovers = new ObservableCollection<dynamic>(JsonConvert.DeserializeObject<ObservableCollection<string>>(JsonString));
            RunnableSyncList = new ObservableCollection<dynamic>(Handovers.Select(s => new HandoversRunnable { name = s}));

            ItemSourceRunnableSyncList = RunnableSyncList;
            if (ItemSourceRunnableSyncList.Count > 0)
            {
                IsVisibleSyncPopup = false;
                IsVisibleTagPopup = true;
            }
            else
                DependencyService.Get<IToastMessage>().ShortAlert("No Record found. Please try again");

           // _userDialogs.HideLoading();
        }

        private void GetTestPacks()
        {
            // ItemSourceWorkPacksList = new ObservableCollection<dynamic>();
            ItemSourceAddedSyncList = new ObservableCollection<dynamic>();
           // _userDialogs.ShowLoading("Getting TastPacks...");
            string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getTestPacks(Settings.ModelName, Settings.ProjectName, Settings.CurrentDB), Settings.CompletionAccessToken);
            if (String.IsNullOrEmpty(JsonString) || JsonString == "null")
            {
                DependencyService.Get<IToastMessage>().ShortAlert("No Record found. Please try again");
                return;
            }
            var TastPacks = new ObservableCollection<dynamic>(JsonConvert.DeserializeObject<ObservableCollection<string>>(JsonString));
            RunnableSyncList = new ObservableCollection<dynamic>(TastPacks.Select(x => new HandoversRunnable { name = x }));

            ItemSourceRunnableSyncList = RunnableSyncList;
            if (ItemSourceRunnableSyncList.Count > 0)
            {
                IsVisibleSyncPopup = false;
                IsVisibleTagPopup = true;
            }
            else
                DependencyService.Get<IToastMessage>().ShortAlert("No Record found. Please try again");

          //  _userDialogs.HideLoading();
        }

        private void GetITR()
        {
            //_userDialogs.ShowLoading("Getting ITR's...");
            RunnableSyncList = new ObservableCollection<dynamic>();
            List<T_CHECKSHEET> ITRs = new List<T_CHECKSHEET>();
            ItemSourceAddedSyncList = new ObservableCollection<dynamic>();


            //get runable workpacks 
            string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getWorkPackList(Settings.ModelName, "dontmatter", false, Settings.CurrentDB), Settings.CompletionAccessToken);
            var workpackList = JsonConvert.DeserializeObject<List<WorkpackModel>>(JsonString);
            if (workpackList != null && workpackList.Any())
            {
                foreach (WorkpackModel wp in workpackList)
                {
                    string JsonStringTags = ModsTools.CompletionWebServiceGet(ApiUrls.getTags(Settings.ModelName, Settings.ProjectName, "workpack", wp.name, "dontmatter", Settings.CurrentDB), Settings.CompletionAccessToken);
                    var _Tag = JsonConvert.DeserializeObject<T_WorkPack>(JsonStringTags);
                    foreach (var entry in _Tag.tagNameToSheetNameMap)
                    {
                        tagToSheetList.Add(entry.Key, entry.Value);
                    }


                    ITRs.AddRange(_Tag.checkSheets);
                    ITRs.ForEach(x => x.workpackName = wp.name);

                }

                RunnableSyncList = new ObservableCollection<dynamic>(ITRs.Select(x => new ItrRunnable { name = x.name + "-" + x.description, ItrNumber = x.name, Itrdescription = x.description, WorkpackName = x.workpackName }).Distinct());
                ItemSourceRunnableSyncList = RunnableSyncList;
                if (ItemSourceRunnableSyncList.Count > 0)
                {
                    IsVisibleSyncPopup = false;
                    IsVisibleTagPopup = true;
                }
            }
            else
            {
                //IsVisibleSyncPopup = false;
                _userDialogs.Alert("ITR's not found", "ITR Sync", "OK");
            }

            
          //  _userDialogs.HideLoading();

        }
        private void WorkpackFiletr()
        {
            //// if
            //bool ClearFilterUnderReviews = true;
            //if (!CheckUnderReviews)
            //    ClearFilterUnderReviews = true;
            //if (CheckSyncAllUsers) return;

            if (TxtFilter != null)
            {
                ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(RunnableSyncList.Where(s=> s.name.ToLower().Contains(TxtFilter.ToLower())));
            }

            if (CheckUnderReviews)
            {
                ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(ItemSourceRunnableSyncList.Where(x => x.inReview == true));
            }
            //else if (!CheckUnderReviews)
            //    ItemSourceRunnableSyncList = new ObservableCollection<dynamic>(RunnableSyncList);
            // 
            // ItemSourceWorkPacksList = 
            //if (CheckSyncAllUsers) { }
            //if (string.IsNullOrWhiteSpace(TxtFilter)) { WorkPacksList = WorkPacksList.Where(x => x.name).ToList(); }
        }


        public async Task DownloadTagsData(string[] IDList)
        {
            ///// new implementation
            //await _ISystemsFullSystemSyncercs.downloadChangesByID(JsonConvert.SerializeObject(IDs));
            List<CheckSheetAnswerModel> CheckSheetAnswerModelList = new List<CheckSheetAnswerModel>();

            //pullDownWorkpacksbyID
            // string WorkpackjsonResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_GetTagsByID(Settings.ModelName, Settings.ProjectName, Settings.CurrentDB), IDs, Settings.CompletionAccessToken);
            string WorkpackjsonResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_GetTagsAndItrsByID(Settings.ModelName, Settings.ProjectName, Settings.CurrentDB), JsonConvert.SerializeObject(IDList), Settings.CompletionAccessToken);
            var SystemsList = JsonConvert.DeserializeObject<T_WorkPack>(WorkpackjsonResult);
            if (SystemsList.tagNameToSheetNameMap != null)
            {
                tagToSheetList = SystemsList.tagNameToSheetNameMap;

                foreach (var tag in tagToSheetList)
                {
                    foreach (var SheetName in tag.Value)
                    {
                        await _CHECKSHEET_PER_TAGRepository.InsertOrReplaceAsync(new T_CHECKSHEET_PER_TAG() { TAGNAME = tag.Key, CHECKSHEETNAME = SheetName, HEADER_ID = "0", JOBPACK = " ", ProjectName = Settings.ProjectName });
                    }
                }
            }
            if (SystemsList.tags != null)
            {
                SystemsList.tags.ForEach(s => s.ProjectName = Settings.ProjectName);
                await _TAGRepository.InsertOrReplaceAsync(SystemsList.tags);
            }
            if (SystemsList.checkSheets != null)
            {
                SystemsList.checkSheets.ForEach(s => s.ProjectName = Settings.ProjectName);
                await _CHECKSHEETRepository.InsertOrReplaceAsync(SystemsList.checkSheets);
            }
            CheckSheetAnswerModelList = SystemsList.checkSheets.Where(s => s.ItrAnswerList != null).SelectMany(s => s.ItrAnswerList).ToList();



            //pullDownSheetQuestions
            List<string> sheetNames = new List<string>();

            sheetNames.AddRange(tagToSheetList.Values.SelectMany(l => l).Distinct().ToList());
            foreach (string sheetname in sheetNames)
            {
                var JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getSheetQuestions(Settings.ProjectName, Settings.CurrentDB, sheetname), Settings.CompletionAccessToken);
                var Quetions = JsonConvert.DeserializeObject<List<T_CHECKSHEET_QUESTION>>(JsonString);

                if (Quetions != null && Quetions.Any())
                {
                    Quetions.ForEach(s => { s.CheckSheet = sheetname; s.ProjectName = Settings.ProjectName; });
                    await _CHECKSHEET_QUESTIONRepository.InsertOrReplaceAsync(Quetions);
                }

            }


            //pullCheckSheetAnswersAsync
            foreach (CheckSheetAnswerModel checkSheetAnswer in CheckSheetAnswerModelList)
            {
                if (checkSheetAnswer.SignOffHeaders.Any())
                {
                    checkSheetAnswer.SignOffHeaders.ForEach(s => { s.SignOffChecksheet = checkSheetAnswer.CheckSheetName; s.SignOffTag = checkSheetAnswer.TagName; s.ProjectName = Settings.ProjectName; });
                    await _SignOffHeaderRepository.InsertOrReplaceAsync(checkSheetAnswer.SignOffHeaders);
                }

                if (checkSheetAnswer.answers.Any())
                {
                    checkSheetAnswer.answers.ForEach(s =>
                    {
                        s.tagName = checkSheetAnswer.TagName; s.ccmsHeaderId = checkSheetAnswer.CcmsHeaderId; s.checkSheetName = checkSheetAnswer.CheckSheetName;
                        s.jobPack = checkSheetAnswer.JobPack; s.IsSynced = true; s.ProjectName = Settings.ProjectName;
                    });

                    if (checkSheetAnswer.answers != null && checkSheetAnswer.answers.Any())
                        await _TAG_SHEET_ANSWERRepository.InsertOrReplaceAsync(checkSheetAnswer.answers);
                }
                if (checkSheetAnswer.TagSheetHeaders != null)
                {
                    PropertyInfo[] Props = typeof(TagSheetHeaders).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (PropertyInfo prop in Props)
                    {
                        var PropValue = prop.GetValue(checkSheetAnswer.TagSheetHeaders);
                        if (PropValue != null)
                        {
                            var T_SheetHeader = new T_TAG_SHEET_HEADER()
                            {
                                TagName = checkSheetAnswer.TagName,
                                ChecksheetName = checkSheetAnswer.CheckSheetName,
                                ColumnKey = prop.Name,
                                ColumnValue = PropValue.ToString(),
                                JobPack = checkSheetAnswer.JobPack,
                                ProjectName = Settings.ProjectName
                            };
                            await _TAG_SHEET_HEADERRepository.InsertOrReplaceAsync(T_SheetHeader);
                        }
                    }
                }
            }
        }

        public void BG_Download_RemaingsTags(string[] ID_List)
        {
            ///// new implementation
            Task.Run( async() =>
            {
                int batchsize = 20;
                for (int x = 0; x < Math.Ceiling((decimal)ID_List.Count() / batchsize); x++)
                {
                    string[] IDs = ID_List.Skip(x * batchsize).Take(batchsize).ToArray();
                    //await _ISystemsFullSystemSyncercs.downloadChangesByID(JsonConvert.SerializeObject(IDs));
                    List<CheckSheetAnswerModel> CheckSheetAnswerModelList = new List<CheckSheetAnswerModel>();

                    //pullDownWorkpacksbyID
                    // string WorkpackjsonResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_GetTagsByID(Settings.ModelName, Settings.ProjectName, Settings.CurrentDB), IDs, Settings.CompletionAccessToken);
                    string WorkpackjsonResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_GetTagsAndItrsByID(Settings.ModelName, Settings.ProjectName, Settings.CurrentDB), JsonConvert.SerializeObject(IDs), Settings.CompletionAccessToken);
                    var SystemsList = JsonConvert.DeserializeObject<T_WorkPack>(WorkpackjsonResult);
                    if (SystemsList.tagNameToSheetNameMap != null)
                    {
                        tagToSheetList = SystemsList.tagNameToSheetNameMap;

                        foreach (var tag in tagToSheetList)
                        {
                            foreach (var SheetName in tag.Value)
                            {
                                //if (await _ITRService.IsImplementedITR(SheetName))
                                //    await GetCheckSheetITRsData(tag.Key, SheetName);
                                bool CCSITR = SystemsList.checkSheets.Where(i => i.name == SheetName).FirstOrDefault().ccsItr;
                                await _CHECKSHEET_PER_TAGRepository.InsertOrReplaceAsync(new T_CHECKSHEET_PER_TAG() { TAGNAME = tag.Key, CHECKSHEETNAME = SheetName, ccsItr = CCSITR, HEADER_ID = "0", JOBPACK = " ", ProjectName = Settings.ProjectName });
                            }
                        }
                    }
                    if (SystemsList.tags != null)
                    {
                        SystemsList.tags.ForEach(s => s.ProjectName = Settings.ProjectName);
                        await _TAGRepository.InsertOrReplaceAsync(SystemsList.tags);
                    }
                    if (SystemsList.checkSheets != null)
                    {
                        SystemsList.checkSheets.ForEach(s => s.ProjectName = Settings.ProjectName);
                        await _CHECKSHEETRepository.InsertOrReplaceAsync(SystemsList.checkSheets);
                    }
                    CheckSheetAnswerModelList = SystemsList.checkSheets.Where(s => s.ItrAnswerList != null).SelectMany(s => s.ItrAnswerList).ToList();

                    List<ITRs> ITRData = SystemsList.checkSheets.Where(i => i.ITRData != null).Select(i => i.ITRData).ToList();

                    if (ITRData.Count > 0)
                    {
                        foreach (ITRs item in ITRData.ToList())
                        {
                            await GetCheckSheetITRsData(item);
                        }
                    }

                    //pullDownSheetQuestions
                    List<string> sheetNames = new List<string>();

                    sheetNames.AddRange(tagToSheetList.Values.SelectMany(l => l).Distinct().ToList());
                    foreach (string sheetname in sheetNames)
                    {
                        var JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getSheetQuestions(Settings.ProjectName, Settings.CurrentDB, sheetname), Settings.CompletionAccessToken);
                        var Quetions = JsonConvert.DeserializeObject<List<T_CHECKSHEET_QUESTION>>(JsonString);

                        if (Quetions != null && Quetions.Any())
                        {
                            Quetions.ForEach(s => { s.CheckSheet = sheetname; s.ProjectName = Settings.ProjectName; });
                            await _CHECKSHEET_QUESTIONRepository.InsertOrReplaceAsync(Quetions);
                        }

                    }


                    //pullCheckSheetAnswersAsync
                    foreach (CheckSheetAnswerModel checkSheetAnswer in CheckSheetAnswerModelList)
                    {
                        if (checkSheetAnswer.SignOffHeaders.Any())
                        {
                            checkSheetAnswer.SignOffHeaders.ForEach(s => { s.SignOffChecksheet = checkSheetAnswer.CheckSheetName; s.SignOffTag = checkSheetAnswer.TagName; s.ProjectName = Settings.ProjectName; });
                            await _SignOffHeaderRepository.InsertOrReplaceAsync(checkSheetAnswer.SignOffHeaders);
                        }

                        if (checkSheetAnswer.answers.Any())
                        {
                            checkSheetAnswer.answers.ForEach(s =>
                            {
                                s.tagName = checkSheetAnswer.TagName; s.ccmsHeaderId = checkSheetAnswer.CcmsHeaderId; s.checkSheetName = checkSheetAnswer.CheckSheetName;
                                s.jobPack = checkSheetAnswer.JobPack; s.IsSynced = true; s.ProjectName = Settings.ProjectName;
                            });

                            if (checkSheetAnswer.answers != null && checkSheetAnswer.answers.Any())
                                await _TAG_SHEET_ANSWERRepository.InsertOrReplaceAsync(checkSheetAnswer.answers);
                        }
                        if (checkSheetAnswer.TagSheetHeaders != null)
                        {
                            PropertyInfo[] Props = typeof(TagSheetHeaders).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                            foreach (PropertyInfo prop in Props)
                            {
                                var PropValue = prop.GetValue(checkSheetAnswer.TagSheetHeaders);
                                if (PropValue != null)
                                {
                                    var T_SheetHeader = new T_TAG_SHEET_HEADER()
                                    {
                                        TagName = checkSheetAnswer.TagName,
                                        ChecksheetName = checkSheetAnswer.CheckSheetName,
                                        ColumnKey = prop.Name,
                                        ColumnValue = PropValue.ToString(),
                                        JobPack = checkSheetAnswer.JobPack,
                                        ProjectName = Settings.ProjectName
                                    };
                                    await _TAG_SHEET_HEADERRepository.InsertOrReplaceAsync(T_SheetHeader);
                                }
                            }
                        }
                    }
                }
            }).ConfigureAwait(false);
        }
        private async Task<bool> GetCheckSheetITRsData(ITRs ITRdata)
        {
            try
            {
                if (ITRdata.ITR7000_030A_031A != null)
                {
                    ITR7000_030A_031AModel ITRSeries_7000 = ITRdata.ITR7000_030A_031A;
                    if (String.IsNullOrEmpty(ITRSeries_7000.Error))
                    {
                        var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                        long RecordID = CHFrecord.Count() + 1;
                        if (ITRSeries_7000.CommonHeaderFooter != null)
                        {
                            ITRSeries_7000.CommonHeaderFooter.ModelName = Settings.ModelName;
                            ITRSeries_7000.CommonHeaderFooter.ROWID = RecordID;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITRSeries_7000.CommonHeaderFooter);
                            if (ITRSeries_7000.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITRSeries_7000.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITRSeries_7000.CommonHeaderFooter.ID; x.SignOffChecksheet = ITRSeries_7000.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITRSeries_7000.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_7000.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITRSeries_7000.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                        }
                        if (ITRSeries_7000.Records_30A_31A != null)
                        {
                            ITRSeries_7000.Records_30A_31A.CommonRowID = RecordID;
                            ITRSeries_7000.Records_30A_31A.CommonHFID = ITRSeries_7000.CommonHeaderFooter.ID;
                            ITRSeries_7000.Records_30A_31A.CCMS_HEADERID = (int)ITRSeries_7000.CommonHeaderFooter.ID;
                            ITRSeries_7000.Records_30A_31A.ModelName = Settings.ModelName;
                            await _RecordsRepository.InsertOrReplaceAsync(ITRSeries_7000.Records_30A_31A);
                        }
                        if (ITRSeries_7000.TubeColors.Count > 0)
                        {
                            var _TubeColors = await _TubeColorsRepository.GetAsync();
                            long TCID = _TubeColors.Count() + 1;

                            ITRSeries_7000.TubeColors.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonHFID = ITRSeries_7000.CommonHeaderFooter.ID; x.CCMS_HEADERID = (int)ITRSeries_7000.CommonHeaderFooter.ID; x.CommonRowID = RecordID; x.RowID = TCID++; });
                            await _TubeColorsRepository.InsertOrReplaceAsync(ITRSeries_7000.TubeColors);
                        }
                    }
                }
                else if (ITRdata.ITR7000_040A_041A_042A != null)
                {
                    ITR7000_040A_041A_042AModel ITRSeries_7000 = ITRdata.ITR7000_040A_041A_042A;
                    if (String.IsNullOrEmpty(ITRSeries_7000.Error))
                    {
                        var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                        long RecordID = CHFrecord.Count() + 1;
                        if (ITRSeries_7000.CommonHeaderFooter != null)
                        {
                            ITRSeries_7000.CommonHeaderFooter.ROWID = RecordID;
                            ITRSeries_7000.CommonHeaderFooter.ModelName = Settings.ModelName;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITRSeries_7000.CommonHeaderFooter);
                            if (ITRSeries_7000.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITRSeries_7000.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITRSeries_7000.CommonHeaderFooter.ID; x.SignOffChecksheet = ITRSeries_7000.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITRSeries_7000.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_7000.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITRSeries_7000.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                        }
                        if (ITRSeries_7000.Records_40A_41A_042A != null)
                        {
                            ITRSeries_7000.Records_40A_41A_042A.ModelName = Settings.ModelName;
                            ITRSeries_7000.Records_40A_41A_042A.CommonRowID = RecordID;
                            ITRSeries_7000.Records_40A_41A_042A.CCMS_HEADERID = (int)ITRSeries_7000.CommonHeaderFooter.ID;
                            ITRSeries_7000.Records_40A_41A_042A.ITRCommonID = ITRSeries_7000.CommonHeaderFooter.ID;
                            await _Records_04XARepository.InsertOrReplaceAsync(ITRSeries_7000.Records_40A_41A_042A);
                        }
                        if (ITRSeries_7000.InsulationDetails.Count > 0)
                        {
                            var _InsulationDetails = await _InsulationDetailsRepository.GetAsync();
                            long InsID = _InsulationDetails.Count() + 1;
                            ITRSeries_7000.InsulationDetails.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = InsID++; x.CCMS_HEADERID = (int)ITRSeries_7000.CommonHeaderFooter.ID; });
                            await _InsulationDetailsRepository.InsertOrReplaceAsync(ITRSeries_7000.InsulationDetails);
                        }
                    }
                }
                else if (ITRdata.ITR7000_080A_090A_091A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR7000_080A_090A_091AModel ITRSeries_7000 = ITRdata.ITR7000_080A_090A_091A;
                    if (String.IsNullOrEmpty(ITRSeries_7000.Error))
                    {
                        // long RecordID = 0;
                        if (ITRSeries_7000.CommonHeaderFooter != null)
                        {
                            ITRSeries_7000.CommonHeaderFooter.ROWID = RecordID;
                            ITRSeries_7000.CommonHeaderFooter.ModelName = Settings.ModelName;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITRSeries_7000.CommonHeaderFooter);
                            if (ITRSeries_7000.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITRSeries_7000.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITRSeries_7000.CommonHeaderFooter.ID; x.SignOffChecksheet = ITRSeries_7000.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITRSeries_7000.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_7000.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITRSeries_7000.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                        }
                        if (ITRSeries_7000.Records_080A_090A_091A != null)
                        {
                            ITRSeries_7000.Records_080A_090A_091A.ModelName = Settings.ModelName;
                            ITRSeries_7000.Records_080A_090A_091A.CommonRowID = RecordID;
                            ITRSeries_7000.Records_080A_090A_091A.CCMS_HEADERID = (int)ITRSeries_7000.CommonHeaderFooter.ID;
                            ITRSeries_7000.Records_080A_090A_091A.ITRCommonID = ITRSeries_7000.CommonHeaderFooter.ID;
                            await _Records_080A_09XARepository.InsertOrReplaceAsync(ITRSeries_7000.Records_080A_090A_091A);
                        }
                    }
                }
                else if (ITRdata.ITR_8000_003A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR_8000_003AModel ITRSeries_8000 = ITRdata.ITR_8000_003A;
                    if (String.IsNullOrEmpty(ITRSeries_8000.Error))
                    {
                        // long RecordID = 0;
                        if (ITRSeries_8000.CommonHeaderFooter != null)
                        {
                            ITRSeries_8000.CommonHeaderFooter.ROWID = RecordID;
                            ITRSeries_8000.CommonHeaderFooter.ModelName = Settings.ModelName;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITRSeries_8000.CommonHeaderFooter);
                            if (ITRSeries_8000.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITRSeries_8000.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITRSeries_8000.CommonHeaderFooter.ID; x.SignOffChecksheet = ITRSeries_8000.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITRSeries_8000.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_8000.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITRSeries_8000.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                            if (ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData != null)
                            {
                                var Instrumentdata8000_003A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID = 1;
                                if (Instrumentdata8000_003A.Count > 0)
                                    InstID = Instrumentdata8000_003A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITRSeries_8000.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }
                        if (ITRSeries_8000.ITR_8000_003ARecords != null)
                        {
                            ITRSeries_8000.ITR_8000_003ARecords.ModelName = Settings.ModelName;
                            ITRSeries_8000.ITR_8000_003ARecords.CommonRowID = RecordID;
                            ITRSeries_8000.ITR_8000_003ARecords.CCMS_HEADERID = (int)ITRSeries_8000.CommonHeaderFooter.ID;
                            ITRSeries_8000.ITR_8000_003ARecords.ITRCommonID = ITRSeries_8000.CommonHeaderFooter.ID;
                            await _Records_8000003ARepository.InsertOrReplaceAsync(ITRSeries_8000.ITR_8000_003ARecords);
                        }
                        if (ITRSeries_8000.ITR_8000_003A_AcceptanceCriteria.Count > 0)
                        {
                            var AccCrRecords = await _Records_8000003A_AcceptanceCriteriaRepository.GetAsync();
                            long AccCrReID = AccCrRecords.Count() + 1;

                            ITRSeries_8000.ITR_8000_003A_AcceptanceCriteria.ForEach(x => { x.RowID = AccCrReID++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITRSeries_8000.CommonHeaderFooter.ID; });
                            await _Records_8000003A_AcceptanceCriteriaRepository.InsertOrReplaceAsync(ITRSeries_8000.ITR_8000_003A_AcceptanceCriteria);
                        }
                    }
                }
                else if (ITRdata.ITR8100_001A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    var RatioTest = await _ITR8100_001A_RatioTestRepository.GetAsync();
                    ITR8100_001AModel ITRSeries_8100_001A = ITRdata.ITR8100_001A;
                    if (String.IsNullOrEmpty(ITRSeries_8100_001A.Error))
                    {
                        if (ITRSeries_8100_001A.CommonHeaderFooter != null)
                        {
                            ITRSeries_8100_001A.CommonHeaderFooter.ROWID = RecordID;
                            ITRSeries_8100_001A.CommonHeaderFooter.ModelName = Settings.ModelName;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITRSeries_8100_001A.CommonHeaderFooter);
                            if (ITRSeries_8100_001A.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITRSeries_8100_001A.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITRSeries_8100_001A.CommonHeaderFooter.ID; x.SignOffChecksheet = ITRSeries_8100_001A.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITRSeries_8100_001A.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_8100_001A.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITRSeries_8100_001A.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                            if (ITRSeries_8100_001A.CommonHeaderFooter.ITRInstrumentData != null)
                            {
                                var Instrumentdata8100_001A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID8100_001A = 1;
                                if (Instrumentdata8100_001A.Count > 0)
                                    InstID8100_001A = Instrumentdata8100_001A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITRSeries_8100_001A.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8100_001A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITRSeries_8100_001A.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRSeries_8100_001A.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }
                        if (ITRSeries_8100_001A.ITR_CTdata != null)
                        {
                            long RatioTestID = RatioTest.Count() + 1;
                            ITRSeries_8100_001A.ITR_CTdata.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = RatioTestID++; x.CCMS_HEADERID = (int)ITRSeries_8100_001A.CommonHeaderFooter.ID; });
                            await _ITR8100_001A_CTdataRepository.InsertOrReplaceAsync(ITRSeries_8100_001A.ITR_CTdata);
                        }
                        if (ITRSeries_8100_001A.ITR_InsulationResistanceTest != null)
                        {
                            long RatioTestID = RatioTest.Count() + 1;
                            ITRSeries_8100_001A.ITR_InsulationResistanceTest.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = RatioTestID++; x.CCMS_HEADERID = (int)ITRSeries_8100_001A.CommonHeaderFooter.ID; });
                            await _ITR8100_001A_IRTestRepository.InsertOrReplaceAsync(ITRSeries_8100_001A.ITR_InsulationResistanceTest);
                        }
                        if (ITRSeries_8100_001A.ITR_RatioTest != null)
                        {
                            long RatioTestID = RatioTest.Count() + 1;
                            ITRSeries_8100_001A.ITR_RatioTest.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = RatioTestID++; x.CCMS_HEADERID = (int)ITRSeries_8100_001A.CommonHeaderFooter.ID; });
                            await _ITR8100_001A_RatioTestRepository.InsertOrReplaceAsync(ITRSeries_8100_001A.ITR_RatioTest);
                        }
                        if (ITRSeries_8100_001A.ITR_TestInstrumentData != null)
                        {
                            ITRSeries_8100_001A.ITR_TestInstrumentData.ModelName = Settings.ModelName;
                            ITRSeries_8100_001A.ITR_TestInstrumentData.CommonRowID = RecordID;
                            ITRSeries_8100_001A.ITR_TestInstrumentData.CCMS_HEADERID = (int)ITRSeries_8100_001A.CommonHeaderFooter.ID;
                            await _ITR8100_001A_TIDataRepository.InsertOrReplaceAsync(ITRSeries_8100_001A.ITR_TestInstrumentData);
                        }
                    }
                }
                else if (ITRdata.ITR8140_001A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR8140_001AModel ITRSeries_8000 = ITRdata.ITR8140_001A;
                    if (String.IsNullOrEmpty(ITRSeries_8000.Error))
                    {
                        // long RecordID = 0;
                        if (ITRSeries_8000.CommonHeaderFooter != null)
                        {
                            ITRSeries_8000.CommonHeaderFooter.ROWID = RecordID;
                            ITRSeries_8000.CommonHeaderFooter.ModelName = Settings.ModelName;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITRSeries_8000.CommonHeaderFooter);
                            if (ITRSeries_8000.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITRSeries_8000.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITRSeries_8000.CommonHeaderFooter.ID; x.SignOffChecksheet = ITRSeries_8000.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITRSeries_8000.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_8000.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITRSeries_8000.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                            if (ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData != null)
                            {
                                var Instrumentdata8140_001A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID8140_001A = 1;
                                if (Instrumentdata8140_001A.Count > 0)
                                    InstID8140_001A = Instrumentdata8140_001A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8140_001A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITRSeries_8000.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }

                        if (ITRSeries_8000.iTR8140_001A_TestInstrumentData != null)
                        {
                            ITRSeries_8000.iTR8140_001A_TestInstrumentData.ModelName = Settings.ModelName;
                            ITRSeries_8000.iTR8140_001A_TestInstrumentData.CommonRowID = RecordID;
                            await _T_ITR8140_001ATestInstrumentDataRepository.InsertOrReplaceAsync(ITRSeries_8000.iTR8140_001A_TestInstrumentData);
                        }
                        if (ITRSeries_8000.iTR_8140_001A_ContactResistanceTests.Count > 0)
                        {
                            var ContactResisTest = await _T_ITR8140_001A_ContactResisTestRepository.GetAsync();
                            long CRTID = ContactResisTest.Count() + 1;
                            ITRSeries_8000.iTR_8140_001A_ContactResistanceTests.ForEach(x => { x.RowID = CRTID++; x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_8000.CommonHeaderFooter.ID; x.TorqueMarkOkValue = x.TorqueMarkOk ? "Yes" : "No"; });

                            ITRSeries_8000.iTR_8140_001A_ContactResistanceTests.OrderBy(y => y.ID);
                            await _T_ITR8140_001A_ContactResisTestRepository.InsertOrReplaceAsync(ITRSeries_8000.iTR_8140_001A_ContactResistanceTests);
                        }
                        if (ITRSeries_8000.iTR8140_001A_InsulationResistanceTest.Count > 0)
                        {
                            var InsuResisTest = await _T_ITR8140_001AInsulationResistanceTestRepository.GetAsync();
                            long IRTID = InsuResisTest.Count() + 1;
                            ITRSeries_8000.iTR8140_001A_InsulationResistanceTest.ForEach(x => { x.RowID = IRTID++; x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_8000.CommonHeaderFooter.ID; });
                            await _T_ITR8140_001AInsulationResistanceTestRepository.InsertOrReplaceAsync(ITRSeries_8000.iTR8140_001A_InsulationResistanceTest);
                        }
                        if (ITRSeries_8000.iTR8140_001A_Dilectric_Test.Count > 0)
                        {
                            var DiaTest = await _T_ITR8140_001ADialectricTestRepository.GetAsync();
                            long DTID = DiaTest.Count() + 1;
                            ITRSeries_8000.iTR8140_001A_Dilectric_Test.ForEach(x => { x.RowID = DTID++; x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_8000.CommonHeaderFooter.ID; });
                            await _T_ITR8140_001ADialectricTestRepository.InsertOrReplaceAsync(ITRSeries_8000.iTR8140_001A_Dilectric_Test);
                        }
                    }
                }
                else if (ITRdata.ITR8100_002A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR8100_002AModel ITRSeries_8000 = ITRdata.ITR8100_002A;
                    if (String.IsNullOrEmpty(ITRSeries_8000.Error))
                    {
                        // long RecordID = 0;
                        if (ITRSeries_8000.CommonHeaderFooter != null)
                        {
                            ITRSeries_8000.CommonHeaderFooter.ROWID = RecordID;
                            ITRSeries_8000.CommonHeaderFooter.ModelName = Settings.ModelName;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITRSeries_8000.CommonHeaderFooter);
                            if (ITRSeries_8000.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITRSeries_8000.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITRSeries_8000.CommonHeaderFooter.ID; x.SignOffChecksheet = ITRSeries_8000.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITRSeries_8000.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_8000.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITRSeries_8000.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                            if (ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData != null)
                            {
                                var Instrumentdata8100_002A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID8100_002A = 1;
                                if (Instrumentdata8100_002A.Count > 0)
                                    InstID8100_002A = Instrumentdata8100_002A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8100_002A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITRSeries_8000.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }
                        if (ITRSeries_8000.ITR8100_002AVT_Data != null)
                        {
                            ITRSeries_8000.ITR8100_002AVT_Data.ModelName = Settings.ModelName;
                            ITRSeries_8000.ITR8100_002AVT_Data.CommonRowID = RecordID;
                            await _ITRRecords_8100_002ARepository.InsertOrReplaceAsync(ITRSeries_8000.ITR8100_002AVT_Data);
                        }
                        if (ITRSeries_8000.ITR8100_002AInsRes_Test.Count > 0)
                        {
                            var InsRes_Test = await _ITRRecords_8100_002A_InsRes_TestRepository.GetAsync();
                            long InsResTestID = InsRes_Test.Count() + 1;

                            ITRSeries_8000.ITR8100_002AInsRes_Test.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = InsResTestID++; });
                            await _ITRRecords_8100_002A_InsRes_TestRepository.InsertOrReplaceAsync(ITRSeries_8000.ITR8100_002AInsRes_Test);
                        }
                        if (ITRSeries_8000.ITR8100_002ARadio_Test.Count > 0)
                        {
                            var Radio_Test = await _ITRRecords_8100_002A_Radio_TestRepository.GetAsync();
                            long Radio_TestID = Radio_Test.Count() + 1;
                            ITRSeries_8000.ITR8100_002ARadio_Test.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = Radio_TestID++; });
                            await _ITRRecords_8100_002A_Radio_TestRepository.InsertOrReplaceAsync(ITRSeries_8000.ITR8100_002ARadio_Test);
                        }
                    }
                }
                else if (ITRdata.ITR8121_002A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR8121_002AModel ITR8121_002A = ITRdata.ITR8121_002A;
                    if (ITR8121_002A.CommonHeaderFooter != null)
                    {
                        ITR8121_002A.CommonHeaderFooter.ROWID = RecordID;
                        ITR8121_002A.CommonHeaderFooter.ModelName = Settings.ModelName;
                        await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITR8121_002A.CommonHeaderFooter);
                        if (ITR8121_002A.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                        {
                            ITR8121_002A.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                            {
                                x.ITRCommonID = ITR8121_002A.CommonHeaderFooter.ID; x.SignOffChecksheet = ITR8121_002A.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITR8121_002A.CommonHeaderFooter.Tag;
                                x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITR8121_002A.CommonHeaderFooter.ID;
                            });
                            await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITR8121_002A.CommonHeaderFooter.CommonHeaderFooterSignOff);
                        }
                        if (ITR8121_002A.CommonHeaderFooter.ITRInstrumentData != null)
                        {
                            var Instrumentdata8121_002A = await _ITRInstrumentDataRepository.GetAsync();
                            long InstID8121_002A = 1;
                            if (Instrumentdata8121_002A.Count > 0)
                                InstID8121_002A = Instrumentdata8121_002A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                            ITR8121_002A.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8121_002A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITR8121_002A.CommonHeaderFooter.ID; });
                            await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITR8121_002A.CommonHeaderFooter.ITRInstrumentData);
                        }
                    }
                    if (ITR8121_002A.TransformerRadioTests != null)
                    {
                        var TransformerRadioTest = await _ITR8121_002A_TransformerRadioTest.GetAsync();
                        long TRTID = TransformerRadioTest.Count() + 1;
                        ITR8121_002A.TransformerRadioTests.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = TRTID++; });
                        await _ITR8121_002A_TransformerRadioTest.InsertOrReplaceAsync(ITR8121_002A.TransformerRadioTests);
                    }
                    if (ITR8121_002A.InspectionControlAndACCs != null)
                    {
                        var InsConAux = await _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents.GetAsync();
                        long InsConAuxID = InsConAux.Count() + 1;
                        ITR8121_002A.InspectionControlAndACCs.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = InsConAuxID++; });
                        await _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents.InsertOrReplaceAsync(ITR8121_002A.InspectionControlAndACCs);
                    }
                    if (ITR8121_002A.ITR_8121_002A_Records != null)
                    {
                        ITR8121_002A.ITR_8121_002A_Records.ModelName = Settings.ModelName;
                        ITR8121_002A.ITR_8121_002A_Records.CommonRowID = RecordID;
                        await _ITR8121_002A_Records.InsertOrReplaceAsync(ITR8121_002A.ITR_8121_002A_Records);
                    }
                }
                else if (ITRdata.ITR_8260_002A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR_8260_002AModel ITR8260_002A = ITRdata.ITR_8260_002A;
                    if (ITR8260_002A.CommonHeaderFooter != null)
                    {
                        ITR8260_002A.CommonHeaderFooter.ROWID = RecordID;
                        ITR8260_002A.CommonHeaderFooter.ModelName = Settings.ModelName;
                        await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITR8260_002A.CommonHeaderFooter);
                        if (ITR8260_002A.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                        {
                            ITR8260_002A.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                            {
                                x.ITRCommonID = ITR8260_002A.CommonHeaderFooter.ID; x.SignOffChecksheet = ITR8260_002A.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITR8260_002A.CommonHeaderFooter.Tag;
                                x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITR8260_002A.CommonHeaderFooter.ID;
                            });
                            await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITR8260_002A.CommonHeaderFooter.CommonHeaderFooterSignOff);
                        }
                        if (ITR8260_002A.CommonHeaderFooter.ITRInstrumentData != null)
                        {
                            var Instrumentdata8260_002A = await _ITRInstrumentDataRepository.GetAsync();
                            long InstID8260_002A = 1;
                            if (Instrumentdata8260_002A.Count > 0)
                                InstID8260_002A = Instrumentdata8260_002A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                            ITR8260_002A.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8260_002A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITR8260_002A.CommonHeaderFooter.ID; });
                            await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITR8260_002A.CommonHeaderFooter.ITRInstrumentData);
                        }
                    }
                    if (ITR8260_002A.ITR_Body != null)
                    {
                        ITR8260_002A.ITR_Body.ModelName = Settings.ModelName;
                        ITR8260_002A.ITR_Body.CCMS_HEADERID = (int)ITR8260_002A.CommonHeaderFooter.ID;
                        ITR8260_002A.ITR_Body.ITRCommonID = ITR8260_002A.CommonHeaderFooter.ID;
                        ITR8260_002A.ITR_Body.CommonRowID = RecordID;
                        await _ITR_8260_002A_BodyRepository.InsertOrReplaceAsync(ITR8260_002A.ITR_Body);
                    }
                    if (ITR8260_002A.ITR_DielectricTestList != null)
                    {
                        var DielectricTest = await _ITR_8260_002A_DielectricTestRepository.GetAsync();
                        long DiTestID = DielectricTest.Count() + 1;
                        ITR8260_002A.ITR_DielectricTestList.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = DiTestID++; });
                        await _ITR_8260_002A_DielectricTestRepository.InsertOrReplaceAsync(ITR8260_002A.ITR_DielectricTestList);
                    }
                }
                else if (ITRdata.ITR8161_001A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR8161_001AModel ITRSeries_8000 = ITRdata.ITR8161_001A;
                    //var ITRSeries_7000 = JsonConvert.DeserializeObject<ITR8161_001AModel>(ITRSeries_7000Result);
                    if (String.IsNullOrEmpty(ITRSeries_8000.Error))
                    {
                        // long RecordID = 0;
                        if (ITRSeries_8000.CommonHeaderFooter != null)
                        {
                            ITRSeries_8000.CommonHeaderFooter.ROWID = RecordID;
                            ITRSeries_8000.CommonHeaderFooter.ModelName = Settings.ModelName;
                            // await _CommonHeaderFooterRepository.QueryAsync("Delet From T_ITRCommonHeaderFooter WHERE Tag = '" + Tag + "' AND ITRNumber = '"+ Checksheet + "' AND ModelName = '" + Settings.ModelName + "'");
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITRSeries_8000.CommonHeaderFooter);
                            if (ITRSeries_8000.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITRSeries_8000.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITRSeries_8000.CommonHeaderFooter.ID; x.SignOffChecksheet = ITRSeries_8000.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITRSeries_8000.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_8000.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITRSeries_8000.CommonHeaderFooter.CommonHeaderFooterSignOff);
                                var abcd = await _CommonHeaderFooterSignOffRepository.GetAsync(x => x.ITRCommonID == ITRSeries_8000.CommonHeaderFooter.ID);
                            }
                            if (ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData != null)
                            {
                                var Instrumentdata8161_001A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID8161_001A = 1;
                                if (Instrumentdata8161_001A.Count > 0)
                                    InstID8161_001A = Instrumentdata8161_001A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8161_001A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITRSeries_8000.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }
                        if (ITRSeries_8000.ITR_8161_001A_Body != null)
                        {
                            ITRSeries_8000.ITR_8161_001A_Body.ModelName = Settings.ModelName;
                            ITRSeries_8000.ITR_8161_001A_Body.CommonRowID = RecordID;
                            ITRSeries_8000.ITR_8161_001A_Body.CCMS_HEADERID = (int)ITRSeries_8000.CommonHeaderFooter.ID;
                            ITRSeries_8000.ITR_8161_001A_Body.ITRCommonID = ITRSeries_8000.CommonHeaderFooter.ID;
                            await _ITRRecords_8161_001A_BodyRepository.InsertOrReplaceAsync(ITRSeries_8000.ITR_8161_001A_Body);
                        }
                        if (ITRSeries_8000.ITR_8161_001A_InsRes.Count > 0)
                        {
                            var records = await _ITRRecords_8161_001A_InsResRepository.GetAsync();
                            long InsResID = records.Count() + 1;
                            ITRSeries_8000.ITR_8161_001A_InsRes.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = InsResID++; });
                            await _ITRRecords_8161_001A_InsResRepository.InsertOrReplaceAsync(ITRSeries_8000.ITR_8161_001A_InsRes);
                        }
                        if (ITRSeries_8000.ITR_8161_001A_ConRes.Count > 0)
                        {
                            var recordsConRes = await _ITRRecords_8161_001A_ConResRepository.GetAsync();
                            long ConResID = recordsConRes.Count() + 1;
                            ITRSeries_8000.ITR_8161_001A_ConRes.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = ConResID++; });
                            await _ITRRecords_8161_001A_ConResRepository.InsertOrReplaceAsync(ITRSeries_8000.ITR_8161_001A_ConRes);
                        }
                    }
                }
                else if (ITRdata.ITR8121_004A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR8121_004AModel ITRSeries_8121_004A = ITRdata.ITR8121_004A;
                    if (String.IsNullOrEmpty(ITRSeries_8121_004A.Error))
                    {
                        // long RecordID = 0;
                        if (ITRSeries_8121_004A.CommonHeaderFooter != null)
                        {
                            ITRSeries_8121_004A.CommonHeaderFooter.ROWID = RecordID;
                            ITRSeries_8121_004A.CommonHeaderFooter.ModelName = Settings.ModelName;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITRSeries_8121_004A.CommonHeaderFooter);
                            if (ITRSeries_8121_004A.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITRSeries_8121_004A.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITRSeries_8121_004A.CommonHeaderFooter.ID; x.SignOffChecksheet = ITRSeries_8121_004A.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITRSeries_8121_004A.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_8121_004A.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITRSeries_8121_004A.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                            if (ITRSeries_8121_004A.CommonHeaderFooter.ITRInstrumentData != null)
                            {
                                var Instrumentdata8121_004A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID8121_004A = 1;
                                if (Instrumentdata8121_004A.Count > 0)
                                    InstID8121_004A = Instrumentdata8121_004A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITRSeries_8121_004A.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8121_004A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITRSeries_8121_004A.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRSeries_8121_004A.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }
                        if (ITRSeries_8121_004A.ITR8121_004A_TestInstrumentData != null)
                        {
                            ITRSeries_8121_004A.ITR8121_004A_TestInstrumentData.ModelName = Settings.ModelName;
                            ITRSeries_8121_004A.ITR8121_004A_TestInstrumentData.CommonRowID = RecordID;
                            await _ITR8121_004ATestInstrumentDataRepository.InsertOrReplaceAsync(ITRSeries_8121_004A.ITR8121_004A_TestInstrumentData);
                        }
                        if (ITRSeries_8121_004A.ITR8121_004A_InispactionForControlAndAuxiliary.Count > 0)
                        {
                            var ControlAndAuxiliary = await _ITR8121_004AInCAndAuxiliaryRepository.GetAsync();
                            long ConAuxID = ControlAndAuxiliary.Count() + 1;
                            ITRSeries_8121_004A.ITR8121_004A_InispactionForControlAndAuxiliary.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = ConAuxID++; });
                            await _ITR8121_004AInCAndAuxiliaryRepository.InsertOrReplaceAsync(ITRSeries_8121_004A.ITR8121_004A_InispactionForControlAndAuxiliary);
                        }
                        if (ITRSeries_8121_004A.ITR8121_004A_TransformerRatioTest.Count > 0)
                        {
                            var TransRatioTest = await _ITR8121_004ATransformerRatioTestRepository.GetAsync();
                            long TransRatioTestID = TransRatioTest.Count() + 1;
                            ITRSeries_8121_004A.ITR8121_004A_TransformerRatioTest.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = TransRatioTestID++; });
                            await _ITR8121_004ATransformerRatioTestRepository.InsertOrReplaceAsync(ITRSeries_8121_004A.ITR8121_004A_TransformerRatioTest);
                        }
                    }
                }
                else if (ITRdata.ITR_8161_002A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR8161_002AModel ITRSeries_8161_002A = ITRdata.ITR_8161_002A;
                    if (String.IsNullOrEmpty(ITRSeries_8161_002A.Error))
                    {
                        if (ITRSeries_8161_002A.CommonHeaderFooter != null)
                        {
                            ITRSeries_8161_002A.CommonHeaderFooter.ROWID = RecordID;
                            ITRSeries_8161_002A.CommonHeaderFooter.ModelName = Settings.ModelName;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITRSeries_8161_002A.CommonHeaderFooter);
                            if (ITRSeries_8161_002A.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITRSeries_8161_002A.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITRSeries_8161_002A.CommonHeaderFooter.ID; x.SignOffChecksheet = ITRSeries_8161_002A.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITRSeries_8161_002A.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_8161_002A.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITRSeries_8161_002A.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                            if (ITRSeries_8161_002A.CommonHeaderFooter.ITRInstrumentData != null)
                            {
                                var Instrumentdata8121_004A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID8121_004A = 1;
                                if (Instrumentdata8121_004A.Count > 0)
                                    InstID8121_004A = Instrumentdata8121_004A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITRSeries_8161_002A.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8121_004A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITRSeries_8161_002A.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRSeries_8161_002A.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }
                        if (ITRSeries_8161_002A.Itr8161_002A_Body != null)
                        {
                            ITRSeries_8161_002A.Itr8161_002A_Body.ModelName = Settings.ModelName;
                            ITRSeries_8161_002A.Itr8161_002A_Body.CommonRowID = RecordID;
                            await _ITR8161_002A_BodyRepository.InsertOrReplaceAsync(ITRSeries_8161_002A.Itr8161_002A_Body);
                        }
                        if (ITRSeries_8161_002A.ITR8161_002A_DielectricTest.Count > 0)
                        {
                            var DielectricTest = await _ITR8161_002A_DielectricTestRepository.GetAsync();
                            long DielectricTestID = DielectricTest.Count() + 1;
                            ITRSeries_8161_002A.ITR8161_002A_DielectricTest.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = DielectricTestID++; });
                            await _ITR8161_002A_DielectricTestRepository.InsertOrReplaceAsync(ITRSeries_8161_002A.ITR8161_002A_DielectricTest);
                        }
                    }
                }
                else if (ITRdata.ITR_8000_101A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR8000_101AModel ITRSeries_8000_101A = ITRdata.ITR_8000_101A;
                    if (String.IsNullOrEmpty(ITRSeries_8000_101A.Error))
                    {
                        if (ITRSeries_8000_101A.CommonHeaderFooter != null)
                        {
                            ITRSeries_8000_101A.CommonHeaderFooter.ROWID = RecordID;
                            ITRSeries_8000_101A.CommonHeaderFooter.ModelName = Settings.ModelName;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITRSeries_8000_101A.CommonHeaderFooter);
                            if (ITRSeries_8000_101A.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITRSeries_8000_101A.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITRSeries_8000_101A.CommonHeaderFooter.ID; x.SignOffChecksheet = ITRSeries_8000_101A.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITRSeries_8000_101A.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_8000_101A.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITRSeries_8000_101A.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                            if (ITRSeries_8000_101A.CommonHeaderFooter.ITRInstrumentData != null)
                            {
                                var Instrumentdata8121_004A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID8121_004A = 1;
                                if (Instrumentdata8121_004A.Count > 0)
                                    InstID8121_004A = Instrumentdata8121_004A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITRSeries_8000_101A.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8121_004A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITRSeries_8000_101A.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRSeries_8000_101A.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }
                        if (ITRSeries_8000_101A.ITR_8000_101A_Generalnformation != null)
                        {
                            ITRSeries_8000_101A.ITR_8000_101A_Generalnformation.ModelName = Settings.ModelName;
                            ITRSeries_8000_101A.ITR_8000_101A_Generalnformation.CommonRowID = RecordID;
                            await _ITR8000_101A_GeneralnformationRepository.InsertOrReplaceAsync(ITRSeries_8000_101A.ITR_8000_101A_Generalnformation);
                        }
                        if (ITRSeries_8000_101A.ITR_8000_101A_RecordISBarrierDetails != null)
                        {
                            ITRSeries_8000_101A.ITR_8000_101A_RecordISBarrierDetails.ModelName = Settings.ModelName;
                            ITRSeries_8000_101A.ITR_8000_101A_RecordISBarrierDetails.CommonRowID = RecordID;
                            await _ITR8000_101A_RecordISBarrierDetailsRepository.InsertOrReplaceAsync(ITRSeries_8000_101A.ITR_8000_101A_RecordISBarrierDetails);
                        }
                    }
                }
                else if (ITRdata.ITR_8140_002A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR8140_002AModel ITR8140_002A = ITRdata.ITR_8140_002A;
                    if (String.IsNullOrEmpty(ITR8140_002A.Error))
                    {
                        if (ITR8140_002A.CommonHeaderFooter != null)
                        {
                            ITR8140_002A.CommonHeaderFooter.ROWID = RecordID;
                            ITR8140_002A.CommonHeaderFooter.ModelName = Settings.ModelName;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITR8140_002A.CommonHeaderFooter);
                            if (ITR8140_002A.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITR8140_002A.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITR8140_002A.CommonHeaderFooter.ID; x.SignOffChecksheet = ITR8140_002A.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITR8140_002A.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITR8140_002A.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITR8140_002A.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                            if (ITR8140_002A.CommonHeaderFooter.ITRInstrumentData != null)
                            {
                                var Instrumentdata8140_002A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID8140_002A = 1;
                                if (Instrumentdata8140_002A.Count > 0)
                                    InstID8140_002A = Instrumentdata8140_002A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITR8140_002A.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8140_002A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITR8140_002A.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITR8140_002A.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }
                        if (ITR8140_002A._8140_002A_Record != null)
                        {
                            ITR8140_002A._8140_002A_Record.ModelName = Settings.ModelName;
                            ITR8140_002A._8140_002A_Record.CommonRowID = RecordID;
                            await _ITR_8140_002A_RecordsRepository.InsertOrReplaceAsync(ITR8140_002A._8140_002A_Record);
                        }
                        if (ITR8140_002A._8140_002A_RecordsMechnical != null)
                        {
                            ITR8140_002A._8140_002A_RecordsMechnical.ModelName = Settings.ModelName;
                            ITR8140_002A._8140_002A_RecordsMechnical.CommonRowID = RecordID;
                            await _ITR_8140_002A_RecordsMechnicalOperation_RecordsRepository.InsertOrReplaceAsync(ITR8140_002A._8140_002A_RecordsMechnical);
                        }
                        if (ITR8140_002A._8140_002A_RecordsAnalogSignal != null)
                        {
                            ITR8140_002A._8140_002A_RecordsAnalogSignal.ModelName = Settings.ModelName;
                            ITR8140_002A._8140_002A_RecordsAnalogSignal.CommonRowID = RecordID;
                            await _ITR_8140_002A_RecordsAnalogSignalRepository.InsertOrReplaceAsync(ITR8140_002A._8140_002A_RecordsAnalogSignal);
                        }
                    }
                }
                else if (ITRdata.ITR_8140_004A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR_8140_004AModel ITR_8140_004A = ITRdata.ITR_8140_004A;
                    if (String.IsNullOrEmpty(ITR_8140_004A.Error))
                    {
                        if (ITR_8140_004A.CommonHeaderFooter != null)
                        {
                            ITR_8140_004A.CommonHeaderFooter.ROWID = RecordID;
                            ITR_8140_004A.CommonHeaderFooter.ModelName = Settings.ModelName;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITR_8140_004A.CommonHeaderFooter);
                            if (ITR_8140_004A.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITR_8140_004A.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITR_8140_004A.CommonHeaderFooter.ID; x.SignOffChecksheet = ITR_8140_004A.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITR_8140_004A.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITR_8140_004A.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITR_8140_004A.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                            if (ITR_8140_004A.CommonHeaderFooter.ITRInstrumentData != null)
                            {
                                var Instrumentdata8140_002A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID8140_002A = 1;
                                if (Instrumentdata8140_002A.Count > 0)
                                    InstID8140_002A = Instrumentdata8140_002A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITR_8140_004A.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8140_002A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITR_8140_004A.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITR_8140_004A.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }
                        if (ITR_8140_004A._8140_004A_Record != null)
                        {
                            ITR_8140_004A._8140_004A_Record.ModelName = Settings.ModelName;
                            ITR_8140_004A._8140_004A_Record.CommonRowID = RecordID;
                            ITR_8140_004A._8140_004A_Record.ITRNumber = ITR_8140_004A.CommonHeaderFooter.ITRNumber;
                            ITR_8140_004A._8140_004A_Record.TagNo = ITR_8140_004A.CommonHeaderFooter.Tag;
                            ITR_8140_004A._8140_004A_Record.ITRCommonID = ITR_8140_004A.CommonHeaderFooter.ID;
                            ITR_8140_004A._8140_004A_Record.CCMS_HEADERID = (int)ITR_8140_004A.CommonHeaderFooter.ID;
                            await _ITR_8140_004A_RecordsRepository.InsertOrReplaceAsync(ITR_8140_004A._8140_004A_Record);
                        }
                    }
                }
                else if (ITRdata.ITR8170_002A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR8170_002AModel ITRSeries_8170_002A = ITRdata.ITR8170_002A;
                    if (String.IsNullOrEmpty(ITRSeries_8170_002A.Error))
                    {
                        if (ITRSeries_8170_002A.CommonHeaderFooter != null)
                        {
                            ITRSeries_8170_002A.CommonHeaderFooter.ROWID = RecordID;
                            ITRSeries_8170_002A.CommonHeaderFooter.ModelName = Settings.ModelName;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITRSeries_8170_002A.CommonHeaderFooter);
                            if (ITRSeries_8170_002A.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITRSeries_8170_002A.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITRSeries_8170_002A.CommonHeaderFooter.ID; x.SignOffChecksheet = ITRSeries_8170_002A.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITRSeries_8170_002A.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_8170_002A.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITRSeries_8170_002A.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                            if (ITRSeries_8170_002A.CommonHeaderFooter.ITRInstrumentData != null)
                            {
                                var Instrumentdata8121_004A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID8121_004A = 1;
                                if (Instrumentdata8121_004A.Count > 0)
                                    InstID8121_004A = Instrumentdata8121_004A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITRSeries_8170_002A.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8121_004A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITRSeries_8170_002A.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRSeries_8170_002A.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }
                        if (ITRSeries_8170_002A.ITR_8170_002A_InsRes != null)
                        {
                            ITRSeries_8170_002A.ITR_8170_002A_InsRes.ModelName = Settings.ModelName;
                            ITRSeries_8170_002A.ITR_8170_002A_InsRes.CommonRowID = RecordID;
                            await _ITR_8170_002A_InsResRepository.InsertOrReplaceAsync(ITRSeries_8170_002A.ITR_8170_002A_InsRes);
                        }
                        if (ITRSeries_8170_002A.ITR_8170_002A_IndifictionLamp != null)
                        {
                            ITRSeries_8170_002A.ITR_8170_002A_IndifictionLamp.ModelName = Settings.ModelName;
                            ITRSeries_8170_002A.ITR_8170_002A_IndifictionLamp.CommonRowID = RecordID;
                            await _ITR_8170_002A_IndifictionLampRepository.InsertOrReplaceAsync(ITRSeries_8170_002A.ITR_8170_002A_IndifictionLamp);
                        }
                        if (ITRSeries_8170_002A.ITR_8170_002A_Voltage_Reading.Count > 0)
                        {
                            var records = await _ITRRecords_8170_002A_Voltage_ReadingRepository.GetAsync();
                            long InsResID = records.Count() + 1;
                            ITRSeries_8170_002A.ITR_8170_002A_Voltage_Reading.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = InsResID++; });
                            await _ITRRecords_8170_002A_Voltage_ReadingRepository.InsertOrReplaceAsync(ITRSeries_8170_002A.ITR_8170_002A_Voltage_Reading);
                        }
                    }
                }

                else if (ITRdata.ITR8300_003A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR_8300_003AModel ITRSeries_8300_003A = ITRdata.ITR8300_003A;
                    if (String.IsNullOrEmpty(ITRSeries_8300_003A.Error))
                    {
                        if (ITRSeries_8300_003A.CommonHeaderFooter != null)
                        {
                            ITRSeries_8300_003A.CommonHeaderFooter.ROWID = RecordID;
                            ITRSeries_8300_003A.CommonHeaderFooter.ModelName = Settings.ModelName;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITRSeries_8300_003A.CommonHeaderFooter);
                            if (ITRSeries_8300_003A.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITRSeries_8300_003A.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITRSeries_8300_003A.CommonHeaderFooter.ID; x.SignOffChecksheet = ITRSeries_8300_003A.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITRSeries_8300_003A.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_8300_003A.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITRSeries_8300_003A.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                            if (ITRSeries_8300_003A.CommonHeaderFooter.ITRInstrumentData != null)
                            {
                                var Instrumentdata8121_004A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID8121_004A = 1;
                                if (Instrumentdata8121_004A.Count > 0)
                                    InstID8121_004A = Instrumentdata8121_004A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITRSeries_8300_003A.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8121_004A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITRSeries_8300_003A.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRSeries_8300_003A.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }
                        if (ITRSeries_8300_003A.ITR_Body != null)
                        {
                            ITRSeries_8300_003A.ITR_Body.ModelName = Settings.ModelName;
                            ITRSeries_8300_003A.ITR_Body.CommonRowID = RecordID;
                            await _ITR_8300_003A_BodyRepository.InsertOrReplaceAsync(ITRSeries_8300_003A.ITR_Body);
                        }
                       
                        if (ITRSeries_8300_003A.ITR_IlluminList.Count > 0)
                        {
                            var records = await _ITR_8300_003A_IlluminRepository.GetAsync();
                            long InsResID = records.Count() + 1;
                            ITRSeries_8300_003A.ITR_IlluminList.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = InsResID++; });
                            await _ITR_8300_003A_IlluminRepository.InsertOrReplaceAsync(ITRSeries_8300_003A.ITR_IlluminList);
                        }
                    }
                }
                else if (ITRdata.ITR_8170_007A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR8170_007AModel ITR8170_007A = ITRdata.ITR_8170_007A;
                    if (String.IsNullOrEmpty(ITR8170_007A.Error))
                    {
                        if (ITR8170_007A.CommonHeaderFooter != null)
                        {
                            ITR8170_007A.CommonHeaderFooter.ROWID = RecordID;
                            ITR8170_007A.CommonHeaderFooter.ModelName = Settings.ModelName;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITR8170_007A.CommonHeaderFooter);
                            if (ITR8170_007A.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITR8170_007A.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITR8170_007A.CommonHeaderFooter.ID; x.SignOffChecksheet = ITR8170_007A.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITR8170_007A.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITR8170_007A.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITR8170_007A.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                            if (ITR8170_007A.CommonHeaderFooter.ITRInstrumentData != null)
                            {
                                var Instrumentdata8170_007A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID8170_007A = 1;
                                if (Instrumentdata8170_007A.Count > 0)
                                    InstID8170_007A = Instrumentdata8170_007A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITR8170_007A.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8170_007A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITR8170_007A.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITR8170_007A.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }
                        if (ITR8170_007A.ITR_8170_007A_OP_Function_Test != null)
                        {
                            ITR8170_007A.ITR_8170_007A_OP_Function_Test.ModelName = Settings.ModelName;
                            ITR8170_007A.ITR_8170_007A_OP_Function_Test.CommonRowID = RecordID;
                            await _ITR_8170_007A_OP_Function_TestRepository.InsertOrReplaceAsync(ITR8170_007A.ITR_8170_007A_OP_Function_Test);
                        }
                    }
                }

            }
            catch (Exception e)
            {
            }
            return true;
        }
        private async void GetTestEquipment()
        {
            string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.Get_TestEquipmentData(Settings.ProjectID, Settings.CurrentDB), Settings.AccessToken);
            List<T_TestEquipmentData> TestEquipmentDataList = JsonConvert.DeserializeObject<List<T_TestEquipmentData>>(JsonString);
            if (TestEquipmentDataList.Count > 0)
            {

                await _TestEquipmentDataRepository.QueryAsync("Delete from T_TestEquipmentData Where ProjectID ='" + Settings.ProjectID + "'");
                //TestEquipmentDataList.ForEach(x => x.ModelName = Settings.ModelName);
                await _TestEquipmentDataRepository.InsertOrReplaceAsync(TestEquipmentDataList);
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

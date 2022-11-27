using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.DataBase.DataTables.WorkPack;
using JGC.Models;
using JGC.ViewModel;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.Common.Extentions;
using JGC.DataBase.DataTables.Completions;
using JGC.DataBase.DataTables.ModsCore;
using JGC.DataBase.DataTables.ITR;

namespace JGC.ViewModels.Completions
{
    public class CompletionSettingViewModel : BaseViewModel
    {
        private readonly IRepository<T_Setting> _settingtable;
        private readonly IRepository<T_UserProjects> _userProjectsRepository;       
        private readonly IRepository<T_UserControl> _UserControlDetails;

        private readonly IRepository<T_DataRefs> _DataRefsRepository;
        private readonly IRepository<T_CompletionsDrawings> _CompletionsDrawingsRepository;
        private readonly IRepository<T_Handover> _HandoverRepository;
        private readonly IRepository<T_HandoverWorkpacks> _HandoverWorkpacksRepository;
        private readonly IRepository<T_HeaderItems> _HeaderItemsRepository;
        private readonly IRepository<T_JOINT> _JOINTRepository;
        private readonly IRepository<T_JOINT_DRAWINGS> _JOINT_DRAWINGSRepository;
        private readonly IRepository<T_JOINT_FLANGEMANAGEMENTCHECKLIST> _JOINT_FLANGEMANAGEMENTCHECKLISTRepository;
        private readonly IRepository<T_JOINT_HEADER> _JOINT_HEADERRepository;
        private readonly IRepository<T_JOINT_QUESTIONS> _JOINT_QUESTIONSRepository;
        private readonly IRepository<T_JS_JOINT_LEAK_RECORDS> _JOINT_LEAK_RECORDSRepository;
        private readonly IRepository<T_JS_PUNCH_LIST> _PUNCH_LISTRepository;
        private readonly IRepository<T_JS_SIGN_OFF> _SIGN_OFFRepository;
        private readonly IRepository<T_PUNCH_SELECTIONS> _PUNCH_SELECTIONSRepository;
        private readonly IRepository<T_CompletionsPunchList> _CompletionsPunchListRepository;
        private readonly IRepository<T_PunchListDropDowns> _PunchListDropDownsRepository;
        private readonly IRepository<T_SignOff> _SignOffRepository;
        private readonly IRepository<T_SyncedTags> _SyncedTagsRepository;
        private readonly IRepository<T_Tag_headers> _Tag_headersRepository;

        private readonly IRepository<T_CHECKSHEET> _CHECKSHEETRepository;
        private readonly IRepository<T_CHECKSHEET_PER_TAG> _CHECKSHEET_PER_TAGRepository;
        private readonly IRepository<T_CHECKSHEET_QUESTION> _CHECKSHEET_QUESTIONRepository;
        private readonly IRepository<T_SYSTEM> _SYSTEMRepository;
        private readonly IRepository<T_TAG> _TAGRepository;
        private readonly IRepository<T_TAG_SHEET_ANSWER> _TAG_SHEET_ANSWERRepository;
        private readonly IRepository<T_TAG_SHEET_HEADER> _TAG_SHEET_HEADERRepository;
        private readonly IRepository<T_SignOffHeader> _SignOffHeaderRepository;
        private readonly IRepository<T_WorkPack> _WorkPackRepository;

        private readonly IRepository<T_CompanyCategoryCode> _CompanyCategoryCodeRepository;
        private readonly IRepository<T_CompletionSystems> _CompletionSystemsRepository;
        private readonly IRepository<T_PunchComponent> _PunchComponentRepository;
        private readonly IRepository<T_PunchSystem> _PunchSystemRepository;
        private readonly IRepository<T_PunchPCWBS> _PunchPCWBSRepository;
        private readonly IRepository<T_PunchFWBS> _PunchFWBSRepository;
        private readonly IRepository<T_SectionCode> _SectionCodeRepository;

        private readonly IRepository<T_ITRCommonHeaderFooter> _CommonHeaderFooterRepository;
        private readonly IRepository<T_ITRRecords_30A_31A> _RecordsRepository;
        private readonly IRepository<T_ITRTubeColors> _TubeColorsRepository;
        private readonly IRepository<T_ITRRecords_040A_041A_042A> _Records_04XARepository;
        private readonly IRepository<T_ITRInsulationDetails> _InsulationDetailsRepository;
        private readonly IRepository<T_CommonHeaderFooterSignOff> _CommonHeaderFooterSignOffRepository;
        private readonly IRepository<T_ITRRecords_080A_090A_091A> _Records_080A_09XARepository;
        private readonly IRepository<T_ITR8000_003ARecords> _Records_8000003ARepository;
        private readonly IRepository<T_ITR8000_003A_AcceptanceCriteria> _Records_8000003A_AcceptanceCriteriaRepository;
        private readonly IRepository<T_ITRRecords_8100_002A> _ITRRecords_8100_002ARepository;
        private readonly IRepository<T_ITRRecords_8100_002A_InsRes_Test> _ITRRecords_8100_002A_InsRes_TestRepository;
        private readonly IRepository<T_ITRRecords_8100_002A_Radio_Test> _ITRRecords_8100_002A_Radio_TestRepository;
        private readonly IRepository<T_ITRRecords_8161_001A_Body> _ITRRecords_8161_001A_BodyRepository;
        private readonly IRepository<T_ITRRecords_8161_001A_InsRes> _ITRRecords_8161_001A_InsResRepository;
        private readonly IRepository<T_ITRRecords_8161_001A_ConRes> _ITRRecords_8161_001A_ConResRepository;
        private readonly IRepository<T_ITR8121_004AInCAndAuxiliary> _ITR8121_004AInCAndAuxiliaryRepository;
        private readonly IRepository<T_ITR8121_004ATransformerRatioTest> _ITR8121_004ATransformerRatioTestRepository;
        private readonly IRepository<T_ITR8121_004ATestInstrumentData> _ITR8121_004ATestInstrumentDataRepository;
        private readonly IRepository<T_ITR8140_001A_ContactResisTest> _T_ITR8140_001A_ContactResisTestRepository;
        private readonly IRepository<T_ITR8140_001AInsulationResistanceTest> _T_ITR8140_001AInsulationResistanceTestRepository;
        private readonly IRepository<T_ITR8140_001ADialectricTest> _T_ITR8140_001ADialectricTestRepository;
        private readonly IRepository<T_ITR8140_001ATestInstrucitonData> _T_ITR8140_001ATestInstrumentDataRepository;


        private readonly IRepository<T_ITR8100_001A_CTdata> _ITR8100_001A_CTdataRepository;
        private readonly IRepository<T_ITR8100_001A_InsulationResistanceTest> _ITR8100_001A_IRTestRepository;
        private readonly IRepository<T_ITR8100_001A_RatioTest> _ITR8100_001A_RatioTestRepository;
        private readonly IRepository<T_ITR8100_001A_TestInstrumentData> _ITR8100_001A_TIDataRepository;
        private readonly IRepository<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents> _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents;
        private readonly IRepository<T_ITR8121_002A_Records> _ITR8121_002A_Records;
        private readonly IRepository<T_ITR8121_002A_TransformerRadioTest> _ITR8121_002A_TransformerRadioTest;
        private readonly IRepository<T_ITR_8260_002A_Body> _ITR_8260_002A_BodyRepository;
        private readonly IRepository<T_ITR_8260_002A_DielectricTest> _ITR_8260_002A_DielectricTestRepository;

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
        private readonly IRepository<T_ITRRecords_8170_002A_Voltage_Reading> _ITRRecords_8170_002A_Voltage_ReadingRepository;
        private readonly IRepository<T_ITR_8170_002A_IndifictionLamp> _ITR_8170_002A_IndifictionLampRepository;
        private readonly IRepository<T_ITR_8170_002A_InsRes> _ITR_8170_002A_InsResRepository;
        private readonly IRepository<T_ITR_8300_003A_Body> _ITR_8300_003A_BodyRepository;
        private readonly IRepository<T_ITR_8300_003A_Illumin> _ITR_8300_003A_IlluminRepository;

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

        public CompletionSettingViewModel(INavigationService navigationService,
               IHttpHelper httpHelper,
               ICheckValidLogin _checkValidLogin,
               IUserDialogs _userDialogs,
               IRepository<T_Setting> _settingtable,
               IRepository<T_UserProjects> _userProjectsRepository,                                
               IRepository<T_UserControl> _UserControlDetails,
                                
               IRepository<T_DataRefs> _DataRefsRepository,
               IRepository<T_CompletionsDrawings> _CompletionsDrawingsRepository,
               IRepository<T_Handover> _HandoverRepository,
               IRepository<T_HandoverWorkpacks> _HandoverWorkpacksRepository,
               IRepository<T_HeaderItems> _HeaderItemsRepository,
               IRepository<T_JOINT> _JOINTRepository,
               IRepository<T_JOINT_DRAWINGS> _JOINT_DRAWINGSRepository,
               IRepository<T_JOINT_FLANGEMANAGEMENTCHECKLIST> _JOINT_FLANGEMANAGEMENTCHECKLISTRepository,
               IRepository<T_JOINT_HEADER> _JOINT_HEADERRepository,
               IRepository<T_JOINT_QUESTIONS> _JOINT_QUESTIONSRepository,
               IRepository<T_JS_JOINT_LEAK_RECORDS> _JOINT_LEAK_RECORDSRepository,
               IRepository<T_JS_PUNCH_LIST> _PUNCH_LISTRepository,
               IRepository<T_JS_SIGN_OFF> _SIGN_OFFRepository,
               IRepository<T_PUNCH_SELECTIONS> _PUNCH_SELECTIONSRepository,
               IRepository<T_CompletionsPunchList> _CompletionsPunchListRepository,
               IRepository<T_PunchListDropDowns> _PunchListDropDownsRepository,
               IRepository<T_SignOff> _SignOffRepository,
               IRepository<T_SyncedTags> _SyncedTagsRepository,
               IRepository<T_Tag_headers> _Tag_headersRepository,

               IRepository<T_CHECKSHEET> _CHECKSHEETRepository,
               IRepository<T_CHECKSHEET_PER_TAG> _CHECKSHEET_PER_TAGRepository,
               IRepository<T_CHECKSHEET_QUESTION> _CHECKSHEET_QUESTIONRepository,
               IRepository<T_SYSTEM> _SYSTEMRepository,
               IRepository<T_TAG> _TAGRepository,
               IRepository<T_TAG_SHEET_ANSWER> _TAG_SHEET_ANSWERRepository,
               IRepository<T_TAG_SHEET_HEADER> _TAG_SHEET_HEADERRepository,
               IRepository<T_SignOffHeader> _SignOffHeaderRepository,
               IRepository<T_WorkPack> _WorkPackRepository,
               IRepository<T_CompanyCategoryCode> _CompanyCategoryCodeRepository,
               IRepository<T_CompletionSystems> _CompletionSystemsRepository,
               IRepository<T_PunchComponent> _PunchComponentRepository,
               IRepository<T_PunchSystem> _PunchSystemRepository,
               IRepository<T_PunchPCWBS> _PunchPCWBSRepository,
               IRepository<T_PunchFWBS> _PunchFWBSRepository,
               IRepository<T_SectionCode> _SectionCodeRepository,
               IRepository<T_ITRCommonHeaderFooter> _CommonHeaderFooterRepository,
               IRepository<T_ITRRecords_30A_31A> _RecordsRepository,
               IRepository<T_ITRTubeColors> _TubeColorsRepository,
               IRepository<T_CommonHeaderFooterSignOff> _CommonHeaderFooterSignOffRepository,
               IRepository<T_ITRRecords_040A_041A_042A> _Records_04XARepository,
               IRepository<T_ITRInsulationDetails> _InsulationDetailsRepository,
               IRepository<T_ITR8000_003ARecords> _Records_8000003ARepository,
               IRepository<T_ITR8000_003A_AcceptanceCriteria> _Records_8000003A_AcceptanceCriteriaRepository,
               IRepository<T_ITRRecords_8100_002A> _ITRRecords_8100_002ARepository,
               IRepository<T_ITRRecords_8100_002A_InsRes_Test> _ITRRecords_8100_002A_InsRes_TestRepository,
               IRepository<T_ITRRecords_8100_002A_Radio_Test> _ITRRecords_8100_002A_Radio_TestRepository,
               IRepository<T_ITRRecords_8161_001A_Body> _ITRRecords_8161_001A_BodyRepository,
               IRepository<T_ITRRecords_8161_001A_InsRes> _ITRRecords_8161_001A_InsResRepository,
               IRepository<T_ITRRecords_8161_001A_ConRes> _ITRRecords_8161_001A_ConResRepository,
               IRepository<T_ITRRecords_080A_090A_091A> _Records_080A_09XARepository,
               IRepository<T_ITR8121_004AInCAndAuxiliary> _ITR8121_004AInCAndAuxiliaryRepository,
               IRepository<T_ITR8121_004ATransformerRatioTest> _ITR8121_004ATransformerRatioTestRepository,
               IRepository<T_ITR8121_004ATestInstrumentData> _ITR8121_004ATestInstrumentDataRepository,
               IRepository<T_ITR8140_001A_ContactResisTest> _T_ITR8140_001A_ContactResisTestRepository,
               IRepository<T_ITR8140_001AInsulationResistanceTest> _T_ITR8140_001AInsulationResistanceTestRepository,
               IRepository<T_ITR8140_001ADialectricTest> _T_ITR8140_001ADialectricTestRepository,
               IRepository<T_ITR8140_001ATestInstrucitonData> _T_ITR8140_001ATestInstrumentDataRepository,
               IRepository<T_ITR8100_001A_CTdata> _ITR8100_001A_CTdataRepository,
               IRepository<T_ITR8100_001A_InsulationResistanceTest> _ITR8100_001A_IRTestRepository,
               IRepository<T_ITR8100_001A_RatioTest> _ITR8100_001A_RatioTestRepository,
               IRepository<T_ITR8100_001A_TestInstrumentData> _ITR8100_001A_TIDataRepository,
               IRepository<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents> _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents,
               IRepository<T_ITR8121_002A_Records> _ITR8121_002A_Records,
               IRepository<T_ITR8121_002A_TransformerRadioTest> _ITR8121_002A_TransformerRadioTest,
               IRepository<T_ITR_8260_002A_Body> _ITR_8260_002A_BodyRepository,
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
                IRepository<T_ITRRecords_8170_002A_Voltage_Reading> _ITRRecords_8170_002A_Voltage_ReadingRepository,
                IRepository<T_ITR_8170_002A_IndifictionLamp> _ITR_8170_002A_IndifictionLampRepository,
                IRepository<T_ITR_8170_002A_InsRes> _ITR_8170_002A_InsResRepository,
               IRepository<T_ITR_8300_003A_Body> _ITR_8300_003A_BodyRepository,
               IRepository<T_ITR_8300_003A_Illumin> _ITR_8300_003A_IlluminRepository)


        : base(navigationService, httpHelper, _checkValidLogin)
        {
            this._settingtable = _settingtable;
            this._userDialogs = _userDialogs;
            this._userProjectsRepository = _userProjectsRepository;           
            this._UserControlDetails = _UserControlDetails;
            this._DataRefsRepository = _DataRefsRepository;
            this._CompletionsDrawingsRepository = _CompletionsDrawingsRepository;
            this._HandoverRepository = _HandoverRepository;
            this._HandoverWorkpacksRepository = _HandoverWorkpacksRepository;
            this._HeaderItemsRepository = _HeaderItemsRepository;
            this._JOINTRepository = _JOINTRepository;
            this._JOINT_DRAWINGSRepository = _JOINT_DRAWINGSRepository;
            this._JOINT_FLANGEMANAGEMENTCHECKLISTRepository = _JOINT_FLANGEMANAGEMENTCHECKLISTRepository;
            this._JOINT_HEADERRepository = _JOINT_HEADERRepository;
            this._JOINT_QUESTIONSRepository = _JOINT_QUESTIONSRepository;
            this._JOINT_LEAK_RECORDSRepository = _JOINT_LEAK_RECORDSRepository;
            this._PUNCH_LISTRepository = _PUNCH_LISTRepository;
            this._SIGN_OFFRepository = _SIGN_OFFRepository;
            this._PUNCH_SELECTIONSRepository = _PUNCH_SELECTIONSRepository;
            this._CompletionsPunchListRepository = _CompletionsPunchListRepository;
            this._PunchListDropDownsRepository = _PunchListDropDownsRepository;
            this._SignOffRepository = _SignOffRepository;
            this._SyncedTagsRepository = _SyncedTagsRepository;
            this._Tag_headersRepository = _Tag_headersRepository;
            this._CHECKSHEETRepository = _CHECKSHEETRepository;
            this._CHECKSHEET_PER_TAGRepository = _CHECKSHEET_PER_TAGRepository;
            this._CHECKSHEET_QUESTIONRepository = _CHECKSHEET_QUESTIONRepository;
            this._SYSTEMRepository = _SYSTEMRepository;
            this._TAGRepository = _TAGRepository;
            this._TAG_SHEET_ANSWERRepository = _TAG_SHEET_ANSWERRepository;
            this._TAG_SHEET_HEADERRepository = _TAG_SHEET_HEADERRepository;
            this._SignOffHeaderRepository = _SignOffHeaderRepository;
            this._WorkPackRepository = _WorkPackRepository;
            this._CompanyCategoryCodeRepository = _CompanyCategoryCodeRepository;
            this._CompletionSystemsRepository = _CompletionSystemsRepository;
            this._PunchComponentRepository = _PunchComponentRepository;
            this._PunchSystemRepository = _PunchSystemRepository;
            this._PunchPCWBSRepository = _PunchPCWBSRepository;
            this._PunchFWBSRepository = _PunchFWBSRepository;
            this._SectionCodeRepository = _SectionCodeRepository;
            this._CommonHeaderFooterRepository = _CommonHeaderFooterRepository;
            this._RecordsRepository = _RecordsRepository;
            this._TubeColorsRepository = _TubeColorsRepository;
            this._Records_04XARepository = _Records_04XARepository;
            this._InsulationDetailsRepository = _InsulationDetailsRepository;
            this._CommonHeaderFooterSignOffRepository = _CommonHeaderFooterSignOffRepository;
            this._Records_8000003ARepository = _Records_8000003ARepository;
            this._Records_8000003A_AcceptanceCriteriaRepository = _Records_8000003A_AcceptanceCriteriaRepository;
            this._Records_080A_09XARepository = _Records_080A_09XARepository;
            this._ITRRecords_8100_002ARepository = _ITRRecords_8100_002ARepository;
            this._ITRRecords_8100_002A_InsRes_TestRepository = _ITRRecords_8100_002A_InsRes_TestRepository;
            this._ITRRecords_8100_002A_Radio_TestRepository = _ITRRecords_8100_002A_Radio_TestRepository;
            this._ITRRecords_8161_001A_BodyRepository = _ITRRecords_8161_001A_BodyRepository;
            this._ITRRecords_8161_001A_InsResRepository = _ITRRecords_8161_001A_InsResRepository;
            this._ITRRecords_8161_001A_ConResRepository = _ITRRecords_8161_001A_ConResRepository;
            this._ITR8121_004AInCAndAuxiliaryRepository = _ITR8121_004AInCAndAuxiliaryRepository;
            this._ITR8121_004ATransformerRatioTestRepository = _ITR8121_004ATransformerRatioTestRepository;
            this._ITR8121_004ATestInstrumentDataRepository = _ITR8121_004ATestInstrumentDataRepository;
            this._T_ITR8140_001A_ContactResisTestRepository = _T_ITR8140_001A_ContactResisTestRepository;
            this._T_ITR8140_001AInsulationResistanceTestRepository = _T_ITR8140_001AInsulationResistanceTestRepository;
            this._T_ITR8140_001ADialectricTestRepository = _T_ITR8140_001ADialectricTestRepository;
            this._T_ITR8140_001ATestInstrumentDataRepository = _T_ITR8140_001ATestInstrumentDataRepository;

            this._ITR8100_001A_CTdataRepository = _ITR8100_001A_CTdataRepository;
            this._ITR8100_001A_IRTestRepository = _ITR8100_001A_IRTestRepository;
            this._ITR8100_001A_RatioTestRepository = _ITR8100_001A_RatioTestRepository;
            this._ITR8100_001A_TIDataRepository = _ITR8100_001A_TIDataRepository;
            this._ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents = _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents;
            this._ITR8121_002A_Records = _ITR8121_002A_Records;
            this._ITR8121_002A_TransformerRadioTest = _ITR8121_002A_TransformerRadioTest;
            this._ITR_8260_002A_BodyRepository = _ITR_8260_002A_BodyRepository;
            this._ITR_8260_002A_DielectricTestRepository = _ITR_8260_002A_DielectricTestRepository;
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
            this._ITRRecords_8170_002A_Voltage_ReadingRepository = _ITRRecords_8170_002A_Voltage_ReadingRepository;
            this._ITR_8170_002A_IndifictionLampRepository = _ITR_8170_002A_IndifictionLampRepository;
            this._ITR_8170_002A_InsResRepository = _ITR_8170_002A_InsResRepository;
            this._ITR_8300_003A_BodyRepository = _ITR_8300_003A_BodyRepository;
            this._ITR_8300_003A_IlluminRepository = _ITR_8300_003A_IlluminRepository;

            Button_BGColor = Settings.ModuleName == "EReporter" ? "#FB1610" : Settings.ModuleName == "TestPackage" ? "#C4BB46" : Settings.ModuleName == "JobSetting" ? "#3B87C7" : "Gray";
            PageHeaderText = "Setting";
            CheckUser();
        }

        private async void CheckUser()
        {
            string ID = Settings.CompletionUserID.ToString();
            var check = await _UserControlDetails.GetAsync(x =>x.ID == ID);
            if (check.FirstOrDefault().ModsUser)
            {
                IsVisibleFactoryReset = true;
            }
            else
            {
                IsVisibleFactoryReset = false;
            }
        }
        private void OnClickButton(string param)
        {
            if (param == "FactoryReset")
            {
                FactoryReset();
            }
            else if (param == "ClearSaveData")
            {
                ClearSaveData();
            }
            else if (param == "PrivacyPolicy")
            {
                Device.OpenUri(new System.Uri("https://www.modsvm.com/privacy-policy"));
            }
        }        
        public async void FactoryReset()
        {
            if (await _userDialogs.ConfirmAsync("Are you sure you want to restore to factory default?", "Factory Reset", "Yes", "No"))
            {
                try
                {
                    //var UserProjectList = await _userProjectsRepository.GetAsync();
                    //if (UserProjectList.Count > 0)
                    await _userProjectsRepository.DeleteAll();
                    await _UserControlDetails.DeleteAll();
                    await _DataRefsRepository.DeleteAll();
                    await _CompletionsDrawingsRepository.DeleteAll();
                    await _HandoverRepository.DeleteAll();
                    await _HandoverWorkpacksRepository.DeleteAll();
                    await _HeaderItemsRepository.DeleteAll();
                    await _JOINTRepository.DeleteAll();
                    await _JOINT_DRAWINGSRepository.DeleteAll();
                    await _JOINT_FLANGEMANAGEMENTCHECKLISTRepository.DeleteAll();
                    //await _JOINT_HEADERRepository.DeleteAll();
                    await _JOINT_QUESTIONSRepository.DeleteAll();
                    await _JOINT_LEAK_RECORDSRepository.DeleteAll();
                    await _PUNCH_LISTRepository.DeleteAll();
                    await _SIGN_OFFRepository.DeleteAll();
                    await _PUNCH_SELECTIONSRepository.DeleteAll();
                    await _CompletionsPunchListRepository.DeleteAll();
                    await _PunchListDropDownsRepository.DeleteAll();
                    await _SignOffRepository.DeleteAll();
                    await _SyncedTagsRepository.DeleteAll();
                    await _Tag_headersRepository.DeleteAll();
                    await _CHECKSHEETRepository.DeleteAll();
                    await _CHECKSHEET_PER_TAGRepository.DeleteAll();
                    await _CHECKSHEET_QUESTIONRepository.DeleteAll();
                    await _SYSTEMRepository.DeleteAll();
                    await _TAGRepository.DeleteAll();
                    await _TAG_SHEET_ANSWERRepository.DeleteAll();
                    await _TAG_SHEET_HEADERRepository.DeleteAll();
                    await _SignOffHeaderRepository.DeleteAll();
                    //await _WorkPackRepository.DeleteAll();

                    await _CompanyCategoryCodeRepository.DeleteAll();
                    await _CompletionSystemsRepository.DeleteAll();
                    await _PunchComponentRepository.DeleteAll();
                    await _PunchSystemRepository.DeleteAll();
                    await _PunchPCWBSRepository.DeleteAll();
                    await _PunchFWBSRepository.DeleteAll();
                    await _SectionCodeRepository.DeleteAll();

                    await _CommonHeaderFooterRepository.DeleteAll();
                    await _RecordsRepository.DeleteAll();
                    await _TubeColorsRepository.DeleteAll();
                    await _Records_04XARepository.DeleteAll();
                    await _InsulationDetailsRepository.DeleteAll();
                    await _CommonHeaderFooterSignOffRepository.DeleteAll();
                    await _Records_080A_09XARepository.DeleteAll();
                    await _ITR8121_004AInCAndAuxiliaryRepository.DeleteAll();
                    await _ITR8121_004ATransformerRatioTestRepository.DeleteAll();
                    await _ITR8121_004ATestInstrumentDataRepository.DeleteAll();
                    await _Records_8000003ARepository.DeleteAll();
                    await _Records_8000003A_AcceptanceCriteriaRepository.DeleteAll();
                    await _T_ITR8140_001A_ContactResisTestRepository.DeleteAll();
                    await _T_ITR8140_001AInsulationResistanceTestRepository.DeleteAll();
                    await _T_ITR8140_001ADialectricTestRepository.DeleteAll();
                    await _T_ITR8140_001ATestInstrumentDataRepository.DeleteAll();
                    await _ITRRecords_8161_001A_BodyRepository.DeleteAll();
                    await _ITRRecords_8161_001A_InsResRepository.DeleteAll();
                    await _ITRRecords_8161_001A_ConResRepository.DeleteAll();
                    await _ITR8100_001A_CTdataRepository.DeleteAll();
                    await _ITR8100_001A_IRTestRepository.DeleteAll();
                    await _ITR8100_001A_RatioTestRepository.DeleteAll();
                    await _ITR8100_001A_TIDataRepository.DeleteAll();
                    await _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents.DeleteAll();
                    await _ITR8121_002A_Records.DeleteAll();
                    await _ITR8121_002A_TransformerRadioTest.DeleteAll();
                    await _ITR_8260_002A_BodyRepository.DeleteAll();
                    await _ITR_8260_002A_DielectricTestRepository.DeleteAll();
                    await _TestEquipmentDataRepository.DeleteAll();
                    await _ITRInstrumentDataRepository.DeleteAll();
                    await _ITRInstrumentDataRepository.DeleteAll();
                    await _ITR8161_002A_BodyRepository.DeleteAll();
                    await _ITR8161_002A_DielectricTestRepository.DeleteAll();
                    await _ITR8000_101A_GeneralnformationRepository.DeleteAll();
                    await _ITR8000_101A_RecordISBarrierDetailsRepository.DeleteAll();
                    await _ITRRecords_8211_002A_BodyRepository.DeleteAll();
                    await _ITRRecords_8211_002A_RunTestRepository.DeleteAll();
                    await _ITR7000_101AHarmony_GenlnfoRepository.DeleteAll();
                    await _ITR7000_101AHarmony_ActivityDetailsRepository.DeleteAll();
                    await _ITR_8170_002A_IndifictionLampRepository.DeleteAll();
                    await _ITR_8170_002A_InsResRepository.DeleteAll();
                    await _ITRRecords_8170_002A_Voltage_ReadingRepository.DeleteAll();
                    await _ITR_8300_003A_BodyRepository.DeleteAll();
                    await _ITR_8300_003A_IlluminRepository.DeleteAll();

                    Settings.AccessToken = string.Empty;
                    Settings.RenewalToken = string.Empty;
                    Settings.DisplayName = string.Empty;
                    Cache.accessToken = string.Empty;
                    Settings.CompletionAccessToken = string.Empty;
                    Cache.accessToken = string.Empty;

                    _pageHelper.TokenExpiry = DateTime.Today.AddDays(-1);
                    _CompletionpageHelper.CompletionTokenExpiry = DateTime.Today.AddDays(-1);

                    await _userDialogs.AlertAsync("Software Reset successfully", "Reset Data", "Ok");                  

                    await navigationService.NavigateAsync<LoginViewModel>();
                }
                catch (Exception e)
                {
                    await _userDialogs.AlertAsync("Error occured E-Report(s) not removed", null, "Ok");
                }

            }
        }
        public async void ClearSaveData()
        {
            if (await _userDialogs.ConfirmAsync($"Are you sure you want to clear all data?", $"Clear data", "Yes", "No"))
            {
                try
                {
                    //await _userProjectsRepository.QueryAsync("Delete from T_UserProjects Where ProjectName ='" + Settings.ProjectName+"'");
                    await _DataRefsRepository.QueryAsync("Delete from T_DataRefs Where modelname ='" + Settings.ModelName + "'");
                    await _CompletionsDrawingsRepository.QueryAsync("Delete from T_CompletionsDrawings Where ProjectName ='" + Settings.ProjectName + "'");
                    await _HandoverRepository.QueryAsync("Delete from T_Handover Where ProjectName ='" + Settings.ProjectName + "'");
                    await _HandoverWorkpacksRepository.QueryAsync("Delete from T_HandoverWorkpacks Where ProjectName ='" + Settings.ProjectName + "'");
                    await _HeaderItemsRepository.QueryAsync("Delete from T_HeaderItems Where modelName ='" + Settings.ModelName + "'");
                    await _JOINTRepository.QueryAsync("Delete from T_JOINT Where ProjectName ='" + Settings.ProjectName + "'");
                    await _JOINT_DRAWINGSRepository.QueryAsync("Delete from T_JOINT_DRAWINGS Where ProjectName ='" + Settings.ProjectName + "'");
                    await _JOINT_FLANGEMANAGEMENTCHECKLISTRepository.QueryAsync("Delete from T_JOINT_FLANGEMANAGEMENTCHECKLIST Where ProjectName ='" + Settings.ProjectName + "'");
                    //await _JOINT_HEADERRepository.DeleteAll();
                    await _JOINT_QUESTIONSRepository.QueryAsync("Delete from T_JOINT_QUESTIONS Where ProjectName ='" + Settings.ProjectName + "'");
                    await _PUNCH_LISTRepository.QueryAsync("Delete from T_JS_PUNCH_LIST Where ProjectName ='" + Settings.ProjectName + "'");
                    await _CompletionsPunchListRepository.QueryAsync("Delete from T_CompletionsPunchList Where project ='" + Settings.ProjectName + "'");
                    await _SignOffRepository.QueryAsync("Delete from T_SignOff Where modelName ='" + Settings.ModelName + "'");
                    await _Tag_headersRepository.QueryAsync("Delete from T_Tag_headers Where ProjectName ='" + Settings.ProjectName + "'");
                    await _CHECKSHEETRepository.QueryAsync("Delete from T_CHECKSHEET Where ProjectName ='" + Settings.ProjectName + "'");
                    await _CHECKSHEET_PER_TAGRepository.QueryAsync("Delete from T_CHECKSHEET_PER_TAG Where ProjectName ='" + Settings.ProjectName + "'");
                    await _CHECKSHEET_QUESTIONRepository.QueryAsync("Delete from T_CHECKSHEET_QUESTION Where ProjectName ='" + Settings.ProjectName + "'");
                    await _SYSTEMRepository.QueryAsync("Delete from T_SYSTEM Where ProjectName ='" + Settings.ProjectName + "'");
                    await _TAGRepository.QueryAsync("Delete from T_TAG Where ProjectName ='" + Settings.ProjectName + "'");
                    await _TAG_SHEET_ANSWERRepository.QueryAsync("Delete from T_TAG_SHEET_ANSWER Where ProjectName ='" + Settings.ProjectName + "'");
                    await _TAG_SHEET_HEADERRepository.QueryAsync("Delete from T_TAG_SHEET_HEADER Where ProjectName ='" + Settings.ProjectName + "'");
                    await _SignOffHeaderRepository.QueryAsync("Delete from T_SignOffHeader Where ProjectName ='" + Settings.ProjectName + "'");

                    await _CompanyCategoryCodeRepository.QueryAsync("Delete from T_CompanyCategoryCode Where ModelName ='" + Settings.ModelName + "'");
                    await _CompletionSystemsRepository.QueryAsync("Delete from T_CompletionSystems Where modelName ='" + Settings.ModelName + "'");
                    await _PunchComponentRepository.QueryAsync("Delete from T_PunchComponent Where ModelName ='" + Settings.ModelName + "'");
                    await _PunchSystemRepository.QueryAsync("Delete from T_PunchSystem Where ModelName ='" + Settings.ModelName + "'");
                    await _PunchPCWBSRepository.QueryAsync("Delete from T_PunchPCWBS Where ModelName ='" + Settings.ModelName + "'");
                    await _PunchFWBSRepository.QueryAsync("Delete from T_PunchFWBS Where ModelName ='" + Settings.ModelName + "'");
                    await _SectionCodeRepository.QueryAsync("Delete from T_SectionCode Where ModelName ='" + Settings.ModelName + "'");

                    await _CommonHeaderFooterRepository.QueryAsync("Delete from T_ITRCommonHeaderFooter Where ModelName ='" + Settings.ModelName + "'");
                    await _RecordsRepository.QueryAsync("Delete from T_ITRRecords_30A_31A Where ModelName ='" + Settings.ModelName + "'");
                    await _TubeColorsRepository.QueryAsync("Delete from T_ITRTubeColors Where ModelName ='" + Settings.ModelName + "'");
                    await _Records_04XARepository.QueryAsync("Delete from T_ITRRecords_040A_041A_042A Where ModelName ='" + Settings.ModelName + "'");
                    await _InsulationDetailsRepository.QueryAsync("Delete from T_ITRInsulationDetails Where ModelName ='" + Settings.ModelName + "'");
                    await _CommonHeaderFooterSignOffRepository.QueryAsync("Delete from T_CommonHeaderFooterSignOff Where ModelName ='" + Settings.ModelName + "'");
                    await _Records_080A_09XARepository.QueryAsync("Delete from T_ITRRecords_080A_090A_091A Where ModelName ='" + Settings.ModelName + "'");
                    await _ITRRecords_8100_002ARepository.QueryAsync("Delete from T_ITRRecords_8100_002A Where ModelName ='" + Settings.ModelName + "'");
                    await _ITRRecords_8100_002A_InsRes_TestRepository.QueryAsync("Delete from T_ITRRecords_8100_002A_InsRes_Test Where ModelName ='" + Settings.ModelName + "'");
                    await _ITRRecords_8100_002A_Radio_TestRepository.QueryAsync("Delete from T_ITRRecords_8100_002A_Radio_Test Where ModelName ='" + Settings.ModelName + "'");
                    await _ITRRecords_8161_001A_BodyRepository.QueryAsync("Delete from T_ITRRecords_8161_001A_Body Where ModelName ='" + Settings.ModelName + "'");
                    await _ITRRecords_8161_001A_InsResRepository.QueryAsync("Delete from T_ITRRecords_8161_001A_InsRes Where ModelName ='" + Settings.ModelName + "'");
                    await _ITRRecords_8161_001A_ConResRepository.QueryAsync("Delete from T_ITRRecords_8161_001A_ConRes Where ModelName ='" + Settings.ModelName + "'");

                    await _ITR8121_004AInCAndAuxiliaryRepository.QueryAsync("Delete from T_ITR8121_004AInCAndAuxiliary Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR8121_004ATransformerRatioTestRepository.QueryAsync("Delete from T_ITR8121_004ATransformerRatioTest Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR8121_004ATestInstrumentDataRepository.QueryAsync("Delete from T_ITR8121_004ATestInstrumentData Where ModelName ='" + Settings.ModelName + "'");

                    await _Records_8000003ARepository.QueryAsync("Delete from T_ITRRecords_8000_003A Where ModelName ='" + Settings.ModelName + "'");
                    await _Records_8000003A_AcceptanceCriteriaRepository.QueryAsync("Delete from T_ITRRecords_8000_003A_AcceptanceCriteria Where ModelName ='" + Settings.ModelName + "'");
                    await _T_ITR8140_001A_ContactResisTestRepository.QueryAsync("Delete from T_ITR8140_001A_ContactResisTest Where ModelName ='" + Settings.ModelName + "'");
                    await _T_ITR8140_001AInsulationResistanceTestRepository.QueryAsync("Delete from T_ITR8140_001AInsulationResistanceTest Where ModelName ='" + Settings.ModelName + "'");
                    await _T_ITR8140_001ADialectricTestRepository.QueryAsync("Delete from T_ITR8140_001ADialectricTest Where ModelName ='" + Settings.ModelName + "'");
                    await _T_ITR8140_001ATestInstrumentDataRepository.QueryAsync("Delete from T_ITR8140_001ATestInstrucitonData Where ModelName ='" + Settings.ModelName + "'");
                    
                    await _ITR8100_001A_CTdataRepository.QueryAsync("Delete from T_ITR8100_001A_CTdata Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR8100_001A_IRTestRepository.QueryAsync("Delete from T_ITR8100_001A_InsulationResistanceTest Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR8100_001A_RatioTestRepository.QueryAsync("Delete from T_ITR8100_001A_RatioTest Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR8100_001A_TIDataRepository.QueryAsync("Delete from T_ITR8100_001A_TestInstrumentData Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents.QueryAsync("Delete from T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR8121_002A_Records.QueryAsync("Delete from T_ITR8121_002A_Records Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR8121_002A_TransformerRadioTest.QueryAsync("Delete from T_ITR8121_002A_TransformerRadioTest Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR_8260_002A_BodyRepository.QueryAsync("Delete from T_ITR_8260_002A_Body Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR_8260_002A_DielectricTestRepository.QueryAsync("Delete from T_ITR_8260_002A_DielectricTest Where ModelName ='" + Settings.ModelName + "'");
                    await _TestEquipmentDataRepository.QueryAsync("Delete from T_TestEquipmentData Where ProjectID ='" + Settings.ProjectID + "'");
                    await _ITRInstrumentDataRepository.QueryAsync("Delete from T_ITRInstrumentData Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR8161_002A_BodyRepository.QueryAsync("Delete from T_ITR8161_002A_Body Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR8161_002A_DielectricTestRepository.QueryAsync("Delete from T_ITR8161_002A_DielectricTest Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR8000_101A_GeneralnformationRepository.QueryAsync("Delete from T_ITR8000_101A_Generalnformation Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR8000_101A_RecordISBarrierDetailsRepository.QueryAsync("Delete from T_ITR8000_101A_RecordISBarrierDetails Where ModelName ='" + Settings.ModelName + "'");
                    await _ITRRecords_8211_002A_BodyRepository.QueryAsync("Delete from T_ITRRecords_8211_002A_Body Where ModelName ='" + Settings.ModelName + "'");
                    await _ITRRecords_8211_002A_RunTestRepository.QueryAsync("Delete from T_ITRRecords_8211_002A_RunTest Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR7000_101AHarmony_GenlnfoRepository.QueryAsync("Delete from T_ITR_7000_101AHarmony_Genlnfo Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR7000_101AHarmony_ActivityDetailsRepository.QueryAsync("Delete from T_ITR_7000_101AHarmony_ActivityDetails Where ModelName ='" + Settings.ModelName + "'");
                    await _ITRRecords_8170_002A_Voltage_ReadingRepository.QueryAsync("Delete from T_ITRRecords_8170_002A_Voltage_Reading Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR_8170_002A_IndifictionLampRepository.QueryAsync("Delete from T_ITR_8170_002A_IndifictionLamp Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR_8170_002A_InsResRepository.QueryAsync("Delete from T_ITR_8170_002A_InsRes Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR_8300_003A_BodyRepository.QueryAsync("Delete from T_ITR_8300_003A_Body Where ModelName ='" + Settings.ModelName + "'");
                    await _ITR_8300_003A_IlluminRepository.QueryAsync("Delete from T_ITR_8300_003A_Illumin Where ModelName ='" + Settings.ModelName + "'");

                    // await _WorkPackRepository.DeleteAll();

                    //Finish
                    await _userDialogs.AlertAsync("Data cleared successfully", "Cleared Data", "Ok");
                }
                catch (Exception e)
                {
                    await _userDialogs.AlertAsync("Error occured " + Settings.ModuleName + " not removed", null, "Ok");
                }
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

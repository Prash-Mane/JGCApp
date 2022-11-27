using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables.Completions;
using JGC.DataBase.DataTables.ModsCore;
using JGC.Models;
using JGC.Models.Completions;
using JGC.Views.Completions;
using Newtonsoft.Json;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using JGC.Common.Extentions;
using JGC.DataBase.DataTables.ITR;
using System.Text.RegularExpressions;
using JGC.Common.Services;
using JGC.Models.ITR;
using System.Reflection;
using JGC.UserControls.CustomControls;
using Xamarin.Forms.DataGrid;

namespace JGC.ViewModels.Completions
{
    public class ITRViewModel : BaseViewModel
    {
        public readonly INavigationService _navigationService;

        public readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly ITRService _ITRService;
        private readonly IRepository<T_TAG> _TAGRepository;
        private readonly IRepository<T_CHECKSHEET> _CheckSheetRepository;
        private readonly IRepository<T_CHECKSHEET_QUESTION> _CheckSheetQuetionsRepository;
        private readonly IRepository<T_CHECKSHEET_PER_TAG> _CheckSheetPerTagRepository;
        private readonly IRepository<T_CommonHeaderFooterSignOff> _CommonHeaderFooterSignOffRepository;
        private readonly IRepository<T_TAG_SHEET_HEADER> _TagSheetHeaderRepository;
        private readonly IRepository<T_TAG_SHEET_ANSWER> _TAG_SHEET_ANSWERRepository;
        private readonly IRepository<T_UserControl> _T_UserControlRepository;
        private readonly IRepository<T_ITRCommonHeaderFooter> _CommonHeaderFooterRepository;
        private readonly IRepository<T_ITRRecords_30A_31A> _RecordsRepository;
        private readonly IRepository<T_ITRTubeColors> _TubeColorsRepository;
        private readonly IRepository<T_ITRRecords_040A_041A_042A> _Records_04XARepository;
        private readonly IRepository<T_ITRInsulationDetails> _InsulationDetailsRepository;
        private readonly IRepository<T_ITRRecords_080A_090A_091A> _Records_080A_09XARepository;

        private readonly IRepository<T_ITR8000_003ARecords> _Records_8000003ARepository;
        private readonly IRepository<T_ITR8000_003A_AcceptanceCriteria> _Records_8000003A_AcceptanceCriteriaRepository;
        private readonly IRepository<T_ITR8100_001A_CTdata> _ITR8100_001A_CTdataRepository;
        private readonly IRepository<T_ITR8100_001A_InsulationResistanceTest> _ITR8100_001A_IRTestRepository;
        private readonly IRepository<T_ITR8100_001A_RatioTest> _ITR8100_001A_RatioTestRepository;
        private readonly IRepository<T_ITR8100_001A_TestInstrumentData> _ITR8100_001A_TIDataRepository;
        private readonly IRepository<T_ITRRecords_8100_002A> _ITRRecords_8100_002ARepository;
        private readonly IRepository<T_ITRRecords_8100_002A_InsRes_Test> _ITRRecords_8100_002A_InsRes_TestRepository;
        private readonly IRepository<T_ITR8140_001A_ContactResisTest> _T_ITR8140_001A_ContactResisTestRepository;
        private readonly IRepository<T_ITR8140_001AInsulationResistanceTest> _T_ITR8140_001AInsulationResistanceTestRepository;
        private readonly IRepository<T_ITR8140_001ADialectricTest> _T_ITR8140_001ADialectricTestRepository;
        private readonly IRepository<T_ITR8140_001ATestInstrucitonData> _T_ITR8140_001ATestInstrumentDataRepository;
        private readonly IRepository<T_ITRRecords_8100_002A_Radio_Test> _ITRRecords_8100_002A_Radio_TestRepository;
        private readonly IRepository<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents> _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents;
        private readonly IRepository<T_ITR8121_002A_Records> _ITR8121_002A_Records;
        private readonly IRepository<T_ITR8121_002A_TransformerRadioTest> _ITR8121_002A_TransformerRadioTest;
        private readonly IRepository<T_ITR_8260_002A_Body> _ITR_8260_002A_BodyRepository;
        private readonly IRepository<T_ITR_8260_002A_DielectricTest> _ITR_8260_002A_DielectricTestRepository;
        private readonly IRepository<T_ITRRecords_8161_001A_Body> _ITRRecords_8161_001A_BodyRepository;
        private readonly IRepository<T_ITRRecords_8161_001A_InsRes> _ITRRecords_8161_001A_InsResRepository;
        private readonly IRepository<T_ITRRecords_8161_001A_ConRes> _ITRRecords_8161_001A_ConResRepository;
        private readonly IRepository<T_ITR8121_004AInCAndAuxiliary> _ITR8121_004AInCAndAuxiliaryRepository;
        private readonly IRepository<T_ITR8121_004ATransformerRatioTest> _ITR8121_004ATransformerRatioTestRepository;
        private readonly IRepository<T_ITR8121_004ATestInstrumentData> _ITR8121_004ATestInstrumentDataRepository;
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
        private readonly IRepository<T_Ccms_signature> _Ccms_signatureRepository;
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
        private readonly IRepository<T_CompletionsUsers> _CompletionsUserRepository;

        public List<T_TAG> _tagsList;
        public static CompletionPageHelper _CompletionpageHelper = new CompletionPageHelper();
        public T_ITRCommonHeaderFooter CommonHeaderFooter;
        public bool Completed, Started, Rejected, CheckSheetCompleted, IsCheckSheetAccessible, IsVisibleRejectButton, IsChecksheetIntiled;
        List<bool> comparer = new List<bool>();
        public string ITR_STATUS_COMPLETE = "#00FF00";
        public string ITR_STATUS_STARTED = "#FF8000";
        string Outstanding = "#FF0000";
        //string REjected = "#FF0000";
        string statusNoJIData = "#D3D3D3";
        string CoreType = string.Empty;
        // private INavigation navi;
        int ITR8121_002A_Record_ID = 0;
        #region Delegate Commands   
        public ICommand ClickCommand
        {
            get
            {
                return new Command<string>(OnClickButtonAsync);
            }
        }
        #endregion

        #region Properties
        private ObservableCollection<dynamic> itemSourceTagList;
        public ObservableCollection<dynamic> ItemSourceTagList
        {
            get { return itemSourceTagList; }
            set { itemSourceTagList = value; RaisePropertyChanged(); }
        }
        private List<string> _DrawingNoList;
        public List<string> DrawingNoList
        {
            get { return _DrawingNoList; }
            set { _DrawingNoList = value; RaisePropertyChanged(); }
        }
        //private ObservableCollection<T_CHECKSHEET_PER_TAG> itemSourceCheckSheets;
        //public ObservableCollection<T_CHECKSHEET_PER_TAG> ItemSourceCheckSheets
        //{
        //    get { return itemSourceCheckSheets; }
        //    set { itemSourceCheckSheets = value; RaisePropertyChanged(); }
        //}
        private ObservableCollection<T_ITRCommonHeaderFooter> itemSourceCheckSheets;
        public ObservableCollection<T_ITRCommonHeaderFooter> ItemSourceCheckSheets
        {
            get { return itemSourceCheckSheets; }
            set { itemSourceCheckSheets = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_ITRTubeColors> sourceTubeColors;
        public ObservableCollection<T_ITRTubeColors> ItemSourceTubeColors
        {
            get { return sourceTubeColors; }
            set { sourceTubeColors = value; RaisePropertyChanged(); }
        }

        private List<string> _ListTestResultAccepted;
        public List<string> ListTestResultAccepted
        {
            get { return _ListTestResultAccepted; }
            set { _ListTestResultAccepted = value; RaisePropertyChanged(); }
        }
        private T_ITRTubeColors selectedTubeColors;
        public T_ITRTubeColors SelectedTubeColors
        {
            get { return selectedTubeColors; }
            set { selectedTubeColors = value; RaisePropertyChanged(); }
        }

        private string selectedTestResultsAccepted;
        public string SelectedTestResultsAccepted
        {
            get { return selectedTestResultsAccepted; }
            set { selectedTestResultsAccepted = value; RaisePropertyChanged(); }
        }
        private T_TAG selectedTag;
        public T_TAG SelectedTag
        {
            get { return selectedTag; }
            set { selectedTag = value; RaisePropertyChanged(); OnTagSelectionChage(); }
        }
        //private T_CHECKSHEET_PER_TAG selectedCheckSheet;
        //public T_CHECKSHEET_PER_TAG SelectedCheckSheet
        //{
        //    get { return selectedCheckSheet; }
        //    set
        //    {
        //        selectedCheckSheet = value;
        //        RaisePropertyChanged();
        //        //  if (SelectedCheckSheet != null) 
        //        // OnCheckSheetSelectionChage(); }
        //    }
        //}
        private T_ITRCommonHeaderFooter selectedCheckSheet;
        public T_ITRCommonHeaderFooter SelectedCheckSheet
        {
            get { return selectedCheckSheet; }
            set
            {
                selectedCheckSheet = value;
                RaisePropertyChanged();
                //  if (SelectedCheckSheet != null) 
                // OnCheckSheetSelectionChage(); }
            }
        }
        private T_CHECKSHEET currentCheckSheet;
        public T_CHECKSHEET CurrentCheckSheet
        {
            get { return currentCheckSheet; }
            set { currentCheckSheet = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_CommonHeaderFooterSignOff> signOffList;
        public ObservableCollection<T_CommonHeaderFooterSignOff> SignOffList
        {
            get { return signOffList; }
            set { signOffList = value; RaisePropertyChanged(); }
        }
        //private ObservableCollection<T_TAG_SHEET_HEADER> tagSheetHeadersList;
        //public ObservableCollection<T_TAG_SHEET_HEADER> TagSheetHeadersList
        //{
        //    get {  return tagSheetHeadersList;}
        //    set
        //    {
        //        tagSheetHeadersList = value;
        //        RaisePropertyChanged();
        //        // OnPropertyChanged();
        //    }
        //}
        private T_ITRCommonHeaderFooter itrSheetHeaders;
        public T_ITRCommonHeaderFooter ItrSheetHeaders
        {
            get { return itrSheetHeaders; }
            set
            {
                itrSheetHeaders = value;
                RaisePropertyChanged();
            }
        }
        private string remarkEntry;
        public string RemarkEntry
        {
            get { return remarkEntry; }
            set { remarkEntry = value; RaisePropertyChanged(); }
        }

        private bool _indexFrame;
        public bool IndexFrame
        {
            get { return _indexFrame; }
            set { _indexFrame = value; RaisePropertyChanged(); }
        }
        private bool _ITR8140_001AFrame;

        public bool ITR8140_001AFrame
        {
            get { return _ITR8140_001AFrame; }
            set
            {
                _ITR8140_001AFrame = value; RaisePropertyChanged();
            }
        }
        private bool _questionFrame;
        public bool QuestionFrame
        {
            get { return _questionFrame; }
            set { _questionFrame = value; RaisePropertyChanged(); }
        }
        private bool itr_040AFrame;
        public bool ITR_040AFrame
        {
            get { return itr_040AFrame; }
            set { itr_040AFrame = value; RaisePropertyChanged(); }
        }
        //private string _CurrentCheckSheetRevNo;
        //public string CurrentCheckSheetRevNo
        //{
        //    get { return _CurrentCheckSheetRevNo; }
        //    set { _CurrentCheckSheetRevNo = value; RaisePropertyChanged(); }
        //}
        private string previousBtnCmd;
        public string PreviousBtnCmd
        {
            get { return previousBtnCmd; }
            set { previousBtnCmd = value; RaisePropertyChanged(); }
        }
        private string nextBtnCmd;
        public string NextBtnCmd
        {
            get { return nextBtnCmd; }
            set { nextBtnCmd = value; RaisePropertyChanged(); }
        }
        private string nextSaveBtnText;
        public string NextSaveBtnText
        {
            get { return nextSaveBtnText; }
            set { nextSaveBtnText = value; RaisePropertyChanged(); }
        }
        private bool previousBtnVisible;
        public bool PreviousBtnVisible
        {
            get { return previousBtnVisible; }
            set { previousBtnVisible = value; RaisePropertyChanged(); }
        }
        private T_ITRRecords_30A_31A _ITRRecord;
        public T_ITRRecords_30A_31A ITRRecord
        {
            get { return _ITRRecord; }
            set { _ITRRecord = value; RaisePropertyChanged(); }
        }
        private string _DescriptionHeader;
        public string DescriptionHeader
        {
            get { return _DescriptionHeader; }
            set { _DescriptionHeader = value; RaisePropertyChanged(); }
        }
        private bool isvisibleToday;
        public bool IsvisibleToday
        {
            get { return isvisibleToday; }
            set { isvisibleToday = value; RaisePropertyChanged(); }
        }
        private T_ITRRecords_040A_041A_042A _ITRRecord04xA;
        public T_ITRRecords_040A_041A_042A ITRRecord04xA
        {
            get { return _ITRRecord04xA; }
            set { _ITRRecord04xA = value; RaisePropertyChanged(); }
        }
        private List<string> _ListTestVoltage;
        public List<string> ListTestVoltage
        {
            get { return _ListTestVoltage; }
            set { _ListTestVoltage = value; RaisePropertyChanged(); }
        }
        private string _SelectedTestVoltage;
        public string SelectedTestVoltage
        {
            get { return _SelectedTestVoltage; }
            set { _SelectedTestVoltage = value; RaisePropertyChanged(); }
        }
        //8100-002A

        private T_ITRRecords_8100_002A _ITRRecord812X;
        public T_ITRRecords_8100_002A ITRRecord812X
        {
            get { return _ITRRecord812X; }
            set { _ITRRecord812X = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_ITRRecords_8100_002A_InsRes_Test> _ITR8100_002A_InsRes_Test;
        public ObservableCollection<T_ITRRecords_8100_002A_InsRes_Test> ITR8100_002A_InsRes_Test
        {
            get { return _ITR8100_002A_InsRes_Test; }
            set { _ITR8100_002A_InsRes_Test = value; RaisePropertyChanged(); }
        }
        private T_ITRRecords_8100_002A_InsRes_Test _Item_InsRes_Test;
        public T_ITRRecords_8100_002A_InsRes_Test Item_InsRes_Test
        {
            get { return _Item_InsRes_Test; }
            set { _Item_InsRes_Test = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_ITRRecords_8100_002A_Radio_Test> _ITR8100_002A_Radio_Test;
        public ObservableCollection<T_ITRRecords_8100_002A_Radio_Test> ITR8100_002A_Radio_Test
        {
            get { return _ITR8100_002A_Radio_Test; }
            set { _ITR8100_002A_Radio_Test = value; RaisePropertyChanged(); }
        }
        private T_ITRRecords_8100_002A_Radio_Test _Item_Radio_Test;
        public T_ITRRecords_8100_002A_Radio_Test Item_Radio_Test
        {
            get { return _Item_Radio_Test; }
            set { _Item_Radio_Test = value; RaisePropertyChanged(); }
        }
        //8100-002A
        //8161-001A
        private T_ITRRecords_8161_001A_Body _ITRRecord8161_1XA;
        public T_ITRRecords_8161_001A_Body ITRRecord8161_1XA
        {
            get { return _ITRRecord8161_1XA; }
            set { _ITRRecord8161_1XA = value; RaisePropertyChanged(); }
        }
        private int _SlNo;
        public int SlNo
        {
            get { return _SlNo; }
            set { _SlNo = value; RaisePropertyChanged(); }
        }

        private string _InsResTestTitle;
        public string InsResTestTitle
        {
            get { return _InsResTestTitle; }
            set { _InsResTestTitle = value; RaisePropertyChanged(); }
        }

        private string _RegTestTitle;
        public string RegTestTitle
        {
            get { return _RegTestTitle; }
            set { _RegTestTitle = value; RaisePropertyChanged(); }
        }

        private string _AcceptCriteria;
        public string AcceptCriteria
        {
            get { return _AcceptCriteria; }
            set { _AcceptCriteria = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_ITRRecords_8161_001A_InsRes> _ITR_8161_001A_InsRes;
        public ObservableCollection<T_ITRRecords_8161_001A_InsRes> ITR_8161_001A_InsRes
        {
            get { return _ITR_8161_001A_InsRes; }
            set { _ITR_8161_001A_InsRes = value; RaisePropertyChanged(); }
        }
        private T_ITRRecords_8161_001A_InsRes _Item_8161_001A_InsRes;
        public T_ITRRecords_8161_001A_InsRes Item_8161_001A_InsRes
        {
            get { return _Item_8161_001A_InsRes; }
            set { _Item_8161_001A_InsRes = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_ITRRecords_8161_001A_ConRes> _ITR_8161_001A_ConRes;
        public ObservableCollection<T_ITRRecords_8161_001A_ConRes> ITR_8161_001A_ConRes
        {
            get { return _ITR_8161_001A_ConRes; }
            set { _ITR_8161_001A_ConRes = value; RaisePropertyChanged(); }
        }
        private T_ITRRecords_8161_001A_ConRes _Item_8161_001A_ConRes;
        public T_ITRRecords_8161_001A_ConRes Item_8161_001A_ConRes
        {
            get { return _Item_8161_001A_ConRes; }
            set { _Item_8161_001A_ConRes = value; RaisePropertyChanged(); }
        }
        //8161-001A
        // 8211-002A
        private T_ITRRecords_8211_002A_Body _ITRRecord8211_2XA;
        public T_ITRRecords_8211_002A_Body ITRRecord8211_2XA
        {
            get { return _ITRRecord8211_2XA; }
            set { _ITRRecord8211_2XA = value; RaisePropertyChanged(); }
        }

        // 8211-002A
        private ObservableCollection<T_ITRInsulationDetails> _ItemSourceInsulationDetails;
        public ObservableCollection<T_ITRInsulationDetails> ItemSourceInsulationDetails
        {
            get { return _ItemSourceInsulationDetails; }
            set { _ItemSourceInsulationDetails = value; RaisePropertyChanged(); }
        }
        private T_ITRInsulationDetails _SelectedInsulationDetails;
        public T_ITRInsulationDetails SelectedInsulationDetails
        {
            get { return _SelectedInsulationDetails; }
            set { _SelectedInsulationDetails = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<string> _PickerValueList;
        public ObservableCollection<string> PickerValueList
        {
            get { return _PickerValueList; }
            set { _PickerValueList = value; RaisePropertyChanged(); }
        }

        private bool itr_080AFrame;
        public bool ITR_080AFrame
        {
            get { return itr_080AFrame; }
            set { itr_080AFrame = value; RaisePropertyChanged(); }
        }
        private bool itr_8100002AFrame;
        public bool ITR_8100002AFrame
        {
            get { return itr_8100002AFrame; }
            set { itr_8100002AFrame = value; RaisePropertyChanged(); }
        }
        private bool itr_8161001AFrame;
        public bool ITR_8161001AFrame
        {
            get { return itr_8161001AFrame; }
            set { itr_8161001AFrame = value; RaisePropertyChanged(); }
        }
        private T_ITRRecords_080A_090A_091A _ITRRecord_80A_91A;
        public T_ITRRecords_080A_090A_091A ITRRecord_80A_91A
        {
            get { return _ITRRecord_80A_91A; }
            set { _ITRRecord_80A_91A = value; RaisePropertyChanged(); }
        }
        private List<string> _ListTestResultAccept;
        public List<string> ListTestResultAccept
        {
            get { return _ListTestResultAccept; }
            set { _ListTestResultAccept = value; RaisePropertyChanged(); }
        }
        private string selectedTestResultAccept;
        public string SelectedTestResultAccept
        {
            get { return selectedTestResultAccept; }
            set { selectedTestResultAccept = value; RaisePropertyChanged(); }
        }
        private string _ITR_4X_Title;
        public string ITR_4X_Title
        {
            get { return _ITR_4X_Title; }
            set { _ITR_4X_Title = value; RaisePropertyChanged(); }
        }
        private string _ITR_80_9X_Title;
        public string ITR_80_9X_Title
        {
            get { return _ITR_80_9X_Title; }
            set { _ITR_80_9X_Title = value; RaisePropertyChanged(); }
        }
        private string _LblFirst;
        public string LblFirst
        {
            get { return _LblFirst; }
            set { _LblFirst = value; RaisePropertyChanged(); }
        }
        private string _LblSecond;
        public string LblSecond
        {
            get { return _LblSecond; }
            set { _LblSecond = value; RaisePropertyChanged(); }
        }
        private string _LblThird;
        public string LblThird
        {
            get { return _LblThird; }
            set { _LblThird = value; RaisePropertyChanged(); }
        }

        //Itr 8000- 003A

        private int rowWidth;
        public int RowWidth
        {
            get { return rowWidth; }
            set { rowWidth = value; RaisePropertyChanged(); }
        }

        private string title8000_003A;
        public string Title8000_003A
        {
            get { return title8000_003A; }
            set { title8000_003A = value; RaisePropertyChanged(); }
        }
        private int dynamicGridHeight;
        public int DynamicGridHeight
        {
            get { return dynamicGridHeight; }
            set { dynamicGridHeight = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_ITR8000_003A_AcceptanceCriteria> _acceptanceCriteriaList;
        public ObservableCollection<T_ITR8000_003A_AcceptanceCriteria> AcceptanceCriteriaList
        {
            get { return _acceptanceCriteriaList; }
            set { _acceptanceCriteriaList = value; RaisePropertyChanged(); }
        }
        private T_ITR8000_003A_AcceptanceCriteria _SelectedacceptanceCriteria;
        public T_ITR8000_003A_AcceptanceCriteria SelectedAcceptanceCriteria
        {
            get { return _SelectedacceptanceCriteria; }
            set { _SelectedacceptanceCriteria = value; RaisePropertyChanged(); }
        }
        private T_ITR8000_003ARecords _Records_8000_003A;
        public T_ITR8000_003ARecords Records_8000_003A
        {
            get { return _Records_8000_003A; }
            set { _Records_8000_003A = value; RaisePropertyChanged(); }
        }
        private bool itr8000003AFRAME;
        public bool Itr8000003AFRAME
        {
            get { return itr8000003AFRAME; }
            set { itr8000003AFRAME = value; RaisePropertyChanged(); }
        }
        private string ACriteria8000_003A;
        public string AcceptanceCriteria8000_003A
        {
            get { return ACriteria8000_003A; }
            set { ACriteria8000_003A = value; RaisePropertyChanged(); }
        }

        private List<string> _DDLCheckList;
        public List<string> DDLCheckList
        {
            get { return _DDLCheckList; }
            set { _DDLCheckList = value; RaisePropertyChanged(); }
        }
        //private string _TYPEAndSIZE { get; set; }
        //private string _Voltage { get; set; }
        //public string Voltage
        //{

        //    get { return _Voltage; }
        //    set { _Voltage = value; RaisePropertyChanged(); }
        //}
        //public string TypeAndSize
        //{

        //    get { return _TYPEAndSIZE; }
        //    set { _TYPEAndSIZE = value; RaisePropertyChanged(); }
        //}


        //8100_001A
        #region 8100_001A

        private ObservableCollection<T_ITR8100_001A_CTdata> iTR8100_001A_CTdata;
        public ObservableCollection<T_ITR8100_001A_CTdata> ITR8100_001A_CTdata
        {
            get { return iTR8100_001A_CTdata; }
            set { iTR8100_001A_CTdata = value; RaisePropertyChanged(); }
        }
        private T_ITR8100_001A_CTdata selectedCTdata;
        public T_ITR8100_001A_CTdata SelectedCTdata
        {
            get { return selectedCTdata; }
            set { selectedCTdata = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_ITR8100_001A_InsulationResistanceTest> iTR8100_001A_InsulationResistanceTest;
        public ObservableCollection<T_ITR8100_001A_InsulationResistanceTest> ITR8100_001A_InsulationResistanceTest
        {
            get { return iTR8100_001A_InsulationResistanceTest; }
            set { iTR8100_001A_InsulationResistanceTest = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_ITR8100_001A_RatioTest> iTR8100_001A_RatioTest;
        public ObservableCollection<T_ITR8100_001A_RatioTest> ITR8100_001A_RatioTest
        {
            get { return iTR8100_001A_RatioTest; }
            set { iTR8100_001A_RatioTest = value; RaisePropertyChanged(); }
        }
        public T_ITR8100_001A_RatioTest _selectedRedioTest;
        public T_ITR8100_001A_RatioTest SelectedRedioTest
        {
            get { return _selectedRedioTest; }
            set { _selectedRedioTest = value; RaisePropertyChanged(); }
        }

        private bool _itr_8100_001AFrame;
        public bool ITR_8100_001AFrame
        {
            get { return _itr_8100_001AFrame; }
            set { _itr_8100_001AFrame = value; RaisePropertyChanged(); }
        }

        public T_ITR8100_001A_TestInstrumentData testInstrumentData8100_001A;
        public T_ITR8100_001A_TestInstrumentData TestInstrumentData8100_001A
        {
            get { return testInstrumentData8100_001A; }
            set { testInstrumentData8100_001A = value; RaisePropertyChanged(); }
        }

        //8140    001A
        private T_ITR8140_001A_ContactResisTest _ContactResisTest { get; set; }
        public T_ITR8140_001A_ContactResisTest ContactResisTest
        {
            get { return _ContactResisTest; }
            set { _ContactResisTest = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_ITR8140_001A_ContactResisTest> _ContactResisTestList { get; set; }

        public ObservableCollection<T_ITR8140_001A_ContactResisTest> ContactResisTestList
        {
            get { return _ContactResisTestList; }
            set { _ContactResisTestList = value; RaisePropertyChanged(); }
        }
        private T_ITR8140_001AInsulationResistanceTest _InsulationResistanceTest { get; set; }
        public T_ITR8140_001AInsulationResistanceTest InsulationResistanceTest
        {
            get { return _InsulationResistanceTest; }
            set { _InsulationResistanceTest = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_ITR8140_001AInsulationResistanceTest> _InsulationRTests { get; set; }

        public ObservableCollection<T_ITR8140_001AInsulationResistanceTest> InsulationRTests
        {
            get { return _InsulationRTests; }
            set { _InsulationRTests = value; RaisePropertyChanged(); }
        }
        private T_ITR8140_001ATestInstrucitonData _TestInstrucitonData;
        public T_ITR8140_001ATestInstrucitonData TestInstrucitonData
        {
            get { return _TestInstrucitonData; }
            set { _TestInstrucitonData = value; RaisePropertyChanged(); }
        }
        private T_ITR8140_001ADialectricTest diTest { get; set; }
        public T_ITR8140_001ADialectricTest _diTest
        {
            get { return diTest; }
            set
            {
                diTest = value; RaisePropertyChanged();
            }
        }
        private ObservableCollection<T_ITR8140_001ADialectricTest> diTests;
        public ObservableCollection<T_ITR8140_001ADialectricTest> DieTests
        {
            get { return diTests; }
            set
            {
                diTests = value; RaisePropertyChanged();
            }
        }
        private ObservableCollection<TestEquipmentDataModel> _TestEquipmentDataModelList;
        public ObservableCollection<TestEquipmentDataModel> TestEquipmentDataModelList
        {
            get { return _TestEquipmentDataModelList; }
            set
            {
                _TestEquipmentDataModelList = value; RaisePropertyChanged();
            }
        }

        private ObservableCollection<T_ITRInstrumentData> _ITRInstrumentDataList;
        public ObservableCollection<T_ITRInstrumentData> ITRInstrumentDataList
        {
            get { return _ITRInstrumentDataList; }
            set
            {
                _ITRInstrumentDataList = value; RaisePropertyChanged();
            }
        }
        private T_ITRInstrumentData _SelectedITRInstrument;
        public T_ITRInstrumentData SelectedITRInstrument
        {
            get { return _SelectedITRInstrument; }
            set
            {
                _SelectedITRInstrument = value; RaisePropertyChanged();
            }
        }

        // 8140_002A

        private bool _itr_8140_002AFrame;
        public bool ITR_8140_002AFrame
        {
            get { return _itr_8140_002AFrame; }
            set { _itr_8140_002AFrame = value; RaisePropertyChanged(); }
        }

        private T_ITR_8140_002A_Records iTR8140_002ARecords;
        public T_ITR_8140_002A_Records ITR8140_002ARecords
        {
            get { return iTR8140_002ARecords; }
            set { iTR8140_002ARecords = value; RaisePropertyChanged(); }
        }

        private T_ITR_8140_002A_RecordsMechnicalOperation iTR8140_002ARecordsMO;
        public T_ITR_8140_002A_RecordsMechnicalOperation ITR8140_002ARecordsMO
        {
            get { return iTR8140_002ARecordsMO; }
            set { iTR8140_002ARecordsMO = value; RaisePropertyChanged(); }
        }
        private T_ITR_8140_002A_RecordsAnalogSignal iTR8140_002ARecordsAS;
        public T_ITR_8140_002A_RecordsAnalogSignal ITR8140_002ARecordsAS
        {
            get { return iTR8140_002ARecordsAS; }
            set { iTR8140_002ARecordsAS = value; RaisePropertyChanged(); }
        }

        // 8170-007A

        private bool _itr_8170_007AFrame;
        public bool ITR_8170_007AFrame
        {
            get { return _itr_8170_007AFrame; }
            set { _itr_8170_007AFrame = value; RaisePropertyChanged(); }
        }

        private T_ITR_8170_007A_OP_Function_Test iTR_8170_007AOP_FT;
        public T_ITR_8170_007A_OP_Function_Test ITR_8170_007AOP_FT
        {
            get { return iTR_8170_007AOP_FT; }
            set { iTR_8170_007AOP_FT = value; RaisePropertyChanged(); }
        }

        private bool _IsSimpleITR;
        public bool IsSimpleITR
        {
            get { return _IsSimpleITR; }
            set { _IsSimpleITR = value; RaisePropertyChanged(); }
        }

        private bool _IsStanderdITR;
        public bool IsStanderdITR
        {
            get { return _IsStanderdITR; }
            set { _IsStanderdITR = value; RaisePropertyChanged(); }
        }

        private List<string> _FunctionCheckList;
        public List<string> FunctionCheckList
        {
            get { return _FunctionCheckList; }
            set { _FunctionCheckList = value; RaisePropertyChanged(); }
        }


        private bool simpleITR;
        public bool SimpleITR
        {
            get { return simpleITR; }
            set { simpleITR = value; RaisePropertyChanged(); }
        }

        // 8140_004A

        private bool _itr_8140_004AFrame;
        public bool ITR_8140_004AFrame
        {
            get { return _itr_8140_004AFrame; }
            set { _itr_8140_004AFrame = value; RaisePropertyChanged(); }
        }

        private T_ITR_8140_004A_Records iTR8140_004ARecords;
        public T_ITR_8140_004A_Records ITR8140_004ARecords
        {
            get { return iTR8140_004ARecords; }
            set { iTR8140_004ARecords = value; RaisePropertyChanged(); }
        }

        public ICommand ClickCommandITR8140_001A
        {
            get { return new Command<string>(clickCm); }

        }

        #endregion

        public async void clickCm(string param)
        {
            try
            {
                if (!App.IsBusy)
                {
                    App.IsBusy = true;
                    if (param == "AddNewItem")
                    {
                        var ContactResisTest = await _T_ITR8140_001A_ContactResisTestRepository.GetAsync();
                        long CRTID = ContactResisTest.Count() + 1;
                        T_ITR8140_001A_ContactResisTest CRT = new T_ITR8140_001A_ContactResisTest
                        {
                            RowID = CRTID,
                            row_Id = ContactResisTestList.Count + 1,
                            ITRCommonID = CommonHeaderFooter.ID,
                            CommonRowID = CommonHeaderFooter.ROWID,
                            ModelName = Settings.ModelName,
                            TorqueMarkOkValue = "",
                            CCMS_HEADERID = (int)CommonHeaderFooter.ID,
                        };
                        await _T_ITR8140_001A_ContactResisTestRepository.InsertOrReplaceAsync(CRT);
                        ContactResisTestList.Add(CRT);
                        ContactResisTestList.Skip(1).ForEach(x => x.IsUpdated = true);
                        _userDialogs.Toast("New item added!");
                    }
                    else if (param == "deleteit")
                    {
                        if (ContactResisTest != null)
                        {
                            if (await _userDialogs.ConfirmAsync("Are you sure you want to delete?", "Delete Record", "Yes", "No"))
                            {
                                T_ITR8140_001A_ContactResisTest DeleteItem = ContactResisTestList.Where(x => x.RowID == ContactResisTest.RowID).FirstOrDefault();
                                ContactResisTestList.Remove(DeleteItem);
                                await _T_ITR8140_001A_ContactResisTestRepository.DeleteAsync(DeleteItem);
                                int i = 1;
                                ContactResisTestList.ForEach(x => x.row_Id = i++);
                            }
                        }
                    }

                    else if (param == "AddNewItemITR8300_003A")
                    {
                        var IlluminList = await _ITR_8300_003A_IlluminRepository.GetAsync();
                        long IlluminID = IlluminList.Count() + 1;
                        if (IlluminID > 10) return;
                        T_ITR_8300_003A_Illumin Illumin = new T_ITR_8300_003A_Illumin
                        {
                            RowID = IlluminID,
                            ITRCommonID = CommonHeaderFooter.ID,
                            CommonRowID = CommonHeaderFooter.ROWID,
                            ModelName = Settings.ModelName,
                            CCMS_HEADERID = (int)CommonHeaderFooter.ID,
                        };
                        await _ITR_8300_003A_IlluminRepository.InsertOrReplaceAsync(Illumin);
                        ITR_8300_003A_IlluminList.Add(Illumin);
                        ITR_8300_003A_IlluminList.Skip(1).ForEach(x => x.IsUpdated = true);
                        _userDialogs.Toast("New item added!");

                    }
                    else if (param == "deleteitITR8300_003A")
                    {
                        if (ItemITR_8300_003A_Illumin != null)
                        {
                            if (await _userDialogs.ConfirmAsync("Are you sure you want to delete?", "Delete Record", "Yes", "No"))
                            {
                                T_ITR_8300_003A_Illumin DeleteItem = ITR_8300_003A_IlluminList.Where(x => x.RowID == ItemITR_8300_003A_Illumin.RowID).FirstOrDefault();
                                ITR_8300_003A_IlluminList.Remove(DeleteItem);
                                await _ITR_8300_003A_IlluminRepository.DeleteAsync(DeleteItem);
                                int i = 1;
                                ITR_8300_003A_IlluminList.ForEach(x => x.RowID = i++);
                            }
                        }
                    }
                    App.IsBusy = false;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                App.IsBusy = false;
            }
        }

        //private bool iTR_8161001AFrame;
        //public bool ITR_8161001AFrame
        //{
        //    get { return iTR_8161001AFrame; }
        //    set { iTR_8161001AFrame = value; RaisePropertyChanged(); }
        //}

        private bool itr8121_004AFrame;
        public bool Itr8121_004AFrame
        {
            get { return itr8121_004AFrame; }
            set { itr8121_004AFrame = value; RaisePropertyChanged(); }
        }

        private bool iTR_8000_101AFrame;
        public bool ITR_8000_101AFrame
        {
            get { return iTR_8000_101AFrame; }
            set { iTR_8000_101AFrame = value; RaisePropertyChanged(); }
        }

        private bool iTR_8211_002AFrame;
        public bool ITR_8211_002AFrame
        {
            get { return iTR_8211_002AFrame; }
            set { iTR_8211_002AFrame = value; RaisePropertyChanged(); }
        }

        private bool iTR_8300_003AFrame;
        public bool ITR_8300_003AFrame
        {
            get { return iTR_8300_003AFrame; }
            set { iTR_8300_003AFrame = value; RaisePropertyChanged(); }
        }
        //ITR_8121_002A

        #region ITR_8121_002A
        private bool _iTR_8121_002AFrame;
        public bool ITR_8121_002AFrame
        {
            get { return _iTR_8121_002AFrame; }
            set { _iTR_8121_002AFrame = value; RaisePropertyChanged(); }
        }

        private List<string> _WiringCheckList;
        public List<string> WiringCheckList
        {
            get { return _WiringCheckList; }
            set { _WiringCheckList = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_ITR8121_002A_TransformerRadioTest> _TransformerRadioTestList;
        public ObservableCollection<T_ITR8121_002A_TransformerRadioTest> TransformerRadioTestList
        {
            get { return _TransformerRadioTestList; }
            set { _TransformerRadioTestList = value; RaisePropertyChanged(); }
        }
        private T_ITR8121_002A_TransformerRadioTest _selectedTransRatioTest8121002;
        public T_ITR8121_002A_TransformerRadioTest SelectedTransRatioTest8121002
        {
            get { return _selectedTransRatioTest8121002; }
            set { _selectedTransRatioTest8121002 = value; RaisePropertyChanged(); }
        }

        private T_ITR8121_002A_TransformerRadioTest _SelectedTransformerRadioTest;
        public T_ITR8121_002A_TransformerRadioTest SelectedTransformerRadioTest
        {
            get { return _SelectedTransformerRadioTest; }
            set { _SelectedTransformerRadioTest = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents> _InspectionforControls;
        public ObservableCollection<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents> InspectionforControls
        {
            get { return _InspectionforControls; }
            set { _InspectionforControls = value; RaisePropertyChanged(); }
        }

        private T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents _SelectedInspectionforControls;
        public T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents SelectedInspectionforControls
        {
            get { return _SelectedInspectionforControls; }
            set { _SelectedInspectionforControls = value; RaisePropertyChanged(); }
        }

        private T_ITR8121_002A_Records _Records8121_002A;
        public T_ITR8121_002A_Records Records8121_002A
        {
            get { return _Records8121_002A; }
            set { _Records8121_002A = value; RaisePropertyChanged(); }
        }

        #endregion


        #region ITR_8260_002A
        private int die8260_002AHeight;
        public int Die8260_002AHeight
        {
            get { return die8260_002AHeight; }
            set { die8260_002AHeight = value; RaisePropertyChanged(); }
        }

        private bool _iTR_8260_002AFrame;
        public bool ITR_8260_002AFrame
        {
            get { return _iTR_8260_002AFrame; }
            set { _iTR_8260_002AFrame = value; RaisePropertyChanged(); }
        }


        private T_ITR_8260_002A_Body _Body8260_002A;
        public T_ITR_8260_002A_Body Body8260_002A
        {
            get { return _Body8260_002A; }
            set { _Body8260_002A = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_ITR_8260_002A_DielectricTest> _DielectricTestRecords;
        public ObservableCollection<T_ITR_8260_002A_DielectricTest> DielectricTestRecords
        {
            get { return _DielectricTestRecords; }
            set { _DielectricTestRecords = value; RaisePropertyChanged(); }
        }
        private T_ITR_8260_002A_DielectricTest _dielectricRecord;
        public T_ITR_8260_002A_DielectricTest DielectricRecord
        {
            get { return _dielectricRecord; }
            set { _dielectricRecord = value; RaisePropertyChanged(); }
        }
        private string _InsRes1_1 = "";
        public string InsRes1_1
        {
            get { return _InsRes1_1; }
            set { _InsRes1_1 = value; RaisePropertyChanged(); }
        }
        private string _InsRes1_2 = "";
        public string InsRes1_2
        {
            get { return _InsRes1_2; }
            set { _InsRes1_2 = value; RaisePropertyChanged(); }
        }

        private string _InsRes2_1 = "";
        public string InsRes2_1
        {
            get { return _InsRes2_1; }
            set { _InsRes2_1 = value; RaisePropertyChanged(); }
        }
        private string _InsRes2_2 = "";
        public string InsRes2_2
        {
            get { return _InsRes2_2; }
            set { _InsRes2_2 = value; RaisePropertyChanged(); }
        }

        private string _InsRes1_3 = "";
        public string InsRes1_3
        {
            get { return _InsRes1_3; }
            set { _InsRes1_3 = value; RaisePropertyChanged(); }
        }
        private string _InsRes2_3 = "";
        public string InsRes2_3
        {
            get { return _InsRes2_3; }
            set { _InsRes2_3 = value; RaisePropertyChanged(); }
        }

        private string _AppliedPoint_1 = "";
        public string AppliedPoint_1
        {
            get { return _AppliedPoint_1; }
            set { _AppliedPoint_1 = value; RaisePropertyChanged(); }
        }
        private string _AppliedPoint_2 = "";
        public string AppliedPoint_2
        {
            get { return _AppliedPoint_2; }
            set { _AppliedPoint_2 = value; RaisePropertyChanged(); }
        }
        private string _AppliedPoint_3 = "";
        public string AppliedPoint_3
        {
            get { return _AppliedPoint_3; }
            set { _AppliedPoint_3 = value; RaisePropertyChanged(); }
        }

        private string _ChargeCurrent_1 = "";
        public string ChargeCurrent_1
        {
            get { return _ChargeCurrent_1; }
            set { _ChargeCurrent_1 = value; RaisePropertyChanged(); }
        }

        private string _ChargeCurrent_2 = "";
        public string ChargeCurrent_2
        {
            get { return _ChargeCurrent_2; }
            set { _ChargeCurrent_2 = value; RaisePropertyChanged(); }
        }

        private string _ChargeCurrent_3 = "";
        public string ChargeCurrent_3
        {
            get { return _ChargeCurrent_3; }
            set { _ChargeCurrent_3 = value; RaisePropertyChanged(); }
        }
        private bool _IsReqInsRes1_1;
        public bool IsReqInsRes1_1
        {
            get { return _IsReqInsRes1_1; }
            set { _IsReqInsRes1_1 = value; RaisePropertyChanged(); }
        }
        private bool _IsReqInsRes1_2;
        public bool IsReqInsRes1_2
        {
            get { return _IsReqInsRes1_2; }
            set { _IsReqInsRes1_2 = value; RaisePropertyChanged(); }
        }

        private bool _IsReqInsRes2_1;
        public bool IsReqInsRes2_1
        {
            get { return _IsReqInsRes2_1; }
            set { _IsReqInsRes2_1 = value; RaisePropertyChanged(); }
        }
        private bool _IsReqInsRes2_2;
        public bool IsReqInsRes2_2
        {
            get { return _IsReqInsRes2_2; }
            set { _IsReqInsRes2_2 = value; RaisePropertyChanged(); }
        }

        private bool _IsReqInsRes1_3;
        public bool IsReqInsRes1_3
        {
            get { return _IsReqInsRes1_3; }
            set { _IsReqInsRes1_3 = value; RaisePropertyChanged(); }
        }
        private bool _IsReqInsRes2_3;
        public bool IsReqInsRes2_3
        {
            get { return _IsReqInsRes2_3; }
            set { _IsReqInsRes2_3 = value; RaisePropertyChanged(); }
        }

        private bool _IsReqAppliedPoint_1;
        public bool IsReqAppliedPoint_1
        {
            get { return _IsReqAppliedPoint_1; }
            set { _IsReqAppliedPoint_1 = value; RaisePropertyChanged(); }
        }
        private bool _IsReqAppliedPoint_2;
        public bool IsReqAppliedPoint_2
        {
            get { return _IsReqAppliedPoint_2; }
            set { _IsReqAppliedPoint_2 = value; RaisePropertyChanged(); }
        }
        private bool _IsReqAppliedPoint_3;
        public bool IsReqAppliedPoint_3
        {
            get { return _IsReqAppliedPoint_3; }
            set { _IsReqAppliedPoint_3 = value; RaisePropertyChanged(); }
        }

        private bool _IsReqChargeCurrent_1;
        public bool IsReqChargeCurrent_1
        {
            get { return _IsReqChargeCurrent_1; }
            set { _IsReqChargeCurrent_1 = value; RaisePropertyChanged(); }
        }

        private bool _IsReqChargeCurrent_2;
        public bool IsReqChargeCurrent_2
        {
            get { return _IsReqChargeCurrent_2; }
            set { _IsReqChargeCurrent_2 = value; RaisePropertyChanged(); }
        }

        private bool _IsReqChargeCurrent_3;
        public bool IsReqChargeCurrent_3
        {
            get { return _IsReqChargeCurrent_3; }
            set { _IsReqChargeCurrent_3 = value; RaisePropertyChanged(); }
        }

        private string _TestPhase_1 = "";
        public string TestPhase_1
        {
            get { return _TestPhase_1; }
            set { _TestPhase_1 = value; RaisePropertyChanged(); }
        }

        private string _TestPhase_2 = "";
        public string TestPhase_2
        {
            get { return _TestPhase_2; }
            set { _TestPhase_2 = value; RaisePropertyChanged(); }
        }

        private string _TestPhase_3 = "";
        public string TestPhase_3
        {
            get { return _TestPhase_3; }
            set { _TestPhase_3 = value; RaisePropertyChanged(); }
        }
        #endregion

        private T_ITR8121_004ATestInstrumentData testInstrumentData8121_004A;
        public T_ITR8121_004ATestInstrumentData TestInstrumentData8121_004A
        {

            get { return testInstrumentData8121_004A; }
            set { testInstrumentData8121_004A = value; RaisePropertyChanged(); }
        }
        //private bool _Itr8121_004AFrame;
        //public bool Itr8121_004AFrame
        //{
        //    get { return _Itr8121_004AFrame; }
        //    set { _Itr8121_004AFrame = value; RaisePropertyChanged(); }
        //}
        private bool iTR_8161_002AFrame;
        public bool ITR_8161_002AFrame
        {

            get { return iTR_8161_002AFrame; }
            set { iTR_8161_002AFrame = value; RaisePropertyChanged(); }
        }
        private T_ITR8161_002A_Body _ITRRecord8161_2XA;
        public T_ITR8161_002A_Body ITRRecord8161_2XA
        {
            get { return _ITRRecord8161_2XA; }
            set { _ITRRecord8161_2XA = value; RaisePropertyChanged(); }
        }
        private List<T_ITR8161_002A_DielectricTest> _ITR_8161_002A_DielectricTest;
        public List<T_ITR8161_002A_DielectricTest> ITR_8161_002A_DielectricTest
        {
            get { return _ITR_8161_002A_DielectricTest; }
            set { _ITR_8161_002A_DielectricTest = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_ITR8121_004AInCAndAuxiliary> _InispactionForControlAndAuxiliary8121;
        public ObservableCollection<T_ITR8121_004AInCAndAuxiliary> InispactionForControlAndAuxiliary8121
        {
            get { return _InispactionForControlAndAuxiliary8121; }
            set { _InispactionForControlAndAuxiliary8121 = value; RaisePropertyChanged(); }
        }
        private T_ITR8121_004AInCAndAuxiliary _SelectedInForConAndAuxiliary8121;
        public T_ITR8121_004AInCAndAuxiliary SelectedInForConAndAuxiliary8121
        {
            get { return _SelectedInForConAndAuxiliary8121; }
            set { _SelectedInForConAndAuxiliary8121 = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_ITR8121_004ATransformerRatioTest> _TransformerRatioTest8121;
        public ObservableCollection<T_ITR8121_004ATransformerRatioTest> TransformerRatioTest8121
        {
            get { return _TransformerRatioTest8121; }
            set { _TransformerRatioTest8121 = value; RaisePropertyChanged(); }
        }
        private T_ITR8121_004ATransformerRatioTest _SelectedTransformerRatioTest8121;
        public T_ITR8121_004ATransformerRatioTest SelectedTransformerRatioTest8121
        {
            get { return _SelectedTransformerRatioTest8121; }
            set { _SelectedTransformerRatioTest8121 = value; RaisePropertyChanged(); }
        }
        private T_ITR8000_101A_Generalnformation iTR8000_101AGenlnfo;
        public T_ITR8000_101A_Generalnformation ITR8000_101AGenlnfo
        {
            get { return iTR8000_101AGenlnfo; }
            set { iTR8000_101AGenlnfo = value; RaisePropertyChanged(); }
        }
        private T_ITR8000_101A_RecordISBarrierDetails iTR8000_101AISBarDetails;
        public T_ITR8000_101A_RecordISBarrierDetails ITR8000_101AISBarDetails
        {
            get { return iTR8000_101AISBarDetails; }
            set { iTR8000_101AISBarDetails = value; RaisePropertyChanged(); }
        }
        private List<string> _ResultAcceptedList;
        public List<string> ITR8000101A_ResultAccepted
        {
            get { return _ResultAcceptedList; }
            set { _ResultAcceptedList = value; RaisePropertyChanged(); }
        }
        private List<string> _PunchList;
        public List<string> PunchList
        {
            get { return _PunchList; }
            set { _PunchList = value; RaisePropertyChanged(); }
        }
        private T_ITRRecords_8211_002A_Body _ITRRecord8211_002A;
        public T_ITRRecords_8211_002A_Body ITRRecord8211_002A
        {
            get { return _ITRRecord8211_002A; }
            set { _ITRRecord8211_002A = value; RaisePropertyChanged(); }
        }
        private List<T_ITRRecords_8211_002A_RunTest> _ITR_8211_002A_RunTest;
        public List<T_ITRRecords_8211_002A_RunTest> ITR_8211_002A_RunTest
        {
            get { return _ITR_8211_002A_RunTest; }
            set { _ITR_8211_002A_RunTest = value; RaisePropertyChanged(); }
        }
        private T_ITRRecords_8211_002A_RunTest _Item_8211_002A_RunTest;
        public T_ITRRecords_8211_002A_RunTest Item_8211_002A_RunTest
        {
            get { return _Item_8211_002A_RunTest; }
            set { _Item_8211_002A_RunTest = value; RaisePropertyChanged(); }
        }
        private string _Lbl1;
        public string Lbl1
        {
            get { return _Lbl1; }
            set { _Lbl1 = value; RaisePropertyChanged(); }
        }
        private string _Lbl2;
        public string Lbl2
        {
            get { return _Lbl2; }
            set { _Lbl2 = value; RaisePropertyChanged(); }
        }
        private string _Lbl3;
        public string Lbl3
        {
            get { return _Lbl3; }
            set { _Lbl3 = value; RaisePropertyChanged(); }
        }
        private string _Lbl4;
        public string Lbl4
        {
            get { return _Lbl4; }
            set { _Lbl4 = value; RaisePropertyChanged(); }
        }



        //8300-004A
        private T_ITR_8300_003A_Body _ITR_8300_003A_Body;
        public T_ITR_8300_003A_Body ITRRecord_8300_003A_Body
        {
            get { return _ITR_8300_003A_Body; }
            set { _ITR_8300_003A_Body = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_ITR_8300_003A_Illumin> _ITR_8300_003A_IlluminList;
        public ObservableCollection<T_ITR_8300_003A_Illumin> ITR_8300_003A_IlluminList
        {
            get { return _ITR_8300_003A_IlluminList; }
            set { _ITR_8300_003A_IlluminList = value; RaisePropertyChanged(); }
        }
        private T_ITR_8300_003A_Illumin _ITR_8300_003A_Illumin;
        public T_ITR_8300_003A_Illumin ItemITR_8300_003A_Illumin
        {
            get { return _ITR_8300_003A_Illumin; }
            set { _ITR_8300_003A_Illumin = value; RaisePropertyChanged(); }
        }

        //ITR_7000_101AFrame

        private bool itr_7000_101AFrame;
        public bool ITR_7000_101AFrame
        {
            get { return itr_7000_101AFrame; }
            set { itr_7000_101AFrame = value; RaisePropertyChanged(); }
        }
        private T_ITR_7000_101AHarmony_Genlnfo iTR7000_101AHarmonyGenlnfo;
        public T_ITR_7000_101AHarmony_Genlnfo ITR7000_101AHarmonyGenlnfo
        {
            get { return iTR7000_101AHarmonyGenlnfo; }
            set { iTR7000_101AHarmonyGenlnfo = value; RaisePropertyChanged(); }
        }
        private T_ITR_7000_101AHarmony_ActivityDetails iTR7000_101AHarmony_ActivityDetails;
        public T_ITR_7000_101AHarmony_ActivityDetails ITR7000_101AHarmony_ActivityDetails
        {
            get { return iTR7000_101AHarmony_ActivityDetails; }
            set { iTR7000_101AHarmony_ActivityDetails = value; RaisePropertyChanged(); }
        }

        //8170-002A
        private bool _ITR_8170_002AFrame;
        public bool ITR_8170_002AFrame
        {
            get { return _ITR_8170_002AFrame; }
            set { _ITR_8170_002AFrame = value; RaisePropertyChanged(); }
        }
        private List<T_ITRRecords_8170_002A_Voltage_Reading> _ITR8170_002A_Voltage_Reading;
        public List<T_ITRRecords_8170_002A_Voltage_Reading> ITR_8170_002A_Voltage_Reading
        {
            get { return _ITR8170_002A_Voltage_Reading; }
            set { _ITR8170_002A_Voltage_Reading = value; RaisePropertyChanged(); }
        }
        private T_ITRRecords_8170_002A_Voltage_Reading _Item8170_002A_Voltage_Reading;
        public T_ITRRecords_8170_002A_Voltage_Reading Item8170_002A_Voltage_Reading
        {
            get { return _Item8170_002A_Voltage_Reading; }
            set { _Item8170_002A_Voltage_Reading = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_ITRRecords_8170_002A_Voltage_Reading> _ITR8170_002A_Voltage_ReadingList;
        public ObservableCollection<T_ITRRecords_8170_002A_Voltage_Reading> ITR8170_002A_Voltage_ReadingList
        {
            get { return _ITR8170_002A_Voltage_ReadingList; }
            set { _ITR8170_002A_Voltage_ReadingList = value; RaisePropertyChanged(); }
        }
        private T_ITR_8170_002A_IndifictionLamp _ITR8170_002A_IndifictionLamp;
        public T_ITR_8170_002A_IndifictionLamp ITR_8170_002A_IndifictionLamp
        {
            get { return _ITR8170_002A_IndifictionLamp; }
            set { _ITR8170_002A_IndifictionLamp = value; RaisePropertyChanged(); }
        }
        private T_ITR_8170_002A_InsRes _ITR8170_002A_InsRes;
        public T_ITR_8170_002A_InsRes ITR_8170_002A_InsRes
        {
            get { return _ITR8170_002A_InsRes; }
            set { _ITR8170_002A_InsRes = value; RaisePropertyChanged(); }
        }
        //private ObservableCollection<T_ITRRecords_8170_002A_Voltage_Reading> _ITR8170_002A_Voltage_Reading;
        //public ObservableCollection<T_ITRRecords_8170_002A_Voltage_Reading> ITR_8170_002A_Voltage_Reading
        //{
        //    get { return _ITR8170_002A_Voltage_Reading; }
        //    set { _ITR8170_002A_Voltage_Reading = value; RaisePropertyChanged(); }
        //}
        #endregion

        public async Task<bool> OnCheckSheetSelectionChage()
        {
            try
            {
                if (!await _ITRService.IsImplementedITR(SelectedCheckSheet.ITRNumber))
                    return false;
                // string IsSQL = " SELECT * FROM T_ITRCommonHeaderFooter WHERE Tag = '" + SelectedTag.name + "' AND ITRNumber = '" + SelectedCheckSheet.CHECKSHEETNAME + "' AND ModelName = '" + Settings.ModelName + "'";
                // var IsHeaderExist = await _CommonHeaderFooterRepository.QueryAsync(IsSQL);
                var HeaderFooter = await _CommonHeaderFooterRepository.QueryAsync("Select * From T_ITRCommonHeaderFooter WHERE Tag = '" + SelectedCheckSheet.Tag + "' AND ITRNumber = '" + SelectedCheckSheet.ITRNumber + "' AND ModelName = '" + Settings.ModelName + "'");

                if (HeaderFooter.Count <= 0)
                    return false;

                CommonHeaderFooter = ItrSheetHeaders = HeaderFooter.FirstOrDefault();

                DescriptionHeader = CommonHeaderFooter.ITRDescription;
                GetITRInstrumentData();
                //CommonHeaderFooter = HeaderFooter.FirstOrDefault();
                //string _AFINo = string.Empty;
                //string _ReportNo = string.Empty;
                //string _ITRRev = string.Empty;
                //string DwgRevNo = string.Empty;
                ////get tag headers 
                //var tagHeaders = await _TagSheetHeaderRepository.GetAsync(x => x.ChecksheetName == SelectedCheckSheet.CHECKSHEETNAME && x.TagName == SelectedCheckSheet.TAGNAME);

                //tagHeaders.Where(x => x.ColumnKey == "TagCableNo").ForEach(s => s.ColumnKey = "Tag No");
                //tagHeaders.Where(x => x.ColumnKey == "Description").ForEach(s => s.ColumnKey = "Tag Description");
                //tagHeaders.Where(x => x.ColumnKey == "TagFunction").ForEach(s => s.ColumnKey = "FC Level 1");
                //tagHeaders.Where(x => x.ColumnKey == "TagCategory").ForEach(s => s.ColumnKey = "FC Level 2");
                //tagHeaders.Where(x => x.ColumnKey == "TagClass").ForEach(s => s.ColumnKey = "FC Level 3");
                //tagHeaders.Where(x => x.ColumnKey == "Jobcode").ForEach(s => s.ColumnValue = CommonHeaderFooter.JobCode);
                //tagHeaders.Where(x => x.ColumnKey == "PCWBS").ForEach(s => s.ColumnValue = CommonHeaderFooter.PCWBS);
                //tagHeaders.Where(x => x.ColumnKey == "Drawing").ForEach(s => { s.ColumnKey = "Drawing No"; s.ColumnValue = CommonHeaderFooter.DrawingNo; });
                //_AFINo = CommonHeaderFooter.AFINo;
                //_ReportNo = CommonHeaderFooter.ReportNo;
                //_ITRRev = CommonHeaderFooter.ITRRev;
                //DwgRevNo = CommonHeaderFooter.DrawingRev;
                // CurrentCheckSheetRevNo = CommonHeaderFooter.DrawingRev;
                // DescriptionHeader = CommonHeaderFooter.ITRDescription;


                //tagHeaders.Insert(0, new T_TAG_SHEET_HEADER
                //{
                //    ColumnKey = "ITR REV",
                //    ColumnValue = _ITRRev,
                //    TagName = tagHeaders.FirstOrDefault().TagName,
                //    ProjectName = tagHeaders.FirstOrDefault().ProjectName,
                //    ChecksheetName = tagHeaders.FirstOrDefault().ChecksheetName,
                //    JobPack = tagHeaders.FirstOrDefault().JobPack
                //});
                //tagHeaders.Insert(1, new T_TAG_SHEET_HEADER
                //{
                //    ColumnKey = "Report No",
                //    ColumnValue = _ReportNo,
                //    TagName = tagHeaders.FirstOrDefault().TagName,
                //    ProjectName = tagHeaders.FirstOrDefault().ProjectName,
                //    ChecksheetName = tagHeaders.FirstOrDefault().ChecksheetName,
                //    JobPack = tagHeaders.FirstOrDefault().JobPack
                //});
                //tagHeaders.Insert(7, new T_TAG_SHEET_HEADER
                //{
                //    ColumnKey = "REV NO",
                //    ColumnValue = DwgRevNo,
                //    TagName = tagHeaders.FirstOrDefault().TagName,
                //    ProjectName = tagHeaders.FirstOrDefault().ProjectName,
                //    ChecksheetName = tagHeaders.FirstOrDefault().ChecksheetName,
                //    JobPack = tagHeaders.FirstOrDefault().JobPack
                //});

                //tagHeaders.Add(new T_TAG_SHEET_HEADER
                //{
                //    ColumnKey = "AFI No",
                //    ColumnValue = _AFINo,
                //    TagName = tagHeaders.FirstOrDefault().TagName,
                //    ProjectName = tagHeaders.FirstOrDefault().ProjectName,
                //    ChecksheetName = tagHeaders.FirstOrDefault().ChecksheetName,
                //    JobPack = tagHeaders.FirstOrDefault().JobPack
                //});

                //// _ = Task.Delay(100);
                //TagSheetHeadersList = new ObservableCollection<T_TAG_SHEET_HEADER>(tagHeaders);

                //Check USer Accesssss
                IsVisibleRejectButton = false;
                comparer = new List<bool>();
                if (SelectedCheckSheet.StatusColor != ITR_STATUS_COMPLETE)
                {
                    IsChecksheetIntiled = false;
                    CheckSheetCompleted = false;
                    IsCheckSheetAccessible = true;
                }
                else //OpenITR();
                    IsCheckSheetAccessible = true;
                //Get CheckSheets

                var checkSheets = await _CheckSheetQuetionsRepository.GetAsync(x => x.CheckSheet == SelectedCheckSheet.ITRNumber);

                var checkSheetsPerCategory = checkSheets.GroupBy(i => i.section);

                //get Jack 
                var checkSheetPerTag = await _CheckSheetPerTagRepository.GetAsync(x => x.CHECKSHEETNAME == SelectedCheckSheet.ITRNumber && x.TAGNAME == SelectedTag.name);


                //    //get CheckSheet Data
                var _CurrentCheckSheet = await _CheckSheetRepository.GetAsync(x => x.name == SelectedCheckSheet.ITRNumber);
                CurrentCheckSheet = _CurrentCheckSheet.FirstOrDefault();
                string ansCSsql = " SELECT * FROM T_TAG_SHEET_ANSWER WHERE tagName='" + SelectedTag.name + "' AND checkSheetName='" + SelectedCheckSheet.ITRNumber + "' AND jobPack = '" + checkSheetPerTag.FirstOrDefault().JOBPACK.Trim() + "' or jobPack ISNULL";
                var Answer_CheckSheet = await _TAG_SHEET_ANSWERRepository.QueryAsync(ansCSsql);

                List<string> PickerList = new List<string> { "Y", "N", "NA" };
                PickerValueList = new ObservableCollection<string>(PickerList);
                LoadITRData();
                GetSignOffList();
                GetFormData();


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void GetFormData()
        {
            try
            {
                ItrSheetHeaders.IsDrowingTextShow = true;
                ItrSheetHeaders.IsDrowingDropDownShow = false;
                string ddlJointnoListJSON = ModsTools.CompletionWebServiceGet(ApiUrls.Url_GetFormHeaderDatafromDatahub(Settings.ProjectID, SelectedCheckSheet.Tag, Settings.CurrentDB), Settings.CompletionAccessToken);
                if (ddlJointnoListJSON != string.Empty)
                {
                    var DrowingData = JsonConvert.DeserializeObject<DrowingData>(ddlJointnoListJSON);
                    if (DrowingData != null)
                    {
                        bool isempty = (string.IsNullOrEmpty(ItrSheetHeaders.DrawingNo)) ? true : false;

                        List<string> docNos = DrowingData.Document_No;
                        DrawingNoList = new List<string>();
                        if (docNos != null && docNos.Count > 1)
                        {
                            DrawingNoList = docNos;
                            if (isempty)
                                ItrSheetHeaders.SelectedDrawingNo = DrawingNoList.First();
                            else
                                ItrSheetHeaders.SelectedDrawingNo = ItrSheetHeaders.DrawingNo;
                            ItrSheetHeaders.IsDrowingDropDownShow = true;
                            ItrSheetHeaders.IsDrowingTextShow = false;
                        }
                        else if (docNos.Count == 1)
                        {
                            if (isempty)
                                ItrSheetHeaders.DrawingNo = docNos.FirstOrDefault();
                            ItrSheetHeaders.IsDrowingDropDownShow = false;
                            ItrSheetHeaders.IsDrowingTextShow = true;
                        }
                        else
                        {
                            if (isempty)
                                ItrSheetHeaders.DrawingNo = docNos.FirstOrDefault();
                            ItrSheetHeaders.IsDrowingDropDownShow = false;
                            ItrSheetHeaders.IsDrowingTextShow = true;
                        }

                        ItrSheetHeaders = ItrSheetHeaders;
                        DrowingNo_SelectionChanged();
                    }
                }
            }
            catch (Exception e)
            {
                ItrSheetHeaders.IsDrowingTextShow = true;
            }
        }

        public void DrowingNo_SelectionChanged()
        {
            try
            {
                DrowingData itr7000SeriesFormHeader;
                string ddlJointnoListJSON = ModsTools.CompletionWebServiceGet(ApiUrls.Url_GetDrowRevfromDatahub(Settings.ProjectID, ItrSheetHeaders.SelectedDrawingNo, Settings.CurrentDB), Settings.CompletionAccessToken);
                if (ddlJointnoListJSON != string.Empty)
                {
                    itr7000SeriesFormHeader = JsonConvert.DeserializeObject<DrowingData>(ddlJointnoListJSON);
                    if (itr7000SeriesFormHeader != null)
                    {
                        ItrSheetHeaders.DrawingRev = itr7000SeriesFormHeader.Rev_No.ToString();
                        ItrSheetHeaders.DrawingNo = ItrSheetHeaders.SelectedDrawingNo;
                        ItrSheetHeaders = ItrSheetHeaders;
                    }
                }
            }
            catch
            { }
        }

        public async void OpenITR()
        {
            var _credentials = await ReadLoginPopup("OpenITR");
            if (_credentials != null)
            {
                var HasAccess = await CheckSignOfAccess(_credentials);
                if (HasAccess)
                {
                    IsCheckSheetAccessible = true;
                    IsVisibleRejectButton = true;
                    _userDialogs.Alert("The ITR Sheet is now Unlocked", "", "OK");
                }
                else
                {
                    IsCheckSheetAccessible = false;
                    _userDialogs.Alert("You Dont have access rights to unlock the sheet", "", "OK");
                }
            }
        }

        public async void GetSignOffList()
        {
            //get SignOffList
            var signofflist = await _CommonHeaderFooterSignOffRepository.GetAsync(x => x.SignOffTag == SelectedCheckSheet.Tag && x.SignOffChecksheet == SelectedCheckSheet.ITRNumber &&
            x.ITRCommonID == CommonHeaderFooter.ID && x.CommonRowID == CommonHeaderFooter.ROWID && x.ModelName == Settings.ModelName);
            var Signoff = signofflist.GroupBy(g => new { g.Title })
                    .Select(g => g.First())
                    .ToList();
            Signoff.Where(w => string.IsNullOrEmpty(w.FullName.Trim())).ToList().ForEach(s => s.FullName = "Press to Sign");
            SignOffList = new ObservableCollection<T_CommonHeaderFooterSignOff>(Signoff);
            // UpdateSignoffSection();
        }
        public string ConvertStringToCamelCase(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
        public string GetInitialValue()
        {
            var users = Task.Run(async () => await _T_UserControlRepository.GetAsync(i => Convert.ToInt32(i.ID) == Settings.CompletionUserID)).Result;
            string InitialUserName = string.Empty;
            if (users != null)
                InitialUserName = String.Join("", users.FirstOrDefault().FullName.Split(' ').Select(i => i[0]).ToList());

            return InitialUserName;
        }
        public async void UpdateAFINo(string afino)
        {
            CommonHeaderFooter.AFINo = !String.IsNullOrEmpty(afino) ? afino : "";
            //CommonHeaderFooter.Client = !String.IsNullOrEmpty(CommonHeaderFooter.Client) ? CommonHeaderFooter.Client : "";
            //CommonHeaderFooter.SignSubcontractor = !String.IsNullOrEmpty(CommonHeaderFooter.SignSubcontractor) ? CommonHeaderFooter.SignSubcontractor : "";           
            //CommonHeaderFooter.SignContractor = !String.IsNullOrEmpty(CommonHeaderFooter.SignContractor) ? CommonHeaderFooter.SignContractor : "";          
            //CommonHeaderFooter.Client = !String.IsNullOrEmpty(CommonHeaderFooter.Client) ? CommonHeaderFooter.Client : "";
            //CommonHeaderFooter.SignSubcontractorDate = CommonHeaderFooter.SignSubcontractorDate <= new DateTime(2000, 1, 1) ? DateTime.Now : CommonHeaderFooter.SignSubcontractorDate;
            //CommonHeaderFooter.SignContractorDate = CommonHeaderFooter.SignContractorDate <= new DateTime(2000, 1, 1) ? DateTime.Now : CommonHeaderFooter.SignContractorDate;
            //CommonHeaderFooter.ClientDate = CommonHeaderFooter.ClientDate <= new DateTime(2000, 1, 1) ? new DateTime(2000, 1, 1) : CommonHeaderFooter.ClientDate;
            //CommonHeaderFooter.CreatedDate = CommonHeaderFooter.CreatedDate <= new DateTime(2000, 1, 1) ? new DateTime(2000, 1, 1) : CommonHeaderFooter.CreatedDate;
            //CommonHeaderFooter.CreatedBy = !String.IsNullOrEmpty(CommonHeaderFooter.CreatedBy) ? CommonHeaderFooter.CreatedBy : "";            
            await _CommonHeaderFooterRepository.UpdateAsync(CommonHeaderFooter);
        }

        public ITRViewModel(INavigationService _navigationService,
           IUserDialogs _userDialogs,
           IHttpHelper _httpHelper,
           ICheckValidLogin _checkValidLogin,
           ITRService _ITRService,
           IRepository<T_TAG> _TAGRepository,
           IRepository<T_CHECKSHEET> _CheckSheetRepository,
           IRepository<T_CHECKSHEET_PER_TAG> _CheckSheetPerTagRepository,
           IRepository<T_CommonHeaderFooterSignOff> _CommonHeaderFooterSignOffRepository,
           IRepository<T_TAG_SHEET_HEADER> _TagSheetHeaderRepository,
           IRepository<T_CHECKSHEET_QUESTION> _CheckSheetQuetionsRepository,
           IRepository<T_TAG_SHEET_ANSWER> _TAG_SHEET_ANSWERRepository,
           IRepository<T_UserControl> _T_UserControlRepository,
           IRepository<T_ITRCommonHeaderFooter> _CommonHeaderFooterRepository,
           IRepository<T_ITRRecords_30A_31A> _RecordsRepository,
           IRepository<T_ITRTubeColors> _TubeColorsRepository,
           IRepository<T_ITRRecords_040A_041A_042A> _Records_04XARepository,
           IRepository<T_ITRInsulationDetails> _InsulationDetailsRepository,
           IRepository<T_ITR8000_003A_AcceptanceCriteria> _Records_8000003A_AcceptanceCriteriaRepository,
           IRepository<T_ITR8000_003ARecords> _Records_8000003ARepository,
           IRepository<T_ITRRecords_080A_090A_091A> _Records_080A_09XARepository,
           IRepository<T_ITR8100_001A_CTdata> _ITR8100_001A_CTdataRepository,
           IRepository<T_ITR8100_001A_InsulationResistanceTest> _ITR8100_001A_IRTestRepository,
           IRepository<T_ITR8100_001A_RatioTest> _ITR8100_001A_RatioTestRepository,
           IRepository<T_ITR8100_001A_TestInstrumentData> _ITR8100_001A_TIDataRepository,
           IRepository<T_ITRRecords_8100_002A> _ITRRecords_8100_002ARepository,
           IRepository<T_ITRRecords_8100_002A_InsRes_Test> _ITRRecords_8100_002A_InsRes_TestRepository,
           IRepository<T_ITRRecords_8100_002A_Radio_Test> _ITRRecords_8100_002A_Radio_TestRepository,
           IRepository<T_ITR8140_001A_ContactResisTest> _T_ITR8140_001A_ContactResisTestRepository,
           IRepository<T_ITR8140_001AInsulationResistanceTest> _T_ITR8140_001AInsulationResistanceTestRepository,
           IRepository<T_ITR8140_001ADialectricTest> _T_ITR8140_001ADialectricTestRepository,
           IRepository<T_ITR8140_001ATestInstrucitonData> _T_ITR8140_001ATestInstrumentDataRepository,
           IRepository<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents> _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents,
           IRepository<T_ITR8121_002A_Records> _ITR8121_002A_Records,
           IRepository<T_ITR8121_002A_TransformerRadioTest> _ITR8121_002A_TransformerRadioTest,
           IRepository<T_ITR_8260_002A_Body> _ITR_8260_002A_BodyRepository,
           IRepository<T_ITR_8260_002A_DielectricTest> _ITR_8260_002A_DielectricTestRepository,
           IRepository<T_ITRRecords_8161_001A_Body> _ITRRecords_8161_001A_BodyRepository,
           IRepository<T_ITRRecords_8161_001A_InsRes> _ITRRecords_8161_001A_InsResRepository,
           IRepository<T_ITRRecords_8161_001A_ConRes> _ITRRecords_8161_001A_ConResRepository,
           IRepository<T_ITR8121_004AInCAndAuxiliary> _ITR8121_004AInCAndAuxiliaryRepository,
           IRepository<T_ITR8121_004ATransformerRatioTest> _ITR8121_004ATransformerRatioTestRepository,
           IRepository<T_ITR8121_004ATestInstrumentData> _ITR8121_004ATestInstrumentDataRepository,
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
           IRepository<T_Ccms_signature> _Ccms_signatureRepository,
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
            IRepository<T_CompletionsUsers> _CompletionsUserRepository)
           : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._ITRService = _ITRService;
            this._TAGRepository = _TAGRepository;
            this._CheckSheetRepository = _CheckSheetRepository;
            this._CheckSheetQuetionsRepository = _CheckSheetQuetionsRepository;
            this._CheckSheetPerTagRepository = _CheckSheetPerTagRepository;
            this._CommonHeaderFooterSignOffRepository = _CommonHeaderFooterSignOffRepository;
            this._TagSheetHeaderRepository = _TagSheetHeaderRepository;
            this._TAG_SHEET_ANSWERRepository = _TAG_SHEET_ANSWERRepository;
            this._T_UserControlRepository = _T_UserControlRepository;
            this._CommonHeaderFooterRepository = _CommonHeaderFooterRepository;
            this._RecordsRepository = _RecordsRepository;
            this._TubeColorsRepository = _TubeColorsRepository;
            this._Records_04XARepository = _Records_04XARepository;
            this._Records_8000003ARepository = _Records_8000003ARepository;
            this._InsulationDetailsRepository = _InsulationDetailsRepository;
            this._Records_8000003A_AcceptanceCriteriaRepository = _Records_8000003A_AcceptanceCriteriaRepository;
            this._Records_080A_09XARepository = _Records_080A_09XARepository;
            this._ITR8100_001A_CTdataRepository = _ITR8100_001A_CTdataRepository;
            this._ITR8100_001A_IRTestRepository = _ITR8100_001A_IRTestRepository;
            this._ITR8100_001A_RatioTestRepository = _ITR8100_001A_RatioTestRepository;
            this._ITR8100_001A_TIDataRepository = _ITR8100_001A_TIDataRepository;
            this._ITRRecords_8100_002ARepository = _ITRRecords_8100_002ARepository;
            this._ITRRecords_8100_002A_InsRes_TestRepository = _ITRRecords_8100_002A_InsRes_TestRepository;
            this._ITRRecords_8100_002A_Radio_TestRepository = _ITRRecords_8100_002A_Radio_TestRepository;
            this._T_ITR8140_001AInsulationResistanceTestRepository = _T_ITR8140_001AInsulationResistanceTestRepository;
            this._T_ITR8140_001ADialectricTestRepository = _T_ITR8140_001ADialectricTestRepository;
            this._T_ITR8140_001A_ContactResisTestRepository = _T_ITR8140_001A_ContactResisTestRepository;
            this._T_ITR8140_001ATestInstrumentDataRepository = _T_ITR8140_001ATestInstrumentDataRepository;
            this._ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents = _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents;
            this._ITR8121_002A_Records = _ITR8121_002A_Records;
            this._ITR8121_002A_TransformerRadioTest = _ITR8121_002A_TransformerRadioTest;
            this._ITR_8260_002A_BodyRepository = _ITR_8260_002A_BodyRepository;
            this._ITR_8260_002A_DielectricTestRepository = _ITR_8260_002A_DielectricTestRepository;

            this._ITRRecords_8161_001A_BodyRepository = _ITRRecords_8161_001A_BodyRepository;
            this._ITRRecords_8161_001A_InsResRepository = _ITRRecords_8161_001A_InsResRepository;
            this._ITRRecords_8161_001A_ConResRepository = _ITRRecords_8161_001A_ConResRepository;
            this._ITR8121_004AInCAndAuxiliaryRepository = _ITR8121_004AInCAndAuxiliaryRepository;
            this._ITR8121_004ATransformerRatioTestRepository = _ITR8121_004ATransformerRatioTestRepository;
            this._ITR8121_004ATestInstrumentDataRepository = _ITR8121_004ATestInstrumentDataRepository;
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
            this._Ccms_signatureRepository = _Ccms_signatureRepository;
            this._ITRRecords_8170_002A_Voltage_ReadingRepository = _ITRRecords_8170_002A_Voltage_ReadingRepository;
            this._ITR_8170_002A_IndifictionLampRepository = _ITR_8170_002A_IndifictionLampRepository;
            this._ITR_8170_002A_InsResRepository = _ITR_8170_002A_InsResRepository;
            this._ITR_8300_003A_BodyRepository = _ITR_8300_003A_BodyRepository;
            this._ITR_8300_003A_IlluminRepository = _ITR_8300_003A_IlluminRepository;
            this._ITR_8170_007A_OP_Function_TestRepository = _ITR_8170_007A_OP_Function_TestRepository;
            this._CompletionsUserRepository = _CompletionsUserRepository;
            BindTagsAsync();
            //  CheckSheetCompleted = true;

            RowWidth = 80;
            IndexFrame = true;
            QuestionFrame = ITR_040AFrame = ITR_080AFrame = IsVisibleRejectButton = ITR8140_001AFrame = false;
            RemarkEntry = "";
            ListTestResultAccepted = new List<string> { "Yes", "NO" };
            ListTestVoltage = new List<string> { "500", "100" };
            ListTestResultAccept = new List<string> { "Yes", "NO" };
            WiringCheckList = new List<string> { "--Select--", "Pass", "Fail" };
            FunctionCheckList = new List<string> { "", "Pass", "Fail", "NA" };
            DDLCheckList = new List<string> { "", "Pass", "Fail" };
            ITR8000101A_ResultAccepted = new List<string> { "", "Y", "N", "N/A" };
            PunchList = new List<string> { "", "Y", "N" };
        }
        public void DeleteSelectedItem(int selecteditem)
        {
            var IRTTest = ITR8100_001A_InsulationResistanceTest.Where(x => x.ID == selecteditem).First();
            var CTData = ITR8100_001A_CTdata.Where(x => x.ID == selecteditem).First();
            var RTTest = ITR8100_001A_RatioTest.Where(x => x.ID == selecteditem).First();
            ITR8100_001A_InsulationResistanceTest.Remove(IRTTest);
            ITR8100_001A_CTdata.Remove(CTData);
            ITR8100_001A_RatioTest.Remove(RTTest);
            _ITR8100_001A_IRTestRepository.DeleteAsync(IRTTest);
            _ITR8100_001A_CTdataRepository.DeleteAsync(CTData);
            _ITR8100_001A_RatioTestRepository.DeleteAsync(RTTest);

        }

        public async void BindTagsAsync()
        {
            //Color codding Logic
            _tagsList = new List<T_TAG>();
            ItemSourceTagList = new ObservableCollection<dynamic>();
            var data = await _TAGRepository.GetAsync(i => i.ProjectName == Settings.ProjectName);
            var signOffHeaderData = await _CommonHeaderFooterSignOffRepository.GetAsync();
            var CHFList = await _CommonHeaderFooterRepository.GetAsync();
            var DistictTags = data.GroupBy(x => x.name, (key, group) => group.First());
            if (DistictTags != null && DistictTags.Any())
            {

                DistictTags.ToList().ForEach(x =>
                {
                    Rejected = false;
                    Completed = false;
                    Started = false;

                    List<T_ITRCommonHeaderFooter> CHFItemList = CHFList.ToList().Where(y => y.Tag == x.name && x.ProjectName == Settings.ProjectName).ToList();
                    if (CHFItemList.Any())
                    {
                        if (CHFItemList.Where(y => y.Tag == x.name && y.Rejected).Any())
                            Rejected = true;
                        else if (CHFItemList.All(y => y.Tag == x.name && y.Completed))
                            Completed = true;
                        else if (CHFItemList.Where(y => y.Tag == x.name && y.Started).Any())
                            Started = true;

                        x.StatusColor = (Rejected ? Outstanding : Completed ? ITR_STATUS_COMPLETE : (Started ? ITR_STATUS_STARTED : statusNoJIData));
                        _tagsList.Add(x);
                    }
                    else
                    {
                        x.StatusColor = statusNoJIData;
                        _tagsList.Add(x);
                    }


                    //Completed = true;
                    //Started = false;
                    //T_ITRCommonHeaderFooter CHF = CHFList.ToList().Where(y => y.Tag == x.name && y.Rejected == true).FirstOrDefault();
                    //T_ITRCommonHeaderFooter CHFNotStarted = CHFList.ToList().Where(y => y.Tag == x.name && !String.IsNullOrEmpty(y.CreatedBy)).FirstOrDefault();
                    //if (CHF != null)
                    //    rejected = CHF.Rejected;
                    //else
                    //    rejected = false;
                    //if (!(x.refType == "PIPING JOINT"))
                    //{
                    //var signOffHeader = signOffHeaderData.Where(y => y.SignOffTag == x.name);

                    //if (signOffHeader.Any())
                    //{

                    //    foreach (T_CommonHeaderFooterSignOff signOff in signOffHeader)
                    //    {
                    //        if (signOff.Title?.ToLower() == "client" && signOff.FullName?.ToLower() == "na")
                    //            continue;
                    //        else
                    //            Completed &= !string.IsNullOrEmpty(signOff.FullName?.Trim());
                    //        Started |= !string.IsNullOrWhiteSpace(signOff.FullName) && signOff.FullName != "";

                    //    }
                    //    //if (!Started)
                    //    //    Started |= GetQuestionStatus(x.name, "");
                    //    if (!Started)
                    //        Started = CHFNotStarted != null ? true : false;
                    //}
                    //else
                    //{
                    //    Completed = false;
                    //}
                    //x.StatusColor = (Completed ? ITR_STATUS_COMPLETE : (Started ? ITR_STATUS_STARTED : statusNoJIData));
                    //if (rejected)
                    //    x.StatusColor = Outstanding;
                    //_tagsList.Add(x);
                    //}
                    //else
                    //{
                    // x.StatusColor = statusNoJIData;
                    // _tagsList.Add(x);
                    //joint data not found yet 
                    //}
                    ItemSourceTagList = new ObservableCollection<dynamic>(_tagsList);
                });
            }

        }
        public async void OnTagSelectionChage()
        {
            var signOffHeaderData = await _CommonHeaderFooterSignOffRepository.GetAsync();
            if (SelectedTag == null) return;

            //var result = await _CheckSheetPerTagRepository.GetAsync(x => x.TAGNAME == SelectedTag.name
            var result = await _CommonHeaderFooterRepository.GetAsync(x => x.Tag == SelectedTag.name && x.Project == Settings.ProjectName);
            result = result.OrderBy(x => x.ITRNumber).ToList();
            //Regex reges = new Regex("^[0-9]+$");
            //result = result.Where(x => reges.IsMatch(x.CHECKSHEETNAME.Substring(x.CHECKSHEETNAME.Length - 1)) == false).ToList();
            if (result != null && result.Any())
            {
                // var _CurrentCheckSheet = await _CheckSheetRepository.GetAsync();

                // ItemSourceCheckSheets = new ObservableCollection<T_CHECKSHEET_PER_TAG>(result.GroupBy(x => x.CHECKSHEETNAME).Select(g => g.First()));
                ItemSourceCheckSheets = new ObservableCollection<T_ITRCommonHeaderFooter>(result.GroupBy(x => x.ITRNumber).Select(g => g.First()));
                ItemSourceCheckSheets.ForEach(x =>
                {
                    Rejected = false;
                    Completed = false;
                    Started = false;
                    if (x.Rejected)
                        Rejected = true;
                    else if (x.Completed)
                        Completed = true;
                    else if (x.Started)
                        Started = true;

                    x.StatusColor = (Rejected ? Outstanding : Completed ? ITR_STATUS_COMPLETE : (Started ? ITR_STATUS_STARTED : statusNoJIData));

                    //var signOffHeader = signOffHeaderData.Where(y => y.SignOffTag == x.TAGNAME && y.SignOffChecksheet == x.CHECKSHEETNAME);
                    //var signOffHeader = signOffHeaderData.Where(y => y.SignOffTag == x.Tag && y.SignOffChecksheet == x.ITRNumber);
                    //if (signOffHeader.Any() && SelectedTag.refType != "PIPING JOINT")
                    //{
                    //    Completed = true;
                    //    Started = false;
                    //    Rejected = x.Rejected;
                    //    foreach (T_CommonHeaderFooterSignOff signOff in signOffHeader)
                    //    {

                    //        //Completed &= !string.IsNullOrWhiteSpace(signOff.FullName);
                    //        //Started |= !string.IsNullOrWhiteSpace(signOff.FullName);
                    //        //rejected |= signOff.Rejected;

                    //        if (signOff.Title?.ToLower() == "client" && signOff.FullName?.ToLower() == "na")
                    //            continue;
                    //        else
                    //            Completed &= !string.IsNullOrEmpty(signOff.FullName?.Trim());
                    //        Started |= !string.IsNullOrWhiteSpace(signOff.FullName) && signOff.FullName != "";
                    //        //if (x.Rejected) // signOff.Rejected ||
                    //        //    rejected = true;
                    //    }
                    //    //// x.StatusColor = "#ff2a2a";
                    //    //if (!Started)
                    //    //    Started |= GetQuestionStatus(x.TAGNAME, x.CHECKSHEETNAME);
                    //    if (!Started)
                    //        Started = !string.IsNullOrWhiteSpace(x.CreatedBy) ? true : false;

                    //    x.StatusColor = (Completed ? ITR_STATUS_COMPLETE : (Started ? ITR_STATUS_STARTED : statusNoJIData));

                    //    if (Rejected)
                    //        x.StatusColor = Outstanding;
                    //}
                    //else
                    //{
                    //    Completed = false;
                    //    x.StatusColor = statusNoJIData;
                    //}

                    x.ITRDescription = x.ITRNumber + " " + x.ITRDescription;//_CurrentCheckSheet.Where(y => y.name == x.ITRNumber).Select(z => z.description).FirstOrDefault();
                });
            }
        }

        public bool GetQuestionStatus(string tagName, string sheetName)
        {
            string query;

            query = "Select * FROM T_TAG_SHEET_ANSWER WHERE tagName = '" + tagName + "'"
                  + " AND checkValue != ''";

            if (!String.IsNullOrEmpty(sheetName))
                query += " AND checkSheetName = '" + sheetName + "'";

            var result = Task.Run(async () => await _TAG_SHEET_ANSWERRepository.QueryAsync(query)).Result;
            return result.Count() > 0 ? true : false;

        }


        public async void OnClickButtonAsync(string param)
        {
            if (!App.IsBusy)
            {
                App.IsBusy = true;
                if (param == "ViewPunchList")
                {
                    await navigationService.NavigateAsync<CompletionPunchListViewModel>();
                }
                else if (param == "AddPunchItem")
                {
                    var parameter = new NavigationParameters();
                    T_CompletionsPunchList SelectedPunchList = new T_CompletionsPunchList
                    {
                        location = SelectedTag.location == null ? "" : SelectedTag.location,
                        systemno = SelectedTag.system == null ? "" : SelectedTag.system,
                        tagno = SelectedTag.name == null ? "" : SelectedTag.name,
                        description = SelectedTag.description == null ? "" : SelectedTag.description,
                        project = SelectedTag.ProjectName == null ? "" : SelectedTag.ProjectName,
                        itrname = SelectedTag.SheetName == null ? "" : SelectedTag.SheetName,
                        subsystem = SelectedTag.subSystem == null ? "" : SelectedTag.subSystem,
                        workpack = SelectedTag.workpack == null ? "" : SelectedTag.workpack,
                        jobpack = SelectedTag.jobPack == null ? "" : SelectedTag.jobPack,
                        PCWBS = SelectedTag.pcwbs == null ? "" : SelectedTag.pcwbs,
                        FWBS = SelectedTag.fwbs == null ? "" : SelectedTag.fwbs,
                    };
                    parameter.Add("SelectedPunchListForCreate", SelectedPunchList);
                    if (Settings.CurrentDB == "JGC" || Settings.CurrentDB == "JGC_HARMONY" || Settings.CurrentDB == "JGC_ITR" || Settings.CurrentDB == "JGC_DEMO" ||
                        Settings.CurrentDB == "ROVUMA_TEST" || Settings.CurrentDB == "YOC_DEMO" || Settings.CurrentDB == "JGC_HARMONYCOMP")
                        await navigationService.NavigateAsync<NewPunchViewModel>(parameter);
                    else
                        await navigationService.NavigateAsync<CreateNewPunchViewModel>();
                }
                else if (param == "AddNewItem")
                {
                    try
                    {
                        int rowNo = ITR8100_001A_CTdata.Count() + 1;
                        int NewCTID = ITR8100_001A_CTdata.OrderBy(x => x.ID).Select(x => x.ID).LastOrDefault() + 1;

                        var RTdata = await _ITR8100_001A_RatioTestRepository.GetAsync();
                        long RTID = RTdata.Count() + 1;
                        T_ITR8100_001A_CTdata CTdata = ITR8100_001A_CTdata.FirstOrDefault();
                        T_ITR8100_001A_CTdata NewCTRecord = new T_ITR8100_001A_CTdata()
                        {
                            RowNo = rowNo,
                            IsUpdated = true,
                            ID = NewCTID,
                            CommonHFID = CommonHeaderFooter.ID,
                            CCMS_HEADERID = (int)CommonHeaderFooter.ID,
                            CommonRowID = CommonHeaderFooter.ROWID,
                            RowID = RTID,
                            ITRNumber = CommonHeaderFooter.ITRNumber,
                            TagNO = CommonHeaderFooter.Tag,
                            ModelName = Settings.ModelName,
                            ModelNoTagNo = CTdata.ModelNoTagNo,
                            SerialNo = CTdata.SerialNo,
                            Ratio = CTdata.Ratio,
                            ClassVA = CTdata.ClassVA,
                            ShortCircuitCurrent = CTdata.ShortCircuitCurrent,
                            PrimaryCurrent = CTdata.PrimaryCurrent,
                            SecondaryCurrent = CTdata.SecondaryCurrent
                        };

                        int NewRTID = ITR8100_001A_RatioTest.OrderBy(x => x.ID).Select(x => x.ID).LastOrDefault() + 1;
                        T_ITR8100_001A_RatioTest NewRTRecord = new T_ITR8100_001A_RatioTest()
                        {
                            RowNo = rowNo,
                            IsUpdated = true,
                            ID = NewRTID,
                            CommonHFID = CommonHeaderFooter.ID,
                            CCMS_HEADERID = (int)CommonHeaderFooter.ID,
                            CommonRowID = CommonHeaderFooter.ROWID,
                            RowID = RTID,
                            ITRNumber = CommonHeaderFooter.ITRNumber,
                            TagNO = CommonHeaderFooter.Tag,
                            ModelName = Settings.ModelName
                        };

                        int NewIRTID = ITR8100_001A_InsulationResistanceTest.OrderBy(x => x.ID).Select(x => x.ID).LastOrDefault() + 1;
                        T_ITR8100_001A_InsulationResistanceTest NewIRTRecord = new T_ITR8100_001A_InsulationResistanceTest()
                        {
                            RowNo = rowNo,
                            IsUpdated = true,
                            ID = NewIRTID,
                            CommonRowID = CommonHeaderFooter.ROWID,
                            RowID = RTID,
                            CommonHFID = CommonHeaderFooter.ID,
                            CCMS_HEADERID = (int)CommonHeaderFooter.ID,
                            ModelName = Settings.ModelName,
                        };

                        ITR8100_001A_CTdata.Add(NewCTRecord);
                        ITR8100_001A_RatioTest.Add(NewRTRecord);
                        ITR8100_001A_InsulationResistanceTest.Add(NewIRTRecord);
                        await _ITR8100_001A_CTdataRepository.InsertOrReplaceAsync(NewCTRecord);
                        await _ITR8100_001A_RatioTestRepository.InsertOrReplaceAsync(NewRTRecord);
                        await _ITR8100_001A_IRTestRepository.InsertOrReplaceAsync(NewIRTRecord);
                        DependencyService.Get<IToastMessage>().ShortAlert("Record Added Successfully");
                    }
                    catch (Exception ex)
                    {
                        await _userDialogs.AlertAsync("error", "", "OK");
                    }
                    finally
                    {
                        App.IsBusy = false;
                    }

                }
                else if (param == "AddNewInspectionAuxiliaryCCItem")
                {
                    try
                    {
                        var InsConAux = await _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents.GetAsync();
                        long InsConAuxID = InsConAux.Count() + 1;
                        T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents NewICRecord = new T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents()
                        {
                            id = InspectionforControls.Count + 1,
                            No = InspectionforControls.Count + 1,
                            ID_ITR_8121_002A_InspectionforControl = ITR8121_002A_Record_ID,
                            ModelName = Settings.ModelName,
                            CommonRowID = CommonHeaderFooter.ROWID,
                            CCMS_HEADERID = (int)CommonHeaderFooter.ID,
                            RowID = InsConAuxID,
                        };
                        InspectionforControls.Add(NewICRecord);
                        InspectionforControls.Skip(7).ForEach(x => x.IsUpdated = true);
                        InspectionforControls = new ObservableCollection<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents>(InspectionforControls.ToList());
                        await _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents.InsertOrReplaceAsync(NewICRecord);
                        DependencyService.Get<IToastMessage>().ShortAlert("Record Added Successfully");
                    }
                    catch (Exception ex)
                    {
                        await _userDialogs.AlertAsync("error", "", "OK");
                    }
                    finally
                    {
                        App.IsBusy = false;
                    }
                }
                else if (param == "AddNewTransformerRatioTestItem")
                {
                    try
                    {
                        var TransRTest = await _ITR8121_002A_TransformerRadioTest.GetAsync();
                        long TRTID = TransRTest.Count() + 1;
                        T_ITR8121_002A_TransformerRadioTest TransformerRadioTest = new T_ITR8121_002A_TransformerRadioTest()
                        {
                            ID = TransformerRadioTestList.Count + 1,
                            // TapNo = Convert.ToString(TransformerRadioTestList.Count + 1),
                            ID_8121_002A_TransformerRadioTest = ITR8121_002A_Record_ID,
                            CommonRowID = CommonHeaderFooter.ROWID,
                            RowID = TRTID,
                            ModelName = Settings.ModelName,
                            CCMS_HEADERID = (int)CommonHeaderFooter.ID
                        };
                        await _ITR8121_002A_TransformerRadioTest.InsertOrReplaceAsync(TransformerRadioTest);
                        TransformerRadioTestList.Add(TransformerRadioTest);
                        TransformerRadioTestList.Skip(1).ForEach(x => x.IsUpdated = true);
                        TransformerRadioTestList = new ObservableCollection<T_ITR8121_002A_TransformerRadioTest>(TransformerRadioTestList);
                        DependencyService.Get<IToastMessage>().ShortAlert("Record Added Successfully");
                    }
                    catch (Exception ex)
                    {
                        await _userDialogs.AlertAsync("error", "", "OK");
                    }
                    finally
                    {
                        App.IsBusy = false;
                    }
                }
                else if (param == "DeleteTransformerRadioTestItem")
                {
                    if (SelectedTransformerRadioTest != null)
                    {
                        if (await _userDialogs.ConfirmAsync("Are you sure you want to delete?", "Delete Record", "Yes", "No"))
                        {
                            await _ITR8121_002A_TransformerRadioTest.DeleteAsync(SelectedTransformerRadioTest);
                            TransformerRadioTestList.Remove(SelectedTransformerRadioTest);
                        }
                    }
                    App.IsBusy = false;
                }
                else if (param == "DeleteInspectionforControlsItem")
                {
                    if (SelectedInspectionforControls != null)
                    {
                        if (await _userDialogs.ConfirmAsync("Are you sure you want to delete?", "Delete Record", "Yes", "No"))
                        {
                            await _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents.DeleteAsync(SelectedInspectionforControls);
                            InspectionforControls.Remove(SelectedInspectionforControls);
                        }
                        int i = 1;
                        InspectionforControls.ForEach(x => x.No = i++);
                        InspectionforControls.Skip(7).ForEach(x => x.IsUpdated = true);
                        InspectionforControls = new ObservableCollection<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents>(InspectionforControls);
                    }
                    App.IsBusy = false;
                }
                else if (param == "DeleteITRItem")
                {
                    if (SelectedRedioTest != null)
                    {
                        if (await _userDialogs.ConfirmAsync("Are you sure you want to delete?", "Delete Record", "Yes", "No"))
                        {
                            T_ITR8100_001A_CTdata CTdata = ITR8100_001A_CTdata.Where(x => x.RowID == SelectedRedioTest.RowID).FirstOrDefault();
                            T_ITR8100_001A_InsulationResistanceTest IRTData = iTR8100_001A_InsulationResistanceTest.Where(x => x.RowID == SelectedRedioTest.RowID).FirstOrDefault();

                            string DelCTdata = "DELETE FROM [T_ITR8100_001A_CTdata] WHERE RowID = '" + CTdata.RowID + "' AND CommonRowID = '" + CTdata.CommonRowID + "' AND CommonHFID = '" + CTdata.CommonHFID + "' AND ITRNumber ='" + CTdata.ITRNumber + "' AND TagNO ='" + CTdata.TagNO + "' AND CCMS_HEADERID =" + CTdata.CCMS_HEADERID;
                            await _ITR8100_001A_CTdataRepository.QueryAsync(DelCTdata);
                            string DelRatioTest = "DELETE FROM [T_ITR8100_001A_RatioTest] WHERE RowID = '" + SelectedRedioTest.RowID + "' AND CommonRowID = '" + SelectedRedioTest.CommonRowID + "' AND CommonHFID = '" + SelectedRedioTest.CommonHFID + "' AND ITRNumber ='" + SelectedCheckSheet.ITRNumber + "' AND TagNO ='" + SelectedTag.name + "' AND CCMS_HEADERID =" + SelectedRedioTest.CCMS_HEADERID;
                            await _ITR8100_001A_RatioTestRepository.QueryAsync(DelRatioTest);
                            string DelInResistanceTest = "DELETE FROM [T_ITR8100_001A_InsulationResistanceTest] WHERE CommonHFID = '" + IRTData.CommonHFID + "' AND RowID = '" + IRTData.RowID + "' AND CommonRowID = '" + IRTData.CommonRowID + "' AND CCMS_HEADERID =" + IRTData.CCMS_HEADERID;
                            await _ITR8100_001A_IRTestRepository.QueryAsync(DelInResistanceTest);

                            //await _ITR8100_001A_CTdataRepository.DeleteAsync(CTdata);
                            //await _ITR8100_001A_RatioTestRepository.DeleteAsync(SelectedRedioTest);
                            //await _ITR8100_001A_IRTestRepository.DeleteAsync(IRTData);

                            ITR8100_001A_CTdata.Remove(CTdata);
                            ITR8100_001A_RatioTest.Remove(SelectedRedioTest);
                            ITR8100_001A_InsulationResistanceTest.Remove(IRTData);

                            SelectedRedioTest = null;
                        }
                    }
                    App.IsBusy = false;
                }
                else if (param == "AddNewItem8161001A")
                {
                    try
                    {
                        int SlNum = ITR_8161_001A_ConRes.Select(x => x.SlNo).OrderBy(x => x).LastOrDefault() + 1;
                        int NewID = ITR_8161_001A_ConRes.Select(x => x.ID).OrderBy(x => x).LastOrDefault() + 1;
                        var recordsConRes = await _ITRRecords_8161_001A_ConResRepository.GetAsync();
                        long ConResID = recordsConRes.Count() + 1;

                        T_ITRRecords_8161_001A_ConRes NewRecord = new T_ITRRecords_8161_001A_ConRes()
                        {
                            SlNo = SlNum,
                            IsUpdated = true,
                            ID = NewID,
                            ITRCommonID = CommonHeaderFooter.ID,
                            RowID = ConResID,
                            CommonRowID = CommonHeaderFooter.ROWID,
                            ModelName = Settings.ModelName,
                            CCMS_HEADERID = (int)CommonHeaderFooter.ID
                        };

                        await _ITRRecords_8161_001A_ConResRepository.InsertOrReplaceAsync(NewRecord);
                        ITR_8161_001A_ConRes.Add(NewRecord);
                        DependencyService.Get<IToastMessage>().ShortAlert("Record Added Successfully");
                    }
                    catch (Exception ex)
                    {
                        await _userDialogs.AlertAsync("error", "", "OK");
                    }
                    finally
                    {
                        App.IsBusy = false;
                    }

                }
                else if (param == "DeleteITRItem8161_001A")
                {
                    if (Item_8161_001A_ConRes != null)
                    {
                        if (await _userDialogs.ConfirmAsync("Are you sure you want to delete?", "Delete Record", "Yes", "No"))
                        {
                            string DelConRes = "DELETE FROM [T_ITRRecords_8161_001A_ConRes] WHERE ID = '" + Item_8161_001A_ConRes.ID + "' AND ITRCommonID = '" + Item_8161_001A_ConRes.ITRCommonID + "' AND CommonRowID = '" + Item_8161_001A_ConRes.CommonRowID + "' AND RowID = '" + Item_8161_001A_ConRes.RowID + "' AND CCMS_HEADERID =" + Item_8161_001A_ConRes.CCMS_HEADERID;
                            await _ITRRecords_8161_001A_ConResRepository.QueryAsync(DelConRes);

                            int curr_sl = Item_8161_001A_ConRes.SlNo;

                            ITR_8161_001A_ConRes.Remove(Item_8161_001A_ConRes);
                            int i = 1;
                            ITR_8161_001A_ConRes.ForEach(x => x.SlNo = i++);
                            ITR_8161_001A_ConRes.Skip(1).ForEach(x => x.IsUpdated = true);

                            Item_8161_001A_ConRes = null;
                        }
                    }
                    App.IsBusy = false;
                }
                else if (param == "AddInsConAuxiliarytem")
                {
                    int newSrNo = InispactionForControlAndAuxiliary8121.OrderByDescending(u => u.SrNo).Select(x => x.SrNo).FirstOrDefault();
                    var InCAndAux = await _ITR8121_004AInCAndAuxiliaryRepository.GetAsync();
                    int newID = InCAndAux.OrderByDescending(u => u.ID).Select(x => x.ID).FirstOrDefault();
                    var ControlAndAuxiliary = await _ITR8121_004AInCAndAuxiliaryRepository.GetAsync();
                    long ConAuxID = ControlAndAuxiliary.Count() + 1;
                    T_ITR8121_004AInCAndAuxiliary AddItem = new T_ITR8121_004AInCAndAuxiliary
                    {
                        ID = newID + 1,
                        SrNo = newSrNo + 1,
                        RowID = ConAuxID,
                        ModelName = Settings.ModelName,
                        ITRCommonID = CommonHeaderFooter.ID,
                        CommonRowID = CommonHeaderFooter.ROWID,
                        CCMS_HEADERID = (int)CommonHeaderFooter.ID
                    };
                    await _ITR8121_004AInCAndAuxiliaryRepository.InsertOrReplaceAsync(AddItem);
                    InispactionForControlAndAuxiliary8121.Add(AddItem);
                    if (InispactionForControlAndAuxiliary8121.Count > 3)
                        InispactionForControlAndAuxiliary8121.Skip(3).ForEach(x => x.IsUpdated = true);
                    InispactionForControlAndAuxiliary8121 = new ObservableCollection<T_ITR8121_004AInCAndAuxiliary>(InispactionForControlAndAuxiliary8121);
                    DependencyService.Get<IToastMessage>().ShortAlert("Record Added Successfully");
                    App.IsBusy = false;
                }
                else if (param == "AddTransRatioTestItem")
                {
                    var TransRadioTest = await _ITR8121_004ATransformerRatioTestRepository.GetAsync();
                    int newID = TransRadioTest.OrderByDescending(u => u.ID).Select(x => x.ID).FirstOrDefault();
                    var TransRatioTest = await _ITR8121_004ATransformerRatioTestRepository.GetAsync();
                    long TransRatioTestID = TransRatioTest.Count() + 1;
                    T_ITR8121_004ATransformerRatioTest AddItem = new T_ITR8121_004ATransformerRatioTest
                    {
                        ID = newID + 1,
                        RowID = TransRatioTestID,
                        ModelName = Settings.ModelName,
                        ITRCommonID = CommonHeaderFooter.ID,
                        CommonRowID = CommonHeaderFooter.ROWID,
                        CCMS_HEADERID = (int)CommonHeaderFooter.ID
                    };
                    await _ITR8121_004ATransformerRatioTestRepository.InsertOrReplaceAsync(AddItem);
                    TransformerRatioTest8121.Add(AddItem);
                    if (TransformerRatioTest8121.Count > 1)
                        TransformerRatioTest8121.Skip(1).ForEach(x => x.IsUpdated = true);
                    TransformerRatioTest8121 = new ObservableCollection<T_ITR8121_004ATransformerRatioTest>(TransformerRatioTest8121);
                    DependencyService.Get<IToastMessage>().ShortAlert("Record Added Successfully");
                    App.IsBusy = false;
                }
                else if (param == "DeleteInsConAndAuxiliary8121")
                {
                    if (SelectedInForConAndAuxiliary8121 != null)
                    {
                        if (await _userDialogs.ConfirmAsync("Are you sure you want to delete?", "Delete Record", "Yes", "No"))
                        {
                            T_ITR8121_004AInCAndAuxiliary InConAux = InispactionForControlAndAuxiliary8121.Where(x => x.ID == SelectedInForConAndAuxiliary8121.ID && x.RowID == SelectedInForConAndAuxiliary8121.RowID && x.CCMS_HEADERID == SelectedInForConAndAuxiliary8121.CCMS_HEADERID).FirstOrDefault();

                            //string DelCTdata = "DELETE FROM [T_ITR8121_004AInCAndAuxiliary] WHERE ID = '" + SelectedInForConAndAuxiliary8121.ID + "' AND RowID = '" + SelectedInForConAndAuxiliary8121.RowID + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "'"
                            //                 + " AND ITRCommonID = '" + SelectedInForConAndAuxiliary8121.ITRCommonID + "' AND ModelName = '" + SelectedInForConAndAuxiliary8121.ModelName + "'";
                            //await _ITR8121_004AInCAndAuxiliaryRepository.QueryAsync(DelCTdata);
                            await _ITR8121_004AInCAndAuxiliaryRepository.DeleteAsync(InConAux);
                            InispactionForControlAndAuxiliary8121.Remove(InConAux);

                            SelectedInForConAndAuxiliary8121 = null;
                        }
                    }
                    App.IsBusy = false;
                }
                else if (param == "DeleteTransRatioTest8121")
                {
                    if (SelectedTransformerRatioTest8121 != null)
                    {
                        if (await _userDialogs.ConfirmAsync("Are you sure you want to delete?", "Delete Record", "Yes", "No"))
                        {
                            T_ITR8121_004ATransformerRatioTest TRT = TransformerRatioTest8121.Where(x => x.ID == SelectedTransformerRatioTest8121.ID && x.RowID == SelectedTransformerRatioTest8121.RowID && x.CCMS_HEADERID == SelectedTransformerRatioTest8121.CCMS_HEADERID).FirstOrDefault();

                            //string DelCTdata = "DELETE FROM [T_ITR8121_004ATransformerRatioTest] WHERE ID = '" + SelectedTransformerRatioTest8121.ID + "' AND ITRCommonID = '" + SelectedTransformerRatioTest8121.ITRCommonID + "'"
                            //                 + " AND RowID = '" + SelectedInForConAndAuxiliary8121.RowID + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + SelectedTransformerRatioTest8121.ModelName + "'";
                            //await _ITR8121_004ATransformerRatioTestRepository.QueryAsync(DelCTdata);
                            await _ITR8121_004ATransformerRatioTestRepository.DeleteAsync(TRT);

                            TransformerRatioTest8121.Remove(TRT);

                            SelectedTransformerRatioTest8121 = null;
                        }
                    }
                    App.IsBusy = false;
                }
                else if (param == "AddNewTestInstrumentItem")
                {
                    ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
                    await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentDataList.ToList());

                    var ITRInstrumentData1 = ITRInstrumentDataList.ToList();
                    var Instrumentdata = await _ITRInstrumentDataRepository.GetAsync();
                    long InstID = 1;
                    if (Instrumentdata.Count > 0)
                        InstID = Instrumentdata.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                    T_ITRInstrumentData newItem = new T_ITRInstrumentData { RowID = InstID, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, ID = 0, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID };
                    ITRInstrumentData1.Add(newItem);
                    await _ITRInstrumentDataRepository.InsertOrReplaceAsync(newItem);
                    BindIstrumentData();
                    //int i = 1;
                    //ITRInstrumentData1.ForEach(x => { x.No = i++; x.TestEquipment = TestEquipmentDataModelList.Where(p => p.ID == x.CCMS_EquipmentID).Select(q => q.TestEquipmentDataString).FirstOrDefault(); x.TestEquipmentDataList = TestEquipmentDataModelList.Select(q => q.TestEquipmentDataString).ToList(); });
                    //ITRInstrumentData1.Skip(1).ForEach(x => x.IsDeletable = true);
                    //ITRInstrumentDataList = new ObservableCollection<T_ITRInstrumentData>(ITRInstrumentData1);
                    //DependencyService.Get<IToastMessage>().ShortAlert("Record Added Successfully");
                    App.IsBusy = false;
                }
                else if (param == "DeleteTestInstrumentItem")
                {
                    ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
                    var ITRInstrumentData2 = ITRInstrumentDataList.ToList();
                    ITRInstrumentData2.Remove(SelectedITRInstrument);
                    await _ITRInstrumentDataRepository.DeleteAsync(SelectedITRInstrument);
                    await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentData2);
                    BindIstrumentData();
                    //int i = 1;
                    //ITRInstrumentData2.ForEach(x => { x.No = i++; x.TestEquipment = TestEquipmentDataModelList.Where(p => p.ID == x.CCMS_EquipmentID).Select(q => q.TestEquipmentDataString).FirstOrDefault(); x.TestEquipmentDataList = TestEquipmentDataModelList.Select(q => q.TestEquipmentDataString).ToList(); });
                    //ITRInstrumentData2.Skip(1).ForEach(x => x.IsDeletable = true);
                    //ITRInstrumentDataList = new ObservableCollection<T_ITRInstrumentData>(ITRInstrumentData2);
                    App.IsBusy = false;
                }
                else if (param == "DeleteITRItem8260_002A")
                {
                    if (DielectricRecord != null)
                    {
                        if (await _userDialogs.ConfirmAsync("Are you sure you want to delete?", "Delete Record", "Yes", "No"))
                        {
                            string DelConRes = "DELETE FROM [T_ITR_8260_002A_DielectricTest] WHERE ID = '" + DielectricRecord.ID + "' AND ITRCommonID = '" + DielectricRecord.ITRCommonID + "' AND CommonRowID = '" + DielectricRecord.CommonRowID + "' AND RowID = '" + DielectricRecord.RowID + "' AND CCMS_HEADERID =" + DielectricRecord.CCMS_HEADERID;
                            await _ITR_8260_002A_DielectricTestRepository.QueryAsync(DelConRes);

                            DielectricTestRecords.Remove(DielectricRecord);
                            DielectricTestRecords = DielectricTestRecords;
                            DielectricRecord = null;
                            Die8260_002AHeight = DielectricTestRecords.Count() * 50;
                        }
                    }
                    App.IsBusy = false;
                }
            }
        }
        public async void SignOffClicked(string param, string SignatureName)
        {
            //    if (IsCheckSheetAccessible)
            //    {
            if (SignatureName == "Press to Sign")
            {
                bool requiredSign = false;
                foreach (T_CommonHeaderFooterSignOff item in SignOffList)
                {
                    if (item.Title == param)
                        break;
                    if (item.FullName == "Press to Sign")
                        requiredSign = true;
                }
                if (requiredSign)
                {
                    _userDialogs.Alert("Previous signature sign off is required before making this signature", "", "Ok");
                    return;
                }
                if (!App.IsBusy)
                {
                    App.IsBusy = true;
                    var _credentials = await ReadLoginPopup("SignOff");
                    SaveSignOffHeader(_credentials, param);
                }
            }
            else
            {
                if (SignatureName == Settings.CompletionUserName)
                {
                    int index = SignOffList.ToList().FindIndex(a => a.Title == param) + 1;
                    bool requiredUnSign = false;
                    if (index <= SignOffList.Count - 1)
                    {
                        for (int i = index; i < SignOffList.Count; i++)
                        {
                            if (!(SignOffList[i].FullName == "Press to Sign" || SignOffList[i].FullName == "NA"))
                            {
                                requiredUnSign = true;
                                break;
                            }
                        }
                        if (requiredUnSign)
                        {
                            _userDialogs.Alert("You cannot unsign this signature", "", "Ok");
                            return;
                        }
                    }

                    if (await _userDialogs.ConfirmAsync("Are you sure you want to unsign this signature?", "", "Yes", "No"))
                    {
                        T_CommonHeaderFooterSignOff currentSignOff = SignOffList.Where(x => x.Title == param).FirstOrDefault();
                        string UpdateSQL = @"UPDATE T_CommonHeaderFooterSignOff SET [FullName] = '',[IsSynced] = 1"
                                   + " WHERE [SignOffTag] = '" + currentSignOff.SignOffTag + "' AND [SignOffChecksheet] = '" + currentSignOff.SignOffChecksheet + "'"
                                   + " AND [ITRCommonID] = '" + CommonHeaderFooter.ID + "' AND [CommonRowID] = '" + currentSignOff.CommonRowID + "' AND [ModelName] = '" + Settings.ModelName + "' AND [Title] = '" + currentSignOff.Title + "' AND CCMS_HEADERID =" + currentSignOff.CCMS_HEADERID;
                        await _CommonHeaderFooterSignOffRepository.QueryAsync(UpdateSQL);
                        SignOffList.Where(x => x.Title == param).ToList().ForEach(x => x.FullName = "Press to Sign");
                        SignOffList = new ObservableCollection<T_CommonHeaderFooterSignOff>(SignOffList);

                        CommonHeaderFooter.Completed = false;
                        CommonHeaderFooter.Completedby = "";
                        CommonHeaderFooter.Completedon = new DateTime(2000, 01, 01);

                        await _CommonHeaderFooterRepository.InsertOrReplaceAsync(CommonHeaderFooter);
                    }
                }
            }
            //}
        }
        private async void SaveSignOffHeader(LoginModel _credentials, string param)
        {

            if (String.IsNullOrEmpty(_credentials.UserName) || String.IsNullOrEmpty(_credentials.UserName))
            {
                _userDialogs.Alert("Please enter userName and Password.", "Required User Details", "Ok");
                return;
            }
            _ = Task.Delay(200);
            _CompletionpageHelper.CompletionTokenTimeStamp = DateTime.Now.ToString(AppConstant.DateSaveFormat);
            _CompletionpageHelper.CompletionToken = Settings.CompletionAccessToken = ModsTools.CompletionsCreateToken(_credentials.UserName, _credentials.Password, _CompletionpageHelper.CompletionTokenTimeStamp);
            _CompletionpageHelper.CompletionTokenExpiry = DateTime.Now.AddHours(2);
            _CompletionpageHelper.CompletionUnitID = Settings.UnitID;
            var Result = ModsTools.CompletionsValidateToken(_CompletionpageHelper.CompletionToken, _CompletionpageHelper.CompletionTokenTimeStamp);
            // var result =  _checkValidLogin.GetValidToken(_credentials);


            if (!Result)
            {
                //check offline user is available or not for sign
                var offlineUser = await _CompletionsUserRepository.GetAsync(x => x.UserName == _credentials.UserName && x.Password == _credentials.Password);
                if (offlineUser.Any())
                {
                    T_CompletionsUsers offileUser = offlineUser.FirstOrDefault();
                    List<T_Ccms_signature> offlineSignaturelist = new List<T_Ccms_signature>();
                    if (offileUser.Company_Category_Code != null && offileUser.Function_Code != null && offileUser.Section_Code != null)
                    {                        
                        var signatureData = await _Ccms_signatureRepository.QueryAsync("select * from T_Ccms_signature where signatureName = '" + param + "' and ITR = '" + SelectedCheckSheet.ITRNumber + "' and ProjectName = '" + Settings.ProjectName + "' ");
                        if (signatureData.Any())
                        {
                            offlineSignaturelist = new List<T_Ccms_signature>(signatureData);
                            var filterdata = offlineSignaturelist.Where(x => x.CompanyCategoryCode == offileUser.Company_Category_Code && x.FunctionCode == offileUser.Function_Code && x.SectionCode == offileUser.Section_Code);
                            if (filterdata.Any()) { }
                            else
                            {
                                _userDialogs.Alert("You cannot sign this signature because your User rights do not match, your rights are - User company Code-'" + offileUser.Company_Category_Code + "'" +
                         "User function code - '" + offileUser.Function_Code + "'. user section code- '" + offileUser.Section_Code + "'. The rights you require to sign these off are  User company Code-'" + offlineSignaturelist.FirstOrDefault().CompanyCategoryCode + "'" +
                         "User function code - '" + offlineSignaturelist.FirstOrDefault().FunctionCode + "'. user section code- '" + offlineSignaturelist.FirstOrDefault().SectionCode + "'. ");
                                return;
                            }
                        }
                        else
                        {
                            _userDialogs.Alert("You cannot sign this signature because your User rights do not match");
                            return;
                        }
                    }
                    else
                    {
                        _userDialogs.Alert("You cannot sign this signature because your User rights do not match");
                        return;
                    }
                    var offlineSignOff = SignOffList.Where(x => x.Title == param).FirstOrDefault();
                    offlineSignOff.FullName = offileUser.FullName;
                    offlineSignOff.SignDate = DateTime.Now;
                    offlineSignOff.IsSynced = true;
                    offlineSignOff.Rejected = false;
                    offlineSignOff.RejectedReason = "";

                    //await _CommonHeaderFooterSignOffRepository.UpdateAsync(signOff);
                    string RemoveRejectSQL = @"UPDATE T_CommonHeaderFooterSignOff SET [rejected] = 0, [RejectedReason]='', [IsSynced] = 1"
                                          + " WHERE [SignOffTag] = '" + offlineSignOff.SignOffTag + "' AND [SignOffChecksheet] = '" + offlineSignOff.SignOffChecksheet + "'"
                                          + " AND [ITRCommonID] = '" + CommonHeaderFooter.ID + "' AND [CommonRowID] = '" + offlineSignOff.CommonRowID + "' AND [ModelName] = '" + Settings.ModelName + "' AND CCMS_HEADERID =" + offlineSignOff.CCMS_HEADERID;
                    await _CommonHeaderFooterSignOffRepository.QueryAsync(RemoveRejectSQL);
                    string UpdateSQL = @"UPDATE T_CommonHeaderFooterSignOff SET [FullName] = '" + offileUser.FullName + "',[SignDate] = '" + offlineSignOff.SignDate.Ticks + "'"
                                           + " WHERE [SignOffTag] = '" + offlineSignOff.SignOffTag + "' AND [SignOffChecksheet] = '" + offlineSignOff.SignOffChecksheet + "'"
                                           + " AND [ITRCommonID] = '" + CommonHeaderFooter.ID + "' AND [CommonRowID] = '" + offlineSignOff.CommonRowID + "' AND [ModelName] = '" + Settings.ModelName + "' AND [Title] = '" + offlineSignOff.Title + "' AND CCMS_HEADERID =" + offlineSignOff.CCMS_HEADERID;
                    await _CommonHeaderFooterSignOffRepository.QueryAsync(UpdateSQL);
                    SignOffList.Where(x => x.Title == param).ToList().ForEach(x => x.FullName = offileUser.FullName);
                    SignOffList = new ObservableCollection<T_CommonHeaderFooterSignOff>(SignOffList);

                    if (SignOffList.LastOrDefault().FullName != "Press to Sign" && SignOffList.LastOrDefault().FullName.ToUpper() != "NA")
                    {
                        CommonHeaderFooter.Completed = true;
                        CommonHeaderFooter.Completedby = SignOffList.LastOrDefault().FullName;
                        CommonHeaderFooter.Completedon = DateTime.Now;
                    }
                    else if (SignOffList.Count >= 2 && SignOffList.LastOrDefault().FullName.ToUpper() == "NA")
                    {
                        if (SignOffList[SignOffList.Count - 2].FullName != "Press to Sign")
                        {
                            CommonHeaderFooter.Completed = true;
                            CommonHeaderFooter.Completedby = SignOffList[SignOffList.Count - 2].FullName;
                            CommonHeaderFooter.Completedon = DateTime.Now;
                        }
                    }

                    await _CommonHeaderFooterRepository.InsertOrReplaceAsync(CommonHeaderFooter);
                    return;
                }
            }
            


            if (Result)
            {
                List<T_Ccms_signature> signaturelist = new List<T_Ccms_signature>();
                CommonHeaderFooter.RejectedReason = "";
                CommonHeaderFooter.Rejected = false;
                CommonHeaderFooter.rejectedDate = new DateTime(2000, 01, 01);
                CommonHeaderFooter.RejectedUserID = Settings.CompletionUserID;
                //CommonHeaderFooter.Started = true;
                //CommonHeaderFooter.StartedBy = Settings.CompletionUserName;
                //CommonHeaderFooter.StartedOn = DateTime.Now;

                string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.GetUser(_credentials.UserName, _credentials.Password, Settings.CurrentDB), Settings.CompletionAccessToken);
                var CurrentUser = JsonConvert.DeserializeObject<T_UserControl>(JsonString);
                var signOff = SignOffList.Where(x => x.Title == param).FirstOrDefault();

                if (CurrentUser.Company_Category_Code != null && CurrentUser.Function_Code != null && CurrentUser.Section_Code != null)
                {
                    bool ISvalida = false;
                    var signatureData = await _Ccms_signatureRepository.QueryAsync("select * from T_Ccms_signature where signatureName = '" + param + "' and ITR = '" + SelectedCheckSheet.ITRNumber + "' and ProjectName = '" + Settings.ProjectName + "' ");
                    if (signatureData.Any())
                    {
                        signaturelist = new List<T_Ccms_signature>(signatureData);
                        var filterdata = signaturelist.Where(x => x.CompanyCategoryCode == CurrentUser.Company_Category_Code && x.FunctionCode == CurrentUser.Function_Code && x.SectionCode == CurrentUser.Section_Code);
                        if (filterdata.Any()) { }
                        else
                        {
                            _userDialogs.Alert("You cannot sign this signature because your User rights do not match, your rights are - User company Code-'" + CurrentUser.Company_Category_Code + "'" +
                     "User function code - '" + CurrentUser.Function_Code + "'. user section code- '" + CurrentUser.Section_Code + "'. The rights you require to sign these off are  User company Code-'" + signaturelist.FirstOrDefault().CompanyCategoryCode + "'" +
                     "User function code - '" + signaturelist.FirstOrDefault().FunctionCode + "'. user section code- '" + signaturelist.FirstOrDefault().SectionCode + "'. ");
                            return;
                        }
                    }
                    else
                    {
                        _userDialogs.Alert("You cannot sign this signature because your User rights do not match");
                        return;
                    }
                }
                else
                {
                    _userDialogs.Alert("You cannot sign this signature because your User rights do not match");
                    return;
                }

                signOff.FullName = CurrentUser.FullName;
                signOff.SignDate = DateTime.Now;
                signOff.IsSynced = true;
                signOff.Rejected = false;
                signOff.RejectedReason = "";

                //await _CommonHeaderFooterSignOffRepository.UpdateAsync(signOff);
                string RemoveRejectSQL = @"UPDATE T_CommonHeaderFooterSignOff SET [rejected] = 0, [RejectedReason]='', [IsSynced] = 1"
                                      + " WHERE [SignOffTag] = '" + signOff.SignOffTag + "' AND [SignOffChecksheet] = '" + signOff.SignOffChecksheet + "'"
                                      + " AND [ITRCommonID] = '" + CommonHeaderFooter.ID + "' AND [CommonRowID] = '" + signOff.CommonRowID + "' AND [ModelName] = '" + Settings.ModelName + "' AND CCMS_HEADERID =" + signOff.CCMS_HEADERID;
                await _CommonHeaderFooterSignOffRepository.QueryAsync(RemoveRejectSQL);
                string UpdateSQL = @"UPDATE T_CommonHeaderFooterSignOff SET [FullName] = '" + CurrentUser.FullName + "',[SignDate] = '" + signOff.SignDate.Ticks + "'"
                                       + " WHERE [SignOffTag] = '" + signOff.SignOffTag + "' AND [SignOffChecksheet] = '" + signOff.SignOffChecksheet + "'"
                                       + " AND [ITRCommonID] = '" + CommonHeaderFooter.ID + "' AND [CommonRowID] = '" + signOff.CommonRowID + "' AND [ModelName] = '" + Settings.ModelName + "' AND [Title] = '" + signOff.Title + "' AND CCMS_HEADERID =" + signOff.CCMS_HEADERID;
                await _CommonHeaderFooterSignOffRepository.QueryAsync(UpdateSQL);
                SignOffList.Where(x => x.Title == param).ToList().ForEach(x => x.FullName = CurrentUser.FullName);
                SignOffList = new ObservableCollection<T_CommonHeaderFooterSignOff>(SignOffList);

                if (SignOffList.LastOrDefault().FullName != "Press to Sign" && SignOffList.LastOrDefault().FullName.ToUpper() != "NA")
                {
                    CommonHeaderFooter.Completed = true;
                    CommonHeaderFooter.Completedby = SignOffList.LastOrDefault().FullName;
                    CommonHeaderFooter.Completedon = DateTime.Now;
                }
                else if (SignOffList.Count >= 2 && SignOffList.LastOrDefault().FullName.ToUpper() == "NA")
                {
                    if (SignOffList[SignOffList.Count - 2].FullName != "Press to Sign")
                    {
                        CommonHeaderFooter.Completed = true;
                        CommonHeaderFooter.Completedby = SignOffList[SignOffList.Count - 2].FullName;
                        CommonHeaderFooter.Completedon = DateTime.Now;
                    }
                }

                await _CommonHeaderFooterRepository.InsertOrReplaceAsync(CommonHeaderFooter);
                //OnTagSelectionChage();
                //BindTagsAsync();
            }
            else
            {
                _userDialogs.Alert("Failed to login to this account.", "Login Error", "Ok");
            }
        }

        private async Task<bool> CheckSignOfAccess(LoginModel _credentials)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_credentials.Password) && string.IsNullOrWhiteSpace(_credentials.Password))
                    return false;
                _ = Task.Delay(200);
                _CompletionpageHelper.CompletionTokenTimeStamp = DateTime.Now.ToString(AppConstant.DateSaveFormat);
                _CompletionpageHelper.CompletionToken = Settings.CompletionAccessToken = ModsTools.CompletionsCreateToken(_credentials.UserName, _credentials.Password, _CompletionpageHelper.CompletionTokenTimeStamp);
                _CompletionpageHelper.CompletionTokenExpiry = DateTime.Now.AddHours(2);
                _CompletionpageHelper.CompletionUnitID = Settings.UnitID;
                var Result = ModsTools.CompletionsValidateToken(_CompletionpageHelper.CompletionToken, _CompletionpageHelper.CompletionTokenTimeStamp);
                // var result =  _checkValidLogin.GetValidToken(_credentials);

                if (Result)
                {
                    string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.GetUser(_credentials.UserName, _credentials.Password, Settings.CurrentDB), Settings.CompletionAccessToken);
                    var CurrentUser = JsonConvert.DeserializeObject<T_UserControl>(JsonString);
                    var signOff = SignOffList.FirstOrDefault();

                    if (signOff.FullName == CurrentUser.FullName)
                        return true;
                    else
                        return false;
                }
                else
                {
                    //_userDialogs.Alert("Faild to login to this account.", "Login Error", "Ok");
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async void SaveRejectITR()
        {
            bool AbletoReject = false;
            foreach (T_CommonHeaderFooterSignOff signOff in SignOffList)
            {
                if (signOff.Title.ToLower() == "client" && signOff.FullName.ToLower() == "na" && signOff.FullName != "Press to Sign")
                    AbletoReject = true;
                else if (!string.IsNullOrEmpty(signOff.FullName.Trim()) && signOff.FullName != "Press to Sign")
                    AbletoReject = true;
            }
            if (AbletoReject)
            {
                var rejectReason = await ReadRejectPopup(CommonHeaderFooter.RejectedReason);
                if (string.IsNullOrEmpty(rejectReason)) return;
                CommonHeaderFooter.RejectedReason = rejectReason;
                CommonHeaderFooter.Rejected = true;
                CommonHeaderFooter.rejectedDate = DateTime.Now;
                CommonHeaderFooter.RejectedUserID = Settings.CompletionUserID;
                //CommonHeaderFooter.Remarks = "";
                CommonHeaderFooter.Started = false;
                CommonHeaderFooter.StartedBy = "";
                CommonHeaderFooter.StartedOn = new DateTime(2000, 01, 01);
                CommonHeaderFooter.Completed = false;
                CommonHeaderFooter.Completedby = "";
                CommonHeaderFooter.Completedon = new DateTime(2000, 01, 01);
                SignOffList.ToList().ForEach(x => { x.RejectedReason = rejectReason; x.FullName = ""; x.Rejected = true; x.IsSynced = true; });
                string updateSignOffSQL = @"UPDATE T_CommonHeaderFooterSignOff SET  [Rejected] = 1, [RejectedReason] = '" + rejectReason + "', [FullName] = '', [IsSynced] = 1, SignDate ='" + new DateTime(2000, 01, 01).Ticks + "'"
                                    + " WHERE [ITRCommonID] = '" + SignOffList.FirstOrDefault().ITRCommonID + "' AND [ModelName] = '" + SignOffList.FirstOrDefault().ModelName + "' AND [CommonRowID] = '" + SignOffList.FirstOrDefault().CommonRowID + "' AND [CCMS_HEADERID] = " + SignOffList.FirstOrDefault().CCMS_HEADERID
                                   + " AND [SignOffChecksheet]= '" + SignOffList.FirstOrDefault().SignOffChecksheet + "' AND [SignOffTag]= '" + SignOffList.FirstOrDefault().SignOffTag + "' ";
                await _CommonHeaderFooterSignOffRepository.QueryAsync(updateSignOffSQL);


                await _CommonHeaderFooterRepository.InsertOrReplaceAsync(CommonHeaderFooter);
                //ItemSourceQuetions.ForEach(x => x.AdditionalFields.ForEach(i => i.FieldValue = ""));

                string updateAnsSQL = @"UPDATE T_TAG_SHEET_ANSWER SET  [checkValue] = '', [IsSynced] = 0  WHERE [checksheetname] = '" + SignOffList.FirstOrDefault().SignOffChecksheet + "' AND [tagName] = '" + SignOffList.FirstOrDefault().SignOffTag + "'";
                await _TAG_SHEET_ANSWERRepository.QueryAsync(updateAnsSQL);

                SignOffList.ToList().ForEach(x => x.FullName = "Press to Sign");
                SignOffList = new ObservableCollection<T_CommonHeaderFooterSignOff>(SignOffList);
                //OnTagSelectionChage();
                //BindTagsAsync();
            }
            else
            {
                App.IsBusy = false;
                _userDialogs.Alert("It should have at least one SignOff before it can be rejected", "Unable to reject", "Ok");
            }
        }
        [Obsolete]
        public static Task<LoginModel> ReadLoginPopup(string param)
        {
            try
            {
                var vm = new SignOffPopupViewModel();
                if (param == "OpenITR")
                {
                    vm.LoginButtonText = "Open ITR";
                    vm.LoginHeaderText = "Login to sign off";
                }
                else if (param == "SignOff")
                {
                    vm.LoginButtonText = "Login";
                    vm.LoginHeaderText = "Login to sign off";

                }

                //vm.FilterList = Source;
                var tcs = new TaskCompletionSource<LoginModel>();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var page = new SignOffPopup(vm);
                    await PopupNavigation.PushAsync(page);
                    var value = await vm.GetValue();
                    await PopupNavigation.PopAsync(true);
                    tcs.SetResult(value);
                });
                return tcs.Task;
            }
            catch (Exception ex)
            {
                return new TaskCompletionSource<LoginModel>().Task;
            }
        }

        public static Task<string> ReadRejectPopup(string rejectedReason)
        {
            var vm = new RejectPopupViewModel();
            vm.RejectComment = rejectedReason;
            //vm.FilterList = Source;
            var tcs = new TaskCompletionSource<string>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var page = new RejectPopup(vm);
                await PopupNavigation.PushAsync(page);
                var value = await vm.GetValue();
                await PopupNavigation.PopAsync(true);
                tcs.SetResult(value);
            });
            return tcs.Task;
        }
        public async void LoadITRData()
        {
            if (await _ITRService.ITR_3XA(SelectedCheckSheet.ITRNumber))
            {
                string recordsql = " SELECT * FROM T_ITRRecords_30A_31A WHERE TagNO='" + SelectedTag.name + "' AND ITRNumber='" + SelectedCheckSheet.ITRNumber + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                var Recorddata = await _RecordsRepository.QueryAsync(recordsql);
                ITRRecord = Recorddata.FirstOrDefault();

                if (ITRRecord != null)
                {
                    if (ITRRecord.DateToday > new DateTime(2000, 1, 1))
                    {
                        IsvisibleToday = false;
                    }
                    else
                    {
                        IsvisibleToday = true;
                        ITRRecord.DateToday = DateTime.Now;
                    }
                    ITRRecord.CalibrationDate = ITRRecord.CalibrationDate > new DateTime(2000, 1, 1) ? ITRRecord.CalibrationDate : DateTime.Now;
                    ITRRecord.DueDate = ITRRecord.DueDate > new DateTime(2000, 1, 1) ? ITRRecord.DueDate : DateTime.Now;

                    if (!string.IsNullOrEmpty(ITRRecord.TestResultsAccepted))
                        SelectedTestResultsAccepted = ITRRecord.TestResultsAccepted;
                    else
                        SelectedTestResultsAccepted = ListTestResultAccepted.LastOrDefault(); ;

                    string tubeColorSql = "SELECT * FROM T_ITRTubeColors WHERE RecordsID = '" + ITRRecord.ID + "' AND ModelName = '" + Settings.ModelName + "'  AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var TubeColorsData = await _TubeColorsRepository.QueryAsync(tubeColorSql);
                    ItemSourceTubeColors = new ObservableCollection<T_ITRTubeColors>(TubeColorsData);

                    if (ItemSourceTubeColors.Count <= 0)
                    {
                        var NewTubeitem = Enumerable.Range(0, 96).Select(x => new T_ITRTubeColors { ModelName = Settings.ModelName, CommonRowID = SelectedCheckSheet.ROWID, CommonHFID = CommonHeaderFooter.ID, CCMS_HEADERID = (int)CommonHeaderFooter.ID, Remarks = "" });
                        List<T_ITRTubeColors> NewTubecolors = NewTubeitem.ToList();
                        var _TubeColors = await _TubeColorsRepository.GetAsync();
                        long TCID = _TubeColors.Count() + 1;
                        int i = 0;
                        while (i < 8)
                        {
                            int j = 0;
                            foreach (T_ITRTubeColors item in NewTubecolors.Skip(i * 12).Take(12).ToList())
                            {
                                item.TubeColor = (i == 0 ? "Blue" : (i == 1 ? "Orange" : (i == 2 ? "Green" : (i == 3 ? "Brown" : (i == 4 ? "Grey" : (i == 5 ? "White" : (i == 6 ? "Red" : "Black")))))));
                                item.FiberColor = (j == 0 ? "Blue" : (j == 1 ? "Orange" : (j == 2 ? "Green" : (j == 3 ? "Brown" : (j == 4 ? "Grey" : (j == 5 ? "White" : (j == 6 ? "Red" : (j == 7 ? "Black" : (j == 8 ? "Yellow" : (j == 9 ? "Violet" : (j == 10 ? "Pink" : "Turquoise")))))))))));
                                //item.ModelName = Settings.ModelName;
                                item.RecordsID = ITRRecord.ID;
                                //item.CommonRowID = SelectedCheckSheet.ROWID;
                                item.RowID = TCID++;
                                j++;
                            }
                            i++;
                        }
                        ItemSourceTubeColors = new ObservableCollection<T_ITRTubeColors>(NewTubecolors);
                    }
                }
                else
                {
                    ITRRecord = new T_ITRRecords_30A_31A();
                    ITRRecord.CalibrationDate = ITRRecord.DueDate = DateTime.Now;
                }
            }
            else if (await _ITRService.ITR_8140_001A(SelectedCheckSheet.ITRNumber))
            {
                SimpleITR = (SelectedCheckSheet.ITRNumber == "8140-001A") ? true : false;
                string sql = "SELECT * from T_ITR8140_001A_ContactResisTest where ITRCommonID='" + CommonHeaderFooter.ID + "' AND [CommonRowID] = '" + CommonHeaderFooter.ROWID + "' and ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID + " order by ID asc";
                var ContactResisTestData = await _T_ITR8140_001A_ContactResisTestRepository.QueryAsync(sql);

                if (ContactResisTestData.Count <= 0)
                {
                    var ContactResisTest = await _T_ITR8140_001A_ContactResisTestRepository.GetAsync();
                    long CRTID = ContactResisTest.Count() + 1;
                    T_ITR8140_001A_ContactResisTest newItem = new T_ITR8140_001A_ContactResisTest { ITRCommonID = CommonHeaderFooter.ID, RowID = CRTID, CommonRowID = CommonHeaderFooter.ROWID, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID, TorqueMarkOkValue = "" };
                    ContactResisTestData.Add(newItem);
                    await _T_ITR8140_001A_ContactResisTestRepository.InsertOrReplaceAsync(newItem);
                }
                int i = 1;
                ContactResisTestData.ToList().ForEach(x => x.row_Id = i++);
                //ContactResisTestData.ToList().ForEach(x => x.TorqueMarkOkValue = x.TorqueMarkOk ? "Yes" : "No");
                ContactResisTestData.Skip(1).ForEach(x => x.IsUpdated = true);
                ContactResisTestList = new ObservableCollection<T_ITR8140_001A_ContactResisTest>(ContactResisTestData);


                sql = "Select *  from T_ITR8140_001AInsulationResistanceTest where ITRCommonID='" + CommonHeaderFooter.ID + "' AND [CommonRowID] = '" + CommonHeaderFooter.ROWID + "' AND ModelName='" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID + " order by ID ";
                var cInsultest = await _T_ITR8140_001AInsulationResistanceTestRepository.QueryAsync(sql);

                List<T_ITR8140_001AInsulationResistanceTest> InsultestList = cInsultest.ToList();

                var InsuResisTest = await _T_ITR8140_001AInsulationResistanceTestRepository.GetAsync();
                long IRTID = InsuResisTest.Count() + 1;
                int AddItem = 3 - InsultestList.Count;
                if (AddItem > 0)
                    InsultestList.AddRange(Enumerable.Range(0, AddItem).Select(x => new T_ITR8140_001AInsulationResistanceTest { RowID = IRTID++, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID }).ToList());

                InsultestList[0].Phase1 = "L1-E";
                InsultestList[0].Phase2 = "L1-L2";
                InsultestList[0].Phase3 = "L1-N";
                InsultestList[1].Phase1 = "L2-E";
                InsultestList[1].Phase2 = "L2-L3";
                InsultestList[1].Phase3 = "L2-N";
                InsultestList[2].Phase1 = "L3-E";
                InsultestList[2].Phase2 = "L3-L1";
                InsultestList[2].Phase3 = "L3-N";

                InsulationRTests = new ObservableCollection<T_ITR8140_001AInsulationResistanceTest>(InsultestList);

                //di
                sql = "Select *  from T_ITR8140_001ADialectricTest where ITRCommonID='" + CommonHeaderFooter.ID + "' AND [CommonRowID] = '" + CommonHeaderFooter.ROWID + "' AND ModelName='" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID + " order by ID ";

                var testd = await _T_ITR8140_001ADialectricTestRepository.QueryAsync(sql);

                if (testd.Count <= 0)
                {
                    var DiaTest = await _T_ITR8140_001ADialectricTestRepository.GetAsync();
                    long DTID = DiaTest.Count() + 1;
                    testd = new List<T_ITR8140_001ADialectricTest>();
                    testd.Add(new T_ITR8140_001ADialectricTest { TestPhase = "L1-E", RowID = DTID++, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID });
                    testd.Add(new T_ITR8140_001ADialectricTest { TestPhase = "L2-E", RowID = DTID++, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID });
                    testd.Add(new T_ITR8140_001ADialectricTest { TestPhase = "L3-E", RowID = DTID++, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID });
                    await _T_ITR8140_001ADialectricTestRepository.InsertOrReplaceAsync(testd);
                }

                DieTests = new ObservableCollection<T_ITR8140_001ADialectricTest>(testd);

                sql = "Select *  from T_ITR8140_001ATestInstrucitonData where ITRCommonID='" + CommonHeaderFooter.ID + "' AND [CommonRowID] = '" + CommonHeaderFooter.ROWID + "' AND ModelName='" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID + " order by ID ";
                var InstData = await _T_ITR8140_001ATestInstrumentDataRepository.QueryAsync(sql);

                TestInstrucitonData = InstData.FirstOrDefault();

                if (TestInstrucitonData == null)
                    TestInstrucitonData = new T_ITR8140_001ATestInstrucitonData { CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID };

                TestInstrucitonData.CalibrationDate1 = TestInstrucitonData.CalibrationDate1.HasValue ? ((DateTime)TestInstrucitonData.CalibrationDate1 > new DateTime(2000, 1, 1) ? (DateTime)TestInstrucitonData.CalibrationDate1 : DateTime.Now) : DateTime.Now;
                TestInstrucitonData.CalibrationDate2 = TestInstrucitonData.CalibrationDate2.HasValue ? ((DateTime)TestInstrucitonData.CalibrationDate2 > new DateTime(2000, 1, 1) ? (DateTime)TestInstrucitonData.CalibrationDate2 : DateTime.Now) : DateTime.Now;
                TestInstrucitonData.CalibrationDate3 = TestInstrucitonData.CalibrationDate3.HasValue ? ((DateTime)TestInstrucitonData.CalibrationDate3 > new DateTime(2000, 1, 1) ? (DateTime)TestInstrucitonData.CalibrationDate3 : DateTime.Now) : DateTime.Now;

                BindIstrumentData();
            }
            else if (await _ITRService.ITR_4XA(SelectedCheckSheet.ITRNumber))
            {
                if (SelectedCheckSheet.ITRNumber == "7000-040A")
                    ITR_4X_Title = "Cable Insulation and Continuity (Drum)";
                else if (SelectedCheckSheet.ITRNumber == "7000-041A")
                    ITR_4X_Title = "Cable Insulation and Continuity (Before Back Filling)";
                else if (SelectedCheckSheet.ITRNumber == "7000-042A")
                    ITR_4X_Title = "Cable Insulation and Continuity (After Back Filling / Cable Tray Laying)";
                string record04xAsql = " SELECT * FROM T_ITRRecords_040A_041A_042A WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                var Record04xAdata = await _Records_04XARepository.QueryAsync(record04xAsql);
                ITRRecord04xA = Record04xAdata.ToList().FirstOrDefault();

                if (ITRRecord04xA != null)
                {
                    if (!string.IsNullOrEmpty(ITRRecord04xA.TestVoltage))
                        SelectedTestVoltage = ITRRecord04xA.TestVoltage;
                    else
                        SelectedTestVoltage = ListTestVoltage.FirstOrDefault();
                }

                string InsulationDetailsSql = "SELECT * FROM T_ITRInsulationDetails WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                var InsulationDetailsData = await _InsulationDetailsRepository.QueryAsync(InsulationDetailsSql);
                List<T_ITRInsulationDetails> NewInsulationDetail = InsulationDetailsData.ToList();
                

                if (!String.IsNullOrEmpty(ITRRecord04xA.NumberofCore) && ITRRecord04xA.NumberofCore.Length > 1)
                {
                    int NoofRows = Convert.ToInt32(ITRRecord04xA.NumberofCore.Remove(ITRRecord04xA.NumberofCore.Length - 1, 1));
                    string Endchar = ITRRecord04xA.NumberofCore.Substring(ITRRecord04xA.NumberofCore.Length - 1);
                    int NewAdd = NoofRows - NewInsulationDetail.Count();
                    if (Endchar.ToUpper() == "P" || Endchar.ToUpper() == "T")
                    {
                        var _InsulationDetails = await _InsulationDetailsRepository.GetAsync();
                        long InsID = _InsulationDetails.Count() + 1;
                        if (NewAdd > 0)
                        {
                            var NewInsul = Enumerable.Range(0, NewAdd).Select(x => new T_ITRInsulationDetails
                            {
                                ModelName = Settings.ModelName,
                                ContinuityResult = "Y",
                                CoretoCore = "Y",
                                CoretoArmor = "Y",
                                CoretoSheild = "Y",
                                ArmortoSheild = "Y",
                                SheidtoSheild = "Y",
                                CommonRowID = SelectedCheckSheet.ROWID,
                                ITRCommonID = CommonHeaderFooter.ID,
                                CCMS_HEADERID = (int)CommonHeaderFooter.ID,
                                RowID = InsID++,
                            }); ;
                            NewInsulationDetail.AddRange(NewInsul.ToList());
                        }
                        int i = 1;
                        NewInsulationDetail.ForEach(x => x.CoreNum = Convert.ToString(i++));
                    }
                    else if (Endchar.ToUpper() == "C")
                    {
                        if (NewAdd > 0)
                        {
                            var _InsulationDetails = await _InsulationDetailsRepository.GetAsync();
                            long InsID = _InsulationDetails.Count() + 1;
                            var NewInsul = Enumerable.Range(0, NewAdd / 2).Select(x => new T_ITRInsulationDetails
                            {
                                ModelName = Settings.ModelName,
                                ContinuityResult = "Y",
                                CoretoCore = "Y",
                                CoretoArmor = "Y",
                                CoretoSheild = "Y",
                                ArmortoSheild = "Y",
                                SheidtoSheild = "Y",
                                CommonRowID = SelectedCheckSheet.ROWID,
                                ITRCommonID = CommonHeaderFooter.ID,
                                CCMS_HEADERID = (int)CommonHeaderFooter.ID,
                                RowID = InsID++,
                            });
                            NewInsulationDetail.AddRange(NewInsul.ToList());
                        }
                        int i = 1;
                        NewInsulationDetail.ForEach(x => x.CoreNum = Convert.ToString(i++ + "/" + i++));
                    }
                }
                NewInsulationDetail.ForEach(x =>
                {
                    // for(acceptanceCriteriaList)
                    List<string> continuityResultOptionList = new List<string> { "Y", "N", "NA" };
                    List<string> coretoCoreOptionList = new List<string> { "Y", "N", "NA" };
                    List<string> coretoArmorOptionList = new List<string> { "Y", "N", "NA" }; 
                    List<string> coretoSheildOptionList = new List<string> { "Y", "N", "NA" };
                    List<string> armortoSheildOptionList = new List<string> { "Y", "N", "NA" };
                    List<string> sheidtoSheildOptionList = new List<string> { "Y", "N", "NA" };

                    if (x.ContinuityResult == null) x.ContinuityResult = "";
                    if (x.CoretoCore == null) x.CoretoCore = "";
                    if (x.CoretoArmor == null) x.CoretoArmor = "";
                    if (x.CoretoSheild == null) x.CoretoSheild = "";
                    if (x.ArmortoSheild == null) x.ArmortoSheild = "";
                    if (x.SheidtoSheild == null) x.SheidtoSheild = "";

                    if (!continuityResultOptionList.Contains(x.ContinuityResult))
                        continuityResultOptionList.Add(x.ContinuityResult);
                    if (!coretoCoreOptionList.Contains(x.CoretoCore))
                        coretoCoreOptionList.Add(x.CoretoCore);
                    if (!coretoArmorOptionList.Contains(x.CoretoArmor))
                        coretoArmorOptionList.Add(x.CoretoArmor);
                    if (!coretoSheildOptionList.Contains(x.CoretoSheild))
                        coretoSheildOptionList.Add(x.CoretoSheild);
                    if (!armortoSheildOptionList.Contains(x.ArmortoSheild))
                        armortoSheildOptionList.Add(x.ArmortoSheild);
                    if (!sheidtoSheildOptionList.Contains(x.SheidtoSheild))
                        sheidtoSheildOptionList.Add(x.SheidtoSheild);

                    x.ContinuityResultOptionsList = continuityResultOptionList;
                    x.CoretoCoreOptionsList = coretoCoreOptionList;
                    x.CoretoArmorOptionsList = coretoArmorOptionList;
                    x.CoretoSheildOptionsList = coretoSheildOptionList;
                    x.ArmortoSheildOptionsList = armortoSheildOptionList;
                    x.SheidtoSheildOptionsList = sheidtoSheildOptionList;

                });
                ItemSourceInsulationDetails = new ObservableCollection<T_ITRInsulationDetails>(NewInsulationDetail);
            }
            else if (await _ITRService.ITR_80_9XA(SelectedCheckSheet.ITRNumber))
            {
                if (SelectedCheckSheet.ITRNumber == "7000-080A")
                {
                    LblFirst = "- The Test Pressure shall be 1.1 times of the design pressure.";
                    LblSecond = "- Holding time by nitrogen gas shall be 3 - 5 minutes.";
                    LblThird = "- Confirm NO leakage in the tubing system.";
                    ITR_80_9X_Title = "Instrument Pressure Leak Test for Pressure Lead Line and Sampling System Line";
                }
                else if (SelectedCheckSheet.ITRNumber == "7000-090A")
                {
                    LblFirst = "- The Pressure shall be Normal Operating Pressure";
                    LblSecond = "- Holding time by IA(instrument air) Shall be 3-5 minutes.";
                    LblThird = "- Confirm NO leakage in the tubing system.";
                    ITR_80_9X_Title = "Instrument Pressure Leak Test for Instrument Air Line";
                }
                else if (SelectedCheckSheet.ITRNumber == "7000-091A")
                {
                    LblFirst = "- The Test Pressure shall be Normal operating Pressure.";
                    LblSecond = "- Holding time by IA(instrument air) Shall be X-X minutes.";
                    LblThird = "- Confirm NO leakage in the tubing system.";
                    ITR_80_9X_Title = "Instrument Pressure Leak Test for Instrument Air Pipe";
                }

                string recordsql = " SELECT * FROM T_ITRRecords_080A_090A_091A WHERE Tag='" + SelectedTag.name + "' AND ITR='" + SelectedCheckSheet.ITRNumber + "' AND CommonRowID ='" + SelectedCheckSheet.ROWID + "'  AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                var Recorddata = await _Records_080A_09XARepository.QueryAsync(recordsql);
                if (Recorddata.Count > 0)
                {
                    ITRRecord_80A_91A = Recorddata.FirstOrDefault();
                    SelectedTestResultAccept = ITRRecord_80A_91A.TestResult ? "Yes" : "NO";
                }
                else
                    SelectedTestResultAccept = "NO";
            }
            else if (await _ITRService.ITR_8XA(SelectedCheckSheet.ITRNumber))
            {
                try
                {
                    Title8000_003A = CommonHeaderFooter.ITRDescription;
                    string recordsql = " SELECT * FROM T_ITRRecords_8000_003A WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var Recorddata = await _Records_8000003ARepository.QueryAsync(recordsql);
                    if (Recorddata == null || Recorddata.Count <= 0)
                    {
                        Recorddata.Add(new T_ITR8000_003ARecords { CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID, CableTagNo = CommonHeaderFooter.Tag });
                    }
                    Records_8000_003A = Recorddata.FirstOrDefault();
                    Records_8000_003A.CalibrationDate = Records_8000_003A.CalibrationDate > new DateTime(2000, 1, 1) ? Records_8000_003A.CalibrationDate : DateTime.Now;


                    string tubeColorSql = "SELECT * FROM T_ITRRecords_8000_003A_AcceptanceCriteria WHERE ITRCommonID = '" + CommonHeaderFooter.ID + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var acceptanceCriteria = await _Records_8000003A_AcceptanceCriteriaRepository.QueryAsync(tubeColorSql);
                    List<T_ITR8000_003A_AcceptanceCriteria> acceptanceCriteriaList = new List<T_ITR8000_003A_AcceptanceCriteria>(acceptanceCriteria);

                    var AccCrRecords = await _Records_8000003A_AcceptanceCriteriaRepository.GetAsync();
                    long InsResTestID = AccCrRecords.Count() + 1;
                    int coreCount = acceptanceCriteriaList.Count + 1;
                    int NoOfRow = 0;
                    string TypeChar = string.Empty;
                    if (!String.IsNullOrEmpty(Records_8000_003A.TYPEAndSIZE))
                    {
                        List<string> ArrStr = Records_8000_003A.TYPEAndSIZE.Split('-').ToList();
                        NoOfRow = ArrStr.Count > 1 ? Convert.ToInt32(ArrStr[1]) : 0;
                        TypeChar = ArrStr.Count > 2 ? ArrStr[2].ToUpper() : "C";
                        CoreType = ArrStr[0].ToUpper();
                        if (TypeChar == "P") NoOfRow *= 2;
                        if (TypeChar == "T") NoOfRow *= 3;

                        RowWidth = CoreType == "PWC" ? 80 : 0;
                    }
                    if (!String.IsNullOrEmpty(Records_8000_003A.VOLTAGE))
                    {
                        int volt = GetIntValueFromStringVoltage(Records_8000_003A.VOLTAGE); // Records_8000_003A.VOLTAGE
                        if (volt <= 1)
                            AcceptanceCriteria8000_003A = "ACCEPTANCE CRITERIA 2,000 MΩ @ 500VDC";
                        else if (volt > 1 && volt <= 7)
                            AcceptanceCriteria8000_003A = "ACCEPTANCE CRITERIA 2,000 MΩ @ 1000VDC";
                        else if (volt > 7)
                            AcceptanceCriteria8000_003A = "ACCEPTANCE CRITERIA 10,000 MΩ @ 5000VDC";
                        else
                            AcceptanceCriteria8000_003A = "";
                    }
                    else
                        AcceptanceCriteria8000_003A = "";

                    int addRow = NoOfRow - (coreCount - 1);
                    if (addRow > 0 && CoreType == "PWC" && TypeChar == "C")
                    {
                        if (NoOfRow == 1)
                            acceptanceCriteriaList.Add(new T_ITR8000_003A_AcceptanceCriteria { LECEText = "L1-E", LLCCText = "N/A", ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, RowID = InsResTestID++, CCMS_HEADERID = (int)CommonHeaderFooter.ID });
                        else if (NoOfRow == 2)
                            acceptanceCriteriaList.AddRange(new List<T_ITR8000_003A_AcceptanceCriteria> {
                            new T_ITR8000_003A_AcceptanceCriteria { LECEText = "L1-E", LLCCText = "L1-L2", ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, RowID = InsResTestID++, CCMS_HEADERID = (int)CommonHeaderFooter.ID },
                            new T_ITR8000_003A_AcceptanceCriteria { LECEText = "L2-E", LLCCText = "", IsReadyOnlyLLCCText = true, ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, RowID = InsResTestID++, CCMS_HEADERID = (int)CommonHeaderFooter.ID }
                            });
                        else if (NoOfRow == 3)
                            acceptanceCriteriaList.AddRange(new List<T_ITR8000_003A_AcceptanceCriteria> {
                            new T_ITR8000_003A_AcceptanceCriteria { LECEText = "L1-E", LLCCText = "L1-L2", ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, RowID = InsResTestID++, CCMS_HEADERID = (int)CommonHeaderFooter.ID },
                            new T_ITR8000_003A_AcceptanceCriteria { LECEText = "L2-E", LLCCText = "L2-L3", ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, RowID = InsResTestID++, CCMS_HEADERID = (int)CommonHeaderFooter.ID },
                            new T_ITR8000_003A_AcceptanceCriteria { LECEText = "L3-E", LLCCText = "L3-L1", ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, RowID = InsResTestID++, CCMS_HEADERID = (int)CommonHeaderFooter.ID }
                            });
                        else if (NoOfRow == 3)
                            acceptanceCriteriaList.AddRange(new List<T_ITR8000_003A_AcceptanceCriteria> {
                            new T_ITR8000_003A_AcceptanceCriteria { LECEText = "L1-E", LLCCText = "L1-L2", ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, RowID = InsResTestID++, CCMS_HEADERID = (int)CommonHeaderFooter.ID },
                            new T_ITR8000_003A_AcceptanceCriteria { LECEText = "L2-E", LLCCText = "L2-L3", ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, RowID = InsResTestID++, CCMS_HEADERID = (int)CommonHeaderFooter.ID },
                            new T_ITR8000_003A_AcceptanceCriteria { LECEText = "L3-E", LLCCText = "L3-L1", ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, RowID = InsResTestID++, CCMS_HEADERID = (int)CommonHeaderFooter.ID },
                            new T_ITR8000_003A_AcceptanceCriteria { LECEText = "N-E", LLCCText = "L-N", ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, RowID = InsResTestID++, CCMS_HEADERID = (int)CommonHeaderFooter.ID }
                            });
                    }
                    else
                    {
                        if (addRow > 0 && TypeChar == "C")
                            acceptanceCriteriaList.AddRange(Enumerable.Range(0, addRow).Select(x => new T_ITR8000_003A_AcceptanceCriteria { LECEText = Convert.ToString(coreCount++), LLCCText = "", ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, RowID = InsResTestID++, CCMS_HEADERID = (int)CommonHeaderFooter.ID }).ToList());
                        else if (addRow > 0 && (TypeChar == "P" || TypeChar == "T"))
                        {
                            acceptanceCriteriaList.AddRange(Enumerable.Range(0, addRow).Select(x => new T_ITR8000_003A_AcceptanceCriteria { ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, RowID = InsResTestID++, CCMS_HEADERID = (int)CommonHeaderFooter.ID }).ToList());
                            int i = 1, j = 1;
                            acceptanceCriteriaList.ForEach(x =>
                            {
                                if (j > (TypeChar == "P" ? 2 : 3))
                                {
                                    i++; j = 1;
                                }
                                x.LECEText = Convert.ToString(i + "-" + j++);
                                x.LLCCText = "";
                            });
                        }
                    }
                    if (NoOfRow == 2 && CoreType == "PWC" && TypeChar == "C")
                        acceptanceCriteriaList[1].IsReadyOnlyLLCCText = true;

                    acceptanceCriteriaList.ForEach(x =>
                    {
                        // for(acceptanceCriteriaList)
                        List<string> LECEList = new List<string> { "", "Pass", "Fail" };
                        List<string> LLCCList = new List<string> { "", "Pass", "Fail" };

                        if (x.LECE == null) x.LECE = "";
                        if (x.LLCC == null) x.LLCC = "";
                        if (x.LECE == null) x.LECEText = "";
                        if (x.LLCC == null) x.LLCCText = "";

                        if (!LECEList.Contains(x.LECE))
                            LECEList.Add(x.LECE);

                        if (!LLCCList.Contains(x.LLCC))
                            LLCCList.Add(x.LLCC);

                        x.LECEOptionsList = LECEList;
                        x.LLCCOptionsList = LLCCList;

                    });

                    AcceptanceCriteriaList = new ObservableCollection<T_ITR8000_003A_AcceptanceCriteria>(acceptanceCriteriaList);
                    DynamicGridHeight = AcceptanceCriteriaList.Count() * 50;
                    BindIstrumentData();

                }
                catch (Exception ex)
                {
                }
            }
            else if (await _ITRService.ITR_8100_001A(SelectedCheckSheet.ITRNumber))
            {
                var data = await _ITR8100_001A_RatioTestRepository.GetAsync();
                long RTID = data.Count() + 1;
                int rowNo = 1;
                string recordsqlCT = " SELECT * FROM T_ITR8100_001A_CTdata WHERE TagNO='" + SelectedTag.name + "' AND ITRNumber='" + SelectedCheckSheet.ITRNumber + "' AND CommonRowID='" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                var CTdata = await _ITR8100_001A_CTdataRepository.QueryAsync(recordsqlCT);

                if (CTdata.Count <= 0)
                {
                    CTdata.Add(new T_ITR8100_001A_CTdata
                    {
                        IsUpdated = false,
                        CommonHFID = CommonHeaderFooter.ID,
                        CCMS_HEADERID = (int)CommonHeaderFooter.ID,
                        ITRNumber = CommonHeaderFooter.ITRNumber,
                        CommonRowID = CommonHeaderFooter.ROWID,
                        RowID = RTID,
                        TagNO = CommonHeaderFooter.Tag,
                        ModelName = Settings.ModelName
                    });
                    await _ITR8100_001A_CTdataRepository.InsertOrReplaceAsync(CTdata);
                }
                CTdata.ForEach(x => x.RowNo = rowNo++);
                ITR8100_001A_CTdata = new ObservableCollection<T_ITR8100_001A_CTdata>(CTdata);

                string recordsqlRT = " SELECT * FROM T_ITR8100_001A_RatioTest WHERE TagNO='" + SelectedTag.name + "' AND ITRNumber='" + SelectedCheckSheet.ITRNumber + "' AND CommonRowID='" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                var RTdata = await _ITR8100_001A_RatioTestRepository.QueryAsync(recordsqlRT);

                if (RTdata.Count <= 0)
                {
                    RTdata.Add(new T_ITR8100_001A_RatioTest
                    {
                        IsUpdated = false,
                        ID = (int)RTID,
                        CommonRowID = CommonHeaderFooter.ROWID,
                        RowID = RTID,
                        CommonHFID = CommonHeaderFooter.ID,
                        CCMS_HEADERID = (int)CommonHeaderFooter.ID,
                        ITRNumber = CommonHeaderFooter.ITRNumber,
                        TagNO = CommonHeaderFooter.Tag,
                        ModelName = Settings.ModelName
                    });
                    await _ITR8100_001A_RatioTestRepository.InsertOrReplaceAsync(RTdata);
                }
                rowNo = 1;
                RTdata.ForEach(x => x.RowNo = rowNo++);
                RTdata.Skip(1).ForEach(x => x.IsUpdated = true);
                ITR8100_001A_RatioTest = new ObservableCollection<T_ITR8100_001A_RatioTest>(RTdata);


                string recordsqlIR = " SELECT * FROM T_ITR8100_001A_InsulationResistanceTest WHERE CommonHFID='" + CommonHeaderFooter.ID + "' AND CommonRowID='" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                var IRdata = await _ITR8100_001A_IRTestRepository.QueryAsync(recordsqlIR);

                if (IRdata.Count <= 0)
                {
                    IRdata.Add(new T_ITR8100_001A_InsulationResistanceTest
                    {
                        IsUpdated = false,
                        ID = (int)RTID,
                        CommonRowID = CommonHeaderFooter.ROWID,
                        RowID = RTID,
                        CommonHFID = CommonHeaderFooter.ID,
                        CCMS_HEADERID = (int)CommonHeaderFooter.ID,
                        ModelName = Settings.ModelName
                    });
                    await _ITR8100_001A_IRTestRepository.InsertOrReplaceAsync(IRdata);
                }
                rowNo = 1;
                IRdata.ForEach(x => x.RowNo = rowNo++);
                ITR8100_001A_InsulationResistanceTest = new ObservableCollection<T_ITR8100_001A_InsulationResistanceTest>(IRdata);


                string recordsqlTID = " SELECT * FROM T_ITR8100_001A_TestInstrumentData WHERE TagNO='" + SelectedTag.name + "' AND ITRNumber='" + SelectedCheckSheet.ITRNumber + "' AND CommonRowID='" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                var TIDdata = await _ITR8100_001A_TIDataRepository.QueryAsync(recordsqlTID);
                TestInstrumentData8100_001A = TIDdata.FirstOrDefault();
                if (TestInstrumentData8100_001A == null)
                    TestInstrumentData8100_001A = new T_ITR8100_001A_TestInstrumentData { CommonRowID = CommonHeaderFooter.ROWID, CommonHFID = CommonHeaderFooter.ID, CCMS_HEADERID = (int)CommonHeaderFooter.ID, ITRNumber = SelectedCheckSheet.ITRNumber, TagNO = SelectedTag.name, ModelName = Settings.ModelName };
                TestInstrumentData8100_001A.CalibrationDate = TestInstrumentData8100_001A.CalibrationDate <= new DateTime(2000, 1, 1) ? DateTime.Now : TestInstrumentData8100_001A.CalibrationDate;

                BindIstrumentData();
            }
            else if (await _ITRService.ITR_81_2XA(SelectedCheckSheet.ITRNumber))
            {
                string recordsql = " SELECT * FROM T_ITRRecords_8100_002A WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND CommonRowID='" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                var Record812Xata = await _ITRRecords_8100_002ARepository.QueryAsync(recordsql);
                ITRRecord812X = Record812Xata.ToList().FirstOrDefault();

                if (ITRRecord812X == null)
                    ITRRecord812X = new T_ITRRecords_8100_002A { CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, CCMS_HEADERID = (int)CommonHeaderFooter.ID, ModelName = Settings.ModelName, CalibrationDate1 = new DateTime(2000, 1, 1), CalibrationDate2 = new DateTime(2000, 1, 1) };

                ITRRecord812X.CalibrationDate1 = ITRRecord812X.CalibrationDate1 <= new DateTime(2000, 1, 1) ? DateTime.Now : ITRRecord812X.CalibrationDate1;
                ITRRecord812X.CalibrationDate2 = ITRRecord812X.CalibrationDate2 <= new DateTime(2000, 1, 1) ? DateTime.Now : ITRRecord812X.CalibrationDate2;

                string InsulationDetailsSql = "SELECT * FROM T_ITRRecords_8100_002A_InsRes_Test WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND CommonRowID='" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                var InsulationDetailsData = await _ITRRecords_8100_002A_InsRes_TestRepository.QueryAsync(InsulationDetailsSql);

                List<T_ITRRecords_8100_002A_InsRes_Test> InsRes_Testdata = InsulationDetailsData.ToList();

                if (InsRes_Testdata.Count <= 0)
                {
                    var InsRes_Test = await _ITRRecords_8100_002A_InsRes_TestRepository.GetAsync();
                    long InsResTestID = InsRes_Test.Count() + 1;
                    InsulationDetailsData = Enumerable.Range(0, 3).Select(x => new T_ITRRecords_8100_002A_InsRes_Test { ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, CCMS_HEADERID = (int)CommonHeaderFooter.ID, RowID = InsResTestID++ }).ToList();
                }
                int index1 = 0;
                foreach (T_ITRRecords_8100_002A_InsRes_Test item in InsulationDetailsData)
                {
                    if (index1 == 0)
                    {
                        item.InsHeading = "Primary Winding to Earth :";
                    }
                    if (index1 == 1)
                    {
                        item.InsHeading = "Secondary Winding to Earth :";
                    }
                    if (index1 == 2)
                    {
                        item.InsHeading = "Primary Winding to Secondary Winding :";
                    }
                    index1++;
                }
                ITR8100_002A_InsRes_Test = new ObservableCollection<T_ITRRecords_8100_002A_InsRes_Test>(InsulationDetailsData);

                string Radio_TestSql = "SELECT * FROM T_ITRRecords_8100_002A_Radio_Test WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND CommonRowID='" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                var Radio_Test = await _ITRRecords_8100_002A_Radio_TestRepository.QueryAsync(Radio_TestSql);
                List<T_ITRRecords_8100_002A_Radio_Test> Radio_Testdata = Radio_Test.ToList();

                if (Radio_Testdata.Count <= 0)
                {
                    var Radiotestrecords = await _ITRRecords_8100_002A_Radio_TestRepository.GetAsync();
                    long Radio_TestID = Radiotestrecords.Count() + 1;
                    Radio_Test = Enumerable.Range(0, 3).Select(x => new T_ITRRecords_8100_002A_Radio_Test { ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, CCMS_HEADERID = (int)CommonHeaderFooter.ID, RowID = Radio_TestID++ }).ToList();
                }
                int index = 0;
                foreach (T_ITRRecords_8100_002A_Radio_Test item in Radio_Test)
                {
                    if (index == 0)
                    {
                        item.InsHeading = "L1-L2";
                        item.IsEnabledL1N = true;
                        item.IsEnabledL2N = true;
                        item.IsEnabledL3N = false;
                        item.IsEnabledL1L2 = true;
                        item.IsEnabledL2L3 = false;
                        item.IsEnabledL3L1 = false;
                        item.IsEnabledL1NBG = "White";
                        item.IsEnabledL2NBG = "White";
                        item.IsEnabledL3NBG = "#CCCCCC";
                        item.IsEnabledL1L2BG = "White";
                        item.IsEnabledL2L3BG = "#CCCCCC";
                        item.IsEnabledL3L1BG = "#CCCCCC";
                    }
                    if (index == 1)
                    {
                        item.InsHeading = "L2-L3";
                        item.IsEnabledL1N = false;
                        item.IsEnabledL2N = true;
                        item.IsEnabledL3N = true;
                        item.IsEnabledL1L2 = false;
                        item.IsEnabledL2L3 = true;
                        item.IsEnabledL3L1 = false;
                        item.IsEnabledL1NBG = "#CCCCCC";
                        item.IsEnabledL2NBG = "White";
                        item.IsEnabledL3NBG = "White";
                        item.IsEnabledL1L2BG = "#CCCCCC";
                        item.IsEnabledL2L3BG = "White";
                        item.IsEnabledL3L1BG = "#CCCCCC";
                    }
                    if (index == 2)
                    {
                        item.InsHeading = "L3-L1";
                        item.IsEnabledL1N = true;
                        item.IsEnabledL2N = false;
                        item.IsEnabledL3N = true;
                        item.IsEnabledL1L2 = false;
                        item.IsEnabledL2L3 = false;
                        item.IsEnabledL3L1 = true;
                        item.IsEnabledL1NBG = "White";
                        item.IsEnabledL2NBG = "#CCCCCC";
                        item.IsEnabledL3NBG = "White";
                        item.IsEnabledL1L2BG = "#CCCCCC";
                        item.IsEnabledL2L3BG = "#CCCCCC";
                        item.IsEnabledL3L1BG = "White";
                    }
                    index++;
                }

                ITR8100_002A_Radio_Test = new ObservableCollection<T_ITRRecords_8100_002A_Radio_Test>(Radio_Test);
                BindIstrumentData();
            }
            else if (await _ITRService.ITR_8121_002A(SelectedCheckSheet.ITRNumber))
            {
                try
                {
                    SimpleITR = (SelectedCheckSheet.ITRNumber == "8121-002A") ? true : false;
                    string recordsqlRecords = " SELECT * FROM T_ITR8121_002A_Records WHERE TagNo='" + SelectedTag.name + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND ITRNumber='" + SelectedCheckSheet.ITRNumber + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var T_Record = await _ITR8121_002A_Records.QueryAsync(recordsqlRecords);
                    Records8121_002A = T_Record.FirstOrDefault();
                    if (Records8121_002A == null)
                    {
                        Records8121_002A = new T_ITR8121_002A_Records { CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, ITRNumber = SelectedCheckSheet.ITRNumber, TagNo = SelectedTag.name, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID };
                        await _ITR8121_002A_Records.InsertOrReplaceAsync(Records8121_002A);
                    }
                    ITR8121_002A_Record_ID = Records8121_002A.ID;
                    Records8121_002A.CalibrationDate1 = Records8121_002A.CalibrationDate1 <= new DateTime(2000, 1, 1) ? DateTime.Now : Records8121_002A.CalibrationDate1;
                    Records8121_002A.CalibrationDate2 = Records8121_002A.CalibrationDate2 <= new DateTime(2000, 1, 1) ? DateTime.Now : Records8121_002A.CalibrationDate2;
                    Records8121_002A.CalibrationDate3 = Records8121_002A.CalibrationDate3 <= new DateTime(2000, 1, 1) ? DateTime.Now : Records8121_002A.CalibrationDate3;

                    string recordsql1 = " SELECT * FROM T_ITR8121_002A_TransformerRadioTest WHERE ID_8121_002A_TransformerRadioTest ='" + Records8121_002A.ID + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var TransformerRadioTests = await _ITR8121_002A_TransformerRadioTest.QueryAsync(recordsql1);
                    if (TransformerRadioTests.Count <= 0)
                    {
                        var TransformerRadioTest = await _ITR8121_002A_TransformerRadioTest.GetAsync();
                        long TRTID = TransformerRadioTest.Count() + 1;
                        T_ITR8121_002A_TransformerRadioTest newItem = new T_ITR8121_002A_TransformerRadioTest { RowID = TRTID, CommonRowID = CommonHeaderFooter.ROWID, ID_8121_002A_TransformerRadioTest = Records8121_002A.ID, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID };
                        TransformerRadioTests.Add(newItem);
                        await _ITR8121_002A_TransformerRadioTest.InsertOrReplaceAsync(newItem);
                    }
                    TransformerRadioTests.Skip(1).ForEach(x => x.IsUpdated = true);
                    TransformerRadioTestList = new ObservableCollection<T_ITR8121_002A_TransformerRadioTest>(TransformerRadioTests);


                    string recordsql2 = " SELECT * FROM T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents WHERE ID_ITR_8121_002A_InspectionforControl='" + Records8121_002A.ID + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var T_InspectionforControls = await _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents.QueryAsync(recordsql2);
                    List<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents> InspectionforControlsList = T_InspectionforControls.ToList();
                    if (InspectionforControlsList.Count < 7)
                    {
                        var InsConAux = await _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents.GetAsync();
                        long InsConAuxID = InsConAux.Count() + 1;
                        int additem = 7 - InspectionforControlsList.Count;
                        if (additem > 0)
                            InspectionforControlsList.AddRange(Enumerable.Range(0, additem).Select(x => new T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents { ModelName = Settings.ModelName, ID_ITR_8121_002A_InspectionforControl = Records8121_002A.ID, CommonRowID = CommonHeaderFooter.ROWID, RowID = InsConAuxID++, CCMS_HEADERID = (int)CommonHeaderFooter.ID }).ToList());
                        await _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents.InsertOrReplaceAsync(InspectionforControlsList);
                    }
                    if (String.IsNullOrEmpty(InspectionforControlsList[0].DeviceName))
                        InspectionforControlsList[0].DeviceName = "OIL LEVEL LOW / HIGH";
                    if (String.IsNullOrEmpty(InspectionforControlsList[1].DeviceName))
                        InspectionforControlsList[1].DeviceName = "OIL TEMP. ALARM / TRIP";
                    if (String.IsNullOrEmpty(InspectionforControlsList[2].DeviceName))
                        InspectionforControlsList[2].DeviceName = "WINDING TEMP. ALARM / TRIP";
                    if (String.IsNullOrEmpty(InspectionforControlsList[3].DeviceName))
                        InspectionforControlsList[3].DeviceName = "PRESSURE RELIEF DEVICE TRIP";
                    if (String.IsNullOrEmpty(InspectionforControlsList[4].DeviceName))
                        InspectionforControlsList[4].DeviceName = "BUCHHOLZ RELAY ALARM / TRIP";
                    if (String.IsNullOrEmpty(InspectionforControlsList[5].DeviceName))
                        InspectionforControlsList[5].DeviceName = "MARSHALLING BOX SPACE HEATER";
                    if (String.IsNullOrEmpty(InspectionforControlsList[6].DeviceName))
                        InspectionforControlsList[6].DeviceName = "MARSHALLING BOX LIGHTING";
                    int i = 1;
                    InspectionforControlsList.ForEach(x => { x.No = i++; x.IsUpdated = false; });
                    if (InspectionforControlsList.Count > 7)
                        InspectionforControlsList.Skip(7).ForEach(x => x.IsUpdated = true);
                    InspectionforControls = new ObservableCollection<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents>(InspectionforControlsList);
                    BindIstrumentData();
                }
                catch (Exception Ex)
                {

                }
            }
            else if (await _ITRService.ITR_8260_002A(SelectedCheckSheet.ITRNumber))
            {
                string Bodysqlquery = " SELECT * FROM T_ITR_8260_002A_Body WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                var T_Body = await _ITR_8260_002A_BodyRepository.QueryAsync(Bodysqlquery);
                if (T_Body.Count <= 0)
                    T_Body.Add(new T_ITR_8260_002A_Body());
                Body8260_002A = T_Body.FirstOrDefault();
                Body8260_002A.CalibrationDate1 = Body8260_002A.CalibrationDate1 <= new DateTime(2000, 1, 1) ? DateTime.Now : Body8260_002A.CalibrationDate1;
                Body8260_002A.CalibrationDate2 = Body8260_002A.CalibrationDate2 <= new DateTime(2000, 1, 1) ? DateTime.Now : Body8260_002A.CalibrationDate2;
                string DielectricTestsqlquery = " SELECT * FROM T_ITR_8260_002A_DielectricTest WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                var T_DielectricTest = await _ITR_8260_002A_DielectricTestRepository.QueryAsync(DielectricTestsqlquery);

                List<T_ITR_8260_002A_DielectricTest> DieEleTestRecords = T_DielectricTest.ToList();

                var DielectricTest = await _ITR_8260_002A_DielectricTestRepository.GetAsync();
                long DiTestID = DielectricTest.Count() + 1;

                string Type = string.Empty;
                string Size = string.Empty;
                if (!String.IsNullOrEmpty(Body8260_002A.CableType))
                {
                    List<string> ArrStr = Body8260_002A.CableType.Split('-').ToList();
                    Size = ArrStr[0];
                    Type = ArrStr[1];
                }

                //int additem = 3 - DielectricTestRecords.Count();
                if (DieEleTestRecords.Count() <= 0)
                    DieEleTestRecords.AddRange(Enumerable.Range(0, 3).Select(x => new T_ITR_8260_002A_DielectricTest { CommonRowID = CommonHeaderFooter.ROWID, RowID = DiTestID++, ITRCommonID = CommonHeaderFooter.ID, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID }).ToList());

                if (DieEleTestRecords.Count() > 0)
                {
                    DieEleTestRecords[0].TestPhase = "L1-E";
                    DieEleTestRecords[0].IsUpdated = false;
                }
                if (DieEleTestRecords.Count() > 1)
                {
                    DieEleTestRecords[1].TestPhase = "L2-E";
                    if (Size == "1" && Type.ToUpper() == "C")
                        DieEleTestRecords[1].IsUpdated = true;
                    else
                        DieEleTestRecords[1].IsUpdated = false;
                }
                if (DieEleTestRecords.Count() > 2)
                {
                    DieEleTestRecords[2].TestPhase = "L3-E";
                    if (Size == "1" && Type.ToUpper() == "C")
                        DieEleTestRecords[2].IsUpdated = true;
                    else
                        DieEleTestRecords[2].IsUpdated = false;
                }
                DielectricTestRecords = new ObservableCollection<T_ITR_8260_002A_DielectricTest>(DieEleTestRecords);
                Die8260_002AHeight = DielectricTestRecords.Count() * 50;
                BindIstrumentData();
            }
            else if (await _ITRService.ITR_8161_1XA(SelectedCheckSheet.ITRNumber))
            {
                if (SelectedCheckSheet.ITRNumber == "8161-001A")
                {
                    IsStanderdITR = false;
                    IsSimpleITR = true;
                    InsResTestTitle = "11. Insulation Resistance Test";
                    RegTestTitle = "12. Contact Resistance Test";
                    AcceptCriteria = "Acceptance criteria: V-2181-619-A-MAN-2004 for 6.9kV bus duct, V-2181-619-A-MAN-2005 for LV bus duct";
                }
                else
                {
                    IsStanderdITR = true;
                    IsSimpleITR = false;
                    InsResTestTitle = "12. Insulation Resistance Test";
                    RegTestTitle = "13. Contact Resistance Test";
                    AcceptCriteria = "Acceptance criteria: Highest resistance value - 20% x Highest resistance value &lt; Lowest resistance value";
                }

                string recordsql = " SELECT * FROM T_ITRRecords_8161_001A_Body WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND ModelName = '" + Settings.ModelName + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                var ITR_8161_1XA = await _ITRRecords_8161_001A_BodyRepository.QueryAsync(recordsql);
                ITRRecord8161_1XA = ITR_8161_1XA.ToList().FirstOrDefault();

                if (ITRRecord8161_1XA == null)
                    ITRRecord8161_1XA = new T_ITRRecords_8161_001A_Body { CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, ModelName = Settings.ModelName, CalibrationDate1 = new DateTime(2000, 1, 1), CalibrationDate2 = new DateTime(2000, 1, 1), CCMS_HEADERID = (int)CommonHeaderFooter.ID };

                ITRRecord8161_1XA.CalibrationDate1 = ITRRecord8161_1XA.CalibrationDate1 <= new DateTime(2000, 1, 1) ? DateTime.Now : ITRRecord8161_1XA.CalibrationDate1;
                ITRRecord8161_1XA.CalibrationDate2 = ITRRecord8161_1XA.CalibrationDate2 <= new DateTime(2000, 1, 1) ? DateTime.Now : ITRRecord8161_1XA.CalibrationDate2;

                string InsulationDetailsSql = "SELECT * FROM T_ITRRecords_8161_001A_InsRes WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND ModelName = '" + Settings.ModelName + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                var InsulationDetailsData = await _ITRRecords_8161_001A_InsResRepository.QueryAsync(InsulationDetailsSql);
                List<T_ITRRecords_8161_001A_InsRes> InsDetailsData = InsulationDetailsData.ToList();
                if (InsulationDetailsData.Count <= 0)
                {
                    var records = await _ITRRecords_8161_001A_InsResRepository.GetAsync();
                    long InsResID = records.Count() + 1;
                    InsulationDetailsData = Enumerable.Range(0, 3).Select(x => new T_ITRRecords_8161_001A_InsRes { ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, RowID = InsResID++, ITRCommonID = CommonHeaderFooter.ID, CCMS_HEADERID = (int)CommonHeaderFooter.ID }).ToList();
                }

                int count = 1;
                foreach (T_ITRRecords_8161_001A_InsRes initem in InsulationDetailsData)
                {
                    if (count == 1)
                    {
                        initem.TestPhase1 = "L1-E";
                        initem.TestPhase2 = "L1-L2";
                        initem.TestPhase3 = "L1-N";
                    }
                    if (count == 2)
                    {
                        initem.TestPhase1 = "L2-E";
                        initem.TestPhase2 = "L2-L3";
                        initem.TestPhase3 = "L2-N";
                    }
                    if (count == 3)
                    {
                        initem.TestPhase1 = "L3-E";
                        initem.TestPhase2 = "L3-L1";
                        initem.TestPhase3 = "L3-N";
                    }
                    count++;
                }

                ITR_8161_001A_InsRes = new ObservableCollection<T_ITRRecords_8161_001A_InsRes>(InsulationDetailsData);

                string Radio_TestSql = "SELECT * FROM T_ITRRecords_8161_001A_ConRes WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND ModelName = '" + Settings.ModelName + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                var Radio_Test = await _ITRRecords_8161_001A_ConResRepository.QueryAsync(Radio_TestSql);
                List<T_ITRRecords_8161_001A_ConRes> ConResData = Radio_Test.ToList();
                if (ConResData.Count <= 0)
                {
                    var ConResrecords = await _ITRRecords_8161_001A_ConResRepository.GetAsync();
                    long ConResID = ConResrecords.Count() + 1;
                    Radio_Test.Add(new T_ITRRecords_8161_001A_ConRes { ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, RowID = ConResID, ITRCommonID = CommonHeaderFooter.ID, CCMS_HEADERID = (int)CommonHeaderFooter.ID });
                    await _ITRRecords_8161_001A_ConResRepository.InsertOrReplaceAsync(Radio_Test);
                }
                var num = 1;
                Radio_Test.ForEach(x => x.SlNo = num++);
                Radio_Test.Skip(1).ForEach(x => x.IsUpdated = true);
                ITR_8161_001A_ConRes = new ObservableCollection<T_ITRRecords_8161_001A_ConRes>(Radio_Test);
                BindIstrumentData();
            }
            else if (await _ITRService.ITR_8121_004XA(SelectedCheckSheet.ITRNumber))
            {
                try
                {
                    SimpleITR = (SelectedCheckSheet.ITRNumber == "8121-004A") ? true : false;
                    string recordsql = " SELECT * FROM T_ITR8121_004ATestInstrumentData WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var TIRecorddata = await _ITR8121_004ATestInstrumentDataRepository.QueryAsync(recordsql);
                    TestInstrumentData8121_004A = TIRecorddata.FirstOrDefault();
                    if (TestInstrumentData8121_004A == null)
                    {
                        TestInstrumentData8121_004A = new T_ITR8121_004ATestInstrumentData
                        {
                            CommonRowID = CommonHeaderFooter.ROWID,
                            ITRCommonID = CommonHeaderFooter.ID,
                            ModelName = Settings.ModelName,
                            CCMS_HEADERID = (int)CommonHeaderFooter.ID
                        };
                        await _ITR8121_004ATestInstrumentDataRepository.InsertOrReplaceAsync(TestInstrumentData8121_004A);
                    }
                    TestInstrumentData8121_004A.CalibrationDate = TestInstrumentData8121_004A.CalibrationDate.HasValue ? ((DateTime)TestInstrumentData8121_004A.CalibrationDate > new DateTime(2000, 1, 1) ? (DateTime)TestInstrumentData8121_004A.CalibrationDate : DateTime.Now) : DateTime.Now;
                    TestInstrumentData8121_004A.CalibrationDate1 = TestInstrumentData8121_004A.CalibrationDate1.HasValue ? ((DateTime)TestInstrumentData8121_004A.CalibrationDate1 > new DateTime(2000, 1, 1) ? (DateTime)TestInstrumentData8121_004A.CalibrationDate1 : DateTime.Now) : DateTime.Now;
                    TestInstrumentData8121_004A.CalibrationDate2 = TestInstrumentData8121_004A.CalibrationDate2.HasValue ? ((DateTime)TestInstrumentData8121_004A.CalibrationDate2 > new DateTime(2000, 1, 1) ? (DateTime)TestInstrumentData8121_004A.CalibrationDate2 : DateTime.Now) : DateTime.Now;

                    string InCAndAuxiliarysql = " SELECT * FROM T_ITR8121_004AInCAndAuxiliary WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var InCAndAuxiliaryData = await _ITR8121_004AInCAndAuxiliaryRepository.QueryAsync(InCAndAuxiliarysql);
                    if (InCAndAuxiliaryData.Count < 3)
                    {
                        var ControlAndAuxiliary = await _ITR8121_004AInCAndAuxiliaryRepository.GetAsync();
                        long ConAuxID = ControlAndAuxiliary.Count() + 1;
                        int additem = 3 - InCAndAuxiliaryData.Count;
                        InCAndAuxiliaryData = Enumerable.Range(0, additem).Select(x => new T_ITR8121_004AInCAndAuxiliary { ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, RowID = ConAuxID++, ITRCommonID = CommonHeaderFooter.ID, CCMS_HEADERID = (int)CommonHeaderFooter.ID }).ToList();
                        await _ITR8121_004AInCAndAuxiliaryRepository.InsertOrReplaceAsync(InCAndAuxiliaryData);
                    }
                    if (String.IsNullOrEmpty(InCAndAuxiliaryData[0].DeviceName))
                        InCAndAuxiliaryData[0].DeviceName = "WINDING TEMP. ALARM / TRIP";
                    if (String.IsNullOrEmpty(InCAndAuxiliaryData[1].DeviceName))
                        InCAndAuxiliaryData[1].DeviceName = "MARSHALLING BOX SPACE HEATER";
                    if (String.IsNullOrEmpty(InCAndAuxiliaryData[2].DeviceName))
                        InCAndAuxiliaryData[2].DeviceName = "MARSHALLING BOX LIGHTING";
                    int i = 1;
                    InCAndAuxiliaryData.ForEach(x => { x.SrNo = i++; x.IsUpdated = false; });
                    if (InCAndAuxiliaryData.Count > 3)
                        InCAndAuxiliaryData.Skip(3).ForEach(x => x.IsUpdated = true);

                    InispactionForControlAndAuxiliary8121 = new ObservableCollection<T_ITR8121_004AInCAndAuxiliary>(InCAndAuxiliaryData);


                    string TransformerRatioTestsql = " SELECT * FROM T_ITR8121_004ATransformerRatioTest WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var TransformerRatioTestData = await _ITR8121_004ATransformerRatioTestRepository.QueryAsync(TransformerRatioTestsql);
                    if (TransformerRatioTestData.Count <= 0)
                    {
                        var TransRatioTest = await _ITR8121_004ATransformerRatioTestRepository.GetAsync();
                        long TransRatioTestID = TransRatioTest.Count() + 1;
                        TransformerRatioTestData.Add(new T_ITR8121_004ATransformerRatioTest { ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, RowID = TransRatioTestID, ITRCommonID = CommonHeaderFooter.ID, CCMS_HEADERID = (int)CommonHeaderFooter.ID });
                        await _ITR8121_004ATransformerRatioTestRepository.InsertOrReplaceAsync(TransformerRatioTestData);
                    }
                    if (TransformerRatioTestData.Count > 1)
                        TransformerRatioTestData.Skip(1).ForEach(x => x.IsUpdated = true);

                    TransformerRatioTest8121 = new ObservableCollection<T_ITR8121_004ATransformerRatioTest>(TransformerRatioTestData);
                    BindIstrumentData();
                }
                catch (Exception ex)
                {
                }
            }
            else if (await _ITRService.ITR_8161_2XA(SelectedCheckSheet.ITRNumber))
            {
                try
                {
                    string recordsql = " SELECT * FROM T_ITR8161_002A_Body WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND ModelName = '" + Settings.ModelName + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var ITR_8161_2XA = await _ITR8161_002A_BodyRepository.QueryAsync(recordsql);
                    if (ITR_8161_2XA.Count > 0)
                        ITRRecord8161_2XA = ITR_8161_2XA.ToList().FirstOrDefault();
                    else
                        ITRRecord8161_2XA = new T_ITR8161_002A_Body { CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, ModelName = Settings.ModelName, CalibrationDate1 = new DateTime(2000, 1, 1), CalibrationDate2 = new DateTime(2000, 1, 1), CCMS_HEADERID = (int)CommonHeaderFooter.ID };

                    ITRRecord8161_2XA.CalibrationDate1 = ITRRecord8161_2XA.CalibrationDate1 <= new DateTime(2000, 1, 1) ? DateTime.Now : ITRRecord8161_2XA.CalibrationDate1;
                    ITRRecord8161_2XA.CalibrationDate2 = ITRRecord8161_2XA.CalibrationDate2 <= new DateTime(2000, 1, 1) ? DateTime.Now : ITRRecord8161_2XA.CalibrationDate2;

                    string DielectricTestSql = "SELECT * FROM T_ITR8161_002A_DielectricTest WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND ModelName = '" + Settings.ModelName + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var DielectricTest = await _ITR8161_002A_DielectricTestRepository.QueryAsync(DielectricTestSql);
                    ITR_8161_002A_DielectricTest = DielectricTest.ToList(); ;

                    var DielectricTestList = await _ITR8161_002A_DielectricTestRepository.GetAsync();
                    long DiTestID = DielectricTestList.Count() + 1;

                    int additem = 0;
                    if (ITR_8161_002A_DielectricTest != null)
                        additem = 3 - ITR_8161_002A_DielectricTest.Count();
                    if (additem > 0)
                        ITR_8161_002A_DielectricTest.AddRange(Enumerable.Range(0, additem).Select(x => new T_ITR8161_002A_DielectricTest { CommonRowID = CommonHeaderFooter.ROWID, RowID = DiTestID++, ITRCommonID = CommonHeaderFooter.ID, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID }).ToList());

                    if (ITR_8161_002A_DielectricTest.Count() > 0)
                    {
                        var T_Record = ITR_8161_002A_DielectricTest[0];
                        InsRes1_1 = T_Record.InsRes1;
                        InsRes2_1 = T_Record.InsRes2;
                        AppliedPoint_1 = T_Record.AppliedPoint;
                        ChargeCurrent_1 = T_Record.ChargeCurrent;
                    }
                    if (ITR_8161_002A_DielectricTest.Count() > 1)
                    {
                        var T_Record = ITR_8161_002A_DielectricTest[1];
                        InsRes1_2 = T_Record.InsRes1;
                        InsRes2_2 = T_Record.InsRes2;
                        AppliedPoint_2 = T_Record.AppliedPoint;
                        ChargeCurrent_2 = T_Record.ChargeCurrent;
                    }
                    if (ITR_8161_002A_DielectricTest.Count() > 2)
                    {
                        var T_Record = ITR_8161_002A_DielectricTest[2];
                        InsRes1_3 = T_Record.InsRes1;
                        InsRes2_3 = T_Record.InsRes2;
                        AppliedPoint_3 = T_Record.AppliedPoint;
                        ChargeCurrent_3 = T_Record.ChargeCurrent;
                    }

                    BindIstrumentData();
                }
                catch (Exception ex)
                {
                }
            }
            else if (await _ITRService.ITR_8000_101A(SelectedCheckSheet.ITRNumber))
            {
                try
                {
                    string Generalnformationsql = " SELECT * FROM T_ITR8000_101A_Generalnformation WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND ModelName = '" + Settings.ModelName + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var ITR_8000_101AGeneralnformation = await _ITR8000_101A_GeneralnformationRepository.QueryAsync(Generalnformationsql);
                    if (ITR_8000_101AGeneralnformation.Count > 0)
                        ITR8000_101AGenlnfo = ITR_8000_101AGeneralnformation.ToList().FirstOrDefault();
                    else
                        ITR8000_101AGenlnfo = new T_ITR8000_101A_Generalnformation
                        {
                            CommonRowID = CommonHeaderFooter.ROWID,
                            ITRCommonID = CommonHeaderFooter.ID,
                            ModelName = Settings.ModelName,
                            CCMS_HEADERID = (int)CommonHeaderFooter.ID,
                            ITRNumber = CommonHeaderFooter.ITRNumber,
                            TagNo = CommonHeaderFooter.Tag,
                            SerialNo = CommonHeaderFooter.Tag
                        };


                    string BarrierDetailsSql = "SELECT * FROM T_ITR8000_101A_RecordISBarrierDetails WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND ModelName = '" + Settings.ModelName + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var ITR_8000_101ABarrierDetails = await _ITR8000_101A_RecordISBarrierDetailsRepository.QueryAsync(BarrierDetailsSql);
                    if (ITR_8000_101ABarrierDetails.Count > 0)
                        ITR8000_101AISBarDetails = ITR_8000_101ABarrierDetails.ToList().FirstOrDefault();
                    else
                        ITR8000_101AISBarDetails = new T_ITR8000_101A_RecordISBarrierDetails { CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID, ITRNumber = CommonHeaderFooter.ITRNumber };

                }
                catch (Exception ex)
                {
                }
            }
            else if (await _ITRService.ITR_8211_002A(SelectedCheckSheet.ITRNumber))
            {
                try
                {
                    if (SelectedCheckSheet.ITRNumber == "8211-002A")
                    {
                        IsSimpleITR = true;
                        IsStanderdITR = false;

                        Lbl1 = "90";
                        Lbl2 = "120";
                        Lbl3 = "180";
                        Lbl4 = "240";
                    }
                    else
                    {
                        IsSimpleITR = false;
                        IsStanderdITR = true;

                        Lbl1 = "75";
                        Lbl2 = "90";
                        Lbl3 = "105";
                        Lbl4 = "120";
                    }
                    string recordsql = " SELECT * FROM T_ITRRecords_8211_002A_Body WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND CommonRowID='" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var ITR_8211_2XA00 = await _ITRRecords_8211_002A_BodyRepository.QueryAsync(recordsql);
                    if (ITR_8211_2XA00.Count <= 0)
                        ITR_8211_2XA00.Add(new T_ITRRecords_8211_002A_Body { CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID });

                    ITRRecord8211_002A = ITR_8211_2XA00.ToList().FirstOrDefault();
                    if (ITRRecord8211_002A.Inch == "CW")
                        ITRRecord8211_002A.IsCW = true;
                    else if (ITRRecord8211_002A.Inch == "CCW")
                        ITRRecord8211_002A.IsCCW = true;
                    else
                        ITRRecord8211_002A.IsCW = ITRRecord8211_002A.IsCCW = false;

                    //Added Function Test option list 
                    List<string> functionTestOptionList = new List<string> {"Pass","Fail"};
                    if (!String.IsNullOrEmpty(ITRRecord8211_002A.FuncionTest) && !functionTestOptionList.Contains(ITRRecord8211_002A.FuncionTest))
                        functionTestOptionList.Add(ITRRecord8211_002A.FuncionTest);
                    ITRRecord8211_002A.FuncionTestOptionsList = functionTestOptionList;

                    //Added Direction Of Rotation option list 
                    List<string> directionOfRotationOptionsList = new List<string> { "Pass", "Fail" };
                    if (!String.IsNullOrEmpty(ITRRecord8211_002A.DirectionOfRotation) && !directionOfRotationOptionsList.Contains(ITRRecord8211_002A.DirectionOfRotation))
                        directionOfRotationOptionsList.Add(ITRRecord8211_002A.DirectionOfRotation);
                    ITRRecord8211_002A.DirectionOfRotationOptionsList = directionOfRotationOptionsList;

                    //Added Space Heater Circuit option list 
                    List<string> spaceHeaterOptionsList = new List<string> { "Pass", "Fail" };
                    if (!String.IsNullOrEmpty(ITRRecord8211_002A.SpaceHeater) && !spaceHeaterOptionsList.Contains(ITRRecord8211_002A.SpaceHeater))
                        spaceHeaterOptionsList.Add(ITRRecord8211_002A.SpaceHeater);
                    ITRRecord8211_002A.SpaceHeaterOptionsList = spaceHeaterOptionsList;

                    ITRRecord8211_002A.IsReqInch = "LightGray";

                    string RunTest_TestSql = "SELECT * FROM T_ITRRecords_8211_002A_RunTest WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND ModelName = '" + Settings.ModelName + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var RunTestData = await _ITRRecords_8211_002A_RunTestRepository.QueryAsync(RunTest_TestSql);

                    List<T_ITRRecords_8211_002A_RunTest> RunTestList = RunTestData.ToList();
                    var RunTests = await _ITRRecords_8211_002A_RunTestRepository.GetAsync();
                    long RunTestID = RunTests.Count() + 1;
                    int AddItem = 10 - RunTestList.Count;
                    if (AddItem > 0)
                    {
                        RunTestList.AddRange(Enumerable.Range(0, AddItem).Select(x => new T_ITRRecords_8211_002A_RunTest { ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, RowID = RunTestID++, ITRCommonID = CommonHeaderFooter.ID, CCMS_HEADERID = (int)CommonHeaderFooter.ID }).ToList());
                        await _ITRRecords_8211_002A_RunTestRepository.InsertOrReplaceAsync(RunTestList);
                    }
                    ITR_8211_002A_RunTest = RunTestList;
                    BindIstrumentData();
                }
                catch (Exception ex)
                {
                }
            }
            else if (await _ITRService.ITR_7000_101AHarmony(SelectedCheckSheet.ITRNumber))
            {
                try
                {
                    string Generalnformationsql = " SELECT * FROM T_ITR_7000_101AHarmony_Genlnfo WHERE ITRNumber='" + CommonHeaderFooter.ITRNumber + "' AND ModelName = '" + Settings.ModelName + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var ITR_8000_101AGeneralnformation = await _ITR7000_101AHarmony_GenlnfoRepository.QueryAsync(Generalnformationsql);

                    if (ITR_8000_101AGeneralnformation.Count > 0)
                        ITR7000_101AHarmonyGenlnfo = ITR_8000_101AGeneralnformation.ToList().FirstOrDefault();
                    else
                        ITR7000_101AHarmonyGenlnfo = new T_ITR_7000_101AHarmony_Genlnfo
                        {
                            CommonRowID = CommonHeaderFooter.ROWID,
                            ModelName = Settings.ModelName,
                            CCMS_HEADERID = (int)CommonHeaderFooter.ID,
                            ITRNumber = CommonHeaderFooter.ITRNumber,
                            TagNo = CommonHeaderFooter.Tag
                        };
                    if (SelectedCheckSheet.ITRNumber == "7000-101A")
                    {
                        ITR7000_101AHarmonyGenlnfo.IsBarrierManufacturerReadOnly = true;
                        ITR7000_101AHarmonyGenlnfo.IsBarrierModelReadOnly = true;
                        ITR7000_101AHarmonyGenlnfo.IsBarrierCertificateNoReadOnly = true;
                        ITR7000_101AHarmonyGenlnfo.IsBarrierLocationReadOnly = true;
                        ITR7000_101AHarmonyGenlnfo.IsBarrierCabinetNoReadOnly = true;
                    }
                    else
                    {
                        ITR7000_101AHarmonyGenlnfo.IsBarrierManufacturerReadOnly = false;
                        ITR7000_101AHarmonyGenlnfo.IsBarrierModelReadOnly = false;
                        ITR7000_101AHarmonyGenlnfo.IsBarrierCertificateNoReadOnly = false;
                        ITR7000_101AHarmonyGenlnfo.IsBarrierLocationReadOnly = false;
                        ITR7000_101AHarmonyGenlnfo.IsBarrierCabinetNoReadOnly = false;
                    }
                    string BarrierDetailsSql = "SELECT * FROM T_ITR_7000_101AHarmony_ActivityDetails WHERE ModelName = '" + Settings.ModelName + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var ITR_8000_101ABarrierDetails = await _ITR7000_101AHarmony_ActivityDetailsRepository.QueryAsync(BarrierDetailsSql);
                    if (ITR_8000_101ABarrierDetails.Count > 0)
                        ITR7000_101AHarmony_ActivityDetails = ITR_8000_101ABarrierDetails.ToList().FirstOrDefault();
                    else
                        ITR7000_101AHarmony_ActivityDetails = new T_ITR_7000_101AHarmony_ActivityDetails { CommonRowID = CommonHeaderFooter.ROWID, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID };

                }
                catch (Exception ex)
                {
                    { }
                }
            }
            else if (await _ITRService.ITR_8140_002A(SelectedCheckSheet.ITRNumber))
            {
                if (SelectedCheckSheet.ITRNumber == "8140-002A-Standard")
                {
                    IsStanderdITR = true;
                    IsSimpleITR = false;
                }
                else if (SelectedCheckSheet.ITRNumber == "8140-002A")
                {
                    IsStanderdITR = false;
                    IsSimpleITR = true;
                }
                try
                {
                    string Recordsql = " SELECT * FROM T_ITR_8140_002A_Records WHERE ITRNumber='" + SelectedCheckSheet.ITRNumber + "' AND ModelName = '" + Settings.ModelName + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var T_ITR8140_002ARecord = await _ITR_8140_002A_RecordsRepository.QueryAsync(Recordsql);

                    if (T_ITR8140_002ARecord.Count > 0)
                        ITR8140_002ARecords = T_ITR8140_002ARecord.ToList().FirstOrDefault();
                    else
                    {
                        ITR8140_002ARecords = new T_ITR_8140_002A_Records();
                        ITR8140_002ARecords.CommonRowID = CommonHeaderFooter.ROWID;
                        ITR8140_002ARecords.ModelName = Settings.ModelName;
                        ITR8140_002ARecords.CCMS_HEADERID = (int)CommonHeaderFooter.ID;
                        ITR8140_002ARecords.ITRNumber = CommonHeaderFooter.ITRNumber;
                        ITR8140_002ARecords.TagNo = CommonHeaderFooter.Tag;
                        if (string.IsNullOrEmpty(ITR8140_002ARecords.ESCloseECRemark)) ITR8140_002ARecords.ESCloseECRemark = "Applicable to GIS Only";
                        if (string.IsNullOrEmpty(ITR8140_002ARecords.ESOpenECRemark)) ITR8140_002ARecords.ESOpenECRemark = "Applicable to GIS Only";
                        if (string.IsNullOrEmpty(ITR8140_002ARecords.DSCloseECRemark)) ITR8140_002ARecords.DSCloseECRemark = "Applicable to GIS Only";
                        if (string.IsNullOrEmpty(ITR8140_002ARecords.DSOpenECRemark)) ITR8140_002ARecords.DSOpenECRemark = "Applicable to GIS Only";
                        ITR8140_002ARecords = ITR8140_002ARecords;
                    }

                    string RecordMOsql = " SELECT * FROM T_ITR_8140_002A_RecordsMechnicalOperation WHERE ITRNumber='" + SelectedCheckSheet.ITRNumber + "' AND ModelName = '" + Settings.ModelName + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var T_ITR8140_002AMORecord = await _ITR_8140_002A_RecordsMechnicalOperation_RecordsRepository.QueryAsync(RecordMOsql);

                    if (T_ITR8140_002AMORecord.Count > 0)
                        ITR8140_002ARecordsMO = T_ITR8140_002AMORecord.ToList().FirstOrDefault();
                    else
                    {
                        ITR8140_002ARecordsMO = new T_ITR_8140_002A_RecordsMechnicalOperation();
                        ITR8140_002ARecordsMO.CommonRowID = CommonHeaderFooter.ROWID;
                        ITR8140_002ARecordsMO.ModelName = Settings.ModelName;
                        ITR8140_002ARecordsMO.CCMS_HEADERID = (int)CommonHeaderFooter.ID;
                        ITR8140_002ARecordsMO.ITRNumber = CommonHeaderFooter.ITRNumber;
                        ITR8140_002ARecordsMO.TagNo = CommonHeaderFooter.Tag;
                        if (string.IsNullOrEmpty(ITR8140_002ARecordsMO.DSIndicationCloseRemarks)) ITR8140_002ARecordsMO.DSIndicationCloseRemarks = "Applicable to GIS Only";
                        if (string.IsNullOrEmpty(ITR8140_002ARecordsMO.DSIndicationOpenRemarks)) ITR8140_002ARecordsMO.DSIndicationOpenRemarks = "Applicable to GIS Only";
                        if (string.IsNullOrEmpty(ITR8140_002ARecordsMO.IESDSCBCCondustorRemark)) ITR8140_002ARecordsMO.IESDSCBCCondustorRemark = "DS is Applicable to GIS only.";
                        ITR8140_002ARecordsMO = ITR8140_002ARecordsMO;
                    }
                    string RecordAnasql = " SELECT * FROM T_ITR_8140_002A_RecordsAnalogSignal WHERE ITRNumber='" + SelectedCheckSheet.ITRNumber + "' AND ModelName = '" + Settings.ModelName + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var T_ITR8140_002AAnaRecord = await _ITR_8140_002A_RecordsAnalogSignalRepository.QueryAsync(RecordAnasql);

                    if (T_ITR8140_002AAnaRecord.Count > 0)
                        ITR8140_002ARecordsAS = T_ITR8140_002AAnaRecord.ToList().FirstOrDefault();
                    else
                        ITR8140_002ARecordsAS = new T_ITR_8140_002A_RecordsAnalogSignal { CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = (int)CommonHeaderFooter.ID, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID, ITRNumber = SelectedCheckSheet.ITRNumber, TagNo = CommonHeaderFooter.Tag };


                    BindIstrumentData();
                }
                catch (Exception ex)
                {
                }
            }
            else if (await _ITRService.ITR_8140_004A(SelectedCheckSheet.ITRNumber))
            {
                if (SelectedCheckSheet.ITRNumber == "8140-004A-Standard")
                {
                    IsStanderdITR = true;
                    IsSimpleITR = false;
                }
                else if (SelectedCheckSheet.ITRNumber == "8140-004A")
                {
                    IsStanderdITR = false;
                    IsSimpleITR = true;
                }
                try
                {
                    string Recordsql = " SELECT * FROM T_ITR_8140_004A_Records WHERE ITRNumber='" + SelectedCheckSheet.ITRNumber + "' AND ModelName = '" + Settings.ModelName + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var T_ITR8140_004ARecord = await _ITR_8140_004A_RecordsRepository.QueryAsync(Recordsql);

                    if (T_ITR8140_004ARecord.Count > 0)
                        ITR8140_004ARecords = T_ITR8140_004ARecord.ToList().FirstOrDefault();
                    else
                        ITR8140_004ARecords = new T_ITR_8140_004A_Records { CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, ModelName = Settings.ModelName, InstrumentCalibrationDate = new DateTime(2000, 1, 1), CCMS_HEADERID = (int)CommonHeaderFooter.ID };
                    ITR8140_004ARecords.InstrumentCalibrationDate = ITR8140_004ARecords.InstrumentCalibrationDate <= new DateTime(2000, 1, 1) ? DateTime.Now : ITR8140_004ARecords.InstrumentCalibrationDate;

                    BindIstrumentData();
                }
                catch (Exception ex)
                {
                }



            }
            else if (await _ITRService.ITR_8170_002A(SelectedCheckSheet.ITRNumber))
            {
                try
                {
                    string recordsql = " SELECT * FROM T_ITR_8170_002A_InsRes WHERE CommonRowID='" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var ITR_8170_002A = await _ITR_8170_002A_InsResRepository.QueryAsync(recordsql);
                    if (ITR_8170_002A.Count <= 0)
                    {
                        ITR_8170_002A_InsRes = new T_ITR_8170_002A_InsRes
                        {
                            CommonRowID = CommonHeaderFooter.ROWID,
                            ModelName = Settings.ModelName,
                            CCMS_HEADERID = (int)CommonHeaderFooter.ID
                        };
                    }
                    else
                        ITR_8170_002A_InsRes = ITR_8170_002A.ToList().FirstOrDefault();

                    string recordsqlIL = " SELECT * FROM T_ITR_8170_002A_IndifictionLamp WHERE CommonRowID='" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var ITR_8170_002AIL = await _ITR_8170_002A_IndifictionLampRepository.QueryAsync(recordsqlIL);
                    if (ITR_8170_002AIL.Count <= 0)
                    {
                        ITR_8170_002A_IndifictionLamp = new T_ITR_8170_002A_IndifictionLamp
                        {
                            CommonRowID = CommonHeaderFooter.ROWID,
                            ModelName = Settings.ModelName,
                            CCMS_HEADERID = (int)CommonHeaderFooter.ID
                        };
                    }
                    else
                        ITR_8170_002A_IndifictionLamp = ITR_8170_002AIL.ToList().FirstOrDefault();

                    string InsulationDetailsSql = "SELECT * FROM T_ITRRecords_8170_002A_Voltage_Reading WHERE CommonRowID='" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var InsulationDetailsData = await _ITRRecords_8170_002A_Voltage_ReadingRepository.QueryAsync(InsulationDetailsSql);

                    List<T_ITRRecords_8170_002A_Voltage_Reading> RunTestList = InsulationDetailsData.ToList();
                    var RunTests = await _ITRRecords_8170_002A_Voltage_ReadingRepository.GetAsync();
                    long RunTestID = RunTests.Count() + 1;
                    int AddItem = 2 - RunTestList.Count;
                    if (AddItem > 0)
                    {
                        RunTestList.AddRange(Enumerable.Range(0, AddItem).Select(x => new T_ITRRecords_8170_002A_Voltage_Reading { ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, RowID = RunTestID++, CCMS_HEADERID = (int)CommonHeaderFooter.ID }).ToList());
                        await _ITRRecords_8170_002A_Voltage_ReadingRepository.InsertOrReplaceAsync(RunTestList);
                    }

                    ITR8170_002A_Voltage_ReadingList = new ObservableCollection<T_ITRRecords_8170_002A_Voltage_Reading>(RunTestList);

                    BindIstrumentData();
                }
                catch (Exception ex)
                {
                }
            }
            else if (await _ITRService.ITR_8300_003A(SelectedCheckSheet.ITRNumber))
            {
                try
                {
                    string recordsql = " SELECT * FROM T_ITR_8300_003A_Body WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND CommonRowID='" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var ITR_8300_003A = await _ITR_8300_003A_BodyRepository.QueryAsync(recordsql);
                    if (ITR_8300_003A.Count <= 0)
                        ITR_8300_003A.Add(new T_ITR_8300_003A_Body { CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID });

                    ITRRecord_8300_003A_Body = ITR_8300_003A.ToList().FirstOrDefault();
                    //if (ITRRecord_8300_003A_Body.Inch == "CW")
                    //    ITRRecord_8300_003A_Body.IsCW = true;
                    //else if (ITRRecord_8300_003A_Body.Inch == "CCW")
                    //    ITRRecord_8300_003A_Body.IsCCW = true;
                    //else
                    //    ITRRecord82ITRRecord_8300_003A_Body1_002A.IsCW = ITRRecord8211_002A.IsCCW = false;

                    //ITRRecord_8300_003A_Body.IsReqInch = "LightGray";

                    string RunTest_TestSql = "SELECT * FROM T_ITR_8300_003A_Illumin WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND ModelName = '" + Settings.ModelName + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var RunTestData = await _ITR_8300_003A_IlluminRepository.QueryAsync(RunTest_TestSql);

                    List<T_ITR_8300_003A_Illumin> RunTestList = RunTestData.ToList();
                    if (!RunTestData.Any())
                    {
                        var RunTests = await _ITR_8300_003A_IlluminRepository.GetAsync();
                        long RunTestID = RunTests.Count() + 1;
                        int AddItem = 1;
                        if (AddItem > 0)
                        {
                            RunTestList.AddRange(Enumerable.Range(0, AddItem).Select(x => new T_ITR_8300_003A_Illumin { ModelName = Settings.ModelName, CommonRowID = CommonHeaderFooter.ROWID, RowID = RunTestID++, ITRCommonID = CommonHeaderFooter.ID, CCMS_HEADERID = (int)CommonHeaderFooter.ID }).ToList());
                            await _ITR_8300_003A_IlluminRepository.InsertOrReplaceAsync(RunTestList);
                        }
                    }


                    ITR_8300_003A_IlluminList = new ObservableCollection<T_ITR_8300_003A_Illumin>(RunTestList);
                    BindIstrumentData();
                }
                catch (Exception ex)
                {
                }
            }
            else if (await _ITRService.ITR_8170_007A(SelectedCheckSheet.ITRNumber))
            {
                if (SelectedCheckSheet.ITRNumber == "8170-007A")
                {
                    IsStanderdITR = true;
                    IsSimpleITR = false;
                }
                try
                {
                    string Recordsql = "SELECT * FROM T_ITR_8170_007A_OP_Function_Test WHERE ModelName = '" + Settings.ModelName + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID;
                    var T_ITR_8170_007A_OP_FT = await _ITR_8170_007A_OP_Function_TestRepository.QueryAsync(Recordsql);

                    if (T_ITR_8170_007A_OP_FT.Count > 0)
                        ITR_8170_007AOP_FT = T_ITR_8170_007A_OP_FT.ToList().FirstOrDefault();
                    else
                    {
                        ITR_8170_007AOP_FT = new T_ITR_8170_007A_OP_Function_Test();
                        ITR_8170_007AOP_FT.CommonRowID = CommonHeaderFooter.ROWID;
                        ITR_8170_007AOP_FT.ModelName = Settings.ModelName;
                        ITR_8170_007AOP_FT.CCMS_HEADERID = (int)CommonHeaderFooter.ID;
                        ITR_8170_007AOP_FT = ITR_8170_007AOP_FT;
                    }

                    BindIstrumentData();
                }
                catch (Exception ex)
                {
                }
            }
        }
        public async Task<bool> SaveRecordTubeColorsData()
        {
            bool result = false;
            bool instrumentISNull = !String.IsNullOrEmpty(ITRRecord.CableLength) && !String.IsNullOrEmpty(ITRRecord.CableLengthUnit) && !String.IsNullOrEmpty(ITRRecord.FiberInformation) &&
                                    !String.IsNullOrEmpty(ITRRecord.FiberinformationUnit) && !String.IsNullOrEmpty(ITRRecord.OTDRModel) && !String.IsNullOrEmpty(ITRRecord.SpliceQty) && SelectedTestResultsAccepted != "";
            if (instrumentISNull)
            {
                ITRRecord.TestResultsAccepted = SelectedTestResultsAccepted;
                ITRRecord.IsUpdated = true;
                ITRRecord.AfiNo = !String.IsNullOrEmpty(ITRRecord.AfiNo) ? ITRRecord.AfiNo : "";
                await _RecordsRepository.InsertOrReplaceAsync(ITRRecord);
                await _TubeColorsRepository.InsertOrReplaceAsync(ItemSourceTubeColors.ToList());
                UpdateHeaderFootterdata();

                result = true;
            }
            else
            {
                ITRRecord.IsReqCableLength = String.IsNullOrEmpty(ITRRecord.CableLength) ? true : false;
                ITRRecord.IsReqCableLengthUnit = String.IsNullOrEmpty(ITRRecord.CableLengthUnit) ? true : false;
                ITRRecord.IsReqFiberInformation = String.IsNullOrEmpty(ITRRecord.FiberInformation) ? true : false;
                ITRRecord.IsReqFiberinformationUnit = String.IsNullOrEmpty(ITRRecord.FiberinformationUnit) ? true : false;
                ITRRecord.IsReqOTDRModel = String.IsNullOrEmpty(ITRRecord.OTDRModel) ? true : false;
                ITRRecord.IsReqSpliceQty = String.IsNullOrEmpty(ITRRecord.SpliceQty) ? true : false;
                ITRRecord = ITRRecord;
                result = false;
            }
            return result;
        }
        public async Task<bool> SaveRecordInsulationDetailsAData()
        {
            bool Result = false;
            bool RecordsISNull = !String.IsNullOrEmpty(ITRRecord04xA.NumberofCore) && !String.IsNullOrEmpty(ITRRecord04xA.CableCoreSize) && !String.IsNullOrEmpty(ITRRecord04xA.CableCoreSizeUnit) &&
                                    !String.IsNullOrEmpty(ITRRecord04xA.RatedConductorVoltage) && !String.IsNullOrEmpty(ITRRecord04xA.RatedConductorVoltageUnit) && SelectedTestVoltage != "";
            List<T_ITRInsulationDetails> Insulationdata = ItemSourceInsulationDetails.Where(x => (String.IsNullOrEmpty(x.CoreNum) || String.IsNullOrEmpty(x.ContinuityResult) || String.IsNullOrEmpty(x.CoretoCore) || String.IsNullOrEmpty(x.CoretoArmor) ||
                                                                          String.IsNullOrEmpty(x.CoretoSheild) || String.IsNullOrEmpty(x.ArmortoSheild) || String.IsNullOrEmpty(x.SheidtoSheild))).ToList();
            if (RecordsISNull && !Insulationdata.Any())
            {
                ITRRecord04xA.TestVoltage = SelectedTestVoltage;
                ITRRecord04xA.AfiNo = !String.IsNullOrEmpty(ITRRecord04xA.AfiNo) ? ITRRecord04xA.AfiNo : "";
                ItemSourceInsulationDetails.ForEach(x => { x.IsReqContinuityResult = false; x.IsReqCoretoCore = false; x.IsReqCoretoArmor = false; x.IsReqCoretoSheild = false; x.IsReqArmortoSheild = false; x.IsReqSheidtoSheild = false; });
                await _Records_04XARepository.InsertOrReplaceAsync(ITRRecord04xA);
                //string SaveRecordsql = " UPDATE T_ITRRecords_040A_041A_042A SET TestVoltage ='" + SelectedTestVoltage + "', AfiNo = '' WHERE ITRCommonID='" + CommonHeaderFooter.ID + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "'";
                //await _Records_04XARepository.QueryAsync(SaveRecordsql);
                await _InsulationDetailsRepository.InsertOrReplaceAsync(ItemSourceInsulationDetails.ToList());
                UpdateHeaderFootterdata();
                Result = true;
            }
            else
            {
                ITRRecord04xA.IsReqNumberofCore = String.IsNullOrEmpty(ITRRecord04xA.NumberofCore) ? true : false;
                ITRRecord04xA.IsReqCableCoreSize = String.IsNullOrEmpty(ITRRecord04xA.CableCoreSize) ? true : false;
                ITRRecord04xA.IsReqCableCoreSizeUnit = String.IsNullOrEmpty(ITRRecord04xA.CableCoreSizeUnit) ? true : false;
                ITRRecord04xA.IsReqRatedConductorVoltage = String.IsNullOrEmpty(ITRRecord04xA.RatedConductorVoltage) ? true : false;
                ITRRecord04xA.IsReqRatedConductorVoltageUnit = String.IsNullOrEmpty(ITRRecord04xA.RatedConductorVoltageUnit) ? true : false;
                ITRRecord04xA = ITRRecord04xA;

                ItemSourceInsulationDetails.Where(x => (String.IsNullOrEmpty(x.ContinuityResult))).ForEach(x => x.IsReqContinuityResult = true);
                ItemSourceInsulationDetails.Where(x => (String.IsNullOrEmpty(x.CoretoCore))).ForEach(x => x.IsReqCoretoCore = true);
                ItemSourceInsulationDetails.Where(x => (String.IsNullOrEmpty(x.CoretoArmor))).ForEach(x => x.IsReqCoretoArmor = true);
                ItemSourceInsulationDetails.Where(x => (String.IsNullOrEmpty(x.CoretoSheild))).ForEach(x => x.IsReqCoretoSheild = true);
                ItemSourceInsulationDetails.Where(x => (String.IsNullOrEmpty(x.ArmortoSheild))).ForEach(x => x.IsReqArmortoSheild = true);
                ItemSourceInsulationDetails.Where(x => (String.IsNullOrEmpty(x.SheidtoSheild))).ForEach(x => x.IsReqSheidtoSheild = true);

                ItemSourceInsulationDetails = new ObservableCollection<T_ITRInsulationDetails>(ItemSourceInsulationDetails);
                Result = false;
            }
            return Result;
        }

        public async Task<bool> Save8140_001A()
        {
            bool IsValidated = false;
            try
            {
                //bool IsAnyRecordNull = !String.IsNullOrEmpty(TestInstrucitonData.Instrument1) && !String.IsNullOrEmpty(TestInstrucitonData.InstrumentSerialNo1) && !String.IsNullOrEmpty(TestInstrucitonData.Instrument2)
                //                  && !String.IsNullOrEmpty(TestInstrucitonData.InstrumentSerialNo2) && !String.IsNullOrEmpty(TestInstrucitonData.Instrument3) && !String.IsNullOrEmpty(TestInstrucitonData.InstrumentSerialNo3);

                bool IsAnyRecordNull = true;
                if (SimpleITR)
                    IsAnyRecordNull = !String.IsNullOrEmpty(TestInstrucitonData.AmbientTemperature) && !String.IsNullOrEmpty(TestInstrucitonData.AppliedTestVoltage);

                List<T_ITR8140_001A_ContactResisTest> ContactResisTest = ContactResisTestList.ToList().Where(x => String.IsNullOrEmpty(x.ConnectionFrom) || String.IsNullOrEmpty(x.ConnectionTo) || String.IsNullOrEmpty(x.ContactResMeasuredValL1) ||
                          String.IsNullOrEmpty(x.ContactResMeasuredValL2) || String.IsNullOrEmpty(x.ContactResMeasuredValL3) || String.IsNullOrEmpty(x.TorqueMarkOkValue)).ToList(); // || String.IsNullOrEmpty(x.ContactResMeasuredValN).ToList()|| String.IsNullOrEmpty(x.ContactResMeasuredValL5)
                T_ITR8140_001ADialectricTest item = DieTests.FirstOrDefault();
                DieTests.ToList().ForEach(x => { x.TestVoltage1 = item.TestVoltage1; x.TestVoltage2 = item.TestVoltage1; x.TestVoltageAndDuration = item.TestVoltageAndDuration; });
                List<T_ITR8140_001ADialectricTest> DialectricTest = DieTests.ToList().Where(x => String.IsNullOrEmpty(x.TestPhase) || String.IsNullOrEmpty(x.TestVoltage1) || String.IsNullOrEmpty(x.InsulationResistance1) ||
                         String.IsNullOrEmpty(x.AppliedPoint) || String.IsNullOrEmpty(x.TestVoltageAndDuration) || String.IsNullOrEmpty(x.ChargeCurrent) || String.IsNullOrEmpty(x.TestVoltage2) || String.IsNullOrEmpty(x.InsulationResistance2)).ToList();

                List<T_ITR8140_001AInsulationResistanceTest> InsulationResistanceTest = InsulationRTests.ToList().Where(x => String.IsNullOrEmpty(x.InsulationResistance1) || String.IsNullOrEmpty(x.InsulationResistance2) || String.IsNullOrEmpty(x.InsulationResistance3)).ToList();
                List<T_ITRInstrumentData> ITRInstrumentData = ITRInstrumentDataList.Where(x => String.IsNullOrEmpty(x.TestEquipment)).ToList();

                if (IsAnyRecordNull && !(InsulationResistanceTest.Any() || DialectricTest.Any() || ContactResisTest.Any() || ITRInstrumentData.Any()))
                {
                    ContactResisTestList.ToList().Where(x => (String.IsNullOrEmpty(x.ContactResMeasuredValL5))).ForEach(x => x.ContactResMeasuredValL5 = "");
                    ContactResisTestList.ToList().Where(x => (String.IsNullOrEmpty(x.Remarks))).ForEach(x => x.Remarks = "");
                    ContactResisTestList.ToList().ForEach(x => x.TorqueMarkOk = x.TorqueMarkOkValue == "Yes" ? true : false);
                    await _T_ITR8140_001ATestInstrumentDataRepository.InsertOrReplaceAsync(TestInstrucitonData);
                    await _T_ITR8140_001A_ContactResisTestRepository.InsertOrReplaceAsync(ContactResisTestList);
                    await _T_ITR8140_001ADialectricTestRepository.InsertOrReplaceAsync(DieTests);
                    await _T_ITR8140_001AInsulationResistanceTestRepository.InsertOrReplaceAsync(InsulationRTests);
                    ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
                    await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentDataList.ToList());
                    UpdateHeaderFootterdata();
                    IsValidated = true;
                }
                else
                {
                    //    TestInstrucitonData.IsReqInstrument1 = String.IsNullOrEmpty(TestInstrucitonData.Instrument1) ? true : false;
                    //    TestInstrucitonData.IsReqInstrumentSerialNo1 = String.IsNullOrEmpty(TestInstrucitonData.InstrumentSerialNo1) ? true : false;
                    //    TestInstrucitonData.IsReqInstrument2 = String.IsNullOrEmpty(TestInstrucitonData.Instrument2) ? true : false;
                    //    TestInstrucitonData.IsReqInstrumentSerialNo2 = String.IsNullOrEmpty(TestInstrucitonData.InstrumentSerialNo2) ? true : false;
                    //    TestInstrucitonData.IsReqInstrument3 = String.IsNullOrEmpty(TestInstrucitonData.Instrument3) ? true : false;
                    //    TestInstrucitonData.IsReqInstrumentSerialNo3 = String.IsNullOrEmpty(TestInstrucitonData.InstrumentSerialNo3) ? true : false;

                    TestInstrucitonData.IsReqAmbientTemperature = String.IsNullOrEmpty(TestInstrucitonData.AmbientTemperature) ? true : false;
                    TestInstrucitonData.IsReqAppliedTestVoltage = String.IsNullOrEmpty(TestInstrucitonData.AppliedTestVoltage) ? true : false;

                    List<T_ITR8140_001A_ContactResisTest> validateConResList = ContactResisTestList.ToList();
                    validateConResList.ForEach(x => x.IsReqTorqueMarkOk = false);
                    validateConResList.Where(x => String.IsNullOrEmpty(x.ConnectionFrom)).ForEach(x => x.IsReqConnectionFrom = true);
                    validateConResList.Where(x => String.IsNullOrEmpty(x.ConnectionTo)).ForEach(x => x.IsReqConnectionTo = true);
                    validateConResList.Where(x => String.IsNullOrEmpty(x.ContactResMeasuredValL1)).ForEach(x => x.IsReqContactResMeasuredValL1 = true);
                    validateConResList.Where(x => String.IsNullOrEmpty(x.ContactResMeasuredValL2)).ForEach(x => x.IsReqContactResMeasuredValL2 = true);
                    validateConResList.Where(x => String.IsNullOrEmpty(x.ContactResMeasuredValL3)).ForEach(x => x.IsReqContactResMeasuredValL3 = true);
                    validateConResList.Where(x => String.IsNullOrEmpty(x.TorqueMarkOkValue)).ForEach(x => x.IsReqTorqueMarkOk = true);
                    //validateConResList.Where(x => String.IsNullOrEmpty(x.ContactResMeasuredValN)).ForEach(x => x.IsReqContactResMeasuredValN = true);
                    // validateConResList.Where(x => (String.IsNullOrEmpty(x.ContactResMeasuredValL5))).ForEach(x => x.IsReqContactResMeasuredValL5 = true);

                    List<T_ITR8140_001ADialectricTest> validateDialList = DieTests.ToList();
                    validateDialList.Where(x => String.IsNullOrEmpty(x.TestPhase)).ForEach(x => x.IsReqTestPhase = true);
                    validateDialList.Where(x => String.IsNullOrEmpty(x.TestVoltage1)).ForEach(x => x.IsReqTestVoltage1 = true);
                    validateDialList.Where(x => String.IsNullOrEmpty(x.InsulationResistance1)).ForEach(x => x.IsReqInsulationResistance1 = true);
                    validateDialList.Where(x => String.IsNullOrEmpty(x.AppliedPoint)).ForEach(x => x.IsReqAppliedPoint = true);
                    validateDialList.Where(x => String.IsNullOrEmpty(x.TestVoltageAndDuration)).ForEach(x => x.IsReqTestVoltageAndDuration = true);
                    validateDialList.Where(x => String.IsNullOrEmpty(x.ChargeCurrent)).ForEach(x => x.IsReqChargeCurrent = true);
                    validateDialList.Where(x => String.IsNullOrEmpty(x.TestVoltage2)).ForEach(x => x.IsReqTestVoltage2 = true);
                    validateDialList.Where(x => String.IsNullOrEmpty(x.InsulationResistance2)).ForEach(x => x.IsReqInsulationResistance2 = true);

                    List<T_ITR8140_001AInsulationResistanceTest> validateInsResList = InsulationRTests.ToList();
                    validateInsResList.Where(x => String.IsNullOrEmpty(x.InsulationResistance1)).ForEach(x => x.IsReqInsulationResistance1 = true);
                    validateInsResList.Where(x => String.IsNullOrEmpty(x.InsulationResistance2)).ForEach(x => x.IsReqInsulationResistance2 = true);
                    validateInsResList.Where(x => String.IsNullOrEmpty(x.InsulationResistance3)).ForEach(x => x.IsReqInsulationResistance3 = true);

                    ContactResisTestList = new ObservableCollection<T_ITR8140_001A_ContactResisTest>(validateConResList);
                    DieTests = new ObservableCollection<T_ITR8140_001ADialectricTest>(validateDialList);
                    InsulationRTests = new ObservableCollection<T_ITR8140_001AInsulationResistanceTest>(validateInsResList);
                    TestInstrucitonData = TestInstrucitonData;
                    ValidatetrumentData();
                    IsValidated = false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return IsValidated;
        }
        public async Task<bool> SaveRecord__8000003AData()
        {
            bool Result = false;
            try
            {
                bool RecordsISNull = !String.IsNullOrEmpty(Records_8000_003A.TYPEAndSIZE) && !String.IsNullOrEmpty(Records_8000_003A.VOLTAGE);
                List<T_ITR8000_003A_AcceptanceCriteria> AcceptanceCriteriaData = AcceptanceCriteriaList.Where(x => String.IsNullOrEmpty(x.LECEText) || String.IsNullOrEmpty(x.LECE) || String.IsNullOrEmpty(x.LLCCText) || String.IsNullOrEmpty(x.LLCC)).ToList();
                AcceptanceCriteriaData = AcceptanceCriteriaData.SkipWhile(x => x.IsReadyOnlyLLCCText && String.IsNullOrEmpty(x.LLCCText) && String.IsNullOrEmpty(x.LLCC) && !String.IsNullOrEmpty(x.LECEText) && !String.IsNullOrEmpty(x.LECE)).ToList();
                if (CoreType.ToUpper() == "CTC")
                    AcceptanceCriteriaData = AcceptanceCriteriaData.SkipWhile(x => String.IsNullOrEmpty(x.LLCCText)).ToList();

                List<T_ITRInstrumentData> ITRInstrumentData = ITRInstrumentDataList.Where(x => String.IsNullOrEmpty(x.TestEquipment)).ToList();

                if (RecordsISNull && !AcceptanceCriteriaData.Any() && !ITRInstrumentData.Any())
                {
                    await _Records_8000003ARepository.InsertOrReplaceAsync(Records_8000_003A);
                    await _Records_8000003A_AcceptanceCriteriaRepository.InsertOrReplaceAsync(AcceptanceCriteriaList.ToList());
                    ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
                    await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentDataList.ToList());
                    UpdateHeaderFootterdata();
                    List<T_ITR8000_003A_AcceptanceCriteria> bodyvariables = AcceptanceCriteriaList.ToList();
                    bodyvariables.ForEach(x => { x.IsReqLECEText = false; x.IsReqLECE = false; x.IsReqLLCCText = false; x.IsReqLLCC = false; });
                    AcceptanceCriteriaList = new ObservableCollection<T_ITR8000_003A_AcceptanceCriteria>(bodyvariables);
                    Result = true;
                }
                else
                {
                    Records_8000_003A.IsReqInstrument = String.IsNullOrEmpty(Records_8000_003A.Instrument) ? true : false;
                    Records_8000_003A.IsReqInstrumentSerialNo = String.IsNullOrEmpty(Records_8000_003A.InstrumentSerialNo) ? true : false;
                    Records_8000_003A.IsReqTYPEAndSIZE = String.IsNullOrEmpty(Records_8000_003A.TYPEAndSIZE) ? true : false;
                    Records_8000_003A.IsReqVOLTAGE = String.IsNullOrEmpty(Records_8000_003A.VOLTAGE) ? true : false;

                    AcceptanceCriteriaList.ForEach(x => { x.IsReqLECEText = false; x.IsReqLECE = false; x.IsReqLLCCText = false; x.IsReqLLCC = false; });
                    List<T_ITR8000_003A_AcceptanceCriteria> ValidateAcceptanceCriteria = AcceptanceCriteriaList.ToList();
                    ValidateAcceptanceCriteria.Where(x => (String.IsNullOrEmpty(x.LECEText))).ForEach(x => x.IsReqLECEText = true);
                    ValidateAcceptanceCriteria.Where(x => (String.IsNullOrEmpty(x.LECE))).ForEach(x => x.IsReqLECE = true);
                    ValidateAcceptanceCriteria.Where(x => (String.IsNullOrEmpty(x.LLCCText)) && !x.IsReadyOnlyLLCCText).ForEach(x => x.IsReqLLCCText = true);
                    ValidateAcceptanceCriteria.Where(x => (String.IsNullOrEmpty(x.LLCC)) && !x.IsReadyOnlyLLCCText).ForEach(x => x.IsReqLLCC = true);
                    ValidatetrumentData();
                    Records_8000_003A = Records_8000_003A;
                    AcceptanceCriteriaList = new ObservableCollection<T_ITR8000_003A_AcceptanceCriteria>(ValidateAcceptanceCriteria);
                    Result = false;
                }
            }
            catch (Exception ex)
            {
                Result = false;
            }
            return Result;
        }
        public async Task<bool> SaveRecord_080A_09XA_Data()
        {
            bool Result = false;
            bool RecordsISNull = !String.IsNullOrEmpty(ITRRecord_80A_91A.TestPressure) && !String.IsNullOrEmpty(ITRRecord_80A_91A.TestPressureUom) && SelectedTestResultAccept != "";
            if (RecordsISNull)
            {
                ITRRecord_80A_91A.TestResult = SelectedTestResultAccept == "Yes" ? true : false;
                ITRRecord_80A_91A.AfiNo = !String.IsNullOrEmpty(ITRRecord_80A_91A.AfiNo) ? ITRRecord_80A_91A.AfiNo : "";
                await _Records_080A_09XARepository.InsertOrReplaceAsync(ITRRecord_80A_91A);
                UpdateHeaderFootterdata();
                Result = true;
            }
            else
            {
                ITRRecord_80A_91A.IsReqTestPressure = String.IsNullOrEmpty(ITRRecord_80A_91A.TestPressure) ? true : false;
                ITRRecord_80A_91A.IsReqTestPressureUom = String.IsNullOrEmpty(ITRRecord_80A_91A.TestPressureUom) ? true : false;
                ITRRecord_80A_91A = ITRRecord_80A_91A;
                Result = false;
            }
            return Result;
        }
        public async Task<bool> SaveRecord_8100_001A_Data()
        {
            bool result = false;
            List<T_ITR8100_001A_CTdata> CTdata = ITR8100_001A_CTdata.Where(x => (String.IsNullOrEmpty(x.SerialNo) || String.IsNullOrEmpty(x.ModelNoTagNo) || String.IsNullOrEmpty(x.ClassVA) || String.IsNullOrEmpty(x.Ratio) || String.IsNullOrEmpty(x.ShortCircuitCurrent))).ToList();
            List<T_ITR8100_001A_RatioTest> RatioTest = ITR8100_001A_RatioTest.Where(x => (String.IsNullOrEmpty(x.Phase) || String.IsNullOrEmpty(x.PrimaryCurrent) || String.IsNullOrEmpty(x.SecondaryCurrent) || String.IsNullOrEmpty(x.CalculatedCTRatio))).ToList();
            List<T_ITR8100_001A_InsulationResistanceTest> IRT = ITR8100_001A_InsulationResistanceTest.Where(x => String.IsNullOrEmpty(x.PhaseToearth)).ToList();
            //bool instrumentISNull = !String.IsNullOrEmpty(TestInstrumentData8100_001A.Instrument) && !String.IsNullOrEmpty(TestInstrumentData8100_001A.InstrumentSerialNo);
            List<T_ITRInstrumentData> ITRInstrumentData = ITRInstrumentDataList.Where(x => String.IsNullOrEmpty(x.TestEquipment)).ToList();
            if (!(CTdata.Any() || RatioTest.Any() || IRT.Any() || ITRInstrumentData.Any()))
            {
                await _ITR8100_001A_CTdataRepository.InsertOrReplaceAsync(ITR8100_001A_CTdata);
                await _ITR8100_001A_IRTestRepository.InsertOrReplaceAsync(ITR8100_001A_InsulationResistanceTest);
                await _ITR8100_001A_RatioTestRepository.InsertOrReplaceAsync(ITR8100_001A_RatioTest);
                //string SaveRecordsql = " UPDATE T_ITR8100_001A_TestInstrumentData SET Instrument ='" + Instrument_8100 + "', InstrumentSerialNo = '" + InstrumentSrNo_8100 + "', CalibrationDate ='" + CalibrationDate_8100.Ticks + "'"
                //                 + " WHERE CommonHFID='" + CommonHeaderFooter.ID + "' AND ModelName = '" + Settings.ModelName + "'";
                //var TIDdata = await _ITR8100_001A_TIDataRepository.QueryAsync(SaveRecordsql);
                await _ITR8100_001A_TIDataRepository.InsertOrReplaceAsync(TestInstrumentData8100_001A);
                ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentDataList.ToList());
                UpdateHeaderFootterdata();
                result = true;
            }
            else
            {
                List<T_ITR8100_001A_CTdata> validateCTdataList = ITR8100_001A_CTdata.ToList();
                validateCTdataList.Where(x => (String.IsNullOrEmpty(x.ModelNoTagNo))).ForEach(x => x.IsReqModelNoTagNo = true);
                validateCTdataList.Where(x => (String.IsNullOrEmpty(x.SerialNo))).ForEach(x => x.IsReqSerialNo = true);
                validateCTdataList.Where(x => (String.IsNullOrEmpty(x.PrimaryCurrent))).ForEach(x => x.IsReqPrimaryCurrent = true);
                validateCTdataList.Where(x => (String.IsNullOrEmpty(x.SecondaryCurrent))).ForEach(x => x.IsReqSecondaryCurrent = true);
                validateCTdataList.Where(x => (String.IsNullOrEmpty(x.Ratio))).ForEach(x => x.IsReqRatio = true);
                validateCTdataList.Where(x => (String.IsNullOrEmpty(x.ClassVA))).ForEach(x => x.IsReqClassVA = true);
                validateCTdataList.Where(x => (String.IsNullOrEmpty(x.ShortCircuitCurrent))).ForEach(x => x.IsReqShortCircuitCurrent = true);

                List<T_ITR8100_001A_InsulationResistanceTest> validateIRTList = ITR8100_001A_InsulationResistanceTest.ToList();
                validateIRTList.Where(x => (String.IsNullOrEmpty(x.PhaseToearth))).ForEach(x => x.IsReqPhaseToearth = true);

                List<T_ITR8100_001A_RatioTest> validateRatioTest = ITR8100_001A_RatioTest.ToList();
                validateRatioTest.Where(x => (String.IsNullOrEmpty(x.Phase))).ForEach(x => x.IsReqPhase = true);
                validateRatioTest.Where(x => (String.IsNullOrEmpty(x.PrimaryCurrent))).ForEach(x => x.IsReqPrimaryCurrent = true);
                validateRatioTest.Where(x => (String.IsNullOrEmpty(x.SecondaryCurrent))).ForEach(x => x.IsReqSecondaryCurrent = true);
                validateRatioTest.Where(x => (String.IsNullOrEmpty(x.CalculatedCTRatio))).ForEach(x => x.IsReqCalculatedCTRatio = true);
                // validateRatioTest.Where(x => (String.IsNullOrEmpty(x.AmmeterReading))).ForEach(x => x.IsReqAmmeterReading = true);

                // TestInstrumentData8100_001A.IsReqInstrument = String.IsNullOrEmpty(TestInstrumentData8100_001A.Instrument) ? true : false;
                //TestInstrumentData8100_001A.IsReqInstrumentSerialNo = String.IsNullOrEmpty(TestInstrumentData8100_001A.InstrumentSerialNo) ? true : false;

                ITR8100_001A_CTdata = new ObservableCollection<T_ITR8100_001A_CTdata>(validateCTdataList);
                ITR8100_001A_InsulationResistanceTest = new ObservableCollection<T_ITR8100_001A_InsulationResistanceTest>(validateIRTList);
                ITR8100_001A_RatioTest = new ObservableCollection<T_ITR8100_001A_RatioTest>(validateRatioTest);
                TestInstrumentData8100_001A = TestInstrumentData8100_001A;
                ValidatetrumentData();
                result = false;
            }
            return result;
        }
        public async Task<bool> SaveRecord_8121_002A_Data()
        {
            bool result = false;
            List<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents> InspectionforControlList = InspectionforControls.Where(x => (String.IsNullOrEmpty(x.DeviceName) || String.IsNullOrEmpty(x.WiringCheck))).ToList();

            List<T_ITR8121_002A_TransformerRadioTest> TransformerRadioTest = TransformerRadioTestList.Where(x => (String.IsNullOrEmpty(x.TapNo) || String.IsNullOrEmpty(x.result) || String.IsNullOrEmpty(x.CalculatedRatio) || String.IsNullOrEmpty(x.TestValueL1Error) ||
                                   String.IsNullOrEmpty(x.TestValueL2Error) || String.IsNullOrEmpty(x.TestValueL3Error))).ToList();

            bool IsAnyRecordNull = !String.IsNullOrEmpty(Records8121_002A.HVtoEarth) && !String.IsNullOrEmpty(Records8121_002A.LVtoEarth) && !String.IsNullOrEmpty(Records8121_002A.HVtoLV) && !String.IsNullOrEmpty(Records8121_002A.TestVoltage) &&
                                    !String.IsNullOrEmpty(Records8121_002A.OilTemp) && !String.IsNullOrEmpty(Records8121_002A.DialeticStrengthTest1) &&
                                    !String.IsNullOrEmpty(Records8121_002A.DialeticStrengthTest2) && !String.IsNullOrEmpty(Records8121_002A.DialeticStrengthTest3) && !String.IsNullOrEmpty(Records8121_002A.DialeticStrengthTest4) &&
                                    !String.IsNullOrEmpty(Records8121_002A.DialeticStrengthTest5) && !String.IsNullOrEmpty(Records8121_002A.DialeticStrengthTest6) && !String.IsNullOrEmpty(Records8121_002A.DialeticStrengthTestAverage) && !String.IsNullOrEmpty(Records8121_002A.Result);
            if (SimpleITR)
                IsAnyRecordNull = !String.IsNullOrEmpty(Records8121_002A.AmbientTemperature);

            List<T_ITRInstrumentData> ITRInstrumentData = ITRInstrumentDataList.Where(x => String.IsNullOrEmpty(x.TestEquipment)).ToList();

            if (!(InspectionforControlList.Any() || TransformerRadioTest.Any() || ITRInstrumentData.Any()) && IsAnyRecordNull)
            {
                await _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents.InsertOrReplaceAsync(InspectionforControls);
                await _ITR8121_002A_TransformerRadioTest.InsertOrReplaceAsync(TransformerRadioTestList);
                await _ITR8121_002A_Records.InsertOrReplaceAsync(Records8121_002A);
                ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentDataList.ToList());
                UpdateHeaderFootterdata();

                result = true;
            }
            else
            {
                Records8121_002A.IsReqHVtoEarth = String.IsNullOrEmpty(Records8121_002A.HVtoEarth) ? true : false;
                Records8121_002A.IsReqLVtoEarth = String.IsNullOrEmpty(Records8121_002A.LVtoEarth) ? true : false;
                Records8121_002A.IsReqHVtoLV = String.IsNullOrEmpty(Records8121_002A.HVtoLV) ? true : false;
                Records8121_002A.IsReqTestVoltage = String.IsNullOrEmpty(Records8121_002A.TestVoltage) ? true : false;
                Records8121_002A.IsReqOilTemp = String.IsNullOrEmpty(Records8121_002A.OilTemp) ? true : false;
                Records8121_002A.IsReqInstrument1 = String.IsNullOrEmpty(Records8121_002A.Instrument1) ? true : false;
                Records8121_002A.IsReqInstrument2 = String.IsNullOrEmpty(Records8121_002A.Instrument2) ? true : false;
                Records8121_002A.IsReqInstrument3 = String.IsNullOrEmpty(Records8121_002A.Instrument3) ? true : false;
                Records8121_002A.IsReqInstrumentSerialNo1 = String.IsNullOrEmpty(Records8121_002A.InstrumentSerialNo1) ? true : false;
                Records8121_002A.IsReqInstrumentSerialNo2 = String.IsNullOrEmpty(Records8121_002A.InstrumentSerialNo2) ? true : false;
                Records8121_002A.IsReqInstrumentSerialNo3 = String.IsNullOrEmpty(Records8121_002A.InstrumentSerialNo3) ? true : false;
                Records8121_002A.IsReqDialeticStrengthTest1 = String.IsNullOrEmpty(Records8121_002A.DialeticStrengthTest1) ? true : false;
                Records8121_002A.IsReqDialeticStrengthTest2 = String.IsNullOrEmpty(Records8121_002A.DialeticStrengthTest2) ? true : false;
                Records8121_002A.IsReqDialeticStrengthTest3 = String.IsNullOrEmpty(Records8121_002A.DialeticStrengthTest3) ? true : false;
                Records8121_002A.IsReqDialeticStrengthTest4 = String.IsNullOrEmpty(Records8121_002A.DialeticStrengthTest4) ? true : false;
                Records8121_002A.IsReqDialeticStrengthTest5 = String.IsNullOrEmpty(Records8121_002A.DialeticStrengthTest5) ? true : false;
                Records8121_002A.IsReqDialeticStrengthTest6 = String.IsNullOrEmpty(Records8121_002A.DialeticStrengthTest6) ? true : false;
                Records8121_002A.IsReqDialeticStrengthTestAverage = String.IsNullOrEmpty(Records8121_002A.DialeticStrengthTestAverage) ? true : false;
                Records8121_002A.IsReqResult = String.IsNullOrEmpty(Records8121_002A.Result) ? true : false;
                Records8121_002A.IsReqAmbientTemperature = String.IsNullOrEmpty(Records8121_002A.AmbientTemperature) ? true : false;

                List<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents> validateInsConList = InspectionforControls.ToList();
                validateInsConList.Where(x => (String.IsNullOrEmpty(x.DeviceName))).ForEach(x => x.IsReqDeviceName = true);
                validateInsConList.Where(x => (String.IsNullOrEmpty(x.WiringCheck))).ForEach(x => x.IsReqWiringCheck = true);

                List<T_ITR8121_002A_TransformerRadioTest> validateTranRadioTestList = TransformerRadioTestList.ToList();
                validateTranRadioTestList.Where(x => (String.IsNullOrEmpty(x.TapNo))).ForEach(x => x.IsReqTapNo = true);
                validateTranRadioTestList.Where(x => (String.IsNullOrEmpty(x.TapVoltagePrimary))).ForEach(x => x.IsReqTapVoltagePrimary = true);
                validateTranRadioTestList.Where(x => (String.IsNullOrEmpty(x.TapVoltageSecondary))).ForEach(x => x.IsReqTapVoltageSecondary = true);
                validateTranRadioTestList.Where(x => (String.IsNullOrEmpty(x.CalculatedRatio))).ForEach(x => x.IsReqCalculatedRatio = true);
                validateTranRadioTestList.Where(x => (String.IsNullOrEmpty(x.TestValueL1Ratio))).ForEach(x => x.IsReqTestValueL1Ratio = true);
                validateTranRadioTestList.Where(x => (String.IsNullOrEmpty(x.TestValueL1Error))).ForEach(x => x.IsReqTestValueL1Error = true);
                validateTranRadioTestList.Where(x => (String.IsNullOrEmpty(x.TestValueL2Ratio))).ForEach(x => x.IsReqTestValueL2Ratio = true);
                validateTranRadioTestList.Where(x => (String.IsNullOrEmpty(x.TestValueL2Error))).ForEach(x => x.IsReqTestValueL2Error = true);
                validateTranRadioTestList.Where(x => (String.IsNullOrEmpty(x.TestValueL3Ratio))).ForEach(x => x.IsReqTestValueL3Ratio = true);
                validateTranRadioTestList.Where(x => (String.IsNullOrEmpty(x.TestValueL3Error))).ForEach(x => x.IsReqTestValueL3Error = true);
                validateTranRadioTestList.Where(x => (String.IsNullOrEmpty(x.result))).ForEach(x => x.IsReqresult = true);
                ValidatetrumentData();
                Records8121_002A = Records8121_002A;
                InspectionforControls = new ObservableCollection<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents>(validateInsConList);
                TransformerRadioTestList = new ObservableCollection<T_ITR8121_002A_TransformerRadioTest>(validateTranRadioTestList);

                result = false;
            }
            return result;
        }
        public async Task<bool> SaveRecord_8260_002A_Data()
        {
            bool result = false;

            bool IsAnyRecordNull = !String.IsNullOrEmpty(Body8260_002A.TestVolt) && !String.IsNullOrEmpty(Body8260_002A.CableType) && !String.IsNullOrEmpty(Body8260_002A.OperationVolt)
                                   && !String.IsNullOrEmpty(Body8260_002A.RatedVolt) && !String.IsNullOrEmpty(Body8260_002A.TestVoltDuration);

            List<T_ITR_8260_002A_DielectricTest> DielectricTestRecordsList = DielectricTestRecords.Where(x => String.IsNullOrEmpty(x.InsRes1) || String.IsNullOrEmpty(x.AppliedPoint) || String.IsNullOrEmpty(x.ChargeCurrent) || String.IsNullOrEmpty(x.InsRes2) ||
                                  String.IsNullOrEmpty(x.TestPhase)).ToList();
            List<T_ITRInstrumentData> ITRInstrumentData = ITRInstrumentDataList.Where(x => String.IsNullOrEmpty(x.TestEquipment)).ToList();

            if (IsAnyRecordNull && !(DielectricTestRecordsList.Any() || ITRInstrumentData.Any()))
            {
                await _ITR_8260_002A_BodyRepository.InsertOrReplaceAsync(Body8260_002A);
                await _ITR_8260_002A_DielectricTestRepository.InsertOrReplaceAsync(DielectricTestRecords);

                ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentDataList.ToList());
                UpdateHeaderFootterdata();

                result = true;
            }
            else
            {
                Body8260_002A.IsReqCableType = String.IsNullOrEmpty(Body8260_002A.CableType) ? true : false;
                Body8260_002A.IsReqOperationVolt = String.IsNullOrEmpty(Body8260_002A.OperationVolt) ? true : false;
                Body8260_002A.IsReqRatedVolt = String.IsNullOrEmpty(Body8260_002A.RatedVolt) ? true : false;
                Body8260_002A.IsReqTestVoltDuration = String.IsNullOrEmpty(Body8260_002A.TestVoltDuration) ? true : false;
                Body8260_002A.IsReqTestVolt = String.IsNullOrEmpty(Body8260_002A.TestVolt) ? true : false;

                List<T_ITR_8260_002A_DielectricTest> validateDielectricTestRecordsList = DielectricTestRecords.ToList();
                validateDielectricTestRecordsList.Where(x => (String.IsNullOrEmpty(x.InsRes1))).ForEach(x => x.IsReqInsRes1 = true);
                validateDielectricTestRecordsList.Where(x => (String.IsNullOrEmpty(x.AppliedPoint))).ForEach(x => x.IsReqAppliedPoint = true);
                validateDielectricTestRecordsList.Where(x => (String.IsNullOrEmpty(x.ChargeCurrent))).ForEach(x => x.IsReqChargeCurrent = true);
                validateDielectricTestRecordsList.Where(x => (String.IsNullOrEmpty(x.InsRes2))).ForEach(x => x.IsReqInsRes2 = true);
                validateDielectricTestRecordsList.Where(x => (String.IsNullOrEmpty(x.TestPhase))).ForEach(x => x.IsReqTestPhase = true);
                ValidatetrumentData();
                Body8260_002A = Body8260_002A;
                DielectricTestRecords = new ObservableCollection<T_ITR_8260_002A_DielectricTest>(validateDielectricTestRecordsList);
                Die8260_002AHeight = DielectricTestRecords.Count() * 50;
                result = false;
            }
            return result;
        }
        public async Task<bool> SavRecord8100_002AData()
        {
            bool result = false;
            bool VTdata = !String.IsNullOrEmpty(ITRRecord812X.ModelNo) && !String.IsNullOrEmpty(ITRRecord812X.SerialNo) && !String.IsNullOrEmpty(ITRRecord812X.Ratio) && !String.IsNullOrEmpty(ITRRecord812X.ClassVA) && !String.IsNullOrEmpty(ITRRecord812X.RatedVolt);
            List<T_ITRRecords_8100_002A_Radio_Test> RatioTest = ITR8100_002A_Radio_Test.Where(x => (String.IsNullOrEmpty(x.L1N) || String.IsNullOrEmpty(x.L2N) || String.IsNullOrEmpty(x.L3N) || String.IsNullOrEmpty(x.L1L2) || String.IsNullOrEmpty(x.L2L3) || String.IsNullOrEmpty(x.L3L1))).ToList();
            List<T_ITRRecords_8100_002A_InsRes_Test> InsRes = ITR8100_002A_InsRes_Test.Where(x => String.IsNullOrEmpty(x.WindingL1) || String.IsNullOrEmpty(x.WindingL2) || String.IsNullOrEmpty(x.WindingL3) || String.IsNullOrEmpty(x.WindingResult)).ToList();
            List<T_ITRInstrumentData> ITRInstrumentData = ITRInstrumentDataList.Where(x => String.IsNullOrEmpty(x.TestEquipment)).ToList();
            bool RatioTest1 = false; bool RatioTest2 = false; bool RatioTest3 = false;
            int index = 0;
            foreach (T_ITRRecords_8100_002A_Radio_Test item in RatioTest)
            {
                if (index == 0)
                {
                    RatioTest1 = !String.IsNullOrEmpty(item.PrimaryVoltage) && !String.IsNullOrEmpty(item.L1N) && !String.IsNullOrEmpty(item.L2N) && !String.IsNullOrEmpty(item.L1L2) && !String.IsNullOrEmpty(item.Result);
                }
                if (index == 1)
                {
                    RatioTest2 = !String.IsNullOrEmpty(item.PrimaryVoltage) && !String.IsNullOrEmpty(item.L2N) && !String.IsNullOrEmpty(item.L3N) && !String.IsNullOrEmpty(item.L2L3) && !String.IsNullOrEmpty(item.Result); ;
                }
                if (index == 2)
                {
                    RatioTest3 = !String.IsNullOrEmpty(item.PrimaryVoltage) && !String.IsNullOrEmpty(item.L1N) && !String.IsNullOrEmpty(item.L3N) && !String.IsNullOrEmpty(item.L3L1) && !String.IsNullOrEmpty(item.Result); ;
                }
                index++;
            }

            if (!(InsRes.Any() || ITRInstrumentData.Any()) && VTdata && RatioTest1 && RatioTest2 && RatioTest3)
            {
                await _ITRRecords_8100_002ARepository.InsertOrReplaceAsync(ITRRecord812X);
                await _ITRRecords_8100_002A_InsRes_TestRepository.InsertOrReplaceAsync(ITR8100_002A_InsRes_Test.ToList());
                await _ITRRecords_8100_002A_Radio_TestRepository.InsertOrReplaceAsync(ITR8100_002A_Radio_Test.ToList());
                ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentDataList.ToList());
                UpdateHeaderFootterdata();
                result = true;
            }
            else
            {
                ITRRecord812X.IsReqModelNo = String.IsNullOrEmpty(ITRRecord812X.ModelNo) ? true : false;
                ITRRecord812X.IsReqSerialNo = String.IsNullOrEmpty(ITRRecord812X.SerialNo) ? true : false;
                ITRRecord812X.IsReqRatio = String.IsNullOrEmpty(ITRRecord812X.Ratio) ? true : false;
                ITRRecord812X.IsReqClassVA = String.IsNullOrEmpty(ITRRecord812X.ClassVA) ? true : false;
                ITRRecord812X.IsReqRatedVolt = String.IsNullOrEmpty(ITRRecord812X.RatedVolt) ? true : false;
                ITRRecord812X.IsReqInstrument1 = String.IsNullOrEmpty(ITRRecord812X.Instrument1) ? true : false;
                ITRRecord812X.IsReqInstrumentSerialNo1 = String.IsNullOrEmpty(ITRRecord812X.InstrumentSerialNo1) ? true : false;
                ITRRecord812X.IsReqInstrument2 = String.IsNullOrEmpty(ITRRecord812X.Instrument2) ? true : false;
                ITRRecord812X.IsReqInstrumentSerialNo2 = String.IsNullOrEmpty(ITRRecord812X.InstrumentSerialNo2) ? true : false;

                List<T_ITRRecords_8100_002A_Radio_Test> validateRatioTestList = ITR8100_002A_Radio_Test.ToList();
                validateRatioTestList.Where(x => (String.IsNullOrEmpty(x.PrimaryVoltage))).ForEach(x => x.IsReqPrimaryVoltage = true);
                validateRatioTestList.Where(x => (String.IsNullOrEmpty(x.L1N)) && x.IsEnabledL1N).ForEach(x => x.IsReqL1N = true);
                validateRatioTestList.Where(x => (String.IsNullOrEmpty(x.L2N)) && x.IsEnabledL2N).ForEach(x => x.IsReqL2N = true);
                validateRatioTestList.Where(x => (String.IsNullOrEmpty(x.L3N)) && x.IsEnabledL3N).ForEach(x => x.IsReqL3N = true);
                validateRatioTestList.Where(x => (String.IsNullOrEmpty(x.L1L2)) && x.IsEnabledL1L2).ForEach(x => x.IsReqL1L2 = true);
                validateRatioTestList.Where(x => (String.IsNullOrEmpty(x.L2L3)) && x.IsEnabledL2L3).ForEach(x => x.IsReqL2L3 = true);
                validateRatioTestList.Where(x => (String.IsNullOrEmpty(x.L3L1)) && x.IsEnabledL3L1).ForEach(x => x.IsReqL3L1 = true);
                validateRatioTestList.Where(x => (String.IsNullOrEmpty(x.Result))).ForEach(x => x.IsReqResult = true);

                List<T_ITRRecords_8100_002A_InsRes_Test> validateInsResList = ITR8100_002A_InsRes_Test.ToList();
                validateInsResList.Where(x => (String.IsNullOrEmpty(x.WindingL1))).ForEach(x => x.IsReqWindingL1 = true);
                validateInsResList.Where(x => (String.IsNullOrEmpty(x.WindingL2))).ForEach(x => x.IsReqWindingL2 = true);
                validateInsResList.Where(x => (String.IsNullOrEmpty(x.WindingL3))).ForEach(x => x.IsReqWindingL3 = true);
                validateInsResList.Where(x => (String.IsNullOrEmpty(x.WindingResult))).ForEach(x => x.IsReqWindingResult = true);
                ValidatetrumentData();
                ITRRecord812X = ITRRecord812X;
                ITR8100_002A_Radio_Test = new ObservableCollection<T_ITRRecords_8100_002A_Radio_Test>(validateRatioTestList);
                ITR8100_002A_InsRes_Test = new ObservableCollection<T_ITRRecords_8100_002A_InsRes_Test>(validateInsResList);

                _userDialogs.Alert("One or More Fields are required", "Unable to Save", "Ok");
                result = false;
            }
            return result;
        }
        public async Task<bool> SavRecord8161_001AData()
        {
            bool result = false;
            bool IsBodyNull = true;
            //bool Body = !String.IsNullOrEmpty(ITRRecord8161_1XA.Instrument1) && !String.IsNullOrEmpty(ITRRecord8161_1XA.Instrument2) && !String.IsNullOrEmpty(ITRRecord8161_1XA.InstrumentSerialNo1) && !String.IsNullOrEmpty(ITRRecord8161_1XA.InstrumentSerialNo2) && 
            //            !String.IsNullOrEmpty(ITRRecord8161_1XA.CalibrationDate1.ToString()) && !String.IsNullOrEmpty(ITRRecord8161_1XA.CalibrationDate2.ToString());
            if (IsSimpleITR)
                IsBodyNull = !String.IsNullOrEmpty(ITRRecord8161_1XA.Acceptance) && !String.IsNullOrEmpty(ITRRecord8161_1XA.Temperature) && !String.IsNullOrEmpty(ITRRecord8161_1XA.TestVolt);
            List<T_ITRRecords_8161_001A_ConRes> ConRes = ITR_8161_001A_ConRes.Where(x => (String.IsNullOrEmpty(x.ConnoFrom) || String.IsNullOrEmpty(x.ConnoTo) || String.IsNullOrEmpty(x.CRMVL1) || String.IsNullOrEmpty(x.CRMVL2) || String.IsNullOrEmpty(x.CRMVL3))).ToList();
            List<T_ITRRecords_8161_001A_InsRes> InsRes = ITR_8161_001A_InsRes.Where(x => String.IsNullOrEmpty(x.InsRes1) || String.IsNullOrEmpty(x.InsRes2) || String.IsNullOrEmpty(x.InsRes3)).ToList();
            List<T_ITRInstrumentData> ITRInstrumentData = ITRInstrumentDataList.Where(x => String.IsNullOrEmpty(x.TestEquipment)).ToList();
            if (IsBodyNull && !(InsRes.Any() || ConRes.Any() || ITRInstrumentData.Any()))
            {
                await _ITRRecords_8161_001A_BodyRepository.InsertOrReplaceAsync(ITRRecord8161_1XA);
                await _ITRRecords_8161_001A_InsResRepository.InsertOrReplaceAsync(ITR_8161_001A_InsRes.ToList());
                await _ITRRecords_8161_001A_ConResRepository.InsertOrReplaceAsync(ITR_8161_001A_ConRes.ToList());
                ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentDataList.ToList());
                UpdateHeaderFootterdata();
                result = true;
            }
            else
            {
                ITRRecord8161_1XA.IsReqInstrument1 = String.IsNullOrEmpty(ITRRecord8161_1XA.Instrument1) ? true : false;
                ITRRecord8161_1XA.IsReqInstrumentSerialNo1 = String.IsNullOrEmpty(ITRRecord8161_1XA.InstrumentSerialNo1) ? true : false;
                ITRRecord8161_1XA.IsReqInstrument2 = String.IsNullOrEmpty(ITRRecord8161_1XA.Instrument2) ? true : false;
                ITRRecord8161_1XA.IsReqInstrumentSerialNo2 = String.IsNullOrEmpty(ITRRecord8161_1XA.InstrumentSerialNo2) ? true : false;
                ITRRecord8161_1XA.IsReqAcceptance = String.IsNullOrEmpty(ITRRecord8161_1XA.Acceptance) ? true : false;
                ITRRecord8161_1XA.IsReqTemperature = String.IsNullOrEmpty(ITRRecord8161_1XA.Temperature) ? true : false;
                ITRRecord8161_1XA.IsReqTestVolt = String.IsNullOrEmpty(ITRRecord8161_1XA.TestVolt) ? true : false;

                List<T_ITRRecords_8161_001A_ConRes> ValidateConResList = ITR_8161_001A_ConRes.ToList();
                ValidateConResList.Where(x => (String.IsNullOrEmpty(x.ConnoFrom))).ForEach(x => x.IsReqConnoFrom = true);
                ValidateConResList.Where(x => (String.IsNullOrEmpty(x.ConnoTo))).ForEach(x => x.IsReqConnoTo = true);
                ValidateConResList.Where(x => (String.IsNullOrEmpty(x.CRMVL1))).ForEach(x => x.IsReqCRMVL1 = true);
                ValidateConResList.Where(x => (String.IsNullOrEmpty(x.CRMVL2))).ForEach(x => x.IsReqCRMVL2 = true);
                ValidateConResList.Where(x => (String.IsNullOrEmpty(x.CRMVL3))).ForEach(x => x.IsReqCRMVL3 = true);

                List<T_ITRRecords_8161_001A_InsRes> ValidateInsResList = ITR_8161_001A_InsRes.ToList();
                ValidateInsResList.Where(x => (String.IsNullOrEmpty(x.InsRes1))).ForEach(x => x.IsReqInsRes1 = true);
                ValidateInsResList.Where(x => (String.IsNullOrEmpty(x.InsRes2))).ForEach(x => x.IsReqInsRes2 = true);
                ValidateInsResList.Where(x => (String.IsNullOrEmpty(x.InsRes3))).ForEach(x => x.IsReqInsRes3 = true);
                ValidatetrumentData();
                ITRRecord8161_1XA = ITRRecord8161_1XA;
                ITR_8161_001A_ConRes = new ObservableCollection<T_ITRRecords_8161_001A_ConRes>(ValidateConResList);
                ITR_8161_001A_InsRes = new ObservableCollection<T_ITRRecords_8161_001A_InsRes>(ValidateInsResList);
                _userDialogs.Alert("One or More Fields are required", "Unable to Save", "Ok");
                result = false;
            }
            return result;
        }
        public async Task<bool> SaveRecord_8121_004AData()
        {
            bool result = false;
            try
            {
                List<T_ITR8121_004AInCAndAuxiliary> InCAndAuxiliaryData = InispactionForControlAndAuxiliary8121.Where(x => (String.IsNullOrEmpty(x.DeviceName) || String.IsNullOrEmpty(x.WiringCheck))).ToList();
                List<T_ITR8121_004ATransformerRatioTest> TransformerRatioTestData = TransformerRatioTest8121.Where(x => (String.IsNullOrEmpty(x.TapNo) || String.IsNullOrEmpty(x.TapVoltagePrimary) || String.IsNullOrEmpty(x.TapVoltageSecondary) ||
                                String.IsNullOrEmpty(x.CalculateRatio) || String.IsNullOrEmpty(x.TestValueL1Ratio) || String.IsNullOrEmpty(x.TestValueL1Error) || String.IsNullOrEmpty(x.TestValueL2Ratio) || String.IsNullOrEmpty(x.TestValueL2Error) ||
                                String.IsNullOrEmpty(x.TestValueL3Ratio) || String.IsNullOrEmpty(x.TestValueL3Error) || String.IsNullOrEmpty(x.Result))).ToList();
                bool TestInstrumentData = !String.IsNullOrEmpty(TestInstrumentData8121_004A.HvToEarth) && !String.IsNullOrEmpty(TestInstrumentData8121_004A.LvToEarth) && !String.IsNullOrEmpty(TestInstrumentData8121_004A.HvToLv) && !String.IsNullOrEmpty(TestInstrumentData8121_004A.TestVoltage);
                if (SimpleITR)
                    TestInstrumentData = !String.IsNullOrEmpty(TestInstrumentData8121_004A.AmbientTemperature);
                List<T_ITRInstrumentData> ITRInstrumentData = ITRInstrumentDataList.Where(x => String.IsNullOrEmpty(x.TestEquipment)).ToList();
                if (!(InCAndAuxiliaryData.Any() || TransformerRatioTestData.Any() || ITRInstrumentData.Any()) && TestInstrumentData)
                {
                    await _ITR8121_004AInCAndAuxiliaryRepository.InsertOrReplaceAsync(InispactionForControlAndAuxiliary8121);
                    await _ITR8121_004ATransformerRatioTestRepository.InsertOrReplaceAsync(TransformerRatioTest8121);
                    await _ITR8121_004ATestInstrumentDataRepository.InsertOrReplaceAsync(TestInstrumentData8121_004A);
                    ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
                    await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentDataList.ToList());
                    UpdateHeaderFootterdata();

                    result = true;
                }
                else
                {
                    TestInstrumentData8121_004A.IsReqHvToEarth = String.IsNullOrEmpty(TestInstrumentData8121_004A.HvToEarth) ? true : false;
                    TestInstrumentData8121_004A.IsReqLvToEarth = String.IsNullOrEmpty(TestInstrumentData8121_004A.LvToEarth) ? true : false;
                    TestInstrumentData8121_004A.IsReqHvToLv = String.IsNullOrEmpty(TestInstrumentData8121_004A.HvToLv) ? true : false;
                    TestInstrumentData8121_004A.IsReqTestVoltage = String.IsNullOrEmpty(TestInstrumentData8121_004A.TestVoltage) ? true : false;
                    TestInstrumentData8121_004A.IsReqInstrument = String.IsNullOrEmpty(TestInstrumentData8121_004A.Instrument) ? true : false;
                    TestInstrumentData8121_004A.IsReqInstrumentSerialNo = String.IsNullOrEmpty(TestInstrumentData8121_004A.InstrumentSerialNo) ? true : false;
                    TestInstrumentData8121_004A.IsReqInstrument1 = String.IsNullOrEmpty(TestInstrumentData8121_004A.Instrument1) ? true : false;
                    TestInstrumentData8121_004A.IsReqInstrumentSerialNo1 = String.IsNullOrEmpty(TestInstrumentData8121_004A.InstrumentSerialNo1) ? true : false;
                    TestInstrumentData8121_004A.IsReqInstrument2 = String.IsNullOrEmpty(TestInstrumentData8121_004A.Instrument2) ? true : false;
                    TestInstrumentData8121_004A.IsReqInstrumentSerialNo2 = String.IsNullOrEmpty(TestInstrumentData8121_004A.InstrumentSerialNo2) ? true : false;
                    TestInstrumentData8121_004A.IsReqAmbientTemperature = String.IsNullOrEmpty(TestInstrumentData8121_004A.AmbientTemperature) ? true : false;

                    List<T_ITR8121_004AInCAndAuxiliary> validateInsConAuxList = InispactionForControlAndAuxiliary8121.ToList();
                    validateInsConAuxList.Where(x => (String.IsNullOrEmpty(x.DeviceName))).ForEach(x => x.IsReqDeviceName = true);
                    validateInsConAuxList.Where(x => (String.IsNullOrEmpty(x.WiringCheck))).ForEach(x => x.IsReqWiringCheck = true);

                    List<T_ITR8121_004ATransformerRatioTest> validatetranRadiTestList = TransformerRatioTest8121.ToList();
                    validatetranRadiTestList.Where(x => (String.IsNullOrEmpty(x.TapNo))).ForEach(x => x.IsReqTapNo = true);
                    validatetranRadiTestList.Where(x => (String.IsNullOrEmpty(x.TapVoltagePrimary))).ForEach(x => x.IsReqTapVoltagePrimary = true);
                    validatetranRadiTestList.Where(x => (String.IsNullOrEmpty(x.TapVoltageSecondary))).ForEach(x => x.IsReqTapVoltageSecondary = true);
                    validatetranRadiTestList.Where(x => (String.IsNullOrEmpty(x.CalculateRatio))).ForEach(x => x.IsReqCalculateRatio = true);
                    validatetranRadiTestList.Where(x => (String.IsNullOrEmpty(x.TestValueL1Ratio))).ForEach(x => x.IsReqTestValueL1Ratio = true);
                    validatetranRadiTestList.Where(x => (String.IsNullOrEmpty(x.TestValueL1Error))).ForEach(x => x.IsReqTestValueL1Error = true);
                    validatetranRadiTestList.Where(x => (String.IsNullOrEmpty(x.TestValueL2Ratio))).ForEach(x => x.IsReqTestValueL2Ratio = true);
                    validatetranRadiTestList.Where(x => (String.IsNullOrEmpty(x.TestValueL2Error))).ForEach(x => x.IsReqTestValueL2Error = true);
                    validatetranRadiTestList.Where(x => (String.IsNullOrEmpty(x.TestValueL3Ratio))).ForEach(x => x.IsReqTestValueL3Ratio = true);
                    validatetranRadiTestList.Where(x => (String.IsNullOrEmpty(x.TestValueL3Error))).ForEach(x => x.IsReqTestValueL3Error = true);
                    validatetranRadiTestList.Where(x => (String.IsNullOrEmpty(x.Result))).ForEach(x => x.IsReqResult = true);
                    ValidatetrumentData();
                    TestInstrumentData8121_004A = TestInstrumentData8121_004A;
                    InispactionForControlAndAuxiliary8121 = new ObservableCollection<T_ITR8121_004AInCAndAuxiliary>(validateInsConAuxList);
                    TransformerRatioTest8121 = new ObservableCollection<T_ITR8121_004ATransformerRatioTest>(validatetranRadiTestList);

                    result = false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }
        public async Task<bool> SaveRecord_8161_002AData()
        {
            bool result = false;
            try
            {
                bool Body = !String.IsNullOrEmpty(ITRRecord8161_2XA.TestVolt) && !String.IsNullOrEmpty(ITRRecord8161_2XA.TestVoltDuration);
                bool IsAnyRecordNull1 = !String.IsNullOrEmpty(InsRes1_1) && !String.IsNullOrEmpty(InsRes2_1) && !String.IsNullOrEmpty(AppliedPoint_1)
                                   && !String.IsNullOrEmpty(InsRes1_2) && !String.IsNullOrEmpty(InsRes2_2) && !String.IsNullOrEmpty(AppliedPoint_2)
                                   && !String.IsNullOrEmpty(InsRes1_3) && !String.IsNullOrEmpty(InsRes2_3) && !String.IsNullOrEmpty(AppliedPoint_3)
                                   && !String.IsNullOrEmpty(ChargeCurrent_1) && !String.IsNullOrEmpty(ChargeCurrent_2) && !String.IsNullOrEmpty(ChargeCurrent_3);
                List<T_ITRInstrumentData> ITRInstrumentData = ITRInstrumentDataList.Where(x => String.IsNullOrEmpty(x.TestEquipment)).ToList();
                if (Body && IsAnyRecordNull1 && !ITRInstrumentData.Any())
                {
                    await _ITR8161_002A_BodyRepository.InsertOrReplaceAsync(ITRRecord8161_2XA);
                    if (ITR_8161_002A_DielectricTest != null)
                    {
                        ITR_8161_002A_DielectricTest[0].InsRes1 = InsRes1_1;
                        ITR_8161_002A_DielectricTest[0].InsRes2 = InsRes2_1;
                        ITR_8161_002A_DielectricTest[0].AppliedPoint = AppliedPoint_1;
                        ITR_8161_002A_DielectricTest[0].ChargeCurrent = ChargeCurrent_1;

                        ITR_8161_002A_DielectricTest[1].InsRes1 = InsRes1_2;
                        ITR_8161_002A_DielectricTest[1].InsRes2 = InsRes2_2;
                        ITR_8161_002A_DielectricTest[1].AppliedPoint = AppliedPoint_2;
                        ITR_8161_002A_DielectricTest[1].ChargeCurrent = ChargeCurrent_2;

                        ITR_8161_002A_DielectricTest[2].InsRes1 = InsRes1_3;
                        ITR_8161_002A_DielectricTest[2].InsRes2 = InsRes2_3;
                        ITR_8161_002A_DielectricTest[2].AppliedPoint = AppliedPoint_3;
                        ITR_8161_002A_DielectricTest[2].ChargeCurrent = ChargeCurrent_3;

                        await _ITR8161_002A_DielectricTestRepository.InsertOrReplaceAsync(ITR_8161_002A_DielectricTest);
                    }
                    ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
                    await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentDataList.ToList());
                    UpdateHeaderFootterdata();
                    result = true;
                }
                else
                {
                    ITRRecord8161_2XA.IsReqTestVolt = String.IsNullOrEmpty(ITRRecord8161_2XA.TestVolt) ? true : false;
                    ITRRecord8161_2XA.IsReqTestVoltDuration = String.IsNullOrEmpty(ITRRecord8161_2XA.TestVoltDuration) ? true : false;

                    IsReqInsRes1_1 = String.IsNullOrEmpty(InsRes1_1) ? true : false;
                    IsReqInsRes1_2 = String.IsNullOrEmpty(InsRes1_2) ? true : false;
                    IsReqInsRes1_3 = String.IsNullOrEmpty(InsRes1_3) ? true : false;
                    IsReqInsRes2_1 = String.IsNullOrEmpty(InsRes2_1) ? true : false;
                    IsReqInsRes2_2 = String.IsNullOrEmpty(InsRes2_2) ? true : false;
                    IsReqInsRes2_3 = String.IsNullOrEmpty(InsRes2_3) ? true : false;
                    IsReqAppliedPoint_1 = String.IsNullOrEmpty(AppliedPoint_1) ? true : false;
                    IsReqAppliedPoint_2 = String.IsNullOrEmpty(AppliedPoint_2) ? true : false;
                    IsReqAppliedPoint_3 = String.IsNullOrEmpty(AppliedPoint_3) ? true : false;
                    IsReqChargeCurrent_1 = String.IsNullOrEmpty(ChargeCurrent_1) ? true : false;
                    IsReqChargeCurrent_2 = String.IsNullOrEmpty(ChargeCurrent_2) ? true : false;
                    IsReqChargeCurrent_3 = String.IsNullOrEmpty(ChargeCurrent_3) ? true : false;
                    ValidatetrumentData();
                    ITRRecord8161_2XA = ITRRecord8161_2XA;
                    result = false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }
        public async Task<bool> SaveRecord_8000_101AAData()
        {
            bool result = false;
            try
            {
                bool GenInfo = !String.IsNullOrEmpty(ITR8000_101AGenlnfo.GeneralManufacturer) && !String.IsNullOrEmpty(ITR8000_101AGenlnfo.GeneralModelNo) && !String.IsNullOrEmpty(ITR8000_101AGenlnfo.SerialNo) && !String.IsNullOrEmpty(ITR8000_101AGenlnfo.ExCertNo) &&
                               !String.IsNullOrEmpty(ITR8000_101AGenlnfo.Zone) && !String.IsNullOrEmpty(ITR8000_101AGenlnfo.GasGroup1) && !String.IsNullOrEmpty(ITR8000_101AGenlnfo.TempClass1) && !String.IsNullOrEmpty(ITR8000_101AGenlnfo.Exprotection) && !String.IsNullOrEmpty(ITR8000_101AGenlnfo.GasGroup2) &&
                               !String.IsNullOrEmpty(ITR8000_101AGenlnfo.TempClass2) && !String.IsNullOrEmpty(ITR8000_101AGenlnfo.EquipmentProtectionLevelEPL) && !String.IsNullOrEmpty(ITR8000_101AGenlnfo.IngressProtectionIPRating) && !String.IsNullOrEmpty(ITR8000_101AGenlnfo.HAEVerificationDossierNo);
                if (GenInfo)
                {
                    ITR8000_101AGenlnfo.AfiNo = CommonHeaderFooter.AFINo;
                    ITR8000_101AGenlnfo.AreaClassification = ITR8000_101AGenlnfo.EquipmentExTechnique = "";
                    foreach (var propertyInfo in ITR8000_101AISBarDetails.GetType().GetProperties())
                    {
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            if (propertyInfo.GetValue(ITR8000_101AISBarDetails, null) == null)
                            {
                                propertyInfo.SetValue(ITR8000_101AISBarDetails, string.Empty, null);
                            }
                        }
                    }

                    await _ITR8000_101A_GeneralnformationRepository.InsertOrReplaceAsync(ITR8000_101AGenlnfo);
                    await _ITR8000_101A_RecordISBarrierDetailsRepository.InsertOrReplaceAsync(ITR8000_101AISBarDetails);
                    UpdateHeaderFootterdata();
                    result = true;
                }
                else
                {
                    ITR8000_101AGenlnfo.IsReqGeneralManufacturer = String.IsNullOrEmpty(ITR8000_101AGenlnfo.GeneralManufacturer) ? true : false;
                    ITR8000_101AGenlnfo.IsReqGeneralModelNo = String.IsNullOrEmpty(ITR8000_101AGenlnfo.GeneralModelNo) ? true : false;
                    ITR8000_101AGenlnfo.IsReqSerialNo = String.IsNullOrEmpty(ITR8000_101AGenlnfo.SerialNo) ? true : false;
                    ITR8000_101AGenlnfo.IsReqExCertNo = String.IsNullOrEmpty(ITR8000_101AGenlnfo.ExCertNo) ? true : false;
                    ITR8000_101AGenlnfo.IsReqZone = String.IsNullOrEmpty(ITR8000_101AGenlnfo.Zone) ? true : false;
                    ITR8000_101AGenlnfo.IsReqGasGroup1 = String.IsNullOrEmpty(ITR8000_101AGenlnfo.GasGroup1) ? true : false;
                    ITR8000_101AGenlnfo.IsReqTempClass1 = String.IsNullOrEmpty(ITR8000_101AGenlnfo.TempClass1) ? true : false;
                    ITR8000_101AGenlnfo.IsReqExprotection = String.IsNullOrEmpty(ITR8000_101AGenlnfo.Exprotection) ? true : false;
                    ITR8000_101AGenlnfo.IsReqGasGroup2 = String.IsNullOrEmpty(ITR8000_101AGenlnfo.GasGroup2) ? true : false;
                    ITR8000_101AGenlnfo.IsReqTempClass2 = String.IsNullOrEmpty(ITR8000_101AGenlnfo.TempClass2) ? true : false;
                    ITR8000_101AGenlnfo.IsReqEquipmentProtectionLevelEPL = String.IsNullOrEmpty(ITR8000_101AGenlnfo.EquipmentProtectionLevelEPL) ? true : false;
                    ITR8000_101AGenlnfo.IsReqIngressProtectionIPRating = String.IsNullOrEmpty(ITR8000_101AGenlnfo.IngressProtectionIPRating) ? true : false;
                    ITR8000_101AGenlnfo.IsReqHAEVerificationDossierNo = String.IsNullOrEmpty(ITR8000_101AGenlnfo.HAEVerificationDossierNo) ? true : false;
                    ITR8000_101AGenlnfo = ITR8000_101AGenlnfo;

                    result = false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }
        public async Task<bool> SaveRecord_8211_002AData()
        {
            bool result = false;
            try
            {
                bool Body = true;
                List<T_ITRRecords_8211_002A_RunTest> RunTest8211_002A;
                if (IsSimpleITR)
                {
                    Body = !String.IsNullOrEmpty(ITRRecord8211_002A.Pass_Fail) && !String.IsNullOrEmpty(ITRRecord8211_002A.Ambient_Temp) && !String.IsNullOrEmpty(ITRRecord8211_002A.MotorRatedPower) && !String.IsNullOrEmpty(ITRRecord8211_002A.RatedVolt) && !String.IsNullOrEmpty(ITRRecord8211_002A.StartingCurrent) && !String.IsNullOrEmpty(ITRRecord8211_002A.RotationSpeed) && !String.IsNullOrEmpty(ITRRecord8211_002A.Inch);
                    RunTest8211_002A = ITR_8211_002A_RunTest.Take(6).Where(x => String.IsNullOrEmpty(x.AmbientTemp) || String.IsNullOrEmpty(x.RunCurrentL1) || String.IsNullOrEmpty(x.BearingTempDE) || String.IsNullOrEmpty(x.BearingTempNDE) ||
                    String.IsNullOrEmpty(x.VibraDriveVer) || String.IsNullOrEmpty(x.VibraDriveHori) || String.IsNullOrEmpty(x.VibraDriveAxis) || String.IsNullOrEmpty(x.VibraNonDriveVer) || String.IsNullOrEmpty(x.VibraNonDriveHori) || String.IsNullOrEmpty(x.VibraNonDriveAxis)).ToList();
                }
                else
                {
                    RunTest8211_002A = ITR_8211_002A_RunTest.Take(9).Where(x => String.IsNullOrEmpty(x.AmbientTemp) || String.IsNullOrEmpty(x.RunCurrentL1) || String.IsNullOrEmpty(x.BearingTempDE) || String.IsNullOrEmpty(x.BearingTempNDE) ||
                   String.IsNullOrEmpty(x.VibraDriveVer) || String.IsNullOrEmpty(x.VibraDriveHori) || String.IsNullOrEmpty(x.VibraDriveAxis) || String.IsNullOrEmpty(x.VibraNonDriveVer) || String.IsNullOrEmpty(x.VibraNonDriveHori) || String.IsNullOrEmpty(x.VibraNonDriveAxis)).ToList();
                    Body = !String.IsNullOrEmpty(ITRRecord8211_002A.MotorRatedPower) && !String.IsNullOrEmpty(ITRRecord8211_002A.RatedVolt) && !String.IsNullOrEmpty(ITRRecord8211_002A.StartingCurrent) && !String.IsNullOrEmpty(ITRRecord8211_002A.RotationSpeed) && !String.IsNullOrEmpty(ITRRecord8211_002A.Inch);
                }

                List<T_ITRInstrumentData> ITRInstrumentData = ITRInstrumentDataList.Where(x => String.IsNullOrEmpty(x.TestEquipment)).ToList();
                if (Body && !(RunTest8211_002A.Any() || ITRInstrumentData.Any()))
                {
                    if (ITRRecord8211_002A.IsCW)
                        ITRRecord8211_002A.Inch = "CW";
                    else if (ITRRecord8211_002A.IsCCW)
                        ITRRecord8211_002A.Inch = "CCW";
                    else
                        ITRRecord8211_002A.Inch = "";

                    ITRRecord8211_002A.IsReqInch = String.IsNullOrEmpty(ITRRecord8211_002A.Inch) ? "Red" : "LightGray";

                    foreach (var propertyInfo in ITRRecord8211_002A.GetType().GetProperties())
                    {
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            if (propertyInfo.GetValue(ITRRecord8211_002A, null) == null)
                            {
                                propertyInfo.SetValue(ITRRecord8211_002A, string.Empty, null);
                            }
                        }
                    }

                    ITR_8211_002A_RunTest.ForEach(x =>
                    {
                        foreach (var propertyInfo in x.GetType().GetProperties())
                        {
                            if (propertyInfo.PropertyType == typeof(string))
                            {
                                if (propertyInfo.GetValue(x, null) == null)
                                {
                                    propertyInfo.SetValue(x, string.Empty, null);
                                }
                            }
                        }
                    });
                    await _ITRRecords_8211_002A_BodyRepository.InsertOrReplaceAsync(ITRRecord8211_002A);
                    await _ITRRecords_8211_002A_RunTestRepository.InsertOrReplaceAsync(ITR_8211_002A_RunTest);

                    ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
                    await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentDataList.ToList());
                    UpdateHeaderFootterdata();
                    result = true;
                }
                else
                {
                    ITRRecord8211_002A.IsReqMotorRatedPower = String.IsNullOrEmpty(ITRRecord8211_002A.MotorRatedPower) ? true : false;
                    ITRRecord8211_002A.IsReqRatedVolt = String.IsNullOrEmpty(ITRRecord8211_002A.RatedVolt) ? true : false;
                    ITRRecord8211_002A.IsReqStartingCurrent = String.IsNullOrEmpty(ITRRecord8211_002A.StartingCurrent) ? true : false;
                    ITRRecord8211_002A.IsReqRotationSpeed = String.IsNullOrEmpty(ITRRecord8211_002A.RotationSpeed) ? true : false;
                    ITRRecord8211_002A.IsReqInch = String.IsNullOrEmpty(ITRRecord8211_002A.Inch) ? "Red" : "LightGray";

                    List<T_ITRRecords_8211_002A_RunTest> validateRunTest8211_002A = ITR_8211_002A_RunTest.ToList();
                    if (IsSimpleITR)
                    {
                        validateRunTest8211_002A = ITR_8211_002A_RunTest.Take(6).ToList();
                        ITRRecord8211_002A.IsReqAmbient_Temp = String.IsNullOrEmpty(ITRRecord8211_002A.Ambient_Temp) ? true : false;
                        ITRRecord8211_002A.IsReqPass_Fail = String.IsNullOrEmpty(ITRRecord8211_002A.Pass_Fail) ? true : false;
                    }

                    validateRunTest8211_002A.Where(x => (String.IsNullOrEmpty(x.AmbientTemp))).ForEach(x => x.IsReqAmbientTemp = true);
                    validateRunTest8211_002A.Where(x => (String.IsNullOrEmpty(x.RunCurrentL1))).ForEach(x => x.IsReqRunCurrentL1 = true);
                    validateRunTest8211_002A.Where(x => (String.IsNullOrEmpty(x.BearingTempDE))).ForEach(x => x.IsReqBearingTempDE = true);
                    validateRunTest8211_002A.Where(x => (String.IsNullOrEmpty(x.BearingTempNDE))).ForEach(x => x.IsReqBearingTempNDE = true);
                    validateRunTest8211_002A.Where(x => (String.IsNullOrEmpty(x.VibraDriveVer))).ForEach(x => x.IsReqVibraDriveVer = true);
                    validateRunTest8211_002A.Where(x => (String.IsNullOrEmpty(x.VibraDriveHori))).ForEach(x => x.IsReqVibraDriveHori = true);
                    validateRunTest8211_002A.Where(x => (String.IsNullOrEmpty(x.VibraDriveAxis))).ForEach(x => x.IsReqVibraDriveAxis = true);
                    validateRunTest8211_002A.Where(x => (String.IsNullOrEmpty(x.VibraNonDriveVer))).ForEach(x => x.IsReqVibraNonDriveVer = true);
                    validateRunTest8211_002A.Where(x => (String.IsNullOrEmpty(x.VibraNonDriveHori))).ForEach(x => x.IsReqVibraNonDriveHori = true);
                    validateRunTest8211_002A.Where(x => (String.IsNullOrEmpty(x.VibraNonDriveAxis))).ForEach(x => x.IsReqVibraNonDriveAxis = true);

                    ValidatetrumentData();
                    ITRRecord8211_002A = ITRRecord8211_002A;
                    ITR_8211_002A_RunTest = validateRunTest8211_002A;
                    result = false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }
        public async Task<bool> SaveRecord_7000_101AHarmonyData()
        {
            bool result = false;
            try
            {
                bool GenInfo = !String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.GeneralManufacturer) && !String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.GeneralModelNo) && !String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.SerialNo) && !String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.ExCertNo) &&
                               !String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.EquipmentExTechnique) && !String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.EquipmentGasGroup) && !String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.EquipmentTempClass) && !String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.EquipmentIPRating) &&
                               !String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.AreaClassificationZone) && !String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.AreaClassificationGasGroup) && !String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.AreaClassificationTempClass);
                if (GenInfo)
                {
                    foreach (var propertyInfo in ITR7000_101AHarmonyGenlnfo.GetType().GetProperties())
                    {
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            if (propertyInfo.GetValue(ITR7000_101AHarmonyGenlnfo, null) == null)
                            {
                                propertyInfo.SetValue(ITR7000_101AHarmonyGenlnfo, string.Empty, null);
                            }
                        }
                    }
                    foreach (var propertyInfo in ITR7000_101AHarmony_ActivityDetails.GetType().GetProperties())
                    {
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            if (propertyInfo.GetValue(ITR7000_101AHarmony_ActivityDetails, null) == null)
                            {
                                propertyInfo.SetValue(ITR7000_101AHarmony_ActivityDetails, string.Empty, null);
                            }
                        }
                    }

                    await _ITR7000_101AHarmony_GenlnfoRepository.InsertOrReplaceAsync(ITR7000_101AHarmonyGenlnfo);
                    await _ITR7000_101AHarmony_ActivityDetailsRepository.InsertOrReplaceAsync(ITR7000_101AHarmony_ActivityDetails);
                    UpdateHeaderFootterdata();
                    result = true;
                }
                else
                {
                    ITR7000_101AHarmonyGenlnfo.IsReqGeneralManufacturer = String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.GeneralManufacturer) ? true : false;
                    ITR7000_101AHarmonyGenlnfo.IsReqGeneralModelNo = String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.GeneralModelNo) ? true : false;
                    ITR7000_101AHarmonyGenlnfo.IsReqSerialNo = String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.SerialNo) ? true : false;
                    ITR7000_101AHarmonyGenlnfo.IsReqExCertNo = String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.ExCertNo) ? true : false;
                    ITR7000_101AHarmonyGenlnfo.IsReqAreaClassificationZone = String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.AreaClassificationZone) ? true : false;
                    ITR7000_101AHarmonyGenlnfo.IsReqAreaClassificationGasGroup = String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.AreaClassificationGasGroup) ? true : false;
                    ITR7000_101AHarmonyGenlnfo.IsReqAreaClassificationTempClass = String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.AreaClassificationTempClass) ? true : false;
                    ITR7000_101AHarmonyGenlnfo.IsReqEquipmentExTechnique = String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.EquipmentExTechnique) ? true : false;
                    ITR7000_101AHarmonyGenlnfo.IsReqEquipmentGasGroup = String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.EquipmentGasGroup) ? true : false;
                    ITR7000_101AHarmonyGenlnfo.IsReqEquipmentTempClass = String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.EquipmentTempClass) ? true : false;
                    ITR7000_101AHarmonyGenlnfo.IsReqEquipmentIPRating = String.IsNullOrEmpty(ITR7000_101AHarmonyGenlnfo.EquipmentIPRating) ? true : false;
                    ITR7000_101AHarmonyGenlnfo = ITR7000_101AHarmonyGenlnfo;

                    result = false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }
        public async Task<bool> SaveRecord8140_002AData()
        {
            bool result = false;
            try
            {
                bool EleRec, EleMach, AnaSignal = false;
                if (SelectedCheckSheet.ITRNumber == "8140-002A")
                {
                    EleRec = !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCCloseMResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCOpenMResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCIONMResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCIOFFMResult) &&
                             !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBSpringCMResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBSpringICResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBSpringDCResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.ShutterMResult) &&
                             !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCRackoutPCResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCTestCResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCServiceCResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.ESCloseCResult) &&
                             !String.IsNullOrEmpty(ITR8140_002ARecordsMO.ESOpenCResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.ESIndicationCloseCResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.ESIndicationOpenCResult) &&
                             !String.IsNullOrEmpty(ITR8140_002ARecordsMO.ICBCCDResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.DSCloseMResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.DSOpenMResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.DSIndicationCloseResult) &&
                             !String.IsNullOrEmpty(ITR8140_002ARecordsMO.DSIndicationOpenResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.IESDSCBCCondustorResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.IESCCDResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.ICBCDPResult);

                    EleMach = !String.IsNullOrEmpty(ITR8140_002ARecords.CBCloseECResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.CBOpenECResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.CBSpringCECResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.CBControlSCResult) &&
                              !String.IsNullOrEmpty(ITR8140_002ARecords.LocalRemoteSSCResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.AutoRSCResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.AmpereSSCResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.VoltSSCResult) &&
                              !String.IsNullOrEmpty(ITR8140_002ARecords.LimitSForCBPResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.LimitSForEarthingSResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.CBAuxiliaryCCResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.AuxiliaryRelayCCResult) &&
                              !String.IsNullOrEmpty(ITR8140_002ARecords.CBIndicationONCResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.CBIndicationOFFCResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.FaultLICResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.SpareCOCResult) &&
                              !String.IsNullOrEmpty(ITR8140_002ARecords.SignalCOCResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.WithUpstream1CResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.WithDownstream1CResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.WithUpstream2CResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.WithDownstream2CResult) &&
                              !String.IsNullOrEmpty(ITR8140_002ARecords.ESCloseECResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.ESOpenECResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.DSCloseECResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.DSOpenECResult);
                    AnaSignal = !String.IsNullOrEmpty(ITR8140_002ARecordsAS.Current_mA) && !String.IsNullOrEmpty(ITR8140_002ARecordsAS.ReactivePower_mA) && !String.IsNullOrEmpty(ITR8140_002ARecordsAS.Voltage_mA) && !String.IsNullOrEmpty(ITR8140_002ARecordsAS.ActivePower_mA) &&
                                !String.IsNullOrEmpty(ITR8140_002ARecordsAS.PowerConsumptionPulse);
                }
                else
                {
                    EleRec = !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCCloseMResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCOpenMResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCIONMResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCIOFFMResult) &&
                             !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBSpringCMResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBSpringICResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBSpringDCResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.ShutterMResult) &&
                             !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCRackoutPCResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCTestCResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCServiceCResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.ESCloseCResult) &&
                             !String.IsNullOrEmpty(ITR8140_002ARecordsMO.ESOpenCResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.ESIndicationCloseCResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.ESIndicationOpenCResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.IESCBConductorResult) &&
                             !String.IsNullOrEmpty(ITR8140_002ARecordsMO.ICBCCDResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.IESCCDResult) && !String.IsNullOrEmpty(ITR8140_002ARecordsMO.ICBCDPResult);

                    EleMach = !String.IsNullOrEmpty(ITR8140_002ARecords.CBCloseECResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.CBOpenECResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.CBSpringCECResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.CBControlSCResult) &&
                              !String.IsNullOrEmpty(ITR8140_002ARecords.LocalRemoteSSCResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.AutoRSCResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.AmpereSSCResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.VoltSSCResult) &&
                              !String.IsNullOrEmpty(ITR8140_002ARecords.LimitSForCBPResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.LimitSForEarthingSResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.CBAuxiliaryCCResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.AuxiliaryRelayCCResult) &&
                              !String.IsNullOrEmpty(ITR8140_002ARecords.CBIndicationONCResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.CBIndicationOFFCResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.FaultLICResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.SpareCOCResult) &&
                              !String.IsNullOrEmpty(ITR8140_002ARecords.SignalCOCResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.WithUpstream1CResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.WithDownstream1CResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.WithUpstream2CResult) && !String.IsNullOrEmpty(ITR8140_002ARecords.WithDownstream2CResult);
                    AnaSignal = !String.IsNullOrEmpty(ITR8140_002ARecordsAS.Current_mA) && !String.IsNullOrEmpty(ITR8140_002ARecordsAS.ReactivePower_mA) && !String.IsNullOrEmpty(ITR8140_002ARecordsAS.Voltage_mA) && !String.IsNullOrEmpty(ITR8140_002ARecordsAS.ActivePower_mA) &&
                                !String.IsNullOrEmpty(ITR8140_002ARecordsAS.PowerConsumptionPulse);
                }

                if (EleRec && EleMach && AnaSignal)
                {
                    foreach (var propertyInfo in ITR8140_002ARecords.GetType().GetProperties())
                    {
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            if (propertyInfo.GetValue(ITR8140_002ARecords, null) == null)
                            {
                                propertyInfo.SetValue(ITR8140_002ARecords, string.Empty, null);
                            }
                        }
                    }

                    foreach (var propertyInfo in ITR8140_002ARecordsMO.GetType().GetProperties())
                    {
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            if (propertyInfo.GetValue(ITR8140_002ARecordsMO, null) == null)
                            {
                                propertyInfo.SetValue(ITR8140_002ARecordsMO, string.Empty, null);
                            }
                        }
                    }

                    foreach (var propertyInfo in ITR8140_002ARecordsAS.GetType().GetProperties())
                    {
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            if (propertyInfo.GetValue(ITR8140_002ARecordsAS, null) == null)
                            {
                                propertyInfo.SetValue(ITR8140_002ARecordsAS, string.Empty, null);
                            }
                        }
                    }

                    await _ITR_8140_002A_RecordsRepository.InsertOrReplaceAsync(ITR8140_002ARecords);
                    await _ITR_8140_002A_RecordsMechnicalOperation_RecordsRepository.InsertOrReplaceAsync(ITR8140_002ARecordsMO);
                    await _ITR_8140_002A_RecordsAnalogSignalRepository.InsertOrReplaceAsync(ITR8140_002ARecordsAS);
                    UpdateHeaderFootterdata();
                    result = true;
                }
                else
                {
                    ITR8140_002ARecords.IsReqCBControlSCResult = String.IsNullOrEmpty(ITR8140_002ARecords.CBControlSCResult) ? true : false;
                    ITR8140_002ARecords.IsReqVoltSSCResult = String.IsNullOrEmpty(ITR8140_002ARecords.VoltSSCResult) ? true : false;
                    ITR8140_002ARecords.IsReqAuxiliaryRelayCCResult = String.IsNullOrEmpty(ITR8140_002ARecords.AuxiliaryRelayCCResult) ? true : false;
                    ITR8140_002ARecords.IsReqSpareCOCResult = String.IsNullOrEmpty(ITR8140_002ARecords.SpareCOCResult) ? true : false;
                    ITR8140_002ARecords.IsReqWithDownstream2CResult = String.IsNullOrEmpty(ITR8140_002ARecords.WithDownstream2CResult) ? true : false;
                    ITR8140_002ARecords.IsReqDSOpenECResult = String.IsNullOrEmpty(ITR8140_002ARecords.DSOpenECResult) ? true : false;
                    ITR8140_002ARecords.IsReqWithDownstream1CResult = String.IsNullOrEmpty(ITR8140_002ARecords.WithDownstream1CResult) ? true : false;

                    ITR8140_002ARecords.IsReqCBSpringCECResult = String.IsNullOrEmpty(ITR8140_002ARecords.CBSpringCECResult) ? true : false;
                    ITR8140_002ARecords.IsReqAmpereSSCResult = String.IsNullOrEmpty(ITR8140_002ARecords.AmpereSSCResult) ? true : false;
                    ITR8140_002ARecords.IsReqCBAuxiliaryCCResult = String.IsNullOrEmpty(ITR8140_002ARecords.CBAuxiliaryCCResult) ? true : false;
                    ITR8140_002ARecords.IsReqFaultLICResult = String.IsNullOrEmpty(ITR8140_002ARecords.FaultLICResult) ? true : false;
                    ITR8140_002ARecords.IsReqWithUpstream2CResult = String.IsNullOrEmpty(ITR8140_002ARecords.WithUpstream2CResult) ? true : false;
                    ITR8140_002ARecords.IsReqDSCloseECResult = String.IsNullOrEmpty(ITR8140_002ARecords.DSCloseECResult) ? true : false;

                    ITR8140_002ARecords.IsReqCBCloseECResult = String.IsNullOrEmpty(ITR8140_002ARecords.CBCloseECResult) ? true : false;
                    ITR8140_002ARecords.IsReqLocalRemoteSSCResult = String.IsNullOrEmpty(ITR8140_002ARecords.LocalRemoteSSCResult) ? true : false;
                    ITR8140_002ARecords.IsReqLimitSForCBPResult = String.IsNullOrEmpty(ITR8140_002ARecords.LimitSForCBPResult) ? true : false;
                    ITR8140_002ARecords.IsReqCBIndicationONCResult = String.IsNullOrEmpty(ITR8140_002ARecords.CBIndicationONCResult) ? true : false;
                    ITR8140_002ARecords.IsReqSignalCOCResult = String.IsNullOrEmpty(ITR8140_002ARecords.SignalCOCResult) ? true : false;
                    ITR8140_002ARecords.IsReqESCloseECResult = String.IsNullOrEmpty(ITR8140_002ARecords.ESCloseECResult) ? true : false;

                    ITR8140_002ARecords.IsReqCBOpenECResult = String.IsNullOrEmpty(ITR8140_002ARecords.CBOpenECResult) ? true : false;
                    ITR8140_002ARecords.IsReqAutoRSCResult = String.IsNullOrEmpty(ITR8140_002ARecords.AutoRSCResult) ? true : false;
                    ITR8140_002ARecords.IsReqLimitSForEarthingSResult = String.IsNullOrEmpty(ITR8140_002ARecords.LimitSForEarthingSResult) ? true : false;
                    ITR8140_002ARecords.IsReqCBIndicationOFFCResult = String.IsNullOrEmpty(ITR8140_002ARecords.CBIndicationOFFCResult) ? true : false;
                    ITR8140_002ARecords.IsReqWithUpstream1CResult = String.IsNullOrEmpty(ITR8140_002ARecords.WithUpstream1CResult) ? true : false;
                    ITR8140_002ARecords.IsReqESOpenECResult = String.IsNullOrEmpty(ITR8140_002ARecords.ESOpenECResult) ? true : false;

                    ITR8140_002ARecordsMO.IsReqCBCIOFFMResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCIOFFMResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqShutterMResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.ShutterMResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqESCloseCResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.ESCloseCResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqIESCBConductorResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.IESCBConductorResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqDSIndicationCloseResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.DSIndicationCloseResult) ? true : false;

                    ITR8140_002ARecordsMO.IsReqCBCIONMResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCIONMResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqCBSpringDCResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBSpringDCResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqCBCServiceCResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCServiceCResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqESIndicationOpenCResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.ESIndicationOpenCResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqDSOpenMResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.DSOpenMResult) ? true : false;

                    ITR8140_002ARecordsMO.IsReqCBCCloseMResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCCloseMResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqCBSpringCMResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBSpringCMResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqCBCRackoutPCResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCRackoutPCResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqESOpenCResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.ESOpenCResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqICBCCDResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.ICBCCDResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqDSIndicationOpenResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.DSIndicationOpenResult) ? true : false;

                    ITR8140_002ARecordsMO.IsReqCBCOpenMResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCOpenMResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqCBSpringICResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBSpringICResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqCBCTestCResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.CBCTestCResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqESIndicationCloseCResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.ESIndicationCloseCResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqDSCloseMResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.DSCloseMResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqIESDSCBCCondustorResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.IESDSCBCCondustorResult) ? true : false;

                    ITR8140_002ARecordsMO.IsReqIESCCDResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.IESCCDResult) ? true : false;
                    ITR8140_002ARecordsMO.IsReqICBCDPResult = String.IsNullOrEmpty(ITR8140_002ARecordsMO.ICBCDPResult) ? true : false;

                    ITR8140_002ARecordsAS.IsCurrent_mA = String.IsNullOrEmpty(ITR8140_002ARecordsAS.Current_mA) ? true : false;
                    ITR8140_002ARecordsAS.IsActivePower_mA = String.IsNullOrEmpty(ITR8140_002ARecordsAS.ActivePower_mA) ? true : false;
                    ITR8140_002ARecordsAS.IsReactivePower_mA = String.IsNullOrEmpty(ITR8140_002ARecordsAS.ReactivePower_mA) ? true : false;
                    ITR8140_002ARecordsAS.IsVoltage_mA = String.IsNullOrEmpty(ITR8140_002ARecordsAS.Voltage_mA) ? true : false;
                    ITR8140_002ARecordsAS.IsPowerConsumptionPulse = String.IsNullOrEmpty(ITR8140_002ARecordsAS.PowerConsumptionPulse) ? true : false;

                    ITR8140_002ARecords = ITR8140_002ARecords;
                    ITR8140_002ARecordsMO = ITR8140_002ARecordsMO;
                    ITR8140_002ARecordsAS = ITR8140_002ARecordsAS;
                    result = false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }
        public async Task<bool> SaveRecord8140_004AData()
        {
            bool result = false;
            try
            {
                List<T_ITRInstrumentData> ITRInstrumentData = ITRInstrumentDataList.Where(x => String.IsNullOrEmpty(x.TestEquipment)).ToList();
                if (!ITRInstrumentData.Any())
                {
                    foreach (var propertyInfo in ITR8140_004ARecords.GetType().GetProperties())
                    {
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            if (propertyInfo.GetValue(ITR8140_004ARecords, null) == null)
                            {
                                propertyInfo.SetValue(ITR8140_004ARecords, string.Empty, null);
                            }
                        }
                    }

                    await _ITR_8140_004A_RecordsRepository.InsertOrReplaceAsync(ITR8140_004ARecords);

                    ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
                    await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentDataList.ToList());
                    UpdateHeaderFootterdata();
                    return true;
                }
                else
                {
                    ValidatetrumentData();
                    result = false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }

        public async Task<bool> SaveRecord_8170_002AData()
        {
            bool result = false;
            try
            {
                bool GenInfo = !String.IsNullOrEmpty(ITR_8170_002A_InsRes.AllLineToEarth) && !String.IsNullOrEmpty(ITR_8170_002A_InsRes.AmbientTemp) && !String.IsNullOrEmpty(ITR_8170_002A_InsRes.OnOffOperation);
                bool IsAnyRecordNull1 = !String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.AmmeterforIncomerOperatedOrNot) && !String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.DoorlightOperatedOrNot)
                                   && !String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.IncomerABopenLamp) && !String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.IncomerABpoweronOperatedOrNot)
                                   && !String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.IncomerABclosedLamp) && !String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.IncomerABopenOperatedOrNot)
                                   && !String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.IncomerABclosedOperatedOrNot) && !String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.IncomerABpoweronLamp)
                                   && !String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.IncomerABtripLamp) && !String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.IncomerABtripOperatedOrNot)
                                   && !String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.SpaceHeaterOperatedOrNot) && !String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.VoltmeterforIncomerABOperatedOrNot);

                bool VoltRead = !String.IsNullOrEmpty(ITR8170_002A_Voltage_ReadingList[0].L1_L2) && !String.IsNullOrEmpty(ITR8170_002A_Voltage_ReadingList[0].L2_L3)
                    && !String.IsNullOrEmpty(ITR8170_002A_Voltage_ReadingList[0].L3_L1) && !String.IsNullOrEmpty(ITR8170_002A_Voltage_ReadingList[0].L1_N)
                    && !String.IsNullOrEmpty(ITR8170_002A_Voltage_ReadingList[0].L2_N) && !String.IsNullOrEmpty(ITR8170_002A_Voltage_ReadingList[0].L3_N)
                    && !String.IsNullOrEmpty(ITR8170_002A_Voltage_ReadingList[0].PhaseRotation);
                List<T_ITRInstrumentData> ITRInstrumentData = ITRInstrumentDataList.Where(x => String.IsNullOrEmpty(x.TestEquipment)).ToList();
                if (GenInfo && IsAnyRecordNull1 && !ITRInstrumentData.Any() && VoltRead)
                {
                    foreach (var propertyInfo in ITR_8170_002A_InsRes.GetType().GetProperties())
                    {
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            if (propertyInfo.GetValue(ITR_8170_002A_InsRes, null) == null)
                            {
                                propertyInfo.SetValue(ITR_8170_002A_InsRes, string.Empty, null);
                            }
                        }
                    }
                    foreach (var propertyInfo in ITR_8170_002A_IndifictionLamp.GetType().GetProperties())
                    {
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            if (propertyInfo.GetValue(ITR_8170_002A_IndifictionLamp, null) == null)
                            {
                                propertyInfo.SetValue(ITR_8170_002A_IndifictionLamp, string.Empty, null);
                            }
                        }
                    }

                    ITR8170_002A_Voltage_ReadingList.ForEach(x =>
                    {
                        foreach (var propertyInfo in x.GetType().GetProperties())
                        {
                            if (propertyInfo.PropertyType == typeof(string))
                            {
                                if (propertyInfo.GetValue(x, null) == null)
                                {
                                    propertyInfo.SetValue(x, string.Empty, null);
                                }
                            }
                        }
                    });

                    await _ITR_8170_002A_InsResRepository.InsertOrReplaceAsync(ITR_8170_002A_InsRes);
                    await _ITR_8170_002A_IndifictionLampRepository.InsertOrReplaceAsync(ITR_8170_002A_IndifictionLamp);
                    await _ITRRecords_8170_002A_Voltage_ReadingRepository.InsertOrReplaceAsync(ITR8170_002A_Voltage_ReadingList.ToList());
                    ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
                    await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentDataList.ToList());

                    UpdateHeaderFootterdata();
                    result = true;
                }
                else
                {
                    ITR_8170_002A_InsRes.IsReqAllLineToEarth = String.IsNullOrEmpty(ITR_8170_002A_InsRes.AllLineToEarth) ? true : false;
                    ITR_8170_002A_InsRes.IsReqAmbientTemp = String.IsNullOrEmpty(ITR_8170_002A_InsRes.AmbientTemp) ? true : false;
                    ITR_8170_002A_InsRes.IsReqOnOffOperation = String.IsNullOrEmpty(ITR_8170_002A_InsRes.OnOffOperation) ? true : false;
                    ITR_8170_002A_InsRes = ITR_8170_002A_InsRes;

                    ITR_8170_002A_IndifictionLamp.IsReqAmmeterforIncomerOperatedOrNot = String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.AmmeterforIncomerOperatedOrNot) ? true : false;
                    ITR_8170_002A_IndifictionLamp.IsReqDoorlightOperatedOrNot = String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.DoorlightOperatedOrNot) ? true : false;
                    ITR_8170_002A_IndifictionLamp.IsReqIncomerABopenLamp = String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.IncomerABopenLamp) ? true : false;
                    ITR_8170_002A_IndifictionLamp.IsReqIncomerABpoweronOperatedOrNot = String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.IncomerABpoweronOperatedOrNot) ? true : false;
                    ITR_8170_002A_IndifictionLamp.IsReqIncomerABclosedLamp = String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.IncomerABclosedLamp) ? true : false;
                    ITR_8170_002A_IndifictionLamp.IsReqIncomerABopenOperatedOrNot = String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.IncomerABopenOperatedOrNot) ? true : false;
                    ITR_8170_002A_IndifictionLamp.IsReqIncomerABclosedOperatedOrNot = String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.IncomerABclosedOperatedOrNot) ? true : false;
                    ITR_8170_002A_IndifictionLamp.IsReqIncomerABpoweronLamp = String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.IncomerABpoweronLamp) ? true : false;
                    ITR_8170_002A_IndifictionLamp.IsReqIncomerABtripLamp = String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.IncomerABtripLamp) ? true : false;
                    ITR_8170_002A_IndifictionLamp.IsReqIncomerABtripOperatedOrNot = String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.IncomerABtripOperatedOrNot) ? true : false;
                    ITR_8170_002A_IndifictionLamp.IsReqSpaceHeaterOperatedOrNot = String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.SpaceHeaterOperatedOrNot) ? true : false;
                    ITR_8170_002A_IndifictionLamp.IsReqVoltmeterforIncomerABOperatedOrNot = String.IsNullOrEmpty(ITR_8170_002A_IndifictionLamp.VoltmeterforIncomerABOperatedOrNot) ? true : false;
                    ITR_8170_002A_IndifictionLamp = ITR_8170_002A_IndifictionLamp;

                    ITR8170_002A_Voltage_ReadingList[0].IsReqL1_L2 = String.IsNullOrEmpty(ITR8170_002A_Voltage_ReadingList[0].L1_L2) ? true : false;
                    ITR8170_002A_Voltage_ReadingList[0].IsReqL2_L3 = String.IsNullOrEmpty(ITR8170_002A_Voltage_ReadingList[0].L2_L3) ? true : false;
                    ITR8170_002A_Voltage_ReadingList[0].IsReqL3_L1 = String.IsNullOrEmpty(ITR8170_002A_Voltage_ReadingList[0].L3_L1) ? true : false;
                    ITR8170_002A_Voltage_ReadingList[0].IsReqL1_N = String.IsNullOrEmpty(ITR8170_002A_Voltage_ReadingList[0].L1_N) ? true : false;
                    ITR8170_002A_Voltage_ReadingList[0].IsReqL2_N = String.IsNullOrEmpty(ITR8170_002A_Voltage_ReadingList[0].L2_N) ? true : false;
                    ITR8170_002A_Voltage_ReadingList[0].IsReqL3_N = String.IsNullOrEmpty(ITR8170_002A_Voltage_ReadingList[0].L3_N) ? true : false;
                    ITR8170_002A_Voltage_ReadingList[0].IsReqPhaseRotation = String.IsNullOrEmpty(ITR8170_002A_Voltage_ReadingList[0].PhaseRotation) ? true : false;
                    ITR8170_002A_Voltage_ReadingList = ITR8170_002A_Voltage_ReadingList;

                    result = false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }
        public async Task<bool> SaveRecord_8300_003AData()
        {
            bool result = false;
            try
            {
                //bool Body = !String.IsNullOrEmpty(ITRRecord8211_002A.MotorRatedPower) && !String.IsNullOrEmpty(ITRRecord8211_002A.RatedVolt) && !String.IsNullOrEmpty(ITRRecord8211_002A.StartingCurrent) && !String.IsNullOrEmpty(ITRRecord8211_002A.RotationSpeed) && !String.IsNullOrEmpty(ITRRecord8211_002A.Inch);

                List<T_ITR_8300_003A_Illumin> A_Illumi8300_003 = ITR_8300_003A_IlluminList.Where(x => String.IsNullOrEmpty(x.LUX1) || String.IsNullOrEmpty(x.LUX2) ||
                String.IsNullOrEmpty(x.LUX3) || String.IsNullOrEmpty(x.LUX4)).ToList();
                List<T_ITRInstrumentData> ITRInstrumentData = ITRInstrumentDataList.Where(x => !String.IsNullOrEmpty(x.TestEquipment)).ToList();
                if ((A_Illumi8300_003.Any() || ITRInstrumentData.Any()))
                {



                    ITR_8300_003A_IlluminList.ForEach(x =>
                    {
                        foreach (var propertyInfo in x.GetType().GetProperties())
                        {
                            if (propertyInfo.PropertyType == typeof(string))
                            {
                                if (propertyInfo.GetValue(x, null) == null)
                                {
                                    propertyInfo.SetValue(x, string.Empty, null);
                                }
                            }
                        }
                    });

                    await _ITR_8300_003A_IlluminRepository.InsertOrReplaceAsync(ITR_8300_003A_IlluminList);

                    ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
                    await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentDataList.ToList());
                    UpdateHeaderFootterdata();
                    result = true;
                }
                else
                {


                    List<T_ITR_8300_003A_Illumin> validate8300003a = ITR_8300_003A_IlluminList.ToList();
                    validate8300003a.Where(x => (String.IsNullOrEmpty(x.LUX1))).ForEach(x => x.IsReq = true);
                    validate8300003a.Where(x => (String.IsNullOrEmpty(x.LUX2))).ForEach(x => x.IsReq = true);
                    validate8300003a.Where(x => (String.IsNullOrEmpty(x.LUX3))).ForEach(x => x.IsReq = true);
                    validate8300003a.Where(x => (String.IsNullOrEmpty(x.LUX4))).ForEach(x => x.IsReq = true);


                    ValidatetrumentData();
                    ITR_8300_003A_IlluminList = new ObservableCollection<T_ITR_8300_003A_Illumin>(validate8300003a);
                    result = false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }

        public async Task<bool> SaveRecord_8170_007AData()
        {
            bool result = false;
            try
            {
                bool IsAnyRecordNull1 = !String.IsNullOrEmpty(ITR_8170_007AOP_FT.InsResCheck) && !String.IsNullOrEmpty(ITR_8170_007AOP_FT.AlarmTest)
                                   && !String.IsNullOrEmpty(ITR_8170_007AOP_FT.LocalSignal) && !String.IsNullOrEmpty(ITR_8170_007AOP_FT.MotorSignal)
                                   && !String.IsNullOrEmpty(ITR_8170_007AOP_FT.PanelSpaceHeaterFn) && !String.IsNullOrEmpty(ITR_8170_007AOP_FT.PhaseRotation)
                                   && !String.IsNullOrEmpty(ITR_8170_007AOP_FT.RemoteSignal) && !String.IsNullOrEmpty(ITR_8170_007AOP_FT.SettingValCheck)
                                   && !String.IsNullOrEmpty(ITR_8170_007AOP_FT.SignalOP12mA) && !String.IsNullOrEmpty(ITR_8170_007AOP_FT.SignalOP16mA)
                                   && !String.IsNullOrEmpty(ITR_8170_007AOP_FT.SignalOP20mA) && !String.IsNullOrEmpty(ITR_8170_007AOP_FT.SignalOP4mA)
                                   && !String.IsNullOrEmpty(ITR_8170_007AOP_FT.SignalOP8mA) && !String.IsNullOrEmpty(ITR_8170_007AOP_FT.SuppVolDisL1L2)
                                   && !String.IsNullOrEmpty(ITR_8170_007AOP_FT.SuppVolDisL2L3) && !String.IsNullOrEmpty(ITR_8170_007AOP_FT.SuppVolDisL3L1);
                List<T_ITRInstrumentData> ITRInstrumentData = ITRInstrumentDataList.Where(x => String.IsNullOrEmpty(x.TestEquipment)).ToList();
                if (IsAnyRecordNull1 && !ITRInstrumentData.Any())
                {
                    foreach (var propertyInfo in ITR_8170_007AOP_FT.GetType().GetProperties())
                    {
                        if (propertyInfo.PropertyType == typeof(string))
                        {
                            if (propertyInfo.GetValue(ITR_8170_007AOP_FT, null) == null)
                            {
                                propertyInfo.SetValue(ITR_8170_007AOP_FT, string.Empty, null);
                            }
                        }
                    }

                    await _ITR_8170_007A_OP_Function_TestRepository.InsertOrReplaceAsync(ITR_8170_007AOP_FT);
                    ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
                    await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentDataList.ToList());

                    UpdateHeaderFootterdata();
                    result = true;
                }
                else
                {
                    ITR_8170_007AOP_FT.IsAlarmTest = String.IsNullOrEmpty(ITR_8170_007AOP_FT.AlarmTest) ? true : false;
                    ITR_8170_007AOP_FT.IsInsResCheck = String.IsNullOrEmpty(ITR_8170_007AOP_FT.InsResCheck) ? true : false;
                    ITR_8170_007AOP_FT.IsLocalSignal = String.IsNullOrEmpty(ITR_8170_007AOP_FT.LocalSignal) ? true : false;
                    ITR_8170_007AOP_FT.IsMotorSignal = String.IsNullOrEmpty(ITR_8170_007AOP_FT.MotorSignal) ? true : false;
                    ITR_8170_007AOP_FT.IsPanelSpaceHeaterFn = String.IsNullOrEmpty(ITR_8170_007AOP_FT.PanelSpaceHeaterFn) ? true : false;
                    ITR_8170_007AOP_FT.IsPhaseRotation = String.IsNullOrEmpty(ITR_8170_007AOP_FT.PhaseRotation) ? true : false;
                    ITR_8170_007AOP_FT.IsRemoteSignal = String.IsNullOrEmpty(ITR_8170_007AOP_FT.RemoteSignal) ? true : false;
                    ITR_8170_007AOP_FT.IsSettingValCheck = String.IsNullOrEmpty(ITR_8170_007AOP_FT.SettingValCheck) ? true : false;
                    ITR_8170_007AOP_FT.IsSignalOP12mA = String.IsNullOrEmpty(ITR_8170_007AOP_FT.SignalOP12mA) ? true : false;
                    ITR_8170_007AOP_FT.IsSignalOP16mA = String.IsNullOrEmpty(ITR_8170_007AOP_FT.SignalOP16mA) ? true : false;
                    ITR_8170_007AOP_FT.IsSignalOP20mA = String.IsNullOrEmpty(ITR_8170_007AOP_FT.SignalOP20mA) ? true : false;
                    ITR_8170_007AOP_FT.IsSignalOP4mA = String.IsNullOrEmpty(ITR_8170_007AOP_FT.SignalOP4mA) ? true : false;
                    ITR_8170_007AOP_FT.IsSignalOP8mA = String.IsNullOrEmpty(ITR_8170_007AOP_FT.SignalOP8mA) ? true : false;
                    ITR_8170_007AOP_FT.IsSuppVolDisL1L2 = String.IsNullOrEmpty(ITR_8170_007AOP_FT.SuppVolDisL1L2) ? true : false;
                    ITR_8170_007AOP_FT.IsSuppVolDisL2L3 = String.IsNullOrEmpty(ITR_8170_007AOP_FT.SuppVolDisL2L3) ? true : false;
                    ITR_8170_007AOP_FT.IsSuppVolDisL3L1 = String.IsNullOrEmpty(ITR_8170_007AOP_FT.SuppVolDisL3L1) ? true : false;
                    ITR_8170_007AOP_FT = ITR_8170_007AOP_FT;
                    ValidatetrumentData();
                    result = false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }

        public async void GetRemarksData()
        {
            RemarkEntry = CommonHeaderFooter.Remarks;
        }
        public async void SaveRemarksData()
        {
            CommonHeaderFooter.Remarks = RemarkEntry;
            await _CommonHeaderFooterRepository.UpdateAsync(CommonHeaderFooter);
        }
        private async void UpdateHeaderFootterdata()
        {
            //if (String.IsNullOrEmpty(CommonHeaderFooter.CreatedBy))
            //{
            //    CommonHeaderFooter.CreatedBy = Settings.CompletionUserName;
            //    CommonHeaderFooter.CreatedDate = DateTime.Now;
            //}
            //CommonHeaderFooter.UpdatedBy = Settings.CompletionUserName;
            //CommonHeaderFooter.UpdatedDate = DateTime.Now;
            //CommonHeaderFooter.IsUpdated = true;
            CommonHeaderFooter.Started = true;
            CommonHeaderFooter.StartedBy = Settings.CompletionUserName;
            CommonHeaderFooter.StartedOn = DateTime.Now;
            CommonHeaderFooter.RejectedReason = "";
            CommonHeaderFooter.Rejected = false;
            CommonHeaderFooter.rejectedDate = new DateTime(2000, 01, 01);
            CommonHeaderFooter.RejectedUserID = Settings.CompletionUserID;
            CommonHeaderFooter.IsUpdated = true;
            await _CommonHeaderFooterRepository.UpdateAsync(CommonHeaderFooter);
        }
        public async void CalculateTransformerRatioTest(string param, bool IsBind)
        {
            try
            {
                if (!String.IsNullOrEmpty(param))
                {
                    SelectedTransformerRatioTest8121 = TransformerRatioTest8121.Where(x => x.RowID == Convert.ToInt32(param)).FirstOrDefault();
                    if (!String.IsNullOrEmpty(SelectedTransformerRatioTest8121.TapVoltagePrimary))
                        SelectedTransformerRatioTest8121.TapVoltagePrimary = SelectedTransformerRatioTest8121.TapVoltagePrimary;
                    if (!String.IsNullOrEmpty(SelectedTransformerRatioTest8121.TapVoltageSecondary))
                        SelectedTransformerRatioTest8121.TapVoltageSecondary = SelectedTransformerRatioTest8121.TapVoltageSecondary;

                    if (!String.IsNullOrEmpty(SelectedTransformerRatioTest8121.TapVoltagePrimary) || !String.IsNullOrEmpty(SelectedTransformerRatioTest8121.TapVoltageSecondary))
                    {
                        if (Convert.ToDecimal(SelectedTransformerRatioTest8121.TapVoltageSecondary) > 0)
                        {
                            Decimal CalRatio = Convert.ToDecimal(SelectedTransformerRatioTest8121.TapVoltagePrimary) / Convert.ToDecimal(SelectedTransformerRatioTest8121.TapVoltageSecondary);
                            SelectedTransformerRatioTest8121.CalculateRatio = CalRatio.ToString("F4");
                        }
                    }
                    if (!String.IsNullOrEmpty(SelectedTransformerRatioTest8121.TestValueL1Ratio))
                    {
                        SelectedTransformerRatioTest8121.TestValueL1Ratio = Convert.ToDecimal(SelectedTransformerRatioTest8121.TestValueL1Ratio).ToString("F4");
                        if (Convert.ToDecimal(SelectedTransformerRatioTest8121.TestValueL1Ratio) > 0 && Convert.ToDecimal(SelectedTransformerRatioTest8121.CalculateRatio) > 0)
                            SelectedTransformerRatioTest8121.TestValueL1Error = (100 * (Convert.ToDecimal(SelectedTransformerRatioTest8121.TestValueL1Ratio) / Convert.ToDecimal(SelectedTransformerRatioTest8121.CalculateRatio) - 1)).ToString("F4");
                        else
                            SelectedTransformerRatioTest8121.TestValueL1Error = "";
                    }
                    if (!String.IsNullOrEmpty(SelectedTransformerRatioTest8121.TestValueL2Ratio))
                    {
                        SelectedTransformerRatioTest8121.TestValueL2Ratio = Convert.ToDecimal(SelectedTransformerRatioTest8121.TestValueL2Ratio).ToString("F4");
                        if (Convert.ToDecimal(SelectedTransformerRatioTest8121.TestValueL1Ratio) > 0 && Convert.ToDecimal(SelectedTransformerRatioTest8121.CalculateRatio) > 0)
                            SelectedTransformerRatioTest8121.TestValueL2Error = (100 * (Convert.ToDecimal(SelectedTransformerRatioTest8121.TestValueL2Ratio) / Convert.ToDecimal(SelectedTransformerRatioTest8121.CalculateRatio) - 1)).ToString("F4");
                        else
                            SelectedTransformerRatioTest8121.TestValueL2Error = "";
                    }
                    if (!String.IsNullOrEmpty(SelectedTransformerRatioTest8121.TestValueL3Ratio))
                    {
                        SelectedTransformerRatioTest8121.TestValueL3Ratio = Convert.ToDecimal(SelectedTransformerRatioTest8121.TestValueL3Ratio).ToString("F4");
                        if (Convert.ToDecimal(SelectedTransformerRatioTest8121.TestValueL3Ratio) > 0 && Convert.ToDecimal(SelectedTransformerRatioTest8121.CalculateRatio) > 0)
                            SelectedTransformerRatioTest8121.TestValueL3Error = (100 * (Convert.ToDecimal(SelectedTransformerRatioTest8121.TestValueL3Ratio) / Convert.ToDecimal(SelectedTransformerRatioTest8121.CalculateRatio) - 1)).ToString("F4");
                        else
                            SelectedTransformerRatioTest8121.TestValueL3Error = "";
                    }
                    var TRTlist = TransformerRatioTest8121.ToList();
                    if (TRTlist.Count > 3)
                        TRTlist.Skip(3).ForEach(x => x.IsUpdated = true);
                    if (IsBind)
                        TransformerRatioTest8121 = new ObservableCollection<T_ITR8121_004ATransformerRatioTest>(TRTlist);
                }
            }
            catch (Exception e)
            {

            }
        }

        public async void CalculateTransRatioTest8121_002(string param, bool IsUIBind)
        {
            try
            {
                if (!String.IsNullOrEmpty(param))
                {
                    SelectedTransRatioTest8121002 = TransformerRadioTestList.Where(x => x.RowID == Convert.ToInt32(param)).FirstOrDefault();
                    if (!String.IsNullOrEmpty(SelectedTransRatioTest8121002.TapVoltagePrimary))
                        SelectedTransRatioTest8121002.TapVoltagePrimary = Convert.ToDecimal(SelectedTransRatioTest8121002.TapVoltagePrimary).ToString("F3");
                    if (!String.IsNullOrEmpty(SelectedTransRatioTest8121002.TapVoltageSecondary))
                        SelectedTransRatioTest8121002.TapVoltageSecondary = Convert.ToDecimal(SelectedTransRatioTest8121002.TapVoltageSecondary).ToString("F3");

                    if (!String.IsNullOrEmpty(SelectedTransRatioTest8121002.TapVoltagePrimary) || !String.IsNullOrEmpty(SelectedTransRatioTest8121002.TapVoltageSecondary))
                    {
                        if (Convert.ToDecimal(SelectedTransRatioTest8121002.TapVoltageSecondary) > 0)
                        {
                            Decimal CalRatio = Convert.ToDecimal(SelectedTransRatioTest8121002.TapVoltagePrimary) / Convert.ToDecimal(SelectedTransRatioTest8121002.TapVoltageSecondary);
                            SelectedTransRatioTest8121002.CalculatedRatio = CalRatio.ToString("F3");
                        }

                    }
                    if (!String.IsNullOrEmpty(SelectedTransRatioTest8121002.TestValueL1Ratio))
                    {
                        SelectedTransRatioTest8121002.TestValueL1Ratio = Convert.ToDecimal(SelectedTransRatioTest8121002.TestValueL1Ratio).ToString("F3");
                        if (Convert.ToDecimal(SelectedTransRatioTest8121002.TestValueL1Ratio) > 0 && Convert.ToDecimal(SelectedTransRatioTest8121002.CalculatedRatio) > 0)
                            SelectedTransRatioTest8121002.TestValueL1Error = (100 * (Convert.ToDecimal(SelectedTransRatioTest8121002.TestValueL1Ratio) / Convert.ToDecimal(SelectedTransRatioTest8121002.CalculatedRatio) - 1)).ToString("F3");
                        else
                            SelectedTransRatioTest8121002.TestValueL1Error = "";
                    }
                    if (!String.IsNullOrEmpty(SelectedTransRatioTest8121002.TestValueL2Ratio))
                    {
                        SelectedTransRatioTest8121002.TestValueL2Ratio = Convert.ToDecimal(SelectedTransRatioTest8121002.TestValueL2Ratio).ToString("F3");
                        if (Convert.ToDecimal(SelectedTransRatioTest8121002.TestValueL1Ratio) > 0 && Convert.ToDecimal(SelectedTransRatioTest8121002.CalculatedRatio) > 0)
                            SelectedTransRatioTest8121002.TestValueL2Error = (100 * (Convert.ToDecimal(SelectedTransRatioTest8121002.TestValueL2Ratio) / Convert.ToDecimal(SelectedTransRatioTest8121002.CalculatedRatio) - 1)).ToString("F3");
                        else
                            SelectedTransRatioTest8121002.TestValueL2Error = "";
                    }
                    if (!String.IsNullOrEmpty(SelectedTransRatioTest8121002.TestValueL3Ratio))
                    {
                        SelectedTransRatioTest8121002.TestValueL3Ratio = Convert.ToDecimal(SelectedTransRatioTest8121002.TestValueL3Ratio).ToString("F3");
                        if (Convert.ToDecimal(SelectedTransRatioTest8121002.TestValueL3Ratio) > 0 && Convert.ToDecimal(SelectedTransRatioTest8121002.CalculatedRatio) > 0)
                            SelectedTransRatioTest8121002.TestValueL3Error = (100 * (Convert.ToDecimal(SelectedTransRatioTest8121002.TestValueL3Ratio) / Convert.ToDecimal(SelectedTransRatioTest8121002.CalculatedRatio) - 1)).ToString("F3");
                        else
                            SelectedTransRatioTest8121002.TestValueL3Error = "";
                    }
                    var TRTlist = TransformerRadioTestList.ToList();
                    if (TRTlist.Count > 3)
                        TRTlist.Skip(3).ForEach(x => x.IsUpdated = true);

                    if (IsUIBind)
                        TransformerRadioTestList = new ObservableCollection<T_ITR8121_002A_TransformerRadioTest>(TRTlist);
                }
            }
            catch (Exception e)
            {

            }
        }

        public async void CalculateCT8100_001A()
        {
            try
            {
                if (SelectedCTdata == null) return;

                if (!String.IsNullOrEmpty(SelectedCTdata.PrimaryCurrent) && !String.IsNullOrEmpty(SelectedCTdata.SecondaryCurrent))
                {
                    if (Convert.ToDecimal(SelectedCTdata.SecondaryCurrent) >= 0)
                    {
                        Decimal CalRatio = (Convert.ToDecimal(SelectedCTdata.PrimaryCurrent) / Convert.ToDecimal(SelectedCTdata.SecondaryCurrent)) * (1000);
                        SelectedCTdata.Ratio = CalRatio.ToString("F2");
                        ITR8100_001A_CTdata.Where(x => x.RowID == SelectedCTdata.RowID).FirstOrDefault().Ratio = SelectedCTdata.Ratio;
                    }
                    else
                        SelectedCTdata.Ratio = "0.00";
                }
                else if (String.IsNullOrEmpty(SelectedCTdata.PrimaryCurrent) && !String.IsNullOrEmpty(SelectedCTdata.SecondaryCurrent))
                    SelectedCTdata.Ratio = "0.00";
                else
                    SelectedCTdata.Ratio = "";
                ITR8100_001A_CTdata = new ObservableCollection<T_ITR8100_001A_CTdata>(ITR8100_001A_CTdata);

            }
            catch (Exception e)
            {

            }
        }

        public void CalculateCTRatio8100_001A(string param)
        {
            try
            {
                if (!String.IsNullOrEmpty(param))
                {
                    SelectedRedioTest = ITR8100_001A_RatioTest.Where(x => x.RowID == Convert.ToInt32(param)).FirstOrDefault();
                    if (SelectedRedioTest == null)
                        return;
                    if (!String.IsNullOrEmpty(SelectedRedioTest.PrimaryCurrent) && !String.IsNullOrEmpty(SelectedRedioTest.SecondaryCurrent))
                    {
                        if (SelectedCTdata == null)
                            SelectedCTdata = ITR8100_001A_CTdata.Where(x => x.RowID == Convert.ToInt32(param)).FirstOrDefault();
                        if (Convert.ToDecimal(SelectedCTdata.SecondaryCurrent) >= 0)
                        {
                            string CTRatio = ITR8100_001A_CTdata.Where(x => x.RowNo == SelectedRedioTest.RowNo).Select(x => x.Ratio).FirstOrDefault();
                            Decimal CRatio = (Convert.ToDecimal(SelectedRedioTest.PrimaryCurrent) / Convert.ToDecimal(SelectedRedioTest.SecondaryCurrent)) * (1000);
                            Decimal CalculatedRatio = Convert.ToDecimal(CRatio.ToString("F2"));
                            if (String.IsNullOrEmpty(CTRatio))
                                CTRatio = "0";
                            if (Convert.ToDecimal(CTRatio) <= 0)
                                SelectedRedioTest.CalculatedCTRatio = CalculatedRatio + " (Infinity%)";
                            else
                            {
                                Decimal Ratio = Convert.ToDecimal(CTRatio);
                                var err = (CalculatedRatio - Ratio) * 100 / Ratio;
                                var finalErr = err.ToString("F2");
                                SelectedRedioTest.CalculatedCTRatio = CalculatedRatio + " (Err. " + finalErr + "%)";
                            }
                        }
                        else
                            SelectedRedioTest.CalculatedCTRatio = "0.00";
                    }
                    else if (String.IsNullOrEmpty(SelectedRedioTest.PrimaryCurrent) && !String.IsNullOrEmpty(SelectedRedioTest.SecondaryCurrent))
                        SelectedRedioTest.CalculatedCTRatio = "0.00";
                    else
                        SelectedRedioTest.CalculatedCTRatio = "";

                    ITR8100_001A_RatioTest = new ObservableCollection<T_ITR8100_001A_RatioTest>(ITR8100_001A_RatioTest.ToList());
                }
            }
            catch (Exception e)
            {

            }
        }
        private async void GetITRInstrumentData()
        {
            var TestEquipmentData = await _TestEquipmentDataRepository.GetAsync(x => x.ProjectID == Settings.ProjectID);
            List<TestEquipmentDataModel> TestEquipmentList = new List<TestEquipmentDataModel>();
            TestEquipmentList.Add(new TestEquipmentDataModel { ID = 0, TestEquipmentDataString = "" });
            if (TestEquipmentData.Count > 0)
            {

                foreach (T_TestEquipmentData TE in TestEquipmentData)
                {
                    string FullData = TE.Description + "  -  " + TE.Instrument + "  -  " + TE.Serial + "  -  " + TE.CalibrationDate.ToString("dd-MM-yyyy");

                    TestEquipmentList.Add(new TestEquipmentDataModel { ID = TE.ID, TestEquipmentDataString = FullData });
                }
            }
            TestEquipmentDataModelList = new ObservableCollection<TestEquipmentDataModel>(TestEquipmentList);
        }
        private async void BindIstrumentData()
        {
            var ITRInstrumentData = await _ITRInstrumentDataRepository.QueryAsync("SELECT * FROM T_ITRInstrumentData WHERE ITRCommonID = '" + CommonHeaderFooter.ID + "' AND CommonRowID =  '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID);
            if (ITRInstrumentData.Count <= 0)
            {
                var Instrumentdata = await _ITRInstrumentDataRepository.GetAsync();
                long InstID = 1;
                if (Instrumentdata.Count > 0)
                    InstID = Instrumentdata.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                T_ITRInstrumentData newItem = new T_ITRInstrumentData { RowID = InstID, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, ID = 0, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID };
                ITRInstrumentData.Add(newItem);
                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(newItem);
            }
            int i = 1;
            ITRInstrumentData.ForEach(x => { x.No = i++; x.TestEquipment = TestEquipmentDataModelList.Where(p => p.ID == x.CCMS_EquipmentID).Select(q => q.TestEquipmentDataString).FirstOrDefault(); x.TestEquipmentDataList = TestEquipmentDataModelList.Select(q => q.TestEquipmentDataString).ToList(); });
            ITRInstrumentData.Skip(1).ForEach(x => x.IsDeletable = true);
            ITRInstrumentDataList = new ObservableCollection<T_ITRInstrumentData>(ITRInstrumentData);
        }
        private async void ValidatetrumentData()
        {
            ITRInstrumentDataList.ToList().ForEach(x => { x.CCMS_EquipmentID = TestEquipmentDataModelList.Where(p => p.TestEquipmentDataString == x.TestEquipment).Select(q => q.ID).FirstOrDefault(); });
            await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRInstrumentDataList.ToList());

            var ITRInstrumentData = await _ITRInstrumentDataRepository.QueryAsync("SELECT * FROM T_ITRInstrumentData WHERE ITRCommonID = '" + CommonHeaderFooter.ID + "' AND CommonRowID =  '" + CommonHeaderFooter.ROWID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID);
            if (ITRInstrumentData.Count <= 0)
            {
                var Instrumentdata = await _ITRInstrumentDataRepository.GetAsync();
                long InstID = 1;
                if (Instrumentdata.Count > 0)
                    InstID = Instrumentdata.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                T_ITRInstrumentData newItem = new T_ITRInstrumentData { RowID = InstID, CommonRowID = CommonHeaderFooter.ROWID, ITRCommonID = CommonHeaderFooter.ID, ID = 0, ModelName = Settings.ModelName, CCMS_HEADERID = (int)CommonHeaderFooter.ID };
                ITRInstrumentData.Add(newItem);
                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(newItem);
            }
            int i = 1;
            ITRInstrumentData.ForEach(x => { x.No = i++; x.TestEquipment = TestEquipmentDataModelList.Where(p => p.ID == x.CCMS_EquipmentID).Select(q => q.TestEquipmentDataString).FirstOrDefault(); x.TestEquipmentDataList = TestEquipmentDataModelList.Select(q => q.TestEquipmentDataString).ToList(); x.IsReqCCMS_EquipmentID = String.IsNullOrEmpty(x.TestEquipment) ? true : false; });
            ITRInstrumentData.Skip(1).ForEach(x => x.IsDeletable = true);
            ITRInstrumentDataList = new ObservableCollection<T_ITRInstrumentData>(ITRInstrumentData);
        }
        private int GetIntValueFromStringVoltage(string VoltageStr)
        {
            int ResultValue;
            try
            {
                string digits = VoltageStr.Replace("kV", "");
                ResultValue = Convert.ToInt32(digits);
            }
            catch (Exception ex)
            {
                ResultValue = 0;
            }
            return ResultValue;
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            App.IsBusy = false;
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
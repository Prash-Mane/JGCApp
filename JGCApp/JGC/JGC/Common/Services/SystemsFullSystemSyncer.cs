using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables.Completions;
using JGC.DataBase.DataTables.ITR;
using JGC.Models;
using JGC.Models.Completions;
using JGC.Models.ITR;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JGC.Common.Services
{
    public class SystemsFullSystemSyncer : ISystemsFullSystemSyncercs
    {
        private readonly IUserDialogs _userDialogs;
        private readonly ITRService _ITRService;
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
        private readonly IRepository<T_ITR8140_001A_ContactResisTest> _T_ITR8140_001A_ContactResisTestRepository;
        private readonly IRepository<T_ITR8140_001AInsulationResistanceTest> _T_ITR8140_001AInsulationResistanceTestRepository;
        private readonly IRepository<T_ITR8140_001ADialectricTest> _T_ITR8140_001ADialectricTestRepository;
        private readonly IRepository<T_ITR8140_001ATestInstrucitonData> _T_ITR8140_001ATestInstrumentDataRepository;
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


        private Dictionary<string, List<string>> tagToSheetList = new Dictionary<string, List<string>>();
        private List<CheckSheetAnswerModel> CheckSheetAnswerModelList = new List<CheckSheetAnswerModel>();

        public SQLiteConnection conn;
        public List<T_DataRefs> SystemsList { get; set; }

        public SystemsFullSystemSyncer(
         ITRService _ITRService,
         IUserDialogs _userDialogs,
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
         IRepository<T_TAG_SHEET_HEADER> _TAG_SHEET_HEADERRepository,
         IRepository<T_ITRCommonHeaderFooter> _CommonHeaderFooterRepository,
         IRepository<T_ITRRecords_30A_31A> _RecordsRepository,
         IRepository<T_ITRTubeColors> _TubeColorsRepository,
         IRepository<T_ITRRecords_040A_041A_042A> _Records_04XARepository,
         IRepository<T_ITRInsulationDetails> _InsulationDetailsRepository,
         IRepository<T_CommonHeaderFooterSignOff> _CommonHeaderFooterSignOffRepository,
         IRepository<T_ITR8000_003ARecords> _Records_8000003ARepository,
         IRepository<T_ITR8000_003A_AcceptanceCriteria> _Records_8000003A_AcceptanceCriteriaRepository,

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
         IRepository<T_ITR_8170_007A_OP_Function_Test> _ITR_8170_007A_OP_Function_TestRepository)
        {
            this._userDialogs = _userDialogs;
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
            this._CommonHeaderFooterRepository = _CommonHeaderFooterRepository;
            this._RecordsRepository = _RecordsRepository;
            this._TubeColorsRepository = _TubeColorsRepository;
            this._Records_04XARepository = _Records_04XARepository;
            this._InsulationDetailsRepository = _InsulationDetailsRepository;
            this._Records_8000003ARepository = _Records_8000003ARepository;
            this._Records_8000003A_AcceptanceCriteriaRepository = _Records_8000003A_AcceptanceCriteriaRepository;
            this._CommonHeaderFooterSignOffRepository = _CommonHeaderFooterSignOffRepository;
            this._T_ITR8140_001A_ContactResisTestRepository = _T_ITR8140_001A_ContactResisTestRepository;
            this._T_ITR8140_001ADialectricTestRepository = _T_ITR8140_001ADialectricTestRepository;
            this._T_ITR8140_001ATestInstrumentDataRepository = _T_ITR8140_001ATestInstrumentDataRepository;
            this._T_ITR8140_001AInsulationResistanceTestRepository = _T_ITR8140_001AInsulationResistanceTestRepository;

            this._Records_080A_09XARepository = _Records_080A_09XARepository;
            this._ITR8100_001A_CTdataRepository = _ITR8100_001A_CTdataRepository;
            this._ITR8100_001A_IRTestRepository = _ITR8100_001A_IRTestRepository;
            this._ITR8100_001A_RatioTestRepository = _ITR8100_001A_RatioTestRepository;
            this._ITR8100_001A_TIDataRepository = _ITR8100_001A_TIDataRepository;
            this._ITRRecords_8100_002ARepository = _ITRRecords_8100_002ARepository;
            this._ITRRecords_8100_002A_InsRes_TestRepository = _ITRRecords_8100_002A_InsRes_TestRepository;
            this._ITRRecords_8100_002A_Radio_TestRepository = _ITRRecords_8100_002A_Radio_TestRepository;
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
            this._ITRInstrumentDataRepository = _ITRInstrumentDataRepository;
            this._ITR8161_002A_BodyRepository = _ITR8161_002A_BodyRepository;
            this._ITR8161_002A_DielectricTestRepository = _ITR8161_002A_DielectricTestRepository;
            this._ITR8000_101A_GeneralnformationRepository = _ITR8000_101A_GeneralnformationRepository;
            this._ITR8000_101A_RecordISBarrierDetailsRepository = _ITR8000_101A_RecordISBarrierDetailsRepository; 
            this._ITRRecords_8211_002A_BodyRepository = _ITRRecords_8211_002A_BodyRepository;
            this._ITRRecords_8211_002A_RunTestRepository = _ITRRecords_8211_002A_RunTestRepository;
            this._ITR7000_101AHarmony_GenlnfoRepository = _ITR7000_101AHarmony_GenlnfoRepository;
            this._ITR7000_101AHarmony_ActivityDetailsRepository = _ITR7000_101AHarmony_ActivityDetailsRepository;
            this._Ccms_signatureRepository = _Ccms_signatureRepository;
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

            conn = DependencyService.Get<ISQLite>().GetConnection();
        }

        public async Task<bool> uploadChanges()
        {
            var allTags = await _TAGRepository.GetAsync();
            foreach (T_TAG tag in allTags)
            {
                var CehckSheetPerTAgs = await _CHECKSHEET_PER_TAGRepository.GetAsync(x => x.TAGNAME == tag.name);
                foreach (var checkSheet in CehckSheetPerTAgs)
                {
                    if (await _ITRService.ITR_3XA(checkSheet.CHECKSHEETNAME))
                        await Post_030A_031A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_4XA(checkSheet.CHECKSHEETNAME))
                        await Post_040A_041A_042A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_80_9XA(checkSheet.CHECKSHEETNAME))
                        await Post_080A_090A_091A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_8XA(checkSheet.CHECKSHEETNAME))
                        await Post_8000_003A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_8100_001A(checkSheet.CHECKSHEETNAME))
                        await Post_8100_001A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_81_2XA(checkSheet.CHECKSHEETNAME))
                        await Post_8100_002A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_8140_001A(checkSheet.CHECKSHEETNAME))
                        await Post_8140_001_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_8121_002A(checkSheet.CHECKSHEETNAME))
                        await Post_8121_002A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_8260_002A(checkSheet.CHECKSHEETNAME))
                        await Post_8260_002A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_8161_1XA(checkSheet.CHECKSHEETNAME))
                        await Post_8161_001A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_8121_004XA(checkSheet.CHECKSHEETNAME))
                        await Post_8121_004A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_8161_2XA(checkSheet.CHECKSHEETNAME))
                        await Post_8161_002A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_8000_101A(checkSheet.CHECKSHEETNAME))
                        await Post_8000_101A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_8211_002A(checkSheet.CHECKSHEETNAME))
                        await Post_8211_002A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_7000_101AHarmony(checkSheet.CHECKSHEETNAME))
                        await Post_7000_101AHarmony_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_8140_002A(checkSheet.CHECKSHEETNAME))
                        await Post_8140_002A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_8140_004A(checkSheet.CHECKSHEETNAME))
                        await Post_8140_004A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_8170_002A(checkSheet.CHECKSHEETNAME))
                        await Post_8170_002A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_8300_003A(checkSheet.CHECKSHEETNAME))
                        await Post_8300_003A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);
                    else if (await _ITRService.ITR_8170_007A(checkSheet.CHECKSHEETNAME))
                        await Post_8170_007A_ITRsData(tag.name, checkSheet.CHECKSHEETNAME);

                    var _unsyncedAnswers = await _TAG_SHEET_ANSWERRepository.GetAsync(x => x.IsSynced == false);
                    var unsyncedAnswers = _unsyncedAnswers.Where(x => x.tagName.Trim().ToLower() == tag.name.Trim().ToLower() && x.checkSheetName.Trim().ToLower() == checkSheet.CHECKSHEETNAME.Trim().ToLower());
                    var sdf = Settings.CurrentDB;
                    var _unsyncedSignOffs = await _SignOffHeaderRepository.GetAsync(x => x.IsSynced);
                    var unsyncedSignOffs = _unsyncedSignOffs.Where(x => x.SignOffTag.Trim().ToLower() == tag.name.Trim().ToLower() && x.SignOffChecksheet.Trim().ToLower() == checkSheet.CHECKSHEETNAME.Trim().ToLower());

                    var CommonHeaderFooterSignOff = await _CommonHeaderFooterSignOffRepository.GetAsync(x=> x.IsSynced == true);
                    CommonHeaderFooterSignOff = CommonHeaderFooterSignOff.Where(x => x.SignOffTag.Trim().ToLower() == tag.name.Trim().ToLower() && x.SignOffChecksheet.Trim().ToLower() == checkSheet.CHECKSHEETNAME.Trim().ToLower()).ToList();

                    string Remarks = "";
                    string SQL = @"SELECT * FROM T_CHECKSHEET_QUESTION WHERE [checksheet] = '" + checkSheet.CHECKSHEETNAME + "' AND ( section = 'REMARKS' or section = 'GENERAL REMARKS')";
                    var CHECKSHEET_Data = await _CHECKSHEET_QUESTIONRepository.QueryAsync(SQL);
                    if (CHECKSHEET_Data.Any())
                    {
                        T_CHECKSHEET_QUESTION item = CHECKSHEET_Data.FirstOrDefault();
                        string RemarkSQL = @"SELECT * FROM T_TAG_SHEET_ANSWER WHERE [checkSheetName] = '" + checkSheet.CHECKSHEETNAME + "'"
                                             + " AND [itemno] = '" + item.itemNo + "' AND [tagName] = '" + tag.name + "'";
                        var TAG_SHEET_Data = await _TAG_SHEET_ANSWERRepository.QueryAsync(RemarkSQL);
                        if (TAG_SHEET_Data.Any())
                        {
                            T_TAG_SHEET_ANSWER TAGitem = TAG_SHEET_Data.FirstOrDefault();
                            Remarks = TAGitem.checkValue;
                        }
                    }

                    if (unsyncedAnswers.Count() > 0 || unsyncedSignOffs.Count() > 0)
                    {
                        CheckSheetUploadDTO uploadDTO = new CheckSheetUploadDTO();
                        uploadDTO.sheetName = checkSheet.CHECKSHEETNAME;
                        uploadDTO.tagName = tag.name;
                        uploadDTO.jobPack = "";
                        uploadDTO.modelName = Settings.ModelName;
                        uploadDTO.projectName = Settings.ProjectName;
                        uploadDTO.answers = unsyncedAnswers.Select(x =>
                        new Answer
                        {
                            checkValue = x.checkValue,
                            itemno = x.itemno,
                            isDate = x.isDate,
                            isChecked = x.isChecked,
                            completedBy = x.completedBy,
                            completedOn = x.completedOn,
                            synced = x.IsSynced,
                            jobPack = x.jobPack

                        }).ToList();

                        uploadDTO.signOffs = unsyncedSignOffs.Select(x =>
                        new SignOff
                        {
                            count = x.Count,
                            fullName = x.FullName,
                            jobPack = "",
                            signDate = x.SignDate,
                            synced = x.IsSynced,
                            title = x.Title,
                            rejected = x.Rejected,
                            rejectedReason = x.RejectedReason
                        }).ToList();  //.Where(x => x.SignOffTag == tag.name && x.SignOffChecksheet == checkSheetTag.name).ToList();

                        string query = "SELECT * FROM T_TAG_SHEET_HEADER WHERE ChecksheetName = '" + checkSheet.CHECKSHEETNAME + "' AND TagName = '" + checkSheet.TAGNAME + "'";
                        var HeaderData = await _TAG_SHEET_HEADERRepository.QueryAsync(query);
                        uploadDTO.DwgNo = HeaderData.Where(x => x.ColumnKey == "Drawing").Select(s => s.ColumnValue).First();
                        uploadDTO.DwgRevNo = HeaderData.Where(x => x.ColumnKey == "DwgRevNo").Select(s => s.ColumnValue).First();

                        WebServicePost("cctrAnswer?projectName=" + Settings.ProjectName + "&modelname=" + Settings.ModelName + "&db=" + Settings.CurrentDB, JsonConvert.SerializeObject(uploadDTO), Settings.CompletionAccessToken);

                        var _SignOffHeader = await _SignOffHeaderRepository.GetAsync();
                        var SignOffs = _SignOffHeader.Where(x => x.SignOffTag.Trim().ToLower() == tag.name.Trim().ToLower() && x.SignOffChecksheet.Trim().ToLower() == checkSheet.CHECKSHEETNAME.Trim().ToLower());
                        List<SignOff> _SignOffsHeader = SignOffs.Select(x =>
                        new SignOff
                        {
                            count = x.Count,
                            fullName = x.FullName,
                            jobPack = "",
                            signDate = x.SignDate,
                            synced = x.IsSynced,
                            title = x.Title,
                            rejected = x.Rejected,
                            rejectedReason = x.RejectedReason
                        }).ToList();

                        CompletionsTagITRStatus uploadCmsDto = new CompletionsTagITRStatus();
                        uploadCmsDto.ITR_NO = checkSheet.CHECKSHEETNAME;
                        uploadCmsDto.Itr_Report_No = tag.name;
                        uploadCmsDto.JOB_CODE_KEY = Settings.JobCode;
                        uploadCmsDto.REJECTED_DATE = null;
                        if (_SignOffsHeader.Any() && unsyncedAnswers.Any())
                        {
                            uploadCmsDto.TAG_NO = tag.name;
                            uploadCmsDto.SIGN_ID_NO_1 = "";
                            uploadCmsDto.SIGN_ID_NO_2 = "";
                            uploadCmsDto.SIGN_ID_NO_3 = "";
                            uploadCmsDto.SIGN_ID_NO_4 = "";
                            uploadCmsDto.SIGN_ID_NO_5 = "";
                            uploadCmsDto.SIGN_ID_NO_6 = "";
                            uploadCmsDto.SIGN_ID_NO_7 = "";
                            uploadCmsDto.SIGN_DATE_1 = null;
                            uploadCmsDto.SIGN_DATE_2 = null;
                            uploadCmsDto.SIGN_DATE_3 = null;
                            uploadCmsDto.SIGN_DATE_4 = null;
                            uploadCmsDto.SIGN_DATE_5 = null;
                            uploadCmsDto.SIGN_DATE_6 = null;
                            uploadCmsDto.SIGN_DATE_7 = null;
                            uploadCmsDto.REMARKS = "";
                            uploadCmsDto.REJECTED_USER = "";
                            uploadCmsDto.REJECT_REMARKS = "";
                            uploadCmsDto.REJECTED_DATE = null;
                            uploadCmsDto.UPDATED_ID = Settings.UserID.ToString();
                            uploadCmsDto.UPDATED_DATE = DateTime.Now;

                            if (_SignOffsHeader.FirstOrDefault().rejected)
                            {
                                uploadCmsDto.REJECTED_DATE = _SignOffsHeader.FirstOrDefault().signDate;
                                uploadCmsDto.REJECTED_USER = Settings.UserID.ToString();
                                uploadCmsDto.REJECT_REMARKS = _SignOffsHeader.FirstOrDefault().rejectedReason;
                            }
                            else
                            {
                                foreach (SignOff signature in _SignOffsHeader)
                                {
                                    if (String.IsNullOrEmpty(signature.fullName)) continue;
                                    switch (signature.count)
                                    {
                                        case 1:
                                            uploadCmsDto.SIGN_ID_NO_1 = signature.fullName;
                                            uploadCmsDto.SIGN_DATE_1 = signature.signDate;
                                            uploadCmsDto.REMARKS = !String.IsNullOrEmpty(Remarks) ? Remarks : "";
                                            break;
                                        case 2:
                                            uploadCmsDto.SIGN_ID_NO_2 = signature.fullName;
                                            uploadCmsDto.SIGN_DATE_2 = signature.signDate;
                                            break;
                                        case 3:
                                            uploadCmsDto.SIGN_ID_NO_3 = signature.fullName;
                                            uploadCmsDto.SIGN_DATE_3 = signature.signDate;
                                            break;
                                        case 4:
                                            uploadCmsDto.SIGN_ID_NO_4 = signature.fullName;
                                            uploadCmsDto.SIGN_DATE_4 = signature.signDate;
                                            break;
                                        case 5:
                                            uploadCmsDto.SIGN_ID_NO_5 = signature.fullName;
                                            uploadCmsDto.SIGN_DATE_5 = signature.signDate;
                                            break;
                                        case 6:
                                            uploadCmsDto.SIGN_ID_NO_6 = signature.fullName;
                                            uploadCmsDto.SIGN_DATE_6 = signature.signDate;
                                            break;
                                        case 7:
                                            uploadCmsDto.SIGN_ID_NO_7 = signature.fullName;
                                            uploadCmsDto.SIGN_DATE_7 = signature.signDate;
                                            break;
                                    }
                                }
                            }
                            WebServicePost("SyncLog/TagITRStatusPostToDatahub?ProjectID=" + Settings.ProjectID + "&db=" + Settings.CurrentDB, JsonConvert.SerializeObject(uploadCmsDto), Settings.CompletionAccessToken);
                        }
                    }
                    if(CommonHeaderFooterSignOff.Count() > 0)
                    {
                        var CommonHeaderFooterdata = await _CommonHeaderFooterRepository.QueryAsync("Select * From T_ITRCommonHeaderFooter WHERE Tag = '" + tag.name + "' AND ITRNumber = '" + checkSheet.CHECKSHEETNAME + "' AND ModelName = '" + Settings.ModelName + "' AND IsUpdated = 1");
                        T_ITRCommonHeaderFooter CommonHeaderFooter = CommonHeaderFooterdata.FirstOrDefault();

                        List<SignOff> _SignOffsHeader = CommonHeaderFooterSignOff.Select(x =>
                        new SignOff
                        {
                            count = x.Count,
                            fullName = x.FullName,
                            jobPack = "",
                            signDate = x.SignDate,
                            synced = x.IsSynced,
                            title = x.Title,
                            rejected = x.Rejected,
                            rejectedReason = x.RejectedReason
                        }).ToList();

                        CompletionsTagITRStatus uploadCmsDto = new CompletionsTagITRStatus();
                        uploadCmsDto.ITR_NO = checkSheet.CHECKSHEETNAME;
                        uploadCmsDto.Itr_Report_No = tag.name;
                        uploadCmsDto.JOB_CODE_KEY = Settings.JobCode;
                        uploadCmsDto.REJECTED_DATE = null;
                        if (CommonHeaderFooterSignOff.Any())
                        {
                            uploadCmsDto.TAG_NO = tag.name;
                            uploadCmsDto.SIGN_ID_NO_1 = "";
                            uploadCmsDto.SIGN_ID_NO_2 = "";
                            uploadCmsDto.SIGN_ID_NO_3 = "";
                            uploadCmsDto.SIGN_ID_NO_4 = "";
                            uploadCmsDto.SIGN_ID_NO_5 = "";
                            uploadCmsDto.SIGN_ID_NO_6 = "";
                            uploadCmsDto.SIGN_ID_NO_7 = "";
                            uploadCmsDto.SIGN_DATE_1 = null;
                            uploadCmsDto.SIGN_DATE_2 = null;
                            uploadCmsDto.SIGN_DATE_3 = null;
                            uploadCmsDto.SIGN_DATE_4 = null;
                            uploadCmsDto.SIGN_DATE_5 = null;
                            uploadCmsDto.SIGN_DATE_6 = null;
                            uploadCmsDto.SIGN_DATE_7 = null;
                            uploadCmsDto.REMARKS = "";
                            uploadCmsDto.REJECTED_USER = "";
                            uploadCmsDto.REJECT_REMARKS = "";
                            uploadCmsDto.REJECTED_DATE = null;
                            uploadCmsDto.UPDATED_ID = Settings.UserID.ToString();
                            uploadCmsDto.UPDATED_DATE = DateTime.Now;

                            if (_SignOffsHeader.FirstOrDefault().rejected)
                            {
                                uploadCmsDto.REJECTED_DATE = _SignOffsHeader.FirstOrDefault().signDate;
                                uploadCmsDto.REJECTED_USER = Settings.UserID.ToString();
                                uploadCmsDto.REJECT_REMARKS = _SignOffsHeader.FirstOrDefault().rejectedReason;
                            }
                            else
                            {
                                foreach (SignOff signature in _SignOffsHeader)
                                {
                                    if (String.IsNullOrEmpty(signature.fullName)) continue;
                                    switch (signature.count)
                                    {
                                        case 1:
                                            uploadCmsDto.SIGN_ID_NO_1 = signature.fullName;
                                            uploadCmsDto.SIGN_DATE_1 = signature.signDate;
                                            uploadCmsDto.REMARKS = CommonHeaderFooter.Remarks;
                                            break;
                                        case 2:
                                            uploadCmsDto.SIGN_ID_NO_2 = signature.fullName;
                                            uploadCmsDto.SIGN_DATE_2 = signature.signDate;
                                            break;
                                        case 3:
                                            uploadCmsDto.SIGN_ID_NO_3 = signature.fullName;
                                            uploadCmsDto.SIGN_DATE_3 = signature.signDate;
                                            break;
                                        case 4:
                                            uploadCmsDto.SIGN_ID_NO_4 = signature.fullName;
                                            uploadCmsDto.SIGN_DATE_4 = signature.signDate;
                                            break;
                                        case 5:
                                            uploadCmsDto.SIGN_ID_NO_5 = signature.fullName;
                                            uploadCmsDto.SIGN_DATE_5 = signature.signDate;
                                            break;
                                        case 6:
                                            uploadCmsDto.SIGN_ID_NO_6 = signature.fullName;
                                            uploadCmsDto.SIGN_DATE_6 = signature.signDate;
                                            break;
                                        case 7:
                                            uploadCmsDto.SIGN_ID_NO_7 = signature.fullName;
                                            uploadCmsDto.SIGN_DATE_7 = signature.signDate;
                                            break;
                                    }
                                }
                            }
                            WebServicePost("SyncLog/TagITRStatusPostToDatahub?ProjectID=" + Settings.ProjectID + "&db=" + Settings.CurrentDB, JsonConvert.SerializeObject(uploadCmsDto), Settings.CompletionAccessToken);
                        }
                    }
                }
            }
            return true;
        }

        public static string WebServicePost(string Call, string JSONString, string Token)
        {
            string Result = "";
            Uri uri = new Uri(Settings.CompletionServer_Url + Call);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Post;
            request.Timeout = 300000;
            request.Headers.Add("Token", Token);
            request.ContentType = "application/json; charset=utf-8";

            byte[] bytes = Encoding.UTF8.GetBytes(JSONString);

            using (var streamwriter = request.GetRequestStream())
            {
                streamwriter.Write(bytes, 0, bytes.Length);
            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    Result = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                Result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }

            return Result;
        }

        public async Task<bool> downloadChanges(string SystemName)
        {
            try
            {
                //SystemsList = new List<T_DataRefs>();
                string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getSystems(Settings.ModelName, Settings.CurrentDB), Settings.CompletionAccessToken);
                var SystemsList = JsonConvert.DeserializeObject<List<T_DataRefs>>(JsonString);

                await _DataRefsRepository.InsertOrReplaceAsync(SystemsList);
                //workpacks
                _ = await pullDownWorkpacks(SystemName);
                // _ = await pullDownTagHeaders();
                _ = await pullDownSheetQuestions();
                _ = await pullCheckSheetAnswersAsync();
                return true;
            }
            catch (Exception e)
            {
                await _userDialogs.AlertAsync("error downloading Tags: ", "", null);
                return false;
            }
            //  return true;
        }
        public async Task<bool> downloadChanges(string SystemName, string SyncType)
        {
            try
            {
                string JsonString = ModsTools.CompletionWebServicePost(ApiUrls.Post_GetTagsByColumn(Settings.ModelName, Settings.ProjectName, Settings.CurrentDB, SyncType), SystemName, Settings.CompletionAccessToken);
                var SystemsList = JsonConvert.DeserializeObject<List<T_WorkPack>>(JsonString);

                foreach (T_WorkPack Systems in SystemsList)
                {
                    ////workpacks
                    _ = await pullDownWorkpacks(Systems);
                    //// _ = await pullDownTagHeaders();
                    _ = await pullDownSheetQuestions();
                    _ = await pullCheckSheetAnswersAsync();
                }
                return true;

            }
            catch (Exception e)
            {
                await _userDialogs.AlertAsync("error downloading Tags: ", "", null);
                return false;
            }
            //  return true;
        }

        public async Task<bool> downloadChangesByID(string SystemsList)
        {
            try
            {
                //string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getSystems(Settings.ModelName, Settings.CurrentDB), Settings.CompletionAccessToken);
                //var SystemData = JsonConvert.DeserializeObject<List<T_DataRefs>>(JsonString);
                //await _DataRefsRepository.InsertOrReplaceAsync(SystemData);

                _ = await pullDownWorkpacksbyID(SystemsList);
                _ = await pullDownSheetQuestions();
                _ = await pullCheckSheetAnswersAsync();
                return true;
            }
            catch (Exception e)
            {
                await _userDialogs.AlertAsync("error downloading Tags: ", "", null);
                return false;
            }
        }

        private async Task<bool> pullDownWorkpacks(string SystemName)
        {
            string WorkpackjsonResult = ModsTools.CompletionWebServiceGet(ApiUrls.getTags(Settings.ModelName, Settings.ProjectName, "SYSTEM", SystemName, "dontmatter", Settings.CurrentDB), Settings.CompletionAccessToken);
            var SystemsList = JsonConvert.DeserializeObject<T_WorkPack>(WorkpackjsonResult);
            if (SystemsList.tagNameToSheetNameMap != null)
            {
                tagToSheetList = SystemsList.tagNameToSheetNameMap;
                foreach (var tag in tagToSheetList)
                {
                    foreach (var SheetName in tag.Value)
                    {
                        bool CCSITR = SystemsList.checkSheets.Where(x => x.name == SheetName).FirstOrDefault().ccsItr;
                        await _CHECKSHEET_PER_TAGRepository.InsertOrReplaceAsync(new T_CHECKSHEET_PER_TAG() { TAGNAME = tag.Key, CHECKSHEETNAME = SheetName, ccsItr = CCSITR, HEADER_ID = "0", JOBPACK = " " });
                    }
                }
            }
            if (SystemsList.tags != null)
            {
                SystemsList.tags.ForEach(i => i.ProjectName = Settings.ProjectName);
                await _TAGRepository.InsertOrReplaceAsync(SystemsList.tags);
            }
            if (SystemsList.checkSheets != null)
            {
                SystemsList.checkSheets.ForEach(i => i.ProjectName = Settings.ProjectName);
                await _CHECKSHEETRepository.InsertOrReplaceAsync(SystemsList.checkSheets);
            }
            return true;
        }

        private async Task<bool> pullDownWorkpacks(T_WorkPack SystemsList)
        {
            //string WorkpackjsonResult = ModsTools.CompletionWebServiceGet(ApiUrls.getTags(Settings.ModelName, Settings.ProjectName, "SYSTEM", SystemName, "dontmatter", Settings.CurrentDB), Settings.CompletionAccessToken);
            //var SystemsList = JsonConvert.DeserializeObject<T_WorkPack>(WorkpackjsonResult);
            if (SystemsList.tagNameToSheetNameMap != null)
            {
                tagToSheetList = SystemsList.tagNameToSheetNameMap;
                foreach (var tag in tagToSheetList)
                {
                    foreach (var SheetName in tag.Value)
                    {
                        bool CCSITR = SystemsList.checkSheets.Where(x => x.name == SheetName).FirstOrDefault().ccsItr;
                        await _CHECKSHEET_PER_TAGRepository.InsertOrReplaceAsync(new T_CHECKSHEET_PER_TAG() { TAGNAME = tag.Key, CHECKSHEETNAME = SheetName, ccsItr = CCSITR, HEADER_ID = "0", JOBPACK = " ", ProjectName = Settings.ProjectName });
                    }
                }
            }
            if (SystemsList.tags != null)
            {
                SystemsList.tags.ForEach(i => i.ProjectName = Settings.ProjectName);
                await _TAGRepository.InsertOrReplaceAsync(SystemsList.tags);
            }
            if (SystemsList.checkSheets != null)
            {
                SystemsList.checkSheets.ForEach(i => i.ProjectName = Settings.ProjectName);
                await _CHECKSHEETRepository.InsertOrReplaceAsync(SystemsList.checkSheets);
            }
            return true;
        }

        private async Task<bool> pullDownWorkpacksbyID(string IDs)
        {
            try
            {
                //string WorkpackjsonResultold = ModsTools.CompletionWebServicePost(ApiUrls.Post_GetTagsByID(Settings.ModelName, Settings.ProjectName, Settings.CurrentDB), IDs, Settings.CompletionAccessToken);
                string WorkpackjsonResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_GetTagsAndItrsByID(Settings.ModelName, Settings.ProjectName, Settings.CurrentDB), IDs, Settings.CompletionAccessToken);
                var SystemsList = JsonConvert.DeserializeObject<T_WorkPack>(WorkpackjsonResult);
                if (SystemsList.tagNameToSheetNameMap != null)
                {
                    tagToSheetList = SystemsList.tagNameToSheetNameMap;

                    foreach (var tag in tagToSheetList)
                    {
                        foreach (var SheetName in tag.Value)
                        {
                            // await _Ccms_signatureRepository.QueryAsync("Delete from T_Ccms_signature where ProjectID='" + Settings.ProjectName + "' and Itr='" + SheetName + "'");

                            string SignatureRules = ModsTools.CompletionWebServiceGet(ApiUrls.Url_GetSignaturesForTag(Settings.ModelID, Settings.ProjectName, SheetName, Settings.CurrentDB), Settings.CompletionAccessToken);

                            var SignatureRulesList = JsonConvert.DeserializeObject<List<T_Ccms_signature>>(SignatureRules);
                            if (SignatureRulesList.Any())
                            {
                                await _Ccms_signatureRepository.QueryAsync("delete from T_Ccms_signature where ITR='" + SheetName + "' and ProjectName = '" + Settings.ProjectName + "'");
                                SignatureRulesList.ForEach(x => { x.ProjectName = Settings.ProjectName; });
                                await _Ccms_signatureRepository.InsertOrReplaceAsync(SignatureRulesList);
                            }

                            //if (await _ITRService.IsImplementedITR(SheetName))
                            //    await GetCheckSheetITRsData(tag.Key, SheetName);
                            bool CCSITR = SystemsList.checkSheets.Where(x => x.name == SheetName).FirstOrDefault().ccsItr;
                            await _CHECKSHEET_PER_TAGRepository.InsertOrReplaceAsync(new T_CHECKSHEET_PER_TAG() { TAGNAME = tag.Key, CHECKSHEETNAME = SheetName, ccsItr = CCSITR, HEADER_ID = "0", JOBPACK = " ", ProjectName = Settings.ProjectName });
                        }
                    }
                }
                if (SystemsList.tags != null)
                {
                    SystemsList.tags.ForEach(i => i.ProjectName = Settings.ProjectName);
                    await _TAGRepository.InsertOrReplaceAsync(SystemsList.tags);
                }
                if (SystemsList.checkSheets != null)
                {
                    SystemsList.checkSheets.ForEach(i => i.ProjectName = Settings.ProjectName);
                    await _CHECKSHEETRepository.InsertOrReplaceAsync(SystemsList.checkSheets);
                }
                CheckSheetAnswerModelList = SystemsList.checkSheets.Where(i => i.ItrAnswerList != null).SelectMany(i => i.ItrAnswerList).ToList();

                    List<ITRs> ITRData = SystemsList.checkSheets.Where(i => i.ITRData != null).Select(i => i.ITRData).ToList();

                if (ITRData.Count > 0)
                {
                    foreach (ITRs item in ITRData.ToList())
                    {
                        await GetCheckSheetITRsData(item);
                    }
                }
            }
            catch(Exception e)
            {
                return true;
            }

            return true;
        }

        private async Task<bool> pullDownSheetQuestions()
        {
            List<string> sheetNames = new List<string>();

            sheetNames.AddRange(tagToSheetList.Values.SelectMany(l => l).Distinct().ToList());
            foreach (string sheetname in sheetNames)
            {
                try
                {
                    var JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getSheetQuestions(Settings.ProjectName, Settings.CurrentDB, sheetname), Settings.CompletionAccessToken);
                    var Quetions = JsonConvert.DeserializeObject<List<T_CHECKSHEET_QUESTION>>(JsonString);

                    if (Quetions != null && Quetions.Any())
                    {
                        Quetions.ForEach(async x => {
                            x.CheckSheet = sheetname;
                            x.ProjectName = Settings.ProjectName;

                            //
                            var ImageList = ModsTools.CompletionWebServiceGet(ApiUrls.Url_GetItrImages("CompletionsChecksheet_Image",Convert.ToInt32( x.id)), Settings.CompletionAccessToken);
                            List<VMHub> AllImagesList = JsonConvert.DeserializeObject<List<VMHub>>(ImageList);
                            if (AllImagesList.Count > 0)
                            {
                                foreach (VMHub CurrentImage in AllImagesList.ToArray())
                                {
                                    string Folder = string.Empty;
                                    Folder = ("Photo Store\\CompletionsChecksheet_Image\\" + Settings.ProjectID + "\\" + Settings.UserID + "\\" + x.id);
                                    var compPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath(Folder);

                                    await DependencyService.Get<ISaveFiles>().SavePictureToDisk(Device.RuntimePlatform == Device.UWP ? Folder : compPath,
                                        Device.RuntimePlatform == Device.UWP ? CurrentImage.FileName : Path.GetFileNameWithoutExtension(CurrentImage.FileName), Convert.FromBase64String(CurrentImage.FileBytes).ToArray());
                                }
                            }

                        });
                        await _CHECKSHEET_QUESTIONRepository.InsertOrReplaceAsync(Quetions);

                     
                    }
                }
                catch (Exception e)
                {
                }
            }

            return true;
        }
        private async Task<bool> pullCheckSheetAnswersAsync()
        {
            try
            {
                //var JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getCheckSheetAnswers(Settings.ModelName, Settings.ProjectName, TagName.Key, sheetname, Settings.CurrentDB), Settings.CompletionAccessToken);
                //var CheckSheetAnswers = JsonConvert.DeserializeObject<List<CheckSheetAnswerModel>>(JsonString);
                //foreach (CheckSheetAnswerModel checkSheetAnswer in CheckSheetAnswers)
                foreach (CheckSheetAnswerModel checkSheetAnswer in CheckSheetAnswerModelList)
                {
                    if (checkSheetAnswer.SignOffHeaders.Any())
                    {
                        checkSheetAnswer.SignOffHeaders.ForEach(x => { x.SignOffChecksheet = checkSheetAnswer.CheckSheetName; x.SignOffTag = checkSheetAnswer.TagName; x.ProjectName = Settings.ProjectName; });
                        await _SignOffHeaderRepository.InsertOrReplaceAsync(checkSheetAnswer.SignOffHeaders);
                    }

                    if (checkSheetAnswer.answers.Any())
                    {
                        checkSheetAnswer.answers.ForEach(x =>
                        {
                            x.tagName = checkSheetAnswer.TagName; x.ccmsHeaderId = checkSheetAnswer.CcmsHeaderId; x.checkSheetName = checkSheetAnswer.CheckSheetName;
                            x.jobPack = checkSheetAnswer.JobPack; x.IsSynced = true; x.ProjectName = Settings.ProjectName;
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
                            if (PropValue != null || prop.Name.Contains("DwgRevNo"))
                            {
                                var T_SheetHeader = new T_TAG_SHEET_HEADER()
                                {
                                    TagName = checkSheetAnswer.TagName,
                                    ChecksheetName = checkSheetAnswer.CheckSheetName,
                                    ColumnKey = prop.Name,
                                    ColumnValue = (PropValue == null) ? "" : PropValue.ToString(),
                                    JobPack = checkSheetAnswer.JobPack,
                                    ProjectName = Settings.ProjectName
                                };
                                await _TAG_SHEET_HEADERRepository.InsertOrReplaceAsync(T_SheetHeader);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return true;

        }

        private async Task<bool> GetCheckSheetITRsData(ITRs ITRdata)
        {
            try
            {
                if(ITRdata.ITR7000_030A_031A != null )
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
                            ITRSeries_7000.Records_30A_31A.CCMS_HEADERID =(int)ITRSeries_7000.CommonHeaderFooter.ID;
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
                else if(ITRdata.ITR7000_040A_041A_042A != null)
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
                            ITRSeries_7000.InsulationDetails.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = InsID++; x.CCMS_HEADERID = (int)ITRSeries_7000.CommonHeaderFooter.ID; }) ;
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
                            if(ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData != null)
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
                                if (Instrumentdata8100_001A.Count >0 )
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
                            ITRSeries_8000.ITR8100_002ARadio_Test.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID;x.RowID = Radio_TestID++; });
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
                        ITR8260_002A.ITR_DielectricTestList.ForEach(x => { x.ModelName = Settings.ModelName;x.CommonRowID = RecordID; x.RowID = DiTestID++; });
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
                            ITRSeries_8000.ITR_8161_001A_ConRes.ForEach(x => { x.ModelName = Settings.ModelName;x.CommonRowID = RecordID;x.RowID = ConResID++; });
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
                            ITRSeries_8121_004A.ITR8121_004A_InispactionForControlAndAuxiliary.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID;x.RowID = ConAuxID++; });
                            await _ITR8121_004AInCAndAuxiliaryRepository.InsertOrReplaceAsync(ITRSeries_8121_004A.ITR8121_004A_InispactionForControlAndAuxiliary);
                        }
                        if (ITRSeries_8121_004A.ITR8121_004A_TransformerRatioTest.Count > 0)
                        {
                            var TransRatioTest = await _ITR8121_004ATransformerRatioTestRepository.GetAsync();
                            long TransRatioTestID = TransRatioTest.Count() + 1;
                            ITRSeries_8121_004A.ITR8121_004A_TransformerRatioTest.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID;x.RowID = TransRatioTestID++; });
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
                else if (ITRdata.ITR_8211_002A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR8211_002AModel ITRSeries_8000 = ITRdata.ITR_8211_002A;

                    if (String.IsNullOrEmpty(ITRSeries_8000.Error))
                    {
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
                                var Instrumentdata8121_004A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID8121_004A = 1;
                                if (Instrumentdata8121_004A.Count > 0)
                                    InstID8121_004A = Instrumentdata8121_004A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8121_004A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITRSeries_8000.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }
                        if (ITRSeries_8000.ITR_8211_002A_Body != null)
                        {
                            ITRSeries_8000.ITR_8211_002A_Body.ModelName = Settings.ModelName;
                            ITRSeries_8000.ITR_8211_002A_Body.CommonRowID = RecordID;
                            await _ITRRecords_8211_002A_BodyRepository.InsertOrReplaceAsync(ITRSeries_8000.ITR_8211_002A_Body);
                        }
                        if (ITRSeries_8000.ITR_8211_002A_RunTest.Count > 0)
                        {
                            var records = await _ITRRecords_8211_002A_RunTestRepository.GetAsync();
                            long InsResID = records.Count() + 1;
                            ITRSeries_8000.ITR_8211_002A_RunTest.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = InsResID++; });
                            await _ITRRecords_8211_002A_RunTestRepository.InsertOrReplaceAsync(ITRSeries_8000.ITR_8211_002A_RunTest);
                        }
                    }
                }
                else if (ITRdata.ITR7000_101AHarmony != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR7000_101AHarmonyModel ITR7000_101AHarmony = ITRdata.ITR7000_101AHarmony;
                    if (String.IsNullOrEmpty(ITR7000_101AHarmony.Error))
                    {
                        if (ITR7000_101AHarmony.CommonHeaderFooter != null)
                        {
                            ITR7000_101AHarmony.CommonHeaderFooter.ROWID = RecordID;
                            ITR7000_101AHarmony.CommonHeaderFooter.ModelName = Settings.ModelName;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITR7000_101AHarmony.CommonHeaderFooter);
                            if (ITR7000_101AHarmony.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITR7000_101AHarmony.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITR7000_101AHarmony.CommonHeaderFooter.ID; x.SignOffChecksheet = ITR7000_101AHarmony.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITR7000_101AHarmony.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITR7000_101AHarmony.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITR7000_101AHarmony.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                            if (ITR7000_101AHarmony.CommonHeaderFooter.ITRInstrumentData != null)
                            {
                                var Instrumentdata8121_004A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID8121_004A = 1;
                                if (Instrumentdata8121_004A.Count > 0)
                                    InstID8121_004A = Instrumentdata8121_004A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITR7000_101AHarmony.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8121_004A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITR7000_101AHarmony.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITR7000_101AHarmony.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }
                        if (ITR7000_101AHarmony.ITR_7000_101AHarmony_Genlnfo != null)
                        {
                            ITR7000_101AHarmony.ITR_7000_101AHarmony_Genlnfo.ModelName = Settings.ModelName;
                            ITR7000_101AHarmony.ITR_7000_101AHarmony_Genlnfo.CommonRowID = RecordID;
                            await _ITR7000_101AHarmony_GenlnfoRepository.InsertOrReplaceAsync(ITR7000_101AHarmony.ITR_7000_101AHarmony_Genlnfo);
                        }
                        if (ITR7000_101AHarmony.ITR_7000_101AHarmony_ActivityDetails != null)
                        {
                            ITR7000_101AHarmony.ITR_7000_101AHarmony_ActivityDetails.ModelName = Settings.ModelName;
                            ITR7000_101AHarmony.ITR_7000_101AHarmony_ActivityDetails.CommonRowID = RecordID;
                            await _ITR7000_101AHarmony_ActivityDetailsRepository.InsertOrReplaceAsync(ITR7000_101AHarmony.ITR_7000_101AHarmony_ActivityDetails);
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
                    ITR8170_002AModel ITRSeries_8000 = ITRdata.ITR8170_002A;

                    if (String.IsNullOrEmpty(ITRSeries_8000.Error))
                    {
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
                                var Instrumentdata8121_004A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID8121_004A = 1;
                                if (Instrumentdata8121_004A.Count > 0)
                                    InstID8121_004A = Instrumentdata8121_004A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8121_004A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITRSeries_8000.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRSeries_8000.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }
                        if (ITRSeries_8000.ITR_8170_002A_IndifictionLamp != null)
                        {
                            ITRSeries_8000.ITR_8170_002A_IndifictionLamp.ModelName = Settings.ModelName; 
                            ITRSeries_8000.ITR_8170_002A_IndifictionLamp.CommonRowID = RecordID;
                            await _ITR_8170_002A_IndifictionLampRepository.InsertOrReplaceAsync(ITRSeries_8000.ITR_8170_002A_IndifictionLamp);
                        }
                        if (ITRSeries_8000.ITR_8170_002A_InsRes != null)
                        {
                            ITRSeries_8000.ITR_8170_002A_InsRes.ModelName = Settings.ModelName;
                            ITRSeries_8000.ITR_8170_002A_InsRes.CommonRowID = RecordID;
                            await _ITR_8170_002A_InsResRepository.InsertOrReplaceAsync(ITRSeries_8000.ITR_8170_002A_InsRes);
                        }
                        if (ITRSeries_8000.ITR_8170_002A_Voltage_Reading.Count > 0)
                        {
                            var records = await _ITRRecords_8170_002A_Voltage_ReadingRepository.GetAsync();
                            long InsResID = records.Count() + 1;
                            ITRSeries_8000.ITR_8170_002A_Voltage_Reading.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = InsResID++; });
                            await _ITRRecords_8170_002A_Voltage_ReadingRepository.InsertOrReplaceAsync(ITRSeries_8000.ITR_8170_002A_Voltage_Reading);
                        }
                    }
                }


                else if (ITRdata.ITR8300_003A != null)
                {
                    var CHFrecord = await _CommonHeaderFooterRepository.GetAsync();
                    long RecordID = CHFrecord.Count() + 1;
                    ITR_8300_003AModel ITRSeries_8300 = ITRdata.ITR8300_003A;

                    if (String.IsNullOrEmpty(ITRSeries_8300.Error))
                    {
                        if (ITRSeries_8300.CommonHeaderFooter != null)
                        {
                            ITRSeries_8300.CommonHeaderFooter.ROWID = RecordID;
                            ITRSeries_8300.CommonHeaderFooter.ModelName = Settings.ModelName;
                            await _CommonHeaderFooterRepository.InsertOrReplaceAsync(ITRSeries_8300.CommonHeaderFooter);
                            if (ITRSeries_8300.CommonHeaderFooter.CommonHeaderFooterSignOff.Any())
                            {
                                ITRSeries_8300.CommonHeaderFooter.CommonHeaderFooterSignOff.ForEach(x =>
                                {
                                    x.ITRCommonID = ITRSeries_8300.CommonHeaderFooter.ID; x.SignOffChecksheet = ITRSeries_8300.CommonHeaderFooter.ITRNumber; x.SignOffTag = ITRSeries_8300.CommonHeaderFooter.Tag;
                                    x.ModelName = Settings.ModelName; x.ProjectName = Settings.ProjectName; x.CommonRowID = RecordID; x.CCMS_HEADERID = (int)ITRSeries_8300.CommonHeaderFooter.ID;
                                });
                                await _CommonHeaderFooterSignOffRepository.InsertOrReplaceAsync(ITRSeries_8300.CommonHeaderFooter.CommonHeaderFooterSignOff);
                            }
                            if (ITRSeries_8300.CommonHeaderFooter.ITRInstrumentData != null)
                            {
                                var Instrumentdata8121_004A = await _ITRInstrumentDataRepository.GetAsync();
                                long InstID8300_003A = 1;
                                if (Instrumentdata8121_004A.Count > 0)
                                    InstID8300_003A = Instrumentdata8121_004A.OrderByDescending(x => x.RowID).FirstOrDefault().RowID + 1;
                                ITRSeries_8300.CommonHeaderFooter.ITRInstrumentData.ForEach(x => { x.RowID = InstID8300_003A++; x.CommonRowID = RecordID; x.ModelName = Settings.ModelName; x.CCMS_HEADERID = (int)ITRSeries_8300.CommonHeaderFooter.ID; });
                                await _ITRInstrumentDataRepository.InsertOrReplaceAsync(ITRSeries_8300.CommonHeaderFooter.ITRInstrumentData);
                            }
                        }
                        if (ITRSeries_8300.ITR_Body != null)
                        {
                            ITRSeries_8300.ITR_Body.ModelName = Settings.ModelName;
                            ITRSeries_8300.ITR_Body.CommonRowID = RecordID;
                            await _ITR_8300_003A_BodyRepository.InsertOrReplaceAsync(ITRSeries_8300.ITR_Body);
                        }
                      
                        if (ITRSeries_8300.ITR_IlluminList.Count > 0)
                        {
                            var records = await _ITR_8300_003A_IlluminRepository.GetAsync();
                            long InsResID = records.Count() + 1;
                            int srno = 1;
                            ITRSeries_8300.ITR_IlluminList.ForEach(x => { x.ModelName = Settings.ModelName; x.CommonRowID = RecordID; x.RowID = InsResID++; x.serNO = srno++; });
                            await _ITR_8300_003A_IlluminRepository.InsertOrReplaceAsync(ITRSeries_8300.ITR_IlluminList);
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
        private async Task<bool> Post_030A_031A_ITRsData(string Tag, string Checksheet)

        {
            try
            {
                ITR7000_030A_031AModel itrDataObj = new ITR7000_030A_031AModel();
                itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {

                    string recordsql = " SELECT * FROM T_ITRRecords_30A_31A WHERE TagNO='" + Tag + "' AND ITRNumber='" + Checksheet + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = "+ itrDataObj.CommonHeaderFooter.ID ;
                    var Recorddata = await _RecordsRepository.QueryAsync(recordsql);
                    itrDataObj.Records_30A_31A = Recorddata.FirstOrDefault();

                    string tubeColorSql = "SELECT * FROM T_ITRTubeColors WHERE RecordsID = '" + itrDataObj.Records_30A_31A.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID ; // AND IsUpdated = 1
                    var TubeColorsData = await _TubeColorsRepository.QueryAsync(tubeColorSql);
                    itrDataObj.TubeColors = TubeColorsData.ToList();
                  
                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_7000_030A_031A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception e)
            {
            }
            return true;
        }
        private async Task<bool> Post_8140_001_ITRsData(string Tag, string Checksheet)
        {
            try
            {
                ITR8140_001AModel itrDataObj = new ITR8140_001AModel();

                itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {

                    string recordsql = " SELECT * FROM T_ITR8140_001A_ContactResisTest WHERE ITRCommonID='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var Recorddata = await _T_ITR8140_001A_ContactResisTestRepository.QueryAsync(recordsql);
                    itrDataObj.iTR_8140_001A_ContactResistanceTests = Recorddata.ToList();

                     recordsql = "SELECT * FROM T_ITR8140_001AInsulationResistanceTest WHERE ITRCommonID = '" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID; // AND IsUpdated = 1
                    var recordDa = await _T_ITR8140_001AInsulationResistanceTestRepository.QueryAsync(recordsql);
                    itrDataObj.iTR8140_001A_InsulationResistanceTest = recordDa.ToList();

                    recordsql = "SELECT * FROM T_ITR8140_001ADialectricTest WHERE ITRCommonID = '" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID; // AND IsUpdated = 1
                     var record = await _T_ITR8140_001ADialectricTestRepository.QueryAsync(recordsql);
                    itrDataObj.iTR8140_001A_Dilectric_Test = record.ToList();

                    recordsql = "SELECT * FROM T_ITR8140_001ATestInstrucitonData WHERE ITRCommonID = '" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID; // AND IsUpdated = 1
                    var recordt = await _T_ITR8140_001ATestInstrumentDataRepository.QueryAsync(recordsql);
                    itrDataObj.iTR8140_001A_TestInstrumentData = recordt.FirstOrDefault();

                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_8140_001A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception e)
            {
            }
            return true;
        }
        private async Task<bool> Post_040A_041A_042A_ITRsData(string Tag, string Checksheet)
        {
            try
            {
                ITR7000_040A_041A_042AModel itrDataObj = new ITR7000_040A_041A_042AModel();

                itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet);//CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string recordsql = " SELECT * FROM T_ITRRecords_040A_041A_042A WHERE ITRCommonID ='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID ;
                    var Recorddata = await _Records_04XARepository.QueryAsync(recordsql);
                    itrDataObj.Records_40A_41A_042A = Recorddata.FirstOrDefault();

                    string InsulationDetailsSql = "SELECT * FROM T_ITRInsulationDetails WHERE ITRCommonID = '" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID ; // AND IsUpdated = 1
                    var InsulationDetailsData = await _InsulationDetailsRepository.QueryAsync(InsulationDetailsSql);
                    itrDataObj.InsulationDetails = InsulationDetailsData.ToList();
                 
                 var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_7000_040A_041A_042A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception e)
            {
            }
            return true;
        }
        private async Task<bool> Post_080A_090A_091A_ITRsData(string Tag, string Checksheet)
        {
            try
            {
                ITR7000_080A_090A_091AModel itrDataObj = new ITR7000_080A_090A_091AModel();

                itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string recordsql = " SELECT * FROM T_ITRRecords_080A_090A_091A WHERE ITRCommonID ='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID ;
                    var Recorddata = await _Records_080A_09XARepository.QueryAsync(recordsql);
                    itrDataObj.Records_080A_090A_091A = Recorddata.FirstOrDefault();

                  var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_7000_080A_090A_091A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception e)
            {
            }
            return true;
        }
        private async Task<bool> Post_8000_003A_ITRsData(string Tag, string Checksheet)
        {
            try
            {
                ITR_8000_003AModel itrDataObj = new ITR_8000_003AModel();

                //var CommonHeaderFooter = await _CommonHeaderFooterRepository.QueryAsync("Select * From T_ITRCommonHeaderFooter WHERE Tag = '" + Tag + "' AND ITRNumber = '" + Checksheet + "' AND ModelName = '" + Settings.ModelName + "'");
                itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet);//CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string recordsql = " SELECT * FROM T_ITRRecords_8000_003A WHERE ITRCommonID ='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var Recorddata = await _Records_8000003ARepository.QueryAsync(recordsql);
                    itrDataObj.ITR_8000_003ARecords = Recorddata.FirstOrDefault();

                    string AcceptanceCriteria = "SELECT * FROM T_ITRRecords_8000_003A_AcceptanceCriteria WHERE ITRCommonID = '" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID; // AND IsUpdated = 1
                    var InsulationDetailsData = await _Records_8000003A_AcceptanceCriteriaRepository.QueryAsync(AcceptanceCriteria);
                    itrDataObj.ITR_8000_003A_AcceptanceCriteria = InsulationDetailsData.ToList();
                  
                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_8000_003A_(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception e)
            {
            }
            return true;
        }

        private async Task<bool> Post_8100_001A_ITRsData(string Tag, string Checksheet)
        {
            try
            {
                ITR8100_001AModel itrDataObj = new ITR8100_001AModel();

                //var CommonHeaderFooter = await _CommonHeaderFooterRepository.QueryAsync("Select * From T_ITRCommonHeaderFooter WHERE Tag = '" + Tag + "' AND ITRNumber = '" + Checksheet + "' AND ModelName = '" + Settings.ModelName + "'");
                itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string recordsqlCT = " SELECT * FROM T_ITR8100_001A_CTdata WHERE CommonHFID ='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var RecorddataCT = await _ITR8100_001A_CTdataRepository.QueryAsync(recordsqlCT);
                    itrDataObj.ITR_CTdata = new List<T_ITR8100_001A_CTdata>(RecorddataCT);

                    string recordsqlIR = " SELECT * FROM T_ITR8100_001A_InsulationResistanceTest WHERE CommonHFID ='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var RecorddataIR = await _ITR8100_001A_IRTestRepository.QueryAsync(recordsqlIR);
                    itrDataObj.ITR_InsulationResistanceTest = new List<T_ITR8100_001A_InsulationResistanceTest>(RecorddataIR);

                    string recordsqlRT = " SELECT * FROM T_ITR8100_001A_RatioTest WHERE CommonHFID ='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var RecorddataRT = await _ITR8100_001A_RatioTestRepository.QueryAsync(recordsqlRT);
                    itrDataObj.ITR_RatioTest = new List<T_ITR8100_001A_RatioTest>(RecorddataRT);

                    string recordsqlTID = " SELECT * FROM T_ITR8100_001A_TestInstrumentData WHERE CommonHFID ='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var RecorddataTID = await _ITR8100_001A_TIDataRepository.QueryAsync(recordsqlTID);
                    itrDataObj.ITR_TestInstrumentData = RecorddataTID.FirstOrDefault();

                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_8100_001A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception e)
            {
            }
            return true;
        }
        private async Task<bool> Post_8100_002A_ITRsData(string Tag, string Checksheet)
        {
            try
            {
                ITR8100_002AModel itrDataObj = new ITR8100_002AModel();

                itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string recordsql = " SELECT * FROM T_ITRRecords_8100_002A WHERE ITRCommonID ='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var Recorddata = await _ITRRecords_8100_002ARepository.QueryAsync(recordsql);
                    itrDataObj.ITR8100_002AVT_Data = Recorddata.FirstOrDefault();

                    string InsRes = "SELECT * FROM T_ITRRecords_8100_002A_InsRes_Test WHERE ITRCommonID = '" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID; // AND IsUpdated = 1
                    var InsulationDetailsData = await _ITRRecords_8100_002A_InsRes_TestRepository.QueryAsync(InsRes);
                    itrDataObj.ITR8100_002AInsRes_Test = InsulationDetailsData.ToList();

                    string Radio_Test = "SELECT * FROM T_ITRRecords_8100_002A_Radio_Test WHERE ITRCommonID = '" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID; // AND IsUpdated = 1
                    var Radio_TestData = await _ITRRecords_8100_002A_Radio_TestRepository.QueryAsync(Radio_Test);
                    itrDataObj.ITR8100_002ARadio_Test = Radio_TestData.ToList();

                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_8100_002A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception e)
            {
            }
            return true;
        }
        private async Task<bool> Post_8121_002A_ITRsData(string Tag, string Checksheet)
        {
            ITR8121_002AModel itrDataObj = new ITR8121_002AModel();

            try
            {
                itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string recordsqlRecords = " SELECT * FROM T_ITR8121_002A_Records WHERE ITRCommonID='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var T_Record = await _ITR8121_002A_Records.QueryAsync(recordsqlRecords);
                    itrDataObj.ITR_8121_002A_Records = T_Record.FirstOrDefault();

                    if (itrDataObj.ITR_8121_002A_Records != null)
                    {
                        string recordsql1 = " SELECT * FROM T_ITR8121_002A_TransformerRadioTest WHERE ID_8121_002A_TransformerRadioTest='" + itrDataObj.ITR_8121_002A_Records.ID + "' AND CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                        var T_TransformerRadioTest = await _ITR8121_002A_TransformerRadioTest.QueryAsync(recordsql1);
                        itrDataObj.TransformerRadioTests = new List<T_ITR8121_002A_TransformerRadioTest>(T_TransformerRadioTest);

                        string recordsql2 = " SELECT * FROM T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents WHERE ID_ITR_8121_002A_InspectionforControl='" + itrDataObj.ITR_8121_002A_Records.ID + "' AND CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                        var T_InspectionforControls = await _ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents.QueryAsync(recordsql2);
                        itrDataObj.InspectionControlAndACCs = new List<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents>(T_InspectionforControls);
                    }

                     var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_8121_002A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);

                }
            }
            catch (Exception Ex)
            {

            }

            return true;
        }

        private async Task<bool> Post_8260_002A_ITRsData(string Tag, string Checksheet)
        {
            ITR_8260_002AModel itrDataObj = new ITR_8260_002AModel();

            try
            {
                itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string sqlQuery = " SELECT * FROM T_ITR_8260_002A_Body WHERE ITRCommonID='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var T_Record = await _ITR_8260_002A_BodyRepository.QueryAsync(sqlQuery);
                    itrDataObj.ITR_Body = T_Record.FirstOrDefault();

                     string sqlQuery1 = " SELECT * FROM T_ITR_8260_002A_DielectricTest WHERE ITRCommonID='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                     var T_DielectricTest = await _ITR_8260_002A_DielectricTestRepository.QueryAsync(sqlQuery1);
                     itrDataObj.ITR_DielectricTestList = new List<T_ITR_8260_002A_DielectricTest>(T_DielectricTest);

                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_8260_002A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception Ex)
            {

            }

            return true;
        }
        private async Task<bool> Post_8161_001A_ITRsData(string Tag, string Checksheet)
        {
            try
            {
                ITR8161_001AModel itrDataObj = new ITR8161_001AModel();

                itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string recordsql = " SELECT * FROM T_ITRRecords_8161_001A_Body WHERE ITRCommonID ='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var Recorddata = await _ITRRecords_8161_001A_BodyRepository.QueryAsync(recordsql);
                    itrDataObj.ITR_8161_001A_Body = Recorddata.FirstOrDefault();

                    string InsRes = "SELECT * FROM T_ITRRecords_8161_001A_InsRes WHERE ITRCommonID = '" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID; // AND IsUpdated = 1
                    var InsulationDetailsData = await _ITRRecords_8161_001A_InsResRepository.QueryAsync(InsRes);
                    itrDataObj.ITR_8161_001A_InsRes = InsulationDetailsData.ToList();

                    string Radio_Test = "SELECT * FROM T_ITRRecords_8161_001A_ConRes WHERE ITRCommonID = '" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID; // AND IsUpdated = 1
                    var Radio_TestData = await _ITRRecords_8161_001A_ConResRepository.QueryAsync(Radio_Test);
                    itrDataObj.ITR_8161_001A_ConRes = Radio_TestData.ToList();

                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_8161_001A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception e)
            {
            }
            return true;
        }
        private async Task<bool> Post_8121_004A_ITRsData(string Tag, string Checksheet)
        {
            try
            {
                ITR8121_004AModel itrDataObj = new ITR8121_004AModel();
                itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet);
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string recordsql = " SELECT * FROM T_ITR8121_004ATestInstrumentData WHERE ITRCommonID='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var TIRecorddata = await _ITR8121_004ATestInstrumentDataRepository.QueryAsync(recordsql);
                    itrDataObj.ITR8121_004A_TestInstrumentData = TIRecorddata.FirstOrDefault();

                    string InCAndAuxiliarysql = " SELECT * FROM T_ITR8121_004AInCAndAuxiliary WHERE ITRCommonID='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var InCAndAuxiliaryData = await _ITR8121_004AInCAndAuxiliaryRepository.QueryAsync(InCAndAuxiliarysql);
                    itrDataObj.ITR8121_004A_InispactionForControlAndAuxiliary = InCAndAuxiliaryData.ToList();

                    string TransformerRatioTestsql = " SELECT * FROM T_ITR8121_004ATransformerRatioTest WHERE ITRCommonID='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var TransformerRatioTestData = await _ITR8121_004ATransformerRatioTestRepository.QueryAsync(TransformerRatioTestsql);
                    itrDataObj.ITR8121_004A_TransformerRatioTest = TransformerRatioTestData.ToList();

                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_8121_004A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception e)
            {
            }
            return true;
        }
        private async Task<bool> Post_8161_002A_ITRsData(string Tag, string Checksheet)
        {
            ITR8161_002AModel itrDataObj = new ITR8161_002AModel();

            try
            {
                itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string sqlQuery = " SELECT * FROM T_ITR8161_002A_Body WHERE ITRCommonID='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var T_Record = await _ITR8161_002A_BodyRepository.QueryAsync(sqlQuery);
                    itrDataObj.Itr8161_002A_Body = T_Record.FirstOrDefault();

                    string sqlQuery1 = " SELECT * FROM T_ITR8161_002A_DielectricTest WHERE ITRCommonID='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var T_DielectricTest = await _ITR8161_002A_DielectricTestRepository.QueryAsync(sqlQuery1);
                    itrDataObj.ITR8161_002A_DielectricTest = new List<T_ITR8161_002A_DielectricTest>(T_DielectricTest);

                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_8161_002A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception Ex)
            {

            }

            return true;
        }
        private async Task<bool> Post_8000_101A_ITRsData(string Tag, string Checksheet)
        {
            ITR8000_101AModel itrDataObj = new ITR8000_101AModel();

            try
            {
                itrDataObj.CommonHeaderFooter = itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string sqlQuery = " SELECT * FROM T_ITR8000_101A_Generalnformation WHERE ITRCommonID='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var T_Record = await _ITR8000_101A_GeneralnformationRepository.QueryAsync(sqlQuery);
                    itrDataObj.ITR_8000_101A_Generalnformation = T_Record.FirstOrDefault();

                    string sqlQuery1 = " SELECT * FROM T_ITR8000_101A_RecordISBarrierDetails WHERE ITRCommonID='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var T_BarrierDetails = await _ITR8000_101A_RecordISBarrierDetailsRepository.QueryAsync(sqlQuery1);
                    itrDataObj.ITR_8000_101A_RecordISBarrierDetails = T_BarrierDetails.FirstOrDefault();

                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_8000_101A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception Ex)
            {

            }

            return true;
        }
        private async Task<bool> Post_8211_002A_ITRsData(string Tag, string Checksheet)
        {
            try
            {
                ITR8211_002AModel itrDataObj = new ITR8211_002AModel();

                itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string recordsql = " SELECT * FROM T_ITRRecords_8211_002A_Body WHERE ITRCommonID ='" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var Recorddata = await _ITRRecords_8211_002A_BodyRepository.QueryAsync(recordsql);
                    itrDataObj.ITR_8211_002A_Body = Recorddata.FirstOrDefault();

                    string InsRes = "SELECT * FROM T_ITRRecords_8211_002A_RunTest WHERE ITRCommonID = '" + itrDataObj.CommonHeaderFooter.ID + "' AND CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var InsulationDetailsData = await _ITRRecords_8211_002A_RunTestRepository.QueryAsync(InsRes);
                    itrDataObj.ITR_8211_002A_RunTest = InsulationDetailsData.ToList();

                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_8211_002A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception e)
            {
            }
            return true;
        }
        private async Task<bool> Post_7000_101AHarmony_ITRsData(string Tag, string Checksheet)
        {
            ITR7000_101AHarmonyModel itrDataObj = new ITR7000_101AHarmonyModel();

            try
            {
                itrDataObj.CommonHeaderFooter = itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string sqlQuery = " SELECT * FROM T_ITR_7000_101AHarmony_Genlnfo WHERE ITRNumber='" + itrDataObj.CommonHeaderFooter.ITRNumber + "' AND CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var T_Record = await _ITR7000_101AHarmony_GenlnfoRepository.QueryAsync(sqlQuery);
                    itrDataObj.ITR_7000_101AHarmony_Genlnfo = T_Record.FirstOrDefault();

                    string sqlQuery1 = " SELECT * FROM T_ITR_7000_101AHarmony_ActivityDetails WHERE CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var T_BarrierDetails = await _ITR7000_101AHarmony_ActivityDetailsRepository.QueryAsync(sqlQuery1);
                    itrDataObj.ITR_7000_101AHarmony_ActivityDetails = T_BarrierDetails.FirstOrDefault();

                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_7000_101AHarmony(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception Ex)
            {

            }

            return true;
        }
        private async Task<bool> Post_8140_002A_ITRsData(string Tag, string Checksheet)
        {
            ITR8140_002AModel itrDataObj = new ITR8140_002AModel();

            try
            {
                itrDataObj.CommonHeaderFooter = itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string sqlQuery = " SELECT * FROM T_ITR_8140_002A_Records WHERE ITRNumber='" + itrDataObj.CommonHeaderFooter.ITRNumber + "' AND CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var T_Record = await _ITR_8140_002A_RecordsRepository.QueryAsync(sqlQuery);
                    itrDataObj._8140_002A_Record = T_Record.FirstOrDefault();

                    string sqlQuery1 = " SELECT * FROM T_ITR_8140_002A_RecordsMechnicalOperation WHERE CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var T_BarrierDetails = await _ITR_8140_002A_RecordsMechnicalOperation_RecordsRepository.QueryAsync(sqlQuery1);
                    itrDataObj._8140_002A_RecordsMechnical = T_BarrierDetails.FirstOrDefault();

                    string sqlQuery2 = " SELECT * FROM T_ITR_8140_002A_RecordsAnalogSignal WHERE CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var T_AnalogSignal = await _ITR_8140_002A_RecordsAnalogSignalRepository.QueryAsync(sqlQuery2);
                    itrDataObj._8140_002A_RecordsAnalogSignal = T_AnalogSignal.FirstOrDefault();
                    string data = JsonConvert.SerializeObject(itrDataObj);

                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_8140_002A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception Ex)
            {

            }

            return true;
        }
        private async Task<bool> Post_8140_004A_ITRsData(string Tag, string Checksheet)
        {
            ITR_8140_004AModel itrDataObj = new ITR_8140_004AModel();

            try
            {
                itrDataObj.CommonHeaderFooter = itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string sqlQuery = " SELECT * FROM T_ITR_8140_004A_Records WHERE ITRNumber='" + itrDataObj.CommonHeaderFooter.ITRNumber + "' AND CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var T_Record = await _ITR_8140_004A_RecordsRepository.QueryAsync(sqlQuery);
                    itrDataObj._8140_004A_Record = T_Record.FirstOrDefault();

                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_8140_004A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception Ex)
            {

            }

            return true;
        }
        private async Task<bool> Post_8170_002A_ITRsData(string Tag,string Checksheet)
        {
            ITR8170_002AModel itrDataObj = new ITR8170_002AModel();

            try
            {
                itrDataObj.CommonHeaderFooter = itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string sqlQuery = " SELECT * FROM T_ITR_8170_002A_IndifictionLamp WHERE CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var T_Record = await _ITR_8170_002A_IndifictionLampRepository.QueryAsync(sqlQuery);
                    itrDataObj.ITR_8170_002A_IndifictionLamp = T_Record.FirstOrDefault();

                    string sqlQuery1 = " SELECT * FROM T_ITR_8170_002A_InsRes WHERE CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var InsRes = await _ITR_8170_002A_InsResRepository.QueryAsync(sqlQuery1);
                    itrDataObj.ITR_8170_002A_InsRes = InsRes.FirstOrDefault();

                    string VoltRead = "SELECT * FROM T_ITRRecords_8170_002A_Voltage_Reading WHERE CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var VoltReading = await _ITRRecords_8170_002A_Voltage_ReadingRepository.QueryAsync(VoltRead);
                    itrDataObj.ITR_8170_002A_Voltage_Reading = VoltReading.ToList();

                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_8170_002A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }                          
            catch (Exception Ex)
            {

            }

            return true;
        }

        private async Task<bool> Post_8300_003A_ITRsData(string Tag, string Checksheet)
        {
            ITR_8300_003AModel itrDataObj = new ITR_8300_003AModel();

            try
            {
                itrDataObj.CommonHeaderFooter = itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string sqlQuery = " SELECT * FROM T_ITR_8300_003A_Body WHERE CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var T_Record = await _ITR_8300_003A_BodyRepository.QueryAsync(sqlQuery);
                    itrDataObj.ITR_Body = T_Record.FirstOrDefault();

                   

                    string Illumin = "SELECT * FROM T_ITR_8300_003A_Illumin WHERE CommonRowID = '" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var Illuminlst = await _ITR_8300_003A_IlluminRepository.QueryAsync(Illumin);
                    itrDataObj.ITR_IlluminList = Illuminlst.ToList();

                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_8300_003A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception Ex)
            {

            }

            return true;
        }
        private async Task<bool> Post_8170_007A_ITRsData(string Tag, string Checksheet)
        {
            ITR8170_007AModel itrDataObj = new ITR8170_007AModel();

            try
            {
                itrDataObj.CommonHeaderFooter = itrDataObj.CommonHeaderFooter = await GetCommonHeaderFooterDataforPost(Tag, Checksheet); //CommonHeaderFooter.FirstOrDefault();
                if (itrDataObj.CommonHeaderFooter != null)
                {
                    string sqlQuery = " SELECT * FROM T_ITR_8170_007A_OP_Function_Test WHERE CommonRowID ='" + itrDataObj.CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + itrDataObj.CommonHeaderFooter.ID;
                    var T_Record = await _ITR_8170_007A_OP_Function_TestRepository.QueryAsync(sqlQuery);
                    itrDataObj.ITR_8170_007A_OP_Function_Test = T_Record.FirstOrDefault();

                    var UploadResult = ModsTools.CompletionWebServicePost(ApiUrls.Post_8170_007A(Settings.CurrentDB), JsonConvert.SerializeObject(itrDataObj), Settings.CompletionAccessToken);
                }
            }
            catch (Exception Ex)
            {

            }

            return true;
        }
        private async Task<T_ITRCommonHeaderFooter> GetCommonHeaderFooterDataforPost(string Tag, string Checksheet)
        {
            T_ITRCommonHeaderFooter CommonHeaderFooter = new T_ITRCommonHeaderFooter();
            try
            {
                var CommonHeaderFooterdata = await _CommonHeaderFooterRepository.QueryAsync("Select * From T_ITRCommonHeaderFooter WHERE Tag = '" + Tag + "' AND ITRNumber = '" + Checksheet + "' AND ModelName = '" + Settings.ModelName + "' AND IsUpdated = 1");
                 CommonHeaderFooter = CommonHeaderFooterdata.FirstOrDefault();

                if (CommonHeaderFooter != null)
                {
                    if (CommonHeaderFooter.ReportNo == null)
                        CommonHeaderFooter.ReportNo = "";
                    if (CommonHeaderFooter.DrawingNo == null)
                        CommonHeaderFooter.DrawingNo = "";
                    var CommonHeaderFooterSignOff = await _CommonHeaderFooterSignOffRepository.QueryAsync("Select * From T_CommonHeaderFooterSignOff WHERE SignOffTag = '" + Tag + "' AND SignOffChecksheet = '" + Checksheet + "' AND ModelName = '" + Settings.ModelName + "'"
                                                 + " AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND ITRCommonID = '" + CommonHeaderFooter.ID + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID);
                    CommonHeaderFooter.CommonHeaderFooterSignOff = CommonHeaderFooterSignOff.ToList();

                    var InstrumentData = await _ITRInstrumentDataRepository.QueryAsync("Select * From T_ITRInstrumentData WHERE ITRCommonID = '" + CommonHeaderFooter.ID + "' AND CommonRowID = '" + CommonHeaderFooter.ROWID + "' AND ModelName = '" + Settings.ModelName + "' AND CCMS_HEADERID = " + CommonHeaderFooter.ID);
                    CommonHeaderFooter.ITRInstrumentData = InstrumentData.ToList();
                }
            }
            catch (Exception e)
            {
                return CommonHeaderFooter;

            }
            return CommonHeaderFooter;
        }
    }
}

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
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using System.Reflection;
using Plugin.Connectivity;

namespace JGC.ViewModels.Completions
{
    public class TagRegisterViewModel : BaseViewModel
    {
        public readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_TAG> _TAGRepository;
        private readonly IRepository<T_CHECKSHEET> _CheckSheetRepository;
        private readonly IRepository<T_CHECKSHEET_QUESTION> _CheckSheetQuetionsRepository;
        private readonly IRepository<T_CHECKSHEET_PER_TAG> _CheckSheetPerTagRepository;
        private readonly IRepository<T_SignOffHeader> _SignOffHeaderRepository;
        private readonly IRepository<T_TAG_SHEET_HEADER> _TagSheetHeaderRepository;
        private readonly IRepository<T_TAG_SHEET_ANSWER> _TAG_SHEET_ANSWERRepository;
        private readonly IRepository<T_UserControl> _T_UserControlRepository;
        private readonly IRepository<T_CompletionsUsers> _CompletionsUserRepository;

        public List<T_TAG> _tagsList;
        public bool IsVisibleRemarkSection;

        public List<TableFunctionModel> TableFunctionData = new List<TableFunctionModel>();
        public static CompletionPageHelper _CompletionpageHelper = new CompletionPageHelper();
        public bool Completed, Started, rejected, CheckSheetCompleted, IsCheckSheetAccessible, IsVisibleRejectButton, IsChecksheetIntiled;
        private readonly IRepository<T_Ccms_signature> _Ccms_signatureRepository;

        List<bool> comparer = new List<bool>();
        public string ITR_STATUS_COMPLETE = "#00FF00";
        public string ITR_STATUS_STARTED = "#FF8000";
        string Outstanding = "#FF0000";
        //string REjected = "#FF0000";
        string statusNoJIData = "#D3D3D3";
        bool AbletoReject;
        string CCTRImagePath;
        List<CctrImage> CCTRImageSourceList;
        // private INavigation navi;

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

        private ObservableCollection<dynamic> itemSourceTagList;
        public ObservableCollection<dynamic> ItemSourceTagList
        {
            get { return itemSourceTagList; }
            set { itemSourceTagList = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_CHECKSHEET_PER_TAG> itemSourceCheckSheets;
        public ObservableCollection<T_CHECKSHEET_PER_TAG> ItemSourceCheckSheets
        {
            get { return itemSourceCheckSheets; }
            set { itemSourceCheckSheets = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<CheckSheetQuetionList> itemSourceQuetions;
        public ObservableCollection<CheckSheetQuetionList> ItemSourceQuetions
        {
            get { return itemSourceQuetions; }
            set { itemSourceQuetions = value; RaisePropertyChanged(); }
        }

        private CheckSheetQuetionList _selectedSourceQuetions;
        public CheckSheetQuetionList SelectedSourceQuetions
        {
            get { return _selectedSourceQuetions; }
            set
            {
                _selectedSourceQuetions = value;
                RaisePropertyChanged();
            }
        }

        private T_TAG selectedTag;
        public T_TAG SelectedTag
        {
            get { return selectedTag; }
            set { selectedTag = value; RaisePropertyChanged(); OnTagSelectionChage(); }
        }
        private T_CHECKSHEET_PER_TAG selectedCheckSheet;
        public T_CHECKSHEET_PER_TAG SelectedCheckSheet
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
        private ObservableCollection<T_SignOffHeader> signOffList;
        public ObservableCollection<T_SignOffHeader> SignOffList
        {
            get { return signOffList; }
            set { signOffList = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_TAG_SHEET_HEADER> tagSheetHeadersList;
        public ObservableCollection<T_TAG_SHEET_HEADER> TagSheetHeadersList
        {
            get { return tagSheetHeadersList; }
            set
            {
                tagSheetHeadersList = value;
                RaisePropertyChanged();
                // OnPropertyChanged();
            }
        }


        public List<string> UpdatedChecksheetsID = new List<string>();
        public List<string> UpdatedChecksheetsValuesID = new List<string>();


        private ObservableCollection<AdditionalFieldsModel> additionalFields;
        public ObservableCollection<AdditionalFieldsModel> AdditionalFields
        {
            get { return additionalFields; }
            set { additionalFields = value; RaisePropertyChanged(); }
        }

        private string remarkEntry;
        public string RemarkEntry
        {
            get { return remarkEntry; }
            set { remarkEntry = value; RaisePropertyChanged(); }
        }

        private T_TAG_SHEET_ANSWER remarkAnswer;
        public T_TAG_SHEET_ANSWER RemarkAnswer
        {
            get { return remarkAnswer; }
            set { remarkAnswer = value; RaisePropertyChanged(); }
        }
        private bool isStackHeader;
        public bool IsStackHeader
        {
            get { return isStackHeader; }
            set { isStackHeader = value; RaisePropertyChanged(); }
        }
        #endregion

        public async Task<bool> OnCheckSheetSelectionChage()
        {
            try
            {
                IsVisibleRemarkSection = false;

                TagSheetHeadersList = new ObservableCollection<T_TAG_SHEET_HEADER>();
                //Check USer Accesssss
                IsVisibleRejectButton = false;
                comparer = new List<bool>();
                if (SelectedCheckSheet.StatusColor != ITR_STATUS_COMPLETE)
                {
                    IsChecksheetIntiled = false;
                    CheckSheetCompleted = false;
                    IsCheckSheetAccessible = true;
                }
                else OpenITR();

                //Get CheckSheets

                var checkSheets = await _CheckSheetQuetionsRepository.GetAsync(x => x.CheckSheet == SelectedCheckSheet.CHECKSHEETNAME);

                // var checkSheetsPerCategory = checkSheets.GroupBy(i => i.section);

                //get Jack 
                var checkSheetPerTag = await _CheckSheetPerTagRepository.GetAsync(x => x.CHECKSHEETNAME == SelectedCheckSheet.CHECKSHEETNAME && x.TAGNAME == SelectedTag.name);


                //    //get CheckSheet Data
                var _CurrentCheckSheet = await _CheckSheetRepository.GetAsync(x => x.name == SelectedCheckSheet.CHECKSHEETNAME);
                CurrentCheckSheet = _CurrentCheckSheet.FirstOrDefault();

                string ansCSsql = " SELECT * FROM T_TAG_SHEET_ANSWER WHERE tagName='" + SelectedTag.name + "' AND checkSheetName='" + SelectedCheckSheet.CHECKSHEETNAME + "' AND jobPack = '" + checkSheetPerTag.FirstOrDefault().JOBPACK.Trim() + "' or jobPack ISNULL";
                var Answer_CheckSheet = await _TAG_SHEET_ANSWERRepository.QueryAsync(ansCSsql);
                int count = 1;
                List<CheckSheetQuetionList> _checkSheetQuetionList = new List<CheckSheetQuetionList>();
                List<String> questions = new List<String>();
                List<int> Rows = new List<int>();
                int Trow = 0, Tcol = 0;

                foreach (T_CHECKSHEET_QUESTION checkSheetItem in checkSheets)
                {
                    String type = checkSheetItem.section; //.Select(i => i.section).FirstOrDefault();
                    TableFunctionData = new List<TableFunctionModel>();
                    List<CctrImage> cctrimage = new List<CctrImage>();
                    if (type.Contains("QUESTION") || type.ToUpper().Contains("IMAGE"))
                    {

                        //foreach (T_CHECKSHEET_QUESTION checkSheetItem in checkSheetsInForm)
                        //{

                        cctrimage = await GetCCTRImages(checkSheetItem.id);

                        //int index = checkSheetItem.description.IndexOf("-");
                        //if (index < 0)
                        //{//slightly different dash character can come through
                        //    index = checkSheetItem.description.IndexOf("–");
                        //}

                        //string[] myList = QueTabFuncItem.Question.Split('-');
                        //string finalString = String.Join(", ", myList.ToArray(), 0, myList.Count() - 1) + ", and " + myList.LastOrDefault();

                        String itemNo = checkSheetItem.itemNo;
                        String[] SplitcheckSheet = checkSheetItem.description.Split('-');


                        int index = checkSheetItem.description.LastIndexOf('-');
                        String question;
                        String columnName;
                        if (index + 1 < checkSheetItem.description.Length)
                        {
                            question = checkSheetItem.description.Substring(0, index);
                            columnName = checkSheetItem.description.Substring(index + 1);
                        }
                        else
                            continue;


                        String[] splitStrings = itemNo.Split('_');
                        if (splitStrings.Length != 3)
                        {
                            // LogHelperFactory.getLogger().Error(CheckSheetUtil.class, "Parsed itemNo not properly formatted: " + itemNo, null);
                            continue;
                        }

                        String fieldType = splitStrings[0];
                        if (checkSheetItem.answerOptions != null && !String.IsNullOrEmpty(checkSheetItem.answerOptions))
                        {
                            fieldType = "DROPDOWN";
                        }




                        if (index < 0)
                        {
                            continue;
                        }
                        else
                        {
                            if (SplitcheckSheet.Count() == 3)
                            {
                                question = SplitcheckSheet[0].Trim() + SplitcheckSheet[1].Trim();
                                columnName = SplitcheckSheet[2].Trim();
                            }
                            else if (SplitcheckSheet.Count() == 4)
                            {
                                question = SplitcheckSheet[0].Trim() + SplitcheckSheet[1].Trim() + SplitcheckSheet[2].Trim();
                                columnName = SplitcheckSheet[3].Trim();
                            }
                            else
                            {
                                question = SplitcheckSheet[0].Trim();
                                columnName = SplitcheckSheet[1].Trim();
                            }
                        }

                        //CheckSheetQuestionListItem checkSheetQuestion = null;
                        //List<CheckSheetQuetionList> _checkSheetQuetionList = new List<CheckSheetQuetionList>();
                        List<AdditionalFieldsModel> Fields = new List<AdditionalFieldsModel>();

                        if (!questions.Contains(question + splitStrings[1]))
                        {

                            questions.Add(question + splitStrings[1]);

                            string fieldValue = "";

                            string strSQL = @"SELECT * FROM T_TAG_SHEET_ANSWER WHERE [checkSheetName] = '" + checkSheetItem.CheckSheet + "' AND [itemno] = '" + checkSheetItem.itemNo + "' AND [tagName] = '" + SelectedTag.name + "'";
                            var CheckAns = await _TAG_SHEET_ANSWERRepository.QueryAsync(strSQL);

                            if (CheckAns.Count > 0)
                            {
                                T_TAG_SHEET_ANSWER CheckAnsitem = CheckAns.FirstOrDefault();
                                if (CheckAnsitem.itemno.ToLower().Contains("datetime"))
                                    fieldValue = CheckAnsitem.isDate.ToString("dd-MM-yyyy");
                                //else if (CheckAnsitem.itemno.ToLower().Contains("bool"))
                                //    fieldValue = CheckAnsitem.checkValue;
                                else
                                    fieldValue = CheckAnsitem.checkValue;
                            }
                            //fieldValue = CheckAns.Select(item => item.checkValue).FirstOrDefault();



                            List<AnswerOptions> Ans = new List<AnswerOptions>();
                            foreach (string str in checkSheetItem.answerOptions.Split(',').ToList())
                            {
                                if (!String.IsNullOrEmpty(str))
                                {
                                    string ans = str;
                                    ; TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                                    if (str != "NA")
                                        ans = textInfo.ToTitleCase(str.ToLower());
                                    Ans.Add(new AnswerOptions { id = checkSheetItem.id, CheckSheet = checkSheetItem.CheckSheet, itemNo = checkSheetItem.itemNo, Options = ans, isSelected = fieldValue.ToLower() == ans.ToLower() ? "#006633" : "White" });
                                }
                            }

                            if (!columnName.ToLower().Contains("punch") && !columnName.ToUpper().Contains("RESULTS") && !columnName.ToUpper().Contains("ACCEPTED") && !columnName.ToUpper().Contains("IMAGE"))
                            {
                                Fields.Add(new AdditionalFieldsModel()
                                {
                                    ID = checkSheetItem.id,
                                    FieldName = columnName,
                                    FieldValue = fieldValue,
                                    IsEnabledField = (IsCheckSheetAccessible) ? true : false,
                                    FieldPlaceHolder = ConvertStringToCamelCase(columnName.ToLower()),
                                    answerOptions = new List<AnswerOptions>(),
                                    IsVisibleAnsField = false,
                                });

                            }
                            _checkSheetQuetionList.Add(new CheckSheetQuetionList()
                            {
                                srNo = count.ToString() + ": ",
                                id = checkSheetItem.id,
                                Quetion = ConvertStringToCamelCase(question.ToLower()),
                                answerOptional = checkSheetItem.answerOptional,
                                section = checkSheetItem.section,
                                CheckSheet = checkSheetItem.CheckSheet,
                                answerOptions = Ans,
                                itemNo = checkSheetItem.itemNo,
                                AdditionalFields = Fields,
                                IsVisibleAnsField = Ans.Count > 0 ? true : false,
                                TableFunc_List = new List<TableFunctionModel>(),
                                IsVisibleTableFunction = false,
                                CCTRImageList = cctrimage,
                                IsVisibleImagefield = cctrimage.Count != 0 ? true : false,
                                AnswerType = columnName,
                            });
                            count++;
                        }
                        else
                        {
                            string fieldValue = "";

                            string strSQL = @"SELECT * FROM T_TAG_SHEET_ANSWER WHERE [checkSheetName] = '" + checkSheetItem.CheckSheet + "' AND [itemno] = '" + checkSheetItem.itemNo + "' AND [tagName] = '" + SelectedTag.name + "'";
                            var CheckAns = await _TAG_SHEET_ANSWERRepository.QueryAsync(strSQL);


                            if (CheckAns.Count > 0)
                            {
                                T_TAG_SHEET_ANSWER CheckAnsitem = CheckAns.FirstOrDefault();
                                if (CheckAnsitem.itemno.ToLower().Contains("datetime"))
                                    fieldValue = CheckAnsitem.isDate.ToString("dd-MM-yyyy");
                                //else if (CheckAnsitem.itemno.ToLower().Contains("bool"))
                                //    fieldValue = CheckAnsitem.checkValue;
                                else
                                    fieldValue = CheckAnsitem.checkValue;
                            }
                            if (columnName.ToLower().Contains("punch"))
                            {
                                List<AnswerOptions> Ans = new List<AnswerOptions>();
                                foreach (string str in checkSheetItem.answerOptions.Split(',').ToList())
                                {
                                    if (!String.IsNullOrEmpty(str))
                                    {
                                        string ans = str;
                                        ; TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                                        if (str != "NA")
                                            ans = textInfo.ToTitleCase(str.ToLower());
                                        Ans.Add(new AnswerOptions { id = checkSheetItem.id, CheckSheet = checkSheetItem.CheckSheet, itemNo = checkSheetItem.itemNo, Options = ans, isSelected = fieldValue.ToLower() == ans.ToLower() ? "#006633" : "White" });
                                    }
                                }
                                _checkSheetQuetionList.LastOrDefault().AdditionalFields.Add(new AdditionalFieldsModel()
                                {
                                    ID = checkSheetItem.id,
                                    FieldName = columnName,
                                    FieldValue = fieldValue,
                                    IsEnabledField = (IsCheckSheetAccessible) ? true : false,
                                    FieldPlaceHolder = ConvertStringToCamelCase(columnName.ToLower()),
                                    answerOptions = Ans,
                                    IsVisibleAnsField = Ans.Count > 0 ? true : false,
                                });
                            }
                            else if (!columnName.ToUpper().Contains("RESULTS"))
                            {
                                _checkSheetQuetionList.LastOrDefault().AdditionalFields.Add(new AdditionalFieldsModel()
                                {
                                    ID = checkSheetItem.id,
                                    FieldName = columnName,
                                    FieldValue = fieldValue,
                                    IsEnabledField = (IsCheckSheetAccessible) ? true : false,
                                    FieldPlaceHolder = ConvertStringToCamelCase(columnName.ToLower()),
                                    answerOptions = new List<AnswerOptions>(),
                                    IsVisibleAnsField = false,
                                });
                            }
                        }

                        if (_checkSheetQuetionList == null)
                        {
                            //checkSheetQuestion = questionTable.questionList.get(count - 2);
                        }
                        //}
                        //ItemSourceQuetions = new ObservableCollection<CheckSheetQuetionList>(_checkSheetQuetionList);
                    }
                    else if (!type.Contains("PUNCH"))
                    {
                        ////start
                        //List<String> columns = new List<String>();

                        //T_CHECKSHEET_QUESTION checkSheetItemQue = checkSheetItem;
                        //foreach (T_CHECKSHEET_QUESTION checkSheetItemQue in checkSheetsInForm.ToList())
                        //{

                        String answerId = checkSheetItem.itemNo;
                        // parsing the itemNo to get the form location and type of this box.

                        String itemNo = checkSheetItem.itemNo;
                        String[] splitStrings = itemNo.Split('_');
                        if (splitStrings.Length != 3)
                        {
                            //LogHelperFactory.getLogger().Error(CheckSheetUtil.class, "Parsed itemNo not properly formatted: " + itemNo, null);
                            continue;
                        }
                        String fieldType = splitStrings[0];
                        String rowNumber = splitStrings[1];
                        String columnCharacter = splitStrings[2];


                        if (String.IsNullOrEmpty(fieldType) || String.IsNullOrEmpty(rowNumber) || String.IsNullOrEmpty(columnCharacter))
                        {
                            //LogHelperFactory.getLogger().Error(CheckSheetUtil.class, "Error processing a check sheet item", null);
                            continue;
                        }

                        //if (!columns.Contains(columnCharacter))
                        //{
                        if (checkSheetItem.section == "REMARKS" || checkSheetItem.section == "GENERAL REMARKS")
                        {
                            IsVisibleRemarkSection = true;
                            string strSQL = @"SELECT * FROM T_TAG_SHEET_ANSWER WHERE [checkSheetName] = '" + checkSheetItem.CheckSheet + "' AND [itemno] = '" + checkSheetItem.itemNo + "' AND [tagName] = '" + SelectedTag.name + "'";
                            var CheckAns = await _TAG_SHEET_ANSWERRepository.QueryAsync(strSQL);
                            if (CheckAns.Any())
                            {
                                RemarkAnswer = CheckAns.FirstOrDefault();
                                RemarkEntry = CheckAns.FirstOrDefault().checkValue;
                            }
                        }
                        else
                        {
                            string[] DescriptionType = checkSheetItem.description.Split('-');
                            string strSQL = @"SELECT * FROM T_TAG_SHEET_ANSWER WHERE [checkSheetName] = '" + checkSheetItem.CheckSheet + "' AND [itemno] = '" + checkSheetItem.itemNo + "' AND [tagName] = '" + SelectedTag.name + "'";
                            var CheckAns = await _TAG_SHEET_ANSWERRepository.QueryAsync(strSQL);

                            string fieldValue = "";
                            if (CheckAns.Count > 0)
                            {
                                T_TAG_SHEET_ANSWER Item = CheckAns.Select(item => item).FirstOrDefault();
                                string VALUETYPE = Item.itemno.Split('_')[0].ToUpper();
                                switch (VALUETYPE)
                                {
                                    case "DATETIME":
                                        if (Item.isDate > new DateTime(2000, 01, 01))
                                            fieldValue = Item.isDate.ToString("dd/MM/yyyy");
                                        break;
                                    //case "BOOLEAN":
                                    //    fieldValue = Item.isChecked.ToString();
                                    //    break;
                                    default:
                                        fieldValue = Item.checkValue.ToString();
                                        break;
                                }
                                //fieldValue = CheckAns.Select(item => item.checkValue).FirstOrDefault();
                            }

                            if (!Rows.Contains(Convert.ToInt32(rowNumber)))
                            {
                                Tcol = 0;
                                if (Rows.LastOrDefault() == Convert.ToInt32(rowNumber) - 1 && _checkSheetQuetionList.Count > 0)
                                {
                                    Trow++;
                                    _checkSheetQuetionList.LastOrDefault().TableFunc_List.Add(new TableFunctionModel
                                    {
                                        id = checkSheetItem.id,
                                        TagName = SelectedTag.name,
                                        CheckSheet = checkSheetItem.CheckSheet,
                                        section = checkSheetItem.section,
                                        itemNo = checkSheetItem.itemNo,
                                        ProjectName = checkSheetItem.ProjectName,
                                        CheckValue = fieldValue,
                                        Description = DescriptionType[0].Trim(),
                                        TypeValue = DescriptionType[1].Trim(),
                                        AnswerOptions = checkSheetItem.answerOptions,
                                        RowIndex = Trow,
                                        ColumnIndex = Tcol
                                    });
                                    Tcol++;

                                    _checkSheetQuetionList.LastOrDefault().TableFunc_List.Add(new TableFunctionModel
                                    {
                                        id = checkSheetItem.id,
                                        TagName = SelectedTag.name,
                                        CheckSheet = checkSheetItem.CheckSheet,
                                        section = checkSheetItem.section,
                                        itemNo = checkSheetItem.itemNo,
                                        ProjectName = checkSheetItem.ProjectName,
                                        CheckValue = fieldValue,
                                        Description = DescriptionType[0].Trim(),
                                        TypeValue = DescriptionType[1].Trim(),
                                        AnswerOptions = checkSheetItem.answerOptions,
                                        RowIndex = Trow,
                                        ColumnIndex = Tcol
                                    });

                                    Rows.Add(Convert.ToInt32(rowNumber));
                                }
                                else
                                {
                                    Trow = 0;
                                    TableFunctionData.Add(new TableFunctionModel
                                    {
                                        id = checkSheetItem.id,
                                        TagName = SelectedTag.name,
                                        CheckSheet = checkSheetItem.CheckSheet,
                                        section = checkSheetItem.section,
                                        itemNo = checkSheetItem.itemNo,
                                        ProjectName = checkSheetItem.ProjectName,
                                        CheckValue = fieldValue,
                                        Description = DescriptionType[0].Trim(),
                                        TypeValue = "ITEMS", //DescriptionType[1].Trim(),
                                        AnswerOptions = checkSheetItem.answerOptions,
                                        RowIndex = Trow,
                                        ColumnIndex = Tcol
                                    });
                                    Tcol++;
                                    TableFunctionData.Add(new TableFunctionModel
                                    {
                                        id = checkSheetItem.id,
                                        TagName = SelectedTag.name,
                                        CheckSheet = checkSheetItem.CheckSheet,
                                        section = checkSheetItem.section,
                                        itemNo = checkSheetItem.itemNo,
                                        ProjectName = checkSheetItem.ProjectName,
                                        CheckValue = fieldValue,
                                        Description = DescriptionType[0].Trim(),
                                        TypeValue = DescriptionType[1].Trim(),
                                        AnswerOptions = checkSheetItem.answerOptions,
                                        RowIndex = Trow,
                                        ColumnIndex = Tcol
                                    });
                                    _checkSheetQuetionList.Add(new CheckSheetQuetionList()
                                    {
                                        srNo = "",
                                        id = checkSheetItem.id,
                                        Quetion = "",//ConvertStringToCamelCase(checkSheetItem.description.ToLower()),
                                        answerOptional = checkSheetItem.answerOptional,
                                        section = checkSheetItem.section,
                                        CheckSheet = checkSheetItem.CheckSheet,
                                        answerOptions = new List<AnswerOptions>(),
                                        itemNo = checkSheetItem.itemNo,
                                        AdditionalFields = new List<AdditionalFieldsModel>(),
                                        IsVisibleAnsField = false,
                                        TableFunc_List = TableFunctionData,
                                        IsVisibleTableFunction = true,
                                        CCTRImageList = cctrimage,
                                        IsVisibleImagefield = cctrimage.Count != 0 ? true : false,
                                    });
                                    Rows.Add(Convert.ToInt32(rowNumber));
                                }
                            }
                            else
                            {
                                Tcol++;
                                _checkSheetQuetionList.LastOrDefault().TableFunc_List.Add(new TableFunctionModel
                                {
                                    id = checkSheetItem.id,
                                    TagName = SelectedTag.name,
                                    CheckSheet = checkSheetItem.CheckSheet,
                                    section = checkSheetItem.section,
                                    itemNo = checkSheetItem.itemNo,
                                    ProjectName = checkSheetItem.ProjectName,
                                    CheckValue = fieldValue,
                                    Description = DescriptionType[0].Trim(),
                                    TypeValue = DescriptionType[1].Trim(),
                                    AnswerOptions = checkSheetItem.answerOptions,
                                    RowIndex = Trow,
                                    ColumnIndex = Tcol
                                });
                            }
                        }
                    }
                    // }
                }
                if (!Answer_CheckSheet.Any())
                {
                    _userDialogs.Alert("Joint data not found.", "", "Ok");
                    return false;
                }
                ItemSourceQuetions = new ObservableCollection<CheckSheetQuetionList>(_checkSheetQuetionList);
                //get tag headers 

                var tagHeaders = await _TagSheetHeaderRepository.GetAsync(x => x.ChecksheetName == SelectedCheckSheet.CHECKSHEETNAME && x.TagName == SelectedCheckSheet.TAGNAME);
                tagHeaders.ForEach(x =>
                {
                    if (x.ColumnKey == "System") { x.ColumnValue = SelectedTag.system; }
                    if (x.ColumnKey == "Jobcode") { x.ColumnValue = Settings.JobCode; }
                });

                TagSheetHeadersList = new ObservableCollection<T_TAG_SHEET_HEADER>(tagHeaders);
                var DrowingNoDetails = TagSheetHeadersList.Where(x => x.ColumnKey == "DwgRevNo").First();
                TagSheetHeadersList.Remove(DrowingNoDetails);
                TagSheetHeadersList.Insert(5, DrowingNoDetails);
                GetFormData();

                //get SignOffList
                var signofflist = await _SignOffHeaderRepository.GetAsync(x => x.SignOffTag == SelectedCheckSheet.TAGNAME && x.SignOffChecksheet == SelectedCheckSheet.CHECKSHEETNAME);
                var Signoff = signofflist.GroupBy(g => new { g.Title })
                        .Select(g => g.First())
                        .ToList();
                Signoff.Where(w => string.IsNullOrEmpty(w.FullName.Trim())).ToList().ForEach(s => s.FullName = "Press to Sign");
                SignOffList = new ObservableCollection<T_SignOffHeader>(Signoff);

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
                string ddlJointnoListJSON = ModsTools.CompletionWebServiceGet(ApiUrls.Url_GetFormHeaderDatafromDatahub(Settings.ProjectID, SelectedCheckSheet.TAGNAME, Settings.CurrentDB), Settings.CompletionAccessToken);
                if (ddlJointnoListJSON != string.Empty)
                {
                    var DrowingData = JsonConvert.DeserializeObject<DrowingData>(ddlJointnoListJSON);
                    if (DrowingData != null)
                    {
                        var DrowingDetails = TagSheetHeadersList.Where(x => x.ColumnKey == "Drawing").FirstOrDefault();
                        bool isempty = (string.IsNullOrEmpty(DrowingDetails.ColumnValue)) ? true : false;

                        List<string> docNos = DrowingData.Document_No;

                        TagSheetHeadersList.ForEach(s => { s.IsDropdown = false; s.IsLabel = true; });
                        if (docNos != null && docNos.Count > 1)
                        {
                            TagSheetHeadersList.Where(x => x.ColumnKey == "Drawing").ForEach(s =>
                            {
                                s.IsDropdown = true; s.IsLabel = false; s.DrawingNoList = docNos;
                                if (string.IsNullOrEmpty(s.ColumnValue)) { s.ColumnValue = docNos.First(); }
                            });
                        }
                        else if (docNos.Count == 1)
                        {
                            TagSheetHeadersList.Where(x => x.ColumnKey == "Drawing").ForEach(s => { if (string.IsNullOrEmpty(s.ColumnValue)) { s.ColumnValue = docNos.First(); } });
                        }
                        else
                        {
                            TagSheetHeadersList.Where(x => x.ColumnKey == "Drawing").ForEach(s => { if (string.IsNullOrEmpty(s.ColumnValue)) { s.ColumnValue = docNos.First(); } });
                        }

                        DrowingNo_SelectionChanged();
                    }
                }
            }
            catch (Exception e)
            {
                TagSheetHeadersList.ForEach(s => s.IsLabel = true);
            }
        }

        public void DrowingNo_SelectionChanged()
        {
            try
            {
                DrowingData itr7000SeriesFormHeader;
                var DrowingDetails = TagSheetHeadersList.Where(x => x.ColumnKey == "Drawing").FirstOrDefault();
                string ddlJointnoListJSON = ModsTools.CompletionWebServiceGet(ApiUrls.Url_GetDrowRevfromDatahub(Settings.ProjectID, DrowingDetails.ColumnValue, Settings.CurrentDB), Settings.CompletionAccessToken);

                if (ddlJointnoListJSON != string.Empty)
                {
                    itr7000SeriesFormHeader = JsonConvert.DeserializeObject<DrowingData>(ddlJointnoListJSON);
                    if (itr7000SeriesFormHeader != null && itr7000SeriesFormHeader.Rev_No != null)
                    {
                        var DrowingNoDetails = TagSheetHeadersList.Where(x => x.ColumnKey == "DwgRevNo").First();
                        TagSheetHeadersList.Remove(DrowingNoDetails);
                        string Rev_No = itr7000SeriesFormHeader.Rev_No.ToString();
                        DrowingNoDetails.ColumnValue = Rev_No;
                        TagSheetHeadersList.Insert(5, DrowingNoDetails);
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
        //public async void GetTagHeaderList()
        //{
        //    //get tag headers 
        //    var tagHeaders = await _TagSheetHeaderRepository.GetAsync(x => x.ChecksheetName == SelectedCheckSheet.CHECKSHEETNAME && x.TagName == SelectedCheckSheet.TAGNAME);
        //    // _ = Task.Delay(100);
        //    TagSheetHeadersList = new ObservableCollection<T_TAG_SHEET_HEADER>(tagHeaders);
        //}

        //public async void GetSignOffList()
        //{
        //    //get SignOffList
        //    var signofflist = await _SignOffHeaderRepository.GetAsync(x => x.SignOffTag == SelectedCheckSheet.TAGNAME && x.SignOffChecksheet == SelectedCheckSheet.CHECKSHEETNAME);
        //   var Signoff =  signofflist.GroupBy(g => new { g.Title })
        //           .Select(g => g.First())
        //           .ToList();
        //    Signoff.Where(w => string.IsNullOrEmpty(w.FullName.Trim())).ToList().ForEach(s => s.FullName = "Press to Sign");
        //    SignOffList = new ObservableCollection<T_SignOffHeader>(Signoff);
        //}
        public void ChangeAnswer(AnswerOptions Item)
        {
            comparer = new List<bool>();
            if (ItemSourceQuetions.Count > 0)
            {

                foreach (CheckSheetQuetionList item in ItemSourceQuetions.Where(i => i.id == Item.id))
                {
                    item.answerOptions.ForEach(i => i.isSelected = "White");
                    foreach (AnswerOptions AO in item.answerOptions.Where(i => i.Options == Item.Options))
                    {
                        AO.isSelected = "#006633";
                    }
                }
                foreach (AdditionalFieldsModel item in ItemSourceQuetions.SelectMany(x => x.AdditionalFields).Where(i => i.ID == Item.id))
                {
                    item.answerOptions.ForEach(i => i.isSelected = "White");
                    foreach (AnswerOptions AO in item.answerOptions.Where(i => i.Options == Item.Options))
                    {
                        AO.isSelected = "#006633";
                    }
                }
                CheckAnswerSheetCompleted();
            }
            // ItemSourceQuetions = new ObservableCollection<CheckSheetQuetionList>(ItemSourceQuetions);
        }

        public void CheckAnswerSheetCompleted()
        {
            if (ItemSourceQuetions != null)
            {
                foreach (CheckSheetQuetionList item in ItemSourceQuetions)
                {
                    if (item.answerOptions.Any())
                    {
                        if (item.answerOptions.Where(i => i.isSelected == "#006633").Any())
                            comparer.Add(true);
                        else
                            comparer.Add(false);
                    }
                }
                if (!comparer.Contains(false)) CheckSheetCompleted = true;

            }
        }
        public void CheckAnswerInitialed()
        {
            if (ItemSourceQuetions != null)
            {
                var IsAllInitiled = ItemSourceQuetions.SelectMany(x => x.AdditionalFields.Where(y => y.FieldName.Trim().ToUpper() == "INITIAL" & (y.FieldValue == "" || string.IsNullOrWhiteSpace(y.FieldValue))));
                if (IsAllInitiled.Any()) IsChecksheetIntiled = false;
                else IsChecksheetIntiled = true;
            }

        }
        public async void OnExitUpdateCheckSheets()
        {
            List<CheckSheetQuetionList> QuetionList = ItemSourceQuetions.ToList();
            List<string> UpdatedChecksheetIDList = UpdatedChecksheetsID;

            string DwgNo = TagSheetHeadersList.Where(x => x.ColumnKey == "Drawing" && x.ChecksheetName == selectedCheckSheet.CHECKSHEETNAME
            && x.TagName == selectedCheckSheet.TAGNAME).Select(s => s.ColumnValue).First();
            string DwgRevNo = TagSheetHeadersList.Where(x => x.ColumnKey == "DwgRevNo" && x.ChecksheetName == selectedCheckSheet.CHECKSHEETNAME
           && x.TagName == selectedCheckSheet.TAGNAME).Select(s => s.ColumnValue).First();

            string updateSQL = @"UPDATE T_TAG_SHEET_HEADER SET  [ColumnValue] = '" + DwgNo + "'"
                 + " WHERE [ColumnKey] = 'Drawing' AND [ChecksheetName] = '" + selectedCheckSheet.CHECKSHEETNAME + "' AND [TagName] = '" + selectedCheckSheet.TAGNAME + "'";
            await _TagSheetHeaderRepository.QueryAsync(updateSQL);

            string updateSQL1 = @"UPDATE T_TAG_SHEET_HEADER SET  [ColumnValue] = '" + DwgRevNo + "'"
                + " WHERE [ColumnKey] = 'DwgRevNo' AND [ChecksheetName] = '" + selectedCheckSheet.CHECKSHEETNAME + "' AND [TagName] = '" + selectedCheckSheet.TAGNAME + "'";
            await _TagSheetHeaderRepository.QueryAsync(updateSQL1);

            if (UpdatedChecksheetIDList.Count > 0)
            {
                List<AnswerOptions> APlist = QuetionList.ToList().SelectMany(x => x.answerOptions).ToList();
                List<AnswerOptions> AFMlist = QuetionList.ToList().SelectMany(x => x.AdditionalFields).Where(x => x.FieldName.ToLower().Contains("punch")).SelectMany(x => x.answerOptions).ToList();
                foreach (AnswerOptions AO in APlist.Where(i => UpdatedChecksheetIDList.Contains(i.id) && i.isSelected == "#006633"))
                {
                    string strSQL = @"UPDATE T_TAG_SHEET_ANSWER SET  [checkValue] = '" + AO.Options + "', [completedBy]= '" + Settings.CompletionUserName + "', [completedOn]= '" + DateTime.Now.Ticks
                                  + "', [IsSynced] = 0 WHERE [checkSheetName] = '" + AO.CheckSheet + "'" + " AND [itemno] = '" + AO.itemNo + "' AND [tagName] = '" + SelectedTag.name + "'";
                    await _TAG_SHEET_ANSWERRepository.QueryAsync(strSQL);

                    if (AbletoReject)
                    {
                        string RemoveRejectSQL = @"UPDATE T_SignOffHeader SET [rejected] = 1, [RejectedReason]='',[IsSynced] =1 "
                                                                             + " WHERE [SignOffTag] = '" + SelectedTag.name + "' AND [SignOffChecksheet] = '" + AO.CheckSheet + "'";
                        await _SignOffHeaderRepository.QueryAsync(RemoveRejectSQL);
                    }
                    else
                    {
                        string RemoveRejectSQL = @"UPDATE T_SignOffHeader SET [rejected] = 0, [RejectedReason]='',[IsSynced] =1 "
                                                                          + " WHERE [SignOffTag] = '" + SelectedTag.name + "' AND [SignOffChecksheet] = '" + AO.CheckSheet + "'";
                        await _SignOffHeaderRepository.QueryAsync(RemoveRejectSQL);
                    }
                }
                foreach (AnswerOptions AFM in AFMlist.Where(i => UpdatedChecksheetIDList.Contains(i.id) && i.isSelected == "#006633"))
                {
                    string strSQL = @"UPDATE T_TAG_SHEET_ANSWER SET  [checkValue] = '" + AFM.Options + "', [completedBy]= '" + Settings.CompletionUserName + "', [completedOn]= '" + DateTime.Now.Ticks
                                  + "', [IsSynced] = 0 WHERE [checkSheetName] = '" + AFM.CheckSheet + "'" + " AND [itemno] = '" + AFM.itemNo + "' AND [tagName] = '" + SelectedTag.name + "'";
                    await _TAG_SHEET_ANSWERRepository.QueryAsync(strSQL);

                    if (AbletoReject)
                    {
                        string RemoveRejectSQL = @"UPDATE T_SignOffHeader SET [rejected] = 1, [RejectedReason]='',[IsSynced] =1 "
                                                                             + " WHERE [SignOffTag] = '" + SelectedTag.name + "' AND [SignOffChecksheet] = '" + AFM.CheckSheet + "'";
                        await _SignOffHeaderRepository.QueryAsync(RemoveRejectSQL);
                    }
                    else
                    {
                        string RemoveRejectSQL = @"UPDATE T_SignOffHeader SET [rejected] = 0, [RejectedReason]='',[IsSynced] =1 "
                                                                          + " WHERE [SignOffTag] = '" + SelectedTag.name + "' AND [SignOffChecksheet] = '" + AFM.CheckSheet + "'";
                        await _SignOffHeaderRepository.QueryAsync(RemoveRejectSQL);
                    }
                }
                AbletoReject = false;
                UpdatedChecksheetsID.Clear();
            }
            if (UpdatedChecksheetsValuesID.Count > 0)
            {
                List<AdditionalFieldsModel> AFMlist = QuetionList.SelectMany(x => x.AdditionalFields).ToList();
                foreach (AdditionalFieldsModel AFM in AFMlist.Where(t => UpdatedChecksheetsValuesID.Contains(t.ID)))
                {
                    string Updatecheckvalue = " SELECT TSAns.* FROM T_CHECKSHEET_QUESTION as chQue inner join T_TAG_SHEET_ANSWER as TSAns on TSAns.checkSheetName = chQue.CheckSheet and TSAns.itemno = chQue.itemNo"
                                    + " WHERE chQue.id = '" + AFM.ID + "' And TSAns.tagName = '" + SelectedTag.name + "'";

                    var updateValue = await _TAG_SHEET_ANSWERRepository.QueryAsync(Updatecheckvalue);
                    if (updateValue != null)
                    {
                        T_TAG_SHEET_ANSWER TagAnsSheet = updateValue.FirstOrDefault();
                        string isdate = string.Empty;
                        if (TagAnsSheet.itemno.ToLower().Contains("datetime"))
                        {
                            try
                            {
                                //var Fields = QuetionList.Where(x => x.itemNo == TagAnsSheet.itemno).Select(x => x.AdditionalFields.FirstOrDefault());
                                //string selecteddate = Fields.FirstOrDefault().FieldValue;
                                //string selecteddate = AFM.FieldValue;
                                //isdate = ", [isDate]= '" + (string.IsNullOrEmpty(CalibarationDate)? Convert.ToDateTime(TagAnsSheet.checkValue).Ticks.ToString() : CalibarationDate) + "'";
                                isdate = ", [isDate]= '" + Convert.ToDateTime(AFM.FieldValue).Ticks.ToString() + "'";
                                AFM.FieldValue = "";
                            }
                            catch (Exception e)
                            {
                            }
                        }

                        string strSQL = @"UPDATE T_TAG_SHEET_ANSWER SET  [checkValue] = '" + AFM.FieldValue + "'" + isdate + ", [completedBy]= '" + Settings.CompletionUserName + "', [completedOn]= '" + DateTime.Now.Ticks
                                      + "', [IsSynced] = 0 WHERE [checkSheetName] = '" + TagAnsSheet.checkSheetName + "' AND [itemno] = '" + TagAnsSheet.itemno + "' AND [tagName] = '" + TagAnsSheet.tagName + "'";
                        await _TAG_SHEET_ANSWERRepository.QueryAsync(strSQL);
                    }
                }
                UpdatedChecksheetsValuesID.Clear();
            }
            if (RemarkAnswer != null)
            {
                string RemarkSQL = @"UPDATE T_TAG_SHEET_ANSWER SET  [checkValue] = '" + RemarkEntry + "', [IsSynced] = 0 WHERE [checkSheetName] = '" + RemarkAnswer.checkSheetName + "'"
                                             + " AND [itemno] = '" + RemarkAnswer.itemno + "' AND [tagName] = '" + SelectedTag.name + "'";

                await _TAG_SHEET_ANSWERRepository.QueryAsync(RemarkSQL);
            }
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





        public TagRegisterViewModel(INavigationService _navigationService,
       IUserDialogs _userDialogs,
       IHttpHelper _httpHelper,
       ICheckValidLogin _checkValidLogin,
       IRepository<T_TAG> _TAGRepository,
       IRepository<T_CHECKSHEET> _CheckSheetRepository,
       IRepository<T_CHECKSHEET_PER_TAG> _CheckSheetPerTagRepository,
       IRepository<T_SignOffHeader> _SignOffHeaderRepository,
       IRepository<T_TAG_SHEET_HEADER> _TagSheetHeaderRepository,
       IRepository<T_CHECKSHEET_QUESTION> _CheckSheetQuetionsRepository,
       IRepository<T_TAG_SHEET_ANSWER> _TAG_SHEET_ANSWERRepository,
       IRepository<T_UserControl> _T_UserControlRepository,
       IRepository<T_Ccms_signature> _Ccms_signatureRepository,
       IRepository<T_CompletionsUsers> _CompletionsUserRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._TAGRepository = _TAGRepository;
            this._CheckSheetRepository = _CheckSheetRepository;
            this._CheckSheetQuetionsRepository = _CheckSheetQuetionsRepository;
            this._CheckSheetPerTagRepository = _CheckSheetPerTagRepository;
            this._SignOffHeaderRepository = _SignOffHeaderRepository;
            this._TagSheetHeaderRepository = _TagSheetHeaderRepository;
            this._TAG_SHEET_ANSWERRepository = _TAG_SHEET_ANSWERRepository;
            this._T_UserControlRepository = _T_UserControlRepository;
            this._Ccms_signatureRepository = _Ccms_signatureRepository;
            this._CompletionsUserRepository = _CompletionsUserRepository;
            
            BindTagsAsync();
            //  CheckSheetCompleted = true;
            IsVisibleRejectButton = false;
            RemarkEntry = "";
            RemarkAnswer = new T_TAG_SHEET_ANSWER();
        }

        public async void BindTagsAsync()
        {
            //Color codding Logic
            _tagsList = new List<T_TAG>();
            ItemSourceTagList = new ObservableCollection<dynamic>();
            var data = await _TAGRepository.GetAsync(i => i.ProjectName == Settings.ProjectName);
            var signOffHeaderData = await _SignOffHeaderRepository.GetAsync();
            var DistictTags = data.GroupBy(x => x.name, (key, group) => group.First());
            if (DistictTags != null && DistictTags.Any())
            {

                DistictTags.ToList().ForEach(x =>
                {
                    Completed = true;
                    Started = false;
                    rejected = false;
                    if (!(x.refType == "PIPING JOINT"))
                    {
                        var signOffHeader = signOffHeaderData.Where(y => y.SignOffTag == x.name);
                        if (signOffHeader.Any())
                        {
                            foreach (T_SignOffHeader signOff in signOffHeader)
                            {
                                if (signOff.Title.ToLower() == "client" && signOff.FullName.ToLower() == "na")
                                    continue;
                                else
                                    Completed &= !string.IsNullOrEmpty(signOff.FullName.Trim());
                                Started |= !string.IsNullOrWhiteSpace(signOff.FullName) && signOff.FullName != "";
                                if (signOff.Rejected)
                                    rejected = true;
                            }
                            if (!Started)
                                Started |= GetQuestionStatus(x.name, "");
                        }
                        else
                        {
                            Completed = false;
                        }
                        x.StatusColor = (Completed ? ITR_STATUS_COMPLETE : (Started ? ITR_STATUS_STARTED : statusNoJIData));
                        if (rejected)
                            x.StatusColor = Outstanding;
                        _tagsList.Add(x);
                    }
                    else
                    {
                        x.StatusColor = statusNoJIData;
                        _tagsList.Add(x);
                        //joint data not found yet 
                    }
                    ItemSourceTagList = new ObservableCollection<dynamic>(_tagsList);
                });
            }

        }
        private async void OnTagSelectionChage()
        {
            var signOffHeaderData = await _SignOffHeaderRepository.GetAsync();
            if (SelectedTag == null) return;

            var result = await _CheckSheetPerTagRepository.GetAsync(x => x.TAGNAME == SelectedTag.name && !x.ccsItr);
            //Regex reges = new Regex("^[0-9]+$");
            //result = result.Where(x => reges.IsMatch(x.CHECKSHEETNAME.Substring(x.CHECKSHEETNAME.Length - 1)) == true).ToList();
            if (result != null && result.Any())
            {
                var _CurrentCheckSheet = await _CheckSheetRepository.GetAsync();

                ItemSourceCheckSheets = new ObservableCollection<T_CHECKSHEET_PER_TAG>(result.GroupBy(x => x.CHECKSHEETNAME).Select(g => g.First()).OrderBy(x => x.CHECKSHEETNAME));
                ItemSourceCheckSheets.ForEach(x =>
                {
                    var signOffHeader = signOffHeaderData.Where(y => y.SignOffTag == x.TAGNAME && y.SignOffChecksheet == x.CHECKSHEETNAME);
                    if (signOffHeader.Any() && SelectedTag.refType != "PIPING JOINT")
                    {
                        Completed = true;
                        Started = false;
                        rejected = false;
                        foreach (T_SignOffHeader signOff in signOffHeader)
                        {

                            //Completed &= !string.IsNullOrWhiteSpace(signOff.FullName);
                            //Started |= !string.IsNullOrWhiteSpace(signOff.FullName);
                            //rejected |= signOff.Rejected;

                            if (signOff.Title.ToLower() == "client" && signOff.FullName.ToLower() == "na")
                                continue;
                            else
                                Completed &= !string.IsNullOrEmpty(signOff.FullName.Trim());
                            Started |= !string.IsNullOrWhiteSpace(signOff.FullName) && signOff.FullName != "";
                            if (signOff.Rejected)
                                rejected = true;
                        }
                        // x.StatusColor = "#ff2a2a";
                        if (!Started)
                            Started |= GetQuestionStatus(x.TAGNAME, x.CHECKSHEETNAME);

                        x.StatusColor = (Completed ? ITR_STATUS_COMPLETE : (Started ? ITR_STATUS_STARTED : statusNoJIData));

                        if (rejected)
                            x.StatusColor = Outstanding;
                    }
                    else
                    {
                        Completed = false;
                        x.StatusColor = statusNoJIData;
                    }

                    x.description = x.CHECKSHEETNAME + " " + _CurrentCheckSheet.Where(y => y.name == x.CHECKSHEETNAME).Select(z => z.description).FirstOrDefault();
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
                        itrname = SelectedCheckSheet.CHECKSHEETNAME == null ? "" : SelectedCheckSheet.CHECKSHEETNAME,
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
                else if (param == "CopyITRData")
                {
                    if (SelectedCheckSheet == null)
                    {
                        App.IsBusy = false;
                        return;
                    }

                    if (SignOffList.FirstOrDefault().FullName == "Press to Sign" || SignOffList.FirstOrDefault().FullName == "")
                    {
                        _userDialogs.Alert("Please sign off the Subcontractor signature to proceed with ITR copy", "", "OK");
                        App.IsBusy = false;
                        return;
                    }
                    if (Settings.CompletionUserName != SignOffList.FirstOrDefault().FullName)
                    {
                        _userDialogs.Alert("The ITR you are trying to copy is signed off with another signature.", "", "OK");
                        App.IsBusy = false;
                        return;
                    }
                    if (!CheckSheetCompleted && !IsChecksheetIntiled)
                    {
                        //_userDialogs.Alert("The ITR you are trying to add info to is either a Detailed Itr or has not been completed yet:" + SelectedCheckSheet.CHECKSHEETNAME 
                        //                  + ". The function will continue with any other selected ITRs.", "", "OK");
                        _userDialogs.Alert("The Itr cannot be copied, please make sure all questions have been completed", "", "OK");
                        App.IsBusy = false;
                        return;
                    }
                    OnExitUpdateCheckSheets();
                    var parameter = new NavigationParameters();
                    parameter.Add("SelectedITRData", SelectedCheckSheet);
                    parameter.Add("SelectedITRQueAnsData", ItemSourceQuetions.ToList());
                    await navigationService.NavigateAsync<CopyITRDataViewModel>(parameter);
                }
            }
        }

        public async void SignOffClicked(string param, string SignatureName)
        {
            if (IsCheckSheetAccessible)
            {
                if (SignatureName == "Press to Sign")
                {
                    //if (param.ToLower().Trim() == "contractor")
                    //{
                    if (!App.IsBusy)
                    {
                        App.IsBusy = true;
                        var _credentials = await ReadLoginPopup("SignOff");
                        SaveSignOffHeader(_credentials, param);
                    }
                    //}

                    //if (param.ToLower().Trim() == "client")
                    //{
                    //    var _credentials = await ReadLoginPopup("SignOff");
                    //    SaveSignOffHeader(_credentials, param);
                    //}
                }
            }
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
                    var signatureData = await _Ccms_signatureRepository.QueryAsync("select * from T_Ccms_signature where signatureName = '" + param + "' and ITR = '" + SelectedCheckSheet.CHECKSHEETNAME + "' and ProjectName = '" + Settings.ProjectName + "' ");
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

                    var offlineSignOff = SignOffList.Where(x => x.Title == param).FirstOrDefault();
                    offlineSignOff.FullName = offileUser.FullName;
                    offlineSignOff.SignDate = DateTime.Now;
                    offlineSignOff.IsSynced = true;
                    offlineSignOff.Rejected = false;
                    offlineSignOff.RejectedReason = "";
                    await _SignOffHeaderRepository.UpdateAsync(offlineSignOff);
                    string RemoveRejectSQL = @"UPDATE T_SignOffHeader SET  [rejected] = 0, [RejectedReason]='', [IsSynced] = 1 WHERE [SignOffTag] = '" + offlineSignOff.SignOffTag + "' AND [SignOffChecksheet] = '" + offlineSignOff.SignOffChecksheet + "'";
                    await _SignOffHeaderRepository.QueryAsync(RemoveRejectSQL);
                    SignOffList.Where(x => x.Title == param).ToList().ForEach(x => x.FullName = offileUser.FullName);
                    SignOffList = new ObservableCollection<T_SignOffHeader>(SignOffList);
                    OnTagSelectionChage();
                    BindTagsAsync();
                    return;
                }
            }
            

            if (Result)
            {
                List<T_Ccms_signature> signaturelist = new List<T_Ccms_signature>();
                string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.GetUser(_credentials.UserName, _credentials.Password, Settings.CurrentDB), Settings.CompletionAccessToken);
                var CurrentUser = JsonConvert.DeserializeObject<T_UserControl>(JsonString);
                if (CurrentUser.Company_Category_Code != null && CurrentUser.Function_Code != null && CurrentUser.Section_Code != null)
                {
                    bool ISvalida = false;
                    var signatureData = await _Ccms_signatureRepository.QueryAsync("select * from T_Ccms_signature where signatureName = '" + param + "' and ITR = '" + SelectedCheckSheet.CHECKSHEETNAME + "' and ProjectName = '" + Settings.ProjectName + "' ");
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

                var signOff = SignOffList.Where(x => x.Title == param).FirstOrDefault();
                signOff.FullName = CurrentUser.FullName;
                signOff.SignDate = DateTime.Now;
                signOff.IsSynced = true;
                signOff.Rejected = false;
                signOff.RejectedReason = "";
                await _SignOffHeaderRepository.UpdateAsync(signOff);
                string RemoveRejectSQL = @"UPDATE T_SignOffHeader SET  [rejected] = 0, [RejectedReason]='', [IsSynced] = 1 WHERE [SignOffTag] = '" + signOff.SignOffTag + "' AND [SignOffChecksheet] = '" + signOff.SignOffChecksheet + "'";
                await _SignOffHeaderRepository.QueryAsync(RemoveRejectSQL);
                SignOffList.Where(x => x.Title == param).ToList().ForEach(x => x.FullName = CurrentUser.FullName);
                SignOffList = new ObservableCollection<T_SignOffHeader>(SignOffList);
                OnTagSelectionChage();
                BindTagsAsync();
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
            AbletoReject = false;
            foreach (T_SignOffHeader signOff in SignOffList)
            {
                if (signOff.Title.ToLower() == "client" && signOff.FullName.ToLower() == "na" && signOff.FullName != "Press to Sign")
                    AbletoReject = true;
                else if (!string.IsNullOrEmpty(signOff.FullName.Trim()) && signOff.FullName != "Press to Sign")
                    AbletoReject = true;
            }
            if (AbletoReject)
            {
                var rejectReason = await ReadRejectPopup();
                if (string.IsNullOrEmpty(rejectReason)) rejectReason = "";

                if (!string.IsNullOrEmpty(rejectReason))
                {
                    SignOffList.ToList().ForEach(x => { x.RejectedReason = rejectReason; x.FullName = ""; x.Rejected = true; x.IsSynced = true; });
                    await _SignOffHeaderRepository.UpdateAllAsync(SignOffList);
                    //ItemSourceQuetions.ForEach(x => x.AdditionalFields.ForEach(i => i.FieldValue = ""));
                    //ItemSourceQuetions.ForEach(Que =>
                    //{
                    //    foreach (AdditionalFieldsModel AO in Que.AdditionalFields)
                    //    {
                    //        string strSQL = @"UPDATE T_TAG_SHEET_ANSWER SET  [checkValue] = '', [IsSynced] = 0  WHERE [checksheetname] = '" + Que.CheckSheet + "' AND [itemno] = '" + Que.itemNo + "'";
                    //        Task.Run(async () => await _TAG_SHEET_ANSWERRepository.QueryAsync(strSQL));
                    //    }
                    //});
                    //string updateAnsSQL = @"UPDATE T_TAG_SHEET_ANSWER SET  [checkValue] = '', [IsSynced] = 0  WHERE [checksheetname] = '" + SignOffList.FirstOrDefault().SignOffChecksheet + "' AND [tagName] = '" + SignOffList.FirstOrDefault().SignOffTag + "'";
                    //await _TAG_SHEET_ANSWERRepository.QueryAsync(updateAnsSQL);

                    SignOffList.ToList().ForEach(x => x.FullName = "Press to Sign");
                    SignOffList = new ObservableCollection<T_SignOffHeader>(SignOffList);
                    OnTagSelectionChage();
                    BindTagsAsync();
                }
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

        public static Task<string> ReadRejectPopup()
        {
            var vm = new RejectPopupViewModel();
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

        private async Task<List<CctrImage>> GetCCTRImages(String CCTRid)
        {
            try
            {
                CCTRImageSourceList = new List<CctrImage>();
                string Folder = ("Photo Store\\CompletionsChecksheet_Image\\" + Settings.ProjectID + "\\" + Settings.UserID + "\\" + CCTRid);
                CCTRImagePath = await DependencyService.Get<ISaveFiles>().GenerateImagePath(Folder);

                var CCTRImageFiles = await DependencyService.Get<ISaveFiles>().GetAllImages(CCTRImagePath);
                if (CCTRid == "8755" || CCTRid == "8752" || CCTRid == "9148")
                {
                    string sdf = "got";

                }

                if (CCTRImageFiles.Any())
                {

                    foreach (string img in CCTRImageFiles)

                    {
                        string pathconcat = Device.RuntimePlatform == Device.UWP ? "\\" : "/";
                        byte[] data = await DependencyService.Get<ISaveFiles>().ReadBytes(CCTRImagePath + pathconcat + img);
                        ImageSource retSource = null;
                        retSource = ImageSource.FromStream(() => new MemoryStream(data));
                        CCTRImageSourceList.Add(new CctrImage { ID = CCTRid, CCtrimg = retSource });

                    }

                    return CCTRImageSourceList;
                }
                else
                {
                    return new List<CctrImage>();
                }



            }
            catch (Exception ex)
            {
                return new List<CctrImage>();
            }
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

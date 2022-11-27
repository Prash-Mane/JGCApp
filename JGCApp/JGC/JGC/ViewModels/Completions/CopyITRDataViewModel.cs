using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables.Completions;
using JGC.Models.Completions;
using Newtonsoft.Json;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JGC.ViewModels.Completions
{
   public class CopyITRDataViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_CompletionSystems> _CompletionSystemsRepository;
        private readonly IRepository<T_TAG> _TAGRepository;
        private readonly IRepository<T_CHECKSHEET_PER_TAG> _CheckSheetPerTagRepository;
        private readonly IRepository<T_SignOffHeader> _SignOffHeaderRepository;
        private readonly IRepository<T_TAG_SHEET_ANSWER> _TAG_SHEET_ANSWERRepository;

        public List<T_TAG> SelectedTagsList = new List<T_TAG>();
        public List<string> SelectedITRsList = new List<string>();
        T_CHECKSHEET_PER_TAG SelectedITRData = new T_CHECKSHEET_PER_TAG();
        List<CheckSheetQuetionList> SelectedITRQueAnsData = new List<CheckSheetQuetionList>();

        private ObservableCollection<T_CompletionSystems> _systemList;
        public ObservableCollection<T_CompletionSystems> SystemList
        {
            get { return _systemList; }
            set { _systemList = value; RaisePropertyChanged(); }
        }
        private T_CompletionSystems _selectedSystem;
        public T_CompletionSystems SelectedSystem
        {
            get { return _selectedSystem; }
            set
            {
                if (SetProperty(ref _selectedSystem, value))
                {
                    BtnClicked("GetTagList");
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<T_TAG> _TagList;
        public ObservableCollection<T_TAG> TagList
        {
            get { return _TagList; }
            set { _TagList = value; RaisePropertyChanged(); }
        }
        private T_TAG _selectedTags;
        public T_TAG SelectedTag
        {
            get { return _selectedTags; }
            set
            {
                if (SetProperty(ref _selectedTags, value))
                {
                    //BtnClicked("AddorRemoveTags");
                    //RaisePropertyChanged("SelectedTag");
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<string> _ITRList;
        public ObservableCollection<string> ITRList
        {
            get { return _ITRList; }
            set { _ITRList = value; RaisePropertyChanged(); }
        }
        private string _selectedITR;
        public string SelectedITR
        {
            get { return _selectedITR; }
            set
            {
                if (SetProperty(ref _selectedITR, value))
                {
                    BtnClicked("AddorRemoveITRs"); 
                    OnPropertyChanged();
                }
            }
        }
        private bool isVisibleGetITRbtn;
        public bool IsVisibleGetITRbtn
        {
            get { return isVisibleGetITRbtn; }
            set
            {
                SetProperty(ref isVisibleGetITRbtn, value); OnPropertyChanged();
            }
        }
        private bool isVisibleCopyITRbtn;
        public bool IsVisibleCopyITRbtn
        {
            get { return isVisibleCopyITRbtn; }
            set
            {
                SetProperty(ref isVisibleCopyITRbtn, value); OnPropertyChanged();
            }
        }


        public ICommand ClickCommand
        {
            get
            {
                return new Command<string>(BtnClicked);
            }
        }

        public CopyITRDataViewModel(INavigationService _navigationService, IUserDialogs _userDialogs,
        IHttpHelper _httpHelper,ICheckValidLogin _checkValidLogin,
        IRepository<T_CompletionSystems> _CompletionSystemsRepository,
        IRepository<T_TAG> _TAGRepository,
        IRepository<T_CHECKSHEET_PER_TAG> _CheckSheetPerTagRepository,
        IRepository<T_SignOffHeader> _SignOffHeaderRepository,
        IRepository<T_TAG_SHEET_ANSWER> _TAG_SHEET_ANSWERRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._userDialogs = _userDialogs;
            this._CompletionSystemsRepository = _CompletionSystemsRepository;
            this._TAGRepository = _TAGRepository;
            this._CheckSheetPerTagRepository = _CheckSheetPerTagRepository;
            this._SignOffHeaderRepository = _SignOffHeaderRepository;
            this._TAG_SHEET_ANSWERRepository = _TAG_SHEET_ANSWERRepository;
            GetSystemData();
        }
        public async void BtnClicked(string param)
        {
            try
            {
                if (param == "GetTagList")
                {
                    if (SelectedSystem == null)
                        return;

                    if (SelectedSystem.name == "Filter System")
                    {
                        List<T_TAG> Tags = new List<T_TAG>
                        { 
                            new T_TAG(){ name = "Filter System First"}
                        };
                        List<string> Itrs = new List<string> { "Filter Tag(s) First" };
                        TagList = new ObservableCollection<T_TAG>(Tags);
                        ITRList = new ObservableCollection<string>(Itrs);
                        SelectedTagsList.Clear();
                        SelectedITRsList.Clear();
                        SelectedTag = null;
                        SelectedITR = null;
                        IsVisibleGetITRbtn = IsVisibleCopyITRbtn = false;
                    }
                    else
                    {
                        //
                        string tagsQuery = "SELECT DISTINCT tag.* FROM T_CompletionSystems CS INNER JOIN T_TAG Tag on tag.system == CS.name "
                                         + " WHERE CS.modelName == '" + Settings.ModelName + "' and tag.projectname == '" + Settings.ProjectName + "' and tag.system == '" 
                                         + SelectedSystem.name + "' and tag.name != '" + SelectedITRData.TAGNAME + "'";
                        var _Taglist = await _TAGRepository.QueryAsync(tagsQuery);

                        TagList = new ObservableCollection<T_TAG>(_Taglist.GroupBy(x => x.name, (key, group) => group.First()));
                        if (TagList.Count > 0)
                            IsVisibleGetITRbtn = true;
                        
                        List<string> Itrs = new List<string> { "Filter Tag(s) First" };
                        ITRList = new ObservableCollection<string>(Itrs);
                        IsVisibleCopyITRbtn = false;

                        SelectedTagsList.Clear();
                        SelectedITRsList.Clear();
                        SelectedTag = null;
                        SelectedITR = null;
                    }
                }
                else if (param == "AddTags")
                {
                    if (SelectedTag == null)
                        return;

                    if (!SelectedTagsList.Contains(SelectedTag))
                        SelectedTagsList.Add(SelectedTag);
                }
                else if (param == "RemoveTags")
                {
                    if (SelectedTag == null)
                        return;
                    if (SelectedTagsList.Contains(SelectedTag))
                        SelectedTagsList.Remove(SelectedTag);
                }
                else if (param == "GetITRList")
                {
                    if (SelectedTagsList.Count > 0)
                    {
                        string CheckSheetsQuery = "SELECT * FROM T_CHECKSHEET_PER_TAG WHERE CHECKSHEETNAME = '" + SelectedITRData.CHECKSHEETNAME + "' AND TAGNAME in ('" + String.Join("','", SelectedTagsList.Select(x => x.name)) + "')";
                        var CheckSheets = await _CheckSheetPerTagRepository.QueryAsync(CheckSheetsQuery);
                        List<string> Itrs = new List<string>();

                        string getFirstSignsSQL = @"Select * from T_SignOffHeader"
                                                + " WHERE [SignOffChecksheet] = '" + SelectedITRData.CHECKSHEETNAME + "' AND [SignOffTag] = '" + SelectedITRData.TAGNAME + "'";
                        var SignoffList = await _SignOffHeaderRepository.QueryAsync(getFirstSignsSQL);

                        foreach (T_CHECKSHEET_PER_TAG item in CheckSheets)
                        {
                            bool Completed, Started, rejected,SignComplete;
                            SignComplete = false;
                            var signOffHeaderData = await _SignOffHeaderRepository.GetAsync();
                            // Itrs = CheckSheets.Select(x => x.CHECKSHEETNAME + " ~ " + x.TAGNAME).ToList();
                            var signOffHeader = signOffHeaderData.Where(y => y.SignOffTag == item.TAGNAME && y.SignOffChecksheet == item.CHECKSHEETNAME);
                            if (signOffHeader.Any() && SelectedTag.refType != "PIPING JOINT")
                            {
                                T_SignOffHeader signOff = signOffHeader.FirstOrDefault();
                                if (!string.IsNullOrWhiteSpace(signOff.FullName) && SignoffList.Count > 0)
                                { 
                                  if (signOff.FullName == SignoffList.FirstOrDefault().FullName)
                                            Itrs.Add(item.CHECKSHEETNAME + " ~ " + item.TAGNAME);
                                }
                                else 
                                    Itrs.Add(item.CHECKSHEETNAME + " ~ " + item.TAGNAME);
                            }
                            //if (signOffHeader.Any() && SelectedTag.refType != "PIPING JOINT")
                            //{
                            //    Completed = true;
                            //    Started = false;
                            //    rejected = false;
                            //    foreach (T_SignOffHeader signOff in signOffHeader)
                            //    {

                            //        //Completed &= !string.IsNullOrWhiteSpace(signOff.FullName);
                            //        //Started |= !string.IsNullOrWhiteSpace(signOff.FullName);
                            //        //rejected |= signOff.Rejected;

                            //        if (signOff.Title.ToLower() == "client" && signOff.FullName.ToLower() == "na")
                            //            continue;
                            //        else
                            //            Completed &= !string.IsNullOrEmpty(signOff.FullName.Trim());
                            //        Started |= !string.IsNullOrWhiteSpace(signOff.FullName) && signOff.FullName != "";
                            //        if (signOff.Rejected)
                            //            rejected = true;
                            //    }
                            //    // x.StatusColor = "#ff2a2a";
                            //    if (!Started)
                            //        Started |= GetQuestionStatus(item.TAGNAME, item.CHECKSHEETNAME);

                            //    SignComplete = Completed ? true : false;

                            //    if (rejected)
                            //        SignComplete = false;
                            //}
                            //else
                            //{
                            //    Completed = false;
                            //}

                            //if(!SignComplete)
                            //Itrs.Add(item.CHECKSHEETNAME + " ~ " + item.TAGNAME);

                        }

                        ITRList = new ObservableCollection<string>(Itrs);
                        if (ITRList.Count > 0)
                            IsVisibleCopyITRbtn = true;
                        SelectedITR = null;
                        SelectedITRsList.Clear();
                    }
                    else
                        await _userDialogs.AlertAsync("Please select Tags to get an ITR(s)", "Alert", "OK");

                }
                else if (param == "AddITRs")
                {
                    if (SelectedITR == null)
                        return;

                    if (!SelectedITRsList.Contains(SelectedITR))
                        SelectedITRsList.Add(SelectedITR);
                }
                else if (param == "RemoveITRs")
                {
                    if (SelectedITR == null)
                        return;

                    if (SelectedITRsList.Contains(SelectedITR))
                        SelectedITRsList.Remove(SelectedITR);
                }
                else if (param == "CopyITRData")
                {
                    if (SelectedITRsList.Count > 0)
                    {
                        string getFirstSignsSQL = @"Select * from T_SignOffHeader" 
                                                + " WHERE [SignOffChecksheet] = '" + SelectedITRData.CHECKSHEETNAME + "' AND [SignOffTag] = '" + SelectedITRData.TAGNAME + "'";
                       var SignoffList = await _SignOffHeaderRepository.QueryAsync(getFirstSignsSQL);
                    
                        foreach (string item in SelectedITRsList)
                        {
                            if (SignoffList.Count > 0)
                            {
                                string FullName = SignoffList.Select(x => x.FullName).FirstOrDefault();
                                DateTime SignoffDate = SignoffList.Select(x => x.SignDate).FirstOrDefault();
                                string SignTitle = SignoffList.Select(x => x.Title).FirstOrDefault();
                                string Checksheet = item.Split('~').FirstOrDefault().Trim();
                                string Tag = item.Split('~').LastOrDefault().Trim();

                                string CopySignsSQL = @"UPDATE T_SignOffHeader SET [FullName] = '" + FullName + "', [SignDate] = '" + SignoffDate.Ticks + "', [rejected] = 0, [RejectedReason]='', [IsSynced] = 1 "
                                                    + " WHERE [SignOffChecksheet] = '" + Checksheet + "' AND [SignOffTag] = '" + Tag + "' AND [Title] = '"+ SignTitle + "'";
                                await _SignOffHeaderRepository.QueryAsync(CopySignsSQL);
                                foreach (AnswerOptions AO in SelectedITRQueAnsData.SelectMany(x => x.answerOptions.Where(y=>y.isSelected == "#006633")))
                                {
                                    string CheckValue = AO.isSelected == "#006633" ? AO.Options : "";

                                    var GetData = await _TAG_SHEET_ANSWERRepository.GetAsync(x => x.checkSheetName == Checksheet && x.itemno == AO.itemNo && x.tagName == Tag);
                                    string strSQL = @"UPDATE T_TAG_SHEET_ANSWER SET  [checkValue] = '" + CheckValue + "', [completedBy]= '" + GetData.FirstOrDefault().completedBy + "', [completedOn]= '" + GetData.FirstOrDefault().completedOn.Ticks
                                                  + "', [IsSynced] = 0 WHERE [checkSheetName] = '" + Checksheet + "' AND [itemno] = '" + AO.itemNo + "' AND [tagName] = '" + Tag + "'";
                                    await _TAG_SHEET_ANSWERRepository.QueryAsync(strSQL);
                                }
                                foreach (AdditionalFieldsModel AFM in SelectedITRQueAnsData.SelectMany(x => x.AdditionalFields))
                                {
                                    string Updatecheckvalue = " SELECT TSAns.* FROM T_CHECKSHEET_QUESTION as chQue inner join T_TAG_SHEET_ANSWER as TSAns on TSAns.checkSheetName = chQue.CheckSheet and TSAns.itemno = chQue.itemNo"
                                                    + " WHERE chQue.id = '" + AFM.ID + "' And TSAns.tagName = '" + Tag + "'";
                                    
                                    var updateValue = await _TAG_SHEET_ANSWERRepository.QueryAsync(Updatecheckvalue);
                                    if (updateValue != null)
                                    {
                                        T_TAG_SHEET_ANSWER TagAnsSheet = updateValue.FirstOrDefault();

                                        string strSQL = @"UPDATE T_TAG_SHEET_ANSWER SET  [checkValue] = '" + AFM.FieldValue + "', [completedBy]= '" + TagAnsSheet.completedBy + "', [completedOn]= '" + TagAnsSheet.completedOn.Ticks
                                                      + "', [IsSynced] = 0 WHERE [checkSheetName] = '" + TagAnsSheet.checkSheetName + "' AND [itemno] = '" + TagAnsSheet.itemno + "' AND [tagName] = '" + TagAnsSheet.tagName + "'";
                                        await _TAG_SHEET_ANSWERRepository.QueryAsync(strSQL);
                                    }
                                }
                            }
                        }
                        await _userDialogs.AlertAsync("ITR's data has been copied successfully.", "", "OK");
                    }
                    else
                        await _userDialogs.AlertAsync("Please select ITR(s) copy to other ITR's", "Alert", "OK");
                }
            }
            catch (Exception ex)
            {

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
        private async void GetSystemData()
        {
            var _systemlist = await _CompletionSystemsRepository.QueryAsync("SELECT DISTINCT CS.* FROM T_CompletionSystems CS INNER JOIN T_TAG Tag on tag.system == CS.name " 
                                                                            + " WHERE CS.modelName == '" + Settings.ModelName + "' and tag.projectname == '" + Settings.ProjectName + "'");
            var defaultsystem = new T_CompletionSystems()
            {
                ref1 = "Filter System",
                refType = "SYSTEM",
            };
            _systemlist.Insert(0, defaultsystem);
            SystemList = new ObservableCollection<T_CompletionSystems>(_systemlist.Distinct());
            SelectedSystem = SystemList.FirstOrDefault();
        }
        //private void ViewCell_Tapped(object sender, EventArgs e)
        //{ 
        //    if (lastCell != null)
        //        lastCell.View.BackgroundColor = Color.Transparent;
        
        //    var viewCell = (ViewCell)sender;
        //    if (viewCell.View != null)
        //    {
        //        viewCell.View.BackgroundColor = Color.FromHex("#304D61");
        //        viewCell.View.VerticalOptions = LayoutOptions.Fill;
        //        viewCell.View.HorizontalOptions = LayoutOptions.Fill;

        //        lastCell = viewCell;
        //    }
        //}
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.Any())
            {
                SelectedITRData = parameters.GetValue<T_CHECKSHEET_PER_TAG>("SelectedITRData");
                SelectedITRQueAnsData = parameters.GetValue<List<CheckSheetQuetionList>>("SelectedITRQueAnsData");
            }
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }
}

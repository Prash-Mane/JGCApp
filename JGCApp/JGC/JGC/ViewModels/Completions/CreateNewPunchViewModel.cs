using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Xamarin.Forms.Internals;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.Common.Helpers;
using JGC.DataBase.DataTables.Completions;
using JGC.Views.Completions;
using JGC.Models;
using JGC.Common.Constants;
using JGC.DataBase.DataTables.ModsCore;

namespace JGC.ViewModels.Completions
{
    public class CreateNewPunchViewModel : BaseViewModel
    {

        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_CompletionsPunchList> _CompletionsPunchListRepository;
        private readonly IRepository<T_SignOffHeader> _SignOffHeaderRepository;
        public static CompletionPageHelper _CompletionpageHelper = new CompletionPageHelper();
        string PunchListImagePAth;

        #region Delegate Commands   
        public ICommand ButtonClickedCommand
        {
            get
            {
                return new Command<string>(OnClickedAsync);
            }
        }

        public ICommand TappedGestureCommand
        {
            get
            {
                return new Command<string>(OnTapedAsync);
            }
        }
        #endregion

        #region Properties
        private ImageSource _capturedImage;
        public ImageSource CapturedImage
        {
            get
            {
                return _capturedImage;
            }
            set { SetProperty(ref _capturedImage, value); }
        }
        private bool isVisibleStep1 { get; set; }
        public bool IsVisibleStep1
        {
            get { return isVisibleStep1; }
            set { isVisibleStep1 = value; RaisePropertyChanged(); }
        }
        private bool isVisibleStep2 { get; set; }
        public bool IsVisibleStep2
        {
            get { return isVisibleStep2; }
            set { isVisibleStep2 = value; RaisePropertyChanged(); }
        }

        private bool isVisibleStep3 { get; set; }
        public bool IsVisibleStep3
        {
            get { return isVisibleStep3; }
            set { isVisibleStep3 = value; RaisePropertyChanged(); }
        }
        private bool cat_A_Selected { get; set; }
        public bool Cat_A_Selected
        {
            get { return cat_A_Selected; }
            set { cat_A_Selected = value; RaisePropertyChanged(); }
        }
        private bool cat_B_Selected { get; set; }
        public bool Cat_B_Selected
        {
            get { return cat_B_Selected; }
            set { cat_B_Selected = value; RaisePropertyChanged(); }
        }
        private bool cat_C_Selected { get; set; }
        public bool Cat_C_Selected
        {
            get { return cat_C_Selected; }
            set { cat_C_Selected = value; RaisePropertyChanged(); }
        }
        private List<T_CompletionsPunchList> punchListitem;
        public List<T_CompletionsPunchList> PunchList
        {
            get { return punchListitem; }
            set { punchListitem = value; RaisePropertyChanged(); }
        }
        private string lblDescipline;
        public string LblDescipline
        {
            get { return lblDescipline; }
            set { lblDescipline = value; RaisePropertyChanged(); }
        }
        private string lblDrawing;
        public string LblDrawing
        {
            get { return lblDrawing; }
            set { lblDrawing = value; RaisePropertyChanged(); }
        }
        
        private string lblFWBS;
        public string LblFWBS
        {
            get { return lblFWBS; }
            set { lblFWBS = value; RaisePropertyChanged(); }
        }
        private string lblPTU;
        public string LblPTU
        {
            get { return lblPTU; }
            set { lblPTU = value; RaisePropertyChanged(); }
        }
        
        private string lblTCU;
        public string LblTCU
        {
            get { return lblTCU; }
            set { lblTCU = value; RaisePropertyChanged(); }
        }
        private string lblSystem;
        public string LblSystem
        {
            get { return lblSystem; }
            set { lblSystem = value; RaisePropertyChanged(); }
        }
        private string lblSubSystem;
        public string LblSubSystem
        {
            get { return lblSubSystem; }
            set { lblSubSystem = value; RaisePropertyChanged(); }
        }
        private string lblItr;
        public string LblItr
        {
            get { return lblItr; }
            set { lblItr = value; RaisePropertyChanged(); }
        }
        private string lblWorkbook;
        public string LblWorkbook
        {
            get { return lblWorkbook; }
            set { lblWorkbook = value; RaisePropertyChanged(); }
        }
        private string lblTag;
        public string LblTag
        {
            get { return lblTag; }
            set { lblTag = value; RaisePropertyChanged(); }
        }
        private string lblPCWBS;
        public string LblPCWBS
        {
            get { return lblPCWBS; }
            set { lblPCWBS = value; RaisePropertyChanged(); }
        }
        private List<string> itesmSourceOriginator;
        public List<string> ItemSourceOriginator
        {
            get { return itesmSourceOriginator; }
            set { itesmSourceOriginator = value; RaisePropertyChanged(); }
        }
        private string selectedOriginator;
        public string SelectedOriginator
        {
            get { return selectedOriginator; }
            set { selectedOriginator = value; RaisePropertyChanged(); }
        }
        private string txtDescription;
        public string TxtDescription
        {
            get { return txtDescription; }
            set { txtDescription = value; RaisePropertyChanged(); }
        }
        private string txtComments;
        public string TxtComments
        {
            get { return txtComments; }
            set { txtComments = value; RaisePropertyChanged(); }
        }
        private string nextButtonText;
        public string NextButtonText
        {
            get { return nextButtonText; }
            set { nextButtonText = value; RaisePropertyChanged(); }
        }
        private T_CompletionsPunchList selectedPunchList;
        public T_CompletionsPunchList SelectedPunchList
        {
            get { return selectedPunchList; }
            set { selectedPunchList = value; RaisePropertyChanged(); }
        }
        private bool isCreateNewPunch { get; set; }
        public bool IsCreateNewPunch
        {
            get { return isCreateNewPunch; }
            set { isCreateNewPunch = value; RaisePropertyChanged(); }
        }
        private string lblSignarture1;
        public string LblSignarture1
        {
            get { return lblSignarture1; }
            set { lblSignarture1 = value; RaisePropertyChanged(); }
        }
        private string lblSignarture2;
        public string LblSignarture2
        {
            get { return lblSignarture2; }
            set { lblSignarture2 = value; RaisePropertyChanged(); }
        }
        private string uniqNumberEntry;
        public string UniqNumberEntry
        {
            get { return uniqNumberEntry; }
            set { uniqNumberEntry = value; RaisePropertyChanged(); }
        }
        private string uniqNumberSave;
        public string UniqNumberSave
        {
            get { return uniqNumberSave; }
            set { uniqNumberSave = value; RaisePropertyChanged(); }
        }

        private string signOffComments;
        public string SignOffComments
        {
            get { return signOffComments; }
            set { signOffComments = value; RaisePropertyChanged(); }
        }
        private List<string> respDisc;
        public List<string> RespDisc
        {
            get { return respDisc; }
            set { respDisc = value; RaisePropertyChanged(); }
        }
        
        private string selectedRespDisc;
        public string SelectedRespDisc
        {
            get { return selectedRespDisc; }
            set { selectedRespDisc = value; RaisePropertyChanged(); }
        }

        #endregion

        public CreateNewPunchViewModel(INavigationService _navigationService,
           IUserDialogs _userDialogs,
           IHttpHelper _httpHelper,
           IRepository<T_CompletionsPunchList> _CompletionsPunchListRepository,
           IRepository<T_SignOffHeader> _SignOffHeaderRepository,
           ICheckValidLogin _checkValidLogin) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._CompletionsPunchListRepository = _CompletionsPunchListRepository;
            this._SignOffHeaderRepository = _SignOffHeaderRepository;
            IsVisibleStep1 = true;
            IsVisibleStep2 = IsVisibleStep3 = Cat_A_Selected = Cat_B_Selected = Cat_C_Selected = false;
            GetPunchListAsync();
            // LblDescipline = null;
            ItemSourceOriginator = new List<string>() { "Not Selected", Settings.UserName };
            SelectedOriginator = Settings.UserName;
            NextButtonText = "Next";
        }

        private async void GetPunchListAsync()
        {
            var data = await _CompletionsPunchListRepository.GetAsync();
            PunchList = new List<T_CompletionsPunchList>();
            PunchList.AddRange(data);
            RespDisc = PunchList.Where(i=> !string.IsNullOrEmpty(i.respdisc)).Select(i=>i.respdisc).Distinct().OrderBy(x=>x).ToList();
            SelectedRespDisc = RespDisc.FirstOrDefault();
        }

        private async void OnClickedAsync(string param)
        {
            if (!App.IsBusy)
            {
                App.IsBusy = true;
                if (param == "CancelButton")
                {
                    var result = await _userDialogs.ConfirmAsync("Do you Want to Discard Punch List Changes?", "Discard", "Yes", "NO");
                    if (result)
                        await navigationService.GoBackAsync();
                }
                else if (param == "NextButton")
                {
                    if (IsCreateNewPunch)
                    {
                        if (NextButtonText == "Next")
                        {
                            //if (Cat_A_Selected == false && Cat_B_Selected == false && Cat_C_Selected == false)
                            //{
                            //    _ = _userDialogs.AlertAsync("Please select CAT Priority");
                            //    App.IsBusy = false;
                            //    return;
                            //}
                            //if (LblDescipline != null && LblPid != null && LblArea != null)
                            //{
                            //if (LblSubSystem == null || LblSystem == null)
                            //{
                            //    _ = _userDialogs.AlertAsync("Please select System & Subsystem");
                            //    App.IsBusy = false;
                            //    return;
                            //}
                            if (String.IsNullOrEmpty(LblSystem))
                            {
                                DependencyService.Get<IToastMessage>().ShortAlert("Ensure a system is selected");
                                App.IsBusy = false;
                                return;
                            }
                            if (String.IsNullOrEmpty(LblDrawing) || String.IsNullOrEmpty(LblFWBS) || String.IsNullOrEmpty(LblPTU) || String.IsNullOrEmpty(LblSystem) || String.IsNullOrEmpty(LblSubSystem) ||
                                String.IsNullOrEmpty(LblItr) || String.IsNullOrEmpty(LblWorkbook) || String.IsNullOrEmpty(LblTag) || String.IsNullOrEmpty(LblPCWBS) || String.IsNullOrEmpty(LblTCU))
                                {

                                    var result = await _userDialogs.ConfirmAsync("Not all sections have filled out. Continue anyway?", "Continue", "Yes", "NO");
                                    if (!result)
                                    {
                                        App.IsBusy = false;
                                        return;
                                    }
                                }
                                IsVisibleStep1 = false;
                                IsVisibleStep2 = true;
                                NextButtonText = "Save";
                            //}
                            //else
                             //   _ = _userDialogs.AlertAsync("Please select Discipline, PID And Area");

                        }
                        else if (NextButtonText == "Save") SavePunchList(false);
                    }
                    else
                    {
                        if (IsVisibleStep2)
                        {
                            IsVisibleStep3 = true;
                            IsVisibleStep2 = false;
                            NextButtonText = "Save";
                        }
                        else
                        {
                            if (NextButtonText == "Next")
                            {
                                IsVisibleStep1 = false;
                                IsVisibleStep2 = true;
                            }
                            else if (NextButtonText == "Save") SavePunchList(true);
                        }
                    }                   
                }

                else if (param == "PrviousButton")
                {
                    var result = await _userDialogs.ConfirmAsync("Do you Want to Discard Punch List Changes?", "Discard", "Yes", "NO");
                    if (result)
                        if (IsVisibleStep1) await navigationService.GoBackAsync();
                    if (IsCreateNewPunch)
                    {
                        IsVisibleStep1 = true;
                        IsVisibleStep2 = false;
                        NextButtonText = "Next";
                    }
                    else
                    {
                        if (IsVisibleStep3)
                        {
                            IsVisibleStep3 = false;
                            IsVisibleStep2 = true;
                            NextButtonText = "Next";
                        }
                        else
                        {
                            IsVisibleStep1 = true;
                            IsVisibleStep2 = false;
                        }
                    }
                }
                else if (param == "CatA")
                {
                    Cat_A_Selected = true;
                    Cat_B_Selected = Cat_C_Selected = false;
                }
                else if (param == "CatB")
                {
                    Cat_B_Selected = true;
                    Cat_A_Selected = Cat_C_Selected = false;
                }
                else if (param == "CatC")
                {
                    Cat_C_Selected = true;
                    Cat_A_Selected = Cat_B_Selected = false;
                }
                App.IsBusy = false;
            }
        }

        private async void SavePunchList(bool IsUpdate)
        {
            if (IsUpdate)
            {
                try
                {
                    if (SelectedPunchList != null)
                    {
                        SelectedPunchList.priority = Cat_A_Selected ? "A" : Cat_B_Selected ? "B" : Cat_C_Selected ? "C" : "";
                        SelectedPunchList.respdisc = LblDescipline == null ? "" : LblDescipline;
                       // SelectedPunchList.pandid = LblPid == null ? "" : LblPid;
                       // SelectedPunchList.area = LblArea == null ? "" : LblArea;
                        SelectedPunchList.systemno = LblSystem == null ? "" : LblSystem;
                        SelectedPunchList.subsystem = LblSubSystem == null ? "" : LblSubSystem;
                        SelectedPunchList.itrname = LblItr == null ? "" : LblItr;
                        SelectedPunchList.workpack = LblWorkbook == null ? "" : LblWorkbook;
                        SelectedPunchList.tagno = LblTag == null ? "" : LblTag;
                        //SelectedPunchList.location = LblLocation == null ? "" : LblLocation;
                        SelectedPunchList.comments = TxtComments == null ? "" : TxtComments;
                        SelectedPunchList.description = TxtDescription == null ? "" : TxtDescription;
                        SelectedPunchList.imageLocalLocation = PunchListImagePAth == null ? "" : PunchListImagePAth;
                        SelectedPunchList.COLUMN_PUNCH_signOffComment1 = SignOffComments == null || SignOffComments == null ? SelectedPunchList.COLUMN_PUNCH_signOffComment1 : SignOffComments;

                        SelectedPunchList.synced = false;
                        await _CompletionsPunchListRepository.UpdateAsync(SelectedPunchList);
                        await Application.Current.MainPage.DisplayAlert("Update", "Punch List Updated..", "OK");
                        await navigationService.GoBackAsync();
                    }
                    else await Application.Current.MainPage.DisplayAlert("Error", "Punch List Updated Faild", "OK");
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Punch List Updated Faild", "OK");
                }
            }
            else
            {
                //Create New 
                try
                {
                    //check UniqNo is exist
                    string _uniqueno = LblSystem == null ? "" : LblSystem + "."; _uniqueno += LblSubSystem == null ? "" : LblSubSystem + "." + UniqNumberEntry;
                    string systemnumber = LblSystem == null ? "" : LblSystem + "."; systemnumber += LblSubSystem == null ? "" : LblSubSystem;
                    var checkUniqExist = PunchList.Where(x => x.uniqueno.Contains(_uniqueno));
                    if (checkUniqExist.Any())
                    {
                        int _u = 0;
                        PunchList.Where(i => i.uniqueno.Contains(systemnumber)).ForEach(x =>
                        {
                            var unq = x.uniqueno.Split('.');
                            _u = _u < Convert.ToInt32(unq[2]) ? Convert.ToInt32(unq[2]) : _u;
                        });
                        UniqNumberEntry = "00" + (_u + 1).ToString();
                    }

                    T_CompletionsPunchList completionPunchList = new T_CompletionsPunchList();
                    completionPunchList.originator = SelectedOriginator == null ? "" : SelectedOriginator;
                    completionPunchList.ordate = DateTime.Now;
                    //completionPunchList.location = LblLocation == null ? "" : LblLocation;
                    completionPunchList.systemno = LblSystem == null ? "" : LblSystem;
                    completionPunchList.milestone = "";
                    completionPunchList.tagno = LblTag == null ? "" : LblTag;
                    //completionPunchList.pandid = LblPid == null ? "" : LblPid;
                    completionPunchList.description = TxtDescription == null ? "" : TxtDescription;
                    completionPunchList.priority = Cat_A_Selected ? "A" : Cat_B_Selected ? "B" : Cat_C_Selected ? "C" : "";
                    completionPunchList.respdisc = LblDescipline == null ? "" : LblDescipline;
                    completionPunchList.rfqreq = "";
                    completionPunchList.status = "PENDING";
                    completionPunchList.originatordisc = "";
                    completionPunchList.msorder = 0;
                    completionPunchList.originatoruserid = 0;
                    completionPunchList.project = Settings.ProjectName;
                    completionPunchList.itrname = LblItr == null ? "" : LblItr;
                    completionPunchList.comments = TxtComments == null ? "" : TxtComments;
                    completionPunchList.synced = false;
                    completionPunchList.uniqueno = LblSystem == null ? "" : LblSystem + ".";
                    completionPunchList.uniqueno += LblSubSystem == null ? "" : LblSubSystem + "." + UniqNumberEntry;
                    completionPunchList.originator = Settings.CompletionUserName;
                    completionPunchList.originatoruserid = Settings.CompletionUserID;
                    completionPunchList.imageLocalLocation = PunchListImagePAth == null ? "" : PunchListImagePAth;
                    completionPunchList.itrItemNo = "";
                    //completionPunchList.area = LblArea == null ? "" : LblArea;
                    completionPunchList.subsystem = LblSubSystem == null ? "" : LblSubSystem;
                    completionPunchList.workpack = LblWorkbook == null ? "" : LblWorkbook;
                    completionPunchList.jobpack = "";
                    completionPunchList.COLUMN_PUNCH_signOffby1 = LblSignarture1 == null ? "" : LblSignarture1;
                    completionPunchList.COLUMN_PUNCH_signOffComment1 = SignOffComments;
                    completionPunchList.COLUMN_PUNCH_signOffID1 = 0;
                    completionPunchList.COLUMN_PUNCH_signOffby2 = LblSignarture2 == null ? "" : LblSignarture2;
                    completionPunchList.COLUMN_PUNCH_signOffComment2 = "";
                    completionPunchList.COLUMN_PUNCH_signOffID2 = 0;
                    await _CompletionsPunchListRepository.InsertAsync(completionPunchList);
                    await Application.Current.MainPage.DisplayAlert("Save", "Punch List is Saved.", "OK");
                    await navigationService.GoBackAsync();
                }
                catch (Exception Ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Punch List Not Saved.", "OK");
                }

            }
        }

        [Obsolete]
        private async void OnTapedAsync(string param)
        {
            switch (param)
            {
                case "SelectDescipline":
                    if (PunchList == null) return;
                    var Descipline = PunchList.Select(x => x.respdisc).Distinct().ToList();
                    var input1 = await ReadStringInPopup(Descipline);
                    if (!string.IsNullOrWhiteSpace(input1) && input1 != "clear") LblDescipline = input1;
                    else LblDescipline = null;
                    break;

                case "SelectDrawing":
                    //if (PunchList == null) return;
                    //var pid = PunchList.Select(x => x.pandid).Distinct().ToList();
                    //var input2 = await ReadStringInPopup(pid);
                    //if (!string.IsNullOrWhiteSpace(input2) && input2 != "clear") LblPid = input2;
                    //else LblPid = null;
                    break;
                case "SelectPCWBS":
                    if (PunchList == null) return;
                    var pcwbs = PunchList.Select(x => x.PCWBS).Distinct().ToList();
                    var input11 = await ReadStringInPopup(pcwbs);
                    if (!string.IsNullOrWhiteSpace(input11) && input11 != "clear") LblPCWBS = input11;
                    else LblPCWBS = null;
                    break;
                case "SelectFWBS":
                    if (PunchList == null) return;
                    var area = PunchList.Select(x => x.FWBS).Distinct().ToList();
                    var input3 = await ReadStringInPopup(area);
                    if (!string.IsNullOrWhiteSpace(input3) && input3 != "clear") LblFWBS = input3;
                    else LblFWBS = null;
                    break;

                case "SelectLblPTU":
                    if (PunchList == null) return;
                    var ptu = PunchList.Select(x => x.PTU).Distinct().ToList();
                    var input12 = await ReadStringInPopup(ptu);
                    if (!string.IsNullOrWhiteSpace(input12) && input12 != "clear") LblPTU = input12;
                    else LblPTU = null;
                    break;

                case "SelectSystem":
                    if (PunchList == null) return;
                    if (!IsCreateNewPunch) return;
                    var system = PunchList.Select(x => x.systemno).Distinct().ToList();
                    var input4 = await ReadStringInPopup(system);
                    if (!string.IsNullOrWhiteSpace(input4) && input4 != "clear") LblSystem = input4;
                    else LblSystem = null;
                    break;

                case "SelectSubSystem":
                    if (PunchList == null) return;
                    if (!IsCreateNewPunch) return;
                    var subSystem = PunchList.Select(x => x.subsystem).Distinct().ToList();
                    var input5 = await ReadStringInPopup(subSystem);
                    if (!string.IsNullOrWhiteSpace(input5) && input5 != "clear") LblSubSystem = input5;
                    else LblSubSystem = null;
                    break;

                case "SelectITR":
                    if (PunchList == null) return;
                    var ITR = PunchList.Select(x => x.itrname).Distinct().ToList();
                    var input6 = await ReadStringInPopup(ITR);
                    if (!string.IsNullOrWhiteSpace(input6) && input6 != "clear") LblItr = input6;
                    else LblItr = null;
                    break;

                case "SelectWorkPack":
                    if (PunchList == null) return;
                    var workPAck = PunchList.Select(x => x.workpack).Distinct().ToList();
                    var input7 = await ReadStringInPopup(workPAck);
                    if (!string.IsNullOrWhiteSpace(input7) && input7 != "clear") LblWorkbook = input7;
                    else LblWorkbook = null;
                    break;

                case "SelectTag":
                    if (PunchList == null) return;
                    var TAg = PunchList.Select(x => x.tagno).Distinct().ToList();
                    var input8 = await ReadStringInPopup(TAg);
                    if (!string.IsNullOrWhiteSpace(input8) && input8 != "clear") LblTag = input8;
                    else LblTag = null;
                    break;
                case "Signature1Tapped":
                    var _credentials1 = await ReadLoginPopup();
                    LblSignarture1 = await GetSignOffName(_credentials1);
                    SelectedPunchList.COLUMN_PUNCH_signOffby1 = LblSignarture1;

                    break;
                case "Signature2Tapped":
                    var _credentials2 = await ReadLoginPopup();
                    LblSignarture2 = await GetSignOffName(_credentials2);
                    SelectedPunchList.COLUMN_PUNCH_signOffby2 = LblSignarture2;
                    break;
            }
        }

        [Obsolete]
        public static Task<string> ReadStringInPopup(List<string> Source)
        {
            var vm = new FilterPopupViewModel(Source);
            //vm.FilterList = Source;
            var tcs = new TaskCompletionSource<string>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var page = new FilterPopup(vm, Source);
                await PopupNavigation.PushAsync(page);
                var value = await vm.GetValue();
                await PopupNavigation.PopAsync(true);
                tcs.SetResult(value);
            });
            return tcs.Task;
        }

        [Obsolete]
        public static Task<LoginModel> ReadLoginPopup()
        {
            var vm = new SignOffPopupViewModel();
            vm.LoginButtonText = "Login";
            vm.LoginHeaderText = "Login to sign off";
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

        private void LoadPunchListData()
        {
            LblDescipline = SelectedPunchList.respdisc == "" ? null : SelectedPunchList.respdisc;
            //LblPid = SelectedPunchList.pandid == "" ? null : SelectedPunchList.pandid;
            //LblArea = SelectedPunchList.area == "" ? null : SelectedPunchList.area;
            LblSystem = SelectedPunchList.systemno == "" ? null : SelectedPunchList.systemno;
            LblSubSystem = SelectedPunchList.subsystem == "" ? null : SelectedPunchList.subsystem;
            LblItr = SelectedPunchList.itrItemNo == "" ? null : SelectedPunchList.itrItemNo;
            LblWorkbook = SelectedPunchList.workpack == "" ? null : SelectedPunchList.workpack;
            LblTag = SelectedPunchList.tagno == "" ? null : SelectedPunchList.tagno;
            //LblLocation = SelectedPunchList.location == "" ? null : SelectedPunchList.location;
            LblSignarture1 = SelectedPunchList.COLUMN_PUNCH_signOffby1;
            LblSignarture2 = SelectedPunchList.COLUMN_PUNCH_signOffby2;
            UniqNumberEntry = SelectedPunchList.uniqueno.Split('.').Last();
           

            switch (SelectedPunchList.priority)
            {
                case "A":
                    Cat_A_Selected = true;
                    Cat_B_Selected = Cat_C_Selected = false;
                    break;
                case "B":
                    Cat_B_Selected = true;
                    Cat_A_Selected = Cat_C_Selected = false;
                    break;
                case "C":
                    Cat_C_Selected = true;
                    Cat_A_Selected = Cat_B_Selected = false;
                    break;
            }
            if (string.IsNullOrWhiteSpace(SelectedPunchList.originator))
            {
                ItemSourceOriginator.Add(SelectedPunchList.originator);
                SelectedOriginator = SelectedPunchList.originator;
            }
            TxtComments = SelectedPunchList.comments;
            TxtDescription = SelectedPunchList.description;
            PunchListImagePAth = SelectedPunchList.imageLocalLocation;
            SignOffComments = SelectedPunchList.COLUMN_PUNCH_signOffComment1;
            LoadImage(SelectedPunchList.imageLocalLocation);
        }
        private async void LoadImage(string imageLocalLocation)
        {
            if (!string.IsNullOrWhiteSpace(imageLocalLocation))
            {
                var imageSrc = await DependencyService.Get<ISaveFiles>().GetImage(imageLocalLocation);
                CapturedImage = imageSrc;

            }
        }

        private async Task<string> GetSignOffName(LoginModel _credentials)
        {
            string FullNAme = "";
            _CompletionpageHelper.CompletionTokenTimeStamp = DateTime.Now.ToString(AppConstant.DateSaveFormat);
            _CompletionpageHelper.CompletionToken = Settings.CompletionAccessToken = ModsTools.CompletionsCreateToken(_credentials.UserName, _credentials.Password, _CompletionpageHelper.CompletionTokenTimeStamp);
            _CompletionpageHelper.CompletionTokenExpiry = DateTime.Now.AddHours(2);
            _CompletionpageHelper.CompletionUnitID = Settings.UnitID;
            var Result = ModsTools.CompletionValidateToken(_CompletionpageHelper.CompletionToken, _CompletionpageHelper.CompletionTokenTimeStamp);
            // var result =  _checkValidLogin.GetValidToken(_credentials);
            if (Result)
            {
                string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.GetUser(_credentials.UserName, _credentials.Password, Settings.CurrentDB), Settings.CompletionAccessToken);
                var CurrentUser = JsonConvert.DeserializeObject<T_UserControl>(JsonString);
                FullNAme = CurrentUser.FullName;
            }
            else
            {
                _userDialogs.Alert("Failed to login to this account.", "Login Error", "Ok");
            }
            return FullNAme;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            App.IsBusy = false;
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.Count > 1 && parameters.ContainsKey("SelectedPunchList"))
            {
                SelectedPunchList = parameters.GetValue<T_CompletionsPunchList>("SelectedPunchList");
                LoadPunchListData();
                IsCreateNewPunch = false;
            }
            else if(parameters.Count > 1 && parameters.ContainsKey("SelectedPunchListForCreate"))
            {
                SelectedPunchList = parameters.GetValue<T_CompletionsPunchList>("SelectedPunchListForCreate");
                LoadPunchListData();
                IsCreateNewPunch = true;
            }
            else
            {
                UniqNumberEntry = "001";
                IsCreateNewPunch = true;
            }
        }

        public async void CaptureImageSave(byte[] imageAsByte)
        {
            generatepath();
            if (imageAsByte != null)
            {
                string path = await DependencyService.Get<ISaveFiles>().SavePictureToDisk(PunchListImagePAth, DateTime.Now.ToString(AppConstant.CameraDateFormat), imageAsByte.ToArray());
                if (path != null)
                {
                    PunchListImagePAth = path;
                    await _userDialogs.AlertAsync("Successfully saved..!", null, "Ok");
                }
            }
            else
                await _userDialogs.AlertAsync("Please select camera and take a picture to save", null, "OK");
        }

        private async void generatepath()
        {
            string Folder = ("Photo Store" + "\\" + "PunchList" + "\\");
            PunchListImagePAth = await DependencyService.Get<ISaveFiles>().GenerateImagePath(Folder);
        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }
}

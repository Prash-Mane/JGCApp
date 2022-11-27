using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.Common.Services;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.DataBase.DataTables.Completions;
using JGC.DataBase.DataTables.ModsCore;
using JGC.Models;
using JGC.Models.Completions;
using JGC.Views.Completions;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace JGC.ViewModels.Completions
{
    public class NewPunchViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_CompletionsPunchList> _CompletionsPunchListRepository;
        private readonly IRepository<T_PunchComponent> _PunchComponentRepository;
        private readonly IRepository<T_PunchSystem> _PunchSystemRepository;
        private readonly IRepository<T_PunchPCWBS> _PunchPCWBSRepository;
        private readonly IRepository<T_PunchFWBS> _PunchFWBSRepository;
        private readonly IRepository<T_SectionCode> _SectionCodeRepository;
        private readonly IRepository<T_CompanyCategoryCode> _CompanyCategoryCodeRepository;
        private readonly IRepository<T_CompletionSystems> _CompletionSystemsRepository;
        private readonly IRepository<T_CHECKSHEET_PER_TAG> _CheckSheetPerTagRepository;
        private readonly IRepository<T_CompletionsUsers> _CompletionsUserRepository;


        public static CompletionPageHelper _CompletionpageHelper = new CompletionPageHelper();
        string PunchListImagePAth;
        List<T_PunchComponent> PunchComponent = new List<T_PunchComponent>();
        List<T_PunchSystem> PunchSystem = new List<T_PunchSystem>();
        //List<T_CompletionSystems> PunchSystem = new List<T_CompletionSystems>();
        List<T_PunchPCWBS> PunchPCWBS = new List<T_PunchPCWBS>();
        List<T_PunchFWBS> PunchFWBS = new List<T_PunchFWBS>();
        List<string> SectionCodeList = new List<string>();
        List<string> CategoryCodeList = new List<string>();
        List<string> PunchTypeList = new List<string>();
        List<string> IssueOwnerList = new List<string>();
        bool IsItrNo;
        #region properties
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
        private string nextButtonText;
        public string NextButtonText
        {
            get { return nextButtonText; }
            set { nextButtonText = value; RaisePropertyChanged(); }
        }
        private bool isCreateNewPunch { get; set; }
        public bool IsCreateNewPunch
        {
            get { return isCreateNewPunch; }
            set { isCreateNewPunch = value; RaisePropertyChanged(); }
        }
        private string lblSystem;
        public string LblSystem
        {
            get { return lblSystem; }
            set { lblSystem = value; RaisePropertyChanged(); }
        }
        private string lblPCWBS;
        public string LblPCWBS
        {
            get { return lblPCWBS; }
            set { lblPCWBS = value; RaisePropertyChanged(); }
        }
        private string lblFWBS;
        public string LblFWBS
        {
            get { return lblFWBS; }
            set { lblFWBS = value; RaisePropertyChanged(); }
        }
        private string lblComponentCategory;
        public string LblComponentCategory
        {
            get { return lblComponentCategory; }
            set { lblComponentCategory = value; RaisePropertyChanged(); }
        }
        private string lblComponent;
        public string LblComponent
        {
            get { return lblComponent; }
            set { lblComponent = value; RaisePropertyChanged(); }
        }
        private string lblAction;
        public string LblAction
        {
            get { return lblAction; }
            set { lblAction = value; RaisePropertyChanged(); }
        }
        private string lblPunchCategory;
        public string LblPunchCategory
        {
            get { return lblPunchCategory; }
            set { lblPunchCategory = value; RaisePropertyChanged(); }
        }
        private string punchDescription;
        public string PunchDescription
        {
            get { return punchDescription; }
            set { punchDescription = value; RaisePropertyChanged(); }
        }
        private string punchComment;
        public string PunchComment
        {
            get { return punchComment; }
            set { punchComment = value; RaisePropertyChanged(); }
        }
        private string lblResponsibleSection;
        public string LblResponsibleSection
        {
            get { return lblResponsibleSection; }
            set { lblResponsibleSection = value; RaisePropertyChanged(); }
        }
        private string lblResponsiblePosition;
        public string LblResponsiblePosition
        {
            get { return lblResponsiblePosition; }
            set { lblResponsiblePosition = value; RaisePropertyChanged(); }
        }
        private string lblTagNo;
        public string LblTagNo
        {
            get { return lblTagNo; }
            set { lblTagNo = value; RaisePropertyChanged(); }
        }
        private string lblLocation;
        public string LblLocation
        {
            get { return lblLocation; }
            set { lblLocation = value; RaisePropertyChanged(); }
        }
        private string lblITRNo;
        public string LblITRNo
        {
            get { return lblITRNo; }
            set { lblITRNo = value; RaisePropertyChanged(); }
        }
        private string lblPunchType;
        public string LblPunchType
        {
            get { return lblPunchType; }
            set { lblPunchType = value; RaisePropertyChanged(); }
        }
        private string lblIssuerOwner;
        public string LblIssuerOwner
        {
            get { return lblIssuerOwner; }
            set { lblIssuerOwner = value; RaisePropertyChanged(); }
        }
        private string signOffComments;
        public string SignOffComments
        {
            get { return signOffComments; }
            set { signOffComments = value; RaisePropertyChanged(); }
        }
        private int subContractorID;
        public int SubContractorID
        {
            get { return subContractorID; }
            set { subContractorID = value; RaisePropertyChanged(); }
        }
        private string lblSignartureSubContractor;
        public string LblSignartureSubContractor
        {
            get { return lblSignartureSubContractor; }
            set { lblSignartureSubContractor = value; RaisePropertyChanged(); }
        }
        private DateTime subContractorSignOn;
        public DateTime SubContractorSignOn
        {
            get { return subContractorSignOn; }
            set { subContractorSignOn = value; RaisePropertyChanged(); }
        }
        private int contractorID;
        public int ContractorID
        {
            get { return contractorID; }
            set { contractorID = value; RaisePropertyChanged(); }
        }
        private string lblSignartureContractor;
        public string LblSignartureContractor
        {
            get { return lblSignartureContractor; }
            set { lblSignartureContractor = value; RaisePropertyChanged(); }
        }
        private DateTime contractorSignOn;
        public DateTime ContractorSignOn
        {
            get { return contractorSignOn; }
            set { contractorSignOn = value; RaisePropertyChanged(); }
        }
        private int clientID;
        public int ClientID
        {
            get { return clientID; }
            set { clientID = value; RaisePropertyChanged(); }
        }
        private string lblSignartureClient;
        public string LblSignartureClient
        {
            get { return lblSignartureClient; }
            set { lblSignartureClient = value; RaisePropertyChanged(); }
        }
        private DateTime clientSignOn;
        public DateTime ClientSignOn
        {
            get { return clientSignOn; }
            set { clientSignOn = value; RaisePropertyChanged(); }
        }

        private T_CompletionsPunchList selectedPunchList;
        public T_CompletionsPunchList SelectedPunchList
        {
            get { return selectedPunchList; }
            set { selectedPunchList = value; RaisePropertyChanged(); }
        }
        private List<T_CompletionsPunchList> punchListitem;
        public List<T_CompletionsPunchList> PunchList
        {
            get { return punchListitem; }
            set { punchListitem = value; RaisePropertyChanged(); }
        }
        private ImageSource _capturedImage;
        public ImageSource CapturedImage
        {
            get
            {
                return _capturedImage;
            }
            set { SetProperty(ref _capturedImage, value); }
        }
        private string _status;
        public string _Status
        {
            get { return _status; }
            set { _status = value; RaisePropertyChanged(); }
        }
        #endregion
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
        public NewPunchViewModel(INavigationService _navigationService,
           IUserDialogs _userDialogs,
           IHttpHelper _httpHelper,
           ICheckValidLogin _checkValidLogin,
           IRepository<T_CompletionsPunchList> _CompletionsPunchListRepository,
           IRepository<T_PunchComponent> _PunchComponentRepository,
           IRepository<T_PunchSystem> _PunchSystemRepository,
           IRepository<T_PunchPCWBS> _PunchPCWBSRepository,
           IRepository<T_PunchFWBS> _PunchFWBSRepository,
           IRepository<T_SectionCode> _SectionCodeRepository,
           IRepository<T_CompanyCategoryCode> _CompanyCategoryCodeRepository,
           IRepository<T_CompletionSystems> _CompletionSystemsRepository,
           IRepository<T_CHECKSHEET_PER_TAG> _CheckSheetPerTagRepository,

           IRepository<T_CompletionsUsers> _CompletionsUserRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._CompletionsPunchListRepository = _CompletionsPunchListRepository;
            this._PunchComponentRepository = _PunchComponentRepository;
            this._PunchSystemRepository = _PunchSystemRepository;
            this._PunchPCWBSRepository = _PunchPCWBSRepository;
            this._PunchFWBSRepository = _PunchFWBSRepository;
            this._SectionCodeRepository = _SectionCodeRepository;
            this._CompanyCategoryCodeRepository = _CompanyCategoryCodeRepository;
            this._CompletionSystemsRepository = _CompletionSystemsRepository;
            this._CheckSheetPerTagRepository = _CheckSheetPerTagRepository;
            this._CompletionsUserRepository = _CompletionsUserRepository;

            IsVisibleStep1 = true;
            NextButtonText = "Next";
            GetPunchListAsync();
        }
        private async void GetPunchListAsync()
        {

            CategoryCodeList = new List<string>() { "03 - Management", "08 - Engineer", "09 - Inspector(Civil & Structure)", "10 - Inspector(Building)", "11 - Inspector(Equipment)",
                                                    "12 - Inspector(Piping)", "13 - Inspector(Instrumental)", "14 - Inspector(Electrical)", "15 - Inspector(Paint & Insulation)" };
            var data = await _CompletionsPunchListRepository.GetAsync();
            PunchList = new List<T_CompletionsPunchList>();
            PunchList.AddRange(data);

            var _PunchComponent = await _PunchComponentRepository.GetAsync(i => i.Jobcode == Settings.JobCode && i.ModelName == Settings.ModelName);
            if (_PunchComponent != null)
                PunchComponent = _PunchComponent.ToList();
            var _PunchSystem = await _PunchSystemRepository.GetAsync(i => i.Jobcode == Settings.JobCode && i.ModelName == Settings.ModelName);
            if (_PunchSystem != null)
                PunchSystem = _PunchSystem.ToList();
            //var _PunchSystem = await _CompletionSystemsRepository.GetAsync(i => i.modelName == Settings.ModelName);
            //if (_PunchSystem != null)
            //    PunchSystem = _PunchSystem.ToList();
            var _PunchPCWBS = await _PunchPCWBSRepository.GetAsync(i => i.Jobcode == Settings.JobCode && i.ModelName == Settings.ModelName);
            if (_PunchPCWBS != null)
                PunchPCWBS = _PunchPCWBS.ToList();
            var _PunchFWBS = await _PunchFWBSRepository.GetAsync(i => i.Jobcode == Settings.JobCode && i.ModelName == Settings.ModelName);
            if (_PunchFWBS != null)
            {
                // PunchFWBS = _PunchFWBS.ToList();
                var fwbs = _PunchFWBS.Where(x => x.fwbs != "").Select(x => x.fwbs).Distinct().ToList();
                foreach (string item in fwbs)
                {
                    string fwbsitem = item.Substring(0, 1) + "000";
                    long number1 = 0;
                    bool canConvert = long.TryParse(fwbsitem, out number1);
                    if (canConvert)
                        if (!PunchFWBS.Select(x => x.fwbs).Contains(fwbsitem) && Convert.ToInt32(fwbsitem) % 1000 == 0)
                            PunchFWBS.Add(new T_PunchFWBS { fwbs = fwbsitem, Jobcode = Settings.JobCode, ModelName = Settings.ModelName });
                }
            }

            var responsibleSection = PunchComponent.Where(x => !String.IsNullOrEmpty(x.section_code)).Select(x => x.section_code).Distinct().ToList();
            var SectionCodes = await _SectionCodeRepository.GetAsync(x => x.ModelName == Settings.ModelName);
            var _SectionCodes = SectionCodes.Where(x => responsibleSection.Any(y => x.SectionCode.ToUpperInvariant().Contains(y.ToUpperInvariant()))).ToList();
            foreach (T_SectionCode item in _SectionCodes)
                SectionCodeList.Add(item.SectionCode + " - " + item.Description);

            //var responsiblePosition =  PunchComponent.Where(x => !String.IsNullOrEmpty(x.category_code)).Select(x => x.category_code).Distinct().ToList();
            //var CompanyCategoryCodes = await _CompanyCategoryCodeRepository.GetAsync(x => x.ModelName == Settings.ModelName);
            //var _CompanyCategoryCodes = CompanyCategoryCodes.Where(x => responsiblePosition.Any(y => x.CompanyCategoryCode.ToUpperInvariant().Contains(y.ToUpperInvariant()))).ToList();
            //foreach (T_CompanyCategoryCode item in _CompanyCategoryCodes)
            //    CategoryCodeList.Add(item.CompanyCategoryCode + " - " + item.Description);

            PunchTypeList = new List<string>() { "Daily System Walkdown", "Official System Walkdown", "Non Conformance Report (NCR)", "KIZUKI", "Material Exception Report (MER)",
                          "Site Instruction (SI)", "Vendor Carry Over", "Module Yard Carry Over", "Itr Carry Over", "Piping Testpackage Carry Over", "Field Engineer", "Construction Carry Over", };

            var userList = await _CompletionsUserRepository.GetAsync();
            IssueOwnerList = userList.Where(x => x.Company_Category_Code == "C" && !String.IsNullOrEmpty(x.FullName)).Select(x => x.FullName).OrderBy(x => x).Distinct().ToList();
        }
        private async void OnClickedAsync(string param)
        {
            if (!App.IsBusy)
            {
                App.IsBusy = true;
                if (param == "CancelButton")
                {
                    var result = await _userDialogs.ConfirmAsync("Do you Want to Discard Punch List Changes?", "Discard", "Yes", "No");
                    if (result)
                        await navigationService.GoBackAsync();
                }
                else if (param == "NextButton")
                {
                    if (String.IsNullOrEmpty(LblSystem) || String.IsNullOrEmpty(LblPCWBS) || String.IsNullOrEmpty(LblFWBS) || String.IsNullOrEmpty(PunchDescription) || String.IsNullOrEmpty(LblPunchCategory))
                    {
                        _userDialogs.AlertAsync("Please, fill all the required fields", "", "Ok");
                        App.IsBusy = false;
                        return;
                    }
                    if (IsCreateNewPunch)
                    {
                        if (NextButtonText == "Next")
                        {
                            IsVisibleStep1 = false;
                            IsVisibleStep2 = true;
                            NextButtonText = "Save";
                        }
                        else if (NextButtonText == "Save")
                        {
                            if (SelectedPunchList == null)
                                SavePunchList(false);
                            else if (SelectedPunchList.status.ToUpper().Contains("PENDING"))
                                SavePunchList(true);
                            else SavePunchList(false);

                        }
                    }
                    else
                    {
                        if (IsVisibleStep2)
                        {
                            if (String.IsNullOrEmpty(LblResponsibleSection) || String.IsNullOrEmpty(LblLocation))
                            {
                                _userDialogs.AlertAsync("Please, fill all the required fields", "", "Ok");
                                App.IsBusy = false;
                                return;
                            }
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
                    var result = await _userDialogs.ConfirmAsync("Do you Want to Discard Punch List Changes?", "Discard", "Yes", "No");
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
                App.IsBusy = false;
            }
        }
        [Obsolete]
        private async void OnTapedAsync(string param)
        {
            try
            {
                switch (param)
                {
                    case "SelectSystem":
                        if (PunchSystem == null) return;
                        // if (!IsCreateNewPunch) return;
                        var system = PunchSystem.Select(x => x.SystemKey + " : " + x.SystemValue).Distinct().ToList();//PunchSystem.Where(x => !x.name.ToLower().StartsWith("sub system") && !x.name.EndsWith(".")).Select(x => x.name + " : " + x.attDesc).Distinct().ToList();
                        var _system = await ReadStringInPopup(system);
                        if (!string.IsNullOrWhiteSpace(_system) && _system != "clear") LblSystem = _system;
                        else LblSystem = null;
                        break;
                    case "SelectPCWBS":
                        if (PunchPCWBS == null) return;
                        var pcwbs = PunchPCWBS.Select(x => x.pcwbs).Distinct().ToList();
                        var _pcwbs = await ReadStringInPopup(pcwbs);
                        if (!string.IsNullOrWhiteSpace(_pcwbs) && _pcwbs != "clear") LblPCWBS = _pcwbs;
                        else LblPCWBS = null;
                        break;
                    case "SelectFWBS":
                        if (PunchFWBS == null) return;
                        var fwbs = PunchFWBS.Select(x => x.fwbs).Distinct().ToList();
                        var _fwbs = await ReadStringInPopup(fwbs);
                        if (!string.IsNullOrWhiteSpace(_fwbs) && _fwbs != "clear")
                        {
                            if (LblFWBS != _fwbs)
                                LblComponentCategory = LblComponent = LblAction = null;
                            LblFWBS = _fwbs;
                        }
                        else LblFWBS = LblComponentCategory = LblComponent = LblAction = PunchDescription = LblPunchCategory = null;
                        break;

                    case "SelectComponentCategory":
                        if (PunchComponent == null) return;
                        if (LblFWBS == null) return;
                        var componentCategory = PunchComponent.Where(x => x.fwbs.Substring(0, 1) == LblFWBS.Substring(0, 1)).Select(x => x.punch_level1).Distinct().ToList();
                        await Task.Delay(200);
                        var _componentCategory = await ReadStringInPopup(componentCategory);
                        if (!string.IsNullOrWhiteSpace(_componentCategory) && _componentCategory != "clear")
                        {
                            if (LblComponentCategory != _componentCategory)
                                LblComponent = LblAction = null;
                            LblComponentCategory = _componentCategory;
                        }
                        else LblComponentCategory = LblComponent = LblAction = PunchDescription = null;
                        break;

                    case "SelectComponent":
                        if (PunchComponent == null) return;
                        if (LblComponentCategory == null) return;
                        var component = PunchComponent.Where(x => x.punch_level1 == LblComponentCategory).Select(x => x.punch_level2).Distinct().ToList();
                        await Task.Delay(200);
                        var _component = await ReadStringInPopup(component);
                        if (!string.IsNullOrWhiteSpace(_component) && _component != "clear")
                        {
                            if (LblComponent != _component)
                                LblAction = null;
                            LblComponent = _component;
                        }
                        else LblComponent = LblAction = PunchDescription = null;
                        break;

                    case "SelectAction":
                        if (PunchComponent == null) return;
                        if (LblComponent == null) return;
                        var action = PunchComponent.Where(x => x.punch_level2 == LblComponent).Select(x => x.punch_level3).Distinct().ToList();
                        await Task.Delay(200);
                        var _action = await ReadStringInPopup(action);
                        if (!string.IsNullOrWhiteSpace(_action) && _action != "clear")
                        {
                            if (LblAction != _action)
                                LblAction = null;
                            LblAction = _action;
                            PunchDescription = LblComponentCategory + " - " + LblComponent + " " + LblAction;
                            T_PunchComponent SelectedPunch = PunchComponent.Where(x => x.fwbs.Substring(0, 1) == LblFWBS.Substring(0, 1) && x.punch_level1.Trim() == LblComponentCategory && x.punch_level2.Trim() == LblComponent
                                           && x.punch_level3.Trim() == LblAction).ToList().FirstOrDefault();
                            if (SelectedPunch != null)
                            {
                                LblPunchCategory = SelectedPunch.punch_category_code;
                                LblResponsibleSection = GetResponsibleSectionData(SelectedPunch.section_code);
                                LblResponsiblePosition = GetResponsiblePositionData(SelectedPunch.category_code);
                            }
                        }
                        else LblAction = PunchDescription = null;
                        break;

                    case "SelectPunchCategory":
                        if (PunchComponent == null) return;
                        var punchCategory = new List<string> { "A", "B", "C", "D" };
                        //PunchComponent.Where(x => x.fwbs.Trim() == LblFWBS && x.punch_level1.Trim() == LblComponentCategory && x.punch_level2.Trim() == LblComponent 
                        //                && x.punch_level3.Trim() == LblAction).Select(x => x.punch_category_code).ToList();
                        var _punchCategory = await ReadStringInPopup(punchCategory);
                        if (!string.IsNullOrWhiteSpace(_punchCategory) && _punchCategory != "clear")
                        {
                            if (LblPunchCategory != _punchCategory)
                                LblPunchCategory = null;
                            LblPunchCategory = _punchCategory;
                        }
                        else LblPunchCategory = null;
                        break;

                    case "SelectResponsibleSection":
                        if (PunchComponent == null) return;
                        if (LblAction == null && PunchDescription == null) return;
                        var _responsibleSection = await ReadStringInPopup(SectionCodeList);
                        if (!string.IsNullOrWhiteSpace(_responsibleSection) && _responsibleSection != "clear") LblResponsibleSection = _responsibleSection;
                        else LblResponsibleSection = null;
                        break;

                    case "SelectResponsiblePosition":
                        if (PunchComponent == null) return;
                        if (LblAction == null && PunchDescription == null) return;
                        var _responsiblePosition = await ReadStringInPopup(CategoryCodeList);
                        if (!string.IsNullOrWhiteSpace(_responsiblePosition) && _responsiblePosition != "clear")
                        {
                            if (LblResponsiblePosition != _responsiblePosition)
                                LblResponsiblePosition = null;
                            LblResponsiblePosition = _responsiblePosition;
                        }
                        else LblResponsiblePosition = null;
                        break;
                    case "SelectTagNo":
                        if (PunchList == null) return;
                        //List<T_CompletionsPunchList> PList = PunchList;
                        //if (!String.IsNullOrEmpty(LblSystem))
                        //    PList = PList.Where(x => x.systemno == LblSystem).ToList();
                        //if (!String.IsNullOrEmpty(LblPCWBS))
                        //    PList = PList.Where(x => x.PCWBS == LblPCWBS).ToList();
                        //if (!String.IsNullOrEmpty(LblFWBS))
                        //    PList = PList.Where(x => x.FWBS.Substring(0, 1) == LblFWBS.Substring(0, 1)).ToList();

                        //var tagNo = PList.Select(x => x.tagno).Distinct().ToList();
                        var tagNo = PunchList.Select(x => x.tagno).OrderBy(x => x).Distinct().ToList();
                        var _tagNo = await ReadStringInPopup(tagNo);
                        if (!string.IsNullOrWhiteSpace(_tagNo) && _tagNo != "clear") { LblTagNo = _tagNo; IsItrNo = false; }
                        else LblTagNo = null;
                        break;
                    case "SelectITRNo":
                        if (LblTagNo == null) return;
                        if (!IsItrNo)
                        {
                            // var ITRNo = PunchList.Where(x => x.tagno == LblTagNo && x.itrname != "").Select(x => x.itrname).Distinct().ToList();
                            var Itrs = await _CheckSheetPerTagRepository.GetAsync(x => x.TAGNAME == LblTagNo);
                            var ITRNo = Itrs.Select(x => x.CHECKSHEETNAME).ToList();
                            var _ITRNo = await ReadStringInUnEditablePopup(ITRNo);
                            if (!string.IsNullOrWhiteSpace(_ITRNo) && _ITRNo != "clear") LblITRNo = _ITRNo;
                            else
                                LblITRNo = null;
                        }
                        break;
                    case "SelectPunchType":
                        if (PunchTypeList == null) return;
                        var _PunchType = await ReadStringInPopup(PunchTypeList);
                        if (!string.IsNullOrWhiteSpace(_PunchType) && _PunchType != "clear")
                        {
                            if (LblPunchType != _PunchType)
                                LblPunchType = null;
                            LblPunchType = _PunchType;
                        }
                        else LblPunchType = null;
                        break;

                    case "SelectIssueOwner":
                        if (IssueOwnerList == null) return;
                        var _IssueOwner = await ReadStringInPopup(IssueOwnerList);
                        if (!string.IsNullOrWhiteSpace(_IssueOwner) && _IssueOwner != "clear")
                        {
                            if (LblIssuerOwner != _IssueOwner)
                                LblIssuerOwner = null;
                            LblIssuerOwner = _IssueOwner;
                        }
                        else LblIssuerOwner = null;
                        break;
                    case "SubContractorTapped":
                        if (SelectedPunchList.status.Trim() == "PENDING") return;
                        var _credentials1 = await ReadLoginPopup();
                        //SignatureInfo SubContractorUserDatail = await GetSignOffName(_credentials1);
                        SignatureInfo SubContractorUserDatail = await GetSignOffName(_credentials1, "SubContractor");
                        if (SubContractorUserDatail != null)
                        {
                            SubContractorID = SubContractorUserDatail.UserID;
                            LblSignartureSubContractor = SubContractorUserDatail.UserName;
                            SubContractorSignOn = SubContractorUserDatail.SignDate;
                            _Status = "RECTIFIED";
                        }
                        break;
                    case "ContractorTapped":
                        if (SelectedPunchList.status.Trim() == "PENDING") return;
                        if (!string.IsNullOrEmpty(LblSignartureSubContractor))
                        {
                            var _credentials2 = await ReadLoginPopup();
                            SignatureInfo ContractorUserDatail = await GetSignOffName(_credentials2, "Contractor");
                            if (ContractorUserDatail != null)
                            {
                                ContractorID = ContractorUserDatail.UserID;
                                LblSignartureContractor = ContractorUserDatail.UserName;
                                ContractorSignOn = ContractorUserDatail.SignDate;
                                _Status = "CLEARED";
                            }
                        }
                        else
                        {
                            _userDialogs.Alert("Please ensure the first sign off is completed before completing the second signature..", "", "Ok");
                        }
                        break;
                    case "ClientTapped":
                        if (SelectedPunchList.status.Trim() == "PENDING") return;
                        if (!string.IsNullOrEmpty(LblSignartureSubContractor) && !string.IsNullOrEmpty(LblSignartureContractor))
                        {
                            var _credentials3 = await ReadLoginPopup();
                            SignatureInfo ClientUserDatail = await GetSignOffName(_credentials3, "Client");
                            if (ClientUserDatail != null)
                            {
                                ClientID = ClientUserDatail.UserID;
                                LblSignartureClient = ClientUserDatail.UserName;
                                ClientSignOn = ClientUserDatail.SignDate;
                                _Status = "CLOSED";
                            }
                        }
                        else
                        {
                            _userDialogs.Alert("Please ensure the first two sign off is completed before completing the third signature..", "", "Ok");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private async void SavePunchList(bool IsUpdate)
        {
            try
            {
                if (String.IsNullOrEmpty(LblResponsibleSection) || String.IsNullOrEmpty(LblLocation))
                {
                    _userDialogs.AlertAsync("Please, fill all the required fields", "", "Ok");
                    return;
                }

                if (IsUpdate)
                {
                    if (SelectedPunchList != null)
                    {
                        SelectedPunchList.synced = false;
                        SelectedPunchList.systemno = LblSystem == null ? "" : LblSystem.Split(':').FirstOrDefault().Trim();
                        SelectedPunchList.PCWBS = LblPCWBS == null ? "" : LblPCWBS;
                        SelectedPunchList.FWBS = LblFWBS == null ? "" : LblFWBS;
                        SelectedPunchList.PunchDescriptionLevel1 = LblComponentCategory == null ? "" : LblComponentCategory;
                        SelectedPunchList.PunchDescriptionLevel2 = LblComponent == null ? "" : LblComponent;
                        SelectedPunchList.PunchDescriptionLevel3 = LblAction == null ? "" : LblAction;
                        SelectedPunchList.priority = LblPunchCategory == null ? "" : LblPunchCategory;
                        SelectedPunchList.description = PunchDescription == null ? "" : PunchDescription;
                        SelectedPunchList.comments = PunchComment == null ? "" : PunchComment;
                        SelectedPunchList.respdisc = LblResponsibleSection == null ? "" : LblResponsibleSection.Split('-').FirstOrDefault().Trim();
                        SelectedPunchList.RespPosition = LblResponsiblePosition == null ? "" : LblResponsiblePosition.Split('-').FirstOrDefault().Trim();
                        SelectedPunchList.tagno = LblTagNo == null ? "" : LblTagNo;
                        SelectedPunchList.location = LblLocation == null ? "" : LblLocation;
                        SelectedPunchList.itrname = LblITRNo == null ? "" : LblITRNo;
                        SelectedPunchList.imageLocalLocation = PunchListImagePAth == null ? "" : PunchListImagePAth;
                        SelectedPunchList.PunchType = LblPunchType == null ? "" : LblPunchType;
                        SelectedPunchList.IssuerOwner = LblIssuerOwner == null ? "" : LblIssuerOwner;

                        SelectedPunchList.COLUMN_PUNCH_signOffID1 = SubContractorID;
                        SelectedPunchList.COLUMN_PUNCH_signOffby1 = LblSignartureSubContractor;
                        SelectedPunchList.COLUMN_PUNCH_signOffOn1 = CheckDateFunction(SubContractorSignOn);

                        SelectedPunchList.COLUMN_PUNCH_signOffID2 = ContractorID;
                        SelectedPunchList.COLUMN_PUNCH_signOffby2 = LblSignartureContractor;
                        SelectedPunchList.COLUMN_PUNCH_signOffOn2 = CheckDateFunction(ContractorSignOn);

                        SelectedPunchList.COLUMN_PUNCH_signOffID3 = ClientID > 0 ? ClientID.ToString() : "";
                        SelectedPunchList.COLUMN_PUNCH_signOffby3 = LblSignartureClient;
                        SelectedPunchList.COLUMN_PUNCH_signOffOn3 = CheckDateFunction(ClientSignOn);

                        SelectedPunchList.COLUMN_PUNCH_signOffComment1 = SignOffComments;
                        SelectedPunchList.status = _Status;

                        await _CompletionsPunchListRepository.UpdateAsync(SelectedPunchList);
                        await Application.Current.MainPage.DisplayAlert("Update", "Punch List Updated..", "OK");
                        await navigationService.GoBackAsync();
                    }
                    else await Application.Current.MainPage.DisplayAlert("Error", "Punch List Updated Faild", "OK");
                }
                else
                {
                    ////Create New  
                    T_CompletionsPunchList NewPunchListItem = new T_CompletionsPunchList();
                    NewPunchListItem.project = Settings.ProjectName;
                    NewPunchListItem.systemno = LblSystem == null ? "" : LblSystem.Split(':').FirstOrDefault().Trim();
                    NewPunchListItem.PCWBS = LblPCWBS == null ? "" : LblPCWBS;
                    NewPunchListItem.FWBS = LblFWBS == null ? "" : LblFWBS;
                    NewPunchListItem.PunchDescriptionLevel1 = LblComponentCategory == null ? "" : LblComponentCategory;
                    NewPunchListItem.PunchDescriptionLevel2 = LblComponent == null ? "" : LblComponent;
                    NewPunchListItem.PunchDescriptionLevel3 = LblAction == null ? "" : LblAction;
                    NewPunchListItem.priority = LblPunchCategory == null ? "" : LblPunchCategory;
                    NewPunchListItem.description = PunchDescription == null ? "" : PunchDescription;
                    NewPunchListItem.comments = PunchComment == null ? "" : PunchComment;
                    NewPunchListItem.respdisc = LblResponsibleSection == null ? "" : LblResponsibleSection.Split('-').FirstOrDefault().Trim();
                    NewPunchListItem.RespPosition = LblResponsiblePosition == null ? "" : LblResponsiblePosition.Split('-').FirstOrDefault().Trim(); ;
                    NewPunchListItem.tagno = LblTagNo == null ? "" : LblTagNo;
                    NewPunchListItem.location = LblLocation == null ? "" : LblLocation;
                    NewPunchListItem.itrname = LblITRNo == null ? "" : LblITRNo;
                    NewPunchListItem.PunchType = LblPunchType == null ? "" : LblPunchType;
                    NewPunchListItem.IssuerOwner = LblIssuerOwner == null ? "" : LblIssuerOwner;
                    NewPunchListItem.imageLocalLocation = PunchListImagePAth == null ? "" : PunchListImagePAth;

                    NewPunchListItem.COLUMN_PUNCH_signOffID1 = SubContractorID;
                    NewPunchListItem.COLUMN_PUNCH_signOffby1 = LblSignartureSubContractor;
                    NewPunchListItem.COLUMN_PUNCH_signOffOn1 = CheckDateFunction(SubContractorSignOn);

                    NewPunchListItem.COLUMN_PUNCH_signOffID2 = ContractorID;
                    NewPunchListItem.COLUMN_PUNCH_signOffby2 = LblSignartureContractor;
                    NewPunchListItem.COLUMN_PUNCH_signOffOn2 = CheckDateFunction(ContractorSignOn);

                    NewPunchListItem.COLUMN_PUNCH_signOffID3 = ClientID > 0 ? ClientID.ToString() : "";
                    NewPunchListItem.COLUMN_PUNCH_signOffby3 = LblSignartureClient;
                    NewPunchListItem.COLUMN_PUNCH_signOffOn3 = CheckDateFunction(ClientSignOn);

                    NewPunchListItem.COLUMN_PUNCH_signOffComment1 = SignOffComments;
                    NewPunchListItem.synced = false;
                    NewPunchListItem.status = "PENDING";
                    NewPunchListItem.uniqueno = NewPunchListItem.originator = NewPunchListItem.milestone = NewPunchListItem.pandid = NewPunchListItem.rfqreq = "";
                    NewPunchListItem.originator = Settings.CompletionUserName;
                    NewPunchListItem.originatoruserid = Settings.CompletionUserID;
                    NewPunchListItem.ordate = DateTime.Now;
                    NewPunchListItem.originatordisc = NewPunchListItem.itrItemNo = NewPunchListItem.area = NewPunchListItem.subsystem = NewPunchListItem.workpack = "";
                    NewPunchListItem.jobpack = NewPunchListItem.TCU = NewPunchListItem.PTU = "";
                    await _CompletionsPunchListRepository.InsertAsync(NewPunchListItem);
                    await Application.Current.MainPage.DisplayAlert("Save", "Punch List is Saved.", "OK");
                    await navigationService.GoBackAsync();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Punch List Updated Faild", "OK");
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
        public static Task<string> ReadStringInUnEditablePopup(List<string> Source)
        {
            var vm = new UnEditableFilterPopupViewModel(Source);
            //vm.FilterList = Source;
            var tcs = new TaskCompletionSource<string>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var page = new UnEditableFilterPopup(vm, Source);
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
        private async Task<SignatureInfo> GetSignOffName(LoginModel _credentials, string signFor)
        {
            bool rights = false;
            SignatureInfo SignInfo = new SignatureInfo();
            //_CompletionpageHelper.CompletionTokenTimeStamp = DateTime.Now.ToString(AppConstant.DateSaveFormat);
            //_CompletionpageHelper.CompletionToken = Settings.CompletionAccessToken = ModsTools.CompletionsCreateToken(_credentials.UserName, _credentials.Password, _CompletionpageHelper.CompletionTokenTimeStamp);
            //_CompletionpageHelper.CompletionTokenExpiry = DateTime.Now.AddHours(2);
            //_CompletionpageHelper.CompletionUnitID = Settings.UnitID;
            if (CrossConnectivity.Current.IsConnected)
            {
                _CompletionpageHelper.CompletionTokenTimeStamp = DateTime.Now.ToString(AppConstant.DateSaveFormat);
                _CompletionpageHelper.CompletionToken = Settings.CompletionAccessToken = ModsTools.CompletionsCreateToken(_credentials.UserName, _credentials.Password, _CompletionpageHelper.CompletionTokenTimeStamp);
                _CompletionpageHelper.CompletionTokenExpiry = DateTime.Now.AddHours(2);
                _CompletionpageHelper.CompletionUnitID = Settings.UnitID;

                var Result = ModsTools.CompletionValidateToken(_CompletionpageHelper.CompletionToken, _CompletionpageHelper.CompletionTokenTimeStamp);
                // var result =  _checkValidLogin.GetValidToken(_credentials);


                if (!Result)
                {
                    //check offline user is available or not for sign
                    var offlineUser = await _CompletionsUserRepository.GetAsync(x => x.UserName == _credentials.UserName && x.Password == _credentials.Password);
                    if (offlineUser.Any())
                    {
                        T_CompletionsUsers offileUser = offlineUser.FirstOrDefault();
                        List<T_Ccms_signature> offlineSignaturelist = new List<T_Ccms_signature>();

                        if (signFor == "SubContractor" && (offileUser.Company_Category_Code == "S" || offileUser.Company_Category_Code == "M" || offileUser.Company_Category_Code == "C"))
                            rights = true;
                        else if (signFor == "Contractor" && offileUser.Company_Category_Code == "M" || offileUser.Company_Category_Code == "C")
                            rights = true;
                        else if (signFor == "Client" && offileUser.Company_Category_Code == "C")
                            rights = true;

                        if (rights)
                        {
                            SignInfo.UserID = Convert.ToInt32(offileUser.ID);
                            SignInfo.UserName = offileUser.FullName;
                            SignInfo.SignDate = DateTime.Now;
                            return SignInfo;
                        }
                        else
                        {
                            _userDialogs.Alert("Unable to complete " + signFor + " signature as you do not have the rights.", "", "Ok");
                            return null;
                        }                        
                    }
                }



                if (Result)
                {
                    string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.GetUser(_credentials.UserName, _credentials.Password, Settings.CurrentDB), Settings.CompletionAccessToken);
                    var CurrentUser = JsonConvert.DeserializeObject<T_UserControl>(JsonString);
                    // SignInfo.UserID = Convert.ToInt32(CurrentUser.ID);
                    //SignInfo.UserName = CurrentUser.FullName;
                    // SignInfo.SignDate = DateTime.Now;
                    if (signFor == "SubContractor" && (CurrentUser.Company_Category_Code == "S" || CurrentUser.Company_Category_Code == "M" || CurrentUser.Company_Category_Code == "C"))
                        rights = true;
                    else if (signFor == "Contractor" && CurrentUser.Company_Category_Code == "M" || CurrentUser.Company_Category_Code == "C")
                        rights = true;
                    else if (signFor == "Client" && CurrentUser.Company_Category_Code == "C")
                        rights = true;

                    if (rights)
                    {
                        SignInfo.UserID = Convert.ToInt32(CurrentUser.ID);
                        SignInfo.UserName = CurrentUser.FullName;
                        SignInfo.SignDate = DateTime.Now;
                    }
                    else
                    {
                        _userDialogs.Alert("Unable to complete " + signFor + " signature as you do not have the rights.", "", "Ok");
                        return null;
                    }
                }
                else
                {
                    _userDialogs.Alert("Failed to login to this account.", "Login Error", "Ok");
                    return null;
                }
            }
            else
            {

                var userDetails = await _CompletionsUserRepository.GetAsync();
                var _cuser = userDetails.Where(u => u.UserName == _credentials.UserName && u.Password == _credentials.Password).ToList();
                if (_cuser.Any())
                {

                    var _currentUser = _cuser.FirstOrDefault();
                    if (signFor == "SubContractor" && (_currentUser.Company_Category_Code == "S" || _currentUser.Company_Category_Code == "M" || _currentUser.Company_Category_Code == "C"))
                        rights = true;
                    else if (signFor == "Contractor" && _currentUser.Company_Category_Code == "M" || _currentUser.Company_Category_Code == "C")
                        rights = true;
                    else if (signFor == "Client" && _currentUser.Company_Category_Code == "C")
                        rights = true;

                    if (rights)
                    {
                        SignInfo.UserID = Convert.ToInt32(_currentUser.ID);
                        SignInfo.UserName = _currentUser.FullName;
                        SignInfo.SignDate = DateTime.Now;
                    }
                    else
                    {
                        _userDialogs.Alert("Unable to complete " + signFor + " signature as you do not have the rights.", "", "Ok");
                        return null;
                    }
                }
                else
                {
                    await _userDialogs.AlertAsync("Account not found, double check your details or attempt to login online", "Login Failed", "OK");
                    return null;
                }
            }


            return SignInfo;
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
        private async void LoadImage(string imageLocalLocation)
        {
            if (!string.IsNullOrWhiteSpace(imageLocalLocation))
            {
                if (!imageLocalLocation.Contains("https://"))
                {
                    var imageSrc = await DependencyService.Get<ISaveFiles>().GetImage(imageLocalLocation);
                    CapturedImage = imageSrc;
                }
                else
                    CapturedImage = imageLocalLocation;
            }
        }
        private DateTime CheckDateFunction(DateTime DateValue)
        {
            if (DateValue < Convert.ToDateTime("1970-01-01"))
                DateValue = Convert.ToDateTime("1970-01-01");
            return DateValue;
        }
        private void LoadPunchListData()
        {
            LblSystem = String.IsNullOrEmpty(SelectedPunchList.systemno) ? null : GetSystemData(SelectedPunchList.systemno);
            LblPCWBS = SelectedPunchList.PCWBS == "" ? null : SelectedPunchList.PCWBS;
            LblFWBS = SelectedPunchList.FWBS == "" ? null : SelectedPunchList.FWBS.Substring(0, 1) + "000";
            LblComponentCategory = SelectedPunchList.PunchDescriptionLevel1 == "" ? null : SelectedPunchList.PunchDescriptionLevel1;
            LblComponent = SelectedPunchList.PunchDescriptionLevel2 == "" ? null : SelectedPunchList.PunchDescriptionLevel2;
            LblAction = SelectedPunchList.PunchDescriptionLevel3 == "" ? null : SelectedPunchList.PunchDescriptionLevel3;
            LblPunchCategory = SelectedPunchList.priority == "" ? null : SelectedPunchList.priority;
            PunchDescription = SelectedPunchList.description == "" ? null : SelectedPunchList.description;
            PunchComment = SelectedPunchList.comments == "" ? null : SelectedPunchList.comments;
            LblResponsibleSection = String.IsNullOrEmpty(SelectedPunchList.respdisc) ? null : GetResponsibleSectionData(SelectedPunchList.respdisc);
            LblResponsiblePosition = String.IsNullOrEmpty(SelectedPunchList.RespPosition) ? null : GetResponsiblePositionData(SelectedPunchList.RespPosition); //SelectedPunchList.RespPosition;
            LblTagNo = SelectedPunchList.tagno == "" ? null : SelectedPunchList.tagno;
            LblLocation = SelectedPunchList.location == "" ? null : SelectedPunchList.location;
            LblITRNo = SelectedPunchList.itrname == "" ? null : SelectedPunchList.itrname;
            LblPunchType = SelectedPunchList.PunchType == "" ? null : SelectedPunchList.PunchType;
            LblIssuerOwner = SelectedPunchList.IssuerOwner == "" ? null : SelectedPunchList.IssuerOwner;

            //LblLocation = SelectedPunchList.location == "" ? null : SelectedPunchList.location;
            SubContractorID = SelectedPunchList.COLUMN_PUNCH_signOffID1;
            LblSignartureSubContractor = SelectedPunchList.COLUMN_PUNCH_signOffby1;
            SubContractorSignOn = SelectedPunchList.COLUMN_PUNCH_signOffOn1;

            ContractorID = SelectedPunchList.COLUMN_PUNCH_signOffID2;
            LblSignartureContractor = SelectedPunchList.COLUMN_PUNCH_signOffby2;
            ContractorSignOn = SelectedPunchList.COLUMN_PUNCH_signOffOn2;

            ClientID = String.IsNullOrEmpty(SelectedPunchList.COLUMN_PUNCH_signOffID3) ? 0 : Convert.ToInt32(SelectedPunchList.COLUMN_PUNCH_signOffID3);
            LblSignartureClient = SelectedPunchList.COLUMN_PUNCH_signOffby3;
            ClientSignOn = SelectedPunchList.COLUMN_PUNCH_signOffOn3;
            SignOffComments = SelectedPunchList.COLUMN_PUNCH_signOffComment1;

            PunchListImagePAth = SelectedPunchList.imageLocalLocation;
            _Status = SelectedPunchList.status;
            LoadImage(SelectedPunchList.imageLocalLocation);
        }
        private string GetResponsibleSectionData(string strValue)
        {
            var ResponsibleSectionData = Task.Run(async () => await _SectionCodeRepository.QueryAsync("SELECT * FROM T_SectionCode Where ModelName = '" + Settings.ModelName + "' AND (SectionCode = '" + strValue + "' OR Description = '" + strValue + "')")).Result;
            T_SectionCode Item = ResponsibleSectionData.FirstOrDefault();
            if (Item != null)
                return Item.SectionCode + " - " + Item.Description;
            else return null;
        }
        private string GetResponsiblePositionData(string strValue)
        {
            //var ResponsiblePositionData = Task.Run(async () => await _CompanyCategoryCodeRepository.QueryAsync("SELECT * FROM T_CompanyCategoryCode Where ModelName = '" + Settings.ModelName + "' AND (CompanyCategoryCode = '" + strValue + "' OR Description = '" + strValue + "')")).Result;
            //T_CompanyCategoryCode Item = ResponsiblePositionData.FirstOrDefault();
            //if (Item != null)
            //    return Item.CompanyCategoryCode + " - " + Item.Description;
            //else return null;
            var sdf = CategoryCodeList;
            return CategoryCodeList.Where(x => x.Split('-').FirstOrDefault().Contains(strValue)).FirstOrDefault();
        }
        private string GetSystemData(string strValue)
        {
            //var SystemData = Task.Run(async () => await _CompletionSystemsRepository.QueryAsync("SELECT * FROM T_CompletionSystems Where modelName = '" + Settings.ModelName + "' AND name = '" + strValue + "'")).Result;
            //T_CompletionSystems Item = SystemData.FirstOrDefault();
            //if (Item != null)
            //    return Item.name + " : " + Item.attDesc;
            //else return null;
            var SystemData = Task.Run(async () => await _PunchSystemRepository.QueryAsync("SELECT * FROM T_PunchSystem Where modelName = '" + Settings.ModelName + "' AND SystemKey = '" + strValue + "'")).Result;
            T_PunchSystem Item = SystemData.FirstOrDefault();
            if (Item != null)
                return Item.SystemKey + " : " + Item.SystemValue;
            else return null;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            App.IsBusy = false;
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            App.IsBusy = false;
            if (parameters.Count > 0 && parameters.ContainsKey("SelectedPunchList"))
            {
                SelectedPunchList = parameters.GetValue<T_CompletionsPunchList>("SelectedPunchList");
                SelectedPunchList.status = !String.IsNullOrEmpty(SelectedPunchList.status) ? SelectedPunchList.status : "";
                LoadPunchListData();
                //IsCreateNewPunch = false;
                if (SelectedPunchList.status.ToUpper().Contains("PENDING"))
                    IsCreateNewPunch = true;
                else
                    IsCreateNewPunch = false;
            }
            else if (parameters.Count > 0 && parameters.ContainsKey("SelectedPunchListForCreate"))
            {
                SelectedPunchList = parameters.GetValue<T_CompletionsPunchList>("SelectedPunchListForCreate");
                SelectedPunchList.status = !String.IsNullOrEmpty(SelectedPunchList.status) ? SelectedPunchList.status : "";
                LoadPunchListData();
                IsCreateNewPunch = IsItrNo = true;
            }
            else
                IsCreateNewPunch = true;
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }

}

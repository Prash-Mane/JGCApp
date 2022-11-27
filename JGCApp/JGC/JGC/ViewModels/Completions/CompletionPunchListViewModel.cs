using Acr.UserDialogs;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables.Completions;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.Common.Extentions;
using JGC.Common.Helpers;

namespace JGC.ViewModels.Completions
{
    public class CompletionPunchListViewModel : BaseViewModel
    {

        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_CompletionsPunchList> _CompletionsPunchListRepository;


        private ObservableCollection<T_CompletionsPunchList> itemSourceCompletionsPunchList;
        public ObservableCollection<T_CompletionsPunchList> ItemSourceCompletionsPunchList
        {
            get { return itemSourceCompletionsPunchList; }
            set { itemSourceCompletionsPunchList = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_CompletionsPunchList> punchListitem;
        public ObservableCollection<T_CompletionsPunchList> PunchListitem
        {
            get { return punchListitem; }
            set { punchListitem = value; RaisePropertyChanged(); }
        }
        private T_CompletionsPunchList selectedPunchList;
        public T_CompletionsPunchList SelectedPunchList
        {
            get { return selectedPunchList; }
            set { selectedPunchList = value; RaisePropertyChanged(); }
        }

        //private list<strings> itemSourceFilterSystem;
        //public ObservableCollection<T_CompletionsPunchList> ItemSourceCompletionsPunchList
        //{
        //    get { return itemSourceFilterSystem; }
        //    set { itemSourceFilterSystem = value; RaisePropertyChanged(); }
        //}

        private ObservableCollection<string> filterSourceSytems;
        public ObservableCollection<string> FilterSourceSytems
        {
            get { return filterSourceSytems; }
            set { filterSourceSytems = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<string> filterSourceSubSytems;
        public ObservableCollection<string> FilterSourceSubSytems
        {
            get { return filterSourceSubSytems; }
            set { filterSourceSubSytems = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<string> filterSourcePCWBS;
        public ObservableCollection<string> FilterSourcePCWBS
        {
            get { return filterSourcePCWBS; }
            set { filterSourcePCWBS = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<string> filterSourceFWBS;
        public ObservableCollection<string> FilterSourceFWBS
        {
            get { return filterSourceFWBS; }
            set { filterSourceFWBS = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<string> filterSourceLocation;
        public ObservableCollection<string> FilterSourceLocation
        {
            get { return filterSourceLocation; }
            set { filterSourceLocation = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<string> filterSourceDiscipilines;
        public ObservableCollection<string> FilterSourceDiscipilines
        {
            get { return filterSourceDiscipilines; }
            set { filterSourceDiscipilines = value; RaisePropertyChanged(); }
        }

        //private ObservableCollection<string> filterSourceWorkPacks;
        //public ObservableCollection<string> FilterSourceWorkPacks
        //{
        //    get { return filterSourceWorkPacks; }
        //    set { filterSourceWorkPacks = value; RaisePropertyChanged(); }
        //}
        private ObservableCollection<string> filterSourceTags;
        public ObservableCollection<string> FilterSourceTags
        {
            get { return filterSourceTags; }
            set { filterSourceTags = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<string> filterSourceITR;
        public ObservableCollection<string> FilterSourceITR
        {
            get { return filterSourceITR; }
            set { filterSourceITR = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<string> filterSourceResponsibleSection;
        public ObservableCollection<string> FilterSourceResponsibleSection
        {
            get { return filterSourceResponsibleSection; }
            set { filterSourceResponsibleSection = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<string> filterSourceStatus;
        public ObservableCollection<string> FilterSourceStatus
        {
            get { return filterSourceStatus; }
            set { filterSourceStatus = value; RaisePropertyChanged(); }
        }

        private int pickerSelectedIndexSystem;
        public int PickerSelectedIndexSystem
        {
            get { return pickerSelectedIndexSystem; }
            set { pickerSelectedIndexSystem = value; RaisePropertyChanged(); }
        }
        private int pickerSelectedIndexSubSystem;
        public int PickerSelectedIndexSubSystem
        {
            get { return pickerSelectedIndexSubSystem; }
            set { pickerSelectedIndexSubSystem = value; RaisePropertyChanged(); }
        }
        private int pickerSelectedIndexPCWBS;
        public int PickerSelectedIndexPCWBS
        {
            get { return pickerSelectedIndexPCWBS; }
            set { pickerSelectedIndexPCWBS = value; RaisePropertyChanged(); }
        }
        private int pickerSelectedIndexFWBS;
        public int PickerSelectedIndexFWBS
        {
            get { return pickerSelectedIndexFWBS; }
            set { pickerSelectedIndexFWBS = value; RaisePropertyChanged(); }
        }
        private int pickerSelectedIndexLocation;
        public int PickerSelectedIndexLocation
        {
            get { return pickerSelectedIndexLocation; }
            set { pickerSelectedIndexLocation = value; RaisePropertyChanged(); }
        }
        private int pickerSelectedIndexDisci;
        public int PickerSelectedIndexDisci
        {
            get { return pickerSelectedIndexDisci; }
            set { pickerSelectedIndexDisci = value; RaisePropertyChanged(); }
        }

        //private int pickerSelectedIndexWorkPack;
        //public int PickerSelectedIndexWorkPack
        //{
        //    get { return pickerSelectedIndexWorkPack; }
        //    set { pickerSelectedIndexWorkPack = value; RaisePropertyChanged(); }
        //}
        private int pickerSelectedIndexTags;
        public int PickerSelectedIndexTags
        {
            get { return pickerSelectedIndexTags; }
            set { pickerSelectedIndexTags = value; RaisePropertyChanged(); }
        }
        private int pickerSelectedIndexITR;
        public int PickerSelectedIndexITR
        {
            get { return pickerSelectedIndexITR; }
            set { pickerSelectedIndexITR = value; RaisePropertyChanged(); }
        }
        private int pickerSelectedIndexResponsibleSection;
        public int PickerSelectedIndexResponsibleSection
        {
            get { return pickerSelectedIndexResponsibleSection; }
            set { pickerSelectedIndexResponsibleSection = value; RaisePropertyChanged(); }
        }
        private int pickerSelectedIndexStatus;
        public int PickerSelectedIndexStatus
        {
            get { return pickerSelectedIndexStatus; }
            set { pickerSelectedIndexStatus = value; RaisePropertyChanged(); }
        }

        #region Delegate Commands   
        public ICommand ButtonCommand
        {
            get
            {
                return new Command<string>(OnButtonClicked);
            }
        }

        private async void OnButtonClicked(string param)
        {
            if (!App.IsBusy)
            {
                App.IsBusy = true;
                if (param == "CreateNewPunch")
                {
                    if (Settings.CurrentDB == "JGC" || Settings.CurrentDB == "JGC_HARMONY" || Settings.CurrentDB == "JGC_ITR" || Settings.CurrentDB == "JGC_DEMO" || 
                        Settings.CurrentDB == "ROVUMA_TEST" || Settings.CurrentDB == "YOC_DEMO" || Settings.CurrentDB == "JGC_HARMONYCOMP")
                        await navigationService.NavigateAsync<NewPunchViewModel>();
                    else
                        await navigationService.NavigateAsync<CreateNewPunchViewModel>();
                }
                else if (param == "SelectThisPunch")
                {
                    if (SelectedPunchList != null)
                    {
                        var parameter = new NavigationParameters();
                        parameter.Add("SelectedPunchList", SelectedPunchList);
                        if (Settings.CurrentDB == "JGC" || Settings.CurrentDB == "JGC_HARMONY" || Settings.CurrentDB == "JGC_ITR" || Settings.CurrentDB == "JGC_DEMO" || 
                            Settings.CurrentDB == "ROVUMA_TEST" || Settings.CurrentDB == "YOC_DEMO" || Settings.CurrentDB == "JGC_HARMONYCOMP")
                            await navigationService.NavigateAsync<NewPunchViewModel>(parameter);
                        else
                            await navigationService.NavigateAsync<CreateNewPunchViewModel>(parameter);
                    }
                    else
                        App.IsBusy = false;

                }
            }
        }
        #endregion
        public CompletionPunchListViewModel(INavigationService _navigationService,
        IUserDialogs _userDialogs,
        IHttpHelper _httpHelper,
        IRepository<T_CompletionsPunchList> _CompletionsPunchListRepository,
        ICheckValidLogin _checkValidLogin) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._CompletionsPunchListRepository = _CompletionsPunchListRepository;

            // BindPunchListItems();
        }

        private async void BindPunchListItems()
        {
            //Bind DataGrid
            var PunchListdata = await _CompletionsPunchListRepository.GetAsync(i=>i.project == Settings.ProjectName);
            PunchListitem = new ObservableCollection<T_CompletionsPunchList>(PunchListdata);


            if (PunchListitem != null && PunchListitem.Any())
            {
                //BInd Filters
                FilterSourceSytems = new ObservableCollection<string>(PunchListitem.Where(x => !String.IsNullOrEmpty(x.systemno)).Select(x => x.systemno).Distinct());
                FilterSourceSubSytems = new ObservableCollection<string>(PunchListitem.Where(x => !String.IsNullOrEmpty(x.subsystem)).Select(x => x.subsystem).Distinct());
                FilterSourcePCWBS = new ObservableCollection<string>(PunchListitem.Where(x => !String.IsNullOrEmpty(x.PCWBS)).Select(x => x.PCWBS).Distinct());
                FilterSourceFWBS = new ObservableCollection<string>(PunchListitem.Where(x => !String.IsNullOrEmpty(x.FWBS)).Select(x => x.FWBS).Distinct());
                FilterSourceLocation = new ObservableCollection<string>(PunchListitem.Where(x => !String.IsNullOrEmpty(x.location)).Select(x => x.location).Distinct());
                FilterSourceDiscipilines = new ObservableCollection<string>(PunchListitem.Where(x => !String.IsNullOrEmpty(x.respdisc)).Select(x => x.respdisc).Distinct());
                FilterSourceTags = new ObservableCollection<string>(PunchListitem.Where(x => !String.IsNullOrEmpty(x.tagno)).Select(x => x.tagno).Distinct());
                FilterSourceITR = new ObservableCollection<string>(PunchListitem.Where(x => !String.IsNullOrEmpty(x.itrname)).Select(x => x.itrname).OrderBy(x => x).Distinct());
                FilterSourceResponsibleSection = new ObservableCollection<string>(PunchListitem.Where(x => !String.IsNullOrEmpty(x.respdisc)).Select(x => x.respdisc).Distinct());
                List<string> Status = new List<string>() { "Cleared", "Closed", "Open", "Pending", "Rectified", "Rejected" };
                //new List<string>() { "03 - Management", "08 - Engineer", "09 - Inspector(Civil & Structure)", "10 - Inspector(Building)", "11 - Inspector(Equipment)",
                //                                    "12 - Inspector(Piping)", "13 - Inspector(Instrumental)", "14 - Inspector(Electrical)", "15 - Inspector(Paint & Insulation)" };
                FilterSourceStatus = new ObservableCollection<string>(Status); //new ObservableCollection<string>(PunchListitem.Where(x => !String.IsNullOrEmpty(x.RespPosition)).Select(x => x.RespPosition).Distinct());
                //FilterSourceWorkPacks = new ObservableCollection<string>(PunchListitem.Where(x => x.workpack != null).Select(x => x.workpack).Distinct());
                //PunchListitem.ForEach(x => {
                //    FilterSourceSytems.Contains(x.systemno);
                //    FilterSourceSubSytems.Add(x.subsystem);
                //    FilterSourceDiscipilines.Add(x.respdisc);
                //    FilterSourceTags.Add(x.tagno);
                //    FilterSourceWorkPacks.Add(x.workpack);
                //});

                FilterSourceSytems.Insert(0, "Filter System"); PickerSelectedIndexSystem = 0;
                FilterSourceSubSytems.Insert(0, "Filter SubSystem"); PickerSelectedIndexSubSystem = 0;
                FilterSourcePCWBS.Insert(0, "Filter PCWBS"); PickerSelectedIndexPCWBS = 0;
                FilterSourceFWBS.Insert(0, "Filter FWBS"); PickerSelectedIndexFWBS = 0;
                FilterSourceLocation.Insert(0, "Filter Location"); PickerSelectedIndexLocation = 0;
                FilterSourceDiscipilines.Insert(0, "Filter Disciplines"); PickerSelectedIndexDisci = 0;
                FilterSourceTags.Insert(0, "Filter Tag"); PickerSelectedIndexTags = 0;
                FilterSourceITR.Insert(0, "Filter ITR"); PickerSelectedIndexITR = 0;
                FilterSourceResponsibleSection.Insert(0, "Filter Responsible Section"); PickerSelectedIndexResponsibleSection = 0;
                FilterSourceStatus.Insert(0, "Filter Status"); PickerSelectedIndexStatus = 0;
                //FilterSourceWorkPacks.Insert(0, "Filter Work Packs"); PickerSelectedIndexWorkPack = 0;

            }
            else
            {
                FilterSourceSytems = new ObservableCollection<string>(); 
                FilterSourceSubSytems = new ObservableCollection<string>(); 
                FilterSourcePCWBS = new ObservableCollection<string>(); 
                FilterSourceFWBS = new ObservableCollection<string>();
                FilterSourceLocation = new ObservableCollection<string>();
                FilterSourceDiscipilines = new ObservableCollection<string>();
                FilterSourceTags = new ObservableCollection<string>();
                FilterSourceITR = new ObservableCollection<string>();
                FilterSourceResponsibleSection = new ObservableCollection<string>();
                FilterSourceStatus = new ObservableCollection<string>();

                FilterSourceSytems.Insert(0, "Filter System"); PickerSelectedIndexSystem = 0;
                FilterSourceSubSytems.Insert(0, "Filter SubSystem"); PickerSelectedIndexSubSystem = 0;
                FilterSourcePCWBS.Insert(0, "Filter PCWBS"); PickerSelectedIndexPCWBS = 0;
                FilterSourceFWBS.Insert(0, "Filter FWBS"); PickerSelectedIndexFWBS = 0;
                FilterSourceLocation.Insert(0, "Filter Location"); PickerSelectedIndexLocation = 0;
                FilterSourceDiscipilines.Insert(0, "Filter Disciplines"); PickerSelectedIndexDisci = 0;
                FilterSourceTags.Insert(0, "Filter Tag"); PickerSelectedIndexTags = 0;
                FilterSourceITR.Insert(0, "Filter ITR"); PickerSelectedIndexITR = 0;
                FilterSourceResponsibleSection.Insert(0, "Filter Responsible Section"); PickerSelectedIndexResponsibleSection = 0;
                FilterSourceStatus.Insert(0, "Filter Status"); PickerSelectedIndexStatus = 0;
            }
            ItemSourceCompletionsPunchList = new ObservableCollection<T_CompletionsPunchList>(PunchListdata);
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            App.IsBusy = false;
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            App.IsBusy = false;
            base.OnNavigatedTo(parameters);
            BindPunchListItems();
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }

    }
}
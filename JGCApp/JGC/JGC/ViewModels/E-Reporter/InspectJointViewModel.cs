using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Interfaces;
using JGC.DataBase.DataTables;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using JGC.Common.Extentions;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using JGC.Views.E_Reporter;
using System.Collections.ObjectModel;
using JGC.DataBase;
using System.Linq;
using JGC.Models;
using JGC.Common.Helpers;
using JGC.Models.E_Reporter;
using System.Threading.Tasks;

namespace JGC.ViewModels.E_Reporter
{
   public class InspectJointViewModel :BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_Welders> _weldersRepository;
        private readonly IRepository<T_RT_Defects> _defectsRepository;
        private readonly IRepository<T_WPS_MSTR> _wpsRepository;
        private readonly IRepository<T_BaseMetal> _baseMetalRepository;
        private readonly IRepository<T_DWR_HeatNos> _DWR_HeatNosRepository;
        private readonly IRepository<T_WeldProcesses> _WeldProcessesRepository;
        private readonly IRepository<T_DWR> _DWRRepository;
        private readonly IRepository<T_EReports> _eReportRepository;

        public static EReporterHelper _EReporterHelper = new EReporterHelper();
        JointPopupModel JointDetail = new JointPopupModel();
        InspectResultModel InspectResult = new InspectResultModel();
       // T_DWR CreateNewDWR = new T_DWR();

        #region Properties

        private string _AFI_Number;
        public string AFI_Number
        {
            get { return _AFI_Number; }
            set { _AFI_Number = value; RaisePropertyChanged(); }
        }
        private string _Revision_No;
        public string Revision_No
        {
            get { return _Revision_No; }
            set { _Revision_No = value; RaisePropertyChanged(); }
        }
        private DateTime _Welded_Date;
        public DateTime Welded_Date
        {
            get { return _Welded_Date; }
            set { _Welded_Date = value; RaisePropertyChanged(); }
        }
        private string _Spool_Drawing_Number;
        public string Spool_Drawing_Number
        {
            get { return _Spool_Drawing_Number; }
            set { _Spool_Drawing_Number = value; RaisePropertyChanged(); }
        }
        private string _Joint_No;
        public string Joint_No
        {
            get { return _Joint_No; }
            set { _Joint_No = value; RaisePropertyChanged(); }
        }
        private DateTime _FitUpDate;
        public DateTime FitUpDate
        {
            get { return _FitUpDate; }
            set { _FitUpDate = value; RaisePropertyChanged(); }
        }

        private T_DWR _SelectedDWRReport;
        public T_DWR SelectedDWRReport
        {
            get { return _SelectedDWRReport; }
            set { _SelectedDWRReport = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_Welders> _RootWelderList;
        public ObservableCollection<T_Welders> RootWelderList
        {
            get { return _RootWelderList; }
            set { _RootWelderList = value; RaisePropertyChanged(); }
        }
        private T_Welders _SelectedRootWelder1;
        public T_Welders SelectedRootWelder1
        {
            get { return _SelectedRootWelder1; }
            set
            {
                if (SetProperty(ref _SelectedRootWelder1, value))
                {
                    RaisePropertyChanged("SelectedRootWelder1");
                    OnPropertyChanged();
                }
            }
        }
        private T_Welders _SelectedRootWelder2;
        public T_Welders SelectedRootWelder2
        {
            get { return _SelectedRootWelder2; }
            set
            {
                if (SetProperty(ref _SelectedRootWelder2, value))
                {
                    RaisePropertyChanged("SelectedRootWelder2");
                    OnPropertyChanged();
                }
            }
        }
        private T_Welders _SelectedRootWelder3;
        public T_Welders SelectedRootWelder3
        {
            get { return _SelectedRootWelder3; }
            set
            {
                if (SetProperty(ref _SelectedRootWelder3, value))
                {
                    RaisePropertyChanged("SelectedRootWelder3");
                    OnPropertyChanged();
                }
            }
        }
        private T_Welders _SelectedRootWelder4;
        public T_Welders SelectedRootWelder4
        {
            get { return _SelectedRootWelder4; }
            set
            {
                if (SetProperty(ref _SelectedRootWelder4, value))
                {
                    RaisePropertyChanged("SelectedRootWelder4");
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<T_WeldProcesses> _RootWeldProcess;
        public ObservableCollection<T_WeldProcesses> RootWeldProcess
        {
            get { return _RootWeldProcess; }
            set { _RootWeldProcess = value; RaisePropertyChanged(); }
        }
        private T_WeldProcesses _SelectedRootWeldProcess;
        public T_WeldProcesses SelectedRootWeldProcess
        {
            get { return _SelectedRootWeldProcess; }
            set
            {
                if (SetProperty(ref _SelectedRootWeldProcess, value))
                {
                    RaisePropertyChanged("SelectedRootWeldProcess");
                    OnPropertyChanged();
                }
            }
        }
        private string _RootProdWelder1Image;
        public string RootProdWelder1Image
        {
            get { return _RootProdWelder1Image; }
            set { _RootProdWelder1Image = value; RaisePropertyChanged(); }
        }
        private string _RootProdWelder2Image;
        public string RootProdWelder2Image
        {
            get { return _RootProdWelder2Image; }
            set { _RootProdWelder2Image = value; RaisePropertyChanged(); }
        }
        private string _RootProdWelder3Image;
        public string RootProdWelder3Image
        {
            get { return _RootProdWelder3Image; }
            set { _RootProdWelder3Image = value; RaisePropertyChanged(); }
        }
        private string _RootProdWelder4Image;
        public string RootProdWelder4Image
        {
            get { return _RootProdWelder4Image; }
            set { _RootProdWelder4Image = value; RaisePropertyChanged(); }
        }


        private ObservableCollection<T_Welders> _FillCapWelderList;
        public ObservableCollection<T_Welders> FillCapWelderList
        {
            get { return _FillCapWelderList; }
            set { _FillCapWelderList = value; RaisePropertyChanged(); }
        }
        private T_Welders _SelectedFillCapWelder1;
        public T_Welders SelectedFillCapWelder1
        {
            get { return _SelectedFillCapWelder1; }
            set
            {
                if (SetProperty(ref _SelectedFillCapWelder1, value))
                {
                    RaisePropertyChanged("SelectedFillCapWelder1");
                    OnPropertyChanged();
                }
            }
        }
        private T_Welders _SelectedFillCapWelder2;
        public T_Welders SelectedFillCapWelder2
        {
            get { return _SelectedFillCapWelder2; }
            set
            {
                if (SetProperty(ref _SelectedFillCapWelder2, value))
                {
                    RaisePropertyChanged("SelectedFillCapWelder2");
                    OnPropertyChanged();
                }
            }
        }
        private T_Welders _SelectedFillCapWelder3;
        public T_Welders SelectedFillCapWelder3
        {
            get { return _SelectedFillCapWelder3; }
            set
            {
                if (SetProperty(ref _SelectedFillCapWelder3, value))
                {
                    RaisePropertyChanged("SelectedFillCapWelder3");
                    OnPropertyChanged();
                }
            }
        }
        private T_Welders _SelectedFillCapWelder4;
        public T_Welders SelectedFillCapWelder4
        {
            get { return _SelectedFillCapWelder4; }
            set
            {
                if (SetProperty(ref _SelectedFillCapWelder4, value))
                {
                    RaisePropertyChanged("SelectedFillCapWelder4");
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<T_WeldProcesses> _FillCapWeldProcess;
        public ObservableCollection<T_WeldProcesses> FillCapWeldProcess
        {
            get { return _FillCapWeldProcess; }
            set { _FillCapWeldProcess = value; RaisePropertyChanged(); }
        }
        private T_WeldProcesses _SelectedFillCapWeldProcess;
        public T_WeldProcesses SelectedFillCapWeldProcess
        {
            get { return _SelectedFillCapWeldProcess; }
            set
            {
                if (SetProperty(ref _SelectedFillCapWeldProcess, value))
                {
                    RaisePropertyChanged("SelectedFillCapWeldProcess");
                    OnPropertyChanged();
                }
            }
        }
        private string _FillCapProdWelder1Image;
        public string FillCapProdWelder1Image
        {
            get { return _FillCapProdWelder1Image; }
            set { _FillCapProdWelder1Image = value; RaisePropertyChanged(); }
        }
        private string _FillCapProdWelder2Image;
        public string FillCapProdWelder2Image
        {
            get { return _FillCapProdWelder2Image; }
            set { _FillCapProdWelder2Image = value; RaisePropertyChanged(); }
        }
        private string _FillCapProdWelder3Image;
        public string FillCapProdWelder3Image
        {
            get { return _FillCapProdWelder3Image; }
            set { _FillCapProdWelder3Image = value; RaisePropertyChanged(); }
        }
        private string _FillCapProdWelder4Image;
        public string FillCapProdWelder4Image
        {
            get { return _FillCapProdWelder4Image; }
            set { _FillCapProdWelder4Image = value; RaisePropertyChanged(); }
        }


        private ObservableCollection<T_WPS_MSTR> _WPSNoList;
        public ObservableCollection<T_WPS_MSTR> WPSNoList
        {
            get { return _WPSNoList; }
            set { _WPSNoList = value; RaisePropertyChanged(); }
        }
        private T_WPS_MSTR _SelectedWPSNo;
        public T_WPS_MSTR SelectedWPSNo
        {
            get { return _SelectedWPSNo; }
            set
            {
                if (SetProperty(ref _SelectedWPSNo, value))
                {
                    RaisePropertyChanged("SelectedWPSNo");
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<T_BaseMetal> _BaseMetal1;
        public ObservableCollection<T_BaseMetal> BaseMetal1
        {
            get { return _BaseMetal1; }
            set { _BaseMetal1 = value; RaisePropertyChanged(); }
        }
        private T_BaseMetal _SelectedBaseMetal1;
        public T_BaseMetal SelectedBaseMetal1
        {
            get { return _SelectedBaseMetal1; }
            set
            {
                if (SetProperty(ref _SelectedBaseMetal1, value))
                {
                    RaisePropertyChanged("SelectedBaseMetal1");
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<T_BaseMetal> _BaseMetal2;
        public ObservableCollection<T_BaseMetal> BaseMetal2
        {
            get { return _BaseMetal2; }
            set { _BaseMetal2 = value; RaisePropertyChanged(); }
        }
        private T_BaseMetal _SelectedBaseMetal2;
        public T_BaseMetal SelectedBaseMetal2
        {
            get { return _SelectedBaseMetal2; }
            set
            {
                if (SetProperty(ref _SelectedBaseMetal2, value))
                {
                    RaisePropertyChanged("SelectedBaseMetal2");
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<T_DWR_HeatNos> _HeatNo1;
        public ObservableCollection<T_DWR_HeatNos> HeatNo1
        {
            get { return _HeatNo1; }
            set { _HeatNo1 = value; RaisePropertyChanged(); }
        }
        private T_DWR_HeatNos _SelectedHeatNo1;
        public T_DWR_HeatNos SelectedHeatNo1
        {
            get { return _SelectedHeatNo1; }
            set { _SelectedHeatNo1 = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_DWR_HeatNos> _HeatNo2;
        public ObservableCollection<T_DWR_HeatNos> HeatNo2
        {
            get { return _HeatNo2; }
            set { _HeatNo2 = value; RaisePropertyChanged(); }
        }
        private T_DWR_HeatNos _SelectedHeatNo2;
        public T_DWR_HeatNos SelectedHeatNo2
        {
            get { return _SelectedHeatNo2; }
            set { _SelectedHeatNo2 = value; RaisePropertyChanged(); }
        }
        private List<T_RT_Defects> _VICommentList;
        public List<T_RT_Defects> VICommentList
        {
            get { return _VICommentList; }
            set { _VICommentList = value; RaisePropertyChanged(); }
        }
        //private string lblBaseMetal1;
        //public string LblBaseMetal1
        //{
        //    get { return lblBaseMetal1; }
        //    set { lblBaseMetal1 = value; RaisePropertyChanged(); }
        //}
        //private string lblBaseMetal2;
        //public string LblBaseMetal2
        //{
        //    get { return lblBaseMetal2; }
        //    set { lblBaseMetal2 = value; RaisePropertyChanged(); }
        //}
        //private string lblHeatNo1;
        //public string LblHeatNo1
        //{
        //    get { return lblHeatNo1; }
        //    set { lblHeatNo1 = value; RaisePropertyChanged(); }
        //}
        //private string lblHeatNo2;
        //public string LblHeatNo2
        //{
        //    get { return lblHeatNo2; }
        //    set { lblHeatNo2 = value; RaisePropertyChanged(); }
        //}
        #endregion
        #region Delegate Commands  
        public ICommand OnBtnClickCommand
        {
            get
            {
                return new Command<string>(OnBtnClicked);
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


        public InspectJointViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_Welders> _weldersRepository,
            IRepository<T_RT_Defects> _defectsRepository,
            IRepository<T_WPS_MSTR> _wpsRepository,
            IRepository<T_BaseMetal> _baseMetalRepository,
            IRepository<T_DWR_HeatNos> _DWR_HeatNosRepository,
            IRepository<T_WeldProcesses> _WeldProcessesRepository,
            IRepository<T_DWR> _DWRRepository,
            IRepository<T_EReports> _eReportRepository
           ) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._weldersRepository = _weldersRepository;
            this._defectsRepository = _defectsRepository;
            this._wpsRepository = _wpsRepository;
            this._baseMetalRepository = _baseMetalRepository;
            this._DWR_HeatNosRepository = _DWR_HeatNosRepository;
            this._WeldProcessesRepository = _WeldProcessesRepository;
            this._DWRRepository = _DWRRepository;
            this._eReportRepository = _eReportRepository;
            _userDialogs.HideLoading();
            IsHeaderBtnVisible = true;
            PageHeaderText = "Joint Details";
            DWRHelper.DWRTargetType = typeof(InspectJointViewModel);

            //RootProdWelder1Image = RootProdWelder2Image = RootProdWelder3Image = RootProdWelder4Image = FillCapProdWelder1Image = FillCapProdWelder2Image = FillCapProdWelder3Image = FillCapProdWelder4Image = "Greenradio.png";
        }
        private async void OnBtnClicked(string param)
        {
            if (param == "BackBtn")
            {
                await navigationService.NavigateFromMenuAsync(typeof(JointDetailsViewModel));
            }
            else if (param == "ViewDrawingBtn")
            {
                var navigationParameters = new NavigationParameters();
                DWRHelper.SelectedDWR = SelectedDWRReport;
                navigationParameters.Add("id", SelectedDWRReport);
                await navigationService.NavigateFromMenuAsync(typeof(DWRDrawingViewModel), navigationParameters);
            }
            else if (param == "JointDetailsBtn")
            {
                await PopupNavigation.PushAsync(new DWRPopup("JointDetails"), true);
            }
            else if (param == "InspectionResultBtn")
            {
                await PopupNavigation.PushAsync(new DWRPopup("InspectionResult"), true);
            }
            else if(param == "SaveBtn")
            {
                    Save("SaveBtn");
            }
            else if (param == "SaveNextBtn")
            {
                    Save("SaveNextBtn"); 
            }
            else if (param == "RootProdWelder1")
            {
                if (RootProdWelder1Image == "Greenradio.png")
                {
                    SelectedDWRReport.RootWelder1ProductionJoint = false;
                    RootProdWelder1Image = "Redradio.png";
                }
                else
                {
                    SelectedDWRReport.RootWelder1ProductionJoint = true;
                    RootProdWelder1Image = "Greenradio.png";
                }
            }
            else if (param == "RootProdWelder2")
            {
                if (RootProdWelder2Image == "Greenradio.png")
                {
                    SelectedDWRReport.RootWelder2ProductionJoint = false;
                    RootProdWelder2Image = "Redradio.png";
                }
                else
                {
                    SelectedDWRReport.RootWelder2ProductionJoint = true;
                    RootProdWelder2Image = "Greenradio.png";
                }
            }
            else if (param == "RootProdWelder3")
            {
                if (RootProdWelder3Image == "Greenradio.png")
                {
                    SelectedDWRReport.RootWelder3ProductionJoint = false;
                    RootProdWelder3Image = "Redradio.png";
                }
                else
                {
                    SelectedDWRReport.RootWelder3ProductionJoint = true;
                    RootProdWelder3Image = "Greenradio.png";
                }
            }
            else if (param == "RootProdWelder4")
            {
                if (RootProdWelder4Image == "Greenradio.png")
                {
                    SelectedDWRReport.RootWelder4ProductionJoint = false;
                    RootProdWelder4Image = "Redradio.png";
                }
                else
                {
                    SelectedDWRReport.RootWelder4ProductionJoint = true;
                    RootProdWelder4Image = "Greenradio.png";
                }
            }
            else if (param == "FillCapProdWelder1")
            {
                if (FillCapProdWelder1Image == "Greenradio.png")
                {
                    SelectedDWRReport.FillCapWelder1ProductionJoint = false;
                    FillCapProdWelder1Image = "Redradio.png";
                }
                else
                {
                   SelectedDWRReport.FillCapWelder1ProductionJoint = true;
                    FillCapProdWelder1Image = "Greenradio.png";
                }
            }
            else if (param == "FillCapProdWelder2")
            {
                if (FillCapProdWelder2Image == "Greenradio.png")
                {
                    SelectedDWRReport.FillCapWelder2ProductionJoint = false;
                    FillCapProdWelder2Image = "Redradio.png";
                }
                else
                {
                    SelectedDWRReport.FillCapWelder2ProductionJoint = true;
                    FillCapProdWelder2Image = "Greenradio.png";
                }
            }
            else if (param == "FillCapProdWelder3")
            {
                if (FillCapProdWelder3Image == "Greenradio.png")
                {
                    SelectedDWRReport.FillCapWelder3ProductionJoint = false;
                    FillCapProdWelder3Image = "Redradio.png";
                }
                else
                {
                    SelectedDWRReport.FillCapWelder3ProductionJoint = true;
                    FillCapProdWelder3Image = "Greenradio.png";
                }
            }
            else if (param == "FillCapProdWelder4")
            {
                if (FillCapProdWelder4Image == "Greenradio.png")
                {
                    SelectedDWRReport.FillCapWelder4ProductionJoint = false;
                    FillCapProdWelder4Image = "Redradio.png";
                }
                else
                {
                    SelectedDWRReport.FillCapWelder4ProductionJoint = false;
                    FillCapProdWelder4Image = "Greenradio.png";
                }
            }

        }
        [Obsolete]
        private async void OnTapedAsync(string param)
        {
            switch (param)
            {
                case "SelectBaseMetal1":
                    if (BaseMetal1 == null) return;
                    var BaseMetal1List = BaseMetal1.Select(x => x.Base_Material).Distinct().ToList();
                    var BM1 = await GetPopup(BaseMetal1List);
                    if (!string.IsNullOrWhiteSpace(BM1) && BM1 != "clear")
                    {
                       T_BaseMetal BMetal = BaseMetal1.Where( x=>x.Base_Material == BM1).FirstOrDefault();
                        if(BMetal == null)
                          await CreateNewBaseMetal(BM1, "BaseMetal1");
                        else
                          SelectedBaseMetal1 = BMetal;
                    }
                    else
                        SelectedBaseMetal1 = BaseMetal1.FirstOrDefault();
                    break;
                case "SelectBaseMetal2":
                    if (BaseMetal2 == null) return;
                    var BaseMetal2List = BaseMetal2.Select(x => x.Base_Material).Distinct().ToList();
                    var BM2 = await GetPopup(BaseMetal2List);
                    if (!string.IsNullOrWhiteSpace(BM2) && BM2 != "clear")
                    {
                        T_BaseMetal BMetal = BaseMetal2.Where(x => x.Base_Material == BM2).FirstOrDefault();
                        if (BMetal == null)
                            await CreateNewBaseMetal(BM2, "BaseMetal2");
                        else
                            SelectedBaseMetal2 = BMetal;
                    }
                    else
                        SelectedBaseMetal2 = BaseMetal2.FirstOrDefault();
                    break;
                case "SelectHeatNo1":
                    if (HeatNo1 == null) return;
                    var HeatNo1List = HeatNo1.Select(x => x.Heat_No).Distinct().ToList();
                    var HN1 = await GetPopup(HeatNo1List);
                    if (!string.IsNullOrWhiteSpace(HN1) && HN1 != "clear")
                    {
                        T_DWR_HeatNos HeatItem = HeatNo1.Where(x => x.Heat_No == HN1).FirstOrDefault();
                        if (HeatItem == null)
                            await CreateNewHeatNo(HN1, "HeatNo1");
                        else
                            SelectedHeatNo1 = HeatItem;
                    }
                    else
                        SelectedHeatNo1 = HeatNo1.FirstOrDefault();

                    break;
                case "SelectHeatNo2":
                    if (HeatNo2== null) return;
                    var HeatNo2List = HeatNo2.Select(x => x.Heat_No).Distinct().ToList();
                    var HN2 = await GetPopup(HeatNo2List);
                    if (!string.IsNullOrWhiteSpace(HN2) && HN2 != "clear")
                    {
                        T_DWR_HeatNos HeatItem = HeatNo2.Where(x => x.Heat_No == HN2).FirstOrDefault();
                        if (HeatItem == null)
                            await CreateNewHeatNo(HN2, "HeatNo2");
                        else
                            SelectedHeatNo2 = HeatItem;
                    }
                    else
                        SelectedHeatNo2 = HeatNo2.FirstOrDefault();
                    break;

            }
        }
        [Obsolete]
        public static Task<string> GetPopup(List<string> Source)
        {
            var viewModel = new DropDownEntryViewModel(Source);
           // viewModel.FilterList = Source;
            var tcs = new TaskCompletionSource<string>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var page = new DropDownEntryPopup(viewModel);
                await PopupNavigation.PushAsync(page);
                var value = await viewModel.GetValue();
                await PopupNavigation.PopAsync(true);
                tcs.SetResult(value);
            });
            return tcs.Task;
        }
        private async Task CreateNewBaseMetal(string BaseMetalValue, string BindParam)
        {
            T_BaseMetal NewBaseMetal = new T_BaseMetal { Base_Material = BaseMetalValue, Updated = true };
            await Task.Run( async () => await _baseMetalRepository.InsertOrReplaceAsync(NewBaseMetal));
            var BaseMetals = await _baseMetalRepository.QueryAsync<T_BaseMetal>(@"SELECT * FROM T_BaseMetal");
            BaseMetals.Insert(0, new T_BaseMetal() { Base_Material = "- Select -" });
            BaseMetal1 = BaseMetal2 = new ObservableCollection<T_BaseMetal>(BaseMetals);
            if(BindParam == "BaseMetal1")
               SelectedBaseMetal1 = NewBaseMetal;
            else
               SelectedBaseMetal2 = NewBaseMetal;

        }
        private async Task CreateNewHeatNo(string HeatNoValue, string BindParam)
        {
            T_DWR_HeatNos NewHeatNo = new T_DWR_HeatNos { Project_ID = Settings.ProjectID, Ident_Code = BindParam == "HeatNo1" ?SelectedDWRReport.IdentCode1 : SelectedDWRReport.IdentCode2, Heat_No = HeatNoValue, Updated =true };
            await Task.Run(async () => await _DWR_HeatNosRepository.InsertOrReplaceAsync(NewHeatNo));
            var HeatNos = await _DWR_HeatNosRepository.QueryAsync<T_DWR_HeatNos>("SELECT * FROM [T_DWR_HeatNos] WHERE [PROJECT_ID] = " + Settings.ProjectID + " ORDER BY [HEAT_NO] ASC");
            HeatNos.Insert(0, new T_DWR_HeatNos() { Heat_No = "- Select -" });
            if (!string.IsNullOrEmpty(SelectedDWRReport.IdentCode1))
                HeatNo1 = new ObservableCollection<T_DWR_HeatNos>(HeatNos.Where(x => x.Ident_Code == SelectedDWRReport.IdentCode1.Replace("'", "''")));
            else
                HeatNo1 = new ObservableCollection<T_DWR_HeatNos>(HeatNos);
            if (!string.IsNullOrEmpty(SelectedDWRReport.IdentCode2))
                HeatNo2 = new ObservableCollection<T_DWR_HeatNos>(HeatNos.Where(x => x.Ident_Code == SelectedDWRReport.IdentCode2.Replace("'", "''")));
            else
                HeatNo2 = new ObservableCollection<T_DWR_HeatNos>(HeatNos);

            if (BindParam == "HeatNo1")
                SelectedHeatNo1 = NewHeatNo;
            else
                SelectedHeatNo2 = NewHeatNo;

        }
        private async void BindAllDWRControl(bool SaveAndNextClicked)
        { 
            //Load Welders
            var WelderList = await _weldersRepository.QueryAsync<T_Welders>(@"SELECT * FROM T_Welders");
            //var WelderList = Welders.Select(x => new Welder() { Welder_Name = x.Welder_Name, Project_ID = x.Project_ID, SubContractor = x.SubContractor, Welder_Code = x.Welder_Code }).ToList();
            WelderList.Insert(0, new T_Welders() { Welder_Name = "Select -" , });
            RootWelderList = FillCapWelderList = new ObservableCollection<T_Welders>(WelderList);

            //Load WeldProcesses
            var WeldProcesses = await _WeldProcessesRepository.QueryAsync(@"SELECT * FROM T_WeldProcesses");
            WeldProcesses.Insert(0, new T_WeldProcesses() { Description = "Select -" });
            RootWeldProcess = FillCapWeldProcess = new ObservableCollection<T_WeldProcesses>(WeldProcesses);

            //Load WPS No.
            var WPSNoData = await _wpsRepository.QueryAsync<T_WPS_MSTR>(@"SELECT * FROM T_WPS_MSTR");
            //var WPSNoData = WpsNos.Select(x => new WPS() { Wps_No = x.Wps_No, Description = x.Description, Project_ID = x.Project_ID }).ToList();
            WPSNoData.Insert(0, new T_WPS_MSTR() { Description = "Select -" });
            WPSNoList = new ObservableCollection<T_WPS_MSTR>(WPSNoData);

            //Load BaseMetals 
            var BaseMetals = await _baseMetalRepository.QueryAsync<T_BaseMetal>(@"SELECT * FROM T_BaseMetal");
            BaseMetals = BaseMetals.Where(x => x.Base_Material != "***" && x.Base_Material != "-").ToList();
            BaseMetals.Insert(0, new T_BaseMetal() { Base_Material = "- Select -"});
            BaseMetal1 = BaseMetal2 = new ObservableCollection<T_BaseMetal>(BaseMetals);

            //Load HeatNos
            var HeatNos = await _DWR_HeatNosRepository.QueryAsync<T_DWR_HeatNos>("SELECT * FROM [T_DWR_HeatNos] WHERE [PROJECT_ID] = " + Settings.ProjectID + " ORDER BY [HEAT_NO] ASC");
            HeatNos.Insert(0, new T_DWR_HeatNos() { Heat_No = "- Select -" });
            if (!string.IsNullOrEmpty(SelectedDWRReport.IdentCode1))
                HeatNo1  = new ObservableCollection<T_DWR_HeatNos>(HeatNos.Where(x=>x.Ident_Code == SelectedDWRReport.IdentCode1.Replace("'", "''")));
            else
                HeatNo1  = new ObservableCollection<T_DWR_HeatNos>(HeatNos);
            if (!string.IsNullOrEmpty(SelectedDWRReport.IdentCode2))
                HeatNo2 = new ObservableCollection<T_DWR_HeatNos>(HeatNos.Where(x => x.Ident_Code == SelectedDWRReport.IdentCode2.Replace("'", "''")));
            else
                HeatNo2 = new ObservableCollection<T_DWR_HeatNos>(HeatNos);

            //Load InspectionResult
            var VIComments = await _defectsRepository.QueryAsync("SELECT * FROM [T_RT_Defects]");
            VIComments.Insert(0, new T_RT_Defects() { Description = "Select -" });
            VICommentList = VIComments.ToList();

            //
            if(!SaveAndNextClicked)
            AFI_Number = SelectedDWRReport.AFINo;

            Revision_No = SelectedDWRReport.RevNo;
            Spool_Drawing_Number = SelectedDWRReport.SpoolDrawingNo;
            Joint_No = SelectedDWRReport.JointNo;
            if (SelectedDWRReport.WeldedDate == Convert.ToDateTime("01/01/0001 0:00") || SelectedDWRReport.WeldedDate == Convert.ToDateTime("01/01/2000 0:00")
                    || SelectedDWRReport.WeldedDate == Convert.ToDateTime("01/01/1900 0:00"))
                Welded_Date = DateTime.Now;
            else
            Welded_Date = SelectedDWRReport.WeldedDate;

            if (SelectedDWRReport.FitUpDate == Convert.ToDateTime("01/01/0001 0:00") || SelectedDWRReport.FitUpDate == Convert.ToDateTime("01/01/2000 0:00")
                    || SelectedDWRReport.FitUpDate == Convert.ToDateTime("01/01/1900 0:00"))
                FitUpDate = DateTime.Now;
            else
                FitUpDate = SelectedDWRReport.FitUpDate;

            //Root 
            if (!String.IsNullOrEmpty(SelectedDWRReport.RootWelder1))
                SelectedRootWelder1 = RootWelderList.Where(x => x.Welder_Code == SelectedDWRReport.RootWelder1).FirstOrDefault();
            else
                SelectedRootWelder1 = RootWelderList.FirstOrDefault();

            if (!String.IsNullOrEmpty(SelectedDWRReport.RootWelder2))
                SelectedRootWelder2 = RootWelderList.Where(x => x.Welder_Code == SelectedDWRReport.RootWelder2).FirstOrDefault();
            else
                SelectedRootWelder2 = RootWelderList.FirstOrDefault();

            if (!String.IsNullOrEmpty(SelectedDWRReport.RootWelder3))
                SelectedRootWelder3 = RootWelderList.Where(x => x.Welder_Code == SelectedDWRReport.RootWelder3).FirstOrDefault();
            else
                SelectedRootWelder3 = RootWelderList.FirstOrDefault();

            if (!String.IsNullOrEmpty(SelectedDWRReport.RootWelder4))
                SelectedRootWelder4 = RootWelderList.Where(x => x.Welder_Code == SelectedDWRReport.RootWelder4).FirstOrDefault();
            else
                SelectedRootWelder4 = RootWelderList.FirstOrDefault();

            if (!String.IsNullOrEmpty(SelectedDWRReport.RootWeldProcess))
                SelectedRootWeldProcess = RootWeldProcess.Where(x => x.Weld_Process == SelectedDWRReport.RootWeldProcess).FirstOrDefault();
            else
                SelectedRootWeldProcess = RootWeldProcess.FirstOrDefault();

            if (SelectedDWRReport.RootWelder1ProductionJoint)
                RootProdWelder1Image = "Greenradio.png";
            else
                RootProdWelder1Image = "Redradio.png";
            if (SelectedDWRReport.RootWelder2ProductionJoint)
                RootProdWelder2Image = "Greenradio.png";
            else
                RootProdWelder2Image = "Redradio.png";
            if (SelectedDWRReport.RootWelder3ProductionJoint)
                RootProdWelder3Image = "Greenradio.png";
            else
                RootProdWelder3Image = "Redradio.png";
            if (SelectedDWRReport.RootWelder4ProductionJoint)
                RootProdWelder4Image = "Greenradio.png";
            else
                RootProdWelder4Image = "Redradio.png";

            //FillCap
            if (!String.IsNullOrEmpty(SelectedDWRReport.FillCapWelder1))
                SelectedFillCapWelder1 = FillCapWelderList.Where(x => x.Welder_Code == SelectedDWRReport.FillCapWelder1).FirstOrDefault();
            else
                SelectedFillCapWelder1 = FillCapWelderList.FirstOrDefault();

            if (!String.IsNullOrEmpty(SelectedDWRReport.FillCapWelder2))
                SelectedFillCapWelder2 = FillCapWelderList.Where(x => x.Welder_Code == SelectedDWRReport.FillCapWelder2).FirstOrDefault();
            else
                SelectedFillCapWelder2 = FillCapWelderList.FirstOrDefault();

            if (!String.IsNullOrEmpty(SelectedDWRReport.FillCapWelder3))
                SelectedFillCapWelder3 = FillCapWelderList.Where(x => x.Welder_Code == SelectedDWRReport.FillCapWelder3).FirstOrDefault();
            else
                SelectedFillCapWelder3 = FillCapWelderList.FirstOrDefault();

            if (!String.IsNullOrEmpty(SelectedDWRReport.FillCapWelder4))
                SelectedFillCapWelder4 = FillCapWelderList.Where(x => x.Welder_Code == SelectedDWRReport.FillCapWelder4).FirstOrDefault();
            else
                SelectedFillCapWelder4 = FillCapWelderList.FirstOrDefault();

            if (!String.IsNullOrEmpty(SelectedDWRReport.FillCapWeldProcess))
                SelectedFillCapWeldProcess = FillCapWeldProcess.Where(x => x.Weld_Process == SelectedDWRReport.FillCapWeldProcess).FirstOrDefault();
            else
                SelectedFillCapWeldProcess = FillCapWeldProcess.FirstOrDefault();

            if (SelectedDWRReport.FillCapWelder1ProductionJoint)
                FillCapProdWelder1Image = "Greenradio.png";
            else
                FillCapProdWelder1Image = "Redradio.png";
            if (SelectedDWRReport.FillCapWelder2ProductionJoint)
                FillCapProdWelder2Image = "Greenradio.png";
            else
                FillCapProdWelder2Image = "Redradio.png";
            if (SelectedDWRReport.FillCapWelder3ProductionJoint)
                FillCapProdWelder3Image = "Greenradio.png";
            else
                FillCapProdWelder3Image = "Redradio.png";
            if (SelectedDWRReport.FillCapWelder4ProductionJoint)
                FillCapProdWelder4Image = "Greenradio.png";
            else
                FillCapProdWelder4Image = "Redradio.png";

            if (!String.IsNullOrEmpty(SelectedDWRReport.WPSNo))
                SelectedWPSNo = WPSNoList.Where(x => x.Wps_No == SelectedDWRReport.WPSNo).FirstOrDefault();
            else
                SelectedWPSNo = WPSNoList.FirstOrDefault();

            if (!String.IsNullOrEmpty(SelectedDWRReport.BaseMetal1))
            {
                SelectedBaseMetal1 = BaseMetal1.Where(x => x.Base_Material == SelectedDWRReport.BaseMetal1).FirstOrDefault();
                if (SelectedBaseMetal1 == null)
                await CreateNewBaseMetal(SelectedDWRReport.BaseMetal1, "BaseMetal1");
            }
            else
                SelectedBaseMetal1 = BaseMetal1.FirstOrDefault();

            if (!String.IsNullOrEmpty(SelectedDWRReport.BaseMetal2))
            {
                SelectedBaseMetal2 = BaseMetal2.Where(x => x.Base_Material == SelectedDWRReport.BaseMetal2).FirstOrDefault();
                if (SelectedBaseMetal2 == null)
                await CreateNewBaseMetal(SelectedDWRReport.BaseMetal2, "BaseMetal2");
            }
            else
                SelectedBaseMetal2 = BaseMetal2.FirstOrDefault();

            if (!String.IsNullOrEmpty(SelectedDWRReport.HeatNo1))
            {
                SelectedHeatNo1 = HeatNo1.Where(x => x.Heat_No == SelectedDWRReport.HeatNo1).FirstOrDefault();
                if (SelectedHeatNo1 == null)
                    await CreateNewHeatNo(SelectedDWRReport.HeatNo1, "HeatNo1");
            }
            else
                SelectedHeatNo1 = HeatNo1.FirstOrDefault();

            if (!String.IsNullOrEmpty(SelectedDWRReport.HeatNo2))
            {
                SelectedHeatNo2 = HeatNo2.Where(x => x.Heat_No == SelectedDWRReport.HeatNo2).FirstOrDefault();
                if (SelectedHeatNo2 == null)
                    await CreateNewHeatNo(SelectedDWRReport.HeatNo2, "HeatNo2");
            }
            else
                SelectedHeatNo2 = HeatNo2.FirstOrDefault();

            //JointDeails
            JointDetail.SheetNo = SelectedDWRReport.SheetNo;
            JointDetail.Size = SelectedDWRReport.Size;
            JointDetail.Thickness = SelectedDWRReport.Thickness;
            JointDetail.SpoolNo = SelectedDWRReport.SpoolNo;
            JointDetail.Weldtype = SelectedDWRReport.WeldType;
            JointDetail.LineClass = SelectedDWRReport.LineClass;
            EReporterHelper.JointDetailFields = JointDetail;

            //InspectResult
            InspectResult.VI = !String.IsNullOrEmpty(SelectedDWRReport.VI) ? SelectedDWRReport.VI : "- Select -";
            InspectResult.DI = !String.IsNullOrEmpty(SelectedDWRReport.DI) ? SelectedDWRReport.DI : "- Select -";
            InspectResult.VI_Comment = !String.IsNullOrEmpty(SelectedDWRReport.VIComment) ? VICommentList.Where(x=>x.Description == SelectedDWRReport.VIComment).FirstOrDefault() : VICommentList.FirstOrDefault(); 
            InspectResult.DI_Comment = !String.IsNullOrEmpty(SelectedDWRReport.DIComment) ? SelectedDWRReport.DIComment : "";
            InspectResult.Remark = !String.IsNullOrEmpty(SelectedDWRReport.Remarks)? SelectedDWRReport.Remarks : "";
            InspectResult.VICommentList = VICommentList;
            InspectResult.DWRID = SelectedDWRReport.ID.ToString();
            InspectResult.RowID = SelectedDWRReport.RowID.ToString();
            InspectResult.JobCode = SelectedDWRReport.JobCode;
            EReporterHelper.InspectResultFields = InspectResult;
        }
        private async void Save(string BtnPressed)
        {
            try
            {
                if (await _userDialogs.ConfirmAsync($"Are you sure you want to Save Data?", $"Save DWR", "Yes", "No"))
                {
                    string RootWelder1 = SelectedRootWelder1.ToString() != " - Select -" ? SelectedRootWelder1.Welder_Code : "";
                    string RootWelder2 = SelectedRootWelder2.ToString() != " - Select -" ? SelectedRootWelder2.Welder_Code : "";
                    string RootWelder3 = SelectedRootWelder3.ToString() != " - Select -" ? SelectedRootWelder3.Welder_Code : "";
                    string RootWelder4 = SelectedRootWelder4.ToString() != " - Select -" ? SelectedRootWelder4.Welder_Code : "";
                    string RootWeldProcess = SelectedRootWeldProcess.ToString() != " - Select -" ? SelectedRootWeldProcess.Weld_Process : "";

                    string FillCapWelder1 = SelectedFillCapWelder1.ToString() != " - Select -" ? SelectedFillCapWelder1.Welder_Code : "";
                    string FillCapWelder2 = SelectedFillCapWelder2.ToString() != " - Select -" ? SelectedFillCapWelder2.Welder_Code : "";
                    string FillCapWelder3 = SelectedFillCapWelder3.ToString() != " - Select -" ? SelectedFillCapWelder3.Welder_Code : "";
                    string FillCapWelder4 = SelectedFillCapWelder4.ToString() != " - Select -" ? SelectedFillCapWelder4.Welder_Code : "";
                    string FillCapWeldProcess = SelectedFillCapWeldProcess.ToString() != " - Select -" ? SelectedFillCapWeldProcess.Weld_Process : "";

                    string WPS = SelectedWPSNo.ToString() != " - Select -" ? SelectedWPSNo.Wps_No : "";
                    string BaseMetal1 = SelectedBaseMetal1.ToString() != "- Select -" ? SelectedBaseMetal1.Base_Material : "";
                    string BaseMetal2 = SelectedBaseMetal2.ToString() != "- Select -" ? SelectedBaseMetal2.Base_Material : "";
                    string HeatNo1 = SelectedHeatNo1.ToString() != "- Select -" ? SelectedHeatNo1.Heat_No : "";
                    string HeatNo2 = SelectedHeatNo2.ToString() != "- Select -" ? SelectedHeatNo2.Heat_No : "";

                    string PCWBS = "A120";

                    if (String.IsNullOrEmpty(AFI_Number) || String.IsNullOrEmpty(RootWelder1) || String.IsNullOrEmpty(FillCapWelder1) || String.IsNullOrEmpty(RootWeldProcess) || String.IsNullOrEmpty(FillCapWeldProcess) ||
                      String.IsNullOrEmpty(WPS) || String.IsNullOrEmpty(BaseMetal1) || String.IsNullOrEmpty(BaseMetal2) || String.IsNullOrEmpty(HeatNo1) || String.IsNullOrEmpty(HeatNo2))
                    {
                        await _userDialogs.AlertAsync("Required fields are AFI Number, RootWelder1, FillCapWelder1, RootWeldProcess, FillCapWeldProcess, WPS, BaseMetal1, BaseMetal2, HeatNo1 and HeatNo2", null, "Ok");
                        return;
                    }

                    string Vi = EReporterHelper.InspectResultFields.VI != "- Select -" ? EReporterHelper.InspectResultFields.VI : "";
                    string Di = EReporterHelper.InspectResultFields.DI != "- Select -" ? EReporterHelper.InspectResultFields.DI : "";
                    string ViComment = EReporterHelper.InspectResultFields.VI_Comment.ToString() != " - Select -" ? EReporterHelper.InspectResultFields.VI_Comment.Description : "";
                    string DiComment = EReporterHelper.InspectResultFields.DI_Comment != "- Select -" ? EReporterHelper.InspectResultFields.DI_Comment : "";
                    bool Reject;
                    if ((Vi == "REJ" || Di == "REJ") && SelectedDWRReport.RejectedByUserID == 0 )
                    {
                        SelectedDWRReport.RejectedByUserID = Settings.UserID;
                        SelectedDWRReport.RejectedOn = DateTime.Now;
                    }

                    string SaveQuery = string.Empty; string ERSaveQuery = string.Empty;
                    SaveQuery = @"Update [T_DWR] SET [AFINo] = '" + AFI_Number + "',[RevNo] = '" + Revision_No + "',[SpoolDrawingNo] = '" + Spool_Drawing_Number + "',[JointNo] = '" + Joint_No + "'"
                              + ",[WeldedDate] = '" + Welded_Date.Ticks + "',[FitUpDate] = '" + FitUpDate.Ticks + "',[RootWelder1] = '" + RootWelder1 + "',[RootWelder2] = '" + RootWelder2 + "',[RootWelder3] = '" + RootWelder3 + "',[RootWelder4] ='" + RootWelder4 + "'"
                              + ",[FillCapWelder1] = '" + FillCapWelder1 + "',[FillCapWelder2] = '" + FillCapWelder2 + "',[FillCapWelder3] = '" + FillCapWelder3 + "',[FillCapWelder4] ='" + FillCapWelder4 + "'"
                              + ",[RootWeldProcess] = '" + RootWeldProcess + "',[FillCapWeldProcess] = '" + FillCapWeldProcess + "'"
                              + ", [RootWelder1ProductionJoint] = '" + Convert.ToInt32(SelectedDWRReport.RootWelder1ProductionJoint) + "',[RootWelder2ProductionJoint] = '" + Convert.ToInt32(SelectedDWRReport.RootWelder2ProductionJoint) + "'"
                              + ",[RootWelder3ProductionJoint] = '" + Convert.ToInt32(SelectedDWRReport.RootWelder3ProductionJoint) + "',[RootWelder4ProductionJoint] ='" + Convert.ToInt32(SelectedDWRReport.RootWelder4ProductionJoint) + "'"
                              + ", [FillCapWelder1ProductionJoint] = '" + Convert.ToInt32(SelectedDWRReport.FillCapWelder1ProductionJoint) + "',[FillCapWelder2ProductionJoint] = '" + Convert.ToInt32(SelectedDWRReport.FillCapWelder2ProductionJoint) + "'"
                              + ",[FillCapWelder3ProductionJoint] = '" + Convert.ToInt32(SelectedDWRReport.FillCapWelder3ProductionJoint) + "',[FillCapWelder4ProductionJoint] ='" + Convert.ToInt32(SelectedDWRReport.FillCapWelder4ProductionJoint) + "'"
                              + ",[WPSNo] = '" + WPS + "',[BaseMetal1] = '" + BaseMetal1 + "',[BaseMetal2] = '" + BaseMetal2 + "',[HeatNo1] ='" + HeatNo1 + "',[HeatNo2] ='" + HeatNo2 + "'"
                              + ",[SheetNo] = '" + EReporterHelper.JointDetailFields.SheetNo + "',[Size] = '" + EReporterHelper.JointDetailFields.Size + "',[SpoolNo] = '" + EReporterHelper.JointDetailFields.SpoolNo + "'"
                              + ",[Thickness] ='" + EReporterHelper.JointDetailFields.Thickness + "',[WeldType] ='" + EReporterHelper.JointDetailFields.Weldtype + "',[LineClass] ='" + EReporterHelper.JointDetailFields.LineClass + "'"
                              + ",[VI] ='" + Vi + "',[DI] ='" + Di + "',[VIComment] ='" + ViComment + "',[DIComment] ='" + DiComment + "',[RejectedByUserID] ='" + SelectedDWRReport.RejectedByUserID + "'"
                              + ",[RejectedOn] ='" + SelectedDWRReport.RejectedOn.Ticks + "', [Remarks] ='" + EReporterHelper.InspectResultFields.Remark + "',[PCWBS]='" + PCWBS + "', [Updated] = '1'"
                              + " WHERE [ID] ='" + SelectedDWRReport.ID + "' AND ProjectID = '" + SelectedDWRReport.ProjectID + "' AND SpoolDrawingNo = '" + SelectedDWRReport.SpoolDrawingNo
                              + "' AND TestPackage='" + SelectedDWRReport.TestPackage + "' AND JointNo='" + SelectedDWRReport.JointNo + "'";


                    await _DWRRepository.QueryAsync(SaveQuery);

                    ERSaveQuery = @"Update [T_EReports] SET [AFINo] = '" + AFI_Number + "',[PCWBS] = '" + PCWBS + "' WHERE [ID] ='" + SelectedDWRReport.ID + "' AND RowID = '" + SelectedDWRReport.RowID + "'";
                    await _eReportRepository.QueryAsync(ERSaveQuery);

                    await _userDialogs.AlertAsync("Successfully saved..!", null, "Ok");
                    if (BtnPressed == "SaveNextBtn")
                    {
                        var DWRsList = await _DWRRepository.GetAsync(i => i.ProjectID == Settings.ProjectID);
                        List<long> DWR_IDs = DWRsList.ToList().Select(i => i.RowID).ToList();
                        int index = DWR_IDs.IndexOf(SelectedDWRReport.RowID);
                        index++;
                        if (index <= DWR_IDs.Count() - 1)
                        {
                            long NextDWRID = DWR_IDs[index];

                            //Clear();
                            var data = await _DWRRepository.GetAsync(i => i.RowID == NextDWRID);
                            SelectedDWRReport = data.FirstOrDefault();
                            DWRHelper.SelectedDWR = SelectedDWRReport;
                            BindAllDWRControl(true);
                        }
                        else
                        {
                            await _userDialogs.AlertAsync("This is last DWR from the downloaded list", null, "Ok");
                        }
                    }
                    else
                    {
                        var data = await _DWRRepository.GetAsync(i => i.RowID == SelectedDWRReport.RowID);
                        DWRHelper.SelectedDWR = data.FirstOrDefault();
                        await navigationService.NavigateFromMenuAsync(typeof(DWRControlLogViewModel));
                    }
                }
            }
            catch(Exception ex)
            {
                
            }
        }
        private async void Clear()
        {
            AFI_Number = Revision_No = Spool_Drawing_Number = Joint_No = string.Empty;
            Welded_Date = FitUpDate = DateTime.Now;

            //Root 
            SelectedRootWelder1 = RootWelderList.FirstOrDefault();
            SelectedRootWelder2 = RootWelderList.FirstOrDefault();
            SelectedRootWelder3 = RootWelderList.FirstOrDefault();
            SelectedRootWelder4 = RootWelderList.FirstOrDefault();
            SelectedRootWeldProcess = RootWeldProcess.FirstOrDefault();
            RootProdWelder1Image = "Redradio.png";
            RootProdWelder2Image = "Redradio.png";
            RootProdWelder3Image = "Redradio.png";
            RootProdWelder4Image = "Greenradio.png";
            RootProdWelder4Image = "Redradio.png";

            //FillCap
            SelectedFillCapWelder1 = FillCapWelderList.FirstOrDefault();
            SelectedFillCapWelder2 = FillCapWelderList.FirstOrDefault();
            SelectedFillCapWelder3 = FillCapWelderList.FirstOrDefault();
            SelectedFillCapWelder4 = FillCapWelderList.FirstOrDefault();
            FillCapProdWelder1Image = "Redradio.png";
            FillCapProdWelder2Image = "Redradio.png";
            FillCapProdWelder3Image = "Redradio.png";
            FillCapProdWelder4Image = "Redradio.png";

           SelectedWPSNo = WPSNoList.FirstOrDefault();
           SelectedBaseMetal1 = BaseMetal1.FirstOrDefault();
           SelectedBaseMetal2 = BaseMetal2.FirstOrDefault();
           SelectedHeatNo1 = HeatNo1.FirstOrDefault();
           SelectedHeatNo2 = HeatNo2.FirstOrDefault();

            //JointDeails
            JointDetail.SheetNo = JointDetail.Size = JointDetail.Thickness = JointDetail.SpoolNo = JointDetail.Weldtype = JointDetail.LineClass = string.Empty;
            EReporterHelper.JointDetailFields = JointDetail;

            //InspectResult
            InspectResult.VI = "- Select -";
            InspectResult.DI = "- Select -";
            InspectResult.VI_Comment = VICommentList.FirstOrDefault(); 
            InspectResult.DI_Comment = string.Empty;
            InspectResult.Remark = string.Empty;
            InspectResult.VICommentList = VICommentList;
            InspectResult.DWRID = "0";
            InspectResult.JobCode= Settings.JobCode;
            EReporterHelper.InspectResultFields = InspectResult;
        }
        //private async void Create(string BtnPressed)
        //{
        //    try
        //    {
        //        if (await _userDialogs.ConfirmAsync($"Are you sure you want to Save Data?", $"Create new DWR", "Yes", "No"))
        //        {
        //            CreateNewDWR.ID = 0;
        //            CreateNewDWR.ProjectID = Settings.ProjectID;
        //            CreateNewDWR.JobCode = Settings.JobCode;
        //            CreateNewDWR.AFINo = AFI_Number;
        //            CreateNewDWR.RevNo = Revision_No;
        //            CreateNewDWR.SpoolDrawingNo = Spool_Drawing_Number;
        //            CreateNewDWR.JointNo = Joint_No;
        //            CreateNewDWR.WeldedDate = Welded_Date1;

        //            CreateNewDWR.RootWelder1 = SelectedRootWelder1.ToString() != " - Select -" ? SelectedRootWelder1.Welder_Code : "";
        //            CreateNewDWR.RootWelder2 = SelectedRootWelder2.ToString() != " - Select -" ? SelectedRootWelder2.Welder_Code : "";
        //            CreateNewDWR.RootWelder3 = SelectedRootWelder3.ToString() != " - Select -" ? SelectedRootWelder3.Welder_Code : "";
        //            CreateNewDWR.RootWelder4 = SelectedRootWelder4.ToString() != " - Select -" ? SelectedRootWelder4.Welder_Code : "";
        //            CreateNewDWR.RootWeldProcess = SelectedRootWeldProcess.ToString() != " - Select -" ? SelectedRootWeldProcess.Weld_Process : "";

        //            CreateNewDWR.FillCapWelder1 = SelectedFillCapWelder1.ToString() != " - Select -" ? SelectedFillCapWelder1.Welder_Code : "";
        //            CreateNewDWR.FillCapWelder2 = SelectedFillCapWelder2.ToString() != " - Select -" ? SelectedFillCapWelder2.Welder_Code : "";
        //            CreateNewDWR.FillCapWelder3 = SelectedFillCapWelder3.ToString() != " - Select -" ? SelectedFillCapWelder3.Welder_Code : "";
        //            CreateNewDWR.FillCapWelder4 = SelectedFillCapWelder4.ToString() != " - Select -" ? SelectedFillCapWelder4.Welder_Code : "";
        //            CreateNewDWR.FillCapWeldProcess = SelectedFillCapWeldProcess.ToString() != " - Select -" ? SelectedFillCapWeldProcess.Weld_Process : "";

        //            CreateNewDWR.WPSNo = SelectedWPSNo.ToString() != " - Select -" ? SelectedWPSNo.Wps_No : "";
        //            CreateNewDWR.BaseMetal1 = SelectedBaseMetal1.ToString() != "- Select -" ? SelectedBaseMetal1.Base_Material : "";
        //            CreateNewDWR.BaseMetal2 = SelectedBaseMetal2.ToString() != "- Select -" ? SelectedBaseMetal2.Base_Material : "";
        //            CreateNewDWR.HeatNo1 = SelectedHeatNo1.ToString() != "- Select -" ? SelectedHeatNo1.Heat_No : "";
        //            CreateNewDWR.HeatNo2 = SelectedHeatNo2.ToString() != "- Select -" ? SelectedHeatNo2.Heat_No : "";

        //            CreateNewDWR.SheetNo = EReporterHelper.JointDetailFields.SheetNo;
        //            CreateNewDWR.Size = EReporterHelper.JointDetailFields.Size;
        //            CreateNewDWR.SpoolNo = EReporterHelper.JointDetailFields.SpoolNo;
        //            CreateNewDWR.Thickness = EReporterHelper.JointDetailFields.Thickness;
        //            CreateNewDWR.WeldType = EReporterHelper.JointDetailFields.Weldtype;
        //            CreateNewDWR.LineClass = EReporterHelper.JointDetailFields.LineClass;

        //            CreateNewDWR.VI = EReporterHelper.InspectResultFields.VI != "- Select -" ? EReporterHelper.InspectResultFields.VI : "";
        //            CreateNewDWR.DI = EReporterHelper.InspectResultFields.DI != "- Select -" ? EReporterHelper.InspectResultFields.DI : "";
        //            CreateNewDWR.VIComment = EReporterHelper.InspectResultFields.VI_Comment.ToString() != "- Select -" ? EReporterHelper.InspectResultFields.VI_Comment.Description : "";
        //            CreateNewDWR.DIComment = EReporterHelper.InspectResultFields.DI_Comment != "- Select -" ? EReporterHelper.InspectResultFields.DI_Comment : "";
        //            CreateNewDWR.Remarks = EReporterHelper.InspectResultFields.Remark;

        //            CreateNewDWR.Updated = true;

        //            await _DWRRepository.InsertAsync(CreateNewDWR);
        //            await _userDialogs.AlertAsync("Successfully Created..!", null, "Ok");

        //            if(BtnPressed == "SaveBtn")
        //            {
        //                SelectedDWRReport = CreateNewDWR;
        //                CreateNewDWR = null;
        //            }
        //            else
        //            {
        //                SelectedDWRReport = CreateNewDWR = null;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //}

        #region Public

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.Count == 0)
            {
                return;
            }
            if (parameters.Count > 1 && parameters.ContainsKey(NavigationParametersConstants.SelectedDWRReport))
            {
                SelectedDWRReport = (T_DWR)parameters[NavigationParametersConstants.SelectedDWRReport];
                BindAllDWRControl(false);
            }

        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        #endregion
    }
}

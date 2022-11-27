using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.Models;
using Newtonsoft.Json;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using JGC.Common.Extentions;
using Xamarin.Forms;
using JGC.Models.E_Reporter;

namespace JGC.ViewModels.E_Reporter
{
   public class JointDetailsViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_RT_Defects> _RT_DefectsRepository;
        private readonly IRepository<T_Welders> _weldersRepository;
        private readonly IRepository<T_DWR_HeatNos> _DWR_HeatNosRepository;
        private readonly IRepository<T_WPS_MSTR> _WPS_MSTRRepository;
        private readonly IRepository<T_BaseMetal> _BaseMetalRepository;
        private readonly IRepository<T_WeldProcesses> _WeldProcessesRepository;
        private readonly IRepository<T_DWR> _DWRRepository;
        private readonly IRepository<T_Drawings> _drawingsRepository;
        private readonly IRepository<T_EReports_Signatures> _signaturesRepository;
        private readonly IRepository<T_EReports_UsersAssigned> _usersAssignedRepository;
        private T_UserProject userProject;
       // private bool IsNavigatedFromMenu = false;
       // private string NavigateFromSelectionPage = string.Empty;
        T_EReports ThisEReport;
        List<DWR> DWRList = new List<DWR>();


        #region Properties
       
        private ObservableCollection<T_DWR> _DWRDownLoadedList;
        public ObservableCollection<T_DWR> DWRDownLoadedList
        {
            get { return _DWRDownLoadedList; }
            set { _DWRDownLoadedList = value; RaisePropertyChanged(); }
        }
        
        private T_DWR selectedDWRReport;
        public T_DWR SelectedDWRReport
        {
            get { return selectedDWRReport; }
            set {
                if (SetProperty(ref selectedDWRReport, value))
                {
                    DWRHelper.SelectedDWR = selectedDWRReport;
                    RaisePropertyChanged();
                }
            }
        }
        
        private string downloadingFor;
        public string DownloadingFor
        {
            get { return downloadingFor; }
            set
            {
                SetProperty(ref downloadingFor, value); OnPropertyChanged();
            }
        }
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }
        private string downloadedTestPack;
        public string DownloadedTestPack
        {
            get { return downloadedTestPack; }
            set { SetProperty(ref downloadedTestPack, value); }
        }
        private string downloadedSpoolDWGNo;
        public string DownloadedSpoolDWGNo
        {
            get { return downloadedSpoolDWGNo; }
            set { SetProperty(ref downloadedSpoolDWGNo, value); }
        }
        private string downloadedJointNo;
        public string DownloadedJointNo
        {
            get { return downloadedJointNo; }
            set { SetProperty(ref downloadedJointNo, value); }
        }
        
        private string _PageHeaderTitle;
        public string PageHeaderTitle
        {
            get { return _PageHeaderTitle; }
            set { SetProperty(ref _PageHeaderTitle, value); }
        }

        #endregion
        #region Delegate Commands  
        public ICommand OnBtnClickCommand
        {
            get
            {
                return new Command<string>(OnBtnClick);
            }
        }
        public ICommand NextBtnCommand
        {
            get
            {
                return new Command(OnClickNextButton);
            }
        }
        #endregion


        public JointDetailsViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_UserProject> _userProjectRepository,
            IRepository<T_RT_Defects> _RT_DefectsRepository,
            IRepository<T_Welders> _weldersRepository,
            IRepository<T_DWR_HeatNos> _DWR_HeatNosRepository,
            IRepository<T_WPS_MSTR> _WPS_MSTRRepository,
            IRepository<T_BaseMetal> _BaseMetalRepository,
            IRepository<T_WeldProcesses> _WeldProcessesRepository,
            IRepository<T_DWR> _DWRRepository,
            IRepository<T_Drawings> _drawingsRepository,
            IRepository<T_EReports_Signatures> _signaturesRepository,
            IRepository<T_EReports_UsersAssigned> _usersAssignedRepository
           ) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._userProjectRepository = _userProjectRepository;
            this._RT_DefectsRepository = _RT_DefectsRepository;
            this._weldersRepository = _weldersRepository;
            this._DWR_HeatNosRepository = _DWR_HeatNosRepository;
            this._WPS_MSTRRepository = _WPS_MSTRRepository;
            this._BaseMetalRepository = _BaseMetalRepository;
            this._WeldProcessesRepository = _WeldProcessesRepository;
            this._DWRRepository = _DWRRepository;
            this._drawingsRepository = _drawingsRepository;
            this._signaturesRepository = _signaturesRepository;
            this._usersAssignedRepository = _usersAssignedRepository;
            _userDialogs.HideLoading();
            JobSettingHeaderVisible = true;
            PageHeaderText = "Joint Details";
            Settings.DownloadParam = "JointDetailsPage";
            DWRHelper.DWRTargetType = typeof(JointDetailsViewModel);
        }

        private void OnBtnClick(string param)
        {
            if (param == "searchDownloadedList")
            {
                GetDWRListData(true);
            }
        }
        private async void OnClickNextButton()
        {
            if (SelectedDWRReport != null)
            {
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add(NavigationParametersConstants.SelectedDWRReport, SelectedDWRReport);
                navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
                 await navigationService.NavigateFromMenuAsync(typeof(InspectJointViewModel), navigationParameters);
            }
            else
                _userDialogs.AlertAsync("", "Please select any row", "Ok");
        }
        private async Task GetDWRListData(bool search)
        {
            try
            {
                var DWRsList = await _DWRRepository.GetAsync(i=>i.ProjectID==Settings.ProjectID);

                if (search)
                {
                    List<T_DWR> SearchEReports = new List<T_DWR>();
                    SearchText = string.Empty;
                    if (!String.IsNullOrWhiteSpace(DownloadedTestPack))
                        SearchText += DownloadedTestPack;
                    if (!String.IsNullOrWhiteSpace(DownloadedSpoolDWGNo))
                        SearchText += DownloadedSpoolDWGNo;
                    if (!String.IsNullOrWhiteSpace(DownloadedJointNo))
                        SearchText += DownloadedJointNo;

                    foreach (T_DWR row in DWRsList)
                    {
                        Boolean CanAdd = true;
                        if (SearchText != string.Empty)
                        {
                            string RowValue = row.TestPackage + row.SpoolDrawingNo + row.JointNo;

                            if (!RowValue.ToUpper().Contains(SearchText.ToUpper()))
                                CanAdd = false;
                        }
                        if (CanAdd)
                        {
                            SearchEReports.Add(row);
                        }
                    }
                    DWRDownLoadedList = new ObservableCollection<T_DWR>(SearchEReports);
                }
                else
                    DWRDownLoadedList = new ObservableCollection<T_DWR>(DWRsList);

                _userDialogs.HideLoading();
            }
            catch (Exception ex)
            {

            }
        }
        #region Public

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            GetDWRListData(false);
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        #endregion

    }

}

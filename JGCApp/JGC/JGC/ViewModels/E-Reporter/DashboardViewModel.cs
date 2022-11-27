
using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using Newtonsoft.Json;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.Common.Extentions;
using System.Collections.ObjectModel;


namespace JGC.ViewModels
{
   public class DashboardViewModel : BaseViewModel
    {

        #region fields
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_EReports> _eReportRepository;
        protected readonly INavigationService _navigationService;        
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private T_UserProject _selectedUserProject;
        private string _selectedEReportItem;

        private List<T_EReports> _EReports;
        private T_EReports selectedItem;
        private bool isRefreshing;
        #endregion

        #region Properties
        private ObservableCollection<T_EReports> eReports;
        public ObservableCollection<T_EReports> EReports
        {
            get { return eReports; }
            set { eReports = value; RaisePropertyChanged(); }
        }

        public T_EReports SelectedNextReport
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                //System.Diagnostics.Debug.WriteLine("Selected EReport: " + value?.ID);
            }
        }
        private bool _noDataIsVisible;
        public bool NoDataIsVisible
        {
            get { return _noDataIsVisible; }
            set { SetProperty(ref _noDataIsVisible, value); }
        }
        private bool _btnNextIsVisible;
        public bool BtnNextIsVisible
        {
            get { return _btnNextIsVisible; }
            set { SetProperty(ref _btnNextIsVisible, value); }
        }
        
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        #endregion

        #region Delegate Commands   
        public ICommand NextBtnCommand
        {
            get
            {
                return new Command(OnClickNextButton);
            }
        }
        public ICommand SearchBtnCommand
        {
            get
            {
                return new Command(OnClickSearchButton);
            }
        }
        
        #endregion

        public DashboardViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_UserProject> _userProjectRepository,
            IRepository<T_EReports> _eReportRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._userProjectRepository = _userProjectRepository;
            this._eReportRepository = _eReportRepository;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            try
            {
                GetDownloadedData();
                PageHeaderText = "Dashboard";
            }
            catch (Exception ex)
            {
                _userDialogs.HideLoading();
            }
            _userDialogs.HideLoading();

        }

        #region Private
        private async void GetDownloadedData()
        {
            var UserProjectList = await _userProjectRepository.GetAsync();
            if (UserProjectList.Count > 0)
                _selectedUserProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();
            var ER = await _eReportRepository.QueryAsync<T_EReports>(@"SELECT * FROM T_EReports WHERE upper(ModelName) = '" + _selectedUserProject.ModelName.ToUpper() + "' AND [ReportType] = '" + Settings.Report + "' ORDER BY [Priority] ASC,[ReportDate] DESC");
            
            EReports = new ObservableCollection<T_EReports>(ER.ToList());
            //string JsonString = ModsTools.WebServiceGet(ApiUrls.Url_getEReportHeaders("MODSTestMaster", Settings.Report, 1578, "Mikes Laptop"), Settings.AccessToken);
            //EReports = JsonConvert.DeserializeObject<List<T_EReports>>(JsonString);

            if (EReports.Count <=0)
            {
                NoDataIsVisible = true;
                BtnNextIsVisible = false;
            }
            else
            {
                NoDataIsVisible = false;
                BtnNextIsVisible = true;
            }
            //_userDialogs.HideLoading();
        }

        private async void OnClickNextButton()
        {
            if (SelectedNextReport == null)
                return;
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add(NavigationParametersConstants.ReportDetailsParameter, SelectedNextReport);
            navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
            //if(Settings.Report== "Daily Weld Report")
            //    await navigationService.NavigateAsync<DWR_EReporterViewModel>(navigationParameters);
            //else 
            if (Settings.Report == "Receiving Inspection Request")
                await navigationService.NavigateAsync<RIR_EReporterViewModel>(navigationParameters);
            else if (Settings.Report == "Material Receiving Report")
                await navigationService.NavigateAsync<MRR_EReporterViewModel>(navigationParameters);
            else if (Settings.Report == "Construction Material Requisition")
                await navigationService.NavigateAsync<CMR_EReporterViewModel>(navigationParameters);
            IsRunningTasks = false;
        }
        private async void OnClickSearchButton()
        {

        }
        #endregion
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

            //if (parameters.Count > 1 && parameters.ContainsKey(NavigationParametersConstants.ReportDetailsParameter))
            //{
               // _selectedUserProject = (T_UserProject)parameters[NavigationParametersConstants.SelectedProjectParameter];
              //  _selectedEReportItem = parameters[NavigationParametersConstants.ReportDetailsParameter].ToString();
              // EReports= _eReport.QueryAsync<T_EReports>($@"SELECT * FROM T_EReports WHERE [ModelName] = '" + _selectedUserProject.ModelName + "' AND [ReportType] = '" + _selectedEReportItem + "' ORDER BY [Priority] ASC,[ReportDate] DESC");

            //}
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        #endregion

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {


            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion

     
    }
}

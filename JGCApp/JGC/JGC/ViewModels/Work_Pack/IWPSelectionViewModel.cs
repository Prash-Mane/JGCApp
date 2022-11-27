using Acr.UserDialogs;
using JGC.Common.Interfaces;
using Prism.Navigation;
using JGC.Common.Extentions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using JGC.Common.Helpers;
using JGC.ViewModels.E_Test_Package;
using JGC.DataBase.DataTables;
using JGC.DataBase;
using JGC.DataBase.DataTables.WorkPack;
using System.Linq;
using System.Collections.ObjectModel;

namespace JGC.ViewModels.Work_Pack
{
    public class IWPSelectionViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_IWP> _iwpRepository;

        private T_UserProject userProject;

        #region Properties

        private ObservableCollection<T_IWP> jobSetting;
        public ObservableCollection<T_IWP> JobSetting
        {
            get { return jobSetting; }
            set { jobSetting = value; RaisePropertyChanged(); }
        }
        private T_IWP selectedJobSetting;
        public T_IWP SelectedJobSetting
        {
            get { return selectedJobSetting; }
            set { selectedJobSetting = value; RaisePropertyChanged(); }
        }

        private string _JBSPCWBS;
        public string JBSPCWBS
        {
            get { return _JBSPCWBS; }
            set { _JBSPCWBS = value; RaisePropertyChanged(); }
        }
        private string _JBSFWBS;
        public string JBSFWBS
        {
            get { return _JBSFWBS; }
            set { _JBSFWBS = value; RaisePropertyChanged(); }
        }
        private string _JBSIWP;
        public string JBSIWP
        {
            get { return _JBSIWP; }
            set { _JBSIWP = value; RaisePropertyChanged(); }
        }
        private string _JBSDiscipline;
        public string JBSDiscipline
        {
            get { return _JBSDiscipline; }
            set { _JBSDiscipline = value; RaisePropertyChanged(); }
        }
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
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

        public IWPSelectionViewModel(INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper, 
            ICheckValidLogin _checkValidLogin,
            IRepository<T_UserProject> _userProjectRepository,
            IRepository<T_IWP> _iwpRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._userProjectRepository = _userProjectRepository;
            this._iwpRepository = _iwpRepository;
            _userDialogs.HideLoading();
            PageHeaderText = "IWP SelectionPage";
            GetDownloadJobSettingListData(false);
            //JobSettingHeaderVisible = true; 
        }
        #region private 
        private async void OnClickNextButton()
        {
            if (SelectedJobSetting != null)
            {
                //IWPHelper.SelectedJobSetting = SelectedJobSetting;
                IWPHelper.IWP_ID = SelectedJobSetting.ID;
                IWPHelper.PreviousIWP_ID = 0;

                //await navigationService.NavigateAsync<IWPPdfViewModel>();
                  await navigationService.NavigateFromMenuAsync(typeof(IWPPdfViewModel));    
            }

        }
        private async Task GetDownloadJobSettingListData(bool search)
        {
            //NoDataIsVisible = false;
            var UserProjectList = await _userProjectRepository.GetAsync();
            if (UserProjectList.Count > 0)
                userProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();

            //var JobSettingList = await _iwpRepository.GetAsync();
            var JobSettingList = await _iwpRepository.QueryAsync<T_IWP>(@"SELECT * FROM T_IWP WHERE upper(ModelName) = '" + userProject.ModelName.ToUpper() + "'");

            if (search)
            {
                List<T_IWP> SearchJobSettings = new List<T_IWP>();
                string Searchstring = string.Empty;
                if (!String.IsNullOrWhiteSpace(JBSPCWBS))
                    Searchstring = Searchstring + JBSPCWBS.ToUpper();
                if (!String.IsNullOrWhiteSpace(JBSFWBS))
                    Searchstring = Searchstring + JBSFWBS.ToUpper();
                if (!String.IsNullOrWhiteSpace(JBSIWP))
                    Searchstring = Searchstring + JBSIWP.ToUpper();
                if (!String.IsNullOrWhiteSpace(JBSDiscipline))
                    Searchstring = Searchstring + JBSDiscipline.ToUpper();

                foreach (T_IWP row in JobSettingList)
                {
                    Boolean CanAdd = true;
                    if (Searchstring != string.Empty)
                    {
                        string RowValue = row.PCWBS + row.FWBS + row.Title + row.Discipline;

                        if (!RowValue.ToUpper().Contains(Searchstring))
                            CanAdd = false;
                    }
                    if (CanAdd)
                    {
                        SearchJobSettings.Add(row);
                    }
                }
                JobSetting = new ObservableCollection<T_IWP>(SearchJobSettings);
            }
            else
                JobSetting = new ObservableCollection<T_IWP>(JobSettingList);

            if (JobSetting.Count <= 0)
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
        
        private async void OnClickSearchButton()
        {            
             GetDownloadJobSettingListData(true);            
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
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        #endregion
    }
}

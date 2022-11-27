using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.Models;
using JGC.Common.Extentions;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System;

namespace JGC.ViewModels.E_Test_Package
{
   public class ETestPackageVewModel : BaseViewModel
    {

        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_ETestPackages> _eTestPackagesRepository;
        private readonly IRepository<T_ControlLogSignature> _controlLogSignatureRepository;
        private readonly IRepository<T_AttachedDocument> _attachedDocumentRepository;
        private readonly IRepository<T_PunchList> _punchListRepository;
        private readonly IRepository<T_PunchImage> _punchImageRepository;
        private readonly IRepository<T_DrainRecordContent> _drainRecordContentRepository;
        private readonly IRepository<T_DrainRecordAcceptedBy> _drainRecordAcceptedByRepository;
        private readonly IRepository<T_TestRecordDetails> _testRecordDetailsRepository;
        private readonly IRepository<T_TestRecordConfirmation> _testRecordConfirmationRepository;
        private readonly IRepository<T_TestRecordAcceptedBy> _testRecordAcceptedByRepository;
        private readonly IRepository<T_TestRecordImage> _testRecordImageRepository;
        private readonly IRepository<T_TestLimitDrawing> _testLimitDrawingRepository;
        private readonly IRepository<T_AdminPunchCategories> _adminPunchCategoriesRepository;

        private T_UserProject _selectedUserProject;
        private bool IsBack = false;

        #region Properties       
        private ObservableCollection<T_ETestPackages> eTestPackages;
        public ObservableCollection<T_ETestPackages> ETestPackages
        {
            get { return eTestPackages; }
            set { eTestPackages = value; RaisePropertyChanged(); }
        }
        private T_ETestPackages ETPSelected;
        public T_ETestPackages SelectedETP
        {
            get { return ETPSelected; }
            set {
                //ETPSelected = value; RaisePropertyChanged();
                if (SetProperty(ref ETPSelected, value))
                {
                    RaisePropertyChanged();
                }
            }
        }

        private string pcwbs;
        public string PCWBS
        {
            get { return pcwbs; }
            set
            {
                if (SetProperty(ref pcwbs, value))
                {
                    //GetETestPackagesListData(false);
                    RaisePropertyChanged();
                }
            }
        }

        private string testPackage;
        public string TestPackage
        {
            get { return testPackage; }
            set
            {
                if (SetProperty(ref testPackage, value))
                {
                    //GetETestPackagesListData(false);
                    RaisePropertyChanged();
                }
            }
        }

        private ObservableCollection<string> actionByList;
        public ObservableCollection<string> ActionByList
        {
            get { return actionByList; }
            set { actionByList = value; RaisePropertyChanged(); }
        }
        private string selectedActionBy;
        public string SelectedActionBy
        {
            get { return selectedActionBy; }
            set
            {
                if (SetProperty(ref selectedActionBy, value))
                {
                    //GetETestPackagesListData(false);
                    RaisePropertyChanged();
                }
            }
        }
        
        private ObservableCollection<string> pendingActionList;
        public ObservableCollection<string> PendingActionList
        {
            get { return pendingActionList; }
            set { pendingActionList = value; RaisePropertyChanged(); }
        }
        private string selectedPendingAction;
        public string SelectedPendingAction
        {
            get { return selectedPendingAction; }
            set
            {
                if (SetProperty(ref selectedPendingAction, value))
                {
                    //GetETestPackagesListData(false);
                    RaisePropertyChanged();
                }
            }
        }        
            
        private ObservableCollection<string> priorityList;
        public ObservableCollection<string> PriorityList
        {
            get { return priorityList; }
            set { priorityList = value; RaisePropertyChanged(); }
        }
        private string selectedPriority;
        public string SelectedPriority
        {
            get { return selectedPriority; }
            set
            {
                if (SetProperty(ref selectedPriority, value))
                {
                    //GetETestPackagesListData(false);
                    RaisePropertyChanged();
                }
            }
        }       
            
        private ObservableCollection<string> subContractorList;
        public ObservableCollection<string> SubContractorList
        {
            get { return subContractorList; }
            set { subContractorList = value; RaisePropertyChanged(); }
        }
        private string selectedSubContractor;
        public string SelectedSubContractor
        {
            get { return selectedSubContractor; }
            set
            {
                if (SetProperty(ref selectedSubContractor, value))
                {
                    //GetETestPackagesListData(false);
                    RaisePropertyChanged();
                }
            }
        }        
            
        private ObservableCollection<string> testMethodList;
        public ObservableCollection<string> TestMethodList
        {
            get { return testMethodList; }
            set { testMethodList = value; RaisePropertyChanged(); }
        }
        private string selectedTestMethod;
        public string SelectedTestMethod
        {
            get { return selectedTestMethod; }
            set
            {
                if (SetProperty(ref selectedTestMethod, value))
                {
                    //GetETestPackagesListData(false);
                    RaisePropertyChanged();
                }
            }
        }
        private bool _noDataIsVisible;
        public bool NoDataIsVisible
        {
            get { return _noDataIsVisible; }
            set { SetProperty(ref _noDataIsVisible, value); }
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
        public ETestPackageVewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_UserProject> _userProjectRepository,
            IRepository<T_ETestPackages> _eTestPackagesRepository,
            IRepository<T_ControlLogSignature> _controlLogSignatureRepository,
            IRepository<T_AttachedDocument> _attachedDocumentRepository,
            IRepository<T_PunchList> _punchListRepository,
            IRepository<T_PunchImage> _punchImageRepository,
            IRepository<T_DrainRecordContent> _drainRecordContentRepository,
            IRepository<T_DrainRecordAcceptedBy> _drainRecordAcceptedByRepository,
            IRepository<T_TestRecordDetails> _testRecordDetailsRepository,
            IRepository<T_TestRecordConfirmation> _testRecordConfirmationRepository,
            IRepository<T_TestRecordAcceptedBy> _testRecordAcceptedByRepository,
            IRepository<T_TestRecordImage> _testRecordImageRepository,
            IRepository<T_TestLimitDrawing> _testLimitDrawingRepository,
            IRepository<T_AdminPunchCategories> _adminPunchCategoriesRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._userProjectRepository = _userProjectRepository;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._eTestPackagesRepository= _eTestPackagesRepository;
            this._controlLogSignatureRepository = _controlLogSignatureRepository;
            this._attachedDocumentRepository = _attachedDocumentRepository;
            this._punchListRepository = _punchListRepository;
            this._punchImageRepository = _punchImageRepository;
            this._drainRecordContentRepository = _drainRecordContentRepository;
            this._drainRecordAcceptedByRepository = _drainRecordAcceptedByRepository;
            this._testRecordDetailsRepository = _testRecordDetailsRepository;
            this._testRecordConfirmationRepository = _testRecordConfirmationRepository;
            this._testRecordAcceptedByRepository = _testRecordAcceptedByRepository;
            this._testRecordImageRepository = _testRecordImageRepository;
            this._testLimitDrawingRepository = _testLimitDrawingRepository;
            this._adminPunchCategoriesRepository = _adminPunchCategoriesRepository;
            GetETestPackagesListData(true);
            _userDialogs.HideLoading();
            PageHeaderText = "Test Package List";
        }
        #region Private
        private async Task GetETestPackagesListData(bool DDLBind)
        {
            try
            {
                if (!IsBack)
                {
                    var UserProjectList = await _userProjectRepository.GetAsync();

                    if (UserProjectList.Count > 0)
                        _selectedUserProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();

                    var ETP = await _eTestPackagesRepository.QueryAsync<T_ETestPackages>(@"SELECT * FROM T_ETestPackages WHERE upper(ModelName) = '" + _selectedUserProject.ModelName.ToUpper() + "'");
                    List<T_ETestPackages> ETestPackagesData = new List<T_ETestPackages>(ETP);
                    
                    if (ETestPackagesData.Count > 0 && DDLBind)
                    {
                        //ActionByList = new ObservableCollection<string>(ETestPackagesData.Where(l => l.ActionBy != null).Select(l => l.ActionBy).Distinct().ToList());
                        //PendingActionList = new ObservableCollection<string>(ETestPackagesData.Where(c => c.Status != null).Select(c => c.Status).Distinct().ToList());                       
                        //PriorityList = new ObservableCollection<int>(ETestPackagesData.Select(c => Convert.ToInt32(c.Priority)).Distinct().ToList());                      
                        //SubContractorList = new ObservableCollection<string>(ETestPackagesData.Where(s => s.SubContractor != null).Select(s => s.SubContractor).Distinct().ToList());
                        //TestMethodList = new ObservableCollection<string>(ETestPackagesData.Where(c => c.TestMedia != null).Select(c => c.TestMedia).Distinct().ToList());

                        var action = ETestPackagesData.Where(l => l.ActionBy != null).Select(l => l.ActionBy).Distinct().ToList();
                        action.Insert(0, "ALL");
                        ActionByList = new ObservableCollection<string>(action);


                        var pending = ETestPackagesData.Where(c => c.Status != null).Select(c => c.Status).Distinct().ToList();
                        pending.Insert(0, "ALL");
                        PendingActionList = new ObservableCollection<string>(pending);



                        List<string> priority = ETestPackagesData.Select(c => (Convert.ToInt32(c.Priority)).ToString()).Distinct().ToList();
                        priority.Insert(0, "ALL");
                        PriorityList = new ObservableCollection<string>(priority);


                        var subcontractor = ETestPackagesData.Where(s => s.SubContractor != null).Select(s => s.SubContractor).Distinct().ToList();
                        subcontractor.Insert(0, "ALL");
                        SubContractorList = new ObservableCollection<string>(subcontractor);


                        var testmethod = ETestPackagesData.Where(c => c.TestMedia != null).Select(c => c.TestMedia).Distinct().ToList();
                        testmethod.Insert(0, "ALL");
                        TestMethodList = new ObservableCollection<string>(testmethod);
                    }
                    if (!DDLBind)
                    {
                        List<T_ETestPackages> Searchlist = new List<T_ETestPackages>();
                        string[] Searchstring = { pcwbs, testPackage,
                                                  selectedActionBy !=null? selectedActionBy : null,
                                                  selectedPendingAction != null ? selectedPendingAction : null,
                                                  selectedPriority ,
                                                  selectedSubContractor!=null? selectedSubContractor :null,
                                                  selectedTestMethod != null ?selectedTestMethod :null};

                        foreach (T_ETestPackages row in ETestPackagesData)
                        {
                            string[] rowstring ={ row.PCWBS, row.TestPackage,
                                                  selectedActionBy !=null? row.ActionBy :null,
                                                  selectedPendingAction != null ? row.Status :null,
                                                  selectedPriority,
                                                  selectedSubContractor!=null? row.SubContractor :null,
                                                  selectedTestMethod != null ?  row.TestMedia :null};
                            Boolean CanAdd = true;
                            for (int i = 0; i < Searchstring.Count(); i++)
                            {
                                if (Searchstring[i] == null || Searchstring[i] == "ALL")
                                    continue;
                                if (!rowstring[i].ToUpper().Contains(Searchstring[i].ToUpper()))
                                    CanAdd = false;
                            }
                            if (CanAdd)
                            {
                                Searchlist.Add(row);
                            }
                        }
                        ETestPackages = new ObservableCollection<T_ETestPackages>(Searchlist);
                    }
                   else
                    {
                        ETestPackages = new ObservableCollection<T_ETestPackages>(ETP);
                    }

                    if (ETestPackages.Count > 0)
                        NoDataIsVisible = false;
                    else
                        NoDataIsVisible = true;
                }

            }
            catch (Exception ex)
            {

            }
        }
        private async void OnClickSearchButton()
        {
            await GetETestPackagesListData(false);
        }
        private async void OnClickNextButton()
        {
            try
            {
                if (SelectedETP == null)
                    return;


                var PunchCategories = await _adminPunchCategoriesRepository.QueryAsync("SELECT * FROM [T_AdminPunchCategories] WHERE [ProjectID] = '" + Settings.ProjectID + "'");

                CurrentPageHelper.PunchCategories = PunchCategories.Select(i => i).ToList();

                CurrentPageHelper.ETestPackage = SelectedETP;
                //var navigationParameters = new NavigationParameters();
                //navigationParameters.Add(NavigationParametersConstants.SelectedETP, SelectedETP);
                //navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
                //await navigationService.NavigateAsync<PunchOverviewViewModel>();


                await navigationService.NavigateFromMenuAsync(typeof(PunchOverviewViewModel));
                IsRunningTasks = false;
            }
            catch(Exception ex)
            {
                var data2 = await Application.Current.MainPage.DisplayAlert("Exception = " + ex.ToString(), "Are you sure you want to sign this signature?", "Yes", "No");
            }
            

        }
        private async void OnBackPressed()
        {
            CheckValidLogin._pageHelper = new PageHelper();
        }
        #endregion
        #region Public
        public async void DeleteTestPackage(T_ETestPackages SelectedETP)
        {
            if (SelectedETP == null || IsRunningTasks)
            {
                return;
            }
            if (await _userDialogs.ConfirmAsync($"Are you sure you want to delete?", $"Delete Test Package", "Yes", "No"))
            {
                var exist = await _eTestPackagesRepository.QueryAsync<T_ETestPackages>("SELECT [ID] FROM [T_ETestPackages] WHERE [ID] = '" + SelectedETP.ID + "'");

                if (exist.Count() > 0)
                {
                    try
                    {
                        await _eTestPackagesRepository.QueryAsync<T_ETestPackages>(@"DELETE FROM [T_ETestPackages] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ID] = '" + SelectedETP.ID + "'");
                        await _controlLogSignatureRepository.QueryAsync<T_ControlLogSignature>("DELETE FROM [T_ControlLogSignature] WHERE [ProjectID] = " + Settings.ProjectID + " AND [ETestPackageID] = '" + SelectedETP.ID + "'");
                        await _testLimitDrawingRepository.QueryAsync<T_TestLimitDrawing>("DELETE FROM [T_TestLimitDrawing] WHERE [ProjectID] = " + Settings.ProjectID + " AND [ETestPackageID] = '" + SelectedETP.ID + "'");
                        await _attachedDocumentRepository.QueryAsync<T_AttachedDocument>("DELETE FROM [T_AttachedDocument] WHERE [ProjectID] = " + Settings.ProjectID + " AND [ETestPackageID] = '" + SelectedETP.ID + "'");
                        await _punchListRepository.QueryAsync<T_PunchList>("DELETE FROM [T_PunchList] WHERE [ProjectID] = " + Settings.ProjectID + " AND [ETestPackageID] = '" + SelectedETP.ID + "'");
                        await _punchImageRepository.QueryAsync<T_PunchImage>("DELETE FROM [T_PunchImage] WHERE [ProjectID] = " + Settings.ProjectID + " AND [ETestPackageID] = '" + SelectedETP.ID + "'");

                        await _testRecordDetailsRepository.QueryAsync<T_TestRecordDetails>("DELETE FROM [T_TestRecordDetails] WHERE [ProjectID] = " + Settings.ProjectID + " AND [ETestPackageID] = '" + SelectedETP.ID + "'");
                        await _testRecordConfirmationRepository.QueryAsync<T_TestRecordConfirmation>("DELETE FROM [T_TestRecordConfirmation] WHERE [ProjectID] = " + Settings.ProjectID + " AND [ETestPackageID] = '" + SelectedETP.ID + "'");
                        await _testRecordAcceptedByRepository.QueryAsync<T_TestRecordAcceptedBy>("DELETE FROM [T_TestRecordAcceptedBy] WHERE [ProjectID] = " + Settings.ProjectID + " AND [ETestPackageID] = '" + SelectedETP.ID + "'");

                        await _drainRecordContentRepository.QueryAsync<T_DrainRecordContent>("DELETE FROM [T_DrainRecordContent] WHERE [ProjectID] = " + Settings.ProjectID + " AND [ETestPackageID] = '" + SelectedETP.ID + "'");
                        await _drainRecordAcceptedByRepository.QueryAsync<T_DrainRecordAcceptedBy>("DELETE FROM [T_DrainRecordAcceptedBy] WHERE [ProjectID] = " + Settings.ProjectID + " AND [ETestPackageID] = '" + SelectedETP.ID + "'");

                        await _testRecordImageRepository.QueryAsync<T_TestRecordImage>("DELETE FROM [T_TestRecordImage] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ETestPackageID] = '" + SelectedETP.ID + "'");
                        GetETestPackagesListData(true);
                    }
                    catch(Exception ex)
                    {

                    }

                }
            }

                IsRunningTasks = false;
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            IsBack = true;
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

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
using JGC.Models.E_Test_Package;
using System.Collections.ObjectModel;
using System;
using System.Data;
using Rg.Plugins.Popup.Services;
using JGC.UserControls.PopupControls;

namespace JGC.ViewModels.E_Test_Package
{

    public class PunchOverviewViewModel : BaseViewModel
    {

        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_TestLimitDrawing> _testLimitDrawingRepository;
        private readonly IRepository<T_PunchList> _punchListRepository;       

        #region Properties     
        private T_ETestPackages ETPSelected;
        public T_ETestPackages SelectedETP
        {
            get { return ETPSelected; }
            set { ETPSelected = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<PunchOverview> punchOverviewList;
        public ObservableCollection<PunchOverview> PunchOverviewList
        {
            get { return punchOverviewList; }
            set { punchOverviewList = value; RaisePropertyChanged(); }
        }

        
        private PunchOverview selectedPunchOverview;
        public PunchOverview SelectedPunchOverview
        {
            get { return selectedPunchOverview; }
            set
            {
                if (SetProperty(ref selectedPunchOverview, value))
                {
                    selectedPunchOverview = value;
                    // GotoPunchView(selectedPunchOverview);
                    RaisePropertyChanged();
                   // OnPropertyChanged();
                }
            }
        }

        private string selectedETPTitle;
        public string SelectedETPTitle
        {
            get { return selectedETPTitle; }
            set { selectedETPTitle = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<string> layerNameList;
        public ObservableCollection<string> LayerNameList
        {
            get { return layerNameList; }
            set { layerNameList = value; RaisePropertyChanged(); }
        }       
        private ObservableCollection<string> categoryList;
        public ObservableCollection<string> CategoryList
        {
            get { return categoryList; }
            set { categoryList = value; RaisePropertyChanged(); }
        }
        
        private ObservableCollection<string> statusList;
        public ObservableCollection<string> StatusList
        {
            get { return statusList; }
            set { statusList = value; RaisePropertyChanged(); }
        }
        private string selectedLayerName;
        public string SelectedLayerName
        {
            get { return selectedLayerName; }
            set
            {
                if (SetProperty(ref selectedLayerName, value))
                {
                    GetPunchOverViewListData(false);
                    OnPropertyChanged();
                }
            }
        }
        private string selectedCategory;
        public string SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                if (SetProperty(ref selectedCategory, value))
                {
                    GetPunchOverViewListData(false);
                    OnPropertyChanged();
                }
            }
        }
        private string selectedStatus;
        public string SelectedStatus
        {
            get { return selectedStatus; }
            set
            {
                if (SetProperty(ref selectedStatus, value))
                {
                    GetPunchOverViewListData(false);
                    OnPropertyChanged();
                }
            }
        }

        #endregion
        public PunchOverviewViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_UserProject> _userProjectRepository,
            IRepository<T_TestLimitDrawing> _testLimitDrawingRepository,
            IRepository<T_PunchList> _punchListRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._userProjectRepository = _userProjectRepository;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._testLimitDrawingRepository = _testLimitDrawingRepository;
            this._punchListRepository = _punchListRepository;
            _userDialogs.HideLoading();           
            PageHeaderText = "Punch Overview";
            IsHeaderBtnVisible = true;
        }
        #region Private
        private async void GetPunchOverViewListData(bool DDLBind)
        {
            try
            {
                    string SQL = "SELECT TL.ID, TL.DisplayName, PL.PunchID, PL.Description, PL.Status, PL.Updated, PL.PunchAdminID, PL.Category, APL.LayerName,TL.IsPID"
                        + " FROM((T_TestLimitDrawing TL"
                        + " LEFT OUTER JOIN T_PunchList PL ON TL.ProjectID = PL.ProjectID AND TL.ETestPackageID = PL.ETestPackageID AND TL.ID = PL.VMHub_DocumentsID)"
                        + " LEFT OUTER JOIN T_AdminPunchLayer APL ON PL.PunchAdminID = APL.ID) "
                        + " WHERE TL.[ProjectID] = '" + ETPSelected.ProjectID + "'"
                        + " AND TL.[ETestPackageID] = '" + ETPSelected.ID + "'"
                        + " ORDER BY TL.[OrderNo] ASC ";

                    var data = await _testLimitDrawingRepository.QueryAsync<PunchOverview>(SQL);
                    string previousDisplayName = "";
                    List<PunchOverview> PunchOverviewData = new List<PunchOverview>();

                    foreach (PunchOverview PunchOverview in data)
                    {

                        if (previousDisplayName == PunchOverview.DisplayName)
                        {
                            string punchquery = "SELECT [PunchID] FROM [T_PunchList] WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'"
                                             + " AND [ETestPackageID] = '" + ETPSelected.ID + "'"
                                             + " AND [VMHub_DocumentsID] = '" + PunchOverview.ID + "'"
                                             + " AND [Status] = 'Open'";
                            var punchdata = await _punchListRepository.QueryAsync<T_PunchList>(punchquery);
                            bool activePunches = false;
                            if (punchdata.Count > 0)
                                activePunches = true;

                            string drawingStatus = PunchOverview.PunchID == string.Empty ? "No Punches" : (activePunches ? "Active Punches" : "All Punches Closed");

                            string StatusImage = drawingStatus == "No Punches" ? "Grayradio.png" : (drawingStatus == "Active Punches" ? "Yellowradio.png" : "Greenradio.png");

                            PunchOverview PunchOverviewrow = new PunchOverview { ID = PunchOverview.ID, PunchAdminID = 0, LayerName = null, DisplayName = PunchOverview.DisplayName, Category = null, PunchID = null, Description = null, Status = drawingStatus, StatusImage = StatusImage, Updated = false };

                            PunchOverviewData.Add(PunchOverviewrow);

                            previousDisplayName = PunchOverview.DisplayName;
                        }
                    else 
                    {
                        string statusImage = PunchOverview.Status != null ? PunchOverview.Status.ToUpper() == "CANCELLED" ? "Grayradio.png" : (PunchOverview.Status.ToUpper() == "CLOSED" ? "Greenradio.png" : "Yellowradio.png") : "Yellowradio.png";

                        PunchOverview POview = new PunchOverview { ID = PunchOverview.ID, PunchAdminID = PunchOverview.PunchAdminID, LayerName = PunchOverview.LayerName, DisplayName = PunchOverview.DisplayName, Category = PunchOverview.Category, PunchID = PunchOverview.PunchID, Description = PunchOverview.Description, Status = PunchOverview.Status, StatusImage = statusImage, Updated = PunchOverview.Updated, IsPID = PunchOverview.IsPID };
                        PunchOverviewData.Add(POview);
                    }
                    /*
                     * Punches Key
                        Grey - Cancelled
                        Open (includes TPC Confirmed ones) - Yellow
                        Green - Closed
                     */
                    
                    }
                    if (PunchOverviewData.Count > 0 && DDLBind)
                    {
                        List<string> Layer = PunchOverviewData.Where(l => l.LayerName != null).Select(l => l.LayerName).Distinct().ToList();
                        Layer.Insert(0, "ALL");
                        LayerNameList = new ObservableCollection<string>(Layer);

                        List<string> Category = PunchOverviewData.Where(c => c.Category != null).Select(c => c.Category).Distinct().ToList();
                        Category.Insert(0, "ALL");
                        CategoryList = new ObservableCollection<string>(Category);

                        List<string> Status = PunchOverviewData.Where(s => s.Status != null).Select(s => s.Status).Distinct().ToList();
                        Status.Insert(0, "ALL");
                        StatusList = new ObservableCollection<string>(Status);
                    }
                    if (!DDLBind)
                    {
                        List<PunchOverview> Searchlist = new List<PunchOverview>();
                        string[] Searchstring = { selectedLayerName!=null? "~" + selectedLayerName +"~" :null,
                                      selectedCategory != null ? "~" + selectedCategory + "~":null,
                                      selectedStatus!=null? "~" + selectedStatus + "~":null };

                        foreach (PunchOverview row in PunchOverviewData)
                        {
                            Boolean CanAdd = true;
                            foreach (string str in Searchstring)
                            {
                                if (str != null && str != "~ALL~")
                                {
                                    string RowValue = "~" + row.LayerName + "~" + row.Category + "~" + row.Status + "~";

                                    if (!RowValue.ToUpper().Contains(str.ToUpper()))
                                        CanAdd = false;
                                }
                            }
                            if (CanAdd)
                            {
                                Searchlist.Add(row);
                            }
                        }
                        PunchOverviewList = new ObservableCollection<PunchOverview>(Searchlist.OrderBy(x => x.IsPID));
                    }
                    else
                        PunchOverviewList = new ObservableCollection<PunchOverview>(PunchOverviewData.OrderBy(x => x.IsPID));

                    CurrentPageHelper.CurrentPunchOverview = PunchOverviewList.FirstOrDefault();                

            }
            catch(Exception ex)
            {

            }
        }
        public async void GotoPunchView()
        {
            if (SelectedPunchOverview == null)
                return;
            //var navigationParameters = new NavigationParameters();
            //navigationParameters.Add(NavigationParametersConstants.SelectedPunchOverview, SelectedPunchOverview);
            //navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
            CurrentPageHelper.CurrentPunchOverview = SelectedPunchOverview;
            CurrentPageHelper.IsOnlyOverview = true;
           await navigationService.NavigateFromMenuAsync(typeof(PunchViewModel));
        }

        public async void ShowDescriptionPopup(string Description)
        {
            if (Description == null)
                return;
            if (!string.IsNullOrWhiteSpace(Description))
                await PopupNavigation.PushAsync(new ShowWrapTextPopup("Description", Description), true);
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
            //if (parameters.Count == 0)
            //{
            //    return;
            //}

            //if (parameters.Count > 1 && parameters.ContainsKey(NavigationParametersConstants.SelectedETP))
            //{
                ETPSelected = CurrentPageHelper.ETestPackage;//(T_ETestPackages)parameters[NavigationParametersConstants.SelectedETP];
                SelectedETPTitle = ETPSelected.TestPackage;
                GetPunchOverViewListData(true);              
           // }
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        #endregion
    }
}

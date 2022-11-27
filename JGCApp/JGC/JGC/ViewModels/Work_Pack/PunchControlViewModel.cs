using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.DataBase.DataTables.WorkPack;
using JGC.Models.Work_Pack;
using JGC.UserControls.PopupControls.ColorSelection_CustomColor;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JGC.ViewModels
{
    public class PunchControlViewModel: BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IResizeImageService _resizeImageService;
        private readonly IRepository<T_CWPDrawings> _cwpDrawingsRepository;
        private readonly IRepository<T_IWPPunchControlItem> _iwpPunchControlItemRepository;
        private readonly IRepository<T_IWPPunchImage> _iwpPunchImagesRepository;
        private readonly IRepository<T_IWPPunchCategories> _iWPPunchCategoriesRepository;
        private readonly IRepository<T_IWPFunctionCodes> _iWPFunctionCodesRepository;
        private readonly IRepository<T_IWPCompanyCategoryCodes> _iWPCompanyCategoryCodesRepository;
        private readonly IRepository<T_IWPAdminPunchLayer> _iWPAdminPunchLayerRepository;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private readonly IRepository<T_IWPControlLogSignatures> _iwpControlLogSignaturesRepository;
        private readonly IRepository<T_IWP> _iwpRepository;
        private readonly IRepository<T_IWPDrawings> _iwpDrawingsRepository;
        private readonly IRepository<T_IWPAttachments> _iwpAttachmentsRepository;
        private readonly IRepository<T_CwpTag> _CwpTAg;

        private readonly IMedia _media;
        private ObservableCollection<IWPPunchListModel> PunchesListMemory = new ObservableCollection<IWPPunchListModel>();


        public INavigation Navigation { get; }
        static List<IWPPunchPointer> CurrentPunchPointer = new List<IWPPunchPointer>();
        private T_UserProject userProject;
        public T_UserDetails CurrentUserDetail;
        private T_IWP IWP;
        public PunchControlLayerModel SelectedPunchControlLayer;

        #region properties  
        private bool pdfGrid;
        public bool PDFGrid
        {
            get { return pdfGrid; }
            set
            {
                SetProperty(ref pdfGrid, value);
            }
        }
        private bool punchCategoryGrid;
        public bool PunchCategoryGrid
        {
            get { return punchCategoryGrid; }
            set
            {
                SetProperty(ref punchCategoryGrid, value);
            }
        }
        private bool punchControlGrid;
        public bool PunchControlGrid
        {
            get { return punchControlGrid; }
            set
            {
                SetProperty(ref punchControlGrid, value);
            }
        }
        private bool punchLayerGrid;
        public bool PunchLayerGrid
        {
            get { return punchLayerGrid; }
            set
            {
                SetProperty(ref punchLayerGrid, value);
            }
        }
        private bool cameraGrid;
        public bool CameraGrid
        {
            get { return cameraGrid; }
            set
            {
                SetProperty(ref cameraGrid, value);
            }
        }
        private ObservableCollection<T_IWPDrawings> drawingList;
        public ObservableCollection<T_IWPDrawings> DrawingList
        {
            get { return drawingList; }
            set { drawingList = value; RaisePropertyChanged(); }
        }
        private T_IWPDrawings selectedDrawing;
        public T_IWPDrawings SelectedDrawing
        {
            get { return selectedDrawing; }
            set
            {
                if (SetProperty(ref selectedDrawing, value))
                {
                     OnClickButton("OpenPDF");
                    GeneratePunchDataTable(SelectedDrawing);
                    OnPropertyChanged();
                }
            }
        }
        private string selectedPDF;
        public string SelectedPDF
        {
            get { return selectedPDF; }
            set
            {
                SetProperty(ref selectedPDF, value);
            }
        }
        private List<string> _cameraItems;
        public List<string> CameraItems
        {
            get { return _cameraItems; }
            set { _cameraItems = value; RaisePropertyChanged(); }
        }
        private string _selectedCameraItem;
        public string SelectedCameraItem
        {
            get { return _selectedCameraItem; }
            set
            {
                if (SetProperty(ref _selectedCameraItem, value))
                {
                    //SelectedCameraItems(_selectedCameraItem);
                    OnPropertyChanged();
                }
            }
        }
        private bool isSaveVisible;
        public bool IsSaveVisible
        {
            get { return isSaveVisible; }
            set
            {
                SetProperty(ref isSaveVisible, value);
            }
        }
        private bool showbuttons;
        public bool Showbuttons
        {
            get { return showbuttons; }
            set
            {
                SetProperty(ref showbuttons, value);
            }
        }
        private ObservableCollection<IWPPunchListModel> punchesList;
        public ObservableCollection<IWPPunchListModel> PunchesList
        {
            get { return punchesList; }
            set { punchesList = value; RaisePropertyChanged(); }
        }
        private IWPPunchListModel selectedPunch;
        public IWPPunchListModel SelectedPunch
        {
            get { return selectedPunch; }
            set
            {
                if (SetProperty(ref selectedPunch, value))
                {
                    //  OnClickButton("LoadPunch");
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<IWPPunchListModel> tagsComponentList;
        public ObservableCollection<IWPPunchListModel> TagsComponentList
        {
            get { return tagsComponentList; }
            set { tagsComponentList = value; RaisePropertyChanged(); }
        }
        private IWPPunchListModel selectedtagsComponent;
        public IWPPunchListModel SelectedtagsComponent
        {
            get { return selectedtagsComponent; }
            set
            {
                if (SetProperty(ref selectedtagsComponent, value))
                {
                    GeneratePunchDataTableBasedOnTagSelection(SelectedtagsComponent);
                    //  OnClickButton("LoadPunch");
                    OnPropertyChanged();
                }
            }
        }

        private IWPPunchListModel cameraPunch;
        public IWPPunchListModel CameraPunch
        {
            get { return cameraPunch; }
            set
            {
                if (SetProperty(ref cameraPunch, value))
                {
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<T_IWPAdminPunchLayer> punchLayersList;
        public ObservableCollection<T_IWPAdminPunchLayer> PunchLayersList
        {
            get { return punchLayersList; }
            set { punchLayersList = value; RaisePropertyChanged(); }
        }
        private T_IWPAdminPunchLayer selectedpunchLayer;
        public T_IWPAdminPunchLayer SelectedPunchLayer
        {
            get { return selectedpunchLayer; }
            set
            {
                if (SetProperty(ref selectedpunchLayer, value))
                {
                    GetSelectedpunchLayer();
                    OnPropertyChanged();
                }
            }
        }
       
        
        private ObservableCollection<T_IWPPunchCategories> punchCategories;
        public ObservableCollection<T_IWPPunchCategories> PunchCategories
        {
            get { return punchCategories; }
            set { punchCategories = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_IWPPunchCategories> _categoryList;
        public ObservableCollection<T_IWPPunchCategories> CategoryList
        {
            get { return _categoryList; }
            set { _categoryList = value; RaisePropertyChanged(); }
        }
        private T_IWPPunchCategories selectedCategory;
        public T_IWPPunchCategories SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                if (SetProperty(ref selectedCategory, value))
                {
                    //GetSelectedCategory(true);
                    OnPropertyChanged();
                }
            }
        }
      
        private ObservableCollection<T_IWPFunctionCodes> _functionCodeList;
        public ObservableCollection<T_IWPFunctionCodes> FunctionCodeList
        {
            get { return _functionCodeList; }
            set { _functionCodeList = value; RaisePropertyChanged(); }
        }
        private T_IWPFunctionCodes selectedFunctionCode;
        public T_IWPFunctionCodes SelectedFunctionCode
        {
            get { return selectedFunctionCode; }
            set
            {
                if (SetProperty(ref selectedFunctionCode, value))
                {
                    //GetSelectedFunctionCode(true);
                    OnPropertyChanged();
                }
            }
        }

        
        private ObservableCollection<T_IWPCompanyCategoryCodes> companyCategoryCodes;
        public ObservableCollection<T_IWPCompanyCategoryCodes> CompanyCategoryCodes
        {
            get { return companyCategoryCodes; }
            set { companyCategoryCodes = value; RaisePropertyChanged(); }
        }
        private T_IWPCompanyCategoryCodes selectedCompanyCategoryCodes;
        public T_IWPCompanyCategoryCodes SelectedCompanyCategoryCodes
        {
            get { return selectedCompanyCategoryCodes; }
            set
            {
                if (SetProperty(ref selectedCompanyCategoryCodes, value))
                {
                    //GetSelectedCompanyCategoryCodes(true);
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<T_IWPAdminPunchLayer> punchControlLayersList;
        public ObservableCollection<T_IWPAdminPunchLayer> PunchControlLayersList
        {
            get { return punchControlLayersList; }
            set { punchControlLayersList = value; RaisePropertyChanged(); }
        }
        private string _punchDescription;
        public string punchDescription
        {
            get { return _punchDescription; }
            set
            {
                SetProperty(ref _punchDescription, value);
            }
        }
        private string _punchRemarks;
        public string punchRemarks
        {
            get { return _punchRemarks; }
            set
            {
                SetProperty(ref _punchRemarks, value);
            }
        }
        private string punchControlpandID;
        public string PunchControlpandID
        {
            get { return punchControlpandID; }
            set
            {
                SetProperty(ref punchControlpandID, value);
            }
        }
        private string createdBy;
        public string CreatedBy
        {
            get { return createdBy; }
            set
            {
                SetProperty(ref createdBy, value);
            }
        }
        private string createdOn;
        public string CreatedOn
        {
            get { return createdOn; }
            set
            {
                SetProperty(ref createdOn, value);
            }
        }
        private string controlDescription;
        public string ControlDescription
        {
            get { return controlDescription; }
            set
            {
                SetProperty(ref controlDescription, value);
                OnPropertyChanged();
            }
        }
        private string controlRejectedReason;
        public string ControlRejectedReason
        {
            get { return controlRejectedReason; }
            set
            {
                SetProperty(ref controlRejectedReason, value);
            }
        }
        private string cancelledImage;
        public string CancelledImage
        {
            get { return cancelledImage; }
            set
            {
                SetProperty(ref cancelledImage, value);
            }
        }
        private string cancelledBy;
        public string CancelledBy
        {
            get { return cancelledBy; }
            set
            {
                SetProperty(ref cancelledBy, value);
            }
        }
        private string cancelledOn;
        public string CancelledOn
        {
            get { return cancelledOn; }
            set
            {
                SetProperty(ref cancelledOn, value);
            }
        }
        private string updatedBy;
        public string UpdatedBy
        {
            get { return updatedBy; }
            set
            {
                SetProperty(ref updatedBy, value);
            }
        }
        private string updatedOn;
        public string UpdatedOn
        {
            get { return updatedOn; }
            set
            {
                SetProperty(ref updatedOn, value);
            }
        }
        private string workCompletedImage;
        public string WorkCompletedImage
        {
            get { return workCompletedImage; }
            set
            {
                SetProperty(ref workCompletedImage, value);
            }
        }
        private string workCompletedBy;
        public string WorkCompletedBy
        {
            get { return workCompletedBy; }
            set
            {
                SetProperty(ref workCompletedBy, value);
            }
        }
        private string workCompletedOn;
        public string WorkCompletedOn
        {
            get { return workCompletedOn; }
            set
            {
                SetProperty(ref workCompletedOn, value);
            }
        }
        private string workConfirmedImage;
        public string WorkConfirmedImage
        {
            get { return workConfirmedImage; }
            set
            {
                SetProperty(ref workConfirmedImage, value);
            }
        }
        private string workConfirmedBy;
        public string WorkConfirmedBy
        {
            get { return workConfirmedBy; }
            set
            {
                SetProperty(ref workConfirmedBy, value);
            }
        }
        private string workConfirmedOn;
        public string WorkConfirmedOn
        {
            get { return workConfirmedOn; }
            set
            {
                SetProperty(ref workConfirmedOn, value);
            }
        }
        private T_IWPDrawings pdfDrawing;
        public T_IWPDrawings PDFDrawing
        {
            get { return pdfDrawing; }
            set
            {
                SetProperty(ref pdfDrawing, value);
            }
        }

        private string _PunchSearchText;
        public string PunchSearchText
        {
            get { return _PunchSearchText; }
            set
            {
                SetProperty(ref _PunchSearchText, value);
            }
        }
        
        #endregion

        #region Delegate Commands  
        public ICommand BtnCommand
        {
            get
            {
                return new Command<string>(OnClickButton);
            }
        }
        #endregion

        public PunchControlViewModel(INavigationService _navigationService,
           IUserDialogs _userDialogs,
           IHttpHelper _httpHelper,
           IMedia _media,
           ICheckValidLogin _checkValidLogin,
           IResizeImageService _resizeImageService,
           IRepository<T_CWPDrawings> _cwpDrawingsRepository,
           IRepository<T_IWPPunchControlItem> _iwpPunchControlItemRepository,
           IRepository<T_IWPPunchImage> _iwpPunchImagesRepository,
           IRepository<T_IWPPunchCategories> _iWPPunchCategoriesRepository,
           IRepository<T_IWPFunctionCodes> _iWPFunctionCodesRepository,
           IRepository<T_IWPCompanyCategoryCodes> _iWPCompanyCategoryCodesRepository,
           IRepository<T_IWPAdminPunchLayer> _iWPAdminPunchLayerRepository,
           IRepository<T_UserProject> _userProjectRepository,
           IRepository<T_UserDetails> _userDetailsRepository,
           IRepository<T_IWP> _iwpRepository,
           IRepository<T_IWPDrawings> _iwpDrawingsRepository,
           IRepository<T_IWPAttachments> _iwpAttachmentsRepository,
           IRepository<T_IWPControlLogSignatures> _iwpControlLogSignaturesRepository,
           IRepository<T_CwpTag> _CwpTAg
           ) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._resizeImageService = _resizeImageService;
            this._userDialogs = _userDialogs;
            this._media = _media;
            this._iwpPunchControlItemRepository = _iwpPunchControlItemRepository;
            this._iwpPunchImagesRepository = _iwpPunchImagesRepository;
            this._iWPPunchCategoriesRepository = _iWPPunchCategoriesRepository;
            this._iWPFunctionCodesRepository = _iWPFunctionCodesRepository;
            this._iWPCompanyCategoryCodesRepository = _iWPCompanyCategoryCodesRepository;
            this._iWPAdminPunchLayerRepository = _iWPAdminPunchLayerRepository;
            this._userProjectRepository = _userProjectRepository;
            this._userDetailsRepository = _userDetailsRepository;
            this._iwpRepository = _iwpRepository;
            this._iwpControlLogSignaturesRepository = _iwpControlLogSignaturesRepository;
            this._iwpDrawingsRepository = _iwpDrawingsRepository;
            this._iwpAttachmentsRepository = _iwpAttachmentsRepository;
            this._CwpTAg = _CwpTAg;
            _media.Initialize();
            this._cwpDrawingsRepository = _cwpDrawingsRepository;
            CameraItems = new List<string> {"Camera" };
            PageHeaderText = "Punch Control";
            PunchLayerGrid = Showbuttons = JobSettingHeaderVisible = true;
        }
        private async Task GetPunchLayerGridData()
        {
            var getIWP = await _iwpRepository.QueryAsync<T_IWP>("SELECT * FROM [T_IWP] WHERE [ID] = '" + IWPHelper.IWP_ID + "'");

            IWP = getIWP.FirstOrDefault();

            var UserDetailsList = await _userDetailsRepository.GetAsync();
            if (UserDetailsList.Count > 0)
                CurrentUserDetail = UserDetailsList.Where(p => p.ID == Settings.UserID).FirstOrDefault();

            var UserProjectList = await _userProjectRepository.GetAsync();
            if (UserProjectList.Count > 0)
                userProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();

            var AdminPunchLayer = await _iWPAdminPunchLayerRepository.QueryAsync<T_IWPAdminPunchLayer>("SELECT * FROM [T_IWPAdminPunchLayer] WHERE [ProjectID] = '" + Settings.ProjectID + "' ORDER BY [LayerNo] ASC");

            PunchLayersList = new ObservableCollection<T_IWPAdminPunchLayer>(AdminPunchLayer);
            SelectedPunchLayer = PunchLayersList.FirstOrDefault();

        }
        public async void UpdateGrid(string Option, IWPPunchListModel Punchitem)
        {
            if (Option == "CaptureImage")
            {               
                PunchControlGrid = PunchLayerGrid = PDFGrid = PunchCategoryGrid = false;
                CameraGrid = true;
            }
            else if (Option == "DeleteRow")
            {
                //Can Delete
                string deletePunch = " DELETE FROM [T_IWPPunchControlItem] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [IWPID] = '" + IWPHelper.IWP_ID 
                                   + "' AND [PunchID] = '" + Punchitem.PunchID + "'";
                await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(deletePunch);

                string deletepunchimage = " DELETE FROM [T_IWPPunchImage] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [IWPID] = '" + IWPHelper.IWP_ID 
                                        + "' AND [LinkedID] = '" + Punchitem.ID + "'";
                await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchImage>(deletepunchimage);

                await _userDialogs.AlertAsync("Punch has been deleted successfully.", "Delete Punch", "Ok");

                GeneratePunchDataTable(null);
            }
        }
        public async void LoadAddEditPunchTabsAsync(string param)
        {
            // Temporary code for hide and show grids
            PunchCategoryGrid = true;
            CameraGrid = PunchControlGrid = PDFGrid = PDFGrid = false;
            punchDescription = punchRemarks = null;


            string Categories = " SELECT DISTINCT * FROM [T_IWPPunchCategories] "
                              + " WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [SystemPunch] = 0 ORDER BY [Category] ASC";
            var Categorylist = await _iWPPunchCategoriesRepository.QueryAsync(Categories);
            CategoryList = new ObservableCollection<T_IWPPunchCategories>(Categorylist);


            string FunctionCodes = " SELECT * FROM [T_IWPFunctionCodes] "
                                 + " WHERE [ProjectID] = '" + Settings.ProjectID + "' ORDER BY [FunctionCode] ASC";
            var functionCodeList = await _iWPFunctionCodesRepository.QueryAsync(FunctionCodes);

            FunctionCodeList = new ObservableCollection<T_IWPFunctionCodes>(functionCodeList);

            string CompanyCategoryCode = " SELECT * FROM [T_IWPCompanyCategoryCodes] "
                                 + " WHERE [ProjectID] = '" + Settings.ProjectID + "' ORDER BY [CompanyCategoryCode] ASC";

            var CompanyCategoryCodeslist = await _iWPCompanyCategoryCodesRepository.QueryAsync(CompanyCategoryCode);
            CompanyCategoryCodes = new ObservableCollection<T_IWPCompanyCategoryCodes>(CompanyCategoryCodeslist);
        }

        private async void CreateNewPunch_Click(string param)
        {
            if (CurrentPunchPointer.Count > 0)
            {
                CurrentPunchPointer.Clear();
            }
           Task.Run(async () => await _userDialogs.AlertAsync("Please select two points for the punch", "Create New Punch", "Ok"));
                        IWPHelper.IsDrawVisible = true;  
        }              
        //private async Task GetCWPDrawingList()
        //{
        //    string SQL = "SELECT CD.CWPID, CD.DisplayName, COUNT(PCI.PunchID) AS PunchCount, MAX(PCI.[Status]) AS MaxStatus "
        //                + " FROM (T_CWPDrawings CD"
        //                + " LEFT OUTER JOIN T_IWPPunchControlItem PCI ON PCI.ProjectID = CD.Project_ID AND PCI.IWPID = CD.IWPID AND PCI.VMHub_DocumentsID = CD.VMHubID) "
        //                + " WHERE CD.[Project_ID] = '" + Settings.ProjectID + "'"
        //                + " AND CD.[IWPID] = '" + IWP.ID + "'"
        //                + " GROUP BY CD.CWPID, CD.DisplayName";

        //    var data = await _cwpDrawingsRepository.QueryAsync<IWPSpoolDrawingModel>(SQL);
        //    List<IWPSpoolDrawingModel> SpoolDrawing = new List<IWPSpoolDrawingModel>(data);
        //    int i = 0;
        //    foreach (IWPSpoolDrawingModel SpoolDrawingitem in SpoolDrawing)
        //    {
        //        SpoolDrawingitem.OrderNo = ++i;
        //        bool hasPunches = SpoolDrawingitem.PunchCount > 0;
        //        bool activePunches = SpoolDrawingitem.MaxStatus != null ? SpoolDrawingitem.MaxStatus.ToUpper() == "OPEN" : false;

        //        string drawingStatus = !hasPunches ? "No Punches" : (activePunches ? "Active Punches" : "All Punches Closed");

        //        SpoolDrawingitem.StatusImage = drawingStatus == "No Punches" ? "Grayradio.png" : (drawingStatus == "Active Punches" ? "Yellow.png" : "Greenradio.png");
        //    }

        //    DrawingList = new ObservableCollection<IWPSpoolDrawingModel>(SpoolDrawing);
        //}

        private async Task GetCWPDrawingList()
        {
            //var getIWP = Task.Run(async () => await _iwpRepository.QueryAsync<T_IWP>("SELECT * FROM [T_IWP] WHERE [ID] = '" + IWPHelper.IWP_ID + "'")).Result;

            //CurrentIWP = getIWP.FirstOrDefault();

            string SQLdrawing = "SELECT * FROM [T_IWPDrawings] WHERE [IWPID] = '" + IWPHelper.IWP_ID + "'";

            var Dlist = await _iwpDrawingsRepository.QueryAsync<T_IWPDrawings>(SQLdrawing);

            string SQLAttachment = "SELECT * FROM [T_IWPAttachments] WHERE [LinkedID] = '" + IWPHelper.IWP_ID + "'";

            var Alist = await _iwpAttachmentsRepository.QueryAsync<T_IWPAttachments>(SQLAttachment);

            foreach (T_IWPAttachments attached in Alist)
            {
                T_IWPDrawings addAttached = new T_IWPDrawings
                {
                    BinaryCode = attached.FileBytes,
                    FileName = attached.FileName,
                    JobCode = IWP.JobCode,
                    Name = Path.GetFileNameWithoutExtension(attached.FileName),
                    ProjectID = attached.ProjectID,
                    IWPID = attached.LinkedID,
                };
                Dlist.Add(addAttached);
            }
            //if (IsPDF)
            //    DrawingList = new ObservableCollection<T_IWPDrawings>(Dlist.Where(i => Path.GetExtension(i.FileName.ToLower()) == ".pdf"));
            // else
            DrawingList = new ObservableCollection<T_IWPDrawings>(Dlist.Where(i => Path.GetExtension(i.FileName.ToLower()) != ".pdf"));

        }

        private async void GeneratePunchDataTable(T_IWPDrawings drawingselected)
        {
            string SQL = "";
            if (drawingselected == null)
            {
                SQL = "SELECT [ID],[PunchID],[Status],[Category],[PunchAdminID],[Description],[VMHub_DocumentsID],[CWPID],[OtherComponent]"
               + " FROM [T_IWPPunchControlItem] "
               + " WHERE [ProjectID] = '" + Settings.ProjectID + "'" + " AND [IWPID] ='" + IWPHelper.IWP_ID + "'"
               // + " AND [VMHub_DocumentsID] = '" + SelectedDrawing. + "'" + " AND [PunchAdminID] = '" + SelectedPunchLayer.ID + "'"
               + " AND [PunchAdminID] = '" + SelectedPunchLayer.ID + "'"
               + " ORDER BY [PunchNo] DESC";
            }else
            {
                SQL = "SELECT [ID],[PunchID],[Status],[Category],[PunchAdminID],[Description],[VMHub_DocumentsID],[CWPID],[OtherComponent]"
                + " FROM [T_IWPPunchControlItem] "
                + " WHERE [ProjectID] = '" + Settings.ProjectID + "'" + " AND [IWPID] ='" + IWPHelper.IWP_ID + "'"
                 + " AND [VMHub_DocumentsID] = '" + SelectedDrawing.VMHub_DocumentsID + "'"
                + " AND [PunchAdminID] = '" + SelectedPunchLayer.ID + "'"
                + " ORDER BY [PunchNo] DESC";
            }

           

            var data = await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(SQL);
            List<IWPPunchListModel> punches = new List<IWPPunchListModel>();


            foreach (T_IWPPunchControlItem PLitem in data)
            {

                var imageSQL = "SELECT [IWPID] FROM [T_IWPPunchImage] "
                             + " WHERE [ProjectID] = '" + Settings.ProjectID + "'" + " AND [IWPID] = '" + IWPHelper.IWP_ID + "'"
                             + " AND [LinkedID] = '" + PLitem.ID + "'";

                //string imageSQL = " SELECT * FROM [T_IWPPunchImage] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [IWPID]= '" + 3369 + "' AND [LinkedID]= '" + 14 + "'";

                var Existimage = await _iwpPunchImagesRepository.QueryAsync<T_IWPPunchImage>(imageSQL);
                bool hasImages = Existimage.Count > 0 ? true : false;

                //Temporary added 
                // bool hasImages = false;

                string punchIDColor = "";
                if (PLitem.Category == "A")
                    punchIDColor = "#ff0000";
                else if (PLitem.Category == "B")
                    punchIDColor = "#0000ff";
                else if (PLitem.Category == "C")
                    punchIDColor = "#00ff00";


                string cameraImage = hasImages ? "Greencam.png" : "cam.png";
                //string deleteImage = (PLitem.TPCConfirmed || PLitem.Live) ? "" : "dlt2.png";

                //Temporary added 
                string deleteImage =  "dlt2.png";
                string tagsorothercomponent = await GetPunchOrOtherComponent(PLitem.CWPID);
                if (string.IsNullOrWhiteSpace(tagsorothercomponent)) tagsorothercomponent = PLitem.OtherComponent;

                punches.Add(new IWPPunchListModel
                {
                    ID = PLitem.ID,
                    PunchID = PLitem.PunchID,
                    Category = PLitem.Category,
                    Status = PLitem.Status,
                    HasImages = hasImages,
                    Camera = cameraImage,
                    Delete = deleteImage,
                    PunchAdminID = PLitem.PunchAdminID,
                    PunchIDColor = punchIDColor,
                    Description = PLitem.Description,
                    VMHub_DocumentsID = PLitem.VMHub_DocumentsID,
                    TagsOrOtherComponent = tagsorothercomponent,
                    CWPID = PLitem.CWPID


                }); ;
            }
            PunchesList = new ObservableCollection<IWPPunchListModel>(punches);
            PunchesListMemory = new ObservableCollection<IWPPunchListModel>(punches);

            TagsComponentList = new ObservableCollection<IWPPunchListModel>(punches.Where(x=>!string.IsNullOrWhiteSpace(x.TagsOrOtherComponent)));
            TagsComponentList = new ObservableCollection<IWPPunchListModel>(TagsComponentList.GroupBy(elem => elem.TagsOrOtherComponent).Select(group => group.First()));
           
        }

        private async Task<string> GetPunchOrOtherComponent(int cwpid)
        {
            if (cwpid == 0) return "";
            else { 
                //var Tagdata = Task(x=>x) await _CwpTAg.GetAsync(x => x.ID == cwpid.ToString());

                var Tagdata =  await _CwpTAg.GetAsync();
               
               var data = Tagdata.Where(x => x.ID == cwpid.ToString().Trim()).FirstOrDefault();
                if (data != null) return data.TagNo; else return "";
            }
        }


        private async void GeneratePunchDataTableBasedOnTagSelection(IWPPunchListModel SelectedPunchTAg)
        {
            string SQL = "";
            if (SelectedPunchTAg == null)
            {
                SQL = "SELECT [ID],[PunchID],[Status],[Category],[PunchAdminID],[Description],[VMHub_DocumentsID],[CWPID]"
               + " FROM [T_IWPPunchControlItem] "
               + " WHERE [ProjectID] = '" + Settings.ProjectID + "'" + " AND [IWPID] ='" + IWPHelper.IWP_ID + "'"
               // + " AND [VMHub_DocumentsID] = '" + SelectedDrawing. + "'" + " AND [PunchAdminID] = '" + SelectedPunchLayer.ID + "'"
               + " AND [PunchAdminID] = '" + SelectedPunchLayer.ID + "'"
               + " ORDER BY [PunchNo] DESC";
            }
            else if (SelectedPunchTAg.CWPID != 0)
            {
                SQL = "SELECT [ID],[PunchID],[Status],[Category],[PunchAdminID],[Description],[VMHub_DocumentsID],[CWPID]"
                + " FROM [T_IWPPunchControlItem] "
                + " WHERE [ProjectID] = '" + Settings.ProjectID + "'" + " AND [IWPID] ='" + IWPHelper.IWP_ID + "'"
                 + " AND [CWPID] = '" + SelectedPunchTAg.CWPID + "'" 
                + " AND [PunchAdminID] = '" + SelectedPunchLayer.ID + "'"
                + " ORDER BY [PunchNo] DESC";
            }
            else
            {

                SQL = "SELECT [ID],[PunchID],[Status],[Category],[PunchAdminID],[Description],[VMHub_DocumentsID],[CWPID]"
               + " FROM [T_IWPPunchControlItem] "
               + " WHERE [ProjectID] = '" + Settings.ProjectID + "'" + " AND [IWPID] ='" + IWPHelper.IWP_ID + "'"
                + " AND  [OtherComponent] = '" + SelectedPunchTAg.TagsOrOtherComponent + "'"
               + " AND [PunchAdminID] = '" + SelectedPunchLayer.ID + "'"
               + " ORDER BY [PunchNo] DESC";
            }



            var data = await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(SQL);
            List<IWPPunchListModel> punches = new List<IWPPunchListModel>();


            foreach (T_IWPPunchControlItem PLitem in data)
            {

                var imageSQL = "SELECT [IWPID] FROM [T_IWPPunchImage] "
                             + " WHERE [ProjectID] = '" + Settings.ProjectID + "'" + " AND [IWPID] = '" + IWPHelper.IWP_ID + "'"
                             + " AND [LinkedID] = '" + PLitem.ID + "'";

                //string imageSQL = " SELECT * FROM [T_IWPPunchImage] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [IWPID]= '" + 3369 + "' AND [LinkedID]= '" + 14 + "'";

                var Existimage = await _iwpPunchImagesRepository.QueryAsync<T_IWPPunchImage>(imageSQL);
                bool hasImages = Existimage.Count > 0 ? true : false;

                //Temporary added 
                // bool hasImages = false;

                string punchIDColor = "";
                if (PLitem.Category == "A")
                    punchIDColor = "#ff0000";
                else if (PLitem.Category == "B")
                    punchIDColor = "#0000ff";
                else if (PLitem.Category == "C")
                    punchIDColor = "#00ff00";


                string cameraImage = hasImages ? "Greencam.png" : "cam.png";
                //string deleteImage = (PLitem.TPCConfirmed || PLitem.Live) ? "" : "dlt2.png";

                //Temporary added 
                string deleteImage = "dlt2.png";
               // string tagsorothercomponent = await GetPunchOrOtherComponent(PLitem.CWPID);
              //  if (string.IsNullOrWhiteSpace(tagsorothercomponent)) tagsorothercomponent = PLitem.OtherComponent;

                punches.Add(new IWPPunchListModel
                {
                    ID = PLitem.ID,
                    PunchID = PLitem.PunchID,
                    Category = PLitem.Category,
                    Status = PLitem.Status,
                    HasImages = hasImages,
                    Camera = cameraImage,
                    Delete = deleteImage,
                    PunchAdminID = PLitem.PunchAdminID,
                    PunchIDColor = punchIDColor,
                    Description = PLitem.Description,
                    VMHub_DocumentsID = PLitem.VMHub_DocumentsID,
                    TagsOrOtherComponent = "",
                    CWPID = PLitem.CWPID

                });
            }
            PunchesList = new ObservableCollection<IWPPunchListModel>(punches);
            PunchesListMemory = new ObservableCollection<IWPPunchListModel>(punches);

          //  TagsComponentList = new ObservableCollection<IWPPunchListModel>(punches.Where(x => !string.IsNullOrWhiteSpace(x.TagsOrOtherComponent)));
        }
        private void GetSelectedpunchLayer()
        {
            if (SelectedPunchLayer == null)
                return;
            
            LoadPunchViewTabAsync();
            GeneratePunchDataTable(null);
        }
        private async void LoadPunchViewTabAsync()
        {
           
            //string SQL = "SELECT TL.ID, TL.DisplayName, TL.OrderNo, COUNT(PL.PunchID) AS PunchCount, MAX(PL.[Status]) AS MaxStatus "
            //            + " FROM (T_TestLimitDrawing TL"
            //            + " LEFT OUTER JOIN T_PunchList PL ON PL.ProjectID = TL.ProjectID AND PL.ETestPackageID = TL.ETestPackageID AND PL.VMHub_DocumentsID = TL.ID) "
            //            + " WHERE TL.[ProjectID] = '" + ETPSelected.ProjectID + "'"
            //            + " AND TL.[ETestPackageID] = '" + ETPSelected.ID + "'"
            //            + Filters[0]
            //            + " GROUP BY TL.ID, TL.DisplayName, TL.OrderNo "
            //            + Filters[1]
            //            + " ORDER BY TL.OrderNo ASC";

            //var data = await _testLimitDrawingRepository.QueryAsync<SpoolDrawingModel>(SQL);
            //List<SpoolDrawingModel> SpoolDrawing = new List<SpoolDrawingModel>(data);
            //foreach (SpoolDrawingModel SpoolDrawingitem in SpoolDrawing)
            //{
            //    bool hasPunches = SpoolDrawingitem.PunchCount > 0;
            //    bool activePunches = SpoolDrawingitem.MaxStatus != null ? SpoolDrawingitem.MaxStatus.ToUpper() == "OPEN" : false;

            //    string drawingStatus = !hasPunches ? "No Punches" : (activePunches ? "Active Punches" : "All Punches Closed");

            //    SpoolDrawingitem.StatusImage = drawingStatus == "No Punches" ? "Grayradio.png" : (drawingStatus == "Active Punches" ? "Yellowradio.png" : "Greenradio.png");
            //}

            //DrawingList = new ObservableCollection<SpoolDrawingModel>(SpoolDrawing);

        }
       
        private Boolean CheckNewPunchIsPopulated()
        {
            string[] liststring = new string[] { 
                                                 SelectedCategory != null ? SelectedCategory.ToString() : "",
                                                 SelectedCompanyCategoryCodes != null ? SelectedCompanyCategoryCodes.ToString() : "",
                                                 SelectedFunctionCode != null ? SelectedFunctionCode.ToString() : "",
                                                 punchDescription
                                                };
            foreach (string str in liststring)
            {
                if (str == null || str == "")
                    return false;
            }

            return true;
        }
        public async Task<bool> CheckPunchLayerIsComplete(int punchAdminID, string category, string functionCode)
        {
            //string UpdateQuery = " SELECT CLS.[Signed] FROM (((T_IWPControlLogSignatures as CLS "
            //             + " INNER JOIN T_IWPAdminControlLog ACL ON (CLS.ControlLogAdminID = ACL.ID) AND (CLS.ProjectID = ACL.ProjectID)) "
            //             + " INNER JOIN T_IWPPunchLayer  ACLPL ON (ACL.ID = ACLPL.AdminControlLog_ID) AND (ACL.ProjectID = ACLPL.ProjectID)) "
            //             + " INNER JOIN T_IWPPunchCategory ACLPC ON (ACLPL.ProjectID = ACLPC.ProjectID) AND (ACLPL.AdminControlLog_ID = ACLPC.ControlLogAdminID)) "
            //             + " INNER JOIN T_AdminControlLogActionParty ACPAP ON (ACLPC.ProjectID = ACPAP.ProjectID) AND (ACLPC.ControlLogAdminID = ACPAP.ControlLogAdminID) "

            //             + " WHERE CLS.[ProjectID] = '" + Settings.ProjectID + "' AND CLS.[IWP_ID] = '" + IWPHelper.IWP_ID + "' AND CLS.[Signed] = 1 "
            //             + " AND ACLPL.[PunchLayer] = '" + punchAdminID + "' AND ACLPC.[Category] = '" + category + "'"
            //             + " AND (ACL.[PunchesCompleted] = 1 OR ACL.[PunchesConfirmed] = 1) "
            //             + " AND  ACPAP.[FunctionCode] = '" + functionCode + "'";

            //var UpdatedControlLog = await _iwpControlLogSignaturesRepository.QueryAsync<T_IWPControlLogSignatures>(UpdateQuery);

            //return !UpdatedControlLog.Select(i => i.Signed).FirstOrDefault();

          return true;

        }
        private async Task<bool> CreatePunch(IWPPunchPointer punchPointer, string iwp, string category, string description, string remarks,
                                     string functionCode, string companyCategoryCode, bool signoffall, bool OnDocument)
        {
            bool result = true;
            try
            {
                T_IWPPunchControlItem newPunchitem = new T_IWPPunchControlItem();
                var ID = await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>("SELECT * FROM [T_IWPPunchControlItem] WHERE IWPID = "+ IWP.ID);
                if (ID.Count > 0)
                    newPunchitem.ID = ID.Select(i => i.ID).Max() + 1;
                else
                newPunchitem.ID = 1;
                newPunchitem.ProjectID = IWP.ProjectID;
                newPunchitem.IWPID = IWP.ID;
               // newPunchitem.CWPID = SelectedDrawing.CWPID;
                //newPunchitem.TestPackage = iwp;
                newPunchitem.VMHub_DocumentsID = Convert.ToInt32( PDFDrawing.VMHub_DocumentsID);
                newPunchitem.PunchAdminID = SelectedPunchLayer.ID;

                string punchNoSQL = " SELECT * FROM [T_IWPPunchControlItem] "
                                  + " WHERE[ProjectID] = '" + IWP.ProjectID + "'" + " AND [IWPID] = '" + IWP.ID + "'"
                                  + " AND [PunchAdminID] = '" + SelectedPunchLayer.ID + "'" + " ORDER BY [PunchNo] DESC";
                var pitem = await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(punchNoSQL);
                var punchitem = pitem.FirstOrDefault();
                if (punchitem != null)
                    newPunchitem.PunchNo = 1 + punchitem.PunchNo;
                else
                    newPunchitem.PunchNo = 1;

                newPunchitem.Category = category;
                newPunchitem.PunchID = SelectedPunchLayer.Prefix + "-" + newPunchitem.PunchNo.ToString("000") + "-" + category;
                newPunchitem.Description = description != null ? description : "";
                newPunchitem.Remarks = remarks != null? remarks : "";
                newPunchitem.FunctionCode = functionCode;
                newPunchitem.CompanyCategoryCode = companyCategoryCode;
                newPunchitem.OnDocument = OnDocument;
                newPunchitem.XPOS1 = Convert.ToInt32(punchPointer.FirstPoint.X);
                newPunchitem.YPOS1 = Convert.ToInt32(punchPointer.FirstPoint.Y);
                newPunchitem.XPOS2 = Convert.ToInt32(punchPointer.SecondPoint.X);
                newPunchitem.YPOS2 = Convert.ToInt32(punchPointer.SecondPoint.Y);

                newPunchitem.CreatedByUserID = CurrentUserDetail.ID;
                newPunchitem.CreatedBy = CurrentUserDetail.FullName;
                newPunchitem.CreatedOn = DateTime.Now.Date;

                newPunchitem.Updated = true;
                newPunchitem.UpdatedBy = CurrentUserDetail.FullName;
                newPunchitem.UpdatedByUserID = CurrentUserDetail.ID;
                newPunchitem.UpdatedOn = DateTime.Now.Date;
                newPunchitem.IsCreated = true;


                if (signoffall)
                {
                    newPunchitem.Status = "Closed";
                    //newPunchitem.TPCConfirmed = true;
                    //newPunchitem.TPCConfirmedByUserID = CurrentUserDetail.ID;
                    //newPunchitem.TPCConfirmedBy = CurrentUserDetail.FullName;
                    //newPunchitem.TPCConfirmedOn = DateTime.UtcNow;

                    newPunchitem.WorkCompleted = true;
                    newPunchitem.WorkCompletedByUserID = CurrentUserDetail.ID;
                    newPunchitem.WorkCompletedBy = CurrentUserDetail.FullName;
                    newPunchitem.WorkCompletedOn = DateTime.Now.Date;

                    newPunchitem.WorkConfirmed = true;
                    newPunchitem.WorkConfirmedByUserID = CurrentUserDetail.ID;
                    newPunchitem.WorkConfirmedBy = CurrentUserDetail.FullName;
                    newPunchitem.WorkConfirmedOn = DateTime.Now.Date;
                }
                else
                {
                    newPunchitem.Status = "Open";
                    
                    newPunchitem.WorkCompleted = false;
                    newPunchitem.WorkCompletedByUserID = 0;
                    newPunchitem.WorkCompletedBy = "";
                    newPunchitem.WorkCompletedOn = Convert.ToDateTime("01/01/2000 0:00");

                    newPunchitem.WorkConfirmed = false;
                    newPunchitem.WorkConfirmedByUserID = 0;
                    newPunchitem.WorkConfirmedBy = "";
                    newPunchitem.WorkConfirmedOn = Convert.ToDateTime("01/01/2000 0:00");

                }
                newPunchitem.WorkRejectedReason = "";
                newPunchitem.WorkRejectedBy = "";
                newPunchitem.CancelledBy = "";
                newPunchitem.DrawingRevision = "";
                newPunchitem.WorkRejectedOn= Convert.ToDateTime("01/01/2000 0:00");
                newPunchitem.CancelledOn = Convert.ToDateTime("01/01/2000 0:00");
                await _iwpPunchControlItemRepository.InsertOrReplaceAsync(newPunchitem);
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        private async Task PopulatePunchControlTabAsync(string punchID)
        {
            //TimeZoneInfo projectTimeZone = TimeZoneInfo.FindSystemTimeZoneById(CurrentProject.TimeZone);
            //Get all details
            string SQL = "SELECT * FROM [T_IWPPunchControlItem] WHERE [ProjectID] = '" + Settings.ProjectID + "'"
                       + " AND [IWPID] = '" + IWPHelper.IWP_ID + "' AND [PunchID] = '" + punchID + "'";
                      // + " AND [PunchAdminID] = '" + SelectedPunchOverview.PunchAdminID + "'";

            var AdminPunchLayer = await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(SQL);
            TimeZoneInfo projectTimeZone;
            ////PunchControlpandID = AdminPunchLayer.Select(i => i.PunchID).ToString();
            try
            { 
                projectTimeZone = TimeZoneInfo.FindSystemTimeZoneById(userProject.TimeZone);
            }
            catch 
            {
               projectTimeZone = TimeZoneInfo.Local;
            }
            
           
            foreach (T_IWPPunchControlItem PunchControlItem in AdminPunchLayer)
            {
                PunchControlpandID = PunchControlItem.PunchID;
                ControlDescription = PunchControlItem.Description;

                CreatedBy = PunchControlItem.CreatedBy;
                CreatedOn = ModsTools.AdjustDateTime(PunchControlItem.CreatedOn, projectTimeZone);

                UpdatedBy = PunchControlItem.UpdatedBy;
                UpdatedOn = string.Empty;
                if (PunchControlItem.UpdatedOn.ToString(AppConstant.DateFormat) == Convert.ToDateTime("01/01/2000 0:00").ToString(AppConstant.DateFormat)
                   || PunchControlItem.UpdatedOn.ToString(AppConstant.DateFormat) == Convert.ToDateTime("01/01/1900 0:00").ToString(AppConstant.DateFormat)
                   || PunchControlItem.UpdatedOn.ToString(AppConstant.DateFormat) == Convert.ToDateTime("01/01/0001 0:00").ToString(AppConstant.DateFormat))
                {
                    UpdatedOn = "";
                }
                else
                {
                    UpdatedOn = ModsTools.AdjustDateTime(PunchControlItem.UpdatedOn, projectTimeZone);
                }

                bool isIssuer = PunchControlItem.CreatedByUserID == userProject.User_ID;
                bool workCompletedBy = PunchControlItem.WorkCompletedByUserID == userProject.User_ID;
                bool workConfirmedBy = PunchControlItem.WorkConfirmedByUserID == userProject.User_ID;

                if (PunchControlItem.Cancelled)
                {
                    CancelledBy = PunchControlItem.CancelledBy;
                    CancelledOn = ModsTools.AdjustDateTime(PunchControlItem.CancelledOn, projectTimeZone);
                    CancelledImage = "Greenradio.png";
                }
                else
                {
                    CancelledBy = "";
                    CancelledOn = "";
                    CancelledImage = "Grayradio.png";
                }

                ControlRejectedReason = PunchControlItem.WorkRejectedReason;

                if (PunchControlItem.WorkCompleted)
                {
                    WorkCompletedBy = PunchControlItem.WorkCompletedBy;
                    WorkCompletedOn = ModsTools.AdjustDateTime(PunchControlItem.WorkCompletedOn, projectTimeZone);
                    WorkCompletedImage = "Greenradio.png";
                }
                else
                {
                    WorkCompletedBy = "";
                    WorkCompletedOn = "";
                    WorkCompletedImage = "Grayradio.png";
                }
                ModsTools.WorkCompletedEnabled = ((PunchControlItem.Cancelled == false && PunchControlItem.WorkCompleted == false) ||
                                                  (PunchControlItem.Cancelled == false && PunchControlItem.WorkCompleted == true &&
                                                  (isIssuer == true || workCompletedBy) && PunchControlItem.WorkConfirmed == false));



                if (PunchControlItem.WorkConfirmed)
                {
                    WorkConfirmedBy = PunchControlItem.WorkConfirmedBy;


                    WorkConfirmedOn = ModsTools.AdjustDateTime(PunchControlItem.WorkConfirmedOn, projectTimeZone);
                    WorkConfirmedImage = "Greenradio.png";
                }
                else
                {
                    WorkConfirmedBy = "";
                    WorkConfirmedOn = "";
                    WorkConfirmedImage = "Grayradio.png";
                }
                ModsTools.WorkConfirmedEnabled = ((PunchControlItem.Cancelled == false && PunchControlItem.WorkCompleted == true && PunchControlItem.WorkConfirmed == false)
                                                || (PunchControlItem.Cancelled == false && PunchControlItem.WorkCompleted == true && PunchControlItem.WorkConfirmed == true));

                var PunchControlLayer = await _iWPAdminPunchLayerRepository.QueryAsync<T_IWPAdminPunchLayer>("SELECT * FROM [T_IWPAdminPunchLayer] WHERE [ProjectID] = '" + Settings.ProjectID + "' ORDER BY [LayerNo] ASC");
                PunchControlLayersList = new ObservableCollection<T_IWPAdminPunchLayer>(PunchControlLayer);
            }

        }
        private async void btnCancelled_Click()
        {
            string messageCaption = "Mark As Cancelled";

            if (String.IsNullOrEmpty(WorkConfirmedBy))
            {
                if (String.IsNullOrEmpty(CancelledBy))
                {
                    if (await _userDialogs.ConfirmAsync("Are you sure you want to cancel this punch?", messageCaption, "Yes", "No"))
                    {
                        string sql = "SELECT [CreatedByUserID] FROM [T_IWPPunchControlItem] "
                                   + " WHERE [ProjectID] = '" + Settings.ProjectID + "'" + " AND [IWPID] ='" + IWPHelper.IWP_ID + "'" + " AND [PunchID] ='" + SelectedPunch.PunchID + "'";
                        var userCancelled = await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(sql);
                        int createdByUserID = userCancelled.Select(i => i.CreatedByUserID).FirstOrDefault();


                        if (Settings.UserID == createdByUserID)
                        {
                            try
                            {
                                string updateSql = "UPDATE [T_IWPPunchControlItem] SET [Status] = 'Cancelled' , [Cancelled] = 1, [CancelledBy] = '" + CurrentUserDetail.FullName + "'"
                                            + ", [CancelledByUserID] = '" + CurrentUserDetail.ID + "'" + ", [CancelledOn] = '" + Convert.ToDateTime(DateTime.UtcNow.ToString(AppConstant.DateSaveFormat)).Ticks + "'" + ", [Updated] = 1 "
                                            + " WHERE [ProjectID] = '" + Settings.ProjectID + "'" + " AND [IWPID] = '" + IWPHelper.IWP_ID + "'" + " AND [PunchID] = '" + SelectedPunch.PunchID + "'";
                                await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(updateSql);

                                await PopulatePunchControlTabAsync(SelectedPunch.PunchID);
                                UpdatedWorkPack(IWPHelper.IWP_ID);
                            }
                            catch (Exception ex)
                            {
                                await _userDialogs.AlertAsync("Error occured cancelling this punch", messageCaption, "Ok");
                            }
                        }
                        else
                            await _userDialogs.AlertAsync("Sorry, only the punch issuer " + CreatedBy + " can cancel this punch", messageCaption, "Ok");
                    }
                }
                else
                    await _userDialogs.AlertAsync("This has already been cancelled.", messageCaption, "Ok");
            }

            else
                await _userDialogs.AlertAsync("This punch's work has already been confirmed.", messageCaption, "Ok");
        }

        private async void btnWorkCompleted_Click()
        {
            bool WorkCompletedAlready = (WorkCompletedBy != string.Empty);

            string messageCaption = "Mark As Work Completed";

            if (WorkCompletedAlready)
            {
                messageCaption = "Remove Work Completed";
            }

            if (String.IsNullOrEmpty(WorkConfirmedBy))
            {
                if (String.IsNullOrEmpty(CancelledBy))
                {
                    if (ModsTools.WorkCompletedEnabled)
                    {

                        string functionCode = "", companyCategoryCode = "";
                        Boolean isIssuer = false;
                        string sql = "SELECT * FROM [T_IWPPunchControlItem]"
                                    + " WHERE [ProjectID] = '" + Settings.ProjectID + "'" + " AND [IWPID] = '" + IWPHelper.IWP_ID + "'" + " AND[PunchID] = '" + SelectedPunch.PunchID + "'";
                        var userCompleted = await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(sql);

                        foreach (T_IWPPunchControlItem uCompleted in userCompleted)
                        {
                            functionCode = uCompleted.FunctionCode;
                            companyCategoryCode = uCompleted.CompanyCategoryCode;
                            isIssuer = uCompleted.CreatedByUserID == CurrentUserDetail.ID ? true : false;
                        }

                        if ((CurrentUserDetail.Company_Category_Code.ToUpper() == companyCategoryCode.ToUpper() && CurrentUserDetail.Function_Code.ToUpper() == functionCode.ToUpper()) || isIssuer == true)
                        {
                            try
                            {
                                string status = "Open";
                                string WkCompleted = WorkCompletedAlready ? "0" : "1";
                                string WkCompletedBy = WorkCompletedAlready ? "" : CurrentUserDetail.FullName;
                                string WkCompletedByUserID = WorkCompletedAlready ? "0" : CurrentUserDetail.ID.ToString();
                                DateTime WkConfirmedOn = WorkCompletedAlready ? Convert.ToDateTime("01/01/2000 0:00") : Convert.ToDateTime(DateTime.Now.ToString(AppConstant.DateSaveFormat));

                                string updateSQL = " UPDATE [T_IWPPunchControlItem] SET [Status] = '" + status + "'" + ", [WorkCompleted] = '" + WkCompleted + "'"
                                                  + ", [WorkCompletedBy] ='" + WkCompletedBy + "'" + ", [WorkCompletedByUserID] = '" + WkCompletedByUserID + "'"
                                                  + ", [WorkCompletedOn] = '" + WkConfirmedOn.Ticks + "', [Updated] = 1 , [WorkRejected] = 0"
                                                  + " WHERE [ProjectID] = '" + Settings.ProjectID + "'" + " AND [IWPID] = '" + IWPHelper.IWP_ID + "'" + " AND [PunchID] = '" + SelectedPunch.PunchID + "'";

                                await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(updateSQL);
                                await PopulatePunchControlTabAsync(SelectedPunch.PunchID);
                                UpdatedWorkPack(IWPHelper.IWP_ID);
                            }
                            catch (Exception)
                            {
                                await _userDialogs.AlertAsync("Error occured completing the work for this punch", messageCaption, "OK");
                            }

                        }
                        else
                            await _userDialogs.AlertAsync("Sorry this punch's work assignment does not match your user settings.", messageCaption, "OK");
                    }
                    else
                        await _userDialogs.AlertAsync("Sorry you do not have permission to change this.", messageCaption, "OK");
                }
                else
                    await _userDialogs.AlertAsync("This has already been cancelled.", messageCaption, "OK");
            }
            else
                await _userDialogs.AlertAsync("This punch's work has already been confirmed.", messageCaption, "OK");
        }

        private async void btnWorkConfirmed_Click()
        {
            bool WorkConfirmedAlready = (WorkConfirmedBy != string.Empty);

            string messageCaption = "Mark As Work Confirmed";


            if (WorkConfirmedAlready)
            {
                messageCaption = "Remove Work Confirmed";
            }

            if (!String.IsNullOrEmpty(WorkCompletedBy))
            {
                if (String.IsNullOrEmpty(CancelledBy))
                {
                    if (ModsTools.WorkConfirmedEnabled)
                    {
                        if (ModsTools.CheckCanAddPunch_WP(SelectedPunchLayer, CurrentUserDetail))
                        {
                            try
                            {
                                string status = WorkConfirmedAlready ? "Open" : "Closed";
                                string WkConfirmed = WorkConfirmedAlready ? "0" : "1";
                                string WkConfirmedBy = WorkConfirmedAlready ? "" : CurrentUserDetail.FullName;
                                string WkConfirmedByUserID = WorkConfirmedAlready ? "0" : CurrentUserDetail.ID.ToString();
                                DateTime WkConfirmedOn = WorkConfirmedAlready ? Convert.ToDateTime("01/01/1900 0:00") : Convert.ToDateTime(DateTime.UtcNow.ToString(AppConstant.DateSaveFormat));

                                string updateSQL = " UPDATE [T_IWPPunchControlItem] SET [Status] = '" + status + "'" + ", [WorkConfirmed] = '" + WkConfirmed + "'"
                                                 + ", [WorkConfirmedBy] = '" + WkConfirmedBy + "', [WorkConfirmedByUserID] = '" + WkConfirmedByUserID + "'"
                                                 + ", [WorkConfirmedOn] = '" + WkConfirmedOn.Ticks + "', [Updated] = 1 "
                                                 + " WHERE [ProjectID] = '" + Settings.ProjectID + "'" + " AND [IWPID] = '" + IWPHelper.IWP_ID + "'" + " AND [PunchID] = '" + SelectedPunch.PunchID + "'";
                                await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(updateSQL);
                                await PopulatePunchControlTabAsync(SelectedPunch.PunchID);
                                UpdatedWorkPack(IWPHelper.IWP_ID);
                            }
                            catch (Exception e)
                            {
                                await _userDialogs.AlertAsync("Error occured confirming the work for this punch", messageCaption, "Ok");
                            }
                        }
                        else
                            await _userDialogs.AlertAsync("Sorry you do not have the correct user rights to confirm this punch's work.", messageCaption, "Ok");
                    }
                    else
                        await _userDialogs.AlertAsync("Sorry you do not have permission to change this.", messageCaption, "Ok");
                }
                else
                    await _userDialogs.AlertAsync("This has already been cancelled.", messageCaption, "Ok");
            }
            else
                await _userDialogs.AlertAsync("This punch's work has not yet been completed.", messageCaption, "Ok");
        }

        private async void btnWorkRejected_Click()
        {
            bool WorkCompletedAlready = (WorkCompletedBy != string.Empty);
            bool WorkConfirmedAlready = (WorkConfirmedBy != string.Empty);

            DateTime datetimeNow = DateTime.UtcNow;
            DateTime completedDate = DateTime.MinValue;
            string completedBy = "";
            string functionCode = "", companyCategoryCode = "";

            string sql = "SELECT * FROM [T_IWPPunchControlItem] "
                       + " WHERE [ProjectID] = '" + Settings.ProjectID + "'" + " AND [IWPID] ='" + IWPHelper.IWP_ID + "'" + " AND [PunchID] ='" + SelectedPunch.PunchID + "'";
            var WkRejected = await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(sql);
            foreach (T_IWPPunchControlItem Rejected in WkRejected)
            {
                completedBy = Rejected.WorkCompletedBy;
                completedDate = Rejected.WorkCompletedOn;
            }


            if (!WorkConfirmedAlready)
            {
                if (ControlRejectedReason != string.Empty)
                {
                    //Check if the user can add/confirm work for punches on this punch layer
                    Boolean canSign = ModsTools.CheckCanAddPunch_WP(SelectedPunchLayer, CurrentUserDetail);

                    //If not then check if the work for the punch has been assigned to them
                    if (!canSign)
                    {
                        string PunchListsql = "SELECT * FROM [T_IWPPunchControlItem] "
                                    + " WHERE [ProjectID] = '" + Settings.ProjectID + "'" + " AND [IWPID] ='" + IWPHelper.IWP_ID + "'" + " AND [PunchID] ='" + SelectedPunch.PunchID + "'";
                        var PunchList = await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(PunchListsql);
                        foreach (T_IWPPunchControlItem Punchitem in WkRejected)
                        {
                            functionCode = Punchitem.FunctionCode;
                            companyCategoryCode = Punchitem.CompanyCategoryCode;
                        }


                        if (CurrentUserDetail.Company_Category_Code.ToUpper() == companyCategoryCode.ToUpper() && CurrentUserDetail.Function_Code.ToUpper() == functionCode.ToUpper())
                            canSign = true;
                    }

                    if (canSign) //Can sign off work completed
                    {

                        if (!WorkCompletedAlready)
                        {
                            await _userDialogs.AlertAsync("Sorry the work must be completed in order to reject it.", "Mark As Work Completed", "Ok");
                            return;
                        }
                        else
                        {
                            try
                            {
                                string WkCompletedBy = "";
                                DateTime WkCompletedOn = Convert.ToDateTime("01/01/2000 0:00");
                                DateTime WkRejectedOn = DateTime.Now.Date;

                                DateTime WkRejectedCompletedOn = DateTime.Now.Date;


                                string updateSQL = " UPDATE [T_IWPPunchControlItem] SET [Status] = '" + "Open" + "'" + " , [WorkCompleted] = 0"
                                                  + ", [WorkCompletedBy] ='" + WkCompletedBy + "'" + ", [WorkCompletedByUserID] = 0"
                                                  + ", [WorkCompletedOn] = '" + WkCompletedOn.Ticks + "', [Updated] = 1 , [WorkRejected] = '1', [WorkRejectedByUserID] = '" + CurrentUserDetail.ID + "'"
                                                  + ", [WorkRejectedOn] ='" + WkRejectedOn.Ticks + "'" + ",[WorkRejectedReason] = '" + ControlRejectedReason + "'"
                                                  + ", [WorkRejectedBy] ='" + completedBy + "'" + ",[WorkRejectedOn] = '" + WkRejectedCompletedOn.Ticks + "'"
                                                  + " WHERE [ProjectID] = '" + Settings.ProjectID + "'" + " AND [IWPID] = '" + IWPHelper.IWP_ID + "'" + " AND [PunchID] = '" + SelectedPunch.PunchID + "'";
                                await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(updateSQL);
                                UpdatedWorkPack(IWPHelper.IWP_ID);
                                await PopulatePunchControlTabAsync(SelectedPunch.PunchID);
                               // MarkETestPackageAsUpdated();
                            }
                            catch (Exception e)
                            {
                                await _userDialogs.AlertAsync("Error occured completing the work for this rejected punch", "Error saving", "Ok");
                            }
                        }
                    }
                    else
                        await _userDialogs.AlertAsync("Sorry you do not have the correct user rights to reject this punch.", "Error saving", "Ok");
                }
                else
                    await _userDialogs.AlertAsync("Sorry you must enter a rejected reason before rejecting the work.", "Rejected reason box blank", "Ok");
            }
            else
                await _userDialogs.AlertAsync("Sorry you cannot reject work that has been confirmed, only completed.", "Error", "Ok");

        }


        #region Public
        public async void OnClickButton(string param)
        {
            if (param == "NewPunch")
            {
                 CreateNewPunch_Click(param);               
            }
            else if (param == "BackFromPunchControl")
            {
                GeneratePunchDataTable(null);
                PunchControlGrid = PDFGrid = PunchCategoryGrid = false;
                PunchLayerGrid = true;
            }
            else if (param == "Back")
            {
                // _ = GetCWPDrawingList();
                // GeneratePunchDataTable();
                PunchLayerGrid = PDFGrid = PunchCategoryGrid = false;
                PunchControlGrid = true;
                
            }
            else if (param == "BackOnPunchLayerGrid")
            {
                CameraGrid = PunchControlGrid = PDFGrid = PunchCategoryGrid = false;
                PunchLayerGrid = true;

            }
            else if (param == "ColorPicker")
            {
                await Navigation.PushPopupAsync(new ColorSelectionPopup());
            }
            else if (param == "BackfromCameraGrid")
            {
                GeneratePunchDataTable(null);
                PunchControlGrid = CameraGrid = PDFGrid = PunchCategoryGrid = false;
                PunchLayerGrid = true;
            }
            else if (param == "Cancelled")
            {
                btnCancelled_Click();
            }
            else if (param == "WorkCompleted")
            {
                btnWorkCompleted_Click();
            }
            else if (param == "WorkConfirmed")
            {
                btnWorkConfirmed_Click();
            }
            else if (param == "RejectedReason")
            {
                btnWorkRejected_Click();
            }
            else if(param == "PunchSerach")
            {
                if (string.IsNullOrWhiteSpace(PunchSearchText))
                {
                    if(PunchesListMemory!= null)
                    PunchesList = PunchesListMemory;
                }
                else 
                {
                    PunchesList = new ObservableCollection<IWPPunchListModel>(PunchesListMemory.Where(x=>x.PunchID.ToLower().Contains(PunchSearchText.ToLower()) || x.Description.ToLower().Contains(PunchSearchText.ToLower()) || x.Status.ToLower().Contains(PunchSearchText.ToLower())));
                }
                 
            }
            else if (param == "PunchShowAll")
            {
                GeneratePunchDataTable(null);

            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            Task tsk1 = GetPunchLayerGridData();
            await Task.WhenAll(tsk1);
            Task tsk2 = GetCWPDrawingList();
           string Categories = " SELECT DISTINCT * FROM [T_IWPPunchCategories] "
                              + " WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [SystemPunch] = 0 ORDER BY [Category] ASC";
            var Categorylist = await _iWPPunchCategoriesRepository.QueryAsync(Categories);
            PunchCategories = new ObservableCollection<T_IWPPunchCategories>(Categorylist);


        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        public async void PopUp(string id)
        {
            IWPPunchListModel current = PunchesList.Where(i => i.PunchID == id).FirstOrDefault();
            SelectedPunch = current;
            GetPopUp(true);
        }
        public async void GetPopUp(bool PopUP)
        {
            try
            {
                string punchID = "";
                if (PopUP)
                {
                    punchID = SelectedPunch.PunchID;

                    string SQL = "SELECT CD.DisplayName, PCI.PunchID, PCI.Description, PCI.Status, PCI.Updated, PCI.PunchAdminID, PCI.Category, APL.LayerName"
                       + " FROM((T_CWPDrawings CD"
                       + " LEFT OUTER JOIN T_IWPPunchControlItem PCI ON CD.Project_ID = PCI.ProjectID AND CD.IWPID = PCI.IWPID AND CD.VMHubID = PCI.VMHub_DocumentsID)"
                       + " LEFT OUTER JOIN T_IWPAdminPunchLayer APL ON PCI.PunchAdminID = APL.ID) "
                       + " WHERE CD.[Project_ID] = '" + Settings.ProjectID + "'"
                       + " AND CD.[IWPID] = '" + IWP.ID + "'"
                       + " AND PCI.PunchID = '" + punchID + "'"
                       + " ORDER BY PCI.[PunchID] ASC ";

                    var data = await _cwpDrawingsRepository.QueryAsync<PunchControlLayerModel>(SQL);
                    SelectedPunchControlLayer = data.Select(i => i).FirstOrDefault();
                }
                else
                    return;
                //    punchid = selectedpunchoverview.punchid;
                ////currentpagehelper.punchid = punchid;

                //if (string.isnullorempty(punchid))
                //    return;


                
                ///////////

                string PunchListSQL = "SELECT * FROM [T_IWPPunchControlItem] "
                       + " WHERE [ProjectID] = '" + Settings.ProjectID + "'"
                       + " AND [IWPID] ='" + IWPHelper.IWP_ID + "'"
                       + " AND [PunchID] ='" + SelectedPunch.PunchID + "'";
                      // + " AND [PunchAdminID] ='" + SelectedPunchOverview.PunchAdminID + "'";

            var APunchLayer = await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(PunchListSQL);

            T_IWPPunchControlItem AdminPunchLayer = APunchLayer.FirstOrDefault();

                string AdminFunctionCodeSQL = "SELECT DISTINCT (Description) FROM [T_IWPFunctionCodes] "
                           + " WHERE [ProjectID] = '" + IWP.ProjectID + "'"
                           + " AND [FunctionCode] ='" + AdminPunchLayer.FunctionCode + "'";

                var FunCodeDescr = await _iWPFunctionCodesRepository.QueryAsync<T_IWPFunctionCodes>(AdminFunctionCodeSQL);
                string FunctionCodeDescription = "";
                if (FunCodeDescr.Count > 0)
                    FunctionCodeDescription = FunCodeDescr.Select(i => i.Description).FirstOrDefault();

                string companyNote = AdminPunchLayer.CompanyCategoryCode == "M" ? "Main Contractor" : (AdminPunchLayer.CompanyCategoryCode == "S" ? "Sub Contractor" : "Client");


                //IF ONLY TPC CONFIRMED
                string message = "Description" + Environment.NewLine + AdminPunchLayer.Description + Environment.NewLine + Environment.NewLine + ""
                               + "Assigned To : " + companyNote + ", " + FunctionCodeDescription + Environment.NewLine +
                                 (AdminPunchLayer.Cancelled ? "Cancelled" : (Environment.NewLine
                               + "Work Completion : " + (AdminPunchLayer.WorkCompleted ? "Completed" : "Outstanding") + Environment.NewLine
                               + "Work Confirmed : " + (AdminPunchLayer.WorkConfirmed ? "Confirmed" : "Outstanding")));

                /*  string message = "Description" + Environment.NewLine + "Description" + Environment.NewLine + Environment.NewLine + ""
                      + "Assigned To : " + "companyNote" + ", " + "FunctionCodeDescription" + Environment.NewLine
                      + " Cancelled "+ Environment.NewLine
                       + " Work Completion : WorkCompleted " + Environment.NewLine
                       + " Work Confirmed : Confirmed" ;    
                */

                //if (AdminPunchLayer.TPCConfirmed)
                //{
                    if (await _userDialogs.ConfirmAsync(message + Environment.NewLine + "" + Environment.NewLine + "" + "Do You Want To Jump To Punch Control?", punchID + " Punch Details", "Yes", "No"))
                    {
                        CurrentPageHelper.PunchID = punchID;
                        PunchControlGrid = true;
                        PunchLayerGrid = false;
                        await PopulatePunchControlTabAsync(AdminPunchLayer.PunchID);
                        //tcETestPackage.SelectedIndex = 5;
                    }
                    //else
                    //    CurrentPageHelper.PunchID = string.Empty;
                //}
                //else
                //    await _userDialogs.AlertAsync(message, punchID + " Punch Details", "Ok");
            }
            catch (Exception e)
            {

            }

        }

        public bool SavePunch_Click()
        {
            
            //check if punch exists first else
            bool punchExists = false;
            T_IWPPunchControlItem PunchItem = new T_IWPPunchControlItem();

            if (CurrentPageHelper.PunchID != null)
            {
                string punchID = CurrentPageHelper.PunchID; //SelectedPunch.PunchID;
                string sql = "SELECT * FROM [T_IWPPunchControlItem] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [IWPID] = '"
                           + IWPHelper.IWP_ID + "' AND [PunchAdminID] = '" + SelectedPunchLayer.ID + "' AND [PunchID] = '" + punchID + "'";

                var data = Task.Run(async () => await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(sql)).Result;

                PunchItem = data.FirstOrDefault();
            }
            if (punchExists)
            {

                string PunchIDNew = SelectedPunchLayer.Prefix + "-" + PunchItem.PunchNo.ToString("000") + "-" + SelectedCategory.Category;

                string updateQuery = " UPDATE [T_IWPPunchControlItem] SET Category ='" + SelectedCategory.Category + "' , Description ='" + punchDescription + "'"
                                   + " , Remarks ='" + punchRemarks + "',FunctionCode ='" + SelectedFunctionCode.FunctionCode + "' ,CompanyCategoryCode='" + SelectedCompanyCategoryCodes.CompanyCategoryCode + "'"
                                   //  + " ,PCWBS ='" + PCBWBS + "', Updated ='" + 1 + "', UpdatedBy ='" + CurrentUserDetail.FullName + "'"
                                   //  + " ,UpdatedOn ='" + Convert.ToDateTime(DateTime.UtcNow.ToString(AppConstant.DateSaveFormat)).Ticks + "' ,PresetPunchID ='" + PresetPunchID + "'"
                                   + " ,PunchID ='" + PunchItem.PunchID + "'  WHERE [ID] = '" + PunchItem.ID + "'";

                var Updateddata = Task.Run(async () => await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>(updateQuery)).Result;

                if (Updateddata.Count > 0)
                {
                    CurrentPunchPointer.Clear();
                    Task.Run(async () => await _userDialogs.AlertAsync("Punch updated.", "Save", "Ok"));
                }
                return true;


            }
            else if (CheckNewPunchIsPopulated())
            {
                string category = SelectedCategory.ToString();
                string functionCode = SelectedFunctionCode.FunctionCode.ToString();

                if (Task.Run(async () => await CheckPunchLayerIsComplete(SelectedPunchLayer.ID, category, functionCode)).Result)
                {
                    //PunchItem punchItem = new PunchItem();
                    T_IWPPunchControlItem punchItem = new T_IWPPunchControlItem();

                    Boolean savedPunches = true;
                    for (int i = 0; i <= (IWPHelper.PathPoints.Count - 1); i += 2)   //CurrentPunchPointer.Count
                    {
                        Point fp = new Point(IWPHelper.PathPoints[i].Points[0].X, IWPHelper.PathPoints[i].Points[0].Y);
                        Point sp = new Point(IWPHelper.PathPoints[i + 1].Points[0].X, IWPHelper.PathPoints[i + 1].Points[0].Y);
                        CurrentPunchPointer = new List<IWPPunchPointer> { new IWPPunchPointer { FirstPoint = fp, SecondPoint = sp } };

                        if (Task.Run(async () => await CreatePunch(CurrentPunchPointer[0], IWP.Title, category, punchDescription, punchRemarks, functionCode,
                            SelectedCompanyCategoryCodes.CompanyCategoryCode, false, true)).Result)
                        {

                            UpdatedWorkPack(IWP.ID);
                        }
                        else
                        {
                            savedPunches = false;
                            Task.Run(async () => await _userDialogs.AlertAsync("Error occured punch not saved.", "Save Punch", "Ok"));
                            return false;
                        }
                    }
                    if (savedPunches)
                    {
                        CurrentPunchPointer.Clear();
                    }
                    return true;
                }
                else 
                {
                    Task.Run(async () => await _userDialogs.AlertAsync("Unable to add this punch as category " + category + " punches have been marked as completed", "Save Punch", "Ok"));
                    return true;
                }

            }
            else 
            {
                Task.Run(async () => await _userDialogs.AlertAsync("Not all information has been completed, please fill out all required fields before saving.", "Save Punch", "Ok"));
                return false;
            }


        }
        public void LoadPunchLayerImageAsync(string pdfname)
        {
            if (SelectedDrawing != null)
            {
                SelectedPDF = SelectedDrawing.Name;
                PunchControlGrid= PunchLayerGrid = PunchCategoryGrid = false;
                PDFGrid = true;
            }
            else
            {
                SelectedDrawing = SelectedDrawing = DrawingList.Where(i => i.Name == pdfname).FirstOrDefault();
                PunchControlGrid= PunchLayerGrid = PunchCategoryGrid = false;
                PDFGrid = true;
            }
        }
        private async void UpdatedWorkPack(int IWP)
        {
            string SQL = "UPDATE [T_IWP] SET [Updated] = 1 WHERE [ID] = '" + IWP + "'";
            var result = await _iwpRepository.QueryAsync(SQL);
        }
        public async Task<byte[]> ResizeImage(byte[] imageAsByte)
        {
            return await _resizeImageService.GetResizeImage(imageAsByte);
        }
        #endregion
    }
}

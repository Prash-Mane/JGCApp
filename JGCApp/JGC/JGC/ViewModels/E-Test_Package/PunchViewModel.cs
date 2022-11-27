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
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System;
using SkiaSharp;
using System.IO;
using SkiaSharp.Views.Forms;
using JGC.UserControls.Touch;
using static JGC.Common.Enumerators.Enumerators;
using JGC.UserControls.Zoom;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using JGC.UserControls.PopupControls.ColorSelection_CustomColor;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using JGC.UserControls.PopupControls;

namespace JGC.ViewModels.E_Test_Package
{

    public class PunchViewModel : BaseViewModel
    {

        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IResizeImageService _resizeImageService;
        private readonly IRepository<T_AdminPunchLayer> _adminPunchLayerRepository;
        private readonly IRepository<T_TestLimitDrawing> _testLimitDrawingRepository;
        private readonly IRepository<T_PunchList> _punchListRepository;
        private readonly IRepository<T_PunchImage> _punchImageRepository;
        private readonly IRepository<T_AdminFunctionCodes> _adminFunctionCodesRepository;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private readonly IRepository<T_ETestPackages> _eTestPackagesRepository;
        private readonly IRepository<T_ControlLogSignature> _controlLogSignatureRepository;
        private readonly IRepository<T_AdminPresetPunches> _adminPresetPunchesRepository;
        private readonly IRepository<T_AdminPunchCategories> _adminPunchCategoriesRepository;

        SKBitmap Bitmap = new SKBitmap();
        TouchManipulationBitmap bitmap;
        List<long> touchIds = new List<long>();
        SKCanvas _saveBitmapCanvas;
        static List<PunchPointer> CurrentPunchPointer = new List<PunchPointer>();

        // Declare SKPaint 
        Dictionary<long, SKPath> _inProgressPaths = new Dictionary<long, SKPath>();
        List<SKPath> _completedPaths = new List<SKPath>();
        MatrixDisplay matrixDisplay = new MatrixDisplay();
        public bool _drawLine, _drawDot, _drawFinger, _zoom;
        string InspectionPath;
        private readonly IMedia _media;
        private byte[] imageAsByte = null;
        public INavigation Navigation { get; }

        SKImageInfo info;
        SKSurface surface;
        SKCanvas canvas;
        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Red,
            StrokeWidth = Device.RuntimePlatform == Device.UWP ? 5 : 10,
        };

        #region Properties
        public T_UserDetails CurrentUserDetail;
        private T_UserProject userProject;
        private T_ETestPackages ETPSelected;
        public T_ETestPackages SelectedETP
        {
            get { return ETPSelected; }
            set { ETPSelected = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_AdminPunchLayer> punchLayersList;
        public ObservableCollection<T_AdminPunchLayer> PunchLayersList
        {
            get { return punchLayersList; }
            set { punchLayersList = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_AdminPunchLayer> punchControlLayersList;
        public ObservableCollection<T_AdminPunchLayer> PunchControlLayersList
        {
            get { return punchControlLayersList; }
            set { punchControlLayersList = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<SpoolDrawingModel> drawingList;
        public ObservableCollection<SpoolDrawingModel> DrawingList
        {
            get { return drawingList; }
            set { drawingList = value; RaisePropertyChanged(); }
        }

        private SpoolDrawingModel selectedDrawing;
        public SpoolDrawingModel SelectedDrawing
        {
            get { return selectedDrawing; }
            set
            {
                if (SetProperty(ref selectedDrawing, value))
                {
                    // OnClickButton("OpenPDF");
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<PunchListModel> punchesList;
        public ObservableCollection<PunchListModel> PunchesList
        {
            get { return punchesList; }
            set { punchesList = value; RaisePropertyChanged(); }
        }

        private PunchListModel selectedPunch;
        public PunchListModel SelectedPunch
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
        private T_AdminPunchLayer selectedpunchLayer;
        public T_AdminPunchLayer SelectedpunchLayer
        {
            get { return selectedpunchLayer; }
            set
            {
                if (SetProperty(ref selectedpunchLayer, value))
                {
                    GetSelectedpunchLayer(true);
                    OnPropertyChanged();
                }
            }
        }
        private T_AdminPunchLayer selectedpunchControlLayer;
        public T_AdminPunchLayer SelectedpunchControlLayer
        {
            get { return selectedpunchControlLayer; }
            set
            {
                SetProperty(ref selectedpunchControlLayer, value);
            }
        }
        private PunchOverview selectedPunchOverview;
        public PunchOverview SelectedPunchOverview
        {
            get { return selectedPunchOverview; }
            set
            {
                if (SetProperty(ref selectedPunchOverview, value))
                {
                    //GotoPunchView(selectedPunchOverview);
                    OnPropertyChanged();
                }
            }
        }

        private List<string> punchTypeList;
        public List<string> PunchTypeList
        {
            get { return punchTypeList; }
            set
            {
                SetProperty(ref punchTypeList, value);
            }
        }
        private string selectedpunchType;
        public string SelectedpunchType
        {
            get { return selectedpunchType; }
            set
            {
                if (SetProperty(ref selectedpunchType, value))
                {
                    OnClickButton("LoadPunchDescriptionList");
                    OnPropertyChanged();
                }
            }
        }

        private List<string> presetpunchTypeList;
        public List<string> PresetPunchTypeList
        {
            get { return presetpunchTypeList; }
            set
            {
                SetProperty(ref presetpunchTypeList, value);
            }
        }
        private string selectedpresetpunchType;
        public string SelectedpresetpunchType
        {
            get { return selectedpresetpunchType; }
            set
            {
                SetProperty(ref selectedpresetpunchType, value);
            }
        }

        private List<string> presetDescriptionList;
        public List<string> PresetDescriptionList
        {
            get { return presetDescriptionList; }
            set
            {
                SetProperty(ref presetDescriptionList, value);
            }
        }
        private string selectedPresetDescription;
        public string SelectedPresetDescription
        {
            get { return selectedPresetDescription; }
            set
            {
                SetProperty(ref selectedPresetDescription, value);
              
            }
        }
           

        private List<string> presetCategoryList;
        public List<string> PresetCategoryList
        {
            get { return presetCategoryList; }
            set
            {
                SetProperty(ref presetCategoryList, value);
            }
        }
        private string selectedPresetCategory;
        public string SelectedPresetCategory
        {
            get { return selectedPresetCategory; }
            set
            {
                SetProperty(ref selectedPresetCategory, value);
            }
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

        private string _PCBWBS;
        public string PCBWBS
        {
            get { return _PCBWBS; }
            set
            {
                SetProperty(ref _PCBWBS, value);
            }
        }
        private string _SpoolDrawingText;
        public string SpoolDrawingText
        {
            get { return _SpoolDrawingText; }
            set
            {
                SetProperty(ref _SpoolDrawingText, value);
            }
        }
        private ObservableCollection<CompanyCategoryCodeModel> companyCategoryCodes;
        public ObservableCollection<CompanyCategoryCodeModel> CompanyCategoryCodes
        {
            get { return companyCategoryCodes; }
            set
            {
                SetProperty(ref companyCategoryCodes, value);
            }
        }
        private CompanyCategoryCodeModel selectedCompanyCategoryCodes;
        public CompanyCategoryCodeModel SelectedCompanyCategoryCodes
        {
            get { return selectedCompanyCategoryCodes; }
            set
            {
                SetProperty(ref selectedCompanyCategoryCodes, value);
            }
        }


        private ObservableCollection<T_AdminFunctionCodes> functionCodeList;
        public ObservableCollection<T_AdminFunctionCodes> FunctionCodeList
        {
            get { return functionCodeList; }
            set { functionCodeList = value; RaisePropertyChanged(); }
        }

        private T_AdminFunctionCodes selectedFunctionCode;
        public T_AdminFunctionCodes SelectedFunctionCode
        {
            get { return selectedFunctionCode; }
            set
            {
                SetProperty(ref selectedFunctionCode, value);
            }
        }

        private string presetPunchID;
        public string PresetPunchID
        {
            get { return presetPunchID; }
            set
            {
                SetProperty(ref presetPunchID, value);
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

        private bool newPunchGrid;
        public bool NewPunchGrid
        {
            get { return newPunchGrid; }
            set
            {
                SetProperty(ref newPunchGrid, value);
            }
        }

        private bool pdfGrid;
        public bool PDFGrid
        {
            get { return pdfGrid; }
            set
            {
                SetProperty(ref pdfGrid, value);
            }
        }
        private bool cameraGrid;
        public bool CameraGrid
        {
            get { return cameraGrid; }
            set { SetProperty(ref cameraGrid, value); }
        }
        private string workUnComplete;
        public string WorkUnComplete
        {
            get { return workUnComplete; }
            set
            {
                SetProperty(ref workUnComplete, value);
            }
        }

        private string unChecked;
        public string UnChecked
        {
            get { return unChecked; }
            set
            {
                SetProperty(ref unChecked, value);
            }
        }

        private string workUnConfirmed;
        public string WorkUnConfirmed
        {
            get { return workUnConfirmed; }
            set
            {
                SetProperty(ref workUnConfirmed, value);
            }
        }

        private string allPunchesClosed;
        public string AllPunchesClosed
        {
            get { return allPunchesClosed; }
            set
            {
                SetProperty(ref allPunchesClosed, value);
            }
        }

        private string showAll;
        public string ShowAll
        {
            get { return showAll; }
            set
            {
                SetProperty(ref showAll, value);
            }
        }

        private string spool;
        public string Spool
        {
            get { return spool; }
            set
            {
                SetProperty(ref spool, value);
            }
        }

        private string pandID;
        public string PANDID
        {
            get { return pandID; }
            set
            {
                SetProperty(ref pandID, value);
            }
        }
        private string cancleparam;
        public string Cancleparam
        {
            get { return cancleparam; }
            set
            {
                SetProperty(ref cancleparam, value);
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
        private ImageSource PDFimage;
        public ImageSource PDFImage
        {
            get { return PDFimage; }
            set
            {
                SetProperty(ref PDFimage, value);
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

        private string punchControlpandID;
        public string PunchControlpandID
        {
            get { return punchControlpandID; }
            set
            {
                SetProperty(ref punchControlpandID, value);
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
        private string tpcConfirmedBy;
        public string TPCConfirmedBy
        {
            get { return tpcConfirmedBy; }
            set
            {
                SetProperty(ref tpcConfirmedBy, value);
            }
        }
        private string tpcConfirmedOn;
        public string TPCConfirmedOn
        {
            get { return tpcConfirmedOn; }
            set
            {
                SetProperty(ref tpcConfirmedOn, value);
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
        private bool isTouchVisible;
        public bool IsTouchVisible
        {
            get { return isTouchVisible; }
            set
            {
                SetProperty(ref isTouchVisible, value);
            }
        }

        private ObservableCollection<string> _ImageFiles;
        public ObservableCollection<string> ImageFiles
        {
            get { return _ImageFiles; }
            set { _ImageFiles = value; RaisePropertyChanged(); }
        }

        private string _btnSaveDelete;
        public string btnSaveDelete
        {
            get { return _btnSaveDelete; }
            set
            {
                SetProperty(ref _btnSaveDelete, value);
            }
        }

        private bool _btnSave;
        public bool btnSave
        {
            get { return _btnSave; }
            set { SetProperty(ref _btnSave, value); }
        }

        private bool renameImage;
        public bool RenameImage
        {
            get { return renameImage; }
            set
            {
                SetProperty(ref renameImage, value);
            }
        }


        private List<string> _cameraItems;
        public List<string> CameraItems
        {
            get { return _cameraItems; }
            set { _cameraItems = value; RaisePropertyChanged(); }
        }

        private string _selectedImageFiles;
        public string SelectedImageFiles
        {
            get { return _selectedImageFiles; }
            set
            {
                if (SetProperty(ref _selectedImageFiles, value))
                {
                    LoadImageFiles(_selectedImageFiles);
                    OnPropertyChanged();
                }
            }
        }
        private string newImageName;
        public string NewImageName
        {
            get { return newImageName; }
            set
            {
                SetProperty(ref newImageName, value);
            }
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
        public PunchViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            IMedia _media,
            ICheckValidLogin _checkValidLogin,
            IResizeImageService _resizeImageService,
            IRepository<T_UserProject> _userProjectRepository,
            IRepository<T_AdminPunchLayer> _adminPunchLayerRepository,
            IRepository<T_TestLimitDrawing> _testLimitDrawingRepository,
            IRepository<T_PunchList> _punchListRepository,
            IRepository<T_PunchImage> _punchImageRepository,
            IRepository<T_AdminFunctionCodes> _adminFunctionCodesRepository,
            IRepository<T_UserDetails> _userDetailsRepository,
            IRepository<T_ETestPackages> _eTestPackagesRepository,
            IRepository<T_ControlLogSignature> _controlLogSignatureRepository,
            IRepository<T_AdminPresetPunches> _adminPresetPunchesRepository,
            IRepository<T_AdminPunchCategories> _adminPunchCategoriesRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._adminPunchLayerRepository = _adminPunchLayerRepository;
            this._testLimitDrawingRepository = _testLimitDrawingRepository;
            this._checkValidLogin = _checkValidLogin;
            this._resizeImageService = _resizeImageService;
            this._punchListRepository = _punchListRepository;
            this._punchImageRepository = _punchImageRepository;
            this._adminFunctionCodesRepository = _adminFunctionCodesRepository;
            this._userProjectRepository = _userProjectRepository;
            this._userDetailsRepository = _userDetailsRepository;
            this._eTestPackagesRepository = _eTestPackagesRepository;
            this._controlLogSignatureRepository = _controlLogSignatureRepository;
            this._adminPresetPunchesRepository = _adminPresetPunchesRepository;
            this._adminPunchCategoriesRepository = _adminPunchCategoriesRepository;
            this._userDialogs = _userDialogs;
            this._media = _media;
            _media.Initialize();
            _userDialogs.HideLoading();
            PageHeaderText = "Punch View";
            IsHeaderBtnVisible = PunchLayerGrid = true;
            Showbuttons = true;
            CameraItems = new List<string> { "Camera" };


        }
        #region Private
        private async Task GetPunchLayerGridData()
        {
            WorkUnComplete = UnChecked = WorkUnConfirmed = WorkUnConfirmed = AllPunchesClosed = ShowAll = PANDID = "Grayradio.png";
            Spool = "Greenradio.png";
            var AdminPunchLayer = await _adminPunchLayerRepository.QueryAsync<T_AdminPunchLayer>("SELECT * FROM [T_AdminPunchLayer] WHERE [ProjectID] = '" + Settings.ProjectID + "' ORDER BY [LayerNo] ASC");
            List<T_AdminPunchLayer> APL = new List<T_AdminPunchLayer> { new T_AdminPunchLayer { LayerName = "Test Limits", LayerNo = 2, ID = 0, } };
            APL.AddRange(AdminPunchLayer);
            PunchLayersList = new ObservableCollection<T_AdminPunchLayer>(APL);
            SelectedpunchLayer = PunchLayersList.FirstOrDefault();
            GetSelectedpunchLayer(false);
            LoadPunchViewTabAsync();
            GeneratePunchDataTable(CurrentPageHelper.CurrentDrawing.ID);

        }
        private async void OnBackPressed()
        {
            CheckValidLogin._pageHelper = new PageHelper();
        }
        public async void OnClickButton(string param)
        {
            if (param == "WorkUnComplete" || param == "UnChecked" || param == "WorkUnConfirmed" || param == "AllPunchesClosed" || param == "ShowAll")
            {
                WorkUnComplete = UnChecked = WorkUnConfirmed = WorkUnConfirmed = AllPunchesClosed = ShowAll = "Grayradio.png";
                switch (param)
                {
                    case "WorkUnComplete":
                        WorkUnComplete = "Greenradio.png";
                        break;
                    case "UnChecked":
                        UnChecked = "Greenradio.png";
                        break;
                    case "WorkUnConfirmed":
                        WorkUnConfirmed = "Greenradio.png";
                        break;
                    case "AllPunchesClosed":
                        AllPunchesClosed = "Greenradio.png";
                        break;
                    case "ShowAll":
                        ShowAll = "Greenradio.png";
                        break;
                }
                LoadPunchViewTabAsync();

            }
            else if (param == "Spool" || param == "PANDID")
            {
                Spool = PANDID = "Grayradio.png";
                if (param == "Spool")
                    Spool = "Greenradio.png";
                else
                    PANDID = "Greenradio.png";

                LoadPunchViewTabAsync();

            }
            else if (param == "LoadPunch") // param == "NewPunch" ||
            {

                CreateNewPunch_Click();

            }
            else if (param == "SaveNewPunch")
            {
                SavePunch_Click();
            }
            else if (param == "NoPunch")
            {
                NoPunchClick();
            }
            //else if (param == "Cancel" || param == "OpenPDF") //  
            //{
            //    if (SelectedDrawing != null)
            //    {
            //        SelectedPDF = SelectedDrawing.DisplayName;
            //        CurrentPageHelper.SelectedDrawing = SelectedDrawing;
            //        PunchLayerGrid = NewPunchGrid = false;
            //        PDFGrid = true;
            //    }
            //}
            else if (param == "Back")
            {
              //  GeneratePunchDataTable(SelectedDrawing.ID);
                SelectedDrawing = null;
                SelectedPunch = null;
                NewPunchGrid = PDFGrid = false;
                PunchLayerGrid = true;
            }
            else if (param == "BackFromPunchControl")
            {
                GetSelectedpunchLayer(true);
                PunchControlGrid = PDFGrid = NewPunchGrid = false;
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
            else if (param == "Copy")
            {
                btnPunchControlTransfer_Click();
            }
            else if (param == "LoadPunchDescriptionList")
            {
                if (SelectedpunchType != null)
                {
                    //cbPresetDescription.Items.Clear();
                    //cbPresetDescription.Items.Add("");
                    //cbPresetDescription.SelectedIndex = 0;

                    string descriptionsql = "SELECT DISTINCT [Description] FROM [T_AdminPresetPunches] "
                                          + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "' AND [PunchType] = '" + SelectedpunchType + "' ORDER BY [Description] ASC";
                    var DescriptionList = await _adminPresetPunchesRepository.QueryAsync(descriptionsql);

                    PresetDescriptionList = DescriptionList.Select(i => i.Description).ToList();


                    //string punchType = cbPresetType.SelectedItem.ToString();

                    //string[] presetPunchDescriptions = LocalSQLFunctions.GetArray("SELECT DISTINCT [Description] FROM [AdminPresetPunchHH] WHERE [ProjectID] = @ProjectID AND [PunchType] = @PunchType ORDER BY [Description] ASC",
                    //    new OleDbParameter[]
                    //    {
                    //    new OleDbParameter("@ProjectID", Convert.ToInt32(CurrentProject.Project_ID)),
                    //    new OleDbParameter("@PunchType", punchType),
                    //    });

                    //if (presetPunchDescriptions != null && presetPunchDescriptions.Count() > 0)
                    //    cbPresetDescription.Items.AddRange(presetPunchDescriptions);
                }
            }
            else if (param == "SelectPresentPunch")
            {
                SelectPresetPunch_Click();
            }
            else if (param == "Clear")
            {
                CapturedImage = "";
                btnSaveDelete = "Save";
                RenameImage = false;
                SelectedImageFiles = SelectedCameraItem = null;
                SelectedCameraItems(_selectedCameraItem);
            }
            else if (param == "AddFromFile")
            {
                PickImagesFromGallery();
            }
            else if (param == "CaptureImageSave")
            {
                if (btnSaveDelete == "Save")
                    CaptureImageSave();
                else if (btnSaveDelete == "Delete")
                    CaptureImageDelete();
            }
            else if (param == "ColorPicker")
            {

                _drawFinger = true;
                await Navigation.PushPopupAsync(new ColorSelectionPopup());
            }
            else if (param == "BackfromCameraGrid")
            {
                NewPunchGrid = PDFGrid = CameraGrid = false;
                PunchLayerGrid = true;
            }

        }
        private async void LoadPunchViewTabAsync()
        {
            List<string> Filters = await GetPunchLayerFilter();

            string SQL = "SELECT TL.ID, TL.DisplayName, TL.OrderNo, COUNT(PL.PunchID) AS PunchCount, MAX(PL.[Status]) AS MaxStatus "
                        + " FROM (T_TestLimitDrawing TL"
                        + " LEFT OUTER JOIN T_PunchList PL ON PL.ProjectID = TL.ProjectID AND PL.ETestPackageID = TL.ETestPackageID AND PL.VMHub_DocumentsID = TL.ID) "
                        + " WHERE TL.[ProjectID] = '" + ETPSelected.ProjectID + "'"
                        + " AND TL.[ETestPackageID] = '" + ETPSelected.ID + "'"
                        + Filters[0]
                        + " GROUP BY TL.ID, TL.DisplayName, TL.OrderNo "
                        + Filters[1]
                        + " ORDER BY TL.OrderNo ASC";

            var data = await _testLimitDrawingRepository.QueryAsync<SpoolDrawingModel>(SQL);
            List<SpoolDrawingModel> SpoolDrawing = new List<SpoolDrawingModel>(data);
            foreach (SpoolDrawingModel SpoolDrawingitem in SpoolDrawing)
            {
                bool hasPunches = SpoolDrawingitem.PunchCount > 0;
                bool activePunches = SpoolDrawingitem.MaxStatus != null ? SpoolDrawingitem.MaxStatus.ToUpper() == "OPEN" : false;

                string drawingStatus = !hasPunches ? "No Punches" : (activePunches ? "Active Punches" : "All Punches Closed");

                SpoolDrawingitem.StatusImage = drawingStatus == "No Punches" ? "Grayradio.png" : (drawingStatus == "Active Punches" ? "Yellowradio.png" : "Greenradio.png");
            }

            DrawingList = new ObservableCollection<SpoolDrawingModel>(SpoolDrawing);

        }
        public async void GeneratePunchDataTable(int ID)
        {
            string SQL = "SELECT [PunchID],[Status],[Category],[TPCConfirmed],[Live],[PunchAdminID]"
                + " FROM [T_PunchList] "
                + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'" + " AND [ETestPackageID] ='" + ETPSelected.ID + "'"
                + " AND [VMHub_DocumentsID] = '" + ID + "'" + " AND [PunchAdminID] = '" + CurrentPageHelper.CurrentLayer.ID + "'"
                + " ORDER BY [PunchNo] DESC";

            var data = await _punchListRepository.QueryAsync<T_PunchList>(SQL);
            List<PunchListModel> punches = new List<PunchListModel>();


            foreach (T_PunchList PLitem in data)
            {
                string tpcConfirmed = PLitem.TPCConfirmed ? "Yes" : "No";

                var imageSQL = "SELECT [ID] FROM [T_PunchImage] " +
                               " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'" + " AND [ETestPackageID] = '" + ETPSelected.ID + "'"
                             + " AND [PunchID] = '" + PLitem.PunchID + "'";

                var Existimage = await _punchImageRepository.QueryAsync<T_PunchImage>(imageSQL);
                bool hasImages = Existimage.Count > 0 ? true : false;

                string punchIDColor = "";
                if (PLitem.Category == "A")
                    punchIDColor = "#F63B3B";
                else if (PLitem.Category == "B")
                    punchIDColor = "#4949F6";


                string cameraImage = hasImages ? "Greencam.png" : "cam.png";
                bool deleteImage = (PLitem.TPCConfirmed || PLitem.Live) ? false : true;

                punches.Add(new PunchListModel
                {
                    PunchID = PLitem.PunchID,
                    Category = PLitem.Category,
                    Status = PLitem.Status,
                    HasImages = hasImages,
                    Camera = cameraImage,
                    Delete = deleteImage,
                    PunchAdminID = PLitem.PunchAdminID,
                    PunchIDColor = punchIDColor,
                    TPCConfirmed = PLitem.TPCConfirmed
                });
            }
            PunchesList = new ObservableCollection<PunchListModel>(punches);

        }
        private void GetSelectedpunchLayer(bool Isbind)
        {
            //if (SelectedpunchLayer != null)
            //   CurrentPageHelper.CurrentLayer = SelectedpunchLayer;
            //else 
            //   CurrentPageHelper.CurrentLayer = PunchLayersList.FirstOrDefault();
            if (Isbind && selectedpunchLayer != null)
            {
                CurrentPageHelper.CurrentLayer = SelectedpunchLayer;
                CurrentPageHelper.CurrentPunchOverview = SelectedPunchOverview;
            }
            else if (PunchLayersList != null && PunchLayersList.Count > 0)
            {
                //cbPunchLayer.SelectedIndex = 0;

                foreach (T_AdminPunchLayer item in PunchLayersList)
                {
                    if ((item.ID == SelectedPunchOverview.PunchAdminID))
                    {
                        CurrentPageHelper.CurrentLayer = item;

                        SelectedpunchLayer = item;
                        break;
                    }
                    else
                        CurrentPageHelper.CurrentLayer = PunchLayersList.FirstOrDefault();
                }
            }

            LoadPunchViewTabAsync();
            GeneratePunchDataTable(CurrentPageHelper.CurrentDrawing.ID);
        }
        private async Task<List<string>> GetPunchLayerFilter()
        {
            string filter = "", havingFilter = "";

            //PunchLayer Filters
            if (UnChecked == "Greenradio.png")
                havingFilter = " HAVING COUNT(PL.PunchID) = 1 ";
            else if (WorkUnComplete == "Greenradio.png")
                filter = " AND PL.[WorkCompleted] = 1 ";
            else if (WorkUnConfirmed == "Greenradio.png")
                filter = " AND PL.[WorkCompleted] = 1 AND PL.[WorkConfirmed] = 0 ";
            else if (AllPunchesClosed == "Greenradio.png")
                havingFilter = " HAVING MAX(PL.[Status]) <> 'Open' ";

            //Drawing filters
            if (Spool == "Greenradio.png")
                filter += " AND TL.[IsSpoolDrawing] = 1 ";
            else if (PANDID == "Greenradio.png")
                filter += " AND TL.[IsPID] = 1 ";

            return new List<string> { filter, havingFilter };
        }

        public async void LoadPunchLayerImageAsync(string pdfname)
        {
            if (SelectedDrawing != null)
            {
                SelectedPDF = SelectedDrawing.DisplayName;
                CurrentPageHelper.SelectedDrawing = SelectedDrawing;
                PunchLayerGrid = NewPunchGrid = false;
                PDFGrid = true;
            }
            else
            {
                CurrentPageHelper.SelectedDrawing = SelectedDrawing = DrawingList.Where(i => i.DisplayName == pdfname).FirstOrDefault();               
                SelectedPDF = SelectedDrawing.DisplayName;
                PunchLayerGrid = NewPunchGrid = false;
                PDFGrid = true;
            }
            if (CurrentPageHelper.SelectedDrawing != null)
            {
                string SQL = "SELECT * FROM [T_TestLimitDrawing]"
                                 + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'"
                                 + " AND [ETestPackageID] = '" + ETPSelected.ID + "'"
                                 + " AND [ID] ='" + CurrentPageHelper.SelectedDrawing.ID + "'";

                var data = await _testLimitDrawingRepository.QueryAsync<T_TestLimitDrawing>(SQL);
                CurrentPageHelper.CurrentDrawing = data.Select(i => i).FirstOrDefault();
            }
        }

        public async void PopUp(string id)
        {
            PunchListModel current = PunchesList.Where(i => i.PunchID == id).FirstOrDefault();
            SelectedPunch = current;
            getPopUp(true);
        }
        private async void getPopUp(bool PopUP)
        {
            try
            {
                string punchID = "";
                if (PopUP)
                {
                    punchID = SelectedPunch.PunchID;

                    string SQL = "SELECT TL.ID, TL.DisplayName, PL.PunchID, PL.Description, PL.Status, PL.Updated, PL.PunchAdminID, PL.Category, APL.LayerName"
                       + " FROM((T_TestLimitDrawing TL"
                       + " LEFT OUTER JOIN T_PunchList PL ON TL.ProjectID = PL.ProjectID AND TL.ETestPackageID = PL.ETestPackageID AND TL.ID = PL.VMHub_DocumentsID)"
                       + " LEFT OUTER JOIN T_AdminPunchLayer APL ON PL.PunchAdminID = APL.ID) "
                       + " WHERE TL.[ProjectID] = '" + ETPSelected.ProjectID + "'"
                       + " AND TL.[ETestPackageID] = '" + ETPSelected.ID + "'"
                       + " AND PL.PunchID = '" + punchID + "'"
                       + " ORDER BY TL.[OrderNo] ASC ";

                    var data = await _testLimitDrawingRepository.QueryAsync<PunchOverview>(SQL);
                    SelectedPunchOverview = data.Select(i => i).FirstOrDefault();
                }

                else
                    punchID = SelectedPunchOverview.PunchID;
                //CurrentPageHelper.PunchID = punchID;

                if (String.IsNullOrEmpty(punchID))
                    return;
                //string description = "", functionCode = "", companyCategoryCode = "";
                //Boolean tpcConfirmed = false, cancelled = false, workCompleted = false, workConfirmed = false;

                string PunchListSQL = "SELECT * FROM [T_PunchList] "
                           + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'"
                           + " AND [ETestPackageID] ='" + ETPSelected.ID + "'"
                           + " AND [PunchID] ='" + punchID + "'"
                           + " AND [PunchAdminID] ='" + SelectedPunchOverview.PunchAdminID + "'";

                var APunchLayer = await _punchListRepository.QueryAsync<T_PunchList>(PunchListSQL);

                T_PunchList AdminPunchLayer = APunchLayer.FirstOrDefault();

                string AdminFunctionCodeSQL = "SELECT DISTINCT (Description) FROM [T_AdminFunctionCodes] "
                           + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'"
                           + " AND [FunctionCode] ='" + AdminPunchLayer.FunctionCode + "'";

                var FunCodeDescr = await _adminFunctionCodesRepository.QueryAsync<T_AdminFunctionCodes>(AdminFunctionCodeSQL);
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


                if (AdminPunchLayer.TPCConfirmed)
                {
                    if (await _userDialogs.ConfirmAsync(message + Environment.NewLine + "" + Environment.NewLine + "" + "Do You Want To Jump To Punch Control?", punchID + " Punch Details", "Yes", "No"))
                    {
                        //CurrentPageHelper.PunchID = punchID;
                        PunchControlGrid = true;
                        PunchLayerGrid = false;
                        await PopulatePunchControlTabAsync(AdminPunchLayer.PunchID);
                        // tcETestPackage.SelectedIndex = 5;
                    }
                    //else
                    // CurrentPageHelper.PunchID = string.Empty;
                }
                else
                    await _userDialogs.AlertAsync(message, punchID + " Punch Details", "Ok");
            }
            catch (Exception e)

            {

            }
        }
        private async Task PopulatePunchControlTabAsync(string punchID)
        {

            //    lblControlPunchID.Text = punchID;

            //TimeZoneInfo projectTimeZone = TimeZoneInfo.FindSystemTimeZoneById(CurrentProject.TimeZone);
            //Get all details
            string SQL = "SELECT * FROM [T_PunchList] WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'"
                       + " AND [ETestPackageID] = '" + ETPSelected.ID + "'"
                       + " AND [PunchID] = '" + punchID + "'"
                       + " AND [PunchAdminID] = '" + SelectedPunchOverview.PunchAdminID + "'";

            var AdminPunchLayer = await _punchListRepository.QueryAsync<T_PunchList>(SQL);

            // PunchControlpandID = AdminPunchLayer.Select(i => i.PunchID).ToString();

            TimeZoneInfo projectTimeZone = TimeZoneInfo.Local;

            //  var projectTimeZone = DateTimeZoneProviders.Tzdb[userProject.TimeZone];
            foreach (T_PunchList punchList in AdminPunchLayer)
            {
                PunchControlpandID = punchList.PunchID;
                ControlDescription = punchList.Description;

                CreatedBy = punchList.CreatedBy;
                CreatedOn = ModsTools.AdjustDateTime(punchList.CreatedOn, projectTimeZone);
                UpdatedBy = punchList.UpdatedBy;
                UpdatedOn = string.Empty;
                if (punchList.UpdatedOn.ToString(AppConstant.DateFormat) == Convert.ToDateTime("01/01/2000 0:00").ToString(AppConstant.DateFormat)
                   || punchList.UpdatedOn.ToString(AppConstant.DateFormat) == Convert.ToDateTime("01/01/1900 0:00").ToString(AppConstant.DateFormat)
                   || punchList.UpdatedOn.ToString(AppConstant.DateFormat) == Convert.ToDateTime("01/01/0001 0:00").ToString(AppConstant.DateFormat))
                {
                    UpdatedOn = "";
                }
                else
                {
                    UpdatedOn = ModsTools.AdjustDateTime(punchList.UpdatedOn, projectTimeZone);
                }

                if (punchList.TPCConfirmed)
                {
                    TPCConfirmedBy = punchList.TPCConfirmedBy;
                    TPCConfirmedOn = ModsTools.AdjustDateTime(punchList.TPCConfirmedOn, projectTimeZone);
                }
                else
                {
                    TPCConfirmedBy = "";
                    TPCConfirmedOn = "";
                }


                //Boolean cancelled = SQLReader.GetBoolean(Reader, "Cancelled");
                //Boolean workCompleted = SQLReader.GetBoolean(Reader, "WorkCompleted");
                //Boolean workConfirmed = SQLReader.GetBoolean(Reader, "WorkConfirmed");
                //Boolean isIssuer = (SQLReader.GetInt(Reader, "CreatedByUserID") == CurrentUser.ID);
                //Boolean workCompletedBy = (SQLReader.GetInt(Reader, "WorkCompletedByUserID") == CurrentUser.ID);
                //Boolean workConfirmedBy = (SQLReader.GetInt(Reader, "WorkConfirmedByUserID") == CurrentUser.ID);


                bool isIssuer = punchList.CreatedByUserID == userProject.User_ID;
                bool workCompletedBy = punchList.WorkCompletedByUserID == userProject.User_ID;
                bool workConfirmedBy = punchList.WorkConfirmedByUserID == userProject.User_ID;

                if (punchList.Cancelled)
                {
                    CancelledBy = punchList.CancelledBy;
                    CancelledOn = ModsTools.AdjustDateTime(punchList.CancelledOn, projectTimeZone);
                    CancelledImage = "Greenradio.png";
                }
                else
                {
                    CancelledBy = "";
                    CancelledOn = "";
                    CancelledImage = "Grayradio.png";
                }

                ControlRejectedReason = punchList.WorkRejectedReason;

                if (punchList.WorkCompleted)
                {
                    WorkCompletedBy = punchList.WorkCompletedBy;
                    WorkCompletedOn = ModsTools.AdjustDateTime(punchList.WorkCompletedOn, projectTimeZone);
                    WorkCompletedImage = "Greenradio.png";
                }
                else
                {
                    WorkCompletedBy = "";
                    WorkCompletedOn = "";
                    WorkCompletedImage = "Grayradio.png";
                }
                ModsTools.WorkCompletedEnabled = ((punchList.Cancelled == false && punchList.WorkCompleted == false) ||
                                                  (punchList.Cancelled == false && punchList.WorkCompleted == true &&
                                                  (isIssuer == true || workCompletedBy) && punchList.WorkConfirmed == false));



                if (punchList.WorkConfirmed)
                {
                    WorkConfirmedBy = punchList.WorkConfirmedBy;


                    WorkConfirmedOn = ModsTools.AdjustDateTime(punchList.WorkConfirmedOn, projectTimeZone);
                    WorkConfirmedImage = "Greenradio.png";
                }
                else
                {
                    WorkConfirmedBy = "";
                    WorkConfirmedOn = "";
                    WorkConfirmedImage = "Grayradio.png";
                }
                ModsTools.WorkConfirmedEnabled = ((punchList.Cancelled == false && punchList.WorkCompleted == true && punchList.WorkConfirmed == false)
                                                || (punchList.Cancelled == false && punchList.WorkCompleted == true && punchList.WorkConfirmed == true));

                var PunchControlLayer = await _adminPunchLayerRepository.QueryAsync<T_AdminPunchLayer>("SELECT * FROM [T_AdminPunchLayer] WHERE [ProjectID] = '" + Settings.ProjectID + "' ORDER BY [LayerNo] ASC");
                PunchControlLayersList = new ObservableCollection<T_AdminPunchLayer>(PunchControlLayer);
            }

        }

        private async void btnCancelled_Click()
        {
            string messageCaption = "Mark As Cancelled";

            if (WorkConfirmedBy == string.Empty)
            {
                if (CancelledBy == string.Empty)
                {
                    if (await _userDialogs.ConfirmAsync("Are you sure you want to cancel this punch?", messageCaption, "Yes", "No"))
                    {
                        string sql = "SELECT [CreatedByUserID] FROM [T_PunchList] "
                                   + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'" + " AND [ETestPackageID] ='" + ETPSelected.ID + "'" + " AND [PunchID] ='" + SelectedPunchOverview.PunchID + "'";
                        var userCancelled = await _punchListRepository.QueryAsync<T_PunchList>(sql);
                        int createdByUserID = userCancelled.Select(i => i.CreatedByUserID).FirstOrDefault();


                        if (Settings.UserID == createdByUserID)
                        {
                            try
                            {
                                string updateSql = "UPDATE [T_PunchList] SET [Status] = 'Cancelled' , [Cancelled] = 1, [CancelledBy] = '" + CurrentUserDetail.FullName + "'"
                                            + ", [CancelledByUserID] = '" + CurrentUserDetail.ID + "'" + ", [CancelledOn] = '" + Convert.ToDateTime(DateTime.UtcNow.ToString(AppConstant.DateSaveFormat)).Ticks + "'" + ", [Updated] = 1 , Live = 1"
                                            + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'" + " AND [ETestPackageID] = '" + ETPSelected.ID + "'" + " AND [PunchID] = '" + SelectedPunchOverview.PunchID + "'";
                                await _punchListRepository.QueryAsync<T_PunchList>(updateSql);


                                //    //SAVED
                                //    Task tsk1 = LoadPunchViewTabAsync(true);
                                //    Task tsk2 = ShowPunchLayerImageAsync();
                                await PopulatePunchControlTabAsync(SelectedPunchOverview.PunchID);
                                //    //Task tsk4 = LoadPunchOverviewTabAsync();

                                //    await Task.WhenAll(tsk1, tsk2, tsk3);

                                //    await CheckCanSignControlLogAfterPunchAsync();

                                MarkETestPackageAsUpdated();


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

            if (WorkConfirmedBy == string.Empty)
            {
                if (CancelledBy == string.Empty)
                {
                    if (ModsTools.WorkCompletedEnabled)
                    {

                        string functionCode = "", companyCategoryCode = "";
                        Boolean isIssuer = false;
                        string sql = "SELECT * FROM [T_PunchList]"
                                    + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'" + " AND[ETestPackageID] = '" + ETPSelected.ID + "'" + " AND[PunchID] = '" + SelectedPunchOverview.PunchID + "'";
                        var userCompleted = await _punchListRepository.QueryAsync<T_PunchList>(sql);

                        foreach (T_PunchList uCompleted in userCompleted)
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
                                DateTime WkConfirmedOn = WorkCompletedAlready ? Convert.ToDateTime("01/01/1900 0:00") : Convert.ToDateTime(DateTime.UtcNow.ToString(AppConstant.DateSaveFormat));

                                string updateSQL = " UPDATE [T_PunchList] SET [Status] = '" + status + "'" + ", [WorkCompleted] = '" + WkCompleted + "'"
                                                  + ", [WorkCompletedBy] ='" + WkCompletedBy + "'" + ", [WorkCompletedByUserID] = '" + WkCompletedByUserID + "'"
                                                  + ", [WorkCompletedOn] = '" + WkConfirmedOn.Ticks + "', [Updated] = 1 , [WorkRejected] = 0 , Live = 1"
                                                  + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'" + " AND [ETestPackageID] = '" + ETPSelected.ID + "'" + " AND [PunchID] = '" + SelectedPunchOverview.PunchID + "'";

                                await _punchListRepository.QueryAsync<T_PunchList>(updateSQL);

                                //        //SAVED
                                //        Task tsk1 = LoadPunchViewTabAsync(true);
                                //        Task tsk2 = ShowPunchLayerImageAsync();
                                await PopulatePunchControlTabAsync(SelectedPunchOverview.PunchID);
                                //        //Task tsk4 = LoadPunchOverviewTabAsync();
                                //        await Task.WhenAll(tsk1, tsk2, tsk3);
                                //        await CheckCanSignControlLogAfterPunchAsync();

                                MarkETestPackageAsUpdated();
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

            if (WorkCompletedBy != string.Empty)
            {
                if (CancelledBy == string.Empty)
                {
                    if (ModsTools.WorkConfirmedEnabled)
                    {
                        if (ModsTools.CheckCanAddPunch(CurrentPageHelper.CurrentLayer, CurrentUserDetail))
                        {
                            try
                            {
                                string status = WorkConfirmedAlready ? "Open" : "Closed";
                                string WkConfirmed = WorkConfirmedAlready ? "0" : "1";
                                string WkConfirmedBy = WorkConfirmedAlready ? "" : CurrentUserDetail.FullName;
                                string WkConfirmedByUserID = WorkConfirmedAlready ? "0" : CurrentUserDetail.ID.ToString();
                                DateTime WkConfirmedOn = WorkConfirmedAlready ? Convert.ToDateTime("01/01/1900 0:00") : Convert.ToDateTime(DateTime.UtcNow.ToString(AppConstant.DateSaveFormat));

                                string updateSQL = " UPDATE [T_PunchList] SET [Status] = '" + status + "'" + ", [WorkConfirmed] = '" + WkConfirmed + "'"
                                                 + ", [WorkConfirmedBy] = '" + WkConfirmedBy + "', [WorkConfirmedByUserID] = '" + WkConfirmedByUserID + "'"
                                                 + ", [WorkConfirmedOn] = '" + WkConfirmedOn.Ticks + "', [Updated] = 1 , Live = 1"
                                                 + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'" + " AND [ETestPackageID] = '" + ETPSelected.ID + "'" + " AND [PunchID] = '" + SelectedPunchOverview.PunchID + "'";
                                await _punchListRepository.QueryAsync<T_PunchList>(updateSQL);
                                ////SAVED
                                //Task tsk1 = LoadPunchViewTabAsync(true);
                                //Task tsk2 = ShowPunchLayerImageAsync();
                                await PopulatePunchControlTabAsync(SelectedPunchOverview.PunchID);
                                ////Task tsk4 = LoadPunchOverviewTabAsync();

                                //await Task.WhenAll(tsk1, tsk2, tsk3);
                                //await CheckCanSignControlLogAfterPunchAsync();

                                MarkETestPackageAsUpdated();


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

            DateTime datetimeNow = DateTime.Now.Date;
            DateTime completedDate = DateTime.MinValue;
            string completedBy = "";
            string functionCode = "", companyCategoryCode = "";

            string sql = "SELECT * FROM [T_PunchList] "
                       + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'" + " AND [ETestPackageID] ='" + ETPSelected.ID + "'" + " AND [PunchID] ='" + SelectedPunchOverview.PunchID + "'";
            var WkRejected = await _punchListRepository.QueryAsync<T_PunchList>(sql);
            foreach (T_PunchList Rejected in WkRejected)
            {
                completedBy = Rejected.WorkCompletedBy;
                completedDate = Rejected.WorkCompletedOn;
            }


            if (!WorkConfirmedAlready)
            {
                if (ControlRejectedReason != string.Empty)
                {
                    //Check if the user can add/confirm work for punches on this punch layer
                    Boolean canSign = ModsTools.CheckCanAddPunch(CurrentPageHelper.CurrentLayer, CurrentUserDetail);

                    //If not then check if the work for the punch has been assigned to them
                    if (!canSign)
                    {
                        string PunchListsql = "SELECT * FROM [T_PunchList] "
                                    + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'" + " AND [ETestPackageID] ='" + ETPSelected.ID + "'" + " AND [PunchID] ='" + SelectedPunchOverview.PunchID + "'";
                        var PunchList = await _punchListRepository.QueryAsync<T_PunchList>(PunchListsql);
                        foreach (T_PunchList Punchitem in WkRejected)
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
                                // WorkRejectedCompletedByUserID
                                // WorkRejectedCompletedOn                              

                                string WkCompletedBy = "";
                                DateTime WkCompletedOn = Convert.ToDateTime("01/01/1900 0:00");
                                DateTime WkRejectedOn = Convert.ToDateTime(DateTime.UtcNow.ToString(AppConstant.DateSaveFormat));

                                DateTime WkRejectedCompletedOn = Convert.ToDateTime(completedDate.ToString(AppConstant.DateSaveFormat));


                                string updateSQL = " UPDATE [T_PunchList] SET [Status] = '" + "Open" + "'" + " , [WorkCompleted] = 0"
                                                  + ", [WorkCompletedBy] ='" + WkCompletedBy + "'" + ", [WorkCompletedByUserID] = 0"
                                                  + ", [WorkCompletedOn] = '" + WkCompletedOn.Ticks + "', [Updated] = 1 , [WorkRejected] = 1"
                                                  + ", [WorkRejectedBy] ='" + CurrentUserDetail.FullName + "'" + ",[WorkRejectedByUserID] = '" + CurrentUserDetail.ID + "'"
                                                  + ", [WorkRejectedOn] ='" + WkRejectedOn.Ticks + "'" + ",[WorkRejectedReason] = '" + ControlRejectedReason + "'"
                                                  + ", [WorkRejectedCompletedBy] ='" + completedBy + "'" + ",[WorkRejectedCompletedOn] = '" + WkRejectedCompletedOn.Ticks + "', Live = 1"
                                                  + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'" + " AND [ETestPackageID] = '" + ETPSelected.ID + "'" + " AND [PunchID] = '" + SelectedPunchOverview.PunchID + "'";
                                await _punchListRepository.QueryAsync<T_PunchList>(updateSQL);

                                ////SAVED
                                //Task tsk1 = LoadPunchViewTabAsync(true);
                                //Task tsk2 = ShowPunchLayerImageAsync();
                                //Task tsk3 = PopulatePunchControlTabAsync(CurrentPageHelper.PunchID);
                                ////Task tsk4 = LoadPunchOverviewTabAsync();
                                //await Task.WhenAll(tsk1, tsk2, tsk3);
                                //ModsTools.MarkETestPackageAsUpdated(Convert.ToInt32(CurrentProject.Project_ID), CurrentPageHelper.ETestPackageID);

                                await PopulatePunchControlTabAsync(SelectedPunchOverview.PunchID);
                                MarkETestPackageAsUpdated();
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

        private async void btnPunchControlTransfer_Click()
        {
            if (PunchLayersList.Count >= 0)
            {
                T_AdminPunchLayer pl = SelectedpunchControlLayer;
                if (pl == null)
                {
                    await _userDialogs.AlertAsync("Please Select punch layer");
                    return;
                }

                if (ModsTools.CheckCanAddPunch(pl, CurrentUserDetail))
                {

                    string functionCode = "",
                        description = "",
                        companyCategoryCode = "",
                        remarks = "",
                        pcwbs = "",
                        category = "",
                        type = "",
                        presetPunchID = "";
                    Boolean onDocument = false;
                    PunchPointer pp = new PunchPointer();
                    string sql = "SELECT * FROM [T_PunchList] "
                               + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'" + " AND [ETestPackageID] ='" + ETPSelected.ID + "'" + " AND [PunchID] ='" + SelectedPunchOverview.PunchID + "'";
                    var ControlTransfer = await _punchListRepository.QueryAsync<T_PunchList>(sql);


                    foreach (T_PunchList item in ControlTransfer)
                    {
                        description = item.Description;
                        functionCode = item.FunctionCode;
                        companyCategoryCode = item.CompanyCategoryCode;
                        pcwbs = item.PCWBS;
                        remarks = item.Remarks;
                        category = item.Category;
                        type = item.Type;
                        presetPunchID = item.PresetPunchID;
                        onDocument = item.OnDocument;
                        pp = new PunchPointer()
                        {
                            FirstPoint = new Point(item.XPOS1, item.YPOS1),
                            SecondPoint = new Point(item.XPOS2, item.YPOS2),
                        };
                    }


                    string ControlLogsql = "SELECT CLS.[Signed] "
                                         + " FROM (((T_ControlLogSignature as CLS"
                                         + " INNER JOIN T_AdminControlLog ACL ON (CLS.ControlLogAdminID = ACL.ID) AND (CLS.ProjectID = ACL.ProjectID)) "
                                         + " INNER JOIN T_AdminControlLogPunchLayer ACLPL ON (ACL.ID = ACLPL.ControlLogAdminID) AND (ACL.ProjectID = ACLPL.ProjectID)) "
                                         + " INNER JOIN T_AdminControlLogPunchCategory ACLPC  ON (ACLPL.ProjectID = ACLPC.ProjectID) AND (ACLPL.ControlLogAdminID = ACLPC.ControlLogAdminID)) "
                                         + " INNER JOIN T_AdminControlLogActionParty ACLAP ON (ACLPC.ProjectID = ACLAP.ProjectID) AND (ACLPC.ControlLogAdminID = ACLAP.ControlLogAdminID) "

                                         + " WHERE CLS.[ProjectID] = '" + ETPSelected.ProjectID + "' AND CLS.[ETestPackageID] = '" + ETPSelected.ID + "' AND CLS.[Signed] = 1 "
                                         + " AND ACLPL.[PunchAdminID] = '" + SelectedPunchOverview.PunchAdminID + "' AND ACLPC.[Category] = '" + category + "'"
                                         + " AND (ACL.[PunchesCompleted] = 1 OR ACL.[PunchesConfirmed] = 1) "
                                         + " AND  ACLAP.[FunctionCode] = '" + functionCode + "'";

                    var ControlLog = await _controlLogSignatureRepository.QueryAsync<T_ControlLogSignature>(ControlLogsql);

                    if (!ControlLog.Select(i => i.Signed).FirstOrDefault())
                    {


                        CurrentPageHelper.CurrentLayer = pl;


                        T_PunchList punchItem = new T_PunchList();
                        if (await CreatePunch(pp, ETPSelected.TestPackage, category, type, description, remarks, functionCode, companyCategoryCode, pcwbs, false, onDocument, presetPunchID))
                        {
                            //ModsTools.MarkETestPackageAsUpdated(Convert.ToInt32(CurrentProject.Project_ID),
                            //            pageHelper.ETestPackageID);
                            MarkETestPackageAsUpdated();

                            //        Task tsk1 = ShowPunchLayerImageAsync();
                            //        //Task tsk2 = LoadPunchOverviewTabAsync();
                            // await PopulatePunchControlTabAsync(SelectedPunchOverview.PunchID);

                            //        await Task.WhenAll(tsk1);

                            await _userDialogs.AlertAsync("Punch has been successfully coped to " + pl.LayerName + " layer", "Transfer Punch", "Ok");

                        }
                        else
                            await _userDialogs.AlertAsync("Punch not transfered.", "Transfer Punch", "Ok");
                    }
                    else
                        await _userDialogs.AlertAsync("Unable to transfer this punch its category " + category + " has been marked as completed on the selected layer", "Transfer Punch", "Ok");
                }
                else
                    await _userDialogs.AlertAsync("You do not have the correct user rights to add punch items to this layer.", "Punch Transfer", "Ok");
            }
        }

        private async void SelectPresetPunch_Click()
        {
            //CurrentPageHelper.PunchID = string.Empty;
            if (SelectedpunchType != null && SelectedPresetDescription != null)
            {

                T_AdminPresetPunches presetPunch = new T_AdminPresetPunches();
                string SQL = "SELECT * FROM [T_AdminPresetPunches] "
                           + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "' AND [PunchType] = '" + SelectedpunchType + "' AND [Description] = '" + SelectedPresetDescription + "'";

                //presetPunch.Get(Convert.ToInt32(CurrentProject.Project_ID), SelectedpunchType, SelectedPresetDescription);

                var presetPunchList = await _adminPresetPunchesRepository.QueryAsync(SQL);


                if (presetPunchList.Count > 0)
                {
                    presetPunch = presetPunchList.FirstOrDefault();

                    //Populate addpunchtab with preset punch items.

                    if (PresetCategoryList.Contains(presetPunch.Category))
                        SelectedPresetCategory = presetPunch.Category;

                    if (PresetPunchTypeList.Contains(presetPunch.PunchType))
                        SelectedpresetpunchType = presetPunch.PunchType;

                    punchDescription = presetPunch.Description;


                    foreach (T_AdminFunctionCodes item in FunctionCodeList)
                    {
                        if (int.Parse(item.FunctionCode) == int.Parse(presetPunch.FunctionCode))
                        {
                            SelectedFunctionCode = item;
                            break;
                        }
                    }

                    foreach (CompanyCategoryCodeModel item in CompanyCategoryCodes)
                    {

                        if (item.value.ToString() == presetPunch.CategoryCode)
                        {
                            SelectedCompanyCategoryCodes = item;
                            break;
                        }
                    }

                    punchRemarks = presetPunch.Remarks;
                    PresetPunchID = presetPunch.PresetPunchID;
                }
                else
                    await _userDialogs.AlertAsync("Preset punch not found, please check your drop down selections and try again.", "Preset Punch", "Ok");
            }
            else
                await _userDialogs.AlertAsync("Please select type and description.", "Preset Punch", "Ok");
        }
        private async Task<bool> CreatePunch(PunchPointer punchPointer, string testPackage, string category, string type, string description, string remarks,
            string functionCode, string companyCategoryCode, string pcwbs, bool signoffall, bool OnDocument, string presetPunchID)
        {
            bool result = true;
            try
            {
                T_PunchList newPunchitem = new T_PunchList();
                var ID = await _punchListRepository.QueryAsync<T_PunchList>("SELECT * FROM [T_PunchList]");
                if (ID.Count > 0) newPunchitem.ID = ID.Select(i => i.ID).Max() + 1;
                else newPunchitem.ID = 1;
                newPunchitem.ProjectID = ETPSelected.ProjectID;
                newPunchitem.ETestPackageID = SelectedETP.ID;
                newPunchitem.TestPackage = testPackage;
                newPunchitem.VMHub_DocumentsID = CurrentPageHelper.SelectedDrawing.ID;
                newPunchitem.PunchAdminID = CurrentPageHelper.CurrentLayer.ID;

                string punchNoSQL = " SELECT * FROM [T_PunchList] "
                               + " WHERE[ProjectID] = '" + ETPSelected.ProjectID + "'" + " AND [ETestPackageID] = '" + ETPSelected.ID + "'"
                               + " AND [PunchAdminID] = '" + CurrentPageHelper.CurrentLayer.ID + "'" + " ORDER BY [PunchNo] DESC";
                var pitem = await _punchListRepository.QueryAsync<T_PunchList>(punchNoSQL);
                var punchitem = pitem.FirstOrDefault();
                if (punchitem == null)
                    newPunchitem.PunchNo = 1;
                else
                    newPunchitem.PunchNo = 1 + punchitem.PunchNo;

                newPunchitem.Category = category;
                newPunchitem.PunchID = CurrentPageHelper.CurrentLayer.Prefix + "-" + newPunchitem.PunchNo.ToString("000") + "-" + newPunchitem.Category;
                newPunchitem.Type = type;
                if (string.IsNullOrWhiteSpace(description)) description = " ";
                newPunchitem.Description = description;//ControlDescription != null ? ControlDescription : SelectedPresetDescription;
                if (string.IsNullOrWhiteSpace(remarks)) remarks = " ";
                newPunchitem.Remarks = remarks;
                newPunchitem.FunctionCode = functionCode;// punchitem.FunctionCode;
                newPunchitem.CompanyCategoryCode = companyCategoryCode; //punchitem.CompanyCategoryCode;
                newPunchitem.OnDocument = OnDocument;
                newPunchitem.XPOS1 = Convert.ToInt32(punchPointer.FirstPoint.X);
                newPunchitem.YPOS1 = Convert.ToInt32(punchPointer.FirstPoint.Y);
                newPunchitem.XPOS2 = Convert.ToInt32(punchPointer.SecondPoint.X);
                newPunchitem.YPOS2 = Convert.ToInt32(punchPointer.SecondPoint.Y);

                newPunchitem.CreatedByUserID = CurrentUserDetail.ID;
                newPunchitem.CreatedBy = CurrentUserDetail.FullName;
                newPunchitem.CreatedOn = DateTime.Now.Date;
                if (string.IsNullOrWhiteSpace(presetPunchID)) presetPunchID = "0";
                newPunchitem.PresetPunchID = presetPunchID;

                if (signoffall)
                {
                    newPunchitem.Status = "Closed";
                    newPunchitem.TPCConfirmed = true;
                    newPunchitem.TPCConfirmedByUserID = CurrentUserDetail.ID;
                    newPunchitem.TPCConfirmedBy = CurrentUserDetail.FullName;
                    newPunchitem.TPCConfirmedOn = DateTime.Now.Date;

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

                    if (CurrentPageHelper.CurrentLayer.PreLineCheck)
                    {
                        newPunchitem.TPCConfirmed = true;
                        newPunchitem.TPCConfirmedByUserID = CurrentUserDetail.ID;
                        newPunchitem.TPCConfirmedBy = CurrentUserDetail.FullName;
                        newPunchitem.TPCConfirmedOn = DateTime.Now.Date;
                    }
                    else
                    {
                        newPunchitem.TPCConfirmed = false;
                        newPunchitem.TPCConfirmedByUserID = 0;
                        newPunchitem.TPCConfirmedBy = "";
                        newPunchitem.TPCConfirmedOn = Convert.ToDateTime("01/01/1900 0:00");
                    }

                    newPunchitem.WorkCompleted = false;
                    newPunchitem.WorkCompletedByUserID = 0;
                    newPunchitem.WorkCompletedBy = "";
                    newPunchitem.WorkCompletedOn = Convert.ToDateTime("01/01/1900 0:00");

                    newPunchitem.WorkConfirmed = false;
                    newPunchitem.WorkConfirmedByUserID = 0;
                    newPunchitem.WorkConfirmedBy = "";
                    newPunchitem.WorkConfirmedOn = Convert.ToDateTime("01/01/1900 0:00");

                }
                //Cancelled = false;
                //CancelledByUserID = 0;
                //CancelledBy = "";
                ////CancelledOn

                newPunchitem.WorkRejectedReason = "";
                newPunchitem.WorkRejectedBy = "";
                newPunchitem.CancelledBy = "";
                newPunchitem.WorkRejectedCompletedBy = "";

                newPunchitem.UpdatedBy = CurrentUserDetail.FullName;
                newPunchitem.UpdatedOn = DateTime.Now.Date;

                newPunchitem.DisciplineCode = CurrentPageHelper.CurrentLayer.DisciplineCode;
                newPunchitem.IssuerCode = CurrentPageHelper.CurrentLayer.IssuerCode;


                string ETestPackage = " SELECT [SystemNo] FROM [T_ETestPackages] "
                                    + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'" + " AND [ID] = '" + ETPSelected.ID + "'";
                var ETPitem = await _eTestPackagesRepository.QueryAsync<T_ETestPackages>(punchNoSQL);

                newPunchitem.SystemNo = ETPitem.Select(i => i.SystemNo).FirstOrDefault();
                newPunchitem.SpoolDrawingNo = CurrentPageHelper.CurrentDrawing.SpoolDrawingNo;

                newPunchitem.PCWBS = pcwbs;
                newPunchitem.Updated = true;
                MarkETestPackageAsUpdated();
                await _punchListRepository.InsertOrReplaceAsync(newPunchitem);
                CurrentPageHelper.CurrentPunchOverview.PunchAdminID = newPunchitem.PunchAdminID;
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }

        private async void MarkETestPackageAsUpdated()
        {
            string SQL = "UPDATE [T_ETestPackages] SET [Updated] = 1 WHERE [ProjectID] =  '" + ETPSelected.ProjectID + "'"
                                           + " AND [ID] = '" + ETPSelected.ID + "'";
            await _eTestPackagesRepository.QueryAsync<T_ETestPackages>(SQL);
        }

        public void CreateNewPunch_Click()
        {
            if (CurrentPunchPointer.Count > 0)
            {
                //pbSpoolDrawing.Image = CurrentPunchPointer[0].SpoolDrawingImage; //Return image to normal.
                //pbSpoolDrawing.Invalidate();//refreshes the picturebox
                CurrentPunchPointer.Clear();
            }
            var Drawing = Task.Run(async () => await _testLimitDrawingRepository.QueryAsync<T_TestLimitDrawing>("SELECT * FROM [T_TestLimitDrawing]  WHERE [ID] ='" + CurrentPageHelper.SelectedDrawing.ID + "'")).Result;
            T_TestLimitDrawing PDFDrawing = CurrentPageHelper.PDFDrawing = Drawing.FirstOrDefault();
            if (PDFDrawing != null)
            {
                byte[] Base64Stream = Convert.FromBase64String(PDFDrawing.FileBytes);

                PDFImage = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
            }
            if (CurrentPageHelper.CurrentLayer.ID > 0)
            {

                if (ModsTools.CheckCanAddPunch(CurrentUserDetail))
                {
                    if (PDFImage != null) // pbSpoolDrawing.Image != null
                    {
                        Task.Run(async () => await _userDialogs.AlertAsync("Please select two points for the punch", "Create New Punch", "Ok"));
                        CurrentPageHelper.IsDrawVisible = true;

                        //PCBWBS = CurrentPageHelper.CurrentDrawing.PCWBS;
                        //SpoolDrawingText = CurrentPageHelper.CurrentDrawing.DisplayName;

                        //            if (pbSpoolDrawing.SizeMode == PictureBoxSizeMode.Zoom & zoomRatioW == 0)
                        //            {
                        //                //code from image click
                        //                vScrollBarPunchView.Maximum = pnlSpoolDrawing.DisplayRectangle.Height - pnlSpoolDrawing.Height + 50;
                        //                hScrollBarPunchView.Maximum = pnlSpoolDrawing.DisplayRectangle.Width - pnlSpoolDrawing.Width + 50;
                        //                pbZoomImage.Visible = true;

                        //                vScrollZoom.Value = 50;
                        //                pbSpoolDrawing.SizeMode = PictureBoxSizeMode.Normal;
                        //                pbSpoolDrawing.Width = pbSpoolDrawing.Image.Width;
                        //                pbSpoolDrawing.Height = pbSpoolDrawing.Image.Height;
                        //                pbSpoolDrawing.Refresh();

                        //                vScrollBarPunchView.Minimum = 0;

                        //                hScrollBarPunchView.Minimum = 0;
                        //                vScrollZoom.Visible = true;
                        //                zoomRatioW = (double)pbSpoolDrawing.Width / 50.0;
                        //                zoomRatioH = (double)pbSpoolDrawing.Height / 50.0;

                        //            }

                        //            _userDialogs.AlertAsync("Please select two points for the punch", "Create New Punch", "OK");
                        //            CurrentPunchPointer.Add(new PunchPointer() { SpoolDrawingImage = (Image)pbSpoolDrawing.Image.Clone(), SelectingPunchPoints = true });

                    }
                    else
                        Task.Run(async () => await _userDialogs.AlertAsync("Spool drawing is currently not display to pick punch points, please select layer, drawing and press the Display Punch Layer button first.", "Create New Punch", "OK"));

                }
                else
                    Task.Run(async () => await _userDialogs.AlertAsync("You do not have the correct user rights to add punch items to this layer.", "Create New Punch", "OK"));
            }
            else
                Task.Run(async () => await _userDialogs.AlertAsync("Unable to create punches on the test limits layer, please select a punch layer.", "Create New Punch", "OK"));

        }

        public async void LoadAddEditPunchTabsAsync(string param)
        {
            PunchLayerGrid = PDFGrid = false;
            NewPunchGrid = true;
            PresetDescriptionList = null;
            PCBWBS = CurrentPageHelper.CurrentDrawing.PCWBS;
            //SpoolDrawingText = CurrentPageHelper.CurrentDrawing.DisplayName;
            SpoolDrawingText = CurrentPageHelper.SelectedDrawing.DisplayName;

            //Cancleparam = param == "NewPunch" ? "Cancel" : "Back";

            string PunchTypesql = "SELECT DISTINCT [PunchType] FROM [T_AdminPresetPunches] WHERE [ProjectID] = '" + ETPSelected.ProjectID + "' ORDER BY [PunchType] ASC";


            var PunchType = await _adminPresetPunchesRepository.QueryAsync<T_AdminPresetPunches>(PunchTypesql);
            PresetPunchTypeList = PunchTypeList = PunchType.Select(s => s.PunchType).ToList();

            string Categories = " SELECT DISTINCT [Category] FROM [T_AdminPunchCategories] "
                    + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "' AND [SystemPunch] = 0 ORDER BY [Category] ASC";
            var CategoryList = await _adminPunchCategoriesRepository.QueryAsync(Categories);
            PresetCategoryList = CategoryList.Select(i => i.Category).ToList();


            string FunctionCodes = " SELECT * FROM [T_AdminFunctionCodes] "
                                 + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "' ORDER BY [FunctionCode] ASC";
            var functionCodeList = await _adminFunctionCodesRepository.QueryAsync(FunctionCodes);

            FunctionCodeList = new ObservableCollection<T_AdminFunctionCodes>(functionCodeList);


            CompanyCategoryCodes = new ObservableCollection<CompanyCategoryCodeModel>
                             {
                                new CompanyCategoryCodeModel("M - Main Contractor", "M"),
                                new CompanyCategoryCodeModel("S - Sub Contractor", "S"),
                                new CompanyCategoryCodeModel("C - Client", "C"),
                             };
        }

        public bool SavePunch_Click()
        {
            //check if punch exists first else
            bool punchExists = false;
            T_PunchList PunchItem = new T_PunchList();

            if (CurrentPageHelper.PunchID != null)
            {
                string punchID = CurrentPageHelper.CurrentPunchOverview.PunchID.ToString();
                string sql = "SELECT * FROM [T_PunchList] WHERE[ProjectID] = '" + ETPSelected.ProjectID + "' AND [ETestPackageID] = '"
                           + ETPSelected.ID + "' AND [PunchAdminID] = '" + CurrentPageHelper.CurrentLayer.ID + "' AND [PunchID] = '" + punchID + "'";

                var data = Task.Run(async () => await _punchListRepository.QueryAsync<T_PunchList>(sql)).Result;

                PunchItem = data.FirstOrDefault();
            }
            if (punchExists)
            {

                string PunchIDNew = CurrentPageHelper.CurrentLayer.Prefix + "-" + PunchItem.PunchNo.ToString("000") + "-" + SelectedPresetCategory.ToString();

                string updateQuery = " UPDATE [T_PunchList] SET Category ='" + SelectedPresetCategory + "' , Type='" + SelectedpresetpunchType + "' , Description ='" + punchDescription + "'"
                                   + " , Remarks ='" + punchRemarks + "',FunctionCode ='" + SelectedFunctionCode.FunctionCode + "' ,CompanyCategoryCode='" + SelectedCompanyCategoryCodes + "'"
                                   + " ,PCWBS ='" + PCBWBS + "', Updated ='" + 1 + "', UpdatedBy ='" + CurrentUserDetail.FullName + "'"
                                   + " ,UpdatedOn ='" + Convert.ToDateTime(DateTime.UtcNow.ToString(AppConstant.DateSaveFormat)).Ticks + "' ,PresetPunchID ='" + PresetPunchID + "'"
                                   + " ,PunchID ='" + PunchItem.PunchID + "', Live = 1  WHERE [ID] = '" + PunchItem.ID + "'";

                var Updateddata = Task.Run(async () => await _punchListRepository.QueryAsync<T_PunchList>(updateQuery)).Result;

                if (Updateddata.Count > 0)
                {
                    CurrentPunchPointer.Clear();
                    MarkETestPackageAsUpdated();
                    CurrentPageHelper.PunchID = string.Empty;
                    Task.Run(async () => await _userDialogs.AlertAsync("Punch updated.", "Save", "Ok"));
                }
                return true;
            }
            else if (CheckNewPunchIsPopulated())
            {
                string category = SelectedPresetCategory.ToString();
                string functionCode = SelectedFunctionCode.FunctionCode.ToString();

                if (Task.Run(async () => await CheckPunchLayerIsComplete(CurrentPageHelper.CurrentLayer.ID, category, functionCode)).Result)
                {
                    //PunchItem punchItem = new PunchItem();
                    T_PunchList punchItem = new T_PunchList();

                    Boolean savedPunches = true;
                    for (int i = 0; i <= (CurrentPageHelper.PathPoints.Count - 1); i += 2)   //CurrentPunchPointer.Count
                    {
                        Point fp = new Point(CurrentPageHelper.PathPoints[i].Points[0].X, CurrentPageHelper.PathPoints[i].Points[0].Y);
                        Point sp = new Point(CurrentPageHelper.PathPoints[i + 1].Points[0].X, CurrentPageHelper.PathPoints[i + 1].Points[0].Y);
                        CurrentPunchPointer = new List<PunchPointer> { new PunchPointer { FirstPoint = fp, SecondPoint = sp } };

                        if (Task.Run(async () => await CreatePunch(CurrentPunchPointer[0], ETPSelected.TestPackage, category, SelectedpresetpunchType, punchDescription, punchRemarks, functionCode,
                            SelectedCompanyCategoryCodes.value, PCBWBS, false, true, presetPunchID)).Result)
                        {

                        }
                        else
                        {
                            savedPunches = false;
                            Task.Run(async () => await _userDialogs.AlertAsync("Error occured punch not saved.", "Save Punch", "Ok"));
                        }
                    }
                    if (savedPunches)
                    {

                        CurrentPunchPointer.Clear();
                        //Task tsk1 = ShowPunchLayerImageAsync();
                        //Task tsk2 = LoadPunchViewTabAsync(true);

                        ////Task tsk3 = LoadPunchOverviewTabAsync();

                        //await Task.WhenAll(tsk1, tsk2);

                        //ModsTools.MarkETestPackageAsUpdated(Convert.ToInt32(CurrentProject.Project_ID), CurrentPageHelper.ETestPackageID);
                        MarkETestPackageAsUpdated();
                        LoadPunchViewTabAsync();
                        //PunchLayerGrid = NewPunchGrid = false;
                        //PDFGrid = true;

                        //tcETestPackage.SelectedIndex = 1;
                        return true;
                    }

                }
                else
                    Task.Run(async () => await _userDialogs.AlertAsync("Unable to add this punch as category " + category + " punches have been marked as completed", "Save Punch", "Ok"));
                return true;
            }
            else
            {
                Task.Run(async () => await _userDialogs.AlertAsync("Not all information has been completed, please fill out all required fields before saving.", "Save Punch", "Ok"));
                return false;
            }         

            //CurrentPunchPointer[0] = new PunchPointer(); //finally reset punch points for next punch.

        }

        private async void NoPunchClick()
        {
            if (CurrentPageHelper.CurrentLayer.ID > 0)
            {
                if (ModsTools.CheckCanAddPunch(CurrentPageHelper.CurrentLayer, CurrentUserDetail))  //(CurrentPageHelper, CurrentUser))
                {
                    var Drawing = await _testLimitDrawingRepository.QueryAsync<T_TestLimitDrawing>("SELECT * FROM [T_TestLimitDrawing]  WHERE [ID] ='" + CurrentPageHelper.SelectedDrawing.ID + "'");
                    T_TestLimitDrawing PDFDrawing = CurrentPageHelper.PDFDrawing = Drawing.FirstOrDefault();
                    if (PDFDrawing != null)
                    {
                        byte[] Base64Stream = Convert.FromBase64String(PDFDrawing.FileBytes);

                        PDFImage = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
                    }

                    if (PDFImage != null)  //  pbSpoolDrawing.Image != null
                    {
                        // DataTable dt = (DataTable)gvPunches.DataSource;PunchAdminID] = @PunchAdminID ORDER BY[PunchNo] DESC";

                        string SQLipunches = "SELECT [PunchID],[Status],[Category],[TPCConfirmed],[Live],[PunchAdminID]"
                                   + " FROM [T_PunchList]  WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'" + " AND [ETestPackageID] ='" + ETPSelected.ID + "'"
                                   + " AND [VMHub_DocumentsID] = '" + CurrentPageHelper.PDFDrawing.ID + "'" + " AND [PunchAdminID] = '" + CurrentPageHelper.CurrentLayer.ID + "'"
                                   + " ORDER BY [PunchNo] DESC";

                        var data = await _punchListRepository.QueryAsync<T_PunchList>(SQLipunches);

                        bool list1 = data.Where(i => i.Status == "Open" || i.Status == "Closed").Count() == 0;

                        // PunchesList.Where(x => { x.Status = "Open"; x.Status = "Closed"; });
                        if (data.Where(i => i.Status == "Open" || i.Status == "Closed").Count() == 0) //dt.Select("Status = 'Open' or Status = 'Closed'").Length == 0
                        {
                            if (await _userDialogs.ConfirmAsync("Are you sure you want to mark no punch to this spool drawing?", "No Punch", "Yes", "No"))
                            {
                                string punchType = "No Comment";
                                string description = "No Comment";

                                T_AdminPresetPunches presetPunch = new T_AdminPresetPunches();
                                CurrentPunchPointer.Add(new PunchPointer());
                                CurrentPunchPointer[0].FirstPoint = new Point(0, 0);
                                CurrentPunchPointer[0].SecondPoint = new Point(0, 0);

                                string SQL = " SELECT * FROM [T_AdminPresetPunches] WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'"
                                           + " AND [PunchType] = '" + punchType + "'"
                                           + " AND [Description] = '" + description + "'";

                                string Error = "Not Found";
                                var DescriptionList = await _adminPresetPunchesRepository.QueryAsync(SQL);

                                presetPunch = DescriptionList.FirstOrDefault();

                                string sqlQ = "SELECT CLS.[Signed] FROM " +
                                              "(((T_ControlLogSignature as CLS INNER JOIN T_AdminControlLog as ACL ON (CLS.ControlLogAdminID = ACL.ID) AND (CLS.ProjectID = ACL.ProjectID)) " +
                                              "INNER JOIN T_AdminControlLogPunchLayer as ACLPL ON (ACL.ID = ACLPL.ControlLogAdminID) AND (ACL.ProjectID = ACLPL.ProjectID)) " +
                                              "INNER JOIN T_AdminControlLogPunchCategory as ACLPC ON (ACLPL.ProjectID = ACLPC.ProjectID) AND (ACLPL.ControlLogAdminID = ACLPC.ControlLogAdminID)) " +
                                              "INNER JOIN T_AdminControlLogActionParty as ACLAP ON (ACLPC.ProjectID = ACLAP.ProjectID) AND (ACLPC.ControlLogAdminID = ACLAP.ControlLogAdminID) " +

                                              "WHERE CLS.[ProjectID] = '" + ETPSelected.ProjectID + "' AND CLS.[ETestPackageID] = '" + ETPSelected.ID + "' AND CLS.[Signed] = 1 " +
                                              "AND ACLPL.[PunchAdminID] = '" + CurrentPageHelper.CurrentLayer.ID + "' AND ACLPC.[Category] = '" + presetPunch.Category + "' " +
                                              "AND (ACL.[PunchesCompleted] = 1 OR ACL.[PunchesConfirmed] = 1) " +
                                              "AND  ACLAP.[FunctionCode] = '" + presetPunch.FunctionCode + "'";

                                var CLSData = await _controlLogSignatureRepository.QueryAsync<T_ControlLogSignature>(sqlQ);
                                bool check = CLSData.Select(i => i.Signed).FirstOrDefault();

                                if (!CLSData.Select(i => i.Signed).FirstOrDefault())
                                {
                                    T_PunchList punchItem = new T_PunchList();
                                    if (await CreatePunch(CurrentPunchPointer[0], ETPSelected.TestPackage, presetPunch.Category,
                                            presetPunch.PunchType, presetPunch.Description, presetPunch.Remarks, "", "", CurrentPageHelper.CurrentDrawing.PCWBS, true, false, ""))
                                    {
                                        //ModsTools.MarkETestPackageAsUpdated(Convert.ToInt32(CurrentProject.Project_ID), CurrentPageHelper.ETestPackageID);
                                        MarkETestPackageAsUpdated();

                                        CurrentPunchPointer[0] = new PunchPointer(); //finally reset punch points for next punch.

                                    }
                                    else
                                        await _userDialogs.AlertAsync("Error occured punch not saved.", "No Punch", "OK");
                                }
                                else
                                    await _userDialogs.AlertAsync("Unable to add this punch as category " + presetPunch.Category + " punches have been marked as completed", "No Punch", "OK");
                            }
                        }
                        else
                            await _userDialogs.AlertAsync("Unable to add No Punch when there are open or closed Punches.", "No Punch", "OK");
                    }
                    else
                        await _userDialogs.AlertAsync("Spool drawing is currently not display to pick punch points, please select layer, drawing and press the Display Punch Layer button first.", "No Punch", "OK");
                }
                else
                    await _userDialogs.AlertAsync("You do not have the correct user rights to add punch items to this layer.", "No Punch", "OK");
            }
            else
                await _userDialogs.AlertAsync("Unable to create punches on the test limits layer, please select a punch layer.", "No Punch", "OK");
        }

        private Boolean CheckNewPunchIsPopulated()
        {
            string[] liststring = new string[] { PCBWBS != null ?PCBWBS:"", punchDescription != null ? punchDescription : "",
                                                 SelectedPresetCategory != null ? SelectedPresetCategory.ToString() : "",
                                                 SelectedpresetpunchType != null ? SelectedpresetpunchType.ToString() : "",
                                                 SelectedCompanyCategoryCodes != null ? SelectedCompanyCategoryCodes.ToString() : "",
                                                 SelectedFunctionCode != null ? SelectedFunctionCode.ToString() : ""
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
            string UpdateQuery = " SELECT CLS.[Signed] FROM (((T_ControlLogSignature as CLS "
                         + " INNER JOIN T_AdminControlLog ACL ON (CLS.ControlLogAdminID = ACL.ID) AND (CLS.ProjectID = ACL.ProjectID)) "
                         + " INNER JOIN T_AdminControlLogPunchLayer ACLPL ON (ACL.ID = ACLPL.ControlLogAdminID) AND (ACL.ProjectID = ACLPL.ProjectID)) "
                         + " INNER JOIN T_AdminControlLogPunchCategory ACLPC ON (ACLPL.ProjectID = ACLPC.ProjectID) AND (ACLPL.ControlLogAdminID = ACLPC.ControlLogAdminID)) "
                         + " INNER JOIN T_AdminControlLogActionParty ACPAP ON (ACLPC.ProjectID = ACPAP.ProjectID) AND (ACLPC.ControlLogAdminID = ACPAP.ControlLogAdminID) "

                         + " WHERE CLS.[ProjectID] = '" + ETPSelected.ProjectID + "' AND CLS.[ETestPackageID] = '" + ETPSelected.ID + "' AND CLS.[Signed] = 1 "
                         + " AND ACLPL.[PunchAdminID] = '" + punchAdminID + "' AND ACLPC.[Category] = '" + category + "'"
                         + " AND (ACL.[PunchesCompleted] = 1 OR ACL.[PunchesConfirmed] = 1) "
                         + " AND  ACPAP.[FunctionCode] = '" + functionCode + "'";

            var UpdatedControlLog = await _controlLogSignatureRepository.QueryAsync<T_ControlLogSignature>(UpdateQuery);

            return !UpdatedControlLog.Select(i => i.Signed).FirstOrDefault();

        }
        #endregion

        #region Public          
        public async void UpdateGrid(string Option, PunchListModel Punchitem)
        {
            if (Option == "CaptureImage")
            {
                CameraGrid = true;
                PunchLayerGrid = !PunchLayerGrid;
            }
            else if (Option == "DeleteRow")
            {
                //Can Delete
                if (await _userDialogs.ConfirmAsync("Are you sure you want to delete?", "Delete Record", "Yes", "No"))
                {
                    string deletePunch = " DELETE FROM [T_PunchList] WHERE [ProjectID] = '" + ETPSelected.ProjectID + "' AND [ETestPackageID] = '" + ETPSelected.ID
                                  + "' AND [PunchID] = '" + Punchitem.PunchID + "'";
                    await _punchListRepository.QueryAsync<T_PunchList>(deletePunch);

                    string deletepunchimage = " DELETE FROM [T_PunchImage] WHERE [ProjectID] = '" + ETPSelected.ProjectID + "' AND [ETestPackageID] = '" + ETPSelected.ID
                                       + "' AND [PunchID] = '" + Punchitem.PunchID + "'";
                    await _punchImageRepository.QueryAsync<T_PunchImage>(deletepunchimage);

                    await _userDialogs.AlertAsync("Punch has been deleted successfully.", "Delete Punch", "Ok");

                    GeneratePunchDataTable(CurrentPageHelper.CurrentDrawing.ID);
                }
            }
        }

        //camera functinality 
        #region Camera Fuctionalty
        private async void generatepath()
        {

            string Folder = ("Photo Store" + "\\" + CurrentPageHelper.ETestPackage.ID.ToString());  //Row.Number
            InspectionPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath(Folder);

        }

        private async void PickImagesFromGallery()
        {
            //generatepath();
            //try
            //{

            //    var PickFile = await DependencyService.Get<ISaveFiles>().PickFile(InspectionPath);
            //    ImageFiles = new ObservableCollection<string>(await DependencyService.Get<ISaveFiles>().GetAllImages(InspectionPath));
            //    if (PickFile)
            //        await _userDialogs.AlertAsync("Added image file successfully", "Saved Image", "OK");
            //}
            //catch (Exception ex)
            //{

            //}
            try
            {
                var mediaFile = await PickFileAsync();
                if (mediaFile == null)
                {
                    return;
                }
                var memoryStream = new MemoryStream();
                await mediaFile.GetStream().CopyToAsync(memoryStream);
                imageAsByte = memoryStream.ToArray();
                Stream stream = new MemoryStream(imageAsByte);
                CapturedImage = ImageSource.FromStream(() => stream);

            }
            catch (Exception ex)
            {

            }
        }

        private async void CaptureImageSave()
        {
            if (imageAsByte != null)
            {
                TestPackageImage img = new TestRecordImage();

                //string path = await DependencyService.Get<ISaveFiles>().SavePictureToDisk(InspectionPath, DateTime.Now.ToString(AppConstant.CameraDateFormat), imageAsByte.ToArray());
                //if (path != null)
                //{
                //    generatepath();
                //    RenameImage = false;
                //    await _userDialogs.AlertAsync("Successfully saved..!", null, "Ok");
                //}

                Byte[] fileBytes = imageAsByte;
                img.DisplayName = "Image";
                img.Extension = ".PNG";
                img.FileName = "NewImage";
                img.FileSize = fileBytes.Count();
                img.FileBytes = Convert.ToBase64String(fileBytes);
                bool insertImage = false;
                try
                {
                    var SQL = img.InsertQuery(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID);
                    // var data = await _TestRecordImageRepository.QueryAsync(SQL);
                    insertImage = true;
                }
                catch (Exception EX)
                {
                    insertImage = false;
                }

                if (insertImage)
                {
                    ////CameraLB.Items.Add(img);
                    //UpdateETestPackageStatus(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID);

                    //string SQL1 = "UPDATE [T_PunchList] SET [Updated] = TRUE WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ETestPackageID] = '" + CurrentPageHelper.ETestPackage.ID + "' AND [PunchID] = '" + CurrentPageHelper.PunchID + "'";

                    //var data1 = await _TestRecordImageRepository.QueryAsync(SQL1);
                    //_userDialogs.AlertAsync("Saved Successfully...! .", "Camera Image Save", "OK");
                }
                else
                    _userDialogs.AlertAsync("Error saving image to database.", "Camera Image Save", "OK");

            }

            else
                _userDialogs.AlertAsync("Please select camera and take a picture to save", null, "OK");
        }


        //private void SaveCameraImage(Image image, TestPackageImage img)
        //{
        //    Byte[] fileBytes = ReduceImageFileSize(image);

        //    img.FileSize = fileBytes.Count();
        //    img.FileBytes = Convert.ToBase64String(fileBytes);


        //    if (img.Insert(Convert.ToInt32(CurrentProject.Project_ID), CurrentPageHelper.ETestPackageID))
        //    {
        //        CameraLB.Items.Add(img);
        //        ModsTools.MarkETestPackageAsUpdated(Convert.ToInt32(CurrentProject.Project_ID), CurrentPageHelper.ETestPackageID);

        //        if (LocalSQLFunctions.UpdateRecord("UPDATE [PunchListHH] SET [Updated] = TRUE WHERE [ProjectID] = @ProjectID AND [ETestPackageID] = @ETestPackageID AND [PunchID] = @PunchID",
        //                new OleDbParameter[]
        //                {
        //                new OleDbParameter("@ProjectID", Convert.ToInt32(CurrentProject.Project_ID)),
        //                new OleDbParameter("@ETestPackageID", CurrentPageHelper.ETestPackageID),
        //                new OleDbParameter("@PunchID", CurrentPageHelper.PunchID)

        //                }))
        //        {

        //        }
        //        else
        //            MessageBox.Show("Error saving image to database.", "Camera Image Save");
        //    }
        //    else
        //        MessageBox.Show("Error saving image to database.", "Camera Image Save");
        //}




        private async void CaptureImageDelete()
        {
            if (await _userDialogs.ConfirmAsync($"Are you sure you want to delete?", $"Delete image", "Yes", "No"))
            {
                bool isdelete = await DependencyService.Get<ISaveFiles>().DeleteImage(InspectionPath + "/" + SelectedImageFiles);
                if (isdelete)
                {
                    ImageFiles = new ObservableCollection<string>(await DependencyService.Get<ISaveFiles>().GetAllImages(InspectionPath));
                    btnSaveDelete = "Save";
                    RenameImage = false;
                    CapturedImage = NewImageName = "";
                }
            }
        }

        private async void LoadImageFiles(string SelectedImageFiles)
        {
            if (SelectedImageFiles == null)
                return;
            CapturedImage = await DependencyService.Get<ISaveFiles>().GetImage(InspectionPath + "/" + SelectedImageFiles);
            btnSaveDelete = "Delete";
            RenameImage = true;
        }

        private async void CameraService()
        {
            try
            {
                var mediaFile = await TakePhotoAsync();
                if (mediaFile == null)
                {
                    return;
                }
                var memoryStream = new MemoryStream();
                await mediaFile.GetStream().CopyToAsync(memoryStream);
                imageAsByte = await _resizeImageService.GetResizeImage(memoryStream.ToArray());
                Stream stream = new MemoryStream(imageAsByte);
                CapturedImage = ImageSource.FromStream(() => stream);

            }
            catch (Exception ex)
            {

            }
        }

        private async Task<MediaFile> TakePhotoAsync()
        {
            var denied = await CheckPermissions();
            if (denied)
            {
                await Application.Current.MainPage.DisplayAlert("Unable to take photos.", "Permissions Denied. Please modify app permisions in settings.", "OK");

                return null;
            }

            if (!_media.IsCameraAvailable || !_media.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", "No camera available!", "OK");
                return null;
            }

            MediaFile file = null;
            try
            {
                file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    DefaultCamera = CameraDevice.Rear
                });


            }
            catch (Exception ex)
            {

            }

            return file == null ? null : file;
        }
        public async Task<MediaFile> PickFileAsync()
        {
            var denied = await CheckPermissions();
            if (denied)
            {
                await Application.Current.MainPage.DisplayAlert("Unable to pick a file.", "Permissions Denied. Please modify app permisions in settings.", "OK");

                return null;
            }

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Gallery", "Picking a photo is not supported.", "OK");
                return null;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();
            return file == null ? null : file;
        }
        private async Task<bool> CheckPermissions()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                    storageStatus = results[Permission.Storage];
                    cameraStatus = results[Permission.Camera];
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                        storageStatus = results[Permission.Storage];
                        cameraStatus = results[Permission.Camera];
                    });
                }
            }

            return cameraStatus != PermissionStatus.Granted && storageStatus != PermissionStatus.Granted;
        }

        private void SelectedCameraItems(string SelectedCameraItem)
        {
            if (SelectedCameraItem == null)
                return;
            CameraService();
        }
        public async Task<byte[]> ResizeImage(byte[] imageAsByte)
        {
            return await _resizeImageService.GetResizeImage(imageAsByte);
        }
        #endregion

        //public async void GetPunchRecordImages()
        //{
        //    List<PunchImage> list = new List<PunchImage>();

        //    string SQL = "SELECT * FROM [T_TestRecordImage] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ETestPackageID]= '" + CurrentPageHelper.ETestPackage.ID + "'";

        //    var data = await _punchImageRepository.QueryAsync<T_PunchImage>(SQL);

        //    foreach (T_PunchImage TR in data)
        //    {

        //        PunchImage img = new PunchImage
        //        {
        //            DisplayName = TR.DisplayName,
        //            FileName = TR.FileName,
        //            Extension = TR.Extension,
        //            FileSize = TR.FileSize,
        //            FileBytes = TR.FileBytes,
        //        };

        //        list.Add(img);
        //    }

        //    ImageFiles = new ObservableCollection<PunchImage>(list);
        //}
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            // CheckValidLogin._pageHelper = new PageHelper();
            base.OnNavigatedFrom(parameters);
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            //if (parameters.Count == 0)
            //{
            //    return;
            //}

            //if (parameters.Count > 1 && parameters.ContainsKey(NavigationParametersConstants.SelectedPunchOverview))
            //{
            SelectedPunchOverview = CurrentPageHelper.CurrentPunchOverview; ;
            ETPSelected = CurrentPageHelper.ETestPackage;
            var UserDetailsList = await _userDetailsRepository.GetAsync();
            if (UserDetailsList.Count > 0)
                CurrentUserDetail = UserDetailsList.Where(p => p.ID == Settings.UserID).FirstOrDefault();

            var UserProjectList = await _userProjectRepository.GetAsync();
            if (UserProjectList.Count > 0)
                userProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();


            if (SelectedPunchOverview != null && SelectedPunchOverview.PunchAdminID != 0 && CurrentPageHelper.IsOnlyOverview)
            {
                getPopUp(false);

            }

            if (SelectedPunchOverview != null)
            {
                string SQL = "SELECT * FROM [T_TestLimitDrawing]"
                             + " WHERE [ProjectID] = '" + ETPSelected.ProjectID + "'"
                             + " AND [ETestPackageID] = '" + ETPSelected.ID + "'"
                             + " AND [ID] ='" + SelectedPunchOverview.ID + "'";

                var data = await _testLimitDrawingRepository.QueryAsync<T_TestLimitDrawing>(SQL);
                CurrentPageHelper.CurrentDrawing = data.Select(i => i).FirstOrDefault();
                GetPunchLayerGridData();
            }
            //}
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        #endregion
    }
}

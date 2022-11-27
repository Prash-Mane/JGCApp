using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Extentions;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.DataBase.DataTables.WorkPack;
using JGC.Models.E_Test_Package;
using JGC.Models.Work_Pack;
using JGC.Views;
using JGC.Views.Work_Pack;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JGC.ViewModels.Work_Pack
{
    public class AddPunchControlViewModel : BaseViewModel, INotifyPropertyChanged
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IResizeImageService _resizeImageService;
        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_CWPDrawings> _CWPDrawingsRepository;
        private readonly IRepository<T_TagMilestoneStatus> _tagMilestoneStatusRepository;
        private readonly IRepository<T_TagMilestoneImages> _tagMilestoneImagesRepository;
        private readonly IRepository<T_IWP> _iwpRepository;
        private IList<T_TagMilestoneStatus> CWPTagsData;
        private List<TagMilestoneStatusModel> CWPList = new List<TagMilestoneStatusModel>();
        private T_UserDetails userDetail;
        public byte[] imageAsByte = null;
        private readonly IMedia _media;
        private T_UserProject CurrentUserProject;
        private readonly IRepository<T_CWPDrawings> _cwpDrawingsRepository;
        private readonly IRepository<T_IWPPunchControlItem> _iwpPunchControlItemRepository;
        private readonly IRepository<T_IWPPunchImage> _iwpPunchImagesRepository;
        private readonly IRepository<T_IWPPunchCategories> _iWPPunchCategoriesRepository;
        private readonly IRepository<T_IWPFunctionCodes> _iWPFunctionCodesRepository;
        private readonly IRepository<T_IWPCompanyCategoryCodes> _iWPCompanyCategoryCodesRepository;
        private readonly IRepository<T_IWPAdminPunchLayer> _iWPAdminPunchLayerRepository;
        private readonly IRepository<T_CwpTag> _CwpTAg;
        private readonly IRepository<T_IWPDrawings> _iwpDrawingsRepository;
        private readonly IRepository<T_IWPAttachments> _iwpAttachmentsRepository;

        private T_IWP IWP;
        public PunchControlLayerModel SelectedPunchControlLayer;
        private readonly IRepository<T_IwpPresetPunch> _IwpPresetPunchRepository;
        static List<IWPPunchPointer> CurrentPunchPointer = new List<IWPPunchPointer>();



      
        #region properties
        private ObservableCollection<TagMilestoneStatusModel> _cwpTagStatus;
        public ObservableCollection<TagMilestoneStatusModel> CWPTagStatusLists
        {
            get { return _cwpTagStatus; }
            set { _cwpTagStatus = value; RaisePropertyChanged(); }
        }
        private T_CwpTag _selectedCWPTag;
        public T_CwpTag SelectedCWPTag
        {
            get { return _selectedCWPTag; }
            set { _selectedCWPTag = value; RaisePropertyChanged(); }
        }

        private List<T_CwpTag> _cwpTag;
        public List<T_CwpTag> CWPTags
        {
            get { return _cwpTag; }
            set { _cwpTag = value; RaisePropertyChanged(); }
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
        private bool isSaveVisible;
        public bool IsSaveVisible
        {
            get { return isSaveVisible; }
            set
            {
                SetProperty(ref isSaveVisible, value);
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

        private DateTime statusValue { get; set; }
        public DateTime StatusValue
        {
            get { return statusValue; }
            set
            {
                statusValue = value;
                RaisePropertyChanged();
            }
        }

        private DateTime propertyMinimumDate { get; set; }
        public DateTime PropertyMinimumDate
        {
            get { return propertyMinimumDate; }
            set
            {
                propertyMinimumDate = value;
                RaisePropertyChanged();
            }
        }

        private DateTime propertyMaximumDate { get; set; }
        public DateTime PropertyMaximumDate
        {
            get { return propertyMaximumDate; }
            set
            {
                propertyMaximumDate = value;
                RaisePropertyChanged();
            }
        }
        private ObservableCollection<string> _cameraItems;
        public ObservableCollection<string> CameraItems
        {
            get { return _cameraItems; }
            set { _cameraItems = value; RaisePropertyChanged(); }
        }

        private bool _cameraGrid = false;
        public bool CameraGrid
        {
            get { return _cameraGrid; }
            set { SetProperty(ref _cameraGrid, value); }
        }

        private bool _PDFGrid = false;
        public bool PDFGrid
        {
            get { return _PDFGrid; }
            set { SetProperty(ref _PDFGrid, value); }
        }
        private bool addPunchGrid = true;
        public bool AddPunchGrid
        {
            get { return addPunchGrid; }
            set { SetProperty(ref addPunchGrid, value); }
        }
        private bool _showSaveRename;
        public bool ShowSaveRename
        {
            get { return _showSaveRename; }
            set { SetProperty(ref _showSaveRename, value); }
        }
        private string _btnSaveRename;
        public string btnSaveRename
        {
            get { return _btnSaveRename; }
            set { SetProperty(ref _btnSaveRename, value); }
        }
      
        private ObservableCollection<T_TagMilestoneImages> _ImageFiles;
        public ObservableCollection<T_TagMilestoneImages> ImageFiles
        {
            get { return _ImageFiles; }
            set { _ImageFiles = value; RaisePropertyChanged(); }
        }
        private T_TagMilestoneImages _selectedImageFiles;
        public T_TagMilestoneImages SelectedImageFiles
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
            set { SetProperty(ref newImageName, value); }
        }

        private bool _isVisibleTagMemberUi;
        public bool IsVisibleTagMemberUi
        {
            get { return _isVisibleTagMemberUi; }
            set { SetProperty(ref _isVisibleTagMemberUi, value); }
        }

        private bool _isVisibleTagStatus;
        public bool IsVisibleTagStatus
        {
            get { return _isVisibleTagStatus; }
            set { SetProperty(ref _isVisibleTagStatus, value); }
        }

        private string btnText;
        public string BtnText
        {
            get { return btnText; }
            set { SetProperty(ref btnText, value); }
        }

        private string serachString;
        public string SerachString
        {
            get { return serachString; }
            set { SetProperty(ref serachString, value); }
        }

        bool HasTagMemberFunctionbality;

        private bool iSVisibleBackbtn;
        public bool ISVisibleBackbtn
        {
            get { return iSVisibleBackbtn; }
            set { SetProperty(ref iSVisibleBackbtn, value); }
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
                  //  OnClickButton("OpenPDF");
                    OnPropertyChanged();

                    PDFGrid = true;
                    // PDFGrid = false;
                    AddPunchGrid = false;
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
               _= CascadepresetDropdown("punchtype");
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

      
        private string _punchRemarks;
        public string PunchRemarks
        {
            get { return _punchRemarks; }
            set
            {
                SetProperty(ref _punchRemarks, value);
            }
        }

        private List<string> functionCodeList;
        public List<string> FunctionCodeList
        {
            get { return functionCodeList; }
            set { functionCodeList = value; RaisePropertyChanged(); }
        }

        private string selectedFunctionCode;
        public string SelectedFunctionCode
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

        private List<string> _sectionCOde;
        public List<string> SectionCodeList
        {
            get { return _sectionCOde; }
            set
            {
                SetProperty(ref _sectionCOde, value);
            }
        }
        private string _selectedSectionCode;
        public string SelectedSectionCode
        {
            get { return _selectedSectionCode; }
            set
            {
                SetProperty(ref _selectedSectionCode, value);
            }
        }

        private List<string> _FWBSList;
        public List<string> FWBSList
        {
            get { return _FWBSList; }
            set
            {
                SetProperty(ref _FWBSList, value);
            }
        }
        private string _selectedFWBS;
        public string SelectedFWBS
        {
            get { return _selectedFWBS; }
            set
            {
                SetProperty(ref _selectedFWBS, value);
               _= CascadepresetDropdown("FWBS");
            }
        }

        private List<string> _ComponentCategoryList;
        public List<string> ComponentCategoryList
        {
            get { return _ComponentCategoryList; }
            set
            {
                SetProperty(ref _ComponentCategoryList, value);
            }
        }

        private string _selectedComponentCategory;
        public string SelectedComponentCategory
        {
            get { return _selectedComponentCategory; }
            set
            {
                SetProperty(ref _selectedComponentCategory, value);
                _ = CascadepresetDropdown("ComponantCategory");
            }
        }

        private List<string> _ComponentList;
        public List<string> ComponentList
        {
            get { return _ComponentList; }
            set
            {
                SetProperty(ref _ComponentList, value);
            }
        }

        private string _selectedComponent;
        public string SelectedComponent
        {
            get { return _selectedComponent; }
            set
            {
                SetProperty(ref _selectedComponent, value);
                _ = CascadepresetDropdown("componant");
            }
        }

        private List<string> _ActionList;
        public List<string> ActionList
        {
            get { return _ActionList; }
            set
            {
                SetProperty(ref _ActionList, value);
            }
        }

        private string _selectedAction;
        public string SelectedAction
        {
            get { return _selectedAction; }
            set
            {
                SetProperty(ref _selectedAction, value);
            }
        }

        private DateTime? targetDAte { get; set; }
        public DateTime? TargetDAte
        {
            get { return targetDAte; }
            set
            {
                targetDAte = value;
                RaisePropertyChanged();
            }
        }

        private string _preSetDescription;
        public string PreSetDescription
        {
            get { return _preSetDescription; }
            set
            {
                SetProperty(ref _preSetDescription, value);
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
        private T_IWPDrawings pdfDrawing;
        public T_IWPDrawings PDFDrawing
        {
            get { return pdfDrawing; }
            set
            {
                SetProperty(ref pdfDrawing, value);
            }
        }

        private ObservableCollection<T_IWPPunchCategories> punchCategories;
        public ObservableCollection<T_IWPPunchCategories> PunchCategories
        {
            get { return punchCategories; }
            set { punchCategories = value; RaisePropertyChanged(); }
        }


        private string _TxtOtherComponent;
        public string TxtOtherComponent
        {
            get { return _TxtOtherComponent; }
            set
            {
                SetProperty(ref _TxtOtherComponent, value);
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
      

        public AddPunchControlViewModel(INavigationService _navigationService,
           IUserDialogs _userDialogs,
           IHttpHelper _httpHelper,
           IMedia _media,
           ICheckValidLogin _checkValidLogin,
           IResizeImageService _resizeImageService,
           IRepository<T_UserDetails> _userDetailsRepository,
           IRepository<T_UserProject> _userProjectRepository, 
           IRepository<T_CWPDrawings> _CWPDrawingsRepository,
           IRepository<T_TagMilestoneStatus> _tagMilestoneStatusRepository,
           IRepository<T_TagMilestoneImages> _tagMilestoneImagesRepository,
           IRepository<T_CWPDrawings> _cwpDrawingsRepository,
           IRepository<T_IWPPunchControlItem> _iwpPunchControlItemRepository,
           IRepository<T_IWPPunchImage> _iwpPunchImagesRepository,
           IRepository<T_IWPPunchCategories> _iWPPunchCategoriesRepository,
           IRepository<T_IWPFunctionCodes> _iWPFunctionCodesRepository,
           IRepository<T_IWPCompanyCategoryCodes> _iWPCompanyCategoryCodesRepository,
           IRepository<T_IWPAdminPunchLayer> _iWPAdminPunchLayerRepository,
           IRepository<T_IwpPresetPunch> _IwpPresetPunchRepository,
           IRepository<T_CwpTag> _CwpTAg,
           IRepository<T_IWPDrawings> _iwpDrawingsRepository,
           IRepository<T_IWPAttachments> _iwpAttachmentsRepository,
        IRepository<T_IWP> _iwpRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._resizeImageService = _resizeImageService;
            this._userDialogs = _userDialogs;
            this._userDetailsRepository = _userDetailsRepository;
            this._userProjectRepository = _userProjectRepository;
            this._CWPDrawingsRepository = _CWPDrawingsRepository;
            this._tagMilestoneStatusRepository = _tagMilestoneStatusRepository;
            this._tagMilestoneImagesRepository = _tagMilestoneImagesRepository;
            this._iwpRepository = _iwpRepository;
            this._media = _media;
            this._cwpDrawingsRepository = _cwpDrawingsRepository;
            this._iwpPunchControlItemRepository = _iwpPunchControlItemRepository;
            this._iwpPunchImagesRepository = _iwpPunchImagesRepository;
            this._iWPPunchCategoriesRepository = _iWPPunchCategoriesRepository;
            this._iWPFunctionCodesRepository = _iWPFunctionCodesRepository;
            this._iWPCompanyCategoryCodesRepository = _iWPCompanyCategoryCodesRepository;
            this._iWPAdminPunchLayerRepository = _iWPAdminPunchLayerRepository;
            this._IwpPresetPunchRepository = _IwpPresetPunchRepository;
            this._CwpTAg = _CwpTAg;
            this._iwpDrawingsRepository = _iwpDrawingsRepository;
            this._iwpAttachmentsRepository = _iwpAttachmentsRepository;
            _media.Initialize();
            _userDialogs.HideLoading();
            HasTagMemberFunctionbality = false;
            PageHeaderText = "CWP Tag Status";
            btnSaveRename = "Save Image";
            JobSettingHeaderVisible = Showbuttons =true;
            GetUserDetails();
            GetCWPTagStatusLists();
            
           _ =BindData();
        }

        
        #region private  
        //new methods
        private async void GetUserDetails()
        {
           
            var UserDetailsList = await _userDetailsRepository.GetAsync();
            if (UserDetailsList.Count > 0)
                userDetail = UserDetailsList.Where(p => p.ID == Settings.UserID).FirstOrDefault();

            var QueryResult = await _userProjectRepository.GetAsync(x => x.Project_ID == Settings.ProjectID);
            CurrentUserProject = QueryResult.FirstOrDefault();
           
        }
        private async void GetCWPTagStatusLists()
        {
           

            string TMSSQL = "SELECT * FROM T_CwpTag WHERE IWPID = '" + IWPHelper.IWP_ID + "' AND ProjectID = '" + Settings.ProjectID+"'";
            var CWPTagNoData = await _CwpTAg.QueryAsync<T_CwpTag>(TMSSQL);

            CWPTags = CWPTagNoData.Distinct().ToList();
            
        }

        private async Task GetPunchLayerGridData()
        {
            var getIWP = await _iwpRepository.QueryAsync<T_IWP>("SELECT * FROM [T_IWP] WHERE [ID] = '" + IWPHelper.IWP_ID + "'");

            IWP = getIWP.FirstOrDefault();

            var UserDetailsList = await _userDetailsRepository.GetAsync();
            if (UserDetailsList.Count > 0)
                userDetail = UserDetailsList.Where(p => p.ID == Settings.UserID).FirstOrDefault();

            var UserProjectList = await _userProjectRepository.GetAsync();
            if (UserProjectList.Count > 0)
                CurrentUserProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();

            var AdminPunchLayer = await _iWPAdminPunchLayerRepository.QueryAsync<T_IWPAdminPunchLayer>("SELECT * FROM [T_IWPAdminPunchLayer] WHERE [ProjectID] = '" + Settings.ProjectID + "' ORDER BY [LayerNo] ASC");

            PunchLayersList = new ObservableCollection<T_IWPAdminPunchLayer>(AdminPunchLayer);
            SelectedPunchLayer = PunchLayersList.FirstOrDefault();

        }

        private void GetSelectedpunchLayer()
        {
           // throw new NotImplementedException();
        }

       

        private async Task GetDrawingList()
        {
            //var getIWP = Task.Run(async () => await _iwpRepository.QueryAsync<T_IWP>("SELECT * FROM [T_IWP] WHERE [ID] = '" + IWPHelper.IWP_ID + "'")).Result;

            //CurrentIWP = getIWP.FirstOrDefault();

            string SQLdrawing = "SELECT * FROM [T_IWPDrawings] WHERE [IWPID] = '" + IWPHelper.IWP_ID + "'";

            var Dlist =  await _iwpDrawingsRepository.QueryAsync<T_IWPDrawings>(SQLdrawing);

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

        private async Task BindData()
        {
          await GetPunchLayerGridData();
          await GetDrawingList();

            //cascading down dropdowns
            //fwbs
            string FWBSListsql = "SELECT DISTINCT [FWBS] FROM [T_IwpPresetPunch] WHERE [ProjectID] = '" + Settings.ProjectID + "' ORDER BY [FWBS] ASC";
            var datafwbs = await _IwpPresetPunchRepository.QueryAsync<T_IwpPresetPunch>(FWBSListsql);
            FWBSList  = datafwbs.Select(s => s.FWBS).ToList();
           

            CameraItems = new ObservableCollection<string> { "Camera" };

            //sectioncode
            string SectionCodesql = "SELECT DISTINCT [SectionCode] FROM [T_IwpPresetPunch] WHERE [ProjectID] = '" + Settings.ProjectID + "' ORDER BY [SectionCode] ASC";
            var sectionacode = await _IwpPresetPunchRepository.QueryAsync<T_IwpPresetPunch>(SectionCodesql);
            SectionCodeList = sectionacode.Select(s => s.SectionCode).ToList();


            //punchcategory
          
            PresetCategoryList = new List<string> { "A", "B", "C" };


            //function code
            string FunctionCodes = "SELECT [FunctionCode] FROM [T_IWPFunctionCodes] "
                                 + " WHERE [ProjectID] = '" + Settings.ProjectID+ "' ORDER BY [FunctionCode] ASC";
            var functionCodeList = await _iWPFunctionCodesRepository.QueryAsync<T_IWPFunctionCodes>(FunctionCodes);
            FunctionCodeList = functionCodeList.Select(i=>i.FunctionCode).ToList();

            //compony cat codes
            CompanyCategoryCodes = new ObservableCollection<CompanyCategoryCodeModel>
                             {
                                new CompanyCategoryCodeModel("M - Main Contractor", "M"),
                                new CompanyCategoryCodeModel("S - Sub Contractor", "S"),
                                new CompanyCategoryCodeModel("C - Client", "C"),
                             };

            string _Categories = " SELECT DISTINCT * FROM [T_IWPPunchCategories] "
                            + " WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [SystemPunch] = 0 ORDER BY [Category] ASC";
            var Categorylist = await _iWPPunchCategoriesRepository.QueryAsync(_Categories);
            PunchCategories = new ObservableCollection<T_IWPPunchCategories>(Categorylist);

        }

        private async Task SetPresetPunchDAta()
        {
            if (SelectedFWBS != null && PresetPunchTypeList != null && SelectedComponentCategory != null && SelectedComponent != null && SelectedAction != null)
            {
                //Action
                string presetpunchSql = "SELECT * FROM [T_IwpPresetPunch] WHERE [ProjectID] = '" + Settings.ProjectID + "'";
                var persetList = await _IwpPresetPunchRepository.QueryAsync<T_IwpPresetPunch>(presetpunchSql);

                var data = persetList.Where(x => x.FWBS == SelectedFWBS && x.PunchType == SelectedpresetpunchType && x.ComponentCategory == SelectedComponentCategory && x.Component == SelectedComponent && x.Action == SelectedAction).ToList();
                if (data.Any())
                {
                    var presetpunchdata = data.FirstOrDefault();

                    SelectedFunctionCode = presetpunchdata.FunctionCode;
                    SelectedCompanyCategoryCodes = CompanyCategoryCodes.Where(x=>x.value == presetpunchdata.CompanyCategoryCode).FirstOrDefault();
                    SelectedSectionCode = presetpunchdata.SectionCode;
                    SelectedPresetCategory = presetpunchdata.PunchCategory;
                    PreSetDescription = SelectedComponent + " " + SelectedAction;


                }

            }
            else await _userDialogs.AlertAsync("You must select an FWBS, Punch Type, Component, Component Category and Action before selecting a preset punch", "pre-set Punch", "Ok");
        }



        public  bool CheckCanAddPunch()
        {
            return (userDetail.Company_Category_Code.ToUpper() == SelectedPunchLayer.CompanyCategoryCode.ToUpper()
                && userDetail.Function_Code.ToUpper() == SelectedPunchLayer.FunctionCode.ToUpper());
        }
        public async Task SavePresetPunch()
        {
             if (CheckNewPunchIsPopulated())
            {
                if (CheckCanAddPunch())
                {

                string category = SelectedPresetCategory;
                string functionCode = SelectedFunctionCode;

               
                    //PunchItem punchItem = new PunchItem();
                    T_IWPPunchControlItem punchItem = new T_IWPPunchControlItem();

                    Boolean savedPunches = true;

                if(IWPHelper.PathPoints.Count == 0) 
                {
                    if (await CreatePunch(new IWPPunchPointer(), IWP.Title, category, PreSetDescription, PunchRemarks, functionCode,
                          SelectedCompanyCategoryCodes.value, false, true, SelectedSectionCode))
                    {
                        await _userDialogs.AlertAsync("Punch successfully added.", "Save Punch", "Ok");
                        UpdatedWorkPack(IWP.ID);

                        SelectedFunctionCode = null;
                        SelectedCompanyCategoryCodes = null;
                        SelectedSectionCode = null;
                        SelectedPresetCategory = null;
                        SelectedpresetpunchType = null;
                        SelectedPunchLayer = null;
                        PreSetDescription = null;
                        SelectedFWBS = null;
                        SelectedComponent = null;
                        SelectedAction = null;
                    }
                    else
                    {
                        savedPunches = false;
                        await _userDialogs.AlertAsync("Error occured punch not saved.", "Save Punch", "Ok");
                        // return false;
                    }
                }
                else
                {
                    for (int i = 0; i <= (IWPHelper.PathPoints.Count - 1); i += 2)   //CurrentPunchPointer.Count
                    {
                        Point fp = new Point(IWPHelper.PathPoints[i].Points[0].X, IWPHelper.PathPoints[i].Points[0].Y);
                        Point sp = new Point(IWPHelper.PathPoints[i + 1].Points[0].X, IWPHelper.PathPoints[i + 1].Points[0].Y);
                        CurrentPunchPointer = new List<IWPPunchPointer> { new IWPPunchPointer { FirstPoint = fp, SecondPoint = sp } };

                        if (await CreatePunch(CurrentPunchPointer[0], IWP.Title, category, PreSetDescription, PunchRemarks, functionCode,
                            SelectedCompanyCategoryCodes.value, false, true, SelectedSectionCode))
                        {
                            await _userDialogs.AlertAsync("Punch successfully added.", "Save Punch", "Ok");
                            UpdatedWorkPack(IWP.ID);
                            
                        }
                        else
                        {
                            savedPunches = false;
                            await _userDialogs.AlertAsync("Error occured punch not saved.", "Save Punch", "Ok");
                            // return false;
                        }
                    }
                        SelectedFunctionCode = null;
                        SelectedCompanyCategoryCodes = null;
                        SelectedSectionCode = null;
                        SelectedPresetCategory = null;
                        SelectedpresetpunchType = null;
                        // SelectedPunchLayer = null;
                        PreSetDescription = null;
                        SelectedFWBS = null;
                        SelectedComponent = null;
                        SelectedAction = null;
                    }


                if (savedPunches)
                {
                    CurrentPunchPointer.Clear();
                }
                    //   // return true;

                    //else
                    //{
                    //    Task.Run(async () => await _userDialogs.AlertAsync("Unable to add this punch as category " + category + " punches have been marked as completed", "Save Punch", "Ok"));
                    //  //  return true;
                    //}
                }
                else
                    await _userDialogs.AlertAsync("Sorry you do not have the correct user rights to add punch.", "", "Ok");
            }
            else
            {
                Task.Run(async () => await _userDialogs.AlertAsync("Not all information has been completed, please fill out all required fields before saving.", "Save Punch", "Ok"));
              //  return false;
            }

            
        }
        private Boolean CheckNewPunchIsPopulated()
        {
            string[] liststring = new string[] {
                                                 SelectedPresetCategory != null ? SelectedPresetCategory.ToString() : "",
                                                 SelectedCompanyCategoryCodes != null ? SelectedCompanyCategoryCodes.ToString() : "",
                                                 SelectedFunctionCode != null ? SelectedFunctionCode.ToString() : "",
                                                
                                                         
            SelectedSectionCode != null ? SelectedSectionCode.ToString() : "",
            SelectedPresetCategory != null ? SelectedPresetCategory.ToString() : "",
            SelectedpresetpunchType != null ?SelectedpresetpunchType.ToString() : "",
            SelectedPunchLayer != null ?SelectedPunchLayer.ToString() : "",
            PreSetDescription != null ?PreSetDescription.ToString() : "",
            SelectedFWBS != null ?SelectedFWBS.ToString() : "",
            SelectedComponent != null ?SelectedComponent.ToString() : "",
            SelectedAction != null ?SelectedAction.ToString() : ""
        };
            foreach (string str in liststring)
            {
                if (str == null || str == "")
                    return false;
            }

            return true;
        }

        private async Task<bool> CreatePunch(IWPPunchPointer punchPointer, string iwp, string category, string description, string remarks,
                                    string functionCode, string companyCategoryCode, bool signoffall, bool OnDocument, string _SectionCode)
        {
            bool result = true;
            try
            {
                T_IWPPunchControlItem newPunchitem = new T_IWPPunchControlItem();
                var ID = await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>("SELECT * FROM [T_IWPPunchControlItem] WHERE IWPID = " + IWP.ID);
                if (ID.Count > 0)
                    newPunchitem.ID = ID.Select(i => i.ID).Max() + 1;
                else
                    newPunchitem.ID = 1;
                newPunchitem.ProjectID = IWP.ProjectID;
                newPunchitem.IWPID = IWP.ID;
                newPunchitem.CWPID = SelectedCWPTag != null ? Convert.ToInt32(SelectedCWPTag.ID) : 0;
                //newPunchitem.TestPackage = iwp;
                newPunchitem.VMHub_DocumentsID = SelectedDrawing!=null ? Convert.ToInt32(PDFDrawing.VMHub_DocumentsID):0;
                newPunchitem.PunchAdminID = SelectedPunchLayer.ID;
                newPunchitem.SectionCode = SelectedSectionCode !=null ? SelectedSectionCode :"";
                newPunchitem.PunchType = SelectedpresetpunchType != null ? SelectedpresetpunchType : ""; ;
                newPunchitem.Component = SelectedComponent != null ? SelectedComponent : "";
                newPunchitem.ComponentCategory = SelectedComponentCategory != null? SelectedComponentCategory : "";
                newPunchitem.Action = SelectedAction != null ? SelectedAction : "";

                newPunchitem.OtherComponent = TxtOtherComponent != null ? TxtOtherComponent : "";
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
                newPunchitem.Description = PreSetDescription != null ? PreSetDescription : "";
                newPunchitem.Remarks = remarks != null ? remarks : "";
                newPunchitem.FunctionCode = functionCode;
                newPunchitem.CompanyCategoryCode = companyCategoryCode;
                newPunchitem.OnDocument = OnDocument;
                newPunchitem.XPOS1 = Convert.ToInt32(punchPointer.FirstPoint.X);
                newPunchitem.YPOS1 = Convert.ToInt32(punchPointer.FirstPoint.Y);
                newPunchitem.XPOS2 = Convert.ToInt32(punchPointer.SecondPoint.X);
                newPunchitem.YPOS2 = Convert.ToInt32(punchPointer.SecondPoint.Y);

                newPunchitem.CreatedByUserID = userDetail.ID;
                newPunchitem.CreatedBy = userDetail.FullName;
                newPunchitem.CreatedOn = DateTime.Now.Date;

                newPunchitem.Updated = true;
                newPunchitem.UpdatedBy = userDetail.FullName;
                newPunchitem.UpdatedByUserID = userDetail.ID;
                newPunchitem.UpdatedOn = DateTime.Now.Date;
                newPunchitem.IsCreated = true;

              //  var TargsetDAte = TargetDAte;
                newPunchitem.TargetDate = TargetDAte ??  Convert.ToDateTime("01/01/2000 0:00");

                if (signoffall)
                {
                    newPunchitem.Status = "Closed";
                    //newPunchitem.TPCConfirmed = true;
                    //newPunchitem.TPCConfirmedByUserID = CurrentUserDetail.ID;
                    //newPunchitem.TPCConfirmedBy = CurrentUserDetail.FullName;
                    //newPunchitem.TPCConfirmedOn = DateTime.UtcNow;

                    newPunchitem.WorkCompleted = true;
                    newPunchitem.WorkCompletedByUserID = userDetail.ID;
                    newPunchitem.WorkCompletedBy = userDetail.FullName;
                    newPunchitem.WorkCompletedOn = DateTime.Now.Date;

                    newPunchitem.WorkConfirmed = true;
                    newPunchitem.WorkConfirmedByUserID = userDetail.ID;
                    newPunchitem.WorkConfirmedBy = userDetail.FullName;
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
                newPunchitem.WorkRejectedOn = Convert.ToDateTime("01/01/2000 0:00");
                newPunchitem.CancelledOn = Convert.ToDateTime("01/01/2000 0:00");
               
                await _iwpPunchControlItemRepository.InsertOrReplaceAsync(newPunchitem);
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }

        public async Task<int> GetNewPuchID()
        {
            T_IWPPunchControlItem newPunchitem = new T_IWPPunchControlItem();
            var ID = await _iwpPunchControlItemRepository.QueryAsync<T_IWPPunchControlItem>("SELECT * FROM [T_IWPPunchControlItem] WHERE IWPID = " + IWP.ID);
            if (ID.Count > 0)
                return ID.Select(i => i.ID).Max() + 1;
            else
                return  1;
        }

       
        private async Task CascadepresetDropdown(string SelectionChanged)
        {
            switch(SelectionChanged)
            {
                case "FWBS":
                    //punchtype
                    string PunchTypesql = "SELECT DISTINCT [PunchType] FROM [T_IwpPresetPunch] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [FWBS]='"+SelectedFWBS+"' ORDER BY [PunchType] ASC";
                    var PunchType = await _IwpPresetPunchRepository.QueryAsync<T_IwpPresetPunch>(PunchTypesql);
                    PresetPunchTypeList = PunchTypeList = PunchType.Select(s => s.PunchType).ToList();
                    break;

                case "punchtype":
                    //componant category
                    string componantcatsql = "SELECT DISTINCT [ComponentCategory] FROM [T_IwpPresetPunch] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [FWBS]='" +
                        SelectedFWBS + "' AND [PunchType] ='"+SelectedpresetpunchType+"'  ORDER BY [ComponentCategory] ASC";
                    var compcatlist = await _IwpPresetPunchRepository.QueryAsync<T_IwpPresetPunch>(componantcatsql);
                    ComponentCategoryList = compcatlist.Select(s => s.ComponentCategory).ToList();
                    break;

                case "ComponantCategory":
                    //componant 
                    string componantsql = "SELECT DISTINCT [Component] FROM [T_IwpPresetPunch] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [FWBS]='" +
                        SelectedFWBS + "' AND [PunchType] ='" + SelectedpresetpunchType + "' AND [ComponentCategory] = '"+SelectedComponentCategory+"'  ORDER BY [Component] ASC";
                    var complist = await _IwpPresetPunchRepository.QueryAsync<T_IwpPresetPunch>(componantsql);
                    ComponentList = complist.Select(s => s.Component).ToList();
                    break;

                case "componant":
                    //Action
                    string Actionsql = "SELECT DISTINCT [Action] FROM [T_IwpPresetPunch] WHERE [ProjectID] = '" + Settings.ProjectID + "'  AND [FWBS]='" +
                        SelectedFWBS + "' AND [PunchType] ='" + SelectedpresetpunchType + "' AND [ComponentCategory] = '" + SelectedComponentCategory + "' AND [Component] = '"+SelectedComponent+"' ORDER BY [Action] ASC";
                    var _actionlist = await _IwpPresetPunchRepository.QueryAsync<T_IwpPresetPunch>(Actionsql);
                    ActionList = _actionlist.Select(s => s.Action).ToList();
                    break;
            }
           
        }
          public void LoadPunchLayerImageAsync(string pdfname)
        {
            if (SelectedDrawing != null)
            {
                SelectedPDF = SelectedDrawing.Name;
              //  PunchLayerGrid = PunchCategoryGrid = false;
                PDFGrid = true;
            }
            else
            {
                SelectedDrawing = SelectedDrawing = DrawingList.Where(i => i.Name == pdfname).FirstOrDefault();
               // PunchLayerGrid = PunchCategoryGrid = false;
                PDFGrid = true;
            }
        }

        public  void CreateNewPunch_Click()
        {
            if (CurrentPunchPointer.Count > 0)
            {
                CurrentPunchPointer.Clear();
            }
            Task.Run(async () => await _userDialogs.AlertAsync("Please select two points for the punch", "Create New Punch", "Ok"));
            IWPHelper.IsDrawVisible = true;
           
        }

      
        //end


    
        
   
        // Camera 
    
        private async void LoadImageFiles(T_TagMilestoneImages SelectedImageFiles)
        {
            if (SelectedImageFiles == null)
                return;
            var bytes = Convert.FromBase64String(SelectedImageFiles.FileBytes);
            Stream stream = new MemoryStream(bytes);
            CapturedImage = ImageSource.FromStream(() => stream);
            btnSaveRename = "Rename Image";
        }
        private string GetNewImageDisplayName()
        {
            try
            {
                DateTime CurrentUTC = DateTime.UtcNow;
                TimeZoneInfo ProjectTimeZone = TimeZoneInfo.FindSystemTimeZoneById(CurrentUserProject.TimeZone);
                CurrentUTC = TimeZoneInfo.ConvertTimeFromUtc(CurrentUTC, ProjectTimeZone);
                return CurrentUTC.ToString(AppConstant.CameraDateFormat);
            }
            catch (Exception e)
            {
                DateTime CurrentUTC = DateTime.UtcNow;
                TimeZoneInfo ProjectTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                CurrentUTC = TimeZoneInfo.ConvertTimeFromUtc(CurrentUTC, ProjectTimeZone);
                return CurrentUTC.ToString(AppConstant.CameraDateFormat);
            }
        }
        private async void UpdatedWorkPack(int IWP)
        {
            string SQL = "UPDATE [T_IWP] SET [Updated] = 1 WHERE [ID] = '" + IWP + "'";
            var result = await _iwpRepository.QueryAsync(SQL);
        }
        #endregion

        #region Public
        public async void OnClickButton(string param)
        {
            if(param == "OpenCameraPage")
            {
              
                    AddPunchGrid = false;
                    CameraGrid = true;
               

               
               // PDFGrid = true;
            }
            else if (param == "SelectPresetPunch")
            {
                await SetPresetPunchDAta();
            }
            else if (param == "SavePunch")
            {
                await SavePresetPunch();
            }
            else if (param == "CancelPunch")
            {
                SelectedFunctionCode = null;
                SelectedCompanyCategoryCodes = null;
                SelectedSectionCode = null;
                SelectedPresetCategory = null;
                SelectedpresetpunchType = null;
               // SelectedPunchLayer = null;
                PreSetDescription = null;
                SelectedFWBS = null;
                SelectedComponent = null;
                SelectedAction = null;
            }
            
            else if (param == "BackFromPDF") 
            {
                PDFGrid = false;
                // PDFGrid = false;
                AddPunchGrid = true;
            }


            else if (param == "Back")
            {
                await navigationService.GoBackAsync();
            }
            else if (param == "Clear")
            {
                CapturedImage = null;
                SelectedImageFiles = null;
            }
           
            else if (param == "CameraBack")
            {
                BtnText = "Save";
                IsVisibleTagMemberUi = false;
                IsVisibleTagStatus = true;
               // OnClickSelectedCWPTags(SelectedCWPTags);
                  CameraGrid = false;
               // PDFGrid = false;
                AddPunchGrid = true;

            }
            else if (param == "Rename Image")
            {
                if (CapturedImage != null)
                {
                    NewImageName = SelectedImageFiles.DisplayName;
                    ShowSaveRename = true;
                }
                else
                    await Application.Current.MainPage.DisplayAlert(null, "Please select image before renameing", "OK");
            }
            
          
            else if (param == "BackfromCameraGrid")
            {
               
                CameraGrid = PDFGrid  = false;
                AddPunchGrid = true;
            }

            if (param == "NewPunch")
            {

                CreateNewPunch_Click();
            }
        }
        public async Task<byte[]> ResizeImage(byte[] imageAsByte)
        {
            return await _resizeImageService.GetResizeImage(imageAsByte);
        }



        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            
            base.OnNavigatedFrom(parameters);
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
           
            //Navigation.NavigateAsync(new IWPPdfPage(), null);
            base.OnNavigatedTo(parameters);
           

        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
           
        }
        #endregion

    }
}

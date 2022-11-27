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
using Xamarin.Forms;
using System.Windows.Input;
using System;
using JGC.Models.E_Test_Package;
using System.Collections.ObjectModel;
using System.IO;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using JGC.UserControls.CustomControls;
using JGC.UserControls.Touch;
using Rg.Plugins.Popup.Extensions;
using JGC.UserControls.Zoom;
using JGC.UserControls.PopupControls.ColorSelection_CustomColor;
using Rg.Plugins.Popup.Services;
using JGC.UserControls.PopupControls;
using JGC.ViewModels.E_Reporter;
using JGC.Views.E_Reporter;

namespace JGC.ViewModels.E_Test_Package
{
    public class TestRecordViewModel : BaseViewModel
    {

        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IResizeImageService _resizeImageService;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_ETestPackages> _eTestPackagesRepository;
        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private readonly IRepository<T_AdminTestRecordDetails> _adminTestRecordDetails;
        private readonly IRepository<T_AdminTestRecordConfirmation> _adminTestRecordConfirmation;
        private readonly IRepository<T_TestRecordConfirmation> _testRecordConfirmationRepository;
        private readonly IRepository<T_AdminTestRecordAcceptedBy> _adminTestRecordAcceptedByRepository;
        private readonly IRepository<T_TestRecordAcceptedBy> _testRecordAcceptedByRepository;
        private readonly IRepository<T_TestRecordDetails> _testRecordDetails;
        private readonly IRepository<T_ControlLogSignature> _controlLogSignature;
        private readonly IRepository<T_AttachedDocument> _attachedDocumentRepository;
        private readonly IRepository<T_AdminControlLog> _AdminControlLog;
        public readonly IRepository<T_AdminControlLogFolder> _AdminControlLogFolderRepository;

        private readonly IRepository<T_TestRecordImage> _TestRecordImageRepository;




        private T_UserDetails UserDetails;
        private T_UserProject CurrentUserProject;
        string InspectionPath;
        private byte[] imageAsByte;
        private readonly IMedia _media;
        public TouchManipulationBitmap bitmap;
        //MatrixDisplay matrixDisplay = new MatrixDisplay();
        SKCanvas _saveBitmapCanvas;
        public bool _drawFinger, _zoom;
        private string _filePath;
        Dictionary<long, SKPath> _inProgressPaths = new Dictionary<long, SKPath>();
        List<SKPath> _completedPaths = new List<SKPath>();
        public SKBitmap Bitmap = new SKBitmap();
        public List<long> touchIds = new List<long>();
        public INavigation Navigation { get; }


        public TestRecordViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            IRepository<T_AdminTestRecordDetails> _adminTestRecordDetails,
            ICheckValidLogin _checkValidLogin,
            IResizeImageService _resizeImageService,
            IRepository<T_ETestPackages> _eTestPackagesRepository,
            IRepository<T_AdminControlLogFolder> _AdminControlLogFolderRepository,
            IRepository<T_UserDetails> _userDetailsRepository,
            IRepository<T_TestRecordDetails> _testRecordDetails,
            IMedia _media,
            IRepository<T_TestRecordConfirmation> _testRecordConfirmationRepository,
            IRepository<T_AdminTestRecordConfirmation> _adminTestRecordConfirmation,
            IRepository<T_TestRecordAcceptedBy> _testRecordAcceptedByRepository,
            IRepository<T_AdminTestRecordAcceptedBy> _adminTestRecordAcceptedByRepository,
            IRepository<T_ControlLogSignature> _controlLogSignature,
            IRepository<T_AttachedDocument> _attachedDocumentRepository,
            IRepository<T_AdminControlLog> _AdminControlLog,
            IRepository<T_TestRecordImage> _TestRecordImageRepository,
        IRepository<T_UserProject> _userProjectRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._userProjectRepository = _userProjectRepository;
            this._checkValidLogin = _checkValidLogin;
            this._resizeImageService = _resizeImageService;
            this._adminTestRecordAcceptedByRepository = _adminTestRecordAcceptedByRepository;
            this._userDialogs = _userDialogs;
            this._adminTestRecordDetails = _adminTestRecordDetails;
            this._eTestPackagesRepository = _eTestPackagesRepository;
            this._testRecordDetails = _testRecordDetails;
            this._testRecordConfirmationRepository = _testRecordConfirmationRepository;
            this._userDetailsRepository = _userDetailsRepository;
            this._testRecordAcceptedByRepository = _testRecordAcceptedByRepository;
            this._adminTestRecordConfirmation = _adminTestRecordConfirmation;
            this._AdminControlLog = _AdminControlLog;
            this._AdminControlLogFolderRepository = _AdminControlLogFolderRepository;
            this._media = _media;
            this._controlLogSignature = _controlLogSignature;
            this._attachedDocumentRepository = _attachedDocumentRepository;
            this._TestRecordImageRepository = _TestRecordImageRepository;
            _media.Initialize();
            _userDialogs.HideLoading();
            CameraItems = new List<string> { "Camera" };
            LoadCertificationTabsAsync();
            PageHeaderText = "Test Record";
            IsHeaderBtnVisible = MainGrid = Showbuttons = true;
            RenameImage = ColorPickerbtnVisible = false;
            GetTestRecordImages(false);
            Getuserdetails();
        }

        private async void Getuserdetails()
        {
            var UserDetailssd = await _userDetailsRepository.GetAsync(x => x.ID == Settings.UserID);
            UserDetails = UserDetailssd.FirstOrDefault();
        }



        #region Properties
        private bool _signatureGrid;
        public bool SignatureGrid
        {
            get { return _signatureGrid; }
            set { SetProperty(ref _signatureGrid, value); }
        }

        private bool cameraGrid;
        public bool CameraGrid
        {
            get { return cameraGrid; }
            set { SetProperty(ref cameraGrid, value); }
        }

        private bool mainGrid;
        public bool MainGrid
        {
            get { return mainGrid; }
            set { SetProperty(ref mainGrid, value); }
        }

        //fields
        private string txtAFINo;
        public string TxtAFINo
        {
            get { return txtAFINo; }
            set { txtAFINo = value; RaisePropertyChanged(); }
        }

        private string testMedia;
        public string TestMedia
        {
            get { return testMedia; }
            set { testMedia = value; RaisePropertyChanged(); }
        }
        private string testPackage;
        public string TestPackage
        {
            get { return testPackage; }
            set { SetProperty(ref testPackage, value); }
        }
        private string testPressure;
        public string TestPressure
        {
            get { return testPressure; }
            set { testPressure = value; RaisePropertyChanged(); }
        }

        private bool btnSaveDetails;
        public bool BtnSaveDetails
        {
            get { return btnSaveDetails; }
            set { btnSaveDetails = value; RaisePropertyChanged(); }
        }

        private string testRecordRemarks;
        public string TestRecordRemarks
        {
            get { return testRecordRemarks; }
            set { testRecordRemarks = value; RaisePropertyChanged(); }
        }

        private string entry1;
        public string Entry1
        {
            get { return entry1; }
            set { entry1 = value; RaisePropertyChanged(); }
        }
        private string entry2;
        public string Entry2
        {
            get { return entry2; }
            set { entry2 = value; RaisePropertyChanged(); }
        }
        private string entry3;
        public string Entry3
        {
            get { return entry3; }
            set { entry3 = value; RaisePropertyChanged(); }
        }
        private string entry4;
        public string Entry4
        {
            get { return entry4; }
            set { entry4 = value; RaisePropertyChanged(); }
        }

        private string entryDiscription1;
        public string EntryDiscription1
        {
            get { return entryDiscription1; }
            set { entryDiscription1 = value; RaisePropertyChanged(); }
        }

        private string entryDiscription2;
        public string EntryDiscription2
        {
            get { return entryDiscription2; }
            set { entryDiscription2 = value; RaisePropertyChanged(); }
        }
        private string entryDiscription3;
        public string EntryDiscription3
        {
            get { return entryDiscription3; }
            set { entryDiscription3 = value; RaisePropertyChanged(); }
        }
        private string entryDiscription4;
        public string EntryDiscription4
        {
            get { return entryDiscription4; }
            set { entryDiscription4 = value; RaisePropertyChanged(); }
        }

        private List<T_AdminTestRecordDetails> testRecordDetails;
        public List<T_AdminTestRecordDetails> TestRecordDetails
        {
            get { return testRecordDetails; }
            set { testRecordDetails = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<RecordConfirmation> recordConfirmationSource;
        public ObservableCollection<RecordConfirmation> RecordConfirmationSource
        {
            get { return recordConfirmationSource; }
            set { recordConfirmationSource = value; RaisePropertyChanged(); }
        }

        private RecordConfirmation selectedConfirmationSource;
        public RecordConfirmation SelectedConfirmationSource
        {
            get { return selectedConfirmationSource; }
            set { selectedConfirmationSource = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<RecordAcceptedBy> recordAcceptedBySource;
        public ObservableCollection<RecordAcceptedBy> RecordAcceptedBySource
        {
            get { return recordAcceptedBySource; }
            set { recordAcceptedBySource = value; RaisePropertyChanged(); }
        }

        private RecordAcceptedBy selectedrecordAcceptedBySource;
        public RecordAcceptedBy SelectedrecordAcceptedBySource
        {
            get { return selectedrecordAcceptedBySource; }
            set { selectedrecordAcceptedBySource = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<TestPackageImage> _ImageFiles;
        public ObservableCollection<TestPackageImage> ImageFiles
        {
            get { return _ImageFiles; }
            set { _ImageFiles = value; RaisePropertyChanged(); }
        }

        private List<ETPEntryDescription> _etpDescription;
        public List<ETPEntryDescription> EtpDescription
        {
            get { return _etpDescription; }
            set { _etpDescription = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<ETPEntryDescription> _etpEntryDescription;
        public ObservableCollection<ETPEntryDescription> EtpEntryDescription
        {
            get { return _etpEntryDescription; }
            set { _etpEntryDescription = value; RaisePropertyChanged(); }
        }
        private bool _showbuttons;
        public bool Showbuttons
        {
            get { return _showbuttons; }
            set { SetProperty(ref _showbuttons, value); }
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

        private bool _isVisibleSaveDelete;
        public bool IsVisibleSaveDelete
        {
            get { return _isVisibleSaveDelete; }
            set { SetProperty(ref _isVisibleSaveDelete, value); }
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


        private bool _colorPickerbtnVisible;
        public bool ColorPickerbtnVisible
        {
            get { return _colorPickerbtnVisible; }
            set
            {
                SetProperty(ref _colorPickerbtnVisible, value);
            }
        }

        private List<string> _cameraItems;
        public List<string> CameraItems
        {
            get { return _cameraItems; }
            set { _cameraItems = value; RaisePropertyChanged(); }
        }

        private TestPackageImage _selectedImageFiles;
        public TestPackageImage SelectedImageFiles
        {
            get { return _selectedImageFiles; }
            set { _selectedImageFiles = value; RaisePropertyChanged(); }
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

        private string _cameraIcon;
        public string CameraIcon
        {
            get { return _cameraIcon; }
            set
            {
                SetProperty(ref _cameraIcon, value);
            }
        }

        private bool isSaveVisible;
        public bool IsSaveVisible
        {
            get { return isSaveVisible; }
            set { SetProperty(ref isSaveVisible, value); }
        }

        #endregion


        #region Button Click Events
        public ICommand BtnCommand
        {
            get
            {
                return new Command<string>(OnClickButton);
            }
        }

        private async void OnClickButton(string param)
        {
            if (param == "SignatureGrid")
            {
                SignatureGrid = true;
                MainGrid = false;
            }
            else if (param == "BackfromSignatureGrid")
            {
                SignatureGrid = false;
                MainGrid = true;
            }
            else if (param == "CameraGrid")
            {
                GetTestRecordImages(false);
                MainGrid = false;
                CameraGrid = true;
            }
            else if (param == "BackfromCameraGrid")
            {
                CameraGrid = false;
                MainGrid = true;
            }
            else if (param == "SaveAfiNumber")
            {
                await btnSaveAFINo_Click();
            }
            else if (param == "RecordRemark")
            {
                await btnSaveTestRecordRemarks_Click();
            }
            else if (param == "SaveDetails")
            {
                await btnSaveDetails_ClickAsync();
            }
            else if (param == "CaptureImageSave")
            {
                if (btnSaveDelete == "Save")
                {
                    CameraSaveBT_Click("");
                    RenameImage = true;
                    btnSaveDelete = "Delete";
                    ColorPickerbtnVisible = false;
                    GetTestRecordImages(true);
                    //SelectedImageFiles = ImageFiles.LastOrDefault();
                }
                else if (btnSaveDelete == "Delete")
                {
                    ImageDelete();
                }
            }
            else if (param == "ColorPicker")
            {
                IsSaveVisible = true;
                _drawFinger = true;
                await Navigation.PushPopupAsync(new ColorSelectionPopup());
            }
            else if (param == "RenameImage")
            {
                if (SelectedImageFiles != null)
                {
                    Showbuttons = false;
                }
                else
                    _userDialogs.AlertAsync("", "Please select image for rename", "OK");
            }
            else if (param == "CancleRename")
            {
                Showbuttons = true;
            }
            else if (param == "SaveRename")
            {
                BTRenameImage_Click();
                GetTestRecordImages(false);
            }
        }
        #endregion

        #region TestRecord Data Load /Save

        public async void ShowDescriptionPopup(string Description)
        {
            if (Description == null)
                return;
            if (!string.IsNullOrWhiteSpace(Description))
                await PopupNavigation.PushAsync(new ShowWrapTextPopup("Description", Description), true);
        }
        private async Task LoadCertificationTabsAsync()
        {
            CurrentUserProject = new T_UserProject();
            var QueryResult = await _userProjectRepository.GetAsync(x => x.Project_ID == Settings.ProjectID);
            CurrentUserProject = QueryResult.FirstOrDefault();
            TestRecordDetails = new List<T_AdminTestRecordDetails>();
            
            //Load Top Details
            Boolean visualTest = false;
            string SQL = "SELECT * FROM[T_ETestPackages] WHERE[ProjectID] ='" + Settings.ProjectID + "' AND[ID] = '" + CurrentPageHelper.ETestPackage.ID + "'";
            var testPackage = await _eTestPackagesRepository.QueryAsync<T_ETestPackages>(SQL);

            if (testPackage.Any())
            {
                var _testPackage = testPackage.FirstOrDefault();
                TestPackage = _testPackage.TestPackage;
                TxtAFINo = _testPackage.AFINo;
                TestMedia = _testPackage.TestMedia;
                TestPressure = _testPackage.TestPressure;
                visualTest = (testMedia.ToUpper().Trim() == "V");
                TestRecordRemarks = _testPackage.TestRecordRemarks;
            }
            BtnSaveDetails = !visualTest;          
            string SQL2 = "SELECT * FROM[T_AdminTestRecordDetails] WHERE[ProjectID] ='" + Settings.ProjectID + "' AND [ProjectName] = '" + CurrentUserProject.ProjectName + "' ORDER BY[OrderNo] ASC";
            var result = await _adminTestRecordDetails.QueryAsync<T_AdminTestRecordDetails>(SQL2);

            if (result.Any())
            {
                EtpDescription = new List<ETPEntryDescription>();
                foreach (T_AdminTestRecordDetails RecordDetails in result)
                {
                    ETPEntryDescription description = new ETPEntryDescription();
                    TestRecordDetails.Add(RecordDetails);
                    description.EntryDescription = RecordDetails.Description;
                   
                    if (visualTest)                                          
                        description.EntryDescription = "N/A";

                    else
                    {
                        int id = RecordDetails.ID;

                        string SQL3 = "SELECT * FROM [T_TestRecordDetails] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ETestPackageID] = '" + CurrentPageHelper.ETestPackage.ID + "' AND [DetailsAdminID] = '" + id + "'";
                        var inputValue = await _testRecordDetails.QueryAsync<T_TestRecordDetails>(SQL3);

                        if (inputValue.Any())                        
                            description.Entry = inputValue.FirstOrDefault().InputValue;

                        else                        
                            description.Entry = "";                        
                    }

                    EtpDescription.Add(description);                  
                }
                EtpEntryDescription = new ObservableCollection<ETPEntryDescription>(EtpDescription);               
            }
           
            var tsk1 = await GenerateContentTableAsync(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, "T_AdminTestRecordConfirmation", "T_TestRecordConfirmation", CurrentUserProject);
            var tsk2 = await GenerateAcceptedByTableAsync(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, "T_AdminTestRecordAcceptedBy", "T_TestRecordAcceptedBy", CurrentUserProject);

            RecordConfirmationSource = new ObservableCollection<RecordConfirmation>(tsk1);
            RecordAcceptedBySource = new ObservableCollection<RecordAcceptedBy>(tsk2);
        }

        private async Task btnSaveAFINo_Click()
        {
            if (!string.IsNullOrWhiteSpace(TxtAFINo))
            {
                string SQL = "UPDATE[T_ETestPackages] SET[AFINO] = '" + TxtAFINo + "', [AFINoUpdated] = 1 WHERE[ProjectID] = '" + Settings.ProjectID + "' AND[ID] = '" + CurrentPageHelper.ETestPackage.ID + "'";
                await _eTestPackagesRepository.QueryAsync<T_ETestPackages>(SQL);
                await _userDialogs.AlertAsync("Successfully saved..!", null, "Ok");
                UpdateETestPackageStatus(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID);
            }
            else
            {
                await _userDialogs.AlertAsync("Please enter RFI No.", null, "Ok");
            }
        }

        private async Task btnSaveDetails_ClickAsync()
        {

            if (TestRecordDetails.Any())
            {
                //Remove E isting
                string SQL = "DELETE FROM [T_TestRecordDetails] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ETestPackageID] = '" + CurrentPageHelper.ETestPackage.ID + "'";
                var sf = await _testRecordDetails.QueryAsync(SQL);

                List<CertificationDetails> testRecordDetailsList = new List<CertificationDetails>();
              

                for(int index = 0; index< TestRecordDetails.Count;index++)
                {
                    string input;
                    input = EtpEntryDescription[index].Entry;
                    if (input != string.Empty)
                    {
                        CertificationDetails details = new CertificationDetails()
                        {
                            DetailsAdminID = TestRecordDetails[index].ID,
                            InputValue = input,
                        };

                        testRecordDetailsList.Add(details);
                    }
                }
              
                if (testRecordDetailsList != null && testRecordDetailsList.Count > 0)
                {
                    var result = await InsertCertificationDetailsList(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, testRecordDetailsList, true);
                    if (result)
                    {
                        UpdateETestPackageStatus(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID);
                        await _userDialogs.AlertAsync("Successfully saved..!", null, "Ok");
                    }
                    else
                    {
                        await _userDialogs.AlertAsync("Faild", null, "Ok");
                    }
                }
            }
        }
        private async Task btnSaveTestRecordRemarks_Click()
        {
            if (!string.IsNullOrWhiteSpace(TestRecordRemarks))
            {
                string SQL = "UPDATE[T_ETestPackages] SET [TestRecordRemarks]= '" + TestRecordRemarks + "', [TestRecordRemarksUpdated] = 1 WHERE[ProjectID] = '" + Settings.ProjectID + "' AND[ID] = '" + CurrentPageHelper.ETestPackage.ID + "'";
                await _eTestPackagesRepository.QueryAsync<T_ETestPackages>(SQL);
                await _userDialogs.AlertAsync("Successfully saved..!", null, "Ok");
                UpdateETestPackageStatus(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID);
            }
            else
            {
                await _userDialogs.AlertAsync("Please enter Remark", null, "Ok");
            }

        }

        private async void UpdateETestPackageStatus(int projectId, int TestPackageID)
        {
            string SQL = "UPDATE [T_ETestPackages] SET [Updated] = 1 WHERE [ProjectID] = '" + projectId + "'"
            + " AND [ID] = '" + TestPackageID + "'";
            var result = await _eTestPackagesRepository.QueryAsync(SQL);
        }
        #endregion

        #region Signature Logic
        private async Task<Boolean> InsertCertificationDetailsList(int projectID, int etestpackageID, List<CertificationDetails> list, Boolean updated)
        {
            Boolean results = true;
            try
            {
                foreach (CertificationDetails value in list)
                {

                    string SQL = "INSERT INTO [T_TestRecordDetails] ([ProjectID],[ETestPackageID],[DetailsAdminID],[InputValue],[Updated]) VALUES " +
                                "('" + projectID + "','" + etestpackageID + "','" + value.DetailsAdminID + "','" + value.InputValue + "', 1 )";
                    await _testRecordDetails.QueryAsync(SQL);
                }

            }
            catch (Exception err)
            {
                string Test = err.Message;
                results = false;
            }
            return results;
        }
        private async Task<List<RecordConfirmation>> GenerateContentTableAsync(int projectID, int etestPackageID, string adminDatabaseTable, string databaseTable, T_UserProject userProject)
        {
            List<RecordConfirmation> ListRecordConfirmation = new List<RecordConfirmation>();
            try
            {
                string SQL = "SELECT ADMIN.[ID],ADMIN.[Description],ADMIN.[OrderNo],ADMIN.[PIC],SIG.[Live],SIG.[Signed],SIG.[SignedBy],SIG.[SignedOn] FROM [" + adminDatabaseTable + "] AS ADMIN LEFT JOIN " +
                    "(SELECT * FROM [" + databaseTable + "] WHERE [ETestPackageID] = '" + etestPackageID + "') AS SIG ON ADMIN.[ID] = SIG.[ADMINID] " +
                    "WHERE ADMIN.[ProjectID] = '" + projectID + "' AND ADMIN.[ProjectName] = '" + userProject.ProjectName + "' ORDER BY ADMIN.[OrderNo] ASC";
                var result = await _adminTestRecordConfirmation.QueryAsync<RecordConfirmation>(SQL);
               

                if (result.Any())
                {
                    foreach (RecordConfirmation Rc in result)
                    {
                        
                        //Image signedImage = signed ? jgcetestpackage.Properties.Resources.greendot : jgcetestpackage.Properties.Resources.greydot;
                        //string signedOn = signed ? AdjustDateTime(SQLReader.GetRealDateTime(Reader, "SignedOn"), userProject) : "";
                        Rc.SignedImage = Rc.Signed ? "Greenradio.png" : "Grayradio.png";
                        Rc.btnNASign = "N/A Sign off"; 
                        ListRecordConfirmation.Add(Rc);
                    }
                }
            }
            catch (Exception Ex)
            {

            }

            return ListRecordConfirmation;
        }
        public async Task<List<RecordAcceptedBy>> GenerateAcceptedByTableAsync(int projectID, int etestPackageID, string adminDatabaseTable, string databaseTable, T_UserProject userProject)
        {
            List<RecordAcceptedBy> ListRecordAcceptedBy = new List<RecordAcceptedBy>();

            string SQL = "SELECT ADMIN.[ID],ADMIN.[Description],ADMIN.[OrderNo],SIG.[Live],SIG.[Signed],SIG.[SignedBy],SIG.[SignedOn] FROM [" + adminDatabaseTable + "] AS ADMIN LEFT JOIN " +
                "(SELECT * FROM [" + databaseTable + "] WHERE [ETestPackageID] = '" + etestPackageID + "') AS SIG ON ADMIN.[ID] = SIG.[ADMINID] " +
                "WHERE ADMIN.[ProjectID] = '" + projectID + "' AND ADMIN.[ProjectName] = '" + userProject.ProjectName + "' ORDER BY ADMIN.[OrderNo] ASC";

            var result = await _testRecordAcceptedByRepository.QueryAsync<RecordAcceptedBy>(SQL);

            if (result.Any())
            {
                foreach (RecordAcceptedBy Ra in result)
                {

                    // Image signedImage = signed ? jgcetestpackage.Properties.Resources.greendot : jgcetestpackage.Properties.Resources.greydot;

                    //Ra.signedOn = signed ? AdjustDateTime(SQLReader.GetRealDateTime(Reader, "SignedOn"), userProject) : "";

                    ListRecordAcceptedBy.Add(Ra);
                }
            }

            return ListRecordAcceptedBy;
        }
        public async void gvTestRecordConfirmation_CellContentClick(string param, RecordConfirmation SelectedRecord)
        {
            SelectedConfirmationSource = SelectedRecord;
            //// await  _userDialogs.AlertAsync("Sorry, You Do Not Have The Correct User Right To Sign Off This Signature", "","OK");
            bool btnNA = false;
            if (param == "btnNA")
            {
                btnNA = true;
            }

            if (SelectedConfirmationSource != null || btnNA == true) //Status Image
            {
                // DataGridView gv = gvTestRecordConfirmation;

                // int rowIndex = e.RowIndex;
                int adminID = SelectedConfirmationSource.ID;

                Boolean signed = SelectedConfirmationSource.Signed;
                Boolean live = SelectedConfirmationSource.Live;

                if (await CanSignOffCertificiationSignature("T_AdminTestRecordConfirmation", "T_TestRecordConfirmation", adminID, signed, live))
                {
                    T_TestRecordConfirmation signature = new T_TestRecordConfirmation()
                    {
                        AdminID = adminID,
                        Signed = !signed,
                        SignedByUserID = Settings.UserID,
                        SignedBy = UserDetails.FullName,
                        SignedOn = DateTime.UtcNow,
                    };
                    if (btnNA == true)
                    {
                        signature.SignedByUserID = 0;
                        signature.SignedBy = "N/A";
                    }

                    if (_ = await SaveNew(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, "T_TestRecordConfirmation", signature))
                    {
                        UpdateETestPackageStatus(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID);

                        RecordConfirmationSource = new ObservableCollection<RecordConfirmation>();
                        //   gvTestRecordConfirmation.ClearSelection();
                        var tsk1 = await GenerateContentTableAsync(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, "T_AdminTestRecordConfirmation", "T_TestRecordConfirmation", CurrentUserProject);
                        RecordConfirmationSource = new ObservableCollection<RecordConfirmation>(tsk1);
                    }
                    else
                        await _userDialogs.AlertAsync("Error ocurred saving signature", null, "Ok");

                }
            }
        }
        public async void gvTestRecordAcceptedBy_CellContentClick(string param, RecordAcceptedBy SelectedRecord)
        {
            SelectedrecordAcceptedBySource = SelectedRecord;

            if (SelectedrecordAcceptedBySource != null) //Status Image
            {
                // DataGridView gv = gvTestRecordAcceptedBy;

                // int rowIndex = e.RowIndex;
                int adminID = SelectedrecordAcceptedBySource.ID;
                //int signatureNo =
                Boolean signed = SelectedrecordAcceptedBySource.Signed;
                Boolean live = SelectedrecordAcceptedBySource.Live;

                int orderNo = SelectedrecordAcceptedBySource.OrderNo;

                if (await CanSignOffAcceptedBySignature("T_AdminTestRecordAcceptedBy", "T_TestRecordAcceptedBy", RecordConfirmationSource, adminID, signed, live, orderNo))
                {

                    T_TestRecordConfirmation signature = new T_TestRecordConfirmation()
                    {
                        AdminID = adminID,
                        Signed = !signed,
                        SignedByUserID = UserDetails.ID,
                        SignedBy = UserDetails.FullName,
                        SignedOn = DateTime.UtcNow,
                    };

                    if (await SaveNew(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, "T_TestRecordAcceptedBy", signature))
                    {
                        if (!signed)
                        {
                            Boolean controlLogLinked = false;
                            int controlLogAdminID = 0;

                            string SQL = "SELECT * FROM [T_AdminTestRecordAcceptedBy] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ID] = '" + CurrentPageHelper.ETestPackage.ID + "'";
                            var result = await _adminTestRecordAcceptedByRepository.QueryAsync<T_AdminTestRecordAcceptedBy>(SQL);

                            if (result.Any())
                            {
                                controlLogLinked = result.FirstOrDefault().ControlLogLinked;
                                controlLogAdminID = result.FirstOrDefault().ControlLogAdminID;
                            }

                            if (controlLogLinked)
                            {
                                //get control log row index.
                                if (await CanSignControlLogSignature(UserDetails, Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, CurrentPageHelper.ETestPackage.SubContractor, controlLogAdminID, false))
                                {
                                    T_ControlLogSignature controlLogSignature = new T_ControlLogSignature()
                                    {
                                        ControlLogAdminID = controlLogAdminID,
                                        Signed = !signed,
                                        SignedByUserID = UserDetails.ID,
                                        SignedBy = UserDetails.FullName,
                                        SignedOn = DateTime.UtcNow,
                                    };

                                    if (await SaveNewAsync(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, controlLogSignature))
                                    {
                                        T_AdminControlLog adminControlLog1 = new T_AdminControlLog();
                                        var getData = await _AdminControlLog.GetAsync(x => x.ProjectID == Settings.ProjectID && x.ID == controlLogAdminID);  //adminControlLog.Get(Convert.ToInt32(CurrentProject.Project_ID), controlLogAdminID); //Get all control log admin details for checks

                                        //Adjust minor milestones
                                        List<T_AdminControlLog> list = await GetMinorMileStones(Settings.ProjectID, getData.FirstOrDefault().SignatureNo); //get list of minor milestones associated

                                        if (list != null && list.Count > 0) //if returned some minor milestones 
                                        {
                                            foreach (T_AdminControlLog adminControlLog in list) //go through list and adjust the signature's control log id and save.
                                            {
                                                controlLogSignature.ControlLogAdminID = adminControlLog.ID;
                                                await SaveNewAsync(Convert.ToInt32(Settings.ProjectID), CurrentPageHelper.ETestPackage.ID, controlLogSignature);
                                            }
                                        }


                                        AutoSignControlLogSignatures(UserDetails, Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, CurrentPageHelper.ETestPackage.SubContractor);
                                        UpdateETestPackageStatus(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID);



                                        //  await LoadControlLogTabAsync();
                                    }
                                    else
                                        await Application.Current.MainPage.DisplayAlert("Control Log Signature", "Error ocurred saving signature", "OK");

                                }
                            }
                        }

                        UpdateETestPackageStatus(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID);


                        var tsk2 = await GenerateAcceptedByTableAsync(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, "T_AdminTestRecordAcceptedBy", "T_TestRecordAcceptedBy", CurrentUserProject);
                        RecordAcceptedBySource = new ObservableCollection<RecordAcceptedBy>(tsk2);
                    }
                    else
                        await _userDialogs.AlertAsync("Error ocurred saving signature", null, "OK");

                }

            }
        }


        public async void AutoSignControlLogSignatures(T_UserDetails CurrentUser, int projectID, int etestPackageID, string subContractor)
        {
            //Get next control log id
            int nextControlLogAdminID = await GetNextControlLogIDAsync(projectID, etestPackageID);

            //check if you can sign the next control log as NA

            string sql = "SELECT * FROM  " +
               "[T_ControlLogSignature] CL " +
               "INNER JOIN [T_AdminControlLogNaAutoSignatures] NA ON CL.[ControlLogAdminID] = NA.[ControlLogAdminID] " +
               "WHERE CL.[ProjectID] = '" + projectID + "' AND CL.[ETestPackageID] = '" + etestPackageID + "' " +
              "AND CL.[Signed] = 1 AND CL.[SignedByUserID] = 0 AND NA.[ProjectID] = '" + projectID + "' AND NA.[AutoSignOffControlLogAdminID] = '" + nextControlLogAdminID + "'";


            var result = await _controlLogSignature.QueryAsync<T_AdminControlLogNaAutoSignatures>(sql);
            var canSign = result;


            if (canSign.Any())
            {
                if (await CanSignControlLogSignature(CurrentUser, projectID, etestPackageID, subContractor, nextControlLogAdminID, false, true))
                {
                    T_ControlLogSignature controlLogSignature = new T_ControlLogSignature()
                    {
                        ControlLogAdminID = nextControlLogAdminID,
                        Signed = true,
                        SignedByUserID = 0,
                        SignedBy = "NA",
                        SignedOn = DateTime.UtcNow,
                    };

                    if (await SaveNewAsync(projectID, etestPackageID, controlLogSignature))
                    {
                        var sqldqweryData = await _AdminControlLog.GetAsync(x => x.ProjectID == projectID && x.ID == nextControlLogAdminID);

                        int signatureNo = sqldqweryData.FirstOrDefault().SignatureNo;


                        //Adjust minor milestones
                        List<T_AdminControlLog> list = await GetMinorMileStones(projectID, signatureNo); //get list of minor milestones associated

                        if (list != null && list.Count > 0) //if returned some minor milestones 
                        {
                            foreach (T_AdminControlLog adminControlLog in list) //go through list and adjust the signature's control log id and save.
                            {
                                controlLogSignature.ControlLogAdminID = adminControlLog.ID;
                                await SaveNewAsync(projectID, etestPackageID, controlLogSignature);
                            }
                        }

                        //Loop around
                        AutoSignControlLogSignatures(CurrentUser, projectID, etestPackageID, subContractor);
                    }
                }
            }
        }


        public async Task<int> GetNextControlLogIDAsync(int projectID, int etestPackageID)
        {
            var lastSigniture = await _AdminControlLog.QueryAsync<T_AdminControlLog>("SELECT * " +
                "FROM (T_AdminControlLog ADMIN INNER JOIN " +
                "T_ControlLogSignature CL ON ADMIN.ProjectID = CL.ProjectID AND ADMIN.ID = CL.ControlLogAdminID) " +
                "WHERE  (ADMIN.ProjectID = '" + projectID + "') AND (CL.ETestPackageID = '" + etestPackageID + "') AND (CL.Signed = '" + true + "') " +
                "ORDER BY ADMIN.[SignatureNo] DESC ");

            int lastSignatureNo = 0;

            if (!lastSigniture.Any()) return lastSignatureNo;
            else return lastSigniture.FirstOrDefault().SignatureNo;


        }
        public async Task<List<T_AdminControlLog>> GetMinorMileStones(int projectID, int signatureNo)
        {
            // List<T_AdminControlLog> list = new List<T_AdminControlLog>();
            var SQL = await _AdminControlLog.GetAsync(x => x.ProjectID == projectID && x.AssociatedSignatureNo == signatureNo);

            return SQL.ToList();
        }
        public async Task<bool> SaveNewAsync(int projectID, int etestPackageID, T_ControlLogSignature controlLogModel)
        {

            //CheckExists
            var objcontrolLogSignature = await _controlLogSignature.GetAsync(x => x.ProjectID == projectID && x.ETestPackageID == etestPackageID && x.ControlLogAdminID == controlLogModel.ControlLogAdminID);

            var CheckExists = objcontrolLogSignature.Any();
            if (CheckExists)

            {
                T_ControlLogSignature ControlLogSignature = new T_ControlLogSignature();
                ControlLogSignature = objcontrolLogSignature.FirstOrDefault();
                //Found use sql update.

                ControlLogSignature.Reject = false;
                ControlLogSignature.Signed = controlLogModel.Signed;
                ControlLogSignature.SignedByUserID = controlLogModel.SignedByUserID;
                ControlLogSignature.SignedBy = controlLogModel.SignedBy;
                ControlLogSignature.SignedOn = controlLogModel.SignedOn;
                ControlLogSignature.Live = false;
                ControlLogSignature.Updated = true;
                ControlLogSignature.ProjectID = projectID;
                ControlLogSignature.ETestPackageID = etestPackageID;
                ControlLogSignature.ControlLogAdminID = controlLogModel.ControlLogAdminID;

                string SQL = " UPDATE [T_ControlLogSignature] SET[Reject] = " + false + ", [Signed] = " + controlLogModel.Signed + ",[SignedByUserID] = '"
                    + controlLogModel.SignedByUserID + "' , [SignedBy] = '" + controlLogModel.SignedBy + "',[SignedOn] = '" + controlLogModel.SignedOn.ToString("dd-MMM-yyyy")
                    + "', [Live] = " + false + ", [Updated] = " + true + " WHERE[ProjectID] = " + projectID + " AND[ETestPackageID] = " + etestPackageID
                    + " AND[ControlLogAdminID] = " + controlLogModel.ControlLogAdminID + "";
                try
                {
                    var dBreturn = await _controlLogSignature.QueryAsync(SQL);

                    return true;

                }
                catch (Exception Ex)
                { return false; }



            }
            else
            {
                //Insert fresh details.
                T_ControlLogSignature ControlLogSignature = new T_ControlLogSignature();
                //  ControlLogSignature = objcontrolLogSignature.FirstOrDefault();
                ControlLogSignature.Signed = controlLogModel.Signed;
                ControlLogSignature.SignedByUserID = controlLogModel.SignedByUserID;
                ControlLogSignature.SignedBy = controlLogModel.SignedBy;
                ControlLogSignature.SignedOn = DateTime.UtcNow;
                ControlLogSignature.Live = false;
                ControlLogSignature.Updated = true;
                ControlLogSignature.ProjectID = projectID;
                ControlLogSignature.ETestPackageID = etestPackageID;
                ControlLogSignature.ControlLogAdminID = controlLogModel.ControlLogAdminID;

                var dBreturn = await _controlLogSignature.InsertAsync(ControlLogSignature);
                if (dBreturn == 1)
                    return true;
                else
                    return false;
            }

        }

        public async Task<Boolean> CanSignControlLogSignature(T_UserDetails CurrentUser, int projectID, int etestPackageID, string subContractor, int controllogAdminID, bool showPopups = true, bool naAutoSignOff = true)
        {
            Boolean live = false, signed = false;

            var getdBdata = await _controlLogSignature.GetAsync(x => x.ProjectID == projectID && x.ETestPackageID == etestPackageID && x.ControlLogAdminID == controllogAdminID);
            if (getdBdata.Any())
            {
                live = getdBdata.FirstOrDefault().Live;
                signed = getdBdata.FirstOrDefault().Signed;
            }

            var controlLogAdminDetails = await _AdminControlLog.GetAsync(x => x.ProjectID == projectID && x.ID == controllogAdminID);
            var adminControlLog = controlLogAdminDetails.FirstOrDefault();  //adminControlLog.Get(projectID, controllogAdminID); //Get all control log admin details for checks


            string popupCaptionText = "Control Log Signature";

            if (adminControlLog.Milestone) //If not a major milestone then it cannot be signed off manually.
            {
                if (!live)
                {
                    if (signed) //Remove signature
                    {
                        var nextSignatureSigned = await CheckNextControlLogSigned(projectID, etestPackageID, adminControlLog.SignatureNo);

                        if (nextSignatureSigned)
                        {
                            if (showPopups)
                                await Application.Current.MainPage.DisplayAlert(popupCaptionText, "Prior signatures are signed, these must be removed before removing this signature", "ok");

                            return false;
                        }

                    }
                    else //Add Signature
                    {
                        if (adminControlLog.SignatureNo > 1)
                        {
                            Boolean previousSignatureSigned = await CheckPreviousControlLogSigned(projectID, etestPackageID, adminControlLog.SignatureNo - 1);


                            if (!previousSignatureSigned) //if previous signature is not signed then cannot sign.
                            {
                                if (showPopups)
                                    await Application.Current.MainPage.DisplayAlert(popupCaptionText, "Previous signatures are not signed", "ok");
                                return false;
                            }
                        }
                    } //End of first test

                    //Second test

                    // Allow Test pack coordinator the ability to remove any signatures.
                    if (signed && CheckIsTestPackCoordinator(CurrentUser, subContractor))
                        return true;
                    else
                    {
                        //Check user details against control log signature details.
                        if (!naAutoSignOff)
                        {
                            if (!ControlLogRightsCheck(adminControlLog, CurrentUser, subContractor))
                            {
                                if (showPopups)
                                {
                                    var ansr = await Application.Current.MainPage.DisplayActionSheet("Sorry, you do not have the rights to adjust this signature", "Sign by Other", "OK");
                                    if (ansr == "Sign by Other")
                                    {
                                        var vm = await ReadLoginPopup();
                                        if (vm.Password != null && vm.UserName != null)
                                        {
                                            var UserDetailsList = await _userDetailsRepository.GetAsync(x => x.UserName == vm.UserName && x.Password == vm.Password);
                                            if (UserDetailsList.Any())
                                            {
                                                CurrentUser = UserDetailsList.FirstOrDefault();
                                                return true;
                                            }
                                            else
                                            {
                                                await Application.Current.MainPage.DisplayAlert("Login", AppConstant.LOGIN_FAILURE, "OK");
                                                return false;
                                            }


                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }

                                }
                                //await Application.Current.MainPage.DisplayAlert(popupCaptionText, "Sorry, you do not have the rights to adjust this signature", "ok");
                                //return false;
                            }
                        }

                    }

                    //Documents check.
                    if (!await ControlLogDocumentsCheck(projectID, etestPackageID, adminControlLog, signed))
                    {

                        var folderNames = await _AdminControlLogFolderRepository.QueryAsync<T_AdminFolders>("SELECT DISTINCT AF.[FolderName] FROM [AdminControlLogFolderHH] CLF INNER JOIN [AdminFolderHH] AF" +
                                                                          " ON CLF.[ProjectID] = AF.[ProjectID] AND CLF.[FolderAdminID] = AF.[ID] " +
                                                                          "WHERE CLF.[ProjectID] = '" + projectID + "' AND CLF.[ControlLogAdminID] = '" + adminControlLog.ID + "'");

                        if (showPopups)

                            await Application.Current.MainPage.DisplayAlert(popupCaptionText, "This signature requires " + string.Join(",", folderNames.FirstOrDefault().FolderName) + " documents to be uploaded prior, this can only be uploaded on VMLive.", "ok");
                        return false;

                    }


                    ////Punch checks
                    //if (!ModsTools.ControlLogPunchLayerCheck(projectID, etestPackageID, adminControlLog, signed))
                    //{
                    //    if (showPopups)
                    //        MessageBox.Show("Unable to sign off as there are outstanding punches.", popupCaptionText);
                    //    return false;
                    //}


                    //Never returned false so passed all tests       
                    return true;

                }
                else
                {
                    if (showPopups)
                        await Application.Current.MainPage.DisplayAlert(popupCaptionText, "Selected milestone has already been uploaded to VMLive and cannot be removed on the handheld.", "ok");

                }
            }
            else
            {
                if (showPopups)
                    await Application.Current.MainPage.DisplayAlert(popupCaptionText, "Minor milestones cannot be signed off manually, these are done automatically by various events setup by the E-Test Package admin.", "ok");
            }

            return false;
        }

        private async Task<Boolean> CanSignOffAcceptedBySignature(string adminDatabaseTable, string databaseTable, ObservableCollection<RecordConfirmation> gvContent, int adminID, Boolean signed, Boolean live, int orderNo)
        {
            Boolean previousSignaturesNotSigned = false;
            if (orderNo > 1)
            {
                string SQL = "SELECT *  FROM[" + adminDatabaseTable + "] AS T1 INNER JOIN(SELECT* FROM [" +
                     databaseTable + "] WHERE [ETestPackageID] = " + CurrentPageHelper.ETestPackage.ID +
                     " AND ([Signed] = 0 OR [Signed] IS NULL)) AS T2 ON T1.[ID] = T2.[AdminID] WHERE T1.[ProjectID] = " +
                    Settings.ProjectID + " AND T1.[OrderNo] =  '" + --orderNo + "'";

                var result = await _adminTestRecordAcceptedByRepository.QueryAsync<T_AdminTestRecordAcceptedBy>(SQL); // 

                previousSignaturesNotSigned = result.Any();

            }

            if (previousSignaturesNotSigned)
            {
                await _userDialogs.AlertAsync("Previous accepted by signatures must be completed prior.", "Accepted By Signature", "OK");
                return false;
            }

            if (await CanSignOffCertificiationSignature(adminDatabaseTable, databaseTable, adminID, signed, live))
            {
                if (!signed)
                {
                    foreach (RecordConfirmation row in gvContent)
                    {

                        if (!row.Signed)
                        {
                            await _userDialogs.AlertAsync("Sorry, all content signatures must be signed before this signature is added.", "Accepted By Signature", "OK");
                            return false;
                        }
                    }
                }

                return true;
            }
            return false;
        }

        private async Task<Boolean> CanSignOffCertificiationSignature(string adminDatabaseTable, string databaseTable, int adminID, Boolean signed, Boolean live)
        {
            string companyCategoryCode = "", functionCode = "", description = "";
            string SQL = "SELECT * FROM [" + adminDatabaseTable + "] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ID] = '" + adminID + "'";
            var result = await _adminTestRecordConfirmation.QueryAsync<T_AdminTestRecordConfirmation>(SQL);


            if (result.Any())
            {
                companyCategoryCode = result.FirstOrDefault().CompanyCategoryCode;
                functionCode = result.FirstOrDefault().FunctionCode;
                description = result.FirstOrDefault().Description;
            }

            if (UserDetails.Function_Code.ToUpper().Trim() != functionCode.ToUpper().Trim() || UserDetails.Company_Category_Code.ToUpper().Trim() != companyCategoryCode.ToUpper().Trim())
            {
                var ansr = await Application.Current.MainPage.DisplayActionSheet("Sorry, you do not have the rights to adjust this signature", "Sign by Other", "OK");
                if (ansr == "Sign by Other")
                {
                    var vm = await ReadLoginPopup();
                    if (vm.Password != null && vm.UserName != null)
                    {
                        var UserDetailsList = await _userDetailsRepository.GetAsync(x => x.UserName == vm.UserName && x.Password == vm.Password);
                        if (UserDetailsList.Any())
                        {
                            UserDetails = UserDetailsList.FirstOrDefault();
                            return true;
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Login", AppConstant.LOGIN_FAILURE, "OK");
                            return false;
                        }


                    }
                }
                else
                {
                    return false;
                }
                //await _userDialogs.AlertAsync("Sorry, You Do Not Have The Correct User Right To Sign Off This Signature", "", "Ok");
                //return false;
            }

            if (companyCategoryCode == "S")
            {
                string subContractor = CurrentPageHelper.ETestPackage.SubContractor;

                if (UserDetails.Company_Code.ToUpper().Trim() != subContractor.ToUpper().Trim())
                {
                    var ansr = await Application.Current.MainPage.DisplayActionSheet("Sorry, you do not have the rights to adjust this signature", "Sign by Other", "OK");
                    if (ansr == "Sign by Other")
                    {
                        var vm = await ReadLoginPopup();
                        if (vm.Password != null && vm.UserName != null)
                        {
                            var UserDetailsList = await _userDetailsRepository.GetAsync(x => x.UserName == vm.UserName && x.Password == vm.Password);
                            if (UserDetailsList.Any())
                            {
                                UserDetails = UserDetailsList.FirstOrDefault();
                                return true;
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Login", AppConstant.LOGIN_FAILURE, "OK");
                                return false;
                            }


                        }
                    }
                    else
                    {
                        return false;
                    }

                    //await _userDialogs.AlertAsync("Sorry, You Do Not Have The Correct User Right To Sign Off This Signature", "", "OK");
                    //return false;
                }
            }

            if (live)
            {
                await _userDialogs.AlertAsync("Sorry, this signature has already been uploaded to VMLive and cannot be " + (signed ? "signed" : "removed") + " on the handheld.", "", "OK");
                return false;
            }

            return true;
        }
        public async Task<Boolean> SaveNew(int projectID, int etestPackageID, string databaseTable, T_TestRecordConfirmation signature)
        {
            string SQL = "SELECT *  FROM [" + databaseTable + "] WHERE [ProjectID] = '" + projectID + "' AND [ETestPackageID] = '" + etestPackageID + "' AND [AdminID] = '" + signature.AdminID + "'";
            var sqlreturn = await _testRecordConfirmationRepository.QueryAsync(SQL);

            if (sqlreturn.Any())
            {
                Boolean returnvalue = true;
                try
                {
                   
                    //Found use sql update.
                    string sql = "UPDATE [" + databaseTable + "] SET [Signed] = '" +Convert.ToInt32(signature.Signed) + "' ,[SignedByUserID] = '" + signature.SignedByUserID + "',[SignedBy] = '" + signature.SignedBy
                        + "',[SignedOn] = '" + signature.SignedOn.Ticks + "', [Live] = '" + signature.Live + "', [Updated] = '" + Convert.ToInt32(signature.Updated) + "' WHERE [ProjectID] = '"
                        + Settings.ProjectID + "' AND [ETestPackageID] = '" + CurrentPageHelper.ETestPackage.ID + "' AND [AdminID] = '" + signature.AdminID + "'";

                    _= await _testRecordConfirmationRepository.QueryAsync(sql);
                    return returnvalue;
                }
                catch (Exception Ex)
                {
                    returnvalue = false;
                    return returnvalue;
                }
            }


            else
            {
                try
                {
                    //Insert fresh details.
                    string sql = "INSERT INTO [" + databaseTable + "] ([ProjectID],[ETestPackageID],[AdminID],[Signed],[SignedByUserID],[SignedBy],[SignedOn],[Live],[Updated]) VALUES " +
                                                               "('" + Settings.ProjectID + "','" + CurrentPageHelper.ETestPackage.ID + "','" + signature.AdminID + "','" + Convert.ToInt32( signature.Signed )+ "','"
                                                               + signature.SignedByUserID + "','" + signature.SignedBy + "','" + signature.SignedOn.Ticks + "',0,1)";

                    _ = _testRecordConfirmationRepository.QueryAsync(sql);
                    return true;
                }
                catch
                {
                    return false;
                }

            }

        }
        public async Task<bool> CheckNextControlLogSigned(int projectID, int etestPackageID, int signatureNo)
        {

            var LocalSQLFunctions = await _AdminControlLog.QueryAsync<T_ControlLogSignature>("SELECT * FROM [T_AdminControlLog] AS ADMIN INNER JOIN [T_ControlLogSignature] CL ON ADMIN.[ProjectID] = CL.[ProjectID] AND ADMIN.[ID] = CL.[ControlLogAdminID] " +
                                                            "WHERE ADMIN.[ProjectID] = '" + projectID + "' AND CL.[ETestPackageID] = '" + etestPackageID + "' AND ADMIN.[SignatureNo] > '" + signatureNo + "' AND [MileStone] = 1");


            if (!LocalSQLFunctions.Any()) return false;

            var Singed = LocalSQLFunctions.FirstOrDefault();
            if (Singed != null)
            {
                return Singed.Signed;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> CheckPreviousControlLogSigned(int projectID, int etestPackageID, int signatureNo)
        {

            var LocalSQLFunctions = await _AdminControlLog.QueryAsync<T_ControlLogSignature>("SELECT * FROM [T_AdminControlLog] AS ADMIN INNER JOIN [T_ControlLogSignature] CL ON ADMIN.[ProjectID] = CL.[ProjectID] AND ADMIN.[ID] = CL.[ControlLogAdminID] " +
                                                            "WHERE ADMIN.[ProjectID] = '" + projectID + "' AND CL.[ETestPackageID] = '" + etestPackageID + "' AND ADMIN.[SignatureNo] < '" + signatureNo + "' ORDER BY ADMIN.[SignatureNo] DESC");

            var Singed = LocalSQLFunctions.FirstOrDefault();
            if (Singed != null)
            {
                return Singed.Signed;
            }
            else
            {
                return false;
            }

        }

        public static Boolean CheckIsTestPackCoordinator(T_UserDetails user, string testpackageSubContractor)
        {
            if (user.ETP_SuperUser)
                return true;

            if (user.Company_Code.ToUpper() != testpackageSubContractor.ToUpper())
                return false;

            if (user.Function_Code != "32") //32 is for test pack co-ordinator
                return false;

            return true;
        }

        public static Boolean ControlLogRightsCheck(T_AdminControlLog adminControlLog, T_UserDetails user, string testpackageSubContractor)
        {
            if (user.Function_Code.ToUpper() != adminControlLog.FunctionCode.ToUpper())
                return false;

            if (user.Company_Category_Code.ToUpper() != adminControlLog.CompanyCategoryCode.ToUpper())
                return false;

            if (user.Section_Code.ToUpper() != adminControlLog.SectionCode.ToUpper())
                return false;

            if (user.Company_Category_Code.ToUpper() == "S" && user.Company_Code.ToUpper() != testpackageSubContractor.ToUpper())
                return false;


            return true;
        }
        public async Task<Boolean> ControlLogDocumentsCheck(int projectID, int etestPackageID, T_AdminControlLog adminControlLog, Boolean signed)
        {
            if (signed) //Removing signature so check not required.
                return true;

            //Get document list
            var folders = await _AdminControlLogFolderRepository.GetAsync(x => x.ControlLogAdminID == adminControlLog.ID && x.ProjectID == projectID);


            if (!folders.Any())
                return true;

            foreach (T_AdminControlLogFolder folder in folders)
            {
                var found = await _attachedDocumentRepository.GetAsync(x => x.ProjectID == projectID && x.ETestPackageID == etestPackageID && x.FolderID == folder.FolderAdminID);

                if (!found.Any())
                    return false;
            }

            return true;
        }
        #endregion

        //camera functinality 
        #region Camera Fuctionalty

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


        public async void CameraSaveBT_Click(string FileName)
        {
            TestPackageImage img = null;
            if (string.IsNullOrEmpty(FileName))
            {
                string newDisplayName = GetNewImageDisplayName();

                img = new TestRecordImage()
                {
                    DisplayName = newDisplayName,
                    FileName = newDisplayName + ".jpeg",
                    Extension = ".jpeg",
                };
            }
            else
            {
                img = new TestRecordImage()
                {
                    DisplayName = Path.GetFileNameWithoutExtension(FileName),
                    FileName = FileName,
                    Extension = Path.GetExtension(FileName),
                };
            }


            using (SKImage image = SKImage.FromBitmap(Bitmap))
            {
                SKData data = image.Encode();
                data.Size.CompareTo(30);
                imageAsByte = await _resizeImageService.GetResizeImage(data.ToArray());
            }

            Byte[] fileBytes = imageAsByte;


            if (imageAsByte != null)
            {

                img.FileSize = fileBytes.Count();
                string base64 = Convert.ToBase64String(fileBytes);
                img.FileBytes = base64;

                bool insertImage = false;
                try
                {
                    var SQL = img.InsertQuery(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID);
                    var data = await _TestRecordImageRepository.QueryAsync(SQL);
                    insertImage = true;
                }
                catch (Exception EX)
                {
                    insertImage = false;
                }

                if (insertImage)
                {
                    //CameraLB.Items.Add(img);
                    UpdateETestPackageStatus(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID);

                    string SQL1 = "UPDATE [T_PunchList] SET [Updated] = 1 WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ETestPackageID] = '" + CurrentPageHelper.ETestPackage.ID + "' AND [PunchID] = '" + CurrentPageHelper.PunchID + "'";

                    var data1 = await _TestRecordImageRepository.QueryAsync(SQL1);
                    await _userDialogs.AlertAsync("Saved Successfully...! .", "Image Save", "OK");
                    GetTestRecordImages(true);
                }
                else
                    await _userDialogs.AlertAsync("Error saving image to database.", "Image Save", "OK");
            }

            else
                _userDialogs.AlertAsync("Please select camera and take a picture to save", null, "OK");
        }


        public async void GetTestRecordImages(bool IsBind)
        {
            List<TestPackageImage> list = new List<TestPackageImage>();

            string SQL = "SELECT * FROM [T_TestRecordImage] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ETestPackageID]= '" + CurrentPageHelper.ETestPackage.ID + "'";

            var data = await _TestRecordImageRepository.QueryAsync<T_TestRecordImage>(SQL);

            foreach (T_TestRecordImage TR in data)
            {

                TestRecordImage img = new TestRecordImage
                {
                    DisplayName = TR.DisplayName,
                    FileName = TR.FileName,
                    Extension = TR.Extension,
                    FileSize = TR.FileSize,
                    FileBytes = TR.FileBytes,
                };

                list.Add(img);
            }
            ImageFiles = null;
            ImageFiles = new ObservableCollection<TestPackageImage>(list);

            if (IsBind)
            {
                var count = ImageFiles.Count();
                if (count > 0)
                {
                    SelectedImageFiles = ImageFiles[count - 1];
                }
            }


            if (ImageFiles.Count > 0)
                CameraIcon = "Greencam.png";
            else
                CameraIcon = "cam.png";
        }

        private async void ImageDelete()
        {
            if (SelectedImageFiles != null)
            {
                if (await _userDialogs.ConfirmAsync($"Are you sure you want to delete?", $"Delete image", "Yes", "No"))
                {
                    string SQL = "DELETE FROM [T_TestRecordImage] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ETestPackageID] = '" + CurrentPageHelper.ETestPackage.ID + "' AND [DisplayName] = '" + SelectedImageFiles.DisplayName + "'";

                    try
                    {
                        var data = await _TestRecordImageRepository.QueryAsync(SQL);
                        RenameImage = true;
                        GetTestRecordImages(false);
                    }
                    catch (Exception EX)
                    {
                        _userDialogs.AlertAsync("Error occurred deleting image", "Delete Image", "OK");
                    }

                }
            }
            else
                await Application.Current.MainPage.DisplayAlert("", "Please select image for delete", "OK");
        }

        private async void BTRenameImage_Click()
        {
            if (NewImageName != string.Empty || NewImageName != null)
            {
                // int index = CameraLB.SelectedIndex;
                string newDisplayName = NewImageName;

                foreach (string IllegalChar in new string[] { "/", "\"", "?", "&" })
                    newDisplayName = newDisplayName.Replace(IllegalChar, string.Empty);

                if (newDisplayName != string.Empty)
                {
                    //Check if a different image already has that name
                    object obj = SelectedImageFiles;

                    Boolean alreadyExists = false;
                    foreach (TestPackageImage existingImage in ImageFiles)
                    {
                        if (existingImage != (TestPackageImage)obj && existingImage.DisplayName.ToUpper() == newDisplayName.ToUpper())
                        {
                            alreadyExists = true;
                            break;
                        }
                    }

                    if (!alreadyExists)
                    {

                        TestRecordImage img = SelectedImageFiles as TestRecordImage;
                        string SQL = "UPDATE [T_TestRecordImage] SET [DisplayName] = '" + newDisplayName + "',[FileName] = '" + (newDisplayName + img.Extension) + "' " +
                            "WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ETestPackageID] = '" + CurrentPageHelper.ETestPackage.ID + "' AND [DisplayName] = '" + img.DisplayName + "'";


                        try
                        {

                            var data = await _TestRecordImageRepository.QueryAsync(SQL);
                            img.DisplayName = newDisplayName;
                            img.FileName = newDisplayName + img.Extension;

                            SelectedImageFiles = img;

                            Showbuttons = true;
                            _userDialogs.AlertAsync("File has been renamed successfully", "Rename Image", "OK");
                        }
                        catch (Exception EX)
                        {
                            _userDialogs.AlertAsync("Error occurred renaming the image", "Rename Image", "OK");
                        }
                    }
                    else
                        _userDialogs.AlertAsync("A different file with this name already exists, please adjust name", "Rename Image", "OK");
                }
            }
        }
        public static Task<LoginModel> ReadLoginPopup()
        {
            var vm = new SignOffPopupViewModel();
            var tcs = new TaskCompletionSource<LoginModel>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var page = new SignOffPopup(vm);
                await PopupNavigation.PushAsync(page);
                var value = await vm.GetValue();
                await PopupNavigation.PopAsync(true);
                tcs.SetResult(value);
            });
            return tcs.Task;
        }

        #endregion
    }


}

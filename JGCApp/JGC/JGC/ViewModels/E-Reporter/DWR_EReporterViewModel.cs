using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Interfaces;
using JGC.Common.Extentions;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using Prism.Navigation;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using JGC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Permissions.Abstractions;
using Plugin.Permissions;
using Plugin.Media.Abstractions;
using System.IO;
using Plugin.Media;
using JGC.Common.Helpers;

namespace JGC.ViewModels
{
    public class DWR_EReporterViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IRepository<T_EReports> _eReportsRepository;
        private readonly IRepository<T_Welders> _weldersRepository;
        private readonly IUserDialogs _userDialogs;
        private readonly IRepository<T_EReports_Signatures> _signaturesRepository;
        private readonly IRepository<T_Drawings> _drawingsRepository;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_RT_Defects> defectsRepository;
        private readonly IRepository<T_WPS_MSTR> wpsRepository;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_BaseMetal> _baseMetalRepository;
        private readonly IRepository<T_DWR_HeatNos> _DWR_HeatNosRepository;
        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private readonly IRepository<T_EReports_UsersAssigned> _usersAssignedRepository;
        private T_UserDetails userDetail;
        private byte[] imageAsByte;
        private T_EReports _selectedEReportItem;
        private readonly IMedia _media;
        private string Vi, Di;
        private byte[] ImageAsByte;
        private string InspectionPath;
        private string CameraValue;
       


        #region Properties  
        private bool isVisibleList { get; set; }
        private bool isVisibleList2 { get; set; }
        private bool isVisibleRemarkForm { get; set; }
        private bool isVisibleButtons1 { get; set; }
        private bool isVisibleViPicker { get; set; }
        private bool isVisibleDiPicker { get; set; }

        private DWR CurrentDWR { get; set; }
        private Defects viComment { get; set; }
        public Defects ViComment
        {
            get { return viComment; }
            set { viComment = value; RaisePropertyChanged(); }
        }
        private string diComment { get; set; }
        public string DiComment
        {
            get { return diComment; }
            set { diComment = value; RaisePropertyChanged(); }
        }
        private string remarks; //{ get; set; }
        public string Remarks
        {
            get { return remarks; }
            set { SetProperty(ref remarks, value); }
        }
        private string viAccColor { get; set; }
       public string ViAccColor
        {
            get { return viAccColor; }
            set { viAccColor = value; RaisePropertyChanged(); }
        }
        private string viRejColor { get; set; }
        public string ViRejColor
        {
            get { return viRejColor; }
            set { viRejColor = value; RaisePropertyChanged(); }
        }
        private string viBlankColor { get; set; }
        public string ViBlankColor
        {
            get { return viBlankColor; }
            set { viBlankColor = value; RaisePropertyChanged(); }
        }

        private string diAccColor { get; set; }
        public string DiAccColor
        {
            get { return diAccColor; }
            set { diAccColor = value; RaisePropertyChanged(); }
        }
        private string diRejColor { get; set; }
        public string DiRejColor
        {
            get { return diRejColor; }
            set { diRejColor = value; RaisePropertyChanged(); }
        }
        private string diBlankColor { get; set; }
        public string DiBlankColor
        {
            get { return diBlankColor; }
            set { diBlankColor = value; RaisePropertyChanged(); }
        }
        private string viCameraIcon { get; set; }
        private string diCameraIcon { get; set; }
        public string ViCameraIcon
        {
            get { return viCameraIcon; }
            set { viCameraIcon = value; RaisePropertyChanged(); }
        }
        public string DiCameraIcon
        {
            get { return diCameraIcon; }
            set { diCameraIcon = value; RaisePropertyChanged(); }
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
        private ObservableCollection<T_EReports_Signatures> _signatureList;
        public ObservableCollection<T_EReports_Signatures> SignatureList
        {
            get { return _signatureList; }
            set { _signatureList = value; RaisePropertyChanged("SignatureList"); OnPropertyChanged(); }
        }

        //private ObservableCollection<DWRRow> dWRRows { get; set; }
      
        //private ObservableCollection<DWRRow> projectDetails { get; set; }

        private bool mainGrid;
        public bool   MainGrid
        {
            get { return mainGrid; }
            set { mainGrid = value; RaisePropertyChanged(); }
        }

        public bool IsVisibleList    
        {
            get { return isVisibleList; }
            set { isVisibleList = value; RaisePropertyChanged(); }
        }
        public bool IsVisibleRemarkForm
        {
            get { return isVisibleRemarkForm; }
            set { isVisibleRemarkForm = value; RaisePropertyChanged(); }
        }
        private bool isVisibleEditForm { get; set; }
        public bool IsVisibleEditForm
        {
            get { return isVisibleEditForm; }
            set { isVisibleEditForm = value; RaisePropertyChanged(); }
        }
        private bool isVisibleEditForm2 { get; set; }
        public bool IsVisibleEditForm2
        {
            get { return isVisibleEditForm2; }
            set { isVisibleEditForm2 = value; RaisePropertyChanged(); }
        }

        private bool signaturesGrid { get; set; }
        public bool SignaturesGrid
        {
            get { return signaturesGrid; }
            set { signaturesGrid = value; RaisePropertyChanged(); }
        }
        
        private bool attachmentsGrid { get; set; }
        public bool AttachmentsGrid
        {
            get { return attachmentsGrid; }
            set { attachmentsGrid = value; RaisePropertyChanged(); }
        }

        public bool IsVisibleList2
        {
            get { return isVisibleList2; }
            set { isVisibleList2 = value; RaisePropertyChanged(); }
        }
        public bool IsVisibleButtons1
        {
            get { return isVisibleButtons1; }
            set { isVisibleButtons1 = value; RaisePropertyChanged(); }
        }
        private bool isVisibleButtons2 { get; set; }
        public bool IsVisibleButtons2
        {
            get { return isVisibleButtons2; }
            set { isVisibleButtons2 = value; RaisePropertyChanged(); }
        }
        public bool IsVisibleViPicker
        {
            get { return isVisibleViPicker; }
            set { isVisibleViPicker = value; RaisePropertyChanged(); }
        }
        public bool IsVisibleDiPicker
        {
            get { return isVisibleDiPicker; }
            set { isVisibleDiPicker = value; RaisePropertyChanged(); }
        }
        //public ObservableCollection<DWRRow> DWRRows
        //{
        //    get { return dWRRows; }
        //    set { dWRRows = value; RaisePropertyChanged(); }
        //}
        //public ObservableCollection<DWRRow> ProjectDetails
        //{
        //    get { return projectDetails; }
        //    set { projectDetails = value; RaisePropertyChanged(); }
        //}
        //private DWRRow selectedDWRRow;
        //public DWRRow SelectedDWRRow
        //{
        //    get { return selectedDWRRow; }
        //    set
        //    {
        //        //SetProperty(ref selectedDWRRow, value );
        //        //RaisePropertyChanged();
        //        selectedDWRRow = value;
        //    }
        //}
        private bool _detailsArrow;
        public bool DetailsArrow
        {
            get { return _detailsArrow; }
            set { SetProperty(ref _detailsArrow, value); }
        }
        private bool _signaturesArrow;
        public bool SignaturesArrow
        {
            get { return _signaturesArrow; }
            set { SetProperty(ref _signaturesArrow, value); }
        }
        private bool _attachmentsArrow;

        
        public bool AttachmentsArrow
        {
            get { return _attachmentsArrow; }
            set { SetProperty(ref _attachmentsArrow, value); }
        }

        private ObservableCollection<Defects> viDefects;
        public ObservableCollection<Defects> ViDefects
        {
            get { return viDefects; }
            set { viDefects = value; RaisePropertyChanged(); }
        }

        private T_EReports_Signatures _selectedSignatureItem;
        public T_EReports_Signatures SelectedSignatureItem
        {
            get { return _selectedSignatureItem; }
            set
            {
                if (SetProperty(ref _selectedSignatureItem, value))
                {
                   // UpdateSignatureItem(_selectedSignatureItem);
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<T_Drawings> _attachmentList;
        public ObservableCollection<T_Drawings> AttachmentList
        {
            get { return _attachmentList; }
            set { _attachmentList = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Welder> weldersList;
        public ObservableCollection<Welder> WeldersList
        {
            get { return weldersList; }
            set { weldersList = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<WPS> wPSNoList;
        public ObservableCollection<WPS> WPSNoList
        {
            get { return wPSNoList; }
            set { wPSNoList = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_BaseMetal> baseMetalItemSource1;
        public ObservableCollection<T_BaseMetal> BaseMetalItemSource1
        {
            get { return baseMetalItemSource1; }
            set { baseMetalItemSource1 = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_BaseMetal> baseMetalItemSource2;
        public ObservableCollection<T_BaseMetal> BaseMetalItemSource2
        {
            get { return baseMetalItemSource2; }
            set { baseMetalItemSource2 = value; RaisePropertyChanged(); }
        }

        private T_BaseMetal baseMetal1;
        public T_BaseMetal SelectedBaseMetal1
        {
            get { return baseMetal1; }
            set
            {
                if (SetProperty(ref baseMetal1, value))
                {
                    RaisePropertyChanged("SelectedBaseMetal1");
                    OnPropertyChanged();
                }
            }
        }
        
        private string baseMetal1Text;
        public string SelectedBaseMetal1Text
        {
            get { return baseMetal1Text; }
            set
            {
                if (SetProperty(ref baseMetal1Text, value))
                {
                    RaisePropertyChanged("SelectedBaseMetal1Text");
                    OnPropertyChanged();
                }
            }
        }




        private T_BaseMetal baseMetal2;
        public T_BaseMetal SelectedBaseMetal2
        {
            get { return baseMetal2; }
            set
            {
                //SetProperty(ref baseMetal2, value);
                //RaisePropertyChanged();
                if (SetProperty(ref baseMetal2, value))
                {
                    RaisePropertyChanged("SelectedBaseMetal1");
                    OnPropertyChanged();
                }

            }
        }
        private string baseMetal2Text;
        public string SelectedBaseMetal2Text
        {
            get { return baseMetal2Text; }
            set
            {
                if (SetProperty(ref baseMetal2Text, value))
                {
                    RaisePropertyChanged("SelectedBaseMetal2Text");
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<T_DWR_HeatNos> heatNosItemSource1;
        public ObservableCollection<T_DWR_HeatNos> HeatNosItemSource1
        {
            get { return heatNosItemSource1; }
            set { heatNosItemSource1 = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_DWR_HeatNos> heatNosItemSource2;
        public ObservableCollection<T_DWR_HeatNos> HeatNosItemSource2
        {
            get { return heatNosItemSource2; }
            set { heatNosItemSource2 = value; RaisePropertyChanged(); }
        }
        
        private T_DWR_HeatNos selectedHeatNos1;
        public T_DWR_HeatNos SelectedHeatNos1
        {
            get { return selectedHeatNos1; }
            set { selectedHeatNos1 = value; RaisePropertyChanged(); }
        }
        private T_DWR_HeatNos selectedHeatNos2;
        public T_DWR_HeatNos SelectedHeatNos2
        {
            get { return selectedHeatNos2; }
            set { selectedHeatNos2 = value; RaisePropertyChanged(); }
        }

        private string heatNos1;
        public string SelectedHeatNos1Text
        {
            get { return heatNos1; }
            set
            {
                if (SetProperty(ref heatNos1, value))
                {
                    RaisePropertyChanged("SelectedHeatNos1Text");
                    OnPropertyChanged();
                }

            }
        }
        private string heatNos2;
        public string SelectedHeatNos2Text
        {
            get { return heatNos2; }
            set
            {
                if (SetProperty(ref heatNos2, value))
                {
                    RaisePropertyChanged("SelectedHeatNos2Text");
                    OnPropertyChanged();
                }

            }
        }

        private WPS selctedWpsNumber;
        public WPS SelctedWpsNumber
        {

            get { return selctedWpsNumber; }
            set { selctedWpsNumber = value; RaisePropertyChanged(); }
            
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
        private string newImageName;
        public string NewImageName
        {
            get { return newImageName; }
            set
            {
                SetProperty(ref newImageName, value);
            }
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
        private bool showRename;
        public bool ShowRename
        {
            get { return showRename; }
            set
            {
                SetProperty(ref showRename, value);
            }
        }
        private string _selectedCameraItem;
        public string SelectedCameraItem
        {
            get { return _selectedCameraItem; }
            set
            {
                if (SetProperty(ref _selectedCameraItem, value))
                {
                    SelectedCameraItems(_selectedCameraItem);
                    OnPropertyChanged();
                }
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
        private T_Drawings _selectedAttachedItem;
        public T_Drawings SelectedAttachedItem
        {
            get { return _selectedAttachedItem; }
            set
            {
                if (SetProperty(ref _selectedAttachedItem, value))
                {
                    LoadAttachmentPDF(_selectedAttachedItem);
                    OnPropertyChanged();
                }
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
        private bool _cameraGrid;
        public bool CameraGrid
        {
            get { return _cameraGrid; }
            set { SetProperty(ref _cameraGrid, value); }
        }
        
        private Welder welder1;
        public Welder Welder1
        {
            get { return welder1; }
            set { SetProperty(ref welder1, value); }
        }

        private Welder welder2;
        public Welder Welder2
        {
            get { return welder2; }
            set { SetProperty(ref welder2, value); }
        }
        private Welder welder3;
        public Welder Welder3
        {
            get { return welder3; }
            set { SetProperty(ref welder3, value); }
        }
        private Welder welder4;
        public Welder Welder4
        {
            get { return welder4; }
            set { SetProperty(ref welder4, value); }
        }
        private Welder welder5;
        public Welder Welder5
        {
            get { return welder5; }
            set { SetProperty(ref welder5, value); }
        }
        private Welder welder6;
        public Welder Welder6
        {
            get { return welder6; }
            set { SetProperty(ref welder6, value); }
        }
        private Welder welder7;
        public Welder Welder7
        {
            get { return welder7; }
            set { SetProperty(ref welder7, value); }
        }
        private Welder welder8;
        public Welder Welder8
        {
            get { return welder8; }
            set { SetProperty(ref welder8, value); }
        }
        private string _item;
        public string DWRitem
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        private ProductionButtonColor obj;
        public ProductionButtonColor ProductionButtonColor
        {
            get { return obj; }
            set { obj = value; RaisePropertyChanged(); }
        }

        private string pdfUrl;
        public string PdfUrl
        {
            //get { return pdfUrl; }
            //set
            //{
            //    SetProperty(ref pdfUrl, value);
            //}

            get { return pdfUrl; }
            set { pdfUrl = value; RaisePropertyChanged(); }
        }


        private ObservableCollection<string> _ImageFiles;
        public ObservableCollection<string> ImageFiles
        {
            get { return _ImageFiles; }
            set { _ImageFiles = value; RaisePropertyChanged(); }
        }

        private bool showBasemetal1;
        public bool ShowBasemetal1
        {
            get { return showBasemetal1; }
            set { SetProperty(ref showBasemetal1, value); }
        }

        private bool showBasemetal2;
        public bool ShowBasemetal2
        {
            get { return showBasemetal2; }
            set { SetProperty(ref showBasemetal2, value); }
        }

        private bool showHeatNos1;
        public bool ShowHeatNos1
        {
            get { return showHeatNos1; }
            set { SetProperty(ref showHeatNos1, value); }
        }
        private bool showHeatNos2;
        public bool ShowHeatNos2
        {
            get { return showHeatNos2; }
            set { SetProperty(ref showHeatNos2, value); }
        }

        private string btnMetalText1;
        public string BtnMetalText1
        {
            get { return btnMetalText1; }
            set { btnMetalText1 = value; RaisePropertyChanged(); }
        }

        private string btnMetalText2;
        public string BtnMetalText2
        {
            get { return btnMetalText2; }
            set { btnMetalText2 = value; RaisePropertyChanged(); }
        }
        private string btnHeatNosText1;
        public string BtnHeatNosText1
        {
            get { return btnHeatNosText1; }
            set { btnHeatNosText1 = value; RaisePropertyChanged(); }
        }
        private string btnHeatNosText2;
        public string BtnHeatNosText2
        {
            get { return btnHeatNosText2; }
            set { btnHeatNosText2 = value; RaisePropertyChanged(); }
        }
      

        

        #endregion



        public DWR_EReporterViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_UserProject> userProjectRepository,
            IRepository<T_RT_Defects> _DefectstRepository,
            IRepository<T_Welders> WeldersRepository,
            IRepository<T_EReports_Signatures> _signaturesRepository,
            IRepository<T_Drawings> drawingsRepository,
            IRepository<T_UserDetails> _userDetailsRepository,
            IRepository<T_WPS_MSTR> _wpsRepository,
            IRepository<T_BaseMetal> baseMetalRepository,
            IRepository<T_DWR_HeatNos> DWR_HeatNosRepository,
            IRepository<T_EReports_UsersAssigned> usersAssignedRepository,
            IMedia _media,
            IRepository<T_EReports> _eReportsRepository ) : base(_navigationService, _httpHelper, _checkValidLogin)

        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._signaturesRepository = _signaturesRepository;
            this._drawingsRepository = drawingsRepository;
            this._eReportsRepository = _eReportsRepository;
            this.defectsRepository = _DefectstRepository;
            this._userDetailsRepository = _userDetailsRepository;
            this._weldersRepository = WeldersRepository;
            this._userProjectRepository = userProjectRepository;
            this._media = _media;
            this.wpsRepository = _wpsRepository;
            this._baseMetalRepository = baseMetalRepository;
            this._DWR_HeatNosRepository = DWR_HeatNosRepository;
            this._usersAssignedRepository = usersAssignedRepository;
            _media.Initialize();
            _userDialogs.HideLoading();

            //isVisible logic
            MainGrid = Showbuttons = true;
            IsVisibleButtons1 = IsVisibleList = DetailsArrow=  true;
            IsVisibleRemarkForm = IsVisibleList2= IsVisibleEditForm = IsVisibleEditForm2 = IsVisibleButtons2 = false;
            PageHeaderText = "Daily Weld Reports";
            ShowBasemetal1 = ShowBasemetal2 = ShowHeatNos1 = ShowHeatNos2 = false;
            BtnHeatNosText1 = BtnHeatNosText2 = BtnMetalText1 = BtnMetalText2 = "Pre-set";
        }



        #region Delegate Commands  
        public ICommand BtnCommand
        {
            get
            {
                return new Command<string>(OnClickButton);
            }
        }

        #endregion


        #region Private

        private async void OnClickButton(string param)
        {
            if (param == "Details" || param == "Signatures" || param == "Attachments")
            {
                DetailsArrow = SignaturesArrow = AttachmentsArrow = false;
            }

            if (param == "Details")
            {
               // SelectedDWRRow = null;
                DetailsArrow = IsVisibleList = IsVisibleButtons1 = IsVisibleEditForm2 = !DetailsArrow;
                IsVisibleList2 = IsVisibleRemarkForm = SignaturesGrid = IsVisibleButtons2 = AttachmentsArrow = SignaturesGrid = IsVisibleEditForm2 = IsVisibleEditForm = CameraGrid = AttachmentsGrid = !IsVisibleList;
                btnSave = false;

                var returndata = await _eReportsRepository.QueryAsync<T_EReports>(@"SELECT * FROM T_EReports WHERE [ID] = " + _selectedEReportItem.ID);
                _selectedEReportItem = returndata.FirstOrDefault();
                CurrentDWR = JsonConvert.DeserializeObject<DWR>(returndata.FirstOrDefault().JSONString);
                if (CurrentDWR != null)
                {
                   // DWRRows = new ObservableCollection<DWRRow>(CurrentDWR.DWRRows);
                }
            }
            else if (param == "Signatures")
            {
                GetSignatureData(_selectedEReportItem.ID);
                SignaturesArrow = SignaturesGrid = !SignaturesArrow;
                IsVisibleList = IsVisibleList2 = IsVisibleButtons1 = IsVisibleButtons2 = IsVisibleRemarkForm = IsVisibleEditForm = IsVisibleEditForm2 = AttachmentsArrow = CameraGrid = AttachmentsGrid = false;
                btnSave = true;
            }
            else if (param == "Attachments")
            {
                GetAttachmentData(_selectedEReportItem.ID);
                AttachmentsArrow = AttachmentsGrid = !AttachmentsArrow;
                IsVisibleList = IsVisibleList2 = IsVisibleButtons1 = IsVisibleButtons2 = IsVisibleRemarkForm = IsVisibleEditForm = IsVisibleEditForm = IsVisibleEditForm2 = SignaturesGrid = CameraGrid = false;
                btnSave = false;
            }
            else if (param == "Editdetails")
            {
                if (SelectedDWRRow != null)
                {
                    string checkSQL = " SELECT DISTINCT UA.[CanEdit],SIG.[SignatureNo] FROM[T_EReports_UsersAssigned] AS UA " +
                                      " INNER JOIN[T_EReports_Signatures] AS SIG ON UA.[EReportID] = SIG.[EReportID] AND UA.[SignatureRulesID] = SIG.[SignatureRulesID] "
                                    + " WHERE UA.[EReportID] = '" + _selectedEReportItem.ID + "' AND UA.[UserID] = '" + Settings.UserID + "' AND SIG.[Signed] = 0 AND SIG.[SignatureNo] < 4";

                    var checkdata = await _usersAssignedRepository.QueryAsync<T_EReports_UsersAssigned>(checkSQL);
                    if (checkdata.Count > 0)
                    {
                        IsVisibleList = IsVisibleList2 = IsVisibleButtons1 = IsVisibleButtons2 = IsVisibleRemarkForm = false;
                        IsVisibleEditForm = DetailsArrow = true;

                        GetDetailsData();
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("DWR Reports", " Please Select Report", "OK");
                }


            }
            else if (param == "ViCamera")
            {
                CameraValue = "VI";
                _userDialogs.ShowLoading("Loading...");
                generatepath(CameraValue);
                btnSaveDelete = "Save";
                CameraItems = new List<string> { "USB2.0_Camera", "USB2.0_Camera 1" };
                IsVisibleRemarkForm = !IsVisibleRemarkForm;
                MainGrid = !MainGrid;
                CameraGrid = !CameraGrid;
                SelectedImageFiles = "";
                _userDialogs.HideLoading();

            }

            else if (param == "DiCamera")
            {
                CameraValue = "DI";
                _userDialogs.ShowLoading("Loading...");
                generatepath(CameraValue);
                btnSaveDelete = "Save";
                CameraItems = new List<string> { "USB2.0_Camera", "USB2.0_Camera 1" };
                IsVisibleRemarkForm = !IsVisibleRemarkForm;
                MainGrid = !MainGrid;
                CameraGrid = !CameraGrid;
                SelectedImageFiles = "";
                _userDialogs.HideLoading();


            }
            else if (param == "ViAcc")
            {
                Vi = "ACC";
                IsVisibleViPicker = false;
                ViAccColor = "#8ab6ff";
                ViBlankColor = ViRejColor = "White";
            }
            else if (param == "ViReject")
            {
                Vi = "REJ";
                IsVisibleViPicker = true;
                ViRejColor = "#ef3840";
                ViBlankColor = ViAccColor = "White";

            }
            else if (param == "ViBlank")
            {
                Vi = "";
                IsVisibleViPicker = false;
                ViBlankColor = "#c7c7c7";
                ViRejColor = ViAccColor = "White";
            }
            else if (param == "DiAcc")
            {
                Di = "ACC";
                IsVisibleDiPicker = false;
                DiAccColor = "#8ab6ff";
                DiBlankColor = DiRejColor = "White";
            }

            else if (param == "DiReject")
            {
                Di = "REJ";
                IsVisibleDiPicker = true;
                DiRejColor = "#ef3840";
                DiBlankColor = DiAccColor = "White";
            }
            else if (param == "DiBlank")
            {
                Di = "";
                IsVisibleDiPicker = false;
                DiBlankColor = "#c7c7c7";
                DiRejColor = DiAccColor = "White";
            }
            else if (param == "SaveJoint")
            {



                DWR CurrentDWR = JsonConvert.DeserializeObject<DWR>(_selectedEReportItem.JSONString);
                DWRRow ObjDWRRow = CurrentDWR.DWRRows.Single(x => x.Number == SelectedDWRRow.Number);

                if (ObjDWRRow != null)
                {

                    ObjDWRRow.VI = Vi;
                    ObjDWRRow.DI = Di;
                    ObjDWRRow.DI_Comment = DiComment;
                    if (ViComment != null)
                        ObjDWRRow.VI_Comment = ViComment.Description;

                    ObjDWRRow.Remarks = Remarks;
                }

                //   var result = DWRRows;

                string JSONString = JsonConvert.SerializeObject(CurrentDWR);
                await _eReportsRepository.QueryAsync<T_EReports>(@"UPDATE T_EReports SET [JSONString] = '" + JSONString.Replace("'", "''") + "' , [Updated] = '" + 1 + "' WHERE [ID] = " + _selectedEReportItem.ID);
                var returndata = await _eReportsRepository.QueryAsync<T_EReports>(@"SELECT * FROM T_EReports WHERE [ID] = " + _selectedEReportItem.ID);
                _selectedEReportItem = returndata.FirstOrDefault();
                CurrentDWR = JsonConvert.DeserializeObject<DWR>(returndata.FirstOrDefault().JSONString);
                if (CurrentDWR != null)
                {
                    DWRRows = new ObservableCollection<DWRRow>(CurrentDWR.DWRRows);
                }

                SelectedDWRRow = ObjDWRRow;
                //GetDetails();
                await _userDialogs.AlertAsync("Inspection data saved successfully", "Save Inspection Data", "OK");



            }
            else if (param == "SaveForm")
            {
                string white = "#ffffff";
                DWR CurrentDWR = JsonConvert.DeserializeObject<DWR>(_selectedEReportItem.JSONString);
                DWRRow ObjDWRRow = CurrentDWR.DWRRows.Single(x => x.Number == SelectedDWRRow.Number);

                if (ObjDWRRow != null)
                {

                    ObjDWRRow.Root_Welder_1 = Welder1 == null ? "" : Welder1.Welder_Code;
                    ObjDWRRow.Root_Welder_2 = Welder2 == null ? "" : Welder2.Welder_Code;
                    ObjDWRRow.Root_Welder_3 = Welder3 == null ? "" : Welder3.Welder_Code;
                    ObjDWRRow.Root_Welder_4 = Welder4 == null ? "" : Welder4.Welder_Code;
                    ObjDWRRow.FillCap_Welder_1 = Welder5 == null ? "" : Welder5.Welder_Code;
                    ObjDWRRow.FillCap_Welder_2 = Welder6 == null ? "" : Welder6.Welder_Code;
                    ObjDWRRow.FillCap_Welder_3 = Welder7 == null ? "" : Welder7.Welder_Code;
                    ObjDWRRow.FillCap_Welder_4 = Welder8 == null ? "" : Welder8.Welder_Code;


                    ObjDWRRow.Root_Welder_1_Production_Joint = ProductionButtonColor.welder1 == white ? false : true;
                    ObjDWRRow.Root_Welder_2_Production_Joint = ProductionButtonColor.welder2 == white ? false : true;
                    ObjDWRRow.Root_Welder_3_Production_Joint = ProductionButtonColor.welder3 == white ? false : true;
                    ObjDWRRow.Root_Welder_4_Production_Joint = ProductionButtonColor.welder4 == white ? false : true;

                    ObjDWRRow.FillCap_Welder_1_Production_Joint = ProductionButtonColor.welder5 == white ? false : true;
                    ObjDWRRow.FillCap_Welder_2_Production_Joint = ProductionButtonColor.welder6 == white ? false : true;
                    ObjDWRRow.FillCap_Welder_3_Production_Joint = ProductionButtonColor.welder7 == white ? false : true;
                    ObjDWRRow.FillCap_Welder_4_Production_Joint = ProductionButtonColor.welder8 == white ? false : true;

                    ObjDWRRow.WPS_No = SelctedWpsNumber == null ? "" : SelctedWpsNumber.Wps_No;
                    if (BtnMetalText1 == "Pre-set")
                        ObjDWRRow.Base_Metal_1 = SelectedBaseMetal1 == null ? "" : SelectedBaseMetal1.Base_Material;
                    else
                        ObjDWRRow.Base_Metal_1 = String.IsNullOrEmpty(SelectedBaseMetal1Text.Trim()) ? "" : SelectedBaseMetal1Text;

                    if (BtnMetalText2 == "Pre-set")
                        ObjDWRRow.Base_Metal_2 = SelectedBaseMetal2 == null ? "" : SelectedBaseMetal2.Base_Material;
                    else
                        ObjDWRRow.Base_Metal_2 = String.IsNullOrEmpty(SelectedBaseMetal2Text.Trim()) ? "" : SelectedBaseMetal2Text;

                    if(BtnHeatNosText1 == "Pre-set")
                        ObjDWRRow.Heat_No_1 = SelectedHeatNos1 == null ? "" : SelectedHeatNos1.Heat_No;
                    else
                        ObjDWRRow.Heat_No_1 = String.IsNullOrEmpty(SelectedHeatNos1Text.Trim()) ? "" : SelectedHeatNos1Text;

                    if (BtnHeatNosText2 == "Pre-set")
                        ObjDWRRow.Heat_No_2 = SelectedHeatNos2 == null ? "" : SelectedHeatNos2.Heat_No;
                    else
                        ObjDWRRow.Heat_No_2 = String.IsNullOrEmpty(SelectedHeatNos2Text.Trim()) ? "" : SelectedHeatNos2Text;
                }

                var result = DWRRows;

                string JSONString = JsonConvert.SerializeObject(CurrentDWR);
                await _eReportsRepository.QueryAsync<T_EReports>(@"UPDATE T_EReports SET [JSONString] = '" + JSONString.Replace("'", "''") + "' , [Updated] = '" + 1 + "' WHERE [ID] = " + _selectedEReportItem.ID);
                var returndata = await _eReportsRepository.QueryAsync<T_EReports>(@"SELECT * FROM T_EReports WHERE [ID] = " + _selectedEReportItem.ID);
                _selectedEReportItem = returndata.FirstOrDefault();

                CurrentDWR = JsonConvert.DeserializeObject<DWR>(_selectedEReportItem.JSONString);
                if (CurrentDWR != null)
                {
                    DWRRows = new ObservableCollection<DWRRow>(CurrentDWR.DWRRows);
                }
                await _userDialogs.AlertAsync(" data saved successfully", "Save", "OK");
                // GetDetails();

            }
            else if (param == "Save")
            {

                SaveSignaturesofDWR();

            }

            else if (param == "EditFormNextBtn")
            {
                IsVisibleRemarkForm = IsVisibleEditForm = false;
                IsVisibleEditForm2 = true;
            }
            else if (param == "BackToList")
            {
                DetailsArrow = IsVisibleList = IsVisibleButtons1 = true;
                IsVisibleRemarkForm = IsVisibleList2 = IsVisibleButtons2 = false;
                // SelectedDWRRow = null;

            }
            else if (param == "EditForm2BackBtn")
            {
                IsVisibleEditForm = true;
                IsVisibleEditForm2 = false;
            }
            else if (param == "BackFromCameraGrid")
            {
                CameraGrid = !CameraGrid;
                MainGrid = !MainGrid;
                IsVisibleRemarkForm = !IsVisibleRemarkForm;
                GetInspectionDetails();
            }
            else if (param == "Clear")
            {
                CapturedImage = "";
                btnSaveDelete = "Save";
                RenameImage = false;
                SelectedCameraItems(_selectedCameraItem);
            }
            else if (param.Contains("welder"))
            {
                ProductionButtonColorChnage(param);
            }
            else if (param == "SaveImg")
            {


                string Folder = ("Photo Store" + "\\" + CurrentDWR.JobCode + "\\" + _selectedEReportItem.ID.ToString());
                await DependencyService.Get<ISaveFiles>().SavePictureToDisk(Folder, Device.RuntimePlatform == Device.UWP ? "CapturedImage.png" : "CapturedImage", ImageAsByte);
                await Application.Current.MainPage.DisplayAlert("Successfully saved..!", null, "Ok");

            }

            else if (param == "CaptureImageSave")
            {
                if (btnSaveDelete == "Save")
                    CaptureImageSave();
                else if (btnSaveDelete == "Delete")
                    CaptureImageDelete();
            }
            else if (param == "Clear")
            {
                CapturedImage = "";
                btnSaveDelete = "Save";
                SelectedCameraItems(_selectedCameraItem);
            }
            else if (param == "AddFromFile")
            {
                PickImagesFromGallery();
            }
            else if (param == "RenameImage")
            {
                ShowRename = true;
                Showbuttons = false;
            }
            else if (param == "CancelRename")
            {
                ShowRename = false;
                Showbuttons = true;
                NewImageName = "";
            }
            else if (param == "SaveRename")
            {
                if (NewImageName != null)
                    SaveRenameImage();
                else
                    _userDialogs.AlertAsync("Please enter new name", null, "Ok");
            }
            else if (param == "Back")
            {
                CapturedImage = "";
                CameraGrid = !CameraGrid;
                MainGrid = !CameraGrid;
            }
            else if (param == "EditMetal1")
            {
                if (BtnMetalText1 == "Pre-set")
                {
                    SelectedBaseMetal1Text = "";
                    BtnMetalText1 = "Manual";
                    ShowBasemetal1 = true;
                }
                else
                {
                    try
                    {
                        BtnMetalText1 = "Pre-set";
                        ShowBasemetal1 = false;
                        //T_BaseMetal data = new T_BaseMetal(); 
                        if (!String.IsNullOrWhiteSpace(SelectedBaseMetal1Text))
                        {
                            SelectedBaseMetal1 = new T_BaseMetal { Base_Material = SelectedBaseMetal1Text };
                            BaseMetalItemSource1.Add(SelectedBaseMetal1);
                            BaseMetalItemSource1 = BaseMetalItemSource1;

                            SelectedBaseMetal1 = BaseMetalItemSource1.Where(a => a.Base_Material == SelectedBaseMetal1.Base_Material).LastOrDefault();
                        }


                    }
                    catch (Exception ex)
                    { }


                }

            }
            else if (param == "EditMetal2")
            {
                if (BtnMetalText2 == "Pre-set")
                {
                    BtnMetalText2 = "Manual";
                    ShowBasemetal2 = true;
                    SelectedBaseMetal2Text = "";
                }
                else
                {
                    try
                    {
                        BtnMetalText2 = "Pre-set";
                        ShowBasemetal2 = false;
                        // T_BaseMetal data = new T_BaseMetal(); 
                        if (!String.IsNullOrWhiteSpace(SelectedBaseMetal2Text))
                        {
                            SelectedBaseMetal2 = new T_BaseMetal { Base_Material = SelectedBaseMetal2Text };//SelectedBaseMetal2; 
                            BaseMetalItemSource2.Add(SelectedBaseMetal2);
                            BaseMetalItemSource2 = BaseMetalItemSource2;
                            await Task.Delay(100);
                            SelectedBaseMetal2 = BaseMetalItemSource2.Where(a => a.Base_Material == SelectedBaseMetal2.Base_Material).LastOrDefault();
                        }


                    }
                    catch (Exception ex)
                    { }
                }

            }
            else if (param == "EditHeatNos1")
            {
                if (BtnHeatNosText1 == "Pre-set")
                {
                    BtnHeatNosText1 = "Manual";
                    ShowHeatNos1 = true;
                    SelectedHeatNos1Text = "";
                }
                else
                {
                    try
                    {
                        BtnHeatNosText1 = "Pre-set";
                        ShowHeatNos1 = false;
                        //T_DWR_HeatNos data = new T_DWR_HeatNos();
                        //data = SelectedHeatNos1;
                        // HeatNosItemSource1.Add(data);
                        // await Task.Delay(100);
                        //SelectedHeatNos1 = data;
                        if (!String.IsNullOrWhiteSpace(SelectedHeatNos1Text))
                        {
                            T_DWR_HeatNos data = new T_DWR_HeatNos { Project_ID = Settings.ProjectID, Ident_Code = SelectedDWRRow.Ident_Code1, Heat_No = SelectedHeatNos1Text };
                            HeatNosItemSource1.Add(data);
                            await Task.Delay(100);
                            SelectedHeatNos1 = data;
                        }
                    }
                    catch { }

                }

            }
            else if (param == "EditHeatNos2")
            {
                if (BtnHeatNosText2 == "Pre-set")
                {
                    BtnHeatNosText2 = "Manual";
                    ShowHeatNos2 = true;
                    SelectedHeatNos2Text = "";
                }
                else
                {
                    try
                    {
                        BtnHeatNosText2 = "Pre-set";
                        ShowHeatNos2 = false;
                        if (!String.IsNullOrWhiteSpace(SelectedHeatNos2Text))
                        {
                            //T_DWR_HeatNos data = new T_DWR_HeatNos();
                            T_DWR_HeatNos data = new T_DWR_HeatNos { Project_ID = Settings.ProjectID, Ident_Code = SelectedDWRRow.Ident_Code2, Heat_No = SelectedHeatNos2Text };
                            //data = SelectedHeatNos2;                        
                            HeatNosItemSource2.Add(data);
                            await Task.Delay(100);
                            SelectedHeatNos2 = data;
                        }

                    }
                    catch { }
                }

            }
            else if (param == "PdfFullScreen")
            {
                PdfUriModel pdfuri = new PdfUriModel();
                pdfuri.PdfUriPath = PdfUrl;

                var navigationParameters = new NavigationParameters();
                navigationParameters.Add("uri", pdfuri);
                navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
                await navigationService.NavigateAsync<PDFvieverViewModel>(navigationParameters);


            }
            else if (param == "BackToMain")
            {
               await _navigationService.GoBackAsync();
            }
          

        }
        private void ChangeColor()
        {
            ViAccColor = ViBlankColor = ViRejColor = DiAccColor = DiBlankColor = DiRejColor = "White";
            if (Vi == "ACC")
            {
                ViAccColor = "#8ab6ff";
                IsVisibleViPicker = false;
            }
            else if (Vi == "REJ")
            {
                IsVisibleViPicker = true;
                ViRejColor = "#ef3840";

            }
            else if (Vi == "")
            {
                ViBlankColor = "#c7c7c7";
                IsVisibleViPicker = false;
            }


            if (Di == "ACC")
            {
                DiAccColor = "#8ab6ff";
                IsVisibleDiPicker = false;
            }
            else if (Di == "REJ")
            {
                IsVisibleDiPicker = true;
                DiRejColor = "#ef3840";
            }
            else if (Di == "")
            {
                DiBlankColor = "#c7c7c7";
                IsVisibleDiPicker = false;
            }
        }


        private void SelectedCameraItems(string SelectedCameraItem)
        {
            if (SelectedCameraItem == null)
                return;
            CameraService();
        }
        private async void LoadImageFiles(string SelectedImageFiles)
        {
            if (SelectedImageFiles == null)
                return;
            CapturedImage = await DependencyService.Get<ISaveFiles>().GetImage(InspectionPath + "/" + SelectedImageFiles);
            btnSaveDelete = "Delete";
            RenameImage = true;
        }

        private async void CaptureImageSave()
        {
            if (imageAsByte != null)
            {
                string fileName = DateTime.Now.ToString(AppConstant.CameraDateFormat) + "~" + SelectedDWRRow.Number + "-" + SelectedDWRRow.Joint_No;
                string path = await DependencyService.Get<ISaveFiles>().SavePictureToDisk(InspectionPath, fileName, imageAsByte.ToArray());
                if (path != null)
                {
                    generatepath(CameraValue);
                    RenameImage = false;
                    btnSaveDelete = "Save";
                    await _userDialogs.AlertAsync("Successfully saved..!", null, "Ok");
                }
            }
        
            else
                _userDialogs.AlertAsync("Please select camera and take a picture to save", null, "OK");
        }

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
        private async void generatepath(String Field)
        {
            if (Field == "VI")
            {
                string Folder = ("Photo Store" + "\\" + CurrentDWR.JobCode + "\\" + _selectedEReportItem.ID.ToString() + "\\" + SelectedDWRRow.Number + "\\" + Field);  //Row.Number
                InspectionPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath(Folder);

                ImageFiles = new ObservableCollection<string>(await DependencyService.Get<ISaveFiles>().GetAllImages(InspectionPath));
                if (ImageFiles.Any())
                    ViCameraIcon = "Greencam.png";
                else
                    ViCameraIcon = "cam.png";
            }
            else if (Field == "DI")
            {

                string Folder = ("Photo Store" + "\\" + CurrentDWR.JobCode + "\\" + _selectedEReportItem.ID.ToString() + "\\" + SelectedDWRRow.Number + "\\" + Field);
                InspectionPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath(Folder);

                ImageFiles = new ObservableCollection<string>(await DependencyService.Get<ISaveFiles>().GetAllImages(InspectionPath));
                if (ImageFiles.Any())
                    DiCameraIcon = "Greencam.png";
                else
                    DiCameraIcon = "cam.png";

            }
        }
        private async void SaveRenameImage()
        {
            try
            {
                if (await DependencyService.Get<ISaveFiles>().RenameImage(InspectionPath, SelectedImageFiles, NewImageName + ".jpg"))
                {
                    ImageFiles = new ObservableCollection<string>(await DependencyService.Get<ISaveFiles>().GetAllImages(InspectionPath));
                    ShowRename = false;
                    Showbuttons = true;
                    CapturedImage = "";
                    await _userDialogs.AlertAsync("Successfully rename..!", null, "Ok");
                }
            }
            catch (Exception ex)
            {

            }

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
                imageAsByte = memoryStream.ToArray();
                Stream stream = new MemoryStream(imageAsByte);
                CapturedImage = ImageSource.FromStream(() => stream);
                btnSaveDelete = "Save";
                SelectedImageFiles = null;
            }
            catch (Exception ex)
            {

            }
        }
        private async void PickImagesFromGallery()
        {
            try
            {
               
                var PickFile = await DependencyService.Get<ISaveFiles>().DWRPickFile(InspectionPath, SelectedDWRRow.Number, SelectedDWRRow.Joint_No);
                ImageFiles = new ObservableCollection<string>(await DependencyService.Get<ISaveFiles>().GetAllImages(InspectionPath));
                if (PickFile)
                    await _userDialogs.AlertAsync("Added image file successfully", "Saved Image", "OK");
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
        private async void GetInspectionDetails()
        {
           
            //load welders
            var _defects = await defectsRepository.QueryAsync<T_RT_Defects>(@"SELECT * FROM T_RT_Defects");
            var defects = _defects.Select(x => new Defects() { RT_Defect_Code = x.RT_Defect_Code, Description = x.Description }).ToList();

            ViDefects = new ObservableCollection<Defects>(defects);
            generatepath("VI");
            generatepath("DI");
            if (SelectedDWRRow != null)
            {
                Vi = SelectedDWRRow.VI;
                Di = SelectedDWRRow.DI;
                ChangeColor();
                await Task.Delay(100);
                ViComment = defects.FirstOrDefault(x => x.Description == SelectedDWRRow.VI_Comment);
                DiComment = SelectedDWRRow.DI_Comment;
                Remarks = SelectedDWRRow.Remarks;
                
            }
           
            
        }
        private async void OnReportClicked(string param)
        {
           
        }
        private void ProductionButtonColorChnage(string param)
        {
            ProductionButtonColor buttonColor = new ProductionButtonColor();
            buttonColor = ProductionButtonColor;

            string white = "#ffffff";
            string red = "#FB1610";

            switch (param)
            {
                case "welder1":
                    if (buttonColor.welder1 == white)
                        buttonColor.welder1 = red;
                    else
                        buttonColor.welder1 = white;
                    break;
                case "welder2":
                    if (buttonColor.welder2 == white)
                        buttonColor.welder2 = red;
                    else
                        buttonColor.welder2 = white;
                    break;
                case "welder3":
                    if (buttonColor.welder3 == white)
                        buttonColor.welder3 = red;
                    else
                        buttonColor.welder3 = white;
                    break;
                case "welder4":
                    if (buttonColor.welder4 == white)
                        buttonColor.welder4 = red;
                    else
                        buttonColor.welder4 = white;
                    break;
                case "welder5":
                    if (buttonColor.welder5 == white)
                        buttonColor.welder5 = red;
                    else
                        buttonColor.welder5 = white;
                    break;
                case "welder6":
                    if (buttonColor.welder6 == white)
                        buttonColor.welder6 = red;
                    else
                        buttonColor.welder6 = white;
                    break;
                case "welder7":
                    if (buttonColor.welder7 == white)
                        buttonColor.welder7 = red;
                    else
                        buttonColor.welder7 = white;
                    break;
                case "welder8":
                    if (buttonColor.welder8 == white)
                        buttonColor.welder8 = red;
                    else
                        buttonColor.welder8 = white;
                    break;

            }
            ProductionButtonColor = buttonColor;
        }
        private async void GetDetailsData()
        {
            try {
                  SelectedBaseMetal1 = SelectedBaseMetal2 = null;
                var returndata = await _eReportsRepository.QueryAsync<T_EReports>(@"SELECT * FROM T_EReports WHERE [ID] = " + _selectedEReportItem.ID);
                _selectedEReportItem = returndata.FirstOrDefault();
                CurrentDWR = JsonConvert.DeserializeObject<DWR>(_selectedEReportItem.JSONString);
                if (CurrentDWR != null)
                {
                    DWRRows = new ObservableCollection<DWRRow>(CurrentDWR.DWRRows);
                }

                SelectedDWRRow = DWRRows.Where(x => x.Number == SelectedDWRRow.Number && x.Spool_No == SelectedDWRRow.Spool_No && x.Joint_No == SelectedDWRRow.Joint_No).FirstOrDefault();

                //load welders
            var _welders = await _weldersRepository.QueryAsync<T_Welders>(@"SELECT * FROM T_Welders");
            var welders = _welders.Select(x => new Welder() { Welder_Name = x.Welder_Name, Project_ID = x.Project_ID, SubContractor = x.SubContractor, Welder_Code = x.Welder_Code }).ToList();
            WeldersList = new ObservableCollection<Welder>(welders);
            WeldersList.Insert(0, new Welder() { Welder_Name = "Select Welder" });
           
            //load wps no.
            var WpsNo = await wpsRepository.QueryAsync<T_WPS_MSTR>(@"SELECT * FROM T_WPS_MSTR");
            var wp = WpsNo.Select(x => new WPS() { Wps_No = x.Wps_No, Description = x.Description, Project_ID = x.Project_ID }).ToList();
            WPSNoList = new ObservableCollection<WPS>(wp);
            WPSNoList.Insert(0, new WPS() { Description = "Select WPS No." });
                if (string.IsNullOrWhiteSpace(SelectedDWRRow.WPS_No))
                {
                    await Task.Delay(100);
                    SelctedWpsNumber = WPSNoList.First();
                }
                else
                {
                    await Task.Delay(100);
                    if(WPSNoList.Where(x => x.Wps_No == SelectedDWRRow.WPS_No).Count() <= 0)
                    {
                        WPS newRow = new WPS
                        {
                            Wps_No = SelectedDWRRow.WPS_No,
                            Description = "",
                            Project_ID = Settings.ProjectID,                             
                        };
                        WPSNoList.Add(newRow);
                    }

                    SelctedWpsNumber = WPSNoList.Where(x => x.Wps_No == SelectedDWRRow.WPS_No).FirstOrDefault();
                }
                //Load Base metal 
                var _baseMetal = await _baseMetalRepository.QueryAsync<T_BaseMetal>(@"SELECT * FROM T_BaseMetal");


            if (_baseMetal.Any())
            {
                    List<T_BaseMetal> _BM2 = new List<T_BaseMetal>();
                    _BM2.AddRange(_baseMetal);

                    if (!String.IsNullOrWhiteSpace(SelectedDWRRow.Base_Metal_1) && SelectedDWRRow.Base_Metal_1 !="")
                    {
                        if(!_baseMetal.Where(x => x.Base_Material == SelectedDWRRow.Base_Metal_1).Any())
                        {
                            _baseMetal.Add(new T_BaseMetal { Base_Material = SelectedDWRRow.Base_Metal_1 });
                        }
                    }
                    if (!String.IsNullOrWhiteSpace(SelectedDWRRow.Base_Metal_2) && SelectedDWRRow.Base_Metal_2 != "")
                    {
                        if (!_BM2.Where(x => x.Base_Material == SelectedDWRRow.Base_Metal_2).Any())
                        {
                            _BM2.Add(new T_BaseMetal { Base_Material = SelectedDWRRow.Base_Metal_2 });
                        }
                    }
                    BaseMetalItemSource1 = new ObservableCollection<T_BaseMetal>(_baseMetal.ToList());
                    BaseMetalItemSource2 = new ObservableCollection<T_BaseMetal>(_BM2.ToList());
                    
                  SelectedBaseMetal1 = BaseMetalItemSource1.Where(x => x.Base_Material == SelectedDWRRow.Base_Metal_1).FirstOrDefault();
                    if(String.IsNullOrWhiteSpace(SelectedDWRRow.Base_Metal_1) && SelectedDWRRow.Base_Metal_1 == "")
                    {
                        BtnMetalText1 = "Manual";
                        ShowBasemetal1 = true;
                    }

                    //SelectedBaseMetal2 = new T_BaseMetal { Base_Material = SelectedDWRRow.Base_Metal_2 };
                    SelectedBaseMetal2 = BaseMetalItemSource2.Where(x => x.Base_Material == SelectedDWRRow.Base_Metal_2).FirstOrDefault();
                    if (String.IsNullOrEmpty(SelectedDWRRow.Base_Metal_2) && SelectedDWRRow.Base_Metal_2 == "")
                    {
                        BtnMetalText2 = "Manual";
                        ShowBasemetal2 = true;
                    }
                }
            else 
            {
                T_BaseMetal metal1 = new T_BaseMetal { Base_Material = SelectedDWRRow.Base_Metal_1 };
                T_BaseMetal metal2 = new T_BaseMetal { Base_Material = SelectedDWRRow.Base_Metal_2 };
                    if (!string.IsNullOrWhiteSpace(metal1.Base_Material) && metal1.Base_Material != "")
                    {
                        BaseMetalItemSource1 = new ObservableCollection<T_BaseMetal>();
                        BaseMetalItemSource1.Add(metal1);
                       await Task.Delay(100);
                        SelectedBaseMetal1 = metal1;   //Where(x => x.Base_Material == metal1.Base_Material).FirstOrDefault();
                    }
                    if (!string.IsNullOrWhiteSpace(metal2.Base_Material) && metal2.Base_Material != "")
                    {
                        BaseMetalItemSource2 = new ObservableCollection<T_BaseMetal>();
                        BaseMetalItemSource2.Add(metal2);
                        await Task.Delay(100);
                        SelectedBaseMetal2 = metal2;    //Where(x => x.Base_Material == metal2.Base_Material).FirstOrDefault();
                    }
                   
                   
            }

            //heat nos.
          //  var fullHeatNo = await _DWR_HeatNosRepository.GetAsync();
           
            HeatNosItemSource1 = new ObservableCollection<T_DWR_HeatNos>();
            HeatNosItemSource2 = new ObservableCollection<T_DWR_HeatNos>();
            string identcode1 = SelectedDWRRow.Ident_Code1;
            string identcode2 = SelectedDWRRow.Ident_Code2;

               // var allHeatNos = await _DWR_HeatNosRepository.QueryAsync<T_DWR_HeatNos>("SELECT * FROM [T_DWR_HeatNos] WHERE [PROJECT_ID] = " + Settings.ProjectID + " ORDER BY [HEAT_NO] ASC");
                List<T_DWR_HeatNos> fullHeatNoArray = new List<T_DWR_HeatNos>();
            if (string.IsNullOrEmpty(identcode1) || string.IsNullOrEmpty(identcode2))
            {
                var  allHeatNos = await _DWR_HeatNosRepository.QueryAsync<T_DWR_HeatNos>("SELECT * FROM [T_DWR_HeatNos] WHERE [PROJECT_ID] = " + Settings.ProjectID  + " ORDER BY [HEAT_NO] ASC");
                fullHeatNoArray = allHeatNos.ToList();
            }

            if (!string.IsNullOrEmpty(SelectedDWRRow.Ident_Code1))
            {
               var  heatnoArray = await _DWR_HeatNosRepository.QueryAsync<T_DWR_HeatNos>("SELECT * FROM [T_DWR_HeatNos] WHERE [PROJECT_ID] = " + Settings.ProjectID + " AND [IDENT_CODE] = '" + identcode1.Replace("'", "''") + "' ORDER BY [HEAT_NO] ASC");
                
                HeatNosItemSource1 = new ObservableCollection<T_DWR_HeatNos>(heatnoArray.ToList());
            }
            else
                HeatNosItemSource1 = new ObservableCollection<T_DWR_HeatNos>(fullHeatNoArray);

            if (!string.IsNullOrEmpty(identcode2))
            {
               var heatnoArray = await _DWR_HeatNosRepository.QueryAsync("SELECT * FROM [T_DWR_HeatNos] WHERE [PROJECT_ID] = " + Settings.ProjectID + " AND [IDENT_CODE] = '" + identcode2.Replace("'", "''") + "' ORDER BY [HEAT_NO] ASC");

                HeatNosItemSource2 =new ObservableCollection<T_DWR_HeatNos> (heatnoArray.ToList());
            }
            else
                HeatNosItemSource2 = new ObservableCollection<T_DWR_HeatNos>(fullHeatNoArray);


            // bind data 
                if (HeatNosItemSource1.Any())
                    SelectedHeatNos1 = HeatNosItemSource1.Where(x => x.Heat_No == SelectedDWRRow.Heat_No_1).FirstOrDefault();

                if (String.IsNullOrWhiteSpace(SelectedDWRRow.Heat_No_1) && SelectedDWRRow.Heat_No_1 == "")
                {
                    BtnHeatNosText1 = "Manual";
                    ShowHeatNos1 = true;
                }

                else
                {
                    T_DWR_HeatNos heatnos = new T_DWR_HeatNos { Heat_No = SelectedDWRRow.Heat_No_1 , Project_ID = Settings.ProjectID};
                    HeatNosItemSource1.Add(heatnos);
                    SelectedHeatNos1 = heatnos;
                }

                if (HeatNosItemSource2.Any())
                    SelectedHeatNos2 = HeatNosItemSource2.Where(x => x.Heat_No == SelectedDWRRow.Heat_No_2).FirstOrDefault();

                if (String.IsNullOrWhiteSpace(SelectedDWRRow.Heat_No_2) && SelectedDWRRow.Heat_No_2 =="")
                {
                    BtnHeatNosText2 = "Manual";
                    ShowHeatNos2 = true;
                }
                else
                {
                    T_DWR_HeatNos heatnos = new T_DWR_HeatNos { Heat_No = SelectedDWRRow.Heat_No_2, Project_ID = Settings.ProjectID };
                    HeatNosItemSource2.Add(heatnos);
                    SelectedHeatNos2 = heatnos;
                }


               


            var data = SelectedDWRRow;
            //loadWelders
            Welder1 = WeldersList.Where(x=>x.Welder_Code == SelectedDWRRow.Root_Welder_1).FirstOrDefault();
            Welder2 = WeldersList.Where(x => x.Welder_Code == SelectedDWRRow.Root_Welder_2).FirstOrDefault();
            Welder3 = WeldersList.Where(x => x.Welder_Code == SelectedDWRRow.Root_Welder_3).FirstOrDefault();
            Welder4 = WeldersList.Where(x => x.Welder_Code == SelectedDWRRow.Root_Welder_4).FirstOrDefault();
            Welder5 = WeldersList.Where(x => x.Welder_Code == SelectedDWRRow.FillCap_Welder_1).FirstOrDefault();
            Welder6 = WeldersList.Where(x => x.Welder_Code == SelectedDWRRow.FillCap_Welder_2).FirstOrDefault();
            Welder7 = WeldersList.Where(x => x.Welder_Code == SelectedDWRRow.FillCap_Welder_3).FirstOrDefault();
            Welder8 = WeldersList.Where(x => x.Welder_Code == SelectedDWRRow.FillCap_Welder_4).FirstOrDefault();

                string white = "#ffffff";
                string red = "#FB1610";
                var ObjWelderPoroduction = (new ProductionButtonColor()
                {
                    welder1 = selectedDWRRow.Root_Welder_1_Production_Joint ? red : white,
                    welder2 = selectedDWRRow.Root_Welder_2_Production_Joint ? red : white,
                    welder3 = selectedDWRRow.Root_Welder_3_Production_Joint ? red : white,
                    welder4 = selectedDWRRow.Root_Welder_4_Production_Joint ? red : white,
                    welder5 = selectedDWRRow.FillCap_Welder_1_Production_Joint ? red : white,
                    welder6 = selectedDWRRow.FillCap_Welder_2_Production_Joint ? red : white,
                    welder7 = selectedDWRRow.FillCap_Welder_3_Production_Joint ? red : white,
                    welder8 = selectedDWRRow.FillCap_Welder_4_Production_Joint ? red : white
                });

            ProductionButtonColor = ObjWelderPoroduction;
            }
            catch (Exception Ex)
            {
            }
        }
        private async void SaveDWR()
        {


        }
        private async void GetSignatureData(int ID)
            {
            var signaturesData = await _signaturesRepository.QueryAsync<T_EReports_Signatures>(@"SELECT * FROM T_EReports_Signatures WHERE [EReportID] = '" + ID + "' ORDER BY [SignatureNo] ASC");

            var UserDetailsList = await _userDetailsRepository.GetAsync();
            if (UserDetailsList.Count > 0)
                userDetail = UserDetailsList.Where(p => p.ID == Settings.UserID).FirstOrDefault();
            SignatureList = new ObservableCollection<T_EReports_Signatures>(signaturesData.Distinct());

          
        }
        private async void GetAttachmentData(int ID)
            {
           
             var AttachmentData = await _drawingsRepository.QueryAsync<T_Drawings>(@"SELECT * FROM T_Drawings WHERE [EReportID] = " + ID);
            AttachmentList = new ObservableCollection<T_Drawings>(AttachmentData.Distinct());

            if (SelectedDWRRow != null)
            {
               SelectedAttachedItem = AttachmentList.Where(x=>x.Name == SelectedDWRRow.Spool_Drawing_No).FirstOrDefault(); //SelectedDWRRow.s
            }
        }
        public async void UpdateSignatureItem()
            {
            T_EReports_Signatures _selectedSignatureItem = new T_EReports_Signatures();
            _selectedSignatureItem = SelectedSignatureItem;

            if (_selectedSignatureItem == null)
                return;

          
            foreach (T_EReports_Signatures CurrentSignture in SignatureList.Where(x => x.SignatureNo == _selectedSignatureItem.SignatureNo && x.SignatureRulesID == _selectedSignatureItem.SignatureRulesID).ToList())
            {

                    //Found, now adjust
                    if (CurrentSignture.VMSigned)
                    {
                    //This has been signed off in VMlive database so cannot change
                    await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "This signature has been signed off in the VMLive database and cannot be changed on the handheld. If you need to remove this signature then remove it in VMLive and re-download this report", "OK");
                    }
                    else
                    {
                        //Check to see if previous sign off has been done
                        Boolean PreviousSigned = false;

                        if (CurrentSignture.SignatureNo == 1)
                            PreviousSigned = true;
                        else
                        {
                            foreach (T_EReports_Signatures PreviousSignature in SignatureList)
                            {
                                if (PreviousSignature.SignatureNo == (CurrentSignture.SignatureNo - 1))
                                {
                                    PreviousSigned = PreviousSignature.Signed;
                                    break;
                                }
                            }
                        }

                        if (PreviousSigned)
                        {
                        //Check to see if you can sign off.

                           var CheckExists = await _usersAssignedRepository.GetAsync(x=>x.EReportID == _selectedEReportItem.ID && x.SignatureRulesID == CurrentSignture.SignatureRulesID && x.UserID == userDetail.ID);
                            if (CheckExists.Any())
                            {
                                //Can Sign Off
                                if (CurrentSignture.Signed)
                                {
                                    //Check to see if next sign off has been completed
                                    Boolean NextSigned = true;

                                    if (CurrentSignture.SignatureNo == SignatureList.Count())
                                        NextSigned = false;
                                    else
                                    {
                                        foreach (T_EReports_Signatures PreviousSignature in SignatureList)
                                        {
                                            if (PreviousSignature.SignatureNo == (CurrentSignture.SignatureNo + 1))
                                            {
                                                NextSigned = PreviousSignature.Signed;
                                                break;
                                            }
                                        }
                                    }

                                    if (NextSigned)
                                    await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "Unable to remove this signature as the following signature has been completed.", "OK");

                                    else
                                    {
                                        //Remove Sign off
                                        CurrentSignture.Signed = false;
                                        CurrentSignture.SignedByFullName = "";
                                        CurrentSignture.SignedByUserID = 0;
                                        CurrentSignture.SignedOn = Convert.ToDateTime("2000-01-01");
                                        CurrentSignture.Updated = true;
                                    }
                                }
                                else
                                {
                                    Boolean CanUpdate = true;
                                if (_selectedEReportItem.ReportType.ToUpper() == "DAILY WELD REPORT" && CurrentSignture.SignatureNo > 1)
                                {
                                    if (CurrentDWR != null)
                                    {
                                        foreach (DWRRow Row in CurrentDWR.DWRRows)
                                        {
                                            if (string.IsNullOrEmpty(Row.VI) || string.IsNullOrEmpty(Row.DI) || string.IsNullOrEmpty(Row.WPS_No) || string.IsNullOrEmpty(Row.Base_Metal_1) || string.IsNullOrEmpty(Row.Heat_No_1) || string.IsNullOrEmpty(Row.Base_Metal_2) || string.IsNullOrEmpty(Row.Heat_No_2))
                                            {
                                                await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "Unable to sign off as joint(s) is incomplete. Please ensure the following are completed for every joint, VI, DI, WPS No, Base Metal 1, Base Metal 2, Heat No 1 and Heat No 2.", "OK");

                                                CanUpdate = false;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "Unable to sign off as joint(s) is incomplete. Please ensure the following are completed for every joint, VI, DI, WPS No, Base Metal 1, Base Metal 2, Heat No 1 and Heat No 2.", "OK");

                                        CanUpdate = false;
                                    }
                                }

                                    if (CanUpdate)
                                    {
                                        //Add details to Sign Off
                                        CurrentSignture.Signed = true;
                                        CurrentSignture.SignedByFullName = CurrentSignture.Signed ? userDetail.FullName : "";
                                        CurrentSignture.SignedByUserID = CurrentSignture.Signed ? userDetail.ID : CurrentSignture.SignedByUserID;
                                        CurrentSignture.SignedOn = DateTime.UtcNow;
                                        CurrentSignture.Updated = true;
                                    }
                                }

                            }
                            else
                            {
                            //Do not have rights for this sign off
                            await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "You currently do not have the user rights to sign off this signature", "OK");

                           
                            }
                        }
                        else
                        await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "Previous sign off must be completed to sign off this signature", "OK");
                    }

                    break;
            }

            SignatureList = new ObservableCollection<T_EReports_Signatures>(SignatureList);
        }
        private async void SaveSignaturesofDWR()
        {
            try
            {

                foreach (T_EReports_Signatures item in SignatureList)
                {
                    await _signaturesRepository.QueryAsync<T_EReports_Signatures>(@"UPDATE T_EReports_Signatures SET [Signed] = " + Convert.ToInt32(item.Signed) + ",[SignedByUserID] = " + item.SignedByUserID
                                                                                    + ",[SignedByFullName] = '" + item.SignedByFullName + "',[SignedOn] = '" + item.SignedOn.Ticks
                                                                                    + "', [Updated] = " + Convert.ToInt32(true) + " WHERE [EReportID] = " + item.EReportID + " AND [SignatureRulesID] = " + item.SignatureRulesID);
                    await _eReportsRepository.QueryAsync<T_EReports>(@"UPDATE T_EReports SET [Updated] = '" + 1 + "' WHERE [ID] = " + _selectedEReportItem.ID);
                }
                _userDialogs.AlertAsync("E-Report Signatures data saved successfully", "Save Signatures Data", "OK");
            }
            catch (Exception ex)
            {

            }
        }
        private async void LoadAttachmentPDF(T_Drawings selectedAttachedItem)
       {
            if (selectedAttachedItem == null || IsRunningTasks)
            {
                return;
            }
            IsRunningTasks = true;
           
            PdfUrl = selectedAttachedItem.FileLocation;
            IsRunningTasks = false;
        }

           #endregion

        #region Public
        public async void NavigateToEDWR_EReporterPage(string Report)
            {
                if (Report == null || IsRunningTasks)
                {
                    return;
                }

                IsRunningTasks = true;
                _userDialogs.ShowLoading("Loading...");
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
                await navigationService.NavigateAsync<DWR_EReporterViewModel>(navigationParameters);
                IsRunningTasks = false;
            }




        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            MainGrid = true;
            CameraGrid = false;
            base.OnNavigatedTo(parameters);
            if (parameters.Count == 0)
            {
                return;
            }

            if (parameters.Count > 1 && parameters.ContainsKey(NavigationParametersConstants.ReportDetailsParameter))
            {              
                _selectedEReportItem = (T_EReports)parameters[NavigationParametersConstants.ReportDetailsParameter];
                // _eReportsRepository.QueryAsync<T_EReports>($@"SELECT * FROM T_EReports WHERE [ModelName] = '" + _selectedUserProject.ModelName + "' AND [ReportType] = '" + _selectedEReportItem + "' ORDER BY [Priority] ASC,[ReportDate] DESC"); 

              //  CurrentProject = UserProjectList.Where(p => p.User_ID == Settings.UserID && p.Project_ID == Settings.ProjectID).FirstOrDefault();
                CurrentDWR = JsonConvert.DeserializeObject<DWR>(_selectedEReportItem.JSONString);
                if (CurrentDWR != null)
                {
                    DWRRows = new ObservableCollection<DWRRow>(CurrentDWR.DWRRows);
                }

                DWRitem = _selectedEReportItem.ReportNo + ", " + _selectedEReportItem.ReportDate.ToString(AppConstant.DateFormat);
            }
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        #endregion


        //new code 
        #region Delegate Commands   
        public ICommand NextBtnCommand
        {
            get
            {
                return new Command<string>(OnClickNextButton);
            }
        }
        #endregion
        public async void OnClickNextButton(string param)
        {
            if (param == "Step1")
            {
                if (IsVisibleList)
                {
                    if (SelectedDWRRow != null)
                    {
                       /* IsVisibleList =*/ IsVisibleRemarkForm = IsVisibleButtons1 =false;
                        IsVisibleList2 = IsVisibleButtons2 = true;

                        var returndata = await _eReportsRepository.QueryAsync<T_EReports>(@"SELECT * FROM T_EReports WHERE [ID] = " + _selectedEReportItem.ID);
                        _selectedEReportItem = returndata.FirstOrDefault();
                        CurrentDWR = JsonConvert.DeserializeObject<DWR>(_selectedEReportItem.JSONString);
                        if (CurrentDWR != null)
                        {
                           // DWRRows = new ObservableCollection<DWRRow>(CurrentDWR.DWRRows);
                        }

                        DWRitem = _selectedEReportItem.ReportNo + ", " + _selectedEReportItem.ReportDate.ToString(AppConstant.DateFormat);

                        ProjectDetails = new ObservableCollection<DWRRow>();
                        SelectedDWRRow = DWRRows.Where(x => x.Number == SelectedDWRRow.Number && x.Spool_No == SelectedDWRRow.Spool_No && x.Joint_No == SelectedDWRRow.Joint_No).FirstOrDefault();
                        ProjectDetails.Add(SelectedDWRRow);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("DWR Reports", " Please Select Report", "OK");

                    }                 
                  
                }
                //else
                //{
                //    IsVisibleList = IsVisibleList2 = IsVisibleButtons2= false;
                //    IsVisibleRemarkForm = true;
                //    GetInspectionDetails();
                //}
               
            }
            else if(param == "Step2")
            {
                IsVisibleList = IsVisibleList2 = IsVisibleButtons2 = false;
                IsVisibleRemarkForm = true;
                GetInspectionDetails();
            }
            //else
            //{
            //    IsVisibleList = true;
            //    IsVisibleList2 = IsVisibleRemarkForm = IsVisibleButtons1= false;
            //}

        }
    }
}

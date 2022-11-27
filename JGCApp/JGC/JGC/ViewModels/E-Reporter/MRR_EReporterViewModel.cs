using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.Common.Extentions;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using JGC.UserControls.PopupControls;
using JGC.ViewModels.E_Reporter;
using JGC.Views.E_Reporter;

namespace JGC.ViewModels
{
    public class MRR_EReporterViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IRepository<T_EReports> _eReportsRepository;
        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private readonly IRepository<T_EReports_Signatures> _signaturesRepository;
        private readonly IRepository<T_Drawings> _drawingsRepository;
        private readonly IRepository<T_StorageAreas> _StorageAreasRepository;
        private readonly IRepository<T_PartialRequest> _PartialRequest;
        private readonly IRepository<T_EReports_UsersAssigned> _usersAssignedRepository;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IResizeImageService _resizeImageService;
        private T_EReports _selectedEReportItem;
        private readonly IMedia _media;
        private T_UserDetails userDetail;        
        private string MRRPath;
        private byte[] imageAsByte = null;
        MRR CurrentMRR = new MRR();
        private T_UserDetails OtherUser;

        #region Properties  
        private ObservableCollection<MRRRow> _mrrRows;
        public ObservableCollection<MRRRow> MRRRows
        {
            get { return _mrrRows; }
            set { _mrrRows = value; RaisePropertyChanged(); }
        }
       
        private MRRRow selectedMRRRows;
        public MRRRow SelectedMRRRows
        {
            get { return selectedMRRRows; }
            set
            {
                if (SetProperty(ref selectedMRRRows, value))
                {
                   // ShowDescriptionPopup();
                    OnPropertyChanged();
                }
            }
        }
      
        private ObservableCollection<T_EReports_Signatures> _signatureList;
        public ObservableCollection<T_EReports_Signatures> SignatureList
        {
            get { return _signatureList; }
            set { _signatureList = value; RaisePropertyChanged("SignatureList"); OnPropertyChanged(); }
        }

        private ObservableCollection<T_Drawings> _attachmentList;
        public ObservableCollection<T_Drawings> AttachmentList
        {
            get { return _attachmentList; }
            set { _attachmentList = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<MRRStorageAreas> storageAreas;
        public ObservableCollection<MRRStorageAreas> StorageAreas
        {
            get { return storageAreas; }
            set { storageAreas = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<MRRHeatNos> heatNos;
        public ObservableCollection<MRRHeatNos> HeatNos
        {
            get { return heatNos; }
            set { heatNos = value; RaisePropertyChanged(); }
        }
        private MRRStorageAreas selectedStorageArea;
        public MRRStorageAreas SelectedStorageArea
        {
            get { return selectedStorageArea; }
            set
            {
                SetProperty(ref selectedStorageArea, value);                
            }

        }
        private MRRHeatNos selectedHeatNo;
        public MRRHeatNos SelectedHeatNo
        {
            get { return selectedHeatNo; }
            set
            {
                SetProperty(ref selectedHeatNo, value);
            }
        }
        private ObservableCollection<string> ddlstorageAreas;
        public ObservableCollection<string> ddlStorageAreas
        {
            get { return ddlstorageAreas; }
            set { ddlstorageAreas = value; RaisePropertyChanged(); }
        }
        //CameraItems
        private string _item;
        public string MRRitem
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }
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
        
        private bool _btnSaveEdit;
        public bool btnSaveEdit
        {
            get { return _btnSaveEdit; }
            set { SetProperty(ref _btnSaveEdit, value); }
        }
        private bool _btnRPR;
        public bool btnRPR
        {
            get { return _btnRPR; }
            set { SetProperty(ref _btnRPR, value); }
        }
        private bool _cameraIsVisible;
        public bool cameraIsVisible
        {
            get { return _cameraIsVisible; }
            set { SetProperty(ref _cameraIsVisible, value); }
        }
        private string _acceptBGColor;
        public string AcceptBGColor
        {
            get { return _acceptBGColor; }
            set { SetProperty(ref _acceptBGColor, value); }
        }
        private string _rejectBGColor;
        public string RejectBGColor
        {
            get { return _rejectBGColor; }
            set { SetProperty(ref _rejectBGColor, value); }
        }
        
        private string _acceptTextColor;
        public string AcceptTextColor
        {
            get { return _acceptTextColor; }
            set { SetProperty(ref _acceptTextColor, value); }
        }
        private string _rejectTextColor;
        public string RejectTextColor
        {
            get { return _rejectTextColor; }
            set { SetProperty(ref _rejectTextColor, value); }
        }
        
        private bool _detailsGrid;
        public bool DetailsGrid
        {
            get { return _detailsGrid; }
            set { SetProperty(ref _detailsGrid, value); }
        }
        private bool _signaturesGrid;
        public bool SignaturesGrid
        {
            get { return _signaturesGrid; }
            set { SetProperty(ref _signaturesGrid, value); }
        }
        private bool _attachmentsGrid;
        public bool AttachmentsGrid
        {
            get { return _attachmentsGrid; }
            set { SetProperty(ref _attachmentsGrid, value); }
        }
        private bool _mainGrid;
        public bool MainGrid
        {
            get { return _mainGrid; }
            set { SetProperty(ref _mainGrid, value); }
        }
        private bool _cameraGrid;
        public bool CameraGrid
        {
            get { return _cameraGrid; }
            set { SetProperty(ref _cameraGrid, value); }
        }
        private bool _editMRRGrid;
        public bool EditMRRGrid
        {
            get { return _editMRRGrid; }
            set { SetProperty(ref _editMRRGrid, value); }
        }        
        private bool _storageAreaGrid;
        public bool StorageAreaGrid
        {
            get { return _storageAreaGrid; }
            set { SetProperty(ref _storageAreaGrid, value); }
        }
        private bool _heatNoGrid;
        public bool HeatNoGrid
        {
            get { return _heatNoGrid; }
            set { SetProperty(ref _heatNoGrid, value); }
        }
        private bool _btnSave;
        public bool btnSave
        {
            get { return _btnSave; }
            set { SetProperty(ref _btnSave, value); }
        }       
        private string _colorSaveNext;
        public string ColorSaveNext
        {
            get { return _colorSaveNext; }
            set { SetProperty(ref _colorSaveNext, value); }
        }
        
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { SetProperty(ref _remark, value); }
        }
        private string _remark2;
        public string Remark2
        {
            get { return _remark2; }
            set { SetProperty(ref _remark2, value); }
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

        private ObservableCollection<string> _ImageFiles;
        public ObservableCollection<string> ImageFiles
        {
            get { return _ImageFiles; }
            set { _ImageFiles = value; RaisePropertyChanged(); }
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
        private string pdfUrl;
        public string PdfUrl
        {
            get { return pdfUrl; }
            set
            {
                SetProperty(ref pdfUrl, value);
            }
        }

        private string btnName;
        public string BtnName
        {
            get { return btnName; }
            set
            {
                SetProperty(ref btnName, value);
            }
        }
        private string good;
        public string Good
        {
            get { return good; }
            set
            {
                SetProperty(ref good, value);
                if (good != null)
                    CalculateShortAndOver();
            }
        }

        private string damage;
        public string Damage
        {
            get { return damage; }
            set
            {
                SetProperty(ref damage, value);
                if (damage != null)
                    CalculateShortAndOver();
            }
        }

       

        private string offSpec;
        public string Off_Spec
        {
            get { return offSpec; }
            set
            {
                SetProperty(ref offSpec, value);
                if (offSpec != null)
                    CalculateShortAndOver();
            }
        }
        private string _short;
        public string Short
        {
            get { return _short; }
            set
            {
                SetProperty(ref _short, value);
            }
        }
        private string over;
        public string Over
        {
            get { return over; }
            set
            {
                SetProperty(ref over, value);
            }
        }
        
        private string receivedQuantity;
        public string ReceivedQuantity
        {
            get { return receivedQuantity; }
            set
            {
                SetProperty(ref receivedQuantity, value);
            }
        }
        private string poNo;
        public string PONo
        {
            get { return poNo; }
            set
            {
                SetProperty(ref poNo, value);
            }
        }
        private string PTNo;
        public string PTNO
        {
            get { return PTNo; }
            set
            {
                SetProperty(ref PTNo, value);
            }
        }
        private string selectedStorageAreas;
        public string SelectedStorageAreas
        {
            get { return selectedStorageAreas; }
            set
            {
                SetProperty(ref selectedStorageAreas, value);
                OnPropertyChanged("SelectedStorageAreas");
            }
        }
        private string heatNo;
        public string HeatNo
        {
            get { return heatNo; }
            set
            {
                SetProperty(ref heatNo, value);
            }
        }
        private string storagGQty;
        public string StoragGQty
        {
            get { return storagGQty; }
            set
            {
                SetProperty(ref storagGQty, value);
            }
        }
        private string heatGQty;
        public string HeatGQty
        {
            get { return heatGQty; }
            set
            {
                SetProperty(ref heatGQty, value);
            }
        }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (SetProperty(ref searchText, value))
                {
                    Search(searchText);
                    OnPropertyChanged();
                }
            }
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
        private bool showbuttons;
        public bool Showbuttons
        {
            get { return showbuttons; }
            set
            {
                SetProperty(ref showbuttons, value);
            }
        }
        private bool accept;
        public bool Accept
        {
            get { return accept; }
            set
            {
                SetProperty(ref accept, value);
            }
        }
        private bool reject;
        public bool Reject
        {
            get { return reject; }
            set
            {
                SetProperty(ref reject, value);
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
        private bool isVisiblePdfFull;
        public bool IsVisiblePdfFull
        {
            get { return isVisiblePdfFull; }
            set { SetProperty(ref isVisiblePdfFull, value); }
        }
        private bool showHideStorageArea;
        public bool ShowHideStorageArea
        {
            get { return showHideStorageArea; }
            set { SetProperty(ref showHideStorageArea, value); }
        }
        private string editDoneText;
        public string EditDoneText
        {
            get { return editDoneText; }
            set { SetProperty(ref editDoneText, value); }
        }
        private string editStorageArea;
        public string EditStorageArea
        {
            get { return editStorageArea; }
            set
            {
                SetProperty(ref editStorageArea, value);
            }
        }

        private string showPDFs;
        public string ShowPDFs
        {
            get { return showPDFs; }
            set
            {
                SetProperty(ref showPDFs, value);
            }
        }

        private string showImages;
        public string ShowImages
        {
            get { return showImages; }
            set
            {
                SetProperty(ref showImages, value);
            }
        }
        private bool pdfview;
        public bool PDFview
        {
            get { return pdfview; }
            set
            {
                SetProperty(ref pdfview, value);
            }
        }
        private bool imagesview;
        public bool ImagesView
        {
            get { return imagesview; }
            set
            {
                SetProperty(ref imagesview, value);
            }
        }

        private ImageSource _attachmentImage;
        public ImageSource AttachmentImage
        {
            get
            {
                return _attachmentImage;
            }
            set { SetProperty(ref _attachmentImage, value); }
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

        public MRR_EReporterViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            IMedia _media,
            ICheckValidLogin _checkValidLogin,
            IResizeImageService _resizeImageService,
            IRepository<T_UserDetails> _userDetailsRepository,
            IRepository<T_EReports> _eReportsRepository,
            IRepository<T_EReports_Signatures> _signaturesRepository,
            IRepository<T_Drawings> _drawingsRepository,
            IRepository<T_StorageAreas> _StorageAreasRepository,
            IRepository<T_PartialRequest> _PartialRequest,
            IRepository<T_EReports_UsersAssigned> _usersAssignedRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._media = _media;
            this._checkValidLogin = _checkValidLogin;
            this._resizeImageService = _resizeImageService;
            this._userDialogs = _userDialogs;
            this._eReportsRepository = _eReportsRepository;
            this._signaturesRepository = _signaturesRepository;
            this._drawingsRepository = _drawingsRepository;
            this._userDetailsRepository = _userDetailsRepository;
            this._StorageAreasRepository = _StorageAreasRepository;
            this._PartialRequest = _PartialRequest;
            this._usersAssignedRepository = _usersAssignedRepository;
            _media.Initialize();
            DetailsArrow = DetailsGrid = MainGrid = Showbuttons = true;
            IsVisiblePdfFull = false;
            PageHeaderText = "Material Receiving Report";
            _userDialogs.HideLoading();
            ShowPDFs = "Greenradio.png";
            ShowImages = "Grayradio.png";
            PDFview = true;
            ImagesView = false;
        }

        #region Private       
        private async void OnClickButton(string param)
        {
            DetailsArrow = SignaturesArrow = AttachmentsArrow = false;
            if (param == "Details" || param == "BacktoDetails")
            {
                _userDialogs.ShowLoading("Loading...");
                DetailsArrow = DetailsGrid = EditMRRGrid = DetailsGrid = btnRPR = btnSaveEdit = cameraIsVisible = !DetailsArrow;
                SignaturesGrid = AttachmentsGrid = EditMRRGrid = btnSave = !DetailsGrid;
                btnSaveEdit = btnRPR = cameraIsVisible = false;
                BtnName = "Edit";
                GetDetailsData();
                _userDialogs.HideLoading();
            }
            else if (param == "Signatures")
            {
                _userDialogs.ShowLoading("Loading...");
                SignaturesArrow = SignaturesGrid = !SignaturesArrow;
                DetailsGrid = AttachmentsGrid = EditMRRGrid = !SignaturesGrid;
                GetSignatureData(_selectedEReportItem.ID);
                btnSaveEdit = true;
                btnRPR= cameraIsVisible = btnSave = false;
                BtnName = "Save";
                _userDialogs.HideLoading();
            }
            else if (param == "Attachments")
            {
                _userDialogs.ShowLoading("Loading...");
                AttachmentsArrow = AttachmentsGrid = !AttachmentsArrow;
                DetailsGrid = SignaturesGrid = EditMRRGrid = !AttachmentsGrid;
                btnSaveEdit = btnSave = btnRPR = cameraIsVisible = false;
                GetAttachmentData(_selectedEReportItem.ID);
                _userDialogs.HideLoading();
            }
            else if (param == "Camera")
            {
                _userDialogs.ShowLoading("Loading...");
                generatepath();
                btnSaveDelete = "Save";
                RenameImage = false;
                CameraItems = new List<string> { "USB2.0_Camera", "USB2.0_Camera 1" };
               // ImageFiles = new List<string> { "Image File Name", "Image File Name 1" };
                MainGrid = !MainGrid;
                CameraGrid = !MainGrid;
                _userDialogs.HideLoading();
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
                   await _userDialogs.AlertAsync("Please enter new name", null, "Ok");
            }
            else if (param == "Back")
            {
                CapturedImage = "";
                CameraGrid = !CameraGrid;
                MainGrid = !CameraGrid;
            }
            else if (param == "Accept" || param == "Reject" || param == "Accept_Reject")
            {
                DetailsArrow = true;
                if (param == "Accept")
                {
                    Accept = true;
                    Reject = !Accept;
                }
                else if(param == "Reject")
                {
                    Reject = true;
                    Accept = !Reject;
                }
                else
                {
                  if(Reject)
                    Accept = !Reject;
                }
                changebuttoncolors();
            }
           
            else if (param == "Save")
            {
                if (EditMRRGrid)
                {
                    DetailsArrow = true;
                    SaveDetailsofMRR();
                }
                else if (SignaturesGrid)
                {
                    SignaturesArrow = true;
                    SaveSignaturesofMRR();
                }
            }
            else if (param == "Edit")
            {
                DetailsArrow = true;
                if (SelectedMRRRows == null)
                    return;               
                BindMRRDataFormEdit(SelectedMRRRows);
                DetailsGrid = btnRPR = btnSaveEdit = cameraIsVisible = !DetailsGrid;
                EditMRRGrid = DetailsArrow = btnSave = !DetailsGrid;
                ColorSaveNext = "#FB1610";
            }
            else if (param == "RPR")
            {
                CheckRPR();
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
                RenameImage = false;
                SelectedImageFiles = SelectedCameraItem = null;
                SelectedCameraItems(_selectedCameraItem);
            }
            else if (param == "AddStorageArea" || param == "AddHeat")
            {
                if(param == "AddStorageArea")
                {
                    if (SelectedStorageAreas != null)
                    {
                        if (StoragGQty != null && StringToDoubleCheck(StoragGQty))
                        {
                            MRRStorageAreas SA = new MRRStorageAreas();
                            SA.Storage_Area = SelectedStorageAreas;
                            SA.Good_Quantity = Convert.ToDouble(StoragGQty);
                            StorageAreas.Add(SA);
                            Good = string.Format("{0:N3}", CalculateMRRSStorageAreaListQty(StorageAreas)); 
                            SelectedStorageAreas = null;
                            StoragGQty = null;
                        }
                        else
                            await _userDialogs.AlertAsync("Storage quantity is invalid, please ensure a storage area is provided and the value provided is numeric.", "Add Storage Area", "Ok");
                    }
                    else
                        await _userDialogs.AlertAsync("Storage area must be selected from the drop down", "Add Storage Area", "Ok");
                }
                else
                {
                    if (HeatNo != null && HeatGQty != null && StringToDoubleCheck(HeatGQty))
                        {
                            MRRHeatNos HN = new MRRHeatNos();
                            HN.Heat_No = HeatNo;
                            HN.Quantity = Convert.ToDouble(HeatGQty);
                            HeatNos.Add(HN);
                            HeatNo = null;
                            HeatGQty = null;
                        }
                        else
                            await _userDialogs.AlertAsync("Heat No is invalid, please ensure a storage area is provided and the value provided is numeric.", "Add Heat No", "Ok");                   
                }
            }
            else if (param == "StorageAreaGrid")
            {
                SelectedStorageAreas = null;
                StoragGQty = null;
                StorageAreaGrid = true;
                EditDoneText = "Edit";
                ShowHideStorageArea = true;
                MainGrid = false;
            }
            else if(param == "EditDoneStorageArea")
            {
                if(EditDoneText == "Edit")
                {                    
                    EditDoneText = "Done";
                    EditStorageArea = SelectedStorageAreas;
                    ShowHideStorageArea = false;                 
                }
                else if(EditDoneText == "Done")
                {
                    if (SelectedStorageAreas == null )
                    {
                        if (!String.IsNullOrWhiteSpace(EditStorageArea) )
                        {
                            string sql = "INSERT INTO [T_StorageAreas] ([Project_ID],[Store_Location],[Storage_Area_Code],[Sheet_No]) VALUES "
                                       + "('" + Settings.ProjectID + "', '" + CurrentMRR.Store_Location + "','" + EditStorageArea + "','')";
                            await _StorageAreasRepository.QueryAsync<T_StorageAreas>(sql);

                            await _userDialogs.AlertAsync("Added successfully.", null, "Ok");
                        }
                        GetddlStorageAreasData();                       
                    }
                    else
                    {
                        var data = await _StorageAreasRepository.QueryAsync<T_StorageAreas>(@"SELECT * FROM [T_StorageAreas] WHERE [Storage_Area_Code] = '" + SelectedStorageAreas + "'");
                        T_StorageAreas SAData = data.FirstOrDefault();


                        string sqlupdate = @"Update T_StorageAreas SET [Storage_Area_Code] = '" + EditStorageArea + "' WHERE  [Project_ID] = '" +
                               SAData.Project_ID + "' AND [Store_Location] = '" + SAData.Store_Location + "' AND [Storage_Area_Code] = '" + SelectedStorageAreas + "'";

                         await _StorageAreasRepository.QueryAsync<T_StorageAreas>(sqlupdate);

                        GetddlStorageAreasData();
                        SelectedStorageAreas = EditStorageArea;
                        await _userDialogs.AlertAsync("Updated successfully.", null, "Ok");
                    }

                    EditDoneText = "Edit";
                    ShowHideStorageArea = true;
                }
            }
            else if (param == "BackfromStorageGrid")
            {
                StorageAreaGrid = false;
                MainGrid = DetailsArrow = true;
            }            
            else if (param == "HeatNoGrid")
            {
                HeatNo = null;
                HeatGQty = null;
                HeatNoGrid = true;               
                MainGrid = false;
            }
            else if (param == "BackfromHeatGrid")
            {
                HeatNoGrid = false;
                MainGrid = DetailsArrow = true;
            }
            else if (param == "PdfFullScreen")
            {
                if (PDFview)
                {
                    PdfUriModel pdfuri = new PdfUriModel();
                    pdfuri.PdfUriPath = PdfUrl;

                    var navigationParameters = new NavigationParameters();
                    navigationParameters.Add("uri", pdfuri);
                    navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
                    await navigationService.NavigateAsync<PDFvieverViewModel>(navigationParameters);
                }
                else if (ImagesView)
                {
                    await _userDialogs.AlertAsync("Please Select PDF", null, "Ok");
                }

            }
            else if (param == "ClosePdfFullScreen")
            {
                IsVisiblePdfFull = false;
                MainGrid = true;
            }
            if (param == "ShowPDFs")
            {
                ShowPDFs = "Greenradio.png";
                ShowImages = "Grayradio.png";
                PDFview = true;
                ImagesView = false;
                //GetDrawingList(true);
                //if (CurrentDrawings != null)
                //    SelectedDrawings = DrawingsList.Where(i => i.Name == CurrentDrawings.Name).Select(i => i).FirstOrDefault();
                var AttachmentData = await _drawingsRepository.QueryAsync<T_Drawings>(@"SELECT * FROM T_Drawings WHERE [EReportID] = " + _selectedEReportItem.ID);
                AttachmentList = new ObservableCollection<T_Drawings>(AttachmentData.Distinct().Where(x => x.FileName.Trim().Contains("pdf")));
            }
            if (param == "ShowImages")
            {
                ShowImages = "Greenradio.png";
                ShowPDFs = "Grayradio.png";
                ImagesView = true;
                PDFview = false;
                var AttachmentData = await _drawingsRepository.QueryAsync<T_Drawings>(@"SELECT * FROM T_Drawings WHERE [EReportID] = " + _selectedEReportItem.ID);
                AttachmentList = new ObservableCollection<T_Drawings>(AttachmentData.Distinct().Where(x => !x.FileName.Trim().Contains("pdf")));
                //  GetDrawingList(false);
            }


        }
        private async void CaptureImageSave()
        {
            if (imageAsByte != null)
            {
                string path = await DependencyService.Get<ISaveFiles>().SavePictureToDisk(MRRPath, DateTime.Now.ToString(AppConstant.CameraDateFormat), imageAsByte.ToArray());
                if (path != null)
                {
                    generatepath();
                    RenameImage = false;
                    SelectedImageFiles = SelectedCameraItem = null;
                    await _eReportsRepository.QueryAsync<T_EReports>(@"UPDATE T_EReports SET [Updated] = '" + 1 + "' WHERE [ID] = " + _selectedEReportItem.ID);
                    await _userDialogs.AlertAsync("Saved successfully", null, "Ok");
                }
            }
            else
                _userDialogs.AlertAsync("Please select camera and take a picture to save", null, "OK");

        }
        private async void CaptureImageDelete()
        {
            if (await _userDialogs.ConfirmAsync($"Are you sure you want to delete?", $"Delete image", "Yes", "No"))
            {
                bool isdelete = await DependencyService.Get<ISaveFiles>().DeleteImage(MRRPath + "/" + SelectedImageFiles);
                if (isdelete)
                {
                    ImageFiles = new ObservableCollection<string>(await DependencyService.Get<ISaveFiles>().GetAllImages(MRRPath));
                    btnSaveDelete = "Save";
                    RenameImage = false;
                    CapturedImage = NewImageName = "";
                }
            }
        }
        private async void generatepath()
        {
            string path = ("Photo Store" + "\\" + CurrentMRR.JobCode + "\\" + _selectedEReportItem.ID.ToString() + "\\" + "PO Item");
            MRRPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath(path);

            ImageFiles = new ObservableCollection<string>(await DependencyService.Get<ISaveFiles>().GetAllImages(MRRPath));
            if (ImageFiles.Count > 0)
                CameraIcon = "Greencam.png";
            else
                CameraIcon = "cam.png";
        }
        private async void SaveRenameImage()
        {
            try
            {
                if (await DependencyService.Get<ISaveFiles>().RenameImage(MRRPath, SelectedImageFiles, NewImageName + ".jpg"))
                {
                    ImageFiles = new ObservableCollection<string>(await DependencyService.Get<ISaveFiles>().GetAllImages(MRRPath));
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
        private async void LoadAttachmentPDF(T_Drawings selectedAttachedItem)
        {
            if (selectedAttachedItem == null || IsRunningTasks)
            {
                return;
            }
            if (selectedAttachedItem.FileName.Trim().Contains("pdf"))
            {
                IsRunningTasks = true;
                // byte[] PDFBytes = Convert.FromBase64String(selectedAttachedItem.BinaryCode);
                PdfUrl = selectedAttachedItem.FileLocation;
                IsRunningTasks = false;
            }
            else
            {
                IsRunningTasks = true;
                // byte[] PDFBytes = Convert.FromBase64String(selectedAttachedItem.BinaryCode);
                // PdfUrl = selectedAttachedItem.FileLocation;
                AttachmentImage = await DependencyService.Get<ISaveFiles>().GetImage(selectedAttachedItem.FileLocation);
                IsRunningTasks = false;
            }
        }
        private async void changebuttoncolors()
        {
            AcceptTextColor = RejectTextColor = "black";
            AcceptBGColor = RejectBGColor = "White";
            if (Accept)
            {
                AcceptBGColor = "#99CCFF";
                AcceptTextColor = "White";
            }
            if (Reject)
            {
                RejectBGColor = "Red";
                RejectTextColor = "White";
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
            CapturedImage = await DependencyService.Get<ISaveFiles>().GetImage(MRRPath + "/" + SelectedImageFiles);
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
                btnSaveDelete = "Save";
                RenameImage = false;
            }
            catch (Exception ex)
            {

            }
        }
        private async void PickImagesFromGallery()
        {
            try
            {
                var PickFile = await DependencyService.Get<ISaveFiles>().PickFile(MRRPath);
                ImageFiles = new ObservableCollection<string>(await DependencyService.Get<ISaveFiles>().GetAllImages(MRRPath));
                if (PickFile)
                {
                    await _eReportsRepository.QueryAsync<T_EReports>(@"UPDATE T_EReports SET [Updated] = '" + 1 + "' WHERE [ID] = " + _selectedEReportItem.ID);
                    await _userDialogs.AlertAsync("Added image file successfully", "Saved Image", "OK");
                }
                //var mediaFile = await PickFileAsync();
                //if (mediaFile == null)
                //{
                //    return;
                //}
                //var memoryStream = new MemoryStream();
                //await mediaFile.GetStream().CopyToAsync(memoryStream);
                //imageAsByte = null;
                //imageAsByte = memoryStream.ToArray();
                //Stream stream = new MemoryStream(imageAsByte);
                //CapturedImage = ImageSource.FromStream(() => stream);
                //btnSaveDelete = "Save";
                //RenameImage = false;
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
        private async void GetDetailsData()
        {
            if (_selectedEReportItem.JSONString != string.Empty)
            {
                CurrentMRR = JsonConvert.DeserializeObject<MRR>(_selectedEReportItem.JSONString);
                if (CurrentMRR.MRRRows != null)
                {
                    MRRRows = new ObservableCollection<MRRRow>(CurrentMRR.MRRRows);
                    btnSaveEdit = btnRPR = cameraIsVisible = true;
                    BtnName = "Edit";
                }
                // GetSignatureDateData(_selectedEReportItem.ID);                
              //  CameraIcon = "cam.png";
              
            }
            MRRitem = _selectedEReportItem.ReportNo + ", " + _selectedEReportItem.ReportDate.ToString(AppConstant.DateFormat) + ", Ship No. " + CurrentMRR.Ship_No;
            generatepath();
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
            // var gata = await _drawingsRepository.GetAsync();
            var AttachmentData = await _drawingsRepository.QueryAsync<T_Drawings>(@"SELECT * FROM T_Drawings WHERE [EReportID] = " + ID);
            AttachmentList = new ObservableCollection<T_Drawings>(AttachmentData.Distinct().Where(x => x.FileName.Trim().Contains("pdf")));
        }
        private async void SaveDetailsofMRR()
        {
            Boolean CanSave = true;
            double shippedValue = SelectedMRRRows.Shipped_Qty, goodValue = 0, damageValue = 0, offspecValue = 0, shortValue = 0, overValue = 0;

            foreach (string currentvalue in new List<string> { Damage, Off_Spec, Short, Over })
            {
                if (!StringToDoubleCheck(currentvalue))
                {
                    //if cant convert then cant save, display error message.
                    CanSave = false;
                    await _userDialogs.AlertAsync("Not all received quantities are numeric please adjust.", "Save Item","Ok");
                    break;
                }
            }
            if (CanSave)
            {
                goodValue = Convert.ToDouble(Good);
                damageValue = Convert.ToDouble(Damage);
                offspecValue = Convert.ToDouble(Off_Spec);
                shortValue = Convert.ToDouble(Short);
                overValue = Convert.ToDouble(Over);
                if (shortValue > 0)
                {
                    if (!await _userDialogs.ConfirmAsync($"Material Shortage: "+ shortValue + ", are you sure you want to save?", $"", "Ok", "Cancel"))
                        return;
                }
                else if (overValue > 0) 
                {
                    if (!await _userDialogs.ConfirmAsync($"Material over: " + overValue + ", are you sure you want to save?", $"", "Ok", "Cancel"))
                        return;
                }

            }

            ////Received Quantity JGC checks
            //if (CanSave)
            //{
            //    CanSave = false;
                
            //    if (SelectedMRRRows.Shipped_Qty == (goodValue + damageValue + offspecValue + shortValue))
            //        CanSave = true;
            //    else
            //    {
            //        if (SelectedMRRRows.Shipped_Qty < (goodValue + damageValue + offspecValue + shortValue))
            //        {
            //            CanSave = false;
            //           await _userDialogs.AlertAsync("Received quantities are greater then the shipped quantity.", "Save Item","Ok");
            //        }
            //        else
            //        {
            //            if (SelectedMRRRows.Shipped_Qty > (goodValue + damageValue + offspecValue + shortValue)) 
            //                CanSave = true;
            //            else
            //            {
            //                if (SelectedMRRRows.Shipped_Qty < (goodValue + damageValue + offspecValue + overValue))
            //                    CanSave = true;
            //                else
            //                {
            //                    if (SelectedMRRRows.Shipped_Qty > (goodValue + damageValue + offspecValue + overValue))
            //                    {
            //                        CanSave = false;
            //                        await _userDialogs.AlertAsync("Received quantities (Good,Damage,Off Spec., Over) are greater then the shipped quantity.", "Save Item", "Ok");
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}


            ////Heat Nos Check

            //if (CanSave)
            //{
            //    double totalQuantity = 0;

            //    if (HeatNos != null)
            //    {
            //        foreach (MRRHeatNos heatNo in HeatNos)
            //        {
            //            totalQuantity = totalQuantity + heatNo.Quantity;
            //        }

            //        if (Math.Round(totalQuantity, 3, MidpointRounding.AwayFromZero) > Math.Round(shippedValue, 3, MidpointRounding.AwayFromZero))
            //        {
            //            CanSave = false;
            //            await _userDialogs.AlertAsync("Total heat no. quantity is greater then the shipped quantity.", "Save Item", "Ok");
            //        }
            //    }
            //}

            ////Storage Area Codes
            //if (CanSave)
            //{
            //    double totalQuantity = 0;

            //    if (StorageAreas != null)
            //    {
            //        foreach (MRRStorageAreas storageArea in StorageAreas)
            //        {
            //            totalQuantity = totalQuantity + storageArea.Good_Quantity;
            //        }

            //        if (totalQuantity > shippedValue)
            //        {
            //            CanSave = false;
            //            await _userDialogs.AlertAsync("Total storage area quantity is greater then the shipped quantity.", "Save Item", "Ok");
            //        }
            //    }
            //}

            if (CanSave)
            {
                SelectedMRRRows.Good = goodValue;
                SelectedMRRRows.Damage = damageValue;
                SelectedMRRRows.Off_Spec = offspecValue;
                SelectedMRRRows.Short = shortValue;
                SelectedMRRRows.Over = overValue;
                SelectedMRRRows.Remarks_1 = Remark;
                SelectedMRRRows.Remarks_2 = Remark2;
                SelectedMRRRows.Client_Accepted = Accept;
                SelectedMRRRows.Client_Rejected = Reject;
                SelectedMRRRows.Storage_Area_Codes = StorageAreas.ToList();
                SelectedMRRRows.Heat_Nos = HeatNos.ToList();
                SelectedMRRRows.Updated = true;

                string JSONString = ModsTools.ToJson(CurrentMRR);
                await _eReportsRepository.QueryAsync<T_EReports>(@"UPDATE T_EReports SET [JSONString] = '" + JSONString.Replace("'", "''") + "' , [Updated] = '" + 1 + "' WHERE [ID] = " + _selectedEReportItem.ID);
                var returndata = await _eReportsRepository.QueryAsync<T_EReports>(@"SELECT * FROM T_EReports WHERE [ID] = " + _selectedEReportItem.ID);
                _selectedEReportItem = returndata.FirstOrDefault();
                await _userDialogs.AlertAsync("Inspection data saved successfully", "Save Inspection Data", "OK");

                OnClickButton("Details");
            }
        }
        public async void UpdateSignatureItem(bool IsLogedInUser)
        {

            if (!SelectedSignatureItem.Signed && SelectedSignatureItem.DisplaySignatureName == "MCS Data Processor/Storekeeper")
            {
                if (!await _userDialogs.ConfirmAsync($"There are Over/Short quantities in this report, are you sure you want to sign off instead of partial receiving?", $"", "Ok", "Cancel"))
                    return;
            }
            T_UserDetails signeduser = new T_UserDetails();

            if (IsLogedInUser)
                signeduser = userDetail;
            else
                signeduser = OtherUser;

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

                        var CheckExists = await _usersAssignedRepository.GetAsync(x => x.EReportID == _selectedEReportItem.ID && x.SignatureRulesID == CurrentSignture.SignatureRulesID && x.UserID == signeduser.ID);
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
                              

                                if (CurrentSignture.SendToDataHub && _selectedEReportItem.ReportType.ToUpper() == "MATERIAL RECEIVING REPORT")
                                {
                                    MRR currentMRR = new MRR();
                                   
                                    string jsonString = _selectedEReportItem.JSONString;

                                    if (jsonString != string.Empty)
                                    {
                                        bool quantityIssue = false;
                                        currentMRR = JsonConvert.DeserializeObject<MRR>(jsonString);

                                        if (currentMRR.MRRRows != null)
                                        {
                                            foreach (MRRRow row in currentMRR.MRRRows)
                                            {
                                                double totaladded = row.Good + row.Damage + row.Off_Spec + row.Short;

                                                if (Math.Round(row.Shipped_Qty, row.Shipped_Qty.ToString().Substring(row.Shipped_Qty.ToString().IndexOf(".") + 1).Length, MidpointRounding.AwayFromZero) !=
                                                            Math.Round(totaladded, totaladded.ToString().Substring(totaladded.ToString().IndexOf(".") + 1).Length, MidpointRounding.AwayFromZero))
                                                {
                                                    quantityIssue = true;
                                                    break;
                                                }
                                            }
                                        }

                                        if (quantityIssue)
                                        {
                                            CanUpdate = false;
                                            await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "To sign this signature please make sure the total of 'Good Quantity','Damaged Quantity','Off Spec Quantity' and 'Short Quantity' is equal 'Shipped Quantity', for all items.", "OK");

                                        }
                                    }
                                }


                                if (CanUpdate)
                                {
                                    //Add details to Sign Off
                                    CurrentSignture.Signed = true;
                                    CurrentSignture.SignedByFullName = CurrentSignture.Signed ? signeduser.FullName : "";
                                    CurrentSignture.SignedByUserID = CurrentSignture.Signed ? signeduser.ID : CurrentSignture.SignedByUserID;
                                    CurrentSignture.SignedOn = DateTime.UtcNow;
                                    CurrentSignture.Updated = true;
                                }
                            }

                            //Reload Signature Tab
                            // LoadSignaturesTab();
                        }
                        else
                        {
                            //Do not have rights for this sign off
                         //   await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "You currently do not have the user rights to sign off this signature", "OK");

                            var ansr = await Application.Current.MainPage.DisplayActionSheet("You currently do not have the user rights to sign off this signature", "Sign by Other", "OK");
                            if (ansr == "Sign by Other")
                            {
                                var vm = await ReadLoginPopup();
                                if (vm.Password != null && vm.UserName != null)
                                {
                                    var UserDetailsList = await _userDetailsRepository.GetAsync(x => x.UserName == vm.UserName && x.Password == vm.Password);
                                    if (UserDetailsList.Any())
                                    {
                                        OtherUser = UserDetailsList.FirstOrDefault();
                                        UpdateSignatureItem(false);
                                    }
                                    else
                                    {
                                        await Application.Current.MainPage.DisplayAlert("Login", AppConstant.LOGIN_FAILURE, "OK");
                                    }


                                }
                            }

                        }
                    }
                    else
                        await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "Previous sign off must be completed to sign off this signature", "OK");
                }

                break;

                //item.Signed = !_selectedSignatureItem.Signed;
                //item.SignedByFullName = item.Signed ? userDetail.FullName : "";
                //item.SignedByUserID = item.Signed ? userDetail.ID : item.SignedByUserID;
                //item.SignedOn = item.Signed ? DateTime.Now : Convert.ToDateTime("01/01/2000 0:00");
            }

            SignatureList = new ObservableCollection<T_EReports_Signatures>(SignatureList);


        }
        private async void SaveSignaturesofMRR()
        {
            try
            {

            foreach (T_EReports_Signatures item in SignatureList)
            {
                await _signaturesRepository.QueryAsync<T_EReports_Signatures>(@"UPDATE T_EReports_Signatures SET [Signed] = " + Convert.ToInt32(item.Signed) + ",[SignedByUserID] = " + item.SignedByUserID
                                                                                + ",[SignedByFullName] = '" + item.SignedByFullName + "',[SignedOn] = '" + item.SignedOn.Ticks
                                                                                + "', [Updated] = "+ Convert.ToInt32(true) + " WHERE [EReportID] = " + item.EReportID + " AND [SignatureRulesID] = " + item.SignatureRulesID);
                await _eReportsRepository.QueryAsync<T_EReports>(@"UPDATE T_EReports SET [Updated] = '" + 1 + "' WHERE [ID] = " + _selectedEReportItem.ID);
            }
            _userDialogs.AlertAsync("E-Report Signatures data saved successfully", "Save Signatures Data", "OK");
            }
            catch (Exception ex)
            {

            }
        }
        private void BindMRRDataFormEdit(MRRRow SelectedMRRRows)
        {
            Good = string.Format("{0:N3}", SelectedMRRRows.Good);
            Damage = string.Format("{0:N3}", SelectedMRRRows.Damage);
            Off_Spec = string.Format("{0:N3}", SelectedMRRRows.Off_Spec);
            Short = string.Format("{0:N3}", SelectedMRRRows.Short);
            Over = string.Format("{0:N3}", SelectedMRRRows.Over);
            PONo= "PO No: " + SelectedMRRRows.PO_No + ", PJ Sub Commodity: " + SelectedMRRRows.PJ_Sub_Commodity + ", Item No.: " + SelectedMRRRows.Item_No;
            PTNO = "PT No.: " + SelectedMRRRows.PT_No + ", Description: " + SelectedMRRRows.Description;
            ReceivedQuantity = "Received Quantity (Shipped Quantity " + string.Format("{0:N3}", SelectedMRRRows.Shipped_Qty) + ") " + SelectedMRRRows.Unit;
            GetddlStorageAreasData();
            StorageAreas = new ObservableCollection<MRRStorageAreas>(SelectedMRRRows.Storage_Area_Codes);
            HeatNos = new ObservableCollection<MRRHeatNos>(SelectedMRRRows.Heat_Nos);
            Remark = SelectedMRRRows.Remarks_1;
            Remark2 = SelectedMRRRows.Remarks_2;
            Accept = SelectedMRRRows.Client_Accepted;
            Reject = SelectedMRRRows.Client_Rejected;
            OnClickButton("Accept_Reject");
        }
        private async void GetddlStorageAreasData()
        {
            var data = await _StorageAreasRepository.QueryAsync<T_StorageAreas>(@"SELECT [Storage_Area_Code] FROM [T_StorageAreas] WHERE [Project_ID] = " +
                              Settings.ProjectID + " AND [Store_Location] = '" + CurrentMRR.Store_Location + "' ORDER BY [Storage_Area_Code] ASC");
           
            ddlStorageAreas = new ObservableCollection<string>(data.Select(i => i.Storage_Area_Code));
        }
        private async void Search(string searchtext)
       {
           
            List<MRRRow> mrr = new List<MRRRow>();
            foreach (MRRRow row in CurrentMRR.MRRRows)
            {
                Boolean CanAdd = true;
                if (searchtext != string.Empty)
                {
                    string RowValue = row.PO_No + " " + row.PJ_Sub_Commodity + " " + row.Item_No + " " + row.PT_No + " " + row.Description;

                    if (!RowValue.ToUpper().Contains(searchtext.ToUpper()))
                        CanAdd = false;
                }
                if (CanAdd)
                {
                    mrr.Add(row);
                }
            }
            MRRRows = new ObservableCollection<MRRRow>(mrr);
        }

        private async void CheckRPR()
        {
            if (await CheckCanEditMRREReport())
            {
                if (await _userDialogs.ConfirmAsync($"Are you sure you want to request partial issuance?", $"Request Partial Issuance", "Yes", "No"))
                {
                    if (await CheckPartialRequest())
                    {
                        T_PartialRequest RP = new T_PartialRequest()
                        {
                            ID = await getID(),
                            EReportID = _selectedEReportItem.ID,
                            RequestedOn = DateTime.Now,
                            RequestedByUserID = Settings.UserID
                        };
                        await _PartialRequest.InsertOrReplaceAsync(RP);
                        var data = _PartialRequest.GetAsync();
                        //(@"INSERT INTO T_PartialRequest([EReportID],[RequestedOn],[RequestedByUserID]) VALUES (" + _selectedEReportItem.ID + ", "+ DateTime.Now + ", "+ Settings.UserID + ")")
                        await _eReportsRepository.QueryAsync<T_EReports>("UPDATE [T_EReports] SET [Updated] = " + 1 + " WHERE [ID] = " + _selectedEReportItem.ID);
                    }
                    else
                    {
                        await _userDialogs.AlertAsync("You are unable to request partial receiving, as you are not assigned to edit this report", "Request Partial Receiving", "Ok");
                    }
                }
            }

        }

        private async Task<bool> CheckCanEditMRREReport()
        {
            var RPRdata = await _usersAssignedRepository.QueryAsync<RPR>(@"SELECT DISTINCT UA.[CanEdit] as UA_CanEdit,SIG.[SignatureNo] as SIG_SignatureNo 
                                                                          FROM [T_EReports_UsersAssigned] AS UA INNER JOIN [T_EReports_Signatures] AS SIG ON UA.[EReportID] = SIG.[EReportID] AND UA.[SignatureRulesID] = SIG.[SignatureRulesID] 
                                                                          WHERE UA.[EReportID] = " + _selectedEReportItem.ID + " AND UA.[UserID] = " + Settings.UserID + " " + " " +
                                                                          "AND SIG.[Signed] = " + Convert.ToInt32(false) + " AND UA.[CanEdit] = " + Convert.ToInt32(true) + " ORDER BY SIG.[SignatureNo] ASC");
            return RPRdata.FirstOrDefault().UA_CanEdit;
        }
        private async Task<bool> CheckPartialRequest()
        {
            var data = await _PartialRequest.QueryAsync<int>(@"SELECT ID FROM [T_PartialRequest] WHERE [EReportID] =  " + _selectedEReportItem.ID);
            return data.Count == 0 ? true : false;
        }
        private async Task<int> getID()
        {
            var data = await _PartialRequest.QueryAsync<int>(@"SELECT * FROM [T_PartialRequest]");
            return data.Count + 1;
        }
        private double CalculateMRRSStorageAreaListQty(ObservableCollection<MRRStorageAreas> storageAreas)
        {
            double goodQty = 0;

            if (storageAreas.Count > 0)
                foreach (MRRStorageAreas existingValue in storageAreas)
                    goodQty = goodQty + existingValue.Good_Quantity;

            return goodQty;
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
        private void CalculateShortAndOver()
        {
            if (SelectedMRRRows != null)
            {
                double _good, _damage, _offSpec;
                if (string.IsNullOrWhiteSpace(Good)) _good = 0.0;
                else _good = Double.Parse(Good);

                if (string.IsNullOrWhiteSpace(Damage)) _damage = 0.0;
                else _damage = Double.Parse(Damage);


                if (string.IsNullOrWhiteSpace(Off_Spec)) _offSpec = 0.0;
                else _offSpec = Double.Parse(Off_Spec);

                var CalculatedResult = SelectedMRRRows.Shipped_Qty - (_good + _damage + _offSpec);

                if (CalculatedResult > 0)
                {
                    Short = CalculatedResult.ToString();
                    Over = "0.0";
                }
                else if (CalculatedResult < 0)
                {
                    Over = Math.Abs(CalculatedResult).ToString();
                    Short = "0.0";
                }
                else if (CalculatedResult == 0)
                {
                    Over = "0.0";
                    Short = "0.0";
                }
            }
        }
        #endregion

        #region Public
        public async void ShowDescriptionPopup(string Description)
        {
            if (Description == null)
                return;
            if (!string.IsNullOrWhiteSpace(Description))
                await PopupNavigation.PushAsync(new ShowWrapTextPopup("Description", Description), true);
        }
        //public async void NavigateToEDWR_EReporterPage(string Report)
        //{
        //    if (Report == null || IsRunningTasks)
        //    {
        //        return;
        //    }

        //    IsRunningTasks = true;
        //    _userDialogs.ShowLoading("Loading...");
        //    var navigationParameters = new NavigationParameters();
        //    navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
        //    await navigationService.NavigateAsync<DWR_EReporterViewModel>(navigationParameters);
        //    IsRunningTasks = false;
        //}
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

            if (parameters.Count > 1 && parameters.ContainsKey(NavigationParametersConstants.ReportDetailsParameter))
            {
                _selectedEReportItem = (T_EReports)parameters[NavigationParametersConstants.ReportDetailsParameter];

                GetDetailsData();
            }
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        public async void NavigateToRemoveStorageAreas()
        {
            if (SelectedStorageArea != null || IsRunningTasks)
            {
                if (await UserDialogs.Instance.ConfirmAsync($"Are you sure you want to remove this storage area?", $"Remove Storage Area", "Yes", "No"))
                {
                    StorageAreas.Remove(SelectedStorageArea);
                    Good = string.Format("{0:N3}", CalculateMRRSStorageAreaListQty(StorageAreas));
                }
            }
        }        
        public async void NavigateToRemoveHeatNo()
        {
            if (SelectedHeatNo != null || IsRunningTasks)
            {
                if (await UserDialogs.Instance.ConfirmAsync($"Are you sure you want to remove this heat no?", $"Remove Heat No", "Yes", "No"))
                    HeatNos.Remove(SelectedHeatNo);               
            }
        }
        #endregion

        #region protected
        protected Boolean StringToDoubleCheck(string value)
        {
            Boolean result = false;

            try
            {
                Convert.ToDouble(value);
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }
        public class RPR
        {
            public bool UA_CanEdit { get; set; }
            public int SIG_SignatureNo { get; set; }
        }
        #endregion
    }
}

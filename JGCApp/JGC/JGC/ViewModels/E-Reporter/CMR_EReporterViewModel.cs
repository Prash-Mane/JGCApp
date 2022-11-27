using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.Models;
using Newtonsoft.Json;
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
using JGC.Common.Extentions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using JGC.UserControls.PopupControls;
using JGC.ViewModels.E_Reporter;
using JGC.Views.E_Reporter;

namespace JGC.ViewModels
{
    public class CMR_EReporterViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IRepository<T_EReports> _eReportsRepository;
        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private readonly IRepository<T_EReports_Signatures> _signaturesRepository;
        private readonly IRepository<T_Drawings> _drawingsRepository;
        private readonly IRepository<T_StorageAreas> _StorageAreasRepository;
        private readonly IRepository<T_EReports_UsersAssigned> _usersAssignedRepository;
        private readonly IRepository<T_CMR_StorageAreas> _CMS_AllStorageAreasRepository;
        private readonly IRepository<T_CMR_HeatNos> _CMR_HeatNosRepository;
        private readonly IRepository<T_PartialRequest> _PartialRequest;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IResizeImageService _resizeImageService;
        private T_EReports _selectedEReportItem;
        private readonly IMedia _media;
        private T_UserDetails userDetail;
        private bool Accept, Reject;
        private byte[] imageAsByte;
        private string CMRPath;
        private T_UserDetails OtherUser;
        CMR CurrentCMR = new CMR();

        #region Properties  
        private ObservableCollection<CMRSummaryRows> summary;
        public ObservableCollection<CMRSummaryRows> CMRSummary
        {
            get { return summary; }
            set { summary = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<CMRDetailRows> detail;
        public ObservableCollection<CMRDetailRows> CMRDetail
        {
            get { return detail; }
            set { detail = value; RaisePropertyChanged(); }
        }
        private CMRSummaryRows selectedItem;
        public CMRSummaryRows SelectedCMRSummary
        {
            get { return selectedItem; }
            set
            {              
                if (SetProperty(ref selectedItem, value))
                {
                    //ShowDescriptionPopup();
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

        private ObservableCollection<CMRStorageAreas> storageAreas;
        public ObservableCollection<CMRStorageAreas> StorageAreas
        {
            get { return storageAreas; }
            set { storageAreas = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<string> ddlstorageAreas;
        public ObservableCollection<string> ddlStorageAreas
        {
            get { return ddlstorageAreas; }
            set { ddlstorageAreas = value; RaisePropertyChanged(); }
        }
        private CMRStorageAreas deleteStorageAreas;
        public CMRStorageAreas DeleteStorageAreas
        {
            get { return deleteStorageAreas; }
            set
            {
                SetProperty(ref deleteStorageAreas, value);
            }

        }

        private ObservableCollection<string> ddlheatNos;
        public ObservableCollection<string> ddlHeatNos
        {
            get { return ddlheatNos; }
            set { ddlheatNos = value; RaisePropertyChanged(); }
        }

        //CameraItems
        private string selectedStorageAreas;
        public string SelectedStorageAreas
        {
            get { return selectedStorageAreas; }
            set
            {
                if (SetProperty(ref selectedStorageAreas, value))
                {
                    LoadStorageAreasCode(selectedStorageAreas);
                    OnPropertyChanged();
                }
            }
        }

        private string _CMRStorageAreaCode;
        public string CMRStorageAreaCodes
        {
            get { return _CMRStorageAreaCode; }
            set { SetProperty(ref _CMRStorageAreaCode, value); }
        }
        private string _item;
        public string CMRitem
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
        private bool _editCMRGrid;
        public bool EditCMRGrid
        {
            get { return _editCMRGrid; }
            set { SetProperty(ref _editCMRGrid, value); }
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
        private bool _btnBacktoDetails;
        public bool btnBacktoDetails
        {
            get { return _btnBacktoDetails; }
            set { SetProperty(ref _btnBacktoDetails, value); }
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
            get {
                return _selectedImageFiles;
            }
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
                  //  UpdateSignatureItem(_selectedSignatureItem);
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

        private string pjc;
        public string PJC
        {
            get { return pjc; }
            set
            {
                SetProperty(ref pjc, value);
            }
        }
        private string pName;
        public string PName
        {
            get { return pName; }
            set
            {
                SetProperty(ref pName, value);
            }
        }
        private string unit;
        public string Unit
        {
            get { return unit; }
            set
            {
                SetProperty(ref unit, value);
            }
        }
        private string issued_Qty;
        public string CMRIssued_Qty
        {
            get { return issued_Qty; }
            set
            {
                SetProperty(ref issued_Qty, value);
            }
        }
        private string over_Issd_Adj;
        public string CMROver_Issd_Adj
        {
            get { return over_Issd_Adj; }
            set
            {
                SetProperty(ref over_Issd_Adj, value);
            }
        }
        private double _availStockQty;
        public double availStockQty
        {
            get { return _availStockQty; }
            set
            {
                SetProperty(ref _availStockQty, value);
            }
        }
        private string issuedbyMCS;
        public string IssuedbyMCS
        {
            get { return issuedbyMCS; }
            set
            {
                SetProperty(ref issuedbyMCS, value);
            }
        }

        private string selectedHeatNo;
        public string SelectedHeatNo
        {
            get { return selectedHeatNo; }
            set
            {
                SetProperty(ref selectedHeatNo, value);
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

        private bool storageAreaGrid;
        public bool StorageAreaGrid
        {
            get { return storageAreaGrid; }
            set { SetProperty(ref storageAreaGrid, value); }
        }
        private bool showHideHeatNo;
        public bool ShowHideHeatNo
        {
            get { return showHideHeatNo; }
            set { SetProperty(ref showHideHeatNo, value); }
        }
        private string editDone;
        public string EditDone
        {
            get { return editDone; }
            set { SetProperty(ref editDone, value); }
        }
        private string editDoneParam;
        public string EditDoneParam
        {
            get { return editDoneParam; }
            set { SetProperty(ref editDoneParam, value); }
        }
        private string editHeatNo;
        public string EditHeatNo
        {
            get { return editHeatNo; }
            set { SetProperty(ref editHeatNo, value); }
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

        public CMR_EReporterViewModel(
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
            IRepository<T_CMR_StorageAreas> _CMS_AllStorageAreasRepository,
            IRepository<T_CMR_HeatNos> _CMR_HeatNosRepository,
            IRepository<T_EReports_UsersAssigned> _usersAssignedRepository,
            IRepository<T_PartialRequest> _PartialRequest) : base(_navigationService, _httpHelper, _checkValidLogin)
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
            this._CMS_AllStorageAreasRepository = _CMS_AllStorageAreasRepository;
            this._CMR_HeatNosRepository = _CMR_HeatNosRepository;
            this._usersAssignedRepository = _usersAssignedRepository;
            this._PartialRequest = _PartialRequest;
            _media.Initialize();
            DetailsArrow = DetailsGrid = MainGrid = Showbuttons = true;
            IsVisiblePdfFull = false;
            PageHeaderText = "Construction Material Requisition";
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
                DetailsArrow = DetailsGrid = EditCMRGrid = DetailsGrid = btnRPR = btnSaveEdit = cameraIsVisible = !DetailsArrow;
                SignaturesGrid = AttachmentsGrid = EditCMRGrid = btnSave = btnBacktoDetails = !DetailsGrid;
                btnSaveEdit = btnRPR = cameraIsVisible = false;
                BtnName = "Edit";
                GetDetailsData();
                _userDialogs.HideLoading();
            }
            else if (param == "Signatures")
            {
                _userDialogs.ShowLoading("Loading...");
                SignaturesArrow = SignaturesGrid = !SignaturesArrow;
                DetailsGrid = AttachmentsGrid = EditCMRGrid = btnBacktoDetails = !SignaturesGrid;
                GetSignatureData(_selectedEReportItem.ID);
                btnSaveEdit = true;
                btnRPR = cameraIsVisible = btnSave = false;
                BtnName = "Save";
                _userDialogs.HideLoading();
            }
            else if (param == "Attachments")
            {
                _userDialogs.ShowLoading("Loading...");
                AttachmentsArrow = AttachmentsGrid = !AttachmentsArrow;
                DetailsGrid = SignaturesGrid = EditCMRGrid = btnBacktoDetails = !AttachmentsGrid;
                btnSaveEdit = btnRPR = cameraIsVisible = false;
                GetAttachmentData(_selectedEReportItem.ID);
                _userDialogs.HideLoading();
            }
            else if (param == "Camera")
            {
                _userDialogs.ShowLoading("Loading...");
                generatepath("Commodity");
                btnSaveDelete = "Save";
                RenameImage = false;
                CameraItems = new List<string> { "USB2.0_Camera", "USB2.0_Camera 1" };
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
                    _userDialogs.AlertAsync("Please enter new name", null, "Ok");
            }
            else if (param == "Back")
            {
                CapturedImage = "";
                CameraGrid = !CameraGrid;
                MainGrid = !CameraGrid;
            }
            else if (param == "Save")
            {
                if (EditCMRGrid)
                {
                    DetailsArrow = true;
                    SaveDetailsofCMR();
                }
                else if (SignaturesGrid)
                {
                    SignaturesArrow = true;
                    SaveSignaturesofCMR();
                }
            }
            else if (param == "Edit")
            {
                DetailsArrow = true;
                if (SelectedCMRSummary == null)
                    return;
                SelectedStorageAreas = CMRStorageAreaCodes = IssuedbyMCS = SelectedHeatNo = null;
                BindCMRDataFormEdit();
                DetailsGrid = btnRPR = btnSaveEdit = cameraIsVisible = !DetailsGrid;
                EditCMRGrid = btnSave = DetailsArrow = btnBacktoDetails = !DetailsGrid;
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
                SelectedImageFiles =  SelectedCameraItem = null;
                SelectedCameraItems(_selectedCameraItem);
            }
            else if (param == "AddStorageArea")
            {
                if (IssuedbyMCS != null && CMRStorageAreaCodes != null && SelectedHeatNo != null)
                {
                    if (StringToDoubleCheck(IssuedbyMCS))
                    {
                        if (Convert.ToDouble(IssuedbyMCS) > 0)
                        {
                           
                                if (Convert.ToDouble(IssuedbyMCS) > availStockQty)
                                {
                                    _userDialogs.AlertAsync("Issued quantity cannot be more than available quantity, please adjust.", "Add Storage Area", "OK");
                                    return;
                                }

                                CMRStorageAreas sa = new CMRStorageAreas();

                                sa.Storage_Area_Available = CMRStorageAreaCodes;
                                sa.Avail_Stock_Qty = availStockQty;
                                sa.Issued_Qty = Convert.ToDouble(IssuedbyMCS);
                                sa.Storage_Area_Code = CMRStorageAreaCodes;
                                sa.Heat_No = SelectedHeatNo;
                                sa.Is_Partially_Issued = false;
                                CMRIssued_Qty += sa.Issued_Qty;
                                StorageAreas.Add(sa);
                                CMRIssued_Qty = string.Format("{0:N3}", CalculateCMRSStorageAreaListQty(StorageAreas));

                                SelectedStorageAreas = CMRStorageAreaCodes = IssuedbyMCS = SelectedHeatNo = null;
                                _userDialogs.AlertAsync(" Added storage area successfully", "Add storage area", "OK");
                           
                        }
                        else
                        {
                            _userDialogs.AlertAsync("Storage quantity is invalid, please ensure a storage area is provided and the value provided is numeric.", "Add storage area", "OK");
                        }
                    }
                    else
                        _userDialogs.AlertAsync("Please ensure all storage area information is provided.", "Add Storage Area", "OK");
                }
                else
                {
                    _userDialogs.AlertAsync("Please ensure all storage area information is provided.", "Add Storage Area", "OK");
                }
            }
            else if (param == "StorageAreaGrid")
            {
                StorageAreaGrid = true;
                MainGrid = DetailsArrow = false;
                EditDone = "Edit";
                EditDoneParam = "EditParam";
                ShowHideHeatNo = true;
            }
            else if(param == "EditParam")
            {
                EditDone = "Done";
                EditDoneParam = "DoneParam";
                EditHeatNo = SelectedHeatNo;
                ShowHideHeatNo = false;
            }
            else if (param == "DoneParam")
            {
                EditDone = "Edit";
                EditDoneParam = "EditParam";
                if (EditHeatNo == null) 
                {
                    _userDialogs.AlertAsync("Please Enter Heat No.", "Add Storage Area", "OK");
                    return;
                }
                ddlHeatNos.Add(EditHeatNo);
                SelectedHeatNo = ddlHeatNos.LastOrDefault();
                ShowHideHeatNo = true;
            }
            else if (param == "BackFromStorageArea")
            {
                StorageAreaGrid = false;
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
        private async void LoadStorageAreasCode(string SAC)
        {
            if (SAC == null)
                return;
            // String separator  
            string[] stringSeparators = new string[] { " (Qty. " };
            string[] NewString = SAC.Split(stringSeparators, StringSplitOptions.None);
            CMRStorageAreaCodes = NewString[0].Trim();
            availStockQty = Convert.ToDouble(NewString[1].Replace(")", "").Trim());

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
        private void SelectedCameraItems(string SelectedCameraItem)
        {
            if (SelectedCameraItem == null)
                return;
            btnSaveDelete = "Save";
            CameraService();
        }
        private async void LoadImageFiles(string SelectedImageFiles)
        {
            if (SelectedImageFiles == null)
                return;
            CapturedImage = await DependencyService.Get<ISaveFiles>().GetImage(CMRPath + "/" + SelectedImageFiles);
            btnSaveDelete = "Delete";
            RenameImage = true;
            //CapturedImage = ImageSource.FromStream(() => stream);
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
                imageAsByte = null;
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
                 var PickFile=  await DependencyService.Get<ISaveFiles>().PickFile(CMRPath);               
                 ImageFiles = new ObservableCollection<string>(await DependencyService.Get<ISaveFiles>().GetAllImages(CMRPath));
                if (PickFile) 
                {
                    await _eReportsRepository.QueryAsync<T_EReports>(@"UPDATE T_EReports SET [Updated] = '" + 1 + "' WHERE [ID] = " + _selectedEReportItem.ID);
                    await _userDialogs.AlertAsync("Added image file successfully", "Saved Image", "OK");
                }
                   
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

        private async void CaptureImageSave()
        {
            if (imageAsByte != null)
            {
                string path = await DependencyService.Get<ISaveFiles>().SavePictureToDisk(CMRPath, DateTime.Now.ToString(AppConstant.CameraDateFormat), imageAsByte.ToArray());
                if (path != null)
                {
                    generatepath("Commodity");
                    //RenameImage = false;
                    CapturedImage = "";
                    SelectedImageFiles = SelectedCameraItem = null;
                    await _eReportsRepository.QueryAsync<T_EReports>(@"UPDATE T_EReports SET [Updated] = '" + 1 + "' WHERE [ID] = " + _selectedEReportItem.ID);
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
                bool isdelete = await DependencyService.Get<ISaveFiles>().DeleteImage(CMRPath + "/" + SelectedImageFiles);
                if (isdelete)
                {
                    ImageFiles = new ObservableCollection<string>(await DependencyService.Get<ISaveFiles>().GetAllImages(CMRPath));
                    btnSaveDelete = "Save";
                    RenameImage = false;
                    CapturedImage = NewImageName  = "";
                }
            }
        }
        private async void generatepath(string Field)
        {
            string Folder = ("Photo Store" + "\\" + CurrentCMR.JobCode + "\\" + _selectedEReportItem.ID.ToString() + "\\" + Field);
            CMRPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath(Folder);

            ImageFiles = new ObservableCollection<string>(await DependencyService.Get<ISaveFiles>().GetAllImages(CMRPath));
        }
        private async void SaveRenameImage()
        {
            try
            {
                if (await DependencyService.Get<ISaveFiles>().RenameImage(CMRPath, SelectedImageFiles, NewImageName + ".jpg"))
                {
                    ImageFiles = new ObservableCollection<string>(await DependencyService.Get<ISaveFiles>().GetAllImages(CMRPath));
                    ShowRename = false;
                    Showbuttons = true;
                    CapturedImage = "";
                    await _userDialogs.AlertAsync("Successfully rename..!", null, "Ok");
                }
            }
            catch(Exception ex)
            {

            }
            
        }

        private async void GetDetailsData()
        {
            if (_selectedEReportItem.JSONString != string.Empty)
            {
                CurrentCMR = JsonConvert.DeserializeObject<CMR>(_selectedEReportItem.JSONString);
                if (CurrentCMR.CMRSummaryRows != null)
                {
                    CMRSummary = new ObservableCollection<CMRSummaryRows>(CurrentCMR.CMRSummaryRows);
                    btnSaveEdit = btnRPR = cameraIsVisible = true;
                    BtnName = "Edit";
                }
                // GetSignatureDateData(_selectedEReportItem.ID);                
                CameraIcon = "cam.png";
            }

            CMRitem = _selectedEReportItem.ReportNo + ", " + _selectedEReportItem.ReportDate.ToString(AppConstant.DateFormat) + ", MRS No. " + CurrentCMR.MRS_No;

            generatepath("CMR");
            //string Folder = ("Photo Store" + "\\" + CurrentCMR.JobCode + "\\" + _selectedEReportItem.ID.ToString() + "\\" + "CMR");
            //CMRPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath(Folder);

        }
       private async void Search(string searchtext)
       {           
            List<CMRSummaryRows> SummaryRow = new List<CMRSummaryRows>();
            foreach (CMRSummaryRows row in CurrentCMR.CMRSummaryRows)
            {
                Boolean CanAdd = true;
                if (searchtext != string.Empty)
                {
                    string RowValue = row.PJ_Commodity + " " + row.Sub_Commodity + " " + row.Size_Descr + " " + row.Unit + " " + row.Commodity_Descr;

                    if (!RowValue.ToUpper().Contains(searchtext.ToUpper()))
                        CanAdd = false;
                }
                if (CanAdd)
                {
                    SummaryRow.Add(row);
                }
            }
            CMRSummary = new ObservableCollection<CMRSummaryRows>(SummaryRow);
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
            AttachmentList = new ObservableCollection<T_Drawings>(AttachmentData.Distinct().Where(x=>x.FileName.Trim().Contains("pdf")));
        }
        private async void SaveDetailsofCMR()
        {
            if (StringToDoubleCheck(CMROver_Issd_Adj))
            {
                SelectedCMRSummary.Issued_Qty = Convert.ToDouble(CMRIssued_Qty);
                SelectedCMRSummary.Over_Issd_Adj = Convert.ToDouble(CMROver_Issd_Adj);
                SelectedCMRSummary.Storage_Areas = StorageAreas.ToList();

                string JSONString = ModsTools.ToJson(CurrentCMR);
                await _eReportsRepository.QueryAsync<T_EReports>(@"UPDATE T_EReports SET [JSONString] = '" + JSONString.Replace("'", "''") + "' , [Updated] = '" + 1 + "' WHERE [ID] = " + _selectedEReportItem.ID);
                var returndata = await _eReportsRepository.QueryAsync<T_EReports>(@"SELECT * FROM T_EReports WHERE [ID] = " + _selectedEReportItem.ID);
                _selectedEReportItem = returndata.FirstOrDefault();
                _userDialogs.AlertAsync("Item saved successfully", "Save Item", "OK");
                OnClickButton("Details");
            }
            else
                _userDialogs.AlertAsync("Item failed to save", "Save Item", "OK");

        }
        public async void UpdateSignatureItem(bool IsLogedInUser)
        {
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

                                if (CurrentSignture.SendToDataHub && _selectedEReportItem.ReportType.ToUpper() == "CONSTRUCTION MATERIAL REQUISITION")
                                {
                                    CMR currentCMR = new CMR();
                                    var data = await _eReportsRepository.GetAsync(x => x.ID == _selectedEReportItem.ID);
                                    string jsonString = data.FirstOrDefault().JSONString;

                                    if (jsonString != string.Empty)
                                    {
                                        currentCMR = JsonConvert.DeserializeObject<CMR>(jsonString);

                                        if (currentCMR.CMRSummaryRows != null)
                                        {
                                            foreach (CMRSummaryRows row in currentCMR.CMRSummaryRows)
                                            {

                                                double totaladded = row.Issued_Qty + row.Over_Issd_Adj;

                                                if (Math.Round(row.Request_Qty, row.Request_Qty.ToString().Substring(row.Request_Qty.ToString().IndexOf(".") + 1).Length, MidpointRounding.AwayFromZero) >
                                                            Math.Round(totaladded, totaladded.ToString().Substring(totaladded.ToString().IndexOf(".") + 1).Length, MidpointRounding.AwayFromZero))
                                                {

                                                    await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "To sign this signature please make sure the 'Issued Quantity' plus 'Adjustment against Over Issued Quantity' is equal to or greater than 'Requested Quantity', for all items.", "OK");

                                                    CanUpdate = false;
                                                    break;
                                                }
                                            }
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

                        
                        }
                        else
                        {
                            //Do not have rights for this sign off
                           // await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "You currently do not have the user rights to sign off this signature", "OK");

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

              
            }

            SignatureList = new ObservableCollection<T_EReports_Signatures>(SignatureList);


        }
        private async void SaveSignaturesofCMR()
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
        private void BindCMRDataFormEdit()
        {
            PJC = "PJ Commodity: " + SelectedCMRSummary.PJ_Commodity + ", PJ Sub Commodity: " + SelectedCMRSummary.Sub_Commodity + ", Size: " + SelectedCMRSummary.Size_Descr;
            PName = "Part Name: " + SelectedCMRSummary.Part_Name + ", Description: " + SelectedCMRSummary.Commodity_Descr;
            Unit = "Unit: " + SelectedCMRSummary.Unit + ", Reserved: " + string.Format("{0:N3}", SelectedCMRSummary.Reserved_To_Subcon) + ", Requested: " + string.Format("{0:N3}", SelectedCMRSummary.Request_Qty);
            CMRIssued_Qty = string.Format("{0:N3}", SelectedCMRSummary.Issued_Qty);
            GetddlStorageAreasData();
            GetddlHeatNosData();
            CMROver_Issd_Adj = SelectedCMRSummary.Over_Issd_Adj.ToString();
            if (SelectedCMRSummary.Storage_Areas != null)
                StorageAreas = new ObservableCollection<CMRStorageAreas>(SelectedCMRSummary.Storage_Areas);
            else StorageAreas = new ObservableCollection<CMRStorageAreas>();
        }
        private async void GetddlStorageAreasData()
        {
            string SQL = "SELECT * FROM [T_CMR_StorageAreas]"   //[Storage_Area] & \" (Qty. \" & [Avail_Stock_Qty] & \")\"
                + " WHERE [Project_ID] = " + Settings.ProjectID
                + " AND [Job_Code_Key] = '" + CurrentCMR.JobCode + "'"
                + " AND [Store_Location] = '" + CurrentCMR.Store_Location + "'"
                + " AND [PJ_Commodity] = '" + SelectedCMRSummary.PJ_Commodity + "'"
                + " AND [Sub_Commodity] = '" + SelectedCMRSummary.Sub_Commodity + "'"
                + " AND [Size_Descr] = '" + SelectedCMRSummary.Size_Descr + "'"
                + " ORDER BY [Storage_Area] ASC";

            //var CMSdata = await _CMS_AllStorageAreasRepository.GetAsync();
            var data = await _CMS_AllStorageAreasRepository.QueryAsync<T_CMR_StorageAreas>(SQL);
            List<string> ddl = new List<string>();
            foreach (T_CMR_StorageAreas item in data)
                ddl.Add(item.Storage_Area + " (Qty. " + string.Format("{0:N3}", item.Avail_Stock_Qty) + ")");
            ddlStorageAreas = new ObservableCollection<string>(ddl);
        }
        private async void GetddlHeatNosData()
        {

            string SQL = "SELECT [Heat_No] FROM [T_CMR_HeatNos]"
                + " WHERE [Project_ID] = " + Settings.ProjectID
                + " AND [Job_Code_Key] = '" + CurrentCMR.JobCode + "'"
                + " AND [Store_Location] = '" + CurrentCMR.Store_Location + "'"
                + " AND [PJ_Commodity] = '" + SelectedCMRSummary.PJ_Commodity + "'"
                + " AND [Sub_Commodity] = '" + SelectedCMRSummary.Sub_Commodity + "'"
                + " AND [Size_Descr] = '" + SelectedCMRSummary.Size_Descr + "'"
                + " ORDER BY [Heat_No] ASC";

            var data = await _CMR_HeatNosRepository.QueryAsync<T_CMR_HeatNos>(SQL);
            List<string> ddl = new List<string>();
            foreach (T_CMR_HeatNos item in data)
                ddl.Add(item.Heat_No);
            ddlHeatNos = new ObservableCollection<string>(ddl);

        }
        private async void CheckRPR()
        {
            if (await CheckCanEditCMREReport())
            {
                if (await _userDialogs.ConfirmAsync($"Are you sure you want to request partial issuance?", $"Request Partial Issuance", "Yes", "No"))
                {
                    if ( await CheckPartialRequest())
                    {                       
                        T_PartialRequest RP = new T_PartialRequest() {
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

        private async Task<bool> CheckCanEditCMREReport()
        {
            var RPRdata = await _usersAssignedRepository.QueryAsync<RPR>(@"SELECT DISTINCT UA.[CanEdit] as UA_CanEdit,SIG.[SignatureNo] as SIG_SignatureNo 
                                                                          FROM [T_EReports_UsersAssigned] AS UA INNER JOIN [T_EReports_Signatures] AS SIG ON UA.[EReportID] = SIG.[EReportID] AND UA.[SignatureRulesID] = SIG.[SignatureRulesID] 
                                                                          WHERE UA.[EReportID] = " + _selectedEReportItem.ID + " AND UA.[UserID] = " + Settings.UserID + " " + " " +
                                                                          "AND SIG.[Signed] = " + Convert.ToInt32(false) + " AND UA.[CanEdit] = "+ Convert.ToInt32(true) + " ORDER BY SIG.[SignatureNo] ASC");
            return RPRdata.FirstOrDefault().UA_CanEdit;
        }
        private async Task<bool> CheckPartialRequest()
        {
           var data = await _PartialRequest.QueryAsync<int>(@"SELECT ID FROM [T_PartialRequest] WHERE [EReportID] =  " + _selectedEReportItem.ID);
            return data.Count ==0 ? true : false;
        }
        private async Task<int> getID()
        {
            var data = await _PartialRequest.QueryAsync<int>(@"SELECT * FROM [T_PartialRequest]");
            return data.Count + 1;
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
        public class RPR
        {
            public bool UA_CanEdit { get; set; }
            public int SIG_SignatureNo { get; set; }
        }
        #region Public
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
            if (DeleteStorageAreas != null || IsRunningTasks)
            {
                if (await UserDialogs.Instance.ConfirmAsync($"Are you sure you want to remove this storage area?", $"Remove Storage Area", "Yes", "No"))
                {
                    var Issued_Qty = DeleteStorageAreas.Issued_Qty;
                    StorageAreas.Remove(DeleteStorageAreas);
                    CMRIssued_Qty = string.Format("{0:N3}", (Convert.ToDouble(CMRIssued_Qty) - Issued_Qty));
                    await _userDialogs.AlertAsync(" Removed storage area successfully", "Removed storage area", "OK");
                }
            }
        }
        private double CalculateCMRSStorageAreaListQty(ObservableCollection<CMRStorageAreas> storageAreas)
        {
            double issuedQty = 0;

            if (storageAreas.Count > 0)
                foreach (CMRStorageAreas existingValue in storageAreas)
                    issuedQty = issuedQty + existingValue.Issued_Qty;

            return issuedQty;
        }

        public async void ShowDescriptionPopup(string Description)
        {
            if (Description == null)
                return;
            if (!string.IsNullOrWhiteSpace(Description))
                await PopupNavigation.PushAsync(new ShowWrapTextPopup("Description", Description), true);
        }
        public Boolean StringToDoubleCheck(string value)
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
        #endregion
    }
}

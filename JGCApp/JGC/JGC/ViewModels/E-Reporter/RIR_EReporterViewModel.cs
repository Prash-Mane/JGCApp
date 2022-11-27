using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using Prism.Navigation;
using JGC.Common.Extentions;
using Newtonsoft.Json;
using JGC.Models;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using JGC.Common.Helpers;
using JGC.UserControls.CustomControls;
using JGC.UserControls.PopupControls;
using Rg.Plugins.Popup.Services;
using JGC.ViewModels.E_Reporter;
using JGC.Views.E_Reporter;

namespace JGC.ViewModels
{
   public class RIR_EReporterViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IRepository<T_EReports> _eReportsRepository;
        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private readonly IRepository<T_EReports_Signatures> _signaturesRepository;
        private readonly IRepository<T_Drawings> _drawingsRepository;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IResizeImageService _resizeImageService;
        private readonly IRepository<T_EReports_UsersAssigned> _usersAssignedRepository;
        private T_EReports _selectedEReportItem;
        RIR CurrentRIR = new RIR();
        private string _filePath;
        private readonly IMedia _media;
        private T_UserDetails userDetail;
        private bool Accepted, Damage, Off_Sprc;
        private byte[] imageAsByte;
        private string InspectionPath;
        private T_UserDetails OtherUser;


        #region Properties  
        private ObservableCollection<RIRRow> _report;
        public ObservableCollection<RIRRow> RIRreports
        {
            get { return _report; }
            set { _report = value; RaisePropertyChanged(); }
        }
        private RIRRow _selectedRIRReport;
        public RIRRow SelectedRIRReport
        {
            get { return _selectedRIRReport; }

            set {
                if (SetProperty(ref _selectedRIRReport, value))
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
        //CameraItems
        private string _item;
        public string RIRitem
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
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { SetProperty(ref _selectedDate,value); }
        }
        
        private bool _btnSave;
        public bool btnSave
        {
            get { return _btnSave; }
            set { SetProperty(ref _btnSave, value); }
        }
        private string _acceptBGColor;
        public string AcceptBGColor
        {
            get { return _acceptBGColor; }
            set { SetProperty(ref _acceptBGColor, value); }
        }
        private string _damageBGColor;
        public string DamageBGColor
        {
            get { return _damageBGColor; }
            set { SetProperty(ref _damageBGColor, value); }
        }
        private string _offSPBGColor;
        public string OffSPBGColor
        {
            get { return _offSPBGColor; }
            set { SetProperty(ref _offSPBGColor, value); }
        }
        private string _acceptTextColor;
        public string AcceptTextColor
        {
            get { return _acceptTextColor; }
            set { SetProperty(ref _acceptTextColor, value); }
        }
        private string _damageTextColor;
        public string DamageTextColor
        {
            get { return _damageTextColor; }
            set { SetProperty(ref _damageTextColor, value); }
        }
        private string _OffSprcTextColor;
        public string OffSprcTextColor
        {
            get { return _OffSprcTextColor; }
            set { SetProperty(ref _OffSprcTextColor, value); }
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
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { SetProperty(ref _remark, value);}
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
        private ObservableCollection<string> _cameraItems;
        public ObservableCollection<string> CameraItems
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
                OnPropertyChanged();
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

        public RIR_EReporterViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            IMedia _media,
            ICheckValidLogin _checkValidLogin,
            IResizeImageService _resizeImageService,
            IRepository<T_UserDetails> _userDetailsRepository,
            IRepository<T_EReports> _eReportsRepository,
            IRepository<T_EReports_Signatures> _signaturesRepository,
            IRepository<T_EReports_UsersAssigned> usersAssignedRepository,
            IRepository<T_Drawings> _drawingsRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
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
            this._usersAssignedRepository = usersAssignedRepository;

            DetailsArrow = DetailsGrid = MainGrid = Showbuttons = true;
            IsVisiblePdfFull = false;
            _media.Initialize();
            _userDialogs.HideLoading();
            PageHeaderText = "Receiving Inspection Request";
            ShowPDFs = "Greenradio.png";
            ShowImages = "Grayradio.png";
            PDFview = true;
            ImagesView = false;
        }
        #region Private       
        private async void OnClickButton(string param)
        {
            DetailsArrow = SignaturesArrow =  AttachmentsArrow =false;
            if (param == "Details")
            {
                _userDialogs.ShowLoading("Loading...");
                DetailsArrow = DetailsGrid= !DetailsArrow;
                SignaturesGrid = AttachmentsGrid =!DetailsGrid;
                btnSave = false;
                GetDetailsData();
                _userDialogs.HideLoading();
            }
            else if (param == "Signatures")
            {
                _userDialogs.ShowLoading("Loading...");
                SignaturesArrow = SignaturesGrid= !SignaturesArrow;
                DetailsGrid = AttachmentsGrid= !SignaturesGrid;
                GetSignatureData(_selectedEReportItem.ID);
                btnSave = true;
                _userDialogs.HideLoading();
            }
            else if(param == "Attachments")
            {
                _userDialogs.ShowLoading("Loading...");
                AttachmentsArrow = AttachmentsGrid = !AttachmentsArrow;
                DetailsGrid = SignaturesGrid = !AttachmentsGrid;
                btnSave = false;
                GetAttachmentData(_selectedEReportItem.ID);
                _userDialogs.HideLoading();
            }
            else if(param == "Camera")
            {
                _userDialogs.ShowLoading("Loading...");

                btnSaveDelete = "Save";
                RenameImage = false;
                CameraItems = new ObservableCollection<string> { "USB2.0_Camera", "USB2.0_Camera 1" };
                //ImageFiles = new List<string> { "Image File Name", "Image File Name 1" };
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
                if(NewImageName!=null)
                  SaveRenameImage();
                else
                     _userDialogs.AlertAsync("Please enter new name", null, "Ok");
            }
            else if(param == "Back")
            {
                CapturedImage = "";                
                CameraGrid = !CameraGrid;
                MainGrid = !CameraGrid;
            }
            else if (param == "Accept")
            {
                Accepted = !Accepted;
                changebuttoncolors();
            }
            else if (param == "Damage")
            {
                Damage = !Damage;
                changebuttoncolors();
            }
            else if (param == "Off-Sprc")
            {
                Off_Sprc = !Off_Sprc;
                changebuttoncolors();
            }
            else if(param == "Save")
            {
                if (DetailsGrid)
                {
                    DetailsArrow = true;
                    SaveDetailsofRIR();
                }
                else if (SignaturesGrid)
                {
                    SignaturesArrow = true;
                    SaveSignaturesofRIR();
                }                
            }
            else if(param == "CaptureImageSave")
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
        private async void LoadAttachmentPDF(T_Drawings selectedAttachedItem)
        {
            if (selectedAttachedItem == null || IsRunningTasks)
            {
                return;
            }
            //IsRunningTasks = true;
            //// byte[] PDFBytes = Convert.FromBase64String(selectedAttachedItem.BinaryCode);
            // PdfUrl = selectedAttachedItem.FileLocation;
            // IsRunningTasks = false;

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
            AcceptTextColor = DamageTextColor = OffSprcTextColor = "black";
            AcceptBGColor = DamageBGColor = OffSPBGColor = "#c7c7c7";           
            if (Accepted)
            {
                AcceptBGColor = "Green";
                AcceptTextColor = "White";
            }
            if (Damage)
            {
                DamageBGColor = "Red";
                DamageTextColor = "White";
            }
            if (Off_Sprc)
            {
                OffSPBGColor = "Gray";
                OffSprcTextColor = "White";
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

                string path = await DependencyService.Get<ISaveFiles>().SavePictureToDisk(InspectionPath, DateTime.Now.ToString(AppConstant.CameraDateFormat), imageAsByte.ToArray());
                if (path != null)
                {
                    generatepath();
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
        private async void generatepath()
        {
            string Folder = ("Photo Store" + "\\" + CurrentRIR.JobCode + "\\" + _selectedEReportItem.ID.ToString() + "\\" + "Inspection");
            InspectionPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath(Folder);

            ImageFiles = new ObservableCollection<string>(await DependencyService.Get<ISaveFiles>().GetAllImages(InspectionPath));

            if (ImageFiles.Count > 0)
                CameraIcon = "Greencam.png";
            else
                CameraIcon = "cam.png";
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
                var PickFile = await DependencyService.Get<ISaveFiles>().PickFile(InspectionPath);
                ImageFiles = new ObservableCollection<string>(await DependencyService.Get<ISaveFiles>().GetAllImages(InspectionPath));
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

                //await DependencyService.Get<ISavePicture>().SavePictureToDisk(Device.RuntimePlatform == Device.UWP ? "CapturedImage.png" : "CapturedImage", data.ToArray());
                //await Application.Current.MainPage.DisplayAlert("Successfully saved..!", null, "Ok");
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
                CurrentRIR = JsonConvert.DeserializeObject<RIR>(_selectedEReportItem.JSONString);
                if (CurrentRIR.RIRRows != null)
                {
                    RIRreports = new ObservableCollection<RIRRow>(CurrentRIR.RIRRows);
                    btnSave = true;
                }
                // GetSignatureDateData(_selectedEReportItem.ID); 
                Accepted = CurrentRIR.Result_Accepted;
                Damage = CurrentRIR.Result_Damage;
                Off_Sprc = CurrentRIR.Result_OffSpec;
                Remark = CurrentRIR.Inspection_Remarks;
                if (CurrentRIR.Date_Inspected != Convert.ToDateTime("01/01/0001 0:00") && CurrentRIR.Date_Inspected != Convert.ToDateTime("01/01/2000 0:00") 
                    && CurrentRIR.Date_Inspected != Convert.ToDateTime("01/01/1900 0:00"))
                    SelectedDate = CurrentRIR.Date_Inspected;
                else
                    SelectedDate = DateTime.Now;
                changebuttoncolors();
            }
            RIRitem = _selectedEReportItem.ReportNo + ", " + _selectedEReportItem.ReportDate.ToString(AppConstant.DateFormat) + ", Ship No. " + CurrentRIR.Ship_No;
            generatepath();


        }
        private async void GetSignatureData(int ID)
        {
            var signaturesData= await _signaturesRepository.QueryAsync<T_EReports_Signatures>(@"SELECT * FROM T_EReports_Signatures WHERE [EReportID] = '" + ID + "' ORDER BY [SignatureNo] ASC");
            
            var UserDetailsList = await _userDetailsRepository.GetAsync();
            if (UserDetailsList.Count > 0)
                userDetail = UserDetailsList.Where(p => p.ID == Settings.UserID).FirstOrDefault();
            SignatureList = new ObservableCollection<T_EReports_Signatures>(signaturesData.Distinct());
        }
        private async void GetAttachmentData(int ID)
        {
           // var gata = await _drawingsRepository.GetAsync();
            var AttachmentData = await _drawingsRepository.QueryAsync<T_Drawings>(@"SELECT * FROM T_Drawings WHERE [EReportID] = "+ ID );
            AttachmentList = new ObservableCollection<T_Drawings>(AttachmentData.Distinct().Where(x => x.FileName.Trim().Contains("pdf")));
        }
        private async void SaveDetailsofRIR()
        {
            CurrentRIR.Date_Inspected = SelectedDate;
            CurrentRIR.Inspection_Remarks = Remark;
            CurrentRIR.Result_Accepted = Accepted;
            CurrentRIR.Result_Damage = Damage;
            CurrentRIR.Result_OffSpec = Off_Sprc;
            string JSONString = ModsTools.ToJson(CurrentRIR);
            await _eReportsRepository.QueryAsync<T_EReports>(@"UPDATE T_EReports SET [JSONString] = '" + JSONString.Replace("'", "''") + "' , [Updated] = '" + 1 + "' WHERE [ID] = " + _selectedEReportItem.ID);          
            var returndata = await _eReportsRepository.QueryAsync<T_EReports>(@"SELECT * FROM T_EReports WHERE [ID] = " + _selectedEReportItem.ID);
            _selectedEReportItem = returndata.FirstOrDefault();
            _userDialogs.AlertAsync("Inspection data saved successfully", "Save Inspection Data", "OK");
            GetDetailsData();

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
                          //  await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "You currently do not have the user rights to sign off this signature", "OK");
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
        private async void SaveSignaturesofRIR()
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
            catch(Exception ex)
            {

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

        #region Public
        public async void ShowDescriptionPopup(string Description)
        {
            if (Description == null)
                return;
            if (!string.IsNullOrWhiteSpace(Description))
                await PopupNavigation.PushAsync(new ShowWrapTextPopup("Item Description", Description), true);
            SelectedRIRReport = null;
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
        #endregion
    }
}

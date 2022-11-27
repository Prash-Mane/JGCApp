using Acr.UserDialogs;
using JGC.Common.Interfaces;
using Prism.Navigation;
using JGC.Common.Extentions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using JGC.DataBase.DataTables.WorkPack;
using JGC.DataBase;
using System.Linq;
using Xamarin.Forms.Internals;
using JGC.DataBase.DataTables;
using JGC.Common.Helpers;
using JGC.Common.Constants;
using JGC.Models.Work_Pack;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using JGC.Common.Converters;
using JGC.Views.Work_Pack;
using Rg.Plugins.Popup.Services;

namespace JGC.ViewModels.Work_Pack
{
    public class CWPTagStatusViewModel : BaseViewModel, INotifyPropertyChanged
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IResizeImageService _resizeImageService;
        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_TagMilestoneStatus> _tagMilestoneStatusRepository;
        private readonly IRepository<T_TagMilestoneImages> _tagMilestoneImagesRepository;
        private readonly IRepository<T_IWP> _iwpRepository;
        private IList<T_TagMilestoneStatus> CWPTagsData;
        private List<TagMilestoneStatusModel> CWPList = new List<TagMilestoneStatusModel>();
        private T_UserDetails userDetail;
        public byte[] imageAsByte = null;
        private readonly IMedia _media;
        private T_UserProject CurrentUserProject;
        private readonly IRepository<T_CwpTag> _CwpTAg;

        #region properties
        private ObservableCollection<TagMilestoneStatusModel> _cwpTagStatus;
        public ObservableCollection<TagMilestoneStatusModel> CWPTagStatusLists
        {
            get { return _cwpTagStatus; }
            set { _cwpTagStatus = value; RaisePropertyChanged(); }
        }
        private TagMilestoneStatusModel _selectedCWPTag;
        public TagMilestoneStatusModel SelectedCWPTag
        {
            get { return _selectedCWPTag; }
            set { _selectedCWPTag = value; RaisePropertyChanged(); }
        }

        private List<string> _cwpTag;
        public List<string> CWPTags
        {
            get { return _cwpTag; }
            set { _cwpTag = value; RaisePropertyChanged(); }
        }

        private string selectedCWPTags;
        public string SelectedCWPTags
        {
            get { return selectedCWPTags; }
            set
            {
                if (SetProperty(ref selectedCWPTags, value))
                {
                    if (HasTagMemberFunctionbality)
                    {
                        BtnText = "Next";
                        IsVisibleTagMemberUi = true;
                        IsVisibleTagStatus = false;
                    }

                    OnClickSelectedCWPTags(selectedCWPTags);
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
        private bool _cameraGrid;
        public bool CameraGrid
        {
            get { return _cameraGrid; }
            set { SetProperty(ref _cameraGrid, value); }
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
        private string _selectedCameraItem;
        public string SelectedCameraItem
        {
            get { return _selectedCameraItem; }
            set
            {
                if (SetProperty(ref _selectedCameraItem, value))
                {
                    //  SelectedCameraItems(_selectedCameraItem);
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

        public CWPTagStatusViewModel(INavigationService _navigationService,
           IUserDialogs _userDialogs,
           IHttpHelper _httpHelper,
           IMedia _media,
           ICheckValidLogin _checkValidLogin,
           IResizeImageService _resizeImageService,
           IRepository<T_UserDetails> _userDetailsRepository,
           IRepository<T_UserProject> _userProjectRepository,
           IRepository<T_TagMilestoneStatus> _tagMilestoneStatusRepository,
           IRepository<T_TagMilestoneImages> _tagMilestoneImagesRepository,
           IRepository<T_CwpTag> _CwpTAg,
           IRepository<T_IWP> _iwpRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._resizeImageService = _resizeImageService;
            this._userDialogs = _userDialogs;
            this._userDetailsRepository = _userDetailsRepository;
            this._userProjectRepository = _userProjectRepository;
            this._tagMilestoneStatusRepository = _tagMilestoneStatusRepository;
            this._tagMilestoneImagesRepository = _tagMilestoneImagesRepository;
            this._iwpRepository = _iwpRepository;
            this._media = _media;
            this._CwpTAg = _CwpTAg;
            _media.Initialize();
            _userDialogs.HideLoading();
            HasTagMemberFunctionbality = false;
            PageHeaderText = "CWP Tag Status";
            btnSaveRename = "Save Image";
            JobSettingHeaderVisible = true;
            GetUserDetails();
            GetCWPTagStatusLists();


        }

        #region private  
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
            // cwp tags member ui trgger
            var CurrrentIWP = await _iwpRepository.QueryAsync<T_IWP>("SELECT * FROM [T_IWP] WHERE [ID] = '" + IWPHelper.IWP_ID + "'");
            var SelectedIWP = CurrrentIWP.Where(x => x.FWBS.StartsWith("6") || x.FWBS.StartsWith("3"));
            if (SelectedIWP.Any())
            {
                HasTagMemberFunctionbality = true;
                BtnText = "Next";
                IsVisibleTagMemberUi = true;
                IsVisibleTagStatus = false;
            }
            else
            {
                BtnText = "Save";
                IsVisibleTagMemberUi = false;
                IsVisibleTagStatus = true;
            }

            string TMSSQL = "SELECT * FROM T_CwpTag WHERE IWPID = '" + IWPHelper.IWP_ID + "' AND ProjectID = '" + Settings.ProjectID + "'";
            var CWPTagNoData = await _CwpTAg.QueryAsync<T_CwpTag>(TMSSQL);

            CWPTags = CWPTagNoData.Select(i=>i.TagNo).Distinct().ToList();

            //string TMSSQL = "SELECT * FROM T_TagMilestoneStatus WHERE IWPID = '" + IWPHelper.IWP_ID + "'";
            //var CWPTagNoData = await _tagMilestoneStatusRepository.QueryAsync<T_TagMilestoneStatus>(TMSSQL);

            //CWPTags = CWPTagNoData.Select(i => i.TagNo).Distinct().ToList();

            //var input4 = await ReadStringInPopup(CWPTags);
        }

        [Obsolete]
        public static Task<string> ReadStringInPopup(List<string> Source)
        {
            var vm = new FilterPopupViewModel(Source);
            //vm.FilterList = Source;
            var tcs = new TaskCompletionSource<string>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var page = new FilterPopup(vm, Source);
                await PopupNavigation.PushAsync(page);
                var value = await vm.GetValue();
                await PopupNavigation.PopAsync(true);
                tcs.SetResult(value);
            });
            return tcs.Task;
        }
        private async void OnClickSelectedCWPTags(string SelectedTagNo)
        {
            //if (HasTagMemberFunctionbality)
            //{
            //    BtnText = "Next";
            //    IsVisibleTagMemberUi = true;
            //    IsVisibleTagStatus = false;
            //}

            if (IsVisibleTagStatus)
            {
                string TMSSQL;
                if (HasTagMemberFunctionbality)
                    TMSSQL = "SELECT * FROM T_TagMilestoneStatus WHERE TagNo='" + SelectedTagNo + "' AND IWPID = '" + IWPHelper.IWP_ID + "' AND TagMember = '" + SelectedCWPTag.TagMember + "'";
                else
                    TMSSQL = "SELECT * FROM T_TagMilestoneStatus WHERE TagNo='" + SelectedTagNo + "' AND IWPID = '" + IWPHelper.IWP_ID + "'";
                CWPTagsData = await _tagMilestoneStatusRepository.QueryAsync<T_TagMilestoneStatus>(TMSSQL);

                CWPList = new List<TagMilestoneStatusModel>();
                foreach (T_TagMilestoneStatus item in CWPTagsData)
                {
                    //bool signedbycms = false;
                    //if (item.SignedBy == "Signed in CMS")
                    //    signedbycms = true;

                    string CImage = IsExistImageFiles(item);
                    CWPList.Add(new TagMilestoneStatusModel
                    {
                        Project_ID = item.Project_ID,
                        IWPID = item.IWPID,
                        CWPTagID = item.CWPTagID,
                        TagNo = item.TagNo,
                        Milestone = item.Milestone,
                        MilestoneAttribute = item.MilestoneAttribute,
                        UpdatedByUserID = item.UpdatedByUserID,

                        // SignedBy = item.SignedBy,
                        StatusValue = item.StatusValue,
                        SignedInVM = item.SignedInVM,
                        CameraImage = CImage,
                        SignedInCMS = item.SignedInCMS,
                        TagMember = item.TagMember
                    });

                }
                //  if (HasTagMemberFunctionbality) CWPList.ForEach(x=> { x.MilestoneAtri = ""; });
            }
            else
            {
                string TMSSQL = "SELECT DISTINCT TagMember,TagNo  FROM T_TagMilestoneStatus WHERE TagNo='" + SelectedTagNo + "' AND IWPID = '" + IWPHelper.IWP_ID + "' ORDER BY TagMember";
                CWPTagsData = await _tagMilestoneStatusRepository.QueryAsync<T_TagMilestoneStatus>(TMSSQL);

                CWPList = new List<TagMilestoneStatusModel>();
                foreach (T_TagMilestoneStatus item in CWPTagsData)
                {

                    CWPList.Add(new TagMilestoneStatusModel
                    {
                        TagNo = item.TagNo,
                        TagMember = item.TagMember

                    });
                }
            }
            CWPTagStatusLists = new ObservableCollection<TagMilestoneStatusModel>(CWPList);
        }

        private async void SearchTagMemebr()
        {
            string TMSSQL = "SELECT DISTINCT TagMember,TagNo  FROM T_TagMilestoneStatus WHERE TagNo='" + SelectedCWPTags + "' AND TagMember LIKE '" + SerachString + "%' AND IWPID = '" + IWPHelper.IWP_ID + "' ORDER BY TagMember";
            CWPTagsData = await _tagMilestoneStatusRepository.QueryAsync<T_TagMilestoneStatus>(TMSSQL);

            CWPList = new List<TagMilestoneStatusModel>();
            foreach (T_TagMilestoneStatus item in CWPTagsData)
            {

                CWPList.Add(new TagMilestoneStatusModel
                {
                    TagNo = item.TagNo,
                    TagMember = item.TagMember

                });
            }
            CWPTagStatusLists = new ObservableCollection<TagMilestoneStatusModel>(CWPList);
        }


        //private void SelectedCameraItems(string SelectedCameraItem)
        //{
        //    if (SelectedCameraItem == null)
        //        return;
        //  //  CameraService();
        //}
        //private async void CameraService()
        //{
        //    try
        //    {
        //        var mediaFile = await TakePhotoAsync();
        //        if (mediaFile == null)
        //        {
        //            return;
        //        }
        //        var memoryStream = new MemoryStream();
        //        await mediaFile.GetStream().CopyToAsync(memoryStream);
        //        imageAsByte = memoryStream.ToArray();
        //        Stream stream = new MemoryStream(imageAsByte);
        //        CapturedImage = ImageSource.FromStream(() => stream);
        //        btnSaveRename = "Save Image";
        //        SelectedImageFiles= null;

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        public async void SavePickedImageFromGallery(MediaFile mediaFile)
        {
            try
            {
                //var mediaFile = await PickFileAsync();
                //if (mediaFile == null)
                //{
                //    return;
                //}
                //try
                //{
                //var memoryStream = new MemoryStream();
                //await mediaFile.GetStream().CopyToAsync(memoryStream);
                //imageAsByte = null;
                //imageAsByte = memoryStream.ToArray();
                //Stream stream = new MemoryStream(imageAsByte);
                //CapturedImage = ImageSource.FromStream(() => stream);

                if (SelectedCWPTag != null)
                {
                    T_TagMilestoneImages pickedImage = new T_TagMilestoneImages
                    {
                        Project_ID = SelectedCWPTag.Project_ID,
                        CWPID = SelectedCWPTag.CWPTagID,
                        Milestone = SelectedCWPTag.Milestone,
                        MilestoneAttribute = SelectedCWPTag.MilestoneAttribute,
                        DisplayName = Path.GetFileNameWithoutExtension(mediaFile.Path),
                        FileBytes = Convert.ToBase64String(imageAsByte),
                        Extension = Path.GetExtension(mediaFile.Path),
                        FileName = Path.GetFileName(mediaFile.Path),
                        FileSize = imageAsByte.Length,
                        Updated = true,
                        TagMember = SelectedCWPTag.TagMember
                    };
                    await _tagMilestoneImagesRepository.InsertOrReplaceAsync(pickedImage);
                    BindImageFiles(pickedImage.DisplayName);
                    btnSaveRename = "Rename Image";
                    UpdatedWorkPack(IWPHelper.IWP_ID);
                    await _userDialogs.AlertAsync("Image has been added successfully", "Image added", "Ok");
                }
            }
            catch (Exception ex)
            {
                await _userDialogs.AlertAsync("Error", "Unable to add image", "Ok");
            }
            //}
            //catch (Exception ex)
            //{

            //}
        }
        public async Task<MediaFile> TakePhotoAsync()
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
            catch (Exception ex) { }

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
            if (param == "ClickedRadioBtn")
            {
                if (SelectedCWPTag != null)
                {
                    T_TagMilestoneStatus DBlist = CWPTagsData.Where(i => !i.SignedInVM && !i.SignedInCMS && i.Milestone == SelectedCWPTag.Milestone && i.MilestoneAttribute == SelectedCWPTag.MilestoneAttribute).FirstOrDefault();
                    if (DBlist != null && !DBlist.SignedInVM && !DBlist.SignedInCMS)
                    {
                        CWPList.Where(i => i.Milestone == SelectedCWPTag.Milestone && i.MilestoneAttribute == SelectedCWPTag.MilestoneAttribute && i.TagMember == SelectedCWPTag.TagMember).ToList()
                                    .ForEach(x => { x.UpdatedByUserID = userDetail.ID.ToString(); x.StatusValue = DateTime.Now; x.SignedInVM = !x.SignedInVM; x.IsUpdated = !x.IsUpdated; });  // DateTime.Now.ToString(AppConstant.DateSaveFormat)
                        CWPTagStatusLists = new ObservableCollection<TagMilestoneStatusModel>(CWPList);
                        CWPTagStatusLists = new ObservableCollection<TagMilestoneStatusModel>(CWPList);
                    }
                    else if (DBlist != null)
                    {
                        CWPList.Where(i => i.Milestone == SelectedCWPTag.Milestone && i.MilestoneAttribute == SelectedCWPTag.MilestoneAttribute && i.TagMember == SelectedCWPTag.TagMember).ToList()
                                                            .ForEach(x => { x.UpdatedByUserID = DBlist.UpdatedByUserID; x.StatusValue = DBlist.StatusValue; x.SignedInVM = !x.SignedInVM; x.IsUpdated = !x.IsUpdated; });  // DateTime.Now.ToString(AppConstant.DateSaveFormat)
                        CWPTagStatusLists = new ObservableCollection<TagMilestoneStatusModel>(CWPList);
                        CWPTagStatusLists = new ObservableCollection<TagMilestoneStatusModel>(CWPList);
                    }
                    else if (SelectedCWPTag.SignedInCMS)
                        await _userDialogs.AlertAsync("This milestone has been signed in CMS and cant be removed from this device ", "Unable to remove", "Ok");
                    else
                        await _userDialogs.AlertAsync("This milestone cant be removed from this device ", "Unable to remove", "Ok");
                }
            }
            else if (param == "Back")
            {
                await navigationService.GoBackAsync();
            }
            else if (param == "Clear")
            {
                CapturedImage = null;
                SelectedImageFiles = null;
                SelectedCameraItem = null;
            }
            else if (param == "Save")
            {
                if (IsVisibleTagMemberUi)
                {
                    if (SelectedCWPTag == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Tag Member", "Please Select Tag Member", "OK");
                        return;
                    }
                    else
                    {
                        ISVisibleBackbtn = true;
                        string TMSSQL = "SELECT * FROM T_TagMilestoneStatus WHERE TagNo='" + SelectedCWPTags + "' AND IWPID = '" + IWPHelper.IWP_ID + "'  AND TagMember = '" + SelectedCWPTag.TagMember + "'";
                        CWPTagsData = await _tagMilestoneStatusRepository.QueryAsync<T_TagMilestoneStatus>(TMSSQL);

                        CWPList = new List<TagMilestoneStatusModel>();
                        foreach (T_TagMilestoneStatus item in CWPTagsData)
                        {
                            //bool signedbycms = false;
                            //if (item.SignedBy == "Signed in CMS")
                            //    signedbycms = true;
                            string CImage = IsExistImageFiles(item);
                            CWPList.Add(new TagMilestoneStatusModel
                            {
                                Project_ID = item.Project_ID,
                                IWPID = item.IWPID,
                                CWPTagID = item.CWPTagID,
                                TagNo = item.TagNo,
                                Milestone = item.Milestone,
                                MilestoneAttribute = item.MilestoneAttribute,
                                UpdatedByUserID = item.UpdatedByUserID,
                                // SignedBy = item.SignedBy,
                                StatusValue = item.StatusValue,
                                SignedInVM = item.SignedInVM,
                                CameraImage = CImage,
                                SignedInCMS = item.SignedInCMS,
                                TagMember = item.TagMember


                            });

                        }
                        CWPList.ForEach(x => { x.MilestoneAtri = ""; });
                        CWPTagStatusLists = new ObservableCollection<TagMilestoneStatusModel>(CWPList);
                        CWPTagStatusLists = new ObservableCollection<TagMilestoneStatusModel>(CWPList);
                        IsVisibleTagMemberUi = false;
                        IsVisibleTagStatus = true;
                        BtnText = "Save";
                    }
                }
                else
                {
                    if (CWPTags == null)
                    {
                        await _userDialogs.AlertAsync("No tags available to save", "Alert", "Ok");
                        return;
                    }
                    if (SelectedCWPTags == null)
                    {
                        await _userDialogs.AlertAsync("Please select a tag before saving", "Alert", "Ok");
                        return;
                    }


                    var isinvalid = CWPTagStatusLists.Where(i => i.StatusValue > DateTime.Now).ToList();
                    if (isinvalid.Count > 0)
                    {
                        await _userDialogs.AlertAsync("Please select a date same or older than current date", "Invalid date", "Ok");
                        CWPTagStatusLists = new ObservableCollection<TagMilestoneStatusModel>(CWPTagStatusLists);
                    }
                    else
                    {
                        if (await _userDialogs.ConfirmAsync($"Are you sure you want to save this CWP tags?", $"Save CWP tags", "Yes", "No"))
                        {
                            foreach (TagMilestoneStatusModel item in CWPTagStatusLists.Where(i => i.IsUpdated == true))
                            {
                                string ticksDate = null;
                                if (item.StatusValue.HasValue)
                                {
                                    ticksDate = Convert.ToDateTime(item.StatusValue.ToString()).Ticks.ToString();
                                }

                                string updatedSQL = "UPDATE T_TagMilestoneStatus SET UpdatedByUserID = '" + item.UpdatedByUserID + "', SignedInVM = 1"
                                                  + ", StatusValue = '" + ticksDate + "', Updated = 1 "
                                                  + " WHERE Milestone = '" + item.Milestone + "' AND MilestoneAttribute = '" + item.MilestoneAttribute + "'AND TagNo =  '" + item.TagNo + "'AND TagMember =  '" + item.TagMember + "'";
                                //                  + " WHERE Milestone = '" + item.Milestone + "' AND MilestoneAttribute = '" + item.MilestoneAttribute + "' AND CWPTagID =  '"+item.CWPTagID+"'";
                                await _tagMilestoneStatusRepository.QueryAsync<T_TagMilestoneStatus>(updatedSQL);
                            }
                            OnClickSelectedCWPTags(selectedCWPTags);
                            await _userDialogs.AlertAsync("", "Tags saved successfully", "Ok");
                            UpdatedWorkPack(IWPHelper.IWP_ID);
                        }
                    }
                }
            }
            else if (param == "CameraBack")
            {
                BtnText = "Save";
                IsVisibleTagMemberUi = false;
                IsVisibleTagStatus = true;
                OnClickSelectedCWPTags(SelectedCWPTags);
                CameraGrid = false;

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
            else if (param == "Save Image")
            {
                if (CapturedImage != null)
                {
                    if (SelectedCWPTag != null)
                    {
                        string fileName = GetNewImageDisplayName();
                        T_TagMilestoneImages pickedImage = new T_TagMilestoneImages
                        {
                            Project_ID = SelectedCWPTag.Project_ID,
                            CWPID = SelectedCWPTag.CWPTagID,
                            Milestone = SelectedCWPTag.Milestone,
                            MilestoneAttribute = SelectedCWPTag.MilestoneAttribute,
                            DisplayName = fileName,
                            FileBytes = Convert.ToBase64String(imageAsByte),
                            Extension = ".jpg",
                            FileName = fileName + ".jpg",
                            FileSize = imageAsByte.Length,
                            Updated = true,
                            TagMember = SelectedCWPTag.TagMember
                        };
                        await _tagMilestoneImagesRepository.InsertOrReplaceAsync(pickedImage);
                        BindImageFiles(fileName);
                        await _userDialogs.AlertAsync("Image has been saved successfully", "Image saved", "Ok");
                        //SelectedImageFiles = ImageFiles.Where(i => i.DisplayName == fileName).FirstOrDefault();
                        UpdatedWorkPack(IWPHelper.IWP_ID);
                        btnSaveRename = "Rename Image";
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert(null, "Please select image before saving", "OK");
            }
            else if (param == "SaveRename")
            {
                if (SelectedImageFiles != null)
                {
                    string renameSQL = "UPDATE T_TagMilestoneImages SET DisplayName ='" + NewImageName + "', FileName = '" + NewImageName + SelectedImageFiles.Extension + "', Updated = 1 "
                                     + " Where DisplayName = '" + SelectedImageFiles.DisplayName + "' AND CWPID = '" + SelectedImageFiles.CWPID + "' AND Project_ID = '" + SelectedImageFiles.Project_ID
                                     + "' AND Milestone = '" + SelectedImageFiles.Milestone + "' AND MilestoneAttribute = '" + SelectedImageFiles.MilestoneAttribute + "' AND TagMember = '" + SelectedImageFiles.TagMember + "'";
                    await _tagMilestoneImagesRepository.QueryAsync<T_TagMilestoneImages>(renameSQL);
                    BindImageFiles(NewImageName);
                    ShowSaveRename = false;
                    await _userDialogs.AlertAsync("Image has been renamed successfully", "Image renamed", "Ok");
                    UpdatedWorkPack(IWPHelper.IWP_ID);
                }
            }
            else if (param == "CancelRename")
            {
                ShowSaveRename = false;
            }
            //else if (param == "AddFromFile")
            //{
            //   // PickImagesFromGallery();
            //}
            else if (param == "DeleteImage")
            {
                if (SelectedImageFiles != null)
                {
                    string renameSQL = "DELETE FROM T_TagMilestoneImages Where DisplayName = '" + SelectedImageFiles.DisplayName + "' AND CWPID = '" + SelectedImageFiles.CWPID
                                 + "' AND Project_ID = '" + SelectedImageFiles.Project_ID + "' AND Milestone = '" + SelectedImageFiles.Milestone
                                 + "' AND MilestoneAttribute = '" + SelectedImageFiles.MilestoneAttribute + "' AND TagMember = '" + SelectedImageFiles.TagMember + "'";
                    await _tagMilestoneImagesRepository.QueryAsync<T_TagMilestoneImages>(renameSQL);
                    OnClickButton("Clear");
                    BindImageFiles("");
                    await _userDialogs.AlertAsync("Image has been deleted successfully", "Image deleted", "Ok");
                    UpdatedWorkPack(IWPHelper.IWP_ID);
                }
                else
                    await Application.Current.MainPage.DisplayAlert(null, "Please select image before deleting", "OK");
            }
            else if (param == "Search")
            {
                SearchTagMemebr();
            }
            else if (param == "Selectedcwptag")
            {
                if (CWPTags == null) return;

                var inputResult = await ReadStringInPopup(CWPTags);
                if (!string.IsNullOrWhiteSpace(inputResult) && inputResult != "clear") SelectedCWPTags = inputResult;
                else SelectedCWPTags = null;

            }
            else if (param == "Backfrommilestone")
            {
                IsVisibleTagStatus = false;
                IsVisibleTagMemberUi = true;
                ISVisibleBackbtn = false;
                BtnText = "Next";
                OnClickSelectedCWPTags(SelectedCWPTags);
            }
        }

        public async Task BindImageFiles(string fileName)
        {
            string TagMilestoneImageSQL = "Select * from T_TagMilestoneImages Where CWPID ='" + SelectedCWPTag.CWPTagID + "' AND Milestone = '" + SelectedCWPTag.Milestone + "'AND MilestoneAttribute = '" + SelectedCWPTag.MilestoneAttribute + "'";
            var TagMilestoneImages = await _tagMilestoneImagesRepository.QueryAsync<T_TagMilestoneImages>(TagMilestoneImageSQL);
            ImageFiles = new ObservableCollection<T_TagMilestoneImages>(TagMilestoneImages);
            if (fileName != "")
                SelectedImageFiles = ImageFiles.Where(i => i.DisplayName == fileName).FirstOrDefault();
        }
        public string IsExistImageFiles(T_TagMilestoneStatus item)
        {
            string cameraImage = "cam.png";
            string TagMilestoneImageSQL = "Select * from T_TagMilestoneImages Where CWPID ='" + item.CWPTagID + "' AND Milestone = '" + item.Milestone + "' AND MilestoneAttribute = '" + item.MilestoneAttribute + "'";
            var data = Task.Run(async () => await _tagMilestoneImagesRepository.QueryAsync<T_TagMilestoneImages>(TagMilestoneImageSQL));

            List<T_TagMilestoneImages> TagMilestoneImages = data.Result.ToList();
            if (TagMilestoneImages.Count > 0)
                cameraImage = "Greencam.png";
            return cameraImage;

        }
        public async Task<byte[]> ResizeImage(byte[] imageAsByte)
        {
            return await _resizeImageService.GetResizeImage(imageAsByte);
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
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

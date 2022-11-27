using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
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

namespace JGC.ViewModels.E_Reporter
{
   public class AttachmentViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IMedia _media;
        private readonly IRepository<T_Drawings> _drawingsRepository;
        private string AttachmentPath;
        public byte[] imageAsByte = null;

        #region properties
        private ObservableCollection<T_Drawings> attachmentsFileList;

        public ObservableCollection<T_Drawings> AttachmentsFileList
        {
            get { return attachmentsFileList; }
            set { attachmentsFileList = value; RaisePropertyChanged(); }
        }
        private T_Drawings selectedAttachment { get; set; }
        public T_Drawings SelectedAttachment
        {
            get { return selectedAttachment; }
            set { 
                selectedAttachment = value; 
                RaisePropertyChanged();
                if(selectedAttachment != null) 
                {
                    SelectedImageChanged(selectedAttachment);
                }
            }
        }

        private void SelectedImageChanged(T_Drawings selectedAttachment)
        {
            AttachmentName = selectedAttachment.Name;
            PdfUrl = selectedAttachment.FileLocation; //AttachmentPath +"\\"+ selectedAttachment;
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
        public bool isVisibleAttachmentGrid { get; set; }
        public bool IsVisibleAttachmentGrid
        {
            get { return isVisibleAttachmentGrid; }
            set { isVisibleAttachmentGrid = value; RaisePropertyChanged(); }
        }
        public bool isVisibleCameraGrid { get; set; }
        public bool IsVisibleCameraGrid
        {
            get { return isVisibleCameraGrid; }
            set { isVisibleCameraGrid = value; RaisePropertyChanged(); }
        }
        public string _AttachmentName { get; set; }
        public string AttachmentName
        {
            get { return _AttachmentName; }
            set { _AttachmentName = value; RaisePropertyChanged(); }
        }
        #endregion
        #region Delegate Commands  
        public ICommand OnClickCommand
        {
            get
            {
                return new Command<string>(OnClicked);
            }
        }

        public async void OnClicked(string param)
        {
            if (param == "VisibleCameraGrid")
            {
                IsVisibleAttachmentGrid = false;
                IsVisibleCameraGrid = true;
            }
            else if (param == "BackToAttachment")
            {
                IsVisibleCameraGrid = false;
                IsVisibleAttachmentGrid = true;
            }
            else if(param == "AddFromFiles")
            {
                FileData fileData = await DependencyService.Get<ISaveFiles>().PickPDF(""); //await CrossFilePicker.Current.PickFile();
                if (fileData.FileName != null)
                {
                    if (Path.GetExtension(fileData.FileName).ToLower() == ".pdf")
                    {
                        T_Drawings Drawing = new T_Drawings
                        {
                            EReportID = DWRHelper.SelectedDWR.ID,
                            RowID = DWRHelper.SelectedDWR.RowID,
                            Sheet_No = DWRHelper.SelectedDWR.SheetNo,
                            Revision = DWRHelper.SelectedDWR.RevNo,
                            JobCode = Settings.JobCode,
                            Name = Path.GetFileNameWithoutExtension(fileData.FileName),
                            Total_Sheets = "",
                            FileName = fileData.FileName,
                        };
                        string DWRFolder = ("PDF Store" + "\\" + Settings.JobCode + "\\" + DWRHelper.SelectedDWR.ID);
                        byte[] DWRPDFBytes = fileData.DataArray; //Convert.FromBase64String(BinaryCode);
                        Drawing.FileLocation = await DependencyService.Get<ISaveFiles>().SavePDFToDisk(DWRFolder, Device.RuntimePlatform == Device.UWP? fileData.FileName : Path.GetFileNameWithoutExtension(fileData.FileName), DWRPDFBytes);
                        await _drawingsRepository.InsertOrReplaceAsync(Drawing);
                        Task.Delay(2000);
                        GetAttachments();
                    }
                    else
                    {
                        await _userDialogs.AlertAsync("Please select PDF file", "", "OK");
                    }
                }
            }
            else if(param == "DeletePDF")
            {
                if(SelectedAttachment != null)
                {
                    if (await _userDialogs.ConfirmAsync($"Are you sure you want to delete attachment?", $"Delete Attachment", "Yes", "No"))
                    {
                        await _drawingsRepository.QueryAsync(@" DELETE FROM [T_Drawings] WHERE [EReportID] = '" + SelectedAttachment.EReportID + "'"
                                                       + " AND [RowID] = '" + SelectedAttachment.RowID + "' AND [Name] = '" + SelectedAttachment.Name + "'");
                        SelectedAttachment = null;
                        AttachmentName = string.Empty;
                        await _userDialogs.AlertAsync("Attachment has been deleted successfully", "", "OK");
                        GetAttachments();
                    }
                }
                else
                await _userDialogs.AlertAsync("Please select attachment", "", "OK");
            }
            else if (param == "RenamePDF")
            {
                if (SelectedAttachment != null && !String.IsNullOrEmpty(AttachmentName))
                {
                    if (await _userDialogs.ConfirmAsync($"Are you sure you want to rename attachment?", $"Rename Attachment", "Yes", "No"))
                    {
                        string fileName = AttachmentName + Path.GetExtension(SelectedAttachment.FileName);
                        await _drawingsRepository.QueryAsync(@" UPDATE [T_Drawings] SET [Name] = '" + AttachmentName + "', [FileName] = '" + fileName + "'"
                                                       + " WHERE [EReportID] = '" + SelectedAttachment.EReportID + "'"
                                                       + " AND [RowID] = '" + SelectedAttachment.RowID + "' AND [Name] = '" + SelectedAttachment.Name + "'");
                        SelectedAttachment = null;
                        AttachmentName = string.Empty;
                        await _userDialogs.AlertAsync("Attachment has been renamed successfully", "", "OK");
                        GetAttachments();
                    }
                }
                else
                    await _userDialogs.AlertAsync("Please select attachment", "", "OK");
            }
        }


        #endregion


        public AttachmentViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IMedia _media,
            IRepository<T_Drawings> _drawingsRepository
           ) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._media = _media;
            this._drawingsRepository = _drawingsRepository;
            _userDialogs.HideLoading();
            IsHeaderBtnVisible = IsVisibleAttachmentGrid = true;
            PageHeaderText = "Joint Details";
            PdfUrl = @"C:\\Users\\STW\\Documents\\PDF Store\\0-7723-2\\IWP-2300-A000-001.pdf";
            _media.Initialize();
            DWRHelper.DWRTargetType = typeof(AttachmentViewModel);
            GetAttachments();
            IsVisibleCameraGrid = false;
        }

        private async void GetAttachments()
        {
           var Drawings = await _drawingsRepository.GetAsync(x=>x.RowID == DWRHelper.SelectedDWR.RowID && x.EReportID == DWRHelper.SelectedDWR.ID);
            List<T_Drawings> DrawingList = Drawings.ToList();
            AttachmentsFileList = new ObservableCollection<T_Drawings>(DrawingList);
        }
        #region

        #endregion

        #region Public

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

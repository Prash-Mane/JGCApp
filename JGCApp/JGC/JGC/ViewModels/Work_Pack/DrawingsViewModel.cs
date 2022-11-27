using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables.WorkPack;
using JGC.Models;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using JGC.Common.Extentions;
using System.Windows.Input;
using System.Threading.Tasks;

namespace JGC.ViewModels.Work_Pack
{
    public class DrawingsViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_IWP> _iwpRepository;
        private readonly IRepository<T_IWPDrawings> _iwpDrawingsRepository;
        private readonly IRepository<T_IWPAttachments> _iwpAttachmentsRepository;
        private T_IWP CurrentIWP;
        public T_IWPAttachments CurrentDrawings;

        #region Properties

        private ObservableCollection<T_IWPAttachments> drawingsList;
        public ObservableCollection<T_IWPAttachments> DrawingsList
        {
            get { return drawingsList; }
            set { drawingsList = value; RaisePropertyChanged(); }
        }
        private T_IWPAttachments selecteddrawings;
        public T_IWPAttachments SelectedDrawings
        {
            get { return selecteddrawings; }
            set { selecteddrawings = value; RaisePropertyChanged(); }
        }
        private ImageSource drawingsimage;
        public ImageSource Drawingsimage
        {
            get { return drawingsimage; }
            set { drawingsimage = value; RaisePropertyChanged(); }
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

        public DrawingsViewModel(INavigationService _navigationService,
           IUserDialogs _userDialogs,
           IHttpHelper _httpHelper,
           ICheckValidLogin _checkValidLogin,
           IRepository<T_IWP> _iwpRepository,
           IRepository<T_IWPDrawings> _iwpDrawingsRepository,
           IRepository<T_IWPAttachments> _iwpAttachmentsRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._iwpRepository = _iwpRepository;
            this._iwpDrawingsRepository = _iwpDrawingsRepository;
            this._iwpAttachmentsRepository = _iwpAttachmentsRepository;
            _userDialogs.HideLoading();
            PageHeaderText = "DrawingsPage";
            JobSettingHeaderVisible = true;
            ShowPDFs = "Greenradio.png";
            ShowImages = "Grayradio.png";
            PDFview = true;
            ImagesView = false;


        }

        #region Private

        private void GetDrawingList(bool IsPDF)
        {
            var getIWP = Task.Run(async () => await _iwpRepository.QueryAsync<T_IWP>("SELECT * FROM [T_IWP] WHERE [ID] = '" + IWPHelper.IWP_ID + "'")).Result;

            CurrentIWP = getIWP.FirstOrDefault();

            string SQLdrawing = "SELECT * FROM [T_IWPDrawings] WHERE [IWPID] = '" + IWPHelper.IWP_ID + "'";

            var Dlist = Task.Run(async () => await _iwpDrawingsRepository.QueryAsync<T_IWPAttachments>(SQLdrawing)).Result;

            string SQLAttachment = "SELECT * FROM [T_IWPAttachments] WHERE [LinkedID] = '" + IWPHelper.IWP_ID + "'";

            var Alist = Task.Run(async () => await _iwpAttachmentsRepository.QueryAsync<T_IWPAttachments>(SQLAttachment)).Result;

            foreach(T_IWPAttachments attached in Alist)
            {
                T_IWPAttachments addAttached = new T_IWPAttachments
                {
                    FileBytes = attached.FileBytes,
                    FileName = attached.FileName,
                  //  JobCode = CurrentIWP.JobCode,
                    DisplayName = Path.GetFileNameWithoutExtension(attached.FileName),
                    ProjectID = attached.ProjectID,
                    LinkedID = attached.LinkedID,
                    Extension = attached.Extension
                };
                Dlist.Add(addAttached);
            }
            if(IsPDF)
                DrawingsList = new ObservableCollection<T_IWPAttachments>(Dlist.Where(i => Path.GetExtension(i.FileName.ToLower()) == ".pdf" && i.Extension.ToLower() == ".pdf"));
            else
                DrawingsList = new ObservableCollection<T_IWPAttachments>(Dlist.Where(i => Path.GetExtension(i.FileName.ToLower()) != ".pdf" && i.Extension != null && i.Extension.ToLower()!= ".pdf"));

        }
        public async void LoadDrawings(T_IWPAttachments selecteditem)
        {
            if (selecteditem == null)
                return;
                CurrentDrawings = selecteditem;
                string PDFpath = await DependencyService.Get<ISaveFiles>().GetIWPFileLocation("PDF Store\\" + CurrentIWP.JobCode + "\\Drawings\\" + IWPHelper.IWP_ID);

                PdfUrl = PDFpath + "\\" + selecteditem.FileName;           
        }
        public async void OnClickButton(string param)
        {
            if (param == "PdfFullScreen")
            {
                PdfUriModel pdfuri = new PdfUriModel();
                pdfuri.PdfUriPath = PdfUrl;
                if (pdfuri.PdfUriPath != null)
                {
                    var navigationParameters = new NavigationParameters();
                    navigationParameters.Add("uri", pdfuri);
                    navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
                    await navigationService.NavigateAsync<PDFvieverViewModel>(navigationParameters);
                }
            }
            if (param == "ShowPDFs")
            {
                ShowPDFs = "Greenradio.png";
                ShowImages = "Grayradio.png";
                PDFview = true;
                ImagesView = false;
                GetDrawingList(true);
                if(CurrentDrawings != null)
                    SelectedDrawings = DrawingsList.Where(i=>i.DisplayName == CurrentDrawings.DisplayName).Select(i=>i).FirstOrDefault();
            }
            if (param == "ShowImages")
            {
                ShowImages = "Greenradio.png";
                ShowPDFs = "Grayradio.png";
                ImagesView = true;
                PDFview = false;
                GetDrawingList(false);
            }
        }
        #endregion

        #region Public
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            GetDrawingList(true);
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        #endregion
    }
}

using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.Models;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.Common.Extentions;
using JGC.Common.Helpers;

namespace JGC.ViewModels.E_Reporter
{
   public class DWRDrawingViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_Drawings> _drawingsRepository;


        #region Properties

        private string radioBtnSourceImage { get; set; }
        public string RadioBtnSourceImage
        {
            get { return radioBtnSourceImage; }
            set { radioBtnSourceImage = value; RaisePropertyChanged(); }
        }

        private string radioBtnSourcePdf { get; set; }
        public string RadioBtnSourcePdf
        {
            get { return radioBtnSourcePdf; }
            set { radioBtnSourcePdf = value; RaisePropertyChanged(); }
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
        private T_DWR _SelectedDWRReport;
        public T_DWR SelectedDWRReport
        {
            get { return _SelectedDWRReport; }
            set { _SelectedDWRReport = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_Drawings> _attachmentList;
        public ObservableCollection<T_Drawings> AttachmentList
        {
            get { return _attachmentList; }
            set { _attachmentList = value; RaisePropertyChanged(); }
        }

        private bool isShowBackButton;
        public bool IsShowBackButton
        {
            get { return isShowBackButton; }
            set { isShowBackButton = value; RaisePropertyChanged(); }
        }
        #endregion

        #region Delegate Commands  
        public ICommand BtnCommand
        {
            get
            {
                return new Command<string>(BtnClicked);
            }
        }

        private async void BtnClicked(string param)
        {
            if (param == "pdf")
            {

                RadioBtnSourcePdf = "Greenradio.png";
                RadioBtnSourceImage = "Grayradio.png";
            }
            else if (param == "Images") 
            {
                RadioBtnSourcePdf = "Grayradio.png";
                RadioBtnSourceImage = "Greenradio.png";
            }
            else if (param == "BackBtn")
            {
                if (DWRHelper.DWRTargetType == typeof(InspectJointViewModel))
                {
                    var navigationParameters = new NavigationParameters();
                    navigationParameters.Add(NavigationParametersConstants.SelectedDWRReport, DWRHelper.SelectedDWR);
                    navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
                    await navigationService.NavigateFromMenuAsync(typeof(InspectJointViewModel), navigationParameters);
                }
                else
                    await navigationService.NavigateFromMenuAsync(DWRHelper.DWRTargetType);
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
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add(NavigationParametersConstants.SelectedDWRReport, SelectedDWRReport);
                navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
                await navigationService.NavigateFromMenuAsync(typeof(InspectJointViewModel), navigationParameters);
            }
          
        }

        #endregion


        public DWRDrawingViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_Drawings> drawingsRepository
           ) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._drawingsRepository = drawingsRepository;
            _userDialogs.HideLoading();
            //DWRHelper.DWRTargetType = typeof(DWRDrawingViewModel);
            IsHeaderBtnVisible = true;
            PageHeaderText = "Joint Details";
            RadioBtnSourcePdf = "Greenradio.png";
            RadioBtnSourceImage = "Grayradio.png";
          
        }

        private async void GetAttachmentData(int reportid)
        {
            if (reportid == 0)
            {
                var AttachmentData = await _drawingsRepository.GetAsync(x => x.RowID == DWRHelper.SelectedDWR.RowID && x.EReportID == DWRHelper.SelectedDWR.ID);
                AttachmentList = new ObservableCollection<T_Drawings>(AttachmentData.GroupBy(x => x.Name, (key, group) => group.First()));
                

                if (AttachmentList != null)
                {
                    SelectedAttachedItem = AttachmentList.FirstOrDefault(); //SelectedDWRRow.s
                }
            }
            else if (reportid > 0) 
            {
                var AttachmentData = await _drawingsRepository.QueryAsync<T_Drawings>(@"SELECT * FROM T_Drawings WHERE Ereportid = " + reportid + "");
                AttachmentList = new ObservableCollection<T_Drawings>(AttachmentData.Distinct());

                if (AttachmentList != null)
                {
                    SelectedAttachedItem = AttachmentList.FirstOrDefault(); //SelectedDWRRow.s
                }
            }

         
        }

        private async void GetAttachmentDataFromID(int ID)
        {

            var AttachmentData = await _drawingsRepository.QueryAsync<T_Drawings>(@"SELECT * FROM T_Drawings");
            AttachmentList = new ObservableCollection<T_Drawings>(AttachmentData.Where(x=>x.EReportID == ID).Distinct());

            if (AttachmentList != null)
            {
                SelectedAttachedItem = AttachmentList.FirstOrDefault(); //SelectedDWRRow.s
            }
        }

        #region Public

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("id"))
            {
                SelectedDWRReport = (T_DWR)parameters["id"];
                GetAttachmentData(SelectedDWRReport.ID);
                IsShowBackButton = true;
            }
            else 
            {
                GetAttachmentData(0);
                IsShowBackButton = false;
            }

        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        #endregion
    }
}

using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Interfaces;
using JGC.Models;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using JGC.Common.Extentions;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.Common.Helpers;
using JGC.DataBase.DataTables.WorkPack;
using JGC.DataBase;
using System.Linq;

namespace JGC.ViewModels.Work_Pack
{
    public class IWPPdfViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;

        private readonly IRepository<T_IWP> _iwpRepository;
        private readonly IRepository<T_IWPStatus> _iwpStatusRepository;
        private readonly IRepository<T_Predecessor> _predecessorRepository;
        private readonly IRepository<T_Successor> _successorRepository;

        private T_IWP CurrentIWP;

        public IWPPdfViewModel(INavigationService _navigationService,
           IUserDialogs _userDialogs,
           IHttpHelper _httpHelper,
           ICheckValidLogin _checkValidLogin,
           IRepository<T_IWP> _iwpRepository,
           IRepository<T_IWPStatus> _iwpStatusRepository,
           IRepository<T_Predecessor> _predecessorRepository,
           IRepository<T_Successor> _successorRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._iwpRepository = _iwpRepository;
            this._iwpStatusRepository = _iwpStatusRepository;
            this._predecessorRepository = _predecessorRepository;
            this._successorRepository = _successorRepository;
            _userDialogs.HideLoading();
            PageHeaderText = "IWP PDF";
            JobSettingHeaderVisible = true;
            IWPPdfGrid = true;
            LoadIWP();
        }
        #region properties
        private string pdfUrl;
        public string PdfUrl
        {
            get { return pdfUrl; }
            set
            {
                SetProperty(ref pdfUrl, value); RaisePropertyChanged(); 
            }
            
        }

        private bool isVisiblePdfFull;
        public bool IsVisiblePdfFull
        {
            get { return isVisiblePdfFull; }
            set { SetProperty(ref isVisiblePdfFull, value); }
        }


        private bool iwppdfGrid;
        public bool IWPPdfGrid
        {
            get { return iwppdfGrid; }
            set { SetProperty(ref iwppdfGrid, value); }
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
        public ICommand SearchCommand => new Command(OnSearchClick);
        #endregion
        private async void OnSearchClick(object obj)
        {
            await navigationService.NavigateAsync<IWPSelectionViewModel>();
        }
        #region Private
        private async void LoadIWP()
        {
            IWPHelper.PreviousIWP_ID = 0;
            LoadIWPTab();
        }
        private async void LoadIWPTab()
        {
            // our code
            var getIWP = await _iwpRepository.QueryAsync<T_IWP>("SELECT * FROM [T_IWP] WHERE [ID] = '" + IWPHelper.IWP_ID +"'");

            CurrentIWP = getIWP.FirstOrDefault();
            if(CurrentIWP != null)
            {
                var getIWPStatus = await _iwpStatusRepository.QueryAsync<T_IWPStatus>("SELECT * FROM [T_IWPStatus] WHERE [IWP_ID] = '" + IWPHelper.IWP_ID + "'");
                CurrentIWP.IWPStatusList = getIWPStatus.ToList();

                var getPredecessor = await _predecessorRepository.QueryAsync<T_Predecessor>("SELECT * FROM [T_Predecessor] WHERE [IWP_ID] = '" + IWPHelper.IWP_ID + "'");
                CurrentIWP.PredecessorList = getPredecessor.ToList();

                var getSuccessor = await _successorRepository.QueryAsync<T_Successor>("SELECT * FROM [T_Successor] WHERE [IWP_ID] = '" + IWPHelper.IWP_ID + "'");
                CurrentIWP.SuccessorList = getSuccessor.ToList();

            }

            if (CurrentIWP.Error == null || CurrentIWP.Error == string.Empty)
            {
                string PDFpath = await DependencyService.Get<ISaveFiles>().GetIWPFileLocation("PDF Store" + "\\" + CurrentIWP.JobCode + "\\" + CurrentIWP.Title + ".pdf");

                PdfUrl = string.Format( PDFpath );
            }
        }

        private async void OnClickButton(string param)
        {
            if (param == "PdfFullScreen")
            {
                PdfUriModel pdfuri = new PdfUriModel();
                pdfuri.PdfUriPath = PdfUrl;

                var navigationParameters = new NavigationParameters();
                navigationParameters.Add("uri", pdfuri);
                navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
                await navigationService.NavigateAsync<PDFvieverViewModel>(navigationParameters);

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
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        #endregion
    }
}
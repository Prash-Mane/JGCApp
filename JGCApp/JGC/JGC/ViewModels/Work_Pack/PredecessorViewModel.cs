using Acr.UserDialogs;
using JGC.Common.Interfaces;
using Prism.Navigation;
using JGC.Common.Extentions;
using JGC.Common.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using JGC.Models;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.DataBase;
using JGC.DataBase.DataTables.WorkPack;
using JGC.Common.Helpers;
using System.Linq;
using System.Collections.ObjectModel;

namespace JGC.ViewModels.Work_Pack
{
    public class PredecessorViewModel:BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_IWP> _iwpRepository;
        private readonly IRepository<T_Predecessor> _predecessorRepository;
        private T_IWP CurrentIWP;

        #region properties
        private bool predecessorGrid;
        public bool PredecessorGrid
        {
            get { return predecessorGrid; }
            set { SetProperty(ref predecessorGrid, value); }
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

        private ObservableCollection<T_Predecessor> predecessorList;
        public ObservableCollection<T_Predecessor> PredecessorList
        {
            get { return predecessorList; }
            set { predecessorList = value; RaisePropertyChanged(); }
        }
        private T_Predecessor selectedpredecessor;
        public T_Predecessor SelectedPredecessor
        {
            get { return selectedpredecessor; }
            //set { selecteddrawings = value; RaisePropertyChanged(); }
            set
            {
                if (SetProperty(ref selectedpredecessor, value))
                {
                    LoadPredecessor(selectedpredecessor);
                    OnPropertyChanged();
                }
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
        public ICommand BackBtnCommand
        {
            get
            {
                return new Command<string>(OnClickBackButton);
            }
        }

        #endregion

        public PredecessorViewModel(INavigationService _navigationService,
        IUserDialogs _userDialogs,
        IHttpHelper _httpHelper,
        ICheckValidLogin _checkValidLogin,
        IRepository<T_IWP> _iwpRepository,
        IRepository<T_Predecessor> _predecessorRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._iwpRepository = _iwpRepository;
            this._predecessorRepository = _predecessorRepository;
            _userDialogs.HideLoading();
            PageHeaderText = "Predecessor";
            JobSettingHeaderVisible = true;
            PredecessorGrid = true;
            GetPredecessorData();
        }
       
        private async void OnClickBackButton(string param)
        {
                await navigationService.NavigateAsync<IWPSelectionViewModel>();
                IsRunningTasks = false;       
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

        private async void GetPredecessorData()
        {
            var getIWP = await _iwpRepository.QueryAsync<T_IWP>("SELECT * FROM [T_IWP] WHERE [ID] = '" + IWPHelper.IWP_ID + "'");

            CurrentIWP = getIWP.FirstOrDefault();

            var getPredecessor = await _predecessorRepository.QueryAsync<T_Predecessor>("SELECT * FROM [T_Predecessor] WHERE [IWP_ID] = '" + IWPHelper.IWP_ID + "'");
            PredecessorList = new ObservableCollection<T_Predecessor>(getPredecessor);
        }
        private async void LoadPredecessor(T_Predecessor predecessor)
        {
            if (predecessor == null)
                return;

            string PDFpath = await DependencyService.Get<ISaveFiles>().GetIWPFileLocation("PDF Store" + "\\" + CurrentIWP.JobCode);

            PdfUrl = PDFpath + "\\" + predecessor.Title + ".pdf";
        }
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

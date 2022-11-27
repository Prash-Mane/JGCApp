using Acr.UserDialogs;
using JGC.Common.Interfaces;
using Prism.Navigation;
using JGC.Common.Extentions;
using JGC.Common.Constants;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.Models;
using JGC.DataBase;
using JGC.DataBase.DataTables.WorkPack;
using JGC.Common.Helpers;
using System.Linq;
using System.Collections.ObjectModel;

namespace JGC.ViewModels.Work_Pack
{
    public class SuccessorViewModel: BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_IWP> _iwpRepository;
        private readonly IRepository<T_Successor> _successorRepository;
       

        private T_IWP CurrentIWP;


        #region properties
        private bool successorGrid;
        public bool SuccessorGrid
        {
            get { return successorGrid; }
            set { SetProperty(ref successorGrid, value); }
        }
        private bool iwpselection;
        public bool IWPSelection
        {
            get { return iwpselection; }
            set { SetProperty(ref iwpselection, value); }
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
        private ObservableCollection<T_Successor> successorList;
        public ObservableCollection<T_Successor> SuccessorList
        {
            get { return successorList; }
            set { successorList = value; RaisePropertyChanged(); }
        }
        private T_Successor selectedsuccessor;
        public T_Successor SelectedSuccessor
        {
            get { return selectedsuccessor; }
            set
            {
                if (SetProperty(ref selectedsuccessor, value))
                {
                    LoadSuccessor(selectedsuccessor);
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
        public SuccessorViewModel(INavigationService _navigationService,
           IUserDialogs _userDialogs,
           IHttpHelper _httpHelper,
           ICheckValidLogin _checkValidLogin,
           IRepository<T_IWP> _iwpRepository,
           IRepository<T_Successor> _successorRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._successorRepository = _successorRepository;
            this._iwpRepository = _iwpRepository;
            _userDialogs.HideLoading();
            PageHeaderText = "Successor";
            JobSettingHeaderVisible = true;
            SuccessorGrid = true;

            GetSuccessorData();
        }
        
        #region Proivate
        private async void OnClickBackButton(string param)
        {
            await navigationService.NavigateAsync<IWPSelectionViewModel>();
            IsRunningTasks = false;
        }
        private async void OnClickButton(string param)
        {
             if (param == "Back")
            {
                SuccessorGrid = false;
                IWPSelection = true;

            }
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

        private async void GetSuccessorData()
        {
            var getIWP = await _iwpRepository.QueryAsync<T_IWP>("SELECT * FROM [T_IWP] WHERE [ID] = '" + IWPHelper.IWP_ID + "'");

            CurrentIWP = getIWP.FirstOrDefault();

            var getSuccessor = await _successorRepository.QueryAsync<T_Successor>("SELECT * FROM [T_Successor] WHERE [IWP_ID] = '" + IWPHelper.IWP_ID + "'");
            SuccessorList = new ObservableCollection<T_Successor>( getSuccessor);
        }
        private async void LoadSuccessor(T_Successor successor)
        {
            if (successor == null)
            return;

            string PDFpath = await DependencyService.Get<ISaveFiles>().GetIWPFileLocation("PDF Store" + "\\" + CurrentIWP.JobCode);

            PdfUrl = PDFpath + "\\" + successor.Title + ".pdf";
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

using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.Models;
using JGC.Common.Extentions;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System;
using System.IO;
using Xamarin.Forms.Internals;
using System.Windows.Input;

namespace JGC.ViewModels.E_Test_Package
{    
    public class PandidViewModel : BaseViewModel
    {

        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_TestLimitDrawing> _drawingsRepository;

        #region Properties

        private List<T_UserProject> userProjects;
        public List<T_UserProject> UserProjects
        {
            get => userProjects;
            set
            {
                SetProperty(ref userProjects, value);
                RaisePropertyChanged("UserProjectList");
            }
        }
        private T_ETestPackages ETPSelected;
        public T_ETestPackages SelectedETP
        {
            get { return ETPSelected; }
            set { ETPSelected = value; RaisePropertyChanged(); }
        }
        private ImageSource attachedimage;
        public ImageSource Attachedimage
        {
            get { return attachedimage; }
            set { attachedimage = value; RaisePropertyChanged(); }
        }

        #endregion
        public PandidViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_UserProject> _userProjectRepository,
            IRepository<T_TestLimitDrawing> _drawingsRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._userProjectRepository = _userProjectRepository;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._drawingsRepository = _drawingsRepository;
            _userDialogs.HideLoading();
            MainGrid = true;

            PageHeaderText = "Pandid";
            IsHeaderBtnVisible = true;
        }
        #region Private       
        public ICommand BtnCommand
        {
            get
            {
                return new Command<string>(OnClickButton);
            }
        }

        private async void OnClickButton(string param)
        {
            if (param == "ImageFullScreen")
            {
                ImageFullScreen = true;
                MainGrid = false;

            }
            else if (param == "BackfromSignatureGrid")
            {
                ImageFullScreen = false;
                MainGrid = true;
            }

        }
        #endregion

        private bool mainGrid;
        public bool MainGrid
        {
            get { return mainGrid; }
            set { SetProperty(ref mainGrid, value); }
        }
        private bool _imageFullScreen;
        public bool ImageFullScreen
        {
            get { return _imageFullScreen; }
            set { SetProperty(ref _imageFullScreen, value); }
        }

        private ObservableCollection<T_TestLimitDrawing> _attachmentList;
        public ObservableCollection<T_TestLimitDrawing> AttachmentList
        {
            get { return _attachmentList; }
            set { _attachmentList = value; RaisePropertyChanged(); }
        }
        private T_TestLimitDrawing selectedAttachedItem;
        public T_TestLimitDrawing SelectedAttachedItem
        {
            get { return selectedAttachedItem; }
            set
            {
                if (SetProperty(ref selectedAttachedItem, value))
                {
                    LoadAttachment(selectedAttachedItem);
                    OnPropertyChanged();
                }
            }
    }   



        private async void GetAttachmentData()
        {
            string sql = "SELECT * FROM [T_TestLimitDrawing] WHERE [ProjectID] = '" + ETPSelected.ProjectID + "' AND[ETestPackageID] = '" + ETPSelected.ID + "' AND[IsPID] = 1 ORDER BY[OrderNo] ASC";         // var gata = await _drawingsRepository.GetAsync();
            var AttachmentData = await _drawingsRepository.QueryAsync<T_TestLimitDrawing>(sql);
            AttachmentList = new ObservableCollection<T_TestLimitDrawing>(AttachmentData.Distinct());
        }


        private async void LoadAttachment(T_TestLimitDrawing AttachedItem)
        {
            if (AttachedItem == null)
                return;

           
           byte[] Base64Stream = Convert.FromBase64String(AttachedItem.FileBytes);

            Attachedimage = ImageSource.FromStream(() => new MemoryStream(Base64Stream));
        }



        #region Public

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            // CheckValidLogin._pageHelper = new PageHelper();
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            ETPSelected = CurrentPageHelper.ETestPackage;
            GetAttachmentData();
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        #endregion
    }
}

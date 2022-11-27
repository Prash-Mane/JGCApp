using Acr.UserDialogs;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables.Completions;
using Prism.Navigation;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.Common.Extentions;

namespace JGC.ViewModels.Completions
{
    public class CompletionDrawingViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        public readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_CompletionsDrawings> _CompletionsDrawingsRepository;
        public SKBitmap Bitmap = new SKBitmap();
        byte[] Base64Stream;
        SKBitmap SKBitmap;

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged();
            }
        }

        #region Delegate Commands   
        public ICommand TapedCommand
        {
            get
            {
                return new Command<string>(OnTapedAsync);
            }
        }
        private string _path;

        public string path
        {
            get { return _path; }
            set
            {
                if (SetProperty(ref _path, value))
                {
                    //SelectedCameraItems(_selectedCameraItem);
                    OnPropertyChanged();
                }
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

        private string fullPdfUrl;
        public string FullPdfUrl
        {
            get { return fullPdfUrl; }
            set
            {
                SetProperty(ref fullPdfUrl, value);
            }
        }

        private ObservableCollection<T_CompletionsDrawings> _DrawingList;
        public ObservableCollection<T_CompletionsDrawings> DrawingList
        {
            get { return _DrawingList; }
            set
            {
                _DrawingList = value;
                RaisePropertyChanged();

            }
        }
        private T_CompletionsDrawings _SelectedDrawingList;
        public T_CompletionsDrawings SelectedDrawingList
        {
            get { return _SelectedDrawingList; }
            set
            {
                if (SetProperty(ref _SelectedDrawingList, value))
                {
                    OnClickSelectedDrawingList();
                    RaisePropertyChanged();
                }
            }
        }
        private bool _mainpage;
        public bool Mainpage
        {
            get { return _mainpage; }
            set
            {
                SetProperty(ref _mainpage, value);
            }
        }
        private bool _fullscreenview;
        public bool FullScreenView
        {
            get { return _fullscreenview; }
            set
            {
                SetProperty(ref _fullscreenview, value);
            }
        }

        private ImageSource _drawingImage;
        public ImageSource DrawingImage
        {
            get
            {
                return _drawingImage;
            }
            set { SetProperty(ref _drawingImage, value); }
        }
        private string _image;
        public string Image
        {

            get { return _image; }
            set { _image = value; RaisePropertyChanged(); }
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
        public async void OnClickButton(string param)
        {
            if (param == "FullScreen" && SelectedDrawingList != null)
            {
                FullPdfUrl = PdfUrl;
                Mainpage = false;
                FullScreenView = true;
            }
            else
            {
                await _userDialogs.AlertAsync("No drawing selected.", null, "Ok");
            }
        }
        #endregion

        public CompletionDrawingViewModel(INavigationService _navigationService,
           IUserDialogs _userDialogs,
           IHttpHelper _httpHelper,
           ICheckValidLogin _checkValidLogin,
           IRepository<T_CompletionsDrawings> _CompletionsDrawingsRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._CompletionsDrawingsRepository = _CompletionsDrawingsRepository;
            //ImageSource = "camera.png";

            GetDrawing();
            IsBusy = false;
            Mainpage = true;
            FullScreenView = false;
        }
        private async void GetDrawing()
        {
            var completionsDrawingsList = await _CompletionsDrawingsRepository.GetAsync();
            DrawingList = new ObservableCollection<T_CompletionsDrawings>(completionsDrawingsList);
        }
        private async void OnClickSelectedDrawingList()
        {
            try
            {
                PdfUrl = "";
                //var dd = SelectedDrawingList;
                string DirectoryPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath("Drawings" + "\\" + Settings.ProjectName);

                PdfUrl = DirectoryPath + "\\" + SelectedDrawingList.filename;
                //string ImageBase64 = await DependencyService.Get<ISaveMediaFiles>().ConvertPDFtoByte(DirectoryPath + "\\" + SelectedDrawingList.filename);
                //byte[] bytes = System.Convert.FromBase64String(ImageBase64);

                //using (MemoryStream ms = new MemoryStream(bytes))
                //{
                //    DrawingImage = ImageSource.FromStream(() => ms);
                //}

                //Stream stream = new MemoryStream(bytes);
                //DrawingImage = ImageSource.FromStream(() => stream);

                /*  new code*/



            }
            catch (Exception e)
            {

            }
        }
        private async void OnTapedAsync(string param)
        {
            //if (param == "TagRegister")
            //    await navigationService.NavigateAsync<TagRegisterViewModel>();
            //else if (param == "Sync")
            //    await PopupNavigation.PushAsync(new SyncPage());
            //else if (param == "PunchList")
            //    await navigationService.NavigateAsync<PunchListViewModel>();
            //else if (param == "Drawings")
            //    await navigationService.NavigateAsync<DrawingViewModel>();
            //else if (param == "handover")
            //    await navigationService.NavigateAsync<HandoverViewModel>();
            //else if (param == "TestPacks")
            await navigationService.NavigateAsync<CompletionTestPackViewModel>();
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
        public class DrawingImageData
        {
            public string Image { get; set; }
        }


    }
}

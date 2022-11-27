using Acr.UserDialogs;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables.Completions;
using JGC.Models.Completions;
using JGC.Views.Completions;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.Common.Extentions;

namespace JGC.ViewModels.Completions
{
    public class HandoverViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_Handover> _CompletionsHandoverRepository;
        private readonly IRepository<T_HandoverWorkpacks> _HandoverWorkpackRepository;
        string directory;
        ObservableCollection<ImageData> modelimagedata;

        #region Delegate Commands   
        public ICommand TapedCommand
        {
            get
            {
                return new Command<string>(OnTapedAsync);
            }
        }
        private ObservableCollection<ImageData> imagestatus;
        public ObservableCollection<ImageData> ImageStatus
        {
            get { return imagestatus; }
            set
            {
                imagestatus = value;
                RaisePropertyChanged();

            }
        }

        private List<T_Handover> handoversList;
        public List<T_Handover> HandoversList
        {
            get { return handoversList; }
            set
            {
                handoversList = value;
                RaisePropertyChanged();
            }
        }


        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                RaisePropertyChanged();
                SearchDocument(SearchText);
            }
        }

        private void SearchDocument(string searchText)
        {

            if (!string.IsNullOrWhiteSpace(searchText) && modelimagedata != null && modelimagedata.Any())
            {
                ImageStatus = new ObservableCollection<ImageData>(modelimagedata.Where(x => x.ImageName.Trim().ToLower().Contains(searchText.Trim().ToLower())));
            }
            else
            {

                ImageStatus = modelimagedata;
            }


        }

        private ObservableCollection<string> _handoverData;
        public ObservableCollection<string> HandoverData
        {
            get { return _handoverData; }
            set { _handoverData = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_HandoverWorkpacks> _handoverWorkpackData;
        public ObservableCollection<T_HandoverWorkpacks> HandoverWorkpackData
        {
            get { return _handoverWorkpackData; }
            set { _handoverWorkpackData = value; RaisePropertyChanged(); }
        }
        //private ObservableCollection<T_Handover> _HandoverPage;
        //public ObservableCollection<T_Handover> HandoverPage
        //{
        //    get { return _HandoverPage; }
        //    set { _HandoverPage = value; RaisePropertyChanged(); }
        //}
        private string _selectedImageHandover;
        public string SelectedImageHandover
        {
            get
            {
                return _selectedImageHandover;
            }
            set
            {
                if (SetProperty(ref _selectedImageHandover, value))
                {
                    LoadImageFiles(_selectedImageHandover, system);
                    OnPropertyChanged();
                }
            }
        }
        private string _system;
        public string system
        {
            get { return _system; }
            set { _system = value; RaisePropertyChanged(); }
        }
        private bool _selectionIsSystem;
        public bool selectionIsSystem
        {
            get { return _selectionIsSystem; }
            set { _selectionIsSystem = value; RaisePropertyChanged(); }
        }


        #endregion
        public HandoverViewModel(INavigationService _navigationService,
           IUserDialogs _userDialogs,
           IHttpHelper _httpHelper,
           ICheckValidLogin _checkValidLogin,
           IRepository<T_Handover> _CompletionsHandoverRepository,
           IRepository<T_HandoverWorkpacks> _CompletionsHandoverWorkRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._CompletionsHandoverRepository = _CompletionsHandoverRepository;
            this._HandoverWorkpackRepository = _CompletionsHandoverWorkRepository;

            //ImageStatus = new List<ImageData>
            //{
            //    new ImageData { ImageName = "1553-7575-htgrgr", Image = "ITR.png" },
            //    new ImageData  { ImageName = "1953-7575-trtnjy6", Image = "ITR.png" },
            //    new ImageData  { ImageName = "1299-7575-tr662njy6", Image = "ITR.png" },
            //    new ImageData  { ImageName = "1153-6575-trtnjy6", Image = "ITR.png" },
            //    new ImageData  { ImageName = "1153-6575-trtnjy6", Image = "ITR.png" },
            //    new ImageData  { ImageName = "1153-6575-trtnjy6", Image = "ITR.png" },
            //    new ImageData  { ImageName = "1153-6575-trtnjy6", Image = "ITR.png" },
            //    new ImageData  { ImageName = "1153-6575-trtnjy6", Image = "ITR.png" }
            //};
            GetSystemLists();
            //GetHandoverWorkpackList();
        }


        private async void GetSystemLists()
        {
            //Binding Handover_DropDownList Using MC_Subsystem and SH_System 
            string HandoverSQL = "SELECT * FROM T_Handover";
            var Handover = await _CompletionsHandoverRepository.QueryAsync<T_Handover>(HandoverSQL);
            HandoversList = new List<T_Handover>();
            HandoversList.AddRange(Handover);
            List<string> HandoverList = new List<string>();
            HandoverList.AddRange(Handover.Select(i => " MC " + i.subsystem).Distinct().ToList());
            HandoverList.AddRange(Handover.Select(i => " SH " + i.system).Distinct().ToList());
            HandoverData = new ObservableCollection<string>(HandoverList);
        }
        //private async void GetHandoverWorkpackList()
        //{
        //    //Binding Handover_DropDown View Workpack List Using HandoverWorkpacks Data
        //   // string HandoverWorkpacksSQL = "SELECT * FROM T_HandoverWorkpacks";
        //   // var HandoverWorkpacksData = await _CompletionsHandoverRepository.QueryAsync<T_HandoverWorkpacks>(HandoverWorkpacksSQL);
        //   // List<string> HandoverWorkpackList = new List<string>();
        //   // HandoverWorkpackList.AddRange(HandoverWorkpacksData.Select(i =>  i.COLUMN_HANDOVER_WP).Distinct().ToList());
        //  //  HandoverWorkpackData = new ObservableCollection<string>(HandoverWorkpackList);
        //}
        private async void LoadImageFiles(string SelectedImageHandover, string system)
        {

            //String strNew = SelectedImageHandover.Substring(2);
            try
            {

                String str = SelectedImageHandover.Remove(0, 3).Trim();
                selectionIsSystem = SelectedImageHandover.Contains("SH");
                //Binding Handover_DropDown View Workpack List Using HandoverWorkpacks Data
                if (selectionIsSystem)
                {

                    directory = "Handovers" + "\\" + str;

                    var HandoverWorkpackSystem = await _HandoverWorkpackRepository.GetAsync(x => x.COLUMN_HANDOVER_WPSYSTEM == str);
                    HandoverWorkpackData = new ObservableCollection<T_HandoverWorkpacks>(HandoverWorkpackSystem.Distinct());
                }
                else
                {


                    var systemvalue = HandoversList.Where(x => x.subsystem == str).Select(x => x.system).FirstOrDefault();
                    directory = "Handovers" + "\\" + systemvalue + "\\" + str;

                    var HandoverWorkpackSubsystem = await _HandoverWorkpackRepository.GetAsync(x => x.COLUMN_HANDOVER_WPSUBSYSTEM == str); //QueryAsync<T_HandoverWorkpacks>("SELECT [COLUMN_HANDOVER_WP] FROM [T_HandoverWorkpacks] WHERE (COLUMN_HANDOVER_WPSYSTEM) = " + str+"" );
                    HandoverWorkpackData = new ObservableCollection<T_HandoverWorkpacks>(HandoverWorkpackSubsystem.Distinct());
                }

                var DirectoryExists = await DependencyService.Get<ISaveFiles>().DirectoryExists(directory);
                if (DirectoryExists)
                {
                    List<ImageData> ImageStatus1 = new List<ImageData>();
                    var directoryPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath(directory);
                    var Imags1 = await DependencyService.Get<ISaveFiles>().GetAllImages(directoryPath);
                    var data1 = Imags1;
                    foreach (string imagepath in data1)
                    {
                        string imgPath = directoryPath + "/" + imagepath;
                        var imageSource = await DependencyService.Get<ISaveFiles>().GetThumbnail(imgPath);
                        string imageNumber = imagepath.Split('\\').Last().Replace(".png", "");
                        var imageName = HandoversList.Where(x => x.number.ToString().Trim() == imageNumber.Trim()).Select(x => x.name).FirstOrDefault();


                        ImageStatus1.Add(new ImageData { ImagePath = imgPath, ImageFromSource = imageSource, ImageName = imageName });
                    }

                    ImageStatus = new ObservableCollection<ImageData>(ImageStatus1);
                    modelimagedata = ImageStatus;
                }
                else
                {
                    _userDialogs.AlertAsync("Pages Not Available", "Alert", "OK");
                }



                //ImageStatus = new ObservableCollection<ImageData>();
                //var DirectoryExists = await DependencyService.Get<ISaveMediaFiles>().DirectoryExists(directory);
                //if (DirectoryExists)
                //{
                //    var Imags1 = await DependencyService.Get<ISaveMediaFiles>().GetAllImages(directory);

                //    foreach (string imagepath in Imags1)
                //    {
                //        ImageSource imageSource;


                //            imageSource = await DependencyService.Get<ISaveMediaFiles>().GetImage(imagepath);



                //       // string imageNumber = imagepath.Split('\\').Last().Replace(".png", "");


                //        //var imageName = HandoversList.Where(x => x.number.ToString().Trim() == imageNumber.Trim()).Select(x => x.name).FirstOrDefault();
                //        ImageStatus.Add(new ImageData {ImagePath = imagepath,   /* imagepath.Replace('\\', '/')*/ ImageName = "fgh" });
                //    }
                //}
            }
            catch (Exception Ex)
            {

            }

        }

        private async Task<byte[]> getfile(string path)
        {
            byte[] FileBytes;
            if (Device.RuntimePlatform == Device.UWP)
                FileBytes = await DependencyService.Get<ISaveFiles>().ReadBytes(path);
            else
                FileBytes = File.ReadAllBytes(path);

            return FileBytes;
        }

        [Obsolete]
        public async void OnTapedAsync(ImageData param)
        {
            await PopupNavigation.PushAsync(new DocumentViewerPopup(param, ImageStatus.ToList()));
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



    }
}

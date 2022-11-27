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
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace JGC.ViewModels.Completions
{
    public class CompletionTestPackViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_Handover> _handoverRepository;

        #region Delegate Commands   
        public ICommand TestPackCommand
        {
            get
            {
                return new Command<string>(ButtonClicked);
            }
        }

        private void ButtonClicked(string obj)
        {
            throw new NotImplementedException();
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

        #endregion

        #region Properties
        private ObservableCollection<string> itemSourceTestPacks;
        public ObservableCollection<string> ItemSourceTestPacks
        {
            get { return itemSourceTestPacks; }
            set { itemSourceTestPacks = value; RaisePropertyChanged(); }
        }


        private List<T_Handover> testPackList;
        public List<T_Handover> TestPackList
        {
            get { return testPackList; }
            set { testPackList = value; RaisePropertyChanged(); }
        }



        private ImageData selectedImageItem;


        public ImageData SelectedImageItem
        {
            get { return selectedImageItem; }
            set
            {

                selectedImageItem = value;
                RaisePropertyChanged();

            }
        }


        private string selectedTestPack;
        public string SelectedTestPack
        {
            get { return selectedTestPack; }
            set
            {
                selectedTestPack = value;
                if (!string.IsNullOrWhiteSpace(selectedTestPack) && selectedTestPack != "Select Test Pack")
                    TestpackSelected(selectedTestPack);
                RaisePropertyChanged();
            }
        }

        private async void TestpackSelected(string testPack)
        {
            try
            {
                string directory = "TestPacks" + "\\" + testPack;
                // var directories = await DependencyService.Get<ISaveMediaFiles>().GetIWPFileLocation;
                // var testPackDirectory = directories.FirstOrDefault();
                // var  Imags = await DependencyService.Get<ISaveMediaFiles>().GetImageFiles(directory);

                List<ImageData> ImageDataList = new List<ImageData>();
                ImageStatus = new ObservableCollection<ImageData>();

                var DirectoryExists = await DependencyService.Get<ISaveFiles>().DirectoryExists(directory);
                if (DirectoryExists)
                {
                    //  var directoryPath = await DependencyService.Get<ISaveMediaFiles>().GenerateImagePath(directory);
                    var Imags1 = await DependencyService.Get<ISaveFiles>().GetAllImages(directory);
                    var data1 = Imags1;
                    foreach (string imagepath in data1)
                    {
                        // string imgPath = directory + "/" + imagepath;
                        // var imageSource = await DependencyService.Get<ISaveMediaFiles>().GetThumbnail(imgPath);
                        string imageNumber = imagepath.Split('/', '\\').Last().Replace(".png", "");
                        var imageName = TestPackList.Where(x => x.number.ToString().Trim() == imageNumber.Trim()).Select(x => x.name).FirstOrDefault();
                        ImageData sdf = new ImageData { ImagePath = imagepath, ImageFromSource = null, ImageName = imageName };

                        ImageDataList.Add(sdf);
                    }

                    ImageStatus = new ObservableCollection<ImageData>(ImageDataList);
                }
                else
                {
                    _userDialogs.AlertAsync("Pages Not Available", "Alert", "OK");
                }

            }
            catch (Exception ex)
            {
            }// throw new NotImplementedException();
        }
        #endregion


        public CompletionTestPackViewModel(INavigationService _navigationService,
           IUserDialogs _userDialogs,
           IHttpHelper _httpHelper,
           IRepository<T_Handover> _handoverRepository,
           ICheckValidLogin _checkValidLogin) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._handoverRepository = _handoverRepository;
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            ImageStatus = new ObservableCollection<ImageData>();
            BindTestPackPicker();
            SelectedImageItem = new ImageData();
        }

        private async void BindTestPackPicker()
        {
            //ItemSourceSourceHandover
            string DefaultItem = "Select Test Pack";
            var data = await _handoverRepository.GetAsync(x => x.testpack);
            // ItemSourceSourceHandover = new ObservableCollection<T_Handover><T>();
            TestPackList = new List<T_Handover>();
            TestPackList.AddRange(data);
            ItemSourceTestPacks = new ObservableCollection<string>(TestPackList.Select(x => x.testpackname).Distinct());
            ItemSourceTestPacks.Insert(0, DefaultItem);
            SelectedTestPack = DefaultItem;

        }

        [Obsolete]
        public async void OnTapedAsync(ImageData param)
        {
            await PopupNavigation.PushAsync(new DocumentViewerPopup(param, ImageStatus.ToList()));
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

using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Extentions;
using JGC.Common.Helpers;
using JGC.Models.MasterModel;
using JGC.ViewModel;
using JGC.ViewModels.E_Reporter;
using JGC.ViewModels.E_Test_Package;
using JGC.ViewModels.Work_Pack;
using JGC.Views.MasterPage;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace JGC.ViewModels.MasterViewModel
{
    public class MainMasterDetailViewModel : BindableBase
    {
        private readonly INavigationService navigationService;
        private readonly IUserDialogs userDialogs;

        public static MainMasterDetailViewModel Instance { get; private set; }

        public MainMasterDetailViewModel(
            INavigationService navigationService,
            IUserDialogs userDialogs)
        {
            this.navigationService = navigationService;
            this.userDialogs = userDialogs;
            FillMenuItems();
            ImageFillMenu();
            TapBackCommand = new Command(TapOnBackImage);
           // TapIconCommand = new Command(OnClickIcon);
            
            //Cache.IsGoFromSplashScreen = false;
            Instance = this;

            App.ChangeMenuPresenter = (isPresented) =>
            {
                IsGestureEnabled = isPresented;
            };
            ChangeColor();
            ChangeTestColor();

        
        }


        private ObservableCollection<MasterPageItemModel> menuList;
        /// <summary>
        /// Get and set the user menu list
        /// </summary>
        public ObservableCollection<MasterPageItemModel> MenuList
        {
            get { return menuList; }
            set { menuList = value; RaisePropertyChanged(); }
        }

        


        #region DelegateCommand

        public String someImage;
        public String SomeImage
        {
            set
            {
                if (someImage != value)
                {
                    someImage = value;
                }
            }
            get
            {
                return someImage;
            }
        }
        public String arrowIcon;
        public String ArrowIcon
        {
            set
            {
                if (arrowIcon != value)
                {
                    arrowIcon = value;
                }
            }
            get
            {
                return arrowIcon;
            }
        }
        
        public ICommand TapBackCommand { get; set; }
        public ICommand TapIconCommand {
            get
            {
                return new Command(OnClickIcon);
            }
        }
        public ICommand HeaderCommand { get => new Command(GoHome); }

        #endregion

        //private MasterPageItemModel selecteMenu;
        //not used?
        //public MasterPageItemModel SelectedMenu
        //{
        //	get { return selecteMenu; }
        //	set
        //	{
        //		selecteMenu = value;
        //              if (value?.Title != null) //why??
        //		{
        //			var menuDetails = cragRepository.GetAsync(x => x.is_enabled && x.is_downloaded).Result;
        //			if (menuDetails.Count > 0)
        //			{
        //				var selectedItems = menuDetails?.FirstOrDefault(s => s.crag_name == selecteMenu.Title);
        //			}
        //		}
        //		RaisePropertyChanged();
        //	}
        //}



        public void ImageFillMenu()
        {
            try
            {
                if (Settings.ModuleName == "JobSetting")
                {
                    SomeImage = "WorkPackage_logo.png";
                    ArrowIcon = "icon_arrow_left.png";
                }
                else if (Settings.ModuleName == "TestPackage")
                {
                    SomeImage = "TestPackage_logo.png";
                    ArrowIcon = "TestPack_icon_arrow_left.png";
                }
                else if(Settings.ModuleName == "EReporter")
                {
                    SomeImage = "EReporter_logo.png";
                    ArrowIcon = "icon_arrow_left.png";

                }
            }
            catch (Exception ex)
            {

            }
        }
        public void FillMenuItems()
        {
            try
            {
                if (Settings.ModuleName == "JobSetting")
                {
                    CommonSelectedItemChangeColor();

                }
                else if (Settings.ModuleName == "TestPackage")
                {
                    CommonSelectedItemChangeTestPackColor();

                }
                else if (Settings.ModuleName == "EReporter")
                {
                    CommonSelectedItemChangeErepoterColor();
                }
            }
            catch (Exception ex)
            {

            }
        }

       

        public async void OnClickIcon()
        {
            //await navigationService.NavigateAsync<IWPSelectionViewModel>();
            IsPresentedMenu = false;
            await navigationService.NavigateFromMenuAsync(typeof(ModuleViewModel));
        }

        //TODO: REFACTOR THIS CODE!
        public void ChangeColor()
        {
           
            MessagingCenter.Subscribe<MainMasterDetailPage>(this, "ChangeSelectedItemColorMenu", async (sender) =>
            {
                try
                {
                        CommonSelectedItemChangeColor();
                }
                catch (Exception ex)
                {
                    
                }
            });
        }
        private async void CommonSelectedItemChangeColor()
        {
            try
            {
                MasterPageItemModel item = SelectedMenuItem;
                var tempMenuList = new List<MasterPageItemModel>();               

                tempMenuList.Add(new MasterPageItemModel
                {
                    Title = "IWP PDF",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(IWPPdfViewModel),
                    ActiveTextColor = Color.FromHex("#3B87C7"),
                });

                tempMenuList.Add(new MasterPageItemModel
                {
                    Title = "Drawings",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(DrawingsViewModel),
                    ActiveTextColor = Color.FromHex("#3B87C7"),
                });

                tempMenuList.Add(new MasterPageItemModel
                {
                    Title = "Control Log",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(IWPControlLogViewModel),
                    ActiveTextColor = Color.FromHex("#3B87C7"),
                });
                
                tempMenuList.Add(new MasterPageItemModel
                {
                    Title = "CWP Tag Status",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(CWPTagStatusViewModel),
                    ActiveTextColor = Color.FromHex("#3B87C7"),
                });

                tempMenuList.Add(new MasterPageItemModel
                {
                    Title = "Man Power Resource",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(ManPowerResourceViewModel),
                    ActiveTextColor = Color.FromHex("#3B87C7"),
                });

                tempMenuList.Add(new MasterPageItemModel
                {
                    Title = "Punch Control",
                    IconSource = "ExpandImage.png",
                    ContentHeight = 0,
                    IsContentVisible = false,
                   
                    ActiveTextColor = Color.FromHex("#3B87C7"),
                });

                tempMenuList.Add(new MasterPageItemModel
                 {
                     Title = "Predecessor",
                     IconSource = "",
                     ContentHeight = 0,
                     IsContentVisible = false,
                     TargetType = typeof(PredecessorViewModel),
                     ActiveTextColor = Color.FromHex("#3B87C7"),
                 });

                tempMenuList.Add(new MasterPageItemModel
                {
                    Title = "Successor",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(SuccessorViewModel),
                    ActiveTextColor = Color.FromHex("#3B87C7"),
                });

                MenuList = new ObservableCollection<MasterPageItemModel>(tempMenuList);

            }

            catch (Exception ex)
            {
               
            }
        }

        public void ChangeTestColor()
        {

            MessagingCenter.Subscribe<MainMasterDetailPage>(this, "ChangeSelectedItemColorMenu", async (sender) =>
            {
                try
                {
                    CommonSelectedItemChangeTestPackColor();
                }
                catch (Exception ex)
                {

                }
            });
        }
        private async void CommonSelectedItemChangeTestPackColor()
        {
            try
            {
                MasterPageItemModel item = SelectedMenuItem;
                var TestMenuList = new List<MasterPageItemModel>();

                TestMenuList.Add(new MasterPageItemModel
                {
                    Title = "Punch Overview",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(PunchOverviewViewModel),
                    ActiveTextColor = Color.FromHex("#C4BB46"),
                });

                TestMenuList.Add(new MasterPageItemModel
                {
                    Title = "Punch View",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(PunchViewModel),
                    ActiveTextColor = Color.FromHex("#C4BB46"),
                });

                TestMenuList.Add(new MasterPageItemModel
                {
                    Title = "Control Log",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(ControlLogViewModel),
                    ActiveTextColor = Color.FromHex("#C4BB46"),
                });

                TestMenuList.Add(new MasterPageItemModel
                {
                    Title = "Pandid",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(PandidViewModel),
                    ActiveTextColor = Color.FromHex("#C4BB46"),
                });

                TestMenuList.Add(new MasterPageItemModel
                {
                    Title = "Pre-Test Record",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(PreTestRecordViewModel),
                    ActiveTextColor = Color.FromHex("#C4BB46"),
                });

                TestMenuList.Add(new MasterPageItemModel
                {
                    Title = "Test Record",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(TestRecordViewModel),
                    ActiveTextColor = Color.FromHex("#C4BB46"),
                });

                TestMenuList.Add(new MasterPageItemModel
                {
                    Title = "Drain Record",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(DrainRecordViewModel),
                    ActiveTextColor = Color.FromHex("#C4BB46"),
                });

                TestMenuList.Add(new MasterPageItemModel
                {
                    Title = "Post-Test Record",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(PostTestRecordViewModel),
                    ActiveTextColor = Color.FromHex("#C4BB46"),
                });

                MenuList = new ObservableCollection<MasterPageItemModel>(TestMenuList);

            }

            catch (Exception ex)
            {

            }
        }
        private void CommonSelectedItemChangeErepoterColor()
        {
            try
            {
                MasterPageItemModel item = SelectedMenuItem;
                var TempMenuList = new List<MasterPageItemModel>();

                TempMenuList.Add(new MasterPageItemModel
                {
                    Title = "Joint Detail",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(JointDetailsViewModel),
                    ActiveTextColor = Color.FromHex("#3B87C7"),
                });

                TempMenuList.Add(new MasterPageItemModel
                {
                    Title = "Drawing",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(DWRDrawingViewModel),
                    ActiveTextColor = Color.FromHex("#3B87C7"),
                });

                TempMenuList.Add(new MasterPageItemModel
                {
                    Title = "Control Log",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(DWRControlLogViewModel),
                    ActiveTextColor = Color.FromHex("#3B87C7"),
                });

                TempMenuList.Add(new MasterPageItemModel
                {
                    Title = "Attachment",
                    IconSource = "",
                    ContentHeight = 0,
                    IsContentVisible = false,
                    TargetType = typeof(AttachmentViewModel),
                    ActiveTextColor = Color.FromHex("#3B87C7"),
                });

                MenuList = new ObservableCollection<MasterPageItemModel>(TempMenuList);

            }

            catch (Exception ex)
            {

            }
        }

        private async void TapOnBackImage()
        {
            try
            {
                Cache.MasterPage.IsPresented = false;
            }
            catch (Exception ex)
            {
            }
        }


        private IList<MasterPageItemModel> items;
        public IList<MasterPageItemModel> Items
        {
            get
            {
                return items;
            }
            set
            {
                SetProperty(ref items, value);
            }
        }

        private bool isGestureEnabled = true;

        public bool IsGestureEnabled
        {
            get
            {
                return isGestureEnabled;
            }
            set
            {
                SetProperty(ref isGestureEnabled, value);
            }
        }

        private bool isPresentedMenu;

        public bool IsPresentedMenu
        {
            get
            {
                return isPresentedMenu;
            }
            set
            {
                SetProperty(ref isPresentedMenu, value);
            }
        }

        private MasterPageItemModel selectedmenuItem;
        public MasterPageItemModel SelectedMenuItem
        {
            //get
            //{
            //    return selectedmenuItem;
            //}
            //set
            //{
            //    SetProperty(ref selectedmenuItem, value);
            //    RaisePropertyChanged();
            //}
            get { return selectedmenuItem; }
            set { selectedmenuItem = value; RaisePropertyChanged(); }
        }

        public ICommand MenuSelectedItemCommand
        {
            get
            {
                return new Command<MasterPageItemModel>(async item =>
                {
                    try
                    {
                        if (item.TargetType == null)
                        {
                            bool IsAlready = MenuList.Any(x => x.Title == "Add Punch");
                            if (IsAlready)
                            {
                                item.IconRotation = 0;
                                MenuList.RemoveAt(6); MenuList.RemoveAt(6);
                                MenuList = new ObservableCollection<MasterPageItemModel>(MenuList);
                            }
                            else
                            {
                                item.IconRotation = 180;
                                MenuList.Insert(6, new MasterPageItemModel
                                {
                                    Title = "View/Edit Punch",
                                    IconSource = "",
                                    ContentHeight = 0,
                                    IsContentVisible = false,
                                    TargetType = typeof(PunchControlViewModel),
                                    ActiveTextColor = Color.FromHex("#3B87C7"),
                                    SubMenuBGColor = Color.FromHex("#BFC9CA"),
                                });

                                MenuList.Insert(6, new MasterPageItemModel
                                {
                                    Title = "Add Punch",
                                    IconSource = "",
                                    ContentHeight = 0,
                                    IsContentVisible = false,
                                    TargetType = typeof(AddPunchControlViewModel),
                                    ActiveTextColor = Color.FromHex("#3B87C7"),
                                    SubMenuBGColor = Color.FromHex("#BFC9CA"),
                                });

                                MenuList = new ObservableCollection<MasterPageItemModel>(MenuList);
                            }

                           // await UserDialogs.Instance.AlertAsync("This section is under development", "Alert", "Ok");
                            SelectedMenuItem = null;
                            IsPresentedMenu = false;
                            return;
                        }
                        if ((item.TargetType == typeof(DWRDrawingViewModel) || item.TargetType == typeof(DWRControlLogViewModel) ||
                            item.TargetType == typeof(AttachmentViewModel)) && DWRHelper.SelectedDWR == null)
                        await  UserDialogs.Instance.AlertAsync("Please select DWR", "Alert", "Ok");
                        else
                        {
                            await navigationService.NavigateFromMenuAsync(item.TargetType);
                            SelectedMenuItem = null;
                            IsPresentedMenu = false;
                        }
                    }
                    catch (Exception exception)
                    {
                        
                    }
                });
            }
        }

        async void GoHome()
        {
            //if (!menuList.Any(mi => mi.ItemId != 0)) //no crags available
            //return;

            IsPresentedMenu = false;
            navigationService.NavigateFromMenuAsync(typeof(LoginViewModel));
        }
    }
}

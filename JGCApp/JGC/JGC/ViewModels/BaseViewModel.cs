using Acr.UserDialogs;
using Plugin.Connectivity;
using Prism.Mvvm;
using Prism.Navigation;
using JGC.Common.Extentions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using JGC.ViewModel;
using JGC.Common.Interfaces;
using JGC.Common.Enumerators;
using System.Windows.Input;
using System.Linq;
using Rg.Plugins.Popup.Services;
using JGC.Common.Helpers;
using JGC.Models;
using JGC.Common.Constants;
using JGC.ViewModels.E_Test_Package;
using JGC.ViewModels.Work_Pack;
using JGC.ViewModels.E_Reporter;
using JGC.ViewModels.Completions;
namespace JGC.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware, IDestructible
    {
        protected readonly INavigationService navigationService;
        protected readonly IHttpHelper httpHelper;
        protected readonly ICheckValidLogin checkValidLogin;
        protected readonly IUserDialogs _userDialogs;
        protected readonly INavigationService _navigationService;
       
      
        private string pageHeaderText;
        public static INavigationService Navigation { get; set; }
      
        private bool isRunningTasks;
        public bool IsRunningTasks
        {
            get { return isRunningTasks; }
            set
            {
                SetProperty(ref isRunningTasks, value);
            }
        }
        private bool isHeaderBtnVisible;
        public bool IsHeaderBtnVisible
        {
            get { return isHeaderBtnVisible; }
            set
            {
                SetProperty(ref isHeaderBtnVisible, value);
            }
        }
        private bool jobSettingHeaderVisible;
        public bool JobSettingHeaderVisible
        {
            get { return jobSettingHeaderVisible; }
            set
            {
                SetProperty(ref jobSettingHeaderVisible, value);
            }
        }

        protected virtual async void OnNavigation(string param, NavigationParameters parameters = null)
        {
            try
            {               
                if (string.IsNullOrEmpty(param))
                {
                    App.IsNavigating = false;
                    return;
                }
                if (App.IsNavigating)
                { 
                    await Task.Delay(1000).ConfigureAwait(false);
                    return;
                }
                if (param.ToUpper().Contains(PageHeaderText.ToUpper()))
                    return;
                //App.IsNavigating = true;
                var pageType = (ApplicationActivity)Enum.Parse(typeof(ApplicationActivity), param);
                await PageNavigation(pageType, parameters);
            }
            catch (Exception ex)
            {
               
            }
        }
        protected async Task PageNavigation(ApplicationActivity page, NavigationParameters parameters = null)
        {
            try
            {
                if (PopupNavigation.PopupStack.Count > 0)
                    PopupNavigation.PopAllAsync();

                if (!CrossConnectivity.Current.IsConnected && page != ApplicationActivity.LoginPage) //&& (page == ApplicationActivity.LoginPage)
                {
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.AlertAsync(page + " Requires an Internet Connection.", "Network Error", "Continue");
                    //});
                   // App.IsNavigating = false;
                    App.IsNavigating = true;
                    return;
                }
                else
                {
                    App.IsNavigating = true;
                }

                if (!App.IsNavigating)
                    return;
                              
                switch (page)
                {                      
                    case ApplicationActivity.LoginPage:
                        if (!String.IsNullOrEmpty(Settings.AccessToken))
                        {
                            CheckValidLogin._pageHelper = new PageHelper();
                            Settings.AccessToken=Settings.Report = null;
                            Settings.UserID = Settings.ProjectID = 0;
                            CurrentPageHelper.CurrentSessionSignature = null;
                        }
                        await navigationService.NavigateAsync<LoginViewModel>();
                        break;
                    case ApplicationActivity.ProjectListPage:
                        await navigationService.NavigateAsync<ProjectViewModel>();
                        break;
                    case ApplicationActivity.EReportSelectionPage:
                        //await navigationService.NavigateAsync<EReportSelectionViewModel>();
                        await navigationService.NavigateFromMenuAsync<EReportSelectionViewModel>();
                        break;
                    case ApplicationActivity.DashboardPage:
                        await navigationService.NavigateAsync<DashboardViewModel>();
                        break;
                    case ApplicationActivity.DownloadPage:  
                        if(!String.IsNullOrEmpty(Settings.DownloadParam))
                           await navigationService.NavigateFromMenuAsync(typeof(DWR_AddJointViewModel));
                        else
                           await navigationService.NavigateAsync<DownloadViewModel>();
                        break;
                    //case ApplicationActivity.DWR_EReporterPage:
                    //    await navigationService.NavigateAsync<DWR_EReporterViewModel>();
                    //    break;
                    case ApplicationActivity.RIR_EReporterPage:
                        await navigationService.NavigateAsync<RIR_EReporterViewModel>();
                        break;
                    case ApplicationActivity.MRR_EReporterPage:
                        await navigationService.NavigateAsync<MRR_EReporterViewModel>();
                        break;
                    case ApplicationActivity.CMR_EReporterPage:
                        await navigationService.NavigateAsync<CMR_EReporterViewModel>();
                        break;
                    case ApplicationActivity.UploadPage:
                        await navigationService.NavigateAsync<UploadViewModel>();
                        break;
                    case ApplicationActivity.SettingPage:
                        await navigationService.NavigateAsync<SettingViewModel>();
                        break;
                    case ApplicationActivity.ModulesPage:
                        await navigationService.NavigateAsync<ModuleViewModel>();
                        break;
                    case ApplicationActivity.PDFviever:
                        await navigationService.NavigateAsync<PDFvieverViewModel>();
                        break;
                    // Test Package
                    case ApplicationActivity.ETestPackageList:
                        await navigationService.NavigateAsync<ETestPackageVewModel>();
                        break;
                    case ApplicationActivity.PackageDetailPage:
                        await navigationService.NavigateAsync<PackageDetailViewModel>();
                        break;
                    case ApplicationActivity.PunchOverviewPage:
                        await navigationService.NavigateAsync<PunchOverviewViewModel>();
                        break;
                    case ApplicationActivity.PunchViewPage:
                        await navigationService.NavigateAsync<PunchViewModel>();
                        break;
                    case ApplicationActivity.TestRecordPage:
                        await navigationService.NavigateAsync<TestRecordViewModel>();
                        break;
                    case ApplicationActivity.ControlLogPage:
                        await navigationService.NavigateAsync<ControlLogViewModel>();
                        break;
                    case ApplicationActivity.PandidPage:
                        await navigationService.NavigateAsync<PandidViewModel>();
                        break;
                    case ApplicationActivity.DrainRecordPage:
                        await navigationService.NavigateAsync<DrainRecordViewModel>();
                        break;
                    case ApplicationActivity.PreTestRecordPage:
                        await navigationService.NavigateAsync<PreTestRecordViewModel>();
                        break;
                    case ApplicationActivity.PostTestRecordPage:
                        await navigationService.NavigateAsync<PostTestRecordViewModel>();
                        break;

                    //JobSetting
                    case ApplicationActivity.IWPSelectionPage:
                        await navigationService.NavigateAsync<IWPSelectionViewModel>();
                        break;
                    case ApplicationActivity.IWPPdfPage:
                        await navigationService.NavigateAsync<IWPPdfViewModel>();
                        break;
                    case ApplicationActivity.DrawingsPage:
                        await navigationService.NavigateAsync<DrawingsViewModel>();
                        break;
                    case ApplicationActivity.IWPControlLogPage:
                        await navigationService.NavigateAsync<IWPControlLogViewModel>();
                        break;
                    case ApplicationActivity.CWPTagStatusPage:
                        await navigationService.NavigateAsync<CWPTagStatusViewModel>();
                        break;
                    case ApplicationActivity.ManPowerResourcePage:
                        await navigationService.NavigateAsync<ManPowerResourceViewModel>();
                        break;
                    case ApplicationActivity.AddPunchControlPage:
                        await navigationService.NavigateAsync<AddPunchControlViewModel>();
                        break;
                    case ApplicationActivity.EditPunchControlPage:
                        await navigationService.NavigateAsync<EditPunchControlViewModel>();
                        break;
                    case ApplicationActivity.PunchControlPage:
                        await navigationService.NavigateAsync<PunchControlViewModel>();
                        break;
                    case ApplicationActivity.PredecessorPage:
                        await navigationService.NavigateAsync<PredecessorViewModel>();
                        break;
                    case ApplicationActivity.SuccessorPage:
                        await navigationService.NavigateAsync<SuccessorViewModel>();
                        break;
                    //new DWR
                    case ApplicationActivity.AttachmentPage:
                        await navigationService.NavigateAsync<AttachmentViewModel>();
                        break;
                    case ApplicationActivity.DWRDrawingPage:
                        await navigationService.NavigateAsync<DWRDrawingViewModel>();
                        break;
                    case ApplicationActivity.DWRControlLogPage:
                        await navigationService.NavigateAsync<DWRControlLogViewModel>();
                        break;
                    case ApplicationActivity.JointDetailsPage:
                        await navigationService.NavigateFromMenuAsync(typeof(JointDetailsViewModel));
                        //await navigationService.NavigateAsync<JointDetailsViewModel>();
                        break;
                    case ApplicationActivity.InspectJointPage:
                        await navigationService.NavigateAsync<InspectJointViewModel>();
                        break;

                    //Completions
                    case ApplicationActivity.CompletionProjectList:
                        await navigationService.NavigateAsync<CompletionProjectViewModel>();
                        break;
                    case ApplicationActivity.CompletionsDashboard:
                        await navigationService.NavigateAsync<CompletionsDashboardViewModel>();
                        break;
                    case ApplicationActivity.TagRegisterPage:
                        await navigationService.NavigateAsync<TagRegisterViewModel>();
                        break;
                    case ApplicationActivity.SyncPage:
                        await navigationService.NavigateAsync<SyncViewModel>();
                        break;
                    case ApplicationActivity.CompletionPunchList:
                        await navigationService.NavigateAsync<CompletionPunchListViewModel>();
                        break;
                    case ApplicationActivity.CreateNewPunchPage:
                        await navigationService.NavigateAsync<CreateNewPunchViewModel>();
                        break;
                    case ApplicationActivity.CompletionDrawingPage:
                        await navigationService.NavigateAsync<CompletionDrawingViewModel>();
                        break;
                    case ApplicationActivity.HandOverpage:
                        await navigationService.NavigateAsync<HandoverViewModel>();
                        break;
                    case ApplicationActivity.CompletionTestPack:
                        await navigationService.NavigateAsync<CompletionTestPackViewModel>();
                        break;
                }

                App.IsNavigating = false;
            }
            catch (Exception ex)
            {
                
            }
        }
      
        public ICommand FooterCommand
        {
            get { return new Command<string>((page) => OnNavigation(page)); }
        }
        public static async Task NotificationClickExecute()
        {
            if (PopupNavigation.PopupStack.Any())
                return;

            //await Navigation.NavigateFromMenuAsync<ManageDownloadsViewModel>(animated: false);
        }
        private string _testonline;
      

       
        public string TestOnline
        {
            get { return _testonline; }
            set { SetProperty(ref _testonline, value); }
        }
        void CheckInternetConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                TestOnline = "Offline.png";
            }
            else
            {
                TestOnline = "Online.png";
            }
        }
        public BaseViewModel(INavigationService navigationService, IHttpHelper httpHelper = null,ICheckValidLogin _checkValidLogin=null) 
        {
            this.navigationService = navigationService;
            this.httpHelper = httpHelper;
            this.checkValidLogin = _checkValidLogin;
            System.Diagnostics.Debug.WriteLine(this.GetType());
            var task = Task.Run(async () =>
            {
                while (true)
                {
                    CheckInternetConnection();
                    await Task.Delay(2000);
                }
            });
        }
        public BaseViewModel() 
        {
        }
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
           
            
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
          
        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {

        }
        public virtual void Destroy() //for cases when whole navigation stack is reset and NavigatedFrom was not called
        {           
        }

        protected virtual async void OnNavigationBack()
        {
            string preVM = NavigationExtention.GetPreLastVM(navigationService);
            if (Settings.ModuleName == "JobSetting" && preVM != "IWPSelectionViewModel" && preVM != "ProjectViewModel" && preVM != "DownloadViewModel"
                && preVM != "UploadViewModel" && preVM != "SettingViewModel" && preVM != "ModuleViewModel")
            {
                await navigationService.GoBackToRootAsync();
            }
            else
            {
                string Param = NavigationExtention.GetNavigationPreLastVM(navigationService);
                if (!String.IsNullOrEmpty(Param))
                {
                    if(Param != "JGC.ViewModels.MasterViewModel.MasterNavigationViewModel")
                    try { await navigationService.NavigateFromMenuAsync(Type.GetType(Param)); } catch { return; }
                }
              
            }
        }
        public ICommand onBackPressed
        {
            get
            {
                try
                {
                      var data =  new Command<string>((page) => { try { OnNavigationBack(); } catch { return; } });
                    if (data != null)
                        return data; 
                    else  return new Command<string>(NewFunction());
                }
                catch
                {
                    return new Command<string>(NewFunction());
                }
            }
            
        }

        private Action<string> NewFunction()
        {
            return null;
        }

        

        public string PageHeaderText
        {
            get { return pageHeaderText; }
            set
            {
                SetProperty(ref pageHeaderText, value);
            }
        }
    }
}

using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using JGC.Common.Extentions;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.Common.Helpers;
using JGC.ViewModels.E_Reporter;

namespace JGC.ViewModels
{
    public class EReportSelectionViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private T_UserProject _selectedUserProject;
        #region Properties  
        private List<string> userProjects;
        public List<string> UserProjects
        {
            get => userProjects;
            set
            {
                SetProperty(ref userProjects, value);
                RaisePropertyChanged("UserProjects");
            }
        }
        private List<string> reportList;
        public List<string> ReportList
        {
            get => reportList;
            set
            {
                SetProperty(ref reportList, value);
                RaisePropertyChanged("ReportList");
            }
        }
        private string backgroundColorQc;
        public string BackgroundColorQc
        {
            get { return backgroundColorQc; }
            set { backgroundColorQc = value; RaisePropertyChanged();}
        }
        private string backgroundColorMaterial;
        public string BackgroundColorMaterial
        {
            get { return backgroundColorMaterial; }
            set { backgroundColorMaterial = value; RaisePropertyChanged(); }
        }
        private string textColorQc { get; set; }
        public string TextColorQc
        {
            get { return textColorQc; }
            set { textColorQc = value; RaisePropertyChanged(); }
        }
        private string textColorMaterial { get; set; }
        public string TextColorMaterial
        {
            get { return textColorMaterial; }
            set { textColorMaterial = value; RaisePropertyChanged(); }
        }       
        private bool isVisibleArrow1 { get; set; }
        public bool IsVisibleArrow1
        {
            get { return isVisibleArrow1; }
            set { isVisibleArrow1 = value; RaisePropertyChanged(); }
        }
        private bool isVisibleArrow2 { get; set; }
        public bool IsVisibleArrow2
        {
            get { return isVisibleArrow2; }
            set { isVisibleArrow2 = value; RaisePropertyChanged(); }
        }
        private string selectedReport;
        public string SelectedReport
        {
            get { return selectedReport; }
            set
            {
                if (SetProperty(ref selectedReport, value))
                {
                    NavigateToEDWR_EReporterPage(selectedReport);
                }
            }
        }
        
        private bool isVisibleList { get; set; }
        public bool IsVisibleList
        {
            get { return isVisibleList; }
            set { isVisibleList = value; RaisePropertyChanged(); }
        }
        private bool isVisibleDWR { get; set; }
        public bool IsVisibleDWR
        {
            get { return isVisibleDWR; }
            set { isVisibleDWR = value; RaisePropertyChanged(); }
        }
        private bool isVisibleArrow3 { get; set; }
        public bool IsVisibleArrow3
        {
            get { return isVisibleArrow3; }
            set { isVisibleArrow3 = value; RaisePropertyChanged(); }
        }
        private bool isVisibleAddJoints { get; set; }
        public bool IsVisibleAddJoints
        {
            get { return isVisibleAddJoints; }
            set { isVisibleAddJoints = value; RaisePropertyChanged(); }
        }
        private bool isVisibleInspectJoints { get; set; }
        public bool IsVisibleInspectJoints
        {
            get { return isVisibleInspectJoints; }
            set { isVisibleInspectJoints = value; RaisePropertyChanged(); }
        }
        private bool isVisibleDWRbtns { get; set; }
        public bool IsVisibleDWRbtns
        {
            get { return isVisibleDWRbtns; }
            set { isVisibleDWRbtns = value; RaisePropertyChanged(); }
        }
        #endregion

        #region Delegate Commands  
        public ICommand ReportListCommand
        {
            get
            {
                return new Command<string>(OnReportClicked);
            }
        }
        public ICommand JointsCommand
        {
            get
            {
                return new Command<string>(OnJointsClicked);
            }
        }

        #endregion

        public EReportSelectionViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_UserProject> _userProjectRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            _userDialogs.HideLoading();
            BackgroundColorMaterial = BackgroundColorQc = "White";
            TextColorQc = TextColorMaterial = "Black";
            Settings.DownloadParam = "";
            // _selectedUserProject = EReporterHelper.EReporter_SelectedProject;
            PageHeaderText = "EReport Selection";
            DWRHelper.DWRTargetType = typeof(EReportSelectionViewModel);
        }

        #region Private
        private async void OnReportClicked(string param)
        {
            try
            {
                if (param == "QC Report")
                {
                    //ReportList = new List<string> { "Daily Weld Report" , "Add Joints", "Inspect Joints"};
                    Settings.DownloadParam = "JointDetailsPage";
                    BackgroundColorQc = "#FB1610";
                    BackgroundColorMaterial = "White";
                    TextColorQc = "White";
                    TextColorMaterial = "Black";
                    IsVisibleArrow1 = true;
                    IsVisibleArrow2 = IsVisibleList = false;
                    IsVisibleDWR = IsVisibleDWRbtns = true;
                }
                else if(param == "Material Report")
                {
                    Settings.DownloadParam = "";
                    ReportList = new List<string> { "Receiving Inspection Request", "Material Receiving Report", "Construction Material Requisition" };
                    BackgroundColorMaterial = "#FB1610";
                    BackgroundColorQc = "White";
                    TextColorQc = "Black";
                    TextColorMaterial = "White";
                    IsVisibleArrow1 = IsVisibleDWR = IsVisibleArrow3 = IsVisibleAddJoints = IsVisibleInspectJoints = IsVisibleDWRbtns = false;
                    IsVisibleArrow2 = IsVisibleList = true;
                }
                 else if (param == "DWR")
                {
                    IsVisibleArrow3 = IsVisibleAddJoints = IsVisibleInspectJoints = true;
                }

            }
            catch (Exception ex)
            {

            }
        }
       private async void OnJointsClicked(string param)
        {
            if (param == "AddJoints")
                await navigationService.NavigateFromMenuAsync(typeof(DWR_AddJointViewModel));
            else
                await navigationService.NavigateFromMenuAsync(typeof(DWR_InspectJointViewModel));
        }
        #endregion
        #region Public
        public async void NavigateToEDWR_EReporterPage(string Report)
        {
            if (Report == null || IsRunningTasks)
            {
                return;
            }
            IsRunningTasks = true;
            _userDialogs.ShowLoading("Loading...");
            Settings.Report = Report;
            await navigationService.NavigateAsync<DashboardViewModel>();
            IsRunningTasks = false;

            // IsRunningTasks = true;
            // _userDialogs.ShowLoading("Loading...");
            //// Settings.Report = Report;

            // if (Report == "Daily Weld Report")
            // {

            //     await navigationService.NavigateAsync<DashboardViewModel>();
            //     IsRunningTasks = false;
            // }
            // else if (Report == "Add Joints") 
            // {
            //     Settings.Report = "Daily Weld Report";
            //      // await navigationService.NavigateAsync<JointDetailsViewModel>();
            //       await navigationService.NavigateFromMenuAsync(typeof(JointDetailsViewModel));
            //      IsRunningTasks = false;
            //        //SS Settings.Report = Report;


            // }
            // else if (Report == "Inspect Joints")
            // {
            //     Settings.Report = "Daily Weld Report";
            //     await navigationService.NavigateAsync<InspectJointViewModel>();
            //   //  await navigationService.NavigateFromMenuAsync(typeof(InspectJointViewModel));

            //     IsRunningTasks = false;
            // }


        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.Count == 0)
            {
                return;
            }

            if (parameters.Count > 1 && parameters.ContainsKey(NavigationParametersConstants.SelectedProjectParameter))
            {
                _selectedUserProject = (T_UserProject)parameters[NavigationParametersConstants.SelectedProjectParameter];
            }
        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
        }
        #endregion
    }
}

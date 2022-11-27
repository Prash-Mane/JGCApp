using Acr.UserDialogs;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.DataBase.DataTables.WorkPack;
using Prism.Common;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace JGC.ViewModels.Work_Pack
{
     public class ManPowerResourceViewModel:BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        protected readonly IUserDialogs _userDialogs;
        protected readonly IHttpHelper _httpHelper;
        protected readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_ManPowerResource> _manPowerResourceRepository;
        private readonly IRepository<T_ManPowerLog> _manPowerLogRepository; 
        private readonly IRepository<T_WorkerScanned> _workerScannedRepository;
        private readonly IRepository<T_IWP> _iwpRepository;
        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private T_IWP CurrentIWP;
        ZXingScannerPage scanPage;
        private INavigation NavPage;
        public T_UserDetails CurrentUserDetail;

        #region Properties      
        private DateTime selectedDate { get; set; }
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set {
                selectedDate = value;
                if (selectedDate < DateTime.Today)
                    selectedDate = DateTime.Today;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<T_WorkerScanned> workerList;
        public ObservableCollection<T_WorkerScanned> WorkerList
        {
            get { return workerList; }
            set { workerList = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<T_ManPowerLog> assignedList;
        public ObservableCollection<T_ManPowerLog> AssignedList
        {
            get { return assignedList; }
            set { assignedList = value; RaisePropertyChanged(); }
        }
        private T_WorkerScanned selectedWorker;
        public T_WorkerScanned SelectedWorker
        {
            get { return selectedWorker; }
            set
            {
                SetProperty(ref selectedWorker, value);
            }
        }
        private T_ManPowerLog selectedAssignedWorker;
        public T_ManPowerLog SelectedAssignedWorker
        {
            get { return selectedAssignedWorker; }
            set
            {
                SetProperty(ref selectedAssignedWorker, value);
            }
        }

        #endregion

        #region Delegate Commands  
        public ICommand BtnCommand
        {
            get
            {
                try
                {
                    return new Command<string>(OnClickButton);
                }
                catch(Exception ex)
                {
                    return null;
                }
            }
        }       

        #endregion

        public ManPowerResourceViewModel(INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_ManPowerResource> _manPowerResourceRepository,
            IRepository<T_ManPowerLog> _manPowerLogRepository,
            IRepository<T_WorkerScanned> _workerScannedRepository,
             IRepository<T_UserDetails> _userDetailsRepository,
            IRepository<T_IWP> _iwpRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._manPowerResourceRepository = _manPowerResourceRepository;
            this._manPowerLogRepository = _manPowerLogRepository;
            this._workerScannedRepository = _workerScannedRepository;
            this._userDetailsRepository = _userDetailsRepository;
            this._iwpRepository = _iwpRepository;
            _userDialogs.HideLoading();
            PageHeaderText = "Man Power Resource";
            JobSettingHeaderVisible = true;

            //load ManPawerResource
            PopulateCurrentuserAsWorker();
            GetMPRData();
            GetAssignedList();
        }

        private async void OnClickButton(string param)
        {
            if(param == "AddWorker")
            {
                List<T_ManPowerLog> AllAssigneditem = new List<T_ManPowerLog>();

                if (SelectedWorker != null)
                {
                    T_ManPowerLog Assigneditem = new T_ManPowerLog()
                    {
                        ProjectID = SelectedWorker.ProjectID,// Settings.ProjectID,
                        IWPID = CurrentIWP.ID,
                        WPID = CurrentIWP.Title,
                        WorkerID = SelectedWorker.WorkerID,
                        WorkerName = SelectedWorker.WorkerName,
                        CompanyCode = SelectedWorker.CompanyCode,
                        SectionCode = SelectedWorker.SectionCode,
                        FunctionCode = SelectedWorker.FunctionCode,
                        StartTime = DateTime.Now,
                        EndTime = null,
                        Updated = true
                    };
                    bool IsInserted = AssignedList.Where(i => i.WorkerID == Assigneditem.WorkerID).Count() > 0 ? false : true;

                    if (IsInserted)
                        AllAssigneditem.Add(Assigneditem);
                    else
                       await UserDialogs.Instance.AlertAsync($"This worker already exist", $"Already exist", "Ok");

                    await _workerScannedRepository.QueryAsync<T_WorkerScanned>("DELETE FROM T_WorkerScanned WHERE WorkerID = '"+ SelectedWorker.WorkerID + "'");
                }
                else
                {
                    List<T_WorkerScanned> RMList = new List<T_WorkerScanned>();
                    RMList = WorkerList.ToList();
                    foreach (T_WorkerScanned worker in WorkerList)
                    {
                        T_ManPowerLog Assigneditem = new T_ManPowerLog()
                        {
                            ProjectID = worker.ProjectID,// Settings.ProjectID,
                            IWPID = CurrentIWP.ID,
                            WPID = CurrentIWP.Title,
                            WorkerID = worker.WorkerID,
                            WorkerName = worker.WorkerName,
                            CompanyCode = worker.CompanyCode,
                            SectionCode = worker.SectionCode,
                            FunctionCode = worker.FunctionCode,
                            StartTime = DateTime.Now,
                            EndTime = null,
                            Updated = true
                        };
                        bool IsInserted = AssignedList.Where(i => i.WorkerID == Assigneditem.WorkerID).Count() > 0 ? false : true;

                        if (IsInserted)
                            AllAssigneditem.Add(Assigneditem);
                        await _workerScannedRepository.QueryAsync<T_WorkerScanned>("DELETE FROM T_WorkerScanned WHERE WorkerID = '" + worker.WorkerID + "'");
                    }

                }

                GetMPRData();
                if (AllAssigneditem.Count > 0)
                {
                    await _manPowerLogRepository.InsertOrReplaceAsync(AllAssigneditem);
                    UpdatedWorkPack(IWPHelper.IWP_ID);
                    GetAssignedList();
                }               
            }
            else if(param == "ScanQRcode")
            {
                try
                {
                    string scanResult =string.Empty;
                    scanPage = new ZXingScannerPage();
                    scanPage.OnScanResult += (result) => {
                    scanPage.IsScanning = false;

                        Device.BeginInvokeOnMainThread(() => {
                             NavPage.PopModalAsync();
                            scanResult = result.Text;
                            UserDialogs.Instance.AlertAsync(result.Text, $"result", "Ok");
                        });
                    };

                    await NavPage.PushModalAsync(scanPage);
                    if (!String.IsNullOrEmpty(scanResult.Trim()))
                    {
                        var MPR = await _manPowerResourceRepository.QueryAsync<T_ManPowerResource>("Select * from T_ManPowerResource where BeaconID = '" + scanResult + "'");

                        if (MPR.Count > 0)
                        {
                            var WorkerScanList = new List<T_WorkerScanned>();
                            T_ManPowerResource mpr = MPR.FirstOrDefault();

                            WorkerScanList.Add(new T_WorkerScanned()
                            {
                                ProjectID = Settings.ProjectID,
                                WorkerID = mpr.WorkerID,
                                WorkerName = mpr.WorkerName,
                                CompanyCode = mpr.CompanyCode,
                                SectionCode = mpr.SectionCode,
                                FunctionCode = mpr.FunctionCode,
                                RFID = mpr.RFID,
                                BeaconID = mpr.BeaconID
                            });

                            string sql = " Select * from T_WorkerScanned where WorkerID = '" + mpr.WorkerID + "'";
                            var WS = await _workerScannedRepository.QueryAsync<T_WorkerScanned>(sql);
                            bool IsInserted = WS.Count() > 0 ? false : true;
                            if (IsInserted)
                                await _workerScannedRepository.InsertOrReplaceAsync(WorkerScanList);
                            else
                                await UserDialogs.Instance.AlertAsync($"This worker already exist", $"Already exist", "Ok");

                            GetMPRData();
                        }                      
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        public async void ScanQRCode(string QRCode)
        {
            try
            {               
                if (!String.IsNullOrEmpty(QRCode.Trim()))
                {
                    var MPR = await _manPowerResourceRepository.QueryAsync<T_ManPowerResource>("Select * from T_ManPowerResource where BeaconID = '" + QRCode + "'");

                    if (MPR.Count > 0)
                    {
                        var WorkerScanList = new List<T_WorkerScanned>();
                        T_ManPowerResource mpr = MPR.FirstOrDefault();

                        WorkerScanList.Add(new T_WorkerScanned()
                        {
                            ProjectID = Settings.ProjectID,
                            WorkerID = mpr.WorkerID,
                            WorkerName = mpr.WorkerName,
                            CompanyCode = mpr.CompanyCode,
                            SectionCode = mpr.SectionCode,
                            FunctionCode = mpr.FunctionCode,
                            RFID = mpr.RFID,
                            BeaconID = mpr.BeaconID
                        });

                        string sql = " Select * from T_WorkerScanned where WorkerID = '" + mpr.WorkerID + "'";
                        var WS = await _workerScannedRepository.QueryAsync<T_WorkerScanned>(sql);
                        bool IsInserted = WS.Count() > 0 ? false : true;
                        if (IsInserted)
                            await _workerScannedRepository.InsertOrReplaceAsync(WorkerScanList);
                        else
                            await UserDialogs.Instance.AlertAsync($"This worker already exist", $"Already exist", "Ok");

                        GetMPRData();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private async void GetMPRData()
        {
            var getIWP = await _iwpRepository.QueryAsync<T_IWP>("SELECT * FROM [T_IWP] WHERE [ID] = '" + IWPHelper.IWP_ID + "'");

            CurrentIWP = getIWP.FirstOrDefault();
            var MPR = await _workerScannedRepository.GetAsync();
            WorkerList = new ObservableCollection<T_WorkerScanned>(MPR);            
        }
        private async void GetAssignedList()
        {
            var list = await _manPowerLogRepository.GetAsync(i=>i.IWPID == IWPHelper.IWP_ID && i.ISDeleted==false);
            AssignedList = new ObservableCollection<T_ManPowerLog>(list);
        }
        private async void UpdatedWorkPack(int IWP)
        {
            string SQL = "UPDATE [T_IWP] SET [Updated] = 1 WHERE [ID] = '" + IWP + "'";
            var result = await _iwpRepository.QueryAsync(SQL);
        }

        public async void PopulateCurrentuserAsWorker()
        {
            var UserDetailsList = await _userDetailsRepository.GetAsync();
            if (UserDetailsList.Count > 0)
                CurrentUserDetail = UserDetailsList.Where(p => p.ID == Settings.UserID).FirstOrDefault();
            try
            {
                if (!String.IsNullOrEmpty(CurrentUserDetail.WorkerID.Trim()))
                {
                    var MPR = await _manPowerResourceRepository.QueryAsync<T_ManPowerResource>("Select * from T_ManPowerResource where WorkerID = '" + CurrentUserDetail.WorkerID + "'");

                    if (MPR.Count > 0)
                    {
                        var WorkerScanList = new List<T_WorkerScanned>();
                        T_ManPowerResource mpr = MPR.FirstOrDefault();

                        WorkerScanList.Add(new T_WorkerScanned()
                        {
                            ProjectID = Settings.ProjectID,
                            WorkerID = mpr.WorkerID,
                            WorkerName = mpr.WorkerName,
                            CompanyCode = mpr.CompanyCode,
                            SectionCode = mpr.SectionCode,
                            FunctionCode = mpr.FunctionCode,
                            RFID = mpr.RFID,
                            BeaconID = mpr.BeaconID
                        });

                        string sql = " Select * from T_WorkerScanned where WorkerID = '" + mpr.WorkerID + "'";
                        var WS = await _workerScannedRepository.QueryAsync<T_WorkerScanned>(sql);
                        bool IsInserted = WS.Count() > 0 ? false : true;
                        if (IsInserted)
                            await _workerScannedRepository.InsertOrReplaceAsync(WorkerScanList);
                        else
                            await UserDialogs.Instance.AlertAsync($"This worker already exist", $"Already exist", "Ok");

                        GetMPRData();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Public
        public async void NavigateToRemoveWorkerItem()
        {
            if (SelectedWorker != null && await UserDialogs.Instance.ConfirmAsync($"Are you sure you want to remove this Worker?", $"Remove Worker", "Yes", "No"))
            {
                // WorkerList.Remove(SelectedWorker);       
                await _workerScannedRepository.QueryAsync<T_WorkerScanned>("Delete From T_WorkerScanned where WorkerID = '" + SelectedWorker.WorkerID + "'");
                GetMPRData();
            }
        }
        public async void NavigateToRemoveAssignedWorkerItem()
        {
            if (SelectedAssignedWorker != null && await UserDialogs.Instance.ConfirmAsync($"Are you sure you want to remove this Assigned Worker?", $"Remove Assigned Worker", "Yes", "No"))
            {
                // AssignedList.Remove(SelectedAssignedWorker);
                await _manPowerLogRepository.QueryAsync<T_ManPowerLog>("Update [T_ManPowerLog] SET [Updated] = 1 , [ISDeleted] = 1 , [EndTime]= '"+DateTime.Now.Ticks+"' where WorkerID  = '" + SelectedAssignedWorker.WorkerID + "'");               
                UpdatedWorkPack(IWPHelper.IWP_ID);
                GetAssignedList();
            }
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
        #endregion
    }
}

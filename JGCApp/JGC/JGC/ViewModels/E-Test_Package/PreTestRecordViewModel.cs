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
using System.Windows.Input;
using Xamarin.Forms;
using System;
using JGC.Models.E_Test_Package;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Services;
using JGC.UserControls.PopupControls;
using JGC.ViewModels.E_Reporter;
using JGC.Views.E_Reporter;

namespace JGC.ViewModels.E_Test_Package
{
  
    public class PreTestRecordViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_ETestPackages> _eTestPackagesRepository;
        private readonly IRepository<T_AdminPreTestRecordContent> _adminPreTestRecordContentRepository;
        private readonly IRepository<T_PreTestRecordAcceptedBy> _PreTestRecordAcceptedByRepository;

        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private readonly IRepository<T_ControlLogSignature> _controlLogSignature;
        private readonly IRepository<T_AttachedDocument> _attachedDocumentRepository;
        private readonly IRepository<T_AdminControlLog> _AdminControlLog;
        private readonly IRepository<T_AdminControlLogFolder> _AdminControlLogFolderRepository;
        private readonly IRepository<T_PreTestRecordContent> _PreTestRecordContentRepository;
        private readonly IRepository<T_AdminPreTestRecordAcceptedBy> _AdminPreTestRecordAcceptedByRepository;
        private readonly IRepository<T_AdminControlLogPunchLayer> _AdminControlLogPunchLayerRepository;
        private readonly IRepository<T_AdminControlLogPunchCategory> _AdminControlLogPunchCategoryRepository;
        private readonly IRepository<T_AdminControlLogActionParty> _AdminControlLogActionPartyRepository;
        private readonly IRepository<T_PunchList> _PunchListRepository;

        private T_UserDetails UserDetails;
        private T_UserProject CurrentUserProject;
        public PreTestRecordViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_AdminPreTestRecordContent> _adminPreTestRecordContentRepository,
            IRepository<T_PreTestRecordAcceptedBy> _PreTestRecordAcceptedByRepository,
            IRepository<T_ETestPackages> _eTestPackagesRepository,

            IRepository<T_UserDetails> _userDetailsRepository,
            IRepository<T_ControlLogSignature> _controlLogSignature,
            IRepository<T_AttachedDocument> _attachedDocumentRepository,
            IRepository<T_AdminControlLog> _AdminControlLog,
            IRepository<T_AdminControlLogFolder> _AdminControlLogFolderRepository,
            IRepository<T_PreTestRecordContent> _PreTestRecordContentRepository,
            IRepository<T_AdminPreTestRecordAcceptedBy> _AdminPreTestRecordAcceptedByRepository,
            IRepository<T_AdminControlLogPunchLayer> _AdminControlLogPunchLayerRepository,
            IRepository<T_AdminControlLogPunchCategory> _AdminControlLogPunchCategoryRepository,
            IRepository<T_AdminControlLogActionParty> _AdminControlLogActionPartyRepository,
            IRepository<T_PunchList> _PunchListRepository,
        IRepository<T_UserProject> _userProjectRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._userProjectRepository = _userProjectRepository;
            this._eTestPackagesRepository = _eTestPackagesRepository;
            this._adminPreTestRecordContentRepository = _adminPreTestRecordContentRepository;
            this._PreTestRecordAcceptedByRepository = _PreTestRecordAcceptedByRepository;

            this._userDetailsRepository = _userDetailsRepository;
            this._controlLogSignature = _controlLogSignature;
            this._attachedDocumentRepository = _attachedDocumentRepository;
            this._AdminControlLog = _AdminControlLog;
            this._AdminControlLogFolderRepository = _AdminControlLogFolderRepository;
            this._PreTestRecordContentRepository = _PreTestRecordContentRepository;
            this._AdminPreTestRecordAcceptedByRepository = _AdminPreTestRecordAcceptedByRepository;
            this._AdminControlLogPunchLayerRepository = _AdminControlLogPunchLayerRepository;
            this._AdminControlLogPunchCategoryRepository = _AdminControlLogPunchCategoryRepository;
            this._AdminControlLogActionPartyRepository = _AdminControlLogActionPartyRepository;
            this._PunchListRepository = _PunchListRepository;
            _userDialogs.HideLoading();
            MainGrid = true;
            IsHeaderBtnVisible = true;
            LoadCertificationTabsAsync();
            PageHeaderText = "Drain Record";
        }

        #region properties
        private bool _signatureGrid;
        public bool SignatureGrid
        {
            get { return _signatureGrid; }
            set { SetProperty(ref _signatureGrid, value); }
        }

        private DateTime _DrainingDate;
        public DateTime DrainingDate
        {
            get { return _DrainingDate; }
            set { SetProperty(ref _DrainingDate, value); }
        }

        private String _TestMedia;
        public String TestMedia
        {
            get { return _TestMedia; }
            set { SetProperty(ref _TestMedia, value); }
        }
        private string testPackage;
        public string TestPackage
        {
            get { return testPackage; }
            set { SetProperty(ref testPackage, value); }
        }

        private String _Remarks;
        public String Remarks
        {
            get { return _Remarks; }
            set { SetProperty(ref _Remarks, value); }
        }



        private bool mainGrid;
        public bool MainGrid
        {
            get { return mainGrid; }
            set { SetProperty(ref mainGrid, value); }
        }
        private string drainRecordRemarks;
        public string DrainRecordRemarks
        {
            get { return drainRecordRemarks; }
            set { drainRecordRemarks = value; RaisePropertyChanged(); }
        }
        public ICommand BtnCommand
        {
            get
            {
                return new Command<string>(OnClickButton);
            }
        }


        private ObservableCollection<RecordConfirmation> recordConfirmationSource;
        public ObservableCollection<RecordConfirmation> RecordConfirmationSource
        {
            get { return recordConfirmationSource; }
            set { recordConfirmationSource = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<RecordAcceptedBy> recordAcceptedBySource;
        public ObservableCollection<RecordAcceptedBy> RecordAcceptedBySource
        {
            get { return recordAcceptedBySource; }
            set { recordAcceptedBySource = value; RaisePropertyChanged(); }
        }

        private RecordConfirmation selectedConfirmationSource;
        public RecordConfirmation SelectedConfirmationSource
        {
            get { return selectedConfirmationSource; }
            set { selectedConfirmationSource = value; RaisePropertyChanged(); }
        }
        private RecordAcceptedBy selectedrecordAcceptedBySource;
        public RecordAcceptedBy SelectedrecordAcceptedBySource
        {
            get { return selectedrecordAcceptedBySource; }
            set { selectedrecordAcceptedBySource = value; RaisePropertyChanged(); }
        }
        #endregion

        public async void ShowDescriptionPopup(string Description)
        {
            if (Description == null)
                return;
            if (!string.IsNullOrWhiteSpace(Description))
                await PopupNavigation.PushAsync(new ShowWrapTextPopup("Description", Description), true);
        }
        private async Task LoadCertificationTabsAsync()
        {
            try
            {
                var user = await _userDetailsRepository.GetAsync(x => x.ID == Settings.UserID);
                UserDetails = new T_UserDetails();
                UserDetails = user.FirstOrDefault();


                //Load Top Details
                Boolean visualTest = false;

                string SQL = "SELECT * FROM[T_ETestPackages] WHERE[ProjectID] ='" + Settings.ProjectID + "' AND[ID] = '" + CurrentPageHelper.ETestPackage.ID + "'";

                var testPackage = await _eTestPackagesRepository.QueryAsync<T_ETestPackages>(SQL);


                if (testPackage.Any())
                {
                    var _testPackage = testPackage.FirstOrDefault();
                    TestPackage = _testPackage.TestPackage;
                    TestMedia = _testPackage.TestMedia;
                    DrainRecordRemarks = _testPackage.DrainRecordRemarks;
                    DrainingDate = (DateTime)_testPackage.DrainingDate;
                    if (DrainingDate < Convert.ToDateTime("2000-01-01"))
                        DrainingDate = DateTime.Now;

                    //calDrainingDate.SelectionStart = drainingDate;
                    //calDrainingDate.SelectionEnd = drainingDate;
                    //calDrainingDate.SetDate(drainingDate);
                }

                CurrentUserProject = new T_UserProject();
                var QueryResult = await _userProjectRepository.GetAsync(x => x.Project_ID == Settings.ProjectID);
                CurrentUserProject = QueryResult.FirstOrDefault();

                // var CurrentUserProject = await _userProjectRepository.GetAsync(x => x.Project_ID == Settings.ProjectID);
                var tsk1 = await GenerateContentTableAsync(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, "T_AdminPreTestRecordContent", "T_PreTestRecordContent", CurrentUserProject);
                var tsk2 = await GenerateAcceptedByTableAsync(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, "T_AdminPreTestRecordAcceptedBy", "T_PreTestRecordAcceptedBy", CurrentUserProject);

                RecordConfirmationSource = new ObservableCollection<RecordConfirmation>(tsk1);
                RecordAcceptedBySource = new ObservableCollection<RecordAcceptedBy>(tsk2);

            }
            catch (Exception Ex)
            { }


        }
        private async Task<List<RecordConfirmation>> GenerateContentTableAsync(int projectID, int etestPackageID, string adminDatabaseTable, string databaseTable, T_UserProject userProject)
        {
            List<RecordConfirmation> ListRecordConfirmation = new List<RecordConfirmation>();
            try
            {
                string SQL = "SELECT ADMIN.[ID],ADMIN.[Description],ADMIN.[OrderNo],ADMIN.[PIC],SIG.[Live],SIG.[Signed],SIG.[SignedBy],SIG.[SignedOn] FROM [" + adminDatabaseTable + "] AS ADMIN LEFT JOIN " +
                    "(SELECT * FROM [" + databaseTable + "] WHERE [ETestPackageID] = '" + etestPackageID + "') AS SIG ON ADMIN.[ID] = SIG.[ADMINID] " +
                    "WHERE ADMIN.[ProjectID] = '" + projectID + "' ORDER BY ADMIN.[OrderNo] ASC";

                var reuslt = await _adminPreTestRecordContentRepository.QueryAsync<RecordConfirmation>(SQL);

                if (reuslt.Any())
                {
                    foreach (RecordConfirmation Rc in reuslt)
                    {
                        //Image signedImage = signed ? jgcetestpackage.Properties.Resources.greendot : jgcetestpackage.Properties.Resources.greydot;
                        //string signedOn = signed ? AdjustDateTime(SQLReader.GetRealDateTime(Reader, "SignedOn"), userProject) : "";
                        Rc.SignedImage = Rc.Signed ? "Greenradio.png" : "Grayradio.png";
                        Rc.btnNASign = "N/A Sign off";

                        ListRecordConfirmation.Add(Rc);
                    }
                }
            }
            catch (Exception Ex)
            {

            }

            return ListRecordConfirmation;
        }


        public async Task<List<RecordAcceptedBy>> GenerateAcceptedByTableAsync(int projectID, int etestPackageID, string adminDatabaseTable, string databaseTable, T_UserProject userProject)
        {
            List<RecordAcceptedBy> ListRecordAcceptedBy = new List<RecordAcceptedBy>();

            string SQL = "SELECT ADMIN.[ID],ADMIN.[Description],ADMIN.[OrderNo],SIG.[Live],SIG.[Signed],SIG.[SignedBy],SIG.[SignedOn] FROM [" + adminDatabaseTable + "] AS ADMIN LEFT JOIN " +
                "(SELECT * FROM [" + databaseTable + "] WHERE [ETestPackageID] = '" + etestPackageID + "') AS SIG ON ADMIN.[ID] = SIG.[ADMINID] " +
                "WHERE ADMIN.[ProjectID] = '" + projectID + "' ORDER BY ADMIN.[OrderNo] ASC";

            var result = await _PreTestRecordAcceptedByRepository.QueryAsync<RecordAcceptedBy>(SQL);

            if (result.Any())
            {
                foreach (RecordAcceptedBy Ra in result)
                {
                    Ra.SignedImage = Ra.Signed ? "Greenradio.png" : "Grayradio.png";

                    ListRecordAcceptedBy.Add(Ra);
                }
            }
            return ListRecordAcceptedBy;
        }

        private async void btnSaveDrainingDate_Click()
        {
            if (DrainingDate <= DateTime.Now.Date)
            {
                string SQL = "UPDATE [T_ETestPackages] SET [DrainingDate] = '" + DrainingDate.Ticks + "', [DrainingDateUpdated] = 1 WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ID] = '" + CurrentPageHelper.ETestPackage.ID + "'";
                var reslut = await _eTestPackagesRepository.QueryAsync(SQL);
                UpdateETestPackageStatus(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID);

                await _userDialogs.AlertAsync("Successfully Saved....!", null, "Ok");
            }
            else
                await _userDialogs.AlertAsync("Draining date must not be in the future, please reselect the date", "Save Draining Date", "Ok");

        }

        private async void btnSaveDrainRecordRemarks_Click()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(DrainRecordRemarks))
                {
                    string SQL = "UPDATE [T_ETestPackages] SET [DrainRecordRemarks] = '" + DrainRecordRemarks + "', [DrainRecordRemarksUpdated] = 1 WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ID] = '" + CurrentPageHelper.ETestPackage.ID + "'";

                    var result = await _eTestPackagesRepository.QueryAsync(SQL);
                    UpdateETestPackageStatus(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID);
                    await _userDialogs.AlertAsync("Successfully Saved....!", null, "Ok");
                }
                else
                {
                    await _userDialogs.AlertAsync("Please enter Remark", null, "Ok");
                }
            }
            catch (Exception Ex)
            {
                await _userDialogs.AlertAsync("Faild", null, "Ok");
            }
        }
        private async void UpdateETestPackageStatus(int projectId, int TestPackageID)
        {
            string SQL = "UPDATE [T_ETestPackages] SET [Updated] = 1 WHERE [ProjectID] = '" + projectId + "'"
            + " AND [ID] = '" + TestPackageID + "'";
            var result = await _eTestPackagesRepository.QueryAsync(SQL);
        }

        public string SaveDrainingDate { get; private set; }

        private async void OnClickButton(string param)
        {
            if (param == "SignatureGrid")
            {
                SignatureGrid = true;
                MainGrid = false;
            }
            else if (param == "BackfromSignatureGrid")
            {
                SignatureGrid = false;
                MainGrid = true;
            }

            else if (param == "SaveDrainingDate")
            {
                btnSaveDrainingDate_Click();
            }
            else if (param == "BackfromSignatureGrid")
            {

            }

            else if (param == "SaveRemarks")
            {
                btnSaveDrainRecordRemarks_Click();
            }
            else if (param == "BackfromSignatureGrid")
            {

            }
        }





        #region Signature Logic


        public async void gvDrainRecordContent_CellContentClick(string param, RecordConfirmation SelectedRecord)
        {
            SelectedConfirmationSource = SelectedRecord;
            //// await  _userDialogs.AlertAsync("Sorry, You Do Not Have The Correct User Right To Sign Off This Signature", "","OK");
            bool btnNA = false;
            if (param == "btnNA")
            {
                btnNA = true;
            }


            if (SelectedConfirmationSource != null || btnNA == true) //Status Image
            {


                // int rowIndex = e.RowIndex;
                int adminID = SelectedConfirmationSource.ID;

                Boolean signed = SelectedConfirmationSource.Signed;
                Boolean live = SelectedConfirmationSource.Live;

                if (await CanSignOffCertificiationSignature("T_AdminPreTestRecordContent", "T_PreTestRecordContent", adminID, signed, live))
                {
                    T_PreTestRecordContent signature = new T_PreTestRecordContent()
                    {
                        AdminID = adminID,
                        Signed = !signed,
                        SignedByUserID = Settings.UserID,
                        SignedBy = UserDetails.FullName,
                        SignedOn = DateTime.UtcNow,
                    };
                    if (btnNA == true)
                    {
                        signature.SignedByUserID = 0;
                        signature.SignedBy = "N/A";
                    }
                    if (await SaveNew(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, "T_PreTestRecordContent", signature))
                    {
                        UpdateETestPackageStatus(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID);

                        //   gvDrainRecordConfirmation.ClearSelection();
                        var tsk1 = await GenerateContentTableAsync(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, "T_AdminPreTestRecordContent", "T_PreTestRecordContent", CurrentUserProject);
                        RecordConfirmationSource = new ObservableCollection<RecordConfirmation>(tsk1);
                    }
                    else
                        await _userDialogs.AlertAsync("Error ocurred saving signature", null, "Ok");

                }
            }
        }
        public async void gvDrainRecordAcceptedBy_CellContentClick(string param, RecordAcceptedBy SelectedRecord)
        {
            SelectedrecordAcceptedBySource = SelectedRecord;

            if (SelectedrecordAcceptedBySource != null) //Status Image
            {


                // int rowIndex = e.RowIndex;
                int adminID = SelectedrecordAcceptedBySource.ID;
                //int signatureNo =
                Boolean signed = SelectedrecordAcceptedBySource.Signed;
                Boolean live = SelectedrecordAcceptedBySource.Live;

                int orderNo = SelectedrecordAcceptedBySource.OrderNo;

                if (await CanSignOffAcceptedBySignature("T_AdminPreTestRecordAcceptedBy", "T_PreTestRecordAcceptedBy", RecordConfirmationSource, adminID, signed, live, orderNo))
                {

                    T_PreTestRecordContent signature = new T_PreTestRecordContent()
                    {
                        AdminID = adminID,
                        Signed = !signed,
                        SignedByUserID = UserDetails.ID,
                        SignedBy = !signed ? UserDetails.FullName : "",
                        SignedOn = DateTime.UtcNow,
                    };

                    if (await SaveNew(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, "T_PreTestRecordAcceptedBy", signature))
                    {
                        if (!signed)
                        {
                            Boolean controlLogLinked = false;
                            int controlLogAdminID = 0;

                            string SQL = "SELECT * FROM [T_AdminPreTestRecordAcceptedBy] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ID] = '" + CurrentPageHelper.ETestPackage.ID + "'";
                            var result = await _AdminPreTestRecordAcceptedByRepository.QueryAsync<T_AdminPreTestRecordAcceptedBy>(SQL);

                            if (result.Any())
                            {
                                controlLogLinked = result.FirstOrDefault().ControlLogLinked;
                                controlLogAdminID = result.FirstOrDefault().ControlLogAdminID;
                            }

                            if (controlLogLinked)
                            {
                                //get control log row index.
                                if (await CanSignControlLogSignature(UserDetails, Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, CurrentPageHelper.ETestPackage.SubContractor, controlLogAdminID, false))
                                {
                                    T_ControlLogSignature controlLogSignature = new T_ControlLogSignature()
                                    {
                                        ControlLogAdminID = controlLogAdminID,
                                        Signed = !signed,
                                        SignedByUserID = UserDetails.ID,
                                        SignedBy = !signed ? UserDetails.FullName : "",
                                        SignedOn = DateTime.UtcNow,
                                    };

                                    if (await SaveNewAsync(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, controlLogSignature))
                                    {
                                        T_AdminControlLog adminControlLog1 = new T_AdminControlLog();
                                        var getData = await _AdminControlLog.GetAsync(x => x.ProjectID == Settings.ProjectID && x.ID == controlLogAdminID);  //adminControlLog.Get(Convert.ToInt32(CurrentProject.Project_ID), controlLogAdminID); //Get all control log admin details for checks

                                        //Adjust minor milestones
                                        List<T_AdminControlLog> list = await GetMinorMileStones(Settings.ProjectID, getData.FirstOrDefault().SignatureNo); //get list of minor milestones associated

                                        if (list != null && list.Count > 0) //if returned some minor milestones 
                                        {
                                            foreach (T_AdminControlLog adminControlLog in list) //go through list and adjust the signature's control log id and save.
                                            {
                                                controlLogSignature.ControlLogAdminID = adminControlLog.ID;
                                                await SaveNewAsync(Convert.ToInt32(Settings.ProjectID), CurrentPageHelper.ETestPackage.ID, controlLogSignature);
                                            }
                                        }


                                        AutoSignControlLogSignatures(UserDetails, Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, CurrentPageHelper.ETestPackage.SubContractor);
                                        UpdateETestPackageStatus(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID);



                                        //  await LoadControlLogTabAsync();
                                    }
                                    else
                                        await Application.Current.MainPage.DisplayAlert("Control Log Signature", "Error ocurred saving signature", "OK");

                                }
                            }
                        }

                        UpdateETestPackageStatus(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID);


                        var tsk2 = await GenerateAcceptedByTableAsync(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, "T_AdminPreTestRecordAcceptedBy", "T_PreTestRecordAcceptedBy", CurrentUserProject);
                        RecordAcceptedBySource = new ObservableCollection<RecordAcceptedBy>(tsk2);
                    }
                    else
                        await _userDialogs.AlertAsync("Error ocurred saving signature", null, "OK");

                }

            }
        }


        public async void AutoSignControlLogSignatures(T_UserDetails CurrentUser, int projectID, int etestPackageID, string subContractor)
        {
            //Get next control log id
            int nextControlLogAdminID = await GetNextControlLogIDAsync(projectID, etestPackageID);

            //check if you can sign the next control log as NA

            string sql = "SELECT * FROM  " +
               "[T_ControlLogSignature] CL " +
               "INNER JOIN [T_AdminControlLogNaAutoSignatures] NA ON CL.[ControlLogAdminID] = NA.[ControlLogAdminID] " +
               "WHERE CL.[ProjectID] = '" + projectID + "' AND CL.[ETestPackageID] = '" + etestPackageID + "' " +
              "AND CL.[Signed] = TRUE AND CL.[SignedByUserID] = 0 AND NA.[ProjectID] = '" + projectID + "' AND NA.[AutoSignOffControlLogAdminID] = '" + nextControlLogAdminID + "'";


            var result = await _controlLogSignature.QueryAsync<T_AdminControlLogNaAutoSignatures>(sql);
            var canSign = result;


            if (canSign.Any())
            {
                if (await CanSignControlLogSignature(CurrentUser, projectID, etestPackageID, subContractor, nextControlLogAdminID, false, true))
                {
                    T_ControlLogSignature controlLogSignature = new T_ControlLogSignature()
                    {
                        ControlLogAdminID = nextControlLogAdminID,
                        Signed = true,
                        SignedByUserID = 0,
                        SignedBy = "NA",
                        SignedOn = DateTime.UtcNow,
                    };

                    if (await SaveNewAsync(projectID, etestPackageID, controlLogSignature))
                    {
                        var sqldqweryData = await _AdminControlLog.GetAsync(x => x.ProjectID == projectID && x.ID == nextControlLogAdminID);

                        int signatureNo = sqldqweryData.FirstOrDefault().SignatureNo;


                        //Adjust minor milestones
                        List<T_AdminControlLog> list = await GetMinorMileStones(projectID, signatureNo); //get list of minor milestones associated

                        if (list != null && list.Count > 0) //if returned some minor milestones 
                        {
                            foreach (T_AdminControlLog adminControlLog in list) //go through list and adjust the signature's control log id and save.
                            {
                                controlLogSignature.ControlLogAdminID = adminControlLog.ID;
                                await SaveNewAsync(projectID, etestPackageID, controlLogSignature);
                            }
                        }

                        //Loop around
                        AutoSignControlLogSignatures(CurrentUser, projectID, etestPackageID, subContractor);
                    }
                }
            }
        }


        public async Task<int> GetNextControlLogIDAsync(int projectID, int etestPackageID)
        {
            var lastSigniture = await _AdminControlLog.QueryAsync<T_AdminControlLog>("SELECT * " +
                "FROM (T_AdminControlLog ADMIN INNER JOIN " +
                "T_ControlLogSignature CL ON ADMIN.ProjectID = CL.ProjectID AND ADMIN.ID = CL.ControlLogAdminID) " +
                "WHERE  (ADMIN.ProjectID = '" + projectID + "') AND (CL.ETestPackageID = '" + etestPackageID + "') AND (CL.Signed = '" + true + "') " +
                "ORDER BY ADMIN.[SignatureNo] DESC ");

            int lastSignatureNo = 0;

            if (!lastSigniture.Any()) return lastSignatureNo;
            else return lastSigniture.FirstOrDefault().SignatureNo;


        }
        public async Task<List<T_AdminControlLog>> GetMinorMileStones(int projectID, int signatureNo)
        {
            // List<T_AdminControlLog> list = new List<T_AdminControlLog>();
            var SQL = await _AdminControlLog.GetAsync(x => x.ProjectID == projectID && x.AssociatedSignatureNo == signatureNo);

            return SQL.ToList();
        }
        public async Task<bool> SaveNewAsync(int projectID, int etestPackageID, T_ControlLogSignature controlLogModel)
        {

            //CheckExists
            var objcontrolLogSignature = await _controlLogSignature.GetAsync(x => x.ProjectID == projectID && x.ETestPackageID == etestPackageID && x.ControlLogAdminID == controlLogModel.ControlLogAdminID);

            var CheckExists = objcontrolLogSignature.Any();
            if (CheckExists)

            {
                T_ControlLogSignature ControlLogSignature = new T_ControlLogSignature();
                ControlLogSignature = objcontrolLogSignature.FirstOrDefault();
                //Found use sql update.

                ControlLogSignature.Reject = false;
                ControlLogSignature.Signed = controlLogModel.Signed;
                ControlLogSignature.SignedByUserID = controlLogModel.SignedByUserID;
                ControlLogSignature.SignedBy = controlLogModel.SignedBy;
                ControlLogSignature.SignedOn = controlLogModel.SignedOn;
                ControlLogSignature.Live = false;
                ControlLogSignature.Updated = true;
                ControlLogSignature.ProjectID = projectID;
                ControlLogSignature.ETestPackageID = etestPackageID;
                ControlLogSignature.ControlLogAdminID = controlLogModel.ControlLogAdminID;

                string SQL = " UPDATE [T_ControlLogSignature] SET[Reject] = " + false + ", [Signed] = " + controlLogModel.Signed + ",[SignedByUserID] = '"
                    + controlLogModel.SignedByUserID + "' , [SignedBy] = '" + controlLogModel.SignedBy + "',[SignedOn] = '" + controlLogModel.SignedOn.ToString("dd-MMM-yyyy")
                    + "', [Live] = " + false + ", [Updated] = " + true + " WHERE[ProjectID] = " + projectID + " AND[ETestPackageID] = " + etestPackageID
                    + " AND[ControlLogAdminID] = " + controlLogModel.ControlLogAdminID + "";
                try
                {
                    var dBreturn = await _controlLogSignature.QueryAsync(SQL);

                    return true;

                }
                catch (Exception Ex)
                { return false; }



            }
            else
            {
                //Insert fresh details.
                T_ControlLogSignature ControlLogSignature = new T_ControlLogSignature();
                //  ControlLogSignature = objcontrolLogSignature.FirstOrDefault();
                ControlLogSignature.Signed = controlLogModel.Signed;
                ControlLogSignature.SignedByUserID = controlLogModel.SignedByUserID;
                ControlLogSignature.SignedBy = controlLogModel.SignedBy;
                ControlLogSignature.SignedOn = DateTime.UtcNow;
                ControlLogSignature.Live = false;
                ControlLogSignature.Updated = true;
                ControlLogSignature.ProjectID = projectID;
                ControlLogSignature.ETestPackageID = etestPackageID;
                ControlLogSignature.ControlLogAdminID = controlLogModel.ControlLogAdminID;

                var dBreturn = await _controlLogSignature.InsertAsync(ControlLogSignature);
                if (dBreturn == 1)
                    return true;
                else
                    return false;
            }

        }

        public async Task<Boolean> CanSignControlLogSignature(T_UserDetails CurrentUser, int projectID, int etestPackageID, string subContractor, int controllogAdminID, bool showPopups = true, bool naAutoSignOff = true)
        {
            Boolean live = false, signed = false;

            var getdBdata = await _controlLogSignature.GetAsync(x => x.ProjectID == projectID && x.ETestPackageID == etestPackageID && x.ControlLogAdminID == controllogAdminID);
            if (getdBdata.Any())
            {
                live = getdBdata.FirstOrDefault().Live;
                signed = getdBdata.FirstOrDefault().Signed;
            }

            var controlLogAdminDetails = await _AdminControlLog.GetAsync(x => x.ProjectID == projectID && x.ID == controllogAdminID);
            var adminControlLog = controlLogAdminDetails.FirstOrDefault();  //adminControlLog.Get(projectID, controllogAdminID); //Get all control log admin details for checks


            string popupCaptionText = "Control Log Signature";

            if (adminControlLog.Milestone) //If not a major milestone then it cannot be signed off manually.
            {
                if (!live)
                {
                    if (signed) //Remove signature
                    {
                        var nextSignatureSigned = await CheckNextControlLogSigned(projectID, etestPackageID, adminControlLog.SignatureNo);

                        if (nextSignatureSigned)
                        {
                            if (showPopups)
                                await Application.Current.MainPage.DisplayAlert(popupCaptionText, "Prior signatures are signed, these must be removed before removing this signature", "ok");

                            return false;
                        }

                    }
                    else //Add Signature
                    {
                        if (adminControlLog.SignatureNo > 1)
                        {
                            Boolean previousSignatureSigned = await CheckPreviousControlLogSigned(projectID, etestPackageID, adminControlLog.SignatureNo - 1);


                            if (!previousSignatureSigned) //if previous signature is not signed then cannot sign.
                            {
                                if (showPopups)
                                    await Application.Current.MainPage.DisplayAlert(popupCaptionText, "Previous signatures are not signed", "ok");
                                return false;
                            }
                        }
                    } //End of first test

                    //Second test

                    // Allow Test pack coordinator the ability to remove any signatures.
                    if (signed && CheckIsTestPackCoordinator(CurrentUser, subContractor))
                        return true;
                    else
                    {
                        //Check user details against control log signature details.
                        if (!naAutoSignOff)
                        {
                            if (!ControlLogRightsCheck(adminControlLog, CurrentUser, subContractor))
                            {
                               
                                    if (showPopups)
                                    {
                                        var ansr = await Application.Current.MainPage.DisplayActionSheet("Sorry, you do not have the rights to adjust this signature", "Sign by Other", "OK");
                                        if (ansr == "Sign by Other")
                                        {
                                            var vm = await ReadLoginPopup();
                                            if (vm.Password != null && vm.UserName != null)
                                            {
                                                var UserDetailsList = await _userDetailsRepository.GetAsync(x => x.UserName == vm.UserName && x.Password == vm.Password);
                                                if (UserDetailsList.Any())
                                                {
                                                    CurrentUser = UserDetailsList.FirstOrDefault();
                                                    return true;
                                                }
                                                else
                                                {
                                                    await Application.Current.MainPage.DisplayAlert("Login", AppConstant.LOGIN_FAILURE, "OK");
                                                    return false;
                                                }


                                            }
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    
                                }
                                //    await Application.Current.MainPage.DisplayAlert(popupCaptionText, "Sorry, you do not have the rights to adjust this signature", "ok");
                                //return false;
                            }
                        }

                    }

                    //Documents check.
                    if (!await ControlLogDocumentsCheck(projectID, etestPackageID, adminControlLog, signed))
                    {

                        var folderNames = await _AdminControlLogFolderRepository.QueryAsync<T_AdminFolders>("SELECT DISTINCT AF.[FolderName] FROM [AdminControlLogFolderHH] CLF INNER JOIN [AdminFolderHH] AF" +
                                                                          " ON CLF.[ProjectID] = AF.[ProjectID] AND CLF.[FolderAdminID] = AF.[ID] " +
                                                                          "WHERE CLF.[ProjectID] = '" + projectID + "' AND CLF.[ControlLogAdminID] = '" + adminControlLog.ID + "'");

                        if (showPopups)

                            await Application.Current.MainPage.DisplayAlert(popupCaptionText, "This signature requires " + string.Join(",", folderNames.FirstOrDefault().FolderName) + " documents to be uploaded prior, this can only be uploaded on VMLive.", "ok");
                        return false;

                    }


                    //Punch checks
                    if (!await ControlLogPunchLayerCheck(projectID, etestPackageID, adminControlLog, signed))
                    {
                        if (showPopups)
                            await _userDialogs.AlertAsync("Unable to sign off as there are outstanding punches.", popupCaptionText);
                        return false;
                    }


                    //Never returned false so passed all tests       
                    return true;

                }
                else
                {
                    if (showPopups)
                        await Application.Current.MainPage.DisplayAlert(popupCaptionText, "Selected milestone has already been uploaded to VMLive and cannot be removed on the handheld.", "ok");

                }
            }
            else
            {
                if (showPopups)
                    await Application.Current.MainPage.DisplayAlert(popupCaptionText, "Minor milestones cannot be signed off manually, these are done automatically by various events setup by the E-Test Package admin.", "ok");
            }

            return false;
        }

        private async Task<Boolean> CanSignOffAcceptedBySignature(string adminDatabaseTable, string databaseTable, ObservableCollection<RecordConfirmation> gvContent, int adminID, Boolean signed, Boolean live, int orderNo)
        {
            Boolean previousSignaturesNotSigned = false;
            if (orderNo > 1)
            {
                string SQL = "SELECT *  FROM[" + adminDatabaseTable + "] AS T1 INNER JOIN(SELECT* FROM [" +
                     databaseTable + "] WHERE [ETestPackageID] = " + CurrentPageHelper.ETestPackage.ID +
                     " AND ([Signed] = 0 OR [Signed] IS NULL)) AS T2 ON T1.[ID] = T2.[AdminID] WHERE T1.[ProjectID] = " +
                    Settings.ProjectID + " AND T1.[OrderNo] =  '" + --orderNo + "'";

                var result = await _AdminPreTestRecordAcceptedByRepository.QueryAsync<T_AdminPreTestRecordAcceptedBy>(SQL); // 

                previousSignaturesNotSigned = result.Any();

            }

            if (previousSignaturesNotSigned)
            {
                await _userDialogs.AlertAsync("Previous accepted by signatures must be completed prior.", "Accepted By Signature", "OK");
                return false;
            }

            if (await CanSignOffCertificiationSignature(adminDatabaseTable, databaseTable, adminID, signed, live))
            {
                if (!signed)
                {
                    foreach (RecordConfirmation row in gvContent)
                    {

                        if (!row.Signed)
                        {
                            await _userDialogs.AlertAsync("Sorry, all content signatures must be signed before this signature is added.", "Accepted By Signature", "OK");
                            return false;
                        }
                    }
                }

                return true;
            }
            return false;
        }

        private async Task<Boolean> CanSignOffCertificiationSignature(string adminDatabaseTable, string databaseTable, int adminID, Boolean signed, Boolean live)
        {
            string companyCategoryCode = "", functionCode = "", description = "";

            string SQL = "SELECT * FROM [" + adminDatabaseTable + "] WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [ID] = '" + adminID + "'";
            var result = await _adminPreTestRecordContentRepository.QueryAsync<T_AdminPreTestRecordContent>(SQL);


            if (result.Any())
            {
                companyCategoryCode = result.FirstOrDefault().CompanyCategoryCode;
                functionCode = result.FirstOrDefault().FunctionCode;
                description = result.FirstOrDefault().Description;
            }

            if (UserDetails.Function_Code.ToUpper().Trim() != functionCode.ToUpper().Trim() || UserDetails.Company_Category_Code.ToUpper().Trim() != companyCategoryCode.ToUpper().Trim())
            {
                var ansr = await Application.Current.MainPage.DisplayActionSheet("Sorry, you do not have the rights to adjust this signature", "Sign by Other", "OK");
                if (ansr == "Sign by Other")
                {
                    var vm = await ReadLoginPopup();
                    if (vm.Password != null && vm.UserName != null)
                    {
                        var UserDetailsList = await _userDetailsRepository.GetAsync(x => x.UserName == vm.UserName && x.Password == vm.Password);
                        if (UserDetailsList.Any())
                        {
                            UserDetails = UserDetailsList.FirstOrDefault();
                            return true;
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Login", AppConstant.LOGIN_FAILURE, "OK");
                            return false;
                        }


                    }
                }
                else
                {
                    return false;
                }

                //await _userDialogs.AlertAsync("Sorry, You Do Not Have The Correct User Right To Sign Off This Signature", description, "Ok");
                //return false;
            }

            if (companyCategoryCode == "S")
            {
                string subContractor = CurrentPageHelper.ETestPackage.SubContractor;

                if (UserDetails.Company_Code.ToUpper().Trim() != subContractor.ToUpper().Trim())
                {
                    var ansr = await Application.Current.MainPage.DisplayActionSheet("Sorry, you do not have the rights to adjust this signature", "Sign by Other", "OK");
                    if (ansr == "Sign by Other")
                    {
                        var vm = await ReadLoginPopup();
                        if (vm.Password != null && vm.UserName != null)
                        {
                            var UserDetailsList = await _userDetailsRepository.GetAsync(x => x.UserName == vm.UserName && x.Password == vm.Password);
                            if (UserDetailsList.Any())
                            {
                                UserDetails = UserDetailsList.FirstOrDefault();
                                return true;
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Login", AppConstant.LOGIN_FAILURE, "OK");
                                return false;
                            }


                        }
                    }
                    else
                    {
                        return false;
                    }
                    //await _userDialogs.AlertAsync("Sorry, You Do Not Have The Correct User Right To Sign Off This Signature", description, "OK");
                    //return false;
                }
            }

            if (live)
            {
                await _userDialogs.AlertAsync("Sorry, this signature has already been uploaded to VMLive and cannot be " + (signed ? "signed" : "removed") + " on the handheld.", description, "OK");
                return false;
            }

            return true;
        }
        public async Task<Boolean> SaveNew(int projectID, int etestPackageID, string databaseTable, T_PreTestRecordContent signature)
        {
            if (databaseTable == "T_PreTestRecordContent")
            {
                string SQL = "SELECT *  FROM [" + databaseTable + "] WHERE [ProjectID] = '" + projectID + "' AND [ETestPackageID] = '" + etestPackageID + "' AND [AdminID] = '" + signature.AdminID + "'";
                var sqlreturn = await _PreTestRecordContentRepository.QueryAsync(SQL);

                if (sqlreturn.Any())
                {
                    Boolean returnvalue = true;
                    try
                    {
                        //Found use sql update.
                        string sql = "UPDATE [" + databaseTable + "] SET [Signed] = '" + Convert.ToInt32(signature.Signed) + "' ,[SignedByUserID] = '" + signature.SignedByUserID + "',[SignedBy] = '" + signature.SignedBy
                           + "',[SignedOn] = '" + signature.SignedOn.Ticks + "', [Live] = '" + signature.Live + "', [Updated] = 1 WHERE [ProjectID] = '"
                           + Settings.ProjectID + "' AND [ETestPackageID] = '" + CurrentPageHelper.ETestPackage.ID + "' AND [AdminID] = '" + signature.AdminID + "'";

                        var data = _PreTestRecordContentRepository.QueryAsync(sql);
                        return returnvalue;
                    }
                    catch (Exception Ex)
                    {
                        returnvalue = false;
                        return returnvalue;
                    }
                }

                else
                {
                    try
                    {
                        ////Insert fresh details.
                        //string sql = "INSERT INTO [" + databaseTable + "] ([ProjectID],[ETestPackageID],[AdminID],[Signed],[SignedByUserID],[SignedBy],[SignedOn],[Live],[Updated]) VALUES " +
                        //                                           "('" + Settings.ProjectID + "','" + CurrentPageHelper.ETestPackage.ID + "','" + signature.AdminID + "','" + signature.Signed + "','"
                        //                                           + signature.SignedByUserID + "','" + signature.SignedBy + "','" + signature.SignedOn.Ticks + "',0,1)";

                        //Insert fresh details.
                        string sql = "INSERT INTO [" + databaseTable + "] ([ProjectID],[ETestPackageID],[AdminID],[Signed],[SignedByUserID],[SignedBy],[SignedOn],[Live],[Updated]) VALUES " +
                                                                   "('" + Settings.ProjectID + "','" + CurrentPageHelper.ETestPackage.ID + "','" + signature.AdminID + "','" + Convert.ToInt32(signature.Signed) + "','"
                                                                   + signature.SignedByUserID + "','" + signature.SignedBy + "','" + signature.SignedOn.Ticks + "',0,1)";
                        var data = _PreTestRecordContentRepository.QueryAsync(sql);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            else
            {

                string SQL = "SELECT *  FROM [" + databaseTable + "] WHERE [ProjectID] = '" + projectID + "' AND [ETestPackageID] = '" + etestPackageID + "' AND [AdminID] = '" + signature.AdminID + "'";
                var sqlreturn = await _PreTestRecordAcceptedByRepository.QueryAsync(SQL);

                if (sqlreturn.Any())
                {
                    Boolean returnvalue = true;
                    try
                    {
                        //Found use sql update.
                        //string sql = "UPDATE [" + databaseTable + "] SET [Signed] = '" + signature.Signed + "',[SignedByUserID] = '" + signature.SignedByUserID + "',[SignedBy] = '" + signature.SignedBy
                        //    + "',[SignedOn] = '" + signature.SignedOn.Ticks + "', [Live] = '" + signature.Live + "', [Updated] = '" + signature.Updated + "' WHERE [ProjectID] = '"
                        //    + Settings.ProjectID + "' AND [ETestPackageID] = '" + CurrentPageHelper.ETestPackage.ID + "' AND [AdminID] = '" + signature.AdminID + "'";

                        string sql = "UPDATE [" + databaseTable + "] SET [Signed] = '" + Convert.ToInt32(signature.Signed) + "' ,[SignedByUserID] = '" + signature.SignedByUserID + "',[SignedBy] = '" + signature.SignedBy
                          + "',[SignedOn] = '" + signature.SignedOn.Ticks + "', [Live] = '" + signature.Live + "', [Updated] = 1 WHERE [ProjectID] = '"
                          + Settings.ProjectID + "' AND [ETestPackageID] = '" + CurrentPageHelper.ETestPackage.ID + "' AND [AdminID] = '" + signature.AdminID + "'";
                        var data = _PreTestRecordAcceptedByRepository.QueryAsync(sql);
                        return returnvalue;
                    }
                    catch (Exception Ex)
                    {
                        returnvalue = false;
                        return returnvalue;
                    }
                }


                else
                {
                    try
                    {
                        //Insert fresh details.
                        //string sql = "INSERT INTO [" + databaseTable + "] ([ProjectID],[ETestPackageID],[AdminID],[Signed],[SignedByUserID],[SignedBy],[SignedOn],[Live],[Updated]) VALUES " +
                        //                                           "('" + Settings.ProjectID + "','" + CurrentPageHelper.ETestPackage.ID + "','" + signature.AdminID + "','" + signature.Signed + "','"
                        //                                           + signature.SignedByUserID + "','" + signature.SignedBy + "','" + signature.SignedOn.Ticks + "',0,1)";
                        string sql = "INSERT INTO [" + databaseTable + "] ([ProjectID],[ETestPackageID],[AdminID],[Signed],[SignedByUserID],[SignedBy],[SignedOn],[Live],[Updated]) VALUES " +
                                                                "('" + Settings.ProjectID + "','" + CurrentPageHelper.ETestPackage.ID + "','" + signature.AdminID + "','" + Convert.ToInt32(signature.Signed) + "','"
                                                                + signature.SignedByUserID + "','" + signature.SignedBy + "','" + signature.SignedOn.Ticks + "',0,1)";
                       // var data = _PreTestRecordContentRepository.QueryAsync(sql);

                        var data = _PreTestRecordAcceptedByRepository.QueryAsync(sql);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }

                }


            }


        }
        public async Task<bool> CheckNextControlLogSigned(int projectID, int etestPackageID, int signatureNo)
        {

            var LocalSQLFunctions = await _AdminControlLog.QueryAsync<T_ControlLogSignature>("SELECT * FROM [T_AdminControlLog] AS ADMIN INNER JOIN [T_ControlLogSignature] CL ON ADMIN.[ProjectID] = CL.[ProjectID] AND ADMIN.[ID] = CL.[ControlLogAdminID] " +
                                                            "WHERE ADMIN.[ProjectID] = '" + projectID + "' AND CL.[ETestPackageID] = '" + etestPackageID + "' AND ADMIN.[SignatureNo] > '" + signatureNo + "' AND [MileStone] = 1");


            if (!LocalSQLFunctions.Any()) return false;

            var Singed = LocalSQLFunctions.FirstOrDefault();
            if (Singed != null)
            {
                return Singed.Signed;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> CheckPreviousControlLogSigned(int projectID, int etestPackageID, int signatureNo)
        {

            var LocalSQLFunctions = await _AdminControlLog.QueryAsync<T_ControlLogSignature>("SELECT * FROM [T_AdminControlLog] AS ADMIN INNER JOIN [T_ControlLogSignature] CL ON ADMIN.[ProjectID] = CL.[ProjectID] AND ADMIN.[ID] = CL.[ControlLogAdminID] " +
                                                            "WHERE ADMIN.[ProjectID] = '" + projectID + "' AND CL.[ETestPackageID] = '" + etestPackageID + "' AND ADMIN.[SignatureNo] < '" + signatureNo + "' ORDER BY ADMIN.[SignatureNo] DESC");

            var Singed = LocalSQLFunctions.FirstOrDefault();
            if (Singed != null)
            {
                return Singed.Signed;
            }
            else
            {
                return false;
            }

        }

        public static Boolean CheckIsTestPackCoordinator(T_UserDetails user, string testpackageSubContractor)
        {
            if (user.ETP_SuperUser)
                return true;

            if (user.Company_Code.ToUpper() != testpackageSubContractor.ToUpper())
                return false;

            if (user.Function_Code != "32") //32 is for test pack co-ordinator
                return false;

            return true;
        }

        public static Boolean ControlLogRightsCheck(T_AdminControlLog adminControlLog, T_UserDetails user, string testpackageSubContractor)
        {
            if (user.Function_Code.ToUpper() != adminControlLog.FunctionCode.ToUpper())
                return false;

            if (user.Company_Category_Code.ToUpper() != adminControlLog.CompanyCategoryCode.ToUpper())
                return false;

            if (user.Section_Code.ToUpper() != adminControlLog.SectionCode.ToUpper())
                return false;

            if (user.Company_Category_Code.ToUpper() == "S" && user.Company_Code.ToUpper() != testpackageSubContractor.ToUpper())
                return false;


            return true;
        }
        public async Task<Boolean> ControlLogDocumentsCheck(int projectID, int etestPackageID, T_AdminControlLog adminControlLog, Boolean signed)
        {
            if (signed) //Removing signature so check not required.
                return true;

            //Get document list
            var folders = await _AdminControlLogFolderRepository.GetAsync(x => x.ControlLogAdminID == adminControlLog.ID && x.ProjectID == projectID);


            if (!folders.Any())
                return true;

            foreach (T_AdminControlLogFolder folder in folders)
            {
                var found = await _attachedDocumentRepository.GetAsync(x => x.ProjectID == projectID && x.ETestPackageID == etestPackageID && x.FolderID == folder.FolderAdminID);

                if (!found.Any())
                    return false;
            }

            return true;
        }

        public async Task<Boolean> ControlLogPunchLayerCheck(int projectID, int etestPackageID, T_AdminControlLog adminControlLog, bool signed)
        {
            if (signed)  //Removing signature so check not required.
                return true;

            if (adminControlLog.PunchesCompleted || adminControlLog.PunchesConfirmed)
            {
                //Setup SQL Param List
                //List<OleDbParameter> punchParams = new List<OleDbParameter>()
                //{
                //        new OleDbParameter("@ETestPackageID", etestPackageID)
                //};

                //Get all values needed
                //using (OleDbConnection CurrentConnection = new OleDbConnection(Constants.LocalConnectionString))
                //{
                // CurrentConnection.Open();

                string punchLayersInStatment = "";
                string punchCategoryInStatment = "";
                string punchActionPartyInStatment = "";
                string SQL = "SELECT * FROM [T_AdminControlLogPunchLayer] WHERE [ProjectID] = '" + projectID + "' AND [ControlLogAdminID] = '" + adminControlLog.ID + "'";

                var data = await _AdminControlLogPunchLayerRepository.QueryAsync<T_AdminControlLogPunchLayer>(SQL);

                foreach (T_AdminControlLogPunchLayer CP in data)
                {
                    int punchAdminID = CP.PunchAdminId;
                    punchLayersInStatment += (punchLayersInStatment != string.Empty ? "," : "") + punchAdminID.ToString();
                }

                //Get Categories
                string SQL1 = "SELECT * FROM [T_AdminControlLogPunchCategory] WHERE [ProjectID] ='" + projectID + "' AND [ControlLogAdminID] = '" + adminControlLog.ID + "'";
                var data1 = await _AdminControlLogPunchCategoryRepository.QueryAsync<T_AdminControlLogPunchCategory>(SQL1);

                foreach (T_AdminControlLogPunchCategory PC in data1)
                {
                    string category = PC.Category;
                    punchCategoryInStatment += ((punchCategoryInStatment != string.Empty ? "," : "") + "'" + category + "'");
                }

                bool allFunctionCodes = false;
                //Get Action Parties
                string SQL2 = "SELECT * FROM [T_AdminControlLogActionParty] WHERE [ProjectID] = '" + projectID + "' AND [ControlLogAdminID] = '" + adminControlLog.ID + "'";
                var data2 = await _AdminControlLogActionPartyRepository.QueryAsync<T_AdminControlLogActionParty>(SQL2);

                foreach (T_AdminControlLogActionParty cp in data2)
                {
                    string actionParty = cp.FunctionCode;
                    if (actionParty.ToUpper() == "ALL")
                    {
                        allFunctionCodes = true;
                        break;
                    }
                    punchActionPartyInStatment += (punchActionPartyInStatment != string.Empty ? "," : "") + "'" + actionParty + "'";
                }


                //Build up the punch check SQL based of values
                string punchSQL =
                    "SELECT [ID] FROM [T_PunchList] WHERE [ETestPackageID] = " + etestPackageID + " " +
                    "AND ([Cancelled] = FALSE OR [Cancelled] IS NULL) " +
                    "AND [Category] IN (" + punchCategoryInStatment + ") " +
                    "AND [PunchAdminID] IN (" + punchLayersInStatment + ") " +
                    (!allFunctionCodes ? "AND [FunctionCode] IN (" + punchActionPartyInStatment + ") " : "") +
                    (adminControlLog.PunchesCompleted ? "AND ([WorkCompleted] = FALSE OR [WorkCompleted] IS NULL) " : "") +
                    (adminControlLog.PunchesConfirmed ? "AND ([WorkConfirmed] = FALSE OR [WorkConfirmed] IS NULL) " : "");


                var data3 = await _PunchListRepository.QueryAsync<T_PunchList>(punchSQL);


                if (data3 != null && data3.Any())
                    return false;
            }
            // }
            return true;
        }
        public static Task<LoginModel> ReadLoginPopup()
        {
            var vm = new SignOffPopupViewModel();
            var tcs = new TaskCompletionSource<LoginModel>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var page = new SignOffPopup(vm);
                await PopupNavigation.PushAsync(page);
                var value = await vm.GetValue();
                await PopupNavigation.PopAsync(true);
                tcs.SetResult(value);
            });
            return tcs.Task;
        }
        #endregion
    }
}

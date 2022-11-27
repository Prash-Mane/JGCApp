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
using System;
using JGC.Models.E_Test_Package;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using System.Windows.Input;
using JGC.ViewModels.E_Reporter;
using JGC.Views.E_Reporter;
using Rg.Plugins.Popup.Services;

namespace JGC.ViewModels.E_Test_Package
{


    public class ControlLogViewModel : BaseViewModel
    {

        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private readonly IRepository<T_AdminControlLog> _AdminControlLog;
        private readonly IRepository<T_ControlLogSignature> _controlLogSignature;
        private readonly IRepository<T_AdminControlLogFolder> _AdminControlLogFolderRepository;
        private readonly IRepository<T_AttachedDocument> _attachedDocumentRepository;
        private readonly IRepository<T_ETestPackages> _eTestPackages;
        private readonly IRepository<T_AdminControlLogPunchLayer> _AdminControlLogPunchLayerRepository;
        private readonly IRepository<T_AdminControlLogPunchCategory> _AdminControlLogPunchCategoryRepository;
        private readonly IRepository<T_AdminControlLogActionParty> _AdminControlLogActionPartyRepository;
        private readonly IRepository<T_PunchList> _PunchListRepository;
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        private string lblRejectDetails;
        public string LblRejectDetails
        {
            get { return lblRejectDetails; }
            set { lblRejectDetails = value; RaisePropertyChanged(); }
        }

        private T_ETestPackages ETPSelected;
        public T_ETestPackages SelectedETP
        {
            get { return ETPSelected; }
            set { ETPSelected = value; RaisePropertyChanged(); }
        }
        private ShowHideMinor btnShowHideMinor;
        public ShowHideMinor BtnShowHideMinor
        {
            get { return btnShowHideMinor; }
            set { btnShowHideMinor = value; RaisePropertyChanged(); }
        }


        

        private ObservableCollection<ControlLogModel> controlLogItemSource;
        public ObservableCollection<ControlLogModel> ControlLogItemSource
        {
            get { return controlLogItemSource; }
            set { controlLogItemSource = value; RaisePropertyChanged(); }
        }

        private T_UserProject selectedProject;
        public T_UserProject SelectedProject
        {
            get { return selectedProject; }
            set
            {
                if (SetProperty(ref selectedProject, value))
                {
                    NavigateToEReportSelectionPage(selectedProject);
                }
            }
        }
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
        #endregion
        public ControlLogViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_UserProject> _userProjectRepository,
            IRepository<T_AdminControlLog> AdminControlLog,
            IRepository<T_UserDetails> userDetailsRepository,
            IRepository<T_ControlLogSignature> controlLogSignature,
            IRepository<T_AdminControlLogFolder> AdminControlLogFolderRepository,
            IRepository<T_ETestPackages> eTestPackages,
              IRepository<T_AdminControlLogPunchLayer> _AdminControlLogPunchLayerRepository,
         IRepository<T_AdminControlLogPunchCategory> _AdminControlLogPunchCategoryRepository,
           IRepository<T_AdminControlLogActionParty> _AdminControlLogActionPartyRepository,
        IRepository<T_PunchList> _PunchListRepository,
        IRepository<T_AttachedDocument> attachedDocumentRepository) : base(_navigationService, _httpHelper, _checkValidLogin)


        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._userProjectRepository = _userProjectRepository;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._AdminControlLog = AdminControlLog;
            this._userDetailsRepository = userDetailsRepository;
            this._controlLogSignature = controlLogSignature;
            this._AdminControlLogFolderRepository = AdminControlLogFolderRepository;
            this._attachedDocumentRepository = attachedDocumentRepository;
            this._eTestPackages = eTestPackages;
            this._AdminControlLogPunchLayerRepository = _AdminControlLogPunchLayerRepository;
            this._AdminControlLogPunchCategoryRepository = _AdminControlLogPunchCategoryRepository;
            this._AdminControlLogActionPartyRepository = _AdminControlLogActionPartyRepository;
            this._PunchListRepository = _PunchListRepository;
            _userDialogs.HideLoading();
            PageHeaderText = "Control Log";
            IsHeaderBtnVisible = true;
        }



        #region Delegate Commands  
        public ICommand BtnCommand
        {
            get
            {
                return new Command<string>(OnClickButton);
            }
        }
        #endregion



        #region Private

        private async void OnClickButton(string param)
        {

            if (param == "ShowHideMinor")
            {
                btnShowHideMinor_Clicked(param);
               _userDialogs.HideLoading();
            }
            //else 
            //{
            //  var pr = param;
            //  var answer =  await Application.Current.MainPage.DisplayAlert("Control Log Signature", "Are you sure you want to sign this signature?", "Yes", "No");
            //}
        }
            protected async Task LoadControlLogTabAsync()
            {
               // var selectedEtp = 
            var cureentProj = await _userProjectRepository.GetAsync(x => x.Project_ID == CurrentPageHelper.ETestPackage.ProjectID);
            var CurrentProject = cureentProj.FirstOrDefault();
            var gvControlLog = GenerateControlLogTableAsync(CurrentProject.Project_ID, CurrentPageHelper.ETestPackage.ID, BtnShowHideMinor.Checked, CurrentProject);
            // gvControlLog.ClearSelection();

            //Get Rejection Details
            string sql =
                "SELECT TA.[SignatureName],TC.[RejectComment], TC.[RejectedOn], TC.[RejectedBy] " +
                "FROM [T_AdminControlLog] AS TA " +
                "LEFT JOIN [T_ControlLogSignature] TC ON TA.[ID] = TC.[ControlLogAdminID] AND TA.[ProjectID] = TC.[ProjectID] " +
                "WHERE TC.[ETestPackageID] = '" + CurrentPageHelper.ETestPackage.ID + "' AND TC.[Reject] = '" + 1 + "' " + //not sure why the etestpackageid has to put in like this instead of a oleparameter??
                "AND TA.[ProjectID] ='" + CurrentProject.Project_ID + "'" +
                "ORDER BY TA.[SignatureNo] DESC";
           

            try
            {
                var data = await _AdminControlLog.QueryAsync<ControlLogModel>(sql);
                var rejectData = data.FirstOrDefault();
                if (rejectData != null)
                {
                    string signatureName = rejectData.SignatureName;
                    string rejectComment = rejectData.RejectComment;
                    string rejectedBy = rejectData.RejectedBy;
                    string rejectedOn = rejectData.RejectedOn.ToString();

                    LblRejectDetails = signatureName + ": Rejected By " + rejectedBy + " On " + rejectedOn + "\n" + rejectComment;
                }
                else
                {
                    LblRejectDetails = "";
                }
            }
            catch (Exception E)
            {

            }
        }
        


        private async Task GenerateControlLogTableAsync(int projectID, int etestPackageID, int showMinorMilestones, T_UserProject userProject)
        {
           
            //string SQL = "SELECT DISTINCT ADMIN.[ID],ADMIN.[SignatureName],ADMIN.[Milestone],ADMIN.[SignatureNo],ADMIN.[DisciplineDisplay],CL.[Live],CL.[Signed],CL.[SignedBy],CL.[SignedOn],CL.[Reject] FROM [T_AdminControlLog] AS ADMIN " +
            //    "LEFT OUTER JOIN (SELECT * FROM [T_ControlLogSignature] WHERE [ETestPackageID] = '" + etestPackageID + "') AS CL ON CL.ProjectID = ADMIN.ProjectID AND CL.ControlLogAdminID = ADMIN.ID " +
            //    "WHERE ADMIN.[ProjectID] = @ProjectID AND (ADMIN.[Milestone] = TRUE OR ADMIN.[Milestone] = @Milestone) ORDER BY ADMIN.[SignatureNo] ASC";

            string SQL = "SELECT DISTINCT ADMIN.[ID],ADMIN.[SignatureName],ADMIN.[Milestone],ADMIN.[SignatureNo],ADMIN.[DisciplineDisplay],CL.[Live],CL.[Signed],CL.[SignedBy],CL.[SignedOn],CL.[Reject] FROM [T_AdminControlLog] AS ADMIN " +
           "LEFT OUTER JOIN (SELECT * FROM [T_ControlLogSignature] WHERE [ETestPackageID] = " + etestPackageID + ") AS CL ON CL.ProjectID = ADMIN.ProjectID AND CL.ControlLogAdminID = ADMIN.ID " +
           "WHERE ADMIN.[ProjectID] = '" + projectID + "' AND ADMIN.[ProjectName] = '" + userProject.ProjectName + "' AND (ADMIN.[Milestone] = 1 OR ADMIN.[Milestone] = '" + showMinorMilestones + "') ORDER BY ADMIN.[SignatureNo] ASC";

            try
            {
                int srNo = 0;
                List<ControlLogModel> CLList = new List<ControlLogModel>();
                var data = await _AdminControlLog.QueryAsync<ControlLogModel>(SQL);
                var singedLogs = data.Where(x => x.Signed == true);
                foreach (ControlLogModel CL in data)
                {
                    srNo++;
                    CL.SrNo = srNo;
                    CL.SignedImage = CL.Signed ? "Greenradio.png":(CL.Reject ? "reddot.png" : "Grayradio.png");
                    CLList.Add(CL);
                }

                  ControlLogItemSource = new ObservableCollection<ControlLogModel>(CLList);
                
            }
            catch (Exception err)
            {
                var errr = (err.Message);
            }
          
        }

        public async void ControlLog_SignClick(ControlLogModel controlLogModel)
        {
            if (controlLogModel != null) //Status Image
            {
                //int rowIndex = e.RowIndex;
                int controllogAdminID = controlLogModel.ID;    //  Convert.ToInt32(gvControlLog.Rows[rowIndex].Cells[0].Value.ToString());
                int signatureNo = controlLogModel.SignatureNo; // Convert.ToInt32(gvControlLog.Rows[rowIndex].Cells[4].Value.ToString()); //Get signature no for selected signature.
                Boolean signed = controlLogModel.Signed; //gvControlLog.Rows[rowIndex].Cells[2].Value.ToString().ToUpper() == "TRUE";
                                                         //string popupCaptionText = "Control Log Signature";

                var answer = await Application.Current.MainPage.DisplayAlert("Control Log Signature", "Are you sure you want to sign this signature?", "Yes", "No");

                if (answer)
                {
                    var result = await _userDetailsRepository.GetAsync(x => x.ID == Settings.UserID);
                    var CurrentUser = result.FirstOrDefault();
                  if (await CanSignControlLogSignature(CurrentUser, Convert.ToInt32(Settings.ProjectID), CurrentPageHelper.ETestPackage.ID, CurrentPageHelper.ETestPackage.SubContractor, controllogAdminID))
                    {
                       T_ControlLogSignature controlLogSignature = new T_ControlLogSignature()
                       {
                           ControlLogAdminID = controllogAdminID,
                           Signed = !signed,
                           SignedByUserID = CurrentUser.ID,
                           SignedBy = !signed ? CurrentUser.FullName : "",
                           SignedOn = DateTime.UtcNow,
                       };

                        if (await SaveNewAsync(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, controlLogSignature))
                        {
                            //Adjust minor milestonesz
                            List<T_AdminControlLog> list = await GetMinorMileStones(Settings.ProjectID, signatureNo); //get list of minor milestones associated

                            if (list != null && list.Count > 0) //if returned some minor milestones 
                            {
                                foreach (T_AdminControlLog adminControlLog in list) //go through list and adjust the signature's control log id and save.
                                {
                                    controlLogSignature.ControlLogAdminID = adminControlLog.ID;
                                   await SaveNewAsync(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, controlLogSignature);
                                }
                            }

                            AutoSignControlLogSignatures(CurrentUser, Settings.ProjectID, CurrentPageHelper.ETestPackage.ID, CurrentPageHelper.ETestPackage.SubContractor);
                            UpdateETestPackageStatus(Settings.ProjectID, CurrentPageHelper.ETestPackage.ID);

                            await LoadControlLogTabAsync();
                        }
                        else
                           await Application.Current.MainPage.DisplayAlert("Control Log Signature","Error ocurred saving signature", "OK");

                    }
                }
            }
        }


        public async void UpdateETestPackageStatus(int projectID, int etestpackageID)
        {
            string SQL = "SELECT * FROM [T_AdminControlLog] AS ADMIN INNER JOIN [T_ControlLogSignature] CL ON ADMIN.[ProjectID] = CL.[ProjectID] AND ADMIN.[ID] = CL.[ControlLogAdminID] " +
                                                             "WHERE ADMIN.[ProjectID] = '"+ projectID + "' AND CL.[ETestPackageID] = '"+ etestpackageID + "' AND (CL.[Signed] = 1) ORDER BY ADMIN.[SignatureNo] DESC";

            var result = await _AdminControlLog.QueryAsync<T_AdminControlLog>(SQL);

            int signatureNo = 0;
               if(result.Any()) signatureNo = result.FirstOrDefault().SignatureNo;

            var result1 = await _AdminControlLog.QueryAsync<T_AdminControlLog>("SELECT * FROM[T_AdminControlLog] WHERE[ProjectID] ='" + projectID + "' AND[SignatureNo] > '"+ signatureNo + "' ORDER BY[SignatureNo] ASC");

            string status = string.Empty;
            if (result1.Any()) status = result1.FirstOrDefault().SignatureName;

            status = status == string.Empty ? "Completed" : status;


            var update = await _eTestPackages.QueryAsync("UPDATE[T_ETestPackages] SET[Updated] = 1, [Status] = '"+ status + "' WHERE[ProjectID] = '"+ projectID + "' AND[ID] = '"+ etestpackageID + "'");
          
          //  MarkETestPackageAsUpdated(projectID, etestpackageID);
        }

        public  async Task<List<T_AdminControlLog>> GetMinorMileStones(int projectID, int signatureNo)
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

                string SQL = " UPDATE [T_ControlLogSignature] SET [Reject] = '" + Convert.ToInt32(controlLogModel.Reject) + "', [RejectedBy] = '" + controlLogModel.RejectedBy
                  + "', [RejectedByUserID] = '" + controlLogModel.RejectedByUserID + "', [RejectedOn] = '" + controlLogModel.RejectedOn.Ticks
                  + "', [Signed] = '" + Convert.ToInt32(controlLogModel.Signed) + "',[SignedByUserID] = '"
                  + controlLogModel.SignedByUserID + "' , [SignedBy] = '" + controlLogModel.SignedBy + "',[SignedOn] = '" + controlLogModel.SignedOn.Ticks + "', [Updated] = 1"
                  + " WHERE [ProjectID] = '" + Settings.ProjectID + "' AND[ETestPackageID] = '" + etestPackageID
                  + "' AND[ControlLogAdminID] = '" + controlLogModel.ControlLogAdminID + "'";
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
                if (CurrentPageHelper.CurrentSessionSignature == null) CurrentPageHelper.CurrentSessionSignature = new List<T_ControlLogSignature>();
                CurrentPageHelper.CurrentSessionSignature.Add(ControlLogSignature);
                var dBreturn = await _controlLogSignature.InsertAsync(ControlLogSignature);
                if (dBreturn == 1)
                    return true;
                else
                    return false;
            }
        }

        public async  Task<Boolean> CanSignControlLogSignature(T_UserDetails CurrentUser, int projectID, int etestPackageID, string subContractor, int controllogAdminID, bool showPopups = true, bool naAutoSignOff = true)
        {
            Boolean live = false, signed = false;

            var getdBdata = await _controlLogSignature.GetAsync(x=>x.ProjectID == projectID && x.ETestPackageID == etestPackageID && x.ControlLogAdminID == controllogAdminID);
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
                        if (CurrentPageHelper.CurrentSessionSignature == null) 
                        {
                            await Application.Current.MainPage.DisplayAlert(popupCaptionText, "Selected milestone has already been uploaded to VMLIve and cannot be removed on the Handheld", "ok");
                            return false;
                        }
                        var _Signature = getdBdata.FirstOrDefault();
                        var IsAvailableInCurrentSesssion = CurrentPageHelper.CurrentSessionSignature.Where(x => 
                        x.ControlLogAdminID == _Signature.ControlLogAdminID
                        && x.ETestPackageID == _Signature.ETestPackageID
                        && x.ProjectID == _Signature.ProjectID
                        && x.SignedByUserID == _Signature.SignedByUserID
                       
                        );
                        if (ControlLogRightsCheck(adminControlLog, CurrentUser, subContractor) && IsAvailableInCurrentSesssion.Any())
                        {
                            return true;
                        }
                        else 
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
                            //await Application.Current.MainPage.DisplayAlert(popupCaptionText, "Sorry, you do not have the rights to adjust this signature", "ok");
                            //return false;
                        }
                        ////code commented because of new condition implimentaion
                        //var nextSignatureSigned = await CheckNextControlLogSigned(projectID, etestPackageID, adminControlLog.SignatureNo);

                        //if (nextSignatureSigned)
                        //{
                        //    if (showPopups)
                        //        await Application.Current.MainPage.DisplayAlert(popupCaptionText, "Prior signatures are signed, these must be removed before removing this signature", "ok");
                        
                        //    return false;
                        //}
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
                    //if (signed && CheckIsTestPackCoordinator(CurrentUser, subContractor))
                    //    return true;
                    //else
                    //{
                        //Check user details against control log signature details.
                        //if (!naAutoSignOff)
                        //{
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
                        //await Application.Current.MainPage.DisplayAlert(popupCaptionText, "Sorry, you do not have the rights to adjust this signature", "ok");
                        //        return false;
                            }
                       // }

                   // }
                    //Documents check.
                    if (!await ControlLogDocumentsCheck(projectID, etestPackageID, adminControlLog, signed))
                    {

                        var folderNames = await _AdminControlLogFolderRepository.QueryAsync<T_AdminFolders>("SELECT  * FROM [T_AdminControlLogFolder] CLF INNER JOIN [T_AdminFolders] AF" +
                                                                          " ON CLF.[ProjectID] = AF.[ProjectID] AND CLF.[FolderAdminID] = AF.[ID] " +
                                                                          "WHERE CLF.[ProjectID] = '"+projectID+"' AND CLF.[ControlLogAdminID] = '"+ adminControlLog.ID+ "'");
                       
                        if (showPopups)
                            
                        await Application.Current.MainPage.DisplayAlert(popupCaptionText, "This signature requires " + string.Join(",", folderNames.FirstOrDefault().FolderName) + " documents to be uploaded prior, this can only be uploaded on VMLive.", "ok");
                        return false;
                       
                    }


                    //Punch checks
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
        public async Task<bool> CheckNextControlLogSigned(int projectID, int etestPackageID, int signatureNo)
        {

            var LocalSQLFunctions = await _AdminControlLog.QueryAsync<T_ControlLogSignature>("SELECT * FROM [T_AdminControlLog] AS ADMIN INNER JOIN [T_ControlLogSignature] CL ON ADMIN.[ProjectID] = CL.[ProjectID] AND ADMIN.[ID] = CL.[ControlLogAdminID] " +
                                                            "WHERE ADMIN.[ProjectID] = '"+ projectID + "' AND CL.[ETestPackageID] = '"+ etestPackageID + "' AND ADMIN.[SignatureNo] > '"+ signatureNo + "' AND [MileStone] = 1");


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

            bool IsFunction_Code, IsCompany_Category_Code, IsSection_Code;
            IsFunction_Code = IsCompany_Category_Code = IsSection_Code = false;

            if (!String.IsNullOrEmpty(adminControlLog.FunctionCode))
                IsFunction_Code = true;
            if (!String.IsNullOrEmpty(adminControlLog.CompanyCategoryCode))
                IsCompany_Category_Code = true;
            if (!String.IsNullOrEmpty(adminControlLog.SectionCode))
                IsSection_Code = true;



            if (IsFunction_Code && user.Function_Code.ToUpper() != adminControlLog.FunctionCode.ToUpper())
                return false;

            if (IsCompany_Category_Code && user.Company_Category_Code.ToUpper() != adminControlLog.CompanyCategoryCode.ToUpper())
                return false;

            if (IsSection_Code && user.Section_Code.ToUpper() != adminControlLog.SectionCode.ToUpper())
                return false;

            if (user.Company_Category_Code.ToUpper() == "S" && user.Company_Code.ToUpper() != testpackageSubContractor.ToUpper())
                return false;
            return true;
        }



        private async void btnShowHideMinor_Clicked(string param)
        {
            
            ShowHideMinor _btnShowHideMinor = new ShowHideMinor();
            if (BtnShowHideMinor.Checked == 0)
                _btnShowHideMinor.Checked = 1;
            else
                _btnShowHideMinor.Checked = 0;

            _btnShowHideMinor.Text = _btnShowHideMinor.Checked == 0 ? "Hide Minor" : "Show Minor";
            BtnShowHideMinor = _btnShowHideMinor;

            await LoadControlLogTabAsync();
        }

        public async Task <Boolean> ControlLogDocumentsCheck(int projectID, int etestPackageID, T_AdminControlLog adminControlLog, Boolean signed)
        {
             
            if (signed) //Removing signature so check not required.
                return true;

            //Get document list
            if (adminControlLog.Folder == null || adminControlLog.Folder.Count() == 0)
                return true;

            //Get document list
            var folders = await _AdminControlLogFolderRepository.GetAsync(x => x.ControlLogAdminID == adminControlLog.ID && x.ProjectID == projectID);
          
            if (!folders.Any())
                return true;

            foreach (T_AdminControlLogFolder folder  in folders)
            {
                var found = await _attachedDocumentRepository.GetAsync(x => x.ProjectID == projectID && x.ETestPackageID == etestPackageID && x.FolderID == folder.FolderAdminID);
                   
                if (!found.Any())
                    return false;
            }

            return true;
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
              "AND CL.[Signed] = 1 AND CL.[SignedByUserID] = 0 AND NA.[ProjectID] = '" + projectID + "' AND NA.[AutoSignOffControlLogAdminID] = '" + nextControlLogAdminID+"'";


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
                       // AutoSignControlLogSignatures(CurrentUser, projectID, etestPackageID, subContractor);
                    }
                }
            }
        }
        public  async Task<int> GetNextControlLogIDAsync(int projectID, int etestPackageID)
        {
            var lastSigniture = await _AdminControlLog.QueryAsync<T_AdminControlLog>("SELECT * " +
                "FROM (T_AdminControlLog ADMIN INNER JOIN " +
                "T_ControlLogSignature CL ON ADMIN.ProjectID = CL.ProjectID AND ADMIN.ID = CL.ControlLogAdminID) " +
                "WHERE  (ADMIN.ProjectID = '"+ projectID + "') AND (CL.ETestPackageID = '"+ etestPackageID + "') AND (CL.Signed = '"+true+"') " +
                "ORDER BY ADMIN.[SignatureNo] DESC ");

            int lastSignatureNo = 0;

            if (!lastSigniture.Any()) return lastSignatureNo;
            else return lastSigniture.FirstOrDefault().SignatureNo;


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
                    "AND ([Cancelled] = 0 OR [Cancelled] IS NULL) " +
                    "AND [Category] IN (" + punchCategoryInStatment + ") " +
                    "AND [PunchAdminID] IN (" + punchLayersInStatment + ") " +
                    (!allFunctionCodes ? "AND [FunctionCode] IN (" + punchActionPartyInStatment + ") " : "") +
                    (adminControlLog.PunchesCompleted ? "AND ([WorkCompleted] = 0 OR [WorkCompleted] IS NULL) " : "") +
                    (adminControlLog.PunchesConfirmed ? "AND ([WorkConfirmed] = 0 OR [WorkConfirmed] IS NULL) " : "");


                var data3 = await _PunchListRepository.QueryAsync<T_PunchList>(punchSQL);


                if (data3 != null && data3.Any())
                    return false;
            }
            // }
            return true;
        }

        private async Task GetProjectListData()
        {
            var UserProjectList = await _userProjectRepository.GetAsync();
            if (UserProjectList.Count > 0)
                UserProjects = UserProjectList.Where(p => p.User_ID == Settings.UserID).ToList();

            _userDialogs.HideLoading();
        }
        private async void OnBackPressed()
        {
            CheckValidLogin._pageHelper = new PageHelper();
        }
        #endregion
        #region Public
        public async void NavigateToEReportSelectionPage(T_UserProject Project)
        {
            if (Project == null || IsRunningTasks)
            {
                return;
            }

            IsRunningTasks = true;
            Settings.ProjectID = Project.Project_ID;
            Settings.ModelName = Project.ModelName;
            Settings.JobCode = Project.JobCode;
            _userDialogs.ShowLoading("Loading...");
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add(NavigationParametersConstants.SelectedProjectParameter, Project);
            //navigationParameters.Add(NavigationParametersConstants.SelectedProjectParameter, Project.User_ID);
            navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
            await navigationService.NavigateAsync<EReportSelectionViewModel>(navigationParameters);
            IsRunningTasks = false;
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            // CheckValidLogin._pageHelper = new PageHelper();
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            //ETPSelected = CurrentPageHelper.ETestPackage;
            //LoadControlLogTabAsync();
            //if (parameters.Count == 0)
            //{
            //    return;
            //}

            if (CurrentPageHelper.ETestPackage != null)
            {
               // ETPSelected = CurrentPageHelper.ETestPackage;

                ShowHideMinor _btnShowHideMinor = new ShowHideMinor();
                _btnShowHideMinor.Checked = 1;
                _btnShowHideMinor.Text = "Show Minor";
                BtnShowHideMinor = _btnShowHideMinor;

                LoadControlLogTabAsync();
            }


        }
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
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

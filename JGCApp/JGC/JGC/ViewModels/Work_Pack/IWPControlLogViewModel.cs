using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables;
using JGC.DataBase.DataTables.WorkPack;
using JGC.Models;
using JGC.Models.Work_Pack;
using JGC.ViewModels.E_Reporter;
using JGC.Views.E_Reporter;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace JGC.ViewModels.Work_Pack
{
    public class IWPControlLogViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_UserProject> _userProjectRepository;
        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private readonly IRepository<T_IWP> _iwpRepository;
        private readonly IRepository<T_IWPAdminControlLog> _iwpAdminControlLogRepository;
        private readonly IRepository<T_IWPControlLogSignatures> _iwpControlLogSignaturesRepository;
        private readonly IRepository<T_IWPPunchCategory> _iwpPunchCategoryRepository;
        private readonly IRepository<T_IWPPunchLayer> _iwpPunchLayerRepository;
        private readonly IRepository<T_ManPowerLog> _manPowerLogRepository;
        private T_IWP SelectedIWP;
        private T_UserProject CurrentProject;
        private T_UserDetails CurrentUser;

        #region Properties
        private JobSettingShowHideMinor btnShowHideMinor;
        public JobSettingShowHideMinor BtnShowHideMinor
        {
            get { return btnShowHideMinor; }
            set { btnShowHideMinor = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<IWPControlLogModel> controlLogItemSource;
        public ObservableCollection<IWPControlLogModel> ControlLogItemSource
        {
            get { return controlLogItemSource; }
            set { controlLogItemSource = value; RaisePropertyChanged(); }
        }
        private string lblRejectDetails;
        public string LblRejectDetails
        {
            get { return lblRejectDetails; }
            set { lblRejectDetails = value; RaisePropertyChanged(); }
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
        #endregion
        public IWPControlLogViewModel(INavigationService _navigationService,
           IUserDialogs _userDialogs,
           IHttpHelper _httpHelper,
           ICheckValidLogin _checkValidLogin,
           IRepository<T_UserProject> _userProjectRepository,
           IRepository<T_UserDetails> _userDetailsRepository,
           IRepository<T_IWP> _iwpRepository,
           IRepository<T_IWPAdminControlLog> _iwpAdminControlLogRepository,
           IRepository<T_IWPControlLogSignatures> _iwpControlLogSignaturesRepository,
           IRepository<T_IWPPunchCategory> _iwpPunchCategoryRepository,
           IRepository<T_IWPPunchLayer> _iwpPunchLayerRepository,
           IRepository<T_ManPowerLog> _manPowerLogRepository) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._userProjectRepository = _userProjectRepository;
            this._userDetailsRepository = _userDetailsRepository;
            this._iwpRepository = _iwpRepository;
            this._iwpAdminControlLogRepository = _iwpAdminControlLogRepository;
            this._iwpControlLogSignaturesRepository = _iwpControlLogSignaturesRepository;
            this._iwpPunchCategoryRepository = _iwpPunchCategoryRepository;
            this._iwpPunchLayerRepository = _iwpPunchLayerRepository;
            this._manPowerLogRepository = _manPowerLogRepository;
            _userDialogs.HideLoading();
            PageHeaderText = "Control Log";
            JobSettingHeaderVisible = true;
        }


        #region private 
        private async Task LoadControlLogTabAsync()
        {
            var result = await _userDetailsRepository.GetAsync(x => x.ID == Settings.UserID);
            CurrentUser = result.FirstOrDefault();

            var CurrrentIWP = await _iwpRepository.QueryAsync<T_IWP>("SELECT * FROM [T_IWP] WHERE [ID] = '" + IWPHelper.IWP_ID + "'");
            SelectedIWP = CurrrentIWP.FirstOrDefault();
            var currentProj = await _userProjectRepository.GetAsync(x => x.Project_ID == Settings.ProjectID);
             CurrentProject = currentProj.FirstOrDefault();
            var gvControlLog = GenerateControlLogTableAsync(CurrentProject.Project_ID, IWPHelper.IWP_ID, BtnShowHideMinor.Checked);


            //Get Rejection Details
            string sql =
                "SELECT ACL.[SignatureName],CLS.[RejectComment], CLS.[RejectedOn], CLS.[RejectedBy] " +
                "FROM [T_IWPAdminControlLog] AS ACL " +
                "LEFT JOIN [T_IWPControlLogSignatures] CLS ON ACL.[ID] = CLS.[ControlLogAdminID] AND ACL.[ProjectID] = CLS.[ProjectID] " +
                "WHERE CLS.[IWP_ID] = '" + IWPHelper.IWP_ID + "' AND CLS.[Reject] = '" + 1 + "' " + //not sure why the etestpackageid has to put in like this instead of a oleparameter??
                "AND ACL.[ProjectID] ='" + CurrentProject.Project_ID + "'" +
                "ORDER BY ACL.[SignatureNo] DESC";


            try
            {
                var data = await _iwpAdminControlLogRepository.QueryAsync<IWPControlLogModel>(sql);
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

        private async Task GenerateControlLogTableAsync(int projectID, int IWPID, int showMinorMilestones)
        {
            string SQL = " SELECT DISTINCT ACL.[ID],ACL.[SignatureName],ACL.[MilestoneReturnAfterRevision],ACL.[SignatureNo], " // ACL.[DisciplineDisplay],
                       + " CL.[Signed],CL.[SignedBy],CL.[SignedOn],CL.[Reject] " // CL.[Live],
                       + " FROM [T_IWPAdminControlLog] AS ACL "
                       + " LEFT OUTER JOIN (SELECT * FROM [T_IWPControlLogSignatures] "
                       + " WHERE [IWP_ID] = " + IWPID + ") AS CL ON CL.ProjectID = ACL.ProjectID AND CL.ControlLogAdminID = ACL.ID "
                       + " WHERE ACL.[ProjectID] = '" + projectID + "'"
                       + " ORDER BY ACL.[SignatureNo] ASC";

            try
            {
                int srNo = 0;
                
                var data = await _iwpAdminControlLogRepository.QueryAsync<IWPControlLogModel>(SQL);
                var singedLogs = data.Where(x => x.Signed == true);
                List<IWPControlLogModel> IWPCLList = new List<IWPControlLogModel>();
                foreach (IWPControlLogModel CL in data)
                {
                    srNo++;
                    CL.SrNo = srNo;
                    CL.SignedImage = CL.Signed ? "Greenradio.png" : (CL.Reject ? "reddot.png" : "Grayradio.png");
                    IWPCLList.Add(CL);
                }

                ControlLogItemSource = new ObservableCollection<IWPControlLogModel>(IWPCLList);

            }
            catch (Exception err)
            {
                var errr = (err.Message);
            } 
        }
        private async void OnClickButton(string param)
        {
            if (param == "ShowHideMinor")
            {
                btnShowHideMinor_Clicked(param);
                _userDialogs.HideLoading();
            }
            else if(param == "Back")
            {
               await navigationService.GoBackAsync();
            }
        }

        private async void btnShowHideMinor_Clicked(string param)
        {
            JobSettingShowHideMinor _btnShowHideMinor = new JobSettingShowHideMinor();
            if (BtnShowHideMinor.Checked == 0)
                _btnShowHideMinor.Checked = 1;
            else
                _btnShowHideMinor.Checked = 0;

            _btnShowHideMinor.Text = _btnShowHideMinor.Checked == 0 ? "Hide Minor" : "Show Minor";
            BtnShowHideMinor = _btnShowHideMinor;

            await LoadControlLogTabAsync();
        }

        private async void UpdatedWorkPack(int IWP)
        {
            string SQL = "UPDATE [T_IWP] SET [Updated] = 1 WHERE [ID] = '" + IWP + "'";
            var result = await _iwpRepository.QueryAsync(SQL);
        }
        public async void ControlLog_SignClick(IWPControlLogModel IWP_CLA)
        {
            if (IWP_CLA != null) //Status Image
            {
                int IWP_CLA_ID = IWP_CLA.ID;   
                int signatureNo = IWP_CLA.SignatureNo; 
                Boolean signed = IWP_CLA.Signed;                

                if (await Application.Current.MainPage.DisplayAlert("Control Log Signature", "Are you sure you want to sign this signature?", "Yes", "No"))
                {   
                    if (await CanSignControlLogSignature(SelectedIWP.ID, IWP_CLA))
                    {
                        T_IWPControlLogSignatures controlLogSignature = new T_IWPControlLogSignatures()
                        {
                            ControlLogAdminID = IWP_CLA_ID,
                            Signed = !signed,
                            SignedByUserID = !signed ? CurrentUser.ID:0,
                            SignedBy = !signed ? CurrentUser.FullName:"",
                            SignedOn = !signed ? DateTime.UtcNow: Convert.ToDateTime("01/01/0001 0:00"),
                        };

                       if(IWP_CLA.Reject && IWP_CLA.SignedBy == CurrentUser.FullName)
                        {
                            controlLogSignature.Reject = controlLogSignature.Signed;
                            controlLogSignature.RejectedBy = CurrentUser.FullName;
                            controlLogSignature.RejectedByUserID = CurrentUser.ID;
                            controlLogSignature.RejectedOn = DateTime.UtcNow;
                        }
                            
                        if (await SaveNewAsync(SelectedIWP.ID, controlLogSignature))
                            {
                            //Adjust minor milestones
                            List<T_IWPAdminControlLog> list = await GetMinorMileStones(signatureNo); //get list of minor milestones associated

                            if (list != null && list.Count > 0) //if returned some minor milestones 
                            {
                                foreach (T_IWPAdminControlLog adminControlLog in list) //go through list and adjust the signature's control log id and save.
                                {
                                    controlLogSignature.ControlLogAdminID = adminControlLog.ID;
                                    await SaveNewAsync(SelectedIWP.ID, controlLogSignature);
                                }
                            }

                            if (IWP_CLA.SignatureName.ToLower() == "execution finished")
                               await _manPowerLogRepository.QueryAsync("UPDATE T_ManPowerLog SET Updated=1, EndTime= '" + DateTime.Now.Ticks + "' WHERE IWPID ='"+ IWPHelper.IWP_ID + "'");

                               UpdatedWorkPack(IWPHelper.IWP_ID);
                               await LoadControlLogTabAsync();
                            }
                        else
                            await Application.Current.MainPage.DisplayAlert("Control Log Signature", "Error ocurred saving signature", "OK");
                       
                           
                    }
                    
                }
            }
        }

        public async Task<Boolean> CanSignControlLogSignature(int IWPID, IWPControlLogModel IWP_CLA, bool showPopups = true, bool naAutoSignOff = true)
        {
            var controlLogAdminDetails = await _iwpAdminControlLogRepository.GetAsync(x => x.ProjectID == Settings.ProjectID && x.ID == IWP_CLA.ID);
            var adminControlLog = controlLogAdminDetails.FirstOrDefault();  //adminControlLog.Get(projectID, controllogAdminID); //Get all control log admin details for checks

            var PLayer = await _iwpPunchLayerRepository.GetAsync(x => x.ProjectID == adminControlLog.ProjectID && x.AdminControlLog_ID == adminControlLog.ID);
            var PCategory = await _iwpPunchCategoryRepository.GetAsync(x => x.ProjectID == adminControlLog.ProjectID && x.AdminControlLog_ID == adminControlLog.ID);

            adminControlLog.PunchLayer = PLayer.Select(i=>i.PunchLayer).ToList();
            adminControlLog.PunchCategory = PCategory.Select(i => i.PunchCategory).ToList();

            if (IWP_CLA.Reject && IWP_CLA.SignedBy != CurrentUser.FullName)
            {
                // await Application.Current.MainPage.DisplayAlert("Control Log", "Selected milestone has already been uploaded to VMLive and cannot be removed on the handheld.", "OK");

                var ansr = await Application.Current.MainPage.DisplayActionSheet("You currently do not have the user rights to sign off this signature", "Sign by Other", "OK");
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

             var RequiredSigned =  ControlLogItemSource.Where(i => i.ID < IWP_CLA.ID && i.Signed == false).ToList();
            if(RequiredSigned.Count > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Control Log Signature", "Can not sign this activity. Please sign the previous activity(s) before signing this.", "ok");
                return false;
            }               

            // new Code added according to requirements
            if (await ControlLogRightsCheck(adminControlLog))
                return true;
            else
            {
                bool reuturnvalue = false;
                //await Application.Current.MainPage.DisplayAlert("Control Log Signature", "Sorry, you do not have the rights to adjust this control log signature.", "ok");
                //return false;

                var ansr = await Application.Current.MainPage.DisplayActionSheet("You currently do not have the user rights to sign off this signature", "Sign by Other", "OK");
                if (ansr == "Sign by Other")
                {
                    var vm = await ReadLoginPopup();
                    if (vm.Password != null && vm.UserName != null)
                    {
                        var UserDetailsList = await _userDetailsRepository.GetAsync(x => x.UserName == vm.UserName && x.Password == vm.Password);
                        if (UserDetailsList.Any())
                        {
                            CurrentUser = UserDetailsList.FirstOrDefault();
                            reuturnvalue = true;
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Login", AppConstant.LOGIN_FAILURE, "OK");
                            reuturnvalue = false;
                        }
                    }
                }
                else 
                {
                    reuturnvalue = false;
                }
                return reuturnvalue ;
            }
        }
        public async Task<bool> CheckNextControlLogSigned(int IWPID, int signatureNo)
        {
            string Signed_SQL = " SELECT * FROM [T_IWPAdminControlLog] AS ACL INNER JOIN [T_IWPControlLogSignatures] CLS ON ACL.[ProjectID] = CLS.[ProjectID] AND ACL.[ID] = CLS.[ControlLogAdminID] "
                       + " WHERE ACL.[ProjectID] = '" + Settings.ProjectID + "' AND CLS.[IWP_ID] = '" + IWPID + "' AND ACL.[SignatureNo] > '" + signatureNo + "' AND [MilestoneReturnAfterRevision] = 1";
            var LocalSQLFunctions = await _iwpAdminControlLogRepository.QueryAsync<T_IWPAdminControlLog>(Signed_SQL);
            

            if (!LocalSQLFunctions.Any()) return false;

            var Singed = LocalSQLFunctions.FirstOrDefault();
            if (Singed != null)
            {
                return true;// Singed.Signed;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CheckPreviousControlLogSigned(int IWPID, int signatureNo)
        {
            string sql = " SELECT * FROM [T_AdminControlLog] AS ADMIN INNER JOIN [T_IWPControlLogSignatures] CL ON ADMIN.[ProjectID] = CL.[ProjectID] AND ADMIN.[ID] = CL.[ControlLogAdminID] " +
                         " WHERE ADMIN.[ProjectID] = '" + Settings.ProjectID + "' AND CL.[IWP_ID] = '" + IWPID + "' AND ADMIN.[SignatureNo] < '" + signatureNo + "' ORDER BY ADMIN.[SignatureNo] DESC";
            var LocalSQLFunctions = await _iwpAdminControlLogRepository.QueryAsync<T_ControlLogSignature>(sql);

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

        public async Task<bool> ControlLogRightsCheck(T_IWPAdminControlLog adminControlLog)
        {
            bool IsFunction_Code, IsCompany_Category_Code, IsSection_Code;            
            IsFunction_Code = IsCompany_Category_Code = IsSection_Code = false;

            if (!String.IsNullOrEmpty(adminControlLog.FunctionCode))
                IsFunction_Code = true;
            if (!String.IsNullOrEmpty(adminControlLog.CompanyCategoryCode))
                IsCompany_Category_Code = true;
            if (!String.IsNullOrEmpty(adminControlLog.SectionCode))
                IsSection_Code = true;           

            if (IsFunction_Code && CurrentUser.Function_Code.ToUpper() != adminControlLog.FunctionCode.ToUpper())
                    return false;
            
            if (IsCompany_Category_Code && CurrentUser.Company_Category_Code.ToUpper() != adminControlLog.CompanyCategoryCode.ToUpper())
                    return false;
          
            if (IsSection_Code && CurrentUser.Section_Code.ToUpper() != adminControlLog.SectionCode.ToUpper())
                    return false;

            if (adminControlLog.PunchesCompleted && !(adminControlLog.PunchCategory.Count > 0) )
                return false;

            if (adminControlLog.PunchesConfirmed && !(adminControlLog.PunchLayer.Count > 0))
                return false;

            return true;
        }

        public async Task<bool> SaveNewAsync(int IWPID, T_IWPControlLogSignatures controlLogModel)
        {

            //CheckExists
            var objcontrolLogSignature = await _iwpControlLogSignaturesRepository.GetAsync(x => x.ProjectID == Settings.ProjectID && x.IWP_ID == IWPID && x.ControlLogAdminID == controlLogModel.ControlLogAdminID);

            var CheckExists = objcontrolLogSignature.Any();
            if (CheckExists)
            {
                T_IWPControlLogSignatures ControlLogSignature = new T_IWPControlLogSignatures();
                ControlLogSignature = objcontrolLogSignature.FirstOrDefault();
                //Found use sql update.

                //ControlLogSignature.Reject = false;
                ControlLogSignature.Signed = controlLogModel.Signed;
                ControlLogSignature.SignedByUserID = controlLogModel.SignedByUserID;
                ControlLogSignature.SignedBy = controlLogModel.SignedBy;
                ControlLogSignature.SignedOn = controlLogModel.SignedOn;
                //ControlLogSignature.Live = false;
               // ControlLogSignature.Updated = true;
                ControlLogSignature.ProjectID = Settings.ProjectID;
                ControlLogSignature.IWP_ID = IWPID;
                ControlLogSignature.ControlLogAdminID = controlLogModel.ControlLogAdminID;

                //RejectedBy = CurrentUser.FullName,
                //RejectedByUserID = CurrentUser.ID,
                //RejectedOn = DateTime.UtcNow,

                string SQL = " UPDATE [T_IWPControlLogSignatures] SET [Reject] = '"+ Convert.ToInt32(controlLogModel.Reject) + "', [RejectedBy] = '" + controlLogModel.RejectedBy 
                    + "', [RejectedByUserID] = '" + controlLogModel.RejectedByUserID + "', [RejectedOn] = '" + controlLogModel.RejectedOn.Ticks
                    + "', [Signed] = '" + Convert.ToInt32(controlLogModel.Signed) + "',[SignedByUserID] = '"
                    + controlLogModel.SignedByUserID + "' , [SignedBy] = '" + controlLogModel.SignedBy + "',[SignedOn] = '" + controlLogModel.SignedOn.Ticks + "', [Updated] = 1"
                    //  + "', [Live] = 0, [Updated] = 1 "
                    + " WHERE [ProjectID] = '" + Settings.ProjectID + "' AND [IWP_ID] = '" + IWPID
                    + "' AND[ControlLogAdminID] = '" + controlLogModel.ControlLogAdminID + "'";
                try
                {
                    await _iwpControlLogSignaturesRepository.QueryAsync(SQL);
                    UpdatedWorkPack(IWPID);
                    return true;

                }
                catch (Exception Ex)
                { return false; }
            }
            else
            {
                //Insert fresh details.
                T_IWPControlLogSignatures ControlLogSignature = new T_IWPControlLogSignatures();
                //  ControlLogSignature = objcontrolLogSignature.FirstOrDefault();
                ControlLogSignature.Signed = controlLogModel.Signed;
                ControlLogSignature.SignedByUserID = controlLogModel.SignedByUserID;
                ControlLogSignature.SignedBy = controlLogModel.SignedBy;
                ControlLogSignature.SignedOn = DateTime.UtcNow;
                //ControlLogSignature.Live = false;
                //ControlLogSignature.Updated = true;
                ControlLogSignature.ProjectID = Settings.ProjectID;
                ControlLogSignature.IWP_ID = IWPID;
                ControlLogSignature.ControlLogAdminID = controlLogModel.ControlLogAdminID;

                var dBreturn = await _iwpControlLogSignaturesRepository.InsertAsync(ControlLogSignature);
                if (dBreturn == 1)
                    return true;
                else
                    return false;
            }
        }
        public async Task<List<T_IWPAdminControlLog>> GetMinorMileStones(int signatureNo)
        {
            // List<T_AdminControlLog> list = new List<T_AdminControlLog>();
            var SQL = await _iwpAdminControlLogRepository.GetAsync(x => x.ProjectID == Settings.ProjectID && x.SignatureNo == signatureNo);
            return SQL.ToList();
        }
        #endregion

        #region Public
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (IWPHelper.IWP_ID != null)
            {
                JobSettingShowHideMinor _btnShowHideMinor = new JobSettingShowHideMinor();
                _btnShowHideMinor.Checked = 1;
                _btnShowHideMinor.Text = "Show Minor";
                BtnShowHideMinor = _btnShowHideMinor;

                //BtnShowHideMinor = new JobSettingShowHideMinor{ Checked = 1, Text = "Show Minor" };
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

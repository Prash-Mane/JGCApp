using Acr.UserDialogs;
using JGC.Common.Interfaces;
using JGC.Models.E_Test_Package;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using JGC.Common.Extentions;
using Rg.Plugins.Popup.Services;
using JGC.Views.E_Reporter;
using JGC.DataBase.DataTables;
using JGC.DataBase;
using System.Linq;
using JGC.Common.Helpers;
using JGC.Common.Constants;
using JGC.Models;
using System.Threading.Tasks;

namespace JGC.ViewModels.E_Reporter
{
   public class DWRControlLogViewModel : BaseViewModel
    {
        protected readonly INavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IHttpHelper _httpHelper;
        private readonly ICheckValidLogin _checkValidLogin;
        private readonly IRepository<T_EReports_Signatures> _signaturesRepository;
        private readonly IRepository<T_UserDetails> _userDetailsRepository;
        private readonly IRepository<T_EReports_UsersAssigned> _usersAssignedRepository;
        private readonly IRepository<T_DWR> _DWRRepository;
        private T_UserDetails userDetail;
        private T_UserDetails OtherUser;

        #region properties
        private ObservableCollection<ControlLogModel> controlLogItemSource;
        public ObservableCollection<ControlLogModel> ControlLogItemSource
        {
            get { return controlLogItemSource; }
            set { controlLogItemSource = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<T_EReports_Signatures> _signatureList;
        public ObservableCollection<T_EReports_Signatures> SignatureList
        {
            get { return _signatureList; }
            set { _signatureList = value; RaisePropertyChanged("SignatureList"); OnPropertyChanged(); }
        }

        private T_EReports_Signatures _selectedSignatureItem;
        public T_EReports_Signatures SelectedSignatureItem
        {
            get { return _selectedSignatureItem; }
            set
            {
                if (SetProperty(ref _selectedSignatureItem, value))
                {
                    // UpdateSignatureItem(_selectedSignatureItem);
                    OnPropertyChanged();
                }
            }
        }
        private string _AFINO;
        public string AFINO
        {
            get { return _AFINO; }
            set { SetProperty(ref _AFINO, value); }
        }
        private string _ReportNO;
        public string ReportNO
        {
            get { return _ReportNO; }
            set { SetProperty(ref _ReportNO, value); }
        }
        private string _SpoolNO;
        public string SpoolNO
        {
            get { return _SpoolNO; }
            set { SetProperty(ref _SpoolNO, value); }
        }
        private string _JointNO;
        public string JointNO
        {
            get { return _JointNO; }
            set { SetProperty(ref _JointNO, value); }
        }

        //internal async void SignOff()
        //{
        //    await PopupNavigation.PushAsync(new SignOffPopup(), true);
        //}
        #endregion

        #region Delegate Commands  
        public ICommand BtnCommand
        {
            get
            {
                return new Command(OnClickCloseButton);
            }
        }

        private async void OnClickCloseButton()
        {
            if (DWRHelper.DWRTargetType == typeof(InspectJointViewModel))
            {
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add(NavigationParametersConstants.SelectedDWRReport, DWRHelper.SelectedDWR);
                navigationParameters.Add(NavigationParametersConstants.NavigatonServiceParameter, navigationService);
                await navigationService.NavigateFromMenuAsync(typeof(InspectJointViewModel), navigationParameters);
            }
            else
            await navigationService.NavigateFromMenuAsync(DWRHelper.DWRTargetType);
        }

        #endregion


        public DWRControlLogViewModel(
            INavigationService _navigationService,
            IUserDialogs _userDialogs,
            IHttpHelper _httpHelper,
            ICheckValidLogin _checkValidLogin,
            IRepository<T_EReports_Signatures> _signaturesRepository,
            IRepository<T_UserDetails> _userDetailsRepository,
            IRepository<T_EReports_UsersAssigned> _usersAssignedRepository,
            IRepository<T_DWR> _DWRRepository
           ) : base(_navigationService, _httpHelper, _checkValidLogin)
        {
            this._navigationService = _navigationService;
            this._httpHelper = _httpHelper;
            this._checkValidLogin = _checkValidLogin;
            this._userDialogs = _userDialogs;
            this._signaturesRepository = _signaturesRepository;
            this._userDetailsRepository = _userDetailsRepository;
            this._usersAssignedRepository = _usersAssignedRepository;
            this._DWRRepository = _DWRRepository;
            _userDialogs.HideLoading();
            IsHeaderBtnVisible = true;
            PageHeaderText = "Control Logs";

            AFINO = DWRHelper.SelectedDWR.AFINo;
            ReportNO = DWRHelper.SelectedDWR.ReportNo;
            SpoolNO = DWRHelper.SelectedDWR.SpoolNo;
            JointNO = DWRHelper.SelectedDWR.JointNo;
            GetSignatureData();

        }


        #region Private
        private async void GetSignatureData()
        {
            //var signaturesData = await _signaturesRepository.QueryAsync<T_EReports_Signatures>(@"SELECT * FROM T_EReports_Signatures WHERE [EReportID] = '" + ID + "' ORDER BY [SignatureNo] ASC");
            var signaturesData = await _signaturesRepository.QueryAsync<T_EReports_Signatures>(@"SELECT * FROM T_EReports_Signatures WHERE [RowID] = '" + DWRHelper.SelectedDWR.RowID + "'");
            //var signaturesData = await _signaturesRepository.GetAsync(x => x.RowID == DWRHelper.SelectedDWR.RowID);
            var UserDetailsList = await _userDetailsRepository.GetAsync();
            if (UserDetailsList.Count > 0)
                userDetail = UserDetailsList.Where(p => p.ID == Settings.UserID).FirstOrDefault();
            SignatureList = new ObservableCollection<T_EReports_Signatures>(signaturesData.Distinct());
        }

        public async void UpdateSignatureItem(bool IsLogedInUser)
        {
            T_UserDetails signeduser = new T_UserDetails();

            if (IsLogedInUser)
                signeduser = userDetail;
            else
                signeduser = OtherUser;

                T_EReports_Signatures _selectedSignatureItem = new T_EReports_Signatures();
            _selectedSignatureItem = SelectedSignatureItem;

            if (_selectedSignatureItem == null)
                return;


            foreach (T_EReports_Signatures CurrentSignture in SignatureList.Where(x => x.SignatureNo == _selectedSignatureItem.SignatureNo && x.SignatureRulesID == _selectedSignatureItem.SignatureRulesID).ToList())
            {

                //Found, now adjust
                if (CurrentSignture.VMSigned)
                {
                    //This has been signed off in VMlive database so cannot change
                    await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "This signature has been signed off in the VMLive database and cannot be changed on the handheld. If you need to remove this signature then remove it in VMLive and re-download this report", "OK");
                }
                else
                {
                    //Check to see if previous sign off has been done
                    Boolean PreviousSigned = false;

                    if (CurrentSignture.SignatureNo == 1)
                        PreviousSigned = true;
                    else
                    {
                        foreach (T_EReports_Signatures PreviousSignature in SignatureList)
                        {
                            if (PreviousSignature.SignatureNo == (CurrentSignture.SignatureNo - 1))
                            {
                                PreviousSigned = PreviousSignature.Signed;
                                break;
                            }
                        }
                    }

                    if (PreviousSigned)
                    {
                        //Check to see if you can sign off.

                        var CheckExists = await _usersAssignedRepository.GetAsync(x => x.EReportID == SelectedSignatureItem.EReportID && x.SignatureRulesID == CurrentSignture.SignatureRulesID && x.UserID == signeduser.ID);
                        if (CheckExists.Any())
                        {
                            //Can Sign Off
                            if (CurrentSignture.Signed)
                            {
                                //Check to see if next sign off has been completed
                                Boolean NextSigned = true;

                                if (CurrentSignture.SignatureNo == SignatureList.Count())
                                    NextSigned = false;
                                else
                                {
                                    foreach (T_EReports_Signatures PreviousSignature in SignatureList)
                                    {
                                        if (PreviousSignature.SignatureNo == (CurrentSignture.SignatureNo + 1))
                                        {
                                            NextSigned = PreviousSignature.Signed;
                                            break;
                                        }
                                    }
                                }

                                if (NextSigned)
                                    await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "Unable to remove this signature as the following signature has been completed.", "OK");

                                else
                                {
                                    //Remove Sign off
                                    CurrentSignture.Signed = false;
                                    CurrentSignture.SignedByFullName = "";
                                    CurrentSignture.SignedByUserID = 0;
                                    CurrentSignture.SignedOn = Convert.ToDateTime("2000-01-01");
                                    CurrentSignture.Updated = true;
                                    SaveSignaturesofDWR(CurrentSignture);
                                }
                            }
                            else
                            {
                                Boolean CanUpdate = true;

                                if(CurrentSignture.SignatureNo == 1)
                                {
                                    if((DWRHelper.SelectedDWR.WeldedDate == Convert.ToDateTime("01/01/0001 0:00") || DWRHelper.SelectedDWR.WeldedDate == Convert.ToDateTime("01/01/2000 0:00") || DWRHelper.SelectedDWR.WeldedDate == Convert.ToDateTime("01/01/1900 0:00")) 
                                    || (DWRHelper.SelectedDWR.FitUpDate == Convert.ToDateTime("01/01/0001 0:00") || DWRHelper.SelectedDWR.FitUpDate == Convert.ToDateTime("01/01/2000 0:00") || DWRHelper.SelectedDWR.FitUpDate == Convert.ToDateTime("01/01/1900 0:00")) 
                                    || (!String.IsNullOrEmpty(DWRHelper.SelectedDWR.RootWelder1) && !String.IsNullOrEmpty(DWRHelper.SelectedDWR.RootWelder2) && !String.IsNullOrEmpty(DWRHelper.SelectedDWR.RootWelder3) && !String.IsNullOrEmpty(DWRHelper.SelectedDWR.RootWelder4))
                                    || (!String.IsNullOrEmpty(DWRHelper.SelectedDWR.FillCapWelder1) && !String.IsNullOrEmpty(DWRHelper.SelectedDWR.FillCapWelder2) && !String.IsNullOrEmpty(DWRHelper.SelectedDWR.FillCapWelder3) && !String.IsNullOrEmpty(DWRHelper.SelectedDWR.FillCapWelder4))
                                    || String.IsNullOrEmpty(DWRHelper.SelectedDWR.RootWeldProcess) || String.IsNullOrEmpty(DWRHelper.SelectedDWR.WPSNo) 
                                    || String.IsNullOrEmpty(DWRHelper.SelectedDWR.BaseMetal1) || String.IsNullOrEmpty(DWRHelper.SelectedDWR.BaseMetal2)
                                    || String.IsNullOrEmpty(DWRHelper.SelectedDWR.HeatNo1) || String.IsNullOrEmpty(DWRHelper.SelectedDWR.HeatNo1))
                                    {
                                        CanUpdate = false;
                                        await _userDialogs.AlertAsync("WeldedDate, FitUpDate, RootWelder at least 1, FillCapWelder  at least 1, RootWeldProcess, WPS, BaseMetal1, BaseMetal2, HeatNo1 and HeatNo2 can’t be empty. Please fill these fields before continuing", null, "Ok");
                                       
                                    }
                                }
                                else
                                {
                                    if ((DWRHelper.SelectedDWR.WeldedDate == Convert.ToDateTime("01/01/0001 0:00") || DWRHelper.SelectedDWR.WeldedDate == Convert.ToDateTime("01/01/2000 0:00") || DWRHelper.SelectedDWR.WeldedDate == Convert.ToDateTime("01/01/1900 0:00"))
                                    || (DWRHelper.SelectedDWR.FitUpDate == Convert.ToDateTime("01/01/0001 0:00") || DWRHelper.SelectedDWR.FitUpDate == Convert.ToDateTime("01/01/2000 0:00") || DWRHelper.SelectedDWR.FitUpDate == Convert.ToDateTime("01/01/1900 0:00"))
                                    || (!String.IsNullOrEmpty(DWRHelper.SelectedDWR.RootWelder1) && !String.IsNullOrEmpty(DWRHelper.SelectedDWR.RootWelder2) && !String.IsNullOrEmpty(DWRHelper.SelectedDWR.RootWelder3) && !String.IsNullOrEmpty(DWRHelper.SelectedDWR.RootWelder4))
                                    || (!String.IsNullOrEmpty(DWRHelper.SelectedDWR.FillCapWelder1) && !String.IsNullOrEmpty(DWRHelper.SelectedDWR.FillCapWelder2) && !String.IsNullOrEmpty(DWRHelper.SelectedDWR.FillCapWelder3) && !String.IsNullOrEmpty(DWRHelper.SelectedDWR.FillCapWelder4))
                                    || String.IsNullOrEmpty(DWRHelper.SelectedDWR.RootWeldProcess) || String.IsNullOrEmpty(DWRHelper.SelectedDWR.WPSNo)
                                    || String.IsNullOrEmpty(DWRHelper.SelectedDWR.BaseMetal1) || String.IsNullOrEmpty(DWRHelper.SelectedDWR.BaseMetal2)
                                    || String.IsNullOrEmpty(DWRHelper.SelectedDWR.HeatNo1) || String.IsNullOrEmpty(DWRHelper.SelectedDWR.HeatNo1)
                                    || String.IsNullOrEmpty(DWRHelper.SelectedDWR.VI) || String.IsNullOrEmpty(DWRHelper.SelectedDWR.DI))
                                    {
                                        CanUpdate = false;
                                        await _userDialogs.AlertAsync("WeldedDate, FitUpDate, RootWelder at least 1, FillCapWelder  at least 1, RootWeldProcess, WPS, BaseMetal1, BaseMetal2, HeatNo1, HeatNo2, VI Inspect Result and DI Inspect Result can’t be empty. Please fill these fields before continuing", null, "Ok");
                                       
                                    }
                                }
                                //if (_selectedEReportItem.ReportType.ToUpper() == "DAILY WELD REPORT" && CurrentSignture.SignatureNo > 1)
                                //{
                                //    if (CurrentDWR != null)
                                //    {
                                //        foreach (DWRRow Row in CurrentDWR.DWRRows)
                                //        {
                                //            if (string.IsNullOrEmpty(Row.VI) || string.IsNullOrEmpty(Row.DI) || string.IsNullOrEmpty(Row.WPS_No) || string.IsNullOrEmpty(Row.Base_Metal_1) || string.IsNullOrEmpty(Row.Heat_No_1) || string.IsNullOrEmpty(Row.Base_Metal_2) || string.IsNullOrEmpty(Row.Heat_No_2))
                                //            {
                                //                await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "Unable to sign off as joint(s) is incomplete. Please ensure the following are completed for every joint, VI, DI, WPS No, Base Metal 1, Base Metal 2, Heat No 1 and Heat No 2.", "OK");

                                //                CanUpdate = false;
                                //                break;
                                //            }
                                //        }
                                //    }
                                //    else
                                //    {
                                //        await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "Unable to sign off as joint(s) is incomplete. Please ensure the following are completed for every joint, VI, DI, WPS No, Base Metal 1, Base Metal 2, Heat No 1 and Heat No 2.", "OK");

                                //        CanUpdate = false;
                                //    }
                                //}

                                if (CanUpdate)
                                {

                                    //Add details to Sign Off
                                    CurrentSignture.Signed = true;
                                    CurrentSignture.SignedByFullName = CurrentSignture.Signed ? signeduser.FullName : "";
                                    CurrentSignture.SignedByUserID = CurrentSignture.Signed ? signeduser.ID : CurrentSignture.SignedByUserID;
                                    CurrentSignture.SignedOn = DateTime.UtcNow;
                                    CurrentSignture.Updated = true;
                                    SaveSignaturesofDWR(CurrentSignture);
                                }
                            }

                        }
                        else
                        {
                            //Do not have rights for this sign off
                            // await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "You currently do not have the user rights to sign off this signature", "OK");

                           var ansr =   await Application.Current.MainPage.DisplayActionSheet("You currently do not have the user rights to sign off this signature",  "Sign by Other", "OK");
                            if (ansr == "Sign by Other")
                            {
                                var vm = await ReadLoginPopup();
                                if (vm.Password != null && vm.UserName != null)
                                {
                                    var UserDetailsList = await _userDetailsRepository.GetAsync(x=>x.UserName == vm.UserName && x.Password == vm.Password);
                                    if (UserDetailsList.Any())
                                    {
                                        OtherUser = UserDetailsList.FirstOrDefault();
                                        UpdateSignatureItem(false);
                                    }
                                    else
                                    {
                                       await Application.Current.MainPage.DisplayAlert( "Login", AppConstant.LOGIN_FAILURE, "OK");
                                    }   


                                }
                            }

                        }
                    }
                    else
                        await Application.Current.MainPage.DisplayAlert("Signature Sign Off", "Previous sign off must be completed to sign off this signature", "OK");
                }

                break;
            }

            SignatureList = new ObservableCollection<T_EReports_Signatures>(SignatureList);
        }

        private async void SaveSignaturesofDWR(T_EReports_Signatures item)
        {
            try
            {
                  await _signaturesRepository.QueryAsync<T_EReports_Signatures>(@"UPDATE T_EReports_Signatures SET [Signed] = " + Convert.ToInt32(item.Signed) + ",[SignedByUserID] = " + item.SignedByUserID
                                                                                    + ",[SignedByFullName] = '" + item.SignedByFullName + "',[SignedOn] = '" + item.SignedOn.Ticks
                                                                                    + "', [Updated] = 1 WHERE [EReportID] = " + item.EReportID + " AND [SignatureRulesID] = '" + item.SignatureRulesID
                                                                                    + "' AND [RowID] = '" + item.RowID + "' AND [DisplaySignatureName] = '"+ item.DisplaySignatureName + "'");
                  await _DWRRepository.QueryAsync<T_DWR>(@"UPDATE T_DWR SET [Updated] = 1 WHERE [RowID] = " + item.RowID);
               
               // _userDialogs.AlertAsync("E-Report Signatures data saved successfully", "Save Signatures Data", "OK");
            }
            catch (Exception ex)
            {

            }
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

        #region Public

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

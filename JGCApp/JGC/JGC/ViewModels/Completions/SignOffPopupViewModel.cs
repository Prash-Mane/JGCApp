using JGC.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JGC.ViewModels.Completions
{
    public class SignOffPopupViewModel : BindableBase
    {


        private TaskCompletionSource<LoginModel> tcs = new TaskCompletionSource<LoginModel>();
        #region properties

        public SignOffPopupViewModel()
        {
            UserCred = new LoginModel();
        }

        private LoginModel userCred;

        public LoginModel UserCred
        {
            get { return userCred; }
            set { userCred = value; RaisePropertyChanged(); }
        }

        private string loginHeaderText;
        public string LoginHeaderText
        {
            get { return loginHeaderText; }
            set { loginHeaderText = value; RaisePropertyChanged(); }
        }

        private string loginButtonText;
        public string LoginButtonText
        {
            get { return loginButtonText; }
            set { loginButtonText = value; RaisePropertyChanged(); }
        }

        #endregion

        #region Commands 
        public ICommand OKCommand
        {
            get
            {
                return new Command(() => tcs.SetResult(UserCred));
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                return new Command(() => tcs.SetResult(new LoginModel()));
            }
        }
        #endregion


        public Task<LoginModel> GetValue()
        {
            return tcs.Task;
        }
    }
}

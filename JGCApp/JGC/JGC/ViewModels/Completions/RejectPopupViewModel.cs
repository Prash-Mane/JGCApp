using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JGC.ViewModels.Completions
{
    public class RejectPopupViewModel : BindableBase
    {
        private TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
        #region properties

        private string rejectComment;

        public string RejectComment
        {
            get { return rejectComment; }
            set { rejectComment = value; RaisePropertyChanged(); }
        }

        #endregion

        #region Commands 
        public ICommand OKCommand
        {
            get
            {
                return new Command(() => tcs.SetResult(RejectComment));
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                return new Command(() => tcs.SetResult(null));
            }
        }
        #endregion


        public Task<string> GetValue()
        {
            return tcs.Task;
        }

    }
}

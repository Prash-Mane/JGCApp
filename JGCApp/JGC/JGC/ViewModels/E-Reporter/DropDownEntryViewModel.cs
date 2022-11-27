using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JGC.ViewModels.E_Reporter
{
    public class DropDownEntryViewModel : BindableBase
    {
        private TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
        #region properties
        private string inputString;
        public List<string> FilterList { get; set; }
        public string InputString
        {
            get { return inputString; }
            set
            {
                inputString = value;
                RaisePropertyChanged();
                if (!string.IsNullOrEmpty(InputString))
                    ListviewSource = FilterList.Where(x => x.ToLower().Contains(InputString.ToLower())).ToList();
            }
        }

        private string selectedInput;
        public string SelectedInput
        {
            get { return selectedInput; }
            set
            {
                selectedInput = value;
                RaisePropertyChanged();

                InputString = SelectedInput;
            }
        }

        private List<string> listviewSource;

        public List<string> ListviewSource
        {
            get { return listviewSource; }
            set { listviewSource = value; RaisePropertyChanged(); }
        }

        #endregion

        #region Commands 
        public ICommand OKCommand
        {
            get
            {
                return new Command(() => tcs.SetResult(InputString));
            }
        }
        public ICommand CancelCommand
        {
            get
            {
                return new Command(() => tcs.SetResult("clear"));
            }
        }
        #endregion
        public DropDownEntryViewModel(List<string> Source)
        {
            ListviewSource = Source;
            FilterList = Source;
        }
        public Task<string> GetValue()
        {
            return tcs.Task;
        }
    }
}

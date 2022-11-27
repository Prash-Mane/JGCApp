using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JGC.ViewModels.Completions
{
    public class FilterPopupViewModel : BindableBase
    {
        private TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
        #region properties
        private string inputString;
        public List<string> FilterList { get; set; }
        public List<string> MasterList { get; set; }
        public string InputString
        {
            get { return inputString; }
            set
            {
                inputString = value;
                RaisePropertyChanged();
                if (!string.IsNullOrEmpty(InputString))
                    ListviewSource = FilterList.Where(x => x.ToLower().Contains(InputString.ToLower())).OrderBy(x => x).ToList();
                else
                ListviewSource = MasterList;

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

        public FilterPopupViewModel(List<string> Source)
        {
            ListviewSource = MasterList = Source.OrderBy(x=>x).ToList();
            FilterList = Source.OrderBy(x => x).ToList();
        }

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


        public Task<string> GetValue()
        {
            return tcs.Task;
        }

    }
}

using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JGC.ViewModels.Work_Pack
{
    public class FilterPopupViewModel : BindableBase
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
                if (!string.IsNullOrEmpty(inputString))
                {
                    ListviewSource = FilterList.Where(x => x.ToLower().Contains(inputString.ToLower())).ToList();
                }               
                RaisePropertyChanged();
            }
        }

        private string selectedInput;
        public string SelectedInput
        {
            get { return selectedInput; }
            set
            {
                selectedInput = value;
                InputString = selectedInput;
                RaisePropertyChanged();

                
            }
        }

        private List<string> listviewSource;

        public FilterPopupViewModel(List<string> Source)
        {
            ListviewSource = Source;
            FilterList = Source;
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

                return new Command(() => {
                    if (ListviewSource.Count() > 0)
                        tcs.SetResult(InputString);
                    else
                        tcs.SetResult(null);
                });
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

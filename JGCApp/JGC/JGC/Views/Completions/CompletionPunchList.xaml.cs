using JGC.DataBase.DataTables.Completions;
using JGC.ViewModels.Completions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.Completions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompletionPunchList : ContentPage
    {
        private CompletionPunchListViewModel PunchListVm;
        public CompletionPunchList()
        {
            InitializeComponent();
            PunchListVm = (CompletionPunchListViewModel)this.BindingContext;
        }
        public CompletionPunchList(string ItrName)
        {
            InitializeComponent();
            PunchListVm = (CompletionPunchListViewModel)this.BindingContext;
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
        }

        private void BtnBarcodeScanClicked(object sender, EventArgs e)
        {
        }
        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {   //Filter PunchList
                var _sender = ((Picker)sender);
                if (_sender != null && _sender.ItemsSource.Count > 1)
                {
                    if (_sender.SelectedIndex > 0)
                    {
                        if (PunchListVm != null)
                        {
                            var selectedItem = _sender.SelectedItem.ToString();
                            PunchListVm.ItemSourceCompletionsPunchList = new ObservableCollection<T_CompletionsPunchList>(PunchListVm.PunchListitem.Where(x =>
                                x.systemno.Trim().Equals(selectedItem.Trim()) ||
                                x.subsystem.Trim().Equals(selectedItem.Trim()) ||
                                x.PCWBS.Trim().Equals(selectedItem.Trim()) ||
                                x.FWBS.Trim().Equals(selectedItem.Trim()) ||
                                x.location.Trim().Equals(selectedItem.Trim()) ||
                                x.respdisc.Trim().Equals(selectedItem.Trim()) ||
                                x.status.Trim().ToUpper().Equals(selectedItem.Trim().ToUpper()) ||
                            /*  x.workpack.Trim().Equals(selectedItem.Trim()) ||    */
                                x.tagno.Trim().Equals(selectedItem.Trim()) ||
                                x.itrname.Trim().Equals(selectedItem.Trim()) ||
                            /*  x.rfqreq.Trim().Equals(selectedItem.Trim()) ||      */
                                x.priority.Trim().Equals(selectedItem.Trim())));
                        }
                    }
                    else
                    {
                        if (PunchListVm != null)
                        {
                            PunchListVm.ItemSourceCompletionsPunchList = new ObservableCollection<T_CompletionsPunchList>(PunchListVm.PunchListitem);
                        }
                    }
                }
            }
            catch (Exception Ex)
            { }
        }
    }
}
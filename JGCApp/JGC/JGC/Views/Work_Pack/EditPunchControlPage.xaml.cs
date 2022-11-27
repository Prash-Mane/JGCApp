using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.Work_Pack
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPunchControlPage : ContentPage
    {
        public ObservableCollection<PuchDetail> puchDetails;
        public EditPunchControlPage()
        {
            InitializeComponent();

            puchDetails = new ObservableCollection<PuchDetail>();
            puchDetails.Add(new PuchDetail() { PunchID = "1", CWPTag = "TagXX", Component = "XXXX", Decription = "XXXXX", Status = "Open" });
            puchDetails.Add(new PuchDetail() { PunchID = "1", CWPTag = "TagXX", Component = "XXXX", Decription = "XXXXX", Status = "Closed" });
            puchDetails.Add(new PuchDetail() { PunchID = "1", CWPTag = "TagXX", Component = "XXXX", Decription = "XXXXX", Status = "Closed" });
            puchDetails.Add(new PuchDetail() { PunchID = "1", CWPTag = "TagXX", Component = "XXXX", Decription = "XXXXX", Status = "Open" });

            PunchGrid.ItemsSource = puchDetails;
        }
    }

    public class PuchDetail
    {
        public string PunchID { get; set; }
        public string CWPTag { get; set; }
        public string Component { get; set; }
        public string Decription { get; set; }
        public string Status { get; set; }
        public string Image { get; set; }
    }
}
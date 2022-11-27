using JGC.DataBase.DataTables.Completions;
using JGC.Models.Completions;
using JGC.ViewModels.Completions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using JGC.DataBase.DataTables.ITR;
using JGC.Common.Interfaces;
using JGC.Common.Helpers;
using System.Text.RegularExpressions;
using JGC.UserControls.CustomControls;
using System.ComponentModel;
using Xamarin.Forms.DataGrid;

namespace JGC.Views.Completions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ITRPage : ContentPage
    {
        private TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
        private double width, height = 0;
        string input = "";
        bool IsEntryInitialized = false;
        public ITRViewModel ITRVm;
        ZXingScannerPage scanPage;
        private string FocusedProperty;
        ViewCell lastCell;
        Color lastcolor;
        public ITRPage()
        {
            InitializeComponent();
            var viewModel = (ITRViewModel)BindingContext;
            width = this.Width;
            height = this.Height;
            ITRVm = (ITRViewModel)this.BindingContext;
            TagsCount.Text = "Tags: " + ITRVm.ItemSourceTagList.Count;
            Stack2.IsVisible = GridQuetion.IsVisible = StackRemark.IsVisible = StackSignOff.IsVisible = false;
            BtnReject.IsVisible = false;
            if (Device.Idiom == TargetIdiom.Phone)
            {
                //Taglist.RowHeight = 30;
                CheckSheetList.RowHeight = 30;
            }
            else if (Device.Idiom == TargetIdiom.Desktop)
            {
                CheckSheetList.RowHeight = 40;
            }
        }
        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = lastcolor;

            var viewCell = (ViewCell)sender;
            var color = ITRVm.SelectedTag.StatusColor;

            // var  color = viewCell.View.BackgroundColor.GetHashCode();
            lastcolor = Color.FromHex(color);
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.FromHex("#304D61");
                viewCell.View.VerticalOptions = LayoutOptions.Fill;
                viewCell.View.HorizontalOptions = LayoutOptions.Fill;

                lastCell = viewCell;
            }
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height); //must be called
            if (Device.RuntimePlatform == "Android")
            {
                this.width = width;
                this.height = height;
                if (width > height)
                {
                    //landscap
                    Stack1.Orientation = StackOrientation.Horizontal;
                    Frame1.Margin = new Thickness(-5, 10, 10, 10);
                    Frame2.Margin = new Thickness(10, 10, -5, 10);
                    Frame1.Padding = Frame2.Padding = new Thickness(8);
                }
                else
                {
                    //portrait
                    Stack1.Orientation = StackOrientation.Vertical;
                    Frame1.Margin = new Thickness(5, 5, 5, 0);
                    Frame2.Margin = new Thickness(5, 0, 5, 5);
                    Frame1.Padding = Frame2.Padding = new Thickness(8);
                }
            }
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (((TappedEventArgs)e).Parameter == null) return;
            string param = (string)((TappedEventArgs)e).Parameter;

            if (param == "ChooseSystem")
            {
                if (await CheckTagListNull()) return;
                var list = ITRVm._tagsList.Select(x => x.system).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterSystem.Text = input;
                else lblFilterSystem.Text = "System";
                FileterTAg();
            }
            //else if (param == "ChooseClass")
            //{
            //    if (await CheckTagListNull()) return;
            //    var list = ITRVm._tagsList.Select(x => x.tagClass).Distinct().ToList();
            //    input = await ReadStringInPopup(list);

            //    if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterClass.Text = input;
            //    else lblFilterClass.Text = "Class";

            //    FileterTAg();
            //}
            else if (param == "ChooseFC_Lev_1")
            {
                if (await CheckTagListNull()) return;
                var list = ITRVm._tagsList.Select(x => x.tagCategory).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterFC_Lev_1.Text = input;
                else lblFilterFC_Lev_1.Text = "FC Lev-1";
                FileterTAg();
            }
            else if (param == "ChooseTag")
            {
                if (await CheckTagListNull()) return;
                var list = ITRVm._tagsList.Select(x => x.name).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterTag.Text = input;
                else lblFilterTag.Text = "Tag";

                FileterTAg();
            }
            //else if (param == "ChooseSubSystem")
            //{
            //    if (await CheckTagListNull()) return;
            //    var list = ITRVm._tagsList.Select(x => x.subSystem).Distinct().ToList();
            //    input = await ReadStringInPopup(list);
            //    if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterSubSystem.Text = input;
            //    else lblFilterSubSystem.Text = "SubSystem";
            //    FileterTAg();
            //}
            else if (param == "ChooseFC_Lev_2")
            {
                if (await CheckTagListNull()) return;
                var list = ITRVm._tagsList.Select(x => x.tagClass).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterFC_Lev_2.Text = input;
                else lblFilterFC_Lev_2.Text = "FC Lev-2";
                FileterTAg();
            }
            //else if (param == "ChooseFC_Lev_1")
            //{
            //    if (await CheckTagListNull()) return;
            //    var list = ITRVm._tagsList.Select(x => x.tagCategory).Distinct().ToList();
            //    input = await ReadStringInPopup(list);
            //    if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterFC_Lev_1.Text = input;
            //    else lblFilterFC_Lev_1.Text = "FC Lev-1";
            //    FileterTAg();
            //}
            else if (param == "ChooseITR")
            {
                if (await CheckTagListNull()) return;
                var list = ITRVm._tagsList.Select(x => x.SheetName).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterITR.Text = input;
                else lblFilterITR.Text = "ITR";
                FileterTAg();
            }
            else if (param == "ChooseFWBS")
            {
                if (await CheckTagListNull()) return;
                var list = ITRVm._tagsList.Select(x => x.fwbs).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterFWBS.Text = input;
                else lblFilterFWBS.Text = "FWBS";
                FileterTAg();
            }
            else if (param == "ChooseFC_Lev_3")
            {
                if (await CheckTagListNull()) return;
                var list = ITRVm._tagsList.Select(x => x.tagFunction).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterFC_Lev_3.Text = input;
                else lblFilterFC_Lev_3.Text = "FC Lev-2";
                FileterTAg();
            }
            else if (param == "ChooseDrawings")
            {
                if (await CheckTagListNull()) return;
                var list = ITRVm._tagsList.Select(x => x.drawing).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterDrawings.Text = input;
                else lblFilterDrawings.Text = "Drawings";
                FileterTAg();
            }
            else if (param == "ChoosePCWBS")
            {
                if (await CheckTagListNull()) return;
                var list = ITRVm._tagsList.Select(x => x.pcwbs).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterPCWBS.Text = input;
                else lblFilterPCWBS.Text = "PCWBS";
                FileterTAg();
            }
            else if (param == "ChooseWorkPack")
            {
                if (await CheckTagListNull()) return;
                var list = ITRVm._tagsList.Select(x => x.workpack).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterWorkPack.Text = input;
                else lblFilterWorkPack.Text = "WorkPack";
                FileterTAg();
            }
            else if (param == "ChooseDrawingRevision")
            {
                if (await CheckTagListNull()) return;
                var list = ITRVm._tagsList.Select(x => x.drawingRevision).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterDrawingRevision.Text = input;
                else lblFilterDrawingRevision.Text = "DrawingRevision";
                FileterTAg();
            }
            
        }
        private async Task<bool> CheckTagListNull()
        {
            if (ITRVm._tagsList == null)
            {
                input = await ReadStringInPopup(new List<string>());
                return true;

            }
            return false;
        }
        private void FileterTAg()
        {
            string expression = " 1==1 ";
            if (lblFilterSystem.Text != "System")
            {
                expression += " AND " + string.Format("(iif(system == null, \"\", system).ToString().Contains(\"{0}\"))", lblFilterSystem.Text);
            }
            if (lblFilterFC_Lev_1.Text != "FC Lev-1")
            {
                expression += " AND " + string.Format("(iif(tagCategory == null, \"\", tagCategory).ToString().Contains(\"{0}\"))", lblFilterFC_Lev_1.Text);
            }
            //if (lblFilterClass.Text != "Class")
            //{
            //    expression += " AND " + string.Format("(iif(tagClass == null, \"\", tagClass).ToString().Contains(\"{0}\"))", lblFilterClass.Text);
            //}
            if (lblFilterTag.Text != "Tag")
            {
                expression += " AND " + string.Format("(iif(name == null, \"\", name).ToString().Contains(\"{0}\"))", lblFilterTag.Text);
            }
            //if (lblFilterSubSystem.Text != "SubSystem")
            //{
            //    expression += " AND " + string.Format("(iif(subSystem == null, \"\", subSystem).ToString().Contains(\"{0}\"))", lblFilterSubSystem.Text);
            //}
            if (lblFilterFC_Lev_2.Text != "FC Lev-2")
            {
                expression += " AND " + string.Format("(iif(tagFunction == null, \"\", tagFunction).ToString().Contains(\"{0}\"))", lblFilterFC_Lev_2.Text);
            }
            //if (lblFilterFC_Lev_1.Text != "FC Lev-1")
            //{
            //    expression += " AND " + string.Format("(iif(tagCategory == null, \"\", tagCategory).ToString().Contains(\"{0}\"))", lblFilterFC_Lev_1.Text);
            //}
            if (lblFilterITR.Text != "ITR")
            {
                expression += " AND " + string.Format("(iif(SheetName == null, \"\", SheetName).ToString().Contains(\"{0}\"))", lblFilterITR.Text);
            }

            if (lblFilterFWBS.Text != "FWBS")
            {
                expression += " AND " + string.Format("(iif(fwbs == null, \"\", fwbs).ToString().Contains(\"{0}\"))", lblFilterFWBS.Text);
            }

            if (lblFilterFC_Lev_3.Text != "FC Lev-3")
            {
                expression += " AND " + string.Format("(iif(tagFunction == null, \"\", tagFunction).ToString().Contains(\"{0}\"))", lblFilterFC_Lev_3.Text);
            }
            if (lblFilterDrawings.Text != "Drawings")
            {
                expression += " AND " + string.Format("(iif(drawing == null, \"\", drawing).ToString().Contains(\"{0}\"))", lblFilterDrawings.Text);
            }

            if (lblFilterPCWBS.Text != "PCWBS")
            {
                expression += " AND " + string.Format("(iif(pcwbs == null, \"\", pcwbs).ToString().Contains(\"{0}\"))", lblFilterPCWBS.Text);
            }
            if (lblFilterWorkPack.Text != "WorkPack")
            {
                expression += " AND " + string.Format("(iif(workpack == null, \"\", workpack).ToString().Contains(\"{0}\"))", lblFilterWorkPack.Text);
            }
            if (lblFilterDrawingRevision.Text != "DrawingRevision")
            {
                expression += " AND " + string.Format("(iif(drawingRevision == null, \"\", drawingRevision).ToString().Contains(\"{0}\"))", lblFilterDrawingRevision.Text);
            }
            //if (lblFilterClass.Text != " ")
            //{
            //    // expression += " AND " + string.Format("(" + lblChooseSubsystem + " = {0})", false);
            //}
            ITRVm.ItemSourceTagList = new ObservableCollection<dynamic>(ITRVm._tagsList.AsQueryable().Where(expression));
            TagsCount.Text = "Tags: " + ITRVm.ItemSourceTagList.Count;
        }
        public static Task<string> ReadStringInPopup(List<string> Source)
        {
            var vm = new FilterPopupViewModel(Source);
            //vm.FilterList = Source;
            var tcs = new TaskCompletionSource<string>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var page = new FilterPopup(vm, Source);
                await PopupNavigation.PushAsync(page);
                var value = await vm.GetValue();
                await PopupNavigation.PopAsync(true);
                tcs.SetResult(value == null ? "" : value);
            });
            return tcs.Task;
        }
        private void ListView_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            var thisListView = (ItemTappedEventArgs)e;
            var viewModel = (ITRViewModel)BindingContext;

            // if (e.Item == null) return;
            if (thisListView.Item != null)
            {
                    bool result = Task.Run(async () => await viewModel.OnCheckSheetSelectionChage()).Result;
                    if (result)
                    {
                        viewModel.PreviousBtnCmd = "";
                        viewModel.PreviousBtnVisible = false;
                        viewModel.NextSaveBtnText = "NEXT SECTION";
                        viewModel.NextBtnCmd = "Step2";
                        HeaderText.Text = "Header";
                        Stack1.IsVisible = GridQuetion.IsVisible = StackRemark.IsVisible = StackSignOff.IsVisible = false;
                        Stack2.IsVisible = StackHeader.IsVisible = true;
                        RPDrowingList.SelectedItem = viewModel.ItrSheetHeaders.DrawingNo;
                    //viewModel.SelectedCheckSheet = null;
                }
                    else
                         DisplayAlert("", "No data available for this ITR", "Ok");
            }

        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (!App.IsBusy)
            {
                App.IsBusy = true;
                var confirmed = await DisplayAlert("Exit", "Do you wish to return to the tag selection screen?", "Yes", "No");
                if (confirmed)
                {
                    var viewModel = (ITRViewModel)BindingContext;
                    viewModel.BindTagsAsync();
                    viewModel.OnTagSelectionChage();
                    viewModel.QuestionFrame = viewModel.ITR_040AFrame = viewModel.ITR_080AFrame = viewModel.ITR_8100002AFrame = viewModel.ITR8140_001AFrame =  viewModel.ITR_8161001AFrame = viewModel.Itr8121_004AFrame = viewModel.ITR_8161_002AFrame =
                    viewModel.ITR_8000_101AFrame = viewModel.ITR_8211_002AFrame = viewModel.ITR_7000_101AFrame = viewModel.ITR_8170_002AFrame = false;
                    viewModel.IndexFrame = true;
                    viewModel.PreviousBtnCmd = "";
                    viewModel.PreviousBtnVisible = false;
                    viewModel.NextSaveBtnText = "NEXT SECTION";
                    viewModel.NextBtnCmd = "Step2";
                    Stack1.IsVisible = true;
                    StackHeader.IsVisible = false;
                    Stack2.IsVisible = GridQuetion.IsVisible = StackRemark.IsVisible =
                    StackSignOff.IsVisible =  false;
                    QuetionsHighlight.BackgroundColor = FormsHighlight.BackgroundColor = SignOffHighlight.BackgroundColor = Color.FromHex("#EBF2FA");
                    HeaderText.Text = "Header";
                    BtnReject.IsVisible = false;
                    App.IsBusy = false;
                    StackHeader.IsVisible = false;
                   // viewModel.TagSheetHeadersList = new ObservableCollection<T_TAG_SHEET_HEADER>();
                    viewModel.SignOffList = new ObservableCollection<T_CommonHeaderFooterSignOff>();
                    // CheckSheetList.SelectedItem = null;
                }
                else
                {
                    App.IsBusy = false;
                }
            }

        }
        private void Tapped_De(object sender, EventArgs e)
        {
            var viewModel = (ITRViewModel)BindingContext;
            if (e == null) return;
            var item = (TappedEventArgs)e;
            viewModel.ContactResisTest = (T_ITR8140_001A_ContactResisTest)item.Parameter;

            viewModel.clickCm("deleteit");

        }

        private void Tapped_Delete8300_004A(object sender, EventArgs e)
        {
            var viewModel = (ITRViewModel)BindingContext;
            if (e == null) return;
            var item = (TappedEventArgs)e;
            viewModel.ItemITR_8300_003A_Illumin = (T_ITR_8300_003A_Illumin)item.Parameter;

            viewModel.clickCm("deleteitITR8300_003A");

        }
        private async void Exit_Button_Clicked(object sender, EventArgs e)
        {
            if (!App.IsBusy)
            {
                App.IsBusy = true;
                var confirm = await DisplayAlert("Exit", "You have reached the end of this ITR. Exit to tag selection?", "Yes", "No");
                if (confirm)
                {
                    var viewModel = (ITRViewModel)BindingContext;
                    viewModel.BindTagsAsync();
                    viewModel.OnTagSelectionChage();
                    // viewModel.UpdateSignoffSection();
                    viewModel.QuestionFrame = viewModel.ITR_040AFrame = viewModel.ITR_080AFrame = viewModel.ITR_8100002AFrame=viewModel.ITR8140_001AFrame = viewModel.ITR_8161001AFrame = viewModel.Itr8121_004AFrame = viewModel.ITR_8161_002AFrame = 
                    viewModel.ITR_8000_101AFrame = viewModel.ITR_8211_002AFrame = viewModel.ITR_7000_101AFrame = viewModel.ITR_8170_002AFrame = viewModel.ITR_8300_003AFrame = viewModel.ITR_8170_007AFrame = false;
                    viewModel.IndexFrame = true;
                    viewModel.PreviousBtnCmd = "";
                    viewModel.PreviousBtnVisible = false;
                    viewModel.NextSaveBtnText = "NEXT SECTION";
                    viewModel.NextBtnCmd = "Step2";
                    Stack1.IsVisible = true;
                    Stack2.IsVisible = GridQuetion.IsVisible = StackRemark.IsVisible =
                    StackSignOff.IsVisible = false;
                    QuetionsHighlight.BackgroundColor = FormsHighlight.BackgroundColor = SignOffHighlight.BackgroundColor = Color.FromHex("#EBF2FA");
                    HeaderText.Text = "Header";
                    BtnReject.IsVisible = false;
                    StackHeader.IsVisible = false;
                    //viewModel.TagSheetHeadersList = new ObservableCollection<T_TAG_SHEET_HEADER>();
                    viewModel.SignOffList = new ObservableCollection<T_CommonHeaderFooterSignOff>();
                    App.IsBusy = false;
                    // CheckSheetList.SelectedItem = null;
                }
                else
                {
                    App.IsBusy = false;
                }
            }

        }

       
        private async void ButtonClicked(object sender, EventArgs e)
        {
            var viewModel = (ITRViewModel)BindingContext;
            var btn = (Button)sender;

            if ((string)btn.CommandParameter == "Step1")
            {
                viewModel.QuestionFrame = viewModel.ITR_040AFrame = viewModel. Itr8000003AFRAME = viewModel.ITR_080AFrame = viewModel.ITR_8100_001AFrame = viewModel.ITR_8100002AFrame = viewModel.ITR_8121_002AFrame = 
                viewModel.ITR_8260_002AFrame =  viewModel.ITR_8161001AFrame = viewModel.Itr8121_004AFrame = viewModel.ITR8140_001AFrame = viewModel.ITR_8161_002AFrame = viewModel.ITR_8000_101AFrame = viewModel.ITR_8211_002AFrame = 
                viewModel.ITR_7000_101AFrame = viewModel.ITR_8140_002AFrame = viewModel.ITR_8140_004AFrame = viewModel.ITR_8170_002AFrame = viewModel.ITR_8300_003AFrame = viewModel.ITR_8170_007AFrame = false;
                viewModel.IndexFrame = true;
                HeaderText.Text = "Header";
                viewModel.PreviousBtnCmd = "";
                viewModel.PreviousBtnVisible = false;
                viewModel.NextSaveBtnText = "NEXT SECTION";
                viewModel.NextBtnCmd = "Step2";
                QuetionsHighlight.BackgroundColor = Color.FromHex("#EBF2FA");

                GridQuetion.IsVisible = StackRemark.IsVisible = StackSignOff.IsVisible = false;
                StackHeader.IsVisible = true;
            }
            else if ((string)btn.CommandParameter == "Step2")
            {
                //string AFINO = viewModel.TagSheetHeadersList.Where(x => x.ColumnKey == "AFI No").Select(x => x.ColumnValue).FirstOrDefault();
                string AFINO = viewModel.ItrSheetHeaders.AFINo;
                if (!String.IsNullOrEmpty(AFINO))
                {
                    viewModel.UpdateAFINo(AFINO);
                    viewModel.IndexFrame = false;
                    if (viewModel.SelectedCheckSheet.ITRNumber == "7000-030A" || viewModel.SelectedCheckSheet.ITRNumber == "7000-031A")
                        viewModel.QuestionFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "7000-040A" || viewModel.SelectedCheckSheet.ITRNumber == "7000-041A" || viewModel.SelectedCheckSheet.ITRNumber == "7000-042A")
                        viewModel.ITR_040AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "7000-080A" || viewModel.SelectedCheckSheet.ITRNumber == "7000-090A" || viewModel.SelectedCheckSheet.ITRNumber == "7000-091A")
                        viewModel.ITR_080AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "8000-003A" || viewModel.SelectedCheckSheet.ITRNumber == "8000-004A")
                         viewModel.Itr8000003AFRAME = true; 
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "8140-001A" || viewModel.SelectedCheckSheet.ITRNumber == "8140-001A-Standard")
                        viewModel.ITR8140_001AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "8100-001A")
                        viewModel.ITR_8100_001AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "8100-002A")
                         viewModel.ITR_8100002AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "8121-002A" || viewModel.SelectedCheckSheet.ITRNumber == "8121-002A-Standard")
                        viewModel.ITR_8121_002AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "8260-002A")
                        viewModel.ITR_8260_002AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "8161-001A" || viewModel.SelectedCheckSheet.ITRNumber == "8161-001A-Standard")
                        viewModel.ITR_8161001AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "8121-004A" || viewModel.SelectedCheckSheet.ITRNumber == "8121-004A-Standard")
                        viewModel.Itr8121_004AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "8161-002A")
                        viewModel.ITR_8161_002AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "8000-101A-Standard")
                        viewModel.ITR_8000_101AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "8211-002A" || viewModel.SelectedCheckSheet.ITRNumber == "8211-002A-Standard")
                        viewModel.ITR_8211_002AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "8140-002A-Standard" || viewModel.SelectedCheckSheet.ITRNumber == "8140-002A")
                        viewModel.ITR_8140_002AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "7000-101A" || viewModel.SelectedCheckSheet.ITRNumber == "8000-101A")
                        viewModel.ITR_7000_101AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "8140-004A" || viewModel.SelectedCheckSheet.ITRNumber == "8140-004A-Standard")
                        viewModel.ITR_8140_004AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "8170-002A")
                        viewModel.ITR_8170_002AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "8300-003A")
                        viewModel.ITR_8300_003AFrame = true;
                    else if (viewModel.SelectedCheckSheet.ITRNumber == "8170-007A")
                        viewModel.ITR_8170_007AFrame = true;


                    HeaderText.Text = "Questions";
                    viewModel.PreviousBtnCmd = "Step1";
                    viewModel.PreviousBtnVisible = true;
                    viewModel.NextSaveBtnText = "NEXT SECTION";
                    viewModel.NextBtnCmd = "Step3";
                    QuetionsHighlight.BackgroundColor = Color.Teal;
                    GridQuetion.IsVisible = true;
                    StackHeader.IsVisible = StackRemark.IsVisible = StackSignOff.IsVisible = false;
                }
                else
                    await DisplayAlert("", "Required AFI No", "Ok");
            }
            else if ((string)btn.CommandParameter == "Step3")
            {
                if (viewModel.SelectedCheckSheet.ITRNumber == "7000-030A" || viewModel.SelectedCheckSheet.ITRNumber == "7000-031A")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecordTubeColorsData()).Result;
                    if (result)
                        viewModel.QuestionFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "7000-040A" || viewModel.SelectedCheckSheet.ITRNumber == "7000-041A" || viewModel.SelectedCheckSheet.ITRNumber == "7000-042A")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecordInsulationDetailsAData()).Result;
                    if (result)
                        viewModel.ITR_040AFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "7000-080A" || viewModel.SelectedCheckSheet.ITRNumber == "7000-090A" || viewModel.SelectedCheckSheet.ITRNumber == "7000-091A")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecord_080A_09XA_Data()).Result;
                    if (result)
                        viewModel.ITR_080AFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "8000-003A" || viewModel.SelectedCheckSheet.ITRNumber == "8000-004A")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecord__8000003AData()).Result;
                    if (result)
                        viewModel.Itr8000003AFRAME = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "8100-001A")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecord_8100_001A_Data()).Result;
                    if (result)
                        viewModel.ITR_8100_001AFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                if (viewModel.SelectedCheckSheet.ITRNumber == "8140-001A" || viewModel.SelectedCheckSheet.ITRNumber == "8140-001A-Standard")
                {
                    var result = await viewModel.Save8140_001A();
                    if (result)
                        viewModel.ITR8140_001AFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }

                }
                else if ((viewModel.SelectedCheckSheet.ITRNumber == "8100-002A"))
                {
                    bool result = Task.Run(async () => await viewModel.SavRecord8100_002AData()).Result;
                    if (result)
                        viewModel.ITR_8100002AFrame = false;
                    else
                        return;
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "8121-002A" || viewModel.SelectedCheckSheet.ITRNumber == "8121-002A-Standard")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecord_8121_002A_Data()).Result;
                    if (result)
                        viewModel.ITR_8121_002AFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "8260-002A")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecord_8260_002A_Data()).Result;
                    if (result)
                        viewModel.ITR_8260_002AFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "8161-001A" || viewModel.SelectedCheckSheet.ITRNumber == "8161-001A-Standard")
                {
                    bool result = Task.Run(async () => await viewModel.SavRecord8161_001AData()).Result;
                    if (result)
                        viewModel.ITR_8161001AFrame = false;
                    else
                        return;
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "8121-004A"  || viewModel.SelectedCheckSheet.ITRNumber == "8121-004A-Standard")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecord_8121_004AData()).Result;
                    if (result)
                        viewModel.Itr8121_004AFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "8161-002A")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecord_8161_002AData()).Result;
                    if (result)
                        viewModel.ITR_8161_002AFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "8000-101A-Standard")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecord_8000_101AAData()).Result;
                    if (result)
                        viewModel.ITR_8000_101AFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "8211-002A" || viewModel.SelectedCheckSheet.ITRNumber == "8211-002A-Standard")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecord_8211_002AData()).Result;
                    if (result)
                        viewModel.ITR_8211_002AFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "7000-101A" || viewModel.SelectedCheckSheet.ITRNumber == "8000-101A")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecord_7000_101AHarmonyData()).Result;
                    if (result)
                        viewModel.ITR_7000_101AFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "8140-002A-Standard" || viewModel.SelectedCheckSheet.ITRNumber == "8140-002A")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecord8140_002AData()).Result;
                    if (result)
                        viewModel.ITR_8140_002AFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "8140-004A-Standard" || viewModel.SelectedCheckSheet.ITRNumber == "8140-004A")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecord8140_004AData()).Result;
                    if (result)
                        viewModel.ITR_8140_004AFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "8170-002A")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecord_8170_002AData()).Result;
                    if (result)
                        viewModel.ITR_8170_002AFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "8300-003A")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecord_8300_003AData()).Result;
                    if (result)
                        viewModel.ITR_8300_003AFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                else if (viewModel.SelectedCheckSheet.ITRNumber == "8170-007A")
                {
                    bool result = Task.Run(async () => await viewModel.SaveRecord_8170_007AData()).Result;
                    if (result)
                        viewModel.ITR_8170_007AFrame = false;
                    else
                    {
                        await DisplayAlert("Unable to Save", "One or More Fields are required", "OK");
                        return;
                    }
                }
                viewModel.IndexFrame = true;
                
                viewModel.GetRemarksData();
                HeaderText.Text = "Forms";
                SignOffHighlight.BackgroundColor = Color.FromHex("#EBF2FA");
                GridQuetion.IsVisible = StackHeader.IsVisible = StackSignOff.IsVisible = false;
                StackRemark.IsVisible = true;
                BtnReject.IsVisible = false;
                viewModel.PreviousBtnCmd = "Step2";
                viewModel.PreviousBtnVisible = true;
                viewModel.NextSaveBtnText = "NEXT SECTION";
                viewModel.NextBtnCmd = "Step4";
            }
            else if ((string)btn.CommandParameter == "Step4")
            {
                //SignOff Section
                //viewModel.GetSignOffList();
                viewModel.QuestionFrame = viewModel.ITR_040AFrame = viewModel.ITR_080AFrame = viewModel.ITR_8100002AFrame = viewModel.ITR8140_001AFrame = viewModel.ITR_8161001AFrame = viewModel.Itr8121_004AFrame = viewModel.ITR_8161_002AFrame = 
                viewModel.ITR_8000_101AFrame = viewModel.ITR_8211_002AFrame = viewModel.ITR_7000_101AFrame = viewModel.ITR_8170_002AFrame = viewModel.ITR_8300_003AFrame = viewModel.ITR_8170_007AFrame = false;
                viewModel.IndexFrame = true;
                viewModel.SaveRemarksData();
                HeaderText.Text = "Sign Offs";
                SignOffHighlight.BackgroundColor = Color.Teal;
                StackHeader.IsVisible = GridQuetion.IsVisible = StackRemark.IsVisible = false;
                StackSignOff.IsVisible = true;
                viewModel.PreviousBtnCmd = "Step3";
                viewModel.PreviousBtnVisible = true;
                viewModel.NextSaveBtnText = "EXIT";
                viewModel.NextBtnCmd = "EXIT";
                if (ITRVm.IsCheckSheetAccessible)
                    BtnReject.IsVisible = true;
                else
                    BtnReject.IsVisible = false;
            }
            else if ((string)btn.CommandParameter == "EXIT")
            {
                Exit_Button_Clicked(null, null);
            }
        }
        private void SignOffButtonClicked(object sender, EventArgs e)
        {
            string pram = ((Button)sender).CommandParameter.ToString();
            string SignatureName = ((T_CommonHeaderFooterSignOff)((Button)sender).BindingContext).FullName;
            ITRVm.SignOffClicked(pram, SignatureName);
        }
        private async void BtnBarcodeScanClicked(object sender, EventArgs e)
        {
            scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;

                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopModalAsync();
                    lblFilterSystem.Text = result.Text;
                    FileterTAg();
                    // DisplayAlert("Scanned Barcode", result.Text, "OK");
                });
            };

            await Navigation.PushModalAsync(scanPage);// Navigation.PushAsync();
            //Console.WriteLine("Scanned Barcode: " + result.Text);
        }
        private void RejectButtonClicked(object sender, EventArgs e)
        {
            if (!App.IsBusy)
            {
                App.IsBusy = true;
                ITRVm.SaveRejectITR();
                //ITRVm.BindTagsAsync();
            }
            // Button_Clicked(null, null);
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            string ITR_STATUS_COMPLETE = "#00FF00";

            var value = ((CheckBox)sender).IsChecked;
            if (value)
                ITRVm.ItemSourceTagList = new ObservableCollection<dynamic>(ITRVm._tagsList.Where(x => x.StatusColor != ITR_STATUS_COMPLETE));
            else
                ITRVm.ItemSourceTagList = new ObservableCollection<dynamic>(ITRVm._tagsList);
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
            ((ListView)sender).SelectedItem = null; // de-select the row
        }

        private void CheckBox_CheckedChanged(object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            string ITR_STATUS_COMPLETE = "#00FF00";

            var value = ((CheckBox)sender).IsChecked;
            if (value)
                ITRVm.ItemSourceTagList = new ObservableCollection<dynamic>(ITRVm._tagsList.Where(x => x.StatusColor != ITR_STATUS_COMPLETE));
            else
                ITRVm.ItemSourceTagList = new ObservableCollection<dynamic>(ITRVm._tagsList);
        }

        private void Tapped_DeleteItem(object sender, EventArgs e)
        {
            var viewModel = (ITRViewModel)BindingContext;
            if (e == null) return;
            var item = (TappedEventArgs)e;
            viewModel.SelectedRedioTest = (T_ITR8100_001A_RatioTest)item.Parameter;
           
            viewModel.OnClickButtonAsync("DeleteITRItem");

        }

        private void Tapped_DeleteInspectionforControlsItem(object sender, EventArgs e)
        {
            var viewModel = (ITRViewModel)BindingContext;
            if (e == null) return;
            var item = (TappedEventArgs)e;
            viewModel.SelectedInspectionforControls = (T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents)item.Parameter;

            viewModel.OnClickButtonAsync("DeleteInspectionforControlsItem");
        }

        private void Tapped_DeleteTransformerRadioTestItem(object sender, EventArgs e)
        {
            var viewModel = (ITRViewModel)BindingContext;
            if (e == null) return;
            var item = (TappedEventArgs)e;
            viewModel.SelectedTransformerRadioTest = (T_ITR8121_002A_TransformerRadioTest)item.Parameter;

            viewModel.OnClickButtonAsync("DeleteTransformerRadioTestItem");
        }


        //private void CalculateDialeticStrengthTestAverage(object sender, FocusEventArgs e)
        //{
        //    ITRVm.Records8121_002A.DialeticStrengthTestAverage = (ITRVm.Records8121_002A.DialeticStrengthTest1 + ITRVm.Records8121_002A.DialeticStrengthTest2 + ITRVm.Records8121_002A.DialeticStrengthTest3
        //                                       + ITRVm.Records8121_002A.DialeticStrengthTest4 + ITRVm.Records8121_002A.DialeticStrengthTest5 + ITRVm.Records8121_002A.DialeticStrengthTest6) / 6;
        //}
        private void CustomWhiteEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var viewModel = (ITRViewModel)BindingContext;
                var CalculateItem = ((Entry)sender).ReturnCommandParameter;
                if (String.IsNullOrEmpty(e.NewTextValue) || String.IsNullOrEmpty(e.OldTextValue)) return;
                if (CalculateItem != null)
                    viewModel.CalculateTransformerRatioTest(CalculateItem.ToString(), false);

                ((Entry)sender).Focus();
            }
            catch (Exception ex)
            {

            }
        }

        private void CalculateTransformerRatioTest(object sender, EventArgs e)
        {
            var viewModel = (ITRViewModel)BindingContext;
            if (e == null) return;
            var item = (TappedEventArgs)e;
            viewModel.SelectedTransformerRatioTest8121 = (T_ITR8121_004ATransformerRatioTest)item.Parameter;
            if(viewModel.SelectedTransformerRatioTest8121!= null)
            viewModel.CalculateTransformerRatioTest(viewModel.SelectedTransformerRatioTest8121.RowID.ToString(), true);
        }
        private void CalculateTransformerRatioTest8121_002(object sender, EventArgs e)
        {
            try
            {
                var viewModel = (ITRViewModel)BindingContext;
                if (e == null) return;
                var item = (TappedEventArgs)e;
                viewModel.SelectedTransRatioTest8121002 = (T_ITR8121_002A_TransformerRadioTest)item.Parameter;
                if (viewModel.SelectedTransRatioTest8121002 != null)
                    viewModel.CalculateTransRatioTest8121_002(viewModel.SelectedTransRatioTest8121002.RowID.ToString(), true);
            }
            catch (Exception ex)
            {

            }
        }

        private void Tapped_InsConAuxiliary8121(object sender, EventArgs e)
        {
            var viewModel = (ITRViewModel)BindingContext;
            if (e == null) return;
            var item = (TappedEventArgs)e;
            viewModel.SelectedInForConAndAuxiliary8121 = (T_ITR8121_004AInCAndAuxiliary)item.Parameter;
            viewModel.OnClickButtonAsync("DeleteInsConAndAuxiliary8121");
        }

        private void Tapped_TransRatioTest8121(object sender, EventArgs e)
        {
            var viewModel = (ITRViewModel)BindingContext;
            if (e == null) return;
            var item = (TappedEventArgs)e;
            viewModel.SelectedTransformerRatioTest8121 = (T_ITR8121_004ATransformerRatioTest)item.Parameter;
            viewModel.OnClickButtonAsync("DeleteTransRatioTest8121");
        }
        public static Task<string> ReadCommentPopup()
        {
            var vm = new RejectPopupViewModel();
            //vm.FilterList = Source;
            var tcs = new TaskCompletionSource<string>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var page = new PdfCommentPopup(vm);
                await PopupNavigation.PushAsync(page);
                var value = await vm.GetValue();
                await PopupNavigation.PopAsync(true);
                tcs.SetResult(value);
            });
            return tcs.Task;
        }

        private void CalculateTRT_8121_002(object sender, TextChangedEventArgs e)
        {
            try
            {
                var viewModel = (ITRViewModel)BindingContext;
                var CalculateItem = ((Entry)sender).ReturnCommandParameter;
                if (String.IsNullOrEmpty(e.NewTextValue) || String.IsNullOrEmpty(e.OldTextValue)) return;
                if (CalculateItem != null)
                    viewModel.CalculateTransRatioTest8121_002(CalculateItem.ToString(), false);
            }
            catch (Exception ex)
            {

            }
        }

        //private void CalculateCTRatio8100_001A(object sender, TextChangedEventArgs e)
        //{
        //    try
        //    {
        //        var viewModel = (ITRViewModel)BindingContext;
        //        var CalculateItem = ((ValidationEntry)sender).ReturnCommandParameter;
        //        if (CalculateItem != null)
        //            viewModel.CalculateCTRatio8100_001A(CalculateItem.ToString());                
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        private void CalculateDialeticStrengthTestAverage(object sender, TextChangedEventArgs e)
        {
            var viewModel = (ITRViewModel)BindingContext;

            if (viewModel == null)
                return;

            decimal DST1 = 0, DST2 = 0, DST3 = 0, DST4 = 0, DST5 = 0, DST6 = 0;
            if (!String.IsNullOrEmpty(viewModel.Records8121_002A.DialeticStrengthTest1)) DST1 = Convert.ToDecimal(viewModel.Records8121_002A.DialeticStrengthTest1);
            if (!String.IsNullOrEmpty(viewModel.Records8121_002A.DialeticStrengthTest2)) DST2 = Convert.ToDecimal(viewModel.Records8121_002A.DialeticStrengthTest2);
            if (!String.IsNullOrEmpty(viewModel.Records8121_002A.DialeticStrengthTest3)) DST3 = Convert.ToDecimal(viewModel.Records8121_002A.DialeticStrengthTest3);
            if (!String.IsNullOrEmpty(viewModel.Records8121_002A.DialeticStrengthTest4)) DST4 = Convert.ToDecimal(viewModel.Records8121_002A.DialeticStrengthTest4);
            if (!String.IsNullOrEmpty(viewModel.Records8121_002A.DialeticStrengthTest5)) DST5 = Convert.ToDecimal(viewModel.Records8121_002A.DialeticStrengthTest5);
            if (!String.IsNullOrEmpty(viewModel.Records8121_002A.DialeticStrengthTest6)) DST6 = Convert.ToDecimal(viewModel.Records8121_002A.DialeticStrengthTest6);
            decimal Avg = (DST1 + DST2 + DST3 + DST4 + DST5 + DST6) / 6;
           if (Avg > 0)
                viewModel.Records8121_002A.DialeticStrengthTestAverage = Avg.ToString("F2");
           else
                viewModel.Records8121_002A.DialeticStrengthTestAverage = "";

            viewModel.Records8121_002A = viewModel.Records8121_002A;
        }

        private void Tapped_DeleteTestInstrumentItem(object sender, EventArgs e)
        {
            var viewModel = (ITRViewModel)BindingContext;
            if (e == null) return;
            var item = (TappedEventArgs)e;
            viewModel.SelectedITRInstrument = (T_ITRInstrumentData)item.Parameter;

            viewModel.OnClickButtonAsync("DeleteTestInstrumentItem");
        }
        
        private void CalculateCT8100_001A(object sender, EventArgs e)
        {
            try
            {
                var viewModel = (ITRViewModel)BindingContext;
                if (e == null) return;
                var item = (TappedEventArgs)e;
                viewModel.SelectedCTdata = (T_ITR8100_001A_CTdata)item.Parameter;
                if (viewModel.SelectedCTdata == null) return;
                if (viewModel.SelectedCTdata != null)
                {
                    //viewModel.SelectedCTdata = viewModel.ITR8100_001A_CTdata.Where(x => x.RowID == Convert.ToInt32(CalculateItem)).FirstOrDefault();
                    //viewModel.CalculateCT8100_001A();

                    if (viewModel.SelectedCTdata == null) return;

                    if (!String.IsNullOrEmpty(viewModel.SelectedCTdata.PrimaryCurrent) && !String.IsNullOrEmpty(viewModel.SelectedCTdata.SecondaryCurrent))
                    {
                        if (Convert.ToDecimal(viewModel.SelectedCTdata.SecondaryCurrent) >= 0)
                        {
                            Decimal CalRatio = (Convert.ToDecimal(viewModel.SelectedCTdata.PrimaryCurrent) / Convert.ToDecimal(viewModel.SelectedCTdata.SecondaryCurrent)) * (1000);

                            viewModel.SelectedCTdata.Ratio = CalRatio.ToString("F2");
                            viewModel.ITR8100_001A_CTdata.Where(x => x.RowID == viewModel.SelectedCTdata.RowID).FirstOrDefault().Ratio = viewModel.SelectedCTdata.Ratio;
                        }
                        else
                            viewModel.SelectedCTdata.Ratio = "0.00";
                    }
                    else if (String.IsNullOrEmpty(viewModel.SelectedCTdata.PrimaryCurrent) && !String.IsNullOrEmpty(viewModel.SelectedCTdata.SecondaryCurrent))
                        viewModel.SelectedCTdata.Ratio = "0.00";
                    else
                        viewModel.SelectedCTdata.Ratio = "";
                    viewModel.ITR8100_001A_CTdata = new ObservableCollection<T_ITR8100_001A_CTdata>(viewModel.ITR8100_001A_CTdata);
                }
                viewModel.SelectedCTdata = null;
            }
            catch (Exception ex)
            {

            }
        }

        
        private void TextChanged_CalculateCT8100_001A(object sender, TextChangedEventArgs e)
        {
            try
            {
                var viewModel = (ITRViewModel)BindingContext;
                var CalculateItem = ((ValidationEntry)sender).ReturnCommandParameter;
                if (String.IsNullOrEmpty(e.NewTextValue) || String.IsNullOrEmpty(e.OldTextValue)) return;
                if (CalculateItem != null)
                {
                    viewModel.SelectedCTdata = viewModel.ITR8100_001A_CTdata.Where(x => x.RowID == Convert.ToInt32(CalculateItem.ToString())).FirstOrDefault();

                    if (viewModel.SelectedCTdata == null) return;

                    if (!String.IsNullOrEmpty(viewModel.SelectedCTdata.PrimaryCurrent) && !String.IsNullOrEmpty(viewModel.SelectedCTdata.SecondaryCurrent))
                    {
                        if (Convert.ToDecimal(viewModel.SelectedCTdata.SecondaryCurrent) >= 0)
                        {
                            Decimal CalRatio = (Convert.ToDecimal(viewModel.SelectedCTdata.PrimaryCurrent) / Convert.ToDecimal(viewModel.SelectedCTdata.SecondaryCurrent)) * (1000);

                            viewModel.SelectedCTdata.Ratio = CalRatio.ToString("F2");
                             viewModel.ITR8100_001A_CTdata.Where(x => x.RowID == viewModel.SelectedCTdata.RowID).FirstOrDefault().Ratio = viewModel.SelectedCTdata.Ratio;
                        }
                        else
                            viewModel.SelectedCTdata.Ratio = "0.00";
                    }
                    else if (String.IsNullOrEmpty(viewModel.SelectedCTdata.PrimaryCurrent) && !String.IsNullOrEmpty(viewModel.SelectedCTdata.SecondaryCurrent))
                        viewModel.SelectedCTdata.Ratio = "0.00";
                    else
                        viewModel.SelectedCTdata.Ratio = "";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void TextChanged_CalculateCTRatio8100_001A(object sender, TextChangedEventArgs e)
        {
            try
            {
                var viewModel = (ITRViewModel)BindingContext;
                var CalculateItem = ((ValidationEntry)sender).ReturnCommandParameter;
                if (String.IsNullOrEmpty(e.NewTextValue) || String.IsNullOrEmpty(e.OldTextValue)) return;
                if (CalculateItem != null)
                {
                    viewModel.SelectedRedioTest = viewModel.ITR8100_001A_RatioTest.Where(x => x.RowID == Convert.ToInt32(CalculateItem.ToString())).FirstOrDefault();

                    if (viewModel.SelectedRedioTest == null) return;
                    if (!String.IsNullOrEmpty(viewModel.SelectedRedioTest.PrimaryCurrent) && !String.IsNullOrEmpty(viewModel.SelectedRedioTest.SecondaryCurrent))
                    {
                        if (viewModel.SelectedCTdata == null)
                            viewModel.SelectedCTdata = viewModel.ITR8100_001A_CTdata.Where(x => x.RowID == Convert.ToInt32(CalculateItem.ToString())).FirstOrDefault();
                        if (Convert.ToDecimal(viewModel.SelectedCTdata.SecondaryCurrent) >= 0)
                        {
                            string CTRatio = viewModel.ITR8100_001A_CTdata.Where(x => x.RowNo == viewModel.SelectedRedioTest.RowNo).Select(x => x.Ratio).FirstOrDefault();
                            Decimal CRatio = (Convert.ToDecimal(viewModel.SelectedRedioTest.PrimaryCurrent) / Convert.ToDecimal(viewModel.SelectedRedioTest.SecondaryCurrent)) * (1000);
                            Decimal CalculatedRatio = Convert.ToDecimal(CRatio.ToString("F2"));
                            if (String.IsNullOrEmpty(CTRatio))
                                CTRatio = "0";
                            if (Convert.ToDecimal(CTRatio) <= 0)
                                viewModel.SelectedRedioTest.CalculatedCTRatio = CalculatedRatio + " (Infinity%)";
                            else
                            {
                                Decimal Ratio = Convert.ToDecimal(CTRatio);
                                var err = (CalculatedRatio - Ratio) * 100 / Ratio;
                                var finalErr = err.ToString("F2");
                                viewModel.SelectedRedioTest.CalculatedCTRatio = CalculatedRatio + " (Err. " + finalErr + "%)";
                            }
                        }
                        else
                            viewModel.SelectedRedioTest.CalculatedCTRatio = "0.00";
                    }
                    else if (String.IsNullOrEmpty(viewModel.SelectedRedioTest.PrimaryCurrent) && !String.IsNullOrEmpty(viewModel.SelectedRedioTest.SecondaryCurrent))
                        viewModel.SelectedRedioTest.CalculatedCTRatio = "0.00";
                    else
                        viewModel.SelectedRedioTest.CalculatedCTRatio = "";
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void CalculateCTRatio8100_001A(object sender, EventArgs e)
        {
            try
            {
                var viewModel = (ITRViewModel)BindingContext;
                if (e == null) return;
                var item = (TappedEventArgs)e;
                viewModel.SelectedRedioTest = (T_ITR8100_001A_RatioTest)item.Parameter;
                if (viewModel.SelectedRedioTest == null) return;
                if (!String.IsNullOrEmpty(viewModel.SelectedRedioTest.PrimaryCurrent) && !String.IsNullOrEmpty(viewModel.SelectedRedioTest.SecondaryCurrent))
                {
                    if (Convert.ToDecimal(viewModel.SelectedRedioTest.SecondaryCurrent) >= 0)
                    {
                        string CTRatio = viewModel.ITR8100_001A_CTdata.Where(x => x.RowNo == viewModel.SelectedRedioTest.RowNo).Select(x => x.Ratio).FirstOrDefault();
                        Decimal CRatio = (Convert.ToDecimal(viewModel.SelectedRedioTest.PrimaryCurrent) / Convert.ToDecimal(viewModel.SelectedRedioTest.SecondaryCurrent)) * (1000);
                        Decimal CalculatedRatio = Convert.ToDecimal(CRatio.ToString("F2"));
                        if (String.IsNullOrEmpty(CTRatio))
                            CTRatio = "0";
                        if (Convert.ToDecimal(CTRatio) <= 0)
                            viewModel.SelectedRedioTest.CalculatedCTRatio = CalculatedRatio + " (Infinity%)";
                        else
                        {
                            Decimal Ratio = Convert.ToDecimal(CTRatio);
                            var err = (CalculatedRatio - Ratio) * 100 / Ratio;
                            var finalErr = err.ToString("F2");
                            viewModel.SelectedRedioTest.CalculatedCTRatio = CalculatedRatio + " (Err. " + finalErr + "%)";
                        }
                    }
                    else
                        viewModel.SelectedRedioTest.CalculatedCTRatio = "0.00";
                }
                else if (String.IsNullOrEmpty(viewModel.SelectedRedioTest.PrimaryCurrent) && !String.IsNullOrEmpty(viewModel.SelectedRedioTest.SecondaryCurrent))
                    viewModel.SelectedRedioTest.CalculatedCTRatio = "0.00";
                else
                    viewModel.SelectedRedioTest.CalculatedCTRatio = "";

                viewModel.ITR8100_001A_RatioTest = new ObservableCollection<T_ITR8100_001A_RatioTest>(viewModel.ITR8100_001A_RatioTest.ToList());
                viewModel.SelectedRedioTest = null;
            }
            catch (Exception ex)
            {

            }
        }

        private async void TapGestureRecognizer_LECETapped8000_003A(object sender, EventArgs e)
        {
            if (((TappedEventArgs)e).Parameter == null) return;
            var param = (T_ITR8000_003A_AcceptanceCriteria)((TappedEventArgs)e).Parameter;

            if (param != null)
            {
                ITRVm.SelectedAcceptanceCriteria = param;
                var list = param.LECEOptionsList.Where(x=>!String.IsNullOrEmpty(x)).ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear")
                {
                    ITRVm.SelectedAcceptanceCriteria.LECE = input;
                }
                else if(input == "clear")
                {
                    ITRVm.SelectedAcceptanceCriteria.LECE = ITRVm.SelectedAcceptanceCriteria.LECE;
                }
                else ITRVm.SelectedAcceptanceCriteria.LECE = "";

                if (!ITRVm.SelectedAcceptanceCriteria.LECEOptionsList.Contains(ITRVm.SelectedAcceptanceCriteria.LECE))
                {
                    if(ITRVm.SelectedAcceptanceCriteria.LECEOptionsList.Count>3)
                    ITRVm.SelectedAcceptanceCriteria.LECEOptionsList.RemoveAt(3);
                    ITRVm.SelectedAcceptanceCriteria.LECEOptionsList.Add(ITRVm.SelectedAcceptanceCriteria.LECE);
                }
                ITRVm.AcceptanceCriteriaList = new ObservableCollection<T_ITR8000_003A_AcceptanceCriteria>(ITRVm.AcceptanceCriteriaList.ToList());
               
            }
        }
        private void ChangedCW_CheckBox(object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var viewModel = (ITRViewModel)BindingContext;
            if (viewModel == null) return; if (viewModel.ITRRecord8211_002A == null) return;
            if (viewModel.ITRRecord8211_002A.IsCW && viewModel.ITRRecord8211_002A.Inch == "CW")
                return;
            else if (!viewModel.ITRRecord8211_002A.IsCW && viewModel.ITRRecord8211_002A.Inch == "CW")
            {
                CWcheckbox.IsChecked = true;
                return;
            }
            else if (viewModel.ITRRecord8211_002A.IsCW && (viewModel.ITRRecord8211_002A.Inch == "CCW" || string.IsNullOrEmpty(viewModel.ITRRecord8211_002A.Inch)))
            {
                viewModel.ITRRecord8211_002A.IsCW = true;
                viewModel.ITRRecord8211_002A.IsCCW = false;
                viewModel.ITRRecord8211_002A.Inch = "CW";
                viewModel.ITRRecord8211_002A.IsReqInch = "LightGray";
            }
            viewModel.ITRRecord8211_002A = viewModel.ITRRecord8211_002A;
        }
        private void ChangedCCW_CheckBox(object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var viewModel = (ITRViewModel)BindingContext;
            if (viewModel == null) return; if (viewModel.ITRRecord8211_002A == null) return;
            if (viewModel.ITRRecord8211_002A.IsCCW && viewModel.ITRRecord8211_002A.Inch == "CCW")
                return;
            else if (!viewModel.ITRRecord8211_002A.IsCW && viewModel.ITRRecord8211_002A.Inch == "CCW")
            {
                CCWcheckbox.IsChecked = true;
                return;
            }
            else if (viewModel.ITRRecord8211_002A.IsCCW && (viewModel.ITRRecord8211_002A.Inch == "CW" || string.IsNullOrEmpty(viewModel.ITRRecord8211_002A.Inch)))
            {
                viewModel.ITRRecord8211_002A.IsCCW = true;
                viewModel.ITRRecord8211_002A.IsCW = false;
                viewModel.ITRRecord8211_002A.Inch = "CCW";
                viewModel.ITRRecord8211_002A.IsReqInch = "LightGray";
            }
            viewModel.ITRRecord8211_002A = viewModel.ITRRecord8211_002A;
        }

        private async void TapGestureRecognizer_LLCCTapped8000_003A(object sender, EventArgs e)
        {
            if (((TappedEventArgs)e).Parameter == null) return;
            var param = (T_ITR8000_003A_AcceptanceCriteria)((TappedEventArgs)e).Parameter;

            if (param != null)
            {
                if (param.IsReadyOnlyLLCCText)
                    return;
                    ITRVm.SelectedAcceptanceCriteria = param;
                var list = param.LLCCOptionsList.Where(x => !String.IsNullOrEmpty(x)).ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear")
                {
                    ITRVm.SelectedAcceptanceCriteria.LLCC = input;
                }
                else if (input == "clear")
                {
                    ITRVm.SelectedAcceptanceCriteria.LLCC = ITRVm.SelectedAcceptanceCriteria.LLCC;
                }
                else ITRVm.SelectedAcceptanceCriteria.LLCC = "";

                if (!ITRVm.SelectedAcceptanceCriteria.LLCCOptionsList.Contains(ITRVm.SelectedAcceptanceCriteria.LLCC))
                {
                    if (ITRVm.SelectedAcceptanceCriteria.LECEOptionsList.Count > 3)
                        ITRVm.SelectedAcceptanceCriteria.LECEOptionsList.RemoveAt(3);
                    ITRVm.SelectedAcceptanceCriteria.LLCCOptionsList.Add(ITRVm.SelectedAcceptanceCriteria.LLCC);
                }
                ITRVm.AcceptanceCriteriaList = new ObservableCollection<T_ITR8000_003A_AcceptanceCriteria>(ITRVm.AcceptanceCriteriaList.ToList());
            }
        }

        private void DrowingNo_SelectionChanged(object sender, EventArgs e)
        {
            if (RPDrowingList.SelectedItem != null)
            {
                LblDrow.IsVisible = false;
                var viewModel = (ITRViewModel)BindingContext;
                viewModel.DrowingNo_SelectionChanged();
            }
            else LblDrow.IsVisible = true;
        }

        private void Tapped_DeleteItem8260_002A(object sender, EventArgs e)
        {
            var viewModel = (ITRViewModel)BindingContext;
            if (e == null) return;
            var item = (TappedEventArgs)e;
            viewModel.DielectricRecord = (T_ITR_8260_002A_DielectricTest)item.Parameter;

            viewModel.OnClickButtonAsync("DeleteITRItem8260_002A");
        }

        
        private void CalculateITR8300_003A(object sender, EventArgs e)
        {
            try
            {
                var viewModel = (ITRViewModel)BindingContext;
                if (e == null) return;
                var item = (TappedEventArgs)e;
                viewModel.ItemITR_8300_003A_Illumin = (T_ITR_8300_003A_Illumin)item.Parameter;
                if (viewModel.ItemITR_8300_003A_Illumin == null) return;
                if (viewModel.ItemITR_8300_003A_Illumin != null)
                {
                    Decimal av = 0;
                    if (!string.IsNullOrEmpty(viewModel.ItemITR_8300_003A_Illumin.LUX1))
                        av += Convert.ToDecimal(viewModel.ItemITR_8300_003A_Illumin.LUX1);
                    if (!string.IsNullOrEmpty(viewModel.ItemITR_8300_003A_Illumin.LUX2))
                        av += Convert.ToDecimal(viewModel.ItemITR_8300_003A_Illumin.LUX2);
                    if (!string.IsNullOrEmpty(viewModel.ItemITR_8300_003A_Illumin.LUX3))
                        av += Convert.ToDecimal(viewModel.ItemITR_8300_003A_Illumin.LUX3);
                    if (!string.IsNullOrEmpty(viewModel.ItemITR_8300_003A_Illumin.LUX4))
                        av += Convert.ToDecimal(viewModel.ItemITR_8300_003A_Illumin.LUX4);

                    Decimal avarage = av / 4;
                    if (avarage > 0)
                    {
                        List<T_ITR_8300_003A_Illumin> lst = new List<T_ITR_8300_003A_Illumin>();
                        viewModel.ItemITR_8300_003A_Illumin.Avg = avarage.ToString("F2");
                        viewModel.ITR_8300_003A_IlluminList.Where(x => x.RowID == viewModel.ItemITR_8300_003A_Illumin.RowID).FirstOrDefault().Avg = viewModel.ItemITR_8300_003A_Illumin.Avg;
                        lst = viewModel.ITR_8300_003A_IlluminList.ToList(); ;
                        viewModel.ITR_8300_003A_IlluminList.Clear();
                        viewModel.ITR_8300_003A_IlluminList = new ObservableCollection<T_ITR_8300_003A_Illumin>(lst);
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private async void TapGestureRecognizer_ContinuityResultTapped(object sender, EventArgs e)
        {
            if (((TappedEventArgs)e).Parameter == null) return;
            var param = (T_ITRInsulationDetails)((TappedEventArgs)e).Parameter;

            if (param != null)
            {
                ITRVm.SelectedInsulationDetails = param;
                var list = param.ContinuityResultOptionsList.Where(x => !String.IsNullOrEmpty(x)).ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear")
                {
                    ITRVm.SelectedInsulationDetails.ContinuityResult = input;
                }
                else if (input == "clear")
                {
                    ITRVm.SelectedInsulationDetails.ContinuityResult = ITRVm.SelectedInsulationDetails.ContinuityResult;
                }
                else ITRVm.SelectedInsulationDetails.ContinuityResult = "";

                if (!ITRVm.SelectedInsulationDetails.ContinuityResultOptionsList.Contains(ITRVm.SelectedInsulationDetails.ContinuityResult))
                {
                    if (ITRVm.SelectedInsulationDetails.ContinuityResultOptionsList.Count > 3)
                        ITRVm.SelectedInsulationDetails.ContinuityResultOptionsList.RemoveAt(3);
                    ITRVm.SelectedInsulationDetails.ContinuityResultOptionsList.Add(ITRVm.SelectedInsulationDetails.ContinuityResult);
                }
                ITRVm.ItemSourceInsulationDetails = new ObservableCollection<T_ITRInsulationDetails>(ITRVm.ItemSourceInsulationDetails.ToList());

            }
        }
        private async void TapGestureRecognizer_CoretoCoreTapped(object sender, EventArgs e)
        {
            if (((TappedEventArgs)e).Parameter == null) return;
            var param = (T_ITRInsulationDetails)((TappedEventArgs)e).Parameter;

            if (param != null)
            {
                ITRVm.SelectedInsulationDetails = param;
                var list = param.CoretoCoreOptionsList.Where(x => !String.IsNullOrEmpty(x)).ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear")
                {
                    ITRVm.SelectedInsulationDetails.CoretoCore = input;
                }
                else if (input == "clear")
                {
                    ITRVm.SelectedInsulationDetails.CoretoCore = ITRVm.SelectedInsulationDetails.CoretoCore;
                }
                else ITRVm.SelectedInsulationDetails.CoretoCore = "";

                if (!ITRVm.SelectedInsulationDetails.CoretoCoreOptionsList.Contains(ITRVm.SelectedInsulationDetails.CoretoCore))
                {
                    if (ITRVm.SelectedInsulationDetails.CoretoCoreOptionsList.Count > 3)
                        ITRVm.SelectedInsulationDetails.CoretoCoreOptionsList.RemoveAt(3);
                    ITRVm.SelectedInsulationDetails.CoretoCoreOptionsList.Add(ITRVm.SelectedInsulationDetails.CoretoCore);
                }
                ITRVm.ItemSourceInsulationDetails = new ObservableCollection<T_ITRInsulationDetails>(ITRVm.ItemSourceInsulationDetails.ToList());

            }
        }        
        private async void TapGestureRecognizer_CoretoArmorTapped(object sender, EventArgs e)
        {
            if (((TappedEventArgs)e).Parameter == null) return;
            var param = (T_ITRInsulationDetails)((TappedEventArgs)e).Parameter;

            if (param != null)
            {
                ITRVm.SelectedInsulationDetails = param;
                var list = param.CoretoArmorOptionsList.Where(x => !String.IsNullOrEmpty(x)).ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear")
                {
                    ITRVm.SelectedInsulationDetails.CoretoArmor = input;
                }
                else if (input == "clear")
                {
                    ITRVm.SelectedInsulationDetails.CoretoArmor = ITRVm.SelectedInsulationDetails.CoretoArmor;
                }
                else ITRVm.SelectedInsulationDetails.CoretoArmor = "";

                if (!ITRVm.SelectedInsulationDetails.CoretoArmorOptionsList.Contains(ITRVm.SelectedInsulationDetails.CoretoArmor))
                {
                    if (ITRVm.SelectedInsulationDetails.CoretoArmorOptionsList.Count > 3)
                        ITRVm.SelectedInsulationDetails.CoretoArmorOptionsList.RemoveAt(3);
                    ITRVm.SelectedInsulationDetails.CoretoArmorOptionsList.Add(ITRVm.SelectedInsulationDetails.CoretoArmor);
                }
                ITRVm.ItemSourceInsulationDetails = new ObservableCollection<T_ITRInsulationDetails>(ITRVm.ItemSourceInsulationDetails.ToList());

            }
        }
        private async void TapGestureRecognizer_CoretoSheildTapped(object sender, EventArgs e)
        {
            if (((TappedEventArgs)e).Parameter == null) return;
            var param = (T_ITRInsulationDetails)((TappedEventArgs)e).Parameter;

            if (param != null)
            {
                ITRVm.SelectedInsulationDetails = param;
                var list = param.CoretoSheildOptionsList.Where(x => !String.IsNullOrEmpty(x)).ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear")
                {
                    ITRVm.SelectedInsulationDetails.CoretoSheild = input;
                }
                else if (input == "clear")
                {
                    ITRVm.SelectedInsulationDetails.CoretoSheild = ITRVm.SelectedInsulationDetails.CoretoSheild;
                }
                else ITRVm.SelectedInsulationDetails.CoretoSheild = "";

                if (!ITRVm.SelectedInsulationDetails.CoretoSheildOptionsList.Contains(ITRVm.SelectedInsulationDetails.CoretoSheild))
                {
                    if (ITRVm.SelectedInsulationDetails.CoretoSheildOptionsList.Count > 3)
                        ITRVm.SelectedInsulationDetails.CoretoSheildOptionsList.RemoveAt(3);
                    ITRVm.SelectedInsulationDetails.CoretoSheildOptionsList.Add(ITRVm.SelectedInsulationDetails.CoretoSheild);
                }
                ITRVm.ItemSourceInsulationDetails = new ObservableCollection<T_ITRInsulationDetails>(ITRVm.ItemSourceInsulationDetails.ToList());

            }
        }
        private async void TapGestureRecognizer_ArmortoSheildTapped(object sender, EventArgs e)
        {
            if (((TappedEventArgs)e).Parameter == null) return;
            var param = (T_ITRInsulationDetails)((TappedEventArgs)e).Parameter;

            if (param != null)
            {
                ITRVm.SelectedInsulationDetails = param;
                var list = param.ArmortoSheildOptionsList.Where(x => !String.IsNullOrEmpty(x)).ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear")
                {
                    ITRVm.SelectedInsulationDetails.ArmortoSheild = input;
                }
                else if (input == "clear")
                {
                    ITRVm.SelectedInsulationDetails.ArmortoSheild = ITRVm.SelectedInsulationDetails.ArmortoSheild;
                }
                else ITRVm.SelectedInsulationDetails.ArmortoSheild = "";

                if (!ITRVm.SelectedInsulationDetails.ArmortoSheildOptionsList.Contains(ITRVm.SelectedInsulationDetails.ArmortoSheild))
                {
                    if (ITRVm.SelectedInsulationDetails.ArmortoSheildOptionsList.Count > 3)
                        ITRVm.SelectedInsulationDetails.ArmortoSheildOptionsList.RemoveAt(3);
                    ITRVm.SelectedInsulationDetails.ArmortoSheildOptionsList.Add(ITRVm.SelectedInsulationDetails.ArmortoSheild);
                }
                ITRVm.ItemSourceInsulationDetails = new ObservableCollection<T_ITRInsulationDetails>(ITRVm.ItemSourceInsulationDetails.ToList());

            }
        }
        private async void TapGestureRecognizer_SheidtoSheildTapped(object sender, EventArgs e)
        {
            if (((TappedEventArgs)e).Parameter == null) return;
            var param = (T_ITRInsulationDetails)((TappedEventArgs)e).Parameter;

            if (param != null)
            {
                ITRVm.SelectedInsulationDetails = param;
                var list = param.SheidtoSheildOptionsList.Where(x => !String.IsNullOrEmpty(x)).ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear")
                {
                    ITRVm.SelectedInsulationDetails.SheidtoSheild = input;
                }
                else if (input == "clear")
                {
                    ITRVm.SelectedInsulationDetails.SheidtoSheild = ITRVm.SelectedInsulationDetails.SheidtoSheild;
                }
                else ITRVm.SelectedInsulationDetails.SheidtoSheild = "";

                if (!ITRVm.SelectedInsulationDetails.SheidtoSheildOptionsList.Contains(ITRVm.SelectedInsulationDetails.SheidtoSheild))
                {
                    if (ITRVm.SelectedInsulationDetails.SheidtoSheildOptionsList.Count > 3)
                        ITRVm.SelectedInsulationDetails.SheidtoSheildOptionsList.RemoveAt(3);
                    ITRVm.SelectedInsulationDetails.SheidtoSheildOptionsList.Add(ITRVm.SelectedInsulationDetails.SheidtoSheild);
                }
                ITRVm.ItemSourceInsulationDetails = new ObservableCollection<T_ITRInsulationDetails>(ITRVm.ItemSourceInsulationDetails.ToList());

            }
        }

        private void Tapped_DeleteItem8161_001A(object sender, EventArgs e)
        {
            var viewModel = (ITRViewModel)BindingContext;
            if (e == null) return;
            var item = (TappedEventArgs)e;
            viewModel.Item_8161_001A_ConRes = (T_ITRRecords_8161_001A_ConRes)item.Parameter;

            viewModel.OnClickButtonAsync("DeleteITRItem8161_001A");

        }

        //this method for populate 'Funcion Test', 'Direction Of Rotation' and 'Space Heater Circuit' dropdown list popup 
        private async void TapGestureRecognizer_TappedOnDropdownList(object sender, EventArgs e)
        {
            if (((TappedEventArgs)e).Parameter == null) return;
            var param = (string)((TappedEventArgs)e).Parameter;
            var viewModel = (ITRViewModel)BindingContext;
            if (param == "FuncionTest" && viewModel.ITRRecord8211_002A != null)
            {
                var list = viewModel.ITRRecord8211_002A.FuncionTestOptionsList.Where(x => !String.IsNullOrEmpty(x)).ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear")
                    viewModel.ITRRecord8211_002A.FuncionTest = input;
                else if (input == "clear")
                    viewModel.ITRRecord8211_002A.FuncionTest = viewModel.ITRRecord8211_002A.FuncionTest;
                else
                    viewModel.ITRRecord8211_002A.FuncionTest = "";

                if (!viewModel.ITRRecord8211_002A.FuncionTestOptionsList.Contains(viewModel.ITRRecord8211_002A.FuncionTest))
                {
                    if (viewModel.ITRRecord8211_002A.FuncionTestOptionsList.Count > 2)
                        viewModel.ITRRecord8211_002A.FuncionTestOptionsList.RemoveAt(2);
                    viewModel.ITRRecord8211_002A.FuncionTestOptionsList.Add(viewModel.ITRRecord8211_002A.FuncionTest);
                }
            }
            else if (param == "DirectionOfRotation" && viewModel.ITRRecord8211_002A != null)
            {
                var list = viewModel.ITRRecord8211_002A.DirectionOfRotationOptionsList.Where(x => !String.IsNullOrEmpty(x)).ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear")
                    viewModel.ITRRecord8211_002A.DirectionOfRotation = input;
                else if (input == "clear")
                    viewModel.ITRRecord8211_002A.DirectionOfRotation = viewModel.ITRRecord8211_002A.DirectionOfRotation;
                else
                    viewModel.ITRRecord8211_002A.DirectionOfRotation = "";

                if (!viewModel.ITRRecord8211_002A.DirectionOfRotationOptionsList.Contains(viewModel.ITRRecord8211_002A.DirectionOfRotation))
                {
                    if (viewModel.ITRRecord8211_002A.DirectionOfRotationOptionsList.Count > 2)
                        viewModel.ITRRecord8211_002A.DirectionOfRotationOptionsList.RemoveAt(2);
                    viewModel.ITRRecord8211_002A.DirectionOfRotationOptionsList.Add(viewModel.ITRRecord8211_002A.DirectionOfRotation);
                }
            }
            else if (param == "SpaceHeater" && viewModel.ITRRecord8211_002A != null)
            {
                var list = viewModel.ITRRecord8211_002A.SpaceHeaterOptionsList.Where(x => !String.IsNullOrEmpty(x)).ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear")
                    viewModel.ITRRecord8211_002A.SpaceHeater = input;
                else if (input == "clear")
                    viewModel.ITRRecord8211_002A.SpaceHeater = viewModel.ITRRecord8211_002A.SpaceHeater;
                else
                    viewModel.ITRRecord8211_002A.SpaceHeater = "";

                if (!viewModel.ITRRecord8211_002A.SpaceHeaterOptionsList.Contains(viewModel.ITRRecord8211_002A.SpaceHeater))
                {
                    if (viewModel.ITRRecord8211_002A.SpaceHeaterOptionsList.Count > 2)
                        viewModel.ITRRecord8211_002A.SpaceHeaterOptionsList.RemoveAt(2);
                    viewModel.ITRRecord8211_002A.SpaceHeaterOptionsList.Add(viewModel.ITRRecord8211_002A.SpaceHeater);
                }

            }
            viewModel.ITRRecord8211_002A = viewModel.ITRRecord8211_002A;
        }
    }
}


using JGC.Common.Helpers;
using JGC.DataBase.DataTables.Completions;
using JGC.Models.Completions;
using JGC.UserControls.CustomControls;
using JGC.ViewModels.Completions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace JGC.Views.Completions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TagRegisterPage : ContentPage
    {
        private TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
        private double width, height = 0;
        string input = "";
        bool IsEntryInitialized = false;
        private TagRegisterViewModel TagRegisterVm;
        ZXingScannerPage scanPage;
        private string FocusedItem;
        ViewCell lastCell;
        Color lastcolor;
        IReadOnlyList<Element> Child;
        public TagRegisterPage()
        {
            InitializeComponent();
            width = this.Width;
            height = this.Height;
            TagRegisterVm = (TagRegisterViewModel)this.BindingContext;
            TagsCount.Text = "Tags: " + TagRegisterVm.ItemSourceTagList.Count;
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
            var color = TagRegisterVm.SelectedTag.StatusColor;

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
                GridQuetion.HeightRequest = height - (height /4);
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
                var list = TagRegisterVm._tagsList.Select(x => x.system).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterSystem.Text = input;
                else lblFilterSystem.Text = "System";
                FileterTAg();
            }
            //else if (param == "ChooseClass")
            //{
            //    if (await CheckTagListNull()) return;
            //    var list = TagRegisterVm._tagsList.Select(x => x.tagClass).Distinct().ToList();
            //    input = await ReadStringInPopup(list);

            //    if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterClass.Text = input;
            //    else lblFilterClass.Text = "Class";

            //    FileterTAg();
            //}
            else if (param == "ChooseFC_Lev_1")
            {
                if (await CheckTagListNull()) return;
                var list = TagRegisterVm._tagsList.Select(x => x.tagCategory).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterFC_Lev_1.Text = input;
                else lblFilterFC_Lev_1.Text = "FC Lev-1";
                FileterTAg();
            }
            else if (param == "ChooseTag")
            {
                if (await CheckTagListNull()) return;
                var list = TagRegisterVm._tagsList.Select(x => x.name).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterTag.Text = input;
                else lblFilterTag.Text = "Tag";

                FileterTAg();
            }
            //else if (param == "ChooseSubSystem")
            //{
            //    if (await CheckTagListNull()) return;
            //    var list = TagRegisterVm._tagsList.Select(x => x.subSystem).Distinct().ToList();
            //    input = await ReadStringInPopup(list);
            //    if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterSubSystem.Text = input;
            //    else lblFilterSubSystem.Text = "SubSystem";
            //    FileterTAg();
            //}
            else if (param == "ChooseFC_Lev_2")
            {
                if (await CheckTagListNull()) return;
                var list = TagRegisterVm._tagsList.Select(x => x.tagClass).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterFC_Lev_2.Text = input;
                else lblFilterFC_Lev_2.Text = "FC Lev-2";
                FileterTAg();
            }
            //else if (param == "ChooseFC_Lev_1")
            //{
            //    if (await CheckTagListNull()) return;
            //    var list = TagRegisterVm._tagsList.Select(x => x.tagCategory).Distinct().ToList();
            //    input = await ReadStringInPopup(list);
            //    if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterFC_Lev_1.Text = input;
            //    else lblFilterFC_Lev_1.Text = "FC Lev-1";
            //    FileterTAg();
            //}
            else if (param == "ChooseITR")
            {
                if (await CheckTagListNull()) return;
                var list = TagRegisterVm._tagsList.Select(x => x.SheetName).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterITR.Text = input;
                else lblFilterITR.Text = "ITR";
                FileterTAg();
            }
            else if (param == "ChooseFWBS")
            {
                if (await CheckTagListNull()) return;
                var list = TagRegisterVm._tagsList.Select(x => x.fwbs).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterFWBS.Text = input;
                else lblFilterFWBS.Text = "FWBS";
                FileterTAg();
            }
            else if (param == "ChooseFC_Lev_3")
            {
                if (await CheckTagListNull()) return;
                var list = TagRegisterVm._tagsList.Select(x => x.tagFunction).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterFC_Lev_3.Text = input;
                else lblFilterFC_Lev_3.Text = "FC Lev-2";
                FileterTAg();
            }
            else if (param == "ChooseDrawings")
            {
                if (await CheckTagListNull()) return;
                var list = TagRegisterVm._tagsList.Select(x => x.drawing).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterDrawings.Text = input;
                else lblFilterDrawings.Text = "Drawings";
                FileterTAg();
            }
            else if (param == "ChoosePCWBS")
            {
                if (await CheckTagListNull()) return;
                var list = TagRegisterVm._tagsList.Select(x => x.pcwbs).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterPCWBS.Text = input;
                else lblFilterPCWBS.Text = "PCWBS";
                FileterTAg();
            }
            else if (param == "ChooseWorkPack")
            {
                if (await CheckTagListNull()) return;
                var list = TagRegisterVm._tagsList.Select(x => x.workpack).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterWorkPack.Text = input;
                else lblFilterWorkPack.Text = "WorkPack";
                FileterTAg();
            }
            else if (param == "ChooseDrawingRevision")
            {
                if (await CheckTagListNull()) return;
                var list = TagRegisterVm._tagsList.Select(x => x.drawingRevision).Distinct().ToList();
                input = await ReadStringInPopup(list);
                if (!string.IsNullOrWhiteSpace(input) && input != "clear") lblFilterDrawingRevision.Text = input;
                else lblFilterDrawingRevision.Text = "DrawingRevision";
                FileterTAg();
            }
        }
        private async Task<bool> CheckTagListNull()
        {
            if (TagRegisterVm._tagsList == null)
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
            TagRegisterVm.ItemSourceTagList = new ObservableCollection<dynamic>(TagRegisterVm._tagsList.AsQueryable().Where(expression));
            TagsCount.Text = "Tags: " + TagRegisterVm.ItemSourceTagList.Count;
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
            var viewModel = (TagRegisterViewModel)BindingContext;
           
            // if (e.Item == null) return;
            if (thisListView.Item != null)
            {
               //viewModel.GetTagHeaderList();
               //viewModel.GetSignOffList();
                bool result = Task.Run(async () => await viewModel.OnCheckSheetSelectionChage()).Result;
                if (result)
                {
                    viewModel.PreviousBtnCmd = "";
                    viewModel.PreviousBtnVisible = false;
                    viewModel.NextSaveBtnText = "NEXT SECTION";
                    viewModel.NextBtnCmd = "Step2";
                    HeaderText.Text = "Header";
                    Stack1.IsVisible = GridQuetion.IsVisible =  StackRemark.IsVisible = StackSignOff.IsVisible = false;
                    Stack2.IsVisible = viewModel.IsStackHeader = BtnNext.IsEnabled = true;
                    //viewModel.SelectedCheckSheet = null;
                }
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
                    var viewModel = (TagRegisterViewModel)BindingContext;
                    viewModel.OnExitUpdateCheckSheets();

                    viewModel.PreviousBtnCmd = "";
                    viewModel.PreviousBtnVisible = false;
                    viewModel.NextSaveBtnText = "NEXT SECTION";
                    viewModel.NextBtnCmd = "Step2";

                    Stack1.IsVisible = BtnNext.IsEnabled = true;
                    //StackHeader.IsVisible = false;
                    viewModel.IsStackHeader = false;
                    Stack2.IsVisible = GridQuetion.IsVisible = StackRemark.IsVisible = StackSignOff.IsVisible = false;
                    QuetionsHighlight.BackgroundColor = FormsHighlight.BackgroundColor = SignOffHighlight.BackgroundColor = Color.FromHex("#EBF2FA");
                 
                    HeaderText.Text = "Header";
                    BtnReject.IsVisible = false;
                    App.IsBusy = false;

                    viewModel.TagSheetHeadersList = new ObservableCollection<T_TAG_SHEET_HEADER>();
                    viewModel.ItemSourceQuetions = new ObservableCollection<CheckSheetQuetionList>();
                    viewModel.SignOffList = new ObservableCollection<T_SignOffHeader>();
                    // CheckSheetList.SelectedItem = null;
                }
                else
                {
                    App.IsBusy = false;
                }
            }

        }
        private async void Exit_Button_Clicked(object sender, EventArgs e)
        {
            if (!App.IsBusy)
            {
                App.IsBusy = true;
                var confirm = await DisplayAlert("Exit", "You have reached the end of this ITR. Exit to tag selection?", "Yes", "No");
                if (confirm)
                {
                    var viewModel = (TagRegisterViewModel)BindingContext;
                    viewModel.OnExitUpdateCheckSheets();
                    viewModel.TagSheetHeadersList = new ObservableCollection<T_TAG_SHEET_HEADER>();

                    viewModel.PreviousBtnCmd = "";
                    viewModel.PreviousBtnVisible = false;
                    viewModel.NextSaveBtnText = "NEXT SECTION";
                    viewModel.NextBtnCmd = "Step2";
                    Stack1.IsVisible = BtnNext.IsEnabled = true;
                    Stack2.IsVisible = GridQuetion.IsVisible = StackRemark.IsVisible =  StackSignOff.IsVisible = false;
                    QuetionsHighlight.BackgroundColor = FormsHighlight.BackgroundColor = SignOffHighlight.BackgroundColor = Color.FromHex("#EBF2FA");
                    HeaderText.Text = "Header";
                    BtnReject.IsVisible = false;
                    viewModel.IsStackHeader = false;
                    viewModel.TagSheetHeadersList = new ObservableCollection<T_TAG_SHEET_HEADER>();
                    viewModel.ItemSourceQuetions = new ObservableCollection<CheckSheetQuetionList>();
                    viewModel.SignOffList = new ObservableCollection<T_SignOffHeader>();
                    App.IsBusy = false;
                    // CheckSheetList.SelectedItem = null;
                }
                else
                {
                    App.IsBusy = false;
                }
            }
        }

        //private async void NextButtonClicked(object sender, EventArgs e)
        //{
        //    BtnNext.IsEnabled = true;
        //    var viewModel = (TagRegisterViewModel)BindingContext;
        //    var btnNext = (Button)sender;
        //    if (btnNext.CommandParameter == null)
        //    {
        //        //Quetions Section
        //        HeaderText.Text = "Questions";
        //        QuetionsHighlight.BackgroundColor = Color.Teal;
        //        GridQuetion.IsVisible = BtnPrevious.IsVisible = true;
        //        //StackHeader.IsVisible =
        //        StackRemark.IsVisible = StackTableFunction.IsVisible = StackSignOff.IsVisible = false;
        //        viewModel.IsStackHeader = false;
        //        btnNext.CommandParameter = BtnPrevious.CommandParameter = viewModel.IsVisibleRemarkSection ? "Step1" : "Step2";
                
               
        //        if (TagRegisterVm.IsCheckSheetAccessible)
        //        {
        //            TagRegisterVm.CheckAnswerSheetCompleted();
        //            TagRegisterVm.CheckAnswerInitialed();
        //            if (TagRegisterVm.CheckSheetCompleted && TagRegisterVm.IsChecksheetIntiled) BtnNext.IsEnabled = true;
        //            else BtnNext.IsEnabled = false;
        //        }
        //    }

        //    else if ((string)btnNext.CommandParameter == "Step1")
        //    {
        //        //if (TagRegisterVm.CheckSheetCompleted)
        //        //{
        //        //Remark Section
        //        HeaderText.Text = "Forms";
        //        FormsHighlight.BackgroundColor = Color.Teal;

        //        //StackHeader.IsVisible =
        //        GridQuetion.IsVisible = StackSignOff.IsVisible = StackTableFunction.IsVisible = false;
        //        viewModel.IsStackHeader = false;
        //        StackRemark.IsVisible = true;
        //        btnNext.CommandParameter = BtnPrevious.CommandParameter = "Step2";


        //        //}
        //        //else
        //        //{
        //        //   await DisplayAlert("Alert", "Plaase Complete Checksheet", "OK");
        //        //}
        //    }

        //    else if ((string)btnNext.CommandParameter == "Step2")
        //    {
        //        //SignOff Section
        //        //viewModel.GetSignOffList();
        //        HeaderText.Text = "Sign Offs";
        //        SignOffHighlight.BackgroundColor = Color.Teal;
        //        //StackHeader.IsVisible =
        //            GridQuetion.IsVisible = StackRemark.IsVisible = StackTableFunction.IsVisible = false;
        //        viewModel.IsStackHeader = false;
        //        StackSignOff.IsVisible = true;
        //        BtnNext.Text = "EXIT";
        //        btnNext.CommandParameter = BtnPrevious.CommandParameter = viewModel.IsVisibleRemarkSection ? "Step3" : "Step2";
        //        if (TagRegisterVm.IsCheckSheetAccessible)
        //            BtnReject.IsVisible = true;
        //        else
        //            BtnReject.IsVisible = false;
        //    }
        //    else if ((string)btnNext.CommandParameter == "Step3")
        //    {

        //        Exit_Button_Clicked(null, null);
        //    }
        //}
        //private void PreviousButtonClicked(object sender, EventArgs e)
        //{
        //    BtnNext.IsEnabled = true;
        //    var viewModel = (TagRegisterViewModel)BindingContext;
        //    var btnPrevious = (Button)sender;
        //    if ((string)btnPrevious.CommandParameter == "Step1")
        //    {
        //        //viewModel.GetTagHeaderList();
        //        HeaderText.Text = "Header";
        //        QuetionsHighlight.BackgroundColor = Color.FromHex("#EBF2FA");
        //        btnPrevious.IsVisible = GridQuetion.IsVisible = StackRemark.IsVisible = StackTableFunction.IsVisible = StackSignOff.IsVisible = false;
        //        //StackHeader.IsVisible = true;
        //        viewModel.IsStackHeader = true;
        //        BtnNext.CommandParameter = BtnPrevious.CommandParameter = null;
        //    }
        //    else if ((string)btnPrevious.CommandParameter == "Step2")
        //    {

        //        HeaderText.Text = "Questions";
        //        FormsHighlight.BackgroundColor = Color.FromHex("#EBF2FA");
        //        StackRemark.IsVisible = StackTableFunction.IsVisible = StackRemark.IsVisible = StackSignOff.IsVisible = false;
        //        GridQuetion.IsVisible = true;
               

        //        if (viewModel.TableFunctionData.Count > 0)
        //        StackTableFunction.IsVisible = true;

        //        if (viewModel.IsVisibleRemarkSection)
        //        {
        //            BtnNext.Text = "Next Section";
        //            BtnNext.CommandParameter = BtnPrevious.CommandParameter = "Step2";
        //            BtnReject.IsVisible = false;
        //        }
        //        else
        //            BtnNext.CommandParameter = BtnPrevious.CommandParameter = "Step1";

        //    }
        //    else if ((string)btnPrevious.CommandParameter == "Step3")
        //    {
        //        HeaderText.Text = "Forms";
        //        SignOffHighlight.BackgroundColor = Color.FromHex("#EBF2FA");
        //        GridQuetion.IsVisible = StackSignOff.IsVisible = StackTableFunction.IsVisible = false;//= StackHeader.IsVisible
        //        viewModel.IsStackHeader = false;
        //        //if (viewModel.IsTableFunction)
        //        //{
        //        //    StackTableFunction.IsVisible = true;
        //        //    StackRemark.IsVisible = false;
        //        //}
        //        //else
        //        //{
        //            StackRemark.IsVisible = true;
        //          //  StackTableFunction.IsVisible = false;
                   
        //       // }
        //        BtnNext.Text = "Next Section";
        //        BtnNext.CommandParameter = BtnPrevious.CommandParameter = "Step2";
        //        BtnReject.IsVisible = false;
        //    }
        //}

        private async void ButtonClicked(object sender, EventArgs e)
        {
            var viewModel = (TagRegisterViewModel)BindingContext;
            var btn = (Button)sender;

            if ((string)btn.CommandParameter == "Step1")
            {
                HeaderText.Text = "Header";
                viewModel.PreviousBtnCmd = "";
                viewModel.PreviousBtnVisible = false;
                viewModel.NextSaveBtnText = "NEXT SECTION";
                viewModel.NextBtnCmd = "Step2";
                QuetionsHighlight.BackgroundColor = FormsHighlight.BackgroundColor = SignOffHighlight.BackgroundColor = Color.FromHex("#EBF2FA");
                GridQuetion.IsVisible = StackRemark.IsVisible = StackSignOff.IsVisible = BtnReject.IsVisible = false;
                //StackHeader.IsVisible = true;
                //BtnNext.CommandParameter = BtnPrevious.CommandParameter = null;
                viewModel.IsStackHeader = BtnNext.IsEnabled = true; 
            }
            else if ((string)btn.CommandParameter == "Step2")
            {
                HeaderText.Text = "Questions";
                viewModel.PreviousBtnCmd = "Step1";
                viewModel.PreviousBtnVisible = true;
                viewModel.NextSaveBtnText = "NEXT SECTION";
                viewModel.NextBtnCmd = viewModel.IsVisibleRemarkSection ? "Step3" : "Step4";

                TagRegisterVm.CheckAnswerSheetCompleted();
                TagRegisterVm.CheckAnswerInitialed();

                if (TagRegisterVm.CheckSheetCompleted && TagRegisterVm.IsChecksheetIntiled) 
                    BtnNext.IsEnabled = true;
               else 
                    BtnNext.IsEnabled = false;
                QuetionsHighlight.BackgroundColor = Color.Teal;
                FormsHighlight.BackgroundColor = SignOffHighlight.BackgroundColor = Color.FromHex("#EBF2FA");
                viewModel.IsStackHeader =  StackRemark.IsVisible = StackSignOff.IsVisible = BtnReject.IsVisible = false;
                GridQuetion.IsVisible = true;
            }
            else if ((string)btn.CommandParameter == "Step3")
            {                
                HeaderText.Text = "Forms";
                FormsHighlight.BackgroundColor = Color.Teal;
                SignOffHighlight.BackgroundColor = Color.FromHex("#EBF2FA");
                viewModel.IsStackHeader = GridQuetion.IsVisible = StackSignOff.IsVisible = BtnReject.IsVisible = false;
                StackRemark.IsVisible = true;
                BtnReject.IsVisible = false;
                viewModel.PreviousBtnCmd = "Step2";
                viewModel.PreviousBtnVisible = BtnNext.IsEnabled = true;
                viewModel.NextSaveBtnText = "NEXT SECTION";
                viewModel.NextBtnCmd = "Step4";
                pickdate.IsVisible = false;

            }
            else if ((string)btn.CommandParameter == "Step4")
            {
                HeaderText.Text = "Sign Offs";
                FormsHighlight.BackgroundColor = SignOffHighlight.BackgroundColor = Color.Teal;
                viewModel.IsStackHeader = GridQuetion.IsVisible = StackRemark.IsVisible = false;
                StackSignOff.IsVisible = true;
                viewModel.PreviousBtnCmd = viewModel.IsVisibleRemarkSection ? "Step3" : "Step2";
                viewModel.PreviousBtnVisible = BtnNext.IsEnabled = true;
                viewModel.NextSaveBtnText = "EXIT";
                viewModel.NextBtnCmd = "EXIT";
                if (TagRegisterVm.IsCheckSheetAccessible)
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
            string SignatureName = ((T_SignOffHeader)((Button)sender).BindingContext).FullName;
            TagRegisterVm.SignOffClicked(pram, SignatureName);
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
                TagRegisterVm.SaveRejectITR();
                TagRegisterVm.BindTagsAsync();
            }
            // Button_Clicked(null, null);
        }

        //private void TapGestureRecognizer_TappedOnAns(object sender, EventArgs e)
        //{
        //    var viewModel = (TagRegisterViewModel)BindingContext;
        //    var obj = (Xamarin.Forms.TappedEventArgs)e;
        //    AnswerOptions selectedItem = (AnswerOptions)obj.Parameter;
        //    viewModel.UpdatedChecksheetsID.Add(selectedItem.id);
        //    viewModel.ChangeAnswer(selectedItem);
        //}

        private void Button_ClickedOnAns(object sender, EventArgs e)
        {
            try
            {
                var viewModel = (TagRegisterViewModel)BindingContext;
                if (viewModel.IsCheckSheetAccessible)
                {
                    Button obj = (Button)sender;
                    AnswerOptions selectedItem = (AnswerOptions)obj.CommandParameter;

                    viewModel.UpdatedChecksheetsID.Add(selectedItem.id);
                    viewModel.ChangeAnswer(selectedItem);

                    if (TagRegisterVm.CheckSheetCompleted && TagRegisterVm.IsChecksheetIntiled)
                    {
                        BtnNext.IsEnabled = true;
                    }
                    else
                    {
                        BtnNext.IsEnabled = false;
                    }

                    foreach (Button btn in ((Grid)((Button)sender).Parent).Children)
                    {
                        //btn.BackgroundColor = Color.White;
                        btn.SetValue(BackgroundColorProperty, Color.White);
                    }
                    obj.SetValue(BackgroundColorProperty, Color.FromHex("#006633"));
                    //obj.BackgroundColor = Color.FromHex("#006633");
                }
            }
            catch(Exception ex)
            {

            }

        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            string ITR_STATUS_COMPLETE = "#00FF00";

            var value = ((CheckBox)sender).IsChecked;
            if (value)
                TagRegisterVm.ItemSourceTagList = new ObservableCollection<dynamic>(TagRegisterVm._tagsList.Where(x => x.StatusColor != ITR_STATUS_COMPLETE));
            else
                TagRegisterVm.ItemSourceTagList = new ObservableCollection<dynamic>(TagRegisterVm._tagsList);
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e == null) return; // has been set to null, do not 'process' tapped event
            ((ListView)sender).SelectedItem = null; // de-select the row
        }

        private void DateChanged(object sender, DateChangedEventArgs e)
        {
            var viewModel = (TagRegisterViewModel)BindingContext;
            ((Label)Child[0]).Text = pickdate.Date.ToString("dd-MM-yyyy");
            if (((Label)Child[0]).AutomationId != null)
            {
                if (viewModel.UpdatedChecksheetsValuesID.Count > 0)
                {
                    if (!viewModel.UpdatedChecksheetsValuesID.Contains(((Label)Child[0]).AutomationId))
                        viewModel.UpdatedChecksheetsValuesID.Add(((Label)Child[0]).AutomationId);
                    viewModel.ItemSourceQuetions.ForEach(x => x.AdditionalFields.Where(z => z.ID == ((Label)Child[0]).AutomationId).ForEach(y =>
                    {
                        y.FieldValue = pickdate.Date.ToString();
                        y.FieldPlaceHolder = pickdate.Date.ToString();
                    }));
                }
                else
                {
                    viewModel.UpdatedChecksheetsValuesID.Add(((Label)Child[0]).AutomationId);
                    viewModel.ItemSourceQuetions.ForEach(x => x.AdditionalFields.Where(z => z.ID == ((Label)Child[0]).AutomationId).ForEach(y =>
                    {
                        y.FieldValue = pickdate.Date.ToString();
                        y.FieldPlaceHolder = pickdate.Date.ToString();
                    }));
                }
            }

            pickdate.IsVisible = false;
        }

        private async void FieldValue_Change_Tapped(object sender, EventArgs e)
        {
            if (TagRegisterVm.IsCheckSheetAccessible)
            {
                var child = ((Frame)sender).Children;
                // var FieldValueFromDb = ((TappedEventArgs)sender).Parameter;
                var viewModel = (TagRegisterViewModel)BindingContext;

                string fieldValue = ((Label)child[0]).Text?.ToString();
                if (viewModel != null)
                {
                    try
                    {
                        if (fieldValue.Contains("date Here"))
                        {
                            Child = child;
                            pickdate.IsVisible = true;
                            pickdate.Focus();
                            return;
                        }
                        else
                        {
                            bool flag = false;
                            string str = fieldValue.Remove(0, 2);
                            string str1 = fieldValue.Remove(0, 5);
                            int freq = fieldValue.Count(f => (f == '-'));
                            if (str.First().Equals('-') && str1.First().Equals('-'))
                                flag = true;
                            if (freq == 2 && flag)
                            {
                                Child = child;
                                pickdate.IsVisible = true;
                                pickdate.Focus();
                                return;
                            }
                        }
                    }
                    catch
                    {

                    }
                  
                    if (!String.IsNullOrEmpty(fieldValue) && fieldValue.Contains("Initial"))
                    {
                        ((Label)child[0]).Text = viewModel.GetInitialValue();
                        if (((Label)child[0]).AutomationId != null)
                        {
                            if (viewModel.UpdatedChecksheetsValuesID.Count > 0)
                            {
                                if (!viewModel.UpdatedChecksheetsValuesID.Contains(((Label)child[0]).AutomationId))
                                    viewModel.UpdatedChecksheetsValuesID.Add(((Label)child[0]).AutomationId);
                                viewModel.ItemSourceQuetions.ForEach(x => x.AdditionalFields.Where(z => z.ID == ((Label)child[0]).AutomationId).ForEach(y =>
                                {
                                    y.FieldValue = ((Label)child[0]).Text;
                                    y.FieldPlaceHolder = ((Label)child[0]).Text;

                                }));
                            }
                            else
                            {
                                viewModel.UpdatedChecksheetsValuesID.Add(((Label)child[0]).AutomationId);
                                viewModel.ItemSourceQuetions.ForEach(x => x.AdditionalFields.Where(z => z.ID == ((Label)child[0]).AutomationId).ForEach(y =>
                                {
                                    y.FieldValue = ((Label)child[0]).Text;
                                    y.FieldPlaceHolder = ((Label)child[0]).Text;
                                }));
                            }
                        }

                        viewModel.CheckAnswerInitialed();
                        if (TagRegisterVm.CheckSheetCompleted && TagRegisterVm.IsChecksheetIntiled)
                        {
                            BtnNext.IsEnabled = true;
                        }
                        else
                        {
                            BtnNext.IsEnabled = false;
                        }
                    }
                    else if (!String.IsNullOrEmpty(fieldValue) && !fieldValue.Contains("Initial"))
                    {
                        string text = await ReadCommentPopup();
                        if (!String.IsNullOrEmpty(text) && text != "")
                            ((Label)child[0]).Text = text;

                        if (((Label)child[0]).AutomationId != null)
                        {
                            if (viewModel.UpdatedChecksheetsValuesID.Count > 0)
                            {
                                if (!viewModel.UpdatedChecksheetsValuesID.Contains(((Label)child[0]).AutomationId))
                                    viewModel.UpdatedChecksheetsValuesID.Add(((Label)child[0]).AutomationId);
                                viewModel.ItemSourceQuetions.ForEach(x => x.AdditionalFields.Where(z => z.ID == ((Label)child[0]).AutomationId).ForEach(y =>
                                {
                                    y.FieldValue = ((Label)child[0]).Text;
                                    y.FieldPlaceHolder = ((Label)child[0]).Text;

                                }));
                            }
                            else
                            {
                                viewModel.UpdatedChecksheetsValuesID.Add(((Label)child[0]).AutomationId);
                                viewModel.ItemSourceQuetions.ForEach(x => x.AdditionalFields.Where(z => z.ID == ((Label)child[0]).AutomationId).ForEach(y =>
                                {
                                    y.FieldValue = ((Label)child[0]).Text;
                                    y.FieldPlaceHolder = ((Label)child[0]).Text;
                                }));
                            }
                        }
                    }
                }
            }
        }

        private void DrowingNo_SelectionChanged(object sender, EventArgs e)
        {
            TagRegisterVm.DrowingNo_SelectionChanged();
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

        private void CheckBox_CheckedChanged(object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            string ITR_STATUS_COMPLETE = "#00FF00";

            var value = ((CheckBox)sender).IsChecked;
            if (value)
                TagRegisterVm.ItemSourceTagList = new ObservableCollection<dynamic>(TagRegisterVm._tagsList.Where(x => x.StatusColor != ITR_STATUS_COMPLETE));
            else
                TagRegisterVm.ItemSourceTagList = new ObservableCollection<dynamic>(TagRegisterVm._tagsList);
        }

       
        private void AnswerBtnClicked(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            btn.BackgroundColor = Color.FromHex("#006633");
        }
    }

    internal class CheckedChangedEventArgs
    {
    }
}
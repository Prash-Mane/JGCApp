using JGC.Common.Helpers;
using JGC.Models.Completions;
using JGC.ViewModels.Completions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.Completions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TableFunctionTypePopup : PopupPage
    {
        public TableFunctionTypePopup(TableFunctionTypeViewModel vm, TableFunctionModel item)
        {
            BindingContext = vm;
            InitializeComponent();
            var viewModel = (TableFunctionTypeViewModel)BindingContext;
            viewModel.TableFunctionData = item;
            List<string> DDvalue = new List<string> { "Yes", "No" };
            string VALUETYPE = item.itemNo.Split('_')[0].ToUpper();
            if(item.TypeValue.ToLower().Contains("date"))
                VALUETYPE = "DATETIME";
            else if (!String.IsNullOrEmpty(item.AnswerOptions))
            {
                int i = item.AnswerOptions.Split(',').Count();
                if (i > 2)
                    DDvalue.Add("NA");
                VALUETYPE = "BOOLEAN";
            }

            switch (VALUETYPE)
            {
                case "DATETIME":
                    {
                        DateTime commentDate = !String.IsNullOrEmpty(item.CheckValue) ? DateTime.ParseExact(item.CheckValue, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now;
                        viewModel.TableFunctionCommentDate = commentDate;
                        EntryInput.IsVisible = BooleanPickerInput.IsVisible = false;
                        DatePickerInput.IsVisible = true;
                    }
                    break;
                case "BOOLEAN":
                    {
                        BooleanPickerInput.ItemsSource = DDvalue;
                        viewModel.TableFunctionBoolean = item.CheckValue;
                        EntryInput.IsVisible = DatePickerInput.IsVisible = false;
                        BooleanPickerInput.IsVisible = true;
                    }
                    break;
                default:
                    {
                        EntryInput.Text = item.CheckValue;
                        DatePickerInput.IsVisible = BooleanPickerInput.IsVisible = false;
                        EntryInput.IsVisible = true;
                    }
                    break;
            }
            //if (item.TypeValue == "DATE")
            //{
            //    DateTime commentDate = !String.IsNullOrEmpty(item.CheckValue) ? DateTime.ParseExact(item.CheckValue, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now;
            //    viewModel.TableFunctionCommentDate = commentDate;
            //    EntryInput.IsVisible = BooleanPickerInput.IsVisible = false;
            //    DatePickerInput.IsVisible = true;
            //}
            //else if (item.TypeValue == "BOOLEAN")
            //{
            //    BooleanPickerInput.ItemsSource = new List<string> { "Yes", "No", "N/A" };
            //    BooleanPickerInput.SelectedItem = item.CheckValue == "yes" ? "Yes" : item.CheckValue == "no" ? "No" : "N/A";
            //    EntryInput.IsVisible = DatePickerInput.IsVisible = false;
            //    BooleanPickerInput.IsVisible = true;
            //}
            //else
            //{
            //    //if (item.TypeValue == "INITIAL")
            //    //{
            //    //    string[] strSplit = Settings.CompletionUserName.Split();
            //    //    foreach (string res in strSplit)
            //    //        EntryInput.Text += res.ToUpper().Substring(0, 1);

            //    //    // EntryInput.Text = !String.IsNullOrEmpty(item.CheckValue) ? item.CheckValue : Settings.CompletionUserName;
            //    //}
            //    //else
            //    EntryInput.Text = item.CheckValue;

            //    DatePickerInput.IsVisible = BooleanPickerInput.IsVisible = false;
            //    EntryInput.IsVisible = true;
            //}
        }
        protected override void OnDisappearing()
        {
            App.IsBusy = false;
        }
    }
}
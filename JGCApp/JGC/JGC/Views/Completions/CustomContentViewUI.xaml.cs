using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase.DataTables.Completions;
using JGC.Models.Completions;
using JGC.ViewModels.Completions;
using JGC.Views.Completions;
using Rg.Plugins.Popup.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.Completions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomContentViewUI : ContentView
    {
        public static readonly BindableProperty TableFunctionDataProperty = BindableProperty.Create(nameof(TableFunctionData), typeof(List<TableFunctionModel>), typeof(CustomContentViewUI), defaultValue: new List<TableFunctionModel>(), defaultBindingMode: BindingMode.OneWay, propertyChanged: TableFunctionDataPropertyChanged);
        public SQLiteConnection conn;
        private static void TableFunctionDataPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomContentViewUI)bindable;
            if(newValue!=null)
            control.LoadTableFunctiondata((List<TableFunctionModel>)newValue);
        }       
        public List<TableFunctionModel> TableFunctionData
        {
            get => (List<TableFunctionModel>) base.GetValue(TableFunctionDataProperty); 
            set
            {
                base.SetValue(TableFunctionDataProperty, value);
            }
        }
        public CustomContentViewUI()
        {
            InitializeComponent();
            conn = DependencyService.Get<ISQLite>().GetConnection();
        }
        public ICommand ClickTableFunctionCommand
        {
            get
            {
                return new Command<TableFunctionModel>(OnClick_TableFunction);
            }
        }
        private void LoadTableFunctiondata(List<TableFunctionModel> TableFunctionData)
        {
            if (TableFunctionData.Count <= 0) return;
            FormGridLayout.ColumnDefinitions.Clear();
            FormGridLayout.RowDefinitions.Clear();
            FormGridLayout.Children.Clear();

            // get row and column count as per data
            int col = TableFunctionData.OrderByDescending(x => x.ColumnIndex).FirstOrDefault().ColumnIndex;
            int row = TableFunctionData.OrderByDescending(x => x.RowIndex).FirstOrDefault().RowIndex;

            //set auto width for column
            while (col >= 0)
            {
                FormGridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Auto) });
                col--;
            }

            //Define Header column titles in the first row
            foreach (TableFunctionModel item in TableFunctionData.Where(x => x.RowIndex == 0))
            {
                // FormGridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Auto) });
                var label = new Label
                {
                    Text = item.TypeValue,
                    VerticalOptions = LayoutOptions.Fill,
                    HorizontalOptions = LayoutOptions.Fill,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.Black,
                };
                var frame = new Frame
                {
                    BorderColor = Color.Black,
                    CornerRadius = 0,
                };
                frame.Content = label;
                FormGridLayout.Children.Add(frame, item.ColumnIndex, item.RowIndex);

            }

            //define row column values with thier type
            foreach (TableFunctionModel item in TableFunctionData)
            {
                // FormGridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                var frame = new Frame
                {
                    BorderColor = Color.Gray,
                    CornerRadius = 0,
                };
                if (item.ColumnIndex > 0)
                {
                    var tapGestureRecognizer = new TapGestureRecognizer { CommandParameter = item, Command = ClickTableFunctionCommand };
                    frame.GestureRecognizers.Add(tapGestureRecognizer);

                    var label = new Label
                    {
                        Text = !String.IsNullOrEmpty(item.CheckValue) ? item.CheckValue : "", //item.TypeValue,
                        VerticalOptions = LayoutOptions.Fill,
                        HorizontalOptions = LayoutOptions.Fill,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        TextColor = Color.Black,
                    };
                    frame.Content = label;
                }
                else
                {
                    var label = new Label
                    {
                        Text = item.Description,
                        VerticalOptions = LayoutOptions.Fill,
                        HorizontalOptions = LayoutOptions.Fill,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Color.Black,
                    };
                    frame.Content = label;
                }

                FormGridLayout.Children.Add(frame, item.ColumnIndex, item.RowIndex + 1);
            }
        }
        public async void OnClick_TableFunction(TableFunctionModel param)
        {
            if (!App.IsBusy)
            {
                App.IsBusy = true;
                string ReturnedValue = "";
                ReturnedValue = await ReadTableFunctionTypePopup(param);

                if (ReturnedValue != null)
                {
                    param.CheckValue = ReturnedValue;
                   SaveTypeFunctionChanges(param);

                    var frame = new Frame
                    {
                        BorderColor = Color.Gray,
                        CornerRadius = 0,
                    };
                    var tapGestureRecognizer = new TapGestureRecognizer { CommandParameter = param, Command = ClickTableFunctionCommand };
                    frame.GestureRecognizers.Add(tapGestureRecognizer);
                    var label = new Label
                    {
                        Text = !String.IsNullOrEmpty(param.CheckValue) ? param.CheckValue : "",//param.TypeValue,
                        VerticalOptions = LayoutOptions.Fill,
                        HorizontalOptions = LayoutOptions.Fill,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        TextColor = Color.Black,
                    };
                    frame.Content = label;

                    FormGridLayout.Children.Add(frame, param.ColumnIndex, param.RowIndex + 1);
                }
            }
        }
        public static Task<string> ReadTableFunctionTypePopup(TableFunctionModel item)
        {
            var vm = new TableFunctionTypeViewModel();
            //vm.FilterList = Source;
            var tcs = new TaskCompletionSource<string>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                var page = new TableFunctionTypePopup(vm, item);
                await PopupNavigation.PushAsync(page);
                var value = await vm.GetValue();
                await PopupNavigation.PopAsync(true);
                tcs.SetResult(value);
            });
            return tcs.Task;
        }
        public void SaveTypeFunctionChanges(TableFunctionModel updatedItem)
        {
            try
            {
                string strSQL = string.Empty;

                string VALUETYPE = updatedItem.itemNo.Split('_')[0].ToUpper();
                switch (VALUETYPE)
                {
                    case "DATETIME":
                        {
                            DateTime ISdate = DateTime.ParseExact(updatedItem.CheckValue, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            strSQL = @"UPDATE T_TAG_SHEET_ANSWER SET  [isDate] = '" + ISdate.Ticks + "', [completedBy]= '" + Settings.CompletionUserName + "', [completedOn]= '" + DateTime.Now.Ticks
                                                        + "', [IsSynced] = 0 WHERE [checkSheetName] = '" + updatedItem.CheckSheet + "'" + " AND [itemno] = '" + updatedItem.itemNo + "' AND [tagName] = '" + updatedItem.TagName + "'";
                        }
                        break;
                    //case "BOOLEAN":
                    //    {
                    //        string isChecked = updatedItem.CheckValue;// == "Yes" ? "yes" : updatedItem.CheckValue == "No" ? "no" : "N/A";
                    //        strSQL = @"UPDATE T_TAG_SHEET_ANSWER SET  [isChecked] = '" + isChecked + "', [completedBy]= '" + Settings.CompletionUserName + "', [completedOn]= '" + DateTime.Now.Ticks
                    //                                    + "', [IsSynced] = 0 WHERE [checkSheetName] = '" + updatedItem.CheckSheet + "'" + " AND [itemno] = '" + updatedItem.itemNo + "' AND [tagName] = '" + updatedItem.TagName + "'";
                    //    }
                    //    break;
                    default:
                        {
                            //updatedItem.CheckValue = updatedItem.CheckValue == "N/A" ? "NA" : updatedItem.CheckValue;
                            strSQL = @"UPDATE T_TAG_SHEET_ANSWER SET  [checkValue] = '" + updatedItem.CheckValue + "', [completedBy]= '" + Settings.CompletionUserName + "', [completedOn]= '" + DateTime.Now.Ticks
                                                  + "', [IsSynced] = 0 WHERE [checkSheetName] = '" + updatedItem.CheckSheet + "'" + " AND [itemno] = '" + updatedItem.itemNo + "' AND [tagName] = '" + updatedItem.TagName + "'";
                        }
                        break;
                }

                conn.Query<T_TAG_SHEET_ANSWER>(strSQL);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
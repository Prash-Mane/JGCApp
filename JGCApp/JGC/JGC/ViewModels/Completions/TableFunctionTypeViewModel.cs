using JGC.Models.Completions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace JGC.ViewModels.Completions
{
   public class TableFunctionTypeViewModel : BindableBase
    {
        private TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
        #region properties

        private string tableFunctionComment;

        public string TableFunctionComment
        {
            get { return tableFunctionComment; }
            set { tableFunctionComment = value; RaisePropertyChanged(); }
        }
        private DateTime tableFunctionCommentDate;

        public DateTime TableFunctionCommentDate
        {
            get { return tableFunctionCommentDate; }
            set { tableFunctionCommentDate = value; RaisePropertyChanged(); }
        }
        
        private string tableFunctionBoolean;

        public string TableFunctionBoolean
        {
            get { return tableFunctionBoolean; }
            set { tableFunctionBoolean = value; RaisePropertyChanged(); }
        }
        private TableFunctionModel tableFunctionData;
        public TableFunctionModel TableFunctionData
        {
            get { return tableFunctionData; }
            set { tableFunctionData = value; RaisePropertyChanged(); }
        }
        

        #endregion

        #region Commands 
        public ICommand OKCommand
        {
            get
            {
                return new Command(() => {
                    string VALUETYPE = TableFunctionData.TypeValue.ToLower().Contains("date")? "DATETIME" : TableFunctionData.itemNo.Split('_')[0].ToUpper(); 
                    tcs.SetResult(VALUETYPE == "DATETIME" ? TableFunctionCommentDate.ToString("dd/MM/yyyy") : (VALUETYPE == "BOOLEAN" || !String.IsNullOrEmpty(TableFunctionData.AnswerOptions)) ? TableFunctionBoolean : TableFunctionComment);
                });
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
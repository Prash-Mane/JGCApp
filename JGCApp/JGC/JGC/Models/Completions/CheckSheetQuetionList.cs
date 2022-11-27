using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace JGC.Models.Completions
{
    public class CheckSheetQuetionList
    {
        public string srNo { get; set; }
        public string id { get; set; }
        public string CheckSheet { get; set; }
        public string Quetion { get; set; }
        public string section { get; set; }
        public string itemNo { get; set; }
        public List<AnswerOptions> answerOptions { get; set; }
        public bool answerOptional { get; set; }
        public List<AdditionalFieldsModel> AdditionalFields { get; set; }
        public bool IsVisibleAnsField { get; set; }
        public bool IsVisibleTableFunction { get; set; }
        
        public List<TableFunctionModel> TableFunc_List { get; set; }
        public bool IsVisibleImagefield { get; set; }
        public List<CctrImage> CCTRImageList { get; set; }
        public string AnswerType { get; set; }
    }
    public class AnswerOptions
    {
        public string id { get; set; }
        public string CheckSheet { get; set; }
        public string itemNo { get; set; }
        public string Options { get; set; }
        public string isSelected { get; set; }

        public override string ToString()
        {
            return Options.ToString();
        }
    }
    public class AdditionalFieldsModel
    {
        private string fieldPlaceHolder;
        public string ID { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string FieldPlaceHolder
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FieldValue) || FieldValue == "") return fieldPlaceHolder + " Here";
                else
                    return FieldValue;
            }

            set { fieldPlaceHolder = value; }
        }
        public bool IsEnabledField { get; set; }
        public List<AnswerOptions> answerOptions { get; set; }
        public bool IsVisibleAnsField { get; set; }
    }
    public class CctrImage
    {
        public string ID { get; set; }
        public ImageSource CCtrimg { get; set; }
    }
}

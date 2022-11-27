using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Completions
{
    public class TableFunctionModel
    {
        public string id { get; set; }
        public string CheckSheet { get; set; }
        public string Description { get; set; }
        public string TypeValue { get; set; }
        public string section { get; set; }
        public string itemNo { get; set; }
        public string CheckValue { get; set; }
        public string ProjectName { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public string TagName { get; set; }
        public string AnswerOptions { get; set; }
    }
}

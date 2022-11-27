using JGC.DataBase.DataTables.Completions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Completions
{
    public class TagSheetAnswers
    {
        public string checkSheetName { get; set; }
        public string tagName { get; set; }
        public string jobPack { get; set; }
        public string ccmsHeaderId { get; set; }
        public object cctrHeaderID { get; set; }
        public T_TAG_SHEET_HEADER tagSheetHeaders { get; set; }
        public List<T_SignOffHeader> signOffHeaders { get; set; }
        public List<object> answers { get; set; }
    }
}

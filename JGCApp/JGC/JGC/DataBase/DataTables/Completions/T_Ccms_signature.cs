using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_Ccms_signature")]
   public class T_Ccms_signature
    {
     
        public string ID { get; set; }
        public string signatureName { get; set; }
        public string CompanyCategoryCode { get; set; }
        public string FunctionCode { get; set; }
        public string SectionCode { get; set; }
        public string ProjectName { get; set; }
        public string ITR { get; set; }
    }
}

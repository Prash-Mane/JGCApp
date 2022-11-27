using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_PunchComponent")]
    public class T_PunchComponent
    {
        public string fwbs { get; set; }
        public string punch_level1 { get; set; }
        public string punch_level2 { get; set; }
        public string punch_level3 { get; set; }
        public string punch_category_code { get; set; }
        public string category_code { get; set; }
        public string section_code { get; set; }
        public string Jobcode { get; set; }
        public string ModelName { get; set; }

    }
}

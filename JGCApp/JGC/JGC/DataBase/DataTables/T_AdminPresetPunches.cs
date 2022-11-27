using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_AdminPresetPunches")]
    public class T_AdminPresetPunches
    {
        public long ProjectID { get; set; }
        public string PunchType { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string FunctionCode { get; set; }
        public string CategoryCode { get; set; }
        public string Remarks { get; set; }
        public string PresetPunchID { get; set; }
    }
}

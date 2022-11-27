using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
     [Table("T_IwpPresetPunch")]
    public class T_IwpPresetPunch
    {
        public long ProjectID { get; set; }
        public string FWBS { get; set; }
        public string PunchType { get; set; }
        public string ComponentCategory { get; set; }
        public string Component { get; set; }
        public string Action { get; set; }
        public string PresetPunchID { get; set; }
        public string PunchCategory { get; set; }
        public string CompanyCategoryCode { get; set; }
        public string FunctionCode { get; set; }
        public string SectionCode { get; set; }
    }
}

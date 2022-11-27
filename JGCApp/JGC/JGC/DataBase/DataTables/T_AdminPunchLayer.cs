using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_AdminPunchLayer")]
    public class T_AdminPunchLayer
    {
        [PrimaryKey, Indexed]
        public int ID { get; set; }
        public long ProjectID { get; set; }
        public int LayerNo { get; set; }
        public string Prefix { get; set; }
        public string CompanyCategoryCode { get; set; }
        public string FunctionCode { get; set; }
        public string PreparedConfirmedDisplay { get; set; }
        public string CompletedDisplay { get; set; }
        public string LayerName { get; set; }
        public string DisciplineCode { get; set; }
        public string IssuerCode { get; set; }
        public string Discipline { get; set; }
        public bool PreLineCheck { get; set; }
        public bool OfficialLineCheck { get; set; }
        public override string ToString()
        {
            return LayerNo + " - " + LayerName.ToString() + (OfficialLineCheck ? " (Official Line Check)" : (PreLineCheck ? " (Pre-Line Check)" : ""));
        }
    }
}

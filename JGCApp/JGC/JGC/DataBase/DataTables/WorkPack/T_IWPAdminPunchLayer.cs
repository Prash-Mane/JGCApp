using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_IWPAdminPunchLayer")]
    public class T_IWPAdminPunchLayer
    {
        [PrimaryKey, Indexed]
        public int ID { get; set; }
        public long ProjectID { get; set; }
        public int LayerNo { get; set; }
        public string LayerName { get; set; }
        public string Prefix { get; set; }
        public string IssuerCode { get; set; }
        public string CompanyCategoryCode { get; set; }
        public string FunctionCode { get; set; }
        public string PreparedConfirmedDisplay { get; set; }
        public string CompletedDisplay { get; set; }
        public override string ToString()
        {
            return LayerNo + " - " + LayerName.ToString();
        }
    }
}

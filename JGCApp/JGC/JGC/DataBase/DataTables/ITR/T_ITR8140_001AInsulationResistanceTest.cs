using SQLite;
using System;
using System.Collections.Generic;
using System.Text;


namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR8140_001AInsulationResistanceTest")]
    public class T_ITR8140_001AInsulationResistanceTest
    {
        
        [PrimaryKey]
        public long RowID { get; set; }
        public long CommonRowID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public int ID { get; set; }
        public long ITRCommonID { get; set; }
        public string Phase1 { get; set; }
        public string Phase2 { get; set; }
        public string Phase3 { get; set; }
        public string InsulationResistance1 { get; set; }
        public string InsulationResistance2 { get; set; }
        public string InsulationResistance3 { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqInsulationResistance1 { get; set; }
        [Ignore]
        public bool IsReqInsulationResistance2 { get; set; }
        [Ignore]
        public bool IsReqInsulationResistance3 { get; set; }
    }
}

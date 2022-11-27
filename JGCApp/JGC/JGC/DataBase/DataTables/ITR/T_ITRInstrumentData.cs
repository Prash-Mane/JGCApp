using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITRInstrumentData")]
    public class T_ITRInstrumentData
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public int ID { get; set; }
        public long ITRCommonID { get; set; }
        public int CCMS_EquipmentID { get; set; }
        public string ModelName { get; set; }
        public long CommonRowID { get; set; }
        public int CCMS_HEADERID { get; set; }
        [Ignore]
        public int No { get; set; }
        [Ignore]
        public List<string> TestEquipmentDataList { get; set; }
        [Ignore]
        public string TestEquipment { get; set; }
        [Ignore]
        public bool IsDeletable { get; set; }
        [Ignore]
        public bool IsReqCCMS_EquipmentID { get; set; }
    }
}

using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITRRecords_040A_041A_042A")]
    public class T_ITRRecords_040A_041A_042A
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public int ID { get; set; }
        public long ITRCommonID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string NumberofCore { get; set; }
        public string CableCoreSize { get; set; }
        public string CableCoreSizeUnit { get; set; }
        public string RatedConductorVoltage { get; set; }
        public string RatedConductorVoltageUnit { get; set; }
        public string TestVoltage { get; set; }
        public string AfiNo { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqNumberofCore { get; set; }
        [Ignore]
        public bool IsReqCableCoreSize { get; set; }
        [Ignore]
        public bool IsReqCableCoreSizeUnit { get; set; }
        [Ignore]
        public bool IsReqRatedConductorVoltage { get; set; }
        [Ignore]
        public bool IsReqRatedConductorVoltageUnit { get; set; }
    }
}

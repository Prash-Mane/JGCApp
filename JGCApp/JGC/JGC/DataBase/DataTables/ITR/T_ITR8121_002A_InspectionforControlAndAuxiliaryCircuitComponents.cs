using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents")]
    public class T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public long CommonRowID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public int id { get; set; }
        public int No { get; set; }
        public string DeviceName { get; set; }
        public string WiringCheck { get; set; }
        public string Remarks { get; set; }
        public int ID_ITR_8121_002A_InspectionforControl { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqDeviceName { get; set; }
        [Ignore]
        public bool IsReqWiringCheck { get; set; }
    }
}

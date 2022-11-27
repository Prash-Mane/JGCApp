using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR_8140_004A_Records")]
    public class T_ITR_8140_004A_Records
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public int ID { get; set; }
        public long ITRCommonID { get; set; }
        public string ITRNumber { get; set; }
        public string TagNo { get; set; }
        public string AfiNo { get; set; }
        public string Linetoearth { get; set; }
        public string IncomerAL1L2 { get; set; }
        public string IncomerAL2L3 { get; set; }
        public string IncomerAL3L1 { get; set; }
        public string IncomerAL1N { get; set; }
        public string IncomerAL2N { get; set; }
        public string IncomerAL3N { get; set; }
        public string IncomerAPhaseRotation { get; set; }
        public string IncomerBL1L2 { get; set; }
        public string IncomerBL2L3 { get; set; }
        public string IncomerBL3L1 { get; set; }
        public string IncomerBL1N { get; set; }
        public string IncomerBL2N { get; set; }
        public string IncomerBL3N { get; set; }
        public string IncomerBPhaseRotation { get; set; }
        public string Instrument { get; set; }
        public string InstrumentSerialNo { get; set; }
        public DateTime InstrumentCalibrationDate { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string AppliedTestVolt { get; set; }
        public string AmbientTemp { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
    }
}

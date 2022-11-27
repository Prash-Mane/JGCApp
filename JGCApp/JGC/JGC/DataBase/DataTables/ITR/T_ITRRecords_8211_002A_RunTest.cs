using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITRRecords_8211_002A_RunTest")]
    public class T_ITRRecords_8211_002A_RunTest
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public int ID { get; set; }
        public long CommonRowID { get; set; }
        public long ITRCommonID { get; set; }
        public string AmbientTemp { get; set; }
        public string RunCurrentL1 { get; set; }
        public string RunCurrentL2 { get; set; }
        public string RunCurrentL3 { get; set; }
        public string BearingTempDE { get; set; }
        public string BearingTempNDE { get; set; }
        public string WindTemp1 { get; set; }
        public string WindTemp2 { get; set; }
        public string WindTempL3 { get; set; }
        public string VibraDriveVer { get; set; }
        public string VibraDriveHori { get; set; }
        public string VibraDriveAxis { get; set; }
        public string VibraNonDriveVer { get; set; }
        public string VibraNonDriveHori { get; set; }
        public string VibraNonDriveAxis { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqAmbientTemp { get; set; }
        [Ignore]
        public bool IsReqRunCurrentL1 { get; set; }
        //[Ignore]
        //public bool IsReqRunCurrentL2 { get; set; }
        //[Ignore]
        //public bool IsReqRunCurrentL3 { get; set; }
        [Ignore]
        public bool IsReqBearingTempDE { get; set; }
        [Ignore]
        public bool IsReqBearingTempNDE { get; set; }
        //[Ignore]
        //public bool IsReqWindTemp1 { get; set; }
        //[Ignore]
        //public bool IsReqWindTemp2 { get; set; }
        //[Ignore]
        //public bool IsReqWindTempL3 { get; set; }
        //[Ignore]
        public bool IsReqVibraDriveVer { get; set; }
        [Ignore]
        public bool IsReqVibraDriveHori { get; set; }
        [Ignore]
        public bool IsReqVibraDriveAxis { get; set; }
        [Ignore]
        public bool IsReqVibraNonDriveVer { get; set; }
        [Ignore]
        public bool IsReqVibraNonDriveHori { get; set; }
        [Ignore]
        public bool IsReqVibraNonDriveAxis { get; set; }
    }
}

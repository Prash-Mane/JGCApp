using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITRRecords_8211_002A_Body")]
    public class T_ITRRecords_8211_002A_Body
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public int ID { get; set; }
        public long ITRCommonID { get; set; }
        public string MotorRatedPower { get; set; }
        public string RatedVolt { get; set; }
        public string FuncionTest { get; set; }
        public string DirectionOfRotation { get; set; }
        public string Inch { get; set; }
        public string InsulRes { get; set; }
        public string StartingCurrent { get; set; }
        public string RotationSpeed { get; set; }
        public string SpaceHeater { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string Pass_Fail { get; set; }
        public string Ambient_Temp { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        public string AnalogSignal { get; set; }
        [Ignore]
        public bool IsReqMotorRatedPower { get; set; }
        [Ignore]
        public bool IsReqRatedVolt { get; set; }
        [Ignore]
        public string IsReqInch { get; set; }
        [Ignore]
        public bool IsReqStartingCurrent { get; set; }
        [Ignore]
        public bool IsReqRotationSpeed { get; set; }
        [Ignore]
        public bool IsCW { get; set; }
        [Ignore]
        public bool IsCCW { get; set; }
        [Ignore]
        public bool IsReqPass_Fail { get; set; }
        [Ignore]
        public bool IsReqAmbient_Temp { get; set; }
        [Ignore]
        public List<string> FuncionTestOptionsList { get; set; }
        [Ignore]
        public List<string> DirectionOfRotationOptionsList { get; set; }
        [Ignore]
        public List<string> SpaceHeaterOptionsList { get; set; }

    }
}

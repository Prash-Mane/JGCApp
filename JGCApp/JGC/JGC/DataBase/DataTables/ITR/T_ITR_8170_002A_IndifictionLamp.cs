using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR_8170_002A_IndifictionLamp")]
    public class T_ITR_8170_002A_IndifictionLamp
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public int ID { get; set; }
        public long CommonRowID { get; set; }
        public long ITRCommonID { get; set; }
        public string IncomerABpoweronLamp { get; set; }
        public string IncomerABpoweronOperatedOrNot { get; set; }
        public string IncomerABpoweronRemark { get; set; }
        public string IncomerABopenLamp { get; set; }
        public string IncomerABopenOperatedOrNot { get; set; }
        public string IncomerABopenRemark { get; set; }
        public string IncomerABclosedLamp { get; set; }
        public string IncomerABclosedOperatedOrNot { get; set; }
        public string IncomerABclosedRemark { get; set; }
        public string IncomerABtripLamp { get; set; }
        public string IncomerABtripOperatedOrNot { get; set; }
        public string IncomerABtripRemark { get; set; }
        public string VoltmeterforIncomerABOperatedOrNot { get; set; }
        public string VoltmeterforIncomerABRemark { get; set; }
        public string AmmeterforIncomerOperatedOrNot { get; set; }
        public string AmmeterforIncomerRemark { get; set; }
        public string DoorlightOperatedOrNot { get; set; }
        public string DoorlightRemark { get; set; }
        public string SpaceHeaterOperatedOrNot { get; set; }
        public string SpaceHeaterRemark { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqIncomerABpoweronLamp { get; set; }
        [Ignore]
        public bool IsReqIncomerABpoweronOperatedOrNot { get; set; }
        [Ignore]
        public bool IsReqIncomerABopenLamp { get; set; }
        [Ignore]
        public bool IsReqIncomerABopenOperatedOrNot { get; set; }
        [Ignore]
        public bool IsReqIncomerABclosedLamp { get; set; }
        [Ignore]
        public bool IsReqIncomerABclosedOperatedOrNot { get; set; }
        [Ignore]
        public bool IsReqIncomerABtripLamp { get; set; }
        [Ignore]
        public bool IsReqIncomerABtripOperatedOrNot { get; set; }
        [Ignore]
        public bool IsReqVoltmeterforIncomerABOperatedOrNot { get; set; }
        [Ignore]
        public bool IsReqAmmeterforIncomerOperatedOrNot { get; set; }
        [Ignore]
        public bool IsReqDoorlightOperatedOrNot { get; set; }
        [Ignore]
        public bool IsReqSpaceHeaterOperatedOrNot { get; set; }
    }
}

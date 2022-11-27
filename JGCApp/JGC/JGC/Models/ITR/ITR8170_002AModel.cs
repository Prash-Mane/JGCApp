using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR8170_002AModel
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public List<T_ITRRecords_8170_002A_Voltage_Reading> ITR_8170_002A_Voltage_Reading { get; set; }
        public T_ITR_8170_002A_IndifictionLamp ITR_8170_002A_IndifictionLamp { get; set; }
        public T_ITR_8170_002A_InsRes ITR_8170_002A_InsRes { get; set; }
    }
}

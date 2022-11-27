using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR8100_002AModel
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public T_ITRRecords_8100_002A ITR8100_002AVT_Data { get; set; }
        public List<T_ITRRecords_8100_002A_InsRes_Test> ITR8100_002AInsRes_Test { get; set; }
        public List<T_ITRRecords_8100_002A_Radio_Test> ITR8100_002ARadio_Test { get; set; }
    }
}

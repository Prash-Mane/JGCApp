using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR7000_040A_041A_042AModel
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public T_ITRRecords_040A_041A_042A Records_40A_41A_042A { get; set; }
        public List<T_ITRInsulationDetails> InsulationDetails { get; set; }
    }
}

using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR7000_030A_031AModel
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public T_ITRRecords_30A_31A Records_30A_31A { get; set; }
        public List<T_ITRTubeColors> TubeColors { get; set; }
    }
}

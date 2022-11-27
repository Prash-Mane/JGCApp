using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR8000_101AModel
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public T_ITR8000_101A_Generalnformation ITR_8000_101A_Generalnformation { get; set; }
        public T_ITR8000_101A_RecordISBarrierDetails ITR_8000_101A_RecordISBarrierDetails { get; set; }
    }
}

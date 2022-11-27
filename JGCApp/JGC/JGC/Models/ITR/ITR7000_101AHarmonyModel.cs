using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR7000_101AHarmonyModel
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public T_ITR_7000_101AHarmony_Genlnfo ITR_7000_101AHarmony_Genlnfo { get; set; }
        public T_ITR_7000_101AHarmony_ActivityDetails ITR_7000_101AHarmony_ActivityDetails { get; set; }
    }
}

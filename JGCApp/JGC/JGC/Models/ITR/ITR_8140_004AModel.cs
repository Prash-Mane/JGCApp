using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR_8140_004AModel
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public T_ITR_8140_004A_Records _8140_004A_Record { get; set; }
    }
}

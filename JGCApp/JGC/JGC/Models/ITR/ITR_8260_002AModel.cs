using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR_8260_002AModel
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public List<T_ITR_8260_002A_DielectricTest> ITR_DielectricTestList { get; set; }
        public T_ITR_8260_002A_Body ITR_Body { get; set; }
    }
}

using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR8211_002AModel
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public T_ITRRecords_8211_002A_Body ITR_8211_002A_Body { get; set; }
        public List<T_ITRRecords_8211_002A_RunTest> ITR_8211_002A_RunTest { get; set; }
    }
}

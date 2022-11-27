using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR8161_001AModel
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public T_ITRRecords_8161_001A_Body ITR_8161_001A_Body { get; set; }
        public List<T_ITRRecords_8161_001A_InsRes> ITR_8161_001A_InsRes { get; set; }
        public List<T_ITRRecords_8161_001A_ConRes> ITR_8161_001A_ConRes { get; set; }
    }
}

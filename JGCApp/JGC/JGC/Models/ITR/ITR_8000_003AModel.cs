using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR_8000_003AModel
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public T_ITR8000_003ARecords ITR_8000_003ARecords { get; set; }
        public List<T_ITR8000_003A_AcceptanceCriteria> ITR_8000_003A_AcceptanceCriteria { get; set; }
    }
}

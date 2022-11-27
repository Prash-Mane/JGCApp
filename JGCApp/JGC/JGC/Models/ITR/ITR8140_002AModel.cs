using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR8140_002AModel
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public T_ITR_8140_002A_Records _8140_002A_Record { get; set; }
        public T_ITR_8140_002A_RecordsMechnicalOperation _8140_002A_RecordsMechnical { get; set; }
        public T_ITR_8140_002A_RecordsAnalogSignal _8140_002A_RecordsAnalogSignal { get; set; }
    }
}

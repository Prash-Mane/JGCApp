using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR8121_004AModel
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public List<T_ITR8121_004AInCAndAuxiliary> ITR8121_004A_InispactionForControlAndAuxiliary { get; set; }
        public List<T_ITR8121_004ATransformerRatioTest> ITR8121_004A_TransformerRatioTest { get; set; }
        public T_ITR8121_004ATestInstrumentData ITR8121_004A_TestInstrumentData { get; set; }
    }
}

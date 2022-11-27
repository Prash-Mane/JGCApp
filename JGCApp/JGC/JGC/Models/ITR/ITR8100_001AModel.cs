using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR8100_001AModel
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public List<T_ITR8100_001A_CTdata> ITR_CTdata { get; set; }
        public List<T_ITR8100_001A_InsulationResistanceTest> ITR_InsulationResistanceTest { get; set; }
        public List<T_ITR8100_001A_RatioTest> ITR_RatioTest { get; set; }
        public T_ITR8100_001A_TestInstrumentData ITR_TestInstrumentData { get; set; }
    }
}

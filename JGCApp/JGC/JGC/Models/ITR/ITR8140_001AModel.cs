using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR8140_001AModel
    {

        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public List<T_ITR8140_001A_ContactResisTest> iTR_8140_001A_ContactResistanceTests { get; set; }
        public List<T_ITR8140_001AInsulationResistanceTest> iTR8140_001A_InsulationResistanceTest { get; set; }
        public List<T_ITR8140_001ADialectricTest> iTR8140_001A_Dilectric_Test { get; set; }
        public T_ITR8140_001ATestInstrucitonData iTR8140_001A_TestInstrumentData { get; set; }
    }
}

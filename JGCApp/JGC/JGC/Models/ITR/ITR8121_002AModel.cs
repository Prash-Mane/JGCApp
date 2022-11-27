using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR8121_002AModel 
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public List<T_ITR8121_002A_InspectionforControlAndAuxiliaryCircuitComponents> InspectionControlAndACCs { get; set; }
        public List<T_ITR8121_002A_TransformerRadioTest> TransformerRadioTests { get; set; }
        public T_ITR8121_002A_Records ITR_8121_002A_Records { get; set; }
    }
}

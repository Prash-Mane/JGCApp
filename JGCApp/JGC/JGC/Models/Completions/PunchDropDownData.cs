using JGC.DataBase.DataTables.Completions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Completions
{
    public class PunchDropDownData
    {
        public List<T_PunchComponent> punchComponent { get; set; }
        public Dictionary<String, String> system { get; set; }
        public List<string> fwbs { get; set; }
        public List<string> pcwbs { get; set; }
    }
}

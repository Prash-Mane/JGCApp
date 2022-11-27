using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Completions
{
    public class InitialDataModel
    {
        public List<WorkpackModel> workpacks { get; set; }
        public List<SystemModel> systems { get; set; }
        public List<string> fwbs { get; set; }
        public List<string> pcwbs { get; set; }
    }
}

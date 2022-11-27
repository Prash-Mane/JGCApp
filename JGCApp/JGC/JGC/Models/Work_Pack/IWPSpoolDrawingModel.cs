using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Work_Pack
{
    public class IWPSpoolDrawingModel
    {
        public int CWPID { get; set; }
        public int OrderNo { get; set; }
        public string DisplayName { get; set; }
        public string StatusImage { get; set; }
        public int PunchCount { get; set; }
        public string MaxStatus { get; set; }
        public int VMHubID { get; set; }
        
    }
}

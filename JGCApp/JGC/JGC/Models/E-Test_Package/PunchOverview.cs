using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.E_Test_Package
{
    public class PunchOverview
    {
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public string PunchID { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string StatusImage { get; set; }
        public bool Updated { get; set; }
        public int PunchAdminID { get; set; }
        public string Category { get; set; }
        public string LayerName { get; set; }
        public bool IsPID { get; set; }
    }
}

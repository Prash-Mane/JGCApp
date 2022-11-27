using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.E_Test_Package
{
    public class PunchListModel
    {
        public string PunchID { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public bool HasImages { get; set; }
        public string Camera { get; set; }
        public bool Delete { get; set; }   
        public int PunchAdminID { get; set; }
        public string PunchIDColor { get; set; }
        public string StatusColor { get; set; }
        public bool TPCConfirmed { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Work_Pack
{
    public class IWPPunchListModel
    {
        public int ID { get; set; }
        public string PunchID { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public bool HasImages { get; set; }
        public string Camera { get; set; }
        public string Delete { get; set; }
        public int PunchAdminID { get; set; }
        public string PunchIDColor { get; set; }
        public string StatusColor { get; set; }
        public string Description { get; set; }
        public long VMHub_DocumentsID { get; set; }
        public string TagsOrOtherComponent { get; set; }
        public int CWPID { get; set; }

    }
}

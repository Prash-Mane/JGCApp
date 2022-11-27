using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Work_Pack
{
    public class TagMilestoneStatusModel
    {
        public override string ToString()
        {
            return TagNo;
        }
        public int Project_ID { get; set; }
        public int IWPID { get; set; }
        public int CWPTagID { get; set; }
        public string TagNo { get; set; }
        public string Milestone { get; set; }
        public string MilestoneAttribute { get; set; }
        public string UpdatedByUserID { get; set; }
       // public string SignedBy { get; set; }
        public DateTime? StatusValue { get; set; }
        public bool SignedInVM { get; set; }
        public bool IsUpdated { get; set; }
        public string CameraImage { get; set; }
        public string TagMember { get; set; }
        public bool SignedInCMS { get; set; }

        private string _milestoneAtri;  // the name field
        public string MilestoneAtri    // the Name property
        {
            get { if (_milestoneAtri == null) return MilestoneAttribute; else return _milestoneAtri; }
            set => _milestoneAtri = value;
           // set =>value = MilestoneAtri;
        }
        private bool _IsSinged;  // the name field
        public bool IsSinged    // the Name property
        {
            get { if (SignedInVM || SignedInCMS) return true; else return false; }
            set => _IsSinged = value;
            // set =>value = MilestoneAtri;
        }

    }    
}

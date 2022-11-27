using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Work_Pack
{
    public class IWPControlLogModel
    {
        public int SrNo { get; set; }
        public int ID { get; set; }
        public bool Milestone { get; set; }
        public bool Signed { get; set; }
        public bool Live { get; set; }
        public int SignatureNo { get; set; }
        public string SignedImage { get; set; }
        public string SignedBy { get; set; }
        public DateTime SignedOn { get; set; }
        public string SignatureName { get; set; }
        public bool Reject { get; set; }
        public string RejectComment { get; set; }
        public string RejectedBy { get; set; }
        public DateTime RejectedOn { get; set; }
        public string DisciplineDisplay { get; set; }
    }
}

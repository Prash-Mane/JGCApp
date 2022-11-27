using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.E_Test_Package
{
  public  class ControlLogSignature
    {
        public int ControlLogAdminID { get; set; }
        public Boolean Signed { get; set; }
        public int SignedByUserID { get; set; }
        public string SignedBy { get; set; }
        public DateTime SignedOn { get; set; }
        public Boolean Reject { get; set; }
        public int RejectedByUserID { get; set; }
        public string RejectedBy { get; set; }
        public DateTime RejectedOn { get; set; }
        public string RejectComment { get; set; }
    }
}

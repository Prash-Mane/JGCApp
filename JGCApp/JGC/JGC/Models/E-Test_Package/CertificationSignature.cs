using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.E_Test_Package
{
   public class CertificationSignature
    {
        public int AdminID { get; set; }
        public Boolean Signed { get; set; }
        public int SignedByUserID { get; set; }
        public string SignedBy { get; set; }
        public DateTime SignedOn { get; set; }
    }
}

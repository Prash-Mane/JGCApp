using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.E_Test_Package
{
   public class RecordAcceptedBy
    {
        public int ID { get; set; }
        public bool Signed { get; set; }
        public bool Live { get; set; }
        public int OrderNo { get; set; }
        public string SignedImage { get; set; }
        public string Description { get; set; }
        public string SignedBy { get; set; }
        public DateTime SignedOn { get; set; }
    }
}

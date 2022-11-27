using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.E_Test_Package
{
    public class UploadResult 
    {
        public string Error;
        public Boolean Success { get; set; }
        public int NewID { get; set; }
        public string NewName { get; set; }
        public string Message { get; set; }
    }
}

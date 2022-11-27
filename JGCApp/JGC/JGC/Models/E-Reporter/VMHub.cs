using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models
{
    public class VMHub 
    {
        public long ID { get; set; }
        public string Module { get; set; }
        public string LinkedID { get; set; }
        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public string Comment { get; set; }
        public string Extension { get; set; }
        public string FileSize { get; set; }
        public string FileBytes { get; set; }
        public string Error { get; set; }
    }
}

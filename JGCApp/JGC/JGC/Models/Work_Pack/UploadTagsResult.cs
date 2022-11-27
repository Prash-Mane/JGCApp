using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Work_Pack
{
    public class UploadTagsResult
    {
        public string Error { get; set; }
        public bool Success { get; set; }
        public int NewID { get; set; }
        public string NewName { get; set; }
        public string Message { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Completions
{
    public class VMHubPunch
    {
        public string Error { get; set; }
        public string ref1 { get; set; }
        public string ref2 { get; set; }
        public string ref3 { get; set; }
        public string ref4 { get; set; }
        public string ref5 { get; set; }
        public string reference { get; set; }
        public string url { get; set; }
        public string local { get; set; }
        public string modelname { get; set; }
        public string createdby { get; set; }
        public DateTime createdon { get; set; }
        public string chksum { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Completions
{
    public class DataRef
    {
        public string id { get; set; }
        public string ref1 { get; set; }
        public string ref2 { get; set; }
        public string ref3 { get; set; }
        public string ref4 { get; set; }
        public string ref5 { get; set; }
        public string ref6 { get; set; }
        public string location { get; set; }
        public string attDesc { get; set; }
        public string refType { get; set; }
        public string modelName { get; set; }
        public string workPack { get; set; }
        public string name { get { return ref1; } }


    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Completions
{
    public class SystemModel
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
        public object workPack { get; set; }
        public object error { get; set; }
        public string name
        {
            get
            {
                if ("SYSTEM" == refType)
                    return ref1;
                else
                    return ref2;
            }
        }
    }
}

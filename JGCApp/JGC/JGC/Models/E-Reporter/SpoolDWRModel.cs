using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.E_Reporter
{
    public class SpoolDWRModel
    {
        public bool Selected { get; set; }
        public string SpoolDWR { get; set; }
        public override string ToString()
        {
            return SpoolDWR;
        }
    }
}

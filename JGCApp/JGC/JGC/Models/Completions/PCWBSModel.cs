using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Completions
{
    public class PCWBSModel
    {
        public string pcwbs { get; set; }
        public string name { get { return pcwbs; } }
    }
}

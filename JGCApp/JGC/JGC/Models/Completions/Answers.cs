using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Completions
{
    public class Answers
    {
        public string itemno { get; set; }
        public string checkValue { get; set; }
        public string isChecked { get; set; }
        public long isDate { get; set; }
        public string completedBy { get; set; }
        public long completedOn { get; set; }
        public bool synced { get; set; }
        public string jobPack { get; set; }

    }
}

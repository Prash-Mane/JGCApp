using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Completions
{
    public class WorkpackModel
    {
        public string name { get; set; }
        public bool inReview { get; set; }
        public List<string> reviewers { get; set; }
        public bool isHopper { get; set; }
    }
}

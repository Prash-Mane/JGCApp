using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Completions
{
    public class TagsModel
    {
        public string ID { get; set; }
        public string tags { get; set; }
        public string name { get { return tags; } }
    }
}

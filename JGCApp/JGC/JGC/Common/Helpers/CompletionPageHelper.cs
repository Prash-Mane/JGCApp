using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Common.Helpers
{
    public class CompletionPageHelper
    {
        public string CompletionToken { get; set; }
        public string CompletionTokenTimeStamp { get; set; }
        public DateTime CompletionTokenExpiry { get; set; }
        public string CompletionUnitID { get; set; }
    }
}

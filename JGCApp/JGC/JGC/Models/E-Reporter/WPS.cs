using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models
{
    public class WPS
    {
        public int Project_ID { get; set; }
        public string Wps_No { get; set; }
        public string Description { get; set; }
        public string WpsDescription {
            get { return string.Format("{0} - {1}", Wps_No, Description); } }

    }
}


using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models
{
    public class Welder
    {
        public int Project_ID { get; set; }
        public string SubContractor { get; set; }
        public string Welder_Code { get; set; }
        public string Welder_Name { get; set; }

        public string WelderDetail
        {
            get
            {
                return string.Format("{0} - {1}", Welder_Code, Welder_Name);
            }
            
        }
    }
}

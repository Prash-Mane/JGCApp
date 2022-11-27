using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models
{
    public class Defects
    {
        public string RT_Defect_Code { get; set; }
        public string Description { get; set; }

        public string DefectsShow
        {
            get
            {
                return string.Format("{0} - {1}", RT_Defect_Code, Description);
            }


        }
    }
}

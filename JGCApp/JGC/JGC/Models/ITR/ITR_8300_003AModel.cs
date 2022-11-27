using System;
using System.Collections.Generic;
using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
  public class ITR_8300_003AModel
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public List<T_ITR_8300_003A_Illumin> ITR_IlluminList { get; set; }
        public T_ITR_8300_003A_Body ITR_Body { get; set; }
    }
}

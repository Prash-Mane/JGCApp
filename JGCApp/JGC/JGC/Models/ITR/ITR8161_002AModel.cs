using JGC.DataBase.DataTables.ITR;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.ITR
{
    public class ITR8161_002AModel
    {
        public string Error { get; set; }
        public T_ITRCommonHeaderFooter CommonHeaderFooter { get; set; }
        public T_ITR8161_002A_Body Itr8161_002A_Body { get; set; }
        public List<T_ITR8161_002A_DielectricTest> ITR8161_002A_DielectricTest { get; set; }
    }
}

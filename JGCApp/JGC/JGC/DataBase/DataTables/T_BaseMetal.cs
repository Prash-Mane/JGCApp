using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_BaseMetal")]
    public class T_BaseMetal
    {
        public override string ToString()
        {
            return Base_Material;
        }
        public string Base_Material { get; set; }
        public bool Updated { get; set; }
    }
}

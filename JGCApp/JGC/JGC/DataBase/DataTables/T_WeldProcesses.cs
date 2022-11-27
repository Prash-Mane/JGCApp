using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_WeldProcesses")]
    public class T_WeldProcesses
    {
        public int ProjectID { get; set; }
        public string Weld_Process { get; set; }
        public string Description { get; set; }
        public bool Weld_Process_Group { get; set; }
        public override string ToString()
        {
            return  string.Format("{0} - {1}", Weld_Process, Description);
        }
    }
}

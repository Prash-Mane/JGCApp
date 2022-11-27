using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_TestEquipmentData")]
    public class T_TestEquipmentData
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string Description { get; set; }
        public string Instrument { get; set; }
        public string Serial { get; set; }
        public DateTime CalibrationDate { get; set; }
        public int ProjectID { get; set; }
       // public string ModelName { get; set; }
    }
}

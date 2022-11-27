using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITRInsulationDetails")]
    public class T_ITRInsulationDetails
    {
        [PrimaryKey]
        public long RowID { get; set; }
        public int ID { get; set; }
        public long ITRCommonID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public long CommonRowID { get; set; }
        public string CoreNum { get; set; }
        public string ContinuityResult { get; set; }
        public string CoretoCore { get; set; }
        public string CoretoArmor { get; set; }
        public string CoretoSheild { get; set; }
        public string ArmortoSheild { get; set; }
        public string SheidtoSheild { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqContinuityResult { get; set; }
        [Ignore]
        public bool IsReqCoretoCore { get; set; }
        [Ignore]
        public bool IsReqCoretoArmor { get; set; }
        [Ignore]
        public bool IsReqCoretoSheild { get; set; }
        [Ignore]
        public bool IsReqArmortoSheild { get; set; }
        [Ignore]
        public bool IsReqSheidtoSheild { get; set; }        
        [Ignore]
        public List<string> ContinuityResultOptionsList { get; set; }
        [Ignore]
        public List<string> CoretoCoreOptionsList { get; set; }
        [Ignore]
        public List<string> CoretoArmorOptionsList { get; set; }
        [Ignore]
        public List<string> CoretoSheildOptionsList { get; set; }
        [Ignore]
        public List<string> ArmortoSheildOptionsList { get; set; }
        [Ignore]
        public List<string> SheidtoSheildOptionsList { get; set; }
    }
}

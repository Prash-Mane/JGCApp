using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR_8300_003A_Illumin")]
    public class T_ITR_8300_003A_Illumin
    {
        [PrimaryKey]
        public long RowID { get; set; }

        public int ID { get; set; }
        public long CommonRowID { get; set; }
        public long ITRCommonID { get; set; }
        public string Area { get; set; }
      
       // private string lUX1 { get; set; }
        public string LUX1
        {
            get;
            set;
        }

       

        public string LUX2 { get; set; }
        public string LUX3 { get; set; }
        public string LUX4 { get; set; }
       // public string avg { get; set; }
        public string Avg { get; set; }
        public string Remark { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string Acceptance_Criteria { get; set; }
        public string Pass_Fail { get; set; }
        public string ModelName { get; set; }
        [Ignore]
        public bool IsReq { get; set; }
        [Ignore]
        public int serNO { get; set; }
        public bool IsUpdated { get; set; }
        //private void CalculatedAvg()
        //{

        //    int av = 0;
        //    if (!string.IsNullOrEmpty(LUX1))
        //        av += Convert.ToInt32(LUX1);
        //    if (!string.IsNullOrEmpty(LUX2))
        //        av += Convert.ToInt32(LUX2);
        //    if (!string.IsNullOrEmpty(LUX3))
        //        av += Convert.ToInt32(LUX3);
        //    if (!string.IsNullOrEmpty(LUX4))
        //        av += Convert.ToInt32(LUX4);

        //    avg = (av / 4).ToString();

        //}
    }
}

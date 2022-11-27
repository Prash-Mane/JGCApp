using SQLite;
using System.Collections.Generic;
using System;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_IWP")]
    public class T_IWP
    {
        public override string ToString()
        {
            return Title.ToString();
        }
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public string ModelName { get; set; }
        public string JobCode { get; set; }
        public string PCWBS { get; set; }
        public string FWBS { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BinaryCode { get; set; }
        public Boolean Updated { get; set; }
        [Ignore]
        public List<T_IWPStatus> IWPStatusList { get; set; }
        [Ignore]
        public List<T_Predecessor> PredecessorList { get; set; }
        [Ignore]
        public List<T_Successor> SuccessorList { get; set; }
        public string Discipline { get; set; }
        public DateTime DownloadDate { get; set; }
        public string Error { get; set; }
    }
}

using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_IWPDrawings")]
    public class T_IWPDrawings
    {
        //public override string ToString()
        //{
        //    return DisplayName;
        //}
        //public int ProjectID { get; set; }
        //public int IWPID { get; set; }
        //public int VMHubID { get; set; }
        //public int CWPID { get; set; }
        //public string DisplayName { get; set; }
        //public string FileBytes { get; set; }
        //public string Extension { get; set; }
        //public string FileName { get; set; }
        //public int FileSize { get; set; }


        public override string ToString()
        {
            if (String.IsNullOrEmpty(Sheet_No))
                return Name;
            return (Name + " " + Sheet_No + " of " + Total_Sheets);
        }
        public int ProjectID { get; set; }
        public int IWPID { get; set; }
        public string Name { get; set; }
        public string JobCode { get; set; }
        public long VMHub_DocumentsID { get; set; }
        public string FileName { get; set; }
        public string Sheet_No { get; set; }
        public string Total_Sheets { get; set; }
        public string Revision { get; set; }
        public string BinaryCode { get; set; }
        public string FileLocation { get; set; }
        public string Error { get; set; }

    }
}

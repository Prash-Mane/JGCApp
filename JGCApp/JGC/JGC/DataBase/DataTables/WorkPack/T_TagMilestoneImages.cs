using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_TagMilestoneImages")]
    public class T_TagMilestoneImages
    {
        public override string ToString()
        {
            return DisplayName;
        }
        public int Project_ID { get; set; }
        public int CWPID { get; set; }
        public string Milestone { get; set; }
        public string MilestoneAttribute { get; set; }
        public string DisplayName { get; set; }
        public string FileBytes { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public bool Updated { get; set; }
        public string TagMember { get; set; }
    }
}

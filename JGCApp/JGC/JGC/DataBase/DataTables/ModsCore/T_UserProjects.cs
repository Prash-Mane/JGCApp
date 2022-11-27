using SQLite;

namespace JGC.DataBase.DataTables.ModsCore
{
    [Table("T_UserProjects")]
    public class T_UserProjects
    {
       
        public string ID { get; set; }
        public string User_ID { get; set; }
        public string ProjectName { get; set; }
        public string ModelName { get; set; }
        public string Project_ID { get; set; }
        public string JobCode { get; set; }
        public bool JIFieldVerified1 { get; set; }
        public bool JIFieldVerified2 { get; set; }
        public bool JIFieldVerified3 { get; set; }
        public bool JIFieldVerified4 { get; set; }
        public bool JIFieldVerified5 { get; set; }
        public bool JIFieldVerified6 { get; set; }
        public bool JIFieldVerified7 { get; set; }
       // public string Project_ID { get; set; }
    }
}

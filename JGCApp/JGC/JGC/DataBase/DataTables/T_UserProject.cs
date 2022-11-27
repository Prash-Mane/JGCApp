using JGC.Models;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace JGC.DataBase.DataTables
{
    [Table("T_UserProject")]
    public class T_UserProject 
    {
       // [PrimaryKey, Indexed]
        public int Project_ID { get; set; }
        public int User_ID { get; set; }
        public int ModelID { get; set; }
        public string ProjectName { get; set; }
        public string ModelName { get; set; }
        public string JobCode { get; set; }
        public string TimeZone { get; set; }
        public string WebService { get; set; }
        public string LocalWebService { get; set; }
        public string Error { get; set; }
    }    
}

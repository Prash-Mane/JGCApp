using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_CompletionsUsers")]
    public class T_CompletionsUsers
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public DateTime Expiry { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string WorkerID { get; set; }
        public string Company { get; set; }
        public string Function_Code { get; set; }
        public string Company_Code { get; set; }
        public string Company_Category_Code { get; set; }
        public string Section_Code { get; set; }
        public string Discipline_Code { get; set; }
    }
}

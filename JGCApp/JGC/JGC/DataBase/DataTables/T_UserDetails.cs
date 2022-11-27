using System;
using System.Collections.Generic;
using JGC.Models;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace JGC.DataBase.DataTables
{
    [Table("T_UserDetails")]
    public class T_UserDetails 
    {
        [PrimaryKey, Indexed]
        public int ID { get; set; }
        public string FullName { get; set; }
        public DateTime Expiry { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public bool ModsUser { get; set; }
        public string Company { get; set; }
        public bool ETP_Admin { get; set; }
        public bool ETP_SuperUser { get; set; }
        public string Function_Code { get; set; }
        public string Company_Code { get; set; }
        public string Company_Category_Code { get; set; }
        public string Discipline_Code { get; set; }
        public string WorkerID { get; set; }
        public string Section_Code { get; set; }
        [Ignore]
        public string[] FWBS_Access { get; set; }
        [Ignore]
        public List<T_UserProject> UserProjects { get; set; }

        public string Error { get; set; }

    }   

}

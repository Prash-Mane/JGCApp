using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_CompletionSystems")]
    public class T_CompletionSystems
    {
        public string attDesc { get; set; }
        public string id { get; set; }
        public string location { get; set; }
        public string modelName { get; set; }
        public string ref1 { get; set; }
        public string ref2 { get; set; }
        public string ref3 { get; set; }
        public string ref4 { get; set; }
        public string ref5 { get; set; }
        public string ref6 { get; set; }
        public string refType { get; set; }
        public string workPack { get; set; }
        public string name
        {
            get
            {
                if ("SYSTEM" == refType)
                    return ref1;
                else
                    return ref2;
            }
            set
            {

            }
        }
    }
}

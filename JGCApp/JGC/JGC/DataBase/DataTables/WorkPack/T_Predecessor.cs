﻿using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace JGC.DataBase.DataTables.WorkPack
{
    [Table("T_Predecessor")]
    public class T_Predecessor
    {
        public override string ToString()
        {
            return Title.ToString();
        }
        public int IWP_ID { get; set; }
        public int SubIWP_ID { get; set; }
        public string Title { get; set; }
        public string Error { get; set; }
    }
}

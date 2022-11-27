using JGC.DataBase.DataTables.Completions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Completions
{
    public class HandoversRootModel
    {
        public List<T_Handover> pages { get; set; }
        public List<string> associatedWorkpacks { get; set; }
    }
}

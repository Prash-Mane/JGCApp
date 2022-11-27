using JGC.DataBase.DataTables.Completions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JGC.Common.Interfaces
{
    public interface ISystemsFullSystemSyncercs
    {
        Task<bool> uploadChanges();
        Task<bool> downloadChanges(string id);
        Task<bool> downloadChanges(string values,string Column);
        Task<bool> downloadChangesByID(string systemData);
    }
}

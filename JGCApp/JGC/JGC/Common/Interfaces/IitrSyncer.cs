using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JGC.Common.Interfaces
{
    public interface IitrSyncer
    {
        Task<bool> uploadChanges(string id);
        Task<bool> downloadChanges(string id, string workpckname);
    }
}

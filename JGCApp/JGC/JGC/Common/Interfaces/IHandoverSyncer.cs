using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JGC.Common.Interfaces
{
    public interface IHandoverSyncer
    {
        Task<bool> uploadChanges(string id);
        Task<bool> downloadChangesAsync(string id);
    }
}

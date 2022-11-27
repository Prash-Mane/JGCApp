using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JGC.Common.Interfaces
{
    public interface IWorkpackFullSystemSyncer
    {
        Task<bool> uploadChanges();
        Task<bool> downloadChanges(string id);
    }
}

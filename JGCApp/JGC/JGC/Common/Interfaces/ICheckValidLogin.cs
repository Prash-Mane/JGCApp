using JGC.Common.Helpers;
using JGC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JGC.Common.Interfaces
{
    public interface ICheckValidLogin
    {
        Task<bool> GetValidToken(LoginModel loginModel);
        Task<bool> GetValidCompletionToken(LoginModel loginModel);
    }
}

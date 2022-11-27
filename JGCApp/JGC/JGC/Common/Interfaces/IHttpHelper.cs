using JGC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JGC.Common.Interfaces
{
    public interface IHttpHelper
    {
        Task<OperationResult<T>> GetAsync<T>(string requestUri, string baseUrl = null, CancellationToken cancellationToken = default(CancellationToken));
        Task<OperationResult<T>> PostAsync<T>(string requestUri, object model, string baseUrl = null, CancellationToken cancellationToken = default(CancellationToken));
        Task<OperationResult<T>> GetWithProgressAsync<T>(string requestUri, IProgress<double> progress, string baseUrl = null, CancellationToken cancellationToken = default(CancellationToken));
        Task CheckTokenAsync();
    }
}

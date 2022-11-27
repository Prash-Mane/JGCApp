using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables.Completions;
using JGC.Models.Completions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JGC.Common.Services
{
    public class WorkpackPunchListSyncer : IWorkpackPunchListSyncer
    {
        private readonly IRepository<T_CompletionsPunchList> _CompletionsPunchListRepository;

        public WorkpackPunchListSyncer(IRepository<T_CompletionsPunchList> _CompletionsPunchListRepository)
        {
            this._CompletionsPunchListRepository = _CompletionsPunchListRepository;
        }

        public async Task<bool> downloadChanges(string workpackName)
        {
            try
            {
                string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getPunchLists(Settings.ProjectName, Settings.CurrentDB, workpackName), Settings.CompletionAccessToken);
                var SystemsList = JsonConvert.DeserializeObject<List<T_CompletionsPunchList>>(JsonString);
                await _CompletionsPunchListRepository.InsertAllAsync(SystemsList);

                //  data not get yet
                string JsonString1 = ModsTools.CompletionWebServiceGet(ApiUrls.getPunchListMetaData(Settings.ProjectName, Settings.CurrentDB), Settings.CompletionAccessToken);
                if (string.IsNullOrWhiteSpace(JsonString1))
                {

                }
                //var SystemsList1 = JsonConvert.DeserializeObject<List<T_CompletionsPunchList>>(JsonString1);
                //await _CompletionsPunchListRepository.InsertOrReplaceAsync(SystemsList);
                //ProgressPunchList
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        public Task<bool> uploadChanges(string id)
        {
            throw new NotImplementedException();
        }
    }
}

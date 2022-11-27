using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables.Completions;
using JGC.Models.Completions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JGC.Common.Services
{
    public class ITRSyncer : IitrSyncer
    {

        private readonly IRepository<T_TAG> _TAGRepository;
        private readonly IRepository<T_Tag_headers> _Tag_headersRepository;
        private readonly IRepository<T_CHECKSHEET> _CHECKSHEETRepository;
        private readonly IRepository<T_CHECKSHEET_PER_TAG> _CHECKSHEET_PER_TAGRepository;
        private readonly IRepository<T_CHECKSHEET_QUESTION> _CHECKSHEET_QUESTIONRepository;
        private readonly IRepository<T_CompletionsPunchList> _CompletionsPunchListRepository;
        private readonly IRepository<T_TAG_SHEET_ANSWER> _TAG_SHEET_ANSWERRepository;
        private readonly IRepository<T_TAG_SHEET_HEADER> _TAG_SHEET_HEADERRepository;
        private readonly IRepository<T_SignOffHeader> _SignOffHeaderRepository;
        Dictionary<string, List<string>> tagToSheetList = new Dictionary<string, List<string>>();
        private T_Handover Add;
        string CheckSheetName;
        string TagName;

        string directory;
        public ITRSyncer(
            IRepository<T_TAG> _TAGRepository,
            IRepository<T_Tag_headers> _Tag_headersRepository,
            IRepository<T_CHECKSHEET> _CHECKSHEETRepository,
            IRepository<T_CHECKSHEET_PER_TAG> _CHECKSHEET_PER_TAGRepository,
            IRepository<T_CHECKSHEET_QUESTION> _CHECKSHEET_QUESTIONRepository,
            IRepository<T_CompletionsPunchList> _CompletionsPunchListRepository,
            IRepository<T_TAG_SHEET_ANSWER> _TAG_SHEET_ANSWERRepository,
            IRepository<T_SignOffHeader> _SignOffHeaderRepository,
            IRepository<T_TAG_SHEET_HEADER> _TAG_SHEET_HEADERRepository)
        {

            this._TAGRepository = _TAGRepository;
            this._Tag_headersRepository = _Tag_headersRepository;
            this._CHECKSHEETRepository = _CHECKSHEETRepository;
            this._CHECKSHEET_PER_TAGRepository = _CHECKSHEET_PER_TAGRepository;
            this._CHECKSHEET_QUESTIONRepository = _CHECKSHEET_QUESTIONRepository;
            this._CompletionsPunchListRepository = _CompletionsPunchListRepository;
            this._TAG_SHEET_ANSWERRepository = _TAG_SHEET_ANSWERRepository;
            this._SignOffHeaderRepository = _SignOffHeaderRepository;
            this._TAG_SHEET_HEADERRepository = _TAG_SHEET_HEADERRepository;
        }
        public async Task<bool> downloadChanges(string checkSheetName, string workpackname)
        {
            try
            {
                CheckSheetName = checkSheetName;
                _ = await pullDownWorkpacks(workpackname);
                _ = await pullDownSheetQuestions();
                _ = await pullCheckSheetAnswersAsync();
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        private async Task<bool> pullDownWorkpacks(string WorkpackName)
        {
            try
            {
                string WorkpackjsonResult = ModsTools.CompletionWebServiceGet(ApiUrls.getTags(Settings.ModelName, Settings.ProjectName, "workpack", WorkpackName, "dontmatter", Settings.CurrentDB), Settings.CompletionAccessToken);
                var WorkpacksList = JsonConvert.DeserializeObject<T_WorkPack>(WorkpackjsonResult);
                if (WorkpacksList.tagNameToSheetNameMap != null)
                {
                    TagName = WorkpacksList.tagNameToSheetNameMap.Where(x => x.Value.Contains(CheckSheetName)).FirstOrDefault().Key;
                    await _CHECKSHEET_PER_TAGRepository.InsertOrReplaceAsync(new T_CHECKSHEET_PER_TAG() { TAGNAME = TagName, CHECKSHEETNAME = CheckSheetName, HEADER_ID = "0", JOBPACK = " ", ProjectName = Settings.ProjectName });
                }
                if (WorkpacksList.tags != null)
                {
                    WorkpacksList.tags.ForEach(i => i.ProjectName = Settings.ProjectName);
                    await _TAGRepository.InsertOrReplaceAsync(WorkpacksList.tags);
                }

                if (WorkpacksList.checkSheets != null)
                {
                    WorkpacksList.checkSheets.ForEach(i=>i.ProjectName = Settings.ProjectName);
                    await _CHECKSHEETRepository.InsertOrReplaceAsync(WorkpacksList.checkSheets);
                }

                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }
        private async Task<bool> pullDownSheetQuestions()
        {
            try
            {
                var JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getSheetQuestions(Settings.ProjectName, Settings.CurrentDB, CheckSheetName), Settings.CompletionAccessToken);
                var Quetions = JsonConvert.DeserializeObject<List<T_CHECKSHEET_QUESTION>>(JsonString);

                if (Quetions != null && Quetions.Any())
                {
                    Quetions.ForEach(x => { x.CheckSheet = CheckSheetName; x.ProjectName = Settings.ProjectName; });
                    await _CHECKSHEET_QUESTIONRepository.InsertOrReplaceAsync(Quetions);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        private async Task<bool> pullCheckSheetAnswersAsync()
        {
            try
            {
                var JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getCheckSheetAnswers(Settings.ModelName, Settings.ProjectName, TagName, CheckSheetName, Settings.CurrentDB), Settings.CompletionAccessToken);
                var CheckSheetAnswers = JsonConvert.DeserializeObject<List<CheckSheetAnswerModel>>(JsonString);

                CheckSheetAnswers.ForEach(c => {
                    c.SignOffHeaders.ForEach(x => { x.SignOffChecksheet = c.CheckSheetName; x.SignOffTag = c.TagName; x.ProjectName = Settings.ProjectName; });
                    c.answers.ForEach(i => { i.tagName = c.TagName; i.ccmsHeaderId = c.CcmsHeaderId; i.checkSheetName = c.CheckSheetName; i.jobPack = c.JobPack; i.IsSynced = true;i.ProjectName = Settings.ProjectName; });
                });
                foreach (CheckSheetAnswerModel checkSheetAnswer in CheckSheetAnswers)
                {
                    if (checkSheetAnswer.SignOffHeaders.Any())
                        await _SignOffHeaderRepository.InsertOrReplaceAsync(checkSheetAnswer.SignOffHeaders);

                    if (checkSheetAnswer.answers.Any())
                        await _TAG_SHEET_ANSWERRepository.InsertOrReplaceAsync(checkSheetAnswer.answers);

                    if (checkSheetAnswer.TagSheetHeaders != null)
                    {
                        PropertyInfo[] Props = (typeof(TagSheetHeaders).GetProperties(BindingFlags.Public | BindingFlags.Instance))?.Where(i => i.GetValue(checkSheetAnswer.TagSheetHeaders) != null).ToArray();

                        var T_SheetHeader = Props.Select(x => new T_TAG_SHEET_HEADER()
                        {
                            TagName = checkSheetAnswer.TagName,
                            ChecksheetName = checkSheetAnswer.CheckSheetName,
                            ColumnKey = x.Name,
                            ColumnValue = x.ToString(),
                            JobPack = checkSheetAnswer.JobPack,
                            ProjectName = Settings.ProjectName
                        });
                        await _TAG_SHEET_HEADERRepository.InsertOrReplaceAsync(T_SheetHeader);
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public Task<bool> uploadChanges(string id)
        {
            throw new NotImplementedException();
        }
    }
}

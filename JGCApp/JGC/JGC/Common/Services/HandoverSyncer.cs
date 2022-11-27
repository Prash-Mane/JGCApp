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
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JGC.Common.Services
{
    public class HandoverSyncer : IHandoverSyncer
    {
        private readonly IRepository<T_Handover> _CompletionsHandoverRepository;
        private readonly IRepository<T_HandoverWorkpacks> _HandoverWorkpackRepository;
        private T_Handover Add;

        string directory;
        public HandoverSyncer(
        IRepository<T_Handover> _CompletionsHandoverRepository,
        IRepository<T_HandoverWorkpacks> _CompletionsHandoverWorkRepository)
        {
            this._CompletionsHandoverRepository = _CompletionsHandoverRepository;
            this._HandoverWorkpackRepository = _CompletionsHandoverWorkRepository;
        }
        public async Task<bool> downloadChangesAsync(string HandoverName)
        {
            try
            {

                ////Sync Handover Page 
                string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getHandovers(Settings.ModelName, Settings.ProjectName, Settings.CurrentDB, HandoverName), Settings.CompletionAccessToken);
                List<HandoversRootModel> HandoverPageList = JsonConvert.DeserializeObject<List<HandoversRootModel>>(JsonString);
                if (HandoverPageList.Count > 0)
                {
                    foreach (HandoversRootModel HRModel in HandoverPageList)
                    {
                        string sytem = HRModel.pages.FirstOrDefault().system;
                        string Subsytem = HRModel.pages.FirstOrDefault().subsystem;

                        //Get Imaegs

                        foreach (T_Handover handover in HRModel.pages)
                        {
                            handover.ProjectName = Settings.ProjectName;
                            await _CompletionsHandoverRepository.InsertOrReplaceAsync(handover);
                            await DownloadFiles(handover, HandoverName);
                        }

                        await _HandoverWorkpackRepository.InsertOrReplaceAsync(HRModel.associatedWorkpacks.Select(x => new T_HandoverWorkpacks()
                        {
                            COLUMN_HANDOVER_WP = x,
                            COLUMN_HANDOVER_WPSUBSYSTEM = Subsytem,
                            COLUMN_HANDOVER_WPSYSTEM = sytem,
                            ProjectName = Settings.ProjectName
                        }));
                    }


                    //if (HRModel.associatedWorkpacks != null && HRModel.associatedWorkpacks.Any())
                    //{
                    //    foreach (string handoverWorkpack in HRModel.associatedWorkpacks)
                    //    {

                    //        T_HandoverWorkpacks T_handoverWorkpack = new T_HandoverWorkpacks();
                    //        T_handoverWorkpack.COLUMN_HANDOVER_WP = handoverWorkpack;
                    //        T_handoverWorkpack.COLUMN_HANDOVER_WPSUBSYSTEM = Subsytem;
                    //        T_handoverWorkpack.COLUMN_HANDOVER_WPSYSTEM = sytem;


                    // }
                    //   }

                }
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }
        public async Task<List<dynamic>> DownloadFiles(T_Handover page, string testPachName)
        {
            directory = await DependencyService.Get<ISaveFiles>().GenerateImagePath("Handovers" + "\\" + "" + page.system);
            var FTPForBackEnd = CurrentDBService.getFTPCredentialsForBackEnd(Settings.CurrentDB);
            string path = (FTPForBackEnd[3] + "/CCMS/HHSHMCFileDump/" + Settings.ProjectName + "/" + page.system + "/" + page.number + ".png").Replace(" ", "");

            if (!string.IsNullOrEmpty(page.subsystem))
            {
                path = (FTPForBackEnd[3] + "/CCMS/HHSHMCFileDump/" + Settings.ProjectName + "/" + page.system + "/" + page.subsystem + "/" + page.number + ".png").Replace(" ", "");
                directory = "Handovers" + "\\" + page.system + "\\" + page.subsystem;
                directory = await DependencyService.Get<ISaveFiles>().GenerateImagePath("Handovers" + "\\" + "" + page.system + "\\" + page.subsystem);
            }

            List<dynamic> returnList = new List<dynamic>();
            try
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + FTPForBackEnd[0] + "/" + path);
                request.EnableSsl = true;

                request.Method = WebRequestMethods.Ftp.DownloadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(FTPForBackEnd[1], FTPForBackEnd[2]);

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Byte[] ImageBytes;
                Stream responseStream = response.GetResponseStream();
                using (MemoryStream ms = new MemoryStream())
                {
                    responseStream.CopyTo(ms);
                    ImageBytes = ms.ToArray();


                    string pathgh = await DependencyService.Get<ISaveFiles>().SavePictureToDisk(directory, page.number + ".png", ImageBytes);

                    response.Close();
                    return returnList;
                }
            }
            catch (Exception e)
            {
                return returnList;
            }

        }

        public Task<bool> uploadChanges(string id)
        {
            throw new NotImplementedException();
        }
    }
}

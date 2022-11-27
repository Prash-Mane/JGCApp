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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JGC.Common.Services
{
    public class TestPackSyncer : ITestPackSyncer
    {
        private readonly IRepository<T_Handover> _handoverRepository;
        string directory;

        public TestPackSyncer(IRepository<T_Handover> _handoverRepository)
        {
            this._handoverRepository = _handoverRepository;
        }


        public async Task<bool> downloadChanges(string TestPackName)
        {
            try
            {
                directory = await DependencyService.Get<ISaveFiles>().GenerateImagePath("TestPacks" + "\\" + "" + TestPackName + "");
                string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getTestPack(Settings.ModelName, Settings.ProjectName, TestPackName, Settings.CurrentDB), Settings.CompletionAccessToken);
                var TestPckPagesList = JsonConvert.DeserializeObject<TestPackModel>(JsonString);
                if (TestPckPagesList.pages != null)
                {
                    foreach (T_Handover page in TestPckPagesList.pages)
                    {
                        await _handoverRepository.InsertOrReplaceAsync(new T_Handover()
                        {
                            testpack = page.testpack,
                            name = page.name,
                            number = page.number,
                            system = page.system,
                            subsystem = page.subsystem,
                            testpackname = TestPackName,
                            ProjectName = Settings.ProjectName

                        });
                        await DownloadFiles(page, TestPackName);
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

        public async Task<List<dynamic>> DownloadFiles(T_Handover page, string testPachName)
        {
            var FTPForBackEnd = CurrentDBService.getFTPCredentialsForBackEnd(Settings.CurrentDB);
            string path = FTPForBackEnd[3] + "/CCMS/HHLeakTestFileDump/" + Settings.ModelName + "/" + testPachName + "/" + page.number + ".PNG";
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
                Byte[] bt;
                Stream responseStream = response.GetResponseStream();
                using (MemoryStream ms = new MemoryStream())
                {
                    responseStream.CopyTo(ms);
                    bt = ms.ToArray();

                    string pathgh = await DependencyService.Get<ISaveFiles>().SavePictureToDisk(directory, page.number + ".PNG", bt);

                    response.Close();
                    return returnList;
                }
            }
            catch (Exception e)
            {
                return returnList;
            }

        }

    }
}

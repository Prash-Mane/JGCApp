using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.DataBase;
using JGC.DataBase.DataTables.Completions;
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
    public class DrawingsSyncer : IDrawingsSyncer
    {
        private readonly IRepository<T_CompletionsDrawings> _CompletionsDrawingsRepository;
        string directory;


        public DrawingsSyncer(IRepository<T_CompletionsDrawings> _CompletionsDrawingsRepository)
        {
            this._CompletionsDrawingsRepository = _CompletionsDrawingsRepository;
        }

        public async Task<bool> downloadChanges(string drwaingID)
        {
            List<T_CompletionsDrawings> drawings;
            try
            {
                await _CompletionsDrawingsRepository.QueryAsync<T_CompletionsDrawings>("DELETE FROM [T_CompletionsDrawings]");

                directory = await DependencyService.Get<ISaveFiles>().GenerateImagePath("Drawings" + "\\" + Settings.ProjectName);
                string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getDrawingsForProject(Settings.ProjectName, Settings.CurrentDB), Settings.CompletionAccessToken);
                var drawingsList = JsonConvert.DeserializeObject<List<T_CompletionsDrawings>>(JsonString);
                if (drawingsList != null)
                {
                    drawingsList.ForEach(i => i.ProjectName = Settings.ProjectName);
                    await _CompletionsDrawingsRepository.InsertOrReplaceAsync(drawingsList);
                    DownloadFiles(drawingsList);

                }

                return true;
            }
            catch (Exception Ex)
            {
                return true;
            }
        }



        public async void DownloadFiles(List<T_CompletionsDrawings> drawingsList)
        {
            foreach (T_CompletionsDrawings drwaing in drawingsList)
            {
                var FTPForBackEnd = CurrentDBService.getFTPCredentialsForBackEnd(Settings.CurrentDB);
                string path = FTPForBackEnd[3] + "/CCMS/drawings/" + Settings.ProjectName + "/" + drwaing.filename;
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
                    string bs;
                    Stream responseStream = response.GetResponseStream();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        responseStream.CopyTo(ms);
                        bt = ms.ToArray();
                        bs = Convert.ToBase64String(bt);
                        string pathgh = await DependencyService.Get<ISaveFiles>().SavePictureToDisk(directory, drwaing.filename, bt);

                        response.Close();

                    }
                }
                catch (Exception e)
                {

                }
            }
        }

        public Task<bool> uploadChanges(string id)
        {
            throw new NotImplementedException();
        }
    }
}

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
    public class SystemPunchListSyncer : ISystemPunchListSyncer
    {
        private readonly IRepository<T_CompletionsPunchList> _CompletionsPunchListRepository;
        public SystemPunchListSyncer(IRepository<T_CompletionsPunchList> _CompletionsPunchListRepository)
        {
            this._CompletionsPunchListRepository = _CompletionsPunchListRepository;
        }
        //Downlaod
        public async Task<bool> downloadChanges(string System)
        {
            try
            {
                string JsonString = ModsTools.CompletionWebServiceGet(ApiUrls.getPunchLists(Settings.ProjectName, Settings.CurrentDB), Settings.CompletionAccessToken);
                var HandoverPunchList = JsonConvert.DeserializeObject<List<T_CompletionsPunchList>>(JsonString);
                HandoverPunchList.ForEach(x => x.synced = true);
                await _CompletionsPunchListRepository.InsertOrReplaceAsync(HandoverPunchList);
                
                //string JsonStrings = ModsTools.WebServiceGet(ApiUrls.getPunchListMetaData(Settings.ProjectName, Settings.CurrentDb), Settings.AccessToken);
                //var PunchListMetaData = JsonConvert.DeserializeObject<List<string>>(JsonStrings);
                SyncProgressLogs.UpdateLogs.PunchListLogs = "Punch Lists: Sync Complete";
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }
        //Uplolad
        public async Task<bool> uploadChanges(string id)
        {
            int i = 1;
            Boolean uploadResult = false;
            try
            {
                var punchlist = await _CompletionsPunchListRepository.GetAsync(x => !x.synced);
                foreach (T_CompletionsPunchList punchlistItem in punchlist)
                {
                    SyncProgressLogs.UpdateLogs.PunchListLogs = "Uploading Punchlist " + i + " of " + punchlist.Count + " Completed";
                    string postJSON = JsonConvert.SerializeObject(punchlistItem);
                    string JsonString = ModsTools.CompletionWebServicePost(ApiUrls.postPunchListItem(Settings.ProjectName, Settings.CurrentDB), postJSON, Settings.CompletionAccessToken);
                    T_CompletionsPunchList UploadedPunchList = JsonConvert.DeserializeObject<T_CompletionsPunchList>(JsonString);
                    punchlistItem.synced = true;
                    await _CompletionsPunchListRepository.UpdateAsync(punchlistItem);

                    if (UploadedPunchList != null && string.IsNullOrEmpty(punchlistItem.uniqueno))
                        punchlistItem.uniqueno = UploadedPunchList.uniqueno;

                    //upload images
                    if (!string.IsNullOrWhiteSpace(punchlistItem.imageLocalLocation) && !punchlistItem.imageLocalLocation.Contains("https://"))
                    {
                        UploadPunchListImage(Settings.ModelName, punchlistItem.systemno, punchlistItem.tagno, punchlistItem.location, punchlistItem.originatordisc, punchlistItem.uniqueno, punchlistItem.imageLocalLocation);
                    }
                    i++;
                }
                SyncProgressLogs.UpdateLogs.PunchListLogs = "Uploaded";
                uploadResult = true;
            }
            catch (Exception Ex)
            {
                uploadResult = false;
            }
            return uploadResult;
        }

        private async void UploadPunchListImage(string modelname, string systemName, string tagName, string location, string disc, string uniqueNo, string fileLocation)
        {            
            if (string.IsNullOrWhiteSpace(systemName))
                systemName = "NA";
            if (string.IsNullOrWhiteSpace(tagName))
                tagName = "NA";
            if (string.IsNullOrWhiteSpace(location))
                location = "NA";
            if (string.IsNullOrWhiteSpace(disc))
                disc = "NA";
            if (string.IsNullOrWhiteSpace(uniqueNo))
                uniqueNo = "NA";

            string fileName = "PunchImage" + DateTime.Now.ToString(AppConstant.ExtendedDateParseString) + ".png";
            
            List<string> credentials = CurrentDBService.getFTPCredentialsForBackEnd(Settings.CurrentDB);
            string[] ftpFileLocation = new String[] { credentials[3], "DocumentHub", modelname, "VMReference", systemName, tagName, location, disc, uniqueNo, "Punch Item", uniqueNo };
            Boolean uploadSuccess = false;
            try
            {
                string CreatePath = "ftp://" + credentials[0];
                foreach (string path in ftpFileLocation)
                {
                    CreatePath += "/" + path;
                    // create new directory if not exist
                    if (!DoesFtpDirectoryExist(CreatePath, credentials[1], credentials[2]))
                        CreateFTPDirectory(CreatePath, credentials[1], credentials[2]);
                }
                string ftpFolderpath = string.Join("/", ftpFileLocation);
                var file = await DependencyService.Get<ISaveFiles>().ReadBytes(fileLocation);
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + credentials[0] + "/" + ftpFolderpath + "/" + fileName);
                request.EnableSsl = true;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(credentials[1], credentials[2]);
                request.ContentLength = file.Length;

                var requestStream = request.GetRequestStream();
                requestStream.Write(file, 0, file.Length);
                requestStream.Close();

                var response = (FtpWebResponse)request.GetResponse();
                if (response != null)
                {
                    if (response.ResponseUri != null)
                    {
                        //string HostURL = "https://" + response.ResponseUri.Host;
                        string HostURL = Settings.CompletionServer_Url.Replace("/WebServiceCompletions/API/", ""); 
                        string url = response.ResponseUri.LocalPath.Replace("/" + credentials[3], HostURL);
                        string localpath = "S:/WWW Route" + response.ResponseUri.LocalPath;
                        localpath = localpath.Replace("/", "\\");

                        VMHubPunch punchImage = new VMHubPunch
                        {
                            ref1 = systemName,
                            ref2 = tagName,
                            ref3 = location,
                            ref4 = disc,
                            ref5 = uniqueNo,
                            reference = systemName + "~" + tagName + "~" + location + "~" + disc + "~" + uniqueNo + "~NA",
                            url = url,
                            local = localpath,
                            modelname = modelname,
                            createdby = Settings.CompletionUserName,
                            createdon = DateTime.Now,
                            chksum = ""
                        };
                        ModsTools.CompletionWebServicePost(ApiUrls.PostVMHubPunchImages(Settings.CurrentDB), JsonConvert.SerializeObject(punchImage), Settings.CompletionAccessToken);
                        response.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
            }
        }
        public bool DoesFtpDirectoryExist(string dirPath, string userName, string password)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(dirPath);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(userName, password);
                request.EnableSsl = true;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                return false;
            }
        }
        public bool CreateFTPDirectory(string dirPath, string userName, string password)
        {
            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(dirPath));
                ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                ftpRequest.Credentials = new NetworkCredential(userName, password);
                ftpRequest.EnableSsl = true;
                ftpRequest.UsePassive = true;
                ftpRequest.UseBinary = true;
                ftpRequest.KeepAlive = false;
                FtpWebResponse ftpresponse = (FtpWebResponse)ftpRequest.GetResponse();
                Stream ftpStream = ftpresponse.GetResponseStream();

                ftpStream.Close();
                ftpresponse.Close();
                return true;
            }
            catch (WebException ex)
            {
                return false;
            }
        }
    }
}

using JGC.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace JGC.Common.Services
{
    public class FtpService : IFtpService
    {
        public List<dynamic> DownloadFiles(string FilePth)
        {
            List<dynamic> returnList = new List<dynamic>();
            try
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://bp.vmlive.net/wwwroot/CCMS/Drawings/MODS Test Project/01-GS-999-01.pdf");
                request.EnableSsl = true;

                request.Method = WebRequestMethods.Ftp.DownloadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential("virtualmanagerbp", "virtualmanagerbpW5x7L4E4Yelfin2112");

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                using (MemoryStream ms = new MemoryStream())
                {
                    responseStream.CopyTo(ms);

                    FileStream fWrite = new FileStream("test.txt", FileMode.Create, FileAccess.Write, FileShare.None, 8, FileOptions.None);
                    // Write the number of bytes to the file.
                    fWrite.WriteByte((byte)ms.ToArray().Length);

                    using (Stream file = File.Create("text"))
                    {
                        file.CopyTo(responseStream);// CopyStream(input, file);
                    }
                    response.Close();
                    return returnList;
                }
            }
            catch (Exception e)
            {
                return returnList;
            }

        }

        public List<string> FtpFileNames(string id)
        {
            List<string> fileNames = new List<string>();
            try
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://bp.vmlive.net/wwwroot/CCMS/Drawings/MODS Test Project");
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.EnableSsl = true;
                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential("virtualmanagerbp", "virtualmanagerbpW5x7L4E4Yelfin2112");

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                Console.WriteLine(reader.ReadToEnd());

                Console.WriteLine($"Directory List Complete, status {response.StatusDescription}");

                reader.Close();
                response.Close();
                return fileNames;

            }
            catch (Exception e)
            {
                return fileNames;
            }
        }

        internal static bool uploadFile(string v1, string v2, string v3, string[] ftpFileLocation, string fileName, string fileLocation)
        {
            throw new NotImplementedException();
        }
    }
}

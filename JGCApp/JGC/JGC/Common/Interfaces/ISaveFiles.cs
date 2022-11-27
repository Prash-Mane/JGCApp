using JGC.Common.Helpers;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JGC.Common.Interfaces
{
    public interface ISaveFiles
    {
        Task<string> SavePictureToDisk(string Folder, string filename, byte[] imageData);
        Task<ImageSource> GetImage(string path);
        Task<List<string>> GetAllImages(string path);
        Task<bool> RenameImage(string path, string CurrentName, string NewName);
        Task<string> SavePDFToDisk(string Folder, string fileName, byte[] pdfData);
        Task<string> GenerateImagePath(string Folders);
        Task<bool> DeleteImage(string path);
        Task<bool> DirectoryExists(string Folder);
        Task<string[]> GetDirectories(string Folder);
        Task<List<string>> GetImageFiles(string path);
        Task<string> CreateDWRPhotoDirectories(string JobCode, string EReportID, string JointNumber, string Field);
        Task<bool> RemoveAllFilefromFolder(string path);
        Task<bool> PickFile(string path);
        Task<bool> DWRPickFile(string path, string LineNumber, string JointNo);
        Task<bool> DWRPickFile(string path);
        Task<byte[]> ReadBytes(string path);
        Task<string> GetIWPFileLocation(string path);
        Task<bool> RemovePDFFilefromFolder(string path); 
        Task<ImageSource> GetThumbnail(string path);
        Task<string> ConvertPDFtoByte(string path);
        Task<string> SaveImageAsPdf(Stream path);
        Task<List<string>> GetPdfFiles(string path);
        Task<string> PickPdf(string path);
        Task<string> GeneratePdfPath(string Folders);
        Task<FileData> PickPDF(string Folders);
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetsLibrary;
using Foundation;
using JGC.Common.Interfaces;
using JGC.iOS.DependancyObjects;
using Plugin.FilePicker.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using QuickLook;
using UIKit;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(SaveFiles))]
namespace JGC.iOS.DependancyObjects
{
    public class SaveFiles : ISaveFiles
    {
        public static ALAssetsLibrary lib = new ALAssetsLibrary();
        public static ALAssetsGroup current_album = null;
        public static string file_path = String.Empty, filePath = string.Empty;
        public async Task<string> SavePictureToDisk(string Path, string filename, byte[] imageData)
        {
            return await WriteFileOnDevice(imageData, filename , Path);
        }

        private async Task<string> WriteFileOnDevice(byte[] imgData, string fileName, string path)
        {
            try
            {
               // lib.AddAssetsGroupAlbum("JGC", g => Console.WriteLine("Succeeded"), e => Console.WriteLine("Error: " + e));
                NSDictionary dict = new NSDictionary();
               // var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var directoryname = path;//Path.Combine(documentsDirectory, "JGC");
                Directory.CreateDirectory(directoryname);
                string jpgFilename = fileName + ".jpg";

                var imgpath = Path.Combine(path, jpgFilename);
                var image = new UIImage(NSData.FromArray(imgData));

                var assetUrl = await lib.WriteImageToSavedPhotosAlbumAsync(image.AsJPEG(), dict);
                file_path = assetUrl.AbsoluteUrl.ToString();
                if (File.Exists(imgpath))
                {
                    File.Delete(imgpath);
                    if (!File.Exists(imgpath))
                    {
                        Console.WriteLine("Deleted");
                    }
                }

                lib.Enumerate(ALAssetsGroupType.Album, HandleALAssetsLibraryGroupsEnumerationResultsDelegate, (obj) => { });
                return file_path;
            }
            catch (Exception ex)
            {
            }

            return filePath;
        }

        static void HandleALAssetsLibraryGroupsEnumerationResultsDelegate(ALAssetsGroup group, ref bool stop)
        {
            if (group == null)
            {
                stop = true;
                return;
            }
            if (group.Name == "JGC")
            {
                stop = true;
                current_album = group;
                AddImageToAlbum();
            }
        }

        static void AddImageToAlbum()
        {
            if (current_album != null && !String.IsNullOrEmpty(file_path))
            {
                lib.AssetForUrl(new Foundation.NSUrl(file_path), delegate (ALAsset asset)
                {
                    if (asset != null)
                    {
                        current_album.AddAsset(asset);
                    }
                    else
                    {
                        Console.WriteLine("ASSET == NULL.");
                    }
                }, delegate (NSError assetError)
                {
                    Console.WriteLine(assetError.ToString());
                });
            }
        }

        public async Task<ImageSource> GetImage(string path)
        {
            //var memoryStream = new MemoryStream();
            Image image = new Image();
            //using (var source = File.OpenRead(path))
            //{
            //    await source.CopyToAsync(memoryStream);
            //}
            //return memoryStream;

            image = new Image
            {
                Source = ImageSource.FromFile(path)
            };
            return image.Source;
        }

        public async Task<string> GenerateImagePath(string Path)
        {
            NSDictionary dict = new NSDictionary();
            //var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var directoryname = Path;//Path.Combine(documentsDirectory, "JGC");
            if (!File.Exists(Path))
                Directory.CreateDirectory(directoryname);
            return Path;
        }
        public async Task<List<string>> GetAllImages(string path)
        {           
            return null;
        }
        public Task<bool> RenameImage(string path, string CurrentName, string NewName)
        {
            return null;
        }
        public async Task<string> SavePDFToDisk(string Folder, string fileName, byte[] pdfData)
        {
            string fileLocation = string.Empty;
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

            
              fileLocation = Path.Combine(documentsPath, Folder);

            var filePath = Path.Combine(documentsPath, fileLocation, fileName);
            File.WriteAllBytes(filePath, pdfData);
            OpenPDF(filePath);
            return filePath;
        }
        public void OpenPDF(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);

            QLPreviewController previewController = new QLPreviewController();
            previewController.DataSource = new PDFPreviewControllerDataSource(fi.FullName, fi.Name);

            UINavigationController controller = FindNavigationController();
            if (controller != null)
                controller.PresentViewController(previewController, true, null);
        }

        public async Task<bool> DeleteImage(string path)
        {
            bool result;
            try
            {
                System.IO.File.Delete(path);
                result = true;
            }
            catch(Exception ex)
            {
                result = false;
            }           

            return result;
        }

        private UINavigationController FindNavigationController()
        {
            foreach (var window in UIApplication.SharedApplication.Windows)
            {
                if (window.RootViewController.NavigationController != null)
                    return window.RootViewController.NavigationController;
                else
                {
                    UINavigationController val = CheckSubs(window.RootViewController.ChildViewControllers);
                    if (val != null)
                        return val;
                }
            }

            return null;
        }

        private UINavigationController CheckSubs(UIViewController[] controllers)
        {
            foreach (var controller in controllers)
            {
                if (controller.NavigationController != null)
                    return controller.NavigationController;
                else
                {
                    UINavigationController val = CheckSubs(controller.ChildViewControllers);
                    if (val != null)
                        return val;
                }
            }
            return null;
        }

        public class PDFItem : QLPreviewItem
        {
            string title;
            string uri;

            public PDFItem(string title, string uri)
            {
                this.title = title;
                this.uri = uri;
            }

            public override string ItemTitle
            {
                get { return title; }
            }

            public override NSUrl ItemUrl
            {
                get { return NSUrl.FromFilename(uri); }
            }
        }

        public class PDFPreviewControllerDataSource : QLPreviewControllerDataSource
        {
            string url = "";
            string filename = "";

            public PDFPreviewControllerDataSource(string url, string filename)
            {
                this.url = url;
                this.filename = filename;
            }

            public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
            {
                return new PDFItem(filename, url);
            }

            public override nint PreviewItemCount(QLPreviewController controller)
            {
                return 1;
            }


        }

        public async Task<bool> DirectoryExists(string Folder)
        {

           
                NSDictionary dict = new NSDictionary();
                var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var directoryname = Path.Combine(documentsDirectory, Folder.Replace("\\","/"));
               
              
                if (Directory.Exists(directoryname))
                return true;
                else
               return false; 

        }

        public async Task<string[]> GetDirectories(string Folder)
        {
            NSDictionary dict = new NSDictionary();
            var documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var directoryname = Path.Combine(documentsDirectory, Folder.Replace("\\", "/"));
            return System.IO.Directory.GetDirectories(directoryname);
            

        }
        public async Task<List<string>> GetImageFiles(string path)
        {
            var Files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
            .Where(s => s.ToUpper().EndsWith(".PNG") ||
            s.ToUpper().EndsWith(".BMP") ||
            s.ToUpper().EndsWith(".JPG") ||
            s.ToUpper().EndsWith(".JPEG") ||
            s.ToUpper().EndsWith(".GIF")
            ).ToList();

            return Files;

        }

        public async Task<string> CreateDWRPhotoDirectories(string JobCode, string EReportID, string JointNumber, string Field)
        {
            string Location = ""; //Constants.StartDirectory;
            string[] Folders = new string[] { "Photo Store", JobCode, EReportID, JointNumber, Field };

            foreach (string Folder in Folders)
            {
                Location += ("/" + Folder);
                if (!Directory.Exists(Location))
                    Directory.CreateDirectory(Location);
            }

            return Location;
        }
        public async Task<bool> RemoveAllFilefromFolder(string path)
        {
            bool result = false;

            return result;
        }
        public async Task<bool> PickFile(string path)
        {
            bool result = false;
            byte[] imageData;
            try
            {
                ImageSource image;
                string filename = string.Empty;
                PickMediaOptions mediaOptions = new PickMediaOptions();

                MediaFile files = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
                if (files == null)
                    return result = true;
                if (files != null)
                {
                    filename = files.Path.Split("/").Last();

                    using (var memoryStream = new MemoryStream())
                    {
                        files.GetStream().CopyTo(memoryStream);
                        files.Dispose();
                        imageData = memoryStream.ToArray();
                    }
                    SavePictureToDisk(path, filename, imageData);

                }
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }
        public async Task<bool> DWRPickFile(string path, string LineNumber, string JointNo)
        {
            bool result = false;
            byte[] imageData;
            try
            {
                ImageSource image;
                string filename = string.Empty;
                PickMediaOptions mediaOptions = new PickMediaOptions();

                MediaFile files = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
                if (files == null)
                    return result = true;
                if (files != null)
                {
                    filename = files.Path.Split("/").Last();

                    using (var memoryStream = new MemoryStream())
                    {
                        files.GetStream().CopyTo(memoryStream);
                        files.Dispose();
                        imageData = memoryStream.ToArray();
                    }
                    SavePictureToDisk(path, Path.GetFileNameWithoutExtension(filename) + "~" + LineNumber + "-" + JointNo + Path.GetExtension(filename), imageData);

                }
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }
        public async Task<bool> DWRPickFile(string path)
        {
            bool result = false;
            byte[] imageData;
            try
            {
                ImageSource image;
                string filename = string.Empty;
                PickMediaOptions mediaOptions = new PickMediaOptions();

                MediaFile files = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
                if (files == null)
                    return result = true;
                if (files != null)
                {
                    filename = files.Path.Split("/").Last();

                    using (var memoryStream = new MemoryStream())
                    {
                        files.GetStream().CopyTo(memoryStream);
                        files.Dispose();
                        imageData = memoryStream.ToArray();
                    }
                    SavePictureToDisk(path, Path.GetFileNameWithoutExtension(filename) + Path.GetExtension(filename), imageData);

                }
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }
        public async Task<byte[]> ReadBytes(string path)
        {
            return null;
        }
        public async Task<string> GetIWPFileLocation(string path)
        {
            return null;
        }
        public async Task<bool> RemovePDFFilefromFolder(string path)
        {
            return false;
        }
        public Task<ImageSource> GetThumbnail(string path)
        {
            throw new NotImplementedException();
        }
        public Task<List<string>> GetPdfFiles(string path)
        {
            throw new NotImplementedException();
        }

        public Task<string> PickPdf(string path)
        {
            throw new NotImplementedException();
        }

        public Task<string> GeneratePdfPath(string Folders)
        {
            throw new NotImplementedException();
        }

        public Task<FileData> PickPDF(string Folders)
        {
            throw new NotImplementedException();
        }
        public Task<string> ConvertPDFtoByte(string path)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveImageAsPdf(Stream path)
        {
            throw new NotImplementedException();
        }
    }
}
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.UWP.DependancyObjects;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Data.Pdf;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveFiles))]
namespace JGC.UWP.DependancyObjects
{
    public class SaveFiles : ISaveFiles
    {
        string filepath; 
        string tempPath;
        public async Task<ImageSource> GetImage(string path)
        {
            string newpath = string.Empty, fileName = string.Empty;
            ImageSource image = null;
            StorageFolder picturesDirectory = KnownFolders.PicturesLibrary;
            StorageFolder DocumentsLibrary = KnownFolders.DocumentsLibrary;
            StorageFolder folderDirectory = picturesDirectory;
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    path = path.Replace("/", "\\");
                    fileName = path.Split("\\").Last();

                    int p = path.IndexOf("Photo Store");
                    if (p >= 0)
                    {
                        newpath = path.Remove(0, p).Split("\\" + fileName).First();
                        folderDirectory = await picturesDirectory.GetFolderAsync(newpath);
                    }
                    else 
                    {
                        int q = path.IndexOf("PDF Store");
                        if (q >= 0)
                        {
                            newpath = path.Remove(0, q).Split("\\" + fileName).First();
                            folderDirectory = await DocumentsLibrary.GetFolderAsync(newpath);
                        }
                    }
                    StorageFile sampleFile = await folderDirectory.GetFileAsync(fileName);

                    Stream outStream = await sampleFile.OpenStreamForReadAsync();
                    image = ImageSource.FromStream(() => outStream);
                }
                catch (Exception ex)
                {
                }
            }
            return image;
        }

        public async Task<string> SavePictureToDisk(string path, string filename, byte[] data)
        {
            if (!Regex.IsMatch(filename.ToLower(), @".jpg|.jpeg|.png|.gif$"))
                filename += ".jpg";

            string filepath = path + "\\" + filename;

            StorageFolder picturesDirectory = KnownFolders.PicturesLibrary;
            StorageFolder folderDirectory = picturesDirectory;

            // Get the folder or create it if necessary
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    int pos = path.IndexOf("Photo Store");
                    if (pos >= 0)
                    {
                        string newpath = path.Remove(0, pos);
                        folderDirectory = await picturesDirectory.GetFolderAsync(newpath);
                    }
                }
                catch (Exception ex)
                {
                  //  await Application.Current.MainPage.DisplayAlert("SavePictureToDisk 1", ex.Message + ex.StackTrace, "Ok");
                }

                if (folderDirectory == null)
                {
                    try
                    {
                        folderDirectory = await picturesDirectory.CreateFolderAsync(path);
                    }
                    catch (Exception ex)
                    {
                       // await Application.Current.MainPage.DisplayAlert("SavePictureToDisk 2", ex.Message + ex.StackTrace , "Ok");
                    }
                }
            }

            try
            {

                // Create the file.
                StorageFile storageFile = await folderDirectory.CreateFileAsync(filename,
                                                    CreationCollisionOption.ReplaceExisting);
                filepath = storageFile.Path.ToString();

                // Convert byte[] to Windows buffer and write it out.
                IBuffer buffer = WindowsRuntimeBuffer.Create(data, 0, data.Length, data.Length);
                await FileIO.WriteBufferAsync(storageFile, buffer);
            }
            catch (Exception ex)
            {
              //  await Application.Current.MainPage.DisplayAlert("SavePictureToDisk 3", ex.Message + ex.StackTrace , "Ok");
            }

            return filepath;
        }
        public async Task<string> GenerateImagePath(string path)
        {
            StorageFolder PicturesLibrary = KnownFolders.PicturesLibrary;
            StorageFolder folderDirectory = PicturesLibrary;
            string Result = string.Empty;

            // Get the folder or create it if necessary
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    folderDirectory = await PicturesLibrary.CreateFolderAsync(path, CreationCollisionOption.OpenIfExists);
                    Result = folderDirectory.Path;
                }
                catch (Exception ex)
                {
                    Result = null;
                }

            }
            return Result;
        }
        public async Task<List<string>> GetAllImages(string path)
        {
            StorageFolder PicturesLibrary = KnownFolders.PicturesLibrary;
            StorageFolder folderDirectory = PicturesLibrary;

            int pos = path.IndexOf("Photo Store");
            if (pos >= 0)
            {
                string newpath = path.Remove(0, pos);
                folderDirectory = await PicturesLibrary.GetFolderAsync(newpath);
            }
            var files = await folderDirectory.GetFilesAsync();
            List<string> imageFiles = new List<string>();
            foreach (string filename in files.Select(i => i.Path))
            {
                if (Regex.IsMatch(filename.ToLower(), @".jpg|.jpeg|.png|.gif$"))
                    imageFiles.Add(filename.Replace(path + "\\", ""));
            }

            return imageFiles;
        }

        public async Task<bool> RenameImage(string path, string CurrentName, string NewName)
        {
            StorageFolder PicturesLibrary = KnownFolders.PicturesLibrary;
            StorageFolder folderDirectory = PicturesLibrary;

            int pos = path.IndexOf("Photo Store");
            if (pos >= 0)
            {
                string newpath = path.Remove(0, pos);
                folderDirectory = await PicturesLibrary.GetFolderAsync(newpath);
            }
            var files = await folderDirectory.GetFilesAsync();

            foreach (var file in files)
            {
                if (Regex.IsMatch(file.Path, @".jpg|.jpeg|.png|.gif$"))
                {
                    var imgname = file.Path.Split("\\").Last();
                    if (imgname == CurrentName)
                        await file.RenameAsync(NewName);
                }
                //imageFiles.Add(filename.Replace(path + "\\", ""));
            }
            return true;
        }
        public async Task<string> SavePDFToDisk(string Folder, string fileName, byte[] pdfData)
        {
            try
            {
                
                StorageFolder DocumentsLibrary = KnownFolders.DocumentsLibrary;
                StorageFolder folderDirectory = DocumentsLibrary;
                fileName = fileName.Replace("PNG", "jpg");
                fileName = fileName.Replace("png", "jpg");
                fileName = fileName.Replace("jpeg", "jpg");
                fileName = fileName.Replace("JPEG", "jpg");
                ////get asets folder
                //StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                //StorageFolder assetsFolder = await appInstalledFolder.CreateFolderAsync("Assets\\Content");

                // Get the folder or create it if necessary                
                if (!string.IsNullOrEmpty(Folder))
                {
                    try
                    {
                        folderDirectory = await DocumentsLibrary.CreateFolderAsync(Folder, CreationCollisionOption.OpenIfExists);
                    }
                    catch (Exception ex)
                    {

                    }

                }
                // Create the file.
                StorageFile storageFile = await folderDirectory.CreateFileAsync(fileName,
                                                    CreationCollisionOption.ReplaceExisting);
                filepath = storageFile.Path.ToString();

                // Convert byte[] to Windows buffer and write it out.
                //IBuffer buffer = WindowsRuntimeBuffer.Create(pdfData, 0, pdfData.Length, pdfData.Length);
                await FileIO.WriteBytesAsync(storageFile, pdfData);
                await FileIO.ReadBufferAsync(storageFile);


            }
            catch (Exception ex)
            {

            }

            return filepath;
        }

        public async Task<bool> DeleteImage(string path)
        {

            string newpath = string.Empty, fileName = string.Empty;
            bool result = false;
            StorageFolder picturesDirectory = KnownFolders.PicturesLibrary;
            StorageFolder folderDirectory = picturesDirectory;
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    path = path.Replace("/", "\\");
                    fileName = path.Split("\\").Last();

                    int p = path.IndexOf("Photo Store");
                    if (p >= 0)
                    {
                        newpath = path.Remove(0, p).Split("\\" + fileName).First();
                        folderDirectory = await picturesDirectory.GetFolderAsync(newpath);
                    }
                    var files = await folderDirectory.GetFilesAsync();

                    foreach (var file in files)
                    {
                        string fpath = file.Path;
                        if (Regex.IsMatch(fpath, @".jpg|.jpeg|.png|.gif$"))
                        {
                            string imageFiles = fpath.Split("\\").Last();
                            if (fileName == imageFiles)
                            {
                                await file.DeleteAsync(StorageDeleteOption.Default);
                                result = true;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return result;
        }

        public async Task<bool> DirectoryExists(string Folder)
        {


            StorageFolder picturesDirectory = KnownFolders.PicturesLibrary;
            StorageFolder folderDirectory = picturesDirectory;

            try
            {
                folderDirectory = await picturesDirectory.GetFolderAsync(Folder);
                if (folderDirectory != null) { return true; }
                else return false;
            }
            catch(Exception ex)
            {
              //  await Application.Current.MainPage.DisplayAlert("Exception From DirectoryExists", ex.Message + ex.StackTrace , "Ok");
                return false;
            }
        }

        public async Task<string[]> GetDirectories(string Folder)
        {

            //StorageFolder picturesDirectory = KnownFolders.PicturesLibrary;
            //StorageFolder folderDirectory = picturesDirectory;
            
            StorageFolder StorageLibrary;
            StorageFolder folderDirectory;

            StorageLibrary = KnownFolders.PicturesLibrary;

            try
            {
                //folderDirectory = await picturesDirectory.GetFolderAsync(Folder);

                //return System.IO.Directory.GetDirectories(folderDirectory.Path);


                folderDirectory = StorageLibrary;
                folderDirectory = await folderDirectory.GetFolderAsync(Folder);

                IReadOnlyList<StorageFolder> folders = await folderDirectory.GetFoldersAsync();
                string[] Collection = new string[folders.Count];
                int i = 0;
                foreach (StorageFolder folder in folders)
                {
                    try
                    {
                        Collection[i++] = folder.Path;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                return Collection;
            }
            catch(Exception ex)
            {
              //  await Application.Current.MainPage.DisplayAlert("Exception From GetDirectories", ex.Message + ex.StackTrace , "Ok");
                return new string[] { };
            }



        }

        public async Task<List<string>> GetImageFiles(string path)
        {
            //   var Files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
            //.Where(s => s.ToUpper().EndsWith(".PNG") ||
            //s.ToUpper().EndsWith(".BMP") ||
            //s.ToUpper().EndsWith(".JPG") ||
            //s.ToUpper().EndsWith(".JPEG") ||
            //s.ToUpper().EndsWith(".GIF")
            //).ToList();

            //   return Files;

           // StorageFolder folderDirectory;

            StorageFolder folderDirectory = KnownFolders.PicturesLibrary;

            try
            {
                string newpath;// path.Split(folderDirectory.DisplayName + "\\").LastOrDefault();

                path = path.Replace("/", "\\");
                newpath = path.Split("\\").Last();
                int p = path.IndexOf("Photo Store");
                if (p >= 0)
                {
                    newpath = path.Remove(0, p);
                    folderDirectory = await folderDirectory.GetFolderAsync(newpath);
                }

               // folderDirectory = await folderDirectory.GetFolderAsync(newpath);
                IReadOnlyList<StorageFolder> folders = await folderDirectory.GetFoldersAsync();

                List<string> imageFiles = new List<string>();
                foreach (StorageFolder folder in folders)
                {
                    IReadOnlyList<StorageFolder> subfolders = await folder.GetFoldersAsync();
                    foreach (StorageFolder subfolder in subfolders)
                    {
                        var subfiles = await subfolder.GetFilesAsync();
                        foreach (string subfilename in subfiles.Select(i => i.Path))
                        {
                           // if (Regex.IsMatch(subfilename.ToUpper(), @".JPG|.JPEG|.PNG|.BMP|.GIF$"))
                                imageFiles.Add(subfilename);
                        }
                    }

                    var files = await folder.GetFilesAsync();
                    foreach (string filename in files.Select(i => i.Path))
                    {
                      // if (Regex.IsMatch(filename.ToUpper(), @".JPG|.JPEG|.PNG|.BMP|.GIF$"))
                            imageFiles.Add(filename);
                    }
                }
                return imageFiles;
            }
            catch (Exception ex)
            {
              //  await Application.Current.MainPage.DisplayAlert("Exception From GetDirectories", ex.Message + ex.StackTrace, "Ok");
                return null;
            }

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
            StorageFolder StorageLibrary;
            StorageFolder folderDirectory;
            path = path.Replace("/", "\\");
            var pictures = path;
            int p = pictures.IndexOf("Photo Store");
            if (p >= 0)
            {
                StorageLibrary = KnownFolders.PicturesLibrary;
            }
            else
            {
                StorageLibrary = KnownFolders.DocumentsLibrary;
            }
            folderDirectory = StorageLibrary;
            folderDirectory = await folderDirectory.GetFolderAsync(pictures);

            IReadOnlyList<StorageFolder> folders = await folderDirectory.GetFoldersAsync();

            foreach (StorageFolder folder in folders)
            {
                try
                {
                    await DeleteFolders(folder);
                    await folder.DeleteAsync();
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return result;
        }
        public async Task<bool> RemovePDFFilefromFolder(string path)
        {

            bool result = false;
            StorageFolder StorageLibrary;
            StorageFolder folderDirectory;
            path = path.Replace("/", "\\");
            var pictures = path;
            int p = pictures.IndexOf("Photo Store");
            if (p >= 0)
            {
                StorageLibrary = KnownFolders.PicturesLibrary;
            }
            else
            {
                StorageLibrary = KnownFolders.DocumentsLibrary;
            }
            folderDirectory = StorageLibrary;
            folderDirectory = await folderDirectory.GetFolderAsync(pictures);

            IReadOnlyList<StorageFile> Files = await folderDirectory.GetFilesAsync();

            foreach (StorageFile file in Files)
            {
                try
                {
                    if (Regex.IsMatch(file.Path.ToLower(), @".pdf$"))
                        await file.DeleteAsync();
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return result;
        }
        private async Task<bool> DeleteFolders(StorageFolder storageFolder)
        {

            try
            {
                IReadOnlyList<StorageFile> storageFiles = await storageFolder.GetFilesAsync();

                foreach (StorageFile storageFile in storageFiles)
                {
                    try { await storageFile.DeleteAsync(); }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }

                IReadOnlyList<StorageFolder> folders = await storageFolder.GetFoldersAsync();
                foreach (StorageFolder folder in folders)
                {
                    if (!(bool)await DeleteFolders(folder))
                        return false;

                    try { await folder.DeleteAsync(); }
                    catch (Exception ex)
                    {
                        return false;
                    }
                    await Task.Delay(10);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> PickFile(string path)
        {
            bool result = false;
            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker = new FileOpenPicker
                {
                    ViewMode = Windows.Storage.Pickers.PickerViewMode.List,
                    SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
                };
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".png");

                IReadOnlyList<StorageFile> fileList = await openPicker.PickMultipleFilesAsync();
                if (fileList != null)
                {
                    foreach (var file in fileList)
                    {
                        ImageSource image = null;

                        Stream outStream = await file.OpenStreamForReadAsync();
                        // image = ImageSource.FromStream(() => outStream);
                        byte[] resultBytes = new byte[outStream.Length];
                        resultBytes = GetResizeImage(resultBytes);
                        outStream.Read(resultBytes, 0, (int)outStream.Length);
                        string filename = file.Name;
                        SavePictureToDisk(path, filename, resultBytes);
                        result = true;
                    }

                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public async Task<bool> DWRPickFile(string path, string LineNumber, string JointNo)
        {
            bool result = false;
            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker = new FileOpenPicker
                {
                    ViewMode = Windows.Storage.Pickers.PickerViewMode.List,
                    SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
                };
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".png");

                IReadOnlyList<StorageFile> fileList = await openPicker.PickMultipleFilesAsync();
                if (fileList != null)
                {
                    LineNumber = String.IsNullOrEmpty(LineNumber) ? "0" : LineNumber;
                    JointNo = String.IsNullOrEmpty(JointNo) ? "000" : JointNo;

                    foreach (var file in fileList)
                    {
                        ImageSource image = null;

                        Stream outStream = await file.OpenStreamForReadAsync();
                        // image = ImageSource.FromStream(() => outStream);
                        byte[] resultBytes = new byte[outStream.Length];
                        outStream.Read(resultBytes, 0, (int)outStream.Length);
                        string filename = Path.GetFileNameWithoutExtension(file.Name) + "~" + LineNumber + "-" + JointNo + Path.GetExtension(file.Name);
                        SavePictureToDisk(path, filename, resultBytes);
                        result = true;
                    }

                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        public async Task<bool> DWRPickFile(string path)
        {
            bool result = false;
            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker = new FileOpenPicker
                {
                    ViewMode = Windows.Storage.Pickers.PickerViewMode.List,
                    SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
                };
                openPicker.FileTypeFilter.Add(".jpg");
                openPicker.FileTypeFilter.Add(".png");

                IReadOnlyList<StorageFile> fileList = await openPicker.PickMultipleFilesAsync();
                if (fileList != null)
                {
                    foreach (var file in fileList)
                    {
                        ImageSource image = null;

                        Stream outStream = await file.OpenStreamForReadAsync();
                        // image = ImageSource.FromStream(() => outStream);
                        byte[] resultBytes = new byte[outStream.Length];
                        resultBytes = GetResizeImage(resultBytes);
                        outStream.Read(resultBytes, 0, (int)outStream.Length);
                        string filename = Path.GetFileNameWithoutExtension(file.Name) + Path.GetExtension(file.Name);
                        SavePictureToDisk(path, filename, resultBytes);
                        result = true;
                    }

                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public async Task<byte[]> ReadBytes(string path)
        {
            byte[] result = null;
            string newpath = string.Empty, fileName = string.Empty;
            try
            {

                StorageFolder picturesDirectory = KnownFolders.PicturesLibrary;
                StorageFolder folderDirectory = picturesDirectory;
                // string newpath = path.Split(storageFolder.DisplayName + "\\").LastOrDefault().Replace("\\" + Path.GetFileName(path), "");

                path = path.Replace("/", "\\");
                fileName = path.Split("\\").Last();
                int p = path.IndexOf("Photo Store");
                if (p >= 0)
                {
                    newpath = path.Remove(0, p).Split("\\" + fileName).First();
                    folderDirectory = await picturesDirectory.GetFolderAsync(newpath);
                }
                else
                {
                    newpath = path.Split(picturesDirectory.DisplayName + "\\").LastOrDefault().Replace("\\" + Path.GetFileName(path), "");
                    folderDirectory = await picturesDirectory.GetFolderAsync(newpath);
                }

               // folderDirectory = await picturesDirectory.GetFolderAsync(newpath);
                StorageFile sampleFile = await folderDirectory.GetFileAsync(Path.GetFileName(path));



                using (Stream stream = await sampleFile.OpenStreamForReadAsync())
                {
                    using (var memoryStream = new MemoryStream())
                    {

                        stream.CopyTo(memoryStream);
                        result = memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        public async Task<string> GetIWPFileLocation(string path)
        {
            try
            {

                StorageFolder DocumentsLibrary = KnownFolders.DocumentsLibrary;

                path = path.Replace("/", "\\");
                // string fileName = path.Split("\\").Last();
                List<string> FolderPath = path.Split("\\").ToList();
                //FolderPath.RemoveAt(FolderPath.Count-1);

                foreach (string item in FolderPath)
                    DocumentsLibrary = await DocumentsLibrary.GetFolderAsync(item);

                filepath = DocumentsLibrary.Path;


            }
            catch (Exception ex)
            {

            }

            return filepath;
        }
        public async Task<Xamarin.Forms.ImageSource> GetThumbnail(string path)
        {

            int reqWidth = 100;
            int reqHeight = 100;

            StorageFolder storageFolder = KnownFolders.PicturesLibrary;
            string newpath = path.Split(storageFolder.DisplayName + "\\").LastOrDefault().Replace("\\" + Path.GetFileName(path), "");

            storageFolder = await storageFolder.GetFolderAsync(newpath);
            StorageFile imagefile = await storageFolder.GetFileAsync(Path.GetFileName(path));


            //open file as stream
            using (IRandomAccessStream fileStream = await imagefile.OpenAsync(FileAccessMode.ReadWrite))
            {
                var decoder = await BitmapDecoder.CreateAsync(fileStream);

                var resizedStream = new InMemoryRandomAccessStream();

                BitmapEncoder encoder = await BitmapEncoder.CreateForTranscodingAsync(resizedStream, decoder);
                double widthRatio = (double)reqWidth / decoder.PixelWidth;
                double heightRatio = (double)reqHeight / decoder.PixelHeight;

                double scaleRatio = Math.Min(widthRatio, heightRatio);

                if (reqWidth == 0)
                    scaleRatio = heightRatio;

                if (reqHeight == 0)
                    scaleRatio = widthRatio;

                uint aspectHeight = (uint)Math.Floor(decoder.PixelHeight * scaleRatio);
                uint aspectWidth = (uint)Math.Floor(decoder.PixelWidth * scaleRatio);

                encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Linear;

                encoder.BitmapTransform.ScaledHeight = aspectHeight;
                encoder.BitmapTransform.ScaledWidth = aspectWidth;

                await encoder.FlushAsync();
                resizedStream.Seek(0);
                var outBuffer = new byte[resizedStream.Size];
                await resizedStream.ReadAsync(outBuffer.AsBuffer(), (uint)resizedStream.Size, InputStreamOptions.None);


                return Xamarin.Forms.ImageSource.FromStream(() => new MemoryStream(outBuffer));

            }
        }
        public async Task<string> ConvertPDFtoByte(string FilePath)
        {
            try
            {
                string resultBase64 = "";
                StorageFolder storageFolder = KnownFolders.PicturesLibrary;
                string newpath = FilePath.Split(storageFolder.DisplayName + "\\").LastOrDefault().Replace("\\" + Path.GetFileName(FilePath), "");

                storageFolder = await storageFolder.GetFolderAsync(newpath);
                StorageFile sampleFile = await storageFolder.GetFileAsync(Path.GetFileName(FilePath));

                // Load selected PDF file from the file picker.
                PdfDocument pdfDocument = await PdfDocument.LoadFromFileAsync(sampleFile);

                if (pdfDocument != null && pdfDocument.PageCount > 0)
                {
                    for (int pageIndex = 0; pageIndex < pdfDocument.PageCount; pageIndex++)
                    {
                        //Get page from a PDF file.
                        var pdfPage = pdfDocument.GetPage((uint)pageIndex);

                        if (pdfPage != null)
                        {
                            //Create temporary folder to store images.
                            StorageFolder tempFolder = ApplicationData.Current.TemporaryFolder;
                            //Create image file.
                            StorageFile destinationFile = await KnownFolders.CameraRoll.CreateFileAsync(Guid.NewGuid().ToString() + ".jpg");
                            tempPath = destinationFile.Path;
                            if (destinationFile != null)
                            {
                                IRandomAccessStream randomStream = await destinationFile.OpenAsync(FileAccessMode.ReadWrite);
                                //Crerate PDF rendering options
                                PdfPageRenderOptions pdfPageRenderOptions = new PdfPageRenderOptions();

                                pdfPageRenderOptions.DestinationWidth = (uint)(2048);

                                // Render the PDF's page as stream.
                                await pdfPage.RenderToStreamAsync(randomStream, pdfPageRenderOptions);

                                await randomStream.FlushAsync();
                                //Dispose the random stream
                                randomStream.Dispose();
                                //Dispose the PDF's page.
                                pdfPage.Dispose();
                            }


                            var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(destinationFile.Path);
                            var filebuffer = await file.OpenAsync(FileAccessMode.Read);
                            var reader = new DataReader(filebuffer.GetInputStreamAt(0));
                            var bytes = new byte[filebuffer.Size];
                            await reader.LoadAsync((uint)filebuffer.Size);
                            reader.ReadBytes(bytes);

                            resultBase64 = Convert.ToBase64String(bytes);

                        }
                    }
                }

                return resultBase64;

            }
            catch (Exception e)
            {
                return "";
            }

        }

        public async Task<string> SaveImageAsPdf(Stream _stream)
        {

            string result = "";
            //StorageFolder storageFolder = KnownFolders.PicturesLibrary;
            //string newpath = FilePath.Split(storageFolder.DisplayName + "\\").LastOrDefault().Replace("\\" + Path.GetFileName(FilePath), "");
            //StorageFolder tempFolder = ApplicationData.Current.TemporaryFolder;
            //Create image file.
            StorageFile destinationFile = await KnownFolders.CameraRoll.CreateFileAsync(Guid.NewGuid().ToString() + ".pdf");

            //var path = Path.Combine(storageFolder.Path, $"{Guid.NewGuid().ToString("N")}.pdf");

            var metadata = new SKDocumentPdfMetadata
            {
                Author = "Cool Developer",
                Creation = DateTime.Now,
                Creator = "Cool Developer Library",
                Keywords = "SkiaSharp, Sample, PDF, Developer, Library",
                Modified = DateTime.Now,
                Producer = "SkiaSharp",
                Subject = "SkiaSharp Sample PDF",
                Title = "Sample PDF",
            };


            var stream = new SKFileWStream(destinationFile.Path);
            var document = SKDocument.CreatePdf(stream, metadata);

            var paint = new SKPaint
            {
                TextSize = 64.0f,
                IsAntialias = true,
                Color = (SKColor)0xFF9CAFB7,
                IsStroke = true,
                StrokeWidth = 3,
                TextAlign = SKTextAlign.Center,


            };
            var width = 840;
            var height = 1188;
            // draw page 1
            try
            {
                //    SKRect destdsfRect = new SKRect(0, 0, width, height);
                //    using (var pdfCanvas = document.BeginPage(width, height, destdsfRect))
                //    {
                //        // draw button
                //        var nextPagePaint = new SKPaint
                //        {
                //            IsAntialias = true,
                //            TextSize = 16,
                //            Color = SKColors.OrangeRed
                //        };
                //        var nextText = "Next Page >>";
                //        var btn = new SKRect(width - nextPagePaint.MeasureText(nextText) - 24, 0, width, nextPagePaint.TextSize + 24);
                //        pdfCanvas.DrawText(nextText, btn.Left + 12, btn.Bottom - 12, nextPagePaint);
                //        // make button link
                //        SKRect destRect = new SKRect(0, 0, _stream.Width, _stream.Height);
                //        pdfCanvas.DrawImage(_stream, destRect);

                //        // draw contents
                //        pdfCanvas.DrawText("...PDF 1/2...", width / 2, height / 4, paint);
                //        document.EndPage();
                //    }
                //} catch (Exception x)
                //{


                //var documdent = new PdfSharp.Pdf.PdfDocument();
                //var page = documdent.AddPage();
                //XImage img = XImage.FromFile(tempPath);
                //var gfx = XGraphics.FromPdfPage(page);
                ////var font = new XFont("Verdana", 20);
                //gfx.DrawImage(img, 10, 130);
                //documdent.Save(Path.Combine(Path.GetTempPath(), "test.pdf"));
                //return null;
            }
            catch (Exception E)
            { }
            // end the doc
            document.Close();
            return result;
        }
        public async Task<List<string>> GetPdfFiles(string path)
        {
            StorageFolder PicturesLibrary = KnownFolders.DocumentsLibrary;
            StorageFolder folderDirectory = PicturesLibrary;

            int pos = path.IndexOf("DWR");
            if (pos >= 0)
            {
                string newpath = path.Remove(0, pos);
                folderDirectory = await PicturesLibrary.GetFolderAsync(newpath);
            }
            var files = await folderDirectory.GetFilesAsync();
            List<string> imageFiles = new List<string>();
            foreach (string filename in files.Select(i => i.Path))
            {
                if (Regex.IsMatch(filename.ToLower(), @".pdf$"))
                    imageFiles.Add(filename.Replace(path + "\\", ""));
            }

            return imageFiles;
        }

        public async Task<string> PickPdf(string path)
        {
            string result = "";
            try
            {
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker = new FileOpenPicker
                {
                    ViewMode = Windows.Storage.Pickers.PickerViewMode.List,
                    SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
                };
                openPicker.FileTypeFilter.Add(".pdf");


                IReadOnlyList<StorageFile> fileList = await openPicker.PickMultipleFilesAsync();
                if (fileList != null)
                {
                    foreach (var file in fileList)
                    {
                        ImageSource image = null;

                        Stream outStream = await file.OpenStreamForReadAsync();
                        // image = ImageSource.FromStream(() => outStream);
                        byte[] resultBytes = new byte[outStream.Length];
                        outStream.Read(resultBytes, 0, (int)outStream.Length);
                        string filename = file.Name;
                        result = await SavePDFToDisk("DWR\\JointDetails\\001\\Documents", filename, resultBytes);

                    }

                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public async Task<string> GeneratePdfPath(string path)
        {
            StorageFolder PicturesLibrary = KnownFolders.DocumentsLibrary;
            StorageFolder folderDirectory = PicturesLibrary;
            string Result = string.Empty;

            // Get the folder or create it if necessary
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    folderDirectory = await PicturesLibrary.CreateFolderAsync(path, CreationCollisionOption.OpenIfExists);
                    Result = folderDirectory.Path;
                }
                catch (Exception ex)
                {
                    Result = null;
                }

            }
            return Result;
        }

        public async Task<FileData> PickPDF(string Folders)
        {
            try
            {
                //FileData fileData = await CrossFilePicker.Current.PickFile();

                return await CrossFilePicker.Current.PickFile();
            }
            catch (Exception e)
            {
                return new FileData();
            }
        }
        public byte[] GetResizeImage(byte[] imageAsByte)
        {
            long bytesize = imageAsByte.Length;
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = Convert.ToDouble(bytesize);
            int order = 0;
            while (len >= 1024D && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            //string.Format(CultureInfo.CurrentCulture, "{0:0.##} {1}", len, sizes[order]);
            if (order >= 2 && len > 1.0)
            {
                var imgResizer = DependencyService.Get<IImageResizer>();
                imageAsByte = imgResizer.ResizeImage(imageAsByte, 1024, 1024);
                return GetResizeImage(imageAsByte);
            }
            else
                return imageAsByte;
        }
    }
}

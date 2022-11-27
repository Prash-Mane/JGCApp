using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Pdf;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using JGC.Common.Interfaces;
using JGC.Droid.DependancyObjects;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(SaveFiles))]
namespace JGC.Droid.DependancyObjects
{
    public class SaveFiles : Java.Lang.Object, ISaveFiles
    {
        string filePath;
        public async Task<string> SavePictureToDisk(string path, string filename, byte[] imageData)
        {
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            string externalStorageState = global::Android.OS.Environment.ExternalStorageDirectory.Path;
            var pictures = path;//externalStorageState + "/" + Folder.Replace("\\", "/");
            var directory = new Java.IO.File(pictures);
            if (!directory.Exists())
                directory.Mkdirs();

            //adding a time stamp time file name to allow saving more than one image... otherwise it overwrites the previous saved image of the same name
            if (!Regex.IsMatch(filename.ToLower(), @".jpg|.jpeg|.png|.gif$"))
                filename = filename + ".jpg";
             filePath = System.IO.Path.Combine(pictures, filename);
            try
            {
                System.IO.File.WriteAllBytes(filePath, imageData);
                //mediascan adds the saved image into the gallery
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(filePath)));
                Xamarin.Forms.Forms.Context.SendBroadcast(mediaScanIntent);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }

            return filePath;
        }
        public async Task<ImageSource> GetImage(string path)
        {
            // Open the photo and put it in a Stream to return    
            string externalStorageState = global::Android.OS.Environment.ExternalStorageDirectory.Path;
           // var memoryStream = new MemoryStream();
            Image image = new Image();
            // string _path = externalStorageState + "/" + path.Replace("\\", "/");
            try
            {
                image = new Image
                {
                    Source = ImageSource.FromFile(path)
                };
            }
            catch(Exception Ex)
            {

            }
            return image.Source;
        }
        public async Task<string> GenerateImagePath(string Folders)
        {
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            string externalStorageState = global::Android.OS.Environment.ExternalStorageDirectory.Path;
            var pictures = externalStorageState + "/" + Folders.Replace("\\", "/");
            var directory = new Java.IO.File(pictures);
            if (!directory.Exists())
                directory.Mkdirs();
            return directory.Path;
        }

        public async Task<List<string>> GetAllImages(string path)
        {
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            string externalStorageState = global::Android.OS.Environment.ExternalStorageDirectory.Path;
            var pictures = path;//externalStorageState + "/" + Folders.Replace("\\", "/");
            var directory = new Java.IO.File(pictures);
            if (!directory.Exists())
                directory.Mkdirs();
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            List<string> imageFiles = new List<string>();
            foreach (string filename in files)
            {
                if (Regex.IsMatch(filename.ToLower(), @".jpg|.jpeg|.png|.gif$"))
                    imageFiles.Add(filename.Replace(path+"/", ""));
            }
            return imageFiles;
        }
        public async Task<string> SavePDFToDisk(string Folders, string fileName, byte[] pdfData)
        {
            string fileLocation = string.Empty;
            Android.Net.Uri uri;
            try
            {             
               string externalStorageState = global::Android.OS.Environment.ExternalStorageDirectory.Path;
                string application = "";
                fileLocation = System.IO.Path.Combine(externalStorageState, Folders.Replace("\\", "/"));
                Directory.CreateDirectory(fileLocation);

                string extension = System.IO.Path.GetExtension(fileName);

                switch (extension.ToLower())
                {
                    case ".pdf":
                        application = "application/pdf";
                        break;
                    default:
                        application = "*/*";
                        break;
                }
                fileLocation += "/" + fileName;
                File.WriteAllBytes(fileLocation, pdfData);

                Java.IO.File file = new Java.IO.File(fileLocation);
                file.SetReadable(true);
                uri = Android.Net.Uri.FromFile(file);
                Intent intent = new Intent(Intent.ActionView);
                intent.SetDataAndType(uri, application);
                intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
              

            }
            catch (System.Exception e)
            {
                return null;
            }
            // return fileLocation;
            return uri.Path;

        }   
        public void OpenFile(string filePath, string filename)
        {

            var bytes = File.ReadAllBytes(filePath);

            //Copy the private file's data to the EXTERNAL PUBLIC location
            string externalStorageState = global::Android.OS.Environment.ExternalStorageState;
            string application = "";

            string extension = System.IO.Path.GetExtension(filePath);

            switch (extension.ToLower())
            {               
                case ".pdf":
                    application = "application/pdf";
                    break;               
                default:
                    application = "*/*";
                    break;
            }
            var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/" + filename + extension;
            File.WriteAllBytes(externalPath, bytes);

            Java.IO.File file = new Java.IO.File(externalPath);
            file.SetReadable(true);
            //Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + filePath);
            Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, application);
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

            try
            {
                Xamarin.Forms.Forms.Context.StartActivity(intent);
            }
            catch (Exception)
            {
                Toast.MakeText(Xamarin.Forms.Forms.Context, "There's no app to open dpf files", ToastLength.Short).Show();
            }
        }

        public async Task<bool> DeleteImage(string path)
        {
            bool result=false;

            if (File.Exists(path))
            {
                System.IO.File.Delete(path);
                result = true;
            }         

            return result;
        }

       public async Task<bool> RenameImage(string path, string CurrentName, string NewName)
        {
            bool result = false;
            var pictures = path;
          
            var directory = new Java.IO.File(pictures);
            if (directory.Exists())
            {
                var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                foreach (string filename in files)
                {
                    if (Regex.IsMatch(filename, @".jpg|.jpeg|.png|.gif$"))
                    {
                        if (filename.Replace(path + "/", "") == CurrentName)
                        {
                            Java.IO.File from = new Java.IO.File(directory, CurrentName);
                            Java.IO.File to = new Java.IO.File(directory, NewName);
                            if (from.Exists())
                                from.RenameTo(to);
                            result = true;
                        }
                    }
                }
            }
            return result;
        }
        public async Task<bool> DirectoryExists(string Folder)
        {

            string externalStorageState = global::Android.OS.Environment.ExternalStorageDirectory.Path;
            var pictures = externalStorageState + "/" + Folder.Replace("\\", "/");
            var directory = new Java.IO.File(pictures);
            if (directory.Exists())
                return true;
            else
                return false;
        }

        public async Task<string[]> GetDirectories(string Folder)
        {
            string externalStorageState = global::Android.OS.Environment.ExternalStorageDirectory.Path;
            var pictures = externalStorageState + "/" + Folder.Replace("\\", "/");

            return System.IO.Directory.GetDirectories(pictures);

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
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            string externalStorageState = global::Android.OS.Environment.ExternalStorageDirectory.Path;
            var pictures = externalStorageState + "/" + path.Replace("\\", "/");

            var directory = new Java.IO.File(pictures);
            if (directory.Exists())
            {
                DirectoryInfo directoryinfo = new DirectoryInfo(directory.Path);
                foreach (DirectoryInfo DR in directoryinfo.GetDirectories())
                {
                    DR.Delete(true);
                }
            }
            return result;
        }

        public async Task<bool> PickFile(string path)
        {
            bool result = false;
            byte[] imageData;
            try
            {
                //ImageSource image;
                //FileData filedata = new FileData();
                //filedata = await CrossFilePicker.Current.PickFile();                

                //if (filedata == null)
                //    return result = true;
                //if (filedata != null)
                //{
                //    string filename = filedata.FileName;
                //    byte[] imageData = filedata.DataArray;

                //    if (filedata.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)
                //        || filedata.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                //    {
                //        SavePictureToDisk(path, filename, imageData);
                //        result = true;
                //    }
                //} 

                ImageSource image;
                string filename = string.Empty;
                PickMediaOptions mediaOptions = new PickMediaOptions();

                MediaFile files = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
                if (files == null)
                    return result = false;
                if (files != null)
                {
                    filename = files.Path.Split("/").Last();

                    using (var memoryStream = new MemoryStream())
                    {
                        files.GetStream().CopyTo(memoryStream);
                        files.Dispose();
                        imageData = GetResizeImage(memoryStream.ToArray());
                    }
                    SavePictureToDisk(path, filename, imageData);
                    result = true;

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
                    return result = false;
                if (files != null)
                {
                    filename = files.Path.Split("/").Last();

                    using (var memoryStream = new MemoryStream())
                    {
                        files.GetStream().CopyTo(memoryStream);
                        files.Dispose();
                        imageData = memoryStream.ToArray();
                    }
                    SavePictureToDisk(path, System.IO.Path.GetFileNameWithoutExtension(filename) + "~" + LineNumber + "-" + JointNo + System.IO.Path.GetExtension(filename), imageData);
                    result = true;

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
                    return result = false;
                if (files != null)
                {
                    filename = files.Path.Split("/").Last();

                    using (var memoryStream = new MemoryStream())
                    {
                        files.GetStream().CopyTo(memoryStream);
                        files.Dispose();
                        imageData = GetResizeImage(memoryStream.ToArray());
                    }
                    SavePictureToDisk(path, System.IO.Path.GetFileNameWithoutExtension(filename) + System.IO.Path.GetExtension(filename), imageData);
                    result = true;

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
            bool result = false;
            byte[] imageData;
            try
            {
                ImageSource image;
                string filename = string.Empty;
                using (var streamReader = new StreamReader(path))
                {
                    var bytes = default(byte[]);
                    using (var memstream = new MemoryStream())
                    {
                        streamReader.BaseStream.CopyTo(memstream);
                        imageData = memstream.ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                imageData = null;
            }

            return imageData;
        }
        public async Task<string> GetIWPFileLocation(string path)
        {
            string fileLocation = string.Empty;
            try
            {
               
                Android.Net.Uri uri;
               
                    string externalStorageState = global::Android.OS.Environment.ExternalStorageDirectory.Path;
                    string application = "";
                    fileLocation = System.IO.Path.Combine(externalStorageState, path.Replace("\\", "/"));


                }
            catch (Exception ex)
            {
                return null;
            }

            return fileLocation;
        }

        public async Task<bool> RemovePDFFilefromFolder(string path)
        {
            bool result = false;
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            string externalStorageState = global::Android.OS.Environment.ExternalStorageDirectory.Path;
            var pictures = externalStorageState + "/" + path.Replace("\\", "/");

            var directory = new Java.IO.File(pictures);
            if (directory.Exists())
            {
                DirectoryInfo directoryinfo = new DirectoryInfo(directory.Path);
                foreach (FileInfo DR in directoryinfo.GetFiles())
                {
                    File.Delete(DR.ToString());
                }
            }
            return result;
        }
        public async Task<List<string>> GetPdfFiles(string path)
        {
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            string externalStorageState = global::Android.OS.Environment.ExternalStorageDirectory.Path;
            var pictures = path;//externalStorageState + "/" + Folders.Replace("\\", "/");
            var directory = new Java.IO.File(pictures);
            if (!directory.Exists())
                directory.Mkdirs();
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            List<string> imageFiles = new List<string>();
            foreach (string filename in files)
            {
                if (Regex.IsMatch(filename, @".pdf"))
                    imageFiles.Add(filename.Replace(path + "/", ""));
            }
            return imageFiles;
        }

        public async Task<string> PickPdf(string path)
        {
            string result = "";
            byte[] imageData;
            try
            {

                ImageSource image;
                string filename = string.Empty;
                PickMediaOptions mediaOptions = new PickMediaOptions();

                MediaFile files = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
                if (files == null)
                    return result = "";
                if (files != null)
                {
                    filename = files.Path.Split("/").Last();

                    using (var memoryStream = new MemoryStream())
                    {
                        files.GetStream().CopyTo(memoryStream);
                        files.Dispose();
                        imageData = memoryStream.ToArray();
                    }
                    result = await SavePDFToDisk("DWR\\JointDetails\\001\\Documents", filename, imageData);


                }
            }
            catch (Exception e)
            {

            }

            return result;
        }

        public async Task<string> GeneratePdfPath(string Folders)
        {
            var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments);
            string externalStorageState = global::Android.OS.Environment.ExternalStorageDirectory.Path;
            var pictures = externalStorageState + "/" + Folders.Replace("\\", "/");
            var directory = new Java.IO.File(pictures);
            if (!directory.Exists())
                directory.Mkdirs();
            return directory.Path;
        }

        public async Task<FileData> PickPDF(string Folders)
        {
            try
            {
                //FileData fileData = await CrossFilePicker.Current.PickFile();
                PickMediaOptions mediaOptions = new PickMediaOptions();
                return await CrossFilePicker.Current.PickFile();
                //throw new NotImplementedException();
            }
            catch (Exception e)
            {
                return new FileData();
            }
        }
        public Task<ImageSource> GetThumbnail(string path)
        {
            // byte[] thimbnail;
            int height = 500;
            int width = 500;
            int quality = 100;
            byte[] imageData = File.ReadAllBytes(path);
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);

            float oldWidth = (float)originalImage.Width;
            float oldHeight = (float)originalImage.Height;
            float scaleFactor = 0f;

            if (oldWidth > oldHeight)
            {
                scaleFactor = width / oldWidth;
            }
            else
            {
                scaleFactor = height / oldHeight;
            }

            float newHeight = oldHeight * scaleFactor;
            float newWidth = oldWidth * scaleFactor;

            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, false);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, quality, ms);
                // resizedImage.
                //thimbnail =  ms.ToArray();
                return Task.Delay(0).ContinueWith(t => ImageSource.FromStream(() => new MemoryStream(ms.ToArray())));// Task.Delay(10).ContinueWith(t => ms));
            }



            // throw new NotImplementedException();
        }
        public async Task<string> ConvertPDFtoByte(string Path)
        {
            string base64string = "";
            try
            {

                //initialize PDFRenderer by passing PDF file from location.
                PdfRenderer renderer = new PdfRenderer(GetSeekableFileDescriptor(Path));

                int pageCount = renderer.PageCount;
                for (int i = 0; i < pageCount; i++)
                {

                    // Use `openPage` to open a specific page in PDF.
                    var page = renderer.OpenPage(i);

                    //Creates bitmap
                    Bitmap bmp = Bitmap.CreateBitmap(page.Width * 3, page.Height * 3, Bitmap.Config.Argb8888);

                    //renderes page as bitmap, to use portion of the page use second and third parameter
                    page.Render(bmp, null, null, PdfRenderMode.ForDisplay);
                    //Save the bitmap
                    using (var stream = new MemoryStream())
                    {
                        bmp.Compress(Bitmap.CompressFormat.Png, 0, stream);

                        var bytes = stream.ToArray();
                        base64string = Convert.ToBase64String(bytes);
                    }
                    //SaveImage(bmp);
                    page.Close();
                }
                return base64string;

            }
            catch (Exception Ex)
            {
                return base64string;
            }


        }
        //Method to retrieve PDF file from the location
        private ParcelFileDescriptor GetSeekableFileDescriptor(string Path)
        {
            ParcelFileDescriptor fileDescriptor = null;
            try
            {
                string root = Android.OS.Environment.ExternalStorageDirectory.ToString() + "/Syncfusion/sample.pdf";
                fileDescriptor = ParcelFileDescriptor.Open(new Java.IO.File(Path.Replace("\\", "/")), ParcelFileMode.ReadOnly
                );
            }
            catch (FileNotFoundException e)
            {

            }
            return fileDescriptor;
        }

        public async Task<string> SaveImageAsPdf(Stream _stream)
        {
            string externalStorageState = global::Android.OS.Environment.ExternalStorageDirectory.Path;
            var pictures = externalStorageState + "/" + "pdf";

            PdfDocument doc = new PdfDocument();

            return "";
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
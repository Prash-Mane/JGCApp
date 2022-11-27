using JGC.Common.Interfaces;
using JGC.Models.Completions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JGC.Views.Completions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DocumentViewerPopup : PopupPage
    {
        List<ImageData> ImageList = new List<ImageData>();

        #region Properties
        private string image;
        public string Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged(); }
        }
        #endregion

        public DocumentViewerPopup(ImageData ImageData, List<ImageData> _ImageList)
        {
            InitializeComponent();
            Image = ImageData.ImagePath;
            BindImage(ImageData.ImagePath);
            ImageList = _ImageList;
        }

        private void BindImage(string imageData)
        {
            Image = imageData;
            var _file = Task.Run(async () => await getfile(imageData));
            Stream stream = new MemoryStream(_file.Result);
            ImgTestPAck.Source = ImageSource.FromStream(() => stream);
        }

        private void PrevButton_Clicked(object sender, EventArgs e)
        {
            var curentImageIndex = ImageList.IndexOf(ImageList.Where(x => x.ImagePath == Image).FirstOrDefault());
            if (curentImageIndex > 0)
                Image = ImageList[curentImageIndex - 1].ImagePath;
            BindImage(Image);
        }

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            var curentImageIndex = ImageList.IndexOf(ImageList.Where(x => x.ImagePath == Image).FirstOrDefault());
            if (curentImageIndex >= 0 && curentImageIndex < ImageList.Count - 1)
                Image = ImageList[curentImageIndex + 1].ImagePath;
            BindImage(Image);
        }

        private async Task<byte[]> getfile(string path)
        {
            byte[] FileBytes;
            if (Device.RuntimePlatform == Device.UWP)
                FileBytes = await DependencyService.Get<ISaveFiles>().ReadBytes(path);
            else
                FileBytes = File.ReadAllBytes(path);

            return FileBytes;
        }
    }
}
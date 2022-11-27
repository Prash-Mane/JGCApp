using Acr.UserDialogs;
using JGC.Common.Constants;
using JGC.Common.Helpers;
using JGC.Common.Interfaces;
using JGC.Common.Services;
using JGC.DataBase.DataTables;
using JGC.Models;
using JGC.Models.E_Reporter;
using JGC.UserControls.Touch;
using JGC.ViewModels.E_Reporter;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static JGC.Common.Enumerators.Enumerators;

namespace JGC.Views.E_Reporter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DWRPopup : PopupPage
    {
        private double width, height;
        public SQLiteConnection conn;
        //Camera Canvas
        SKBitmap CameraBitmap = new SKBitmap();
        TouchManipulationBitmap Camerabitmap;
        List<long> CameratouchIds = new List<long>();
        Dictionary<long, SKPath> _inProgressCameraPaths = new Dictionary<long, SKPath>();
        List<SKPath> _completedCameraPaths = new List<SKPath>();
        SKCanvas _saveCameraBitmapCanvas;
        private byte[] imageAsByte = null;
        private string CameraPunchID;
        T_PunchImage SelectedImage;
        List<long> touchIds = new List<long>();
        private string _filePath;
        public string GetImageType;
        private string InspectionPath;
        private bool SetPicker = false;

        public DWRPopup(string Type)
        {
            InitializeComponent();
            try
            {
                if (Type == "InspectionResult")
                {
                    Generatepath("VIDI");
                    List<string> VIDI = new List<string> { "- Select -", "Blank", "ACC", "REJ" };
                    VIDDL.ItemsSource = DIDDL.ItemsSource = VIDI;
                    VICommentDDL.ItemsSource = EReporterHelper.InspectResultFields.VICommentList;
                    VIDDL.SelectedItem = EReporterHelper.InspectResultFields.VI;
                    DIDDL.SelectedItem = EReporterHelper.InspectResultFields.DI;
                    VICommentDDL.SelectedItem = EReporterHelper.InspectResultFields.VI_Comment;
                    DICommentTxt.Text = EReporterHelper.InspectResultFields.DI_Comment;
                    RemarkTxt.Text = EReporterHelper.InspectResultFields.Remark;
                    StackInspectionResult.IsVisible = true;
                    StackJointDetials.IsVisible = false;
                }
                else
                {
                    SheetNoTxt.Text = EReporterHelper.JointDetailFields.SheetNo;
                    SizeTxt.Text = EReporterHelper.JointDetailFields.Size;
                    ThicknessTxt.Text = EReporterHelper.JointDetailFields.Thickness;
                    SpoolNoTxt.Text = EReporterHelper.JointDetailFields.SpoolNo;
                    WeldtypeTxt.Text = EReporterHelper.JointDetailFields.Weldtype;
                    LineClassTxt.Text = EReporterHelper.JointDetailFields.LineClass;

                    StackInspectionResult.IsVisible = false;
                    StackJointDetials.IsVisible = true;

                }
            }
            catch(Exception ex)
            {

            }
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                CameraGrid.HeightRequest = height;
                CameraGrid.WidthRequest = width;
              
            }
        }
        private async void InspectResultClicked(object sender, EventArgs e)
        {
            if((VIDDL.SelectedItem.ToString() == "REJ" || DIDDL.SelectedItem.ToString() == "REJ") && String.IsNullOrWhiteSpace(RemarkTxt.Text))
            {
                await UserDialogs.Instance.AlertAsync("Remark field is require", "Alert", "OK");
                return;
            }
            else if((VIDDL.SelectedItem.ToString() == "REJ"))
            {
                if (VICommentDDL.SelectedItem.ToString().Contains("- Select -"))
                {
                    await UserDialogs.Instance.AlertAsync("VI Comment field is require", "Alert", "OK");
                    return;
                }
            }
            else if ((DIDDL.SelectedItem.ToString() == "REJ") && String.IsNullOrWhiteSpace(DICommentTxt.Text))
            {
                await UserDialogs.Instance.AlertAsync("DI Comment field is require", "Alert", "OK");
                return;
            }

            EReporterHelper.InspectResultFields.VI = VIDDL.SelectedItem.ToString();
            EReporterHelper.InspectResultFields.DI = DIDDL.SelectedItem.ToString();
            EReporterHelper.InspectResultFields.VI_Comment = (T_RT_Defects)VICommentDDL.SelectedItem; 
            EReporterHelper.InspectResultFields.DI_Comment = DICommentTxt.Text;
            EReporterHelper.InspectResultFields.Remark = RemarkTxt.Text;
            
            await PopupNavigation.PopAsync(true);
        }

       
        private void CameraBtn(object sender, EventArgs e)
        {
            var obj = (TappedEventArgs)e;
            GetImageType = (string)obj.Parameter;
            if (GetImageType == "VIImages")
                Generatepath("VI");
            else
                Generatepath("DI");
            CameraPicker.ItemsSource = new List<string> { "- Select -", "Camera" };
            CameraPicker.SelectedItem = "- Select -";
            ListOfPhotoPicker.SelectedItem = "- Select -";
            StackInspectionResult.IsVisible = false;
            CameraGrid.IsVisible = true;
        }

       
        private async void JointDetailClicked(object sender, EventArgs e)
        { 
            EReporterHelper.JointDetailFields.SheetNo = SheetNoTxt.Text;
            EReporterHelper.JointDetailFields.Size = SizeTxt.Text;
            EReporterHelper.JointDetailFields.Thickness = ThicknessTxt.Text;
            EReporterHelper.JointDetailFields.SpoolNo = SpoolNoTxt.Text;
            EReporterHelper.JointDetailFields.Weldtype = WeldtypeTxt.Text;
            EReporterHelper.JointDetailFields.LineClass = LineClassTxt.Text;

            await PopupNavigation.PopAsync(true);
        }

        //Camera 
        #region Camera 
        private async void CameraPickerSelection(object sender, EventArgs e)
        {
            if (((JGC.UserControls.CustomControls.CustomPicker)sender).SelectedItem == null || ((JGC.UserControls.CustomControls.CustomPicker)sender).SelectedItem == "- Select -") return;
            var mediaFile = await TakePhotoAsync("CameraImage");
            UserDialogs.Instance.ShowLoading("Loading..!");
            if (_saveCameraBitmapCanvas != null)
            {
                _completedCameraPaths.Clear();
                _inProgressCameraPaths.Clear();
                CameraUpdateBitmap();
                CameracanvasView.InvalidateSurface();
            }
            if (mediaFile == null)
            {
                UserDialogs.Instance.HideLoading();
                return;
            }
            _filePath = mediaFile.Path;
            using (Stream stream = mediaFile.GetStream())
            {
                CameraBitmap = SKBitmap.Decode(stream);
                this.Camerabitmap = new TouchManipulationBitmap(CameraBitmap);
                this.Camerabitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                CameraUpdateBitmap();
            }
            IsVisible = true;
            UserDialogs.Instance.HideLoading();
        }
        private async void PhotoPickerSelection(object sender, EventArgs e)
        {
            if (((JGC.UserControls.CustomControls.CustomPicker)sender).SelectedItem == null || ((JGC.UserControls.CustomControls.CustomPicker)sender).SelectedItem == "- Select -") return;
            string pathconcat = Device.RuntimePlatform == Device.UWP ? "\\" : "/";
            byte[] data = await DependencyService.Get<ISaveFiles>().ReadBytes(InspectionPath + pathconcat + ((JGC.UserControls.CustomControls.CustomPicker)sender).SelectedItem);
            string base64 = Convert.ToBase64String(data);
            byte[] Base64Stream = Convert.FromBase64String(base64); 

            using (Stream stream = new MemoryStream(Base64Stream))
            {
                CameraBitmap = SKBitmap.Decode(stream);
                this.Camerabitmap = new TouchManipulationBitmap(CameraBitmap);
                this.Camerabitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
                CameraUpdateBitmap();
            }
        }
        private async void ClickedOn_AddFromFile(object sender, EventArgs e)
        {
            try
            {
                var PickFile = await DependencyService.Get<ISaveFiles>().DWRPickFile(InspectionPath);
                if (PickFile)
                {
                    SetPicker = true;
                    if (GetImageType == "VIImages")
                        Generatepath("VI");
                    else
                        Generatepath("DI");

                    await UserDialogs.Instance.AlertAsync("Added image file successfully", "Saved Image", "OK");
                }
            }
            catch (Exception EX)
            {
                await Application.Current.MainPage.DisplayAlert(null, "Error saving image to database.", "OK");
            }
            IsVisible = true;
            UserDialogs.Instance.HideLoading();
        }


        private void CameraBackBtn(object sender, EventArgs e)
        {
            CameraGrid.IsVisible = false;
            StackInspectionResult.IsVisible = true;
        }
        private void ClickedOn_ClearImage(object sender, EventArgs e)
        {
            CameraBitmap = new SKBitmap();
            _saveCameraBitmapCanvas = new SKCanvas(CameraBitmap);
            this.Camerabitmap = new TouchManipulationBitmap(CameraBitmap);
            this.Camerabitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;
            ListOfPhotoPicker.SelectedItem = "- Select -";
            CameraUpdateBitmap();
        }
        private async void ClickedOn_SaveImage(object sender, EventArgs e)
        {
            try
            {
                ResizeImageService obj = new ResizeImageService();
                using (SKImage image = SKImage.FromBitmap(CameraBitmap))
                {
                    if (image == null)
                        return;
                    SKData data = image.Encode();
                    data.Size.CompareTo(30);
                    imageAsByte = await obj.GetResizeImage(data.ToArray());
                }

                if (imageAsByte != null)
                {
                    string fileName = DateTime.Now.ToString(AppConstant.CameraDateFormat);
                    string path = await DependencyService.Get<ISaveFiles>().SavePictureToDisk(InspectionPath, fileName, imageAsByte.ToArray());
                    if (path != null)
                    {
                        SetPicker = true;
                        if (GetImageType == "VIImages")
                            Generatepath("VI");
                        else
                            Generatepath("DI");

                        await UserDialogs.Instance.AlertAsync("Successfully saved..!", null, "Ok");
                    }
                }
                else
                    await UserDialogs.Instance.AlertAsync("Please select camera and take a picture to save", null, "OK");

            }
            catch (Exception ex)
            {

            }
        }


        private void OnCameraCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            if (Camerabitmap != null)
            {
                Camerabitmap.TouchManager.Mode = TouchManipulationMode.IsotropicScale;

                // Display the bitmap
                Camerabitmap.Paint(canvas);
            }
        }
        private void OnCameraTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            Point pt = args.Location;
            SKPoint Camerapoint = new SKPoint((float)(CameracanvasView.CanvasSize.Width * pt.X / CameracanvasView.Width),
                                              (float)(CameracanvasView.CanvasSize.Height * pt.Y / CameracanvasView.Height));

            if (Camerabitmap == null)
                return;

            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (Camerabitmap.HitTest(Camerapoint))
                    {
                        touchIds.Add(args.Id);
                        Camerabitmap.ProcessTouchEvent(args.Id, args.Type, Camerapoint);
                        break;
                    }
                    break;

                case TouchActionType.Moved:
                    if (touchIds.Contains(args.Id))
                    {
                        Camerabitmap.ProcessTouchEvent(args.Id, args.Type, Camerapoint);
                        CameracanvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (touchIds.Contains(args.Id))
                    {
                        Camerabitmap.ProcessTouchEvent(args.Id, args.Type, Camerapoint);
                        touchIds.Remove(args.Id);
                        CameracanvasView.InvalidateSurface();
                    }
                    break;
            }
        }

        private async void CameraUpdateBitmap()
        {
            CameracanvasView.InvalidateSurface();
        }
        
        public async Task<MediaFile> TakePhotoAsync(string pageName)
        {
            MediaFile file = null;
            var denied = await CheckPermissions();
            if (denied)
            {
                await Application.Current.MainPage.DisplayAlert("Unable to take photos.", "Permissions Denied. Please modify app permisions in settings.", "OK");

                return null;
            }

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", "No camera available!", "OK");
                return null;
            }

            try
            {
                if (pageName == "CameraImage")
                {
                    file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Name = pageName,
                        Directory = "JGC",
                    });
                }
                else
                {
                    file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Name = pageName,
                    });
                }

            }
            catch (Exception ex)
            {

            }

            return file == null ? null : file;
        }
        public async Task<MediaFile> PickFileAsync()
        {
            var denied = await CheckPermissions();
            if (denied)
            {
                await Application.Current.MainPage.DisplayAlert("Unable to pick a file.", "Permissions Denied. Please modify app permisions in settings.", "OK");

                return null;
            }

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Gallery", "Picking a photo is not supported.", "OK");
                return null;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();
            return file == null ? null : file;
        }
        private async Task<bool> CheckPermissions()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                    storageStatus = results[Permission.Storage];
                    cameraStatus = results[Permission.Camera];
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                        storageStatus = results[Permission.Storage];
                        cameraStatus = results[Permission.Camera];
                    });
                }
            }

            return cameraStatus != PermissionStatus.Granted && storageStatus != PermissionStatus.Granted;
        }
        private string GetNewImageDisplayName()
        {
            try
            {
                var UserProject = conn.Query<T_UserProject>("Select * from T_UserProject where Project_ID = '" + Settings.ProjectID + "'");
                T_UserProject CurrentUserProject = UserProject.FirstOrDefault();
                DateTime CurrentUTC = DateTime.UtcNow;
                TimeZoneInfo ProjectTimeZone = TimeZoneInfo.FindSystemTimeZoneById(CurrentUserProject.TimeZone);
                CurrentUTC = TimeZoneInfo.ConvertTimeFromUtc(CurrentUTC, ProjectTimeZone);
                return CurrentUTC.ToString(AppConstant.CameraDateFormat);
            }
            catch (Exception e)
            {
                DateTime CurrentUTC = DateTime.UtcNow;
                TimeZoneInfo ProjectTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                CurrentUTC = TimeZoneInfo.ConvertTimeFromUtc(CurrentUTC, ProjectTimeZone);
                return CurrentUTC.ToString(AppConstant.CameraDateFormat);
            }

        }

        private void DIDDL_Selection(object sender, EventArgs e)
        {
            if (((JGC.UserControls.CustomControls.CustomPicker)sender).SelectedItem == "REJ")
            DICommentTxt.IsEnabled = true;
            else
            {
                DICommentTxt.Text = "";
                DICommentTxt.IsEnabled = false;
            }
        }

        private void VIDDL_Selection(object sender, EventArgs e)
        {
            if (((JGC.UserControls.CustomControls.CustomPicker)sender).SelectedItem == "REJ")
                VICommentDDL.IsEnabled = true;
            else
            {
                VICommentDDL.SelectedItem = EReporterHelper.InspectResultFields.VICommentList.FirstOrDefault();
                VICommentDDL.IsEnabled = false;
            }
        }

        private async void Generatepath(String Field)
        {
            try
            {
                if (Field == "VI")
                {
                    string Folder = ("Photo Store\\" + EReporterHelper.InspectResultFields.JobCode + "\\" + Settings.UserID +"\\DWR\\" + EReporterHelper.InspectResultFields.RowID + "\\" + Field);
                    InspectionPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath(Folder);

                    var VIImageFiles = await DependencyService.Get<ISaveFiles>().GetAllImages(InspectionPath);

                    if (VIImageFiles.Any())
                    {
                        VIImage.Source = "Greencam.png";
                        VIImageFiles.Insert(0, "- Select -");
                        ListOfPhotoPicker.ItemsSource = VIImageFiles;
                        if (SetPicker)
                        {
                            ListOfPhotoPicker.SelectedItem = VIImageFiles.LastOrDefault();
                            SetPicker = false;
                        }
                    }
                    else
                    {
                        ListOfPhotoPicker.ItemsSource = new List<string> { "- Select -", };
                        VIImage.Source = "cam.png";
                    }


                }
                else if (Field == "DI")
                {
                    string Folder = ("Photo Store\\" + EReporterHelper.InspectResultFields.JobCode + "\\" + Settings.UserID + "\\DWR\\" + EReporterHelper.InspectResultFields.RowID + "\\" + Field);
                    InspectionPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath(Folder);

                    var DIImageFiles = await DependencyService.Get<ISaveFiles>().GetAllImages(InspectionPath);
                    if (DIImageFiles.Any())
                    {
                        VIImage.Source = "Greencam.png";
                        DIImageFiles.Insert(0, "- Select -");
                        ListOfPhotoPicker.ItemsSource = DIImageFiles;
                        if (SetPicker)
                        {
                            ListOfPhotoPicker.SelectedItem = DIImageFiles.LastOrDefault();
                            SetPicker = false;
                        }
                    }
                    else
                    {
                        ListOfPhotoPicker.ItemsSource = new List<string> { "- Select -", };
                        DIImage.Source = "cam.png";
                    }

                }
                else if (Field == "VIDI")
                {
                    string VIFolder = ("Photo Store\\" + EReporterHelper.InspectResultFields.JobCode + "\\" + Settings.UserID + "\\DWR\\" + EReporterHelper.InspectResultFields.RowID + "\\" + "VI");
                    InspectionPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath(VIFolder);

                    var VIImageFiles = await DependencyService.Get<ISaveFiles>().GetAllImages(InspectionPath);
                    if (VIImageFiles.Any())
                        VIImage.Source = "Greencam.png";
                    else
                        VIImage.Source = "cam.png";


                    string DIFolder = ("Photo Store\\" + EReporterHelper.InspectResultFields.JobCode + "\\" + Settings.UserID + "\\DWR\\" + EReporterHelper.InspectResultFields.RowID + "\\" + "DI");
                    InspectionPath = await DependencyService.Get<ISaveFiles>().GenerateImagePath(DIFolder);

                    var DIImageFiles = await DependencyService.Get<ISaveFiles>().GetAllImages(InspectionPath);
                    if (DIImageFiles.Any())
                        DIImage.Source = "Greencam.png";
                    else
                        DIImage.Source = "cam.png";
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

    }
}
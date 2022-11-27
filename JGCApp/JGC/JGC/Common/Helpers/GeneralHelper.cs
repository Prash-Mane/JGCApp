using JGC.Common.Constants;
using JGC.Common.Extentions;
using JGC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JGC.Common.Helpers
{
    public static class GeneralHelper
    {
        private static PageHelper _pageHelper = CheckValidLogin._pageHelper;
        private static CompletionPageHelper _CompletionpageHelper = CheckValidLogin._CompletionpageHelper;
        public static bool IsEmailValid(string emailaddress)
        {
            try
            {
                Regex regex = new Regex(AppConstant.emailRegex);
                Match match = regex.Match(emailaddress);
                if (match.Success)
                    return true;
                else
                    return false;
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static Exception GetInnerException(Exception exception)
        {
            if (exception.InnerException == null)
                return exception;
            return GetInnerException(exception.InnerException);
        }

      
        public static async Task LogOut()
        {
            Settings.AccessToken = string.Empty;
            Settings.CompletionAccessToken = string.Empty;
            Settings.RenewalToken = string.Empty;
            Settings.DisplayName = string.Empty;
            Cache.accessToken = string.Empty;
            Settings.IsStop = 0;
            _pageHelper.TokenExpiry = DateTime.Today.AddDays(-1);
            _CompletionpageHelper.CompletionTokenExpiry = DateTime.Today.AddDays(-1);

            //var dm = App.ContainerProvider.Resolve(typeof(IDownloadManager)) as IDownloadManager;
            //if (dm != null && dm.DownloadingQueue.Any())
            //    dm.StopAll();

            if (!(Application.Current.MainPage is Viwes.LoginPage))
                await App.Navigation.ResetNavigation<ViewModel.LoginViewModel>();
        }

       
        public static string GetCurrentDate(string format)
        {
            string currentDate = DateTime.Now.Date.ToString(format);
            return currentDate;
        }         

      
    }
}

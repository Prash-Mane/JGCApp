using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Common.Constants
{
    public class AppConstant
    {
        public const string DefaultDate = "20190101000000";
        public const string DateParseString = "yyyyMMdd";
        public const string ExtendedDateParseString = "yyyyMMddHHmmss";
       // public const string UnitID = "Mikes Laptop";

        public const string EmptyJsonArray = "[]";
        public static string DateFormat = "dd-MMM-yyyy";
        public static string CameraDateFormat = "dd-MMM-yyyy HH-mm-ss";
        public static string DateSaveFormat = "yyyy-MM-dd HH:mm:ss";

        public static string SECRET_SHARED_TOKEN = "FAVTCD95qWlbpP1dJen94e0VYN4w0f";
        public static string SECRET_SHARED_TOKENMODS = "J85acbcdfjfO65gC52gaWWvv9453kg0rfj9Gt45k90v";
        //public static string StartDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "").Replace("\\bin\\Debug", "");
        //public static string LocalConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + StartDirectory + "\\VMDataManager.accdb;JET OLEDB:Database Password=jGcJ0bS3tt1ng|";


        public const string EMPTY_STRING = "";
        public const string NETWORK_FAILURE = "No Network Connection found! Please try again.";
        public const string CANCELLED = "Cancelled";
        public const string LOGIN_FAILURE = "Login Failed! Please try again.";
        public const string CHANGEPASSWORD_FAILURE = "Password Change Request Failed! Please try again.";
        public const string UPDATE_FAILURE_MSG = "Your app data is up to date.";

        public const string SPF_USER_DISPLAYNAME = "displayName";
        public const string SPF_ACCESSTOKEN = "accessToken";
        public const string SPF_RENEWALTOKEN = "renewalToken";

        /// <summary>
        /// The email address
        /// </summary>
        public const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
       @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        public const float defaultIOSPageHeight = 667f; //based on iPhone 8, 7, 6s, 6
        public const float defaultIOSPageWidth = 375f;

        public const float defaultDroidPageHeight = 640f; //based on Nexus 6P
        public const float defaultDroidPageWidth = 411f;
        public const int GradientHeaderHeight = 140;
    }
}

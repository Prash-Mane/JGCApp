using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Common.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static int UserID
        {
            get => AppSettings.GetValueOrDefault(nameof(UserID), default(int));
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(UserID), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(UserID));
                    AppSettings.AddOrUpdateValue(nameof(UserID), value);
                }
            }
        }
        public static int CompletionUserID
        {
            get => AppSettings.GetValueOrDefault(nameof(CompletionUserID), default(int));
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(CompletionUserID), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(CompletionUserID));
                    AppSettings.AddOrUpdateValue(nameof(CompletionUserID), value);
                }
            }
        }

        public static int IsStop
        {
            get => AppSettings.GetValueOrDefault(nameof(IsStop), default(int));
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(IsStop), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(IsStop));
                    AppSettings.AddOrUpdateValue(nameof(IsStop), value);
                }
            }
        }
        
        public static string ModelName
        {
            get => AppSettings.GetValueOrDefault(nameof(ModelName), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(ModelName), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(ModelName));
                    AppSettings.AddOrUpdateValue(nameof(ModelName), value);
                }
            }
        }

        public static string UnitID
        {
            get => AppSettings.GetValueOrDefault(nameof(UnitID), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(UnitID), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(UnitID));
                    AppSettings.AddOrUpdateValue(nameof(UnitID), value);
                }
            }
        }

        public static string ModuleName
        {
            get => AppSettings.GetValueOrDefault(nameof(ModuleName), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(ModuleName), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(ModelName));
                    AppSettings.AddOrUpdateValue(nameof(ModuleName), value);
                }
            }
        }

        public static string DownloadParam
        {
            get => AppSettings.GetValueOrDefault(nameof(DownloadParam), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(DownloadParam), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(DownloadParam));
                    AppSettings.AddOrUpdateValue(nameof(DownloadParam), value);
                }
            }
        }
        public static string JobCode
        {
            get => AppSettings.GetValueOrDefault(nameof(JobCode), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(JobCode), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(JobCode));
                    AppSettings.AddOrUpdateValue(nameof(JobCode), value);
                }
            }
        }

        public static int ProjectID
        {
            get => AppSettings.GetValueOrDefault(nameof(ProjectID), default(int));
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(ProjectID), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(ProjectID));
                    AppSettings.AddOrUpdateValue(nameof(ProjectID), value);
                }
            }
        }

        public static int ModelID
        {
            get => AppSettings.GetValueOrDefault(nameof(ModelID), default(int));
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(ModelID), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(ModelID));
                    AppSettings.AddOrUpdateValue(nameof(ModelID), value);
                }
            }
        }

        public static string ProjectName
        {
            get => AppSettings.GetValueOrDefault(nameof(ProjectName), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(ProjectName), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(ProjectName));
                    AppSettings.AddOrUpdateValue(nameof(ProjectName), value);
                }
            }
        }

        public static string Report
        {
            get => AppSettings.GetValueOrDefault(nameof(Report), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(Report), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(Report));
                    AppSettings.AddOrUpdateValue(nameof(Report), value);
                }
            }
        }        

        public static string DisplayName
        {
            get => AppSettings.GetValueOrDefault(nameof(DisplayName), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(DisplayName), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(DisplayName));
                    AppSettings.AddOrUpdateValue(nameof(DisplayName), value);
                }
            }
        }

        public static string AccessToken
        {
            get => AppSettings.GetValueOrDefault(nameof(AccessToken), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(AccessToken), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(AccessToken));
                    AppSettings.AddOrUpdateValue(nameof(AccessToken), value);
                }
            }
        }
        public static string CompletionAccessToken
        {
            get => AppSettings.GetValueOrDefault(nameof(CompletionAccessToken), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(CompletionAccessToken), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(CompletionAccessToken));
                    AppSettings.AddOrUpdateValue(nameof(CompletionAccessToken), value);
                }
            }
        }

        public static string RenewalToken
        {
            get => AppSettings.GetValueOrDefault(nameof(RenewalToken), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(RenewalToken), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(RenewalToken));
                    AppSettings.AddOrUpdateValue(nameof(RenewalToken), value);
                }
            }
        }

        public static string Server_Url
        {
            get => AppSettings.GetValueOrDefault(nameof(Server_Url), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(Server_Url), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(Server_Url));
                    AppSettings.AddOrUpdateValue(nameof(Server_Url), value);
                }
            }
        }
        public static string Server_UrlForConstructionModule
        {
            get => AppSettings.GetValueOrDefault(nameof(Server_UrlForConstructionModule), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(Server_UrlForConstructionModule), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(Server_UrlForConstructionModule));
                    AppSettings.AddOrUpdateValue(nameof(Server_UrlForConstructionModule), value);
                }
            }
        }
        
        public static string CompletionServer_Url
        {
            get => AppSettings.GetValueOrDefault(nameof(CompletionServer_Url), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(CompletionServer_Url), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(CompletionServer_Url));
                    AppSettings.AddOrUpdateValue(nameof(CompletionServer_Url), value);
                }
            }
        }

        public static string AppCurrentVersion
        {
            get => AppSettings.GetValueOrDefault(nameof(AppCurrentVersion), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(AppCurrentVersion), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(AppCurrentVersion));
                    AppSettings.AddOrUpdateValue(nameof(AppCurrentVersion), value);
                }
            }
        }

        public static string UserName
        {
            get => AppSettings.GetValueOrDefault(nameof(UserName), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(UserName), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(UserName));
                    AppSettings.AddOrUpdateValue(nameof(UserName), value);
                }
            }
        }
        public static string CompletionUserName
        {
            get => AppSettings.GetValueOrDefault(nameof(CompletionUserName), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(CompletionUserName), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(CompletionUserName));
                    AppSettings.AddOrUpdateValue(nameof(CompletionUserName), value);
                }
            }
        }
        public static string PassWord
        {
            get => AppSettings.GetValueOrDefault(nameof(PassWord), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(PassWord), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(PassWord));
                    AppSettings.AddOrUpdateValue(nameof(PassWord), value);
                }
            }
        }
        public static string CurrentDB
        {
            get => AppSettings.GetValueOrDefault(nameof(CurrentDB), string.Empty);
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(CurrentDB), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(CurrentDB));
                    AppSettings.AddOrUpdateValue(nameof(CurrentDB), value);
                }
            }
        }
        public static bool IsMODSApp
        {
            get => AppSettings.GetValueOrDefault(nameof(IsMODSApp), default(bool));
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(IsMODSApp), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(IsMODSApp));
                    AppSettings.AddOrUpdateValue(nameof(IsMODSApp), value);
                }
            }
        }
        public static bool IsCompletionApp
        {
            get => AppSettings.GetValueOrDefault(nameof(IsCompletionApp), default(bool));
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(nameof(IsCompletionApp), value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(nameof(IsCompletionApp));
                    AppSettings.AddOrUpdateValue(nameof(IsCompletionApp), value);
                }
            }
        }
    }
}

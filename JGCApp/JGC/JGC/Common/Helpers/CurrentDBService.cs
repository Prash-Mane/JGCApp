using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Common.Helpers
{
    public static class CurrentDBService
    {

        private static List<string> MODS_01_FTP = new List<string> { "www.vmlive.net", "virtualmanagerbp", "virtualmanagerbpW5x7L4E4Yelfin2112", "www.vmlive.net" };
        private static List<string> MODS_VME_FTP = new List<string> { "vme.vmlive.net", "richcox", "afb130velfin2112#", "wwwroot" };
        private static List<string> MODS_03_FTP = new List<string> { "mods03.vmlive.net", "virtualmanagerbp", "virtualmanagerbpW5x7L4E4Yelfin2112", "www.vmlive.net" };
        private static List<string> CHEVRON_FTP = new List<string> { "chevron.vmlive.net", "richcox", "afb130velfin2112#", "wwwroot" };
        private static List<string> CHEVRON_DEV_FTP = new List<string> { "chevronaustest.vmlive.net", "richcox", "afb130velfin2112#", "wwwroute" };
        private static List<string> CHEVRON_AUS_FTP = new List<string> { "chevronaus.vmlive.net", "richcox", "afb130velfin2112#", "wwwroute" };
        private static List<string> BP_FTP = new List<string> { "bp.vmlive.net", "virtualmanagerbp", "virtualmanagerbpW5x7L4E4Yelfin2112", "wwwroot" };
        private static List<string> SHELL_FTP = new List<string> { "shell.vmlive.net", "virtualmanagerbp", "virtualmanagerbpW5x7L4E4Yelfin2112", "wwwroot" };
        private static List<string> JGC_FTP = new List<string> { "JGCTest.vmlive.net", "stw", "P8ML0kTPPD0!0^ozLJ3P", "wwwtest" };
        private static List<string> YOC_FTP = new List<string> { "yocdemo.vmlive.net", "stw", "P8ML0kTPPD0!0^ozLJ3P", "wwwyoc" };
        private static List<string> HARMONY_FTP = new List<string> { "Harmony.vmlive.net", "stw", "zK8TP7Ozaw79nAorJOt1", "wwwroot" };
        //private static List<string> ROVUMA_FTP = new List<string> { "Rovuma.vmlive.net", "stw", "zK8TP7Ozaw79nAorJOt1", "wwwroot" };
        private static List<string> JGC_ITR_FTP = new List<string> { "jgccmpitr.vmlive.net", "richcox", "afb130velfin2112#", "wwwcore" };

        public static string MODS_01 = "MODS_01";
        public static string MODS_VME = "MODS_VME";
        public static string MODS_03 = "MODS_03";
        public static string CHEVRON = "CHEVRON";
        public static string CHEVRON_DEV = "CHEVRON_AUS";
        public static string CHEVRON_AUS = "CHEVRON_AUS_MAIN";
        public static string SHELL = "SHELL";
        public static string BP = "BP";
        public static string JGC = "JGC";
        public static string JGC_DEMO = "JGC_DEMO";
        public static string YOC_DEMO = "YOC_DEMO";
        public static string JGC_HARMONY = "JGC_HARMONY";
        //public static string JGC_ROVUMA = "ROVUMA_TEST";
        public static string JGC_ITR = "JGC_ITR";
        

        public static List<string> getBackEndOptions()
        {
            return new List<string> { MODS_01, MODS_VME, MODS_03, CHEVRON, CHEVRON_DEV, CHEVRON_AUS, SHELL, BP, JGC, JGC_DEMO, JGC_ITR };
        }

        public static List<string> getFTPCredentialsForBackEnd(string backEnd)
        {
            if (MODS_01.Equals(backEnd))
                return MODS_01_FTP;
            if (MODS_VME.Equals(backEnd))
                return MODS_VME_FTP;
            if (MODS_03.Equals(backEnd))
                return MODS_03_FTP;
            if (CHEVRON.Equals(backEnd))
                return CHEVRON_FTP;
            if (CHEVRON_DEV.Equals(backEnd))
                return CHEVRON_DEV_FTP;
            if (CHEVRON_AUS.Equals(backEnd))
                return CHEVRON_AUS_FTP;
            if (SHELL.Equals(backEnd))
                return SHELL_FTP;
            if (BP.Equals(backEnd))
                return BP_FTP;
            if (JGC.Equals(backEnd))
                return JGC_FTP;
            if (JGC_DEMO.Equals(backEnd))
                return JGC_FTP;
            if (YOC_DEMO.Equals(backEnd))
                return YOC_FTP;
            if (JGC_HARMONY.Equals(backEnd))
                return HARMONY_FTP;
            //if (JGC_ROVUMA.Equals(backEnd))
            //    return ROVUMA_FTP;
            if (JGC_ITR.Equals(backEnd))
                return JGC_ITR_FTP;

            return null;
        }

    }
}

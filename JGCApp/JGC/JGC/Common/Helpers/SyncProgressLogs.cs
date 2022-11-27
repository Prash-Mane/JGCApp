using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace JGC.Common.Helpers
{
    public sealed class SyncProgressLogs : INotifyPropertyChanged
    {

        private static readonly SyncProgressLogs instance = new SyncProgressLogs();
        private SyncProgressLogs() { }
        public static SyncProgressLogs UpdateLogs
        {
            get
            {
                return instance;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        //Logs Properties for SyncPage
        private string punchListLogs { get; set; }
        public string PunchListLogs
        {
            get { return punchListLogs; }
            set { punchListLogs = value; RaisePropertyChanged("PunchListLogs"); ; }
        }
        private string itrLogs { get; set; }
        public string ItrLogs
        {
            get { return itrLogs; }
            set { itrLogs = value; RaisePropertyChanged("ItrLogs"); ; }
        }
        private string drawingLogs { get; set; }
        public string DrawingLogs
        {
            get { return drawingLogs; }
            set { drawingLogs = value; RaisePropertyChanged("DrawingLogs"); ; }
        }


    }
}

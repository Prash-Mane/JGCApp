using SQLite;


namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_JS_SIGN_OFF")]
   public class T_JS_SIGN_OFF
    {
        public string COLUMN_SIGN_TYPE { get; set; }
        public string COLUMN_SIGN_USERNAME { get; set; }
        public string COLUMN_SIGN_DATETIME { get; set; }
        public string COLUMN_SIGN_HEADERID { get; set; }
        public string COLUMN_SIGN_JOINTID { get; set; }
        public string COLUMN_SIGN_SLOTNUM { get; set; }
        public string COLUMN_SIGN_SYNCED { get; set; }
        public string COLUMN_FIELDVERIFIED_SIGNED { get; set; }
        public string COLUMN_FIELDVERIFIED_SIGNEDBY { get; set; }
        public string COLUMN_FIELDVERIFIED_SIGNEDON { get; set; }
    }
}

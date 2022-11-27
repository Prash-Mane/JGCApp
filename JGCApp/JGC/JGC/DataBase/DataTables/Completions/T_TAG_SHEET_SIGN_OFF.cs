using SQLite;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_TAG_SHEET_SIGN_OFF")]
    public class T_TAG_SHEET_SIGN_OFF
    {
        public string TAG_CHECKSHEET_SIGN_OFF_TAG { get; set; }
        public string TAG_CHECKSHEET_SIGN_OFF_CHECKSHEET { get; set; }
        public string TAG_CHECKSHEET_SIGN_OFF_COUNT { get; set; }
        public string TAG_CHECKSHEET_SIGN_OFF_TITLE { get; set; }
        public string TAG_CHECKSHEET_SIGN_OFF_FULL_NAME { get; set; }
        public object TAG_CHECKSHEET_SIGN_OFF_SIGNDATE { get; set; }
        public object TAG_CHECKSHEET_SIGN_OFF_SYNCED { get; set; }
        public string TAG_CHECKSHEET_SIGN_OFF_JOBPACK { get; set; }
    }
}

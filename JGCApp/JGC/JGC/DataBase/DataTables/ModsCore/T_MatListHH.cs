using SQLite;

namespace JGC.DataBase.DataTables.ModsCore
{
    [Table("T_MatListHH")]
    public class T_MatListHH
    {
        public string ID { get; set; }
        public string ProjectID { get; set; }
        public string WorkPack { get; set; }
        public string UnitNo { get; set; }
        public string Weight { get; set; }
        public string Barcode { get; set; }
    }
}

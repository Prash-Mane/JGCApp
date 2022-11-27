using SQLite;

namespace JGC.DataBase.DataTables
{
   
    [Table("T_AdminPreTestRecordContent")]
    public class T_AdminPreTestRecordContent
    {
        [PrimaryKey, Indexed]
        public int ID { get; set; }
        public long ProjectID { get; set; }
        public int OrderNo { get; set; }
        public string Description { get; set; }
        public string CompanyCategoryCode { get; set; }
        public string FunctionCode { get; set; }
        public string PIC { get; set; }
    }
}

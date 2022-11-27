using SQLite;


namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_JS_JOINT_LEAK_RECORDS")]
   public class T_JS_JOINT_LEAK_RECORDS
    {
        public int COLUMN_LEAK_RECORD_ID { get; set; }
        public int COLUMN_LEAK_JOINT_ID { get; set; }
        public string COLUMN_LEAK_FLANGENO { get; set; }
        public string COLUMN_LEAK_LEAK_TEST_NUMBER { get; set; }
        public string COLUMN_LEAK_WRENCH_1_SIZE { get; set; }
        public string COLUMN_LEAK_WRENCH_1_SERIAL { get; set; }
        public string COLUMN_LEAK_WRENCH_2_SIZE { get; set; }
        public string COLUMN_LEAK_WRENCH_2_SERIAL { get; set; }
        public string COLUMN_LEAK_SOCKET_SIZE { get; set; }
        public string COLUMN_LEAK_PUMP_SIZE { get; set; }
        public string COLUMN_LEAK_PUMP_PRESSURE { get; set; }
        public string COLUMN_LEAK_TARGET_TORQUE { get; set; }
        public string COLUMN_LEAK_SIGNOFF_NOTES { get; set; }
        public string COLUMN_LEAK_SIGNOFF_NAME { get; set; }
        public string COLUMN_LEAK_SIGNOFF_DATE { get; set; }
        public string COLUMN_LEAK_SIGNED_OFF { get; set; }
        public string COLUMN_LEAK_SYNCED { get; set; }
    }
}

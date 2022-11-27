using SQLite;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_JOINT")]
    public class T_JOINT
    {
        public int _id { get; set; }
        public int COLUMN_JOINT_id { get; set; }
        public string COLUMN_JOINT_area { get; set; }
        public string COLUMN_JOINT_description { get; set; }
        public string COLUMN_JOINT_phase { get; set; }
        public string COLUMN_JOINT_discipline { get; set; }
        public string COLUMN_JOINT_mcclType { get; set; }
        public string COLUMN_JOINT_lineNo { get; set; }
        public string COLUMN_JOINT_pAndId { get; set; }
        public string COLUMN_JOINT_drawing { get; set; }
        public string COLUMN_JOINT_flangeNo { get; set; }
        public string COLUMN_JOINT_workpack { get; set; }
        public string COLUMN_JOINT_size { get; set; }
        public string COLUMN_JOINT_rating { get; set; }
        public string COLUMN_JOINT_service { get; set; }
        public string COLUMN_JOINT_VMJointNo { get; set; }
        public string COLUMN_JOINT_BROKEN { get; set; }
        public string COLUMN_JOINT_BRAKE_REASON { get; set; }
        public string COLUMN_JOINT_CRITICAL { get; set; }
        public string COLUMN_ISO_JOINT_ID { get; set; }
        public int COLUMN_JOINT_BROKEN_CCTRID { get; set; }
        public string ProjectName { get; set; }
    }
}

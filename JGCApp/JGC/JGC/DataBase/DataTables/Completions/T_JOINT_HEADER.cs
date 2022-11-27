using SQLite;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_JOINT_HEADER")]
   public class T_JOINT_HEADER
    {
        public int _id { get; set; }
        public int COLUMN_JOINT_HEADER_JOINTID { get; set; }
        public string COLUMN_JOINT_HEADER_PANDID { get; set; }
        public string COLUMN_JOINT_HEADER_ISO2 { get; set; }
        public string COLUMN_JOINT_HEADER_VENDOR { get; set; }
        public string COLUMN_JOINT_HEADER_SPEC { get; set; }
        public string COLUMN_JOINT_HEADER_EQUIPMENTID { get; set; }
        public object COLUMN_JOINT_HEADER_SYSTEMDESC { get; set; }
        public string COLUMN_JOINT_HEADER_TORQUEVALUE { get; set; }
        public string COLUMN_JOINT_HEADER_CRITICALITYRATING { get; set; }
        public string COLUMN_JOINT_HEADER_TECHNICALNOTES { get; set; }
        public string COLUMN_JOINT_HEADER_TQ_30_REQ { get; set; }
        public string COLUMN_JOINT_HEADER_TQ_70_REQ { get; set; }
        public string COLUMN_JOINT_HEADER_TQ_100_REQ { get; set; }
        public string COLUMN_JOINT_HEADER_WRENCHSIZE { get; set; }
        public string COLUMN_JOINT_HEADER_HYDROPRESSURE_30_REQ { get; set; }
        public string COLUMN_JOINT_HEADER_HYDROPRESSURE_70_REQ { get; set; }
        public string COLUMN_JOINT_HEADER_HYDROPRESSURE_100_REQ { get; set; }
        public string COLUMN_JOINT_HEADER_HYDROPRESSURE_WRENCHSIZE { get; set; }
        public string COLUMN_JOINT_HEADER_REQUIREDTOOL { get; set; }
        public string COLUMN_JOINT_HEADER_REQUIREDBOLTLOAD { get; set; }
        public string COLUMN_JOINT_HEADER_BOLTSTRESS { get; set; }
        public string COLUMN_JOINT_HEADER_TENSIONLOADA { get; set; }
        public string COLUMN_JOINT_HEADER_TENSIONLOADB { get; set; }
        public string COLUMN_JOINT_HEADER_BOLTCOVERAGE { get; set; }
        public string COLUMN_JOINT_HEADER_LUBRICANTTYPE { get; set; }
        public string COLUMN_JOINT_HEADER_TIGHTENMETHOD { get; set; }
        public string ProjectName { get; set; }
    }
}

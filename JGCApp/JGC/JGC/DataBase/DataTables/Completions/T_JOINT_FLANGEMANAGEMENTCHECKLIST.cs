using SQLite;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_JOINT_FLANGEMANAGEMENTCHECKLIST")]
    public class T_JOINT_FLANGEMANAGEMENTCHECKLIST
    {
        public int COLUMN_CHECKLIST_jointid { get; set; }
        public int COLUMN_CHECKLIST_cctrid { get; set; }
        public string COLUMN_CHECKLIST_workpackheaderid { get; set; }
        public string COLUMN_CHECKLIST_milestone { get; set; }
        public string COLUMN_CHECKLIST_checksheettitle { get; set; }
        public string ProjectName { get; set; }
    }
}

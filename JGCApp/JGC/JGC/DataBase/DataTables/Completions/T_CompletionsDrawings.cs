using SQLite;

namespace JGC.DataBase.DataTables.Completions
{
    [Table("T_CompletionsDrawings")]
    public class T_CompletionsDrawings
    {

        public string name { get; set; }
        public string description { get; set; }
        public string filename { get; set; }
        public string sheet { get; set; }
        public string type { get; set; }
        public string revision { get; set; }
        public long revisionDateTime { get; set; }
        public string synced { get; set; }
        public string ProjectName { get; set; }

        public override string ToString()
        {
            return name;
        }

    }
}

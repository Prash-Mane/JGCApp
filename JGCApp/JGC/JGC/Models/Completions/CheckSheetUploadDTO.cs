using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Completions
{
    public class CheckSheetUploadDTO
    {

        public string tagName { get; set; }
        public string sheetName { get; set; }
        public string jobPack { get; set; }
        public string projectName { get; set; }
        public string modelName { get; set; }
        public string DwgNo { get; set; }
        public string DwgRevNo { get; set; }

        public List<SignOff> signOffs { get; set; }
        public List<Answer> answers { get; set; }
    }

    public class SignOff
    {
        public int count { get; set; }
        public string title { get; set; }
        public string fullName { get; set; }
        public DateTime signDate { get; set; }
        public bool synced { get; set; }
        public string jobPack { get; set; }
        public bool rejected { get; set; }
        public string rejectedReason { get; set; }
    }
}

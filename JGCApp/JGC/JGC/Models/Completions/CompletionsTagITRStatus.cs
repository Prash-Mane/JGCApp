using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models.Completions
{
    public class CompletionsTagITRStatus
    {
        public string JOB_CODE_KEY { get; set; }
        public string TAG_NO { get; set; }
        public string ITR_NO { get; set; }
        public string REMARKS { get; set; }
        public DateTime? SIGN_DATE_1 { get; set; }
        public DateTime? SIGN_DATE_2 { get; set; }
        public DateTime? SIGN_DATE_3 { get; set; }
        public DateTime? SIGN_DATE_4 { get; set; }
        public DateTime? SIGN_DATE_5 { get; set; }
        public DateTime? SIGN_DATE_6 { get; set; }
        public DateTime? SIGN_DATE_7 { get; set; }
        public string SIGN_ID_NO_1 { get; set; }
        public string SIGN_ID_NO_2 { get; set; }
        public string SIGN_ID_NO_3 { get; set; }
        public string SIGN_ID_NO_4 { get; set; }
        public string SIGN_ID_NO_5 { get; set; }
        public string SIGN_ID_NO_6 { get; set; }
        public string SIGN_ID_NO_7 { get; set; }
        public string REJECT_REMARKS { get; set; }
        public DateTime? REJECTED_DATE { get; set; }
        public string REJECTED_USER { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public string UPDATED_ID { get; set; }
        public bool ResetSignatures { get; set; }
        public string Itr_Report_No { get; set; }
        public bool Cancel_Flag { get; set; }
    }
}

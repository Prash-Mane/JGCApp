using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models
{
    public class RIR 
    {
        public string JobCode { get; set; }
        public string ReportNo { get; set; }
        public DateTime RIR_Date { get; set; }
        public string Ship_No { get; set; }
        public DateTime Arrival_Date { get; set; }

        public List<RIRRow> RIRRows { get; set; }
        public string Error { get; set; }

        //Filling out information

        //First Sign off
        public Boolean Packing_List { get; set; }
        public Boolean Mill_Certificate { get; set; }
        public Boolean Others_1 { get; set; }
        public Boolean Others_2 { get; set; }
        public Boolean Others_3 { get; set; }

        public string Attachment_Comment { get; set; }

        //Second Sign Off
        public DateTime Date_Inspected { get; set; }

        public Boolean Result_Accepted { get; set; }
        public Boolean Result_Damage { get; set; }
        public Boolean Result_OffSpec { get; set; }
        public string Inspection_Remarks { get; set; }
    }

    public class RIRRow 
    {
        public string PO_No { get; set; }
        public string PO_Title { get; set; }
        public string Vendor { get; set; }
        public string Item_No { get; set; }
        public string Partial_No { get; set; }
        public string Item_Description { get; set; }
        public string Error { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models
{

    public class MRR
    {
        public string JobCode { get; set; }
        public string ReportNo { get; set; }
        public DateTime Received_Date { get; set; }
        public string Ship_No { get; set; }
        public string Store_Location { get; set; }
        public string RIR_No { get; set; }
        public List<MRRRow> MRRRows { get; set; }
        public string Error { get; set; }
    }

    public class MRRRow
    {
        public Boolean Updated { get; set; }
        public string Number { get; set; }
        public string PO_No { get; set; }
        public string Vendor { get; set; }
        public string Item_No { get; set; }
        public string PT_No { get; set; }
        public string PJ_Commodity { get; set; }
        public string PJ_Sub_Commodity { get; set; }
        public string Size { get; set; }
        public string Thickness { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public double Shipped_Qty { get; set; }
        public double PO_Qty { get; set; }
        public double Good { get; set; }
        public double Damage { get; set; }
        public double Off_Spec { get; set; }
        public double Short { get; set; }
        public double Over { get; set; }
        public List<MRRStorageAreas> Storage_Area_Codes { get; set; }
        public List<MRRHeatNos> Heat_Nos { get; set; }
        public Boolean Client_Accepted { get; set; }
        public Boolean Client_Rejected { get; set; }
        public string Remarks_1 { get; set; }
        public string Remarks_2 { get; set; }
        public string Error { get; set; }
    }

    public class MRRStorageAreas
    {
        public MRRStorageAreas()
        {
            this.Issued_Date = Convert.ToDateTime("01/01/2000 0:00");
        }
        public string Storage_Area { get; set; }
        public double Good_Quantity { get; set; }
        public Boolean Is_Partially_Issued { get; set; }
        public DateTime Issued_Date { get; set; }

        public override string ToString()
        {
            return Storage_Area + " (Qty. " + string.Format("{0:N3}", Good_Quantity) + ")";
        }
        public string Error { get; set; }
    }

    public class MRRHeatNos
    {
        public string Heat_No { get; set; }
        public double Quantity { get; set; }

        public override string ToString()
        {
            return Heat_No + " (Qty. " + string.Format("{0:N3}", Quantity) + ")";
        }
        public string Error { get; set; }
    }
}

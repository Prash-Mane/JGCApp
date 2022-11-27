using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models
{
    public class CMR 
    {
        public string JobCode { get; set; }
        public string ReportNo { get; set; }
        public DateTime CMR_Date { get; set; }
        public string Subcon { get; set; }
        public string Store_Location { get; set; }
        public string MRS_No { get; set; }
        public List<CMRSummaryRows> CMRSummaryRows { get; set; }
        public List<CMRDetailRows> CMRDetailRows { get; set; }
        public string Error { get; set; }
    }

    public class CMRSummaryRows
    {
        public Boolean Updated { get; set; }
        public string Number { get; set; }
        public string PJ_Commodity { get; set; }
        public string Sub_Commodity { get; set; }
        public string Size_Descr { get; set; }
        public string Thickness_Descr { get; set; }
        public string Part_Name { get; set; }
        public string Commodity_Descr { get; set; }
        public string Unit { get; set; }
        public double Reserved_To_Subcon { get; set; }
        public double Request_Qty { get; set; }
        public double Issued_Qty { get; set; }
        public double Over_Issd_Adj { get; set; }
        public DateTime Issued_Date { get; set; }
        public List<CMRStorageAreas> Storage_Areas { get; set; }
        public string Error { get; set; }
    }

    public class CMRDetailRows
    {
        public string PCWBS { get; set; }
        public string Spool_Dwg_No { get; set; }
        public string Spool_Piece_No { get; set; }
        public string Revision { get; set; }
        public string PJ_Commodity { get; set; }
        public string Sub_Commodity { get; set; }
        public string Size_Descr { get; set; }
        public string Thickness_Descr { get; set; }
        public string Part_Name { get; set; }
        public string Commodity_Descr { get; set; }
        public string Unit { get; set; }
        public double Reserved_To_Subcon { get; set; }
        public double Request_Qty { get; set; }
        public double Over_Issd_Adj { get; set; }
        public string Error { get; set; }
    }

    public class CMRStorageAreas 
    {
        public string Storage_Area_Available { get; set; }
        public double Avail_Stock_Qty { get; set; }
        public double Issued_Qty { get; set; }
        public string Storage_Area_Code { get; set; }
        public string Heat_No { get; set; }
        public DateTime Issued_Date { get; set; }
        public Boolean Is_Partially_Issued { get; set; }
        public string Error { get; set; }
    }
}

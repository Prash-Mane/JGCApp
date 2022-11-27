using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables
{
    [Table("T_DWR")]
    public class T_DWR
    {
        public long RowID { get; set; }
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public string JobCode { get; set; }
        public string AFINo { get; set; }
        public string ReportNo { get; set; }
        public string SubContractor { get; set; }
        public string SpoolDrawingNo { get; set; }
        public string TestPackage { get; set; }
        public string RevNo { get; set; }
        public string SheetNo { get; set; }
        public string JointNo { get; set; }
        public string SpoolNo { get; set; }
        public string LineClass { get; set; }
        public string Size { get; set; }
        public string Thickness { get; set; }
        public DateTime FitUpDate { get; set; }
        public DateTime WeldedDate { get; set; }
        public string WeldType { get; set; }
        public string RootWeldProcess { get; set; }
        public string RootWelder1 { get; set; }
        public bool RootWelder1ProductionJoint { get; set; }
        public string RootWelder2 { get; set; }
        public bool RootWelder2ProductionJoint { get; set; }
        public string RootWelder3 { get; set; }
        public bool RootWelder3ProductionJoint { get; set; }
        public string RootWelder4 { get; set; }
        public bool RootWelder4ProductionJoint { get; set; }
        public string FillCapWeldProcess { get; set; }
        public string FillCapWelder1 { get; set; }
        public bool FillCapWelder1ProductionJoint { get; set; }
        public string FillCapWelder2 { get; set; }
        public bool FillCapWelder2ProductionJoint { get; set; }
        public string FillCapWelder3 { get; set; }
        public bool FillCapWelder3ProductionJoint { get; set; }
        public string FillCapWelder4 { get; set; }
        public bool FillCapWelder4ProductionJoint { get; set; }
        public string WPSNo { get; set; }
        public string BaseMetal1 { get; set; }
        public string HeatNo1 { get; set; }
        public string BaseMetal2 { get; set; }
        public string HeatNo2 { get; set; }
        public string VI { get; set; }
        public string VIComment { get; set; }
        public string DI { get; set; }
        public string DIComment { get; set; }
        public string Remarks { get; set; }
        public string WorkCode { get; set; }
        public string IdentCode1 { get; set; }
        public string IdentCode2 { get; set; }
        public int RejectedByUserID { get; set; }
        public DateTime RejectedOn { get; set; }
        public string PCWBS { get; set; }
        public bool Updated { get; set; }
        public DateTime DownloadedDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.Models
{
    //public class DWR 
    //{
    //    public string JobCode { get; set; }
    //    public string AFINo { get; set; }
    //    public string ReportNo { get; set; }
    //    public string SubContractor { get; set; }
    //    public List<DWRRow> DWRRows { get; set; }
    //    public string Error { get; set; }
    //    public bool Selected { get; set; }
    //}

    //public class DWRRow 
    //{
    //    public Boolean Updated { get; set; }
    //    public override string ToString()
    //    {
    //        return Spool_Drawing_No + " - " + Joint_No;
    //    }
       
    //    public DWRRow()
    //    {
    //        Root_Welder_1_Production_Joint = false;
    //        Root_Welder_2_Production_Joint = false;
    //        Root_Welder_3_Production_Joint = false;
    //        Root_Welder_4_Production_Joint = false;
    //        FillCap_Welder_1_Production_Joint = false;
    //        FillCap_Welder_2_Production_Joint = false;
    //        FillCap_Welder_3_Production_Joint = false;
    //        FillCap_Welder_4_Production_Joint = false;
    //    }

    //    public string Number { get; set; }
    //    public string Spool_Drawing_No { get; set; }
    //    public string Rev_No { get; set; }
    //    public string Sheet_No { get; set; }
    //    public string Joint_No { get; set; }
    //    public string Spool_No { get; set; }
    //    public string Line_Class { get; set; }
    //    public string Size { get; set; }
    //    public string Thickness { get; set; }
    //    public DateTime FitUp_Date { get; set; }
    //    public DateTime Welded_Date { get; set; }
    //    public string Weld_Type { get; set; }
    //    public string Root_Weld_Process { get; set; }
    //    public string Root_Welder_1 { get; set; }
    //    public bool Root_Welder_1_Production_Joint { get; set; }
    //    public string Root_Welder_2 { get; set; }
    //    public bool Root_Welder_2_Production_Joint { get; set; }
    //    public string Root_Welder_3 { get; set; }
    //    public bool Root_Welder_3_Production_Joint { get; set; }
    //    public string Root_Welder_4 { get; set; }
    //    public bool Root_Welder_4_Production_Joint { get; set; }
    //    public string FillCap_Weld_Process { get; set; }
    //    public string FillCap_Welder_1 { get; set; }
    //    public bool FillCap_Welder_1_Production_Joint { get; set; }
    //    public string FillCap_Welder_2 { get; set; }
    //    public bool FillCap_Welder_2_Production_Joint { get; set; }
    //    public string FillCap_Welder_3 { get; set; }
    //    public bool FillCap_Welder_3_Production_Joint { get; set; }
    //    public string FillCap_Welder_4 { get; set; }
    //    public bool FillCap_Welder_4_Production_Joint { get; set; }
    //    public string WPS_No { get; set; }
    //    public string Base_Metal_1 { get; set; }
    //    public string Heat_No_1 { get; set; }
    //    public string Base_Metal_2 { get; set; }
    //    public string Heat_No_2 { get; set; }
    //    public string VI { get; set; }
    //    public string VI_Comment { get; set; }
    //    public string DI { get; set; }
    //    public string DI_Comment { get; set; }
    //    public string Remarks { get; set; }
    //    public string Work_Code { get; set; }
    //    public string Ident_Code1 { get; set; }
    //    public string Ident_Code2 { get; set; }
    //    public int RejectedByUserID { get; set; }
    //    public string Error { get; set; }
    //}

    public class DWR
    {
        public int ID { get; set; }
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
        public bool Selected { get; set; }
        public string SelectedImage
        {
            get
            {
                if (Selected)
                    return "Greenradio.png";
                else
                    return "Grayradio.png";
            }

        }
    }
}

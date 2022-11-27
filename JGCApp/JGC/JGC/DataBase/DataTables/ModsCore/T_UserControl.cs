using SQLite;
using System;

namespace JGC.DataBase.DataTables.ModsCore
{
    [Table("T_UserControl")]
    public class T_UserControl
    {
        public string ID { get; set; }
        public string FullName { get; set; }
        public DateTime Expiry { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public bool ModsUser { get; set; }
        public bool Cert_Contractor { get; set; }
        public bool Cert_Inspection { get; set; }
        public bool Cert_Client { get; set; }
        public string CCMSDiscipline { get; set; }
        public string Company { get; set; }
        public string SubContractor { get; set; }
        public bool materials_access { get; set; }
        public bool completions_access { get; set; }
        public bool jobSettingAccess { get; set; }
        public bool progressTrackingAccess { get; set; }
        public bool jointIntegrityAccess { get; set; }
        public bool jiFitUp { get; set; }
        public bool jiTensioning { get; set; }
        public bool jiTorque { get; set; }
        public bool jiInspection { get; set; }
        public bool jiClient { get; set; }
        public bool jiBreakInspection { get; set; }
        public bool jiPostAssembly { get; set; }
        public bool fab_access { get; set; }
        public bool jiFieldVerification { get; set; }
        public string NSFID { get; set; }
        public bool ETP_Admin { get; set; }
        public bool ETP_SuperUser { get; set; }
        public string Function_Code { get; set; }
        public string Company_Code { get; set; }
        public string Company_Category_Code { get; set; }
        public string Discipline_Code { get; set; }
        public string Section_Code { get; set; }
        public string UserProjects { get; set; }
        public string FWBS_Access { get; set; }
        public bool EPermit_Admin { get; set; }
        public bool EPermit_ReadOnly { get; set; }
    }
}

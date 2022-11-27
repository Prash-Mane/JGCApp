using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR8000_101A_Generalnformation")]
    public class T_ITR8000_101A_Generalnformation
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public int ID { get; set; }
        public long ITRCommonID { get; set; }
        public string ITRNumber { get; set; }
        public string TagNo { get; set; }
        public string AfiNo { get; set; }
        public string GeneralManufacturer { get; set; }
        public string GeneralModelNo { get; set; }
        public string SerialNo { get; set; }
        public string ExCertNo { get; set; }
        public string AreaClassification { get; set; }
        public string Zone { get; set; }
        public string GasGroup1 { get; set; }
        public string TempClass1 { get; set; }
        public string EquipmentExTechnique { get; set; }
        public string Exprotection { get; set; }
        public string GasGroup2 { get; set; }
        public string TempClass2 { get; set; }
        public string EquipmentProtectionLevelEPL { get; set; }
        public string IngressProtectionIPRating { get; set; }
        public string HAEVerificationDossierNo { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqGeneralManufacturer { get; set; }
        [Ignore]
        public bool IsReqGeneralModelNo { get; set; }
        [Ignore]
        public bool IsReqSerialNo { get; set; }
        [Ignore]
        public bool IsReqExCertNo { get; set; }
        [Ignore]
        public bool IsReqZone { get; set; }
        [Ignore]
        public bool IsReqGasGroup1 { get; set; }
        [Ignore]
        public bool IsReqTempClass1 { get; set; }
        [Ignore]
        public bool IsReqExprotection { get; set; }
        [Ignore]
        public bool IsReqGasGroup2 { get; set; }
        [Ignore]
        public bool IsReqTempClass2 { get; set; }
        [Ignore]
        public bool IsReqEquipmentProtectionLevelEPL { get; set; }
        [Ignore]
        public bool IsReqIngressProtectionIPRating { get; set; }
        [Ignore]
        public bool IsReqHAEVerificationDossierNo { get; set; }
    }
}

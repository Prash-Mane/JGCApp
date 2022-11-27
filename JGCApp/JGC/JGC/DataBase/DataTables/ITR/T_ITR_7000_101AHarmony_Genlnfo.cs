using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR_7000_101AHarmony_Genlnfo")]
    public class T_ITR_7000_101AHarmony_Genlnfo
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public int ID { get; set; }
        public string ITRNumber { get; set; }
        public string TagNo { get; set; }
        public string GeneralManufacturer { get; set; }
        public string GeneralModelNo { get; set; }
        public string SerialNo { get; set; }
        public string ExCertNo { get; set; }
        public string AreaClassificationZone { get; set; }
        public string AreaClassificationGasGroup { get; set; }
        public string AreaClassificationTempClass { get; set; }
        public string EquipmentExTechnique { get; set; }
        public string EquipmentGasGroup { get; set; }
        public string EquipmentTempClass { get; set; }
        public string EquipmentIPRating { get; set; }
        public string IsBarrierManufacturer { get; set; }
        public string IsBarrierModel { get; set; }
        public string IsBarrierCertificateNo { get; set; }
        public string IsBarrierEntityCalcDocNo { get; set; }
        public string IsBarrierLocation { get; set; }
        public string IsBarrierCabinetNo { get; set; }
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
        public bool IsReqAreaClassificationZone { get; set; }
        [Ignore]
        public bool IsReqAreaClassificationGasGroup { get; set; }
        [Ignore]
        public bool IsReqAreaClassificationTempClass { get; set; }
        [Ignore]
        public bool IsReqEquipmentExTechnique { get; set; }
        [Ignore]
        public bool IsReqEquipmentGasGroup { get; set; }
        [Ignore]
        public bool IsReqEquipmentTempClass { get; set; }
        [Ignore]
        public bool IsReqEquipmentIPRating { get; set; }
        [Ignore]
        public bool IsBarrierManufacturerReadOnly { get; set; }
        [Ignore]
        public bool IsBarrierModelReadOnly { get; set; }
        [Ignore]
        public bool IsBarrierCertificateNoReadOnly { get; set; }
        [Ignore]
        public bool IsBarrierLocationReadOnly { get; set; }
        [Ignore]
        public bool IsBarrierCabinetNoReadOnly { get; set; }
    }
}

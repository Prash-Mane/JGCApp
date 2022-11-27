using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR_8140_002A_Records")]
	public class T_ITR_8140_002A_Records
	{
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public int ID { get; set; }
        public int ITRCommonID { get; set; }
        public string ITRNumber { get; set; }
        public string TagNo { get; set; }
        public string CBCloseECResult { get; set; }
        public string CBCloseERemarks { get; set; }
        public string CBOpenECResult { get; set; }
        public string CBOpenERemarks { get; set; }
        public string CBSpringCECResult { get; set; }
        public string CBSpringCERemarks { get; set; }
        public string CBControlSCResult { get; set; }
        public string CBControlSRemarks { get; set; }
        public string LocalRemoteSSCResult { get; set; }
        public string LocalRemoteSSRemarks { get; set; }
        public string AutoRSCResult { get; set; }
        public string AutoRSRemarks { get; set; }
        public string AmpereSSCResult { get; set; }
        public string AmpereSSRemarks { get; set; }
        public string VoltSSCResult { get; set; }
        public string VoltSSRemarks { get; set; }
        public string LimitSForCBPResult { get; set; }
        public string LimitSForCBPRemarks { get; set; }
        public string LimitSForEarthingSResult { get; set; }
        public string LimitSForEarthingSRemarks { get; set; }
        public string CBAuxiliaryCCResult { get; set; }
        public string CBAuxiliaryCRemarks { get; set; }
        public string AuxiliaryRelayCCResult { get; set; }
        public string AuxiliaryRelayCRemarks { get; set; }
        public string CBIndicationONCResult { get; set; }
        public string CBIndicationONRemarks { get; set; }
        public string CBIndicationOFFCResult { get; set; }
        public string CBIndicationOFFRemarks { get; set; }
        public string FaultLICResult { get; set; }
        public string FaultLIRemarks { get; set; }
        public string SpareCOCResult { get; set; }
        public string SpareCORemarks { get; set; }
        public string SignalCOCResult { get; set; }
        public string SignalCORemarks { get; set; }
        public string WithUpstream1CResult { get; set; }
        public string WithUpstream1Remarks { get; set; }
        public string WithDownstream1CResult { get; set; }
        public string WithDownstream1Remarks { get; set; }
        public string WithUpstream2CResult { get; set; }
        public string WithUpstream2Remarks { get; set; }
        public string WithDownstream2CResult { get; set; }
        public string WithDownstream2Remarks { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string ESCloseECResult { get; set; }
        public string ESCloseECRemark { get; set; }
        public string ESOpenECResult { get; set; }
        public string ESOpenECRemark { get; set; }
        public string DSCloseECResult { get; set; }
        public string DSCloseECRemark { get; set; }
        public string DSOpenECResult { get; set; }
        public string DSOpenECRemark { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsReqCBCloseECResult { get; set; }
        [Ignore]
        public bool IsReqCBOpenECResult { get; set; }
        [Ignore]
        public bool IsReqCBSpringCECResult { get; set; }
        [Ignore]
        public bool IsReqCBControlSCResult { get; set; }
        [Ignore]
        public bool IsReqLocalRemoteSSCResult { get; set; }
        [Ignore]
        public bool IsReqAutoRSCResult { get; set; }
        [Ignore]
        public bool IsReqAmpereSSCResult { get; set; }
        [Ignore]
        public bool IsReqVoltSSCResult { get; set; }
        [Ignore]
        public bool IsReqLimitSForCBPResult { get; set; }
        [Ignore]
        public bool IsReqLimitSForEarthingSResult { get; set; }
        [Ignore]
        public bool IsReqCBAuxiliaryCCResult { get; set; }
        [Ignore]
        public bool IsReqAuxiliaryRelayCCResult { get; set; }
        [Ignore]
        public bool IsReqCBIndicationONCResult { get; set; }
        [Ignore]
        public bool IsReqCBIndicationOFFCResult { get; set; }
        [Ignore]
        public bool IsReqFaultLICResult { get; set; }
        [Ignore]
        public bool IsReqSpareCOCResult { get; set; }
        [Ignore]
        public bool IsReqSignalCOCResult { get; set; }
        [Ignore]
        public bool IsReqWithUpstream1CResult { get; set; }
        [Ignore]
        public bool IsReqWithDownstream1CResult { get; set; }
        [Ignore]
        public bool IsReqWithUpstream2CResult { get; set; }
        [Ignore]
        public bool IsReqWithDownstream2CResult { get; set; }
        [Ignore]
        public bool IsReqESCloseECResult { get; set; }
        [Ignore]
        public bool IsReqESOpenECResult { get; set; }
        [Ignore]
        public bool IsReqDSCloseECResult { get; set; }
        [Ignore]
        public bool IsReqDSOpenECResult { get; set; }
    }
}

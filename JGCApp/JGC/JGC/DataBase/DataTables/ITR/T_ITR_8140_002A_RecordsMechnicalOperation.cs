using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR_8140_002A_RecordsMechnicalOperation")]
	public class T_ITR_8140_002A_RecordsMechnicalOperation
	{
		[PrimaryKey]
		public long CommonRowID { get; set; }
		public int ID { get; set; }
		public int ITRCommonID { get; set; }
		public string ITRNumber { get; set; }
		public string TagNo { get; set; }
		public string CBCCloseMResult { get; set; }
		public string CBCCloseMRemark { get; set; }
		public string CBCOpenMResult { get; set; }
		public string CBCOpenMRemark { get; set; }
		public string CBCIONMResult { get; set; }
		public string CBCIONMRemark { get; set; }
		public string CBCIOFFMResult { get; set; }
		public string CBCIOFFMRemark { get; set; }
		public string CBSpringCMResult { get; set; }
		public string CBSpringCMRemark { get; set; }
		public string CBSpringICResult { get; set; }
		public string CBSpringICRemark { get; set; }
		public string CBSpringDCResult { get; set; }
		public string CBSpringDCRemarks { get; set; }
		public string ShutterMResult { get; set; }
		public string ShutterMRemarks { get; set; }
		public string CBCRackoutPCResult { get; set; }
		public string CBCRackoutPRemark { get; set; }
		public string CBCTestCResult { get; set; }
		public string CBCTestRemark { get; set; }
		public string CBCServiceCResult { get; set; }
		public string CBCServiceRemark { get; set; }
		public string ESCloseCResult { get; set; }
		public string ESCloseRemark { get; set; }
		public string ESOpenCResult { get; set; }
		public string ESOpenRemark { get; set; }
		public string ESIndicationCloseCResult { get; set; }
		public string ESIndicationCloseRemark { get; set; }
		public string ESIndicationOpenCResult { get; set; }
		public string ESIndicationOpenRemark { get; set; }
		public string IESCBConductorResult { get; set; }
		public string IESCBConductorRemark { get; set; }
		public string ICBCCDResult { get; set; }
		public string ICBCCDRemark { get; set; }
		public string IESCCDResult { get; set; }
		public string IESCCDRemark { get; set; }
		public string ICBCDPResult { get; set; }
		public string ICBCDPRemark { get; set; }
		public int CCMS_HEADERID { get; set; }
		public string DSCloseMResult { get; set; }
		public string DSCloseMRemarks { get; set; }
		public string DSOpenMResult { get; set; }
		public string DSOpenMRemarks { get; set; }
		public string DSIndicationCloseResult { get; set; }
		public string DSIndicationCloseRemarks { get; set; }
		public string DSIndicationOpenResult { get; set; }
		public string DSIndicationOpenRemarks { get; set; }
		public string IESDSCBCCondustorResult { get; set; }
		public string IESDSCBCCondustorRemark { get; set; }
		public string ModelName { get; set; }
		public bool IsUpdated { get; set; }
		[Ignore]
		public bool IsReqCBCCloseMResult { get; set; }
		[Ignore]
		public bool IsReqCBCOpenMResult { get; set; }
		[Ignore]
		public bool IsReqCBCIONMResult { get; set; }
		[Ignore]
		public bool IsReqCBCIOFFMResult { get; set; }
		[Ignore]
		public bool IsReqCBSpringCMResult { get; set; }
		[Ignore]
		public bool IsReqCBSpringICResult { get; set; }
		[Ignore]
		public bool IsReqCBSpringDCResult { get; set; }
		[Ignore]
		public bool IsReqShutterMResult { get; set; }
		[Ignore]
		public bool IsReqCBCRackoutPCResult { get; set; }
		[Ignore]
		public bool IsReqCBCTestCResult { get; set; }
		[Ignore]
		public bool IsReqCBCServiceCResult { get; set; }
		[Ignore]
		public bool IsReqESCloseCResult { get; set; }
		[Ignore]
		public bool IsReqESOpenCResult { get; set; }
		[Ignore]
		public bool IsReqESIndicationCloseCResult { get; set; }
		[Ignore]
		public bool IsReqESIndicationOpenCResult { get; set; }
		[Ignore]
		public bool IsReqIESCBConductorResult { get; set; }
		[Ignore]
		public bool IsReqICBCCDResult { get; set; }
		[Ignore]
		public bool IsReqIESCCDResult { get; set; }
		[Ignore]
		public bool IsReqICBCDPResult { get; set; }
		[Ignore]
		public bool IsReqDSCloseMResult { get; set; }
		[Ignore]
		public bool IsReqDSOpenMResult { get; set; }
		[Ignore]
		public bool IsReqDSIndicationCloseResult { get; set; }
		[Ignore]
		public bool IsReqDSIndicationOpenResult { get; set; }
		[Ignore]
		public bool IsReqIESDSCBCCondustorResult { get; set; }
		
	}
}

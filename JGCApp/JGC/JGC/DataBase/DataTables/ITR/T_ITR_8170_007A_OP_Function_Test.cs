using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace JGC.DataBase.DataTables.ITR
{
    [Table("T_ITR_8170_007A_OP_Function_Test")]
   public class T_ITR_8170_007A_OP_Function_Test
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public int ID { get; set; }
        public long ITRCommonID { get; set; }
        public string InsResCheck { get; set; }
        public string PhaseRotation { get; set; }
        public string SuppVolDisL1L2 { get; set; }
        public string SuppVolDisL2L3 { get; set; }
        public string SuppVolDisL3L1 { get; set; }
        public string LocalSignal { get; set; }
        public string RemoteSignal { get; set; }
        public string MotorSignal { get; set; }
        public string SettingValCheck { get; set; }
        public string SignalOP4mA { get; set; }
        public string SignalOP8mA { get; set; }
        public string SignalOP12mA { get; set; }
        public string SignalOP16mA { get; set; }
        public string SignalOP20mA { get; set; }
        public string AlarmTest { get; set; }
        public string PanelSpaceHeaterFn { get; set; }
        public string InsResCheckRemarks { get; set; }
        public string PhaseRotationRemarks { get; set; }
        public string SuppVolDisRemarks { get; set; }
        public string LocalSignalRemarks { get; set; }
        public string RemoteSignalRemarks { get; set; }
        public string MotorSignalRemarks { get; set; }
        public string SettingValCheckRemarks { get; set; }
        public string SignalOPRemarks { get; set; }
        public string AlarmTestRemarks { get; set; }
        public string PanelSpaceHeaterFnRemarks { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string SignalOPSpeedmA { get; set; }
        public string SignalOPSpeedVSD { get; set; }
        public string SignalOPSpeedDCS { get; set; }
        public string SignalOPSpeedRemarks { get; set; }
        public string SignalOPCurrentmA { get; set; }
        public string SignalOPCurrentVSD { get; set; }
        public string SignalOPCurrentDCS { get; set; }
        public string SignalOPCurrentRemarks { get; set; }
        public string SignalOPActivePowermA { get; set; }
        public string SignalOPActivePowerVSD { get; set; }
        public string SignalOPActivePowerDCS { get; set; }
        public string SignalOPActivePowerRemarks { get; set; }
        public string ModelName { get; set; }
        [Ignore]
        public bool IsInsResCheck { get; set; }
        [Ignore]
        public bool IsPhaseRotation { get; set; }
        [Ignore]
        public bool IsSuppVolDisL1L2 { get; set; }
        [Ignore]
        public bool IsSuppVolDisL2L3 { get; set; }
        [Ignore]
        public bool IsSuppVolDisL3L1 { get; set; }
        [Ignore]
        public bool IsLocalSignal { get; set; }
        [Ignore]
        public bool IsRemoteSignal { get; set; }
        [Ignore]
        public bool IsMotorSignal { get; set; }
        [Ignore]
        public bool IsSettingValCheck { get; set; }
        [Ignore]
        public bool IsSignalOP4mA { get; set; }
        [Ignore]
        public bool IsSignalOP8mA { get; set; }
        [Ignore]
        public bool IsSignalOP12mA { get; set; }
        [Ignore]
        public bool IsSignalOP16mA { get; set; }
        [Ignore]
        public bool IsSignalOP20mA { get; set; }
        [Ignore]
        public bool IsAlarmTest { get; set; }
        [Ignore]
        public bool IsPanelSpaceHeaterFn { get; set; }
    }
}

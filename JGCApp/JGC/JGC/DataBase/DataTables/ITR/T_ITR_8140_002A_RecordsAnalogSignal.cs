using SQLite;

namespace JGC.DataBase.DataTables.ITR
{
    public class T_ITR_8140_002A_RecordsAnalogSignal
    {
        [PrimaryKey]
        public long CommonRowID { get; set; }
        public int ID { get; set; }
        public int ITRCommonID { get; set; }
        public int CCMS_HEADERID { get; set; }
        public string ITRNumber { get; set; }
        public string TagNo { get; set; }
        public string Current_mA { get; set; }
        public string CurrentOnSWGR { get; set; }
        public string CurrentOnDCS { get; set; }

        public string Voltage_mA { get; set; }
        public string VoltageOnSWGR { get; set; }
        public string VoltageOnDCS { get; set; }

        public string FrequencyOnSWGR { get; set; }
        public string FrequencyOnDCS { get; set; }

        public string ActivePower_mA { get; set; }
        public string ActivePowerOnSWGR { get; set; }
        public string ActivePowerOnDCS { get; set; }

        public string ReactivePower_mA { get; set; }
        public string ReactivePowerOnSWGR { get; set; }
        public string ReactivePowerOnDCS { get; set; }

        public string PowerFactorOnSWGR { get; set; }
        public string PowerFactorOnDCS { get; set; }

        public string PowerConsumptionPulse { get; set; }
        public string PowerConsumptionOnSWGR { get; set; }
        public string PowerConsumptionOnDCS { get; set; }
        public string ModelName { get; set; }
        public bool IsUpdated { get; set; }
        [Ignore]
        public bool IsCurrent_mA { get; set; }
        [Ignore]
        public bool IsActivePower_mA { get; set; }
        [Ignore]
        public bool IsReactivePower_mA { get; set; }
        [Ignore]
        public bool IsVoltage_mA { get; set; }
        [Ignore]
        public bool IsPowerConsumptionPulse { get; set; }
    }
}

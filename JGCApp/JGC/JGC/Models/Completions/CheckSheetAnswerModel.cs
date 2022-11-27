using JGC.DataBase.DataTables.Completions;
using JGC.Models.ITR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace JGC.Models.Completions
{

    public partial class CheckSheetAnswerModel
    {
        [JsonProperty("checkSheetName")]
        public string CheckSheetName { get; set; }

        [JsonProperty("tagName")]
        public string TagName { get; set; }

        [JsonProperty("jobPack")]
        public string JobPack { get; set; }

        [JsonProperty("ccmsHeaderId")]

        public string CcmsHeaderId { get; set; }

        [JsonProperty("cctrHeaderID")]
        public object CctrHeaderId { get; set; }

        [JsonProperty("tagSheetHeaders")]
        public TagSheetHeaders TagSheetHeaders { get; set; }

        [JsonProperty("signOffHeaders")]
        public List<T_SignOffHeader> SignOffHeaders { get; set; }

        [JsonProperty("answers")]
        public List<T_TAG_SHEET_ANSWER> answers { get; set; }
    }

    public partial class SignOffHeader
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("signDate")]
        public long SignDate { get; set; }

        [JsonProperty("rejected")]
        public bool Rejected { get; set; }

        [JsonProperty("rejectedReason")]
        public string RejectedReason { get; set; }
    }

    public partial class TagSheetHeaders
    {
        public string Project { get; set; }
        public string Jobcode { get; set; }
        [JsonProperty("Site")]
        public string Site { get; set; }
        public string DwgRevNo { get; set; }
        [JsonProperty("Tag/Cable No")]
        public string TagCableNo { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("Drawing")]
        public string Drawing { get; set; }
        [JsonProperty("PCWBS")]
        public string PCWBS { get; set; }
        [JsonProperty("FWBS")]
        public string FWBS { get; set; }
        [JsonProperty("Phase")]
        public string Phase { get; set; }
        [JsonProperty("System Description")]
        public string SystemDescription { get; set; }
        [JsonProperty("System")]
        public string System { get; set; }
        [JsonProperty("Barcode")]
        public string Barcode { get; set; }
        [JsonProperty("Location")]
        public string Location { get; set; }
        [JsonProperty("Sub System Description")]
        public string SubSystemDescription { get; set; }
        [JsonProperty("Work Pack")]
        public string WorkPack { get; set; }
        [JsonProperty("Area")]
        public string Area { get; set; }
        [JsonProperty("Sub System")]
        public string SubSystem { get; set; }
        [JsonProperty("Tag Function")]
        public string TagFunction { get; set; }
        [JsonProperty("Tag Category")]
        public string TagCategory { get; set; }
        [JsonProperty("Tag Class")]
        public string TagClass { get; set; }

        // public string __invalid_name__Dwg Rev No. { get; set; }
        // public string __invalid_name__Tag Class { get; set; }
        // public string __invalid_name__Tag Category { get; set; }
        // public string __invalid_name__Tag Function { get; set; }

    }
    public class Answer
    {
        public string itemno { get; set; }
        public string checkValue { get; set; }
        public string isChecked { get; set; }
        public DateTime isDate { get; set; }
        public string completedBy { get; set; }
        public DateTime? completedOn { get; set; }
        public string jobPack { get; set; }
        public bool synced { get; set; }

    }


    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    public class ITRs
    {
        public ITR7000_030A_031AModel ITR7000_030A_031A { get; set; }
        public ITR7000_040A_041A_042AModel ITR7000_040A_041A_042A { get; set; }
        public ITR7000_080A_090A_091AModel ITR7000_080A_090A_091A { get; set; }
        public ITR_8000_003AModel ITR_8000_003A { get; set; }
        public ITR8100_001AModel ITR8100_001A { get; set; }
        public ITR8100_002AModel ITR8100_002A { get; set; }
        public ITR8121_002AModel ITR8121_002A { get; set; }
        public ITR8121_004AModel ITR8121_004A { get; set; }
        public ITR8140_001AModel ITR8140_001A { get; set; }
        public ITR8161_001AModel ITR8161_001A { get; set; }
        public ITR_8260_002AModel ITR_8260_002A { get; set; }
        public ITR8161_002AModel ITR_8161_002A { get; set; }
        public ITR8000_101AModel ITR_8000_101A { get; set; }
        public ITR8211_002AModel ITR_8211_002A { get; set; }
        public ITR7000_101AHarmonyModel ITR7000_101AHarmony { get; set; }
        public ITR8140_002AModel ITR_8140_002A { get; set; }
        public ITR_8140_004AModel ITR_8140_004A { get; set; }
        public ITR8170_002AModel ITR8170_002A { get; set; }
        public ITR_8300_003AModel ITR8300_003A { get; set; }
        public ITR8170_007AModel ITR_8170_007A { get; set; }
        
    }
}

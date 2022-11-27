using JGC.Common.Constants;
using JGC.DataBase.DataTables;
using JGC.DataBase.DataTables.WorkPack;
using JGC.Models;
using JGC.Models.E_Test_Package;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace JGC.Common.Helpers
{
    public class ModsTools
    {
        public static string DateFormat = "dd-MMM-yyyy";
        private static string VMToken1 = "jv5980fj90ewFdr234rfTfsdsF53gssstfsdn4387ns9FGfd3f";
        private static string VMToken2 = "jvr890eJ78jjthjghvxcv3ffgSsfd233";
        private static string VMToken3 = "K675dVsgetSgeawJdHk96G54xgdr";

        public static bool WorkCompletedEnabled = false;
        public static bool WorkConfirmedEnabled = false;

        public static bool CheckForInternetConnection(string Token)
        {
            try
            {
                string JsonString = ModsTools.WebServiceGetWithTimeout("ConnectionTest", Token);

                ConnectionTest CurrentConnectionTest = JsonConvert.DeserializeObject<ConnectionTest>(JsonString);

                return (CurrentConnectionTest.Result.ToUpper() == "SUCCESS");
            }
            catch
            {
                return false;
            }
        }
        public static string GenerateHashForString(string input)
        {
            HashAlgorithm Hasher = new SHA256CryptoServiceProvider();
            byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(input);
            byte[] strHash = Hasher.ComputeHash(strBytes);
            return BitConverter.ToString(strHash).Replace("-", "").ToLowerInvariant().Trim();
        }
        public static string CreateToken(string UserName, string Password, string TimeStamp)
        {
            return GenerateHashForString(AppConstant.SECRET_SHARED_TOKENMODS + UserName.ToUpper() + Password.ToUpper() + TimeStamp);
        }
        public static Boolean ValidateToken(string Token, string TimeStamp)
        {
            return WebServiceGetBooleanWithTimeout(ApiUrls.Url_token(TimeStamp), Token);
        }
        public static Boolean CompletionValidateToken(string Token, string TimeStamp)
        {
            return CompletionWebServiceGetBooleanWithTimeout(ApiUrls.GetToken(TimeStamp,Settings.CurrentDB), Token);
        }
        public static Boolean WebServiceGetBoolean(string Call, string Token)
        {
            Boolean Result = false;
            try
            {
                Uri uri = new Uri(Settings.Server_Url + Call);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                request.Method = WebRequestMethods.Http.Get;
                request.Headers.Add("Token", Token);
                request.Timeout = 30000;
                request.Accept = "application/json;  charset=utf-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.Unauthorized)
                {
                    var streamReader = new StreamReader(response.GetResponseStream());
                    string StringResult = streamReader.ReadToEnd();

                    Result = JsonConvert.DeserializeObject<Boolean>(StringResult);

                    response.Close();
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                Result = false;
            }

            return Result;
        }
        public static Boolean WebServiceGetBooleanWithTimeout(string Call, string Token)
        {
            Boolean Result = false;
            try
            {
                Uri uri = new Uri(Settings.Server_UrlForConstructionModule + Call);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                request.Method = WebRequestMethods.Http.Get;
                request.Headers.Add("Token", Token);
                request.Timeout = 10000;
                request.Accept = "application/json;  charset=utf-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.Unauthorized)
                {
                    var streamReader = new StreamReader(response.GetResponseStream());
                    string StringResult = streamReader.ReadToEnd();

                    Result = JsonConvert.DeserializeObject<Boolean>(StringResult);

                    response.Close();
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                Result = false;
            }

            return Result;
        }
        public static Boolean CompletionWebServiceGetBooleanWithTimeout(string Call, string Token)
        {
            Boolean Result = false;
            try
            {
                Uri uri = new Uri(Settings.CompletionServer_Url + Call);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                request.Method = WebRequestMethods.Http.Get;
                request.Headers.Add("Token", Token);
                request.Timeout = 10000;
                request.Accept = "application/json;  charset=utf-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.Unauthorized)
                {
                    var streamReader = new StreamReader(response.GetResponseStream());
                    string StringResult = streamReader.ReadToEnd();

                    Result = StringResult.Contains("success");

                    response.Close();
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                Result = false;
            }

            return Result;
        }
        public static string WebServiceGetWithTimeout(string Call, string Token)
        {
            string JsonString = "";
            Uri uri = new Uri(Settings.Server_Url + Call);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;
            request.Headers.Add("Token", Token);
            request.Timeout = 10000;
            request.Accept = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            try
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    JsonString += streamReader.ReadToEnd();
                }
            }
            catch (Exception err)
            {
                string error = err.Message;               
            }

            return JsonString;
        }
        public static string WebServiceGet(string Call, string Token)
        {
            string JsonString = "";
            Uri uri = new Uri(Settings.Server_UrlForConstructionModule + Call);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;
            request.Headers.Add("Token", Token);
            request.Timeout = 300000;
            request.Accept = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            try
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    JsonString += streamReader.ReadToEnd();
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
            }

            return JsonString;
        }
        public static string WebServicePost(string Call, string JSONString, string Token)
        {
            string Result = "";
            Uri uri = new Uri(Settings.Server_Url + Call);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Post;
            request.Timeout = 300000;
            request.Headers.Add("Token", Token);
            request.ContentType = "application/json; charset=utf-8";

            byte[] bytes = Encoding.UTF8.GetBytes(JSONString);

            using (var streamwriter = request.GetRequestStream())
            {
                streamwriter.Write(bytes, 0, bytes.Length);
            }

           
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    Result = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                Result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }

            return Result;
        }
        public static Boolean WebServicePostBoolean(string Call, string Token)
        {
            Boolean Result = false;
            Uri uri = new Uri(Settings.Server_Url + Call);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Post;
            request.Timeout = 300000;
            request.Headers.Add("Token", Token);
            //request.ContentType = "application/json; charset=utf-8";
            request.ContentLength = 0;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            try
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string StringResult = streamReader.ReadToEnd();

                    Result = JsonConvert.DeserializeObject<Boolean>(StringResult);
                }
            }
            catch (Exception err)
            {
                //err.Message;
            }

            return Result;
        }

        public static Boolean WebServiceVMHub(VMHub CurrentVMHub, string Token)
        {
            Boolean Result = false;
            Uri uri = new Uri(Settings.Server_Url + "VMHub");
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Post;
            request.Timeout = 300000;
            request.Headers.Add("Token", Token);
            request.ContentType = "application/json; charset=utf-8";

            string Value = ModsTools.ToJson(CurrentVMHub);
            byte[] bytes = Encoding.UTF8.GetBytes(Value);

            using (var streamwriter = request.GetRequestStream())
            {
                streamwriter.Write(bytes, 0, bytes.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            try
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string StringResult = streamReader.ReadToEnd();

                    Result = JsonConvert.DeserializeObject<Boolean>(StringResult);
                }
            }
            catch
            {

            }

            return Result;
        }

        public static string ToJson(Object Item)
        {
            return JsonConvert.SerializeObject(Item);
        }

        public static string AdjustDateTime(DateTime value, TimeZoneInfo projectTimeZone)
        {
            DateTime SignedOnUTC = Convert.ToDateTime(value);
            SignedOnUTC = TimeZoneInfo.ConvertTimeFromUtc(SignedOnUTC, projectTimeZone);

            return SignedOnUTC.ToString(ModsTools.DateFormat);
        }
        //Test Pack
        public static Boolean CheckCanAddPunch(T_AdminPunchLayer currentLayer, T_UserDetails user)
        {
            return (user.Company_Category_Code.ToUpper() == currentLayer.CompanyCategoryCode.ToUpper()
                && user.Function_Code.ToUpper() == currentLayer.FunctionCode.ToUpper());
        }
        //Work pack
        public static Boolean CheckCanAddPunch_WP(T_IWPAdminPunchLayer currentLayer, T_UserDetails user)
        {
            return (user.Company_Category_Code.ToUpper() == currentLayer.CompanyCategoryCode.ToUpper()
                && user.Function_Code.ToUpper() == currentLayer.FunctionCode.ToUpper());
        }
        public static Boolean CheckCanAddPunch(T_UserDetails user)
        {
            return (user.Company_Category_Code.ToUpper() == CurrentPageHelper.CurrentLayer.CompanyCategoryCode.ToUpper()
                && user.Function_Code.ToUpper() == CurrentPageHelper.CurrentLayer.FunctionCode.ToUpper());
        }

        //EtestPackage Upload Methods 


        public static UploadResult WebServicePostWithResult(string Call, string JSONString, string Token)
        {
            UploadResult Result = new UploadResult();
            Uri uri = new Uri(Settings.Server_Url + Call);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Post;
            request.Timeout = 300000;
            request.Headers.Add("Token", Token);
            request.ContentType = "application/json; charset=utf-8";

            byte[] bytes = Encoding.UTF8.GetBytes(JSONString);

            using (var streamwriter = request.GetRequestStream())
            {
                streamwriter.Write(bytes, 0, bytes.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            try
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string StringResult = streamReader.ReadToEnd();

                    Result = JsonConvert.DeserializeObject<UploadResult>(StringResult);
                }
            }
            catch (Exception err)
            {
                Result.Error = err.Message;
                Result.Success = false;
            }

            return Result;
        }

        //public static OleDbParameter[] JsonToOleDbParameters(string JsonString)
        //{
        //    List<OleDbParameter> OleDbParameters = new List<OleDbParameter>();

        //    Dictionary<string, dynamic> Fields = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(JsonString);

        //    foreach (var Field in Fields)
        //    {
        //        if (Field.Value != null && Field.Value.GetType() != typeof(Array))
        //        {
        //            string CurrentValue = Field.Value.ToString();

        //            if (Field.Value.GetType() == typeof(DateTime))
        //                CurrentValue = Field.Value.ToString(Constants.DateSaveFormat);

        //            OleDbParameters.Add(new OleDbParameter("@" + Field.Key.ToUpper(), CurrentValue));
        //        }
        //    }

        //    return OleDbParameters.ToArray();
        //}


        //public static DataTable SetupSearchDataTable()
        //{
        //    DataTable CurrentDataTable = new DataTable();

        //    DataColumn CurrentColumn = new DataColumn("ID", typeof(int));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("ReportType", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("PCWBS", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("AFINo", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("ReportNo", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("ReportDate", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Priority", typeof(Image));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Updated", typeof(Image));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    return CurrentDataTable;
        //}

        //public static DataTable SetupDownloadDataTable()
        //{
        //    DataTable CurrentDataTable = new DataTable();

        //    DataColumn CurrentColumn = new DataColumn("ID", typeof(int));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("ReportType", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("PCWBS", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("AFINo", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("ReportNo", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("ReportDate", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Priority", typeof(Image));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    return CurrentDataTable;
        //}

        //public static DataTable SetupSignaturesDataTable()
        //{
        //    DataTable CurrentDataTable = new DataTable();

        //    DataColumn CurrentColumn = new DataColumn("SignatureRulesID", typeof(int));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Signed", typeof(Image));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("DisplaySignatureName", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("SignedByFullName", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("SignedOn", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    return CurrentDataTable;
        //}

        //public static DataTable SetupRIRDataTable()
        //{
        //    DataTable CurrentDataTable = new DataTable();

        //    DataColumn CurrentColumn = new DataColumn("PO_No", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("PO_Title", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Vendor", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Item_No", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Partial_No", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Item_Description", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    return CurrentDataTable;
        //}
        //public static DataTable SetupMRRDataTable()
        //{
        //    DataTable CurrentDataTable = new DataTable();

        //    DataColumn CurrentColumn = new DataColumn("Updated", typeof(Image));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("PO_No", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("PJ_Sub_Commodity", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Item_No", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("PT_No", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Description", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    return CurrentDataTable;
        //}

        //public static DataTable SetupMRRStorageAreaDataTable()
        //{
        //    DataTable CurrentDataTable = new DataTable();

        //    DataColumn CurrentColumn = new DataColumn("ID", typeof(int));
        //    CurrentColumn.AutoIncrement = true;
        //    CurrentColumn.AutoIncrementSeed = 1;
        //    CurrentColumn.AutoIncrementStep = 1;
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Storage_Area_Available", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Good_Quantity", typeof(double));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Delete", typeof(Image));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    return CurrentDataTable;
        //}

        //public static DataTable SetupMRRHeatNoDataTable()
        //{
        //    DataTable CurrentDataTable = new DataTable();

        //    DataColumn CurrentColumn = new DataColumn("ID", typeof(int));
        //    CurrentColumn.AutoIncrement = true;
        //    CurrentColumn.AutoIncrementSeed = 1;
        //    CurrentColumn.AutoIncrementStep = 1;
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Heat_No", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Quantity", typeof(double));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Delete", typeof(Image));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    return CurrentDataTable;
        //}

        //public static void GenerateRIRTable(DataTable CurrentDataTable, RIR CurrentRIR)
        //{
        //    if (CurrentDataTable.Rows != null)
        //        CurrentDataTable.Rows.Clear();

        //    if (CurrentRIR.RIRRows != null)
        //    {
        //        foreach (RIRRow Row in CurrentRIR.RIRRows)
        //        {
        //            CurrentDataTable.Rows.Add(new object[] { Row.PO_No, Row.PO_Title, Row.Vendor, Row.Item_No, Row.Partial_No, Row.Item_Description });
        //        }
        //    }

        //}

        //public static void GenerateMRRTable(DataTable dataTable, MRR mrr, string searchValue)
        //{
        //    if (dataTable.Rows != null)
        //        dataTable.Rows.Clear();

        //    if (mrr.MRRRows != null)
        //    {
        //        foreach (MRRRow row in mrr.MRRRows)
        //        {
        //            Boolean CanAdd = true;
        //            if (searchValue != string.Empty)
        //            {
        //                string RowValue = row.PO_No + " " + row.PJ_Sub_Commodity + " " + row.Item_No + " " + row.PT_No + " " + row.Description;

        //                if (!RowValue.ToUpper().Contains(searchValue.ToUpper()))
        //                    CanAdd = false;
        //            }

        //            if (CanAdd)
        //            {
        //                Image UpdatedImage = null;

        //                if (row.Updated)
        //                    UpdatedImage = jgcereporter.Properties.Resources.upload_green;
        //                else
        //                    UpdatedImage = jgcereporter.Properties.Resources.empty;

        //                dataTable.Rows.Add(new object[] { UpdatedImage, row.PO_No, row.PJ_Sub_Commodity, row.Item_No, row.PT_No, row.Description });
        //            }
        //        }
        //    }

        //}

        //public static void GenerateMRRStorageAreaTable(DataTable dt, List<MRRStorageAreas> storageAreas)
        //{
        //    if (dt.Rows != null)
        //        dt.Rows.Clear();

        //    if (storageAreas != null && storageAreas.Count > 0)
        //    {
        //        foreach (MRRStorageAreas storageArea in storageAreas)
        //        {
        //            Image Delete = jgcereporter.Properties.Resources.dgdelete;

        //            dt.Rows.Add(new object[] { null, storageArea.Storage_Area, storageArea.Good_Quantity, Delete });
        //        }
        //    }

        //}


        //public static void GenerateCMRStorageAreaTable(DataTable dt, List<CMRStorageAreas> storageAreas)
        //{
        //    if (dt.Rows != null)
        //        dt.Rows.Clear();

        //    if (storageAreas != null && storageAreas.Count > 0)
        //    {
        //        foreach (CMRStorageAreas storageArea in storageAreas)
        //        {
        //            Image Delete = jgcereporter.Properties.Resources.empty;

        //            if (!storageArea.Is_Partially_Issued)
        //                Delete = jgcereporter.Properties.Resources.dgdelete;

        //            dt.Rows.Add(new object[] { null, storageArea.Storage_Area_Available, storageArea.Avail_Stock_Qty, storageArea.Storage_Area_Code, storageArea.Issued_Qty, storageArea.Heat_No, storageArea.Issued_Date, storageArea.Is_Partially_Issued, Delete });
        //        }
        //    }
        //}

        //public static void GenerateMRRHeatNoTable(DataTable dt, List<MRRHeatNos> heatNos)
        //{
        //    if (dt.Rows != null)
        //        dt.Rows.Clear();

        //    if (heatNos != null && heatNos.Count > 0)
        //    {
        //        foreach (MRRHeatNos heatNo in heatNos)
        //        {
        //            Image Delete = jgcereporter.Properties.Resources.dgdelete;

        //            dt.Rows.Add(new object[] { null, heatNo.Heat_No, heatNo.Quantity, Delete });
        //        }
        //    }

        //}

        //public static void GenerateStatusTable(DataTable CurrentDataTable, EReport CurrentEReport, string CurrentTimeZone)
        //{
        //    if (CurrentDataTable.Rows != null)
        //        CurrentDataTable.Rows.Clear();

        //    if (CurrentEReport.Signatures != null)
        //    {
        //        foreach (Signature CurrentSignature in CurrentEReport.Signatures)
        //        {
        //            string SignedByFullName = "", SignedOn = "";
        //            DateTime SignedOnDate = Convert.ToDateTime("2000-01-01");

        //            Image SignedImage = jgcereporter.Properties.Resources.greydot;

        //            if (CurrentSignature.Signed)
        //            {
        //                SignedImage = jgcereporter.Properties.Resources.greendot;
        //                SignedByFullName = CurrentSignature.SignedByFullName;

        //                TimeZoneInfo ProjectTimeZone = TimeZoneInfo.FindSystemTimeZoneById(CurrentTimeZone);
        //                SignedOnDate = TimeZoneInfo.ConvertTimeFromUtc(CurrentSignature.SignedOn, ProjectTimeZone);
        //                SignedOn = SignedOnDate.ToString(ModsTools.DateFormat + " HH:mm");
        //            }




        //            CurrentDataTable.Rows.Add(new object[] { CurrentSignature.SignatureRulesID, SignedImage, CurrentSignature.DisplaySignatureName, SignedByFullName, SignedOn });
        //        }
        //    }

        //}

        //public static DataTable SetupCMRDataTable()
        //{
        //    DataTable CurrentDataTable = new DataTable();

        //    DataColumn CurrentColumn = new DataColumn("Updated", typeof(Image));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("PJ_Commodity", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("PJ_Sub_Commodity", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Size_Descr", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Unit", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Request_Qty", typeof(double));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Issued_Qty", typeof(double));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Issued_Date", typeof(DateTime));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Commodity_Descr", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    return CurrentDataTable;
        //}

        //public static DataTable SetupCMRStorageAreaDataTable()
        //{
        //    DataTable CurrentDataTable = new DataTable();

        //    DataColumn CurrentColumn = new DataColumn("ID", typeof(int));
        //    CurrentColumn.AutoIncrement = true;
        //    CurrentColumn.AutoIncrementSeed = 1;
        //    CurrentColumn.AutoIncrementStep = 1;
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Storage_Area_Available", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Avail_Stock_Qty", typeof(double));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Storage_Area_Code", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Issued_Qty", typeof(double));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Heat_No", typeof(string));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Issued_Date", typeof(DateTime));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Is_Partially_Issued", typeof(Boolean));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    CurrentColumn = new DataColumn("Delete", typeof(Image));
        //    CurrentDataTable.Columns.Add(CurrentColumn);

        //    return CurrentDataTable;
        //}

        //public static void GenerateCMRTable(DataTable dataTable, CMR cmr, string searchValue)
        //{
        //    if (dataTable.Rows != null)
        //        dataTable.Rows.Clear();

        //    if (cmr.CMRSummaryRows != null)
        //    {
        //        foreach (CMRSummaryRow row in cmr.CMRSummaryRows)
        //        {
        //            Boolean CanAdd = true;
        //            if (searchValue != string.Empty)
        //            {
        //                string RowValue = row.PJ_Commodity + " " + row.Sub_Commodity + " " + row.Size_Descr + " " + row.Unit + " " + row.Commodity_Descr;

        //                if (!RowValue.ToUpper().Contains(searchValue.ToUpper()))
        //                    CanAdd = false;
        //            }

        //            if (CanAdd)
        //            {
        //                Image UpdatedImage = null;

        //                if (row.Updated)
        //                    UpdatedImage = jgcereporter.Properties.Resources.upload_green;
        //                else
        //                    UpdatedImage = jgcereporter.Properties.Resources.empty;

        //                dataTable.Rows.Add(new object[] { UpdatedImage, row.PJ_Commodity, row.Sub_Commodity, row.Size_Descr, row.Unit, row.Request_Qty, row.Issued_Qty, row.Issued_Date, row.Commodity_Descr });
        //            }
        //        }
        //    }

        //}

        //public static void GenerateDownloadTable(List<T_EReports> EReportList, string SearchValue)
        //{
        //    if (EReportList == null)
        //        return;


        //    foreach (T_EReports CurrentEReport in EReportList)
        //    {
        //        Boolean CanAdd = true;

        //        if (SearchValue != string.Empty)
        //        {
        //            string RowValue = CurrentEReport.ReportType + " " + CurrentEReport.PCWBS + " " + CurrentEReport.AFINo + " " + CurrentEReport.ReportNo + " " + CurrentEReport.ReportDate.ToString(AppConstant.DateFormat);

        //            if (!RowValue.ToUpper().Contains(SearchValue.ToUpper()))
        //            {
        //                if (Settings.Report.ToUpper() == "DAILY WELD REPORT" && !string.IsNullOrEmpty(CurrentEReport.JSONString))
        //                {
        //                    JObject jobj = JObject.Parse(CurrentEReport.JSONString);

        //                    string[] firstProductNames = jobj["DWRRows"].Select(m => (string)m.SelectToken("Spool_Drawing_No"))
        //                    .Where(n => n != null && n.ToUpper().Contains(SearchValue.ToUpper())).ToArray();

        //                    CanAdd = !(firstProductNames.Length == 0 || firstProductNames == null);
        //                }
        //                else
        //                    CanAdd = false;
        //            }

        //        }

        //        if (CanAdd)
        //        {
        //           // Image PriorityImage = null;

        //           // if (CurrentEReport.Priority)
        //               // PriorityImage = jgcereporter.Properties.Resources.redflag;
        //           // else
        //              //  PriorityImage = jgcereporter.Properties.Resources.empty;

        //           // CurrentDataTable.Rows.Add(new object[] { CurrentEReport.ID, CurrentEReport.ReportType, CurrentEReport.PCWBS, CurrentEReport.AFINo, CurrentEReport.ReportNo, CurrentEReport.ReportDate.ToString(Constants.DateFormat), PriorityImage });
        //        }

        //    }
        //}

        //public static void GenerateGVList(DataTable CurrentDataTable, string ModelName, string ReportType, string SearchValue)
        //{
        //    if (CurrentDataTable.Rows != null)
        //        CurrentDataTable.Rows.Clear();

        //    string SQL = "SELECT * FROM [EReportsHH] WHERE [ModelName] = '" + ModelName + "' AND [ReportType] = '" + ReportType + "' ORDER BY [Priority] ASC,[ReportDate] DESC";


        //    OleDbConnection Connection = new OleDbConnection();
        //    try
        //    {
        //        Connection.ConnectionString = Constants.LocalConnectionString;
        //        Connection.Open();
        //        OleDbCommand SessionCMD = new OleDbCommand(SQL);
        //        SessionCMD.Connection = Connection;
        //        OleDbDataReader Reader = SessionCMD.ExecuteReader();
        //        while (Reader.Read())
        //        {
        //            int ID = SQLReader.GetInt(Reader, "ID");
        //            string ReportNo = SQLReader.GetString(Reader, "ReportNo");
        //            string PCWBS = SQLReader.GetString(Reader, "PCWBS");
        //            string AFINo = SQLReader.GetString(Reader, "AFINo");
        //            string ReportDate = SQLReader.GetDateTime(Reader, "ReportDate");
        //            Boolean Priority = SQLReader.GetBoolean(Reader, "Priority");
        //            Boolean Updated = SQLReader.GetBoolean(Reader, "Updated");

        //            Boolean CanAdd = true;

        //            if (SearchValue != string.Empty)
        //            {
        //                string RowValue = ReportType + " " + PCWBS + " " + AFINo + " " + ReportNo + " " + ReportDate;

        //                if (!RowValue.ToUpper().Contains(SearchValue.ToUpper()))
        //                {
        //                    string jsonString = SQLReader.GetString(Reader, "JsonString");

        //                    if (ReportType.ToUpper() == "DAILY WELD REPORT" && !string.IsNullOrEmpty(jsonString))
        //                    {
        //                        JObject jobj = JObject.Parse(jsonString);

        //                        string[] firstProductNames = jobj["DWRRows"].Select(m => (string)m.SelectToken("Spool_Drawing_No"))
        //                        .Where(n => n != null && n.ToUpper().Contains(SearchValue.ToUpper())).ToArray();

        //                        CanAdd = !(firstProductNames.Length == 0 || firstProductNames == null);
        //                    }
        //                    else
        //                        CanAdd = false;
        //                }
        //            }


        //            if (CanAdd)
        //            {
        //                Image UpdatedImage = null;

        //                if (Updated)
        //                    UpdatedImage = jgcereporter.Properties.Resources.upload_green;
        //                else
        //                    UpdatedImage = jgcereporter.Properties.Resources.empty;

        //                Image PriorityImage = null;

        //                if (Priority)
        //                    PriorityImage = jgcereporter.Properties.Resources.redflag;
        //                else
        //                    PriorityImage = jgcereporter.Properties.Resources.empty;

        //                CurrentDataTable.Rows.Add(new object[] { ID, ReportType, PCWBS, AFINo, ReportNo, ReportDate, PriorityImage, UpdatedImage });
        //            }

        //        }
        //        Reader.Close();
        //        Connection.Close();
        //    }
        //    catch (Exception err)
        //    {
        //        string Error = SQL;
        //        Connection.Close();
        //    }
        //    finally
        //    {
        //        Connection.Close();
        //    }


        //}

        //public static List<Drawing> GenerateDrawingList(int EReportID)
        //{
        //    List<Drawing> DrawingList = new List<Drawing>();

        //    string SQL = "SELECT * FROM [DrawingsHH] WHERE [EReportID] = " + EReportID;

        //    OleDbConnection Connection = new OleDbConnection();
        //    try
        //    {
        //        Connection.ConnectionString = Constants.LocalConnectionString;
        //        Connection.Open();
        //        OleDbCommand SessionCMD = new OleDbCommand(SQL);
        //        SessionCMD.Connection = Connection;
        //        OleDbDataReader Reader = SessionCMD.ExecuteReader();
        //        while (Reader.Read())
        //        {
        //            Drawing CurrentDrawing = new Drawing();
        //            CurrentDrawing.Name = SQLReader.GetString(Reader, "Name");
        //            CurrentDrawing.FileName = SQLReader.GetString(Reader, "FileName");
        //            CurrentDrawing.Sheet_No = SQLReader.GetString(Reader, "Sheet_No");
        //            CurrentDrawing.Total_Sheets = SQLReader.GetString(Reader, "Total_Sheets");
        //            CurrentDrawing.FileLocation = SQLReader.GetString(Reader, "FileLocation");

        //            DrawingList.Add(CurrentDrawing);
        //        }
        //        Reader.Close();
        //        Connection.Close();
        //    }
        //    catch (Exception err)
        //    {
        //        string Error = SQL;
        //        Connection.Close();
        //    }
        //    finally
        //    {
        //        Connection.Close();
        //    }

        //    return (DrawingList);
        //}


        //public static void GenerateVIDefects(ComboBox CurrentComboBox)
        //{
        //    CurrentComboBox.Items.Clear();

        //    string SQL = "SELECT [RT_DEFECT_CODE] + ' - ' + [DESCRIPTION] FROM [RT_DEFECTSHH] ORDER BY [RT_DEFECT_CODE] ASC";

        //    string[] DefectArray = LocalSQLFunctions.GetArray(SQL);

        //    object[] newValues = new object[DefectArray.Length + 1];
        //    newValues[0] = "";                                // set the prepended value
        //    Array.Copy(DefectArray, 0, newValues, 1, DefectArray.Length); // copy the old values

        //    CurrentComboBox.Items.AddRange(newValues);
        //}

        //public static void GenerateWPSNos(ComboBox CurrentComboBox, int projectID)
        //{
        //    CurrentComboBox.Items.Clear();

        //    string SQL = "SELECT [WPS_NO] + ' - ' + [DESCRIPTION] FROM [WPS_MSTRHH] WHERE [PROJECT_ID] = " + projectID + " ORDER BY [WPS_NO] ASC";

        //    string[] DefectArray = LocalSQLFunctions.GetArray(SQL);

        //    object[] newValues = new object[DefectArray.Length + 1];
        //    newValues[0] = "";                                // set the prepended value
        //    Array.Copy(DefectArray, 0, newValues, 1, DefectArray.Length); // copy the old values

        //    CurrentComboBox.Items.AddRange(newValues);
        //}

        //public static void GenerateBaseMetals(ComboBox CurrentComboBox)
        //{
        //    CurrentComboBox.Text = "";
        //    CurrentComboBox.Items.Clear();

        //    string SQL = "SELECT [BASE_MATERIAL] FROM [BASE_METALHH] ORDER BY [BASE_MATERIAL] ASC";

        //    string[] DefectArray = LocalSQLFunctions.GetArray(SQL);

        //    object[] newValues = new object[DefectArray.Length + 1];
        //    newValues[0] = "";                                // set the prepended value
        //    Array.Copy(DefectArray, 0, newValues, 1, DefectArray.Length); // copy the old values

        //    CurrentComboBox.Items.AddRange(newValues);
        //}

        //public static void GenerateHeatNos(ComboBox cbHeatNo1, ComboBox cbHeatNo2, int projectID, string identcode1, string identcode2)
        //{
        //    cbHeatNo1.Items.Clear();
        //    cbHeatNo2.Items.Clear();

        //    object[] fullHeatNoArray = null;
        //    if (string.IsNullOrEmpty(identcode1) || string.IsNullOrEmpty(identcode2))
        //    {
        //        fullHeatNoArray = LocalSQLFunctions.GetArray("SELECT [HEAT_NO] FROM [DWR_HEATNOSHH] WHERE [PROJECT_ID] = " + projectID + " ORDER BY [HEAT_NO] ASC");

        //        object[] newValues = new object[fullHeatNoArray.Length + 1];
        //        newValues[0] = "";                                // set the prepended value
        //        Array.Copy(fullHeatNoArray, 0, newValues, 1, fullHeatNoArray.Length); // copy the old values

        //        fullHeatNoArray = newValues;
        //    }



        //    if (!string.IsNullOrEmpty(identcode1))
        //    {
        //        object[] heatnoArray = LocalSQLFunctions.GetArray("SELECT [HEAT_NO] FROM [DWR_HEATNOSHH] WHERE [PROJECT_ID] = " + projectID + " AND [IDENT_CODE] = '" + identcode1.Replace("'", "''") + "' ORDER BY [HEAT_NO] ASC");

        //        object[] newValues = new object[heatnoArray.Length + 1];
        //        newValues[0] = "";                                // set the prepended value
        //        Array.Copy(heatnoArray, 0, newValues, 1, heatnoArray.Length); // copy the old values


        //        cbHeatNo1.Items.AddRange(newValues);
        //    }
        //    else
        //        cbHeatNo1.Items.AddRange(fullHeatNoArray);

        //    if (!string.IsNullOrEmpty(identcode2))
        //    {
        //        object[] heatnoArray = LocalSQLFunctions.GetArray("SELECT [HEAT_NO] FROM [DWR_HEATNOSHH] WHERE [PROJECT_ID] = " + projectID + " AND [IDENT_CODE] = '" + identcode2.Replace("'", "''") + "' ORDER BY [HEAT_NO] ASC");

        //        object[] newValues = new object[heatnoArray.Length + 1];
        //        newValues[0] = "";                                // set the prepended value
        //        Array.Copy(heatnoArray, 0, newValues, 1, heatnoArray.Length); // copy the old values

        //        cbHeatNo2.Items.AddRange(newValues);
        //    }
        //    else
        //        cbHeatNo2.Items.AddRange(fullHeatNoArray);



        //}


        //public static void GenerateWelders(ComboBox[] comboboxes, int projectID, string subContractor)
        //{
        //    foreach (ComboBox cb in comboboxes)
        //    {
        //        cb.Items.Clear();
        //    }

        //    string SQL = "SELECT [WELDER_CODE] + ' - ' + [WELDER_NAME] FROM [WELDERHH] WHERE [PROJECT_ID] = " + projectID + " AND [SUBCONTRACTOR] = '" + subContractor + "' ORDER BY [WELDER_CODE] ASC";

        //    string[] DefectArray = LocalSQLFunctions.GetArray(SQL);

        //    object[] newValues = new object[DefectArray.Length + 1];
        //    newValues[0] = "";                                // set the prepended value
        //    Array.Copy(DefectArray, 0, newValues, 1, DefectArray.Length); // copy the old values

        //    foreach (ComboBox cb in comboboxes)
        //    {
        //        cb.Items.AddRange(newValues);
        //    }
        //}

        //public static void GenerateStorageAreas(ComboBox CurrentComboBox, string storeLocation, int projectID)
        //{
        //    CurrentComboBox.Items.Clear();

        //    string SQL = "SELECT [STORAGE_AREA_CODE] FROM [STORAGE_AREASHH] WHERE [PROJECT_ID] = " + projectID + " AND [STORE_LOCATION] = '" + storeLocation + "' ORDER BY [STORAGE_AREA_CODE] ASC";

        //    string[] DefectArray = LocalSQLFunctions.GetArray(SQL);

        //    object[] newValues = new object[DefectArray.Length + 1];
        //    newValues[0] = "";                                // set the prepended value
        //    Array.Copy(DefectArray, 0, newValues, 1, DefectArray.Length); // copy the old values

        //    CurrentComboBox.Items.AddRange(newValues);
        //}

        //public static void GenerateCMRStorageAreas(ComboBox CurrentComboBox, CMR cmr, CMRSummaryRow row, int projectID)
        //{
        //    CurrentComboBox.Items.Clear();

        //    string SQL = "SELECT [STORAGE_AREA_CODE] & \" (Qty. \" & [AVAIL_STOCK_QTY] & \")\" FROM [STORAGE_AREASCMRHH]"
        //        + " WHERE [PROJECT_ID] = " + projectID
        //        + " AND [JOB_CODE_KEY] = '" + cmr.JobCode + "'"
        //        + " AND [STORE_LOCATION] = '" + cmr.Store_Location + "'"
        //        + " AND [PJ_COMMODITY] = '" + row.PJ_Commodity + "'"
        //        + " AND [SUB_COMMODITY] = '" + row.Sub_Commodity + "'"
        //        + " AND [SIZE_DESCR] = '" + row.Size_Descr + "'"
        //        + " ORDER BY [STORAGE_AREA_CODE] ASC";

        //    string[] DefectArray = LocalSQLFunctions.GetArray(SQL);

        //    object[] newValues = new object[DefectArray.Length + 1];
        //    newValues[0] = "";                                // set the prepended value
        //    Array.Copy(DefectArray, 0, newValues, 1, DefectArray.Length); // copy the old values

        //    CurrentComboBox.Items.AddRange(newValues);
        //}

        //public static void GenerateCMRHeatNos(ComboBox CurrentComboBox, CMR cmr, CMRSummaryRow row, int projectID)
        //{
        //    CurrentComboBox.Items.Clear();

        //    string SQL = "SELECT [HEAT_NO] FROM [HEAT_NOCMRHH]"
        //        + " WHERE [PROJECT_ID] = " + projectID
        //        + " AND [JOB_CODE_KEY] = '" + cmr.JobCode + "'"
        //        + " AND [STORE_LOCATION] = '" + cmr.Store_Location + "'"
        //        + " AND [PJ_COMMODITY] = '" + row.PJ_Commodity + "'"
        //        + " AND [SUB_COMMODITY] = '" + row.Sub_Commodity + "'"
        //        + " AND [SIZE_DESCR] = '" + row.Size_Descr + "'"
        //        + " ORDER BY [HEAT_NO] ASC";

        //    string[] DefectArray = LocalSQLFunctions.GetArray(SQL);

        //    object[] newValues = new object[DefectArray.Length + 1];
        //    newValues[0] = "";                                // set the prepended value
        //    Array.Copy(DefectArray, 0, newValues, 1, DefectArray.Length); // copy the old values

        //    CurrentComboBox.Items.AddRange(newValues);
        //}

        //public static string CreatePhotoDirectories(string JobCode, string EReportID, string Field)
        //{
        //    string Location = Constants.StartDirectory;
        //    string[] Folders = new string[] { "Photo Store", JobCode, EReportID, Field };

        //    foreach (string Folder in Folders)
        //    {
        //        Location += ("/" + Folder);
        //        if (!Directory.Exists(Location))
        //            Directory.CreateDirectory(Location);
        //    }

        //    return Location;
        //}

        public static string CreateDWRPhotoDirectories(string JobCode, string EReportID, string JointNumber, string Field)
        {
            string Location = ""; //Constants.StartDirectory;
            string[] Folders = new string[] { "Photo Store", JobCode, EReportID, JointNumber, Field };

            foreach (string Folder in Folders)
            {
                Location += ("/" + Folder);
                if (!Directory.Exists(Location))
                    Directory.CreateDirectory(Location);
            }

            return Location;
        }

        public static string CreatePDFDirectories(string JobCode, string EReportID)
        {
            //string Location = AppConstant.StartDirectory;
            string Location = string.Empty;//= Environment.GetFolderPath(Environment.SpecialFolder.Personal); 
            string[] Folders = new string[] { "PDF Store", JobCode, EReportID };

            foreach (string Folder in Folders)
            {
                Location += ("/" + Folder);
                if (!Directory.Exists(Location))
                    Directory.CreateDirectory(Location);
            }

            return Location;
        }


        //completions 
        public static Boolean CompletionsValidateToken(string Token, string TimeStamp)
        {
            return CompletionGetBooleanWithTimeout(ApiUrls.GetToken(TimeStamp, Settings.CurrentDB), Token);
        }

        public static Boolean CompletionGetBooleanWithTimeout(string Call, string Token)
        {
            Boolean Result = false;
            try
            {
                Uri uri = new Uri(Settings.CompletionServer_Url + Call);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                request.Method = WebRequestMethods.Http.Get;
                request.Headers.Add("Token", Token);
                request.Timeout = 10000;
                request.Accept = "application/json;  charset=utf-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.Unauthorized)
                {
                    var streamReader = new StreamReader(response.GetResponseStream());
                    string StringResult = streamReader.ReadToEnd();

                    // Result =   JsonConvert.DeserializeObject<Boolean>(StringResult);
                    Result = StringResult.Contains("success");
                    response.Close();
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
                Result = false;
            }

            return Result;
        }

        public static string CompletionsCreateToken(string UserName, string Password, string TimeStamp)
        {
            return GenerateHashForString(AppConstant.SECRET_SHARED_TOKEN + UserName + Password + TimeStamp);
        }

        public static string CompletionWebServiceGet(string Call, string Token)
        {
        
            string JsonString = "";
            Uri uri = new Uri(Settings.CompletionServer_Url + Call);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;
            request.Headers.Add("Token", Token);
            request.Timeout = 300000;
            request.Accept = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            try//EC1-UN21-GM-23-401B4_1
            {
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    JsonString += streamReader.ReadToEnd();
                }
            }
            catch (Exception err)
            {
                string error = err.Message;
            }

            return JsonString;
        }
        public static string CompletionWebServicePost(string Call, string JSONString, string Token)
        {
            string Result = "";
            Uri uri = new Uri(Settings.CompletionServer_Url + Call);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Post;
            request.Timeout = 300000;
            request.Headers.Add("Token", Token);
            request.ContentType = "application/json; charset=utf-8";

            byte[] bytes = Encoding.UTF8.GetBytes(JSONString);

            using (var streamwriter = request.GetRequestStream())
            {
                streamwriter.Write(bytes, 0, bytes.Length);
            }
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    Result = streamReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                Result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }

            return Result;
        }
    }

    public class ComboboxItem
    {
        public string Text { get; set; }
        public string ValueText { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}

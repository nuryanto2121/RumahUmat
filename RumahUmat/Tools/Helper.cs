using RumahUmat.AES256Encryption;
using RumahUmat.Enums;
using RumahUmat.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumahUmat
{
    public class Helper
    {
        public static OutPut Error(Exception ex)
        {
            OutPut op = new OutPut();
            op.Message = ex.Message;
            op.Status = 500;
            op.Error = true;
            return op;
        }
        public static OutPut Error(string ex)
        {

            OutPut op = new OutPut();
            op.Message = ex;
            op.Status = 500;
            op.Error = true;
            return op;
        }
        public static string GetipAddress()
        {
            HttpContextAccessor dd = new HttpContextAccessor();
            return dd.HttpContext.Connection.RemoteIpAddress.ToString();
        }
        public static string GetUserId()
        {
            HttpContextAccessor dd = new HttpContextAccessor();
            var Headers = dd.HttpContext.Request.Headers;
            string Token = string.Empty;
            foreach (var key in Headers.Keys)
            {
                if (key.ToLower() == "token")
                {
                    Token = Headers[key];
                }
            }

            if (string.IsNullOrEmpty(Token))
            {
                Token = "Can't Get Account Token";
            }
            else
            {
                var Key = EncryptionLibrary.DecryptText(Token);
                string[] Parts = Key.Split(new string[] { ":~!@#" }, StringSplitOptions.None);

                Token = Parts[0];
            }
            return Token;
        }
        public static string ConnectionString(IConfiguration configuration)
        {
            //get
            //{
            //    IConfiguration configuration;
            //    var aa = configuration.GetValue<string>("DBInfo:ConnectionString").ToString();
            //    return aa;
            //}
            return configuration.GetValue<string>("DBInfo:ConnectionString");
        }

        public static string md5(string sPassword)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(sPassword);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();
        }


    }

    public class Functions
    {
        private string connectionString;
        public Functions(string ConnectionString)
        {
            connectionString = ConnectionString;
        }
        public void InsetErrorLog(string UserLog, string Descs)
        {
            using (IDbConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    DynamicParameters spParam = new DynamicParameters();
                    spParam.Add("@p_userlog", UserLog, dbType: DbType.String);
                    spParam.Add("@p_descs", Descs, dbType: DbType.String);
                    var affectedRows = conn.Query("fn_errolog_i", spParam, commandType: CommandType.StoredProcedure);
                    //op.Data = conn.Query<PosApi>("SELECT * FROM \"PosApi\"  LIMIT 10").ToList<PosApi>();
                }
                catch (Exception ex)
                {
                    //op = Helper.Error(ex);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        public DynamicParameters SpParameters(JObject obj, string fn_name)
        {
            DynamicParameters sp = new DynamicParameters();
            try
            {
                List<ParamFunction> ListParam = new List<ParamFunction>();
                ListParam = this.getListParamType(fn_name);

                foreach (JProperty x in obj.Properties())
                {
                    var valData = x.Value;
                    //long result;
                    int result;
                    DateTime temp;
                    ParamFunction ParamData = ListParam.Where(m => m.parameter_name.ToLower() == x.Name.ToLower()).FirstOrDefault();
                    if (ParamData == null)
                    {
                        new Exception("gak boleh null bos");
                    }
                    if (ParamData.data_type.ToLower() == "character varying" || ParamData.data_type.ToLower() == "character" || ParamData.data_type.ToLower() == "char" || ParamData.data_type.ToLower() == "text")
                    {
                        if (valData.ToString().ToLower() == "null")
                        {
                            sp.Add("@" + x.Name.ToString(), null, dbType: DbType.String);
                        }
                        else
                        {
                            sp.Add("@" + x.Name.ToString(), valData.ToString(), dbType: DbType.String);
                        }

                    }
                    else if (ParamData.data_type.ToLower() == "integer")
                    {
                        if (valData != null)
                        {
                            if (valData.ToString().ToLower() == "null")
                            {
                                sp.Add("@" + x.Name.ToString(), null, dbType: DbType.Int32);
                                //return;
                            }
                            else
                            {
                                sp.Add("@" + x.Name.ToString(), Convert.ToInt32(valData.ToString()), dbType: DbType.Int32);
                            }
                        }

                    }
                    else if (ParamData.data_type.ToLower() == "decimal")
                    {
                        if (valData.ToString().ToLower() == "null")
                        {
                            sp.Add("@" + x.Name.ToString(), null, dbType: DbType.Int32);
                            //return;
                        }
                        else
                        {
                            sp.Add("@" + x.Name.ToString(), Convert.ToDecimal(valData.ToString()), dbType: DbType.Int32);
                        }

                    }
                    else if (ParamData.data_type.ToLower() == "numeric")
                    {
                        if (valData.ToString().ToLower() == "null")
                        {
                            sp.Add("@" + x.Name.ToString(), null, dbType: DbType.Int32);
                            //return;
                        }
                        else
                        {
                            sp.Add("@" + x.Name.ToString(), Convert.ToInt32(valData.ToString()), dbType: DbType.Int32);
                        }

                    }
                    else if (ParamData.data_type.ToLower() == "bigint")
                    {
                        if (valData.ToString().ToLower() == "null")
                        {
                            sp.Add("@" + x.Name.ToString(), null, dbType: DbType.Int64);
                            //return;
                        }
                        else
                        {
                            sp.Add("@" + x.Name.ToString(), Convert.ToInt64(valData.ToString()), dbType: DbType.Int64);
                        }

                    }
                    else if (ParamData.data_type.ToLower() == "boolean")
                    {
                        if (valData.ToString().ToLower() == "null")
                        {
                            sp.Add("@" + x.Name.ToString(), null, dbType: DbType.Boolean);
                            //return;
                        }
                        else
                        {
                            sp.Add("@" + x.Name.ToString(), Convert.ToBoolean(valData), dbType: DbType.Boolean);
                        }

                    }
                    else if (ParamData.data_type.ToLower() == "json")
                    {
                        if (valData.ToString().ToLower() == "null")
                        {
                            sp.Add("@" + x.Name.ToString(), null, (DbType)NpgsqlDbType.Json);
                            //return;
                        }
                        else
                        {
                            sp.Add("@" + x.Name.ToString(), valData, (DbType)NpgsqlDbType.Json);
                        }

                    }
                    else if (ParamData.data_type.ToLower() == "datetime" || ParamData.data_type.ToLower() == "timestamp without time zone" || ParamData.data_type.ToLower() == "date" || ParamData.data_type.ToLower() == "timestamp")
                    {
                        if (valData.ToString().ToLower() == "null")
                        {
                            sp.Add("@" + x.Name.ToString(), null, dbType: DbType.DateTime);
                            //return;
                        }
                        else
                        {
                            if (ParamData.data_type.ToLower() == "date")
                            {
                                sp.Add("@" + x.Name.ToString(), DateTime.Parse(valData.ToString()), dbType: DbType.Date);
                            }
                            else
                            {
                                sp.Add("@" + x.Name.ToString(), DateTime.Parse(valData.ToString()), dbType: DbType.DateTime);
                            }

                        }

                    }
                    else if (ParamData.data_type.ToLower() == "xid")
                    {
                        if (valData.ToString().ToLower() == "null")
                        {
                            sp.Add("@" + x.Name.ToString(), null, dbType: DbType.DateTime);
                            //return;
                        }
                        else
                        {
                            sp.Add("@" + x.Name.ToString(), Convert.ToInt32(valData.ToString()), dbType: DbType.Int32);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return sp;
        }
        public List<ParamFunction> getListParamType(string fn_name)
        {
            List<ParamFunction> tt = new List<ParamFunction>();
            using (IDbConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    string[] bb = fn_name.Split('.');
                    fn_name = bb.Count() > 1 ? bb[1] : bb[0];
                    string Query = string.Format("select * from public.get_param_function('{0}');", fn_name);
                    conn.Open();
                    tt = conn.Query<ParamFunction>(Query).ToList<ParamFunction>();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return tt;
        }
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public string GenerateToken(AuthLogin dataLog, DateTime ExpireOn, string IpAddress)
        {
            string _result = string.Empty;
            try
            {
                string RandomNumber = string.Join(":~!@#", new string[] {
                    dataLog.Account,
                    EncryptionLibrary.KeyGenerator.GetUniqueKey(),
                    dataLog.IdMachine,
                    Convert.ToString(ExpireOn.Ticks),
                    dataLog.Psswd,
                    IpAddress
                });

                _result = EncryptionLibrary.EncryptText(RandomNumber);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return _result;
        }
        public object SelectScalar(SQL.Function.Aggregate function, string table, string column, string parameters = null)
        {
            object _result = null;
            parameters = !string.IsNullOrEmpty(parameters) ? "WHERE " + parameters : string.Empty;
            using (IDbConnection conn = new NpgsqlConnection(connectionString))
            {
                StringBuilder sbQuery = new StringBuilder();
                switch (function)
                {
                    case SQL.Function.Aggregate.Max:
                        sbQuery.AppendFormat("SELECT MAX({0}) FROM public.\"{1}\" with(nolock) {2}", table, column, parameters);
                        break;
                    case SQL.Function.Aggregate.Min:
                        sbQuery.AppendFormat("SELECT MIN({0}) FROM public.\"{1}\" with(nolock) {2}", table, column, parameters);
                        break;
                    case SQL.Function.Aggregate.Distinct:
                        sbQuery.AppendFormat("SELECT DISTINCT({0}) FROM public.\"{1}\" with(nolock) {2}", table, column, parameters);
                        break;
                    case SQL.Function.Aggregate.Count:
                        sbQuery.AppendFormat("SELECT COUNT({0}) FROM public.\"{1}\" with(nolock) {2}", table, column, parameters);
                        break;
                    case SQL.Function.Aggregate.Sum:
                        sbQuery.AppendFormat("SELECT SUM({0}) FROM public.\"{1}\" with(nolock) {2}", table, column, parameters);
                        break;
                    case SQL.Function.Aggregate.Avg:
                        sbQuery.AppendFormat("SELECT AVG({0}) FROM public.\"{1}\" with(nolock) {2}", table, column, parameters);
                        break;
                    default:
                        // do nothing 
                        break;
                }

                try
                {
                    conn.Open();
                    _result = conn.ExecuteScalar(sbQuery.ToString());
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return _result;
        }
        public string FormatWhere(string Parameters)
        {
            string _result = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(Parameters)) return _result;

                Parameters = Parameters.Replace("WHERE", "").Replace("Where", "");
                string[] sWheres = Parameters.Split("and");
                for (int x = 0; x < sWheres.Length; x++)
                {
                    string[] sField = sWheres[x].Split("=");
                    string field = sField[0].ToString().Trim();
                    string value = sField[1].ToString().Trim();

                    _result += string.Format(" \"{0}\" = {1} AND ", field, value);
                }
                _result = !string.IsNullOrEmpty(_result) ? _result.Remove(_result.LastIndexOf("AND")) : _result;
                _result = "WHERE " + _result;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return _result;
        }
        public string sWhereLikeList(string sFrom,string sParam)
        {
            string _result = string.Empty;
            try
            {
                var dataField = this.GetFieldType(sFrom);
                dataField.ForEach(delegate (FieldList val)
                {
                    if(val.max_length !=null && val.max_length > 0)
                    {
                        _result += string.Format("lower(\"{0}\") LIKE '%{1}%' OR ", val.column_name, sParam.ToLower());
                    }
                    else
                    {
                        _result += string.Format("lower(\"{0}\"::varchar) LIKE '%{1}%' OR ", val.column_name, sParam.ToLower());
                    }
                    
                });
                _result = !string.IsNullOrEmpty(_result) ? "( "+_result.Remove(_result.LastIndexOf("OR")) +" )": _result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _result;
        }
        public List<FieldList> GetFieldType(string sFrom)
        {
            List<FieldList> tt = new List<FieldList>();
            using (IDbConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    string[] sp = sFrom.Split('.');
                    string FromSource = sp.Count() > 1 ? sp[1].ToString() : sp[0].ToString();
                    StringBuilder sQuery = new StringBuilder();
                    sQuery.AppendFormat("select c.column_name,c.ordinal_position, ");
                    sQuery.AppendFormat("c.data_type, c.character_maximum_length as max_length, ");
                    sQuery.AppendFormat("c.numeric_precision as precision, c.numeric_scale as scale ");
                    sQuery.AppendFormat("from information_schema.tables t ");
                    sQuery.AppendFormat("inner ");
                    sQuery.AppendFormat("join information_schema.columns c on c.table_name = t.table_name ");
                    sQuery.AppendFormat("     and c.table_schema = t.table_schema ");
                    sQuery.AppendFormat("where  t.table_schema not in ('information_schema', 'pg_catalog') ");
                    sQuery.AppendFormat("AND lower(t.table_name) = '{0}' ", FromSource.ToLower());
                    sQuery.AppendFormat("order by c.ordinal_position, t.table_schema; ");
                    conn.Open();
                    tt = conn.Query<FieldList>(sQuery.ToString()).ToList();
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return tt;
        }
    }
}

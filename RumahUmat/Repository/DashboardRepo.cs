using RumahUmat.Models;
using Dapper;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RumahUmat.Repository
{
    public class DashboardRepo
    {
        private string connectionString;
        private Functions fn;
        public DashboardRepo(string ConnectionString)
        {
            connectionString = ConnectionString;
            fn = new Functions(ConnectionString);
        }
        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }
        public OutPut GetRecentSearch(JObject Model)
        {
            OutPut op = new OutPut();
            DynamicParameters spParam = new DynamicParameters();
            using (IDbConnection conn = Connection)
            {
                try
                {
                    //validasi
                    if (Model["_a"] == null)
                    {
                        op.Message = "Param _a can't be null";
                        op.Error = true;
                        return op;
                    }
                    string ipAddress = Helper.GetipAddress();
                    //var remoteIpAddress = Req.HttpContext.Connection.RemoteIpAddress;
                    conn.Open();
                    string SpName = "fn_recentsearch_s";
                    spParam.Add("@p_ipaddress", ipAddress, dbType: DbType.String);
                    spParam.Add("@p_account", Model["_a"].ToString(), dbType: DbType.String);
                    op.Data = conn.Query(SpName, spParam, commandType: CommandType.StoredProcedure);
                    //op.Message = "Data Berhasil di Simpan";
                }
                catch (Exception ex)
                {
                    fn.InsetErrorLog("Error ", ex.Message + ex.StackTrace);
                    op.Error = true;
                    op.Message = ex.Message;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }

            return op;

        }       
        public OutPut SaveRecentSearch(JObject Model)
        {
            OutPut op = new OutPut();
            DynamicParameters spParam = new DynamicParameters();
            using (IDbConnection conn = Connection)
            {
                try
                {
                    //validasi
                    if (Model["s"]==null)
                    {
                        op.Message = "Param s can't be null";
                        op.Error = true;
                        return op;
                    }
                    if (Model["_a"] == null)
                    {
                        op.Message = "Param _a can't be null";
                        op.Error = true;
                        return op;
                    }
                    string ipAddress = Helper.GetipAddress();
                    //var remoteIpAddress = Req.HttpContext.Connection.RemoteIpAddress;
                    conn.Open();
                    string SpName = "fn_recentsearch_i";
                    spParam.Add("@p_ipaddress", ipAddress, dbType: DbType.String);
                    spParam.Add("@p_search", Model["s"].ToString(), dbType: DbType.String);
                    spParam.Add("@p_account", Model["_a"].ToString(), dbType: DbType.String);
                    op.Data = conn.Query(SpName, spParam, commandType: CommandType.StoredProcedure);
                    op.Message = "Data Berhasil di Simpan";
                }
                catch (Exception ex)
                {
                    fn.InsetErrorLog("Error ", ex.Message + ex.StackTrace);
                    op.Error = true;
                    op.Message = ex.Message;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }

            return op;

        }
        public OutPut DeleteRecentSearchItem(JObject Model)
        {
            OutPut op = new OutPut();
            DynamicParameters spParam = new DynamicParameters();
            using (IDbConnection conn = Connection)
            {
                try
                {
                    //validasi
                    if (Model["s"] == null)
                    {
                        op.Message = "Param s can't be null";
                        op.Error = true;
                        return op;
                    }
                    if (Model["_a"] == null)
                    {
                        op.Message = "Param _a can't be null";
                        op.Error = true;
                        return op;
                    }
                    string ipAddress = Helper.GetipAddress();
                    //var remoteIpAddress = Req.HttpContext.Connection.RemoteIpAddress;
                    conn.Open();
                    string SpName = "fn_recentsearch_d_item";
                    spParam.Add("@p_ipaddress", ipAddress, dbType: DbType.String);
                    spParam.Add("@p_search", Model["s"].ToString(), dbType: DbType.String);
                    spParam.Add("@p_account", Model["_a"].ToString(), dbType: DbType.String);
                    op.Data = conn.Query(SpName, spParam, commandType: CommandType.StoredProcedure);
                    op.Message = "Data Berhasil di Delete";
                }
                catch (Exception ex)
                {
                    fn.InsetErrorLog("Error ", ex.Message + ex.StackTrace);
                    op.Error = true;
                    op.Message = ex.Message;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }

            return op;

        }
        public OutPut DeleteRecentSearchALL(JObject Model)
        {
            OutPut op = new OutPut();
            DynamicParameters spParam = new DynamicParameters();
            using (IDbConnection conn = Connection)
            {
                try
                {
                    //validasi
                 
                    if (Model["_a"] == null)
                    {
                        op.Message = "Param _a can't be null";
                        op.Error = true;
                        return op;
                    }
                    string ipAddress = Helper.GetipAddress();
                    //var remoteIpAddress = Req.HttpContext.Connection.RemoteIpAddress;
                    conn.Open();
                    string SpName = "fn_recentsearch_d";
                    spParam.Add("@p_ipaddress", ipAddress, dbType: DbType.String);
                    spParam.Add("@p_account", Model["_a"].ToString(), dbType: DbType.String);
                    op.Data = conn.Query(SpName, spParam, commandType: CommandType.StoredProcedure);
                    op.Message = "Data Berhasil di Delete";
                }
                catch (Exception ex)
                {
                    fn.InsetErrorLog("Error ", ex.Message + ex.StackTrace);
                    op.Error = true;
                    op.Message = ex.Message;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }

            return op;

        }

        public OutPut SaveRecentView(JObject Model)
        {
            OutPut op = new OutPut();
            DynamicParameters spParam = new DynamicParameters();
            using (IDbConnection conn = Connection)
            {
                try
                {
                    //validasi
                    if (Model["i"] == null)
                    {
                        op.Message = "Param i can't be null";
                        op.Error = true;
                        return op;
                    }
                    if (Model["_a"] == null)
                    {
                        op.Message = "Param _a can't be null";
                        op.Error = true;
                        return op;
                    }
                    string ipAddress = Helper.GetipAddress();
                    //var remoteIpAddress = Req.HttpContext.Connection.RemoteIpAddress;
                    conn.Open();
                    string SpName = "fn_recentview_i";
                    spParam.Add("@p_ipaddress", ipAddress, dbType: DbType.String);
                    spParam.Add("@p_campaignid", Convert.ToInt32(Model["i"]), dbType: DbType.Int32);
                    spParam.Add("@p_account", Model["_a"].ToString(), dbType: DbType.String);
                    op.Data = conn.Query(SpName, spParam, commandType: CommandType.StoredProcedure);
                    op.Message = "Data Berhasil di Simpan";
                }
                catch (Exception ex)
                {
                    fn.InsetErrorLog("Error ", ex.Message + ex.StackTrace);
                    op.Error = true;
                    op.Message = ex.Message;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }

            return op;

        }

    }
}

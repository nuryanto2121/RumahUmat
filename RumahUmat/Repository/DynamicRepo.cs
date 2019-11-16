using RumahUmat.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumahUmat.Repository
{
    public class DynamicRepo
    {
        private string connectionString;
        private OptionDBRepo optRepo;
        public string MvSpName = string.Empty;
        Functions fn;
        //public DynamicRepo(IConfiguration configuration)
        public DynamicRepo(string configuration)
        {
            connectionString = configuration;//.GetValue<string>("DBInfo:ConnectionString");
            optRepo = new OptionDBRepo(configuration);
            fn = new Functions(configuration);
        }
        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }

        public OutPut ExecuteMethod(JObject Model, Enums.Enum.Method method)
        {
            OutPut op = new OutPut();
            DynamicParameters spParam = new DynamicParameters();
            using (IDbConnection conn = Connection)
            {
                try
                {
                    //validasi
                    if (Model.Count == 0)
                    {
                        op.Error = true;
                        op.Message = "Data Not Valid";
                        return op;
                    }
                    if (Model["ApiName"] == null)
                    {
                        op.Error = true;
                        op.Message = "No Parameter ApiName!";
                        return op;
                    }
                    string ApiName = Model["ApiName"].ToString();
                    MvSpName = (from opt in optRepo.GetList(method.ToString(), ApiName)
                                select opt.Functions).FirstOrDefault();

                    if (string.IsNullOrEmpty(MvSpName))
                    {
                        op.Error = true;
                        op.Message = "Please Contact Your Administrator (Table setting null or Empty)";
                        return op;
                    }
                    Model.Remove("ApiName");
                    conn.Open();

                    if (method == Enums.Enum.Method.GetDataBy)
                    {
                        string sSortField = Model["SortField"] == null || string.IsNullOrEmpty(Model["SortField"].ToString()) ? string.Empty : "ORDER BY " + Model["SortField"].ToString();
                        string sParameter = Model["InitialWhere"] == null || string.IsNullOrEmpty(Model["InitialWhere"].ToString()) ? string.Empty : "WHERE " + Model["InitialWhere"].ToString();
                        var affectedRows = this.Query(MvSpName, sParameter, sSortField);
                        op.Data = affectedRows;

                    }
                    else
                    {
                        spParam = fn.SpParameters(Model, MvSpName);
                        var affectedRows = conn.Query(MvSpName, spParam, commandType: CommandType.StoredProcedure);
                        op.Data = affectedRows;
                    }


                    //op.Message = "";
                    if (method == Enums.Enum.Method.Delete || method == Enums.Enum.Method.Update)
                    {
                        op.Message = "Data Has Been " + method.ToString() + "d Successfully";
                    }
                    else if (method == Enums.Enum.Method.Insert)
                    {
                        op.Message = "Data Has Been " + method.ToString() + "ed Successfully";
                    }



                }
                catch (Exception ex)
                {
                    fn.InsetErrorLog("Error " + method.ToString(), ex.Message + ex.StackTrace);
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
        public OutPutList<dynamic> GetList(JObject Model, Enums.Enum.Method method)
        {
            var op = new OutPutList<dynamic>();
            DynamicParameters spParam = new DynamicParameters();
            using (IDbConnection conn = Connection)
            {
                try
                {
                    //validasi
                    if (Model.Count == 0)
                    {
                        op.Error = true;
                        op.Message = "Data Not Valid";
                        return op;
                    }
                    if (Model["ApiName"] == null)
                    {
                        op.Error = true;
                        op.Message = "No Parameter ApiName!";
                        return op;
                    }
                    string ApiName = Model["ApiName"].ToString();
                    MvSpName = (from opt in optRepo.GetList(method.ToString(), ApiName)
                                select opt.Functions).FirstOrDefault();

                    if (string.IsNullOrEmpty(MvSpName))
                    {
                        op.Error = true;
                        op.Message = "Please Contact Your Administrator (Table setting null or Empty)";
                        return op;
                    }
                    //Model.Remove("ApiName");
                    conn.Open();
                    int iStart = Convert.ToInt32(Model["CurrentPage"]);
                    int iPerPage = Convert.ToInt32(Model["PerPage"]);
                    string sSortField = string.IsNullOrEmpty(Model["SortField"].ToString()) ? string.Empty : "ORDER BY " + Model["SortField"].ToString();
                    //string sSearchField = Model["SearchField"].ToString();
                    string sParamWhere = Model["ParamWhere"].ToString();
                    string sParameter = string.Empty;
                    //InitialWhere
                    //ParamWhere
                    if (!string.IsNullOrEmpty(Model["InitialWhere"].ToString()))
                    {
                        sParameter += "WHERE " + Model["InitialWhere"].ToString();
                    }
                    if (!string.IsNullOrEmpty(sParamWhere))
                    {
                        string sWhereLike = fn.sWhereLikeList(MvSpName, sParamWhere);
                        sParameter += !string.IsNullOrEmpty(sWhereLike) ? " AND " + sWhereLike : sParameter;
                        //if (!string.IsNullOrEmpty(sSearchField))
                        //{
                        //    string[] sField = sSearchField.Split(",");
                        //    string sParam = string.Empty;
                        //    foreach (var field in sField)
                        //    {
                        //        sParam += string.Format("lower({0}) LIKE '%{1}%' OR ", field, sParamWhere.ToLower());
                        //    }
                        //    sParam = !string.IsNullOrEmpty(sParam) ? sParam.Remove(sParam.LastIndexOf("OR")) : sParam;
                        //    sParameter = string.IsNullOrEmpty(sParameter) ? "WHERE " + sParam : " AND " + sParam;
                        //}
                        //else
                        //{
                        //    op.Error = true;
                        //    op.Message = "Parameter SearchField can't be null";
                        //    return op;
                        //}
                    }
                    //var affectedRows = conn.Query(MvSpName, spParam, commandType: CommandType.StoredProcedure);
                    op.Data = this.QueryList(MvSpName, iStart, iPerPage, sSortField, sParameter);
                    op.Total = Convert.ToInt32(this.CountList(MvSpName, sParameter));
                    op.Current_Page = iStart;
                    op.Last_Page = Convert.ToInt32(Math.Ceiling((Convert.ToDecimal(op.Total) / iPerPage)));





                }
                catch (Exception ex)
                {
                    fn.InsetErrorLog("Error " + method.ToString(), ex.Message + ex.StackTrace);
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
        public List<dynamic> QueryList(string sTable, int iStart, int iPageSize, string sSortField, string sParameter)
        {
            List<dynamic> op = null;
            using (IDbConnection conn = Connection)
            {
                try
                {
                    int iOffset = (iStart * iPageSize) - iPageSize;
                    sParameter = fn.FormatWhere(sParameter);
                    StringBuilder sQuery = new StringBuilder();
                    sQuery.AppendFormat("SELECT * FROM {0} ", sTable);
                    sQuery.AppendFormat(" {0} ", sParameter);
                    sQuery.AppendFormat(" {0} ", sSortField);
                    sQuery.AppendFormat(" LIMIT {0} ", iPageSize);
                    sQuery.AppendFormat(" OFFSET {0} ", iOffset);
                    conn.Open();
                    op = conn.Query<dynamic>(sQuery.ToString()).ToList();
                }
                catch (Exception ex)
                {
                    //op = Helper.Error(ex);
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }

            return op;
        }
        public List<dynamic> Query(string sTable, string sParameter, string sSortField)
        {
            List<dynamic> op = null;
            using (IDbConnection conn = Connection)
            {
                try
                {
                    sParameter = fn.FormatWhere(sParameter);
                    StringBuilder sQuery = new StringBuilder();
                    sQuery.AppendFormat("SELECT * FROM {0} ", sTable);
                    sQuery.AppendFormat(" {0} ", sParameter);
                    sQuery.AppendFormat(" {0} ", sSortField);
                    conn.Open();
                    op = conn.Query<dynamic>(sQuery.ToString()).ToList();
                }
                catch (Exception ex)
                {
                    //op = Helper.Error(ex);
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }

            return op;
        }

        public object CountList(string sTable, string sParameter)
        {
            object op = null;
            using (IDbConnection conn = Connection)
            {
                try
                {
                    sParameter = fn.FormatWhere(sParameter);
                    StringBuilder sQuery = new StringBuilder();
                    sQuery.AppendFormat("SELECT COUNT(*) FROM {0} {1}", sTable, sParameter);
                    conn.Open();
                    op = conn.ExecuteScalar(sQuery.ToString());
                }
                catch (Exception ex)
                {
                    //op = Helper.Error(ex);
                    throw ex;
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

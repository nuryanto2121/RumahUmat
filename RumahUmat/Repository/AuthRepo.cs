using RumahUmat.AES256Encryption;
using RumahUmat.Interface;
using RumahUmat.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
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
    public class AuthRepo
    {
        private string connectionString;
        Functions fn;

        public AuthRepo(IConfiguration configuration)
        {
            //config = configuration;
            connectionString = Helper.ConnectionString(configuration);
            fn = new Functions(connectionString);
        }
        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }

        public List<Donatur> getDataAuth(AuthLogin dataLog)
        {
            List<Donatur> don = new List<Donatur>();
            using (IDbConnection conn = Connection)
            {
                try
                {
                    DynamicParameters spParam = new DynamicParameters();
                    conn.Open();
                    string SpName = "_get_auth";
                    spParam.Add("@p_no_hp", dataLog.Account, dbType: DbType.String);
                    spParam.Add("@p_passwd", dataLog.Psswd, dbType: DbType.String);
                    //var sss = conn.Query(SpName,SpName,commandType:CommandType.StoredProcedure);
                    don = conn.Query<Donatur>(SpName, spParam, commandType: CommandType.StoredProcedure).ToList();
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

            return don;
        }
        public object SaveSignUp(SignUpModel dataLog)
        {
            object data = null;
            using (IDbConnection conn = Connection)
            {
                try
                {
                    DynamicParameters spParam = new DynamicParameters();
                    conn.Open();
                    string SpName = "_sign_up";
                    spParam.Add("@p_nama", dataLog._N, dbType: DbType.String);
                    spParam.Add("@p_no_hp", dataLog._a, dbType: DbType.String);                                        
                    spParam.Add("@p_passwrd", dataLog._p, dbType: DbType.String);
                    spParam.Add("@p_user_input", dataLog._D, dbType: DbType.String);
                    data = conn.Query(SpName, spParam, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    fn.InsetErrorLog("Error Login", ex.Message + " " + ex.StackTrace);
                    data = null;
                    throw (ex);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return data;
        }
        public List<Donatur> GetDataUserBy(string sParameter)
        {
            List<Donatur> uslog = new List<Donatur>();
            using (IDbConnection conn = Connection)
            {
                try
                {
                    //string sQuery = "SELECT * FROM public.\"UserLog\" WHERE \"UserLog\" = @UserLog AND \"Token\" = @Token;";
                    StringBuilder sQuery = new StringBuilder();
                    sQuery.AppendFormat("SELECT * FROM public.\"Nasabah\" ");
                    sQuery.AppendFormat("WHERE {0}",sParameter);
                    conn.Open();
                    uslog = conn.Query<Donatur>(sQuery.ToString()).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }

            return uslog;
        }
    }
}

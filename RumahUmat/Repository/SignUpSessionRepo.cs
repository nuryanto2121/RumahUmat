using RumahUmat.Models;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RumahUmat.Repository
{
    public class SignUpSessionRepo : IRepository<SignUpSession, int>
    {
        private string connectionString;
        private Functions fn;
        public SignUpSessionRepo(string ConnectionString)
        {
            connectionString = ConnectionString;
        }
        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }
        public bool Delete(int key)
        {
            throw new NotImplementedException();
        }

        public SignUpSession GetById(int key)
        {
            throw new NotImplementedException();
        }
        public SignUpSession GetDataSignUpSessionBy(string Account, string UniqueCd, bool StatusActive = true)
        {
            SignUpSession uslog = new SignUpSession();
            using (IDbConnection conn = Connection)
            {
                try
                {

                    string sQuery = "SELECT * FROM public.\"SignUpSession\" WHERE \"StatusActive\" = @StatusActive AND \"UniqueCd\" = @UniqueCd AND \"Account\" = @Account;";
                    conn.Open();
                    uslog = conn.Query<SignUpSession>(sQuery, new { StatusActive = StatusActive, UniqueCd = UniqueCd, Account = Account }).FirstOrDefault();
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
        public List<SignUpSession> GetList()
        {
            throw new NotImplementedException();
        }

        public List<SignUpSession> GetList(int pageSize, int currentPage, string sortName, string sortOrder)
        {
            throw new NotImplementedException();
        }

        public bool Save(SignUpSession domain)
        {
            throw new NotImplementedException();
        }

        public bool Update(SignUpSession domain)
        {
            throw new NotImplementedException();
        }
        public object UpdateStatusSignUpSession(string Account, string UniqueCd)
        {
            object data = null;
            using (IDbConnection conn = Connection)
            {
                try
                {
                    DynamicParameters spParam = new DynamicParameters();
                    conn.Open();
                    string SpName = "fn_signupsession_u";
                    spParam.Add("@p_account", Account, dbType: DbType.String);
                    spParam.Add("@p_uniquecd", UniqueCd, dbType: DbType.String);
                    data = conn.Query(SpName, spParam, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw (ex);
                    fn.InsetErrorLog("Error Login", ex.Message + " " + ex.StackTrace);
                    //data = null;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return data;
        }
        public object SaveSignUpSession(SignUpModel dataLog, string uniquecd, DateTime ExpireOn)
        {
            object data = null;
            using (IDbConnection conn = Connection)
            {
                try
                {
                    DynamicParameters spParam = new DynamicParameters();
                    conn.Open();
                    string SpName = "fn_signupsession_i";
                    spParam.Add("@p_signupwith", dataLog._Suw, dbType: DbType.String);
                    spParam.Add("@p_account", dataLog._a, dbType: DbType.String);
                    spParam.Add("@p_name", dataLog._N, dbType: DbType.String);
                    spParam.Add("@p_device", dataLog._D, dbType: DbType.String);
                    spParam.Add("@p_uniquecd", uniquecd, dbType: DbType.String);
                    spParam.Add("@p_userinput", "SYSTEM", dbType: DbType.String);
                    spParam.Add("@p_expireon", ExpireOn, dbType: DbType.DateTime);
                    data = conn.Query(SpName, spParam, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw (ex);
                    fn.InsetErrorLog("Error Login", ex.Message + " " + ex.StackTrace);
                    //data = null;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            return data;
        }

    }
}

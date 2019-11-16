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
    public class UserLogsRepo : IRepository<UserLogs, int>
    {
        private string connectionString;
        private Functions fn;
        public UserLogsRepo(string ConnectionString)
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
        public bool Delete(int key)
        {
            throw new NotImplementedException();
        }

        public UserLogs GetById(int key)
        {
            throw new NotImplementedException();
        }

        public UserLogs GetDataBy(string UserLog,string Token)
        {
            UserLogs uslog = new UserLogs();
            using (IDbConnection conn = Connection)
            {
                try
                {
                    string sQuery = "SELECT * FROM public.\"UserLog\" WHERE \"UserLog\" = @UserLog AND \"Token\" = @Token;";                    
                    conn.Open();
                    uslog = conn.Query<UserLogs>(sQuery, new { UserLog = UserLog, Token = Token }).FirstOrDefault();
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

        public List<UserLogs> GetList()
        {
            throw new NotImplementedException();
        }
       
        public List<UserLogs> GetList(int pageSize, int currentPage, string sortName, string sortOrder)
        {
            throw new NotImplementedException();
        }

        public bool Save(UserLogs domain)
        {
            bool op = true;
            using (IDbConnection conn = Connection)
            {
                try
                {
                    string sQuery = "INSERT INTO public.\"UserLog\"(\"UserLog\",\"Token\",\"ExpireOn\",\"IpAddress\",\"MachineName\",\"Device\",\"UserInput\",\"UserEdit\") ";
                    sQuery += " VALUES(@UserLog, @Token, @ExpireOn, @IpAddress, @MachineName, @Device, @UserInput, @UserEdit); ";
                    conn.Open();
                    conn.Execute(sQuery, domain);
                }
                catch (Exception ex)
                {
                    //op = Helper.Error(ex);
                    throw ex;
                    //fn.InsetErrorLog("Error Save UserLogs", ex.Message + " " + ex.StackTrace);
                    op = false;

                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }

            return op;
        }

        public bool Update(UserLogs domain)
        {
            throw new NotImplementedException();
        }
        
       

    }
}

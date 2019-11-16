using RumahUmat;
using RumahUmat.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RumahUmat.Repository
{
    public class OptionDBRepo : IRepository<OptionDB,int>
    {
        private string connectionString;
        public OptionDBRepo(string Configuration)
        {
            connectionString = Configuration;
        }
        public OutPut Add(OptionDB item)
        {
            throw new NotImplementedException();
        }
        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }

        public bool Save(OptionDB domain)
        {
            throw new NotImplementedException();
        }

        public bool Update(OptionDB domain)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int key)
        {
            throw new NotImplementedException();
        }

        public List<OptionDB> GetList()
        {
            throw new NotImplementedException();
        }
        public List<OptionDB> GetList(string Method, string ApiName)
        {
            List<OptionDB> op = new List<OptionDB>();
            using (IDbConnection conn = Connection)
            {
                try
                {
                    conn.Open();
                    op = conn.Query<OptionDB>("SELECT * FROM public.\"OptionDb\" where \"ApiName\" = @apiName AND \"Method\" = @method", new { apiName = ApiName, method = Method }).ToList();
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
            return op;
        }
        public List<OptionDB> GetList(int pageSize, int currentPage, string sortName, string sortOrder)
        {
            throw new NotImplementedException();
        }

        public OptionDB GetById(int key)
        {
            OptionDB op = new OptionDB();
            using (IDbConnection conn = Connection)
            {
                try
                {
                    conn.Open();
                    op = conn.Query<OptionDB>("SELECT * FROM public.\"OptionDb\" where OptionDBId = @ID",new { ID = key}).FirstOrDefault();
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

            return op;
        }

        public OptionDB GetById(string Method,string ApiName)
        {
            OptionDB op = new OptionDB();
            using (IDbConnection conn = Connection)
            {
                try
                {
                    conn.Open();
                    op = conn.Query<OptionDB>("SELECT * FROM public.\"OptionDb\" where ApiName = @apiName AND Method = @method", new { apiName = ApiName, method = Method }).FirstOrDefault();
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

            return op;
        }



    }
}

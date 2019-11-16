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
    public class PosApiRepo : IRepositoryController<PosApi>
    {
        private string connectionString;
        public PosApiRepo(string configuration)
        {
            connectionString = configuration;
        }

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }

        public OutPut Add(PosApi item)
        {
            throw new NotImplementedException();
        }

        public OutPut GetDataBy(int id)
        {
            OutPut op = new OutPut();
            using (IDbConnection conn = Connection)
            {
                try
                {
                    conn.Open();
                    op.Data = conn.Query<PosApi>("SELECT * FROM \"PosApi\"  LIMIT 10").ToList<PosApi>();
                }
                catch (Exception ex)
                {
                    op = Helper.Error(ex);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }

            return op;
        }

        public OutPut GetDataList()
        {
            OutPut op = new OutPut();
            using (IDbConnection conn = Connection)
            {
                try
                {
                    conn.Open();
                    op.Data = conn.Query<PosApi>("SELECT * FROM \"option_db\"  LIMIT 10").ToList<PosApi>();
                }
                catch (Exception ex)
                {
                    op = Helper.Error(ex);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }

            return op;
        }

        public OutPut Remove(int id)
        {
            throw new NotImplementedException();
        }

        public OutPut Update(PosApi item)
        {
            throw new NotImplementedException();
        }
        //public IEnumerable<PosApi> FindAll()
        //{
        //    //throw new NotImplementedException();
        //    using (IDbConnection dbConnection = Connection)
        //    {

        //        try
        //        {
        //            dbConnection.Open();
        //            return dbConnection.Query<PosApi>("SELECT * FROM PosApi");
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }

        //}

    }
}

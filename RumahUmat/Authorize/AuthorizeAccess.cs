using RumahUmat.AES256Encryption;
using RumahUmat.Models;
using RumahUmat.Repository;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using RumahUmat;
using Dapper;

namespace RumahUmat.Authorize
{
    public class AuthorizeAccess
    {
        private string connectionString;
        private string _Message;
        private UserLogsRepo _userlogsRepo;
        //public AuthorizeAccess(IConfiguration configuration)
        public AuthorizeAccess(string ConnectionString)
        {
            connectionString =  ConnectionString;//Helper.ConnectionString(configuration);//
            _userlogsRepo = new UserLogsRepo(connectionString);
        }
        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }
        public OutPut getAccessAPI(AuthorizationFilterContext context)
        {
            OutPut op = new OutPut();
            try
            {
                string IpAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
                var Headers = context.HttpContext.Request.Headers;
                foreach (var key in Headers.Keys)
                {
                    if (key.ToLower() == "token")
                    {
                        if (!isAuthorize(Headers[key], IpAddress))
                        {
                            op.Error = true;
                            op.Message = _Message;
                        }
                        return op;
                    }
                    else
                    {
                        op.Error = true;
                        op.Message = "Invalid Token";
                        return op;
                    }
                }

            }
            catch (Exception ex)
            {
                op.Error = true;
                op.Message = ex.Message;
            }

            return op;
        }

        private bool isAuthorize(string Token, string IpAddress)
        {
            bool _result = true;
            try
            {
                var encodeString = Token;//values.First();//req.Request.Headers.GetValues("Token").First();
                if (string.IsNullOrEmpty(encodeString))
                {
                    _Message = "Invalid Token";
                    return false;
                }

                var Key = EncryptionLibrary.DecryptText(encodeString);
                string[] Parts = Key.Split(new string[] { ":~!@#" }, StringSplitOptions.None);
                if (Parts.Count() < 6)
                {
                    _Message = "Invalid Token";
                    return false;
                }
                var UserName = Parts[0];
                var RandomKey = Parts[1];
                var IdMachine = Parts[2];
                long tik = long.Parse(Parts[3]);
                DateTime ExpireOn = new DateTime(tik);
                var Psswd = Parts[4];
                var Ip = Parts[5];

                if (Ip != IpAddress)
                {
                    _Message = "Invalid Ip Address";
                    return false;
                }

                var Auth = new AuthLogin();
                Auth.Account = UserName;
                Auth.Psswd = Psswd;
                var don = this.getDataAuth(Auth);
                if (don.Count == 0)
                {
                    _Message = "Invalid User";
                    return false;
                }
                else
                {
                    UserLogs uslog = _userlogsRepo.GetDataBy(Auth.Account, Token);
                    if (DateTime.Now > uslog.ExpireOn)
                    {
                        _Message = "Your Session Has Expired";
                        return false;
                    }

                }


            }
            catch (Exception ex)
            {
                _result = false;
                _Message = ex.Message;
                //throw (ex);
            }
            return _result;
        }
        private List<Donatur> getDataAuth(AuthLogin dataLog)
        {
            List<Donatur> don = new List<Donatur>();
            using (IDbConnection conn = Connection)
            {
                try
                {
                    DynamicParameters spParam = new DynamicParameters();
                    conn.Open();
                    string SpName = "fn_auth";
                    spParam.Add("@p_email", dataLog.Account, dbType: DbType.String);
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
    }
}

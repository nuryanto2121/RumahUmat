using RumahUmat.AES256Encryption;
using RumahUmat.Interface;
using RumahUmat.Models;
using RumahUmat.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumahUmat.Services
{
    public class AuthService
    {
        private string connectionString;
        Functions fn;
        UserLogsRepo userlogRepo;
        SignUpSessionRepo SignUpSessionRepo;
        AuthRepo AuthRepo;
        IConfiguration config;
        private readonly IEmailSender _emailSender;
        public AuthService(IConfiguration configuration, IEmailSender emailSender)
        //public AuthService(IConfiguration configuration)
        {
            config = configuration;
            AuthRepo = new AuthRepo(configuration);
            connectionString = Helper.ConnectionString(configuration);
            fn = new Functions(connectionString);
            userlogRepo = new UserLogsRepo(connectionString);
            SignUpSessionRepo = new SignUpSessionRepo(connectionString);
            _emailSender = emailSender;
        }


        public OutPut Login(AuthLogin dataLogin, IHttpContextAccessor iHttpContextAccessor)
        {
            OutPut op = new OutPut();
            Dictionary<string, object> ObjOutput = new Dictionary<string, object>();
            List<Donatur> don = new List<Donatur>();
            UserLogs uslog = new UserLogs();

            try
            {
                var IpAddress = iHttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                int TokenExpire = Convert.ToInt32(config.GetValue<string>("appSetting:TokenExpire"));

                dataLogin.Psswd = Helper.md5(Helper.md5(dataLogin.Psswd)).ToLower();
                
                don = AuthRepo.getDataAuth(dataLogin);
                if (don.Count > 0)
                {
                    DateTime ExpireOn = DateTime.Now.AddMinutes(TokenExpire);
                    string Token = fn.GenerateToken(dataLogin, ExpireOn, IpAddress);

                    //save session 
                    uslog.UserLog = dataLogin.Account;
                    uslog.Token = Token;
                    uslog.ExpireOn = ExpireOn;
                    uslog.IpAddress = IpAddress;
                    uslog.Device = dataLogin.Device;
                    uslog.UserInput = don[0].Nama;
                    uslog.UserEdit = don[0].Nama;
                    userlogRepo.Save(uslog);
                    Dictionary<string, string> OutData = new Dictionary<string, string>();
                    OutData.Add(dataLogin.Device, dataLogin.Account);
                    ObjOutput.Add("Akun", OutData);
                    ObjOutput.Add("Token", Token);
                    ObjOutput.Add("Email", don[0].Email);
                    ObjOutput.Add("ExpireOn", ExpireOn);

                    op.Data = ObjOutput;
                }
                else
                {
                    op.Status = 401;
                    op.Error = true;
                    op.Message = "User name or password is incorrect.";
                    return op;
                }
                //validasi

            }
            catch (Exception ex)
            {

                op.Error = true;
                op.Message = ex.Message;
                fn.InsetErrorLog("Error Login", ex.Message + " " + ex.StackTrace);
            }

            return op;
        }

        /// <summary>
        /// Daftar Donatur
        /// </summary>
        /// <param name="dataSignUp"></param>
        /// <param name="PathHtml"></param>
        /// <returns></returns>
        public OutPut SignUpStep1(SignUpModel dataSignUp)
        {
            OutPut op = new OutPut();
            try
            {
                int exTimeCd = Convert.ToInt32(config.GetValue<string>("TimerUniqueCd"));

                if (string.IsNullOrEmpty(dataSignUp._a))
                {
                    throw new Exception("Data Account can't be null.");
                }

                if (dataSignUp.Psswd != dataSignUp._cp)
                {
                    throw new Exception("Password must be.");
                }
                //check sudah terdaftar belom
                string sParameter = string.Format(" \"NoHp\" = '{0}' ", dataSignUp._a);
                var DataLogin = AuthRepo.GetDataUserBy(sParameter);//Convert.ToInt32(fn.SelectScalar(Enums.SQL.Function.Aggregate.Count, "Donatur", "*", sParameter));
                if (DataLogin.Count > 0)
                {
                    throw new Exception("Account Sudah terdaftar");
                }

                var UniqueNo = EncryptionLibrary.KeyGenerator.GetUniqueKey(6);
                var expireCd = DateTime.Now.AddMilliseconds(exTimeCd);
                var dt = SignUpSessionRepo.SaveSignUpSession(dataSignUp, UniqueNo, expireCd);
                //string ParameterCd = string.Format("{0}^{1}^{2}^{3}", dataSignUp.Name, dataSignUp.Account, UniqueNo, dataSignUp.SignUpWith);
                //string Url = config.GetValue<string>("UrlWeb") + fn.Base64Encode(ParameterCd);
                //MailModel DataSendMail = new MailModel();
                //DataSendMail.To = dataSignUp._a;
                //DataSendMail.Subject = "Aktivasi Akun Anda";
                //DataSendMail.Body = _emailSender.BodyEmailSignUp(dataSignUp._N, UniqueNo, PathHtml);
                //op = _emailSender.SendEmailAsync(DataSendMail);

                //op.Message = "Please Cek Your Email.";

            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                op.Error = true;
                fn.InsetErrorLog("Error Login", ex.Message + " " + ex.StackTrace);
            }
            return op;
        }

        /// <summary>
        /// Validasi Daftar Donatur
        /// </summary>
        /// <param name="dataSignUp"></param>
        /// <returns></returns>
        public OutPut SignUpStep2(VerifyCode verify)
        {
            OutPut op = new OutPut();
            try
            {
                verify._a = fn.Base64Decode(verify._a);
                var dataSignUpSession = SignUpSessionRepo.GetDataSignUpSessionBy(verify._a, verify._uc);
                if (dataSignUpSession == null)
                {
                    throw new Exception("Invalid Verifikasi Code.");
                }
                op.Message = "data has been verified.";
                //op.Data = verify;


            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                op.Error = true;
                fn.InsetErrorLog("Error Login", ex.Message + " " + ex.StackTrace);
            }
            return op;
        }
        public OutPut SignUpStep3(SignUpModel dataSigup)
        {
            OutPut op = new OutPut();
            try
            {
                string sParameter = string.Format(" \"NoHp\" = '{0}' ", dataSigup._a);
                var DataLogin = AuthRepo.GetDataUserBy(sParameter);//Convert.ToInt32(fn.SelectScalar(Enums.SQL.Function.Aggregate.Count, "Donatur", "*", sParameter));
                if (DataLogin.Count > 0)
                {
                    throw new Exception("Account Sudah terdaftar");
                }

                if (dataSigup._p != dataSigup._cp)
                {
                    throw new Exception("Password and ConfirmPassword Must be same.");
                }
                //verify._a = fn.Base64Decode(verify._a);




                // Insert to signup
                dataSigup._p = Helper.md5(Helper.md5(dataSigup._p)).ToLower();
                var dtObjt = AuthRepo.SaveSignUp(dataSigup);

                op.Message = "Please Login";
            }
            catch (Exception ex)
            {
                op.Message = ex.Message;
                op.Error = true;
                fn.InsetErrorLog("Error Login", ex.Message + " " + ex.StackTrace);
            }
            return op;
        }

        public string DecodeParam(string sDecode)
        {
            string _result = string.Empty;

            try
            {
                _result = fn.Base64Decode(sDecode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _result;
        }
        public string EncodeParam(string sParam)
        {
            string _result = string.Empty;

            try
            {
                _result = fn.Base64Encode(sParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _result;
        }

    }
}

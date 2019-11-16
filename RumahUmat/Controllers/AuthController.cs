using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RumahUmat.Interface;
using RumahUmat.Models;
using RumahUmat.Repository;
using RumahUmat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace RumahUmat.Controllers
{

    public class AuthController : ControllerBase
    {
        private AuthService AuthService;
        private IHttpContextAccessor iHttpContextAccessor;
        private IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private IHostingEnvironment _env;
        public AuthController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender, IHostingEnvironment env)
        {//IConfiguration
            _configuration = configuration;
            //AuthService = new AuthService(configuration, emailSender);
            AuthService = new AuthService(configuration, emailSender);
            iHttpContextAccessor = httpContextAccessor;
            //_emailSender = emailSender;
            _env = env;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody]AuthLogin JObj)
        {
            var OutResult = new OutPut();

            OutResult = AuthService.Login(JObj, iHttpContextAccessor);
            
            return Ok(OutResult);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SignUp([FromBody]SignUpModel SignUp)
        {
            var OutResult = new OutPut();

            if (string.IsNullOrWhiteSpace(_env.WebRootPath))
            {
                _env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
            //var folderName = Path.Combine("FormatEmail");
            //var pathToSave = Path.Combine(_env.WebRootPath, folderName);

            //var fullPath = Path.Combine(pathToSave, "ConfirmSignUp.html");

            OutResult = AuthService.SignUpStep3(SignUp);
            //if (!OutResult.Error)
            //{
            //    Dictionary<string, string> ObjOutput = new Dictionary<string, string>();
            //    ObjOutput.Add("_a", AuthService.EncodeParam(SignUp._a));
            //    OutResult.Data = ObjOutput;
            //}
            

            return Ok(OutResult);
        }
        //[AllowAnonymous]
        //[HttpPost]
        //public IActionResult ValidasiSignUp([FromBody]VerifyCode verify)
        //{

        //    var OutResult = new OutPut();

        //    //OutResult = AuthService.SignUpStep3(verify);
        //    //if (OutResult.Error)
        //    //{
        //    //    OutResult.Status = 500;
        //    //    return StatusCode(500, OutResult);
        //    //}

        //    return Ok(OutResult);
        //}
        [AllowAnonymous]
        [HttpPost]
        public IActionResult VerifikasiCode([FromBody]VerifyCode verify)
        {

            var OutResult = new OutPut();

            OutResult = AuthService.SignUpStep2(verify);
            //if (OutResult.Error)
            //{
            //    OutResult.Status = 500;
            //    return StatusCode(500, OutResult);
            //}

            return Ok(OutResult);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult DecodeBase64(string sParam)
        {
            var OutResult = new OutPut();
            try
            {
                Dictionary<string, string> OutData = new Dictionary<string, string>();
                OutData.Add("Decode", AuthService.DecodeParam(sParam));
                OutResult.Data = OutData;

            }
            catch (Exception ex)
            {
                OutResult.Error = true;
                OutResult.Message = ex.Message ;
            }

            return Ok(OutResult);
        }
    }
}
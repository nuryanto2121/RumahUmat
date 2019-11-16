using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RumahUmat.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace RumahUmat.Controllers
{
    public class DocumentController : ControllerBase
    {
        private IConfiguration _configuration;
        private IHostingEnvironment _env;
        public DocumentController(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        [HttpPost]
        public async Task<OutPut> Post(HttpRequestMessage request)

        {
            //var rootPath = _configuration.GetValue<string>("appSetting:WebRootPath");
            var contentRoot = _configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            var uploads = Path.Combine(contentRoot, "uploads");
            var op = new OutPut();
            try
            {
                //var httpRequest = Request.Form;
                //var file = Request.Form.Files[0];
                foreach (var file in Request.Form.Files)
                {
                    var postedFile = Request.Form.Files;

                }
            }
            catch (Exception ex)
            {

            }

            return op;

        }
        [HttpGet]
        public IActionResult Get()
        {

            var po = new OutPut();
            po.Message = _env.WebRootPath;
            return Ok(po);
        }
        [HttpPost]
        //public IActionResult Upload()
        public async Task<JsonResult> Upload()
        {
            var op = new OutPut();
            //var ss = new JsonResult(op);
            //var taks = Request.Form.f
            try
            {
                Helper.GetipAddress();
                var aa = _env.WebRootFileProvider;
                if (string.IsNullOrWhiteSpace(_env.WebRootPath))
                {
                    _env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                }
                var dd = _env;
                //var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                string path2 = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\Resources\Images"}";
                //var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var pathToSave = Path.Combine(_env.WebRootPath, folderName);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                var Name =  Request.HttpContext.Request.Form["Name"];
                var ddd = Request.Form["Name"];
                Dictionary<string, string> data = new Dictionary<string, string>();
                foreach (var file in Request.Form.Files)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    data.Add(file.Name, fullPath);

                }
                var path = Path.Combine(_env.WebRootPath, "Resources");
                op.Data = data;
            }
            catch (Exception ex)
            {
                op.Status = 500;
                op.Message = ex.Message;
                //return StatusCode(500, op);
            }

            //return Ok(op);
            return new JsonResult(op);
        }
    }
}
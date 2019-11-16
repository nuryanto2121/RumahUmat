using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RumahUmat.Authorize;
using RumahUmat.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace RumahUmat.Controllers
{
    
    public class DynamicAPIController : ControllerBase
    {
        private readonly DynamicRepo DynamicRepo;
        private string _connectionString;
        public DynamicAPIController(IConfiguration configuration)
        {
            _connectionString = Helper.ConnectionString(configuration);
            //DynamicRepo = new DynamicRepo(configuration);
            DynamicRepo = new DynamicRepo(_connectionString);
        }

        //[APIAuthorizeAttribute]
        [HttpPost]
        public IActionResult GetById([FromBody] JObject Model)
        {

            var po = DynamicRepo.ExecuteMethod(Model, Enums.Enum.Method.GetDataBy);
            
            return Ok(po);
        }
        [HttpPost]
        public IActionResult Save([FromBody] JObject Model)
        {

            var po = DynamicRepo.ExecuteMethod(Model, Enums.Enum.Method.Insert);
            
            return Ok(po);
        }

        [HttpPost]
        public IActionResult Update([FromBody] JObject Model)
        {

            var po = DynamicRepo.ExecuteMethod(Model, Enums.Enum.Method.Update);
            
            return Ok(po);
        }

        [HttpPost]
        public IActionResult Delete([FromBody] JObject Model)
        {

            var po = DynamicRepo.ExecuteMethod(Model, Enums.Enum.Method.Delete);
            
            return Ok(po);
        }

        [HttpPost]
        public IActionResult GetList([FromBody] JObject Model)
        {

            var po = DynamicRepo.GetList(Model, Enums.Enum.Method.List);
            
            return Ok(po);
        }
    }
}
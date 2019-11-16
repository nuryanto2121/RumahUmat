using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RumahUmat.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace RumahUmat.Controllers
{
    public class DashboardController : ControllerBase
    {

        DashboardRepo DashRepo;
        private string _connectionString;
        public DashboardController(IConfiguration configuration)
        {
            _connectionString = Helper.ConnectionString(configuration);
            DashRepo = new DashboardRepo(_connectionString);
        }

        [HttpPost]
        public IActionResult SaveRS([FromBody] JObject Model)
        {

            //var po = DynamicRepo.ExecuteMethod(JObj, Enums.Enum.Method.GetDataBy);
            var po = DashRepo.SaveRecentSearch(Model);

            return Ok(po);
        }
        [HttpPost]
        public IActionResult GetRS([FromBody] JObject Model)
        {

            //var po = DynamicRepo.ExecuteMethod(JObj, Enums.Enum.Method.GetDataBy);
            var po = DashRepo.GetRecentSearch(Model);

            return Ok(po);
        }
        [HttpPost]
        public IActionResult DeleteRSI([FromBody] JObject Model)
        {

            //var po = DynamicRepo.ExecuteMethod(JObj, Enums.Enum.Method.GetDataBy);
            var po = DashRepo.DeleteRecentSearchItem(Model);

            return Ok(po);
        }

        [HttpPost]
        public IActionResult DeleteRSA([FromBody] JObject Model)
        {

            //var po = DynamicRepo.ExecuteMethod(JObj, Enums.Enum.Method.GetDataBy);
            var po = DashRepo.DeleteRecentSearchALL(Model);

            return Ok(po);
        }

        [HttpPost]
        public IActionResult SaveRV([FromBody] JObject Model)
        {

            //var po = DynamicRepo.ExecuteMethod(JObj, Enums.Enum.Method.GetDataBy);
            var po = DashRepo.SaveRecentView(Model);

            return Ok(po);
        }
    }
}
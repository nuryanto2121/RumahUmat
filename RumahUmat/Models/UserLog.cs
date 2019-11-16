using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumahUmat.Models
{
    public class UserLogs : BaseEntity
    {
        public int UserLogId { get; set; }
        public string UserLog { get; set; }
        public string Token { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? ExpireOn { get; set; }
        public string IpAddress { get; set; }
        public string MachineName { get; set; }
        public string Device { get; set; }
        public string UserInput { get; set; }
        public string UserEdit { get; set; }
        public DateTime TimeInput { get; set; }
        public DateTime TimeEdit { get; set; }

    }
    public class SignUpSession : BaseEntity
    {
        public int SignUpSessionId { get; set; }
        public string SignUpWith { get; set; }
        public string Account { get; set; }
        public string Device { get; set; }
        public string Name { get; set; }
        public string UniqueCd { get; set; }
        public bool StatusActive { get; set; }
        public string UserInput { get; set; }
        public DateTime TimeInput { get; set; }
        public DateTime ExpireOn { get; set; }

    }


}

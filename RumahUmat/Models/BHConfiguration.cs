using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumahUmat.Models
{
    public class BHConfiguration
    {
        //public string ApplicationName { get; set; }
        //public string ApplicationBasePath { get; set; } = "/";
        //public int PostPageSize { get; set; } = 10000;
        //public int HomePagePostCount { get; set; } = 30;
        public int TokenExpire { get; set; }
        public string ConnectionString { get; set; }
        public EmailConfiguration Email { get; set; } = new EmailConfiguration();
    }
    public class EmailConfiguration
    {
        public string MailServer { get; set; }
        public string MailServerUsername { get; set; }
        public string MailServerPassword { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string AdminSenderEmail { get; set; }
    }
}

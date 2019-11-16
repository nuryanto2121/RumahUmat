using RumahUmat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumahUmat.Interface
{
    public interface IEmailSender
    {
        OutPut SendEmailAsync(MailModel eMail);
        string BodyEmailSignUp(string Name, string UniqueNo, string PathHtml);
    }
}

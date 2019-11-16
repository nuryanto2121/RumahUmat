using RumahUmat.Interface;
using RumahUmat.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace RumahUmat.Repository
{
    public class AuthMessageSender : IEmailSender
    {
        public EmailSettings _emailSettings { get; }
        public AuthMessageSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public OutPut SendEmailAsync(MailModel eMail)
        {
            //Execute(eMail).Wait();
            //return Task.FromResult(0);
            return send(eMail);
        }
      
        public OutPut send(MailModel mail)
        {
            OutPut _result = new OutPut();

            try
            {
                //var message = new MailMessage();
                MailMessage message = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.UsernameEmail, "Beri Hati")
                };
                message.To.Add(new MailAddress(mail.To));

                message.Subject = mail.Subject;
                message.Body = mail.Body;
                message.IsBodyHtml = true;
                //        message.Priority = MailPriority.High;
                //message.Attachments.Add(new Attachment(Server.MapPath("~/myimage.jpg")));
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                using (var smtp = new SmtpClient(_emailSettings.PrimaryDomain,_emailSettings.PrimaryPort))
                {
                    //smtp.SendMailAsync(message);
                    //smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                }

                _result.Message = "Pesan Terkirim";
            }
            catch (Exception ex)
            {
                throw ex;
                //_result.status = false;
                //_result.Message = ex.Message;
            }

            return _result;
        }
        public string BodyEmailSignUp(string Name,string UniqCd, string PathHtml)
        {
            string body = string.Empty;
            
            try
            {
                
                //string path2 = $"{Directory.GetCurrentDirectory()}{@"\FormatEmail\ConfirmSignUp.html"}";
                
                using (StreamReader reader = new StreamReader(PathHtml))
                {
                    body = reader.ReadToEnd();
                }
                
                body = body.Replace("{UserName}", Name);
                body = body.Replace("{UniqCode}", UniqCd);
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return body;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Repositories.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PlanningApi.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailOptions;

        public EmailSender(IOptions<EmailSettings> emailOptions)
        {
            _emailOptions = emailOptions.Value;
        }

        /// <summary>
        /// Email Sender Sync with subject, message, email receiver : default mailcatcher Options, works for localhost only change the values appsettings.json (referenced on startup file as Ioptions)
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="msg"></param>
        /// <param name="email"></param>
        public void SendEmail(string subject, string msg, string email)
        {

            var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.From = new MailAddress(_emailOptions.FromEmail);
            message.Subject = subject;
            message.Body = msg;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {

                var credential = new NetworkCredential
                {
                    UserName = _emailOptions.UserName,
                    Password = _emailOptions.Password
                };
                smtp.Credentials = credential;
                smtp.Host = _emailOptions.Host;
                smtp.Port = _emailOptions.Port;
                smtp.EnableSsl = _emailOptions.EnableSsl;

                try
                {
                   smtp.Send(message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Email Sender Async with subject, message, email receiver : default mailcatcher Options, works for localhost only change the values appsettings.json (referenced on startup file as Ioptions)
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="msg"></param>
        /// <param name="email"></param>
        public async void SendEmailAsync(string subject, string msg, string email)
        {

            var message = new MailMessage();
            message.To.Add(new MailAddress(email)); 
            message.From = new MailAddress(_emailOptions.FromEmail);
            message.Subject = subject;
            message.Body = msg;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
               
                var credential = new NetworkCredential
                {
                    UserName = _emailOptions.UserName,
                    Password = _emailOptions.Password
                };
                smtp.Credentials = credential;
                smtp.Host = _emailOptions.Host;
                smtp.Port = _emailOptions.Port;
                smtp.EnableSsl = _emailOptions.EnableSsl;

                try { 
                await smtp.SendMailAsync(message);
                } catch(Exception ex)
                {
                    throw ex;
                }
            }
        }


        

    }
}

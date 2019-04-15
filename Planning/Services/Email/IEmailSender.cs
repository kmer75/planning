using Microsoft.AspNetCore.Identity;
using Repositories.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningApi.Services
{
    public interface IEmailSender
    {
        void SendEmailAsync(string subject, string msg, string email);
        void SendEmail(string subject, string msg, string email);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace Society_DataAccess.Services
{
    public class EmailSender : IEmailSender
    {

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("kunalkarne2020@gmail.com", "ctyqyfobnnfxtyjq"),
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("kunalkarne2020@gmail.com"), 
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}

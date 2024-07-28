using BookWheel.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task SendEmailAsync(string to, string from, string subject, string body)
        {
            var emailClient = new SmtpClient(Configuration.GetSection("EmailClient").Value);
            var message = new MailMessage
            {
                From = new MailAddress(from),
                Subject = subject,
                Body = body
            };
            message.To.Add(new MailAddress(to));
            await emailClient.SendMailAsync(message);
        }
    }
}

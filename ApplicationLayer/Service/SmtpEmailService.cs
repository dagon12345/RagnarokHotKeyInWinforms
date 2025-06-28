using ApplicationLayer.Interface;
using Domain.Constants;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ApplicationLayer.Service
{
    public class SmtpEmailService : IEmailService
    {
        public async Task SendConfirmationLinkAsync(string to, string token)
        {
            var subject = "Confirm your email";
            var body = $"Hi! Please enter this confirmation code into your app:\n\n" +
               $"   {token}\n\n" +
               $"This code is valid for 15 minutes.";
            await Send(to, subject, body);
        }
        public async Task SendPasswordResetLinkAsync(string to, string token)
        {
            var subject = "Reset your password";
            var body = $"Hi! Please enter this confirmation code into your app to reset your password:\n\n" +
               $"   {token}\n\n" +
               $"This code is valid for 30 minutes.";
            await Send(to, subject, body);
        }
        public async Task Send(string to, string subject, string body)
        {
            var smtpClient = new SmtpClient(YahooConstants.YahooSmtp)
            {
                Port = 587, // 465 optional
                Credentials = new NetworkCredential(YahooConstants.YahooMail, YahooConstants.YahooAppPassword),
                EnableSsl = true
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress(YahooConstants.YahooMail, GoogleConstants.AppName),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            mailMessage.To.Add(to);
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}

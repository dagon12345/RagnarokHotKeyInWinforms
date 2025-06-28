using ApplicationLayer.Interface;
using Domain.Constants;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
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
            var body = $"Please reset your password, link sent into your email";
            await Send(to, subject, body);
        }
        public async Task Send(string to, string subject, string body)
        {
            var client = new SendGridClient(GoogleConstants.SendGridAPI);
            var from = new EmailAddress(GoogleConstants.GoogleEmail, GoogleConstants.AppName);
            var sendTo = new EmailAddress(to);
            var msg = MailHelper.CreateSingleEmail(from, sendTo, subject, body, null);
            var response = await client.SendEmailAsync(msg);

            if(!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error message:{response.StatusCode}");
            }
        }
    }
}

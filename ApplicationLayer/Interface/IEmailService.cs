using System.Threading.Tasks;

namespace ApplicationLayer.Interface
{
    public interface IEmailService
    {
        Task Send(string to, string subject, string body);
        Task SendConfirmationLinkAsync(string to, string token);
        Task SendPasswordResetLinkAsync(string to, string token);

    }
}

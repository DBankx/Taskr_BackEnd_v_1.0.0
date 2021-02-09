using System.Threading.Tasks;

namespace Taskr.Infrastructure.Mail
{
    public interface IMailService
    {
        Task SendMailAsync(MailRequest request);
    }
}
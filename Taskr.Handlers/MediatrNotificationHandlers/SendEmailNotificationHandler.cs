using System.Threading;
using MediatR;
using Taskr.Infrastructure.Mail;
using Taskr.Infrastructure.MediatrNotifications;

namespace Taskr.Handlers.MediatrNotificationHandlers
{
    public class SendEmailNotificationHandler : INotificationHandler<MailRequestNotification>
    {
        private readonly IMailService _mailService;

        public SendEmailNotificationHandler(IMailService mailService)
        {
            _mailService = mailService;
        }
        
        public async System.Threading.Tasks.Task Handle(MailRequestNotification notification, CancellationToken cancellationToken)
        {
            await _mailService.SendMailAsync(new MailRequest
            {
                Body = notification.Body,
                From = notification.From,
                To = notification.To,
                Subject = notification.Subject
            });
        }
    }
}
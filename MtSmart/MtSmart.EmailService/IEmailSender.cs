namespace MtSmart.EmailService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message, string emailIconPath);
    }
}

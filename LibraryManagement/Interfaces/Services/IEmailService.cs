namespace LibraryManagement.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendHtmlEmailAsync(string toEmail, string subject, string templateFileName, object model);
    }
}

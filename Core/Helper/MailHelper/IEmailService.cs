using Core.Utilities.Results.Abstract;


namespace Core.Helper.MailHelper
{
    public interface IEmailService
    {
        bool IsValidEmail(string email);

        Task<IResult> SendEmailAsync(string email, string name, string subject, string body);
    }
}

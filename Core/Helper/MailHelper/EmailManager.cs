using Core.Configuration.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Text.RegularExpressions;

namespace Core.Helper.MailHelper
{
    public class EmailManager : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailManager(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            var pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            Regex regex = new(pattern);
            return regex.IsMatch(email);
        }

        public async Task<IResult> SendEmailAsync(string email, string name, string subject, string body)
        {
            try
            {
                string senderEmail = _emailConfiguration.Email;
                string senderPassword = _emailConfiguration.Password;
                int port = _emailConfiguration.Port;
                string smtp = _emailConfiguration.SmtpServer;
                string senderName = _emailConfiguration.Name;
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(senderName,senderEmail));
                message.To.Add(new MailboxAddress(name, email));
                message.Subject = subject;
                message.Importance = MessageImportance.High;
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtp, port, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(senderEmail, senderPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return new SuccessResult("Email send succesfully!");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }

        }
    }
}

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
    public class EmailHelper : IEmailHelper
    {
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailHelper(IEmailConfiguration emailConfiguration)
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

        public async Task<IResult> SendEmailAsync(string email, string fullname, string subject, int verifyCode)
        {
            try
            {
                string senderEmail = _emailConfiguration.Email;
                string senderPassword = _emailConfiguration.Password;
                int port = _emailConfiguration.Port;
                string smtp = _emailConfiguration.SmtpServer;

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Karl",senderEmail));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = subject;
                message.Importance = MessageImportance.High;
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = GetBody(fullname,verifyCode)
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

        private string GetBody(string fullName,int code)
        {
            return
                $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  <title>Email Verification</title>\r\n</head>\r\n<body style=\"font-family: Arial, sans-serif;\">\r\n\r\n  <div style=\"max-width: 600px; margin: 0 auto;\">\r\n    <h2>Email Verification Code</h2>\r\n    <p>Dear {fullName},</p>\r\n    <p>Your verification code is: <strong>{code}</strong></p>\r\n    <p>Please use this code to complete your verification process.</p>\r\n    <p>If you did not request this code, please disregard this email.</p>\r\n    <br>\r\n    <p>Best Regards,<br>Karl Ecommerce</p>\r\n  </div>\r\n\r\n</body>\r\n</html>\r\n";
        }
    }
}

using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper.MailHelper
{
    public interface IEmailHelper
    {
        bool IsValidEmail(string email);

        Task<IResult> SendEmailAsync(string email,string fullname, string subject, int verifyCode);
    }
}

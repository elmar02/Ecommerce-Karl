using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs.VerifyDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IVerificationCodeService
    {
        IDataResult<string> CreateVerificationCode(string userId, int code);
        IDataResult<CheckVerifyDTO> FindVerificationCodeByLinkId(string linkId);
        IDataResult<VerificationCode> GetVerificationCodeById(int id);
        IResult IsValidVerificationCode(int code,VerificationCode verificationCode);
    }
}

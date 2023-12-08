using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entities.Concrete;
using Entities.DTOs.VerifyDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class VerificationCodeManager : IVerificationCodeService
    {
        private readonly IVerificationCodeDAL _verifcationCodeDAL;
        private readonly IMapper _mapper;
        public VerificationCodeManager(IVerificationCodeDAL verifcationCodeDAL, IMapper mapper)
        {
            _verifcationCodeDAL = verifcationCodeDAL;
            _mapper = mapper;
        }

        public IDataResult<string> CreateVerificationCode(string userId, int code)
        {
            try
            {
                var verificationCode = new VerificationCode()
                {
                    UserId = userId,
                    Code = code,
                };
                _verifcationCodeDAL.Add(verificationCode);
                return new SuccessDataResult<string>(verificationCode.LinkId.ToString());
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<string>(message: ex.Message);
            }
        }

        public IDataResult<CheckVerifyDTO> FindVerificationCodeByLinkId(string linkId)
        {
            try
            {
                var checkVerify = _verifcationCodeDAL.Get(x => x.LinkId.ToString() == linkId);
                var map = _mapper.Map<CheckVerifyDTO>(checkVerify);
                if (checkVerify == null)
                    return new ErrorDataResult<CheckVerifyDTO>();
                return new SuccessDataResult<CheckVerifyDTO>(map);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<CheckVerifyDTO>(message: ex.Message);
            }
        }

        public IDataResult<VerificationCode> GetVerificationCodeById(int id)
        {
            try
            {
                var checkVerify = _verifcationCodeDAL.Get(x => x.Id == id);
                if (checkVerify == null)
                    return new ErrorDataResult<VerificationCode>();
                return new SuccessDataResult<VerificationCode>(checkVerify);
            }
            catch (Exception)
            {
                return new ErrorDataResult<VerificationCode>();
            }
        }

        public IResult IsValidVerificationCode(int code,VerificationCode verificationCode)
        {
            try
            {
                var expiredDate = verificationCode.ExpiredDate;
                if (expiredDate < DateTime.Now)
                    return new ErrorResult("This verification code has expired!");
                var attempt = verificationCode.AttempCount;
                if (attempt > 3)
                    return new ErrorResult("You attempted wrong more than 3 times!");
                if (code != verificationCode.Code)
                {
                    verificationCode.AttempCount++;
                    verificationCode.UpdatedDate = DateTime.Now;
                    _verifcationCodeDAL.Update(verificationCode);
                    return new ErrorResult($"Invalid Code! ({3-verificationCode.AttempCount} attempts remained!)");
                }
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }
    }
}

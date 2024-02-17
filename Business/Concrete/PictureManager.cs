using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class PictureManager : IPictureService
    {
        private readonly IPictureDAL _pictureDal;
        public PictureManager(IPictureDAL pictureDal)
        {
            _pictureDal = pictureDal;
        }

        public IResult AddPictures(List<string> urls, int productId)
        {
            try
            {
                var pictures = urls.Select(x => new Picture()
                {
                    ProductId = productId,
                    PhotoUrl = x
                });
                _pictureDal.AddRange(pictures);
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }
    }
}

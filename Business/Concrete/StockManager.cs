using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.StockDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class StockManager : IStockService
    {
		private readonly IStockDAL _stockDAL;
        private readonly IMapper _mapper;

        public StockManager(IStockDAL stockDAL, IMapper mapper)
        {
            _stockDAL = stockDAL;
            _mapper = mapper;
        }

        public IResult CreateStock(CreateStockDTO createStockDTO)
        {
            try
            {
                var map = _mapper.Map<Stock>(createStockDTO);
                _stockDAL.Add(map);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult();
            }
        }

        public IDataResult<List<AdminStockListDTO>> GetAllAdminStock(string langCode)
        {
			try
			{
				var stocks = _stockDAL.GetAllAdminStock(langCode);
                return new SuccessDataResult<List<AdminStockListDTO>>(stocks);
			}
			catch (Exception)
			{
                return new ErrorDataResult<List<AdminStockListDTO>>();
			}
        }
    }
}

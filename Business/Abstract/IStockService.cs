using Core.Utilities.Results.Abstract;
using Entities.DTOs.StockDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IStockService
    {
        IDataResult<List<AdminStockListDTO>> GetAllAdminStock(string langCode);
        IResult CreateStock(CreateStockDTO createStockDTO);
    }
}

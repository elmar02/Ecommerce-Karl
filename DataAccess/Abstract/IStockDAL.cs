using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs.StockDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IStockDAL : IRepositoryBase<Stock>
    {
        List<AdminStockListDTO> GetAllAdminStock(string langCode);
    }
}

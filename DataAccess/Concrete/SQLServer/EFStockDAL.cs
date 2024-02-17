using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.StockDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.SQLServer
{
    public class EFStockDAL : EFRepositoryBase<Stock, AppDbContext>, IStockDAL
    {
        public List<AdminStockListDTO> GetAllAdminStock(string langCode)
        {
            using var context = new AppDbContext();
            var stocks = context.Stocks
                .Include(x=>x.Product)
                .ThenInclude(x=>x.ProductLanguages)
                .Select(x=>new AdminStockListDTO()
                {
                    Id = x.Id,
                    ProductName = x.Product.ProductLanguages.FirstOrDefault(x => x.LangCode == langCode).Name,
                    Color = x.ColorCode,
                    Size = x.SizeCode,
                    Stock = x.Count
                }).ToList();
            return stocks;
        }
    }
}

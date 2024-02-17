using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IProductDAL : IRepositoryBase<Product>
    {
        List<AdminProductListDTO> GetAllProductsAdmin(string langCode);
        bool DeleteProductById(int id);
        List<ShopProductListDTO> GetAllShopProducts(string langCode);
        List<ProductNameList> GetAllProductName(string langCode);
    }
}

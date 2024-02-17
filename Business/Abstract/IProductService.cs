using Core.Utilities.Results.Abstract;
using Entities.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductService
    {
        Task<IResult> CreateProductAsync(CreateProductDTO createProductDTO);
        IDataResult<List<AdminProductListDTO>> GetAllProductsAdmin(string langCode);
        IResult DeleteProductById(int id);
        Task<IResult> EditProductAsync(EditProductDTO editProductDTO);
        IDataResult<List<ShopProductListDTO>> GetAllProductsShop(string langCode, FilterDTO filterDTO);
        IDataResult<List<ProductNameList>> GetAllProductName(string langCode);
    }
}

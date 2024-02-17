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
    public class ProductCategoryManager : IProductCategoryServices
    {
        private readonly IProductCategoryDAL _productCategoryDAL;

        public ProductCategoryManager(IProductCategoryDAL productCategoryDAL)
        {
            _productCategoryDAL = productCategoryDAL;
        }

        public IResult AddCategoriesToProduct(List<int> categoryIds, int productId)
        {
            try
            {
                var categories = categoryIds.Select(x => new ProductCategory()
                {
                    CategoryId = x,
                    ProductId = productId
                });
                _productCategoryDAL.AddRange(categories);
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }
    }
}

using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer;
using Entities.Concrete;
using Entities.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductLanguageManager : IProductLanguageService
    {
        private readonly IProductLanguageDAL _productLanguageDAL;

        public ProductLanguageManager(IProductLanguageDAL productLanguageDAL)
        {
            _productLanguageDAL = productLanguageDAL;
        }

        public IResult AddProductLanguages(List<string> languages,List<string> descriptions, int productId)
        {
            try
            {
                var productlanguages = languages.Select((x, index) => new ProductLanguage()
                {
                    ProductId = productId,
                    LangCode = LanguageCodes.Codes.ElementAt(index).Value,
                    Name = x,
                    Description = descriptions[index],
                });
                _productLanguageDAL.AddRange(productlanguages);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult();
            }
        }

        public IResult CheckAllLanguages(List<string> languages)
        {
            var result = _productLanguageDAL.CheckAllLanguages(languages);
            if (!result) return new ErrorResult();
            return new SuccessResult();
        }
    }
}

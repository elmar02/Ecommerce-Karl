using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductLanguageService
    {
        IResult CheckAllLanguages(List<string> languages);
        IResult AddProductLanguages(List<string> languages, List<string> descriptions, int productId);
    }
}

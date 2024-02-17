using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICategoryLanguageService
    {
        IResult AddCategoryLanguages(List<string> languages, int categoryId);
        IResult CheckAllLanguages(List<string> languages);
        IResult CheckLanguagesById(List<string> names,int id);
        IResult DeleteAllLanguages(List<CategoryLanguage> languages);
    }
}

using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CategoryLanguageManager : ICategoryLanguageService
    {
        private readonly ICategoryLanguageDAL _categoryLanguageDAL;

        public CategoryLanguageManager(ICategoryLanguageDAL categoryLanguageDAL)
        {
            _categoryLanguageDAL = categoryLanguageDAL;
        }

        public IResult AddCategoryLanguages(List<string> languages, int categoryId)
        {
            try
            {
                var categorylanguages = languages.Select((x, index) => new CategoryLanguage()
                {
                    CategoryId = categoryId,
                    LangCode = LanguageCodes.Codes.ElementAt(index).Value,
                    Name = x,
                });
                _categoryLanguageDAL.AddRange(categorylanguages);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult();
            }

        }

        public IResult CheckAllLanguages(List<string> languages)
        {
            var result = _categoryLanguageDAL.CheckAllLanguages(languages);
            if(!result) return new ErrorResult();
            return new SuccessResult();
        }

        public IResult CheckLanguagesById(List<string> names, int id)
        {
            var result = _categoryLanguageDAL.CheckLanguagesById(names, id);
            if(!result) return new ErrorResult("Category name already exist!");
            return new SuccessResult();
        }

        public IResult DeleteAllLanguages(List<CategoryLanguage> languages)
        {
            try
            {
                _categoryLanguageDAL.DeleteRange(languages);
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }
    }
}

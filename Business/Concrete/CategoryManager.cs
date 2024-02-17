using Core.Helper;
using Business.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using Entities.Statics;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDAL _categoryDal;
        private readonly ICategoryLanguageService _categoryLanguageService;
        private readonly ISubCategoryService _subCategoryService;
        public CategoryManager(ICategoryDAL categoryDal, ICategoryLanguageService categoryLanguageService, ISubCategoryService subCategoryService)
        {
            _categoryDal = categoryDal;
            _categoryLanguageService = categoryLanguageService;
            _subCategoryService = subCategoryService;
        }

        public IResult CreateCategory(CreateCategoryDTO category)
        {
            try
            {
                var checkNames = _categoryLanguageService.CheckAllLanguages(category.CategoryNames);
                if (checkNames.Success)
                    return new ErrorResult("Category name is already taken");

                if (LanguageCodes.Codes.Count < category.DefaultLanguage)
                    category.DefaultLanguage = 0;

                var defaultCategoryName = category.CategoryNames.ElementAtOrDefault(category.DefaultLanguage);
                if (defaultCategoryName == null)
                    return new ErrorResult("Default Language Input cannot be empty");

                category.CategoryNames = category.CategoryNames.Select(x=> x ?? defaultCategoryName).ToList();

                var newCategory = new Category()
                {
                    SeoUrl = category.CategoryNames.First().ConverToSeo(),
                };
                _categoryDal.Add(newCategory);

                var result = _categoryLanguageService.AddCategoryLanguages(category.CategoryNames, newCategory.Id);
                if(!result.Success) return result;

                var subCategories = _categoryDal.GetAll(x => category.SubCategoryIds.Contains(x.Id)).ToList();
                if (category.SubCategoryIds != null)
                {
                    var result1 = _subCategoryService.AddSubCategories(category.SubCategoryIds, newCategory.Id);
                    if (!result1.Success) return result1;
                }

                subCategories.ForEach(x=>x.IsSubCategory = true);
                _categoryDal.UpdateRange(subCategories);

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public IResult DeleteCategory(int id)
        {
            var result = _categoryDal.DeleteCategoryById(id);
            if(!result)
                return new ErrorResult();
            return new SuccessResult();
        }

        public IResult EditCategory(EditCategoryDTO categoryDTO)
        {
            try
            {
                if (categoryDTO.CategoryNames.Any(x => x == null))
                    return new ErrorResult("Inputs cannot be empty");
                var category = _categoryDal.GetCategoryById(categoryDTO.Id);
                if(category == null)
                    return new ErrorResult();

                var checkLang = _categoryLanguageService.CheckLanguagesById(categoryDTO.CategoryNames, categoryDTO.Id);
                if (!checkLang.Success)
                    return checkLang;

                var deleteLangs = _categoryLanguageService.DeleteAllLanguages(category.CategoryLanguages);
                if (!deleteLangs.Success)
                    return deleteLangs;

                var result = _categoryLanguageService.AddCategoryLanguages(categoryDTO.CategoryNames, categoryDTO.Id);
                if (!result.Success)
                    return result;

                var subs = category.CategorySubCategories;
                if(subs == null)
                    return new SuccessResult();

                var oldSubs = subs;
                if(categoryDTO.SubCategoryIds != null)
                {
                    oldSubs = subs.Where(x => !categoryDTO.SubCategoryIds.Contains(x.SubCategoryId)).ToList();
                }
                if (oldSubs != null)
                {
                    var result2 = _subCategoryService.DeleteSubCategories(oldSubs);
                    if (!result2.Success)
                        return result2;
                }

                if(categoryDTO.SubCategoryIds == null)
                    return new SuccessResult();
                var newSubs = categoryDTO.SubCategoryIds.Where(x=>!subs.Select(x=>x.SubCategoryId).Contains(x)).ToList();
                var result3 = _subCategoryService.AddSubCategories(newSubs, category.Id);
                if (!result3.Success)
                    return result3;

                if (categoryDTO.CategoryNames.First() == null)
                    return new SuccessResult();

                category.SeoUrl = categoryDTO.CategoryNames.First().ConverToSeo();
                _categoryDal.Update(category);

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public IDataResult<List<AdminCategoryListDTO>> GetAllCategoriesAdmin(string langCode)
        {
            try
            {
                var result = _categoryDal.GetAllAdmin(langCode);
                if (result == null)
                    return new ErrorDataResult<List<AdminCategoryListDTO>>();
                return new SuccessDataResult<List<AdminCategoryListDTO>>(result);
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<AdminCategoryListDTO>>();
            }
        }

        public IDataResult<List<AdminSubCategoriesDTO>> GetAllSubCategoriesAdmin(string langCode)
        {
            var subCategories = _categoryDal.GetAllSubCategories(langCode);
            if(subCategories == null)
                return new ErrorDataResult<List<AdminSubCategoriesDTO>>();
            return new SuccessDataResult<List<AdminSubCategoriesDTO>>(subCategories);
        }

        public IDataResult<EditCategoryDTO> GetCategoryAdmin(int id)
        {
            try
            {
                var category = _categoryDal.GetCategoryById(id);
                if (category == null)
                    return new ErrorDataResult<EditCategoryDTO>();
                var dto = new EditCategoryDTO()
                {
                    CategoryNames = category.CategoryLanguages.Select(x => x.Name).ToList(),
                    Id = category.Id,
                    SubCategoryIds = category.CategorySubCategories
                    .Select(x => x.SubCategoryId)
                    .ToList(),
                };
                return new SuccessDataResult<EditCategoryDTO>(dto);
            }
            catch (Exception)
            {
                return new ErrorDataResult<EditCategoryDTO>();
            }
        }
    }
}

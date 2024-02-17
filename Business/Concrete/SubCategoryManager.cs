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
    public class SubCategoryManager : ISubCategoryService
    {
        private readonly ISubCategoryDAL _subCategoryDAL;

        public SubCategoryManager(ISubCategoryDAL subCategoryDAL)
        {
            _subCategoryDAL = subCategoryDAL;
        }

        public IResult AddSubCategories(List<int> subCategoryIds, int categoryId)
        {
            var result = _subCategoryDAL.AddSubCategories(subCategoryIds, categoryId);
            if (!result)
                return new ErrorResult();
            return new SuccessResult();
        }

        public IResult DeleteSubCategories(List<CategorySubCategory> categorySubCategories)
        {
            try
            {
                _subCategoryDAL.DeleteRange(categorySubCategories);
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public IDataResult<List<int>> GetSubCategoryIds(int categoryId)
        {
            try
            {
                var subCategoryIds = _subCategoryDAL.GetAll(x => x.CategoryId == categoryId)
                    .Select(x => x.SubCategoryId).ToList();
                return new SuccessDataResult<List<int>>(subCategoryIds);
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<int>>();
            }
        }
    }
}

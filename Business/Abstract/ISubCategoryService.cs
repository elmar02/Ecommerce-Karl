using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ISubCategoryService
    {
        IResult AddSubCategories(List<int> subCategoryIds, int categoryId);
        IDataResult<List<int>> GetSubCategoryIds(int categoryId);
        IResult DeleteSubCategories(List<CategorySubCategory> categorySubCategories);
    }
}

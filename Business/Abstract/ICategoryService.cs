using Core.Utilities.Results.Abstract;
using Entities.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        IDataResult<List<AdminCategoryListDTO>> GetAllCategoriesAdmin(string langCode);
        IDataResult<List<AdminSubCategoriesDTO>> GetAllSubCategoriesAdmin(string langCode);
        IResult CreateCategory(CreateCategoryDTO category);
        IResult DeleteCategory(int id);
        IDataResult<EditCategoryDTO> GetCategoryAdmin(int id);
        IResult EditCategory(EditCategoryDTO categoryDTO);
    }
}

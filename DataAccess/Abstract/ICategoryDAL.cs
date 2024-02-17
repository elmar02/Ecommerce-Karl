using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface ICategoryDAL : IRepositoryBase<Category>
    {
        List<AdminCategoryListDTO> GetAllAdmin(string langCode);
        List<AdminSubCategoriesDTO>? GetAllSubCategories(string langCode);
        bool DeleteCategoryById(int id);
        Category? GetCategoryById(int id);
    }
}

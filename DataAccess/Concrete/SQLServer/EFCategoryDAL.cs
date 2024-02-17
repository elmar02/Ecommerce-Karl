using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.CategoryDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.SQLServer
{
    public class EFCategoryDAL : EFRepositoryBase<Category, AppDbContext>, ICategoryDAL
    {
        public bool DeleteCategoryById(int id)
        {
            try
            {
                using var context = new AppDbContext();
                var category = context.Categories
                .FirstOrDefault(c => c.Id == id);
                if (category == null)
                    return false;
                var subCategories = context.CategorySubCategories
                    .Include(x=>x.Category)
                    .Include(x=>x.SubCategory)
                    .Where(c => c.SubCategoryId == id || c.CategoryId == id).ToList();
                if (subCategories != null)
                    context.CategorySubCategories.RemoveRange(subCategories);
                context.Categories.Remove(category);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<AdminCategoryListDTO> GetAllAdmin(string langCode)
        {
            try
            {
                using var context = new AppDbContext();
                var categories = context.Categories
                    .Include(x=>x.CategorySubCategories)
                    .Include(x=>x.CategoryLanguages)
                    .Select(x => new AdminCategoryListDTO()
                    {
                        Id = x.Id,
                        Name = x.CategoryLanguages.FirstOrDefault(y => y.LangCode == langCode).Name,
                        SubCategories = x.CategorySubCategories.Select(y => y.SubCategory.CategoryLanguages.FirstOrDefault(x=>x.LangCode == langCode).Name).ToList()
                    }).ToList();

                return categories;
            }
            catch (Exception)
            {
                return [];
            }
        }

        public List<AdminSubCategoriesDTO>? GetAllSubCategories(string langCode)
        {
            try
            {
                using var context = new AppDbContext();
                var subCategories = context.Categories
                    .Include(x=>x.CategoryLanguages)
                    .Select(x => new AdminSubCategoriesDTO()
                    {
                        Id = x.Id,
                        Name = x.CategoryLanguages.FirstOrDefault(y => y.LangCode == langCode).Name
                    }).ToList();
                return subCategories;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Category? GetCategoryById(int id)
        {
            try
            {
                using var context = new AppDbContext();
                var category = context.Categories
                .Include(x => x.CategoryLanguages)
                .Include(x => x.CategorySubCategories)
                    .ThenInclude(x => x.SubCategory)
                .FirstOrDefault(x => x.Id == id);

                return category;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
    }
}

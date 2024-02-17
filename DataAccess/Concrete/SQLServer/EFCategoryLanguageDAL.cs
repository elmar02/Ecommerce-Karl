using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.SQLServer
{
    public class EFCategoryLanguageDAL : EFRepositoryBase<CategoryLanguage, AppDbContext>, ICategoryLanguageDAL
    {
        public bool CheckAllLanguages(List<string> languages)
        {
			try
			{
                using var context = new AppDbContext();
                return context.CategoryLanguages.Any(x=>languages.Contains(x.Name));
            }
			catch (Exception)
			{
                return false;
			}

        }

        public bool CheckLanguagesById(List<string> names, int id)
        {
            try
            {
                using var context = new AppDbContext();
                var result = context.CategoryLanguages
                    .Where(x=> x.CategoryId != id).Any(x=>names.Contains(x.Name));
                return !result;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

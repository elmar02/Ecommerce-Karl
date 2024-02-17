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
    public class EFProductLanguageDAL : EFRepositoryBase<ProductLanguage, AppDbContext>, IProductLanguageDAL
    {
        public bool CheckAllLanguages(List<string> languages)
        {
            try
            {
                using var context = new AppDbContext();
                return context.ProductLanguages.Any(x => languages.Contains(x.Name));
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

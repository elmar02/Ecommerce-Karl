using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface ICategoryLanguageDAL : IRepositoryBase<CategoryLanguage>
    {
        bool CheckAllLanguages(List<string> languages);
        bool CheckLanguagesById(List<string> languages,int id);
    }
}

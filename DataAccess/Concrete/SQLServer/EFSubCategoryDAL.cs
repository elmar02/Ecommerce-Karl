using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
namespace DataAccess.Concrete.SQLServer
{
    public class EFSubCategoryDAL : EFRepositoryBase<CategorySubCategory, AppDbContext>, ISubCategoryDAL
    {

        public bool AddSubCategories(List<int> subCategoryIds, int categoryId)
        {
			try
			{
				using var context = new AppDbContext();
				var subCategories = new List<CategorySubCategory>();
				foreach (var subCategoryId in subCategoryIds)
				{
					subCategories.Add(new CategorySubCategory()
					{
						CategoryId = categoryId,
						SubCategoryId = subCategoryId
					});
				}
				AddRange(subCategories);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
        }
    }
}

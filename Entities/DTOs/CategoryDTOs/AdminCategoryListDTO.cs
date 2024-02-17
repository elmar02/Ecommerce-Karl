using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.CategoryDTOs
{
    public class AdminCategoryListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> SubCategories { get; set; }
    }
}

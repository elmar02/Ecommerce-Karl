using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.CategoryDTOs
{
    public class EditCategoryDTO
    {
        public int Id { get; set; }
        public List<string> CategoryNames { get; set; }
        public List<int>? SubCategoryIds { get; set; }
    }
}

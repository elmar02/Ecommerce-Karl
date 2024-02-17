using Entities.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.CategoryDTOs
{
    public class CreateCategoryDTO
    {
        public List<string> CategoryNames { get; set; }
        public List<int> SubCategoryIds { get; set;}
        public int DefaultLanguage { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class FilterDTO
    {
        public List<int> CategoryIds { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; } = 10000;
        public List<string> Colors { get; set; }
        public List<string> Sizes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class ShopProductListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SeoUrl { get; set; }
        public double Price { get; set; }
        public double DisCount { get; set; } = 0;
        public double Rating { get; set; } = 0;
        public string ThumbnailUrl { get; set; }
        public List<int> CategoryIds { get; set; }

    }
}

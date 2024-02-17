using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.ProductDTOs
{
    public class AdminProductListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public double Price { get; set; }
        public double DisCount { get; set; }
        public double Rating { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsInList { get; set; }
    }
}

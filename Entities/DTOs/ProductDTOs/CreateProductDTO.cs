using Microsoft.AspNetCore.Http;

namespace Entities.DTOs.ProductDTOs
{
    public class CreateProductDTO
    {
        public List<string> Names { get; set; }
        public List<string> Descriptions { get; set;}
        public double Price { get; set; }
        public double Discount { get; set; }
        public bool IsInList { get; set; }
        public bool IsFeatured { get; set; }
        public IFormFile Thumbnail { get; set; }
        public List<IFormFile>? Photos { get; set; }
        public Dictionary<string,string>? Specifications { get; set; }
        public List<int> SubCategoryIds { get; set; }
        public int DefaultLanguage { get; set; }
    }
}

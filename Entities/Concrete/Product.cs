using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Product : BaseEntity, IEntity
    {
        public List<ProductLanguage> ProductLanguages { get; set; }
        public string SeoUrl { get; set; }
        public double Price { get; set; }
        public double DisCount { get; set; } = 0;
        public double Rating { get; set; } = 0;
        List<Stock> Stocks { get; set; }
        public string ThumbnailUrl { get; set; }
        public List<Picture> Pictures { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsInList { get; set; }
        public List<Specification> Specifications { get; set; }
    }
}

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
        public string Description { get; set; }
        public double Price { get; set; }
        public double DisCount { get; set; } = 0;
        public double Rating { get; set; } = 0;
        public int Stock { get; set; } = 0;
        public List<Picture> Pictures { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsInList { get; set; }
        public List<Specification> Specifications { get; set; }
    }
}

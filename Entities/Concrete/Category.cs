﻿using Core.Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Category : BaseEntity, IEntity
    {
        public List<CategoryLanguage> CategoryLanguages { get; set; }
        public bool IsSubCategory { get; set; } = false;
        public List<Category> SubCategories { get; set; }
        public List<Product> Products { get; set; }
    }
}

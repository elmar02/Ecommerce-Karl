﻿using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductCategoryServices
    {
        IResult AddCategoriesToProduct(List<int> categoryIds, int productId);
    }
}

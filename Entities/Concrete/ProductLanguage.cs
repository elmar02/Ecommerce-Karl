using Core.Entities;
using Entities.Common;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ProductLanguage : BaseEntity, IEntity
    {
        public string Name { get; set; }
        public Languages LangCode { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}

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
    public class CategoryLanguage : BaseEntity, IEntity
    {
        public string Name { get; set; }
        public Languages LangCode { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

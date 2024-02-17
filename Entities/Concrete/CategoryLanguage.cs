using Core.Entities;
using Entities.Common;

namespace Entities.Concrete
{
    public class CategoryLanguage : BaseEntity, IEntity
    {
        public string Name { get; set; }
        public string LangCode { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

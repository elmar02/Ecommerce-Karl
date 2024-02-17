using Core.Entities;
using Entities.Common;

namespace Entities.Concrete
{
    public class ProductLanguage : BaseEntity, IEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LangCode { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}

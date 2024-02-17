using Core.Entities;
using Entities.Statics.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Stock : IEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public Colors ColorCode { get; set; }
        public Sizes SizeCode { get; set; }
        public int Count { get; set; }
    }
}

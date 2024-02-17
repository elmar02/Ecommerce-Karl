using Entities.Concrete;
using Entities.Statics.Enums;

namespace Entities.DTOs.StockDTOs
{
    public class CreateStockDTO
    {
        public int ProductId { get; set; }
        public Colors ColorCode { get; set; }
        public Sizes SizeCode { get; set; }
        public int Count { get; set; }
    }
}

using Entities.Statics.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.StockDTOs
{
    public class AdminStockListDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public Sizes Size { get; set; }
        public Colors Color { get; set; }
        public int Stock { get; set; }
    }
}

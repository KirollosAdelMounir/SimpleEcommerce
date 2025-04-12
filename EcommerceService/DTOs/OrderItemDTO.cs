using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceService.DTOs
{
    public class OrderItemDTO
    {
        public int ProductId {  get; set; }
        public int RequestQty { get; set; }
    }
}

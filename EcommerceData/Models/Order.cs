using EcommerceCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceData.Models
{
    public class Order
    { 
        public Guid Id { get; set; } = Guid.NewGuid();
        public List<OrderItem> ProductItems { get; set;} = new List<OrderItem>();
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;
    }
}

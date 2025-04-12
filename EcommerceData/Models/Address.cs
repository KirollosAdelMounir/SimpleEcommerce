using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceData.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string FullAddress { get; set; }
        public bool IsDefault { get; set; } = false;
        public string PostalCode { get; set; }
    }
}

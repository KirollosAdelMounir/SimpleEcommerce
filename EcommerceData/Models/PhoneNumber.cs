using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceData.Models
{
    public class PhoneNumber
    {
        public int Id { get; set; }
        public string PhoneNum { get; set; }
        public bool IsDefault { get; set; } = false;
    }
}

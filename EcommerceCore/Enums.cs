using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceCore
{
    public enum AccountRole
    {
        Admin,
        User
    }
    public enum OrderStatus
    {
        Pending,
        Completed,
        Received,
        Preparing,
        Delivering,
        Cancelled
    }
}

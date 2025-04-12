using EcommerceData.DBContext;
using EcommerceData.Models;
using EcommerceData.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceService.ProductService
{
    public class ProductService : Repository<Product>, IProductService
    {
        public ProductService(EcommerceContext context) : base(context)
        {
        }

        public async Task<bool> UpdateQuantity(int Id, int newAvailableQty)
        {
            return true;
        }
    }
}

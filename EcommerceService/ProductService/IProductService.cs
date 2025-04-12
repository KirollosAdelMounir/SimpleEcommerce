using EcommerceData.Models;
using EcommerceData.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceService.ProductService
{
    public interface IProductService : IRepository<Product>
    {
        Task<bool> UpdateQuantity(int Id, int newAvailableQty);
    }
}

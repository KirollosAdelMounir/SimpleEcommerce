using EcommerceData.Models;
using EcommerceData.Repository;
using EcommerceService.ProductService;
using EcommerceService.SystemUserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceService.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IProductService _productService , ISystemUserService _systemUserService , IRepository<Order> _orderRepository , IRepository<OrderItem> _orderItemRepository)
        {
            ProductService = _productService;
            SystemUserService = _systemUserService;
            OrderRepository = _orderRepository;
            OrderItemRepository = _orderItemRepository;

        }
        public IProductService ProductService { get; }
        public ISystemUserService SystemUserService {  get; }
        public IRepository<Order> OrderRepository { get; }
        public IRepository<OrderItem> OrderItemRepository { get; }
    }
}

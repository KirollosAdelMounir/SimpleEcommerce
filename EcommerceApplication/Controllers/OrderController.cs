using AutoMapper;
using EcommerceCore;
using EcommerceData.Models;
using EcommerceService.DTOs;
using EcommerceService.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Guid userId;
        public OrderController(IUnitOfWork unitofwork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitofwork;
            userId = new Guid(_unitOfWork.SystemUserService.GetUserId());
        }
        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders (Guid UserId = new Guid() , OrderStatus status = OrderStatus.Completed)
        {
            List<Order> orders = new List<Order>();
            if (UserId != Guid.Empty) 
            {
                orders = (await _unitOfWork.SystemUserService.GetById(UserId , x=>x.Orders)).Orders.Where(x=>x.Status == status).ToList();
            }
            else
            {
                orders = (await _unitOfWork.OrderRepository.GetAll(x=>x.Status== status)).ToList();
            }
            return Ok(orders);
        }
        [HttpGet("GetOrderDetails")]
        public IActionResult GetOrderDetails(Guid Id = new Guid())
        {
            Order order = new Order();
            if (Id != Guid.Empty)
            {
                order = _unitOfWork.SystemUserService.GetAllSynchronous(x => x.Id == userId).Include(x => x.Orders).ThenInclude(x => x.ProductItems).ThenInclude(x => x.Product)
                   .FirstOrDefault()?.Orders.FirstOrDefault(x => x.Status == OrderStatus.Pending);
            }
            else
            {
                order = _unitOfWork.OrderRepository.GetAllSynchronous(x => x.Id == Id).Include(x => x.ProductItems).ThenInclude(x => x.Product).FirstOrDefault();
            }

            return Ok(order);
        }
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromBody] OrderItemDTO item)
        {
            var user = await _unitOfWork.SystemUserService.GetById(userId, x => x.Orders);
            var order = user.Orders.FirstOrDefault(x=>x.Status == OrderStatus.Pending);
            var product = _mapper.Map<OrderItem>(item);
            if(order == null)
            {
                order = new Order(); order.ProductItems.Add(product);
                await _unitOfWork.SystemUserService.UpdateAsync(user.Id,  x => {
                    x.Orders.Add(order);
                     _unitOfWork.OrderRepository.ChangeEntityState(order,EntityState.Added);
                     _unitOfWork.OrderItemRepository.ChangeEntityState(product, EntityState.Added);

                });
            }
            else
            {
                await _unitOfWork.OrderRepository.UpdateAsync(order.Id,  x =>
                {
                    order.ProductItems.Add(product);
                     _unitOfWork.OrderItemRepository.ChangeEntityState(product, EntityState.Added);
                });
            }
            return Ok("Item added to your cart");
        }
        [HttpPut("ChangeOrderStatus")]
        public async Task<IActionResult> ChangeOrderStatus(Guid Id , OrderStatus status)
        {
            await _unitOfWork.OrderRepository.UpdateAsync(Id , x=>x.Status = status);
            return Ok("Order status changed successfully");
        }
    }
}

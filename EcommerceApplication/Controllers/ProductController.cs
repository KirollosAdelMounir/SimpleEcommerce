using AutoMapper;
using EcommerceCore;
using EcommerceData.Models;
using EcommerceService.DTOs;
using EcommerceService.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductController(IUnitOfWork unitofwork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitofwork;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddNewProduct")]
        public async Task<IActionResult> AddNewProduct([FromBody]ProductDTO product)
        {
            var newProduct = _mapper.Map<Product>(product);
            return Ok(await _unitOfWork.ProductService.AddAsync(newProduct));
        }
        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts(int Id = 0)
        {
            return Id != 0 ? Ok(await _unitOfWork.ProductService.GetById(Id)) 
                : Ok(await _unitOfWork.ProductService.GetAll(x=>x.AvailableQuantity > 0)); 
        }
    }
}

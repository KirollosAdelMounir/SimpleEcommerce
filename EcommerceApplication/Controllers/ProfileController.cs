using AutoMapper;
using EcommerceService.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProfileController(IUnitOfWork unitofwork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitofwork;
        }
        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut([FromBody] string refreshToken)
        {
            await _unitOfWork.SystemUserService.LogOut(refreshToken);
            return Ok();
        }
    }
}

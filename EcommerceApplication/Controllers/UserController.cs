using AutoMapper;
using EcommerceCore.CoreModels;
using EcommerceData.Models;
using EcommerceService.DTOs;
using EcommerceService.JwtTokenHelper;
using EcommerceService.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserController(IUnitOfWork unitofwork , IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitofwork;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var result = await _unitOfWork.SystemUserService.Login(login.Email, login.Password);
            return result.GetType() == typeof(TokensDTO) ? Ok(result) : BadRequest(result);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO register)
        {
            var newUser = _mapper.Map<SystemUser>(register);
            return Ok (await _unitOfWork.SystemUserService.AddAsync(newUser));
        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshSession([FromBody] string RefreshToken)
        {
            var user = await _unitOfWork.SystemUserService.GetByUniqueDetail(x => x.RefreshTokens.FirstOrDefault(x => x.Token == RefreshToken && x.IsRevoked == false && x.ExpiryDate > DateTime.Now) != null);
            if (user == null)
            {
                return Unauthorized("Session Expired Relogin Again");
            }
            else
            {
                var jwtValue = _unitOfWork.SystemUserService.GetJwtSettings();
                return Ok(JwtTokenHelper.GenerateTokens(user, jwtValue.SecretKey, jwtValue.ExpiryMinutes, jwtValue.Issuer, jwtValue.Audience).accessToken);
            }
        }
    }
}

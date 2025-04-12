using EcommerceCore.CoreModels;
using EcommerceData.DBContext;
using EcommerceData.Models;
using EcommerceData.Repository;
using EcommerceService.DTOs;
using EcommerceService.JwtTokenHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using EcommerceCore;
using Microsoft.IdentityModel.Tokens;


namespace EcommerceService.SystemUserService
{
    public class SystemUserService : Repository<SystemUser>, ISystemUserService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EcommerceContext _context;
        public SystemUserService(EcommerceContext context, IOptions<JwtSettings> jwtSettings, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _jwtSettings = jwtSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        #region Override, Overload and Private Methods
        public string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User ID not found.");
            }
            return userId;
        }
        public JwtSettings GetJwtSettings ()=>_jwtSettings;
        private async Task<TokensDTO> RefreshTokenGenerator(SystemUser user)
        {
            var tokens = JwtTokenHelper.JwtTokenHelper.GenerateTokens(user, _jwtSettings.SecretKey, _jwtSettings.ExpiryMinutes, _jwtSettings.Issuer, _jwtSettings.Audience);
            await UpdateAsync(user.Id,
                refresher =>
                {
                    var newToken = new RefreshToken
                    {
                        Token = tokens.refreshToken,
                        ExpiryDate = DateTime.Now.AddDays(30),
                        IsRevoked = false
                    };
                    refresher.RefreshTokens.Add(newToken);
                    _context.Entry(newToken).State = EntityState.Added;
                });
            return new TokensDTO(tokens.accessToken, tokens.refreshToken);
        }
        public override async Task<SystemUser> AddAsync(SystemUser entity)
        {
            var IsExisted = (await GetByUniqueDetail(x => x.Email == entity.Email)) is null ? false : true;
            if (!IsExisted)
            {
                entity.Password = Encryptions.HashPassword(entity.Password);
                return await base.AddAsync(entity);
            }
            else
            {
                throw new Exception("User Exists");
            }
        }

        #endregion
        public async Task<object> Login(string Email, string Password)
        {
            var User = await GetByUniqueDetail(x => x.Email == Email, x => x.RefreshTokens);
            return Encryptions.VerifyPassword(Password, User.Password) ? await RefreshTokenGenerator(User) : "Incorrect Email or Password";
        }
        public async Task LogOut(string refreshToken)
        {
            await UpdateAsync(new Guid(GetUserId()), x => 
                x.RefreshTokens.FirstOrDefault(x => x.Token.Equals(refreshToken)).IsRevoked = true
            , x => x.RefreshTokens);
        }
    }
}

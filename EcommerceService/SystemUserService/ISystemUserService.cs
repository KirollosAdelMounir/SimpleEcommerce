using EcommerceCore.CoreModels;
using EcommerceData.Models;
using EcommerceData.Repository;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceService.SystemUserService
{
    public interface ISystemUserService : IRepository<SystemUser>
    {
        Task<object> Login(string Email, string Password);
        string GetUserId();
        Task LogOut(string refreshToken);
        JwtSettings GetJwtSettings();
    }
}

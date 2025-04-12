using EcommerceApplication;
using EcommerceCore.CoreModels;
using EcommerceData.DBContext;
using EcommerceData.Repository;
using EcommerceService.AutoMapperProfile;
using EcommerceService.ProductService;
using EcommerceService.SystemUserService;
using EcommerceService.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add Cors for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("*") // Replace with your Swagger UI's domain
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
//Configuring AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add JWT Checker 
// Authentication Configuration (JWT)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });
builder.Services.AddHttpContextAccessor();
//Injecting Jwt Settings 
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
// Adding Database Context
builder.Services.AddDbContext<EcommerceContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConString")));
// Adding Main Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
// Adding Services
builder.Services.AddScoped<ISystemUserService, SystemUserService>();
builder.Services.AddScoped<IProductService, ProductService>();
// Unit Of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");
app.MigrateDatabase();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

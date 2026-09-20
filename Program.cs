using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using SalesDB.Data;
using SalesDB.Mapping;
using SalesDB.Repositories;
using SalesDB.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers(); // Đăng ký DI controllers

builder.Services.AddOpenApi();
// builder.Services.AddSwaggerGen(); // cài thêm dotnet add package Swashbuckle.AspNetCore
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("bearer",
    new OpenApiSecurityScheme()
    {
        Name = "JWT Authentication",
        Description = "Nhập JWT Access Token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    }
    );
    option.AddSecurityRequirement(doc =>
        new OpenApiSecurityRequirement()
        {
            [new OpenApiSecuritySchemeReference("bearer", doc)] = []
        }
    );
});

// iterface , class thuc thi interface
builder.Services.AddDbContext<SalesDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI SERVICE
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomersService, CustomersService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// JWT
builder.Services.AddScoped<IJwtService, JwtService>();

// builder.Services.AddScoped<IOrderService, OrderService>();
// giữ khởi tạo của OrderService cho đến khi dữ liệu trả về cho api

// addsingleton OrderService - khởi tạo 1 lần dùng cho cả hệ thống
// 15:57p Nga => order  chờ 5p vì mạng chậm  => nhận dc dữ liệu của Phát


// 15h58 Phát => order -> lấy dc dữ liệu ngay vì mạng mạnh 

// DI REPO
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// Automapper

builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

var key = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:key chua duoc cau hinh");
//  Kiem tra token
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
    };
});
builder.Services.AddAuthorization();




var app = builder.Build();
//
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHttpsRedirection();



app.Run();


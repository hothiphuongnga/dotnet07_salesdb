using Microsoft.EntityFrameworkCore;
using SalesDB.Data;
using SalesDB.Repositories;
using SalesDB.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers(); // Đăng ký DI controllers

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(); // cài thêm dotnet add package Swashbuckle.AspNetCore

// iterface , class thuc thi interface
builder.Services.AddDbContext<SalesDbContext>(opt=>opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI SERVICE
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomersService, CustomersService>();
// 

// builder.Services.AddScoped<IOrderService, OrderService>();
// giữ khởi tạo của OrderService cho đến khi dữ liệu trả về cho api

// addsingleton OrderService - khởi tạo 1 lần dùng cho cả hệ thống
// 15:57p Nga => order  chờ 5p vì mạng chậm  => nhận dc dữ liệu của Phát


// 15h58 Phát => order -> lấy dc dữ liệu ngay vì mạng mạnh 

// DI REPO
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();   
}
app.MapControllers();

app.UseHttpsRedirection();



app.Run();


using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using SalesDB.Data;
using SalesDB.Dtos.Base;
using SalesDB.Filters;
using SalesDB.Mapping;
using SalesDB.Middlewares;
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

// cấu hình cors
builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowCors", policy =>
    {
        // cho phép nhận request từ domain này
        // https://cybersoft.vn  hay https://demo.cybersoft.vn/
        var allowOrigin = builder.Configuration.GetSection("Cors:AllowOrigins").Get<string[]>() ?? [];
        policy.WithOrigins(allowOrigin)
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
    option.AddPolicy("All", policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

//  FILTER
builder.Services.AddScoped<DemoAuthorizationFilter>();
builder.Services.AddScoped<DemoResourceFilter>();
builder.Services.AddScoped<CacheResourceFilter>();
builder.Services.AddScoped<DemoActionFilter>();
builder.Services.AddScoped<DemoExceptionFilter>();
builder.Services.AddScoped<DemoResultFilter>();




var app = builder.Build();

// STATIC FILES MIDDLEWARE
// tìm file trong wwwroot và trả thẳng cho client nếu tìm thấy ,
// 
// nếu file chứa thông tin tĩnh khong phải tên wwwroot mà là StaticFiles thì phải cấu hình thêm
// 

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")
    ),
    RequestPath = "/files", // truy cập file trong StaticFiles qua đường đẫn /files
    //  giows hạn file
    OnPrepareResponse = opt =>
    {
        var path  = opt.File.PhysicalPath;
        // chỉ cho lấy jpg và png
        if(!path.EndsWith(".jpg") && !path.EndsWith(".png"))
        {
            opt.Context.Response.StatusCode = 403;
            opt.Context.Response.ContentLength = 0;
        }
        //              60s * 60p * 24h
        const int exp = 60 * 60 * 24; // 24 gio
        opt.Context.Response.Headers.Append("Cache-Control",$"public,max-age={exp}");
    }
}
     
);
app.UseCors("AllowCors");
// sử dụng middleware
app.UseGlobalExceptionHandle();
// DI 1 dongf nafy thooi
// đã đóng gói hết các miđleware lại bên trong rồi
// app.UseMiddlewareExtensions();


// chạy theo thứ tự
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

// Routing Middleware 
// so khớp URL + Http method vói endpoin
// chỉ so chứ chưa thực hiện
// HttpContext.GetEndpoint() - từ net 6 là đã tự đôngh thêm UseRouting rồi
// app.UseRouting();
 
app.Use(async (context,next) =>
{
    var endpoint = context.GetEndpoint();

    Console.WriteLine("Path: " + context.Request.Path);
    Console.WriteLine("Method: " + context.Request.Method);
    Console.WriteLine("Endpoint: " + endpoint?.DisplayName);

    await next(); // gọi next để cho đi tiếp
});

// endpoi
// do controler có dùng [Route] nên đã dùng middelware endpoint
app.MapControllers();

app.UseHttpsRedirection();



app.Run();



// request -> useRouting (chọn enpoint phù hợp)
// pas qua các middle khác 
// kiểm tra thời gian của 1 api

// JWT có ba phần
//Header.Payload.Signature
//Header:  thuật toán token hs256..., jwt
//Payload: chứa các claim - tài sản , bảo mật không đưa vào claim
//  Signature: 
// tạo ra token vói Signature : 
// => 401 
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Unicode;
using Microsoft.IdentityModel.Tokens;
using SalesDB.Entities;

namespace SalesDB.Services;

public interface IJwtService
{
    // tạo token
    string GenerateToken(Employee employee);
}

public class JwtService(IConfiguration _config) : IJwtService
{
    public string GenerateToken(Employee employee)
    {
        // lấy thông tin key ... trong appsetting
        var keyJWT = _config["Jwt:Key"] ?? "";
        var issuer = _config["Jwt:Issuer"]; // noi phát hành
        var audience = _config["Jwt:Audience"]; // ai sẽ dùng 
        var exp = _config.GetValue<int>("Jwt:Exp"); // tg het han

        // tạo claim

        var claim = new List<Claim>();
        claim.Add(new Claim(ClaimTypes.NameIdentifier, employee.EmployeeId.ToString()));
        claim.Add(new Claim(ClaimTypes.Role, employee.Role));
        claim.Add(new Claim(ClaimTypes.Name, employee.Name));
        claim.Add(new Claim("Phone", employee.Phone ?? ""));
        // 
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyJWT));

        var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token  = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claim,
            expires: DateTime.UtcNow.AddMinutes(exp),
            signingCredentials: credential
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

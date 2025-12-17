using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ERP.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ERP.Application.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        private readonly string _jwtKey;
        private readonly string _jwtIssuer;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtKey = _configuration["Jwt:Key"]!;
            _jwtIssuer = _configuration["Jwt:Issuer"]!;
        }

        public string GerarToken(Usuario usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Role),
                new Claim("empresaId", usuario.EmpresaId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtIssuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

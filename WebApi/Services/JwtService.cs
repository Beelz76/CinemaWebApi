using WebApi.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Interface;

namespace WebApi.Services
{
    public class JwtService : IJwtService
    {
        private readonly IOptions<JwtAuthenticationOptions> _options;

        public JwtService(IOptions<JwtAuthenticationOptions> options)
        {
            _options = options;
        }

        public string GenerateToken(Guid userUid, string login, bool IsAdmin)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userUid.ToString()),
                new Claim(ClaimTypes.Name, login),
                new Claim(ClaimTypes.Role, IsAdmin? "Admin" : "User")
            }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = _options.Value.Issuer,
                Audience = _options.Value.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key)), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}

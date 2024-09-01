using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Domain.Account;

namespace TaskManager.Infra.Data.Security
{
    public class AuthenticateService : IAuthenticate
    {
        private readonly IPasswordHasher _passwordHasher;
        public readonly IConfiguration _configuration;

        public AuthenticateService(IConfiguration configuration, IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _configuration = configuration;
        }

        public string GenerateToken(Guid userId, string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.
                GetBytes(_configuration["TokenConfigurations:Secret"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["TokenConfigurations:Issuer"],
                Audience = _configuration["TokenConfigurations:Audience"],
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidatePassword(string enteredPassword, string storedHash)
        {
            var hash = _passwordHasher.HashPassword(enteredPassword);

            return storedHash == hash;
        }

        public string CryptoPassword(string password)
        {
            return _passwordHasher.HashPassword(password);
        }
    }
}

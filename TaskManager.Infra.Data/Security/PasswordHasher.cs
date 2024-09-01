using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace TaskManager.Infra.Data.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public readonly IConfiguration _configuration;

        public PasswordHasher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Uma maneira mais simples e menos segura de criptografia, utilizando apenas hash, não aplicando salt para não modificar a estrutura de banco de dados sugerida.
        public string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));

            // Obtém a chave secreta diretamente do arquivo de configuração
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration["TokenConfigurations:Secret"]));

            string hashedPassword;
            var encoding = new ASCIIEncoding();
            byte[] keyBytes = secretKey.Key; // Usa a chave secreta diretamente
            byte[] passwordBytes = encoding.GetBytes(password);

            using (var hmacsha256 = new HMACSHA256(keyBytes))
            {
                byte[] hashBytes = hmacsha256.ComputeHash(passwordBytes);
                hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();
            }

            return hashedPassword;
        }
    }
}

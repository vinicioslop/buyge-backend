using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using buyge_backend.db;
using Microsoft.IdentityModel.Tokens;

namespace buyge_backend
{
    public static class TokenService
    {
        public static string GenerateToken(TbCliente cliente)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, cliente.NmCliente),
                    new Claim(ClaimTypes.Role, cliente.NmTipoConta)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
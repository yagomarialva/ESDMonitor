using BiometricFaceApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BiometricFaceApi.Auth
{
    public class JwtAuthentication
    {
        private readonly IConfiguration configuration;
        public JwtAuthentication(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GenerateJSONWebToken(AuthenticationModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:secretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(configuration["jwt:issuer"],
              configuration["jwt:audience"],
              new List<Claim> { 
                  new Claim(ClaimTypes.Name,user.Username),
                  new Claim(ClaimTypes.Role, user.RolesName)
              },
              expires: DateTime.Now.AddMinutes(60),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

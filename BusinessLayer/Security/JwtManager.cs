using EntityLayer.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Security
{
    public class JwtManager : IJwtManager
    {
        private readonly JwtManager _jwtManager;
        public JwtManager(JwtManager jwtManager)
        {
            _jwtManager = jwtManager;
        }


        public string GenerateJSONWebToken(User userInfo, Role roleInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtManager["Jwt:Key"]));
            if (securityKey.KeySize < 128)
            {
                securityKey = new SymmetricSecurityKey(new byte[16]); // Or some other method to generate a larger key
            }
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.Email, userInfo.Email.ToString()),
        new Claim(ClaimTypes.Role, roleInfo.RoleName),
        new Claim(JwtRegisteredClaimNames.Jti, userInfo.Password.ToString()),

            };

            var token = new JwtSecurityToken(
             issuer: _jwtManager["Jwt:Issuer"],
        audience: _jwtManager["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_jwtManager["Jwt:ExpiryInMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
    }
}

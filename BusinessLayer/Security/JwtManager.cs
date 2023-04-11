using EntityLayer.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Security
{
    public class JwtManager : IJwtManager
    {
        private readonly JwtSettings _jwtSettings;

        public JwtManager(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public string GenerateJSONWebToken(User userInfo, Role roleInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings["Jwt:Key"]));
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
                issuer: _jwtSettings["Jwt:Issuer"],
                audience: _jwtSettings["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_jwtSettings["Jwt:ExpiryInMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

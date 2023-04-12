using DataAccessLayer.Abstract;
using EntityLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Security
{
    public class JwtManager : IJwtManager
    {
        private readonly IConfiguration _configuration;
        private readonly IBaseRepository _baseRepository;
        public JwtManager(IConfiguration configuration, IBaseRepository baseRepository)
        {
            _configuration = configuration;
            _baseRepository = baseRepository;
        }

        public string GenerateJSONWebToken(User userInfo, Role roleInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
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
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async User AuthenticateUser(User login)
        {
            var user = _baseRepository.GetUserByMailAndPassword(login.Email, login.Password);
            return await user;
        }
    }
}

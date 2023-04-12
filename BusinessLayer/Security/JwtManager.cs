using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Security
{
    public class JwtManager : IJwtManager
    {
        private readonly IConfiguration _configuration;
        private readonly IBaseRepository _baseRepository;
        private readonly IUserService _userService;
        public JwtManager(IConfiguration configuration, IBaseRepository baseRepository, IUserService userService)
        {
            _configuration = configuration;
            _baseRepository = baseRepository;
            _userService = userService;
        }

        public async Task<string> GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            if (securityKey.KeySize < 128)
            {
                securityKey = new SymmetricSecurityKey(new byte[16]); // Or some other method to generate a larger key
            }
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
           var roles = await _userService.GetUserRolesByEmail(userInfo.Email);

            var claims = new[]
            {
            new Claim(ClaimTypes.Email, userInfo.Email.ToString()),
            new Claim(ClaimTypes.Role, roles),
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

        public async Task<User> AuthenticateUser(User login)
        {
            var user = await _userService.GetUserByMailAndPassword(login.Email, login.Password);
            return user;
        }
    }
}

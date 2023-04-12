using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.Security;
using DataAccessLayer.Abstract;
using EntityLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Survey.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtManager _jwtManager;
        public AuthController(IUserService userService , IJwtManager jwtManager)
        {
            _userService = userService;
            _jwtManager = jwtManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            IActionResult response = Unauthorized();
         
            var user = await _jwtManager.AuthenticateUser(login);
            if (user != null)
            {
                var tokenString = _jwtManager.GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }




        [HttpGet]
        public async Task<List<User>> GetUsers()
        {
            return await _userService.GetAllUsers();
        }

        [HttpPost]
        public async Task<User> Post([FromBody]User User)
        {
            return await _userService.CreateUser(User);
        }

        [HttpGet("{id}")]
        public async Task<User> GetUserById(int id)
        {
            return await _userService.GetUserById(id);
        }
        [HttpDelete("{id}")]
        public async Task DeleteUserById(int id)
        {
            await _userService.DeleteUserById(id);
        }

    }
}

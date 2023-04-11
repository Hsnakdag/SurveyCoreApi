using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using EntityLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Survey.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
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

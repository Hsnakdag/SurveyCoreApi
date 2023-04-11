using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository _baseRepository;

        public UserService(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<User> CreateUser(User user)
        {
           return await _baseRepository.CreateUser(user);
        }

        public async Task DeleteUserById(int id)
        {
            await _baseRepository.DeleteUserById(id);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _baseRepository.GetAllUsers();
        }

        public async Task<User> GetUserById(int id)
        {
           return await _baseRepository.GetUserById(id);
        }
    }
}

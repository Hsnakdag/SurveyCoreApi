using EntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> CreateUser(User user);
        Task DeleteUserById(int id);
        Task<User> GetUserByMailAndPassword(string email, string password);
        Task<string> GetUserRolesByEmail(string email);
    }
}

using EntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Security
{
    public interface IJwtManager
    {
        Task<User> AuthenticateUser(User login);
        Task<string> GenerateJSONWebToken(User userInfo);
    }
}

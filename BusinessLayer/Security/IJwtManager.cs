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
        string AuthenticateUser(User login);
        string GenerateJSONWebToken(User userInfo, Role roleInfo);
    }
}

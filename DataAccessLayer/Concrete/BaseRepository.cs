using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete
{
    public class BaseRepository : IBaseRepository
    {
        public User CreateUser(User user)
        {
            using (var userDbContext = new UserDbContext())
            {
                userDbContext.Users.Add(user);
                userDbContext.SaveChanges();
                return user;
            }
        }
    }
}

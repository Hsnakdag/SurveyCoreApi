using DataAccessLayer.Abstract;
using DataAccessLayer.Context;
using EntityLayer.Models;
using Microsoft.EntityFrameworkCore;
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
        public async Task<User> CreateUser(User user)
        {
            using (var userDbContext = new UserDbContext())
            {
                userDbContext.Users.Add(user);
                userDbContext.SaveChangesAsync();
                return user;
            }
        }
        public async Task<List<User>> GetAllUsers()
        {
                using(var userDbContext = new UserDbContext())
            {
                return await userDbContext.Users.ToListAsync();
            }
            
        
        }
       public async Task<User> GetUserById(int id)
        {

            using (var userDbContext = new UserDbContext())
            {
                return await userDbContext.Users.FindAsync(id);
            }

        }
       public async Task DeleteUserById(int id)
        {
            using(var userDbContext = new UserDbContext())
            {
                var user = GetUserById(id);
                if (user != null)
                {
                    userDbContext.Users.Remove(await user);
                    await userDbContext.SaveChangesAsync();
                }
            }
        }

     
    }
}

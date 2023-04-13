using BusinessLayer.Abstract;
using BusinessLayer.Context;
using EntityLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
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
            using (var userDbContext = new UserDbContext())
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
            using (var userDbContext = new UserDbContext())
            {
                var user = GetUserById(id);
                if (user != null)
                {
                    userDbContext.Users.Remove(await user);
                    await userDbContext.SaveChangesAsync();
                }
            }
        }
        public async Task<User> GetUserByMailAndPassword(string email, string password)
        {
            using (var userDbContext = new UserDbContext())
            {
                return await userDbContext.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            }

        }
        public async Task<string> GetUserRolesByEmail(string email)
        {
            using (var userDbContext = new UserDbContext())
            {
                var user = await userDbContext.Users.Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return null;
                }

                var role = user.UserRoles.FirstOrDefault()?.Role?.RoleName;
                return role;
            }
        }
        //public async Task<List<User>> GetUserRolesByEmail(string email)
        //{
        //    using(var userDbContext = new UserDbContext())
        //    {
        //        return await userDbContext.Users.Include(u => u.UserRoles)
        //    .ThenInclude(ur => ur.Role)
        //    .Where(u => u.Email == email)
        //    .ToListAsync();
        //    }
        //}

    }
    }

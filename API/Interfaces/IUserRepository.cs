using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<bool> SaveAllAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        void UpdateUser(AppUser user);
    }
}
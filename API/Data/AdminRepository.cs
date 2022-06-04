using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AdminRepository : IAdminRepository
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

    }
}
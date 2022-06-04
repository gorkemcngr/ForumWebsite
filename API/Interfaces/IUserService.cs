using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IUserService
    {
        Task<ActionResult<MemberDto>> GetUser(string username);
        Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto,string userName);
    }
}
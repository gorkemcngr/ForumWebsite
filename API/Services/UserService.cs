using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper=mapper;
        }

        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            AppUser user =await _userRepository.GetUserByUsernameAsync(username);
            return new ObjectResult(_mapper.Map<MemberDto>(user));

        }
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto,string userName)
        {
            var user = await _userRepository.GetUserByUsernameAsync(userName);
            _mapper.Map(memberUpdateDto,user);
            _userRepository.UpdateUser(user);

            if(await _userRepository.SaveAllAsync()) return new NoContentResult();

            return new BadRequestObjectResult("Failded to update the user");
        }
    }
}
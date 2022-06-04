using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly IAdminService _adminService;
        private readonly IPostService _postService;
        public AdminController(IAdminService adminService,IPostService postService)
        {
            _postService = postService;
            _adminService = adminService;
        }
        [Authorize(Policy="RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRoles()
        {
            return await _adminService.GetUsersWithRoles();
        }

        [Authorize(Policy="RequireAdminRole")]
        [HttpGet("users-with-roles/{userName}")]
        public async Task<ActionResult> GetUsersNameWithRoles(string userName)
        {
            return await _adminService.GetUsersNameWithRoles(userName);
        }

        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            return await _adminService.EditRoles(username,roles);
        }

        [HttpPut("post-visibility/{postId}")]
        public async Task<ActionResult> ChangePostVisibility(int postId)
        {
            return await _adminService.ChangePostVisibility(postId);
        }

        [HttpPut("post/{postId}")]
        public async Task<ActionResult> ChangePostPost(ChangePostDto changePostDto,int postId)
        {
            return await _postService.ChangePost(changePostDto,postId);
        }
        [HttpGet("post")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetAllPosts([FromQuery]PostParams postParams)
        {
            return await _postService.GetNonVisiblePosts(postParams);        
        
        }
        [HttpDelete("post/{postId}")]
        public async Task<ActionResult> DeletePost(int postId)
        {
            return await _adminService.DeletePost(postId);
        }
        [HttpPost("category")]
        public async Task<ActionResult<CategoryDto>> AddCategory([FromQuery] string categoryName)
        {
            return await _postService.AddCategory(categoryName);
        }
        [HttpDelete("category/{categoryId}")]
        public async Task<ActionResult> DeleteCategory(int categoryId)
        {
            return await _adminService.DeleteCategory(categoryId);
        }
    }
}
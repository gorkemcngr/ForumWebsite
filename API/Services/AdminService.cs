using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPostRepository _postRepository;
        public AdminService(UserManager<AppUser> userManager,IPostRepository postRepository)
        {
            _userManager = userManager;
            _postRepository = postRepository;
        }

        public async Task<ActionResult> GetUsersWithRoles()
        {
            var users = await _userManager
                .Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .OrderBy(x => x.UserName)
                .Select(x => new   //Çekmek istediğim bilgileri selectle sınırladım.
                {
                    x.Id,
                    Username = x.UserName,
                    Roles = x.UserRoles.Select(x => x.Role.Name).ToList()
                })
                .ToListAsync();

            return new OkObjectResult(users);
        }

        public async Task<ActionResult> GetUsersNameWithRoles(string userName)
        {
            var users = await _userManager
                .Users
                .Where(x => x.UserName.Contains(userName))
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .OrderBy(x => x.UserName)
                .Select(x => new   //Çekmek istediğim bilgileri selectle sınırladım.
                {
                    x.Id,
                    Username = x.UserName,
                    Roles = x.UserRoles.Select(x => x.Role.Name).ToList()
                })
                .ToListAsync();

            return new OkObjectResult(users);
        }

        public async Task<ActionResult> EditRoles(string username, string roles)
        {
            var selectedRoles = roles.Split(",").ToArray();

            var allRoles=new List<String>
            {
                "Member","Moderator","Admin"
            };

            var user = await _userManager.FindByNameAsync(username);

            var UserRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user,UserRoles);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles);

            if(!result.Succeeded) return new BadRequestObjectResult("Problem occurs while editing the role");

            return new OkObjectResult(await _userManager.GetRolesAsync(user));
        }
        public async Task<ActionResult> ChangePostVisibility(int postId)
        {
           var post=await _postRepository.GetPostWithId(postId);
           if(post.PostVisibility==true)
           {
               post.PostVisibility=false;
           }else{
               post.PostVisibility=true;
           }
           if(await _postRepository.SaveAllAsync()) return new NoContentResult();
           return new BadRequestObjectResult("Something went wrong while changing the visibility of post");
        }
        public async Task<ActionResult> DeletePost(int postId)
        {
            var post = await _postRepository.GetPostWithId(postId);
            if(post == null) return new  BadRequestObjectResult("Post doesn't exist");

            int result = _postRepository.DeletePost(post);
            if(result != 1) return new  BadRequestObjectResult("Something went wrong while deleting the post");

            if(await _postRepository.SaveAllAsync()) return new OkResult();

            return new BadRequestObjectResult("Something went wrong while deleting the post");
        }
        public async Task<ActionResult> DeleteCategory(int categoryId)
        {
            var category = await _postRepository.GetCategory(categoryId);
            if(category == null) return new  BadRequestObjectResult("Category doesn't exist");
            var posts = await _postRepository.GetPostsWithCategory(categoryId);
            if(posts.Count>0){
                return new BadRequestObjectResult("There are exist post/posts with that category");
            }
            else{
                 int result = _postRepository.DeleteCategory(category);
            if(result != 1) return new  BadRequestObjectResult("Something went wrong while deleting the category");
            }
           

            

            if(await _postRepository.SaveAllAsync()) return new OkResult();

            return new BadRequestObjectResult("Something went wrong while deleting the category");
        }
    }
}
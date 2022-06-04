using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IAdminService
    {
        Task<ActionResult> GetUsersWithRoles();
        Task<ActionResult> EditRoles(string username,string roles);
        Task<ActionResult> GetUsersNameWithRoles(string userName);
        Task<ActionResult> ChangePostVisibility(int postId);
        Task<ActionResult> DeletePost(int postId);
        Task<ActionResult> DeleteCategory(int categoryId);
    }
}
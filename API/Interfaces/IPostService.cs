using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IPostService
    {
        Task<ActionResult<IEnumerable<PostDto>>> GetAllPosts(PostParams postParams);
        Task<ActionResult<List<Category>>> GetCategories();

        Task<ActionResult<PostDto>> GetPost(int postId);
        Task<ActionResult<IEnumerable<CommentDto>>> GetComments(CommentParams commentParams);
        Task<ActionResult<IEnumerable<PostDto>>> GetUserPosts(string userName);
        Task<ActionResult<CommentDto>> AddComment(CreateCommentDto commentDto,int postId,string userName);
        Task<ActionResult<PostDto>> AddPost(PostDto postDto,string userName);
        Task<ActionResult<IEnumerable<PostDto>>> GetPostsWithCategory(int categoryId);
        Task<ActionResult> AddCommentLike(string username,int commentId);
        Task<ActionResult<IEnumerable<CommentDto>>> GetCommentLikesWithUserName(int userId);
        Task<ActionResult<IEnumerable<MemberDto>>> GetCommentLikesWithCommentId(int commentId);
        Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsUserLikes(CommentParams commentParams);
        Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsWithUserName(CommentParams commentParams);
        Task<ActionResult> ChangePost(ChangePostDto changePostDto,int postId);
        Task<ActionResult<IEnumerable<PostDto>>> GetNonVisiblePosts(PostParams postParams);
        Task<ActionResult<IEnumerable<PostDto>>> GetCurrentUserNonVisiblePosts(PostParams postParams);
        Task<ActionResult<IEnumerable<PostDto>>> GetUsersPost(PostParams postParams);
        Task<ActionResult<CategoryDto>> AddCategory(string categoryName);
    }
}
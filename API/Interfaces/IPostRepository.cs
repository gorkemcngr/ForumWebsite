using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IPostRepository
    {
        Task<PagedList<PostDto>> GetAllPosts(PostParams postParams);
        int DeletePost(Post post);
        Task<PagedList<PostDto>> GetNonVisiblePosts(PostParams postParams);
        Task<bool> SaveAllAsync();
        Task<Post> GetPostWithId(int id);
        Task<List<Category>> GetCategories();
        Task<Category> GetCategory(int id);
        Task<List<Post>> GetPostsWithCategory(int categoryId);
        Task<CommentLike> GetCommentLike(int userId,int commentId);
        Task<Comment> GetCommentWithId(int commentId);
        Task<List<Comment>> getCommentLikesWithUserId(int userId);
        Task<List<AppUser>> getCommentLikesWithCommentId(int commentId);
        int DeleteCommentLike(CommentLike commentLike);
        Task<PagedList<CommentDto>> GetCommentsUserLikes(CommentParams commentParams);
        Task<PagedList<CommentDto>> GetCommentsWithUserName(CommentParams commentParams);
        Task<PagedList<CommentDto>> GetCommentsWithPostId(CommentParams commentParams);
        Task<PagedList<PostDto>> GetCurrentUserNonVisiblePosts(PostParams postParams);
        Task<PagedList<PostDto>> GetUsersPost(PostParams postParams);
        Task<Category> AddCategory(string categoryName);
        int DeleteCategory(Category category);
    }
}
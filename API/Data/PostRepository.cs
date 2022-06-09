using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PostRepository(DataContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PagedList<PostDto>> GetAllPosts(PostParams postParams)
        {
            var query =  _context.Post.AsQueryable();
            if(postParams.CategoryId!=null)
            {
                query=query.Where(x => x.CategoryId==postParams.CategoryId);
            }
            if(postParams.PostTitle !=null)
            {
                query = query.Where(x => x.Title.ToLower().Contains(postParams.PostTitle.ToLower()));
            }
            query=query.Where(x => x.PostVisibility==true); //only get the visible posts
            query=query.OrderByDescending(x => x.Created);

            return await PagedList<PostDto>.CreateAsync(query.ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .AsNoTracking(),postParams.PageNumber, postParams.PageSize);
            /*return await _context.Post
                .Include(p => p.Comment)
                .Include(p => p.AppUser)
                .Include(p => p.Category)
                .ToListAsync(); */
        }
        public async Task<PagedList<PostDto>> GetUsersPost(PostParams postParams)
        {
            var query =  _context.Post.AsQueryable();
            if(postParams.CategoryId!=null)
            {
                query=query.Where(x => x.CategoryId==postParams.CategoryId);
            }
            query=query.Where(x => x.PostVisibility==true); //only get the visible posts
            query=query.Where(x => x.AppUserId==postParams.userId); 
            query=query.OrderByDescending(x => x.Created);

            return await PagedList<PostDto>.CreateAsync(query.ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .AsNoTracking(),postParams.PageNumber, postParams.PageSize);
            /*return await _context.Post
                .Include(p => p.Comment)
                .Include(p => p.AppUser)
                .Include(p => p.Category)
                .ToListAsync(); */
        }

        public async Task<PagedList<PostDto>> GetNonVisiblePosts(PostParams postParams)
        {
            var query =  _context.Post.AsQueryable();
            if(postParams.CategoryId!=null)
            {
                query=query.Where(x => x.CategoryId==postParams.CategoryId);
            }
            query=query.Where(x => x.PostVisibility==false); //only get the visible posts
            query=query.OrderByDescending(x => x.Created);

            return await PagedList<PostDto>.CreateAsync(query.ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .AsNoTracking(),postParams.PageNumber, postParams.PageSize);
            /*return await _context.Post
                .Include(p => p.Comment)
                .Include(p => p.AppUser)
                .Include(p => p.Category)
                .ToListAsync(); */
        }
        public async Task<PagedList<PostDto>> GetCurrentUserNonVisiblePosts(PostParams postParams)
        {
            var query =  _context.Post.AsQueryable();
            if(postParams.CategoryId!=null)
            {
                query=query.Where(x => x.CategoryId==postParams.CategoryId);
            }
            query=query.Where(x => x.AppUserId==postParams.userId);
            query=query.Where(x => x.PostVisibility==false); //only get the visible posts
            query=query.OrderByDescending(x => x.Created);

            return await PagedList<PostDto>.CreateAsync(query.ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .AsNoTracking(),postParams.PageNumber, postParams.PageSize);
            /*return await _context.Post
                .Include(p => p.Comment)
                .Include(p => p.AppUser)
                .Include(p => p.Category)
                .ToListAsync(); */
        }
        public async Task<List<Post>> GetPostsWithCategory(int categoryId)
        {
          var query= _context.Post
                .Include(p => p.Comment)
                .Include(p => p.AppUser)
                .Include(p => p.Category)
                .AsQueryable();

          query = query.Where(p => p.CategoryId == categoryId);

          return query.ToList();
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _context.Category.OrderBy(x => x.Id).ToListAsync();
        }
        public async Task<Category> GetCategory(int id)
        {
            return await _context.Category.FindAsync(id);
        }
        public async Task<Category> AddCategory(string categoryName)
        {
            var result = await _context.Category.AddAsync(new Category{
                CategoryName=categoryName
            });
            return  result.Entity;
        }

        public async Task<PagedList<CommentDto>> GetCommentsWithPostId(CommentParams commentParams)
        {
            var query = _context.Comment.Include(c => c.AppUser).ThenInclude(u => u.Photos).AsQueryable();

            query = query.Where(x => x.PostId==commentParams.PostId);

            return await PagedList<CommentDto>.CreateAsync(query.ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                .AsNoTracking(),commentParams.PageNumber, commentParams.PageSize); 
        }

        public async Task<Post> GetPostWithId(int id)
        {
            return await _context.Post
                .Include(p => p.Comment)
                .Include(p => p.Category)
                .Include(p => p.AppUser).ThenInclude(u => u.Photos)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<CommentLike> GetCommentLike(int userId,int commentId)
        {
            return await _context.CommentLike.Include(c =>c.User).FirstOrDefaultAsync(c => c.CommentId == commentId && c.UserId==userId);
        }
        public async Task<Comment> GetCommentWithId(int commentId)
        {
            return await _context.Comment.FirstOrDefaultAsync(c => c.Id ==commentId);
        }
        public async Task<List<Comment>> getCommentLikesWithUserId(int userId)
        {
           return await _context.CommentLike.Include(x => x.User).Where(c => c.UserId==userId).Select(x => x.Comment).ToListAsync();
           
        }
        public async Task<List<AppUser>> getCommentLikesWithCommentId(int commentId)
        {
            var users = await _context.CommentLike.Include(x => x.User).ThenInclude(x => x.Photos).Where(c => c.CommentId==commentId).Select(c=> c.User).ToListAsync();
            return users;
        }
        public int DeleteCommentLike(CommentLike commentLike)
        {
            return  _context.CommentLike.Remove(commentLike) !=null ? 1 : 0;
        }
        public int DeletePost(Post post)
        {
            return  _context.Post.Remove(post) !=null ? 1 : 0;
        }
        public int DeleteComment(Comment comment)
        {
            return  _context.Comment.Remove(comment) !=null ? 1 : 0;
        }
        public int DeleteCategory(Category category)
        {
            return  _context.Category.Remove(category) !=null ? 1 : 0;
        }
        public async Task<PagedList<CommentDto>> GetCommentsUserLikes(CommentParams commentParams)
        {
            var query = _context.CommentLike.Include(x => x.Comment).ThenInclude(x => x.AppUser).ThenInclude(x => x.Photos).AsQueryable();

            var query2 = query.Where(x => x.UserId==commentParams.UserId).Select(x => x.Comment);

            return await PagedList<CommentDto>.CreateAsync(query2.ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                .AsNoTracking(),commentParams.PageNumber, commentParams.PageSize); 
        }
        public async Task<PagedList<CommentDto>> GetCommentsWithUserName(CommentParams commentParams)
        {
            var query = _context.Comment.Include(x => x.AppUser).ThenInclude(x => x.Photos).AsQueryable();

            query = query.Where(x => x.UserName == commentParams.UserName);

            return await PagedList<CommentDto>.CreateAsync(query.ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                .AsNoTracking(),commentParams.PageNumber, commentParams.PageSize); 

        }
        public async Task<List<Comment>> GetCommentWithUserIdWitPostId(int userId,int postId)
        {
            return await _context.Comment.Where(x => x.AppUserId == userId && x.PostId==postId).ToListAsync();
        }
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
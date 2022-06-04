using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository,IMapper mapper,IUserRepository userRepository,IHttpContextAccessor httpContextAccessor)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _userRepository= userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ActionResult<IEnumerable<PostDto>>> GetAllPosts(PostParams postParams)
        {
            var posts = await _postRepository.GetAllPosts(postParams);
            _httpContextAccessor.HttpContext.Response.AddPaginationHeader(posts.CurrentPage,posts.PageSize,posts.TotalCount,posts.TotalPages);
            return new OkObjectResult(posts);
        }

        public async Task<ActionResult<IEnumerable<PostDto>>> GetNonVisiblePosts(PostParams postParams)
        {
            var posts = await _postRepository.GetNonVisiblePosts(postParams);
            _httpContextAccessor.HttpContext.Response.AddPaginationHeader(posts.CurrentPage,posts.PageSize,posts.TotalCount,posts.TotalPages);
            return new OkObjectResult(posts);
        }
        public async Task<ActionResult<IEnumerable<PostDto>>> GetUsersPost(PostParams postParams)
        {
            var posts = await _postRepository.GetUsersPost(postParams);
            _httpContextAccessor.HttpContext.Response.AddPaginationHeader(posts.CurrentPage,posts.PageSize,posts.TotalCount,posts.TotalPages);
            return new OkObjectResult(posts);
        }
        public async Task<ActionResult<IEnumerable<PostDto>>> GetCurrentUserNonVisiblePosts(PostParams postParams)
        {
            var posts = await _postRepository.GetCurrentUserNonVisiblePosts(postParams);
            _httpContextAccessor.HttpContext.Response.AddPaginationHeader(posts.CurrentPage,posts.PageSize,posts.TotalCount,posts.TotalPages);
            return new OkObjectResult(posts);
        }

        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            var categories =await _postRepository.GetCategories();
            
            return categories;
        }
        public async Task<ActionResult<CategoryDto>> AddCategory(string categoryName)
        {
           var category= await _postRepository.AddCategory(categoryName);
           if(await _postRepository.SaveAllAsync()) return  _mapper.Map<CategoryDto>(category);
           return new BadRequestObjectResult("Something went wrong while adding the category");
        }

        public async Task<ActionResult<PostDto>> GetPost(int postId)
        {
            var post =await _postRepository.GetPostWithId(postId);
            return new ObjectResult(_mapper.Map<PostDto>(post)){StatusCode = 200};
        }

        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments(CommentParams commentParams)
        {
            var comments=await _postRepository.GetCommentsWithPostId(commentParams);
            _httpContextAccessor.HttpContext.Response.AddPaginationHeader(comments.CurrentPage,comments.PageSize,comments.TotalCount,comments.TotalPages);
            
            return new ObjectResult(_mapper.Map<List<CommentDto>>(comments)){StatusCode = 200};
        }

        public async Task<ActionResult<IEnumerable<PostDto>>> GetUserPosts(string userName)
        {
             var user =await _userRepository.GetUserByUsernameAsync(userName);

            IEnumerable<Post> posts = user.Post.ToList();
            
            return new ObjectResult(_mapper.Map<List<PostDto>>(posts));
        }

        public async Task<ActionResult<CommentDto>> AddComment(CreateCommentDto commentDto,int postId,string userName)
        {
           
            var user =await _userRepository.GetUserByUsernameAsync(userName);
            var post = await _postRepository.GetPostWithId(postId);
           var comment=new Comment(){
                Post = post,
                Answer = commentDto.Answer,
                PostId=postId,
                UserName=userName,
                AppUserId=user.Id,
                AppUser=user
           };
            post.Comment.Add(comment);
            commentDto.UserName=userName;
             await _postRepository.SaveAllAsync();
             return new ObjectResult(_mapper.Map<CommentDto>(comment));
        }

        public async Task<ActionResult<PostDto>> AddPost(PostDto postDto,string userName)
        {
           
            var user =await _userRepository.GetUserByUsernameAsync(userName);
            var category = await _postRepository.GetCategory(postDto.CategoryId.Value);
            var post = new Post(){
                Title=postDto.Title,
                 Content=postDto.Content,
                 PostVisibility=false
             };
            

            user.Post.Add(post);
            post.Category=category;
            await _userRepository.SaveAllAsync();
            int id = post.Id;
             postDto.UserName=userName;
             postDto.Id=id;
             return new ObjectResult(postDto);
        }

        public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsWithCategory(int categoryId)
        {
            var posts= await _postRepository.GetPostsWithCategory(categoryId);

            return new ObjectResult(_mapper.Map<List<PostDto>>(posts));
        }

        public async Task<ActionResult> AddCommentLike(string userName,int commentId)
        {
            var user =await _userRepository.GetUserByUsernameAsync(userName);

            if(user == null) return new NotFoundResult();

            CommentLike commentLike = await _postRepository.GetCommentLike(user.Id,commentId);

            if(commentLike != null) {
                int result=_postRepository.DeleteCommentLike(commentLike);
                if(result==1) if(await _userRepository.SaveAllAsync()) return new OkResult();
                return new BadRequestObjectResult("Failed to unlike comment");
            }

            var comment=await _postRepository.GetCommentWithId(commentId);

            commentLike = new CommentLike
            {
                UserId= user.Id,
                User=user,
                CommentId=commentId,
                Comment=comment
            };
            user.CommentLike.Add(commentLike);
            Console.WriteLine(commentLike);

            if(await _userRepository.SaveAllAsync()) return new OkResult();

            return new BadRequestObjectResult("Failed to like comment");
        }

        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentLikesWithUserName(int userId)
        {
            
            var commentLikes=await _postRepository.getCommentLikesWithUserId(userId);
            

            return new OkObjectResult(_mapper.Map<List<CommentDto>>(commentLikes));
        }

        public async Task<ActionResult<IEnumerable<MemberDto>>> GetCommentLikesWithCommentId(int commentId)
        {
            
            var users=await _postRepository.getCommentLikesWithCommentId(commentId);
            

            return new OkObjectResult(_mapper.Map<List<MemberDto>>(users));
        }
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsUserLikes(CommentParams commentParams)
        {
            var comments=await _postRepository.GetCommentsUserLikes(commentParams);
            _httpContextAccessor.HttpContext.Response.AddPaginationHeader(comments.CurrentPage,comments.PageSize,comments.TotalCount,comments.TotalPages);

           return _mapper.Map<CommentDto[]>(comments);
        }
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsWithUserName(CommentParams commentParams)
        {
            var comments = await _postRepository.GetCommentsWithUserName(commentParams);
            _httpContextAccessor.HttpContext.Response.AddPaginationHeader(comments.CurrentPage,comments.PageSize,comments.TotalCount,comments.TotalPages);
            return _mapper.Map<CommentDto[]>(comments);
        }

        public async Task<ActionResult> ChangePost(ChangePostDto changePostDto,int postId)
        {
            var post=await _postRepository.GetPostWithId(postId);
            var category=await _postRepository.GetCategory(changePostDto.CategoryId);
            post.Title=changePostDto.Title;
            post.Content=changePostDto.Content;
            post.CategoryId=changePostDto.CategoryId;
            post.Category = category;
            if(await _postRepository.SaveAllAsync()) return new NoContentResult();

            return new BadRequestObjectResult("Something went wrong while changing the post");
            
        }
        
    } 
}
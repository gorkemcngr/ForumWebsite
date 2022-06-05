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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class PostController : BaseApiController
    {
       
        private readonly IUserRepository __userRepository;
        private readonly IMapper __mapper;
        private readonly IPostRepository __postRepository;
        private readonly IPostService _postService;
        

        public PostController(IUserRepository userRepository,IMapper mapper, IPostRepository postRepository,IPostService postService)
        {
            __postRepository = postRepository;
            __mapper = mapper;
            __userRepository = userRepository;
            _postService = postService;
            
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PostDto>> AddPost(PostDto postDto)
        {
            string userName = User.GetUserName();
            return await _postService.AddPost(postDto,userName);
        }

        [Authorize]
        [HttpPost("{PostId}")]
        public async Task<ActionResult<CommentDto>> AddComment([FromBody]CreateCommentDto commentDto,[FromRoute]int postId)
        {
            string userName = User.GetUserName();
            return await _postService.AddComment(commentDto,postId,userName);
        }
        
        
    
        [HttpGet("comments/{postIdOrUsername}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments([FromQuery]CommentParams commentParams,[FromRoute]string postIdOrUsername)
        {
            int a;
            if(int.TryParse(postIdOrUsername, out a))
            {
                commentParams.PostId=Int32.Parse(postIdOrUsername);
                return await _postService.GetComments(commentParams);
            }
            commentParams.UserName=postIdOrUsername;
            return await _postService.GetCommentsWithUserName(commentParams);
            
            
        }
        [HttpGet("{postId}")]
        public async Task<ActionResult<PostDto>> GetPost([FromRoute]int postId)
        {
            return  await _postService.GetPost(postId);
        }

        [HttpGet("Categories")]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            var categories =await _postService.GetCategories();
            
            return categories;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetAllPosts([FromQuery]PostParams postParams)
        {
            return await _postService.GetAllPosts(postParams);        
        
        }
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetUserPosts([FromQuery]PostParams postParams,[FromRoute]int userId)
        {
            postParams.userId=userId;
           return await _postService.GetUsersPost(postParams);
        }
        [HttpGet("pending-post")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetCurrentUserNonVisiblePosts([FromQuery]PostParams postParams)
        {
            postParams.userId=User.GetUserId();
            return await _postService.GetCurrentUserNonVisiblePosts(postParams);        
        
        }
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetPostsWithCategory(int categoryId)
        {
            return await _postService.GetPostsWithCategory(categoryId);
        }

        [HttpPost("commentLike/{commentId}")]
        public async Task<ActionResult> AddCommentLike(int commentId)
        {
            string userName =  User.GetUserName();
            return await _postService.AddCommentLike(userName,commentId);
        }
        [HttpGet("commentLike")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentLikesWithUserId()
        {
            int userId = User.GetUserId();

            return await _postService.GetCommentLikesWithUserName(userId);
        }
        [HttpGet("commentLike/{commentId}")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetCommentLikesWithCommentId(int commentId)
        {
            return await _postService.GetCommentLikesWithCommentId(commentId);
        }
        [HttpGet("commentUser/{userId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsUserLikes([FromQuery]CommentParams commentParams,[FromRoute]int userId)
        {
            commentParams.UserId=userId;
            return await _postService.GetCommentsUserLikes(commentParams);
        }
        [HttpPut("{postId}")]
        public async Task<ActionResult> ChangePostPost(ChangePostDto changePostDto,int postId)
        {
            return await _postService.ChangePost(changePostDto,postId);
        }
        [Authorize]
        [HttpGet("user-comment/{postId}")]
        public async Task<ActionResult<List<CommentDto>>> GetCommentWithUserIdWitPostId(int postId)
        {
            return await _postService.GetCommentWithUserIdWitPostId(User.GetUserId(),postId);
        }
        [HttpDelete("comment/{commentId}")]
        public async Task<ActionResult> DeleteComment(int commentId)
        {
            return await _postService.DeleteComment(commentId);
        }
        
    }
}
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
    public class MessageService : IMessageService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MessageService(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper,IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto,string username)
        {
            

            if(username == createMessageDto.RecipientUsername.ToLower())
                return new BadRequestObjectResult("You cannot sen messages to your self");

            var sender =await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if(recipient == null) return new NotFoundResult();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content
            };

            _messageRepository.AddMessage(message);

            if(await _messageRepository.SaveAllAsync()) return new OkObjectResult(_mapper.Map<MessageDto>(message));

            return new BadRequestObjectResult("Failed to send message");

        }


        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser(MessageParams messageParams)
        {

            var messages = await _messageRepository.GetMessagesForUser(messageParams);

            _httpContextAccessor.HttpContext.Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);

            return messages;
        }

        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username,string currentUsername)
        {
            return new OkObjectResult(await _messageRepository.GetMessageThread(currentUsername,username));
        }

         public async Task<ActionResult> DeleteMessage(int id, string currentUsername)
        {

            var message =await _messageRepository.GetMessage(id);

            if(message.Sender.UserName != currentUsername && message.Recipient.UserName != currentUsername) return new UnauthorizedResult();

            if(message.Sender.UserName == currentUsername) message.SenderDeleted = true;

            if(message.Recipient.UserName == currentUsername) message.RecipientDeleted = true;

            if(message.SenderDeleted && message.RecipientDeleted) _messageRepository.DeleteMessage(message);

            if(await _messageRepository.SaveAllAsync()) return new OkResult();

            return new BadRequestObjectResult("Problem occurred, Message could not be deleted");
        }
    }
}
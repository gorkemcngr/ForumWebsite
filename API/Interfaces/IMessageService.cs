using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IMessageService
    {
        Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto,string username);
        Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser(MessageParams messageParams);
        Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username,string currentUsername);
        Task<ActionResult> DeleteMessage(int id, string username);
    }
}
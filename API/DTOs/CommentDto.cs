using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string? Answer { get; set; }
        public string? UserName { get; set; }
        public DateTime Created { get; set; } 
        public string? PhotoUrl { get; set; }
    }
}
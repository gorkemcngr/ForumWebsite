using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class CreateCommentDto
    {
        public string Answer { get; set; }
        public string? UserName { get; set; }
    }
}
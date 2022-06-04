using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class CommentParams : PaginationParams
    {
        public int? PostId { get; set; }
        public string? UserName { get; set; }
        public int? UserId { get; set; }
    }
}
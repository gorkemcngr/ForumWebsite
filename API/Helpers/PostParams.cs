using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class PostParams : PaginationParams
    {
        public int? CategoryId { get; set; }
        public int? userId { get; set; }
        public string? PostTitle { get; set; }
    }
}
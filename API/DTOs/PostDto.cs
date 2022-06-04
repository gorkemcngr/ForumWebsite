using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class PostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? UserName { get; set; }
        public int? Id { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime Created { get; set; } 
        public string? PhotoUrl { get; set; }
        public int MyProperty { get; set; }
        public bool PostVisibility { get; set; }
    }
}
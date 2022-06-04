using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public ICollection<Comment>? Comment { get; set; }
        public AppUser? AppUser { get; set; }
        public int AppUserId { get; set; }
        public Category? Category { get; set; }
        public int? CategoryId { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public bool PostVisibility { get; set; }
    }
}
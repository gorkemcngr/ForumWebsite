using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public ICollection<Post>? Post { get; set; }
    }
}
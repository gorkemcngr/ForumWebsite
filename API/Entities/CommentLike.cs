using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class CommentLike
    {
         public AppUser? User { get; set; }
        public int  UserId { get; set; }
        public Comment? Comment { get; set; }  
        public int CommentId { get; set; }
    }
}
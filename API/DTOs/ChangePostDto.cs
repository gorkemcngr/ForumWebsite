using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ChangePostDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int CategoryId { get; set; }
    }
}
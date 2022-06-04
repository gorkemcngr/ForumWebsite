namespace API.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public Post? Post { get; set; }
        public AppUser? AppUser { get; set; }
        public int AppUserId { get; set; }
        public int PostId { get; set; }
        public string? Answer { get; set; }
        public string UserName { get; set; }
        public ICollection<CommentLike>? CommentLike { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

    }
}
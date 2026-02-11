namespace postSystem.Models.Entities
{
    public class PostLike
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string UserFingerprint { get; set; }
    }
}

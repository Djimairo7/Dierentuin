namespace Dierentuin.DTO
{
    public class FeedingTimeDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Species { get; set; }
        public required string FeedingTime { get; set; }
    }
}
namespace Dierentuin.DTO
{
    public class SleepStateDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Species { get; set; }
        public required string SleepState { get; set; }
    }
}
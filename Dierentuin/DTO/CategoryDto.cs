namespace Dierentuin.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<AnimalDto>? Animals { get; set; }
    }
}
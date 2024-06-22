namespace Dierentuin.DTO
{
    public class EnclosureDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<AnimalDto>? Animals { get; set; }
        public required string Climate { get; set; }
        public required string Habitat { get; set; }
        public required string Security { get; set; }
        public required double Size { get; set; }
    }
}

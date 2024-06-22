namespace Dierentuin.DTO
{
    public class AnimalDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Species { get; set; }
        public string? Category { get; set; }
        public int? CategoryId { get; set; }
        public required string Size { get; set; }
        public required string Diet { get; set; }
        public required string Activity { get; set; }
        public required string Prey { get; set; }
        public string? Enclosure { get; set; }
        public int? EnclosureId { get; set; }
        public required double Space { get; set; }
        public required string Security { get; set; }
    }
}

namespace Dierentuin.Models;

public class Enclosure
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Animal> Animals { get; set; } = new List<Animal>();
    public enum ClimateType
    {
        Tropical,
        Temperate,
        Arctic
    }
    public required ClimateType Climate { get; set; }
    public enum HabitatType
    {
        Forest,
        Aquatic,
        Desert,
        Grassland
    }
    [Flags]
    public enum HabitatTypes
    {
        Forest = 1,
        Aquatic = 2,
        Desert = 4,
        Grassland = 8
    }
    public required HabitatTypes Habitat { get; set; }
    public enum SecurityLevel
    {
        Low,
        Medium,
        High
    }
    public required SecurityLevel Security { get; set; }
    public required double Size { get; set; }

}
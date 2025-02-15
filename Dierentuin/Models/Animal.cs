namespace Dierentuin.Models;

public class Animal
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Species { get; set; }
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public enum AnimalSize
    {
        Microscopic,
        VerySmall,
        Small,
        Medium,
        Large,
        VeryLarge
    }
    public required AnimalSize Size { get; set; }
    public enum DietaryClass
    {
        Carnivore,
        Herbivore,
        Omnivore,
        Insectivore,
        Piscivore
    }
    public required DietaryClass Diet { get; set; }
    public enum ActivityPattern
    {
        Diurnal,
        Nocturnal,
        Cathemeral
    }
    public required ActivityPattern Activity { get; set; }
    public required string Prey { get; set; }
    public Enclosure? Enclosure { get; set; } // Navigation property
    public int? EnclosureId { get; set; } // Foreign key
    public required double Space { get; set; }
    public enum SecurityLevel
    {
        Low,
        Medium,
        High
    }
    public required SecurityLevel Security { get; set; }
}
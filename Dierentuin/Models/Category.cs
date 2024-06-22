namespace Dierentuin.Models;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<Animal> Animals { get; set; } = new List<Animal>();
}
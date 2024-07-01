using Microsoft.EntityFrameworkCore;
using Dierentuin.Models;
using Bogus;

namespace Dierentuin.Data
{
    // This static class is responsible for seeding the database with initial data
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            // Create a new DierentuinContext using the service provider
            using (var context = new DierentuinContext(
                serviceProvider.GetRequiredService<DbContextOptions<DierentuinContext>>()))
            {
                // Seed Categories if the table is empty
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Category { Name = "Mammals" },
                        new Category { Name = "Reptiles" },
                        new Category { Name = "Birds" },
                        new Category { Name = "Amphibians" },
                        new Category { Name = "Fish" },
                        new Category { Name = "Invertebrates" }
                    );

                    context.SaveChanges();
                }

                // Seed Enclosures if the table is empty
                if (!context.Enclosures.Any())
                {
                    // Use Bogus to create a faker for generating random enclosure data
                    var enclosureFaker = new Faker<Enclosure>()
                        .RuleFor(e => e.Name, f => f.Address.City())
                        .RuleFor(e => e.Climate, f => f.PickRandom<Enclosure.ClimateTypes>())
                        .RuleFor(e => e.Habitat, f => f.PickRandom<Enclosure.HabitatTypes>())
                        .RuleFor(e => e.Security, f => f.PickRandom<Enclosure.SecurityLevel>())
                        .RuleFor(e => e.Size, f => f.Random.Int(5, 500));

                    // Generate 10 fake enclosures
                    var fakeEnclosures = enclosureFaker.Generate(10);

                    context.Enclosures.AddRange(fakeEnclosures);
                    context.SaveChanges();
                }

                // Seed Animals if the table is empty
                if (!context.Animals.Any())
                {
                    // Define arrays for species and prey
                    string[] species = ["Dog", "Cat", "Lion", "Elephant", "Horse", "Snake", "Wolf", "Bear", "Turtle", "Red Panda", "Giraffe", "Koala", "Shark"];
                    string[] prey = ["Mouse", "Deer", "Horse", "Rabbit", "Boar", "Zebra", "Sheep", "Buffalo"];

                    // Get existing categories and enclosures from the database
                    var categories = context.Categories.ToList();
                    var enclosures = context.Enclosures.ToList();

                    // Use Bogus to create a faker for generating random animal data
                    var animalFaker = new Faker<Animal>()
                        .RuleFor(a => a.Name, f => f.Name.FirstName())
                        .RuleFor(a => a.Species, f => f.PickRandom(species))
                        .RuleFor(a => a.Size, f => f.PickRandom<Animal.AnimalSize>())
                        .RuleFor(a => a.Diet, f => f.PickRandom<Animal.DietaryClass>())
                        .RuleFor(a => a.Activity, f => f.PickRandom<Animal.ActivityPattern>())
                        .RuleFor(a => a.Prey, f => f.PickRandom(prey))
                        .RuleFor(a => a.Enclosure, f => f.PickRandom(enclosures))
                        .RuleFor(a => a.EnclosureId, (f, a) => a.Enclosure?.Id)
                        .RuleFor(a => a.Space, f => f.Random.Int(5, 30))
                        .RuleFor(a => a.Security, f => f.PickRandom<Animal.SecurityLevel>())
                        .RuleFor(a => a.Category, f => f.PickRandom(categories));

                    // Generate 30 fake animals
                    var fakeAnimals = animalFaker.Generate(30);

                    context.Animals.AddRange(fakeAnimals);
                    context.SaveChanges();
                }
            }
        }
    }
}
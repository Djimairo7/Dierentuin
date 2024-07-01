using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Dierentuin.Data; // Add this directive
using Dierentuin.Models;
using Bogus;

namespace Dierentuin.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new DierentuinContext(
                serviceProvider.GetRequiredService<DbContextOptions<DierentuinContext>>()))
            {
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

                if (!context.Enclosures.Any())
                {
                    var enclosureFaker = new Faker<Enclosure>()
                    .RuleFor(e => e.Name, f => f.Address.City())
                    .RuleFor(e => e.Climate, f => f.PickRandom<Enclosure.ClimateTypes>())
                    .RuleFor(e => e.Habitat, f => f.PickRandom<Enclosure.HabitatTypes>())
                    .RuleFor(e => e.Security, f => f.PickRandom<Enclosure.SecurityLevel>())
                    .RuleFor(e => e.Size, f => f.Random.Int(5, 500));

                    var fakeEnclosures = enclosureFaker.Generate(10);

                    context.Enclosures.AddRange(fakeEnclosures);
                    context.SaveChanges();
                }

                if (!context.Animals.Any())
                {
                    string[] species = ["Dog", "Cat", "Lion", "Elephant", "Horse", "Snake", "Wolf", "Bear", "Turtle", "Red Panda", "Giraffe", "Koala", "Shark"];
                    string[] prey = ["Mouse", "Deer", "Horse", "Rabbit", "Boar", "Zebra", "Sheep", "Buffalo"];

                    var categories = context.Categories.ToList();
                    var enclosures = context.Enclosures.ToList();

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

                    var fakeAnimals = animalFaker.Generate(30); // Generate 30 fake animals

                    context.Animals.AddRange(fakeAnimals);
                    context.SaveChanges();
                }
            }
        }
    }
}

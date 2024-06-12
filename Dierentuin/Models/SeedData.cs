using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Dierentuin.Data; // Add this directive
using Dierentuin.Models;

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
                    context.Enclosures.AddRange(
                        new Enclosure
                        {
                            Name = "Savannah Exhibit",
                            Climate = Enclosure.ClimateType.Temperate,
                            Habitat = Enclosure.HabitatTypes.Grassland,
                            Security = Enclosure.SecurityLevel.Medium,
                            Size = 200.0,
                        },
                        new Enclosure
                        {
                            Name = "Tropical Rainforest",
                            Climate = Enclosure.ClimateType.Tropical,
                            Habitat = Enclosure.HabitatTypes.Forest,
                            Security = Enclosure.SecurityLevel.High,
                            Size = 300.0,
                        }
                    );

                    context.SaveChanges();
                }

                if (!context.Animals.Any())
                {
                    var mammals = context.Categories.First(c => c.Name == "Mammals");
                    var savannahExhibit = context.Enclosures.First(e => e.Name == "Savannah Exhibit");

                    context.Animals.AddRange(
                        new Animal
                        {
                            Name = "Harry",
                            Species = "Dog",
                            Size = Animal.AnimalSize.Medium,
                            Diet = Animal.DietaryClass.Carnivore,
                            Activity = Animal.ActivityPattern.Diurnal,
                            Prey = "None",
                            Enclosure = savannahExhibit, // Assign Enclosure
                            EnclosureId = savannahExhibit.Id, // Assign EnclosureId
                            Space = 10,
                            Security = Animal.SecurityLevel.Low,
                            Category = mammals
                        }
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}

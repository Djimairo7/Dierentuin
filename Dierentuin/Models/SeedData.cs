using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Dierentuin.Data;
using System;
using System.Linq;

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

                if (!context.Animals.Any())
                {
                    var mammals = context.Categories.First(c => c.Name == "Mammals");

                    context.Animals.AddRange(
                        new Animal
                        {
                            Name = "Harry",
                            Species = "Dog",
                            Size = Animal.AnimalSize.Medium,
                            Diet = Animal.DietaryClass.Carnivore,
                            Activity = Animal.ActivityPattern.Diurnal,
                            Prey = "None",
                            Enclosure = "1",
                            Space = 10,
                            Security = Animal.SecurityLevel.Low
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}

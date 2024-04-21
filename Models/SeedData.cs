using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Dierentuin.Data;
using System;
using System.Linq;

namespace Dierentuin.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new DierentuinContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<DierentuinContext>>()))
        {
            // Look for any movies.
            if (context.Animals.Any())
            {
                return;   // DB has been seeded
            }
            context.Animals.AddRange(
                new Animal
                {
                    Name = "Harry",
                    Species = "Dog",
                    Category = "???",
                    Size = Animal.AnimalSize.Medium,
                    Diet = Animal.DietaryClass.Carnivore,
                    Activity = Animal.ActivityPattern.Diurnal,
                    Prey = "None",
                    Enclosure = "1",
                    Space = 10,
                    Security = Animal.SecurityLevel.Low
                },
                new Animal
                {
                    Name = "Harry",
                    Species = "Dog",
                    Category = "???",
                    Size = Animal.AnimalSize.Medium,
                    Diet = Animal.DietaryClass.Carnivore,
                    Activity = Animal.ActivityPattern.Diurnal,
                    Prey = "None",
                    Enclosure = "1",
                    Space = 10,
                    Security = Animal.SecurityLevel.Low
                },
                new Animal
                {
                    Name = "Harry",
                    Species = "Dog",
                    Category = "???",
                    Size = Animal.AnimalSize.Medium,
                    Diet = Animal.DietaryClass.Carnivore,
                    Activity = Animal.ActivityPattern.Diurnal,
                    Prey = "None",
                    Enclosure = "1",
                    Space = 10,
                    Security = Animal.SecurityLevel.Low
                },
                new Animal
                {
                    Name = "Harry",
                    Species = "Dog",
                    Category = "???",
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
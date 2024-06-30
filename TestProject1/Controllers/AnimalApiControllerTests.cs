using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Dierentuin.API;
using Dierentuin.Data;
using Dierentuin.Models;
using Dierentuin.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dierentuin.Tests
{
    public class AnimalApiControllerTests
    {
        private readonly DbContextOptions<DierentuinContext> _dbContextOptions;

        public AnimalApiControllerTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DierentuinContext>()
                .UseInMemoryDatabase(databaseName: "DierentuinTestDb")
                .Options;
        }

        private void SeedDatabase(DierentuinContext context)
        {
            context.Animals.AddRange(
                new Animal
                {
                    Id = 1,
                    Name = "Lion",
                    Species = "Panthera leo",
                    CategoryId = 1,
                    Size = Animal.AnimalSize.Large,
                    Diet = Animal.DietaryClass.Carnivore,
                    Activity = Animal.ActivityPattern.Diurnal,
                    Prey = "None",
                    EnclosureId = 1,
                    Space = 100,
                    Security = Animal.SecurityLevel.High
                },
                new Animal
                {
                    Id = 2,
                    Name = "Elephant",
                    Species = "Loxodonta",
                    CategoryId = 2,
                    Size = Animal.AnimalSize.Large,
                    Diet = Animal.DietaryClass.Herbivore,
                    Activity = Animal.ActivityPattern.Diurnal,
                    Prey = "None",
                    EnclosureId = 2,
                    Space = 500,
                    Security = Animal.SecurityLevel.Medium
                });
            context.SaveChanges();
        }

        [Fact]
        public void GetAnimals_ReturnsOkResult_WithListOfAnimals()
        {
            using (var context = new DierentuinContext(_dbContextOptions))
            {
                // Arrange
                context.Database.EnsureDeleted();
                SeedDatabase(context);
                var controller = new AnimalApiController(context);

                // Act
                var result = controller.GetAnimals();

                // Assert
                var actionResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnValue = Assert.IsType<List<AnimalDto>>(actionResult.Value);
                Assert.Equal(2, returnValue.Count);
            }
        }

        [Fact]
        public void GetAnimal_ReturnsNotFound_WhenAnimalDoesNotExist()
        {
            using (var context = new DierentuinContext(_dbContextOptions))
            {
                // Arrange
                context.Database.EnsureDeleted();
                var controller = new AnimalApiController(context);

                // Act
                var result = controller.GetAnimal(999);

                // Assert
                Assert.IsType<NotFoundResult>(result.Result);
            }
        }

        [Fact]
        public async Task PostAnimal_ReturnsCreatedAtActionResult()
        {
            using (var context = new DierentuinContext(_dbContextOptions))
            {
                // Arrange
                context.Database.EnsureDeleted();
                var controller = new AnimalApiController(context);
                var animalDto = new AnimalDto
                {
                    Name = "Tiger",
                    Species = "Panthera tigris",
                    CategoryId = 1,
                    Size = "Large",
                    Diet = "Carnivore",
                    Activity = "Diurnal",
                    Prey = "None",
                    EnclosureId = 1,
                    Space = 100,
                    Security = "High"
                };

                // Act
                var result = await controller.PostAnimal(animalDto);

                // Assert
                var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
                Assert.Equal("GetAnimal", actionResult.ActionName);
                // Pas de verwachte waarde aan
                var createdAnimal = context.Animals.Last();
                Assert.Equal(createdAnimal.Id, actionResult.RouteValues["id"]);
            }
        }

        [Fact]
        public async Task PutAnimal_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            using (var context = new DierentuinContext(_dbContextOptions))
            {
                // Arrange
                context.Database.EnsureDeleted();
                var animal = new Animal
                {
                    Id = 1,
                    Name = "Giraffe",
                    Species = "Giraffa camelopardalis",
                    CategoryId = 3,
                    Size = Animal.AnimalSize.Large,
                    Diet = Animal.DietaryClass.Herbivore,
                    Activity = Animal.ActivityPattern.Diurnal,
                    Prey = "None",
                    EnclosureId = 3,
                    Space = 300,
                    Security = Animal.SecurityLevel.Medium
                };
                context.Animals.Add(animal);
                context.SaveChanges();

                var controller = new AnimalApiController(context);
                var animalDto = new AnimalDto
                {
                    Id = 1,
                    Name = "Updated Giraffe",
                    Species = "Giraffa camelopardalis",
                    CategoryId = 3,
                    Size = "Large",
                    Diet = "Herbivore",
                    Activity = "Diurnal",
                    Prey = "None",
                    EnclosureId = 3,
                    Space = 300,
                    Security = "Medium"
                };

                // Act
                var result = await controller.PutAnimal(1, animalDto);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task DeleteAnimal_ReturnsNoContent_WhenAnimalIsDeleted()
        {
            using (var context = new DierentuinContext(_dbContextOptions))
            {
                // Arrange
                context.Database.EnsureDeleted();
                var animal = new Animal
                {
                    Id = 1,
                    Name = "Zebra",
                    Species = "Equus quagga",
                    CategoryId = 4,
                    Size = Animal.AnimalSize.Medium,
                    Diet = Animal.DietaryClass.Herbivore,
                    Activity = Animal.ActivityPattern.Diurnal,
                    Prey = "None",
                    EnclosureId = 4,
                    Space = 200,
                    Security = Animal.SecurityLevel.Medium
                };
                context.Animals.Add(animal);
                context.SaveChanges();

                var controller = new AnimalApiController(context);

                // Act
                var result = await controller.DeleteAnimal(1);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }
        }
    }
}

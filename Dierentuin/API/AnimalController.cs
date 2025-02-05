using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dierentuin.Data;
using Dierentuin.Models;
using Dierentuin.DTO;

namespace Dierentuin.API
{
    [Route("api/animals")] // Define the route where the api will accesible
    [ApiController]
    public class AnimalApiController : ControllerBase // Inherit from ControllerBase instead of Controller, leaving out the view functionality used in the Controller class
    {
        private readonly DierentuinContext _context;

        public AnimalApiController(DierentuinContext context)
        {
            _context = context;
        }

        [HttpGet] // Simply returns all Animals
        public ActionResult<IEnumerable<AnimalDto>> GetAnimals()
        {
            var animals = _context.Animals
                .Select(a => new AnimalDto // DTO (Data Transfer Object) is used to define what properties will be accesible in the API
                {
                    Id = a.Id,
                    Name = a.Name,
                    Species = a.Species,
                    Category = a.Category!.Name,
                    Size = a.Size.ToString(),
                    Diet = a.Diet.ToString(),
                    Activity = a.Activity.ToString(),
                    Prey = a.Prey,
                    Enclosure = a.Enclosure!.Name,
                    Space = a.Space,
                    Security = a.Security.ToString()
                }).ToList();

            return Ok(animals);
        }

        [HttpGet("{id}")] // Returns a single animal based on it's id
        public ActionResult<AnimalDto> GetAnimal(int id)
        {
            var animal = _context.Animals
                .Select(a => new AnimalDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Species = a.Species,
                    Category = a.Category!.Name,
                    Size = a.Size.ToString(),
                    Diet = a.Diet.ToString(),
                    Activity = a.Activity.ToString(),
                    Prey = a.Prey,
                    Enclosure = a.Enclosure!.Name,
                    Space = a.Space,
                    Security = a.Security.ToString()
                }).FirstOrDefault(e => e.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            return Ok(animal);
        }

        [HttpGet("{id}/sleepstate")] // returns the sleeping state of a given animal by it's id
        public ActionResult<string> GetAnimalSleepState(int id)
        {
            var animal = _context.Animals.Find(id);
            if (animal == null)
            {
                return NotFound();
            }

            string sleepState = GetSleepState(animal.Activity);
            return Ok(sleepState);
        }

        private static string GetSleepState(Animal.ActivityPattern activityPattern)
        {
            var currentHour = DateTime.Now.Hour;

            return activityPattern switch // set the sleepingstate based on the activity pattern of the animals and the current hour of the day
            {
                Animal.ActivityPattern.Diurnal when currentHour >= 6 && currentHour < 18 => "Awake",
                Animal.ActivityPattern.Diurnal => "Sleeping",
                Animal.ActivityPattern.Nocturnal when currentHour >= 18 || currentHour < 6 => "Awake",
                Animal.ActivityPattern.Nocturnal => "Sleeping",
                Animal.ActivityPattern.Cathemeral => "Always Awake",
                _ => throw new ArgumentException("Invalid activity pattern")
            };
        }

        [HttpGet("{id}/feedingtime")] // return the feed of a given animal by it's id
        public ActionResult<string> GetAnimalFeedingTime(int id)
        {
            var animal = _context.Animals.Find(id);
            if (animal == null)
            {
                return NotFound();
            }

            return Ok(animal.Prey);
        }

        [HttpPost] // Allows adding an animal using the API by sending a post request to the animals endpoint with the properties as a request body
        public async Task<ActionResult<AnimalDto>> PostAnimal(AnimalDto animalDto)
        {
            var animal = new Animal
            {
                Name = animalDto.Name,
                Species = animalDto.Species,
                CategoryId = animalDto.CategoryId,
                Size = Enum.Parse<Animal.AnimalSize>(animalDto.Size),
                Diet = Enum.Parse<Animal.DietaryClass>(animalDto.Diet),
                Activity = Enum.Parse<Animal.ActivityPattern>(animalDto.Activity),
                Prey = animalDto.Prey,
                EnclosureId = animalDto.EnclosureId,
                Space = animalDto.Space,
                Security = Enum.Parse<Animal.SecurityLevel>(animalDto.Security)
            };

            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animalDto);
        }

        [HttpPut("{id}")] // Allows editing an animals properties by sending a put request to the animals endpoint with the properties as a request body
        public async Task<IActionResult> PutAnimal(int id, AnimalDto animalDto)
        {
            if (id != animalDto.Id)
            {
                return BadRequest();
            }

            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            animal.Name = animalDto.Name;
            animal.Species = animalDto.Species;
            animal.CategoryId = animalDto.CategoryId;
            animal.Size = Enum.Parse<Animal.AnimalSize>(animalDto.Size);
            animal.Diet = Enum.Parse<Animal.DietaryClass>(animalDto.Diet);
            animal.Activity = Enum.Parse<Animal.ActivityPattern>(animalDto.Activity);
            animal.Prey = animalDto.Prey;
            animal.EnclosureId = animalDto.EnclosureId;
            animal.Space = animalDto.Space;
            animal.Security = Enum.Parse<Animal.SecurityLevel>(animalDto.Security);

            _context.Entry(animal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")] // Allows deleting an animal by sending a delete request to the animal endpoint
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnimalExists(int id)
        {
            return _context.Animals.Any(e => e.Id == id);
        }
    }
}
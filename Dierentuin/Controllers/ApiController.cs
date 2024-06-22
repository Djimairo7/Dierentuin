using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dierentuin.Data;
using Dierentuin.Models;
using Dierentuin.DTO;

namespace Dierentuin.Controllers
{
    [Route("api/actions")]
    [ApiController]
    public class ActionApiController : ControllerBase
    {
        // To-do: Add Actions
    }

    [Route("api/enclosures")]
    [ApiController]
    public class EnclosureApiController : ControllerBase
    {
        private readonly DierentuinContext _context;

        public EnclosureApiController(DierentuinContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EnclosureDto>> GetEnclosures()
        {
            var enclosures = _context.Enclosures
                .Include(e => e.Animals)
                .ThenInclude(a => a.Category)
                .Select(e => new EnclosureDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Animals = e.Animals.Select(a => new AnimalDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Species = a.Species,
                        Category = a.Category!.Name,
                        Size = a.Size.ToString(),
                        Diet = a.Diet.ToString(),
                        Activity = a.Activity.ToString(),
                        Prey = a.Prey,
                        Space = a.Space,
                        Security = a.Security.ToString()
                    }).ToList(),
                    Climate = e.Climate.ToString(),
                    Habitat = e.Habitat.ToString(),
                    Security = e.Security.ToString(),
                    Size = e.Size
                }).ToList();

            return Ok(enclosures);
        }

        [HttpGet("{id}")]
        public ActionResult<EnclosureDto> GetEnclosure(int id)
        {
            var enclosure = _context.Enclosures
                .Include(e => e.Animals)
                .ThenInclude(a => a.Category)
                .Select(e => new EnclosureDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Animals = e.Animals.Select(a => new AnimalDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Species = a.Species,
                        Category = a.Category!.Name,
                        Size = a.Size.ToString(),
                        Diet = a.Diet.ToString(),
                        Activity = a.Activity.ToString(),
                        Prey = a.Prey,
                        Space = a.Space,
                        Security = a.Security.ToString()
                    }).ToList(),
                    Climate = e.Climate.ToString(),
                    Habitat = e.Habitat.ToString(),
                    Security = e.Security.ToString(),
                    Size = e.Size
                }).FirstOrDefault(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            return Ok(enclosure);
        }

        [HttpPost]
        public async Task<ActionResult<AnimalDto>> PostEnclosure(EnclosureDto enclosureDto)
        {
            var enclosure = new Enclosure
            {
                Name = enclosureDto.Name,
                Climate = Enum.Parse<Enclosure.ClimateTypes>(enclosureDto.Climate),
                Habitat = Enum.Parse<Enclosure.HabitatTypes>(enclosureDto.Habitat),
                Security = Enum.Parse<Enclosure.SecurityLevel>(enclosureDto.Security),
                Size = enclosureDto.Size
            };

            _context.Enclosures.Add(enclosure);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEnclosure), new { id = enclosure.Id }, enclosureDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnimal(int id, EnclosureDto enclosureDto)
        {
            if (id != enclosureDto.Id)
            {
                return BadRequest();
            }

            var enclosure = await _context.Enclosures.FindAsync(id);
            if (enclosure == null)
            {
                return NotFound();
            }

            enclosure.Name = enclosureDto.Name;
            enclosure.Climate = Enum.Parse<Enclosure.ClimateTypes>(enclosureDto.Climate);
            enclosure.Habitat = Enum.Parse<Enclosure.HabitatTypes>(enclosureDto.Habitat);
            enclosure.Security = Enum.Parse<Enclosure.SecurityLevel>(enclosureDto.Security);
            enclosure.Size = enclosureDto.Size;

            _context.Entry(enclosure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnclosureExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnclosure(int id)
        {
            var enclosure = await _context.Enclosures.FindAsync(id);
            if (enclosure == null)
            {
                return NotFound();
            }

            _context.Enclosures.Remove(enclosure);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EnclosureExists(int id)
        {
            return _context.Enclosures.Any(e => e.Id == id);
        }
    }

    [Route("api/categories")]
    [ApiController]
    public class CategoryApiController : ControllerBase
    {
        private readonly DierentuinContext _context;

        public CategoryApiController(DierentuinContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoryDto>> GetCategories()
        {
            var categories = _context.Categories
                .Include(e => e.Animals)
                .Select(e => new CategoryDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Animals = e.Animals.Select(a => new AnimalDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Species = a.Species,
                        Size = a.Size.ToString(),
                        Diet = a.Diet.ToString(),
                        Activity = a.Activity.ToString(),
                        Prey = a.Prey,
                        Space = a.Space,
                        Security = a.Security.ToString()
                    }).ToList(),
                }).ToList();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public ActionResult<CategoryDto> GetCategory(int id)
        {
            var category = _context.Categories
                .Include(e => e.Animals)
                .Select(e => new CategoryDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Animals = e.Animals.Select(a => new AnimalDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Species = a.Species,
                        Size = a.Size.ToString(),
                        Diet = a.Diet.ToString(),
                        Activity = a.Activity.ToString(),
                        Prey = a.Prey,
                        Space = a.Space,
                        Security = a.Security.ToString()
                    }).ToList(),
                }).FirstOrDefault(e => e.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> PostCategory(CategoryDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, categoryDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryDto categoryDto)
        {
            if (id != categoryDto.Id)
            {
                return BadRequest();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = categoryDto.Name;

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }

    [Route("api/animals")]
    [ApiController]
    public class AnimalApiController : ControllerBase
    {
        private readonly DierentuinContext _context;

        public AnimalApiController(DierentuinContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AnimalDto>> GetAnimals()
        {
            var animals = _context.Animals
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
                }).ToList();

            return Ok(animals);
        }

        [HttpGet("{id}")]
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

        [HttpPost]
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

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
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

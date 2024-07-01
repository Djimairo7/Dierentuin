using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dierentuin.Data;
using Dierentuin.Models;
using Dierentuin.DTO;

namespace Dierentuin.API
{
    [Route("api/categories")] // Define the route where the api will accesible
    [ApiController]
    public class CategoryApiController : ControllerBase // Inherit from ControllerBase instead of Controller, leaving out the view functionality used in the Controller class
    {
        private readonly DierentuinContext _context;

        public CategoryApiController(DierentuinContext context)
        {
            _context = context;
        }

        [HttpGet] // Simply returns all Categories
        public ActionResult<IEnumerable<CategoryDto>> GetCategories()
        {
            var categories = _context.Categories
                .Include(e => e.Animals)
                .Select(e => new CategoryDto // DTO (Data Transfer Object) is used to define what properties will be accesible in the API
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

        [HttpGet("{id}")] // Returns a single category based on it's id
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

        [HttpPost] // Allows adding a category using the API by sending a post request to the categories endpoint with the properties as a request body
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

        [HttpPut("{id}")] // Allows editing a categories properties by sending a put request to the categories endpoint with the properties as a request body
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

        [HttpDelete("{id}")]  // Allows deleting a category by sending a delete request to the categories endpoint
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
}
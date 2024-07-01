using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dierentuin.Data;
using Dierentuin.Models;
using Dierentuin.DTO;

namespace Dierentuin.API
{
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

        [HttpGet("{id}/sleepstate")]
        public ActionResult<IEnumerable<SleepStateDto>> GetEnclosureSleepStates(int id)
        {
            var enclosure = _context.Enclosures
                .Include(e => e.Animals)
                .FirstOrDefault(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            var animalSleepStates = enclosure.Animals.Select(a => new SleepStateDto
            {
                Id = a.Id,
                Name = a.Name,
                Species = a.Species,
                SleepState = GetSleepState(a.Activity)
            }).ToList();

            return Ok(animalSleepStates);
        }

        private static string GetSleepState(Animal.ActivityPattern activityPattern)
        {
            var currentHour = DateTime.Now.Hour;

            return activityPattern switch
            {
                Animal.ActivityPattern.Diurnal when currentHour >= 6 && currentHour < 18 => "Awake",
                Animal.ActivityPattern.Diurnal => "Sleeping",
                Animal.ActivityPattern.Nocturnal when currentHour >= 18 || currentHour < 6 => "Awake",
                Animal.ActivityPattern.Nocturnal => "Sleeping",
                Animal.ActivityPattern.Cathemeral => "Always Awake",
                _ => throw new ArgumentException("Invalid activity pattern")
            };
        }

        [HttpGet("{id}/feedingtime")]
        public ActionResult<IEnumerable<FeedingTimeDto>> GetEnclosureFeedingTimes(int id)
        {
            var enclosure = _context.Enclosures
                .Include(e => e.Animals)
                .FirstOrDefault(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            var animalFeedingTimes = enclosure.Animals.Select(a => new FeedingTimeDto
            {
                Id = a.Id,
                Name = a.Name,
                Species = a.Species,
                FeedingTime = a.Prey
            }).ToList();

            return Ok(animalFeedingTimes);
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
}
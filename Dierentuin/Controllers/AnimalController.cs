using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dierentuin.Data;
using Dierentuin.Models;

namespace Dierentuin.Controllers
{
    public class AnimalController : Controller
    {
        private readonly DierentuinContext _context;

        public AnimalController(DierentuinContext context)
        {
            _context = context;
        }

        // GET: Animal
        public async Task<IActionResult> Index(string search)
        {
            var animals =
                from a in _context.Animals
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                select a;

            if (!String.IsNullOrEmpty(search))
            {
                animals = animals.Where(a => a.Name.Contains(search));
            }

            return View(await animals.ToListAsync());
        }

        public async Task<IActionResult> ByCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animals = await _context.Animals
                .Where(a => a.CategoryId == id)
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .ToListAsync();

            if (animals == null || animals.Count == 0)
            {
                return NotFound();
            }

            return View(animals);
        }

        // GET: Animal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["Enclosures"] = new SelectList(_context.Enclosures, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Species,CategoryId,EnclosureId,Size,Diet,Activity,Prey,Space,Security")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(animal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", animal.CategoryId);
            ViewData["Enclosures"] = new SelectList(_context.Enclosures, "Id", "Name", animal.EnclosureId);
            return View(animal);
        }

        // GET: Animal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", animal.CategoryId);
            ViewData["Enclosures"] = new SelectList(_context.Enclosures, "Id", "Name", animal.EnclosureId);
            return View(animal);
        }

        // POST: Animal/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Species,CategoryId,EnclosureId,Size,Diet,Activity,Prey,Space,Security")] Animal animal)
        {
            if (id != animal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", animal.CategoryId);
            ViewData["Enclosures"] = new SelectList(_context.Enclosures, "Id", "Name", animal.EnclosureId);
            return View(animal);
        }

        // GET: Animal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal != null)
            {
                _context.Animals.Remove(animal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SleepState(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            var sleepState = GetSleepState(animal.Activity);
            ViewData["SleepState"] = sleepState;

            return View(animal);
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

        public async Task<IActionResult> FeedingTime(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (animal == null)
            {
                return NotFound();
            }

            ViewData["FeedingTime"] = animal.Prey;

            return View(animal);
        }

        private bool AnimalExists(int id)
        {
            return _context.Animals.Any(e => e.Id == id);
        }
    }
}

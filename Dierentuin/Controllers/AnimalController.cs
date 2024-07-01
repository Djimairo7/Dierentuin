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

        // Constructor: Dependency injection of DierentuinContext
        public AnimalController(DierentuinContext context)
        {
            _context = context;
        }

        // GET: Animal - Displays a list of all animals, with optional search functionality
        public async Task<IActionResult> Index(string search)
        {
            var animals =
                from a in _context.Animals
                .Include(a => a.Category)
                .Include(a => a.Enclosure)
                select a;

            // If search string is provided, filter animals by name
            if (!String.IsNullOrEmpty(search))
            {
                animals = animals.Where(a => a.Name.Contains(search));
            }

            return View(await animals.ToListAsync());
        }

        // GET: Animal/ByCategory/{id} - Displays animals filtered by category
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

        // GET: Animal/Details/{id} - Displays details of a specific animal
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

        // GET: Animal/Create - Displays form to create a new animal
        public IActionResult Create()
        {
            // Populate dropdown lists for Categories and Enclosures
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["Enclosures"] = new SelectList(_context.Enclosures, "Id", "Name");
            return View();
        }

        // POST: Animal/Create - Handles the creation of a new animal
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
            // If model state is invalid, repopulate dropdown lists and return to the Create view
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", animal.CategoryId);
            ViewData["Enclosures"] = new SelectList(_context.Enclosures, "Id", "Name", animal.EnclosureId);
            return View(animal);
        }

        // GET: Animal/Edit/{id} - Displays form to edit an existing animal
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
            // Populate dropdown lists for Categories and Enclosures
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", animal.CategoryId);
            ViewData["Enclosures"] = new SelectList(_context.Enclosures, "Id", "Name", animal.EnclosureId);
            return View(animal);
        }

        // POST: Animal/Edit/{id} - Handles the update of an existing animal
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
            // If model state is invalid, repopulate dropdown lists and return to the Edit view
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", animal.CategoryId);
            ViewData["Enclosures"] = new SelectList(_context.Enclosures, "Id", "Name", animal.EnclosureId);
            return View(animal);
        }

        // GET: Animal/Delete/{id} - Displays confirmation page for deleting an animal
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

        // POST: Animal/Delete/{id} - Handles the deletion of an animal
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

        // GET: Animal/SleepState/{id} - Displays the current sleep state of an animal
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

        // Helper method to determine the sleep state based on activity pattern and current time
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

        // GET: Animal/FeedingTime/{id} - Displays the feeding time information for an animal
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

        // Helper method to check if an animal exists in the database
        private bool AnimalExists(int id)
        {
            return _context.Animals.Any(e => e.Id == id);
        }
    }
}
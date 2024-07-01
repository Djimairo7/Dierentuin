using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dierentuin.Data;
using Dierentuin.Models;

namespace Dierentuin.Controllers
{
    public class EnclosureController : Controller
    {
        private readonly DierentuinContext _context;

        // Constructor: Dependency injection of DierentuinContext
        public EnclosureController(DierentuinContext context)
        {
            _context = context;
        }

        // GET: Enclosure - Displays a list of all enclosures
        public async Task<IActionResult> Index()
        {
            return View(await _context.Enclosures.ToListAsync());
        }

        // GET: Enclosure/Details/{id} - Displays details of a specific enclosure
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enclosure = await _context.Enclosures
                .Include(e => e.Animals) // Include the list of animals in this enclosure
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enclosure == null)
            {
                return NotFound();
            }

            return View(enclosure);
        }

        // GET: Enclosure/Create - Displays form to create a new enclosure
        public IActionResult Create()
        {
            return View();
        }

        // POST: Enclosure/Create - Handles the creation of a new enclosure
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Climate,Habitat,Security,Size")] Enclosure enclosure)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enclosure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(enclosure);
        }

        // GET: Enclosure/Edit/{id} - Displays form to edit an existing enclosure
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enclosure = await _context.Enclosures.FindAsync(id);
            if (enclosure == null)
            {
                return NotFound();
            }
            return View(enclosure);
        }

        // POST: Enclosure/Edit/{id} - Handles the update of an existing enclosure
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Climate,Habitat,Security,Size")] Enclosure enclosure)
        {
            if (id != enclosure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enclosure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnclosureExists(enclosure.Id))
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
            return View(enclosure);
        }

        // GET: Enclosure/Delete/{id} - Displays confirmation page for deleting an enclosure
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enclosure = await _context.Enclosures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enclosure == null)
            {
                return NotFound();
            }

            return View(enclosure);
        }

        // POST: Enclosure/Delete/{id} - Handles the deletion of an enclosure
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enclosure = await _context.Enclosures.FindAsync(id);
            if (enclosure != null)
            {
                _context.Enclosures.Remove(enclosure);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Helper method to check if an enclosure exists in the database
        private bool EnclosureExists(int id)
        {
            return _context.Enclosures.Any(e => e.Id == id);
        }
    }
}
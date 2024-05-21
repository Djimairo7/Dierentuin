using System.Collections.Generic;
using System.Threading.Tasks;
using Dierentuin.Controllers;
using Dierentuin.Data;
using Dierentuin.Models;
using Microsoft.EntityFrameworkCore;
using Dierentuin.Controllers.Services;

public class StorageRepository : IStorageRepository
    {
        private readonly DierentuinContext _context;

        public StorageRepository(DierentuinContext context)
        {
            _context = context;
        }

        public async Task<List<Animal>> GetAllAnimalsAsync()
        {
            return await _context.Animals.ToListAsync();
        }

        public async Task<Animal> GetAnimalByIdAsync(int id)
        {
            return await _context.Animals.FindAsync(id);
        }

        public async Task CreateAnimalAsync(Animal animal)
        {
            _context.Animals.Add(animal);
        }

        public async Task UpdateAnimalAsync(Animal animal)
        {
            _context.Entry(animal).State = EntityState.Modified;
        }

        public async Task DeleteAnimalAsync(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
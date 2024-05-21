using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dierentuin.Controllers;
using Dierentuin.Data;
using Dierentuin.Models;
using Microsoft.EntityFrameworkCore;

namespace Dierentuin.Controllers.Services
{
    public interface IStorageRepository
    {
        Task<List<Animal>> GetAllAnimalsAsync();
        Task<Animal> GetAnimalByIdAsync(int id);
        Task CreateAnimalAsync(Animal animal);

        Task UpdateAnimalAsync(Animal animal);
        Task DeleteAnimalAsync(int id);
        Task SaveChangesAsync();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dierentuin.Models;

namespace Dierentuin.Data
{
    public class DierentuinContext(DbContextOptions<DierentuinContext> options) : DbContext(options)
    {
        public DbSet<Animal> Animals { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Enclosure> Enclosures { get; set; } = default!;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dierentuin.Models;

namespace Dierentuin.Data
{
    // This class represents the database context for the Dierentuin application
    // It inherits from DbContext, which is part of Entity Framework Core
    public class DierentuinContext : DbContext
    {
        // Constructor that takes DbContextOptions as a parameter
        // This allows for configuration to be passed in from the startup class
        public DierentuinContext(DbContextOptions<DierentuinContext> options) : base(options)
        {
        }

        // DbSet properties represent tables in the database
        // Each DbSet corresponds to a model in the application

        // Represents the Animals table
        public DbSet<Animal> Animals { get; set; } = default!;

        // Represents the Categories table
        public DbSet<Category> Categories { get; set; } = default!;

        // Represents the Enclosures table
        public DbSet<Enclosure> Enclosures { get; set; } = default!;

        // This method is used to configure the model that creates the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationship between Animal and Enclosure
            modelBuilder.Entity<Animal>()
                .HasOne(a => a.Enclosure)  // Each Animal has one Enclosure
                .WithMany(e => e.Animals)  // Each Enclosure can have many Animals
                .HasForeignKey(a => a.EnclosureId);  // The foreign key in the Animals table

            // Call the base method to ensure any base configuration is applied
            base.OnModelCreating(modelBuilder);
        }
    }
}
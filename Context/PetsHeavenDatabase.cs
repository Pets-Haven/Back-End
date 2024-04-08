﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetsHeaven.Models;

namespace PetsHeaven.Context
{
    public class PetsHeavenDatabase:DbContext 
    {
        public PetsHeavenDatabase(DbContextOptions options):base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}

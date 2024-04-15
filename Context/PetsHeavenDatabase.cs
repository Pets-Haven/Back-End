using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetsHeaven.Models;
using System.Reflection.Emit;

namespace PetsHeaven.Context
{
    public class PetsHeavenDatabase : IdentityDbContext<ApplicationUser>
    {
        public PetsHeavenDatabase(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Cart { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Cart>()
       .HasKey(c => new { c.userId, c.productId });
            builder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Cart)
                .HasForeignKey(c => c.userId);

            builder.Entity<Cart>()
                .HasOne(c => c.Product)
                .WithMany(p => p.Cart)
                .HasForeignKey(c => c.productId);
        }
      
    }
}

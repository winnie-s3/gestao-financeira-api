using GestaoAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestaoAPI.Services
{
    public class GestaoAPIDbContext : DbContext
    {
        public GestaoAPIDbContext(DbContextOptions<GestaoAPIDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Income>()
                .Property(i => i.Amount)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}

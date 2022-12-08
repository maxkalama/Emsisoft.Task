using Microsoft.EntityFrameworkCore;

namespace Emsisoft.DB
{
    public class HashesContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Server=max;Database=MaximKudinovHashes;Integrated Security=true;Encrypt=False;"); //Encrypt=False; breaking change in EF Core 7
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hash>().Property(h => h.Date)
                .HasConversion(v => v.ToDateTime(TimeOnly.MinValue), v => DateOnly.FromDateTime(v));
        }

        public DbSet<Hash> Hashes { get; set; }

    }
}
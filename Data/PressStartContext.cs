using PressStart.Models;
using Microsoft.EntityFrameworkCore;
using PressStart.Data.Configurations;

namespace PressStart.Data
{
    public class PressStartContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data source=PressStart.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GameConfiguration());
        }
    }
}
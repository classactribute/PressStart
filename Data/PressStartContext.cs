using PressStart.Models;
using Microsoft.EntityFrameworkCore;
namespace PressStart.Data
{
    public class PressStartContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data source=PressStart.sqlite");
        }
    }
}
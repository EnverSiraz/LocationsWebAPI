using LocationsWebAPI.Models.ORMs;
using Microsoft.EntityFrameworkCore;

namespace LocationsWebAPI.Contexts
{
    public class LocationsDataBase :DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-2ACUT5F; Database=LocationsDb; trusted_connection=true");
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<District> Districts { get; set; }
        
        
    }
}

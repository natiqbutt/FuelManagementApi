using Microsoft.EntityFrameworkCore;
using FuelManagementApi.Models;

namespace FuelManagementApi.Data
{
    public class FMDbContext : DbContext
    {
        public FMDbContext(DbContextOptions<FMDbContext> options)
            : base(options)
        {
        }
        public DbSet<Fuel> fuels { get; set; }
    }
}
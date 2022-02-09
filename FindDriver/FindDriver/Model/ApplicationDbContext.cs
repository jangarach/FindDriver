using FindDriver.Api.Model.DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace FindDriver.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.ToTable("users", schema: "main");
            });
            builder.Entity<Order>(entity =>
            {
                entity.ToTable("orders", schema: "main");
            });
            builder.Entity<City>(entity =>
            {
                entity.ToTable("cities", schema: "main");
            });
        }
    }
}

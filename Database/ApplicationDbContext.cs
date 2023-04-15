using LittleBank.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LittleBank.Api.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Credit> Credits { get; set; }
        public DbSet<Operation> Operations { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }
    }
}

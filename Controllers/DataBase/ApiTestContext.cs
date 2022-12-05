using Api_Test.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_Test.Controllers.DataBase
{
    public class ApiTestContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<PostRequest> PostRequests { get; set; }

        public ApiTestContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=RemPer.db");
        }
    }
}

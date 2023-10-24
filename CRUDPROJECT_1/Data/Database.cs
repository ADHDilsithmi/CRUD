using CRUDPROJECT_1.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CRUDPROJECT_1.Data
{
    public class Database : DbContext
    {
        public Database(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } 
        


    }
}

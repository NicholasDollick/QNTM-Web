using Microsoft.EntityFrameworkCore;
using QNTM.API.Models;

namespace QNTM.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<Value> Values { get; set; }
    }
}
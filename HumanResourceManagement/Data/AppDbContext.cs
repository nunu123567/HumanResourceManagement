using Microsoft.EntityFrameworkCore;
using HumanResourceManagement.Models;

namespace HumanResourceManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> Todos => Set<TodoItem>();
    }
}
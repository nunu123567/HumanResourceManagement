using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HumanResourceManagement.Data;
using HumanResourceManagement.Models;

namespace HumanResourceManagement.Services
{
    public class EfTodoRepository : ITodoRepository
    {
        private readonly IDbContextFactory<AppDbContext> factory;

        public EfTodoRepository(IDbContextFactory<AppDbContext> factory)
        {
            this.factory = factory;
        }

        public async Task<List<TodoItem>> GetAllAsync()
        {
            await using AppDbContext db = await factory.CreateDbContextAsync();
            return await db.Todos
                .AsNoTracking()
                .OrderBy(t => t.Id)
                .ToListAsync();
        }

        public async Task AddAsync(TodoItem item)
        {
            await using AppDbContext db = await factory.CreateDbContextAsync();
            Debug.WriteLine($"Id before SaveChanges: {item.Id}");
            db.Todos.Add(item);
            await db.SaveChangesAsync();
            Debug.WriteLine($"Id after SaveChanges: {item.Id}");
        }

        public async Task UpdateAsync(TodoItem item)
        {
            await using AppDbContext db = await factory.CreateDbContextAsync();
            db.Todos.Update(item);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(TodoItem item)
        {
            await using AppDbContext db = await factory.CreateDbContextAsync();
            db.Todos.Remove(item);
            await db.SaveChangesAsync();
        }
    }
}
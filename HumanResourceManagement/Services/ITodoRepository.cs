using System.Collections.Generic;
using System.Threading.Tasks;
using HumanResourceManagement.Models;

namespace HumanResourceManagement.Services
{
    public interface ITodoRepository
    {
        Task<List<TodoItem>> GetAllAsync();
        Task AddAsync(TodoItem item);
        Task UpdateAsync(TodoItem item);
        Task DeleteAsync(TodoItem item);
    }
}
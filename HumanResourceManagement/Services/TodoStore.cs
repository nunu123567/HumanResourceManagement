using System.Collections.ObjectModel;
using HumanResourceManagement.Models;

namespace HumanResourceManagement.Services
{
    // Later (SQLite + EF Core) only the inside of this class changes – the pages stay the same.
    public class TodoStore
    {
        public ObservableCollection<TodoItem> Items { get; } = new();

        public void Add(TodoItem item)
        {
            Items.Add(item);
        }

        public void Remove(TodoItem item)
        {
            Items.Remove(item);
        }
    }
}
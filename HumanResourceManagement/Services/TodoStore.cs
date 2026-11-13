using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using HumanResourceManagement.Models;

namespace HumanResourceManagement.Services
{
    public class TodoStore
    {
        private readonly ITodoRepository repository;
        private bool isLoaded;

        public ObservableCollection<TodoItem> Items { get; } = new();

        public TodoStore(ITodoRepository repository)
        {
            this.repository = repository;
        }

        public async Task LoadAsync()
        {
            if (isLoaded) return;
            isLoaded = true;

            List<TodoItem> items = await repository.GetAllAsync();
            foreach (TodoItem item in items)
            {
                Watch(item);
                Items.Add(item);
            }
        }

        public async Task AddAsync(TodoItem item)
        {
            await LoadAsync();
            await repository.AddAsync(item);    // gives item.Id a value
            Watch(item);
            Items.Add(item);
        }

        public async Task RemoveAsync(TodoItem item)
        {
            await repository.DeleteAsync(item);
            item.PropertyChanged -= OnItemChanged;
            Items.Remove(item);
        }

        private void Watch(TodoItem item)
        {
            item.PropertyChanged += OnItemChanged;
        }

        private async void OnItemChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is TodoItem item && e.PropertyName == nameof(TodoItem.IsDone))
            {
                try
                {
                    await repository.UpdateAsync(item);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Could not save the change: {ex.Message}");
                }
            }
        }
    }
}
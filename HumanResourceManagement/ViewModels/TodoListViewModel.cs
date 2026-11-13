using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HumanResourceManagement.Models;
using HumanResourceManagement.Services;

namespace HumanResourceManagement.ViewModels
{
    public partial class TodoListViewModel : ObservableObject
    {
        private readonly TodoStore store;
        private readonly IPreferences preferences;

        // The page shows the store's own collection – not a copy!
        public ObservableCollection<TodoItem> Items => store.Items;

        [ObservableProperty]
        private string summary = "";

        public TodoListViewModel(TodoStore store, IPreferences preferences)
        {
            this.store = store;
            this.preferences = preferences;

            UpdateSummary();
            store.Items.CollectionChanged += (sender, e) => UpdateSummary();
        }

        private void UpdateSummary()
        {
            int count = store.Items.Count;
            Summary = count == 1 ? "1 task" : $"{count} tasks";
        }

        [RelayCommand]
        private void Delete(TodoItem item)
        {
            if (item != null)
            {
                store.Remove(item);
            }
        }

        //public void ApplySort()
        //{
            //string order = preferences.Get(SettingsKeys.SortOrder, SettingsDefaults.SortOrder);

            //List<TodoItem> sorted;
            //if (order == "Due date")
            //{
                //sorted = Items.OrderBy(i => i.DueDate).ThenByDescending(i => i.Priority).ToList();
            //}
            //else
            //{
              //  sorted = Items.OrderByDescending(i => i.Priority).ThenBy(i => i.DueDate).ToList();
            //}

            //for (int i = 0; i < sorted.Count; i++)
            //{
                //int oldIndex = Items.IndexOf(sorted[i]);
               // if (oldIndex != i)
                //{
                //    Items.Move(oldIndex, i);
                //}
           // }
       // }
    }
}
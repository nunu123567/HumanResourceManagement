using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using HumanResourceManagement.Models;
using HumanResourceManagement.Services;

namespace HumanResourceManagement.ViewModels
{
    public partial class AddTaskViewModel : ObservableObject
    {
        public ObservableCollection<string> Categories { get; } = new() { "Study", "Home", "Health", "Work" };

        private readonly TodoStore store;
        private readonly IPreferences preferences;

        public AddTaskViewModel(TodoStore store, IPreferences preferences)
        {
            this.store = store;
            this.preferences = preferences;
        }

        public void OnAppearing()
        {
            if (string.IsNullOrEmpty(Title) && string.IsNullOrEmpty(Notes))
            {
                //ApplyDefaults();
            }
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        private async Task SaveAsync()        
        {
            var item = new TodoItem
            {
                Title = Title.Trim(),
                Notes = Notes?.Trim() ?? "",
                Category = Category,
                DueDate = DueDate,
                Priority = (int)Priority,
                Reminder = Reminder
            };

            try
            {
                await store.AddAsync(item);
            }
            catch (Exception ex)
            {
                Result = $"Could not save: {ex.Message}";
                return;                       // the form keeps its values
            }

            string reminderText = item.Reminder ? "on" : "off";
            Result = $"Saved: {item.Title} · {item.Category} · P{item.Priority} · due {item.DueDate:dd.MM.yyyy} · reminder {reminderText} ({store.Items.Count} in list)";

            Title = "";
            Notes = "";
            ApplyDefaults();                 
        }

        private void ApplyDefaults()
        {
            Category = "Study";
            Priority = 3;
            Reminder = true;
            DueDate = DateTime.Today.AddDays(1);
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TitleCounter))]   // the counter label updates too
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private string title = "";

        private bool CanSave()
        {
            string trimmed = Title?.Trim() ?? string.Empty;
            return !string.IsNullOrEmpty(trimmed) && trimmed.Length <= 40;
        }

        [ObservableProperty]
        private string notes = "";

        [ObservableProperty]
        private string category = "Study";

        [ObservableProperty]
        private DateTime dueDate = DateTime.Today.AddDays(1);

        [ObservableProperty]
        private double priority = 3;

        [ObservableProperty]
        private bool reminder = true;

        [ObservableProperty]
        private string result = "";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddCategoryCommand))]
        private string newCategory = "";

        private bool CanAddCategory()
        {
            string trimmed = NewCategory?.Trim() ?? string.Empty;
            return !string.IsNullOrEmpty(trimmed) &&
           !Categories.Any(c => string.Equals(c, trimmed, StringComparison.OrdinalIgnoreCase));
        }

        [RelayCommand(CanExecute = nameof(CanAddCategory))]
        private void AddCategory()                // -> AddCategoryCommand
        {
            string trimmed = NewCategory?.Trim();
            Categories.Add(trimmed);
            Category = trimmed;
            NewCategory = "";
        }

        public string TitleCounter => $"{Title?.Length ?? 0}/40";
    }


}
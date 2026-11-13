using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
//using ObjCBindings;
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
            //ApplyDefaults();
        }

       // public void ApplyDefaults()
        //{
            // TODO 2c: set Category and Priority from the preferences
            //          (Get returns an int for the priority – it fits into the double Priority)
          //  Category = preferences.Get(SettingsKeys.DefaultCategory, SettingsDefaults.DefaultCategory);
            //Priority = preferences.Get(SettingsKeys.DefaultPriority, SettingsDefaults.DefaultPriority);
        //}

        // Save(): after  Title = "";  Notes = "";
        // TODO 2d: call ApplyDefaults() so the next task starts from the defaults again
        public void OnAppearing()
        {
            // TODO 2e: apply the defaults only if the form is still empty (Title is empty).
            //          Do not overwrite a task the user is in the middle of writing.
            if (string.IsNullOrEmpty(Title) && string.IsNullOrEmpty(Notes))
            {
                //ApplyDefaults();
            }
        }

        [RelayCommand(CanExecute = nameof(CanSave))]
        private void Save()
        {
            var item = new TodoItem
            {
                Title = Title?.Trim() ?? string.Empty,
                Notes = Notes?.Trim() ?? string.Empty,
                Category = Category,
                DueDate = DueDate,
                Priority = (int)Priority,
                Reminder = Reminder
            };

            store.Add(item);

            int count = store.Items.Count;
            string reminderStatus = Reminder ? "reminder on" : "reminder off";
            string formattedDate = DueDate.ToString("dd.MM.yyyy");

            Result = $"Saved: {item.Title} · {Category} · P{item.Priority} · due {formattedDate} · {reminderStatus} ({count} in list)";

            Title = "";
            Notes = "";

            //ApplyDefaults();
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
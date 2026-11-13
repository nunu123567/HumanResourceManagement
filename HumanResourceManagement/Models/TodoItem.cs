using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HumanResourceManagement.Models
{
    public partial class TodoItem : ObservableObject
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Notes { get; set; } = "";
        public string Category { get; set; } = "";
        public DateTime DueDate { get; set; }
        public int Priority { get; set; }
        public bool Reminder { get; set; }

        [ObservableProperty]
        private bool isDone;  

        public string Details => $"{Category} · due {DueDate:dd.MM.yyyy} · P{Priority}";
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagement
{
    public class AddTaskForm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }

        public List<string> Categories { get; } = new() { "Study", "Home", "Health", "Work" };

        private string title = "";
        public string Title
        {
            get => title;
            set
            {
                if (Set(ref title, value ?? ""))
                {
                    OnPropertyChanged(nameof(TitleCounter));
                    SaveCommand.ChangeCanExecute(); 
                }
            }
        }

        public string TitleCounter => $"{Title.Length}/40";

        private string notes = "";
        public string Notes { get => notes; set => Set(ref notes, value); }

        private string category = "Study";
        public string Category { get => category; set => Set(ref category, value); }

        private DateTime dueDate = DateTime.Today.AddDays(1);
        public DateTime DueDate { get => dueDate; set => Set(ref dueDate, value); }

        private double priority = 3;
        public double Priority { get => priority; set => Set(ref priority, value); }

        private bool reminder = true;
        public bool Reminder { get => reminder; set => Set(ref reminder, value); }

        private string result = "";
        public string Result { get => result; set => Set(ref result, value); }

        public Command SaveCommand { get; }

        public AddTaskForm()
        {
            SaveCommand = new Command(Save, CanSave);
        }

        private bool CanSave()
        {
            string trimmed = Title?.Trim() ?? string.Empty;
            return !string.IsNullOrEmpty(trimmed) && trimmed.Length <= 40;
        }

        private void Save()
        {
            string trimmed = Title.Trim();
            string reminderStatus = Reminder ? "reminder on" : "reminder off";
            string formattedDate = DueDate.ToString("dd.MM.yyyy");

            Result = $"Saved: {trimmed} · {Category} · P{(int)Priority} · due {formattedDate} · {reminderStatus}";
        }
    }
}

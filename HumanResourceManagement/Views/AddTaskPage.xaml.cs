using Microsoft.Maui.Controls;
using HumanResourceManagement.ViewModels;

namespace HumanResourceManagement.Views
{
    public partial class AddTaskPage : ContentPage
    {
        private readonly AddTaskViewModel viewModel;
        public AddTaskPage(AddTaskViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
            TitleEntry.Focus();
        }
    }
}
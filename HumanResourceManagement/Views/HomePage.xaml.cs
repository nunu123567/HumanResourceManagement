using HumanResourceManagement.ViewModels;

namespace HumanResourceManagement.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly HomeViewModel viewModel;

        public HomePage(HomeViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.LoadHomeDataAsync();
        }
    }
}
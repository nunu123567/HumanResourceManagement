using HumanResourceManagement.ViewModels;

namespace HumanResourceManagement.Views
{
    public partial class WeatherPage : ContentPage
    {
        private readonly WeatherViewModel viewModel;

        public WeatherPage(WeatherViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.LoadWeatherAsync();
        }
    }
}
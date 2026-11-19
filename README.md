# LifeManagementApp

## 1. Project Description
LifeManagementApp is a .NET 9 MAUI application designed to streamline personal everyday management by combining a fully functional task tracker with real-time local weather forecasts and interactive conditional alerts (such as a kitesurfing weather detector). 

The app features an MVVM architecture, clean separation of concerns using interfaces and services, persistent data storage via Entity Framework Core and SQLite, and remote data fetching through Open-Meteo's REST API.

---

## 2. Core Features & Requirements
* **Home Page**:
  * Displays the **Top 3 unfinished to-dos**, dynamically sorted first by highest priority and then by earliest due date.
  * Shows current weather metrics (temperature and wind speed in $m/s$) for Vaasa via the Open-Meteo API.
  * Provides a dynamic **Kitesurf Alert** ("🪁 Kitesurf weather!") triggered when wind speed exceeds $5.0\text{ m/s}$.
* **To-Do Management Page**:
  * Displays all to-do items inside a reactive `CollectionView`.
  * Supports sorting the list dynamically by priority or due date.
  * Dedicated form/page for adding new items with mandatory title validation (rejects empty or whitespace-only titles).
  * Data fields tracked: `Id`, `Title`, `Notes`, `Category`, `DueDate`, `Priority`, and `IsDone`.
* **Persistent Storage**:
  * Utilizes EF Core Code First with SQLite (`todos.db`) to ensure tasks persist safely across app restarts.
  * Communicates repository actions asynchronously through clean abstraction boundaries (`ITodoRepository` and `TodoStore`).

---

## 3. Architecture & Project Structure
The project adheres strictly to the MVVM (Model-View-ViewModel) pattern and modular clean-code standards:

```text
LifeManagementApp/
├── Models/        TodoItem.cs, WeatherData.cs
├── ViewModels/    HomeViewModel.cs, TodoListViewModel.cs, AddTaskViewModel.cs, WeatherViewModel.cs
├── Views/         HomePage.xaml, TodoListPage.xaml, AddTaskPage.xaml, WeatherPage.xaml
├── Services/      IWeatherService.cs, WeatherService.cs, ITodoRepository.cs, EfTodoRepository.cs, TodoStore.cs, WeatherServiceException.cs
├── Data/          AppDbContext.cs
├── AppShell.xaml  (Shell navigation routing)
├── MauiProgram.cs (Dependency Injection container configuration)
└── README.md      (Project plan and documentation)
```
## Class / Architecture Diagram (Overview)
Views bind exclusively to ViewModels via data binding and x:DataType compiled bindings.

ViewModels execute commands using CommunityToolkit.Mvvm ([ObservableProperty], [RelayCommand]) and interact with Services (TodoStore, IWeatherService).

Services isolate data operations: EfTodoRepository interacts with the SQLite database via AppDbContext, while WeatherService fetches remote forecasts using an injected HttpClient.

## 4. How to Run the App
Clone the repository to your local machine.

Open the solution file (.sln) in Visual Studio 2022 (ensuring .NET MAUI workload is installed).

Select your preferred target framework (e.g., net9.0-windows10.0.19041 or an Android Emulator).

Press F5 or click the Start Debugging button to build and run the application.

## 5. AI Use
During the development of this project, artificial intelligence tools (such as large language models) were utilized for the following purposes:

Debugging & Exception Resolution: Troubleshooting C# compiler errors, XAML namespace mappings, and asynchronous data-binding lifecycle events.

Boilerplate Structuring: Formatting MVVM properties and organizing LINQ queries for the Top 3 priority-based sorting logic.

Documentation Support: Assisting in structuring and formatting this README.md file according to the course template specifications.

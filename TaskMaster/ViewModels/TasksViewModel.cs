using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Models;
using TaskMaster.Data;
using TaskMaster.Services;

namespace TaskMaster.ViewModels
{
    public partial class TasksViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private List<TaskItem> tasks;

        public TasksViewModel(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
            LoadTasks();
        }

        [RelayCommand]
        public async Task LoadTasksAsync()
        {
            try
            {
                var currentUser = _authService.CurrentUser;
                if (currentUser != null)
                {
                    Tasks = await Task.Run(() => _context.Tasks
                        .Where(t => t.Id_Auteur == currentUser.Id_User || t.Id_Realisateur == currentUser.Id_User)
                        .ToList());
                }
                else
                {
                    Tasks = new List<TaskItem>();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", "Impossible de charger les tâches : " + ex.Message, "OK");
            }
        }

        private void LoadTasks()
        {
            var currentUser = _authService.CurrentUser;
            if (currentUser != null)
            {
                Tasks = _context.Tasks
                    .Where(t => t.Id_Auteur == currentUser.Id_User || t.Id_Realisateur == currentUser.Id_User)
                    .ToList();
            }
            else
            {
                Tasks = new List<TaskItem>();
            }
        }

        [RelayCommand]
        private async Task NavigateToCreateTaskAsync()
        {
            try
            {
                await Shell.Current.GoToAsync("//CreateTaskPage");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", "Impossible de naviguer vers la page de création : " + ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task TaskSelectedAsync(TaskItem task)
        {
            if (task != null)
            {
                // TODO: Naviguer vers la page de détails de la tâche
            }
        }
    }
} 
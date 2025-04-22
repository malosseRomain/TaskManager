using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Models;
using TaskMaster.Data;

namespace TaskMaster.ViewModels
{
    public partial class TasksViewModel : ObservableObject
    {
        private readonly AppDbContext _context;

        [ObservableProperty]
        private List<Models.Task> tasks;

        public TasksViewModel(AppDbContext context)
        {
            _context = context;
            LoadTasks();
        }

        private void LoadTasks()
        {
            Tasks = _context.Tasks.ToList();
        }

        [RelayCommand]
        private async System.Threading.Tasks.Task NavigateToCreateTaskAsync()
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
        private async System.Threading.Tasks.Task TaskSelectedAsync(Models.Task task)
        {
            if (task != null)
            {
                // TODO: Naviguer vers la page de détails de la tâche
            }
        }
    }
} 
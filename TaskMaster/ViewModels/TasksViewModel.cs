using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Models;
using TaskMaster.Data;
using TaskMaster.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Diagnostics;
using TaskMaster.Views;

namespace TaskMaster.ViewModels
{
    public partial class TasksViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;
        private readonly ITaskService _taskService;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private ObservableCollection<TaskItem> tasks;

        [ObservableProperty]
        private ObservableCollection<TaskItem> filteredTasks;

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private string selectedSortOption;

        [ObservableProperty]
        private string sortOrder;

        [ObservableProperty]
        private string searchType;

        [ObservableProperty]
        private bool _isBusy;

        public ICommand SortByPriorityCommand { get; }
        public ICommand SortByDueDateCommand { get; }
        public ICommand SortByCategoryCommand { get; }

        public TasksViewModel(AppDbContext context, IAuthService authService, ITaskService taskService, INavigationService navigationService)
        {
            _context = context;
            _authService = authService;
            _taskService = taskService;
            _navigationService = navigationService;
            Tasks = new ObservableCollection<TaskItem>();
            FilteredTasks = new ObservableCollection<TaskItem>();
            
            // Valeurs par défaut
            SelectedSortOption = "Titre";
            SortOrder = "Croissant";
            SearchType = "Tout";
            LoadTasks();
            SortByPriorityCommand = new Command(SortByPriority);
            SortByDueDateCommand = new Command(SortByDueDate);
            SortByCategoryCommand = new Command(SortByCategory);
        }

        partial void OnSearchTextChanged(string value)
        {
            ApplyFiltersAndSort();
        }

        partial void OnSelectedSortOptionChanged(string value)
        {
            ApplyFiltersAndSort();
        }

        partial void OnSortOrderChanged(string value)
        {
            ApplyFiltersAndSort();
        }

        partial void OnSearchTypeChanged(string value)
        {
            ApplyFiltersAndSort();
        }

        private void ApplyFiltersAndSort()
        {
            var query = Tasks.AsQueryable();

            // Appliquer le filtre de recherche
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var searchLower = SearchText.ToLower();
                switch (SearchType)
                {
                    case "Titre":
                        query = query.Where(t => t.Titre.ToLower().Contains(searchLower));
                        break;
                    case "Description":
                        query = query.Where(t => t.Description.ToLower().Contains(searchLower));
                        break;
                    case "Étiquettes":
                        query = query.Where(t => t.Etiquettes.ToLower().Contains(searchLower));
                        break;
                    default: // "Tout"
                        query = query.Where(t => 
                            t.Titre.ToLower().Contains(searchLower) ||
                            t.Description.ToLower().Contains(searchLower) ||
                            t.Etiquettes.ToLower().Contains(searchLower));
                        break;
                }
            }

            // Appliquer le tri
            var isDescending = SortOrder == "Décroissant";
            switch (SelectedSortOption)
            {
                case "Titre":
                    query = isDescending 
                        ? query.OrderByDescending(t => t.Titre)
                        : query.OrderBy(t => t.Titre);
                    break;
                case "Priorité":
                    query = isDescending 
                        ? query.OrderByDescending(t => t.Priorite)
                        : query.OrderBy(t => t.Priorite);
                    break;
                case "Échéance":
                    query = isDescending 
                        ? query.OrderByDescending(t => t.Echeance)
                        : query.OrderBy(t => t.Echeance);
                    break;
                case "Catégorie":
                    query = isDescending 
                        ? query.OrderByDescending(t => t.Categorie)
                        : query.OrderBy(t => t.Categorie);
                    break;
            }

            // Mettre à jour la liste filtrée
            FilteredTasks = new ObservableCollection<TaskItem>(query.ToList());
        }

        [RelayCommand]
        public async Task LoadTasksAsync()
        {
            try
            {
                var loadedTasks = await _taskService.GetAllTasksAsync();
                Tasks = new ObservableCollection<TaskItem>(loadedTasks);
                ApplyFiltersAndSort();
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
                Tasks = new ObservableCollection<TaskItem>(_context.Tasks
                    .Where(t => t.Id_Auteur == currentUser.Id_User || t.Id_Realisateur == currentUser.Id_User)
                    .ToList());
            }
            else
            {
                Tasks = new ObservableCollection<TaskItem>();
            }
        }

        [RelayCommand]
        private async Task NavigateToCreateTask()
        {
            if (IsBusy) return;
            
            try
            {
                IsBusy = true;
                await Shell.Current.GoToAsync(nameof(CreateTaskPage));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Erreur", "Impossible d'ouvrir la page", "OK");
            }
            finally
            {
                IsBusy = false;
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

        [RelayCommand]
        private async Task DeleteTaskAsync(TaskItem task)
        {
            if (task != null)
            {
                bool confirm = await Shell.Current.DisplayAlert("Confirmation", "Voulez-vous vraiment supprimer cette tâche ?", "Oui", "Non");
                if (confirm)
                {
                    try
                    {
                        // Récupère l'instance suivie par le contexte
                        var trackedTask = await _context.Tasks.FindAsync(task.Id_Task);
                        if (trackedTask != null)
                        {
                            _context.Tasks.Remove(trackedTask);
                            await _context.SaveChangesAsync();
                            await LoadTasksAsync();
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Erreur", "Tâche introuvable", "OK");
                        }
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Erreur", "Impossible de supprimer la tâche : " + ex.Message, "OK");
                    }
                }
            }
        }

        [RelayCommand]
        private async Task ModifyTaskAsync(TaskItem task)
        {
            if (task == null)
                return;

            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "taskId", task.Id_Task }
                };
                
                await Shell.Current.GoToAsync($"ModifyTaskPage", parameters);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Impossible d'ouvrir la page de modification : {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task ViewDetailsAsync(TaskItem task)
        {
            if (task == null)
                return;

            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "taskId", task.Id_Task }
                };
                await Shell.Current.GoToAsync($"TaskDetailsPage", parameters);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Impossible d'afficher les détails : {ex.Message}", "OK");
            }
        }

        private void SortByPriority()
        {
            var sortedTasks = Tasks.OrderBy(t => t.Priorite).ToList();
            Tasks.Clear();
            foreach (var task in sortedTasks)
            {
                Tasks.Add(task);
            }
        }

        private void SortByDueDate()
        {
            var sortedTasks = Tasks.OrderBy(t => t.Echeance).ToList();
            Tasks.Clear();
            foreach (var task in sortedTasks)
            {
                Tasks.Add(task);
            }
        }

        private void SortByCategory()
        {
            var sortedTasks = Tasks.OrderBy(t => t.Categorie).ToList();
            Tasks.Clear();
            foreach (var task in sortedTasks)
            {
                Tasks.Add(task);
            }
        }

        // Nouvelle méthode pour mettre à jour une tâche spécifique
        public async Task UpdateTaskInListAsync(TaskItem modifiedTask)
        {
            var taskToUpdate = Tasks.FirstOrDefault(t => t.Id_Task == modifiedTask.Id_Task);
            if (taskToUpdate != null)
            {
                Tasks.Remove(taskToUpdate);
                Tasks.Add(modifiedTask);

                // Mise à jour dans FilteredTasks
                var filteredTaskToUpdate = FilteredTasks.FirstOrDefault(t => t.Id_Task == modifiedTask.Id_Task);
                if (filteredTaskToUpdate != null)
                {
                    FilteredTasks.Remove(filteredTaskToUpdate);
                    FilteredTasks.Add(modifiedTask);
                }

                // Notifier les changements
                OnPropertyChanged(nameof(Tasks));
                OnPropertyChanged(nameof(FilteredTasks));
            }
        }

        // Méthode pour rafraîchir complètement la liste
        [RelayCommand]
        public async Task RefreshTasksAsync()
        {
            try
            {
                var loadedTasks = await _taskService.GetAllTasksAsync();
                Tasks = new ObservableCollection<TaskItem>(loadedTasks);
                ApplyFiltersAndSort();
                OnPropertyChanged(nameof(Tasks));
                OnPropertyChanged(nameof(FilteredTasks));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", "Impossible de rafraîchir les tâches : " + ex.Message, "OK");
            }
        }

        public void ReloadTasks()
        {
            var currentUser = _authService.CurrentUser;
            if (currentUser != null)
            {
                Tasks = new ObservableCollection<TaskItem>(
                    _context.Tasks
                        .Where(t => t.Id_Auteur == currentUser.Id_User || t.Id_Realisateur == currentUser.Id_User)
                        .ToList()
                );
                ApplyFiltersAndSort(); // Met à jour FilteredTasks
            }
            else
            {
                Tasks.Clear();
            }
            OnPropertyChanged(nameof(Tasks));
            OnPropertyChanged(nameof(FilteredTasks));
        }

        public void ForceReload()
        {
            Tasks = new ObservableCollection<TaskItem>(
                _context.Tasks
                    .Where(t => t.Id_Auteur == _authService.CurrentUser.Id_User || 
                               t.Id_Realisateur == _authService.CurrentUser.Id_User)
                    .AsNoTracking() // Important pour éviter les conflits
                    .ToList()
            );
            ApplyFiltersAndSort();
            OnPropertyChanged(nameof(Tasks));
            OnPropertyChanged(nameof(FilteredTasks));
            Debug.WriteLine("Forced reload executed"); // Vérifiez dans la console
        }

        public void HardRefresh()
        {
            // 1. Détache toutes les entités suivies
            _context.ChangeTracker.Clear();

            // 2. Recharge avec une nouvelle requête
            var freshData = _context.Tasks
                .Where(t => t.Id_Auteur == _authService.CurrentUser.Id_User)
                .AsNoTracking()
                .ToList();

            // 3. Réinitialise complètement les collections
            Tasks.Clear();
            foreach (var item in freshData) Tasks.Add(item);
            
            ApplyFiltersAndSort();
            
            // 4. Force la mise à jour de l'UI
            OnPropertyChanged(nameof(Tasks));
            OnPropertyChanged(nameof(FilteredTasks));
            
            Debug.WriteLine($"HARD REFRESH - {Tasks.Count} tâches chargées");
        }

        public void RefreshTaskList()
        {
            var currentUser = _authService.CurrentUser;
            if (currentUser == null) return;

            Tasks = new ObservableCollection<TaskItem>(
                _context.Tasks
                    .Where(t => t.Id_Auteur == currentUser.Id_User)
                    .AsNoTracking()
                    .ToList()
            );
            ApplyFiltersAndSort();
        }
    }
} 
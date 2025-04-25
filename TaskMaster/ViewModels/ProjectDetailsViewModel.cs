using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Models;
using TaskMaster.Data;
using TaskMaster.Services;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace TaskMaster.ViewModels
{
    public partial class ProjectDetailsViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private Projet projet;

        [ObservableProperty]
        private ObservableCollection<TaskItem> taches = new();

        public ProjectDetailsViewModel(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task LoadProjectAsync(int projectId)
        {
            Projet = await _context.Projets
                .FirstOrDefaultAsync(p => p.Id_Projet == projectId);

            if (Projet != null)
            {
                var taches = await _context.Tasks
                    .Where(t => t.Id_Projet == projectId)
                    .ToListAsync();

                Taches.Clear();
                foreach (var tache in taches)
                {
                    Taches.Add(tache);
                }
            }
        }

        [RelayCommand]
        private async Task CreerTache()
        {
            await Shell.Current.GoToAsync($"//CreateTaskPage?projectId={Projet.Id_Projet}");
        }

        [RelayCommand]
        private async Task NaviguerVersTache(TaskItem tache)
        {
            // Temporairement désactivé
            // if (tache == null) return;
            // await Shell.Current.GoToAsync($"//TaskDetailsPage?taskId={tache.Id_Task}");
        }
    }
} 
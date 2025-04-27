using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Models;
using TaskMaster.Data;
using System.Collections.ObjectModel;
using TaskStatus = TaskMaster.Models.TaskStatus;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace TaskMaster.ViewModels
{
    public partial class ModifyTaskViewModel : ObservableObject, IQueryAttributable
    {
        private readonly AppDbContext _context;
        private readonly TasksViewModel _tasksViewModel;

        [ObservableProperty]
        private TaskItem task;

        public List<TaskCategory> Categories { get; } = Enum.GetValues(typeof(TaskCategory)).Cast<TaskCategory>().ToList();
        public List<TaskPriority> Priorities { get; } = Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>().ToList();
        public List<TaskMaster.Models.TaskStatus> Statuses { get; } = 
            Enum.GetValues(typeof(TaskMaster.Models.TaskStatus))
                .Cast<TaskMaster.Models.TaskStatus>()
                .ToList();

        public ObservableCollection<SubTaskViewModel> SousTaches { get; set; } = new();
        public ObservableCollection<CommentViewModel> Commentaires { get; set; } = new();
        public ObservableCollection<Projet> Projets { get; set; } = new();
        [ObservableProperty]
        private Projet selectedProjet;
        public ObservableCollection<UserDisplay> Utilisateurs { get; set; } = new();
        [ObservableProperty]
        private UserDisplay selectedUtilisateur;
        public string Etiquettes { get; set; }

        public ModifyTaskViewModel(AppDbContext context, TasksViewModel tasksViewModel)
        {
            _context = context;
            _tasksViewModel = tasksViewModel;
            Task = new TaskItem();
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("taskId", out var value) && value is int taskId)
            {
                await InitialiserAsync(taskId);
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (Task == null || Task.Id_Task == 0)
            {
                await Shell.Current.DisplayAlert("Erreur", "Tâche invalide", "OK");
                return;
            }

            try
            {
                var existingTask = await _context.Tasks.FindAsync(Task.Id_Task);
                if (existingTask == null)
                {
                    await Shell.Current.DisplayAlert("Erreur", "Tâche introuvable", "OK");
                    return;
                }

                existingTask.Titre = Task.Titre;
                existingTask.Description = Task.Description;
                existingTask.Echeance = Task.Echeance;
                existingTask.Categorie = Task.Categorie;
                existingTask.Priorite = Task.Priorite;
                existingTask.Statut = Task.Statut;
                existingTask.Etiquettes = Task.Etiquettes;
                existingTask.Id_Projet = Task.Id_Projet;
                existingTask.Id_Realisateur = Task.Id_Realisateur;

                // --- Gestion des sous-tâches ---
                // Suppression des sous-tâches retirées
                var sousTachesToRemove = Task.SousTaches
                    .Where(st => !SousTaches.Any(vm => vm.Id_SubTask == st.Id_SubTask))
                    .ToList();
                foreach (var st in sousTachesToRemove)
                    _context.SubTasks.Remove(st);

                // Ajout ou modification des sous-tâches
                foreach (var vm in SousTaches)
                {
                    var existing = Task.SousTaches.FirstOrDefault(st => st.Id_SubTask == vm.Id_SubTask);
                    if (existing != null)
                    {
                        // Modification
                        existing.Titre = vm.Titre;
                        existing.Echeance = vm.Echeance;
                        // etc.
                    }
                    else
                    {
                        // Ajout
                        Task.SousTaches.Add(new SubTask
                        {
                            Titre = vm.Titre,
                            Echeance = vm.Echeance,
                            Statut = TaskStatus.Afaire, // ou autre valeur par défaut
                            Id_TaskParent = Task.Id_Task
                        });
                    }
                }

                // --- Gestion des commentaires ---
                // Même logique que pour les sous-tâches

                await _context.SaveChangesAsync();
                
                // Forcer un rafraîchissement complet
                await _tasksViewModel.RefreshTasksAsync();
                
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Erreur lors de la sauvegarde : {ex.Message}", "OK");
            }
        }

        partial void OnTaskChanged(TaskItem value)
        {
            SousTaches.Clear();
            if (value?.SousTaches != null)
            {
                foreach (var st in value.SousTaches)
                {
                    SousTaches.Add(new SubTaskViewModel
                    {
                        Id_SubTask = st.Id_SubTask,
                        Titre = st.Titre,
                        Echeance = st.Echeance
                    });
                }
            }

            Commentaires.Clear();
            if (value?.Commentaires != null)
            {
                foreach (var c in value.Commentaires)
                {
                    Commentaires.Add(new CommentViewModel
                    {
                        Contenu = c.Contenu
                    });
                }
            }

            // Synchroniser les autres champs
            Etiquettes = value?.Etiquettes ?? string.Empty;

            // Projet sélectionné
            if (value?.Id_Projet != null && Projets.Any())
                SelectedProjet = Projets.FirstOrDefault(p => p.Id_Projet == value.Id_Projet);
            else
                SelectedProjet = null;

            // Utilisateur assigné
            if (value?.Id_Realisateur != null && Utilisateurs.Any())
                SelectedUtilisateur = Utilisateurs.FirstOrDefault(u => u.Id == value.Id_Realisateur);
            else
                SelectedUtilisateur = null;

            System.Diagnostics.Debug.WriteLine($"Id_Projet de la tâche : {value?.Id_Projet}");
            foreach (var p in Projets)
                System.Diagnostics.Debug.WriteLine($"Projet: {p.Id_Projet} - {p.Nom}");

            System.Diagnostics.Debug.WriteLine($"Id_Realisateur de la tâche : {value?.Id_Realisateur}");
            foreach (var u in Utilisateurs)
                System.Diagnostics.Debug.WriteLine($"Utilisateur: {u.Id} - {u.DisplayName}");
        }

        private async Task ChargerProjets()
        {
            var projets = await _context.Projets.ToListAsync();
            Projets.Clear();
            foreach (var projet in projets)
                Projets.Add(projet);
        }

        private async Task ChargerUtilisateurs()
        {
            var users = await _context.Users.ToListAsync();
            Utilisateurs.Clear();
            foreach (var user in users)
            {
                Utilisateurs.Add(new UserDisplay
                {
                    Id = user.Id_User,
                    DisplayName = $"{user.Prenom} {user.Nom}"
                });
            }
        }

        public async Task InitialiserAsync(int taskId)
        {
            await ChargerProjets();
            await ChargerUtilisateurs();

            Task = await _context.Tasks
                .Include(t => t.SousTaches)
                .Include(t => t.Commentaires)
                .FirstOrDefaultAsync(t => t.Id_Task == taskId);
            // OnTaskChanged sera appelé automatiquement
        }
    }
}

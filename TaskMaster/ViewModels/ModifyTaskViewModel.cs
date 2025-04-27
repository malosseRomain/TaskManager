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
        public Projet SelectedProjet { get; set; }
        public ObservableCollection<UserDisplay> Utilisateurs { get; set; } = new();
        public UserDisplay SelectedUtilisateur { get; set; }
        public string Etiquettes { get; set; }

        public ModifyTaskViewModel(AppDbContext context, TasksViewModel tasksViewModel)
        {
            _context = context;
            _tasksViewModel = tasksViewModel;
            Task = new TaskItem();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("taskId", out var value) && value is int taskId)
            {
                Task = _context.Tasks
                    .Include(t => t.SousTaches)
                    .Include(t => t.Commentaires)
                    .FirstOrDefault(t => t.Id_Task == taskId);
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
        }
    }
}

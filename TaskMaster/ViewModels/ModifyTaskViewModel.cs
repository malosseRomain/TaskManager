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

        public ObservableCollection<SubTask> SousTaches { get; set; } = new();
        public ObservableCollection<Commentaire> Commentaires { get; set; } = new();
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
            try
            {
                // 1. Mettez à jour les sous-tâches
                Task.SousTaches = SousTaches
                    .Where(st => !string.IsNullOrEmpty(st.Titre))
                    .ToList();

                // 2. Mettez à jour les commentaires
                Task.Commentaires = Commentaires
                    .Where(c => !string.IsNullOrEmpty(c.Contenu) && Utilisateurs.Any(u => u.Id == c.Id_Auteur))
                    .ToList();

                await _context.SaveChangesAsync();
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.InnerException?.Message ?? ex.Message, "OK");
            }
        }

        [RelayCommand]
        private void AjouterSousTache()
        {
            SousTaches.Add(new SubTask
            {
                Titre = "",
                Id_TaskParent = Task.Id_Task,
                Statut = TaskStatus.Afaire,
                Echeance = DateTime.Now
            });
        }

        [RelayCommand]
        private void AjouterCommentaire()
        {
            if (SelectedUtilisateur == null)
            {
                Shell.Current.DisplayAlert("Erreur", "Sélectionnez un utilisateur", "OK");
                return;
            }

            Commentaires.Add(new Commentaire
            {
                Contenu = "",
                Id_Task = Task.Id_Task,
                Id_Auteur = SelectedUtilisateur.Id,
                DateCreation = DateTime.Now
            });
        }

        [RelayCommand]
        private void SupprimerSousTache(SubTask sousTache)
        {
            if (sousTache != null)
            {
                SousTaches.Remove(sousTache);
                
                // Supprimez également la sous-tâche de la base de données si elle existe déjà
                var sousTacheExistante = Task.SousTaches.FirstOrDefault(st => st.Id_SubTask == sousTache.Id_SubTask);
                if (sousTacheExistante != null)
                {
                    Task.SousTaches.Remove(sousTacheExistante);
                }
            }
        }

        [RelayCommand]
        private void SupprimerCommentaire(Commentaire commentaire)
        {
            try
            {
                if (commentaire == null) return;
                
                // Supprime de la liste observable
                Commentaires.Remove(commentaire);
                
                // Si le commentaire existe en base, le marque pour suppression
                if (commentaire.Id_Commentaire != 0)
                {
                    _context.Commentaires.Remove(commentaire);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur suppression commentaire: {ex.Message}");
            }
        }

        partial void OnTaskChanged(TaskItem value)
        {
            // Sous-tâches
            SousTaches.Clear();
            if (value?.SousTaches != null)
            {
                foreach (var st in value.SousTaches)
                {
                    SousTaches.Add(new SubTask
                    {
                        Id_SubTask = st.Id_SubTask,
                        Titre = st.Titre,
                        Echeance = st.Echeance,
                        Id_TaskParent = st.Id_TaskParent,
                        Statut = st.Statut
                    });
                }
            }

            // Commentaires
            Commentaires.Clear();
            if (value?.Commentaires != null)
            {
                foreach (var c in value.Commentaires)
                {
                    Commentaires.Add(new Commentaire
                    {
                        Id_Commentaire = c.Id_Commentaire,
                        Contenu = c.Contenu,
                        Id_Task = c.Id_Task,
                        Id_Auteur = c.Id_Auteur,
                        DateCreation = c.DateCreation
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
            await ChargerUtilisateurs();
            await ChargerProjets();

            Task = await _context.Tasks
                .Include(t => t.SousTaches)
                .Include(t => t.Commentaires)
                .FirstOrDefaultAsync(t => t.Id_Task == taskId);

            // Assignez l'utilisateur par défaut si la tâche en a un
            if (Task.Id_Realisateur != null)
            {
                SelectedUtilisateur = Utilisateurs.FirstOrDefault(u => u.Id == Task.Id_Realisateur);
            }
        }
    }
}

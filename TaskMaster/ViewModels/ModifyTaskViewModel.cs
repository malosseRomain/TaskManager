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
            try
            {
                // Désactivez le suivi des entités pour éviter les conflits
                _context.ChangeTracker.Clear();

                // Mettez à jour les sous-tâches
                foreach (var stViewModel in SousTaches)
                {
                    var existingSubTask = Task.SousTaches.FirstOrDefault(st => st.Id_SubTask == stViewModel.Id_SubTask);
                    if (existingSubTask != null)
                    {
                        existingSubTask.Titre = stViewModel.Titre;
                        existingSubTask.Echeance = stViewModel.Echeance;
                        _context.Entry(existingSubTask).State = EntityState.Modified;
                    }
                    else
                    {
                        Task.SousTaches.Add(new SubTask
                        {
                            Titre = stViewModel.Titre,
                            Echeance = stViewModel.Echeance,
                            Id_TaskParent = Task.Id_Task
                        });
                    }
                }

                // Mettez à jour les commentaires
                foreach (var cViewModel in Commentaires)
                {
                    var commentaireExistant = Task.Commentaires.FirstOrDefault(c => c.Contenu == cViewModel.Contenu);
                    if (commentaireExistant == null)
                    {
                        Task.Commentaires.Add(new Commentaire
                        {
                            Contenu = cViewModel.Contenu,
                            Id_Task = Task.Id_Task,
                            Id_Auteur = 1, // REMPLACEZ PAR UN ID VALIDE
                            DateCreation = DateTime.Now
                        });
                    }
                }

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
            SousTaches.Add(new SubTaskViewModel
            {
                Titre = "",
                Echeance = DateTime.Now
            });
        }

        [RelayCommand]
        private void AjouterCommentaire()
        {
            Commentaires.Add(new CommentViewModel
            {
                Contenu = ""
            });
        }

        [RelayCommand]
        private void SupprimerSousTache(SubTaskViewModel sousTache)
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
        private void SupprimerCommentaire(CommentViewModel commentaire)
        {
            if (commentaire != null)
            {
                // 1. Supprimez le CommentViewModel de la liste observable
                Commentaires.Remove(commentaire);

                // 2. Trouvez et marquez le Commentaire associé pour suppression
                var commentaireExistant = Task.Commentaires.FirstOrDefault(c => c.Contenu == commentaire.Contenu && c.Id_Auteur == 1); // Adaptez Id_Auteur si nécessaire
                if (commentaireExistant != null)
                {
                    _context.Commentaires.Remove(commentaireExistant); // Suppression explicite du contexte
                }
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

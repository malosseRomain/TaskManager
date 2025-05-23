using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Models;
using TaskMaster.Data;
using TaskStatus = TaskMaster.Models.TaskStatus;
using TaskMaster.Services;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using TaskMaster.Views;
using System.Linq;

namespace TaskMaster.ViewModels
{
    public partial class CreateTaskViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string titre;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private DateTime dateEcheance = DateTime.Now.AddDays(7);

        [ObservableProperty]
        private string selectedCategorie;

        [ObservableProperty]
        private string selectedPriorite;

        [ObservableProperty]
        private StatusDisplay selectedStatut;

        [ObservableProperty]
        private string etiquettes = string.Empty;

        [ObservableProperty]
        private ObservableCollection<SubTaskViewModel> sousTaches = new();

        [ObservableProperty]
        private ObservableCollection<CommentViewModel> commentaires = new();

        [ObservableProperty]
        private ObservableCollection<Projet> projets = new();

        [ObservableProperty]
        private Projet selectedProjet;

        [ObservableProperty]
        private ObservableCollection<UserDisplay> utilisateurs = new();

        [ObservableProperty]
        private UserDisplay selectedUtilisateur;

        [ObservableProperty]
        private bool isBusy;

        public List<string> Categories { get; } = Enum.GetNames(typeof(TaskCategory)).ToList();
        public List<string> Priorites { get; } = Enum.GetNames(typeof(TaskPriority)).ToList();
        public List<StatusDisplay> Statuts { get; } = StatusDisplay.GetStatusDisplays();

        public CreateTaskViewModel(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
            InitialiserFormulaire();
            Task.Run(async () =>
            {
                await ChargerProjets();
                await ChargerUtilisateurs();
            }).Wait();
        }

        private async Task ChargerProjets()
        {
            var projets = await _context.Projets.ToListAsync();

            Projets.Clear();
            foreach (var projet in projets)
            {
                Projets.Add(projet);
            }
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

        private void InitialiserFormulaire()
        {
            Titre = string.Empty;
            Description = string.Empty;
            DateEcheance = DateTime.Now.AddDays(7);
            SelectedCategorie = TaskCategory.Travail.ToString();
            SelectedPriorite = TaskPriority.Moyenne.ToString();
            SelectedStatut = Statuts.First(s => s.Value == TaskStatus.Afaire.ToString());
            Etiquettes = string.Empty;
            SousTaches.Clear();
            Commentaires.Clear();
            SelectedProjet = null;
            SelectedUtilisateur = null;
        }

        [RelayCommand]
        private void AjouterSousTache()
        {
            SousTaches.Add(new SubTaskViewModel());
        }

        [RelayCommand]
        private void SupprimerSousTache(SubTaskViewModel sousTache)
        {
            SousTaches.Remove(sousTache);
        }

        [RelayCommand]
        private void AjouterCommentaire()
        {
            Commentaires.Add(new CommentViewModel());
        }

        [RelayCommand]
        private void SupprimerCommentaire(CommentViewModel commentaire)
        {
            Commentaires.Remove(commentaire);
        }

        [RelayCommand]
        public async Task CreateTaskAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                var currentUser = _authService.CurrentUser;
                if (currentUser == null)
                {
                    await Shell.Current.DisplayAlert("Erreur", "Vous devez être connecté pour créer une tâche", "OK");
                    return;
                }

                var task = new TaskItem
                {
                    Titre = Titre,
                    Description = Description,
                    Echeance = DateEcheance,
                    Categorie = Enum.Parse<TaskCategory>(SelectedCategorie),
                    Priorite = Enum.Parse<TaskPriority>(SelectedPriorite),
                    Statut = Enum.Parse<TaskStatus>(SelectedStatut.Value),
                    DateCreation = DateTime.Now,
                    Id_Auteur = currentUser.Id_User,
                    Id_Realisateur = SelectedUtilisateur?.Id ?? currentUser.Id_User,
                    Etiquettes = Etiquettes,
                    Id_Projet = SelectedProjet?.Id_Projet,
                    SousTaches = SousTaches.Select(st => new SubTask
                    {
                        Titre = st.Titre,
                        Statut = TaskStatus.Afaire,
                        Echeance = st.Echeance
                    }).ToList(),
                    Commentaires = Commentaires.Select(c => new Commentaire
                    {
                        Contenu = c.Contenu,
                        DateCreation = DateTime.Now,
                        Id_Auteur = currentUser.Id_User
                    }).ToList()
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                await Shell.Current.DisplayAlert("Succès", "Tâche créée avec succès !", "OK");
                InitialiserFormulaire();

                if (SelectedProjet != null)
                {
                    await Shell.Current.GoToAsync($"//ProjectsPage");
                    var viewModel = new ProjectDetailsViewModel(_context, _authService);
                    await viewModel.LoadProjectAsync(SelectedProjet.Id_Projet);
                    var page = new ProjectDetailsPage(viewModel);
                    await Shell.Current.Navigation.PushAsync(page);
                }
                else
                {
                    await Shell.Current.GoToAsync("//TasksPage");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
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
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SupprimerTacheAsync(TaskItem task)
        {
            if (task == null || IsBusy) return;

            try
            {
                IsBusy = true;

                // Détacher l'entité si elle est déjà suivie
                var existingTask = _context.Tasks.Local.FirstOrDefault(t => t.Id_Task == task.Id_Task);
                if (existingTask != null)
                {
                    _context.Entry(existingTask).State = EntityState.Detached;
                }

                // Recharger l'entité depuis la base de données pour s'assurer qu'elle n'est pas suivie
                var taskToDelete = await _context.Tasks.FindAsync(task.Id_Task);
                if (taskToDelete != null)
                {
                    _context.Tasks.Remove(taskToDelete);
                    await _context.SaveChangesAsync();
                    await Shell.Current.DisplayAlert("Succès", "Tâche supprimée avec succès !", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Impossible de supprimer la tâche : {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    public partial class SubTaskViewModel : ObservableObject
    {
        public int? Id_SubTask { get; set; }

        public SubTaskViewModel()
        {
            Echeance = DateTime.Now.AddDays(7);
        }

        [ObservableProperty]
        private string titre;

        [ObservableProperty]
        private DateTime? echeance;
    }

    public partial class CommentViewModel : ObservableObject
    {
        [ObservableProperty]
        private string contenu;
    }

    public class UserDisplay
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
    }
} 
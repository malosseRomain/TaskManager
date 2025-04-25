using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Models;
using TaskMaster.Data;
using TaskStatus = TaskMaster.Models.TaskStatus;
using TaskMaster.Services;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using TaskMaster.Views;

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

        public List<string> Categories { get; } = Enum.GetNames(typeof(TaskCategory)).ToList();
        public List<string> Priorites { get; } = Enum.GetNames(typeof(TaskPriority)).ToList();
        public List<StatusDisplay> Statuts { get; } = StatusDisplay.GetStatusDisplays();

        public CreateTaskViewModel(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
            InitialiserFormulaire();
            ChargerProjets();
            ChargerUtilisateurs();
        }

        private async void ChargerProjets()
        {
            var currentUser = _authService.CurrentUser;
            if (currentUser == null) return;

            var projets = await _context.Projets
                .Where(p => p.Id_Createur == currentUser.Id_User)
                .ToListAsync();

            Projets.Clear();
            foreach (var projet in projets)
            {
                Projets.Add(projet);
            }
        }

        private async void ChargerUtilisateurs()
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
            try
            {
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
                    Id_Realisateur = currentUser.Id_User,
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
        }
    }

    public partial class SubTaskViewModel : ObservableObject
    {
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
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Models;
using TaskMaster.Data;
using TaskStatus = TaskMaster.Models.TaskStatus;
using TaskMaster.Services;
using System.Collections.ObjectModel;

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

        public List<string> Categories { get; } = Enum.GetNames(typeof(TaskCategory)).ToList();
        public List<string> Priorites { get; } = Enum.GetNames(typeof(TaskPriority)).ToList();
        public List<StatusDisplay> Statuts { get; } = StatusDisplay.GetStatusDisplays();

        public CreateTaskViewModel(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
            SelectedCategorie = TaskCategory.Travail.ToString();
            SelectedPriorite = TaskPriority.Moyenne.ToString();
            SelectedStatut = Statuts.First(s => s.Value == TaskStatus.Afaire.ToString());
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
                    SousTaches = SousTaches.Select(st => new SubTask
                    {
                        Titre = st.Titre,
                        Statut = TaskStatus.Afaire,
                        Echeance = st.Echeance
                    }).ToList()
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                await Shell.Current.DisplayAlert("Succès", "Tâche créée avec succès !", "OK");
                await Shell.Current.GoToAsync("//TasksPage");
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
} 
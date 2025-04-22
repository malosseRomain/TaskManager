using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Models;
using TaskMaster.Data;
using TaskStatus = TaskMaster.Models.TaskStatus;
using Task = TaskMaster.Models.Task;

namespace TaskMaster.ViewModels
{
    public partial class CreateTaskViewModel : ObservableObject
    {
        private readonly AppDbContext _context;

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
        private string selectedStatut;

        public List<string> Categories { get; } = Enum.GetNames(typeof(TaskCategory)).ToList();
        public List<string> Priorites { get; } = Enum.GetNames(typeof(TaskPriority)).ToList();
        public List<string> Statuts { get; } = Enum.GetNames(typeof(TaskStatus)).ToList();

        public CreateTaskViewModel(AppDbContext context)
        {
            _context = context;
            SelectedCategorie = TaskCategory.Travail.ToString();
            SelectedPriorite = TaskPriority.Moyenne.ToString();
            SelectedStatut = TaskStatus.Afaire.ToString();
        }

        [RelayCommand]
        public async System.Threading.Tasks.Task CreateTaskAsync()
        {
            try
            {
                var task = new Task
                {
                    Titre = Titre,
                    Description = Description,
                    Echeance = DateEcheance,
                    Categorie = Enum.Parse<TaskCategory>(SelectedCategorie),
                    Priorite = Enum.Parse<TaskPriority>(SelectedPriorite),
                    Statut = Enum.Parse<TaskStatus>(SelectedStatut),
                    DateCreation = DateTime.Now,
                    Id_Auteur = 2, // TODO: Remplacer par l'ID de l'utilisateur connecté
                    Id_Realisateur = 2 // TODO: Remplacer par l'ID de l'utilisateur connecté
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }
    }
} 
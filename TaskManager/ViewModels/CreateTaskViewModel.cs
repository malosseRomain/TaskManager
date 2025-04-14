using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TaskManager.Models;
using TaskManager.Data;
using TaskStatusEnum = TaskManager.Models.TaskStatus;
using TaskPriorityEnum = TaskManager.Models.TaskPriority;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TaskManager.ViewModels
{
    public class CreateTaskViewModel : INotifyPropertyChanged
    {
        // Propriétés pour la barre de navigation
        private double _navBarWidth = 300;
        public double NavBarWidth
        {
            get => _navBarWidth;
            set
            {
                _navBarWidth = value;
                OnPropertyChanged();
            }
        }

        private string _navBarToggleText = "←";
        public string NavBarToggleText
        {
            get => _navBarToggleText;
            set
            {
                _navBarToggleText = value;
                OnPropertyChanged();
            }
        }

        // Propriétés pour les filtres
        private TaskStatusEnum? _selectedStatus;
        public TaskStatusEnum? SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged();
            }
        }

        private TaskPriorityEnum? _selectedPriority;
        public TaskPriorityEnum? SelectedPriority
        {
            get => _selectedPriority;
            set
            {
                _selectedPriority = value;
                OnPropertyChanged();
            }
        }

        private string _filterCategory = string.Empty;
        public string FilterCategory
        {
            get => _filterCategory;
            set
            {
                _filterCategory = value;
                OnPropertyChanged();
            }
        }

        private string _filterTags = string.Empty;
        public string FilterTags
        {
            get => _filterTags;
            set
            {
                _filterTags = value;
                OnPropertyChanged();
            }
        }

        // Propriétés pour le tri
        public List<string> SortCriteriaList { get; } = new List<string>
        {
            "Titre",
            "Date d'échéance",
            "Priorité",
            "Statut",
            "Catégorie"
        };

        public List<string> SortOrderList { get; } = new List<string>
        {
            "Ascendant",
            "Descendant"
        };

        private string _selectedSortCriteria = "Titre";
        public string SelectedSortCriteria
        {
            get => _selectedSortCriteria;
            set
            {
                _selectedSortCriteria = value;
                OnPropertyChanged();
            }
        }

        private string _selectedSortOrder = "Ascendant";
        public string SelectedSortOrder
        {
            get => _selectedSortOrder;
            set
            {
                _selectedSortOrder = value;
                OnPropertyChanged();
            }
        }

        // Propriétés existantes
        public TaskItem NewTask { get; set; } = new TaskItem
        {
            Title = string.Empty,
            Description = string.Empty,
            Statut = TaskStatusEnum.Annulee,
            Priorite = TaskPriorityEnum.Basse,
            Auteur = new Person
            {
                Nom = "Auteur inconnu",
                Prenom = "Prénom inconnu",
                Email = "email@inconnu.com"
            },
            Realisateur = new Person
            {
                Nom = "Réalisateur inconnu",
                Prenom = "Prénom inconnu",
                Email = "email@inconnu.com"
            },
            Categorie = "Non spécifié",
            Etiquettes = new List<string>(),
            SousTaches = new List<SubTask>(),
            Commentaires = new List<Comment>(),
        };

        public string EtiquettesInput { get; set; } = string.Empty;
        public string SousTachesInput { get; set; } = string.Empty;
        public string CommentairesInput { get; set; } = string.Empty;

        public List<TaskStatusEnum> StatusList { get; } = Enum.GetValues(typeof(TaskStatusEnum)).Cast<TaskStatusEnum>().ToList();
        public List<TaskPriorityEnum> PriorityList { get; } = Enum.GetValues(typeof(TaskPriorityEnum)).Cast<TaskPriorityEnum>().ToList();

        // Commandes
        public ICommand SaveCommand { get; }
        public ICommand ToggleNavBarCommand { get; }
        public ICommand ApplyFiltersAndSortCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public CreateTaskViewModel()
        {
            SaveCommand = new Command(OnSave);
            ToggleNavBarCommand = new Command(ExecuteToggleNavBar);
            ApplyFiltersAndSortCommand = new Command(ExecuteApplyFiltersAndSort);
        }

        private void ExecuteToggleNavBar()
        {
            if (NavBarWidth > 0)
            {
                NavBarWidth = 0;
                NavBarToggleText = "→";
            }
            else
            {
                NavBarWidth = 300;
                NavBarToggleText = "←";
            }
        }

        private async void ExecuteApplyFiltersAndSort()
        {
            try
            {
                using var context = new TaskDbContext();
                var query = context.Tasks.AsQueryable();

                // Application des filtres
                if (SelectedStatus.HasValue)
                {
                    query = query.Where(t => t.Statut == SelectedStatus.Value);
                }

                if (SelectedPriority.HasValue)
                {
                    query = query.Where(t => t.Priorite == SelectedPriority.Value);
                }

                if (!string.IsNullOrWhiteSpace(FilterCategory))
                {
                    query = query.Where(t => t.Categorie.Contains(FilterCategory));
                }

                if (!string.IsNullOrWhiteSpace(FilterTags))
                {
                    var tags = FilterTags.Split(',').Select(t => t.Trim()).ToList();
                    query = query.Where(t => t.Etiquettes.Any(e => tags.Contains(e)));
                }

                // Application du tri
                switch (SelectedSortCriteria)
                {
                    case "Titre":
                        query = SelectedSortOrder == "Ascendant" 
                            ? query.OrderBy(t => t.Title)
                            : query.OrderByDescending(t => t.Title);
                        break;
                    case "Date d'échéance":
                        query = SelectedSortOrder == "Ascendant"
                            ? query.OrderBy(t => t.Echeance)
                            : query.OrderByDescending(t => t.Echeance);
                        break;
                    case "Priorité":
                        query = SelectedSortOrder == "Ascendant"
                            ? query.OrderBy(t => t.Priorite)
                            : query.OrderByDescending(t => t.Priorite);
                        break;
                    case "Statut":
                        query = SelectedSortOrder == "Ascendant"
                            ? query.OrderBy(t => t.Statut)
                            : query.OrderByDescending(t => t.Statut);
                        break;
                    case "Catégorie":
                        query = SelectedSortOrder == "Ascendant"
                            ? query.OrderBy(t => t.Categorie)
                            : query.OrderByDescending(t => t.Categorie);
                        break;
                }

                // Exécution de la requête
                var filteredTasks = await query.ToListAsync();
                
                // Vous pouvez ajouter ici la logique pour afficher les tâches filtrées
                await Application.Current.MainPage.DisplayAlert("Succès", $"{filteredTasks.Count} tâches trouvées", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", $"Erreur lors de l'application des filtres : {ex.Message}", "OK");
            }
        }

        private async void OnSave()
        {
            // ... Code existant de OnSave ...
        }

        void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
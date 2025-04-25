using TaskMaster.ViewModels;
using TaskMaster.Models;

namespace TaskMaster.Views
{
    public partial class TasksPage : ContentPage
    {
        private readonly TasksViewModel _viewModel;

        public TasksPage(TasksViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        private async void OnNewTaskClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync("//CreateTaskPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erreur", "Impossible de naviguer vers la page de création : " + ex.Message, "OK");
            }
        }

        private async void OnTaskSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is TaskItem selectedTask)
            {
                // Réinitialiser la sélection
                ((CollectionView)sender).SelectedItem = null;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadTasksAsync();
        }
    }
} 
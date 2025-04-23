using TaskMaster.ViewModels;
using TaskMaster.Models;

namespace TaskMaster.Views
{
    public partial class TasksPage : ContentPage
    {
        public TasksPage(TasksViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
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
                try
                {
                    // TODO: Naviguer vers la page de détails de la tâche
                    // await Shell.Current.GoToAsync($"//TaskDetailsPage?taskId={selectedTask.Id}");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erreur", "Impossible de naviguer vers les détails de la tâche : " + ex.Message, "OK");
                }
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is TasksViewModel viewModel)
            {
                await viewModel.LoadTasksAsync();
            }
        }
    }
} 
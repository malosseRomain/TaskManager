using TaskMaster.ViewModels;

namespace TaskMaster.Views
{
    public partial class ProjectDetailsPage : ContentPage
    {
        public ProjectDetailsPage(ProjectDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ProjectDetailsViewModel viewModel)
            {
                await viewModel.LoadProjectAsync(viewModel.Projet.Id_Projet);
            }
        }
    }
} 
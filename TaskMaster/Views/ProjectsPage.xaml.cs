using TaskMaster.ViewModels;

namespace TaskMaster.Views
{
    public partial class ProjectsPage : ContentPage
    {
        public ProjectsPage(ProjectsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void OnCreateProjectClicked(object sender, EventArgs e)
        {
            if (BindingContext is ProjectsViewModel viewModel)
            {
                await viewModel.CreerProjetAsync();
            }
        }
    }
} 
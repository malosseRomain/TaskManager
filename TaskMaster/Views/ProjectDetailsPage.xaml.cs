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
    }
} 
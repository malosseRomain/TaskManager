using TaskMaster.ViewModels;

namespace TaskMaster.Views
{
    public partial class CreateTaskPage : ContentPage
    {
        public CreateTaskPage(CreateTaskViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void OnCreateTaskClicked(object sender, EventArgs e)
        {
            if (BindingContext is CreateTaskViewModel viewModel)
            {
                try
                {
                    await viewModel.CreateTaskAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erreur", ex.Message, "OK");
                }
            }
        }
    }
} 
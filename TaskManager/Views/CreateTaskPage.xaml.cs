using TaskManager.ViewModels;

namespace TaskManager.Views;

public partial class CreateTaskPage : ContentPage
{
    public CreateTaskPage(CreateTaskViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

using TaskManager.ViewModels;

namespace TaskManager.Views;

public partial class CreateTaskPage : ContentPage
{
    public CreateTaskPage()
    {
        InitializeComponent();
        BindingContext = new CreateTaskViewModel();
    }
}

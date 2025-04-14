using TaskManager.ViewModels;

namespace TaskManager.Views;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
        BindingContext = new SignUpViewModel();
    }
}
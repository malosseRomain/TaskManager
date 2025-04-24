namespace TaskMaster.Views;
using TaskMaster.ViewModels;

public partial class TaskDetailsPage : ContentPage
{
	public TaskDetailsPage(TaskDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
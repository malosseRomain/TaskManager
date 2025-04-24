using TaskMaster.ViewModels;

namespace TaskMaster.Views;

public partial class ModifyTaskPage : ContentPage
{
	public ModifyTaskPage(ModifyTaskViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
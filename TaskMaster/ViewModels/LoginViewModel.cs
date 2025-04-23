using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Services;

namespace TaskMaster.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private string _email;
        private string _password;

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new Command(async () => await LoginAsync());
            RegisterCommand = new Command(async () => await RegisterAsync());
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Shell.Current.DisplayAlert("Erreur", "Veuillez remplir tous les champs", "OK");
                return;
            }

            try
            {
                if (await _authService.LoginAsync(Email, Password))
                {
                    await Shell.Current.GoToAsync("//TasksPage");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erreur", "Email ou mot de passe incorrect", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }

        private async Task RegisterAsync()
        {
            await Shell.Current.GoToAsync("//RegisterPage");
        }
    }
} 
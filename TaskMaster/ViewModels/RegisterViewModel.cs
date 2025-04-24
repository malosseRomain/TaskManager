using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Services;

namespace TaskMaster.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string nom;

        [ObservableProperty]
        private string prenom;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string confirmPassword;

        public RegisterViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Nom) || string.IsNullOrWhiteSpace(Prenom) || 
                    string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || 
                    string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    await Shell.Current.DisplayAlert("Erreur", "Tous les champs sont obligatoires", "OK");
                    return;
                }

                if (Password != ConfirmPassword)
                {
                    await Shell.Current.DisplayAlert("Erreur", "Les mots de passe ne correspondent pas", "OK");
                    return;
                }

                var success = await _authService.RegisterAsync(Nom, Prenom, Email, Password);
                if (success)
                {
                    await Shell.Current.DisplayAlert("Succès", "Inscription réussie !", "OK");
                    await Shell.Current.GoToAsync("//LoginPage");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Erreur", "Cet email est déjà utilisé", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task NavigateToLoginAsync()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
} 
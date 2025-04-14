using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace TaskManager.ViewModels
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        // Propriétés pour le formulaire
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        // Commandes
        public ICommand SignUpCommand { get; }
        public ICommand NavigateToLoginCommand { get; }

        public SignUpViewModel()
        {
            // Commande pour créer un compte
            SignUpCommand = new Command(OnSignUp);

            // Commande pour naviguer vers la page de connexion
            NavigateToLoginCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync("//LoginPage");
            });
        }

        private async void OnSignUp()
        {
            try
            {
                // Validation des champs
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) ||
                    string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", "Tous les champs sont obligatoires.", "OK");
                    return;
                }

                if (Password != ConfirmPassword)
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur", "Les mots de passe ne correspondent pas.", "OK");
                    return;
                }

                // Logique pour enregistrer l'utilisateur (exemple)
                await Application.Current.MainPage.DisplayAlert("Succès", "Compte créé avec succès !", "OK");

                // Redirection vers la page de connexion
                await Shell.Current.GoToAsync("//LoginPage");
            }
            catch (Exception ex)
            {
                // Gestion des erreurs
                await Application.Current.MainPage.DisplayAlert("Erreur", $"Une erreur s'est produite : {ex.Message}", "OK");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

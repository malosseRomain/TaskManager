using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaskManager.Views;

namespace TaskManager.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _email = string.Empty;
        private string _password = string.Empty;

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand NavigateToSignUpCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLogin);
            NavigateToSignUpCommand = new Command(async () => await Shell.Current.GoToAsync("//SignUpPage"));
        }

        private async void OnLogin()
        {
            if (Email == "r" && Password == "r")
            {
                await Shell.Current.GoToAsync("//CreateTaskPage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur", "Email ou mot de passe incorrect", "OK");
            }
        }

        void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

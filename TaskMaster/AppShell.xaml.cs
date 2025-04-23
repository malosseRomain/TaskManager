using TaskMaster.Services;

namespace TaskMaster;

public partial class AppShell : Shell
{
    private readonly IAuthService _authService;

    public bool IsAuthenticated => _authService.IsAuthenticated;
    public bool IsNotAuthenticated => !IsAuthenticated;

    public AppShell(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
        _authService.AuthStateChanged += OnAuthStateChanged;
        BindingContext = this;
    }

    private void OnAuthStateChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(IsAuthenticated));
        OnPropertyChanged(nameof(IsNotAuthenticated));
    }
}

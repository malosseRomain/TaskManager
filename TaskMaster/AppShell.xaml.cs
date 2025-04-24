using TaskMaster.Services;
using Microsoft.Extensions.DependencyInjection;
using TaskMaster.ViewModels;
using System.Web;
using TaskMaster.Views;

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

        // Garder uniquement l'enregistrement de la route
        Routing.RegisterRoute("ModifyTaskPage", typeof(ModifyTaskPage));
    }

    private void OnAuthStateChanged(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(IsAuthenticated));
        OnPropertyChanged(nameof(IsNotAuthenticated));
    }
}

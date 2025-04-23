using Microsoft.Maui.Controls;
using System;
using TaskMaster.Services;

namespace TaskMaster
{
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;

        public IServiceProvider Services { get; }

        public App(IServiceProvider services, IAuthService authService)
        {
            InitializeComponent();
            Services = services;

            MainPage = new AppShell(authService);
        }
    }
}

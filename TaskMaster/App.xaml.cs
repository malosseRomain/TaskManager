using Microsoft.Maui.Controls;
using System;

namespace TaskMaster
{
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;

        public IServiceProvider Services { get; }

        public App(IServiceProvider services)
        {
            InitializeComponent();
            Services = services;

            MainPage = new AppShell();
        }
    }
}

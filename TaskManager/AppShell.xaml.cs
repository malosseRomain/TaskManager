﻿namespace TaskManager
{
    using TaskManager.Views;
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
        }
    }
}

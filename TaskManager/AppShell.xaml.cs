namespace TaskManager
{
    using TaskManager.Views;
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("SignUpPage", typeof(SignUpPage));
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("CreateTaskPage", typeof(CreateTaskPage));
        }
    }
}

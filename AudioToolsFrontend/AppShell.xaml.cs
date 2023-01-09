namespace AudioToolsFrontend
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PedalBoardView), typeof(PedalBoardView));
        }
    }
}
using System.Windows.Input;

namespace AudioToolsFrontend
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }
    }
}
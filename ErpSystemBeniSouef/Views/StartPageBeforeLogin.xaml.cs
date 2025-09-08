using System;
using System.Windows.Controls;
using System.Windows.Threading;
using ErpSystemBeniSouef.ViewModel;

namespace ErpSystemBeniSouef.Views
{
    public partial class StartPageBeforeLogin : Page
    {
        private readonly DispatcherTimer dispatcherTimer = new();

        public StartPageBeforeLogin()
        {
            InitializeComponent();

            dispatcherTimer.Interval = TimeSpan.FromSeconds(1); // فتح بعد 4 ثواني
            dispatcherTimer.Tick += MySplash;
            dispatcherTimer.Start();
        }

        private void MySplash(object? sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            var loginPage = new LoginPage();
            MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(loginPage);
        }
    }
}

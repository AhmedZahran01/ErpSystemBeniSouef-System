using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ErpSystemBeniSouef.HelperFunctions;
using ErpSystemBeniSouef.ViewModel;
using ErpSystemBeniSouef.Views.Windows;

namespace ErpSystemBeniSouef.Views
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            if (UsernameText.Text == "1" && PasswordText.Password == "1")
            {
                //AppGlobalCompanyId.CompanyId = 1;
                App.Current.Properties["CompanyId"] = 1;

                var dashboard = new Views.Pages.Products.Dashboard();
                MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(dashboard);
            }

            if (UsernameText.Text == "2" && PasswordText.Password == "2")
            {
                App.Current.Properties["CompanyId"] = 2;
                var dashboard = new Views.Pages.Products.Dashboard();
                MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(dashboard);
            }

            if (UsernameText.Text == "3" && PasswordText.Password == "3")
            {
                App.Current.Properties["CompanyId"] = 3;
                var dashboard = new Views.Pages.Products.Dashboard();
                MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(dashboard);
            }

            if (UsernameText.Text == "4" && PasswordText.Password == "4")
            {
                App.Current.Properties["CompanyId"] = 4;
                var dashboard = new Views.Pages.Products.Dashboard();
                MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(dashboard);
            }

        }
    }
}

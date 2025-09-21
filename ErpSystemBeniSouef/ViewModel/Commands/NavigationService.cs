using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ErpSystemBeniSouef.ViewModel.Commands
{
    public class NavigationService
    {
        public void NavigateToPage(string pageKey)
        {
            switch (pageKey)
            {
                case "DashBoardProducts":
                //case "DashBoardManager":
                    var dashBoardManagerPage = new ErpSystemBeniSouef.Views.Pages.Products.Dashboard();
                    MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(dashBoardManagerPage);
                    break; 

                default:
                    // Optionally handle unknown keys
                    MessageBox.Show("Page not found: " + pageKey);
                    break;
            }
        }

    }
}
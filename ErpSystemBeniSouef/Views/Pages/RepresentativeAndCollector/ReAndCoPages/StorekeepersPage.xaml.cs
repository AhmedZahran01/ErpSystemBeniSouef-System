using ErpSystemBeniSouef.Core.Entities;
using ErpSystemBeniSouef.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ErpSystemBeniSouef.Views.Pages.RepresentativeAndCollector.ReAndCoPages
{
    /// <summary>
    /// Interaction logic for StorekeepersPage.xaml
    /// </summary>
    public partial class StorekeepersPage : Page
    {
        ObservableCollection<Storekeeper> observalStorekeepersRegions = new();

        public StorekeepersPage()
        {
            InitializeComponent();
            SeedDefaultRegions();
            dgStorekeepers.ItemsSource = observalStorekeepersRegions;

        }

        
        private void SeedDefaultRegions()
        {
            //var storekeepers = new List<Storekeepers>
            //{
            //    new Storekeepers { Id = 1, Name = "أحمد علي", user_Number = 1001, user_Password = 1234 },
            //    new Storekeepers { Id = 2, Name = "محمد حسن", user_Number = 1002, user_Password = 2345 },
            //    new Storekeepers { Id = 3, Name = "خالد إبراهيم", user_Number = 1003, user_Password = 3456 },
            //    new Storekeepers { Id = 4, Name = "محمود عبد الله", user_Number = 1004, user_Password = 4567 },
            //    new Storekeepers { Id = 5, Name = "عبد الرحمن يوسف", user_Number = 1005, user_Password = 5678 },
            //    new Storekeepers { Id = 6, Name = "ياسر محمود", user_Number = 1006, user_Password = 6789 },
            //    new Storekeepers { Id = 7, Name = "عمرو سمير", user_Number = 1007, user_Password = 7890 },
            //    new Storekeepers { Id = 8, Name = "حسين علي", user_Number = 1008, user_Password = 8901 },
            //    new Storekeepers { Id = 9, Name = "طارق مصطفى", user_Number = 1009, user_Password = 9012 },
            //    new Storekeepers { Id = 10, Name = "إبراهيم سالم", user_Number = 1010, user_Password = 1122 }
            //};

            //foreach (var storekeeper in storekeepers)
            //    observalStorekeepersRegions.Add(storekeeper);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            //var Dashboard = new RepresentativeAndCollectorPage();
            //MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);

        }

    }
}

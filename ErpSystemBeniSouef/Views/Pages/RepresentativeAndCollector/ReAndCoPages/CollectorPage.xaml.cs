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
    /// Interaction logic for CollectorPage.xaml
    /// </summary>
    public partial class CollectorPage : Page
    {
        ObservableCollection<Collector> observalCollectorRegions = new();
        public CollectorPage()
        {
            InitializeComponent(); SeedDefaultRegions();
            dgCollectors.ItemsSource = observalCollectorRegions;

        }

        private void SeedDefaultRegions()
        {
            var collectors = new List<Collector>
            {
                new Collector { Id = 1, Name = "أحمد علي" },
                new Collector { Id = 2, Name = "محمد حسن" },
                new Collector { Id = 3, Name = "خالد عبد الله" },
                new Collector { Id = 4, Name = "عمرو إبراهيم" },
                new Collector { Id = 5, Name = "محمود يوسف" },
                new Collector { Id = 6, Name = "حسين سعيد" },
                new Collector { Id = 7, Name = "طارق مصطفى" },
                new Collector { Id = 8, Name = "مصطفى كامل" },
                new Collector { Id = 9, Name = "سامح فؤاد" },
                new Collector { Id = 10,Name = "ياسر عبد الحميد" }
            };

            foreach (var collector in collectors)
                observalCollectorRegions.Add(collector);
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            //var Dashboard = new RepresentativeAndCollectorPage();
            //MainWindowViewModel.MainWindow.Frame.NavigationService.Navigate(Dashboard);
        }

    }
}

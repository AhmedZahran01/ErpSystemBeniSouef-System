using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Models;
using ErpSystemBeniSouef.Views.Windows;

namespace ErpSystemBeniSouef.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {

        public static MainWindow MainWindow { get; set; }


        public MainWindowViewModel()
        {

        }

        internal void setContext(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }
    }
}

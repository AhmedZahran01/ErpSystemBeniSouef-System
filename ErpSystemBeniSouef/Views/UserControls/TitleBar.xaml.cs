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

namespace ErpSystemBeniSouef.Views.UserControls
{
    /// <summary>
    /// Interaction logic for TitleBar.xaml
    /// </summary>
    public partial class TitleBar : UserControl
    {
        public TitleBar()
        {
            InitializeComponent();
        }

        private Window GetParentWindow() => Window.GetWindow(this);

        // Click event for the minimize button
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            var window = GetParentWindow();
            window.WindowState = WindowState.Minimized;
        }

        // Click event for the close button
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            var window = GetParentWindow();
            window?.Close();
        }

        // Makes the window draggable when holding the mouse left button on the title bar
        private void UserControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var window = GetParentWindow();
            window.DragMove();
        }
    }
}

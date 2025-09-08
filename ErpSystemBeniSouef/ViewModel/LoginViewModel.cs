using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErpSystemBeniSouef.Models;
using ErpSystemBeniSouef.ViewModel.Commands.Login;

namespace ErpSystemBeniSouef.ViewModel
{
    public class LoginViewModel : ObservableObject
    {
        public MainWindowViewModel mainWindowViewModel { get; set; }
        public string userNmae
        {
            get;
            set;
        }
        public string password { get; set; }

        public LoginCommand Login { get; set; }
        public LoginViewModel()
        {
            Login = new LoginCommand(this);
        }
    }
}

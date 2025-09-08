using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows;

namespace ErpSystemBeniSouef.ViewModel.Commands.Login
{
    public class LoginCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public LoginViewModel LoginViewModel { get; set; }
        public LoginCommand(LoginViewModel loginViewModel)
        {
            LoginViewModel = loginViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;

        }

        public void Execute(object parameter)
        {
            NavigationService navigation = new NavigationService();
            var username = this.LoginViewModel.userNmae;
            var password = this.LoginViewModel.password;
            bool passcorrect = false;
            bool usercorrect = false;

            if (isAdmin(navigation, username, password))
            {
                return;
            }

            //List<ErpSystemBeniSouef.Model.Data.Doctor> doctors = new DoctorDBHandler().GetAll();
            //// micro1 123 doctor open page
            //foreach (var doctor in doctors)
            //{
            //    if ((doctor.Name == username))
            //    {
            //        usercorrect = true;
            //        if ((doctor.Password == password))
            //        {
            //            passcorrect = true;
            //            navigation.NavigateToPage("DashBoardDoctor");
            //            DoctorViewModel.doctor = doctor;
            //        }
            //    }
            //}
             
            if (!usercorrect) MessageBox.Show("Wrong Username.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (!passcorrect) MessageBox.Show("Wrong Password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private bool isAdmin(NavigationService navigation, string username = "Doctor" 
            , string password = "123")
        {
            if (username == "Doctor" && password == "123")
            {
                //DoctorViewModel.doctor = new Doctor()
                //{
                //    Name = username,
                //    Password = password
                //};
                navigation.NavigateToPage("DashBoardDoctor");
            } 
            else { return false; }
            return true;
        }

    }
}

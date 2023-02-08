using Optimization.DB_EF;
using Optimization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_MVVM_Classes;

namespace Optimization.ViewModels
{
    internal class RegistrationVM: ViewModelBase
    {
        ApplicationContext context;
        private readonly Window registration;
        public RegistrationVM(Window _registration) 
        {
            context = new ApplicationContext();
            registration = _registration;
        }


        private string _login;
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        private string _password; 
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _role;
        public string Role
        {
            get { return _role; }
            set
            {
                _role = value;
                OnPropertyChanged();
            }
        }


        private Authorization? authorization = null;
        private AuthorizationVM authorizationVM;

        private RelayCommand _openAuthorization;
        public RelayCommand OpenAuthorization
        {
            get
            {
                return _openAuthorization ??= new RelayCommand(x =>
                {
                    if (string.IsNullOrWhiteSpace(Login))
                    {
                        MessageBox.Show("Вы не ввели логин");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(Password))
                    {
                        MessageBox.Show("Вы не ввели пароль");
                        return;
                    }

                    Account newAccount = new Account { Login = Login, Password = Password, Role = "Пользователь" };
                    context.Accounts.Add(newAccount);
                    context.SaveChanges();

                    MessageBox.Show("Регистрация прошла успешно!");
                    authorization = new Authorization();
                    authorizationVM = new AuthorizationVM(authorization);
                    authorization.DataContext = authorizationVM;
                    authorization.Show();

                    registration.Close();

                });
            }
        }
    }
}

using Optimization.DB_EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_MVVM_Classes;

namespace Optimization.ViewModels
{
    internal class AuthorizationVM: ViewModelBase
    {
        private ApplicationContext context;
        private readonly Window authWindow;
        public AuthorizationVM(Window _authWindow) 
        {
            context = new ApplicationContext();
            authWindow = _authWindow;
            BrushesLogin = System.Drawing.Color.Gray.Name.ToString();
            BrushesPassword = System.Drawing.Color.Gray.Name.ToString();
        }

        private string _Login;
        public string Login
        {
            get { return _Login; }
            set
            {
                _Login = value;
                OnPropertyChanged();
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged();
            }
        }

        private string _BrushesLogin;
        public string BrushesLogin
        {
            get { return _BrushesLogin; }
            set
            {
                _BrushesLogin = value;
                OnPropertyChanged();
            }
        }
        private string _BrushesPassword;
        public string BrushesPassword
        {
            get { return _BrushesPassword; }
            set
            {
                _BrushesPassword = value;
                OnPropertyChanged();
            }
        }

        private AdminWindow? adminWindow = null;
        private AdminWindowVM adminVM;

        private MainWindow? mainWindow = null;
        private MainWindowVM mainWindowVM;

        private Registration? registration = null;
        private RegistrationVM registrationVM;

        private RelayCommand _OpenWindow;
        public RelayCommand OpenWindow
        {
            get
            {
                return _OpenWindow ??= new RelayCommand(x =>
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

                    var account = context.Accounts.ToList().FirstOrDefault(x => x.Login == Login && x.Password == Password);
                    if (account != null)
                    {
                        if (account.Role == "Пользователь")
                        {
                            mainWindow = new MainWindow();
                            mainWindowVM = new MainWindowVM(mainWindow);
                            mainWindow.DataContext = mainWindowVM;
                            mainWindow.Show();
                        }
                        else if (account.Role == "Администратор")
                        {
                            adminWindow = new AdminWindow();
                            adminVM = new AdminWindowVM(adminWindow);
                            adminWindow.DataContext = adminVM;
                            adminWindow.Show();
                        }
                        authWindow.Close();
                    }
                    else
                    {
                        BrushesLogin = System.Drawing.Color.Red.Name.ToString();
                        BrushesPassword = System.Drawing.Color.Red.Name.ToString();
                    }
                });
            }
        }

        private RelayCommand _OpenRegistration;
        public RelayCommand OpenRegistration
        {
            get
            {
                return _OpenRegistration ??= new RelayCommand(x =>
                {
                    registration = new Registration();
                    registrationVM = new RegistrationVM(registration);
                    registration.DataContext = registrationVM;
                    registration.Show();
                    authWindow.Close();
                });
            }
        }


    }
}

using Microsoft.EntityFrameworkCore;
using Optimization.CRUD;
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
    internal class AdminWindowVM : ViewModelBase
    {
        ApplicationContext context;
        AccountCRUD crud;
        MethodCRUD crudMethod;
        private readonly Window adminWindow;
        public AdminWindowVM(Window _adminWindow)
        {
            context = new ApplicationContext();
            crud = new AccountCRUD();
            crudMethod = new MethodCRUD();
            RolesCreate();
            SetAccountData();
            adminWindow = _adminWindow;
        }

        public void RolesCreate()
        {
            Roles = new List<string>();
            Roles.Add("Администратор");
            Roles.Add("Пользователь");
        }
        public void SetAccountData()
        {
            context = new ApplicationContext();
            context.Accounts.Load();
            context.OptimizationMethods.Load();
            Account = context.Accounts.ToList();
            Methods = context.OptimizationMethods.ToList();
        }

        #region Account

        private List<Account> _account;
        public List<Account> Account
        {
            get => _account;
            set
            {
                _account = value;
                OnPropertyChanged();
            }
        }

        private string _newLogin;
        public string NewLogin
        {
            get => _newLogin;
            set
            {
                _newLogin = value;
                OnPropertyChanged();
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged();
            }
        }

        private string _newRole;
        public string NewRole
        {
            get => _newRole;
            set
            {
                _newRole = value;
                OnPropertyChanged();
            }
        }

        private List<string> _roles;
        public List<string> Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                OnPropertyChanged();
            }
        }

        private Account _selectedAccount;
        public Account SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged();
                if (SelectedAccount != null)
                {
                    NewLogin = _selectedAccount.Login;
                    NewPassword = _selectedAccount.Password;
                    if (_selectedAccount.Role == "Администратор")
                    {
                        NewRole = Roles[0];
                    }
                    else
                        NewRole = Roles[1];
                }
            }
        }

        private RelayCommand _addAccount;
        public RelayCommand AddAccount
        {
            get
            {
                return _addAccount ??= new RelayCommand(x =>
                {
                    if (NewLogin != null && NewPassword != null && NewRole != null)
                    {
                        var account = new Account { Login = NewLogin, Password = NewPassword, Role = NewRole };
                        crud.Create(account);
                        SetAccountData();
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля, чтобы добавить новго пользователя!", "Ошибка добавления пользователя");
                    }
                });
            }
        }

        private RelayCommand _removeAccount;
        public RelayCommand RemoveAccount
        {
            get
            {
                return _removeAccount ??= new RelayCommand(x =>
                {
                    if (SelectedAccount != null)
                    {
                        if (SelectedAccount.Role == "Администратор") 
                        {
                            if (Account.Count(a => a.Role == "Администратор") > 1)
                            {
                                crud.Delete(SelectedAccount.Id);
                            }
                            else
                            {
                                MessageBox.Show("Вы не можете удалить последнего администратора!", "Ошибка удаления администратора");
                            }
                        }
                        else 
                        {
                            crud.Delete(SelectedAccount.Id);
                        }
                        SetAccountData();


                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля, чтобы добавить новго пользователя!", "Ошибка удаления пользователя");
                    }
                });
            }
        }

        private RelayCommand _updateAccount;
        public RelayCommand UpdateAccount
        {
            get
            {
                return _updateAccount ??= new RelayCommand(x =>
                {
                    if (SelectedAccount != null)
                    {
                        var account = new Account { Id = SelectedAccount.Id, Login = NewLogin, Password = NewPassword, Role = NewRole };
                        crud.Update(account);
                        SetAccountData();
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля, чтобы добавить новго пользователя!", "Ошибка удаления пользователя");
                    }
                });
            }
        }
        #endregion

        #region Method
        private List<OptimizationMethod> _methods;
        public List<OptimizationMethod> Methods
        {
            get => _methods;
            set
            {
                _methods = value;
                OnPropertyChanged();
            }
        }

        private string _newMethodName;
        public string NewMethodName 
        {
            get { return _newMethodName; }
            set 
            {
                _newMethodName = value;
                OnPropertyChanged();
            }
        }

        private bool _haveRealization;
        public bool HaveRealization
        {
            get { return _haveRealization; }
            set
            {
                _haveRealization = value;
                OnPropertyChanged();
            }
        }

        private OptimizationMethod _selectedMethod;
        public OptimizationMethod SelectedMethod
        {
            get => _selectedMethod;
            set
            {
                _selectedMethod = value;
                OnPropertyChanged();

                if (SelectedMethod != null)
                {
                    NewMethodName = _selectedMethod.Name;
                    HaveRealization = _selectedMethod.Realization;
                }
            }
        }

        private RelayCommand _addMethod;
        public RelayCommand AddMethod
        {
            get
            {
                return _addMethod ??= new RelayCommand(x =>
                {
                    if (NewMethodName != null)
                    {
                        var method = new OptimizationMethod { Name = NewMethodName, Realization = HaveRealization };
                        crudMethod.Create(method);
                        SetAccountData();
                    }
                    else
                    {
                        MessageBox.Show("Введите наименование метода!", "Ошибка добавления метода");
                    }
                });
            }
        }

        private RelayCommand _removeMethod;
        public RelayCommand RemoveMethod
        {
            get
            {
                return _removeMethod ??= new RelayCommand(x =>
                {
                    if (NewMethodName != null)
                    {
                        crudMethod.Delete(SelectedMethod.Id);
                        SetAccountData();
                    }
                    else
                    {
                        MessageBox.Show("Выберите метод для удаления!", "Ошибка удаления метода");
                    }
                });
            }
        }

        private RelayCommand _updateMethod;
        public RelayCommand UpdateMethod
        {
            get
            {
                return _updateMethod ??= new RelayCommand(x =>
                {
                    if (NewMethodName != null)
                    {
                        var method = new OptimizationMethod { Id = SelectedMethod.Id, Name = NewMethodName, Realization = HaveRealization };

                        crudMethod.Update(method);
                        SetAccountData();
                    }
                    else
                    {
                        MessageBox.Show("Не все поля заполнены!", "Ошибка удаления метода");
                    }
                });
            }
        }

        #endregion

        #region Parameter

        private OptimizationMethod _selectedParameter;
        public OptimizationMethod SelectedParameter
        {
            get => _selectedParameter;
            set
            {
                _selectedParameter = value;
                OnPropertyChanged();

                if (SelectedParameter != null)
                {
                    NewParamName = _selectedMethod.Name;
                    NewParamSymbol = _selectedMethod.Realization;
                    NewParamVariant = _selectedParameter.
                }
            }
        }
        #endregion

        #region Menu
        private RelayCommand? _taskDescription;
        public RelayCommand TaskDescription
        {
            get
            {
                return _taskDescription ??= new RelayCommand(x =>
                {
                    MessageBox.Show("Исходная задача оптимизации:\r\n" +
                        "Необходимо найти габаритные размеры теплообменного устройства химического реактора (длину L (м) и ширину S (м)), " +
                        "обеспечивающие минимальные затраты на изготовление   изделия.  Затраты на изготовление изделия связана с его весом." +
                        "Зависимость веса изделия P от геометрических размеров и заданных характеристик теплообменника определяется по формуле:\r\n" +
                        "P = α * (L – S) ^2 + β * 1 / H * (S+ L - γ *N) ^2," +
                        "\r\nгде    Н – высота теплообменника (9 м)," +
                        "\r\n         N –  число витков змеевика (10 шт)," +
                        "\r\n           α, β γ, – нормирующие множители, равные 1." +
                        "\r\nНа габариты теплообменника накладываются следующие ограничения. Длина L должна быть не менее 1 м и не более 15 м, ширина S – не менее 1м и не более 12 м. Кроме того, обязательно должно выполняться условие: сумма (L+S) должна быть не менее 12 м.  " +
                        "Стоимость изготовления 1 кГ изделия составляет 100 у.е. Точность решения – 0,01 м.\r\n", "Описание задания");

                });
            }
        }

        private Authorization? authorization = null;
        private AuthorizationVM authorizationVM;

        private RelayCommand? _exit;
        public RelayCommand Exit
        {
            get
            {
                return _exit ??= new RelayCommand(x =>
                {
                    authorization = new Authorization();
                    authorizationVM = new AuthorizationVM(authorization);
                    authorization.DataContext = authorizationVM;
                    authorization.Show();
                    adminWindow.Close();

                });
            }
        }
        #endregion



    }
}

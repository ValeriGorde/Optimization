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
     //<!--d:DataContext="{d:DesignInstance Type=viewmodels:MainWindowVM}-->
    internal class AdminWindowVM : ViewModelBase
    {
        ApplicationContext context;
        AccountCRUD crud;
        MethodCRUD crudMethod;
        ParameterCRUD crudParam;
        AssignmentCRUD crudAssignmt;
        AssignmentParameterCRUD crudAssignmtParam;

        private readonly Window adminWindow;
        public AdminWindowVM(Window _adminWindow)
        {
            context = new ApplicationContext();
            crud = new AccountCRUD();
            crudMethod = new MethodCRUD();
            crudParam = new ParameterCRUD();
            crudAssignmt = new AssignmentCRUD();
            crudAssignmtParam = new AssignmentParameterCRUD();

            RolesCreate();
            SetAccountData(); //переименовать
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
            Parameters = context.Parameters.ToList();
            Assignments = context.Assignments.ToList();
            AssignmntParams = context.AssignmentParameters.ToList();
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
                        MessageBox.Show("Заполните все поля, чтобы добавить нового пользователя!", "Ошибка добавления пользователя");
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
                        MessageBox.Show("Выберите пользователя, которого необходимо удалить!", "Ошибка удаления пользователя");
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
                        MessageBox.Show("Заполните все поля, чтобы изменить данные пользователя!", "Ошибка изменения данных пользователя");
                    }
                });
            }
        }

        private RelayCommand _removeAllAccount;
        public RelayCommand RemoveAllAccount
        {
            get
            {
                return _removeAllAccount ??= new RelayCommand(x =>
                {
                    NewLogin = "";
                    NewPassword = "";
                    NewRole = null;
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
                        if (crudMethod.Read(method.Id)) 
                        {
                            crudMethod.Create(method);
                            SetAccountData();
                        }
                        else 
                        {
                            MessageBox.Show("Данный метод уже cуществует!", "Ошибка добавления метода");
                        }
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
                        MessageBox.Show("Не все поля заполнены!", "Ошибка обновления метода");
                    }
                });
            }
        }

        private RelayCommand _removeAllMethod;
        public RelayCommand RemoveAllMethod
        {
            get
            {
                return _removeAllMethod ??= new RelayCommand(x =>
                {
                    NewMethodName = "";
                    HaveRealization = false;
                });
            }
        }
        #endregion

        #region Parameter

        private List<Parameter> _parameters;
        public List<Parameter> Parameters 
        {
            get => _parameters;
            set 
            {
                _parameters = value;
                OnPropertyChanged();
            }
        }

        private string _newParamName;
        public string NewParamName 
        {
            get { return _newParamName; }
            set 
            {
                _newParamName = value;
                OnPropertyChanged();
            }
        }

        private string _newParamSymbol;
        public string NewParamSymbol
        {
            get { return _newParamSymbol; }
            set
            {
                _newParamSymbol = value;
                OnPropertyChanged();
            }
        }

        private Parameter _selectedParameter;
        public Parameter SelectedParameter
        {
            get => _selectedParameter;
            set
            {
                _selectedParameter = value;
                OnPropertyChanged();

                if (SelectedParameter != null)
                {
                    NewParamName = _selectedParameter.Name;
                    NewParamSymbol = _selectedParameter.Symbol;
                }
            }
        }

        private RelayCommand _addParameter;
        public RelayCommand AddParameter
        {
            get
            {
                return _addParameter ??= new RelayCommand(x =>
                {

                    if (NewParamName != null && NewParamSymbol != null)
                    {
                        var param = new Parameter { Name = NewParamName, Symbol = NewParamSymbol};
                        crudParam.Create(param);
                        SetAccountData();
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля, чтобы добавить новый параметр!", "Ошибка добавления параметра");
                    }
                });
            }
        }

        private RelayCommand _removeParameter;
        public RelayCommand RemoveParameter
        {
            get
            {
                return _removeParameter ??= new RelayCommand(x =>
                {
                    if (NewParamName != null && NewParamSymbol != null)
                    {
                        crudParam.Delete(SelectedParameter.Id);
                        SetAccountData();
                    }
                    else
                    {
                        MessageBox.Show("Выберите параметр для удаления!", "Ошибка удаления параметра");
                    }
                });
            }
        }

        private RelayCommand _updateParameter;
        public RelayCommand UpdateParameter
        {
            get
            {
                return _updateParameter ??= new RelayCommand(x =>
                {
                    if (NewParamName != null && NewParamSymbol != null)
                    {
                        var param = new Parameter { Id = SelectedParameter.Id, Name = NewParamName, Symbol = NewParamSymbol};

                        crudParam.Update(param);
                        SetAccountData();
                    }
                    else
                    {
                        MessageBox.Show("Не все поля заполнены!", "Ошибка обновления параметра");
                    }
                });
            }
        }

        private RelayCommand _removeAllParameter;
        public RelayCommand RemoveAllParameter
        {
            get
            {
                return _removeAllParameter ??= new RelayCommand(x =>
                {
                    NewParamName = "";
                    NewParamSymbol = "";
                });
            }
        }
        #endregion

        #region Assignment

        private List<Assignment> _assignments;
        public List<Assignment> Assignments
        {
            get => _assignments;
            set
            {
                _assignments = value;
                OnPropertyChanged();
            }
        }

        private Assignment _selectedAssignment;
        public Assignment SelectedAssignment
        {
            get => _selectedAssignment;
            set
            {
                _selectedAssignment = value;
                OnPropertyChanged();

                if (SelectedAssignment != null)
                {
                    NewAssignmtName = _selectedAssignment.Name;
                    NewAssignmtDescription = _selectedAssignment.Description;
                }
            }
        }

        private string _newAssignmtName;
        public string NewAssignmtName
        {
            get { return _newAssignmtName; }
            set 
            {
                _newAssignmtName = value;
                OnPropertyChanged();
            }
        }

        private string _newAssignmtDescription;
        public string NewAssignmtDescription
        {
            get { return _newAssignmtDescription; }
            set 
            {
                _newAssignmtDescription = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _addAssignment;
        public RelayCommand AddAssignment
        {
            get
            {
                return _addAssignment ??= new RelayCommand(x =>
                {

                    if (NewAssignmtName != null && NewAssignmtDescription != null)
                    {
                        var assignmt = new Assignment {  Name = NewAssignmtName, Description = NewAssignmtDescription};
                        crudAssignmt.Create(assignmt);
                        SetAccountData();
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля, чтобы добавить новое задание!", "Ошибка добавления задания");
                    }
                });
            }
        }

        private RelayCommand _removeAssignment;
        public RelayCommand RemoveAssignment
        {
            get
            {
                return _removeAssignment ??= new RelayCommand(x =>
                {
                    if (NewAssignmtName != null && NewAssignmtDescription != null)
                    {
                        crudAssignmt.Delete(SelectedAssignment.Id);
                        SetAccountData();
                    }
                    else
                    {
                        MessageBox.Show("Выберите задание для удаления!", "Ошибка удаления задания");
                    }
                });
            }
        }

        private RelayCommand _updateAssignment;
        public RelayCommand UpdateAssignment
        {
            get
            {
                return _updateAssignment ??= new RelayCommand(x =>
                {
                    if (NewAssignmtName != null && NewAssignmtDescription != null)
                    {
                        var assignmt = new Assignment { Id = SelectedAssignment.Id, Name = NewAssignmtName, Description = NewAssignmtDescription };
                        crudAssignmt.Update(assignmt);
                        SetAccountData();
                    }
                    else
                    {
                        MessageBox.Show("Не все поля заполнены!", "Ошибка обновления задания");
                    }
                });
            }
        }

        private RelayCommand _removeAllAssignment;
        public RelayCommand RemoveAllAssignment
        {
            get
            {
                return _removeAllAssignment ??= new RelayCommand(x =>
                {
                    NewAssignmtName = "";
                    NewAssignmtDescription = "";
                });
            }
        }

        #endregion

        #region AssignmentParam

        private List<AssignmentParameter> _assignmntParams;
        public List<AssignmentParameter> AssignmntParams
        {
            get => _assignmntParams;
            set
            {
                _assignmntParams = value;
                OnPropertyChanged();
            }
        }

        private AssignmentParameter _selectedAssignmntParam;
        public AssignmentParameter SelectedAssignmntParam
        {
            get => _selectedAssignmntParam;
            set
            {
                _selectedAssignmntParam = value;
                OnPropertyChanged();

                if (SelectedAssignmntParam != null)
                {
                    NewAssignmntParamName = _selectedAssignmntParam.Parameter;
                    NewAssignmntParamTask = _selectedAssignmntParam.Assignment;
                    NewAssignmntParamValue = _selectedAssignmntParam.Value;
                }
            }
        }

        private Assignment _newAssignmntParamTask;
        public Assignment NewAssignmntParamTask 
        {
            get => _newAssignmntParamTask;
            set 
            {
                _newAssignmntParamTask = value;
                OnPropertyChanged();
            }
        }

        private double _newAssignmntParamValue;
        public double NewAssignmntParamValue
        {
            get => _newAssignmntParamValue;
            set
            {
                _newAssignmntParamValue = value;
                OnPropertyChanged();
            }
        }

        private Parameter _newAssignmntParamName;
        public Parameter NewAssignmntParamName
        {
            get => _newAssignmntParamName;
            set
            {
                _newAssignmntParamName = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _addAssignmntParam;
        public RelayCommand AddAssignmntParam
        {
            get
            {
                return _addAssignmntParam ??= new RelayCommand(x =>
                {
                    if (NewAssignmntParamName != null && NewAssignmntParamTask != null)
                    {
                        var assignmntParam = new AssignmentParameter { AssignmentId = NewAssignmntParamTask.Id, ParameterId = NewAssignmntParamName.Id, Value = NewAssignmntParamValue};
                        crudAssignmtParam.Create(assignmntParam);
                        SetAccountData();
                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля, чтобы добавить новое значение параметра!", "Ошибка добавления значения параметра");
                    }
                });
            }
        }

        private RelayCommand _removeAssignmntParam;
        public RelayCommand RemoveAssignmntParam
        {
            get
            {
                return _removeAssignmntParam ??= new RelayCommand(x =>
                {
                    if (SelectedAssignmntParam != null)
                    {
                        crudAssignmtParam.Delete(SelectedAssignmntParam.Id);
                        SetAccountData();
                    }
                    else
                    {
                        MessageBox.Show("Выберите значение параметра для удаления!", "Ошибка удаления значения параметра");
                    }
                });
            }
        }

        private RelayCommand _updateAssignmntParam;
        public RelayCommand UpdateAssignmntParam
        {
            get
            {
                return _updateAssignmntParam ??= new RelayCommand(x =>
                {
                    if (SelectedAssignmntParam != null)
                    {
                        var assignmntParam = new AssignmentParameter { AssignmentId = NewAssignmntParamTask.Id, ParameterId = NewAssignmntParamName.Id, Value = NewAssignmntParamValue };

                        crudAssignmtParam.Update(assignmntParam);
                        SetAccountData();
                    }
                    else
                    {
                        MessageBox.Show("Не все поля заполнены!", "Ошибка обновления значения параметра");
                    }
                });
            }
        }

        private RelayCommand _removeAllAssignmntParam;
        public RelayCommand RemoveAllAssignmntParam
        {
            get
            {
                return _removeAllAssignmntParam ??= new RelayCommand(x =>
                {
                    NewAssignmntParamName = null;
                    NewAssignmntParamTask = null;
                    NewAssignmntParamValue = 0.0; 
                });
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

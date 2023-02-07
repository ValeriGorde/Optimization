using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_MVVM_Classes;

namespace Optimization.ViewModels
{
    internal class MainWindowVM: ViewModelBase
    {
        private readonly Window mainWindow;
        public MainWindowVM(Window _mainWindow)
        {
            mainWindow = _mainWindow;

            Methods = new List<string>();
            Methods.Add("Метод Бокса");
            Methods.Add("Метод сканирования");
        }

        private string? _currentMethod;
        public string CurrentMethod
        {
            get { return _currentMethod; }
            set
            {
                _currentMethod = value;
                OnPropertyChanged();
            }
        }

        private List<string>? _methods;
        public List<string> Methods
        {
            get { return _methods; }
            set
            {
                _methods = value;
                OnPropertyChanged();
            }
        }

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

        private RelayCommand? _about;
        public RelayCommand About
        {
            get
            {
                return _about ??= new RelayCommand(x =>
                {
                    MessageBox.Show("Выполнила: Гордеева Валерия, студентка группы 494", "Справка");

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
                    mainWindow.Close();

                });
            }
        }
    }
}

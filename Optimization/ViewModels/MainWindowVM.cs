using Optimization.Filter;
using Optimization.Models;
using Optimization.Plots;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WPF_MVVM_Classes;

namespace Optimization.ViewModels
{
    internal class MainWindowVM: ViewModelBase
    {
        private readonly Window mainWindow;
        private List<Point3D> _point3D = new();
        OutputParams parameters;

        public MainWindowVM(Window _mainWindow)
        {
            mainWindow = _mainWindow;

            Methods = new List<string>();
            Methods.Add("Метод Бокса");
            Methods.Add("Метод сканирования");

            Variants = new List<string>();
            Variants.Add("Вариант 3"); //брать из бд (задание метода)
        }

        #region Params

        private InputParameters _parameter = new InputParameters { Alpha = 1, Beta = 1, Gamma = 1, H = 9, N = 10, LMin = 1, LMax = 15, SMin = 1, SMax = 12, Price = 100, LSSum = 10, Epsilon = 0.01  };
        public InputParameters Parameters
        {
            get { return _parameter; }
            set
            {
                _parameter = value;
                OnPropertyChanged();
            }
        }

        private OutputParams? _outputParam;
        public OutputParams OutputParam
        {
            get { return _outputParam; }
            set
            {
                _outputParam = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private List<Point3D> _dataList;
        public List<Point3D> DataList
        {
            get { return _dataList; }
            set
            {
                _dataList = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _calculate;
        public RelayCommand Calculate
        {
            get
            {
                return _calculate ??= new RelayCommand(c =>
                {
                    parameters = new OutputParams();

                    var calculate = new ScanMethod(Parameters);
                    calculate.Calculation(out var points3D);

                    var temp = new List<double>();

                    foreach (var item in points3D)
                    {
                        temp.Add(item.Z);
                    }
                    
                    parameters.LengthResult = points3D.Find(x => x.Z == temp.Max()).X;
                    parameters.WidthResult = points3D.Find(x => x.Z == temp.Max()).Y;
                    parameters.CostPriceResult = temp.Max();
                    parameters.OutputParamsArr = FiltrationOutput.CalcEqvation(Parameters);


                    OutputParam = parameters;   
                });
            }
        }

        private string _currentVariant;
        public string CurrentVariant
        {
            get => _currentVariant;
            set
            {
                _currentVariant = value;
                OnPropertyChanged();
            }
        }

        private List<string> _variants;
        public List<string> Variants
        {
            get => _variants;
            set
            {
                _variants = value;
                OnPropertyChanged();
            }
        }

        private string _currentMethod;
        public string CurrentMethod
        {
            get => _currentMethod;
            set
            {
                _currentMethod = value;
                OnPropertyChanged();
            }
        }

        private List<string> _methods;
        public List<string> Methods
        {
            get => _methods; 
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
                        "\r\nНа габариты теплообменника накладываются следующие ограничения. Длина L должна быть не менее 1 м и не более 15 м, ширина S – не менее 1м и не более 12 м. " +
                        "Кроме того, обязательно должно выполняться условие: сумма (L+S) должна быть не менее 12 м.  " +
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

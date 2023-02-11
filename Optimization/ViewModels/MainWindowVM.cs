using Optimization.Calculation;
using Optimization.DB_EF;
using Optimization.Filter;
using Optimization.Models;
using Optimization.Plots;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WPF_MVVM_Classes;

namespace Optimization.ViewModels
{
    internal class MainWindowVM : ViewModelBase
    {
        private readonly Window mainWindow;
        private List<Point3D> _point3D = new();
        OutputParams parameters;
        ApplicationContext context;

        public MainWindowVM(Window _mainWindow)
        {
            mainWindow = _mainWindow;
            context = new ApplicationContext(); //привязка к бд
            SetData();
        }

        public void SetData() 
        {
            Variants = context.Assignments.ToList();
            Methods = context.OptimizationMethods.ToList();
        }

        #region Params


        private InputParameters _parameters = new InputParameters { Alpha = 1, Beta = 1, Gamma = 1, H = 9, N = 10, LMin = 1, LMax = 15, SMin = 1, SMax = 12, Price = 100, LSSum = 10, Epsilon = 0.01 };
        public InputParameters Parameters
        {
            get { return _parameters; }
            set
            {
                _parameters = value;
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

        private List<OutputParamsArr> _dataList;
        public List<OutputParamsArr> DataList
        {
            get { return _dataList; }
            set
            {
                _dataList = value;
                OnPropertyChanged();
            }
        }

        private int _sizeRC;
        public int SizeRC 
        {
            get { return _sizeRC; }
            set 
            {
                _sizeRC = value;
                OnPropertyChanged();
            }
        }

        private double[,] _peaksData;
        public double[,] PeaksData
        {
            get { return _peaksData; }
            set 
            { 
                _peaksData = value; 
                OnPropertyChanged(); 
            }
        }

        private PlotModel _model2D;

        public PlotModel Model2D
        {
            get { return _model2D; }
            set
            {
                _model2D = value;
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
                    if (Parameters != null)
                    {
                        if (CurrentMethod != null || CurrentVariant != null)
                        {

                            if (CurrentMethod == "Метод сканирования")
                            {
                                parameters = new OutputParams();

                                var calculate = new ScanMethod(Parameters);
                                calculate.Calculation(out var points3D);

                                var temp = new List<double>();

                                foreach (var point in points3D)
                                {
                                    temp.Add(point.Z);
                                }

                                parameters.LengthResult = points3D.Find(x => x.Z == temp.Max()).X;
                                parameters.WidthResult = points3D.Find(x => x.Z == temp.Max()).Y;
                                parameters.CostPriceResult = temp.Max();
                                parameters.OutputParamsArr = FiltrationOutput.CalcEqvation(Parameters);

                                //заполнение таблицы
                                OutputParam = parameters;

                                MathModel model = new MathModel(Parameters);
                                Func<double, double, double> peaks = (x, y) => model.MainModel(x, y);

                                var xx = ArrayBuilder.CreateVector(Parameters.LMin, Parameters.LMax, 100);
                                var yy = ArrayBuilder.CreateVector(Parameters.SMin, Parameters.SMax, 100);
                                PeaksData = ArrayBuilder.Evaluate(peaks, xx, yy);

                                var cs = new ContourSeries
                                {
                                    Color = OxyColors.Black,
                                    LabelBackground = OxyColors.White,
                                    RowCoordinates = xx,
                                    ColumnCoordinates = yy,
                                    Data = PeaksData,
                                    TrackerFormatString = "L = {2:0.00}, S = {4:0.00}" + Environment.NewLine + "C = {6:0.00}"
                                };

                                //заполнение 3д графика
                                DataList = parameters.OutputParamsArr.ToList();
                                SizeRC = (int)Math.Sqrt(DataList.Count);

                                //построение 2д графика
                                _model2D = new PlotModel { Title = "C = F(L, S)", TitleFontSize = 16 };
                                _model2D.Series.Add(cs);
                                _model2D.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Длина, м" });
                                _model2D.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Ширина, м" });
                                Model2D = _model2D;
                            }
                            else if (CurrentMethod == "Метод Бокса")
                            {
                                MessageBox.Show("Метод Бокса в доработке!", "Обратитесь позже");
                            }
                            else
                            {
                                MessageBox.Show("Данный метод оптимизации не реализован!", "В разработке");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Выберите вариант задания и метод оптимизации!", "Ошибка");
                        }

                    }
                    else
                    {
                        MessageBox.Show("Заполните все поля!", "Ошибка");
                    }

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

        private List<Assignment> _variants;
        public List<Assignment> Variants
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

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
using Microsoft.Office.Interop.Excel;
using System.Windows;
using System.Windows.Documents;
using WPF_MVVM_Classes;
using Window = System.Windows.Window;
using System.IO;
using Parameter = Optimization.Models.Parameter;

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

        #region SetParams
        public void SetParams(int idAssignment)
        {
            var valueH = Tasks.FirstOrDefault(h => h.ParameterId == 1 && h.AssignmentId == idAssignment);
            var valueN = Tasks.FirstOrDefault(n => n.ParameterId == 2 && n.AssignmentId == idAssignment);
            var valueAlpha = Tasks.FirstOrDefault(a => a.ParameterId == 3 && a.AssignmentId == idAssignment);
            var valueBeta = Tasks.FirstOrDefault(b => b.ParameterId == 4 && b.AssignmentId == idAssignment);
            var valueGamma = Tasks.FirstOrDefault(g => g.ParameterId == 5 && g.AssignmentId == idAssignment);
            var valueLMin = Tasks.FirstOrDefault(l => l.ParameterId == 6 && l.AssignmentId == idAssignment);
            var valueLMax = Tasks.FirstOrDefault(l => l.ParameterId == 7 && l.AssignmentId == idAssignment);
            var valueSMin = Tasks.FirstOrDefault(s => s.ParameterId == 8 && s.AssignmentId == idAssignment);
            var valueSMax = Tasks.FirstOrDefault(s => s.ParameterId == 9 && s.AssignmentId == idAssignment);
            var valueLSSum = Tasks.FirstOrDefault(ls => ls.ParameterId == 10 && ls.AssignmentId == idAssignment);
            var valuePrice = Tasks.FirstOrDefault(p => p.ParameterId == 11 && p.AssignmentId == idAssignment);
            var valueEpsilon = Tasks.FirstOrDefault(e => e.ParameterId == 12 && e.AssignmentId == idAssignment);

            if (valueH != null && valueN != null && valueAlpha != null && valueBeta != null && valueGamma != null &&
                valueLMin != null && valueLMax != null && valueSMin != null &&
                valueSMax != null && valueLSSum != null && valuePrice != null && valueEpsilon != null)
            {
                _H = valueH.Value.ToString();
                _N = valueN.Value.ToString();
                _Alpha = valueAlpha.Value.ToString();
                _Beta = valueBeta.Value.ToString();
                _Gamma = valueGamma.Value.ToString();
                _LMin = valueLMin.Value.ToString();
                _LMax = valueLMax.Value.ToString();
                _SMin = valueSMin.Value.ToString();
                _SMax = valueSMax.Value.ToString();
                _LSSum = valueLSSum.Value.ToString();
                _Price = valuePrice.Value.ToString();
                _Epsilon = valueEpsilon.Value.ToString();

                Parameters.H = valueH.Value;
                Parameters.N = valueN.Value;
                Parameters.Alpha = valueAlpha.Value;
                Parameters.Beta = valueBeta.Value;
                Parameters.Gamma = valueGamma.Value;
                Parameters.LMin = valueLMin.Value;
                Parameters.LMax = valueLMax.Value;
                Parameters.SMin = valueSMin.Value;
                Parameters.SMax = valueSMax.Value;
                Parameters.LSSum = valueLSSum.Value;
                Parameters.Price = valuePrice.Value;
                Parameters.Epsilon = valueEpsilon.Value;
            }
            else
            {
                MessageBox.Show("В данном варианте отсутствуют все необходимые параметры!", "Ошибка заполнения параметров");
            }
        }
        #endregion

        public void SetData()
        {
            Variants = context.Assignments.ToList();
            Methods = context.OptimizationMethods.ToList();
            Tasks = context.AssignmentParameters.ToList();
        }



        #region Params

        private string _alpha;
        public string _Alpha
        {
            get { return _alpha; }
            set
            {
                _alpha = value;
                Parameters.Alpha = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private string _beta;
        public string _Beta
        {
            get { return _beta; }
            set
            {
                _beta = value;
                Parameters.Beta = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private string _gamma;
        public string _Gamma
        {
            get { return _gamma; }
            set
            {
                _gamma = value;
                Parameters.Gamma = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private string _h;
        public string _H
        {
            get { return _h; }
            set
            {
                _h = value;
                Parameters.H = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private string _n;
        public string _N
        {
            get { return _n; }
            set
            {
                _n = value;
                Parameters.N = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private string _lMin;
        public string _LMin
        {
            get { return _lMin; }
            set
            {
                _lMin = value;
                Parameters.LMin = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private string _lMax;
        public string _LMax
        {
            get { return _lMax; }
            set
            {
                _lMax = value;
                Parameters.LMax = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private string _sMin;
        public string _SMin
        {
            get { return _sMin; }
            set
            {
                _sMin = value;
                Parameters.SMin = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private string _sMax;
        public string _SMax
        {
            get { return _sMax; }
            set
            {
                _sMax = value;
                Parameters.SMax = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private string _price;
        public string _Price
        {
            get { return _price; }
            set
            {
                _price = value;
                Parameters.Price = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private string _epsilon;
        public string _Epsilon
        {
            get { return _epsilon; }
            set
            {
                _epsilon = value;
                Parameters.Epsilon = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private string _lSSum;
        public string _LSSum
        {
            get { return _lSSum; }
            set
            {
                _lSSum = value;
                Parameters.LSSum = Convert.ToDouble(value);
                OnPropertyChanged();
            }
        }

        private Parameter _paramdb;
        public Parameter Paramdb
        {
            get { return _paramdb; }
            set
            {
                _paramdb = value;
                OnPropertyChanged();
            }
        }

        private InputParameter _parameters = new InputParameter();
        public InputParameter Parameters
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
                            parameters = new OutputParams();

                            if (CurrentMethod.Name == "Метод сканирования")
                            {
                                //добавить проверку на левую и правую границу


                                var calculate = new ScanMethod(Parameters);
                                calculate.Calculation(out var points3D);

                                var temp = new List<double>();

                                foreach (var point in points3D)
                                {
                                    temp.Add(point.Z);
                                }

                                parameters.LengthResult = points3D.Find(x => x.Z == temp.Min()).X;
                                parameters.WidthResult = points3D.Find(x => x.Z == temp.Min()).Y;
                                parameters.CostPriceResult = temp.Min();
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
                            else if (CurrentMethod.Name == "Метод Бокса")
                            {
                                BoxMethod methodBox = new BoxMethod(Parameters);
                                parameters = methodBox.Calc();

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


        private Assignment _currentVariant;
        public Assignment CurrentVariant
        {
            get => _currentVariant;
            set
            {
                _currentVariant = value;
                OnPropertyChanged();

                SetParams(_currentVariant.Id);
            }
        }

        private List<AssignmentParameter> _tasks;
        public List<AssignmentParameter> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
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

        private OptimizationMethod _currentMethod;
        public OptimizationMethod CurrentMethod
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


        private static Microsoft.Office.Interop.Excel.Application? _Excel;
        private static Workbook? _Workbook;
        private static Worksheet? _Worksheet;
        private RelayCommand _save;
        public RelayCommand Save
        {
            get
            {
                return _save ??= new RelayCommand(x =>
                {
                    if (OutputParam != null)
                    {
                        var dialog = new System.Windows.Forms.FolderBrowserDialog();
                        var result = dialog.ShowDialog();
                        if (!string.IsNullOrWhiteSpace(dialog.SelectedPath))
                        {
                            var path = dialog.SelectedPath;

                            ExportExcel.Export(SelectedMethod, SelectedTask, InputParameter, Limitation, OutputParameter);

                            string pathFile = Path.Combine(path, $"Отчет №1. {DateTime.Today.ToShortDateString()}.xlsx");
                            var file = new FileInfo(Path.Combine(path, pathFile));

                            int num = 1;
                            while (file.Exists)
                            {
                                pathFile = Path.Combine(path, $"Отчет №{num}. {DateTime.Today.ToShortDateString()}.xlsx");
                                pathFile.Replace(',', ' ');
                                file = new FileInfo(Path.Combine(pathFile));
                                num++;
                            };

                            var resultShow = System.Windows.MessageBox.Show($"Открыть файл в формате Excel? Он также будет сохранен по пути {pathFile}", "Экспорт в Excel", MessageBoxButton.YesNo);

                            switch (resultShow)
                            {
                                case MessageBoxResult.Yes:
                                    ExportExcel.Save(pathFile);
                                    break;
                                case MessageBoxResult.No:
                                    ExportExcel.SaveAndClose(pathFile);
                                    break;
                            }
                        }
                    }
                    else
                        System.Windows.MessageBox.Show("Выполните расчет");

                });
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

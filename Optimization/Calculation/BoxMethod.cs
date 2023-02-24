using Optimization.Models;
using Optimization.Plots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.Calculation
{
    internal class BoxMethod
    {
        public List<Complex> Complices;
        private static Point[] _StartPoints { get; set; }
        private static Point[] _ComplexPoints { get; set; }
        private Point[] _ErrorPoints { get; set; }
        private double[] _ValuesFunc { get; set; }
        public struct Point
        {
            public double X;
            public double Y;
        }

        public struct Complex
        {
            /// <summary>
            /// номер комплекса
            /// </summary>
            public int NumberComplex;

            /// <summary>
            /// точки в комплекса 
            /// </summary>
            public double PointX;
            public double PointY;

            /// <summary>
            /// Значения точек
            /// </summary>
            public double Func;
        }

        struct ExtrPoint
        {
            /// <summary>
            /// // 0 - если плохая вершина, 1 - если вершина хорошая
            /// </summary>
            public int Flag;

            /// <summary>
            /// индекс в массиве точек комплекса
            /// </summary>
            public int Index;

            /// <summary>
            /// значение функции в этой точке
            /// </summary>
            public double ValueFunc;

            /// <summary>
            /// координаты точки
            /// </summary>
            public Point ValuePoint;
        }
        private InputParameter inputParameters;

        public BoxMethod(InputParameter _inputParameters)
        {
            inputParameters = _inputParameters;
        }

        private void SearchPoints(ref bool flag, ref int countErrorPoints, ref int countComplexPoints, int countPoint)
        {
            countComplexPoints = 0;
            countErrorPoints = 0;
            Random random = new Random();
            Complices = new List<Complex>();

            // находим начальные точки
            for (int i = 0; i < countPoint; i++)
            {
                _StartPoints[i].X = inputParameters.LMin + random.NextDouble() * (inputParameters.LMax - inputParameters.LMin);
                _StartPoints[i].Y = inputParameters.SMin + random.NextDouble() * (inputParameters.SMax - inputParameters.SMin);
            }

            flag = true; // false если хотя бы одна вершина удовлетворяет условиям

            for (int i = 0; i < countPoint; i++)
            {
                // проверяем что найденная вершина удовлетворяет ограничениям второго рода
                if (_StartPoints[i].X + _StartPoints[i].Y >= inputParameters.LSSum)
                {
                    _ComplexPoints[countComplexPoints].X = _StartPoints[i].X;
                    _ComplexPoints[countComplexPoints].Y = _StartPoints[i].Y;

                    flag = false;
                    countComplexPoints++;
                }
                else
                {
                    _ErrorPoints[countErrorPoints].X = _StartPoints[i].X;
                    _ErrorPoints[countErrorPoints].Y = _StartPoints[i].Y;

                    countErrorPoints++;
                }
            }
        }

        private double Expr(Point point)
        {
            return inputParameters.Price * inputParameters.Alpha * (Math.Pow(point.X - point.Y, 2) + inputParameters.Beta * 1 / inputParameters.H * Math.Pow(point.Y + point.X - inputParameters.Gamma * inputParameters.N, 2));
        }

        public OutputParams Calc()
        {
            MathModel model = new MathModel(inputParameters);
            int valueN = (int)Convert.ToDouble(inputParameters.N); 
            // определяем количество вершин комплекса
            int countPoint = 0;
            if (inputParameters.N <= 5)
                countPoint = valueN * 2;
            else
                countPoint = valueN + 1;

            _StartPoints = new Point[countPoint]; // массив исходных точек
            _ComplexPoints = new Point[countPoint];
            _ErrorPoints = new Point[countPoint];
            _ValuesFunc = new double[countPoint];

            var outputParameters = new OutputParams();

            bool flag = true; // false - если хотя бы одна вершина удовлетворяет условиям
            int countErrorPoints = 0;
            int countComplexPoints = 0;

            while (flag)
            {
                SearchPoints(ref flag, ref countErrorPoints, ref countComplexPoints, countPoint);
            }

            double sumComplexPointsX = 0;
            double sumComplexPointsY = 0;

            //считаем сумму значений по каждой координате
            for (int i = 0; i < countComplexPoints; i++)
            {
                sumComplexPointsX += _ComplexPoints[i].X;
                sumComplexPointsY += _ComplexPoints[i].Y;
            }

            // исправление вершин, которые не выполняют ограничения
            for (int i = 0; i < countErrorPoints; i++)
            {
                _ErrorPoints[i].X = 0.5 * (_ErrorPoints[i].X + (1 / (countComplexPoints)) * sumComplexPointsX);
                _ErrorPoints[i].Y = 0.5 * (_ErrorPoints[i].Y + (1 / (countComplexPoints)) * sumComplexPointsY);

                if (_ErrorPoints[i].X + _ErrorPoints[i].Y >= inputParameters.LSSum) // проверяем что в найденной вершине выполняются ограничения второго рода
                {
                    _ComplexPoints[countComplexPoints].X = _ErrorPoints[i].X;
                    _ComplexPoints[countComplexPoints].Y = _ErrorPoints[i].Y;
                    countComplexPoints++;
                }
                else
                {
                    i += 1;
                }
            }

            // вычисление значений функции в вершинах комплекса
            for (int i = 0; i < _ComplexPoints.Length; i++)
            {
                _ValuesFunc[i] = Expr(_ComplexPoints[i]);
            }

            int number = 0;

            while (true)
            {
                for (int i = 0; i < _ComplexPoints.Length; i++)
                {
                    Complices.Add(new Complex { NumberComplex = number, PointX = _ComplexPoints[i].X, PointY = _ComplexPoints[i].Y, Func = _ValuesFunc[i] });
                }

                number++;

                double[] sortValuesFunc = new double[_ValuesFunc.Length];

                for (int i = 0; i < _ValuesFunc.Length; i++)
                {
                    sortValuesFunc[i] = _ValuesFunc[i];
                }
                Array.Sort(sortValuesFunc);

                var extrPoint = new ExtrPoint[2]; // массив для хранения самой "хорошей" и самой "плохой" вершины

                // запоминаем точки самой "хорошей" и самой "плохой" вершины
                for (int i = 0; i < _ValuesFunc.Length; i++)
                {
                    if (_ValuesFunc[i] == sortValuesFunc[0])
                    {
                        extrPoint[0].Flag = 1;
                        extrPoint[0].Index = i;
                        extrPoint[0].ValueFunc = _ValuesFunc[i];
                        extrPoint[0].ValuePoint = _ComplexPoints[i];
                    }

                    else if (_ValuesFunc[i] == sortValuesFunc[sortValuesFunc.Length - 1])
                    {
                        extrPoint[extrPoint.Length - 1].Flag = 0;
                        extrPoint[extrPoint.Length - 1].Index = i;
                        extrPoint[extrPoint.Length - 1].ValueFunc = _ValuesFunc[i];
                        extrPoint[extrPoint.Length - 1].ValuePoint = _ComplexPoints[i];
                    }
                }

                var centerPoint = new Point(); // координата центра комплекса

                double sumMinusExtrValX = 0; // хранение суммы значений вершин по Х минус худшая вершина для промежуточных вычислений
                double sumMinusExtrValY = 0; // хранение суммы значений вершин по Х минус худшая вершина для промежуточных вычислений


                // вычисление промежуточной суммы для координаты центра комплекса
                for (int i = 0; i < _ComplexPoints.Length; i++)
                {
                    sumMinusExtrValX += _ComplexPoints[i].X;
                    sumMinusExtrValY += _ComplexPoints[i].Y;
                }

                // координаты центра комплекса
                centerPoint.X = 1.0 / (countPoint - 1) * (sumMinusExtrValX - extrPoint.Last(x => x.Flag == 0).ValuePoint.X);
                centerPoint.Y = 1.0 / (countPoint - 1) * (sumMinusExtrValY - extrPoint.Last(x => x.Flag == 0).ValuePoint.Y);

                double sumB = 0; // хранение суммы для проверки окончания поиска

                sumB += Math.Abs((centerPoint.X - extrPoint.Last(x => x.Flag == 0).ValuePoint.X)) + Math.Abs((centerPoint.X - extrPoint.Last(x => x.Flag == 1).ValuePoint.X));
                sumB += Math.Abs((centerPoint.Y - extrPoint.Last(x => x.Flag == 0).ValuePoint.Y)) + Math.Abs((centerPoint.Y - extrPoint.Last(x => x.Flag == 1).ValuePoint.Y));

                double B = 1.0 / (2 * inputParameters.N) * sumB;

                if (B < inputParameters.Epsilon)
                {
                    outputParameters.LengthResult = Math.Round(centerPoint.X, 2);
                    outputParameters.WidthResult = Math.Round(centerPoint.Y, 2);
                    outputParameters.CostPriceResult = Math.Round(Expr(centerPoint), 2);

                    return outputParameters;
                }
                else
                {
                    var newPoint = new Point(); // новая координата взамен наихудшей

                    newPoint.X = 2.3 * centerPoint.X - 1.3 * extrPoint.Last(x => x.Flag == 0).ValuePoint.X;
                    newPoint.Y = 2.3 * centerPoint.Y - 1.3 * extrPoint.Last(x => x.Flag == 0).ValuePoint.Y;

                    // проверям ограничений первого рода                
                    if (inputParameters.LMin > newPoint.X)
                    {
                        newPoint.X = inputParameters.LMin + inputParameters.Epsilon;
                    }
                    else if (newPoint.X > inputParameters.LMax)
                    {
                        newPoint.X = inputParameters.LMax - inputParameters.Epsilon;
                    }
                    if (inputParameters.SMin > newPoint.Y)
                    {
                        newPoint.Y = inputParameters.SMin + inputParameters.Epsilon;
                    }
                    else if (newPoint.Y > inputParameters.SMax)
                    {
                        newPoint.Y = inputParameters.LMax - inputParameters.Epsilon;
                    }

                    // проверка ограничений второго рода
                    // пока ограничение не выполняется смещаем координату к центру
                    while ((newPoint.X + newPoint.Y) > inputParameters.LSSum)
                    {
                        newPoint.X = 0.5 * (newPoint.X + centerPoint.X);
                        newPoint.Y = 0.5 * (newPoint.Y + centerPoint.Y);
                    }

                    // вычисляем значение функции в новой точке
                    double newPointF = Expr(newPoint);

                    while (newPointF > extrPoint.Last(x => x.Flag == 0).ValueFunc)
                    {
                        newPoint.X = 0.5 * (newPoint.X + extrPoint.Last(x => x.Flag == 1).ValuePoint.X);
                        newPoint.Y = 0.5 * (newPoint.Y + extrPoint.Last(x => x.Flag == 1).ValuePoint.Y);
                        newPointF = Expr(newPoint);
                    }

                    // записываем значения новой точке в массив вершин Комплекса
                    _ComplexPoints[extrPoint.Last(x => x.Flag == 0).Index] = newPoint;
                    _ValuesFunc[extrPoint.Last(x => x.Flag == 0).Index] = newPointF;
                }
            }
        }

    }
}

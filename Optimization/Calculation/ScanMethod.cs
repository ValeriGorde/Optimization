using Optimization.Models;
using Optimization.Plots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Point = Optimization.Plots.Point;

namespace Optimization.Calculation
{
    internal class ScanMethod
    {
        InputParameter inputParameters;
        
        public int CalculationCount { get; private set; }
        private double step;
        public double k = 10;
        public double r = 2;
        public double n = 2;

        public ScanMethod(InputParameter _inputParameters) 
        {
            inputParameters = _inputParameters;
        }

        public void Calculation(out List<Point3D> points3D) 
        {

            List<double> values;
            Point newMax;

            var funcMax = double.MinValue;
            step = Math.Pow(k, r) * inputParameters.Epsilon;
            points3D = new List<Point3D>();
            var p3D = new List<Point3D>();

            newMax = SearchMaxOnGrid(out p3D, out values);

            step /= k;

            points3D.AddRange(p3D);

            while (funcMax > values.Max())
            {
                newMax = SearchMaxOnGrid(out p3D, out values);

                inputParameters.LMin = newMax.X - step;
                inputParameters.LMax = newMax.Y - step;

                inputParameters.SMin = newMax.X + step;
                inputParameters.SMax = newMax.Y + step;

                step /= k;

                funcMax = values.Max();
                points3D.AddRange(p3D);
            }
        }

        private Point SearchMaxOnGrid(out List<Point3D> points3D, out List<double> values) 
        {
            points3D = new List<Point3D>();
            MathModel model = new MathModel(inputParameters);

            for (var l = inputParameters.LMin; l <= inputParameters.LMax; l += step) 
            {
                for (var w = inputParameters.SMin; w <= inputParameters.SMax; w += step) 
                {
                    if (!model.Conditions(l, w)) 
                    {
                        continue;
                    }

                    CalculationCount++;
                    var value = model.MainModel(l, w);

                    if (value < 0) 
                    {
                        MessageBox.Show($"Длина теплоообменника, м {l} Ширина теплообменника, м {w} Себестоимость изделия, у.е. {value}");
                    }
                    points3D.Add(new Point3D(Math.Round(l, 2), Math.Round(w, 2), Math.Round(value, 2)));
                }
            }

            var valuesList = points3D.Select(p => p.Z).ToList();
            values = valuesList;

            return new Point(points3D.Find(p => p.Z == valuesList.Max()).X, points3D.Find(p => p.Z == valuesList.Max()).Y);
        }
    }
}

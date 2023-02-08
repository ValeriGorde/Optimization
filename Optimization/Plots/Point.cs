using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.Plots
{
    internal class Point
    {
        public Point(double _X, double _Y)
        {
            X = _X;
            Y = _Y;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }
}

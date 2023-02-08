using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.Plots
{
    internal class Point3D
    {
        public Point3D(double _X, double _Y, double _Z)
        {
            X = _X;
            Y = _Y;
            Z = _Z;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }
    }
}

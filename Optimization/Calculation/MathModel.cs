using Optimization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.Calculation
{
    internal class MathModel
    {
        InputParameter inputParameters;
        public MathModel(InputParameter _inputParameters) 
        {
            inputParameters = _inputParameters;
        }

        public double MainModel(double length, double width)
        {
            return inputParameters.Price*inputParameters.Alpha * (Math.Pow(length - width, 2) + inputParameters.Beta * 1 / inputParameters.H * Math.Pow(width + length - inputParameters.Gamma * inputParameters.N, 2));
        }

        public bool Conditions(double length, double width)
        {
            return length >= inputParameters.LMin && length <= inputParameters.LMax && width >= inputParameters.SMin && width <= inputParameters.SMax && length + width >= inputParameters.LSSum;
        }
    }
}

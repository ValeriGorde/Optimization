using Optimization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.Filter
{
    internal class FiltrationOutput
    {
        public static OutputParamsArr[] CalcEqvation(InputParameters inputParameters)
        {

            int sizeArray = (int)((inputParameters.LMax - inputParameters.LMin) * (inputParameters.SMax - inputParameters.SMin) / (0.1 * 0.1) + 1);

            OutputParams outputParams = new OutputParams();
            outputParams.OutputParamsArr = new OutputParamsArr[sizeArray];

            int s = 0;

            for (double i = inputParameters.LMin; i < inputParameters.LMax; i = Math.Round(i + 0.1, 2))
            {
                for (double j = inputParameters.SMin; j < inputParameters.SMax; j = Math.Round(j + 0.1, 2))
                {
                    outputParams.OutputParamsArr[s] = new OutputParamsArr();
                    outputParams.OutputParamsArr[s].Length = Math.Round(i, 2);
                    outputParams.OutputParamsArr[s].Width = Math.Round(j, 2);
                    outputParams.OutputParamsArr[s].CostPrice = Math.Round(inputParameters.Alpha * (Math.Pow(i - j, 2) + (inputParameters.Beta * 1 / inputParameters.H) * Math.Pow(j + i - inputParameters.Gamma * inputParameters.N, 2)), 2);

                    s++;
                }
            }

            return outputParams.OutputParamsArr;
        }
    }
}

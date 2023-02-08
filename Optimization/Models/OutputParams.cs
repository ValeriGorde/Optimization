using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.Models
{
    internal class OutputParams
    {
        /// <summary>
        /// Длина теплообменника
        /// </summary>
        public double LengthResult { get; set; }

        /// <summary>
        /// Ширина теплообменника
        /// </summary>
        public double WidthResult { get; set; }

        /// <summary>
        /// Себестоимость изделия
        /// </summary>
        public double CostPriceResult { get; set; }
        public OutputParamsArr[]? OutputParamsArr { get; set; }
    }
}

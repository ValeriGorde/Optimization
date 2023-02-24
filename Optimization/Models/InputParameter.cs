using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.Models
{
    internal class InputParameter
    {
        [Key]
        public int Id { get; set; }
        public double Alpha { get; set; }
        public double Beta { get; set; }
        public double Gamma { get; set; }
        public double H { get; set; }
        public double N { get; set; }
        public double LMin { get; set; }
        public double LMax { get; set; }
        public double SMin { get; set; }
        public double SMax { get; set; }
        public double Price { get; set; }
        public double Epsilon { get; set; }
        public double LSSum { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();

    }
}

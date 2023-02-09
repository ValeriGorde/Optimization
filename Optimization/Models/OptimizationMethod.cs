using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.Models
{
    internal class OptimizationMethod
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Realization { get; set; }
    }
}

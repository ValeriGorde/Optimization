using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.Models
{
    internal class AssignmentParameter
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
        public int ParameterId { get; set; }
        public Parameter Parameter { get; set; }
    }
}

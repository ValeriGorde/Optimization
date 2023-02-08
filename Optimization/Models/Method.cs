using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.Models
{
    internal class Method
    {
        [Key]
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Activated { get; set; }
    }
}

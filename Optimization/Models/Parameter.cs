﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.Models
{
    internal class Parameter
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        //public int InputParamId { get; set; }
        //public InputParameter InputParameter { get; set; }
        public List<AssignmentParameter> Parameters { get; set; } = new List<AssignmentParameter>();
    }
}

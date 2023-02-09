using Optimization.DB_EF;
using Optimization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.CRUD
{
    internal class MethodCRUD: ICRUD<OptimizationMethod>
    {
        public List<OptimizationMethod> _Method { get; set; }
        ApplicationContext context;

        public MethodCRUD() 
        {
            context = new ApplicationContext();
            _Method = context.OptimizationMethods.ToList();
        }

        public void Create(OptimizationMethod item)
        {
            context.OptimizationMethods.Add(item);
            context.SaveChanges();
        }

        public void Read(int id)
        {
            var method = _Method.Find(m => m.Id == id);
        }

        public void Update(OptimizationMethod item)
        {
            var method = _Method.FirstOrDefault(m => m.Id == item.Id);
            method.Id = item.Id;
            method.Name = item.Name;
            method.Realization = item.Realization;

            context.Update(method);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var method = _Method.FirstOrDefault(m => m.Id == id);

            context.Remove(method);
            context.SaveChanges();
        }
    }
}

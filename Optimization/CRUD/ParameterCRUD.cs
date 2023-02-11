using Optimization.DB_EF;
using Optimization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.CRUD
{
    internal class ParameterCRUD : ICRUD<Parameter>
    {
        public List<Parameter> _Parameter { get; set; }
        ApplicationContext context;

        public ParameterCRUD() 
        {
            context = new ApplicationContext();
            _Parameter = context.Parameters.ToList();
        }

        public void Create(Parameter item)
        {
            context.Parameters.Add(item);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var param = _Parameter.Find(p => p.Id == id);

            context.Parameters.Remove(param);
            context.SaveChanges();
        }

        public bool Read(int id)
        {
            var param = _Parameter.Find(p => p.Id == id);

            if (param == null) return true;
            else return false;
        }

        public void Update(Parameter item)
        {
            var param = context.Parameters.FirstOrDefault(p => p.Id == item.Id);
            param.Id = item.Id;
            param.Name = item.Name;
            param.Symbol = item.Symbol;

            context.Update(param);
            context.SaveChanges();
        }

        public void Delete(Parameter item)
        {
            throw new NotImplementedException();
        }
    }
}
